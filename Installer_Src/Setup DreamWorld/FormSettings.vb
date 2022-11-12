#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Public Class FormSettings

#Region "Declarations"

#Disable Warning CA2213

    Private Backups As New FormAutoBackups
    Dim Banlist As New FormBanList
    Private Bird As New FormBird
    Dim Email As New FormEmailSetup
    Dim FormApache As New FormApache
    Dim FormCache As New FormCaches
    Dim FormDatabase As New FormDatabase
    Dim FormDiva As New FormDiva
    Dim FormDNSName As New FormDnsName
    Dim FormJoomla As New FormJoomla
    Dim FormPermissions As New FormPermissions
    Dim FormPhysics As New FormPhysics
    Dim FormPorts As New FormPorts
    Dim FormPublicity As New FormPublicity
    Dim FormRegions As New FormRegions
    Dim FormRestart As New FormRestart
    Dim FormServerType As New FormServerType
    Dim FormSpeech As New FormSpeech
    Dim FsAssets As New FormFSAssets
    Dim Gloebits As New FormGloebits
    Dim Icecast As New FormIcecast
    Dim Lang As New Language
    Dim Logging As New FormLogging
    Dim Maps As New FormMaps
    Dim Scripts As New FormScripts
    Dim Search As New FormSearch
    Dim SS As New FormSmartStart
    Dim Tide As New FormTide
    Dim Tos As New TosForm
    Dim Voice As New FormVoice
#Enable Warning CA2213

#End Region

#Region "ScreenSize"

    Private ReadOnly Handler As New EventHandler(AddressOf Resize_page)
    Private _screenPosition As ClassScreenpos

    Public Property ScreenPosition As ClassScreenpos
        Get
            Return _screenPosition
        End Get
        Set(value As ClassScreenpos)
            _screenPosition = value
        End Set
    End Property

    'The following detects  the location of the form in screen coordinates
    Private Sub Resize_page(ByVal sender As Object, ByVal e As System.EventArgs)
        'Me.Text = "Form screen position = " + Me.Location.ToString
        ScreenPosition.SaveXY(Me.Left, Me.Top)
        ScreenPosition.SaveHW(Me.Height, Me.Width)
    End Sub

    Private Sub SetScreen()

        Try
            ScreenPosition = New ClassScreenpos(Me.Name)
            AddHandler ResizeEnd, Handler
            Dim xy As List(Of Integer) = ScreenPosition.GetXY()
            Me.Left = xy.Item(0)
            Me.Top = xy.Item(1)
            Dim hw As List(Of Integer) = ScreenPosition.GetHW()

            If hw.Item(0) = 0 Then
                Me.Height = 500
            Else
                Me.Height = hw.Item(0)
            End If
            If hw.Item(1) = 0 Then
                Me.Width = 600
            Else
                Me.Width = hw.Item(1)
            End If
        Catch ex As Exception
            BreakPoint.Dump(ex)
            Me.Height = 500
            Me.Width = 600
            Me.Left = 100
            Me.Top = 100
        End Try
    End Sub

#End Region

