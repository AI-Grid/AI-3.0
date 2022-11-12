#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.ComponentModel
Imports System.IO
Imports System.Text.RegularExpressions

Public Class FormRegion

#Region "Declarations"

    Dim _RegionUUID As String = ""
    Dim BoxSize As Integer = 256
    Dim changed As Boolean

    ' needed a flag to see if we are initialized as the dialogs change on start. true if we need to save a form
    Dim isNew As Boolean

    Dim oldname As String = ""
    Dim OldUUID As String
    Dim RName As String

#End Region

#Region "Properties"

    Public Property Changed1 As Boolean
        Get
            Return changed
        End Get
        Set(value As Boolean)
            changed = value
        End Set
    End Property

    Public Property Initted1 As Boolean
        Get
            Return Initted2
        End Get
        Set(value As Boolean)
            Initted2 = value
        End Set
    End Property

    Public Property IsNew1 As Boolean
        Get
            Return isNew
        End Get
        Set(value As Boolean)
            isNew = value
        End Set
    End Property

    Public Property Oldname1 As String
        Get
            Return oldname
        End Get
        Set(value As String)
            oldname = value
        End Set
    End Property

    Public Property RegionUUID As String
        Get
            Return _RegionUUID
        End Get
        Set(value As String)
            _RegionUUID = value
        End Set
    End Property

    Public Property RName1 As String
        Get
            Return RName
        End Get
        Set(value As String)
            RName = value
        End Set
    End Property

#End Region

#Region "ScreenSize"

    Private ReadOnly Handler As New EventHandler(AddressOf Resize_page)
    Private _screenPosition As ClassScreenpos

    Public Property Initted2 As Boolean
        Get
            Return Initted3
        End Get
        Set(value As Boolean)
            Initted3 = value
        End Set
    End Property

    Public Property Initted3 As Boolean

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
    End Sub

    Private Sub SetScreen()

        ScreenPosition = New ClassScreenpos(Me.Name)
        AddHandler ResizeEnd, Handler
        Dim xy As List(Of Integer) = ScreenPosition.GetXY()
        Me.Left = xy.Item(0)
        Me.Top = xy.Item(1)
    End Sub

#End Region

