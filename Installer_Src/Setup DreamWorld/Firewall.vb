#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.IO
Imports System.Threading

Public Module Firewall

    Dim FirewallLock As New Object

    Function AddFirewallRules() As String

        ' TCP only for 8001 (DiagnosticPort) and both for 8002
        Dim Command As String = $"netsh advfirewall firewall add rule name=""Diagnostics TCP Port {Settings.DiagnosticPort}"" dir=in action=allow protocol=TCP localport={CStr(Settings.DiagnosticPort)}" & vbCrLf _
                          & $"netsh advfirewall firewall add rule name=""Opensim HTTP TCP Port {Settings.HttpPort}"" dir=in action=allow protocol=TCP localport={CStr(Settings.HttpPort)}" & vbCrLf _
                          & $"netsh advfirewall firewall add rule name=""Opensim HTTP UDP Port {Settings.HttpPort}"" dir=in action=allow protocol=UDP localport={CStr(Settings.HttpPort)}" & vbCrLf

        If Settings.ApacheEnable Then
            Command = Command & $"netsh advfirewall firewall add rule name=""Apache HTTP Web Port {CStr(Settings.ApachePort)}"" dir=in action=allow protocol=TCP localport={CStr(Settings.ApachePort)}" & vbCrLf
        End If

        If Settings.ApacheEnable AndAlso Settings.SSLEnabled And Settings.SSLIsInstalled Then
            Command = Command & "netsh advfirewall firewall add rule name=""Apache HTTPS Web Port 443"" dir=in action=allow protocol=TCP localport=443" & vbCrLf
        End If

        ' Icecast needs both ports for both protocols
        If Settings.SCEnable Then
            Command = Command & $"netsh advfirewall firewall add rule name=""Icecast Port1 UDP {CStr(Settings.SCPortBase)}"" dir=in action=allow protocol=UDP localport=" & CStr(Settings.SCPortBase) & vbCrLf _
                          & $"netsh advfirewall firewall add rule name=""Icecast Port1 TCP {CStr(Settings.SCPortBase)}"" dir=in action=allow protocol=TCP localport={CStr(Settings.SCPortBase)}" & vbCrLf _
                          & $"netsh advfirewall firewall add rule name=""Icecast Port2 UDP {CStr(Settings.SCPortBase1)}"" dir=In action=allow protocol=UDP localport={CStr(Settings.SCPortBase1)}" & vbCrLf _
                          & $"netsh advfirewall firewall add rule name=""Icecast Port2 TCP {CStr(Settings.SCPortBase1)}"" dir=In action=allow protocol=TCP localport={CStr(Settings.SCPortBase1)}" & vbCrLf
        End If

        ' regions need both

        Command = Command & $"netsh advfirewall firewall add rule name=""Region TCP Ports {CStr(Settings.FirstRegionPort)}-{CStr(LargestPort())}"" dir=In action=allow protocol=TCP localport={CStr(Settings.FirstRegionPort)}-{CStr(LargestPort())}" & vbCrLf _
                  & $"netsh advfirewall firewall add rule name=""Region UDP Ports {CStr(Settings.FirstRegionPort)}-{CStr(LargestPort())}"" dir=In action=allow protocol=UDP localport={CStr(Settings.FirstRegionPort)}-{CStr(LargestPort())}" & vbCrLf

        Return Command

    End Function

    Public Sub BlockIP(IPAddress As String)

        Dim Command As String = $"netsh advfirewall firewall delete rule name=""Opensim Deny {IPAddress}""" & vbCrLf
        Command += $"netsh advfirewall firewall add rule name=""Opensim Deny {IPAddress}"" dir=In profile=any action=block protocol=any remoteip={IPAddress}" & vbCrLf

        RunFirewall(Command)

    End Sub

    Function DeleteNewFirewallRules() As String

        Dim Command As String = $"netsh advfirewall firewall delete rule name=""Diagnostics TCP Port {CStr(Settings.DiagnosticPort)}""" & vbCrLf _
                          & $"netsh advfirewall firewall delete rule name=""Opensim HTTP TCP Port {CStr(Settings.HttpPort)}""" & vbCrLf _
                          & $"netsh advfirewall firewall delete rule name=""Opensim HTTP UDP Port {CStr(Settings.HttpPort)}""" & vbCrLf

        Command += $"netsh advfirewall firewall delete rule name=""Icecast Port1 UDP {CStr(Settings.SCPortBase)}""" & vbCrLf _
                        & $"netsh advfirewall firewall delete rule name=""Icecast Port1 TCP {CStr(Settings.SCPortBase)}""" & vbCrLf _
                        & $"netsh advfirewall firewall delete rule name=""Icecast Port2 UDP {CStr(Settings.SCPortBase1)}""" & vbCrLf _
                        & $"netsh advfirewall firewall delete rule name=""Icecast Port2 TCP {CStr(Settings.SCPortBase1)}""" & vbCrLf

        Command = Command & $"netsh advfirewall firewall delete rule name=""Apache HTTP Web Port {CStr(Settings.ApachePort)}""" & vbCrLf
        Command = Command & $"netsh advfirewall firewall delete rule name=""Apache HTTPS Web Port 443""" & vbCrLf

        Command = Command & $"netsh advfirewall firewall delete rule name=""Region TCP Ports {CStr(Settings.FirstRegionPort)}-{CStr(LargestPort())}""" & vbCrLf _
                          & $"netsh advfirewall firewall delete rule name=""Region UDP Ports {CStr(Settings.FirstRegionPort)}-{CStr(LargestPort())}""" & vbCrLf

        Return Command

    End Function

    Function DeleteOldFirewallRules() As String

        Dim Command As String = $"netsh advfirewall firewall delete rule name=""Diagnostics TCP Port {CStr(Settings.DiagnosticPort)}""" & vbCrLf _
                          & $"netsh advfirewall firewall delete rule name=""Opensim HTTP TCP Port {CStr(Settings.HttpPort)}""" & vbCrLf _
                          & $"netsh advfirewall firewall delete rule name=""Opensim HTTP UDP Port {CStr(Settings.HttpPort)}""" & vbCrLf

        Command += $"netsh advfirewall firewall delete rule name=""Icecast Port1 UDP {CStr(Settings.SCPortBase)}""" & vbCrLf _
                          & $"netsh advfirewall firewall delete rule name=""Icecast Port1 TCP {CStr(Settings.SCPortBase)}""" & vbCrLf _
                          & $"netsh advfirewall firewall delete rule name=""Icecast Port2 UDP {CStr(Settings.SCPortBase1)}""" & vbCrLf _
                          & $"netsh advfirewall firewall delete rule name=""Icecast Port2 TCP {CStr(Settings.SCPortBase1)}""" & vbCrLf

        If Settings.ApacheEnable Then
            Command = Command & $"netsh advfirewall firewall delete rule name=""Apache HTTP Web Port {CStr(Settings.ApachePort)}""" & vbCrLf
        End If

        For Each RegionUUID In RegionUuids()

            Dim R As Integer = CInt("0" & Region_Port(RegionUUID))
            If R > 0 Then
                Command = Command & $"netsh advfirewall firewall delete rule name=""Region TCP Port {CStr(R)}""" & vbCrLf _
                              & $"netsh advfirewall firewall delete rule name=""Region UDP Port {CStr(R)}""" & vbCrLf
            End If
        Next
        ' also obsolete so we can use two or more dreamgrids on a server
        Command = Command & $"netsh advfirewall firewall delete rule name=""Region TCP Ports""" & vbCrLf _
                          & $"netsh advfirewall firewall delete rule name=""Region UDP Ports""" & vbCrLf

        Settings.FirewallMigrated = 1

        Return Command

    End Function

    Public Sub ReleaseIp(IP As String)

        Dim Command As String = $"netsh advfirewall firewall delete rule name=""Opensim Deny {IP}""" & vbCrLf
        RunFirewall(Command)

    End Sub

    Sub SetFirewall()

        SyncLock FirewallLock
            Dim start As ParameterizedThreadStart = AddressOf SetFirewall2
            Dim _WebThread1 = New Thread(start)
            _WebThread1.SetApartmentState(ApartmentState.STA)
            _WebThread1.Priority = ThreadPriority.BelowNormal
            _WebThread1.Start()
        End SyncLock

    End Sub

    Sub SetFirewall2()

        Dim start As ParameterizedThreadStart = AddressOf RunFirewall
        If Not Settings.FirewallMigrated = 2 Then
            Dim CMD1 As Object = DeleteOldFirewallRules()
            Dim _WebThread1 = New Thread(start)
            _WebThread1.SetApartmentState(ApartmentState.STA)
            _WebThread1.Priority = ThreadPriority.BelowNormal
            _WebThread1.Start(CMD1)
            _WebThread1.Join()
        End If

        Dim CMD2 As Object = DeleteNewFirewallRules() & AddFirewallRules()
        Dim _WebThread2 = New Thread(start)
        _WebThread2.SetApartmentState(ApartmentState.STA)
        _WebThread2.Priority = ThreadPriority.BelowNormal
        _WebThread2.Start(CMD2)

    End Sub

    Private Sub RunFirewall(CMD As Object)

        Dim file = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles/tmp/" & CStr(DateTime.Now.Ticks) & "_fw.bat")
        Try
            Using ns = New StreamWriter(file, False)
                ns.WriteLine(CStr(CMD))
            End Using
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

        Dim pi = New ProcessStartInfo With {
        .Arguments = "",
        .FileName = file,
        .WindowStyle = ProcessWindowStyle.Hidden,
        .Verb = "runas"
    }
        Using ProcessFirewall = New Process With {
            .StartInfo = pi
        }
            Try
                ProcessFirewall.Start()
                ProcessFirewall.WaitForExit()
                FileStuff.DeleteFile(file)
            Catch ex As Exception
                Log(My.Resources.Error_word, "Could not set firewall:" & ex.Message)
            End Try
        End Using
        'Application.ExitThread()
    End Sub

End Module
