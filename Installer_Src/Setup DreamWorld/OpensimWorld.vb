Imports System.IO
Imports System.Net

Module OpensimWorld

    Public Sub ScanOpenSimWorld(Force As Boolean)

        For Each RegionUUID In RegionUuids()

            If OpensimWorldAPIKey(RegionUUID).Length = 0 Then Continue For
            If Not RegionEnabled(RegionUUID) Then Continue For

            If RegionStatus(RegionUUID) = SIMSTATUSENUM.Booted Or
                (Smart_Start(RegionUUID) And Settings.Smart_Start) Then

                Dim Avatars = GetAgentsInRegion(RegionUUID)
                If Avatars <> InRegion(RegionUUID) Then
                    InRegion(RegionUUID) = Avatars
                    SendToOpensimWorld(RegionUUID, Avatars)
                    InRegion(RegionUUID) = Avatars
                ElseIf Force Then
                    InRegion(RegionUUID) = Avatars
                    SendToOpensimWorld(RegionUUID, Avatars)
                End If
            End If

        Next

    End Sub

    Public Sub SendToOpensimWorld(RegionUUID As String, Avatars As Integer)

        Dim k = OpensimWorldAPIKey(RegionUUID)
        If k.Length = 0 Then Return

        Dim Regionname = Region_Name(RegionUUID)
        Dim pos = Uri.EscapeDataString("<128,128,23>")
        Logger("OpensimWorld", $"Update {Regionname}", "Outworldz")
        Dim URL = $"http://beacon.opensimworld.com/index.php/osgate/beacon/?wk={k}&na={Avatars}&rat=0&r={Regionname}&pos={pos}"
        ' If he says to delete it, we do so.
        If Poke(URL) = -1 Then OpensimWorldAPIKey(RegionUUID) = ""
        Application.DoEvents()

    End Sub

    Private Function Poke(URL As String) As Integer

        ' Create a New 'HttpWebRequest' Object to the mentioned URL.
        Dim myHttpWebRequest = CType(WebRequest.Create(URL), HttpWebRequest)

        myHttpWebRequest.Headers("X-SecondLife-Object-Name") = "DreamGrid"
        Dim outputData As String = ""
        ' Assign the response object of 'HttpWebRequest' to a 'HttpWebResponse' variable.
        Try
            Using myHttpWebResponse As HttpWebResponse = CType(myHttpWebRequest.GetResponse(), HttpWebResponse)

                'Debug.Print($"{vbCrLf}The HttpHeaders are {vbCrLf}Name {0}", myHttpWebRequest.Headers)
                ' Print the HTML contents of the page to the console.
                Using streamResponse As Stream = myHttpWebResponse.GetResponseStream()
                    Using streamRead As New StreamReader(streamResponse)
                        Dim readBuff As Char() = New Char(255) {}
                        Dim count As Integer = streamRead.Read(readBuff, 0, 256)
                        outputData = New String(readBuff, 0, count)
                        While count > 0
                            count = streamRead.Read(readBuff, 0, 256)
                        End While
                    End Using
                End Using
            End Using
        Catch
        End Try

        If outputData = "OK" Then Return 1
        If outputData = "DISABLE" Then Return -1
        Return 0

    End Function

End Module
