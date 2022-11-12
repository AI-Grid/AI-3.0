#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Module GridNames

    Public Sub SetServerType()

        If Settings.ServerType = RobustServerName Then
            TextPrint("--> " & My.Resources.Server_Type_is & " Robust")
            Settings.ExternalHostName = Settings.PublicIP
        ElseIf Settings.ServerType = OsgridServer Then
            Settings.DnsName = "hg.osgrid.org"
            Settings.BaseHostName = "hg.osgrid.org"
            Settings.ExternalHostName = WANIP()
            TextPrint("--> " & My.Resources.Server_Type_is & " OSGrid")
        ElseIf Settings.ServerType = RegionServerName Then
            If Settings.OverrideName.Length > 0 Then
                Settings.ExternalHostName = Settings.OverrideName
            Else
                Settings.ExternalHostName = Settings.PublicIP
            End If
            TextPrint("--> " & My.Resources.Server_Type_is & " Region")
        ElseIf Settings.ServerType = MetroServer Then
            Settings.DnsName = "hg.metro.land"
            Settings.ExternalHostName = Settings.PublicIP
            Settings.BaseHostName = "hg.metro.land"
            TextPrint("--> " & My.Resources.Server_Type_is & " Metro")
        End If

        If Settings.OverrideName.Length > 0 Then
            Settings.ExternalHostName = Settings.OverrideName
            TextPrint("--> Region IP=" & Settings.ExternalHostName)
        ElseIf Settings.ServerType = RobustName() Then
            Settings.ExternalHostName = Settings.PublicIP
            TextPrint("--> Region IP=" & Settings.ExternalHostName)
        End If

    End Sub

End Module
