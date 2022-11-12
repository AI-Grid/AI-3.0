#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

' TODO New terrain commands
'   set terrain heights <corner> <min> <max> [<x>] [<y>] - Sets the terrain texture heights on corner #<corner> to <min>/<max>, if <x> Or <y> are specified, it will only set it on regions with a matching coordinate. Specify -1 in <x> Or <y> to wildcard that coordinate. Corner # SW = 0, NW = 1, SE = 2, NE = 3.
'   set terrain texture <number> <uuid> [<x>] [<y>] - Sets the terrain <number> to <uuid>, if <x> Or <y> are specified, it will only set it on regions with a matching coordinate. Specify -1 in <x> Or <y> to wild card that coordinate.

Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Xml

Public Class FormSmartStart

    Private ReadOnly _TerrainList As New List(Of Image)
    Private ReadOnly _TerrainName As New List(Of String)
    Private ReadOnly Handler As New EventHandler(AddressOf Resize_page)

    Private _abort As Boolean
    Private _Index As Integer
    Private _initialized As Boolean

    'The following detects  the location of the form in screen coordinates
    Private _screenPosition As ClassScreenpos

    Private _SelectedPlant As String

#Region "ScreenSize"

    Public Property Abort As Boolean
        Get
            Return _abort
        End Get
        Set(value As Boolean)
            _abort = value
        End Set
    End Property

    Public Property ScreenPosition As ClassScreenpos
        Get
            Return _screenPosition
        End Get
        Set(value As ClassScreenpos)
            _screenPosition = value
        End Set
    End Property

    Public Sub PutSetting(name As String, value As Boolean)

        All.Checked = False
        None.Checked = False

        Settings.SetMySetting(name, CStr(value))
        Settings.SaveSettings()

    End Sub

    Private Shared Sub ClrSetting(check As CheckBox)

        check.Checked = False

    End Sub

    Private Shared Function GetPic(Photoname As String) As Image

        Dim path = IO.Path.Combine(Settings.OpensimBinPath, "Trees")
        path = IO.Path.Combine(path, Photoname & ".jpg")

        Try
            Using fs = New System.IO.FileStream(path, FileMode.Open, FileAccess.Read)
                Dim I As Image = Image.FromStream(fs)
                Return I
            End Using
        Catch ex As Exception
        End Try

        Return My.Resources.NoImage

    End Function

    Private Shared Function GetSetting(tree As String) As Boolean
        Dim b As Boolean
        Select Case Settings.GetMySetting(tree)
            Case ""
                b = False
            Case "True"
                b = True
            Case "False"
                b = False
            Case Else
                b = False
        End Select
        Return b

    End Function

    Private Shared Sub SetSetting(check As CheckBox)

        check.Checked = True

    End Sub

    Private Sub All_CheckedChanged(sender As Object, e As EventArgs) Handles All.CheckedChanged

        If All.Checked Then
            Settings.AllPlants = True
            None.Checked = False

            SetSetting(BeachGrass1)
            SetSetting(Cypress1)
            SetSetting(Cypress2)
            SetSetting(Eelgrass)
            SetSetting(Dogwood)
            SetSetting(Eucalyptus)
            SetSetting(Fern)

            SetSetting(Kelp1)
            SetSetting(Kelp2)
            SetSetting(Oak)
            SetSetting(Palm1)
            SetSetting(Palm2)
            SetSetting(Pine1)
            SetSetting(Pine2)
            SetSetting(Plumeria)
            SetSetting(SeaSword)
            SetSetting(TropicalBush1)
            SetSetting(TropicalBush2)

            SetSetting(WinterAspen)
            SetSetting(WinterPine1)
            SetSetting(WinterPine2)

            Settings.SaveSettings()
        End If

    End Sub

    Private Sub ApplyButton_Click(sender As Object, e As EventArgs) Handles ApplyTerrainEffectButton.Click

        'AI or .r32
        Dim RegionName = ChooseRegion(False)
        Dim RegionUUID As String = FindRegionByName(RegionName)
        If RegionUUID.Length = 0 Then Return

        Dim obj As New TaskObject With {
                            .TaskName = TaskName.ApplyTerrainEffect
                        }
        RebootAndRunTask(RegionUUID, obj)

    End Sub

    Private Sub ApplyPlantButton_Click(sender As Object, e As EventArgs) Handles ApplyPlantButton.Click
        'plant apply
        Dim RegionName = ChooseRegion(False)
        Dim RegionUUID As String = FindRegionByName(RegionName)
        If RegionUUID.Length = 0 Then Return

        Dim obj As New TaskObject With {
                            .TaskName = TaskName.ApplyPlant
                        }
        RebootAndRunTask(RegionUUID, obj)

    End Sub

    Private Sub BakeButton_Click(sender As Object, e As EventArgs) Handles BakeButton.Click

        Dim name = ChooseRegion(False)
        Dim RegionUUID As String = FindRegionByName(name)
        If RegionUUID.Length = 0 Then Return

        Dim obj As New TaskObject With {
                            .TaskName = TaskName.BakeTerrain
                        }
        RebootAndRunTask(RegionUUID, obj)

    End Sub

    Private Sub BeachGrass_CheckedChanged(sender As Object, e As EventArgs) Handles BeachGrass1.CheckedChanged
        Dim thing As CheckBox = CType(sender, CheckBox)
        PutSetting(thing.Name, thing.Checked)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub BeachGrass_MouseHoverd(sender As Object, e As EventArgs) Handles BeachGrass1.MouseHover
        Dim thing As CheckBox = CType(sender, CheckBox)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Delete_TreeButton.Click

        Dim name = ChooseRegion(False)
        Dim RegionUUID As String = FindRegionByName(name)
        If RegionUUID.Length = 0 Then Return

        Dim obj As New TaskObject With {
                            .TaskName = TaskName.DeleteTree
                        }
        RebootAndRunTask(RegionUUID, obj)

    End Sub

    Private Sub Cypress1_CheckedChanged(sender As Object, e As EventArgs) Handles Cypress1.CheckedChanged
        Dim thing As CheckBox = CType(sender, CheckBox)
        PutSetting(thing.Name, thing.Checked)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub Cypress1_Hover(sender As Object, e As EventArgs) Handles Cypress1.MouseHover
        Dim thing As CheckBox = CType(sender, CheckBox)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub Cypress2_CheckedChanged(sender As Object, e As EventArgs) Handles Cypress2.CheckedChanged
        Dim thing As CheckBox = CType(sender, CheckBox)
        PutSetting(thing.Name, thing.Checked)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub Cypress2_Hover(sender As Object, e As EventArgs) Handles Cypress2.MouseHover
        Dim thing As CheckBox = CType(sender, CheckBox)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub DelayRegionReady_TextChanged(sender As Object, e As EventArgs) Handles DelayRegionReady.TextChanged

        Dim digitsOnly = New Regex("[^\d]")
        DelayRegionReady.Text = digitsOnly.Replace(DelayRegionReady.Text, "")
        Settings.TeleportSleepTime = CInt("0" & DelayRegionReady.Text)
        If Settings.TeleportSleepTime < 0 Then Settings.TeleportSleepTime = 0
        TextPrint(My.Resources.Min_time)
        Settings.SaveSettings()

    End Sub

    Private Sub DeletApply_CheckedChanged(sender As Object, e As EventArgs) Handles DeletApply.CheckedChanged

        Settings.DeleteTreesFirst = DeletApply.Checked
        Settings.SaveSettings()

    End Sub

    Private Sub DogwoodCheckbox_CheckedChanged_1(sender As Object, e As EventArgs) Handles Dogwood.MouseHover
        Dim thing As CheckBox = CType(sender, CheckBox)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub DogwoodCheckboxCheckedChanged_1(sender As Object, e As EventArgs) Handles Dogwood.CheckedChanged
        Dim thing As CheckBox = CType(sender, CheckBox)
        PutSetting(thing.Name, thing.Checked)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub Eelgrass_CheckedChanged(sender As Object, e As EventArgs) Handles Eelgrass.CheckedChanged
        Dim thing As CheckBox = CType(sender, CheckBox)
        PutSetting(thing.Name, thing.Checked)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub Eelgrass_Hover(sender As Object, e As EventArgs) Handles Eelgrass.MouseHover
        Dim thing As CheckBox = CType(sender, CheckBox)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub EndsizeX_TextChanged(sender As Object, e As EventArgs) Handles EndsizeX.TextChanged
        If Not _initialized Then Return
        Dim digitsOnly = New Regex("[^\d\.]")
        EndsizeX.Text = digitsOnly.Replace(EndsizeX.Text, "")
        If Convert.ToSingle("0" & EndsizeX.Text, Globalization.CultureInfo.InvariantCulture) > 255 Then EndsizeX.Text = CStr(255)
        MakeSetting()
    End Sub

    Private Sub EndsizeY_TextChanged(sender As Object, e As EventArgs) Handles EndsizeY.TextChanged
        If Not _initialized Then Return
        Dim digitsOnly = New Regex("[^\d\.]")
        EndsizeY.Text = digitsOnly.Replace(EndsizeY.Text, "")
        If Convert.ToSingle("0" & EndsizeY.Text, Globalization.CultureInfo.InvariantCulture) > 255 Then EndsizeY.Text = CStr(255)
        MakeSetting()
    End Sub

    Private Sub EndsizeZ_TextChanged(sender As Object, e As EventArgs) Handles EndsizeZ.TextChanged
        If Not _initialized Then Return
        Dim digitsOnly = New Regex("[^\d\.]")
        EndsizeZ.Text = digitsOnly.Replace(EndsizeZ.Text, "")
        If Convert.ToSingle("0" & EndsizeZ.Text, Globalization.CultureInfo.InvariantCulture) > 255 Then EndsizeZ.Text = CStr(255)
        MakeSetting()
    End Sub

    Private Sub Eucalyptus_CheckedChanged(sender As Object, e As EventArgs) Handles Eucalyptus.CheckedChanged
        Dim thing As CheckBox = CType(sender, CheckBox)
        PutSetting(thing.Name, thing.Checked)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub Eucalyptus_Hover(sender As Object, e As EventArgs) Handles Eucalyptus.MouseHover
        Dim thing As CheckBox = CType(sender, CheckBox)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub Fern_CheckedChanged(sender As Object, e As EventArgs) Handles Fern.CheckedChanged
        Dim thing As CheckBox = CType(sender, CheckBox)
        PutSetting(thing.Name, thing.Checked)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub Fern_Hover(sender As Object, e As EventArgs) Handles Fern.MouseHover
        Dim thing As CheckBox = CType(sender, CheckBox)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub Flat_TextChanged(sender As Object, e As EventArgs) Handles FlatLandLevel.TextChanged
        Dim digitsOnly = New Regex("[^\d\.]")
        FlatLandLevel.Text = digitsOnly.Replace(FlatLandLevel.Text, "")
        If Convert.ToSingle("0" & FlatLandLevel.Text, Globalization.CultureInfo.InvariantCulture) > 100 Then FlatLandLevel.Text = CStr(100)
        Settings.FlatlandLevel = Convert.ToDouble("0" & FlatLandLevel.Text, Globalization.CultureInfo.InvariantCulture)
    End Sub

    Private Sub Grass0_CheckedChanged(sender As Object, e As EventArgs)
        Dim thing As CheckBox = CType(sender, CheckBox)
        PutSetting(thing.Name, thing.Checked)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub Grass0_Hover(sender As Object, e As EventArgs)
        Dim thing As CheckBox = CType(sender, CheckBox)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub Grass1_CheckedChanged(sender As Object, e As EventArgs)
        Dim thing As CheckBox = CType(sender, CheckBox)
        PutSetting(thing.Name, thing.Checked)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub Grass1_Hover(sender As Object, e As EventArgs)
        Dim thing As CheckBox = CType(sender, CheckBox)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub Grass2_CheckedChanged(sender As Object, e As EventArgs)
        Dim thing As CheckBox = CType(sender, CheckBox)
        PutSetting(thing.Name, thing.Checked)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub Grass2_Hover(sender As Object, e As EventArgs)
        Dim thing As CheckBox = CType(sender, CheckBox)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub Grass3_CheckedChanged(sender As Object, e As EventArgs)
        Dim thing As CheckBox = CType(sender, CheckBox)
        PutSetting(thing.Name, thing.Checked)
    End Sub

    Private Sub Grass3_Hover(sender As Object, e As EventArgs)
        Dim thing As CheckBox = CType(sender, CheckBox)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub Grass4_CheckedChanged(sender As Object, e As EventArgs)
        Dim thing As CheckBox = CType(sender, CheckBox)
        PutSetting(thing.Name, thing.Checked)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub Grass4_Hover(sender As Object, e As EventArgs)
        Dim thing As CheckBox = CType(sender, CheckBox)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub HelpLandcapingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpLandcapingToolStripMenuItem.Click
        HelpManual("Landscaping")
    End Sub

    Private Sub HelpPlantEditorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpPlantEditorToolStripMenuItem.Click
        HelpManual("Landscaping")
    End Sub

    Private Sub HelpSmartStartToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpSmartStartToolStripMenuItem.Click
        HelpManual("SmartStart")
    End Sub

    Private Sub HelpTerrainsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpTerrainsToolStripMenuItem.Click
        HelpManual("Terrain")
    End Sub

    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.Click
        HelpManual("SmartStart")
    End Sub

    Private Sub Kelp1_CheckedChanged(sender As Object, e As EventArgs) Handles Kelp1.CheckedChanged
        Dim thing As CheckBox = CType(sender, CheckBox)
        PutSetting(thing.Name, thing.Checked)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub Kelp1_Hover(sender As Object, e As EventArgs) Handles Kelp1.MouseHover
        Dim thing As CheckBox = CType(sender, CheckBox)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub Kelp2_CheckedChanged(sender As Object, e As EventArgs) Handles Kelp2.CheckedChanged
        Dim thing As CheckBox = CType(sender, CheckBox)
        PutSetting(thing.Name, thing.Checked)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub Kelp2_Hover(sender As Object, e As EventArgs) Handles Kelp2.MouseHover
        Dim thing As CheckBox = CType(sender, CheckBox)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub LandscapingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LandscapingToolStripMenuItem.Click
        HelpManual("Landscaping")
    End Sub

    Private Sub ListBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox2.SelectedIndexChanged

        If Not _initialized Then Return
        Settings.Skirtsize = CInt(ListBox2.SelectedItem.ToString)
        Select Case Settings.Skirtsize
            Case 0
                PictureBox4.Image = My.Resources._1X1
            Case 1
                PictureBox4.Image = My.Resources._3x3
            Case 2
                PictureBox4.Image = My.Resources._5x5
            Case 3
                PictureBox4.Image = My.Resources._7x7
        End Select
        Settings.SaveSettings()

    End Sub

    Private Sub Loading(sender As Object, e As EventArgs) Handles MyBase.Load

        _initialized = False
        SetScreen()

        AI.Text = My.Resources.Generated_word
        All.Text = My.Resources.All_word
        ApplyPlantButton.Text = My.Resources.Apply_word
        AutoFillEnable.Text = My.Resources.Enabled_word
        AviName.Text = Settings.SurroundOwner
        BakeButton.Text = My.Resources.Bakeword
        BeachGrass1.Text = My.Resources.BeachGrass
        Delete_TreeButton.Text = My.Resources.Delete_All
        Cypress1.Text = My.Resources.Cypress_1
        Cypress2.Text = My.Resources.Cypress_2
        DelayLabelRegionReady.Text = Global.Outworldz.My.Resources.DelayAfterRegionReady
        DelayLabelShutDown.Text = My.Resources.SSDelay
        DeletApply.Text = My.Resources.Delete_and_then_Apply
        Dogwood.Text = My.Resources.Dogwood
        Eelgrass.Text = My.Resources.Eelgrass
        EndlessLand.Text = My.Resources.AutoFill
        Eucalyptus.Text = My.Resources.Eucalyptus
        Fern.Text = My.Resources.Fern
        FillSizeLabel.Text = My.Resources.FillSizeLabel
        Flat.Text = My.Resources.Flat
        FlatLandLevel.Text = CStr(Settings.FlatlandLevel)
        GroupBox1.Text = My.Resources.Plants
        GroupBox2.Text = My.Resources.Terrain_word
        GroupBox4.Text = My.Resources.Terrain_word
        GroupBox5.Text = My.Resources.Options
        HelpLandcapingToolStripMenuItem.Text = My.Resources.HelpLandcaping
        HelpPlantEditorToolStripMenuItem.Text = My.Resources.HelpPlantEditor
        HelpSmartStartToolStripMenuItem.Text = My.Resources.HelpSmartStart
        HelpTerrainsToolStripMenuItem.Text = My.Resources.HelpTerrains
        HelpToolStripMenuItem.Text = My.Resources.Help_word
        Kelp1.Text = My.Resources.Kelp1
        Kelp2.Text = My.Resources.Kelp2
        Label10.Text = My.Resources.Taper
        Label2.Text = My.Resources.TreelineHigh
        Label3.Text = My.Resources.TreeLineLow
        Label4.Text = My.Resources.StartSizeXYZ
        Label5.Text = My.Resources.EndSizeXYZ
        ParkingRegion.Text = My.Resources.ParkingRegion1
        Label8.Text = My.Resources.Height
        Label9.Text = My.Resources.Smooth
        LandscapingToolStripMenuItem.Text = My.Resources.Landscaping

        ListBox2.SelectedIndex = Settings.Skirtsize

        LoadTerrain.Text = My.Resources.LoadTerrain
        Me.Text = Global.Outworldz.My.Resources.Smart_Start_word
        Noise.Text = My.Resources.Noise
        None.Text = My.Resources.None
        Oak.Text = My.Resources.Oak
        OptionRadioButton.Text = My.Resources.JustOptions
        OwnerLabel.Text = My.Resources.OwnerOfRegions
        Palm1.Text = My.Resources.Palm1
        ParkingSpot.Text = My.Resources.Parking_Spot
        Pine1.Text = My.Resources.Pine1
        Pine2.Text = My.Resources.Pine2
        Plumeria.Text = My.Resources.Plumeria
        QtyLabel.Text = My.Resources.Quantity_word
        RadioButton10.Text = My.Resources.BeachGrass1
        RadioButton11.Text = My.Resources.Pine1
        RadioButton12.Text = My.Resources.Pine2
        RadioButton13.Text = My.Resources.Palm1
        RadioButton14.Text = My.Resources.Pine2
        RadioButton15.Text = My.Resources.TropicalBush1
        RadioButton16.Text = My.Resources.TropicalBush2
        RadioButton17.Text = My.Resources.SeaSword
        RadioButton18.Text = My.Resources.Kelp1
        RadioButton19.Text = My.Resources.Kelp2
        RadioButton20.Text = My.Resources.Eucalyptus
        RadioButton21.Text = My.Resources.Fern
        RadioButton22.Text = My.Resources.Oak
        RadioButton23.Text = My.Resources.Plumeria
        RadioButton24.Text = My.Resources.Eelgrass
        RadioButton26.Text = My.Resources.Dogwood
        RadioButton5.Text = My.Resources.Cypress1
        RadioButton6.Text = My.Resources.Cypress2
        RadioButton7.Text = My.Resources.WinterAspen
        RadioButton8.Text = My.Resources.WinterPine1
        RadioButton9.Text = My.Resources.WinterPine2
        Radius.Text = My.Resources.Radius_Word
        Rand.Text = My.Resources.RandomTerrain
        RebuildTerrainsToolStripMenuItem.Text = My.Resources.RegenTerrains
        AutoFillEnable.Checked = Settings.AutoFill
        RegionMakingToolStripMenuItem.Text = My.Resources.RegionMaker
        RevertButton.Text = My.Resources.Revert
        SaveTerrain.Text = My.Resources.SaveTerrain
        SaveAllTerrain.Text = My.Resources.SaveTerrain
        SeaSword.Text = My.Resources.SeaSword
        Seconds.Name = My.Resources.Seconds
        Seconds.Text = CStr(Settings.SmartStartTimeout)
        SmartStartEnabled.Text = Global.Outworldz.My.Resources.Smart_Start_Enable_word
        SmartStartToolStripMenuItem.Text = My.Resources.Smart_Start_word
        Smooth.Text = My.Resources.Smooth
        SmoothTextBox.Text = CStr(Settings.LandStrength)
        TabPage1.Text = My.Resources.Smart_Start_word
        TabPage2.Text = My.Resources.Landscaping
        TabPage2.Text = My.Resources.Terrain_word
        TabPage3.Text = My.Resources.TreesAndPlants
        TabPage4.Text = My.Resources.EditorWord
        TabPage5.Text = My.Resources.EndlessLandAndSea
        TaperTextBox.Text = CStr(Settings.LandTaper)
        TempCheckBox.Text = My.Resources.Temporary_Regions
        TerrainApply.Text = My.Resources.Apply
        ToolTip1.SetToolTip(Seconds, Global.Outworldz.My.Resources.SecondsTips)
        TropicalBush1.Text = My.Resources.TropicalBush1
        TropicalBush2.Text = My.Resources.TropicalBush2
        ViewTerrainFolderToolStripMenuItem.Text = My.Resources.ViewTerrainFolder
        Water.Text = My.Resources.WaterWord
        WinterAspen.Text = My.Resources.WinterAspen
        WinterPine1.Text = My.Resources.WinterPine1
        WinterPine2.Text = My.Resources.WinterPine2

        If Debugger.IsAttached Then
            EndlessLand.Visible = True
        Else
            EndlessLand.Visible = True
            Settings.AutoFill = False
            Settings.TempRegion = False
        End If

        If Settings.BootOrSuspend Then
            SuspendButton.Checked = False
            ShutDownButton.Checked = True
        Else
            SuspendButton.Checked = True
            ShutDownButton.Checked = False
        End If

        If AviName.Text.Length = 0 Then
            AviName.BackColor = Color.Red
        End If

        Select Case Settings.Skirtsize
            Case 0
                PictureBox4.Image = My.Resources._1X1
            Case 1
                PictureBox4.Image = My.Resources._3x3
            Case 2
                PictureBox4.Image = My.Resources._5x5
            Case 3
                PictureBox4.Image = My.Resources._7x7
        End Select

        SmartStartEnabled.Checked = Settings.Smart_Start
        TempCheckBox.Checked = Settings.TempRegion
        DelayRegionReady.Text = CStr(Settings.TeleportSleepTime)

        SetScreen()

        Select Case Settings.TerrainType
            Case "Flat"
                Flat.Checked = True
            Case "Random"
                Rand.Checked = True
            Case "AI"
                AI.Checked = True
            Case "Water"
                Water.Checked = True
            Case Else
                OptionRadioButton.Checked = True
        End Select

        With AviName
            .AutoCompleteCustomSource = MysqlInterface.GetAvatarList()
            .AutoCompleteMode = AutoCompleteMode.Suggest
            .AutoCompleteSource = AutoCompleteSource.CustomSource
        End With

        BeachGrass1.Checked = GetSetting("BeachGrass1")
        Cypress1.Checked = GetSetting("Cypress1")
        Cypress2.Checked = GetSetting("Cypress2")
        Eelgrass.Checked = GetSetting("Eelgrass")
        Eucalyptus.Checked = GetSetting("Eucalyptus")
        Fern.Checked = GetSetting("Fern")
        Kelp1.Checked = GetSetting("Kelp1")
        Kelp2.Checked = GetSetting("Kelp2")
        Oak.Checked = GetSetting("Oak")
        Palm1.Checked = GetSetting("Palm1")
        Palm2.Checked = GetSetting("Palm2")
        Pine1.Checked = GetSetting("Pine1")
        Pine2.Checked = GetSetting("Pine2")
        Plumeria.Checked = GetSetting("Plumeria")
        SeaSword.Checked = GetSetting("SeaSword")
        TropicalBush1.Checked = GetSetting("TropicalBush1")
        TropicalBush2.Checked = GetSetting("TropicalBush2")
        WinterAspen.Checked = GetSetting("WinterAspen")
        WinterPine1.Checked = GetSetting("WinterPine1")
        WinterPine2.Checked = GetSetting("WinterPine2")

        All.Checked = Settings.AllPlants
        None.Checked = Settings.NoPlants

        LoadTerrainList()

        Dim n = 0
        Dim s As Boolean
        For Each RegionUUID In RegionUuids()
            If Smart_Start(RegionUUID) And Settings.Smart_Start Then Continue For
            Dim name = Region_Name(RegionUUID)
            ParkingSpot.Items.Add(name)
            If name = Settings.ParkingLot Then
                ParkingSpot.SelectedIndex = n
                s = True
            End If
            n += 1
        Next

        With AviName
            .AutoCompleteCustomSource = MysqlInterface.GetAvatarList()
            .AutoCompleteMode = AutoCompleteMode.Suggest
            .AutoCompleteSource = AutoCompleteSource.CustomSource
        End With

        SuspendButton.Visible = True

        HelpOnce("SmartStart")
        _initialized = True
    End Sub

    Private Sub LoadPlant(Name As String)

        _SelectedPlant = Name
        _initialized = False

        PictureBox2.Image = GetPic(Name)

        Dim Setup = New XmlReaderSettings With {
            .IgnoreWhitespace = True
        }

        Dim path = IO.Path.Combine(Settings.OpensimBinPath, "Trees\" & Name & ".xml")
        If File.Exists(path) Then

#Disable Warning CA3075 ' Insecure DTD processing in XML
            Using TextReader = New XmlTextReader(path)
#Enable Warning CA3075 ' Insecure DTD processing in XML
                ' Read until end of file
                While (TextReader.Read())
                    Dim nType As XmlNodeType = TextReader.NodeType
                    ' If node type us a declaration
                    If (nType = XmlNodeType.XmlDeclaration) Then
                        Console.WriteLine("Declaration:" + TextReader.Name.ToString())
                    End If
                    ' if node type Is a comment
                    If (nType = XmlNodeType.Comment) Then
                        Console.WriteLine("Comment:" + TextReader.Name.ToString())
                    End If
                    ' if node type us an attribute
                    If (nType = XmlNodeType.Attribute) Then
                        Console.WriteLine("Attribute:" + TextReader.Name.ToString())
                    End If
                    ' if node type Is an element
                    If (nType = XmlNodeType.Element) Then
                        Console.WriteLine("Element:" + TextReader.Name.ToString())
                        If TextReader.Name.ToString() = "m_tree_quantity" Then
                            Qty.Text = TextReader.ReadInnerXml
                        ElseIf TextReader.Name.ToString() = "m_treeline_low" Then
                            TreeLineLow.Text = TextReader.ReadInnerXml
                        ElseIf TextReader.Name.ToString() = "m_treeline_high" Then
                            TreeLineHight.Text = TextReader.ReadInnerXml
                            Dim v = TextReader.Value
                        ElseIf TextReader.Name.ToString() = "m_initial_scale" Then
                            Dim X = TextReader.ReadInnerXml
                            Dim pattern As Regex
                            pattern = New Regex("<X>(.*?)</X>")
                            Dim matchX As Match = pattern.Match(X)
                            pattern = New Regex("<Y>(.*?)</Y>")
                            Dim matchY As Match = pattern.Match(X)
                            pattern = New Regex("<Z>(.*?)</Z>")
                            Dim matchZ As Match = pattern.Match(X)
                            StartSizeX.Text = matchX.Groups(1).Value
                            StartSizeY.Text = matchY.Groups(1).Value
                            StartSizeZ.Text = matchZ.Groups(1).Value

                        ElseIf TextReader.Name.ToString() = "m_maximum_scale" Then
                            Dim X = TextReader.ReadInnerXml
                            Dim pattern As Regex
                            pattern = New Regex("<X>(.*?)</X>")
                            Dim matchX As Match = pattern.Match(X)
                            pattern = New Regex("<Y>(.*?)</Y>")
                            Dim matchY As Match = pattern.Match(X)
                            pattern = New Regex("<Z>(.*?)</Z>")
                            Dim matchZ As Match = pattern.Match(X)

                            EndsizeX.Text = matchX.Groups(1).Value
                            EndsizeY.Text = matchY.Groups(1).Value
                            EndsizeZ.Text = matchZ.Groups(1).Value

                        ElseIf TextReader.Name.ToString() = "m_range" Then
                            Dim X = TextReader.ReadInnerXml
                            Rad.Text = X

                            ' if node type Is an entity
                        ElseIf (nType = XmlNodeType.Entity) Then
                            Console.WriteLine("Entity:" + TextReader.Name.ToString())

                            ' if node type Is a Process Instruction
                        ElseIf (nType = XmlNodeType.Entity) Then
                            Console.WriteLine("Entity:" + TextReader.Name.ToString())

                            ' if node type a document
                        ElseIf (nType = XmlNodeType.DocumentType) Then
                            Console.WriteLine("Document:" + TextReader.Name.ToString())

                            ' if node type Is white space
                        ElseIf (nType = XmlNodeType.Whitespace) Then
                            Console.WriteLine("WhiteSpace:" + TextReader.Name.ToString())
                        End If
                    End If

                End While
            End Using

        End If
        _initialized = True

    End Sub

    Private Sub LoadTerrain_Click(sender As Object, e As EventArgs) Handles LoadTerrain.Click

        'load menu
        Dim RegionName = ChooseRegion(False)
        If RegionName.Length = 0 Then Return
        Dim RegionUUID As String = FindRegionByName(RegionName)

        Dim Obj = New TaskObject With {
                .TaskName = TaskName.TerrainLoad,
                .Command = ""
            }
        RebootAndRunTask(RegionUUID, Obj)

    End Sub

    Private Sub LoadTerrainList()

        Try
            Dim Terrainfolder = IO.Path.Combine(Settings.OpensimBinPath, "Terrains")
            Dim directory As New System.IO.DirectoryInfo(Terrainfolder)
            Dim File As System.IO.FileInfo() = directory.GetFiles()
            Dim File1 As System.IO.FileInfo

            For Each File1 In File
                If File1.Name.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) Then
                    Dim pic = Bitmap.FromFile(File1.FullName)
                    Dim newImage = New Bitmap(256, 256)
                    Dim gr = Graphics.FromImage(newImage)
                    gr.DrawImageUnscaled(pic, 0, 0)

                    _TerrainList.Add(newImage)
                    _TerrainName.Add(File1.FullName)
                    If PictureBox3.Image Is Nothing Then
                        PictureBox3.Image = newImage
                    End If

                End If
            Next
        Catch
        End Try

    End Sub

    Private Sub MakeSetting()

        Dim Size = 256
        Dim XMLName = _SelectedPlant
        If XMLName Is Nothing Then Return

        Dim quant = CInt("0" & Qty.Text)
        Dim Tlo = CInt("0" & Me.TreeLineLow.Text)
        Dim Thi = CInt("0" & TreeLineHight.Text)
        Dim StartX = CInt("0" & StartSizeX.Text)
        Dim StartY = CInt("0" & StartSizeY.Text)
        Dim StartZ = CInt("0" & StartSizeZ.Text)
        Dim EndX = CInt("0" & EndsizeX.Text)
        Dim EndY = CInt("0" & EndsizeY.Text)
        Dim EndZ = CInt("0" & EndsizeZ.Text)
        Dim RadiusTree = CInt("0" & Rad.Text)
        Dim average = (Thi + Tlo) / 2

        Dim Xml As String = $"<Copse>
  <m_name>{XMLName}</m_name>
  <m_frozen>false</m_frozen>
  <m_tree_type>{XMLName}</m_tree_type>
  <m_tree_quantity>{quant}</m_tree_quantity>
  <m_treeline_low>{Tlo}</m_treeline_low>
  <m_treeline_high>{Thi}</m_treeline_high>
  <m_seed_point>
    <X>{Size / 2}</X>
    <Y>{Size / 2}</Y>
    <Z>{average}</Z>
  </m_seed_point>
  <m_range>{RadiusTree * Size / 256}</m_range>
  <m_initial_scale>
    <X>{StartX}</X>
    <Y>{StartY}</Y>
    <Z>{StartZ}</Z>
  </m_initial_scale>
  <m_maximum_scale>
    <X>{EndX}</X>
    <Y>{EndY}</Y>
    <Z>{EndZ}</Z>
  </m_maximum_scale>
  <m_rate>
    <X>0.01</X>
    <Y>0.01</Y>
    <Z>0.01</Z>
  </m_rate>
