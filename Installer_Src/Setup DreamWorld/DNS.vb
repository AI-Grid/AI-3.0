Imports System.Net

Module DNS

    Public Function GetNewDnsName() As String

        Dim Checkname As String
        Using client As New WebClient
            Try
                Checkname = client.DownloadString("http://ns1.outworldz.com/getnewname.plx/?r=" & RandomNumber.Random)
            Catch ex As Exception
                Try
                    Checkname = client.DownloadString("http://ns2.outworldz.com/getnewname.plx/?r=" & RandomNumber.Random)
                Catch ex1 As Exception
                    ErrorLog("Warn: Cannot get new name:" & ex1.Message)
                    Return ""
                End Try
                BreakPoint.Dump(ex)
                ErrorLog("Error:Cannot get new name:" & ex.Message)
                Return ""
            End Try
        End Using

        Return Checkname

    End Function

    Public Sub NewDNSName()

        If Settings.DnsName.Length = 0 And Settings.EnableHypergrid Then
            Dim newname = GetNewDnsName()
            If newname.Length >= 0 Then
                If RegisterName(newname) Then
                    Settings.DnsName = newname
                    Settings.PublicIP = newname
                    Settings.SaveSettings()
                    MsgBox(My.Resources.NameAlreadySet, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Information_word)
                End If
            End If

        End If

    End Sub

    Public Function RegisterName(DNSName As String) As Boolean

        DNSName = DNSName.Trim
        If DNSName Is Nothing Then Return False
        If DNSName.Length = 0 Then Return False
        Dim Checkname As String = String.Empty

        If IPCheck.IsPrivateIP(DNSName) Then
            Return False
        End If

        Dim DNS = New List(Of String) From {
             "http://ns1.outworldz.net/dns.plx" & GetPostData(DNSName),
             "http://ns2.outworldz.net/dns.plx" & GetPostData(DNSName),
             "http://ns1.outworldz.com/dns.plx" & GetPostData(DNSName),
             "http://ns2.outworldz.com/dns.plx" & GetPostData(DNSName)
            }

        Using client As New WebClient ' download client for web pages
            For Each url In DNS
                Try
                    Checkname = client.DownloadString(url)
                    Logger("DNS", url, "Outworldz")
                Catch ex As Exception
                    ErrorLog("Warn: Cannot register this DNS Name " & ex.Message)
                End Try

                If Checkname = "UPDATE" Then
                    Return True
                ElseIf Checkname = "NAK" Then
                    MsgBox(DNSName & ":" & My.Resources.DDNS_In_Use, vbInformation Or MsgBoxStyle.MsgBoxSetForeground)
                    Exit For
                End If
                Application.doevents()
            Next
        End Using

        Return False

    End Function

    Public Sub SetPublicIP()

        Application.DoEvents()
        Settings.WANIP = WANIP()
        Settings.LANIP = PropMyUPnpMap.LocalIP
        Settings.MacAddress = GetMacByIP(Settings.LANIP)

        ' Region Name override
        If Settings.OverrideName.Length > 0 Then
            RegisterName(Settings.OverrideName)
        End If

        ' set up the alternate DNS names
        Dim a As String() = Settings.AltDnsName.Split(",".ToCharArray())
        For Each part As String In a
            If part.Length > 0 Then
                RegisterName(part)
            End If
        Next

        ' WAN USE
        If Settings.DnsName.Length = 0 Then
            Settings.PublicIP = Settings.LANIP
        ElseIf Settings.DnsName.Length > 0 Then
            Settings.PublicIP = Settings.DnsName()
        ElseIf IsPrivateIP(Settings.PublicIP) Then
            ' NAT'd ROUTER
            Settings.PublicIP = Settings.LANIP
        Else
            ' WAN IP such as Contabo without a NAT
            Settings.PublicIP = Settings.WANIP
        End If

        Settings.BaseHostName = Settings.PublicIP

        RegisterName(Settings.PublicIP)

    End Sub

End Module
