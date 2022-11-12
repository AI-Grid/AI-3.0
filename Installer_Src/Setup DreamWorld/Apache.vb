#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.IO

Module Apache

    Private WithEvents ApacheProcess As New Process()
    Private Const DreamGrid As String = "DreamGrid"
    Private Const JOpensim As String = "JOpensim"
    Private Const WordPress As String = "WordPress"
    Private _ApacheCrashCounter As Integer
    Private _ApacheExited As Boolean
    Private _ApacheUninstalling As Boolean

#Region "Properties"

    Public Property ApacheCrashCounter As Integer
        Get
            Return _ApacheCrashCounter
        End Get
        Set(value As Integer)
            _ApacheCrashCounter = value
        End Set
    End Property

    Public Property PropApacheExited() As Boolean
        Get
            Return _ApacheExited
        End Get
        Set(ByVal Value As Boolean)
            _ApacheExited = Value
        End Set
    End Property

    Public Property PropApacheUninstalling() As Boolean
        Get
            Return _ApacheUninstalling
        End Get
        Set(ByVal Value As Boolean)
            _ApacheUninstalling = Value
        End Set
    End Property

    Public Sub ApacheIcon(Running As Boolean)

        If Not Running Then
            FormSetup.RestartApacheIcon.Image = Global.Outworldz.My.Resources.nav_plain_red
        Else
            FormSetup.RestartApacheIcon.Image = Global.Outworldz.My.Resources.check2
        End If
        Application.DoEvents()

    End Sub