</Copse>
"
        Dim output = IO.Path.Combine(Settings.OpensimBinPath, $"Trees/{XMLName}.xml")

        Try
            Using Writer As New StreamWriter(output, False)
                Writer.Write(Xml)
            End Using
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try
    End Sub

    Private Sub MinLandHeight_TextChanged(sender As Object, e As EventArgs) Handles TreeLineLow.TextChanged
        If Not _initialized Then Return
        Dim digitsOnly = New Regex("[^\d\.]")
        TreeLineLow.Text = digitsOnly.Replace(TreeLineLow.Text, "")
        If CInt("0" & TreeLineLow.Text) < 0 Then TreeLineLow.Text = CStr(0)
        MakeSetting()
    End Sub

    Private Sub Noise_CheckedChanged(sender As Object, e As EventArgs) Handles Noise.CheckedChanged

        Settings.LandNoise = Noise.Checked
        Settings.SaveSettings()

    End Sub

    Private Sub None_CheckedChanged(sender As Object, e As EventArgs) Handles None.CheckedChanged

        If None.Checked Then

            Settings.NoPlants = True
            All.Checked = False

            ClrSetting(BeachGrass1)
            ClrSetting(Cypress1)
            ClrSetting(Cypress2)
            ClrSetting(Dogwood)
            ClrSetting(Eelgrass)
            ClrSetting(Eucalyptus)
            ClrSetting(Fern)
            ClrSetting(Kelp1)
            ClrSetting(Kelp2)
            ClrSetting(Oak)
            ClrSetting(Palm1)
            ClrSetting(Palm2)
            ClrSetting(Pine1)
            ClrSetting(Pine2)
            ClrSetting(Plumeria)
            ClrSetting(SeaSword)
            ClrSetting(TropicalBush1)
            ClrSetting(TropicalBush2)

            ClrSetting(WinterAspen)
            ClrSetting(WinterPine1)
            ClrSetting(WinterPine2)

            Settings.SaveSettings()

        End If

    End Sub

    Private Sub Oak_CheckedChanged(sender As Object, e As EventArgs) Handles Oak.CheckedChanged
        Dim thing As CheckBox = CType(sender, CheckBox)
        PutSetting(thing.Name, thing.Checked)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub Oak_MouseHover(sender As Object, e As EventArgs) Handles Oak.MouseHover
        Dim thing As CheckBox = CType(sender, CheckBox)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub OptionRadioButton_CheckedChanged(sender As Object, e As EventArgs) Handles OptionRadioButton.CheckedChanged

        Settings.TerrainType = "Option"
        TerrainPic.Image = My.Resources.NoImage

    End Sub

    Private Sub Palm1_CheckedChanged(sender As Object, e As EventArgs) Handles Palm1.CheckedChanged
        Dim thing As CheckBox = CType(sender, CheckBox)
        PutSetting(thing.Name, thing.Checked)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub Palm1_MouseHover(sender As Object, e As EventArgs) Handles Palm1.MouseHover
        Dim thing As CheckBox = CType(sender, CheckBox)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub Palm2_CheckedChanged(sender As Object, e As EventArgs) Handles Palm2.CheckedChanged
        Dim thing As CheckBox = CType(sender, CheckBox)
        PutSetting(thing.Name, thing.Checked)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub Palm2_MouseHover(sender As Object, e As EventArgs) Handles Palm2.MouseHover
        Dim thing As CheckBox = CType(sender, CheckBox)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub ParkingSpot_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ParkingSpot.SelectedIndexChanged

        Try
            Settings.ParkingLot = ParkingSpot.SelectedItem.ToString
            TextPrint($"{My.Resources.arrivals} {ParkingSpot.SelectedItem}")
            Settings.SaveSettings()
        Catch

        End Try

    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles NextButton.Click

        _Index += 1
        If _Index >= _TerrainList.Count Then _Index = 0
        PictureBox3.Image = _TerrainList.Item(_Index)
        LabelName.Text = IO.Path.GetFileNameWithoutExtension(_TerrainName.Item(_Index))

    End Sub

    Private Sub PictureBox4_Click(sender As Object, e As EventArgs) Handles PrevButton.Click

        _Index -= 1
        If _Index < 0 Then _Index = _TerrainList.Count - 1
        PictureBox3.Image = _TerrainList.Item(_Index)
        LabelName.Text = IO.Path.GetFileNameWithoutExtension(_TerrainName.Item(_Index))

    End Sub

    Private Sub Pine1_CheckedChanged(sender As Object, e As EventArgs) Handles Pine1.CheckedChanged
        Dim thing As CheckBox = CType(sender, CheckBox)
        PutSetting(thing.Name, thing.Checked)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub Pine1_MouseHover(sender As Object, e As EventArgs) Handles Pine1.MouseHover
        Dim thing As CheckBox = CType(sender, CheckBox)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub Pine2_CheckedChanged(sender As Object, e As EventArgs) Handles Pine2.CheckedChanged
        Dim thing As CheckBox = CType(sender, CheckBox)
        PutSetting(thing.Name, thing.Checked)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub Pine2_MouseHover(sender As Object, e As EventArgs) Handles Pine2.MouseHover
        Dim thing As CheckBox = CType(sender, CheckBox)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub Plumeria_CheckedChanged(sender As Object, e As EventArgs) Handles Plumeria.CheckedChanged
        Dim thing As CheckBox = CType(sender, CheckBox)
        PutSetting(thing.Name, thing.Checked)
        PictureBox1.Image = GetPic(thing.Name)

    End Sub

    Private Sub Plumeria_MouseHover(sender As Object, e As EventArgs) Handles Plumeria.MouseHover
        Dim thing As CheckBox = CType(sender, CheckBox)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub Qty_TextChanged(sender As Object, e As EventArgs) Handles Qty.TextChanged
        If Not _initialized Then Return
        Dim digitsOnly = New Regex("[^\d]")
        Qty.Text = digitsOnly.Replace(Qty.Text, "")
        If CInt("0" & Qty.Text) > 500 Then Qty.Text = CStr(500)
        MakeSetting()
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles Flat.CheckedChanged
        Settings.TerrainType = "Flat"
        TerrainPic.Image = My.Resources.flatland
    End Sub

    Private Sub RadioButton1_CheckedChanged_1(sender As Object, e As EventArgs) Handles ShutDownButton.CheckedChanged

        If Not _initialized Then Return
        If Not ShutDownButton.Checked Then Return
        DelayRegionReady.Text = My.Resources._20
        Settings.BootOrSuspend = True
        Settings.SaveSettings()

    End Sub

    Private Sub RadioButton10_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton10.CheckedChanged
        LoadPlant(CStr(sender.AccessibleDescription))
    End Sub

    Private Sub RadioButton11_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton11.CheckedChanged
        LoadPlant(CStr(sender.AccessibleDescription))
    End Sub

    Private Sub RadioButton12_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton12.CheckedChanged
        LoadPlant(CStr(sender.AccessibleDescription))
    End Sub

    Private Sub RadioButton13_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton13.CheckedChanged
        LoadPlant(CStr(sender.AccessibleDescription))
    End Sub

    Private Sub RadioButton14_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton14.CheckedChanged
        LoadPlant(CStr(sender.AccessibleDescription))
    End Sub

    Private Sub RadioButton15_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton15.CheckedChanged
        LoadPlant(CStr(sender.AccessibleDescription))
    End Sub

    Private Sub RadioButton16_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton16.CheckedChanged
        LoadPlant(CStr(sender.AccessibleDescription))
    End Sub

    Private Sub RadioButton17_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton17.CheckedChanged
        LoadPlant(CStr(sender.AccessibleDescription))
    End Sub

    Private Sub RadioButton18_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton18.CheckedChanged
        LoadPlant(CStr(sender.AccessibleDescription))
    End Sub

    Private Sub RadioButton19_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton19.CheckedChanged
        LoadPlant(CStr(sender.AccessibleDescription))
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles Rand.CheckedChanged
        Settings.TerrainType = "Random"
        TerrainPic.Image = My.Resources.Random
    End Sub

    Private Sub RadioButton2_CheckedChanged_1(sender As Object, e As EventArgs) Handles SuspendButton.CheckedChanged

        If Not _initialized Then Return
        If Not SuspendButton.Checked Then Return
        DelayRegionReady.Text = My.Resources._0
        Settings.BootOrSuspend = False
        Settings.SaveSettings()

    End Sub

    Private Sub RadioButton20_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton20.CheckedChanged
        LoadPlant(CStr(sender.AccessibleDescription))
    End Sub

    Private Sub RadioButton21_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton21.CheckedChanged
        LoadPlant(CStr(sender.AccessibleDescription))
    End Sub

    Private Sub RadioButton22_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton22.CheckedChanged
        LoadPlant(CStr(sender.AccessibleDescription))
    End Sub

    Private Sub RadioButton23_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton23.CheckedChanged
        LoadPlant(CStr(sender.AccessibleDescription))
    End Sub

    Private Sub RadioButton24_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton24.CheckedChanged
        LoadPlant(CStr(sender.AccessibleDescription))
    End Sub

    Private Sub RadioButton25_CheckedChanged(sender As Object, e As EventArgs)
        LoadPlant(CStr(sender.AccessibleDescription))
    End Sub

    Private Sub RadioButton26_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton26.CheckedChanged
        LoadPlant(CStr(sender.AccessibleDescription))
    End Sub

    Private Sub RadioButton3_CheckedChanged(sender As Object, e As EventArgs) Handles AI.CheckedChanged
        Settings.TerrainType = "AI"
        TerrainPic.Image = My.Resources.AI
    End Sub

    Private Sub RadioButton5_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton5.CheckedChanged
        LoadPlant(CStr(sender.AccessibleDescription))
    End Sub

    Private Sub RadioButton6_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton6.CheckedChanged
        LoadPlant(CStr(sender.AccessibleDescription))
    End Sub

    Private Sub RadioButton7_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton7.CheckedChanged
        LoadPlant(CStr(sender.AccessibleDescription))
    End Sub

    Private Sub RadioButton8_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton8.CheckedChanged
        LoadPlant(CStr(sender.AccessibleDescription))
    End Sub

    Private Sub RadioButton9_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton9.CheckedChanged
        LoadPlant(CStr(sender.AccessibleDescription))
    End Sub

    Private Sub RebuildTerrainsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RebuildTerrainsToolStripMenuItem.Click

        SavedAlready.Clear()
        Dim RegionName = ChooseRegion(False)
        If RegionName.Length = 0 Then Return
        Dim RegionUUID As String = FindRegionByName(RegionName)

        Dim obj As New TaskObject With {
                        .TaskName = TaskName.RebuildTerrain
                    }
        RebootAndRunTask(RegionUUID, obj)

    End Sub

    Private Sub RegionMakerEnableCHeckbox_CheckedChanged(sender As Object, e As EventArgs)

        If Not _initialized Then Return
        Settings.AutoFill = AutoFillEnable.Checked
        If AutoFillEnable.Checked Then
            If AviName.Text.Length > 0 Then
                Settings.SaveSettings()
            Else
                MsgBox(My.Resources.MustSetSS, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Info_word)
            End If
        End If

    End Sub

    Private Sub RegionMakingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RegionMakingToolStripMenuItem.Click
        HelpManual("Terrain")
    End Sub

    Private Sub Resize_page(ByVal sender As Object, ByVal e As System.EventArgs)
        'Me.Text = "Form screen position = " + Me.Location.ToString
        ScreenPosition.SaveXY(Me.Left, Me.Top)
    End Sub

    Private Sub RevertButton_Click(sender As Object, e As EventArgs) Handles RevertButton.Click
        Dim name = ChooseRegion(False)
        Dim RegionUUID As String = FindRegionByName(name)
        If RegionUUID.Length = 0 Then Return

        Dim Obj = New TaskObject With {
                .TaskName = TaskName.Revert
            }
        RebootAndRunTask(RegionUUID, Obj)
    End Sub

    Private Sub SaveAllTerrain_Click(sender As Object, e As EventArgs) Handles SaveAllTerrain.Click

        For Each RegionUUID In RegionUuids()
            If Not RegionEnabled(RegionUUID) Then Continue For

            Dim obj As New TaskObject With {
                        .TaskName = TaskName.SaveTerrain
                    }
            RebootAndRunTask(RegionUUID, obj)
        Next

    End Sub

    Private Sub SeaSword_CheckedChanged(sender As Object, e As EventArgs) Handles SeaSword.CheckedChanged
        Dim thing As CheckBox = CType(sender, CheckBox)
        PutSetting(thing.Name, thing.Checked)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub SeaSword_MouseHover(sender As Object, e As EventArgs) Handles SeaSword.MouseHover
        Dim thing As CheckBox = CType(sender, CheckBox)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub Seconds_TextChanged(sender As Object, e As EventArgs) Handles Seconds.TextChanged

        Dim digitsOnly = New Regex("[^\d]")
        Seconds.Text = digitsOnly.Replace(Seconds.Text, "")
        Settings.SmartStartTimeout = CInt("0" & Seconds.Text)
        Settings.SaveSettings()
        If Settings.SmartStartTimeout < 10 Then Settings.SmartStartTimeout = 10
        TextPrint(My.Resources.minkeepalive)

    End Sub

    Private Sub SetScreen()

        ScreenPosition = New ClassScreenpos(Me.Name)
        AddHandler ResizeEnd, Handler
        Dim xy As List(Of Integer) = ScreenPosition.GetXY()
        Me.Left = xy.Item(0)
        Me.Top = xy.Item(1)
    End Sub