#Region "Private Methods"

    Public Sub Init()

        ApacheButton.Text = Global.Outworldz.My.Resources.Apache_Webserver
        BackupButton1.Text = Global.Outworldz.My.Resources.Backup_Settings_word
        BanListButton.Text = Global.Outworldz.My.Resources.Ban_List_word
        Birds.Text = Global.Outworldz.My.Resources.Bird_Settings_word
        CacheButton1.Text = Global.Outworldz.My.Resources.Caches_word
        CurrencyButton.Text = Global.Outworldz.My.Resources.Currency_word
        DatabaseButton2.Text = Global.Outworldz.My.Resources.Database_Setup_word
        DivaButton1.Text = Global.Outworldz.My.Resources.Web
        DNSButton.Text = Global.Outworldz.My.Resources.Hypergrid
        EmailButton.Text = Global.Outworldz.My.Resources.Email_word
        FSAssetsButton.Text = Global.Outworldz.My.Resources.FSasset_word
        IcecastButton.Text = Global.Outworldz.My.Resources.Icecast_word
        LanguageButton.Text = Global.Outworldz.My.Resources.Language
        LoggingButton.Text = Global.Outworldz.My.Resources.Logging_word
        MapsButton.Text = Global.Outworldz.My.Resources.Maps_word
        PermissionsButton.Text = Global.Outworldz.My.Resources.Permissions_word
        PhysicsButton1.Text = Global.Outworldz.My.Resources.Physics_word
        PortsButton1.Text = Global.Outworldz.My.Resources.Network_Ports
        PublicityButton.Text = Global.Outworldz.My.Resources.Publicity_Word
        RegionsButton1.Text = Global.Outworldz.My.Resources.Regions_word
        RestartButton.Text = Global.Outworldz.My.Resources.Restart_Settings_word
        ScriptButton.Text = Global.Outworldz.My.Resources.Scripts_word
        SearchButton.Text = Global.Outworldz.My.Resources.SearchOptions_word
        SpeechButton.Text = Global.Outworldz.My.Resources.Speech
        ServerTypeButton.Text = Global.Outworldz.My.Resources.Server_Type_word
        SmartStartButton.Text = Global.Outworldz.My.Resources.Smart_Start_word
        TideButton.Text = Global.Outworldz.My.Resources.Tides_word
        TOSButton.Text = Global.Outworldz.My.Resources.Terms_of_Service
        ToolTip1.SetToolTip(ApacheButton, Global.Outworldz.My.Resources.ApacheWebServer)
        ToolTip1.SetToolTip(BackupButton1, Global.Outworldz.My.Resources.Backup_Schedule)
        ToolTip1.SetToolTip(BanListButton, Global.Outworldz.My.Resources.BanList_string)
        ToolTip1.SetToolTip(Birds, Global.Outworldz.My.Resources.Click_Birds)
        ToolTip1.SetToolTip(CacheButton1, Global.Outworldz.My.Resources.Click_Caches)
        ToolTip1.SetToolTip(CurrencyButton, Global.Outworldz.My.Resources.Click_Currency)
        ToolTip1.SetToolTip(DatabaseButton2, Global.Outworldz.My.Resources.Click_Database)
        ToolTip1.SetToolTip(DivaButton1, Global.Outworldz.My.Resources.Click_Web)
        ToolTip1.SetToolTip(DNSButton, Global.Outworldz.My.Resources.Click_HG)
        ToolTip1.SetToolTip(EmailButton, Global.Outworldz.My.Resources.No_Email)
        ToolTip1.SetToolTip(FSAssetsButton, Global.Outworldz.My.Resources.Click_Fsassets)
        ToolTip1.SetToolTip(IcecastButton, Global.Outworldz.My.Resources.Click_Icecast)
        ToolTip1.SetToolTip(JoomlaButton, Global.Outworldz.My.Resources.Click_Setup_Jopensim)
        ToolTip1.SetToolTip(LanguageButton, Global.Outworldz.My.Resources.Language)
        ToolTip1.SetToolTip(LoggingButton, Global.Outworldz.My.Resources.Log_Level)
        ToolTip1.SetToolTip(MapsButton, Global.Outworldz.My.Resources.Click_Maps)
        ToolTip1.SetToolTip(PermissionsButton, Global.Outworldz.My.Resources.Click_for_God_Mode)
        ToolTip1.SetToolTip(PhysicsButton1, Global.Outworldz.My.Resources.Click_Physics)
        ToolTip1.SetToolTip(PortsButton1, Global.Outworldz.My.Resources.Click_Ports)
        ToolTip1.SetToolTip(PublicityButton, Global.Outworldz.My.Resources.Click_Publicity)
        ToolTip1.SetToolTip(RegionsButton1, Global.Outworldz.My.Resources.Click_Regions)
        ToolTip1.SetToolTip(RestartButton, Global.Outworldz.My.Resources.Click_Restart)
        ToolTip1.SetToolTip(ScriptButton, Global.Outworldz.My.Resources.Click_to_View_this_word)
        ToolTip1.SetToolTip(ServerTypeButton, Global.Outworldz.My.Resources.Click_Server)
        ToolTip1.SetToolTip(TideButton, Global.Outworldz.My.Resources.Click_Tides)
        ToolTip1.SetToolTip(TOSButton, Global.Outworldz.My.Resources.Setup_TOS)
        ToolTip1.SetToolTip(VoiceButton1, Global.Outworldz.My.Resources.Click_Voice)

        VoiceButton1.Text = Global.Outworldz.My.Resources.Vivox_Voice_word

        SetScreen()

        Me.Visible = True
        Me.ToolTip1.SetToolTip(Me.TOSButton, Global.Outworldz.My.Resources.Setup_TOS)

    End Sub

    Private Sub Advanced_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        SetScreen()
        Init()

    End Sub

#End Region

#Region "Help"

    Private Sub PictureBox1_Click_1(sender As Object, e As EventArgs)

        Dim webAddress As String = PropHttpsDomain + "/Outworldz_installer/technical.htm#Regions"
        Try
            Process.Start(webAddress)
        Catch ex As Exception
        End Try

    End Sub

#End Region