#Region "Start/Stop"

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        SetScreen()
        Text = Global.Outworldz.My.Resources.Regions_word

        RichTextBoxOptions.Text = Global.Outworldz.My.Resources.OptionsHelp
        RichTextBoxMap.Text = Global.Outworldz.My.Resources.MapsHelp
        RichTextBoxPhysics.Text = Global.Outworldz.My.Resources.PhysicsHelp
        RichTextBoxScripts.Text = Global.Outworldz.My.Resources.ScriptsHelp
        RichTextBoxPermissions.Text = Global.Outworldz.My.Resources.PermissionsHelp
        RichTextBoxPublicity.Text = Global.Outworldz.My.Resources.PublicityHelp
        RichTextBoxModules.Text = Global.Outworldz.My.Resources.ModulesHelp

        RegionsGroupbox.Text = Global.Outworldz.My.Resources.Regions_word
        MapGroupBox.Text = Global.Outworldz.My.Resources.Maps_word
        PhysicsGroupbox.Text = Global.Outworldz.My.Resources.Physics_word
        PermissionsGroupbox.Text = Global.Outworldz.My.Resources.Permissions_word
        ScriptsGroupbox.Text = Global.Outworldz.My.Resources.Scripts_word
        PublicityGroupBox.Text = Global.Outworldz.My.Resources.Publicity_Word
        ModulesGroupBox.Text = Global.Outworldz.My.Resources.Modules_word

        GodLevel.Text = Global.Outworldz.My.Resources.Allow_Or_Disallow_Gods_word
        BirdsCheckBox.Text = Global.Outworldz.My.Resources.Bird_Module_word
        ClampPrimLabel.Text = Global.Outworldz.My.Resources.Clamp_Prim_Size_word
        ConciergeCheckBox.Text = Global.Outworldz.My.Resources.Announce_visitors
        DeleteButton.Text = Global.Outworldz.My.Resources.Delete_word
        DeregisterButton.Text = Global.Outworldz.My.Resources.Deregister_word
        DisableGBCheckBox.Text = Global.Outworldz.My.Resources.Disable_Gloebits_word
        DisallowForeigners.Text = Global.Outworldz.My.Resources.Disable_Foreigners_word
        DisallowResidents.Text = Global.Outworldz.My.Resources.Disable_Residents
        EnabledCheckBox.Text = Global.Outworldz.My.Resources.Enabled_word
        FrameRateLabel.Text = Global.Outworldz.My.Resources.FrameRate

        'boxes
        PhysicsGroupbox.Text = Global.Outworldz.My.Resources.Physics_word
        GroupBox2.Text = Global.Outworldz.My.Resources.Sim_Size_word
        PublicityGroupBox.Text = My.Resources.Publicity_Word
        PermissionsGroupbox.Text = Global.Outworldz.My.Resources.Permissions_word
        ModulesGroupBox.Text = Global.Outworldz.My.Resources.Modules_word
        ScriptsGroupbox.Text = Global.Outworldz.My.Resources.Script_Engine_word  '

        XLabel.Text = Global.Outworldz.My.Resources.X
        YLabel.Text = Global.Outworldz.My.Resources.Y

        Gods_Use_Default.Text = Global.Outworldz.My.Resources.Use_Default_word
        GodEstate.Text = Global.Outworldz.My.Resources.Region_Owner_Is_God_word
        GodManager.Text = Global.Outworldz.My.Resources.EstateManagerIsGod_word

        LandingSpotLabel.Text = Global.Outworldz.My.Resources.DefaultLandingSpot

        MapBest.Text = Global.Outworldz.My.Resources.Best_Prims
        MapBetter.Text = Global.Outworldz.My.Resources.Better_Prims
        MapGroupBox.Text = Global.Outworldz.My.Resources.Maps_word
        MapGood.Text = Global.Outworldz.My.Resources.Good_Warp3D_word
        MapNone.Text = Global.Outworldz.My.Resources.None
        Maps_Use_Default.Text = Global.Outworldz.My.Resources.Use_Default_word
        MapSimple.Text = Global.Outworldz.My.Resources.Simple_but_Fast_word

        MaxMAvatarsLabel.Text = Global.Outworldz.My.Resources.Max_Avatars
        MaxNPrimsLabel.Text = Global.Outworldz.My.Resources.Max_NumPrims

        MenuStrip2.Text = Global.Outworldz.My.Resources._0
        NonPhysPrimLabel.Text = Global.Outworldz.My.Resources.Nonphysical_Prim
        NoPublish.Text = Global.Outworldz.My.Resources.No_Publish_Items

        Physics_Default.Text = Global.Outworldz.My.Resources.Use_Default_word
        Physics_Separate.Text = Global.Outworldz.My.Resources.BP
        Physics_ubODE.Text = Global.Outworldz.My.Resources.UBODE_words
        Physics_Hybrid.Text = Global.Outworldz.My.Resources.UbitHybrid
        PhysPrimLabel.Text = Global.Outworldz.My.Resources.Physical_Prim

        Publish.Text = Global.Outworldz.My.Resources.Publish_Items
        PublishDefault.Text = Global.Outworldz.My.Resources.Use_Default_word
        SearchLabel.Text = Global.Outworldz.My.Resources.Search_word

        SaveButton.Text = Global.Outworldz.My.Resources.Save_word
        ScriptDefaultButton.Text = Global.Outworldz.My.Resources.Use_Default_word
        ScriptOffButton.Text = My.Resources.Off
        ScriptRateLabel.Text = Global.Outworldz.My.Resources.Script_Timer_Rate
        SkipAutoCheckBox.Text = Global.Outworldz.My.Resources.Skip_Autobackup_word
        SmartStartCheckBox.Text = Global.Outworldz.My.Resources.Smart_Start_word
        TidesCheckbox.Text = Global.Outworldz.My.Resources.Tide_Enable

        ToolTip1.SetToolTip(GodLevel, Global.Outworldz.My.Resources.AllowGodsTooltip)
        ToolTip1.SetToolTip(BirdsCheckBox, Global.Outworldz.My.Resources.GBoids)
        ToolTip1.SetToolTip(ClampPrimLabel, Global.Outworldz.My.Resources.ClampSize)
        ToolTip1.SetToolTip(ClampPrimSize, Global.Outworldz.My.Resources.ClampSize)
        ToolTip1.SetToolTip(CoordX, Global.Outworldz.My.Resources.CoordX)
        ToolTip1.SetToolTip(CoordY, Global.Outworldz.My.Resources.CoordY)
        ToolTip1.SetToolTip(DisableGBCheckBox, Global.Outworldz.My.Resources.Disable_Gloebits_text)
        ToolTip1.SetToolTip(DisallowForeigners, Global.Outworldz.My.Resources.No_HG)
        ToolTip1.SetToolTip(DisallowResidents, Global.Outworldz.My.Resources.Only_Owners)
        ToolTip1.SetToolTip(FrameRateLabel, Global.Outworldz.My.Resources.FRText)
        ToolTip1.SetToolTip(FrametimeBox, Global.Outworldz.My.Resources.FrameTime)
        ToolTip1.SetToolTip(PhysicsGroupbox, Global.Outworldz.My.Resources.Sim_Rate)
        ToolTip1.SetToolTip(GodManager, Global.Outworldz.My.Resources.EMGod)
        ToolTip1.SetToolTip(LandingSpotTextBox, Global.Outworldz.My.Resources.LandingSpotTooltip)
        ToolTip1.SetToolTip(MaxAgents, Global.Outworldz.My.Resources.Max_Agents)
        ToolTip1.SetToolTip(MaxMAvatarsLabel, Global.Outworldz.My.Resources.Max_Agents)
        ToolTip1.SetToolTip(MaxNPrimsLabel, Global.Outworldz.My.Resources.Viewer_Stops_Counting)
        ToolTip1.SetToolTip(MaxPrims, Global.Outworldz.My.Resources.Not_Normal)
        ToolTip1.SetToolTip(NonphysicalPrimMax, Global.Outworldz.My.Resources.Normal_Prim)
        ToolTip1.SetToolTip(NonPhysPrimLabel, Global.Outworldz.My.Resources.Max_NonPhys)
        ToolTip1.SetToolTip(PhysicalPrimMax, Global.Outworldz.My.Resources.Max_Phys)
        ToolTip1.SetToolTip(PhysPrimLabel, Global.Outworldz.My.Resources.Max_Phys)
        ToolTip1.SetToolTip(GodEstate, Global.Outworldz.My.Resources.Region_Owner_Is_God_word)
        ToolTip1.SetToolTip(RegionName, Global.Outworldz.My.Resources.Region_Name)
        ToolTip1.SetToolTip(ScriptRateLabel, Global.Outworldz.My.Resources.Script_Timer_Text)
        ToolTip1.SetToolTip(ScriptRateLabel, Global.Outworldz.My.Resources.STComment)
        ToolTip1.SetToolTip(ScriptTimerTextBox, Global.Outworldz.My.Resources.STComment)
        ToolTip1.SetToolTip(SkipAutoCheckBox, Global.Outworldz.My.Resources.WillNotSave)
        ToolTip1.SetToolTip(SmartStartCheckBox, Global.Outworldz.My.Resources.GTide)
        ToolTip1.SetToolTip(TidesCheckbox, Global.Outworldz.My.Resources.GTide)
        ToolTip1.SetToolTip(TPCheckBox1, Global.Outworldz.My.Resources.Teleport_Tooltip)

        TPCheckBox1.Text = Global.Outworldz.My.Resources.Teleporter_Enable_word
        UUID.Name = Global.Outworldz.My.Resources.UUID

        RegionName.Select()
        'Scripting
        XEngineButton.Text = Global.Outworldz.My.Resources.XEngine_word
        YEngineButton.Text = Global.Outworldz.My.Resources.YEngine_word

    End Sub

    Public Sub Init(Name As String)

        Me.Focus()

        If Name Is Nothing Then Return
        Name = Name.Trim() ' remove spaces

        ' NEW REGION
        If Name.Length = 0 Then
            IsNew1 = True
            Gods_Use_Default.Checked = True
            RegionName.Text = Global.Outworldz.My.Resources.Name_of_Region_Word
            CoordX.Text = (LargestX() + 8).ToString(Globalization.CultureInfo.InvariantCulture)
            CoordY.Text = (LargestY() + 0).ToString(Globalization.CultureInfo.InvariantCulture)
            EnabledCheckBox.Checked = True
            RadioButton1.Checked = True
            SmartStartCheckBox.Checked = False
            NonphysicalPrimMax.Text = 1024.ToString(Globalization.CultureInfo.InvariantCulture)
            PhysicalPrimMax.Text = 64.ToString(Globalization.CultureInfo.InvariantCulture)
            ClampPrimSize.Checked = False
            ConciergeCheckBox.Checked = False
            MaxPrims.Text = 45000.ToString(Globalization.CultureInfo.InvariantCulture)
            MaxAgents.Text = 100.ToString(Globalization.CultureInfo.InvariantCulture)
            RegionUUID = CreateRegionStruct("")
            UUID.Text = RegionUUID
            Gods_Use_Default.Checked = True

            Core1Button.Checked = True
            Core2Button.Checked = True
            Core3Button.Checked = True
            Core4Button.Checked = True
            Core5Button.Checked = True
            Core6Button.Checked = True
            Core7Button.Checked = True
            Core8Button.Checked = True
            Core9Button.Checked = True
            Core10Button.Checked = True
            Core11Button.Checked = True
            Core12Button.Checked = True
            Core13Button.Checked = True
            Core14Button.Checked = True
            Core15Button.Checked = True
            Core16Button.Checked = True

            Normal.Checked = True
            Changed1 = True
        Else
            ' OLD REGION EDITED all this is required to be filled in!
            IsNew1 = False

            RegionUUID = FindRegionByName(Name)
            APIKey.Text = OpensimWorldAPIKey(RegionUUID)
            Oldname1 = Region_Name(RegionUUID) ' backup in case of rename
            EnabledCheckBox.Checked = RegionEnabled(RegionUUID)
            Me.Text = Name & " " & Global.Outworldz.My.Resources.Region_word ' on screen
            RegionName.Text = Name
            UUID.Text = RegionUUID
            NonphysicalPrimMax.Text = CStr(NonPhysical_PrimMax(RegionUUID))
            PhysicalPrimMax.Text = CStr(Physical_PrimMax(RegionUUID))
            ClampPrimSize.Checked = Clamp_PrimSize(RegionUUID)
            MaxPrims.Text = Max_Prims(RegionUUID)
            MaxAgents.Text = Max_Agents(RegionUUID)
            RegionPort.Text = CStr(Region_Port(RegionUUID))

            If Priority(RegionUUID).Length = 0 Then
                Normal.Checked = True
            Else
                RealTime.Checked = Priority(RegionUUID) = "RealTime"
                High.Checked = Priority(RegionUUID) = "High"
                AboveNormal.Checked = Priority(RegionUUID) = "AboveNormal"
                Normal.Checked = Priority(RegionUUID) = "Normal"
                BelowNormal.Checked = Priority(RegionUUID) = "BelowNormal"
            End If

            Core1Button.Checked = CBool(Cores(RegionUUID) And &H1)
            Core2Button.Checked = CBool(Cores(RegionUUID) And &H2)
            Core3Button.Checked = CBool(Cores(RegionUUID) And &H4)
            Core4Button.Checked = CBool(Cores(RegionUUID) And &H8)
            Core5Button.Checked = CBool(Cores(RegionUUID) And &H10)
            Core6Button.Checked = CBool(Cores(RegionUUID) And &H20)
            Core7Button.Checked = CBool(Cores(RegionUUID) And &H40)
            Core8Button.Checked = CBool(Cores(RegionUUID) And &H80)
            Core9Button.Checked = CBool(Cores(RegionUUID) And &H100)
            Core10Button.Checked = CBool(Cores(RegionUUID) And &H200)
            Core11Button.Checked = CBool(Cores(RegionUUID) And &H400)
            Core12Button.Checked = CBool(Cores(RegionUUID) And &H800)
            Core13Button.Checked = CBool(Cores(RegionUUID) And &H1000)
            Core14Button.Checked = CBool(Cores(RegionUUID) And &H2000)
            Core15Button.Checked = CBool(Cores(RegionUUID) And &H4000)
            Core16Button.Checked = CBool(Cores(RegionUUID) And &H8000)

            Dim C = Environment.ProcessorCount
            If C >= 1 Then Core1Button.Visible = True Else Core1Button.Visible = False
            If C >= 2 Then Core2Button.Visible = True Else Core2Button.Visible = False
            If C >= 3 Then Core3Button.Visible = True Else Core3Button.Visible = False
            If C >= 4 Then Core4Button.Visible = True Else Core4Button.Visible = False
            If C >= 5 Then Core5Button.Visible = True Else Core5Button.Visible = False
            If C >= 6 Then Core6Button.Visible = True Else Core6Button.Visible = False
            If C >= 7 Then Core7Button.Visible = True Else Core7Button.Visible = False
            If C >= 8 Then Core8Button.Visible = True Else Core8Button.Visible = False
            If C >= 9 Then Core9Button.Visible = True Else Core9Button.Visible = False
            If C >= 10 Then Core10Button.Visible = True Else Core10Button.Visible = False
            If C >= 11 Then Core11Button.Visible = True Else Core11Button.Visible = False
            If C >= 12 Then Core12Button.Visible = True Else Core12Button.Visible = False
            If C >= 13 Then Core13Button.Visible = True Else Core13Button.Visible = False
            If C >= 14 Then Core14Button.Visible = True Else Core14Button.Visible = False
            If C >= 15 Then Core15Button.Visible = True Else Core15Button.Visible = False
            If C >= 16 Then Core16Button.Visible = True Else Core16Button.Visible = False

            If Cores(RegionUUID) = 0 Then
                Core1Button.Checked = True
                Core2Button.Checked = True
                Core3Button.Checked = True
                Core4Button.Checked = True
                Core5Button.Checked = True
                Core6Button.Checked = True
                Core7Button.Checked = True
                Core8Button.Checked = True
                Core9Button.Checked = True
                Core10Button.Checked = True
                Core11Button.Checked = True
                Core12Button.Checked = True
                Core13Button.Checked = True
                Core14Button.Checked = True
                Core15Button.Checked = True
                Core16Button.Checked = True
            End If

            ' Size buttons can be zero
            If SizeY(RegionUUID) = 0 Or SizeX(RegionUUID) = 0 Then
                RadioButton1.Checked = True
                BoxSize = 256
            ElseIf SizeY(RegionUUID) = 256 Then
                RadioButton1.Checked = True
                BoxSize = 256 * 1
            ElseIf SizeY(RegionUUID) = 256 * 2 Then
                RadioButton2.Checked = True
                BoxSize = 256 * 2
            ElseIf SizeY(RegionUUID) = 256 * 3 Then
                RadioButton3.Checked = True
                BoxSize = 256 * 3
            ElseIf SizeY(RegionUUID) = 256 * 4 Then
                RadioButton4.Checked = True
                BoxSize = 256 * 4
            ElseIf SizeY(RegionUUID) = 256 * 5 Then
                RadioButton5.Checked = True
                BoxSize = 256 * 5
            ElseIf SizeY(RegionUUID) = 256 * 6 Then
                RadioButton6.Checked = True
                BoxSize = 256 * 6
            ElseIf SizeY(RegionUUID) = 256 * 7 Then
                RadioButton7.Checked = True
                BoxSize = 256 * 7
            ElseIf SizeY(RegionUUID) = 256 * 8 Then
                RadioButton8.Checked = True
                BoxSize = 256 * 8
            ElseIf SizeY(RegionUUID) = 256 * 9 Then
                RadioButton9.Checked = True
                BoxSize = 256 * 9
            ElseIf SizeY(RegionUUID) = 256 * 10 Then
                RadioButton10.Checked = True
                BoxSize = 256 * 10
            ElseIf SizeY(RegionUUID) = 256 * 11 Then
                RadioButton11.Checked = True
                BoxSize = 256 * 11
            ElseIf SizeY(RegionUUID) = 256 * 12 Then
                RadioButton12.Checked = True
                BoxSize = 256 * 12
            ElseIf SizeY(RegionUUID) = 256 * 13 Then
                RadioButton13.Checked = True
                BoxSize = 256 * 14
            ElseIf SizeY(RegionUUID) = 256 * 14 Then
                RadioButton14.Checked = True
                BoxSize = 256 * 14
            ElseIf SizeY(RegionUUID) = 256 * 15 Then
                RadioButton15.Checked = True
                BoxSize = 256 * 15
            ElseIf SizeY(RegionUUID) = 256 * 16 Then
                RadioButton16.Checked = True
                BoxSize = 256 * 16
            Else
                RadioButton1.Checked = True
            End If

            ' global coordinates
            If Coord_X(RegionUUID) <> 0 Then
                CoordX.Text = Coord_X(RegionUUID).ToString(Globalization.CultureInfo.InvariantCulture)
            End If

            If Coord_Y(RegionUUID) <> 0 Then
                CoordY.Text = Coord_Y(RegionUUID).ToString(Globalization.CultureInfo.InvariantCulture)
            End If

            APIKey.Text = OpensimWorldAPIKey(RegionUUID)

        End If

        LandingSpotTextBox.Text = LandingSpot(RegionUUID)

        OldUUID = UUID.Text

        If PropOpensimIsRunning Then
            UUID.ReadOnly = True
        Else
            UUID.ReadOnly = False
        End If
        ' The following are all options.
        If Disallow_Residents(RegionUUID) = "True" Then
            DisallowResidents.Checked = True
        End If

        If Disallow_Foreigners(RegionUUID) = "True" Then
            DisallowForeigners.Checked = True
        End If

        ScriptTimerTextBox.Text = MinTimerInterval(RegionUUID).ToString(Globalization.CultureInfo.InvariantCulture)
        FrametimeBox.Text = FrameTime(RegionUUID).ToString(Globalization.CultureInfo.InvariantCulture)

        If SkipAutobackup(RegionUUID) = "True" Then
            SkipAutoCheckBox.Checked = True
        End If
        If Smart_Start(RegionUUID) Then
            SmartStartCheckBox.Checked = True
        End If

        Select Case DisableGloebits(RegionUUID)
            Case ""
                DisableGBCheckBox.Checked = False
            Case "False"
                DisableGBCheckBox.Checked = False
            Case "True"
                DisableGBCheckBox.Checked = True
        End Select

        RichTextBoxMap.Text = My.Resources.MapsHelp
        RichTextBoxOptions.Text = My.Resources.OptionsHelp
        RichTextBoxPhysics.Text = My.Resources.PhysicsHelp
        RichTextBoxPublicity.Text = My.Resources.PublicityHelp
        RichTextBoxPermissions.Text = My.Resources.PermissionsHelp
        RichTextBoxScripts.Text = My.Resources.ScriptsHelp
        RichTextBoxModules.Text = My.Resources.ModulesHelp

        RName1 = Name

        ''''''''''''''''''''''''''''' DREAMGRID REGION LOAD '''''''''''''''''

        If MapType(RegionUUID).Length = 0 Then
            Maps_Use_Default.Checked = True
            DefaultMap()
        ElseIf MapType(RegionUUID) = "Simple" Then
            MapSimple.Checked = True
            MapPicture.Image = Global.Outworldz.My.Resources.Simple
        ElseIf MapType(RegionUUID) = "Good" Then
            MapGood.Checked = True
            MapPicture.Image = Global.Outworldz.My.Resources.Good
        ElseIf MapType(RegionUUID) = "Better" Then
            MapBetter.Checked = True
            MapPicture.Image = Global.Outworldz.My.Resources.Better
        ElseIf MapType(RegionUUID) = "Best" Then
            MapBest.Checked = True
            MapPicture.Image = Global.Outworldz.My.Resources.Best
        End If

        Select Case RegionPhysics(RegionUUID)
            Case "" : Physics_Default.Checked = True
            Case "-1" : Physics_Default.Checked = True
            Case "0" : Physics_Default.Checked = True
            Case "1" : Physics_Default.Checked = True   ' ODE is gone
            Case "2" : Physics_Bullet.Checked = True
            Case "3" : Physics_Separate.Checked = True
            Case "4" : Physics_ubODE.Checked = True
            Case "5" : Physics_Hybrid.Checked = True
            Case Else : Physics_Default.Checked = True
        End Select

        If GodDefault(RegionUUID) = "True" Then
            GodLevel.Checked = False
            GodEstate.Checked = False
            GodManager.Checked = False
            Gods_Use_Default.Checked = True
        Else
            Select Case AllowGods(RegionUUID)
                Case ""
                    GodLevel.Checked = False
                Case "False"
                    GodLevel.Checked = False
                    Gods_Use_Default.Checked = False
                Case "True"
                    GodLevel.Checked = True
                    Gods_Use_Default.Checked = False
            End Select

            Select Case RegionGod(RegionUUID)
                Case ""
                    GodEstate.Checked = False
                Case "False"
                    GodEstate.Checked = False
                    Gods_Use_Default.Checked = False
                Case "True"
                    GodEstate.Checked = True
                    Gods_Use_Default.Checked = False
            End Select

            Select Case ManagerGod(RegionUUID)
                Case ""
                    GodManager.Checked = False
                Case "False"
                    GodManager.Checked = False
                    Gods_Use_Default.Checked = False
                Case "True"
                    GodManager.Checked = True
                    Gods_Use_Default.Checked = False
            End Select

            ' if none selected, turn default on. This updates old code to new GodDefault global

            If AllowGods(RegionUUID).Length = 0 And
                  RegionGod(RegionUUID).Length = 0 And
                 ManagerGod(RegionUUID).Length = 0 Then
                Gods_Use_Default.Checked = True
            End If
        End If

        Select Case RegionSnapShot(RegionUUID)
            Case ""
                PublishDefault.Checked = True
                NoPublish.Checked = False
                Publish.Checked = False
            Case "False"
                PublishDefault.Checked = False
                NoPublish.Checked = True
                Publish.Checked = False
            Case "True"
                PublishDefault.Checked = False
                NoPublish.Checked = False
                Publish.Checked = True
        End Select

        Select Case Birds(RegionUUID)
            Case ""
                BirdsCheckBox.Checked = False
            Case "False"
                BirdsCheckBox.Checked = False
            Case "True"
                BirdsCheckBox.Checked = True
        End Select

        Select Case Tides(RegionUUID)
            Case ""
                TidesCheckbox.Checked = False
            Case "False"
                TidesCheckbox.Checked = False
            Case "True"
                TidesCheckbox.Checked = True
        End Select

        TPCheckBox1.Checked = Teleport_Sign(RegionUUID)

        Select Case Disallow_Foreigners(RegionUUID)
            Case ""
                DisallowForeigners.Checked = False
            Case "False"
                DisallowForeigners.Checked = False
            Case "True"
                DisallowForeigners.Checked = True
        End Select

        Select Case Disallow_Residents(RegionUUID)
            Case ""
                DisallowResidents.Checked = False
            Case "False"
                DisallowResidents.Checked = False
            Case "True"
                DisallowResidents.Checked = True
        End Select

        Select Case ScriptEngine(RegionUUID)
            Case ""
                ScriptDefaultButton.Checked = True
            Case "Off"
                ScriptOffButton.Checked = True
            Case "XEngine"
                XEngineButton.Checked = True
            Case "YEngine"
                YEngineButton.Checked = True
        End Select

        Select Case Concierge(RegionUUID)
            Case ""
                ConciergeCheckBox.Checked = False
            Case "True"
                ConciergeCheckBox.Checked = True
            Case "False"
                ConciergeCheckBox.Checked = False
        End Select

        Try
            Me.Show() ' time to show the results
            Me.Activate()

            Initted1 = True
            HelpOnce("Region")
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

    End Sub

    Private Sub DefaultMap()

        If Settings.MapType = "None" Then
            MapPicture.Image = Global.Outworldz.My.Resources.blankbox
        ElseIf Settings.MapType = "Simple" Then
            MapPicture.Image = Global.Outworldz.My.Resources.Simple
        ElseIf Settings.MapType = "Good" Then
            MapPicture.Image = Global.Outworldz.My.Resources.Good
        ElseIf Settings.MapType = "Better" Then
            MapPicture.Image = Global.Outworldz.My.Resources.Better
        ElseIf Settings.MapType = "Best" Then
            MapPicture.Image = Global.Outworldz.My.Resources.Best
        Else
            MapPicture.Image = Global.Outworldz.My.Resources.blankbox
        End If

    End Sub

    Private Sub FormRegion_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing

        If Changed1 Then

            Dim v = MsgBox(My.Resources.Save_changes_word, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Save_changes_word)
            If v = vbYes Then
                Dim message = RegionValidate()
                If Len(message) > 0 Then
                    v = MsgBox(message + vbCrLf + Global.Outworldz.My.Resources.Discard_Exit, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Info_word)
                    If v = vbYes Then
                        Me.Hide()
                        Return
                    End If
                End If

                WriteRegion(RegionUUID)
                PropChangedRegionSettings = True
                'GetAllRegions(False)
                Firewall.SetFirewall()

                PropUpdateView() = True
                Changed1 = False
                Close()

            End If

        End If

    End Sub

