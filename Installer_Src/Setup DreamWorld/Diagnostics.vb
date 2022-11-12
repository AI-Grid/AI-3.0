Imports System.Net

Module Diags

    Private _myUPnpMap As UPnp

    Public Property PropMyUPnpMap As UPnp
        Get
            Return _myUPnpMap
        End Get
        Set(value As UPnp)
            _myUPnpMap = value
        End Set
    End Property

    Public Sub DoDiag()

        If IPCheck.IsPrivateIP(Settings.DnsName) Then
            Logger("INFO", Global.Outworldz.My.Resources.LAN_IP, "Diagnostics")
            TextPrint(My.Resources.LAN_IP)
            Return
        End If

        TextPrint("___DIAG_START_______")
        TextPrint(My.Resources.Running_Network)
        Logger("INFO", Global.Outworldz.My.Resources.Running_Network, "Diagnostics")
        Settings.DiagFailed = "False"

        OpenPorts() ' Open router ports with UPnp
        ProbePublicPort() ' Probe using Outworldz like Canyouseeme.org does on HTTP port
        TestPrivateLoopback(True)   ' Diagnostics
        TestPublicLoopback()    ' Http port
        TestAllRegionPorts()    ' All Dos boxes, actually

        If Settings.DiagFailed = "True" Then
            Logger("Error", Global.Outworldz.My.Resources.Diags_Failed, "Diagnostics")
            Dim answer = MsgBox(My.Resources.Diags_Failed, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground)
            If answer = vbYes Then
                ShowLog()
            End If
        Else
            NewDNSName()
        End If
        TextPrint("___DIAG_END_______")

    End Sub

    Public Function GetPostData(Optional Name As String = "") As String

        If Name.Length = 0 Then Name = Settings.PublicIP   ' optional Alt DNS name can come in
        Dim TotalSize As Double
        Dim RegionCount As Integer
        For Each RegionUUID In RegionUuids()
            TotalSize += SizeX(RegionUUID) / 256 * SizeY(RegionUUID) / 256
            RegionCount += 1
        Next

        Dim fs = CreateObject("Scripting.FileSystemObject")
        Dim d As Object = fs.GetDrive(fs.GetDriveName(fs.GetAbsolutePathName("C:")))

        Dim data As String = "?MachineID=" & Settings.MachineId() _
        & "&FriendlyName=" & WebUtility.UrlEncode(Settings.SimName) _
        & "&V=" & WebUtility.UrlEncode(PropMyVersion) _
        & "&OV=" & WebUtility.UrlEncode(PropSimVersion) _
        & "&isPublic=" & CStr(Settings.Gdpr()) _
        & "&GridName=" & Name _
        & "&Port=" & CStr(Settings.HttpPort()) _
        & "&Category=" & Settings.Categories _
        & "&Description=" & Settings.Description _
        & "&IP=" & Name _
        & "&ServerType=" & Settings.ServerType _
        & "&RegionCount=" & CStr(RegionCount) _
        & "&RegionSize=" & CStr(TotalSize) _
        & "&MAC=" & Settings.MacAddress _
        & "&ID0=" & CreateMD5(CStr(d.SerialNumber)) _
        & "&r=" & RandomNumber.Random()
        Return data

    End Function

    Public Function OpenPorts() As Boolean

        If OpenRouterPorts() Then ' open UPnp port
            Logger("INFO", "UPNP OK", "Diagnostics")
            Settings.UPnpDiag = True
            Settings.SaveSettings()
            Return True
        Else
            TextPrint(My.Resources.UPNP_Disabled)
            Settings.UPnpDiag = False
            Settings.SaveSettings()
            Return False
        End If

    End Function

    Public Function OpenRouterPorts() As Boolean

        If Not PropMyUPnpMap.UPnpEnabled And Settings.UPnPEnabled Then
            Settings.UPnPEnabled = False
            Settings.SaveSettings()
            Return False
        End If

        If Not Settings.UPnPEnabled Then
            Return False
        End If
        TextPrint(My.Resources.Open_Router_Ports)

        Log("UPnP", "Local IP seems to be " & PropMyUPnpMap.LocalIP)

        Try

            If Settings.SCEnable Then

                'Icecast 8100-8101
                If PropMyUPnpMap.Exists(Convert.ToInt16(Settings.SCPortBase), UPnp.MyProtocol.TCP) Then
                    PropMyUPnpMap.Remove(Convert.ToInt16(Settings.SCPortBase), UPnp.MyProtocol.TCP)
                End If
                Application.DoEvents()
                If PropMyUPnpMap.Add(PropMyUPnpMap.LocalIP, CType(Settings.SCPortBase, Integer), UPnp.MyProtocol.TCP, "Icecast TCP Public " & Settings.SCPortBase.ToString(Globalization.CultureInfo.InvariantCulture)) Then
                    TextPrint("--> " & My.Resources.Icecast_is_Set & ":TCP:" & Settings.SCPortBase.ToString(Globalization.CultureInfo.InvariantCulture))
                End If
                If Not PropOpensimIsRunning() Then Return False
                '0 UDP
                If PropMyUPnpMap.Exists(Convert.ToInt16(Settings.SCPortBase), UPnp.MyProtocol.UDP) Then
                    PropMyUPnpMap.Remove(Convert.ToInt16(Settings.SCPortBase), UPnp.MyProtocol.UDP)
                End If
                Application.DoEvents()
                If PropMyUPnpMap.Add(PropMyUPnpMap.LocalIP, CType(Settings.SCPortBase, Integer), UPnp.MyProtocol.UDP, "Icecast UDP Public " & Settings.SCPortBase.ToString(Globalization.CultureInfo.InvariantCulture)) Then
                    TextPrint("--> " & My.Resources.Icecast_is_Set & ":UDP:" & Settings.SCPortBase.ToString(Globalization.CultureInfo.InvariantCulture))
                End If
                If Not PropOpensimIsRunning() Then Return False

                '1 TCP
                If PropMyUPnpMap.Exists(Convert.ToInt16(Settings.SCPortBase1), UPnp.MyProtocol.TCP) Then
                    PropMyUPnpMap.Remove(Convert.ToInt16(Settings.SCPortBase1), UPnp.MyProtocol.TCP)
                End If
                Application.DoEvents()
                If PropMyUPnpMap.Add(PropMyUPnpMap.LocalIP, CType(Settings.SCPortBase1, Integer), UPnp.MyProtocol.TCP, "Icecast1 TCP Public " & Settings.SCPortBase1.ToString(Globalization.CultureInfo.InvariantCulture)) Then
                    TextPrint("--> " & My.Resources.Icecast_is_Set & ":TCP:" & Settings.SCPortBase1.ToString(Globalization.CultureInfo.InvariantCulture))
                End If
                If Not PropOpensimIsRunning() Then Return False

                '0 UDP
                If PropMyUPnpMap.Exists(Convert.ToInt16(Settings.SCPortBase1), UPnp.MyProtocol.UDP) Then
                    PropMyUPnpMap.Remove(Convert.ToInt16(Settings.SCPortBase1), UPnp.MyProtocol.UDP)
                End If
                Application.DoEvents()
                If Not PropOpensimIsRunning() Then Return False

                If PropMyUPnpMap.Add(PropMyUPnpMap.LocalIP, CType(Settings.SCPortBase1, Integer), UPnp.MyProtocol.UDP, "Icecast1 UDP Public " & Settings.SCPortBase1.ToString(Globalization.CultureInfo.InvariantCulture)) Then
                    TextPrint("--> " & My.Resources.Icecast_is_Set & ":UDP:" & Settings.SCPortBase1.ToString(Globalization.CultureInfo.InvariantCulture))
                End If
                Application.DoEvents()
            End If ' IceCast

            If Settings.ApacheEnable Then
                If PropMyUPnpMap.Exists(Settings.ApachePort, UPnp.MyProtocol.TCP) Then
                    PropMyUPnpMap.Remove(Settings.ApachePort, UPnp.MyProtocol.TCP)
                End If
                If PropMyUPnpMap.Add(PropMyUPnpMap.LocalIP, Settings.ApachePort, UPnp.MyProtocol.TCP, "Apache TCP Public " & Settings.SCPortBase.ToString(Globalization.CultureInfo.InvariantCulture)) Then
                    TextPrint("--> " & My.Resources.Apache_is_Set & ":TCP:" & Settings.ApachePort.ToString(Globalization.CultureInfo.InvariantCulture))
                End If
                Application.DoEvents()
            End If

            If Not PropOpensimIsRunning() Then Return False

            ' 8001 for Diagnostics
            If PropMyUPnpMap.Exists(Convert.ToInt16(Settings.DiagnosticPort, Globalization.CultureInfo.InvariantCulture), UPnp.MyProtocol.TCP) Then
                PropMyUPnpMap.Remove(Convert.ToInt16(Settings.DiagnosticPort, Globalization.CultureInfo.InvariantCulture), UPnp.MyProtocol.TCP)
            End If
            If PropMyUPnpMap.Add(PropMyUPnpMap.LocalIP, Convert.ToInt16(Settings.DiagnosticPort, Globalization.CultureInfo.InvariantCulture), UPnp.MyProtocol.TCP, "Opensim TCP Grid " & Settings.HttpPort) Then
                TextPrint("--> " & My.Resources.Diag_TCP_is_set_word & ":" & Settings.DiagnosticPort.ToString(Globalization.CultureInfo.InvariantCulture))
            End If
            Application.DoEvents()
            If Not PropOpensimIsRunning() Then Return False

            ' 8002 for TCP
            If PropMyUPnpMap.Exists(Convert.ToInt16(Settings.HttpPort, Globalization.CultureInfo.InvariantCulture), UPnp.MyProtocol.TCP) Then
                PropMyUPnpMap.Remove(Convert.ToInt16(Settings.HttpPort, Globalization.CultureInfo.InvariantCulture), UPnp.MyProtocol.TCP)
            End If
            If PropMyUPnpMap.Add(PropMyUPnpMap.LocalIP, Convert.ToInt16(Settings.HttpPort, Globalization.CultureInfo.InvariantCulture), UPnp.MyProtocol.TCP, "Opensim TCP Grid " & Settings.HttpPort) Then
                TextPrint("--> " & My.Resources.Grid_TCP_is_set_word & ":" & Settings.HttpPort.ToString(Globalization.CultureInfo.InvariantCulture))
            End If
            Application.DoEvents()
            If Not PropOpensimIsRunning() Then Return False

            ' 8002 for UDP
            If PropMyUPnpMap.Exists(Convert.ToInt16(Settings.HttpPort, Globalization.CultureInfo.InvariantCulture), UPnp.MyProtocol.UDP) Then
                PropMyUPnpMap.Remove(Convert.ToInt16(Settings.HttpPort, Globalization.CultureInfo.InvariantCulture), UPnp.MyProtocol.UDP)
            End If
            If PropMyUPnpMap.Add(PropMyUPnpMap.LocalIP, Convert.ToInt16(Settings.HttpPort, Globalization.CultureInfo.InvariantCulture), UPnp.MyProtocol.UDP, "Opensim UDP Grid " & Settings.HttpPort) Then
                TextPrint("--> " & My.Resources.Grid_UDP_is_set_word & ":" & Settings.HttpPort.ToString(Globalization.CultureInfo.InvariantCulture))
            End If

            Application.DoEvents()

            For Each RegionUUID In RegionUuids()
                Dim R As Integer = Region_Port(RegionUUID)

                If PropMyUPnpMap.Exists(R, UPnp.MyProtocol.UDP) Then
                    PropMyUPnpMap.Remove(R, UPnp.MyProtocol.UDP)
                End If
                Application.DoEvents()
                If Not PropOpensimIsRunning() Then Return False

                If PropMyUPnpMap.Add(PropMyUPnpMap.LocalIP, R, UPnp.MyProtocol.UDP, "Opensim UDP Region " & Region_Name(RegionUUID) & " ") Then
                    TextPrint("--> " & Region_Name(RegionUUID) & ":UDP:" & R.ToString(Globalization.CultureInfo.InvariantCulture))
                End If
                Application.DoEvents()
                If Not PropOpensimIsRunning() Then Return False

                If PropMyUPnpMap.Exists(R, UPnp.MyProtocol.TCP) Then
                    PropMyUPnpMap.Remove(R, UPnp.MyProtocol.TCP)
                End If
                Application.DoEvents()
                If Not PropOpensimIsRunning() Then Return False

                If PropMyUPnpMap.Add(PropMyUPnpMap.LocalIP, R, UPnp.MyProtocol.TCP, "Opensim TCP Region " & Region_Name(RegionUUID) & " ") Then
                    TextPrint("--> " & Region_Name(RegionUUID) & ":TCP:" & R.ToString(Globalization.CultureInfo.InvariantCulture))
                End If
                Application.DoEvents()
                If Not PropOpensimIsRunning() Then Return False

            Next
        Catch ex As Exception
            BreakPoint.Dump(ex)
            Log("UPnP", "UPnP Exception caught:  " & ex.Message)
            Return False
        End Try
        Return True 'successfully added

    End Function

    Public Sub PortTest(Weblink As String, Port As Integer)

        Dim result As String = ""
        Using TimedClient As New TimedWebClient With {
               .Timeout = 1000
           }
            Try
                result = TimedClient.DownloadString(Weblink)
            Catch ex As WebException  ' not an error as could be a 404 from Diva being off
            Catch ex As Exception
                BreakPoint.Dump(ex)
                ErrorLog("Err:Loopback fail:" & result & ":" & ex.Message)
                Logger("Error", "Loopback fail: " & result & ":" & ex.Message, "Diagnostics")
            End Try
        End Using

        If result.Contains("<html") Or result.Contains("Ooops!") Or result.Length = 0 Then
            Logger("INFO", Global.Outworldz.My.Resources.Loopback_Passed & " " & Port.ToString(Globalization.CultureInfo.InvariantCulture), "Diagnostics")
            TextPrint(My.Resources.Loopback_Passed & " " & Port.ToString(Globalization.CultureInfo.InvariantCulture))
        Else
            TextPrint(My.Resources.Loopback_Failed & " " & Weblink)
            Logger("INFO", Global.Outworldz.My.Resources.Loopback_Failed & " " & Weblink, "Diagnostics")
            Settings.LoopbackDiag = False
            Settings.DiagFailed = "True"
        End If

    End Sub

    Public Function TestPrivateLoopback(verbose As Boolean) As Boolean

        Dim result As String = ""
        If verbose Then TextPrint(My.Resources.Checking_LAN_Loopback_word)
        If verbose Then Logger("Info", Global.Outworldz.My.Resources.Checking_LAN_Loopback_word, "Diagnostics")
        Dim weblink = $"http://{Settings.LANIP()}:{Settings.DiagnosticPort}/?_TestLoopback={RandomNumber.Random()}"
        If verbose Then Logger("Info", "URL= " & weblink, "Diagnostics")
        Using client As New WebClient
            Try
                result = client.DownloadString(weblink)
            Catch ex As Exception
                If verbose Then Logger("Error", ex.Message, "Diagnostics")
            End Try
        End Using

        If result = "Test Completed" Then
            If verbose Then Logger("INFO", Global.Outworldz.My.Resources.Passed_LAN, "Diagnostics")
            If verbose Then TextPrint(My.Resources.Passed_LAN)
            Return False
        Else
            If verbose Then Logger("INFO", Global.Outworldz.My.Resources.Failed_LAN & " " & weblink & " result was " & result, "Diagnostics")
            If verbose Then TextPrint(My.Resources.Failed_LAN & " " & weblink)
            Settings.LoopbackDiag = False
            Settings.DiagFailed = "True"
            Return True
        End If

    End Function

    Public Sub TestPublicLoopback()

        If IPCheck.IsPrivateIP(Settings.PublicIP) Then
            Logger("INFO", "Local LAN IP", "Diagnostics")
            Return
        End If

        If Settings.ServerType <> RobustServerName Then
            Logger("INFO", "Is Not Robust, Test Skipped", "Diagnostics")
            Return
        End If

        TextPrint(My.Resources.Checking_Loopback_word)
        'Logger("INFO", Global.Outworldz.My.Resources.Checking_Loopback_word, "Diagnostics")
        PortTest("http://" & Settings.PublicIP & ":" & Settings.HttpPort & "/?_TestLoopback=" & RandomNumber.Random, Settings.HttpPort)

    End Sub

    Private Sub ProbePublicPort()

        If Settings.ServerType <> RobustServerName Then
            Logger("INFO", "Server Is Not Robust", "Diagnostics")
            Return
        End If

        Dim isPortOpen As String = ""
        Using client As New WebClient ' download client for web pages

            ' collect some stats and test loopback with a HTTP_ GET to the webserver. Send unique, anonymous random ID, both of the versions of Opensim and this program, and the diagnostics test
            ' results See my privacy policy at https://outworldz.com/privacy.htm

            TextPrint(My.Resources.Checking_Router_word)
            Dim Url = $"{PropHttpsDomain}/cgi/probetest.plx{GetPostData()}"
            Logger("INFO", "Using URL " & Url, "Diagnostics")
            Try
                isPortOpen = client.DownloadString(Url)
            Catch ex As WebException  ' not an error as could be a 404 from Diva being off
                BreakPoint.Dump(ex)
                Logger("Error", Global.Outworldz.My.Resources.Wrong & " " & ex.Message, "Diagnostics")
                ErrorLog(My.Resources.Wrong & " " & ex.Message)
            End Try
        End Using

        If isPortOpen = "yes" Then
            Logger("INFO", Global.Outworldz.My.Resources.Incoming_Works, "Diagnostics")
            TextPrint(My.Resources.Incoming_Works)
        Else
            Settings.LoopbackDiag = False
            Settings.DiagFailed = "True"
            Logger("INFO", Global.Outworldz.My.Resources.Internet_address & " " & Settings.PublicIP & ":" & Settings.HttpPort & Global.Outworldz.My.Resources.Not_Forwarded, "Diagnostics")
            TextPrint(My.Resources.Internet_address & " " & Settings.PublicIP & ":" & Settings.HttpPort & Global.Outworldz.My.Resources.Not_Forwarded)
        End If

    End Sub

    Private Sub TestAllRegionPorts()

        Dim result As String = ""
        Dim Len = RegionCount()

        Dim Used As New List(Of String)
        ' Boot them up

        For Each RegionUUID In RegionUuids()
            If IsBooted(RegionUUID) Then
                Dim RegionName = Region_Name(RegionUUID)

                If Used.Contains(RegionName) Then Continue For
                Used.Add(RegionName)
                Logger("INFO", "Testing region " & RegionName, "Diagnostics")
                Dim Port = GroupPort(RegionUUID)
                TextPrint(My.Resources.Checking_Loopback_word & " " & RegionName)
                Logger("INFO", Global.Outworldz.My.Resources.Checking_Loopback_word & " " & RegionName, "Diagnostics")
                PortTest("http://" & Settings.PublicIP & ":" & Port & "/?_TestLoopback=" & RandomNumber.Random, Port)
            End If
        Next

    End Sub

End Module