#Region "Clicks"

    Private Sub ApacheButton_Click(sender As Object, e As EventArgs) Handles ApacheButton.Click

        FormApache.Close()
        FormApache.Dispose()
        FormApache = New FormApache
        FormApache.Activate()
        FormApache.Visible = True
        FormApache.Select()
        FormApache.BringToFront()

    End Sub

    Private Sub BackupButton1_Click(sender As Object, e As EventArgs) Handles BackupButton1.Click

        Backups.Close()
        Backups.Dispose()
        Backups = New FormAutoBackups
        Backups.Activate()
        Backups.Visible = True
        Backups.Select()
        Backups.BringToFront()

    End Sub

    Private Sub BanListButton_Click(sender As Object, e As EventArgs) Handles BanListButton.Click

        If Settings.ServerType = RobustServerName Then
            Banlist.Close()
            Banlist.Dispose()
            Banlist = New FormBanList

            Banlist.Activate()
            Banlist.Visible = True
            Banlist.Select()
            Banlist.BringToFront()
        Else
            Banlist.Close()
            Banlist.Dispose()
        End If

    End Sub

    Private Sub Birds_Click(sender As Object, e As EventArgs) Handles Birds.Click

        Bird.Close()
        Bird.Dispose()
        Bird = New FormBird
        Bird.Activate()
        Bird.Visible = True
        Bird.Select()
        Bird.BringToFront()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles CurrencyButton.Click

        Gloebits.Close()
        Gloebits.Dispose()
        Gloebits = New FormGloebits
        Gloebits.Activate()
        Gloebits.Visible = True
        Gloebits.Select()
        Gloebits.BringToFront()

    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles PortsButton1.Click

        FormPorts.Close()
        FormPorts.Dispose()
        FormPorts = New FormPorts
        FormPorts.Activate()
        FormPorts.Visible = True
        FormPorts.Select()
        FormPorts.BringToFront()

    End Sub

    Private Sub Button1_Click_2(sender As Object, e As EventArgs) Handles ServerTypeButton.Click

        FormServerType.Close()
        FormServerType.Dispose()
        FormServerType = New FormServerType With {
            .Visible = True
        }
        FormServerType.Select()
        FormServerType.BringToFront()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles PermissionsButton.Click

        FormPermissions.Close()
        FormPermissions.Dispose()
        FormPermissions = New FormPermissions
        FormPermissions.Activate()
        FormPermissions.Visible = True
        FormPermissions.Select()
        FormPermissions.BringToFront()

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles RestartButton.Click

        FormRestart.Close()
        FormRestart.Dispose()
        FormRestart = New FormRestart
        FormRestart.Activate()
        FormRestart.Visible = True
        FormRestart.Select()
        FormRestart.BringToFront()

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles PublicityButton.Click

        FormPublicity.Close()
        FormPublicity.Dispose()
        FormPublicity = New FormPublicity
        FormPublicity.Activate()
        FormPublicity.Visible = True
        FormPublicity.Select()
        FormPublicity.BringToFront()

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles JoomlaButton.Click

        FormJoomla.Close()
        FormJoomla.Dispose()
        FormJoomla = New FormJoomla
        FormJoomla.Activate()
        FormJoomla.Visible = True
        FormJoomla.Select()
        FormJoomla.BringToFront()

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles FSAssetsButton.Click

        FsAssets.Close()
        FsAssets.Dispose()
        FsAssets = New FormFSAssets
        FsAssets.Activate()
        FsAssets.Visible = True
        FsAssets.Select()
        FsAssets.BringToFront()

    End Sub

    Private Sub CacheButton1_Click(sender As Object, e As EventArgs) Handles CacheButton1.Click

        FormCache.Close()
        FormCache.Dispose()
        FormCache = New FormCaches
        FormCache.Activate()
        FormCache.Visible = True
        FormCache.Select()
        FormCache.BringToFront()

    End Sub

    Private Sub DatabaseButton2_Click(sender As Object, e As EventArgs) Handles DatabaseButton2.Click

        FormDatabase.Close()
        FormDatabase.Dispose()
        FormDatabase = New FormDatabase
        FormDatabase.Activate()
        FormDatabase.Visible = True
        FormDatabase.Select()
        FormDatabase.BringToFront()

    End Sub

    Private Sub DivaButton1_Click(sender As Object, e As EventArgs) Handles DivaButton1.Click

        FormDiva.Close()
        FormDiva.Dispose()
        FormDiva = New FormDiva
        FormDiva.Activate()
        FormDiva.Visible = True
        FormDiva.Select()
        FormDiva.BringToFront()

    End Sub

    Private Sub DNSButton_Click(sender As Object, e As EventArgs) Handles DNSButton.Click

        FormDNSName.Close()
        FormDNSName.Dispose()
        FormDNSName = New FormDnsName
        FormDNSName.Activate()
        FormDNSName.Visible = True
        FormDNSName.Select()
        FormDNSName.BringToFront()

    End Sub

    Private Sub EmailButton_Click(sender As Object, e As EventArgs) Handles EmailButton.Click

        Email.Close()
        Email.Dispose()
        Email = New FormEmailSetup
        Email.Activate()
        Email.Visible = True
        Email.Select()
        Email.BringToFront()

    End Sub

    Private Sub LanguageButton_Click(sender As Object, e As EventArgs) Handles LanguageButton.Click

        Lang.Close()
        Lang.Dispose()
        Lang = New Language
        Lang.Activate()
        Lang.Visible = True
        Lang.Select()
        Lang.BringToFront()

    End Sub

    Private Sub LoggingButton_Click(sender As Object, e As EventArgs) Handles LoggingButton.Click

        Logging.Close()
        Logging.Dispose()
        Logging = New FormLogging With {
            .Visible = True
        }
        Logging.Activate()
        Logging.Select()
        Logging.BringToFront()

    End Sub

    Private Sub MapsButton_Click(sender As Object, e As EventArgs) Handles MapsButton.Click

        Maps.Close()
        Maps.Dispose()
        Maps = New FormMaps
        Maps.Activate()
        Maps.Select()
        Maps.Visible = True
        Maps.BringToFront()

    End Sub

    Private Sub PhysicsButton1_Click(sender As Object, e As EventArgs) Handles PhysicsButton1.Click

        FormPhysics.Close()
        FormPhysics.Dispose()
        FormPhysics = New FormPhysics
        FormPhysics.Activate()
        FormPhysics.Visible = True
        FormPhysics.Select()
        FormPhysics.BringToFront()

    End Sub

    Private Sub RegionsButton1_Click(sender As Object, e As EventArgs) Handles RegionsButton1.Click

        FormRegions.Close()
        FormRegions.Dispose()
        FormRegions = New FormRegions
        FormRegions.Activate()
        FormRegions.Visible = True
        FormRegions.Select()
        FormRegions.BringToFront()

    End Sub

    Private Sub ScriptButton_Click(sender As Object, e As EventArgs) Handles ScriptButton.Click

        Scripts.Close()
        Scripts.Dispose()
        Scripts = New FormScripts
        Scripts.Activate()
        Scripts.Visible = True
        Scripts.Select()
        Scripts.BringToFront()

    End Sub

    Private Sub SearchButton_Click(sender As Object, e As EventArgs) Handles SearchButton.Click

        Search.Close()
        Search.Dispose()
        Search = New FormSearch
        Search.Activate()
        Search.Visible = True
        Search.Select()
        Search.BringToFront()

    End Sub

    Private Sub Shoutcast_Click(sender As Object, e As EventArgs) Handles IcecastButton.Click

        Icecast.Close()
        Icecast.Dispose()
        Icecast = New FormIcecast With {
            .Visible = True
        }
        Icecast.Activate()
        Icecast.Select()
        Icecast.BringToFront()

    End Sub

    Private Sub SmartStartButton_Click(sender As Object, e As EventArgs) Handles SmartStartButton.Click

        SS.Close()
        SS.Dispose()
        SS = New FormSmartStart
        SS.Activate()
        SS.Visible = True
        SS.Select()
        SS.BringToFront()

    End Sub

    Private Sub SpeechButton_Click(sender As Object, e As EventArgs) Handles SpeechButton.Click

        FormSpeech.Close()
        FormSpeech.Dispose()
        FormSpeech = New FormSpeech With {
            .Visible = True
        }
        FormSpeech.Select()
        FormSpeech.BringToFront()

    End Sub

    Private Sub TideButton_Click(sender As Object, e As EventArgs) Handles TideButton.Click

        Tide.Close()
        Tide.Dispose()
        Tide = New FormTide
        Tide.Activate()
        Tide.Visible = True
        Tide.Select()
        Tide.BringToFront()

    End Sub

    Private Sub TOSButton_Click(sender As Object, e As EventArgs) Handles TOSButton.Click

        Tos.Close()
        Tos.Dispose()
        Tos = New TosForm
        Tos.Activate()
        Tos.Visible = True
        Tos.Select()
        Tos.BringToFront()

    End Sub

    Private Sub VoiceButton1_Click(sender As Object, e As EventArgs) Handles VoiceButton1.Click

        Voice.Close()
        Voice.Dispose()
        Voice = New FormVoice
        Voice.Activate()
        Voice.Visible = True
        Voice.Select()
        Voice.BringToFront()

    End Sub

#End Region

End Class
