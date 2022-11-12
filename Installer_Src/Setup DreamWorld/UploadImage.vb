#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.IO
Imports System.Net

Module UploadImage

    Private _busy As Boolean

    Public Property Busy As Boolean
        Get
            Return _busy
        End Get
        Set(value As Boolean)
            _busy = value
        End Set
    End Property

#Region "Upload File"

    Public Sub PostContentUploadFile(File As String, CGI As Uri)

        Try
            Dim params As New Specialized.NameValueCollection From {
                {"MachineID", Settings.MachineId()},
                {"DnsName", Settings.PublicIP}
            }

            Dim req As Net.HttpWebRequest = CType(HttpWebRequest.Create(CGI), HttpWebRequest)
            req.Method = "POST"
            req.KeepAlive = True
            req.ReadWriteTimeout = System.Threading.Timeout.Infinite
            req.Credentials = System.Net.CredentialCache.DefaultCredentials

            Dim ar As IAsyncResult = req.BeginGetRequestStream(AddressOf RequestStreamAvailable, New HttpRequestState(req, params, File))
            Busy = True
        Catch ex As Exception
            BreakPoint.Dump(ex)

            Log(My.Resources.Error_word, ex.Message)
            Busy = False
        End Try

        Dim ctr = 600
        While Busy And ctr > 0
            Threading.Thread.Sleep(100)
            Application.DoEvents()
            ctr -= 1
        End While

        Busy = False

    End Sub

    Public Sub UploadCategory()

        If Settings.DnsName.Length = 0 Then Return

        Dim result As String = Nothing
        If Settings.Categories.Length = 0 Then Return

        Using client As New WebClient ' download client for web pages
            Try
                Dim str = PropHttpsDomain & "/cgi/UpdateCategory.plx" & GetPostData()
                result = client.DownloadString(str)
            Catch ex As Exception
                BreakPoint.Dump(ex)
                ErrorLog(My.Resources.Wrong & " " & ex.Message)
            End Try
        End Using

        If result <> "OK" Then
            ErrorLog(My.Resources.Wrong & " " & result)
        End If

    End Sub

    Sub UploadComplete(ByVal data As String)

        If data <> "1" Then
            Log(My.Resources.Error_word, "Upload Failed. " & data)
        End If
        Busy = False

    End Sub

    Sub UploadError(ByVal data As String)

        ErrorLog("Upload Error:" + data)
        Busy = False

    End Sub

    Public Sub UploadPhoto()

        ''' <summary>Upload in a separate thread the photo, if any. Cannot be called unless main web server is known to be on line.</summary>
        If Settings.Gdpr() Then
            UploadCategory()
            If System.IO.File.Exists(IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Photo.png")) Then
                Dim CGI As Uri = New Uri("https://outworldz.com/cgi/uploadphoto.plx")
                Dim Photo As String = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Photo.png")
                PostContentUploadFile(Photo, CGI)
            End If
        End If

    End Sub

    Private Sub RequestStreamAvailable(ByVal ar As IAsyncResult)
        Dim r_State As HttpRequestState = TryCast(ar.AsyncState, HttpRequestState)
        Dim boundary As String = StrDup(20, "-"c) & Date.Now.ToString("yyyyMMdd-hhmm", Globalization.CultureInfo.InvariantCulture)
        r_State.Request.ContentType = "multipart/form-data; boundary=" & boundary
        Debug.Print("multipart/form-data; boundary=" & boundary)

        Dim reqStream As Stream
        Try
            reqStream = r_State.Request.EndGetRequestStream(ar)
        Catch ex As Exception
            BreakPoint.Dump(ex)
            UploadError(ex.Message)
            Return
        End Try

        Try
            Using sw As StreamWriter = New StreamWriter(reqStream)

                ' blank line
                sw.WriteLine()
                Debug.Print("")

                For Each key As String In r_State.Params.Keys
                    sw.WriteLine("--" & boundary)
                    Debug.Print("--" & boundary)

                    sw.WriteLine(String.Format(Globalization.CultureInfo.InvariantCulture, "Content-Disposition: form-data; name=""{0}""", key))
                    Debug.Print(String.Format(Globalization.CultureInfo.InvariantCulture, "Content-Disposition: form-data; name=""{0}""", key))
                    sw.WriteLine()
                    Debug.Print("")
                    sw.WriteLine(WebUtility.UrlEncode(r_State.Params(key)))
                    Debug.Print(WebUtility.UrlEncode(r_State.Params(key)))
                Next

                sw.WriteLine("--" & boundary)
                Debug.Print("--" & boundary)

                sw.WriteLine(String.Format(Globalization.CultureInfo.InvariantCulture, "Content-Disposition: form-data; name=""{0}""", "FILE1"))
                Debug.Print(String.Format(Globalization.CultureInfo.InvariantCulture, "Content-Disposition: form-data; name=""{0}""", "FILE1"))

                sw.Write(String.Format(Globalization.CultureInfo.InvariantCulture, "filename=""{0}""", WebUtility.UrlEncode(IO.Path.GetFileName(r_State.FileName))))
                Debug.Print(String.Format(Globalization.CultureInfo.InvariantCulture, "filename=""{0}""", WebUtility.UrlEncode(IO.Path.GetFileName(r_State.FileName))))

                sw.WriteLine()
                Debug.Print("")
                sw.WriteLine("Content-Type: application/octet-stream")
                Debug.Print("Content-Type: application/octet-stream")
                sw.WriteLine()
                Debug.Print("")
                Dim buffer(1024) As Byte, bytesRead As Integer
                sw.Flush()

                Using fileStream As FileStream = New FileStream(r_State.FileName, FileMode.Open, FileAccess.Read)
                    Do
                        bytesRead = fileStream.Read(buffer, 0, buffer.Length)
                        If bytesRead > 0 Then
                            sw.BaseStream.Write(buffer, 0, bytesRead)
                        End If
                    Loop While (bytesRead > 0)
                End Using

                sw.BaseStream.Flush()
                sw.Write(vbNewLine & "--" & boundary & "--" & vbNewLine)
                Debug.Print(vbNewLine & "--" & boundary & "--" & vbNewLine)

                sw.Flush()
            End Using
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

        r_State.Request.BeginGetResponse(AddressOf ResponseAvailable, r_State)

    End Sub

    Private Sub ResponseAvailable(ByVal ar As IAsyncResult)
        Dim r_State As HttpRequestState = TryCast(ar.AsyncState, HttpRequestState)
        Dim webResp As HttpWebResponse
        Dim sData As String = String.Empty
        Try
            webResp = CType(r_State.Request.EndGetResponse(ar), HttpWebResponse)
        Catch ex As Exception
            webResp = Nothing
            BreakPoint.Dump(ex)
        End Try

        If webResp IsNot Nothing Then
            Using respReadr As StreamReader = New StreamReader(webResp.GetResponseStream())
                sData = respReadr.ReadToEnd()
            End Using
            If webResp.StatusCode = HttpStatusCode.OK Then
                Call UploadComplete(sData)
            Else
                Call UploadError(sData)
            End If
            webResp.Close()
        Else
            Call UploadError(sData)
        End If

        _busy = False
    End Sub

#End Region

#Region "HttpRequestState  Class"

    Private Class HttpRequestState

        Public FileName As String
        Public Params As Specialized.NameValueCollection
        Public Request As HttpWebRequest

        Public Sub New(ByRef _req As HttpWebRequest, ByVal _param As Specialized.NameValueCollection, ByVal _file As String)
            Me.Request = _req
            Me.Params = _param
            Me.FileName = _file
        End Sub

#End Region

    End Class

End Module

'multipart/form-data; boundary=--------------------20190207-0157
'----------------------20190207-0157
'Content-Disposition: form-data; name="MachineID";

'548435328
'----------------------20190207-0157
'Content-Disposition: form-data; name="DnsName";
'
'10.6.1.103
'----------------------20190207-0157
'Content-Disposition: form-data; name="FILE1";
'filename = "Photo.png"
'
'Content-Type: application/ octet - stream
'
'
'----------------------20190207-0157--