#End Region

#Region "LoadOARS"

#End Region

#Region "Properties"

#End Region

#Region "Scrolling text box"

#End Region

#Region "Check boxes"

#End Region

#Region "Photos"

#End Region

#Region "Plants"

    Private Sub AutoFillEnable_CheckedChanged(sender As Object, e As EventArgs) Handles AutoFillEnable.CheckedChanged
        Settings.AutoFill = AutoFillEnable.Checked
    End Sub

    Private Sub AviName_TextChanged(sender As Object, e As EventArgs) Handles AviName.TextChanged

        If Not _initialized Then Return
        AviName.BackColor = Color.Red
        If Not IsMySqlRunning() Then
            StartMySQL()
        End If

        If AviName.Text.Length > 0 Then
            Settings.SurroundOwner = AviName.Text
            Settings.SaveSettings()

            If IsMySqlRunning() Then
                Dim AvatarUUID As String = ""
                Try
                    AvatarUUID = GetAviUUUD(AviName.Text)
                Catch
                End Try
                If AvatarUUID.Length > 0 Then
                    Dim INI = New LoadIni(IO.Path.Combine(Settings.OpensimBinPath, "Estates\Estates.ini"), ";", System.Text.Encoding.UTF8)
                    INI.SetIni("SimSurround", "Owner", AvatarUUID)
                    INI.SaveIni()

                    AviName.BackColor = Color.White
                End If
            End If
        End If

    End Sub

    Private Sub DeleteAllRegions_Click(sender As Object, e As EventArgs) Handles DeleteAllRegions.Click

        Dim ctr = 0
        If PropOpensimIsRunning Then
            Dim msg = MsgBox(My.Resources.Regions_Are_Running, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Info_word)
            Return
        End If
        Dim result = MsgBox(My.Resources.DeleteSims, vbOKCancel Or MsgBoxStyle.MsgBoxSetForeground, My.Resources.Caution_word)
        If result = vbOK Then
            For Each RegionUUID In RegionUuids()
                If Estate(RegionUUID) = "SimSurround" Then
                    DeleteAllRegionData(RegionUUID)
                    ctr += 1
                End If
            Next
        Else
            TextPrint(My.Resources.Cancelled_word)
        End If

        TextPrint($"{ctr} {My.Resources.Regions_Deleted}")

    End Sub

    Private Sub SmartStartEnabled_CheckedChanged(sender As Object, e As EventArgs) Handles SmartStartEnabled.CheckedChanged
        If Not _initialized Then Return

        If SmartStartEnabled.Checked Then
            If SuspendButton.Checked Then DelayRegionReady.Text = My.Resources._0
        End If

        Settings.Smart_Start = SmartStartEnabled.Checked
        TextPrint("Smart Start is " & CStr(SmartStartEnabled.Checked))
        Settings.SaveSettings()
    End Sub

    Private Sub SmartStartToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SmartStartToolStripMenuItem.Click
        HelpManual("SmartStart")
    End Sub

    Private Sub Smooth_CheckedChanged(sender As Object, e As EventArgs) Handles Smooth.CheckedChanged

        If Not _initialized Then Return
        Settings.LandSmooth = Smooth.Checked
        Settings.SaveSettings()

    End Sub

    Private Sub Smooth_TextChanged_2(sender As Object, e As EventArgs) Handles SmoothTextBox.TextChanged

        If Not _initialized Then Return
        Dim digitsOnly = New Regex("[^\d\.]")
        SmoothTextBox.Text = digitsOnly.Replace(SmoothTextBox.Text, "")
        If Convert.ToSingle("0" & SmoothTextBox.Text, Globalization.CultureInfo.InvariantCulture) > 1 Then SmoothTextBox.Text = CStr(1)
        Settings.LandStrength = Convert.ToDouble("0" & SmoothTextBox.Text, Globalization.CultureInfo.InvariantCulture)
        Settings.SaveSettings()

    End Sub

    Private Sub StartSize_TextChanged(sender As Object, e As EventArgs) Handles StartSizeX.TextChanged
        If Not _initialized Then Return
        Dim digitsOnly = New Regex("[^\d\.]")
        StartSizeX.Text = digitsOnly.Replace(StartSizeX.Text, "")
        If Convert.ToSingle("0" & StartSizeX.Text, Globalization.CultureInfo.InvariantCulture) < 0 Then StartSizeX.Text = CStr(0)
        MakeSetting()
    End Sub

    Private Sub StartSizeY_TextChanged(sender As Object, e As EventArgs) Handles StartSizeY.TextChanged
        If Not _initialized Then Return
        Dim digitsOnly = New Regex("[^\d\.]")
        StartSizeY.Text = digitsOnly.Replace(StartSizeY.Text, "")
        If Convert.ToSingle("0" & StartSizeY.Text, Globalization.CultureInfo.InvariantCulture) < 0 Then StartSizeY.Text = CStr(0)
        MakeSetting()
    End Sub

    Private Sub StartSizeZ_TextChanged(sender As Object, e As EventArgs) Handles StartSizeZ.TextChanged
        If Not _initialized Then Return
        Dim digitsOnly = New Regex("[^\d\.]")
        StartSizeZ.Text = digitsOnly.Replace(StartSizeZ.Text, "")
        If Convert.ToSingle("0" & StartSizeZ.Text, Globalization.CultureInfo.InvariantCulture) < 0 Then StartSizeZ.Text = CStr(0)
        MakeSetting()
    End Sub

    Private Sub TaperTextBox_TextChanged(sender As Object, e As EventArgs) Handles TaperTextBox.TextChanged
        If Not _initialized Then Return
        Dim digitsOnly = New Regex("[^\d\.]")
        TaperTextBox.Text = digitsOnly.Replace(TaperTextBox.Text, "")
        If Convert.ToSingle("0" & TaperTextBox.Text, Globalization.CultureInfo.InvariantCulture) > 1 Then TaperTextBox.Text = CStr(1)
        Settings.LandTaper = Convert.ToDouble("0" & TaperTextBox.Text, Globalization.CultureInfo.InvariantCulture)
        MakeSetting()

    End Sub

    Private Sub TempCheckBox_CheckedChanged_1(sender As Object, e As EventArgs) Handles TempCheckBox.CheckedChanged
        If Not _initialized Then Return
        Settings.TempRegion = TempCheckBox.Checked
        Settings.SaveSettings()
    End Sub

    Private Sub TerrainApply_Click(sender As Object, e As EventArgs) Handles TerrainApply.Click

        Dim backupname = IO.Path.Combine(Settings.OpensimBinPath, "Terrains")
        Dim name = ChooseRegion(False)
        Dim RegionUUID As String = FindRegionByName(name)
        If RegionUUID.Length = 0 Then Return
        Dim TerrainName = _TerrainName.Item(_Index)
        TerrainName = TerrainName.Replace(".jpg", ".r32")
        Dim obj As New TaskObject With {
                            .TaskName = TaskName.TerrainLoad,
                            .Command = TerrainName
                        }
        RebootAndRunTask(RegionUUID, obj)

    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TreeLineHight.TextChanged
        If Not _initialized Then Return
        Dim digitsOnly = New Regex("[^\d\.]")
        TreeLineHight.Text = digitsOnly.Replace(TreeLineHight.Text, "")
        If CInt("0" & TreeLineHight.Text) > 255 Then TreeLineHight.Text = CStr(255)
        MakeSetting()
    End Sub

    Private Sub TextBox2_TextChanged_1(sender As Object, e As EventArgs) Handles Rad.TextChanged
        If Not _initialized Then Return
        Dim digitsOnly = New Regex("[^\d]")
        Rad.Text = digitsOnly.Replace(Rad.Text, "")
        If Convert.ToSingle("0" & Rad.Text, Globalization.CultureInfo.InvariantCulture) > 1024 Then Rad.Text = CStr(1024)
        MakeSetting()
    End Sub

    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles SaveTerrain.Click

        'Save menu
        If Not _initialized Then Return
        Dim RegionName = ChooseRegion(False)
        If RegionName.Length = 0 Then Return
        Dim RegionUUID As String = FindRegionByName(RegionName)

        Dim obj As New TaskObject With {
                            .TaskName = TaskName.SaveTerrain
                        }
        RebootAndRunTask(RegionUUID, obj)

    End Sub

    Private Sub TropicalBush1_CheckedChanged(sender As Object, e As EventArgs) Handles TropicalBush1.CheckedChanged
        Dim thing As CheckBox = CType(sender, CheckBox)
        PutSetting(thing.Name, thing.Checked)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub TropicalBush1_MouseHover(sender As Object, e As EventArgs) Handles TropicalBush1.MouseHover
        Dim thing As CheckBox = CType(sender, CheckBox)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub TropicalBush2_CheckedChanged(sender As Object, e As EventArgs) Handles TropicalBush2.CheckedChanged
        Dim thing As CheckBox = CType(sender, CheckBox)
        PutSetting(thing.Name, thing.Checked)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub TropicalBush2_MouseHover(sender As Object, e As EventArgs) Handles TropicalBush2.MouseHover
        Dim thing As CheckBox = CType(sender, CheckBox)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub ViewTerrainFolderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewTerrainFolderToolStripMenuItem.Click

        Process.Start("explorer.exe", IO.Path.Combine(Settings.OpensimBinPath, "Terrains"))

    End Sub

    Private Sub Water_CheckedChanged(sender As Object, e As EventArgs) Handles Water.CheckedChanged
        Settings.TerrainType = "Water"