#End Region

#Region "Events"

    Private Sub BirdsCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles BirdsCheckBox.CheckedChanged

        If BirdsCheckBox.Checked Then
            Log(My.Resources.Info_word, "Region " + Name + " has birds enabled")
        End If
        If Initted1 Then Changed1 = True

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles SaveButton.Click

        Dim message = RegionValidate()
        If Len(message) > 0 Then
            Dim v = MsgBox(message + vbCrLf + Global.Outworldz.My.Resources.Discard_Exit, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Info_word)
            If v = vbYes Then
                Changed1 = False
            End If
        Else
            DeregisterRegionUUID(RegionUUID)
            WriteRegion(RegionUUID)
            PropChangedRegionSettings = True
            'GetAllRegions(False)
            Firewall.SetFirewall()

            PropUpdateView() = True
            Changed1 = False
        End If
        Close()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles DeregisterButton.Click

        Dim response As MsgBoxResult = MsgBox(My.Resources.Another_Region, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground)
        If response = vbYes Then

            StartMySQL()

            Dim RegionUUID As String = FindRegionByName(RegionName.Text)
            If RegionUUID.Length > 0 Then

                If IsRegionReady(GroupPort(RegionUUID)) Then
                    ShutDown(RegionUUID, SIMSTATUSENUM.ShuttingDownForGood)
                End If

                Dim loopctr = 120 ' wait 2 minutes
                While IsRegionReady(GroupPort(RegionUUID)) And loopctr > 0
                    Sleep(1000)
                    loopctr -= 1
                End While

                If loopctr > 0 Then
                    DeregisterRegionUUID(RegionUUID)
                    TextPrint(My.Resources.Region_Removed)
                End If
            End If

        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles DisallowForeigners.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub CheckBox1_CheckedChanged_1(sender As Object, e As EventArgs) Handles DisallowResidents.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub ClampPrimSize_CheckedChanged(sender As Object, e As EventArgs) Handles ClampPrimSize.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub ConciergeCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles ConciergeCheckBox.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub CoordX_TextChanged(sender As Object, e As EventArgs) Handles CoordX.TextChanged

        If Not Initted2 Then Return
        Dim digitsOnly = New Regex("[^\d]")
        CoordX.Text = digitsOnly.Replace(CoordX.Text, "")
        Changed1 = True

    End Sub

    Private Sub Coordy_TextChanged(sender As Object, e As EventArgs) Handles CoordY.TextChanged

        Dim digitsOnly = New Regex("[^\d]")
        CoordY.Text = digitsOnly.Replace(CoordY.Text, "")
        If Initted1 And CoordY.Text.Length >= 0 Then
            Changed1 = True
        End If

    End Sub

    Private Sub DeleteButton_Click(sender As Object, e As EventArgs) Handles DeleteButton.Click

        If RegionStatus(RegionUUID) <> SIMSTATUSENUM.Stopped Then
            MsgBox(My.Resources.Regions_Are_Running, vbInformation Or vbMsgBoxSetForeground)
            Return
        End If

        Dim msg = MsgBox(My.Resources.Are_you_Sure_Delete_Region, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Info_word)
        If msg = vbYes Then
            DeleteAllRegionData(RegionUUID)
            PropChangedRegionSettings = True
            Changed1 = False
            PropUpdateView = True
        End If

        Me.Close()

    End Sub

    Private Sub EnabledCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles EnabledCheckBox.CheckedChanged
        If Initted1 Then Changed1 = True
        If Not EnabledCheckBox.Checked Then DeregisterRegionUUID(RegionUUID)
    End Sub

    Private Sub EnableMaxPrims_text(sender As Object, e As EventArgs) Handles MaxPrims.TextChanged

        Dim digitsOnly = New Regex("[^\d]")
        MaxPrims.Text = digitsOnly.Replace(MaxPrims.Text, "")
        If Initted1 Then Changed1 = True

    End Sub

    Private Sub RLost(sender As Object, e As EventArgs) Handles RegionName.LostFocus

        RegionName.Text = RegionName.Text.Trim() ' remove spaces

    End Sub

    Private Sub ScriptDefaultButton_CheckedChanged(sender As Object, e As EventArgs) Handles ScriptDefaultButton.CheckedChanged

        If Initted1 Then Changed1 = True

    End Sub

    Private Sub ScriptTimerTextBox_focusChanged(sender As Object, e As EventArgs) Handles ScriptTimerTextBox.LostFocus

        Try
            Dim value = Convert.ToDouble(ScriptTimerTextBox.Text, Globalization.CultureInfo.InvariantCulture)
            If value < 0.01 Then ScriptTimerTextBox.Text = ""
        Catch ex As Exception
            ScriptTimerTextBox.Text = ""
        End Try

        If Initted1 Then Changed1 = True

    End Sub

    Private Sub ScriptTimerTextBox_TextChanged(sender As Object, e As EventArgs) Handles ScriptTimerTextBox.TextChanged

        Dim digitsOnly = New Regex("[^\d\.]")
        ScriptTimerTextBox.Text = digitsOnly.Replace(ScriptTimerTextBox.Text, "")

        If Initted1 Then Changed1 = True

    End Sub

    Private Sub SkipAutoCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles SkipAutoCheckBox.CheckedChanged

        If Initted1 Then Changed1 = True

    End Sub

    Private Sub SmartStartCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles SmartStartCheckBox.CheckedChanged

        If Initted1 Then Changed1 = True
        If SmartStartCheckBox.Checked And Settings.WelcomeRegion = RegionName.Text Then

            MsgBox(My.Resources.Default_Not_SS)
            SmartStartCheckBox.Checked = False

        End If
    End Sub

    Private Sub StopHGCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles DisableGBCheckBox.CheckedChanged

        If Initted1 Then Changed1 = True

    End Sub

    Private Sub TextBox1_FocusChanged(sender As Object, e As EventArgs) Handles FrametimeBox.LostFocus

        Try
            Dim value = Convert.ToDouble(FrametimeBox.Text, Globalization.CultureInfo.InvariantCulture)
            If value < 0.01 Then FrametimeBox.Text = ""
        Catch ex As Exception
            FrametimeBox.Text = ""
        End Try

        If Initted1 Then Changed1 = True

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles FrametimeBox.TextChanged

        Dim digitsOnly = New Regex("[^\d\.]")
        FrametimeBox.Text = digitsOnly.Replace(FrametimeBox.Text, "")
        If Initted1 Then Changed1 = True

    End Sub

    Private Sub TidesCheckbox_CheckedChanged(sender As Object, e As EventArgs) Handles TidesCheckbox.CheckedChanged

        If TidesCheckbox.Checked Then
            Log(My.Resources.Info_word, "Region " + Name + " has tides enabled")
        End If
        If Initted1 Then Changed1 = True

    End Sub

    Private Sub TPCheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles TPCheckBox1.CheckedChanged

        If TPCheckBox1.Checked Then
            Log(My.Resources.Info_word, $"Region {Name} has Teleport Board enabled")
        End If
        If Initted1 Then Changed1 = True

    End Sub

    Private Sub UUID_LostFocus(sender As Object, e As EventArgs) Handles UUID.LostFocus

        If UUID.Text <> OldUUID And Initted1 Then
            Dim resp = MsgBox(My.Resources.Change_UUID, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Info_word)
            If resp = vbYes Then
                Dim result As Guid
                If Guid.TryParse(UUID.Text, result) Then
                    OldUUID = UUID.Text
                    Changed1 = True
                Else
                    Dim ok = MsgBox(My.Resources.NotValidUUID, MsgBoxStyle.OkCancel Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Info_word)
                    If ok = vbOK Then
                        UUID.Text = System.Guid.NewGuid.ToString
                        Changed1 = True
                    Else
                        UUID.Text = OldUUID
                        Changed1 = False
                    End If
                End If
            Else
                UUID.Text = OldUUID
                Changed1 = False
            End If
        End If

    End Sub

    Private Sub UUID_TextChanged(sender As Object, e As EventArgs) Handles UUID.TextChanged

        If Initted1 Then Changed1 = True

    End Sub