#End Region

    ''' <summary>Check is Apache port 80 or 8000 is up</summary>
    ''' <returns>boolean</returns>
    Public Function IsApacheRunning() As Boolean

        Dim Up As String
        Using TimedClient As New TimedWebClient With {
              .Timeout = 1000
            }
            Try
                Up = TimedClient.DownloadString($"http://{Settings.LANIP}:{CStr(Settings.ApachePort)}/?_Opensim={RandomNumber.Random}")
            Catch ex As Exception
                If ex.Message.Contains("200 OK") Then
                    ApacheIcon(True)
                    Return True
                End If
                ApacheIcon(False)
                Return False
            End Try
            If Up.Length = 0 Then
                ApacheIcon(False)
                Return False
            End If

        End Using
        ApacheIcon(True)
        Return True

    End Function

    ''' <summary>
    ''' Installs and sets up Apache Web Server
    ''' </summary>
    Public Sub StartApache()

        ' Depends upon PHP for home page
        DoPHPDBSetup()

        SetPath()

        If Settings.SiteMap Then
            Dim SiteMapContents = "<?xml version=""1.0"" encoding=""UTF-8""?>" & vbCrLf
            SiteMapContents += "<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.0909"">" & vbCrLf
            SiteMapContents += "<url>" & vbCrLf
            SiteMapContents += "<loc>http://" & Settings.PublicIP & ":" & Convert.ToString(Settings.ApachePort, Globalization.CultureInfo.InvariantCulture) & "/" & "</loc>" & vbCrLf

            If Settings.CMS = DreamGrid Then
                SiteMapContents += "<loc>http://" & Settings.PublicIP & ":" & Convert.ToString(Settings.ApachePort, Globalization.CultureInfo.InvariantCulture) & "/DreamGrid" & "</loc>" & vbCrLf
            ElseIf Settings.CMS = JOpensim Then
                SiteMapContents += "<loc>http://" & Settings.PublicIP & ":" & Convert.ToString(Settings.ApachePort, Globalization.CultureInfo.InvariantCulture) & "/jOpensim" & "</loc>" & vbCrLf
            ElseIf Settings.CMS = WordPress Then
                SiteMapContents += "<loc>http://" & Settings.PublicIP & ":" & Convert.ToString(Settings.ApachePort, Globalization.CultureInfo.InvariantCulture) & "/WordPress" & "</loc>" & vbCrLf
            Else
                SiteMapContents += "<loc>http://" & Settings.PublicIP & ":" & Convert.ToString(Settings.ApachePort, Globalization.CultureInfo.InvariantCulture) & "/" & Settings.CMS & "</loc>" & vbCrLf
            End If

            SiteMapContents += "<changefreq>daily</changefreq>" & vbCrLf
            SiteMapContents += "<priority>1.0</priority>" & vbCrLf
            SiteMapContents += "</url>" & vbCrLf
            SiteMapContents += "</urlset>" & vbCrLf

            Using outputFile As New StreamWriter(IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Apache\htdocs\Sitemap.xml"), False)
                outputFile.WriteLine(SiteMapContents)
            End Using

        End If

        If Not Settings.ApacheEnable Then
            ApacheIcon(False)
            TextPrint(My.Resources.Apache_Disabled)
            Return
        End If

        If Settings.ApachePort = 80 Then
            ApacheProcess.StartInfo.UseShellExecute = True ' so we can redirect streams
            ApacheProcess.StartInfo.FileName = "net"
            ApacheProcess.StartInfo.CreateNoWindow = True
            ApacheProcess.StartInfo.Arguments = "stop W3SVC"
            ApacheProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            ApacheProcess.StartInfo.CreateNoWindow = True
            Try
                ApacheProcess.Start()
                ApacheProcess.WaitForExit()
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try

        End If

        If Settings.CurrentDirectory <> Settings.LastDirectory Or Not ServiceExists("ApacheHTTPServer") Or ApacheRevision <> Settings.ApacheRev Then

            ' Stop MSFT server if we are on port 80 and enabled
            PropApacheUninstalling = True

            ' old stuff we had named this way
            ApacheProcess.StartInfo.Arguments = "stop " & """" & "Apache HTTP Server" & """"
            Try
                ApacheProcess.Start()
                ApacheProcess.WaitForExit()
            Catch ex As Exception
            End Try
            Application.DoEvents()

            'delete really old service
            ApacheProcess.StartInfo.FileName = "sc"
            ApacheProcess.StartInfo.Arguments = " delete  " & """" & "Apache HTTP Server" & """"
            ApacheProcess.StartInfo.CreateNoWindow = True
            ApacheProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden

            Try
                ApacheProcess.Start()
                ApacheProcess.WaitForExit()
            Catch ex As Exception
            End Try
            Application.DoEvents()

            Application.DoEvents()
            Using ApacheProcess As New Process With {
                    .EnableRaisingEvents = False
                }
                ApacheProcess.StartInfo.UseShellExecute = True ' so we can redirect streams
                ApacheProcess.StartInfo.FileName = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Apache\bin\httpd.exe")
                ApacheProcess.StartInfo.Arguments = "-k install -n " & """" & "ApacheHTTPServer" & """"
                ApacheProcess.StartInfo.CreateNoWindow = True
                ApacheProcess.StartInfo.WorkingDirectory = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Apache\bin\")
                ApacheProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden

                DoApache()

                Try
                    ApacheProcess.Start()
                    ApacheProcess.WaitForExit()
                Catch ex As Exception
                    BreakPoint.Dump(ex)
                    ApacheIcon(False)
                End Try
                Application.DoEvents()

                If ApacheProcess.ExitCode <> 0 Then
                    TextPrint(My.Resources.ApacheFailed)
                    ApacheIcon(False)
                Else
                    PropApacheUninstalling = False ' installed now, trap errors
                End If

                Application.DoEvents()
            End Using

        End If

        TextPrint(My.Resources.Apache_starting)
        DoApache()

        Settings.LastDirectory = Settings.CurrentDirectory
        Settings.SaveSettings()

        Using ApacheProcess As New Process With {
                    .EnableRaisingEvents = False
                }
            ApacheProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            ApacheProcess.StartInfo.FileName = "net"
            ApacheProcess.StartInfo.Arguments = "start ApacheHTTPServer"
            ApacheProcess.StartInfo.UseShellExecute = False
            ApacheProcess.StartInfo.CreateNoWindow = True
            ApacheProcess.StartInfo.RedirectStandardError = True
            ApacheProcess.StartInfo.RedirectStandardOutput = True
            Dim response As String = ""

            Try
                ApacheProcess.Start()
                response = ApacheProcess.StandardOutput.ReadToEnd() & ApacheProcess.StandardError.ReadToEnd()
                ApacheProcess.WaitForExit()
            Catch ex As Exception
                BreakPoint.Dump(ex)
                TextPrint(My.Resources.Apache_Failed & ":" & ex.Message)
            End Try
            Application.DoEvents()

            If ApacheProcess.ExitCode <> 0 Then
                If response.Contains("has already been started") Then
                    ApacheIcon(True)

                    Settings.ApacheRev = ApacheRevision
                    Settings.SaveSettings()

                    Return
                End If
                TextPrint(My.Resources.Apache_Failed & ":" & CStr(ApacheProcess.ExitCode))
                ApacheIcon(False)
            Else
                TextPrint(My.Resources.Apache_running & ":" & Settings.ApachePort)
                ApacheIcon(True)
            End If

        End Using

    End Sub

    ''' <summary>
    ''' Stop Web Service
    ''' </summary>
    Public Sub StopApache()

        If Not Settings.ApacheEnable Then Return

        Using ApacheProcess As New Process()
            TextPrint(My.Resources.Stopping_Apache)

            ApacheProcess.StartInfo.FileName = "net.exe"
            ApacheProcess.StartInfo.Arguments = "stop ApacheHTTPServer"
            ApacheProcess.StartInfo.CreateNoWindow = True
            ApacheProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            Try
                ApacheProcess.Start()
                ApacheProcess.WaitForExit()
            Catch ex As Exception
                BreakPoint.Dump(ex)
                TextPrint(My.Resources.ApacheNot_Stopping & ":" & ex.Message)
            End Try

        End Using

        If IsApacheRunning() Then
            ApacheIcon(True)
            TextPrint(My.Resources.ApacheNot_Stopping)
        Else
            ApacheIcon(False)
            TextPrint(My.Resources.Apache_Stopped)
        End If

    End Sub

    ''' <summary>
    ''' Handle Exited Event And display process information.
    ''' </summary>
    Private Sub ApacheProcess_Exited(ByVal sender As Object, ByVal e As EventArgs) Handles ApacheProcess.Exited

        FormSetup.RestartApacheIcon.Image = Global.Outworldz.My.Resources.nav_plain_red

        If PropAborting Then Return
        If PropApacheUninstalling Then Return

        If Settings.RestartOnCrash And ApacheCrashCounter < 10 Then
            ApacheCrashCounter += 1
            PropApacheExited = True
            ApacheIcon(False)
            Return
        End If
        ApacheCrashCounter = 0

        Dim yesno = MsgBox(My.Resources.Apache_Exited, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Error_word)
        If (yesno = vbYes) Then
            Baretail("""" & IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Logs\Apache\error*.log") & """")
        End If

    End Sub

    ''' <summary>
    ''' Copies curl PHP DLLs to Windows\system32
    ''' </summary>
    Private Sub SetPath()

        Dim DLLList As New List(Of String) From {"libeay32.dll", "libssh2.dll", "ssleay32.dll"}

        For Each item In DLLList
            Dim dest As String = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), item)
            If Not IO.File.Exists(dest) Then
                CopyFileFast(IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\PHP7\curl\" & item), dest)
            End If
        Next

    End Sub

End Module