#Disable Warning CA1303 ' Do not pass literals as localized parameters
        FlatLandLevel.Text = "1"
#Enable Warning CA1303 ' Do not pass literals as localized parameters
        TerrainPic.Image = My.Resources.water
    End Sub

    Private Sub WinterAspen_CheckedChanged(sender As Object, e As EventArgs) Handles WinterAspen.CheckedChanged
        Dim thing As CheckBox = CType(sender, CheckBox)
        PutSetting(thing.Name, thing.Checked)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub WinterAspen_Hover(sender As Object, e As EventArgs) Handles WinterAspen.MouseHover
        Dim thing As CheckBox = CType(sender, CheckBox)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub WinterPine1_CheckedChanged(sender As Object, e As EventArgs) Handles WinterPine1.CheckedChanged
        Dim thing As CheckBox = CType(sender, CheckBox)
        PutSetting(thing.Name, thing.Checked)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub WinterPine1_MouseHover(sender As Object, e As EventArgs) Handles WinterPine1.MouseHover
        Dim thing As CheckBox = CType(sender, CheckBox)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub WinterPine2_CheckedChanged(sender As Object, e As EventArgs) Handles WinterPine2.CheckedChanged
        Dim thing As CheckBox = CType(sender, CheckBox)
        PutSetting(thing.Name, thing.Checked)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

    Private Sub WinterPine2_MouseHover(sender As Object, e As EventArgs) Handles WinterPine2.MouseHover
        Dim thing As CheckBox = CType(sender, CheckBox)
        PictureBox1.Image = GetPic(thing.Name)
    End Sub

#End Region

End Class