#End Region

#Region "Subs"

    ReadOnly WriteLock As New Object

    Public Shared Function FileNameIsOK(ByVal FileName As String) As Boolean
        ' check for invalid chars in file name for INI file
        If FileName Is Nothing Then Return False
        Dim value As Boolean = False
        Try
            value = Not FileName.Intersect(Path.GetInvalidFileNameChars()).Any()
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

        Return value

    End Function

    Shared Function RegionChosen(regionName As String) As String

        Dim result As DialogResult
        Dim chosen As String

        Using Chooseform As New FormChooser ' form for choosing a set of regions
            ' Show testDialog as a modal dialog and determine if DialogResult = OK.
            Chooseform.FillGrid("Group")  ' populate the grid with either Group or RegionName
            result = Chooseform.ShowDialog()
            If result = DialogResult.Cancel Then
                Return ""
            End If

            Try
                ' Read the chosen sim name
                chosen = Chooseform.DataGridView.CurrentCell.Value.ToString()
                If chosen = "! Add New Name" Then
                    chosen = InputBox(My.Resources.Enter_Dos_Name, "", regionName)
                End If
            Catch ex As Exception
                BreakPoint.Dump(ex)
                chosen = ""
            End Try

        End Using
        Return chosen

    End Function

    Private Function RegionValidate() As String

        Dim Message As String

        If Len(RegionName.Text) = 0 Then
            Message = Global.Outworldz.My.Resources.Region_name_must_not_be_blank_word
            Return Message
        End If

        ' UUID
        Dim result As Guid
        If Not Guid.TryParse(UUID.Text, result) Then
            Message = Global.Outworldz.My.Resources.Region_UUID_Is_invalid_word & " " & UUID.Text
            Return Message
        End If

        ' global coordinates
        If Convert.ToInt32("0" & CoordX.Text, Globalization.CultureInfo.InvariantCulture) < 0 Then
            Message = Global.Outworldz.My.Resources.Region_Coordinate_X_cannot_be_less_than_0_word
            Return Message
        ElseIf Convert.ToInt32("0" & CoordX.Text, Globalization.CultureInfo.InvariantCulture) > 65536 Then
            Message = Global.Outworldz.My.Resources.Region_Coordinate_X_is_too_large
            Return Message
        End If

        If Convert.ToInt32("0" & CoordY.Text, Globalization.CultureInfo.InvariantCulture) < 32 Then
            Message = Global.Outworldz.My.Resources.Region_Coordinate_Y_cannot_be_less_than_32
            Return Message
        ElseIf Convert.ToInt32("0" & CoordY.Text, Globalization.CultureInfo.InvariantCulture) > 65536 Then
            Message = Global.Outworldz.My.Resources.Region_Coordinate_Y_Is_too_large
            Return Message
        End If

        Dim aresult As Guid
        If Not Guid.TryParse(UUID.Text, aresult) Then
            Message = Global.Outworldz.My.Resources.UUID0
            Return Message
        End If

        Try
            If (NonphysicalPrimMax.Text.Length = 0) Or (CType(NonphysicalPrimMax.Text, Integer) <= 0) Then
                Message = Global.Outworldz.My.Resources.NVNonPhysPrim
                Return Message
            End If
        Catch ex As Exception
            BreakPoint.Dump(ex)
            Message = Global.Outworldz.My.Resources.NVNonPhysPrim
            Return Message
        End Try

        Try
            If (PhysicalPrimMax.Text.Length = 0) Or (CType(PhysicalPrimMax.Text, Integer) <= 0) Then
                Message = Global.Outworldz.My.Resources.NVPhysPrim
                Return Message
            End If
        Catch ex As Exception
            BreakPoint.Dump(ex)
            Message = Global.Outworldz.My.Resources.NVPhysPrim
            Return Message
        End Try

        Try
            If (MaxPrims.Text.Length = 0) Or (CType(MaxPrims.Text, Integer) <= 0) Then
                Message = Global.Outworldz.My.Resources.NVMaxPrim
                Return Message
            End If
        Catch ex As Exception
            BreakPoint.Dump(ex)
            Message = Global.Outworldz.My.Resources.NVMaxPrim
            Return Message
        End Try
        Try
            If (MaxAgents.Text.Length = 0) Or (CType(MaxAgents.Text, Integer) <= 0) Then
                Message = Global.Outworldz.My.Resources.NVMaxAgents
                Return Message
            End If
        Catch ex As Exception
            BreakPoint.Dump(ex)
            Message = Global.Outworldz.My.Resources.NVMaxAgents
            Return Message
        End Try

        Return ""

    End Function

    ''' <returns>false if it fails</returns>
    Private Function WriteRegion(RegionUUID As String) As Boolean

        Dim Abort As Boolean
        Try

            ' save the Region File, choose an existing DOS box to put it in, or make a new one
            ' rename is possible
            If Oldname1 <> RegionName.Text And Not IsNew1 Then
                Try
                    StartMySQL()
                    MysqlInterface.DeregisterRegionUUID(RegionUUID)
                    My.Computer.FileSystem.RenameFile(RegionIniFilePath(RegionUUID), RegionName.Text + ".ini")
                Catch ex As Exception
                    BreakPoint.Dump(ex)
                    TextPrint(My.Resources.Aborted_word)
                    Return False
                End Try

                ' rename it
                RegionIniFilePath(RegionUUID) = RegionIniFolderPath(RegionUUID) + "/" + RegionName.Text + ".ini"
            End If

            ' might be a new region, so give them a choice

            If IsNew1 Then
                Dim NewGroup As String

                NewGroup = RegionChosen(RegionName.Text)
                If NewGroup.Length = 0 Then
                    TextPrint(My.Resources.Aborted_word)
                    Return False
                End If

                If Not Directory.Exists(RegionIniFilePath(RegionUUID)) Or RegionIniFilePath(RegionUUID).Length = 0 Then
                    MakeFolder(Settings.OpensimBinPath & "Regions\" + NewGroup + "\Region")
                End If

                RegionIniFilePath(RegionUUID) = Settings.OpensimBinPath & "Regions\" + NewGroup + "\Region\" + RegionName.Text + ".ini"
                RegionIniFolderPath(RegionUUID) = System.IO.Path.GetDirectoryName(RegionIniFilePath(RegionUUID))
                Group_Name(RegionUUID) = NewGroup

                Dim theEnd As Integer = RegionIniFolderPath(RegionUUID).LastIndexOf("\", StringComparison.OrdinalIgnoreCase)
                OpensimIniPath(RegionUUID) = RegionIniFolderPath(RegionUUID).Substring(0, theEnd + 1)

            End If

            ' save the changes to the memory structure, then to disk

            'CPU Affinity
            Dim Ncores = 0
            If Core1Button.Checked Then Ncores += &H1
            If Core2Button.Checked Then Ncores += &H2
            If Core3Button.Checked Then Ncores += &H4
            If Core4Button.Checked Then Ncores += &H8
            If Core5Button.Checked Then Ncores += &H10
            If Core6Button.Checked Then Ncores += &H20
            If Core7Button.Checked Then Ncores += &H40
            If Core8Button.Checked Then Ncores += &H80
            If Core9Button.Checked Then Ncores += &H100
            If Core10Button.Checked Then Ncores += &H200
            If Core10Button.Checked Then Ncores += &H400
            If Core12Button.Checked Then Ncores += &H800
            If Core13Button.Checked Then Ncores += &H1000
            If Core14Button.Checked Then Ncores += &H2000
            If Core15Button.Checked Then Ncores += &H4000
            If Core16Button.Checked Then Ncores += &H8000

            If Ncores = 0 Then Ncores = Environment.ProcessorCount
            Cores(RegionUUID) = Ncores

            If RealTime.Checked Then
                Priority(RegionUUID) = "RealTime"
            ElseIf High.Checked Then
                Priority(RegionUUID) = "High"
            ElseIf AboveNormal.Checked Then
                Priority(RegionUUID) = "AboveNormal"
            ElseIf Normal.Checked Then
                Priority(RegionUUID) = "Normal"
            ElseIf BelowNormal.Checked Then
                Priority(RegionUUID) = "BelowNormal"
            Else
                Priority(RegionUUID) = "Normal"
            End If

            Coord_X(RegionUUID) = CInt("0" & CoordX.Text)
            Coord_Y(RegionUUID) = CInt("0" & CoordY.Text)
            Region_Name(RegionUUID) = RegionName.Text

            SizeX(RegionUUID) = BoxSize
            SizeY(RegionUUID) = BoxSize
            RegionEnabled(RegionUUID) = EnabledCheckBox.Checked
            NonPhysical_PrimMax(RegionUUID) = NonphysicalPrimMax.Text
            Physical_PrimMax(RegionUUID) = PhysicalPrimMax.Text
            Clamp_PrimSize(RegionUUID) = ClampPrimSize.Checked

            Concierge(RegionUUID) = CStr(ConciergeCheckBox.Checked)

            Max_Agents(RegionUUID) = MaxAgents.Text
            Max_Prims(RegionUUID) = MaxPrims.Text
            MinTimerInterval(RegionUUID) = ScriptTimerTextBox.Text
            FrameTime(RegionUUID) = FrametimeBox.Text

            Dim Snapshot As String = ""
            If PublishDefault.Checked Then
                Snapshot = ""
            ElseIf NoPublish.Checked Then
                Snapshot = "False"
            ElseIf Publish.Checked Then
                Snapshot = "True"
            End If

            RegionSnapShot(RegionUUID) = Snapshot

            Dim Map As String = ""
            If MapNone.Checked Then
                Map = ""
            ElseIf MapSimple.Checked Then
                Map = "Simple"
            ElseIf MapGood.Checked Then
                Map = "Good"
            ElseIf MapBetter.Checked Then
                Map = "Better"
            ElseIf MapBest.Checked Then
                Map = "Best"
            End If

            MapType(RegionUUID) = Map

            'Case "" : Physics_Default.Checked = True
            'Case "-1" : Physics_Default.Checked = True
            'Case "0" : Physics_Default.Checked = True
            'Case "1" : Physics_ODE.Checked = True
            'Case "2" : Physics_Bullet.Checked = True
            'Case "3" : Physics_Separate.Checked = True
            'Case "4" : Physics_ubODE.Checked = True
            'Case "5" : Physics_Hybrid.Checked = True
            'Case Else : Physics_Default.Checked = True

            Dim Phys As String
            If Physics_Default.Checked Then
                Phys = ""
            ElseIf Physics_Bullet.Checked Then
                Phys = "2"
            ElseIf Physics_Separate.Checked Then
                Phys = "3"
            ElseIf Physics_ubODE.Checked Then
                Phys = "4"
            ElseIf Physics_Hybrid.Checked Then
                Phys = "5"
            Else
                Phys = "2"
            End If

            RegionPhysics(RegionUUID) = Phys

            If Gods_Use_Default.Checked Then
                GodDefault(RegionUUID) = "True"
                AllowGods(RegionUUID) = ""
                RegionGod(RegionUUID) = ""
                ManagerGod(RegionUUID) = ""
            Else
                GodDefault(RegionUUID) = "False"
                AllowGods(RegionUUID) = CStr(GodLevel.Checked)
                RegionGod(RegionUUID) = CStr(GodEstate.Checked)
                ManagerGod(RegionUUID) = CStr(GodManager.Checked)
            End If

            If DisallowForeigners.Checked Then
                Disallow_Foreigners(RegionUUID) = "True"
            Else
                Disallow_Foreigners(RegionUUID) = ""
            End If

            If DisallowResidents.Checked Then
                Disallow_Residents(RegionUUID) = "True"
            Else
                Disallow_Residents(RegionUUID) = ""
            End If

            If SkipAutoCheckBox.Checked Then
                SkipAutobackup(RegionUUID) = "True"
            Else
                SkipAutobackup(RegionUUID) = ""
            End If

            If BirdsCheckBox.Checked Then
                Birds(RegionUUID) = "True"
            Else
                Birds(RegionUUID) = ""
            End If

            If TidesCheckbox.Checked Then
                Tides(RegionUUID) = "True"
            Else
                Tides(RegionUUID) = ""
            End If

            If TPCheckBox1.Checked Then
                Teleport_Sign(RegionUUID) = True
            Else
                Teleport_Sign(RegionUUID) = False
            End If

            If DisableGBCheckBox.Checked Then
                DisableGloebits(RegionUUID) = "True"
            Else
                DisableGloebits(RegionUUID) = ""
            End If

            If NoPublish.Checked Then
                GDPR(RegionUUID) = "False"
            Else
                GDPR(RegionUUID) = ""
            End If

            If Publish.Checked Then
                GDPR(RegionUUID) = "True"
            Else
                GDPR(RegionUUID) = ""
            End If

            If SmartStartCheckBox.Checked Then
                Smart_Start(RegionUUID) = True
            Else
                Smart_Start(RegionUUID) = False
            End If

            ScriptEngine(RegionUUID) = "" ' default is blank
            If ScriptOffButton.Checked = True Then
                ScriptEngine(RegionUUID) = "Off"
            ElseIf XEngineButton.Checked = True Then
                ScriptEngine(RegionUUID) = "XEngine"
            ElseIf YEngineButton.Checked = True Then
                ScriptEngine(RegionUUID) = "YEngine"
            End If

            ' Get Next Port
            If GroupPort(RegionUUID) = 0 Or Region_Port(RegionUUID) = 0 Then
                GroupPort(RegionUUID) = GetPort(RegionUUID)
                Region_Port(RegionUUID) = GroupPort(RegionUUID)
            End If


            FileStuff.CopyFileFast(RegionIniFilePath(RegionUUID), RegionIniFilePath(RegionUUID) & ".bak")

            SyncLock WriteLock
                Try

                    Dim Region = "; * Regions configuration file" &
                                "; * This Is Your World. See Common Settings->[Region Settings]." & vbCrLf &
                                "; Automatically changed by Dreamworld" & vbCrLf &
                                "[" & RegionName.Text & "]" & vbCrLf &
                                "RegionUUID=" & UUID.Text & vbCrLf &
                                "Location=" & CoordX.Text & "," & CoordY.Text & vbCrLf &
                                "InternalAddress = 0.0.0.0" & vbCrLf &
                                "InternalPort=" & Region_Port(RegionUUID) & vbCrLf &
                                "GroupPort=" & GroupPort(RegionUUID) & vbCrLf &
                                "AllowAlternatePorts = False" & vbCrLf &
                                "ExternalHostName=" & Settings.ExternalHostName & vbCrLf &
                                "SizeX=" & BoxSize & vbCrLf &
                                "SizeY=" & BoxSize & vbCrLf &
                                "Enabled=" & CStr(EnabledCheckBox.Checked) & vbCrLf &
                                "NonPhysicalPrimMax=" & NonphysicalPrimMax.Text & vbCrLf &
                                "PhysicalPrimMax=" & PhysicalPrimMax.Text & vbCrLf &
                                "ClampPrimSize=" & CStr(ClampPrimSize.Checked) & vbCrLf &
                                "Concierge =  " & CStr(ConciergeCheckBox.Checked) & vbCrLf &
                                "MaxAgents=" & MaxAgents.Text & vbCrLf &
                                "MaxPrims=" & MaxPrims.Text & vbCrLf &
                                "RegionType = Estate" & vbCrLf & vbCrLf &
                                "DefaultLanding = " & LandingSpotTextBox.Text & vbCrLf & vbCrLf &
                                ";# Extended region properties from Dreamgrid" & vbCrLf &
                                "MinTimerInterval=" & ScriptTimerTextBox.Text & vbCrLf &
                                "FrameTime=" & FrametimeBox.Text & vbCrLf &
                                "RegionSnapShot=" & Snapshot & vbCrLf &
                                "MapType=" & Map & vbCrLf &
                                "Physics=" & Phys & vbCrLf &
                                "GodDefault=" & GodDefault(RegionUUID) & vbCrLf &
                                "AllowGods=" & AllowGods(RegionUUID) & vbCrLf &
                                "RegionGod=" & RegionGod(RegionUUID) & vbCrLf &
                                "ManagerGod=" & ManagerGod(RegionUUID) & vbCrLf &
                                "Birds=" & Birds(RegionUUID) & vbCrLf &
                                "Tides=" & Tides(RegionUUID) & vbCrLf &
                                "Teleport=" & CStr(Teleport_Sign(RegionUUID)) & vbCrLf &
                                "DisableGloebits=" & DisableGloebits(RegionUUID) & vbCrLf &
                                "DisallowForeigners=" & Disallow_Foreigners(RegionUUID) & vbCrLf &
                                "DisallowResidents=" & Disallow_Residents(RegionUUID) & vbCrLf &
                                "SkipAutoBackup=" & SkipAutobackup(RegionUUID) & vbCrLf &
                                "ScriptEngine=" & ScriptEngine(RegionUUID) & vbCrLf &
                                "Publicity=" & GDPR(RegionUUID) & vbCrLf &
                                "OpensimWorldAPIKey=" & OpensimWorldAPIKey(RegionUUID) & vbCrLf &
                                "Priority=" & Priority(RegionUUID) & vbCrLf &
                                "Cores=" & CStr(Cores(RegionUUID)) & vbCrLf &
                                "SmartStart=" & CStr(Smart_Start(RegionUUID)) & vbCrLf

                    Try
                        Using outputFile As New StreamWriter(RegionIniFilePath(RegionUUID), False)
                            outputFile.Write(Region)
                        End Using
                    Catch ex As Exception
                        BreakPoint.Dump(ex)
                        MsgBox(My.Resources.Cannot_save_region_word + ex.Message, MsgBoxStyle.Critical Or MsgBoxStyle.MsgBoxSetForeground)
                        Abort = True
                    End Try
                Catch ex As Exception
                    BreakPoint.Dump(ex)
                End Try

            End SyncLock
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

        PropChangedRegionSettings = True
        PropUpdateView = True

        Oldname1 = RegionName.Text
        If Abort Then Return False
        Return True

    End Function

#End Region

#Region "Scripting"

    Private Sub AboveNormal_CheckedChanged(sender As Object, e As EventArgs) Handles AboveNormal.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub AllowGods_CheckedChanged(sender As Object, e As EventArgs) Handles GodLevel.CheckedChanged

        If Initted1 Then Changed1 = True
        If GodLevel.Checked Then Gods_Use_Default.Checked = False

    End Sub

    Private Sub APIKey_TextChanged(sender As Object, e As EventArgs) Handles APIKey.TextChanged

        If Initted1 Then Changed1 = True
        OpensimWorldAPIKey(RegionUUID) = APIKey.Text
        SendToOpensimWorld(RegionUUID, 0)

    End Sub

    Private Sub BasicsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BasicsToolStripMenuItem.Click
        HelpManual("Region")
    End Sub

    Private Sub BelowNormal_CheckedChanged(sender As Object, e As EventArgs) Handles BelowNormal.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub Core10Button_CheckedChanged(sender As Object, e As EventArgs) Handles Core10Button.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub Core11Button_CheckedChanged(sender As Object, e As EventArgs) Handles Core11Button.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub Core12Button_CheckedChanged(sender As Object, e As EventArgs) Handles Core12Button.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub Core13Button_CheckedChanged(sender As Object, e As EventArgs) Handles Core13Button.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub Core14Button_CheckedChanged(sender As Object, e As EventArgs) Handles Core14Button.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub Core15Button_CheckedChanged(sender As Object, e As EventArgs) Handles Core15Button.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub Core16Button_CheckedChanged(sender As Object, e As EventArgs) Handles Core16Button.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub Core1Button_CheckedChanged(sender As Object, e As EventArgs) Handles Core1Button.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub Core2Button_CheckedChanged(sender As Object, e As EventArgs) Handles Core2Button.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub Core3Button_CheckedChanged(sender As Object, e As EventArgs) Handles Core3Button.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub Core4Button_CheckedChanged(sender As Object, e As EventArgs) Handles Core4Button.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub Core5Button_CheckedChanged(sender As Object, e As EventArgs) Handles Core5Button.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub Core6Button_CheckedChanged(sender As Object, e As EventArgs) Handles Core6Button.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub Core7Button_CheckedChanged(sender As Object, e As EventArgs) Handles Core7Button.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub Core8Button_CheckedChanged(sender As Object, e As EventArgs) Handles Core8Button.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub Core9Button_CheckedChanged(sender As Object, e As EventArgs) Handles Core9Button.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub GodEstate_CheckedChanged(sender As Object, e As EventArgs) Handles GodEstate.CheckedChanged

        If Initted1 Then Changed1 = True
        If GodEstate.Checked Then Gods_Use_Default.Checked = False

    End Sub

    Private Sub GodManager_CheckedChanged(sender As Object, e As EventArgs) Handles GodManager.CheckedChanged

        If Initted1 Then Changed1 = True
        If GodManager.Checked Then Gods_Use_Default.Checked = False

    End Sub

    Private Sub Gods_Use_Default_CheckedChanged(sender As Object, e As EventArgs) Handles Gods_Use_Default.CheckedChanged

        If Gods_Use_Default.Checked Then
            GodLevel.Checked = False
            GodManager.Checked = False
            GodEstate.Checked = False
        End If

    End Sub

    Private Sub High_CheckedChanged(sender As Object, e As EventArgs) Handles High.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub HybridRadioButton_CheckedChanged(sender As Object, e As EventArgs) Handles Physics_Hybrid.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub LandingSpotTextBox_lostfocus(sender As Object, e As EventArgs) Handles LandingSpotTextBox.LostFocus

        If LandingSpotTextBox.Text.Length = 0 Then Return
        Dim Parser = New Regex("<\d*\.?\d*,\d*\.?\d*,\d*\.?\d*>")   ' floats <x, y, z>
        Dim result = Parser.Match(LandingSpotTextBox.Text)
        If Not result.Success Then
            MsgBox(My.Resources.NotValidVector, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Info_word)
            LandingSpotTextBox.BackColor = Color.Red
        Else
            LandingSpotTextBox.BackColor = Color.White
        End If

    End Sub

    Private Sub LandingSpotTextBox_TextChanged(sender As Object, e As EventArgs) Handles LandingSpotTextBox.TextChanged
        If LandingSpotTextBox.Text.Length = 0 Then Return
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim webAddress As String = "https://outworldz.com/search"
        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try
    End Sub

    Private Sub MapBest_CheckedChanged(sender As Object, e As EventArgs) Handles MapBest.CheckedChanged
        If Initted1 Then Changed1 = True
        MapPicture.Image = Global.Outworldz.My.Resources.Best
    End Sub

    Private Sub MapBetter_CheckedChanged(sender As Object, e As EventArgs) Handles MapBetter.CheckedChanged
        If Initted1 Then Changed1 = True
        MapPicture.Image = Global.Outworldz.My.Resources.Better
    End Sub

    Private Sub MapGood_CheckedChanged(sender As Object, e As EventArgs) Handles MapGood.CheckedChanged
        If Initted1 Then Changed1 = True
        MapPicture.Image = Global.Outworldz.My.Resources.Good
    End Sub

    Private Sub MapNone_CheckedChanged(sender As Object, e As EventArgs) Handles MapNone.CheckedChanged
        If Initted1 Then Changed1 = True
        MapPicture.Image = Global.Outworldz.My.Resources.blankbox
    End Sub

    Private Sub Maps_Use_Default_CheckedChanged(sender As Object, e As EventArgs) Handles Maps_Use_Default.CheckedChanged

        If Initted1 Then Changed1 = True
        DefaultMap()

    End Sub

    Private Sub MapSimple_CheckedChanged(sender As Object, e As EventArgs) Handles MapSimple.CheckedChanged
        If Initted1 Then Changed1 = True
        MapPicture.Image = Global.Outworldz.My.Resources.Simple
    End Sub

    Private Sub MapsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MapsToolStripMenuItem.Click
        HelpManual("Map Overrides")
    End Sub

    Private Sub ModulesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ModulesToolStripMenuItem.Click
        HelpManual("Module Overrides")
    End Sub

    Private Sub Normal_CheckedChanged(sender As Object, e As EventArgs) Handles Normal.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub Opensimworld_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles Opensimworld.LinkClicked
        Dim webAddress As String = "https://opensimworld.com"
        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try
    End Sub

    Private Sub OptionsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OptionsToolStripMenuItem.Click
        HelpManual("Options for Regions")
    End Sub

    Private Sub PermissionsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PermissionsToolStripMenuItem.Click
        HelpManual("Permission Overrides")
    End Sub

    Private Sub Physics_Bullet_CheckedChanged(sender As Object, e As EventArgs) Handles Physics_Bullet.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub Physics_Default_CheckedChanged(sender As Object, e As EventArgs) Handles Physics_Default.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub Physics_Separate_CheckedChanged(sender As Object, e As EventArgs) Handles Physics_Separate.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub Physics_ubODE_CheckedChanged(sender As Object, e As EventArgs) Handles Physics_ubODE.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub PhysicsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PhysicsToolStripMenuItem.Click
        HelpManual("Physics Overrides")
    End Sub

    Private Sub PublicityToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PublicityToolStripMenuItem.Click
        HelpManual("Publicity Overrides")
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        BoxSize = 256
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub RadioButton10_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton10.CheckedChanged
        BoxSize = 256 * 10
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub RadioButton11_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton11.CheckedChanged
        BoxSize = 256 * 11
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub RadioButton12_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton12.CheckedChanged
        BoxSize = 256 * 12
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub RadioButton13_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton13.CheckedChanged
        BoxSize = 256 * 13
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub RadioButton14_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton14.CheckedChanged
        BoxSize = 256 * 14
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub RadioButton15_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton15.CheckedChanged
        BoxSize = 256 * 15
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub RadioButton16_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton16.CheckedChanged
        BoxSize = 256 * 16
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        BoxSize = 256 * 2
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub RadioButton3_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton3.CheckedChanged
        BoxSize = 256 * 3
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub RadioButton4_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton4.CheckedChanged
        BoxSize = 256 * 4
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub RadioButton5_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton5.CheckedChanged
        BoxSize = 256 * 5
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub RadioButton6_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton6.CheckedChanged
        BoxSize = 256 * 6
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub RadioButton7_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton7.CheckedChanged
        BoxSize = 256 * 7
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub RadioButton8_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton8.CheckedChanged
        BoxSize = 256 * 8
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub RadioButton9_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton9.CheckedChanged
        BoxSize = 256 * 9
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub RealTime_CheckedChanged(sender As Object, e As EventArgs) Handles RealTime.CheckedChanged
        If Initted1 Then Changed1 = True
    End Sub

    Private Sub RegionName_TextChanged(sender As Object, e As EventArgs) Handles RegionName.Click

        RegionName.Text = RegionName.Text.Replace("Name of Region", "")

    End Sub

    Private Sub ScriptsOffButton_CheckedChanged(sender As Object, e As EventArgs) Handles ScriptOffButton.CheckedChanged

        If Initted1 Then Changed1 = True

    End Sub

    Private Sub ScriptsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ScriptsToolStripMenuItem.Click
        HelpManual("Script Overrides")
    End Sub

    Private Sub XEngineButton_CheckedChanged_1(sender As Object, e As EventArgs) Handles XEngineButton.CheckedChanged

        If Initted1 Then Changed1 = True

    End Sub

    Private Sub YEngineButton_CheckedChanged_1(sender As Object, e As EventArgs) Handles YEngineButton.CheckedChanged

        If Initted1 Then Changed1 = True

    End Sub

#End Region

End Class
