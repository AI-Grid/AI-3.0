﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormRegion
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId:="System.Windows.Forms.ToolTip.SetToolTip(System.Windows.Forms.Control,System.String)")>
    <CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId:="System.Windows.Forms.ButtonBase.set_Text(System.String)")>
    <CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId:="System.Windows.Forms.Label.set_Text(System.String)")>
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormRegion))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.CoordY = New System.Windows.Forms.TextBox()
        Me.CoordX = New System.Windows.Forms.TextBox()
        Me.RegionName = New System.Windows.Forms.TextBox()
        Me.RadioButton4 = New System.Windows.Forms.RadioButton()
        Me.RadioButton3 = New System.Windows.Forms.RadioButton()
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.MaxAgents = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.PhysicalPrimMax = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.MaxPrims = New System.Windows.Forms.TextBox()
        Me.NonphysicalPrimMax = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.ClampPrimSize = New System.Windows.Forms.CheckBox()
        Me.BirdsCheckBox = New System.Windows.Forms.CheckBox()
        Me.TidesCheckbox = New System.Windows.Forms.CheckBox()
        Me.TPCheckBox1 = New System.Windows.Forms.CheckBox()
        Me.AllowGods = New System.Windows.Forms.CheckBox()
        Me.ManagerGod = New System.Windows.Forms.CheckBox()
        Me.RegionGod = New System.Windows.Forms.CheckBox()
        Me.SmartStartCheckBox = New System.Windows.Forms.CheckBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.ScriptTimerTextBox = New System.Windows.Forms.TextBox()
        Me.DisableGBCheckBox = New System.Windows.Forms.CheckBox()
        Me.DisallowForeigners = New System.Windows.Forms.CheckBox()
        Me.DisallowResidents = New System.Windows.Forms.CheckBox()
        Me.FrametimeBox = New System.Windows.Forms.TextBox()
        Me.SkipAutoCheckBox = New System.Windows.Forms.CheckBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.RadioButton8 = New System.Windows.Forms.RadioButton()
        Me.RadioButton7 = New System.Windows.Forms.RadioButton()
        Me.RadioButton6 = New System.Windows.Forms.RadioButton()
        Me.RadioButton5 = New System.Windows.Forms.RadioButton()
        Me.RadioButton10 = New System.Windows.Forms.RadioButton()
        Me.RadioButton9 = New System.Windows.Forms.RadioButton()
        Me.RadioButton12 = New System.Windows.Forms.RadioButton()
        Me.RadioButton11 = New System.Windows.Forms.RadioButton()
        Me.RadioButton16 = New System.Windows.Forms.RadioButton()
        Me.RadioButton15 = New System.Windows.Forms.RadioButton()
        Me.RadioButton14 = New System.Windows.Forms.RadioButton()
        Me.RadioButton13 = New System.Windows.Forms.RadioButton()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Physics_Hybrid = New System.Windows.Forms.RadioButton()
        Me.Physics_Bullet = New System.Windows.Forms.RadioButton()
        Me.Physics_Default = New System.Windows.Forms.RadioButton()
        Me.Physics_Separate = New System.Windows.Forms.RadioButton()
        Me.Physics_ubODE = New System.Windows.Forms.RadioButton()
        Me.RegionPort = New System.Windows.Forms.TextBox()
        Me.Advanced = New System.Windows.Forms.GroupBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.UUID = New System.Windows.Forms.TextBox()
        Me.NameTip = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.DeleteButton = New System.Windows.Forms.Button()
        Me.EnabledCheckBox = New System.Windows.Forms.CheckBox()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.GroupBox8 = New System.Windows.Forms.GroupBox()
        Me.ScriptDefaultButton = New System.Windows.Forms.RadioButton()
        Me.XEngineButton = New System.Windows.Forms.RadioButton()
        Me.YEngineButton = New System.Windows.Forms.RadioButton()
        Me.GroupBox7 = New System.Windows.Forms.GroupBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Publish = New System.Windows.Forms.RadioButton()
        Me.NoPublish = New System.Windows.Forms.RadioButton()
        Me.PublishDefault = New System.Windows.Forms.RadioButton()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.Gods_Use_Default = New System.Windows.Forms.CheckBox()
        Me.MapBox = New System.Windows.Forms.GroupBox()
        Me.Maps_Use_Default = New System.Windows.Forms.RadioButton()
        Me.MapPicture = New System.Windows.Forms.PictureBox()
        Me.MapNone = New System.Windows.Forms.RadioButton()
        Me.MapSimple = New System.Windows.Forms.RadioButton()
        Me.MapBetter = New System.Windows.Forms.RadioButton()
        Me.MapBest = New System.Windows.Forms.RadioButton()
        Me.MapGood = New System.Windows.Forms.RadioButton()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.DeregisterButton = New System.Windows.Forms.Button()
        Me.MenuStrip2 = New System.Windows.Forms.MenuStrip()
        Me.ToolStripMenuItem30 = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupBox1.SuspendLayout()
        Me.Advanced.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.GroupBox8.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.MapBox.SuspendLayout()
        CType(Me.MapPicture, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip2.SuspendLayout()
        Me.SuspendLayout()
        '
        'CoordY
        '
        Me.CoordY.Location = New System.Drawing.Point(286, 22)
        Me.CoordY.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.CoordY.Name = "CoordY"
        Me.CoordY.Size = New System.Drawing.Size(62, 29)
        Me.CoordY.TabIndex = 13
        Me.ToolTip1.SetToolTip(Me.CoordY, Global.Outworldz.My.Resources.Resources.CoordY)
        '
        'CoordX
        '
        Me.CoordX.Location = New System.Drawing.Point(169, 22)
        Me.CoordX.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.CoordX.Name = "CoordX"
        Me.CoordX.Size = New System.Drawing.Size(67, 29)
        Me.CoordX.TabIndex = 12
        Me.ToolTip1.SetToolTip(Me.CoordX, Global.Outworldz.My.Resources.Resources.Coordx)
        '
        'RegionName
        '
        Me.RegionName.Location = New System.Drawing.Point(27, 98)
        Me.RegionName.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.RegionName.Name = "RegionName"
        Me.RegionName.Size = New System.Drawing.Size(317, 29)
        Me.RegionName.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.RegionName, Global.Outworldz.My.Resources.Resources.Region_Name)
        '
        'RadioButton4
        '
        Me.RadioButton4.AutoSize = True
        Me.RadioButton4.Location = New System.Drawing.Point(35, 148)
        Me.RadioButton4.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.RadioButton4.Name = "RadioButton4"
        Me.RadioButton4.Size = New System.Drawing.Size(83, 29)
        Me.RadioButton4.TabIndex = 6
        Me.RadioButton4.TabStop = True
        Me.RadioButton4.Text = "4 X 4"
        Me.ToolTip1.SetToolTip(Me.RadioButton4, "1024 X 1024")
        Me.RadioButton4.UseVisualStyleBackColor = True
        '
        'RadioButton3
        '
        Me.RadioButton3.AutoSize = True
        Me.RadioButton3.Location = New System.Drawing.Point(35, 110)
        Me.RadioButton3.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.RadioButton3.Name = "RadioButton3"
        Me.RadioButton3.Size = New System.Drawing.Size(83, 29)
        Me.RadioButton3.TabIndex = 5
        Me.RadioButton3.TabStop = True
        Me.RadioButton3.Text = "3 X 3"
        Me.ToolTip1.SetToolTip(Me.RadioButton3, "768 X 768")
        Me.RadioButton3.UseVisualStyleBackColor = True
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Location = New System.Drawing.Point(35, 69)
        Me.RadioButton2.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(83, 29)
        Me.RadioButton2.TabIndex = 4
        Me.RadioButton2.TabStop = True
        Me.RadioButton2.Text = "2 X 2"
        Me.ToolTip1.SetToolTip(Me.RadioButton2, "512 X 512")
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Location = New System.Drawing.Point(35, 28)
        Me.RadioButton1.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(83, 29)
        Me.RadioButton1.TabIndex = 3
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "1 X 1"
        Me.ToolTip1.SetToolTip(Me.RadioButton1, "256 X 256")
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'MaxAgents
        '
        Me.MaxAgents.Location = New System.Drawing.Point(21, 351)
        Me.MaxAgents.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.MaxAgents.Name = "MaxAgents"
        Me.MaxAgents.Size = New System.Drawing.Size(67, 29)
        Me.MaxAgents.TabIndex = 20
        Me.ToolTip1.SetToolTip(Me.MaxAgents, Global.Outworldz.My.Resources.Resources.Max_Agents)
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(113, 182)
        Me.Label5.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(207, 25)
        Me.Label5.TabIndex = 30
        Me.Label5.Text = "Nonphysical Prim Size"
        Me.ToolTip1.SetToolTip(Me.Label5, Global.Outworldz.My.Resources.Resources.Max_NonPhys)
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(120, 224)
        Me.Label9.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(216, 25)
        Me.Label9.TabIndex = 31
        Me.Label9.Text = "Physical Prim Max Size"
        Me.ToolTip1.SetToolTip(Me.Label9, Global.Outworldz.My.Resources.Resources.Max_Phys)
        '
        'PhysicalPrimMax
        '
        Me.PhysicalPrimMax.Location = New System.Drawing.Point(21, 218)
        Me.PhysicalPrimMax.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.PhysicalPrimMax.Name = "PhysicalPrimMax"
        Me.PhysicalPrimMax.Size = New System.Drawing.Size(67, 29)
        Me.PhysicalPrimMax.TabIndex = 17
        Me.ToolTip1.SetToolTip(Me.PhysicalPrimMax, Global.Outworldz.My.Resources.Resources.Max_Phys)
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(120, 267)
        Me.Label10.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(157, 25)
        Me.Label10.TabIndex = 34
        Me.Label10.Text = "Clamp Prim Size"
        Me.ToolTip1.SetToolTip(Me.Label10, Global.Outworldz.My.Resources.Resources.ClampSize)
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(120, 308)
        Me.Label11.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(295, 25)
        Me.Label11.TabIndex = 36
        Me.Label11.Text = "Max Number of Prims in a Parcel"
        Me.ToolTip1.SetToolTip(Me.Label11, Global.Outworldz.My.Resources.Resources.Viewer_Stops_Counting)
        '
        'MaxPrims
        '
        Me.MaxPrims.Location = New System.Drawing.Point(21, 301)
        Me.MaxPrims.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.MaxPrims.Name = "MaxPrims"
        Me.MaxPrims.Size = New System.Drawing.Size(67, 29)
        Me.MaxPrims.TabIndex = 19
        Me.ToolTip1.SetToolTip(Me.MaxPrims, Global.Outworldz.My.Resources.Resources.Not_Normal)
        '
        'NonphysicalPrimMax
        '
        Me.NonphysicalPrimMax.Location = New System.Drawing.Point(21, 175)
        Me.NonphysicalPrimMax.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.NonphysicalPrimMax.Name = "NonphysicalPrimMax"
        Me.NonphysicalPrimMax.Size = New System.Drawing.Size(67, 29)
        Me.NonphysicalPrimMax.TabIndex = 16
        Me.ToolTip1.SetToolTip(Me.NonphysicalPrimMax, Global.Outworldz.My.Resources.Resources.Normal_Prim)
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(120, 351)
        Me.Label12.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(288, 25)
        Me.Label12.TabIndex = 38
        Me.Label12.Text = "Max number of Avatars + NPCs"
        Me.ToolTip1.SetToolTip(Me.Label12, Global.Outworldz.My.Resources.Resources.Max_Agents)
        '
        'ClampPrimSize
        '
        Me.ClampPrimSize.AutoSize = True
        Me.ClampPrimSize.Location = New System.Drawing.Point(21, 266)
        Me.ClampPrimSize.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.ClampPrimSize.Name = "ClampPrimSize"
        Me.ClampPrimSize.Size = New System.Drawing.Size(22, 21)
        Me.ClampPrimSize.TabIndex = 18
        Me.ToolTip1.SetToolTip(Me.ClampPrimSize, Global.Outworldz.My.Resources.Resources.ClampSize)
        Me.ClampPrimSize.UseVisualStyleBackColor = True
        '
        'BirdsCheckBox
        '
        Me.BirdsCheckBox.AutoSize = True
        Me.BirdsCheckBox.Location = New System.Drawing.Point(27, 34)
        Me.BirdsCheckBox.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.BirdsCheckBox.Name = "BirdsCheckBox"
        Me.BirdsCheckBox.Size = New System.Drawing.Size(142, 29)
        Me.BirdsCheckBox.TabIndex = 21
        Me.BirdsCheckBox.Text = Global.Outworldz.My.Resources.Resources.Bird_Module_word
        Me.ToolTip1.SetToolTip(Me.BirdsCheckBox, Global.Outworldz.My.Resources.Resources.GBoids)
        Me.BirdsCheckBox.UseVisualStyleBackColor = True
        '
        'TidesCheckbox
        '
        Me.TidesCheckbox.AutoSize = True
        Me.TidesCheckbox.Location = New System.Drawing.Point(27, 72)
        Me.TidesCheckbox.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.TidesCheckbox.Name = "TidesCheckbox"
        Me.TidesCheckbox.Size = New System.Drawing.Size(342, 29)
        Me.TidesCheckbox.TabIndex = 21
        Me.TidesCheckbox.Text = Global.Outworldz.My.Resources.Resources.Tide_Enable
        Me.ToolTip1.SetToolTip(Me.TidesCheckbox, Global.Outworldz.My.Resources.Resources.GTide)
        Me.TidesCheckbox.UseVisualStyleBackColor = True
        '
        'TPCheckBox1
        '
        Me.TPCheckBox1.AutoSize = True
        Me.TPCheckBox1.Location = New System.Drawing.Point(27, 113)
        Me.TPCheckBox1.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.TPCheckBox1.Name = "TPCheckBox1"
        Me.TPCheckBox1.Size = New System.Drawing.Size(221, 29)
        Me.TPCheckBox1.TabIndex = 22
        Me.TPCheckBox1.Text = Global.Outworldz.My.Resources.Resources.Teleporter_Enable_word
        Me.ToolTip1.SetToolTip(Me.TPCheckBox1, Global.Outworldz.My.Resources.Resources.Teleport_Tooltip)
        Me.TPCheckBox1.UseVisualStyleBackColor = True
        '
        'AllowGods
        '
        Me.AllowGods.AutoSize = True
        Me.AllowGods.Location = New System.Drawing.Point(26, 91)
        Me.AllowGods.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.AllowGods.Name = "AllowGods"
        Me.AllowGods.Size = New System.Drawing.Size(246, 29)
        Me.AllowGods.TabIndex = 1858
        Me.AllowGods.Text = Global.Outworldz.My.Resources.Resources.Allow_Or_Disallow_Gods_word
        Me.ToolTip1.SetToolTip(Me.AllowGods, Global.Outworldz.My.Resources.Resources.AllowGodsTooltip)
        Me.AllowGods.UseVisualStyleBackColor = True
        '
        'ManagerGod
        '
        Me.ManagerGod.AutoSize = True
        Me.ManagerGod.Location = New System.Drawing.Point(26, 174)
        Me.ManagerGod.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.ManagerGod.Name = "ManagerGod"
        Me.ManagerGod.Size = New System.Drawing.Size(233, 29)
        Me.ManagerGod.TabIndex = 6
        Me.ManagerGod.Text = Global.Outworldz.My.Resources.Resources.EstateManagerIsGod_word
        Me.ToolTip1.SetToolTip(Me.ManagerGod, Global.Outworldz.My.Resources.Resources.EMGod)
        Me.ManagerGod.UseVisualStyleBackColor = True
        '
        'RegionGod
        '
        Me.RegionGod.AutoSize = True
        Me.RegionGod.Location = New System.Drawing.Point(26, 132)
        Me.RegionGod.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.RegionGod.Name = "RegionGod"
        Me.RegionGod.Size = New System.Drawing.Size(223, 29)
        Me.RegionGod.TabIndex = 1855
        Me.RegionGod.Text = Global.Outworldz.My.Resources.Resources.Region_Owner_Is_God_word
        Me.ToolTip1.SetToolTip(Me.RegionGod, Global.Outworldz.My.Resources.Resources.Region_Owner_Is_God_word)
        Me.RegionGod.UseVisualStyleBackColor = True
        '
        'SmartStartCheckBox
        '
        Me.SmartStartCheckBox.AutoSize = True
        Me.SmartStartCheckBox.Location = New System.Drawing.Point(28, 300)
        Me.SmartStartCheckBox.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.SmartStartCheckBox.Name = "SmartStartCheckBox"
        Me.SmartStartCheckBox.Size = New System.Drawing.Size(136, 29)
        Me.SmartStartCheckBox.TabIndex = 23
        Me.SmartStartCheckBox.Text = Global.Outworldz.My.Resources.Resources.Smart_Start_word
        Me.ToolTip1.SetToolTip(Me.SmartStartCheckBox, Global.Outworldz.My.Resources.Resources.GTide)
        Me.SmartStartCheckBox.UseVisualStyleBackColor = True
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(120, 400)
        Me.Label14.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(208, 25)
        Me.Label14.TabIndex = 41
        Me.Label14.Text = "Script Timer Rate (0.2)"
        Me.ToolTip1.SetToolTip(Me.Label14, Global.Outworldz.My.Resources.Resources.Script_Timer_Text)
        '
        'ScriptTimerTextBox
        '
        Me.ScriptTimerTextBox.Location = New System.Drawing.Point(21, 400)
        Me.ScriptTimerTextBox.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.ScriptTimerTextBox.Name = "ScriptTimerTextBox"
        Me.ScriptTimerTextBox.Size = New System.Drawing.Size(67, 29)
        Me.ScriptTimerTextBox.TabIndex = 40
        Me.ToolTip1.SetToolTip(Me.ScriptTimerTextBox, Global.Outworldz.My.Resources.Resources.STComment)
        '
        'DisableGBCheckBox
        '
        Me.DisableGBCheckBox.AutoSize = True
        Me.DisableGBCheckBox.Location = New System.Drawing.Point(27, 148)
        Me.DisableGBCheckBox.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.DisableGBCheckBox.Name = "DisableGBCheckBox"
        Me.DisableGBCheckBox.Size = New System.Drawing.Size(206, 29)
        Me.DisableGBCheckBox.TabIndex = 24
        Me.DisableGBCheckBox.Text = Global.Outworldz.My.Resources.Resources.Disable_Gloebits_word
        Me.ToolTip1.SetToolTip(Me.DisableGBCheckBox, Global.Outworldz.My.Resources.Resources.Disable_Gloebits_text)
        Me.DisableGBCheckBox.UseVisualStyleBackColor = True
        '
        'DisallowForeigners
        '
        Me.DisallowForeigners.AutoSize = True
        Me.DisallowForeigners.Location = New System.Drawing.Point(27, 183)
        Me.DisallowForeigners.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.DisallowForeigners.Name = "DisallowForeigners"
        Me.DisallowForeigners.Size = New System.Drawing.Size(243, 29)
        Me.DisallowForeigners.TabIndex = 25
        Me.DisallowForeigners.Text = Global.Outworldz.My.Resources.Resources.Disable_Foreigners_word
        Me.ToolTip1.SetToolTip(Me.DisallowForeigners, Global.Outworldz.My.Resources.Resources.No_HG)
        Me.DisallowForeigners.UseVisualStyleBackColor = True
        '
        'DisallowResidents
        '
        Me.DisallowResidents.AutoSize = True
        Me.DisallowResidents.Location = New System.Drawing.Point(27, 222)
        Me.DisallowResidents.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.DisallowResidents.Name = "DisallowResidents"
        Me.DisallowResidents.Size = New System.Drawing.Size(226, 29)
        Me.DisallowResidents.TabIndex = 26
        Me.DisallowResidents.Text = Global.Outworldz.My.Resources.Resources.Disable_Residents
        Me.ToolTip1.SetToolTip(Me.DisallowResidents, Global.Outworldz.My.Resources.Resources.Only_Owners)
        Me.DisallowResidents.UseVisualStyleBackColor = True
        '
        'FrametimeBox
        '
        Me.FrametimeBox.Location = New System.Drawing.Point(21, 449)
        Me.FrametimeBox.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.FrametimeBox.Name = "FrametimeBox"
        Me.FrametimeBox.Size = New System.Drawing.Size(67, 29)
        Me.FrametimeBox.TabIndex = 42
        Me.ToolTip1.SetToolTip(Me.FrametimeBox, Global.Outworldz.My.Resources.Resources.FrameTime)
        '
        'SkipAutoCheckBox
        '
        Me.SkipAutoCheckBox.AutoSize = True
        Me.SkipAutoCheckBox.Location = New System.Drawing.Point(27, 260)
        Me.SkipAutoCheckBox.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.SkipAutoCheckBox.Name = "SkipAutoCheckBox"
        Me.SkipAutoCheckBox.Size = New System.Drawing.Size(286, 29)
        Me.SkipAutoCheckBox.TabIndex = 27
        Me.SkipAutoCheckBox.Text = Global.Outworldz.My.Resources.Resources.Skip_Autobackup_word
        Me.ToolTip1.SetToolTip(Me.SkipAutoCheckBox, Global.Outworldz.My.Resources.Resources.WillNotSave)
        Me.SkipAutoCheckBox.UseVisualStyleBackColor = True
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(120, 449)
        Me.Label15.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(192, 25)
        Me.Label15.TabIndex = 43
        Me.Label15.Text = "Frame Rate (0.0909)"
        Me.ToolTip1.SetToolTip(Me.Label15, Global.Outworldz.My.Resources.Resources.FRText)
        '
        'RadioButton8
        '
        Me.RadioButton8.AutoSize = True
        Me.RadioButton8.Location = New System.Drawing.Point(156, 146)
        Me.RadioButton8.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.RadioButton8.Name = "RadioButton8"
        Me.RadioButton8.Size = New System.Drawing.Size(83, 29)
        Me.RadioButton8.TabIndex = 10
        Me.RadioButton8.TabStop = True
        Me.RadioButton8.Text = "8 X 8"
        Me.ToolTip1.SetToolTip(Me.RadioButton8, "2,048 X 2,048")
        Me.RadioButton8.UseVisualStyleBackColor = True
        '
        'RadioButton7
        '
        Me.RadioButton7.AutoSize = True
        Me.RadioButton7.Location = New System.Drawing.Point(156, 105)
        Me.RadioButton7.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.RadioButton7.Name = "RadioButton7"
        Me.RadioButton7.Size = New System.Drawing.Size(83, 29)
        Me.RadioButton7.TabIndex = 9
        Me.RadioButton7.TabStop = True
        Me.RadioButton7.Text = "7 X 7"
        Me.ToolTip1.SetToolTip(Me.RadioButton7, "1792 X 1792")
        Me.RadioButton7.UseVisualStyleBackColor = True
        '
        'RadioButton6
        '
        Me.RadioButton6.AutoSize = True
        Me.RadioButton6.Location = New System.Drawing.Point(156, 64)
        Me.RadioButton6.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.RadioButton6.Name = "RadioButton6"
        Me.RadioButton6.Size = New System.Drawing.Size(83, 29)
        Me.RadioButton6.TabIndex = 8
        Me.RadioButton6.TabStop = True
        Me.RadioButton6.Text = "6 X 6"
        Me.ToolTip1.SetToolTip(Me.RadioButton6, "1536 X 1536")
        Me.RadioButton6.UseVisualStyleBackColor = True
        '
        'RadioButton5
        '
        Me.RadioButton5.AutoSize = True
        Me.RadioButton5.Location = New System.Drawing.Point(156, 26)
        Me.RadioButton5.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.RadioButton5.Name = "RadioButton5"
        Me.RadioButton5.Size = New System.Drawing.Size(83, 29)
        Me.RadioButton5.TabIndex = 7
        Me.RadioButton5.TabStop = True
        Me.RadioButton5.Text = "5 X 5"
        Me.ToolTip1.SetToolTip(Me.RadioButton5, "1280 X 1280")
        Me.RadioButton5.UseVisualStyleBackColor = True
        '
        'RadioButton10
        '
        Me.RadioButton10.AutoSize = True
        Me.RadioButton10.Location = New System.Drawing.Point(267, 64)
        Me.RadioButton10.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.RadioButton10.Name = "RadioButton10"
        Me.RadioButton10.Size = New System.Drawing.Size(105, 29)
        Me.RadioButton10.TabIndex = 12
        Me.RadioButton10.TabStop = True
        Me.RadioButton10.Text = "10 X 10"
        Me.ToolTip1.SetToolTip(Me.RadioButton10, "2560 X 2560")
        Me.RadioButton10.UseVisualStyleBackColor = True
        '
        'RadioButton9
        '
        Me.RadioButton9.AutoSize = True
        Me.RadioButton9.Location = New System.Drawing.Point(267, 26)
        Me.RadioButton9.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.RadioButton9.Name = "RadioButton9"
        Me.RadioButton9.Size = New System.Drawing.Size(83, 29)
        Me.RadioButton9.TabIndex = 11
        Me.RadioButton9.TabStop = True
        Me.RadioButton9.Text = "9 X 9"
        Me.ToolTip1.SetToolTip(Me.RadioButton9, "2304 X 2304")
        Me.RadioButton9.UseVisualStyleBackColor = True
        '
        'RadioButton12
        '
        Me.RadioButton12.AutoSize = True
        Me.RadioButton12.Location = New System.Drawing.Point(272, 146)
        Me.RadioButton12.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.RadioButton12.Name = "RadioButton12"
        Me.RadioButton12.Size = New System.Drawing.Size(105, 29)
        Me.RadioButton12.TabIndex = 14
        Me.RadioButton12.TabStop = True
        Me.RadioButton12.Text = "12 X 12"
        Me.ToolTip1.SetToolTip(Me.RadioButton12, "3072 X 3072")
        Me.RadioButton12.UseVisualStyleBackColor = True
        '
        'RadioButton11
        '
        Me.RadioButton11.AutoSize = True
        Me.RadioButton11.Location = New System.Drawing.Point(272, 105)
        Me.RadioButton11.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.RadioButton11.Name = "RadioButton11"
        Me.RadioButton11.Size = New System.Drawing.Size(105, 29)
        Me.RadioButton11.TabIndex = 13
        Me.RadioButton11.TabStop = True
        Me.RadioButton11.Text = "11 X 11"
        Me.ToolTip1.SetToolTip(Me.RadioButton11, "2816 X 2816")
        Me.RadioButton11.UseVisualStyleBackColor = True
        '
        'RadioButton16
        '
        Me.RadioButton16.AutoSize = True
        Me.RadioButton16.Location = New System.Drawing.Point(392, 146)
        Me.RadioButton16.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.RadioButton16.Name = "RadioButton16"
        Me.RadioButton16.Size = New System.Drawing.Size(105, 29)
        Me.RadioButton16.TabIndex = 18
        Me.RadioButton16.TabStop = True
        Me.RadioButton16.Text = "16 X 16"
        Me.ToolTip1.SetToolTip(Me.RadioButton16, "4096 X 4096")
        Me.RadioButton16.UseVisualStyleBackColor = True
        '
        'RadioButton15
        '
        Me.RadioButton15.AutoSize = True
        Me.RadioButton15.Location = New System.Drawing.Point(392, 105)
        Me.RadioButton15.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.RadioButton15.Name = "RadioButton15"
        Me.RadioButton15.Size = New System.Drawing.Size(105, 29)
        Me.RadioButton15.TabIndex = 17
        Me.RadioButton15.TabStop = True
        Me.RadioButton15.Text = "15 X 15"
        Me.ToolTip1.SetToolTip(Me.RadioButton15, "3840 X 3840")
        Me.RadioButton15.UseVisualStyleBackColor = True
        '
        'RadioButton14
        '
        Me.RadioButton14.AutoSize = True
        Me.RadioButton14.Location = New System.Drawing.Point(390, 64)
        Me.RadioButton14.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.RadioButton14.Name = "RadioButton14"
        Me.RadioButton14.Size = New System.Drawing.Size(105, 29)
        Me.RadioButton14.TabIndex = 16
        Me.RadioButton14.TabStop = True
        Me.RadioButton14.Text = "14 X 14"
        Me.ToolTip1.SetToolTip(Me.RadioButton14, "3584 X 3584")
        Me.RadioButton14.UseVisualStyleBackColor = True
        '
        'RadioButton13
        '
        Me.RadioButton13.AutoSize = True
        Me.RadioButton13.Location = New System.Drawing.Point(390, 26)
        Me.RadioButton13.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.RadioButton13.Name = "RadioButton13"
        Me.RadioButton13.Size = New System.Drawing.Size(105, 29)
        Me.RadioButton13.TabIndex = 15
        Me.RadioButton13.TabStop = True
        Me.RadioButton13.Text = "13 X 13"
        Me.ToolTip1.SetToolTip(Me.RadioButton13, "3328 X 3328")
        Me.RadioButton13.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Physics_Hybrid)
        Me.GroupBox1.Controls.Add(Me.Physics_Bullet)
        Me.GroupBox1.Controls.Add(Me.Physics_Default)
        Me.GroupBox1.Controls.Add(Me.Physics_Separate)
        Me.GroupBox1.Controls.Add(Me.Physics_ubODE)
        Me.GroupBox1.Location = New System.Drawing.Point(35, 488)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.GroupBox1.Size = New System.Drawing.Size(399, 227)
        Me.GroupBox1.TabIndex = 1879
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Physics"
        Me.ToolTip1.SetToolTip(Me.GroupBox1, Global.Outworldz.My.Resources.Resources.Sim_Rate)
        '
        'Physics_Hybrid
        '
        Me.Physics_Hybrid.AutoSize = True
        Me.Physics_Hybrid.Location = New System.Drawing.Point(8, 188)
        Me.Physics_Hybrid.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.Physics_Hybrid.Name = "Physics_Hybrid"
        Me.Physics_Hybrid.Size = New System.Drawing.Size(93, 29)
        Me.Physics_Hybrid.TabIndex = 139
        Me.Physics_Hybrid.TabStop = True
        Me.Physics_Hybrid.Text = "Hybrid"
        Me.Physics_Hybrid.UseVisualStyleBackColor = True
        '
        'Physics_Bullet
        '
        Me.Physics_Bullet.AutoSize = True
        Me.Physics_Bullet.Location = New System.Drawing.Point(8, 110)
        Me.Physics_Bullet.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.Physics_Bullet.Name = "Physics_Bullet"
        Me.Physics_Bullet.Size = New System.Drawing.Size(161, 29)
        Me.Physics_Bullet.TabIndex = 138
        Me.Physics_Bullet.TabStop = True
        Me.Physics_Bullet.Text = "Bullet physics "
        Me.Physics_Bullet.UseVisualStyleBackColor = True
        '
        'Physics_Default
        '
        Me.Physics_Default.AutoSize = True
        Me.Physics_Default.Location = New System.Drawing.Point(8, 30)
        Me.Physics_Default.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.Physics_Default.Name = "Physics_Default"
        Me.Physics_Default.Size = New System.Drawing.Size(138, 29)
        Me.Physics_Default.TabIndex = 137
        Me.Physics_Default.TabStop = True
        Me.Physics_Default.Text = Global.Outworldz.My.Resources.Resources.Use_Default_word
        Me.Physics_Default.UseVisualStyleBackColor = True
        '
        'Physics_Separate
        '
        Me.Physics_Separate.AutoSize = True
        Me.Physics_Separate.Location = New System.Drawing.Point(8, 148)
        Me.Physics_Separate.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.Physics_Separate.Name = "Physics_Separate"
        Me.Physics_Separate.Size = New System.Drawing.Size(317, 29)
        Me.Physics_Separate.TabIndex = 37
        Me.Physics_Separate.TabStop = True
        Me.Physics_Separate.Text = Global.Outworldz.My.Resources.Resources.BP
        Me.Physics_Separate.UseVisualStyleBackColor = True
        '
        'Physics_ubODE
        '
        Me.Physics_ubODE.AutoSize = True
        Me.Physics_ubODE.Location = New System.Drawing.Point(8, 69)
        Me.Physics_ubODE.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.Physics_ubODE.Name = "Physics_ubODE"
        Me.Physics_ubODE.Size = New System.Drawing.Size(272, 29)
        Me.Physics_ubODE.TabIndex = 35
        Me.Physics_ubODE.TabStop = True
        Me.Physics_ubODE.Text = Global.Outworldz.My.Resources.Resources.UBODE_words
        Me.Physics_ubODE.UseVisualStyleBackColor = True
        '
        'RegionPort
        '
        Me.RegionPort.Enabled = False
        Me.RegionPort.Location = New System.Drawing.Point(169, 69)
        Me.RegionPort.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.RegionPort.Name = "RegionPort"
        Me.RegionPort.Size = New System.Drawing.Size(67, 29)
        Me.RegionPort.TabIndex = 39
        '
        'Advanced
        '
        Me.Advanced.BackColor = System.Drawing.SystemColors.ControlLight
        Me.Advanced.Controls.Add(Me.Label16)
        Me.Advanced.Controls.Add(Me.Label15)
        Me.Advanced.Controls.Add(Me.FrametimeBox)
        Me.Advanced.Controls.Add(Me.Label14)
        Me.Advanced.Controls.Add(Me.ScriptTimerTextBox)
        Me.Advanced.Controls.Add(Me.RegionPort)
        Me.Advanced.Controls.Add(Me.ClampPrimSize)
        Me.Advanced.Controls.Add(Me.Label12)
        Me.Advanced.Controls.Add(Me.Label10)
        Me.Advanced.Controls.Add(Me.NonphysicalPrimMax)
        Me.Advanced.Controls.Add(Me.Label11)
        Me.Advanced.Controls.Add(Me.PhysicalPrimMax)
        Me.Advanced.Controls.Add(Me.Label6)
        Me.Advanced.Controls.Add(Me.Label9)
        Me.Advanced.Controls.Add(Me.MaxPrims)
        Me.Advanced.Controls.Add(Me.Label5)
        Me.Advanced.Controls.Add(Me.MaxAgents)
        Me.Advanced.Controls.Add(Me.Label4)
        Me.Advanced.Controls.Add(Me.Label1)
        Me.Advanced.Controls.Add(Me.UUID)
        Me.Advanced.Controls.Add(Me.CoordY)
        Me.Advanced.Controls.Add(Me.CoordX)
        Me.Advanced.Location = New System.Drawing.Point(27, 448)
        Me.Advanced.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.Advanced.Name = "Advanced"
        Me.Advanced.Padding = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.Advanced.Size = New System.Drawing.Size(534, 510)
        Me.Advanced.TabIndex = 26
        Me.Advanced.TabStop = False
        Me.Advanced.Text = "Regions"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(251, 76)
        Me.Label16.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(113, 25)
        Me.Label16.TabIndex = 44
        Me.Label16.Text = "Region Port"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(12, 97)
        Me.Label6.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(59, 25)
        Me.Label6.TabIndex = 22
        Me.Label6.Text = "UUID"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 28)
        Me.Label4.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(81, 25)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "Map  X:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(251, 28)
        Me.Label1.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(0, 25)
        Me.Label1.TabIndex = 17
        '
        'UUID
        '
        Me.UUID.Enabled = False
        Me.UUID.Location = New System.Drawing.Point(15, 125)
        Me.UUID.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.UUID.Name = "UUID"
        Me.UUID.Size = New System.Drawing.Size(374, 29)
        Me.UUID.TabIndex = 15
        '
        'NameTip
        '
        Me.NameTip.AutoSize = True
        Me.NameTip.Location = New System.Drawing.Point(37, 133)
        Me.NameTip.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.NameTip.Name = "NameTip"
        Me.NameTip.Size = New System.Drawing.Size(463, 25)
        Me.NameTip.TabIndex = 25
        Me.NameTip.Text = "Alpha-Numeric plus minus sign and space character"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.RadioButton16)
        Me.GroupBox2.Controls.Add(Me.RadioButton15)
        Me.GroupBox2.Controls.Add(Me.RadioButton14)
        Me.GroupBox2.Controls.Add(Me.RadioButton13)
        Me.GroupBox2.Controls.Add(Me.RadioButton12)
        Me.GroupBox2.Controls.Add(Me.RadioButton11)
        Me.GroupBox2.Controls.Add(Me.RadioButton10)
        Me.GroupBox2.Controls.Add(Me.RadioButton9)
        Me.GroupBox2.Controls.Add(Me.RadioButton8)
        Me.GroupBox2.Controls.Add(Me.RadioButton7)
        Me.GroupBox2.Controls.Add(Me.RadioButton6)
        Me.GroupBox2.Controls.Add(Me.RadioButton5)
        Me.GroupBox2.Controls.Add(Me.RadioButton4)
        Me.GroupBox2.Controls.Add(Me.RadioButton3)
        Me.GroupBox2.Controls.Add(Me.RadioButton2)
        Me.GroupBox2.Controls.Add(Me.RadioButton1)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Location = New System.Drawing.Point(27, 176)
        Me.GroupBox2.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Padding = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.GroupBox2.Size = New System.Drawing.Size(519, 198)
        Me.GroupBox2.TabIndex = 23
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Sim Size"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(259, 83)
        Me.Label3.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(0, 25)
        Me.Label3.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(259, 44)
        Me.Label2.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(0, 25)
        Me.Label2.TabIndex = 2
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(42, 385)
        Me.Button1.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(104, 41)
        Me.Button1.TabIndex = 10
        Me.Button1.Text = Global.Outworldz.My.Resources.Resources.Save_word
        Me.Button1.UseVisualStyleBackColor = True
        '
        'DeleteButton
        '
        Me.DeleteButton.Location = New System.Drawing.Point(414, 391)
        Me.DeleteButton.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.DeleteButton.Name = "DeleteButton"
        Me.DeleteButton.Size = New System.Drawing.Size(100, 41)
        Me.DeleteButton.TabIndex = 11
        Me.DeleteButton.Text = Global.Outworldz.My.Resources.Resources.Delete_word
        Me.DeleteButton.UseVisualStyleBackColor = True
        '
        'EnabledCheckBox
        '
        Me.EnabledCheckBox.AutoSize = True
        Me.EnabledCheckBox.Location = New System.Drawing.Point(27, 58)
        Me.EnabledCheckBox.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.EnabledCheckBox.Name = "EnabledCheckBox"
        Me.EnabledCheckBox.Size = New System.Drawing.Size(110, 29)
        Me.EnabledCheckBox.TabIndex = 2
        Me.EnabledCheckBox.Text = Global.Outworldz.My.Resources.Resources.Enabled_word
        Me.EnabledCheckBox.UseVisualStyleBackColor = True
        '
        'GroupBox6
        '
        Me.GroupBox6.BackColor = System.Drawing.SystemColors.ControlLight
        Me.GroupBox6.Controls.Add(Me.GroupBox8)
        Me.GroupBox6.Controls.Add(Me.GroupBox7)
        Me.GroupBox6.Controls.Add(Me.Label13)
        Me.GroupBox6.Controls.Add(Me.GroupBox3)
        Me.GroupBox6.Controls.Add(Me.GroupBox4)
        Me.GroupBox6.Controls.Add(Me.MapBox)
        Me.GroupBox6.Controls.Add(Me.GroupBox5)
        Me.GroupBox6.Controls.Add(Me.GroupBox1)
        Me.GroupBox6.Location = New System.Drawing.Point(554, 58)
        Me.GroupBox6.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Padding = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.GroupBox6.Size = New System.Drawing.Size(846, 899)
        Me.GroupBox6.TabIndex = 1879
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Region Specific Settings"
        '
        'GroupBox8
        '
        Me.GroupBox8.Controls.Add(Me.ScriptDefaultButton)
        Me.GroupBox8.Controls.Add(Me.XEngineButton)
        Me.GroupBox8.Controls.Add(Me.YEngineButton)
        Me.GroupBox8.Location = New System.Drawing.Point(35, 727)
        Me.GroupBox8.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.GroupBox8.Name = "GroupBox8"
        Me.GroupBox8.Padding = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.GroupBox8.Size = New System.Drawing.Size(413, 155)
        Me.GroupBox8.TabIndex = 1887
        Me.GroupBox8.TabStop = False
        Me.GroupBox8.Text = "Script Engine"
        '
        'ScriptDefaultButton
        '
        Me.ScriptDefaultButton.AutoSize = True
        Me.ScriptDefaultButton.Location = New System.Drawing.Point(26, 43)
        Me.ScriptDefaultButton.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.ScriptDefaultButton.Name = "ScriptDefaultButton"
        Me.ScriptDefaultButton.Size = New System.Drawing.Size(138, 29)
        Me.ScriptDefaultButton.TabIndex = 1858
        Me.ScriptDefaultButton.TabStop = True
        Me.ScriptDefaultButton.Text = Global.Outworldz.My.Resources.Resources.Use_Default_word
        Me.ScriptDefaultButton.UseVisualStyleBackColor = True
        '
        'XEngineButton
        '
        Me.XEngineButton.AutoSize = True
        Me.XEngineButton.Location = New System.Drawing.Point(22, 83)
        Me.XEngineButton.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.XEngineButton.Name = "XEngineButton"
        Me.XEngineButton.Size = New System.Drawing.Size(117, 29)
        Me.XEngineButton.TabIndex = 7
        Me.XEngineButton.TabStop = True
        Me.XEngineButton.Text = Global.Outworldz.My.Resources.Resources.XEngine_word
        Me.XEngineButton.UseVisualStyleBackColor = True
        '
        'YEngineButton
        '
        Me.YEngineButton.AutoSize = True
        Me.YEngineButton.Location = New System.Drawing.Point(26, 120)
        Me.YEngineButton.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.YEngineButton.Name = "YEngineButton"
        Me.YEngineButton.Size = New System.Drawing.Size(116, 29)
        Me.YEngineButton.TabIndex = 9
        Me.YEngineButton.TabStop = True
        Me.YEngineButton.Text = Global.Outworldz.My.Resources.Resources.YEngine_word
        Me.YEngineButton.UseVisualStyleBackColor = True
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.SkipAutoCheckBox)
        Me.GroupBox7.Controls.Add(Me.DisallowResidents)
        Me.GroupBox7.Controls.Add(Me.DisallowForeigners)
        Me.GroupBox7.Controls.Add(Me.DisableGBCheckBox)
        Me.GroupBox7.Controls.Add(Me.SmartStartCheckBox)
        Me.GroupBox7.Controls.Add(Me.TPCheckBox1)
        Me.GroupBox7.Controls.Add(Me.TidesCheckbox)
        Me.GroupBox7.Controls.Add(Me.BirdsCheckBox)
        Me.GroupBox7.Location = New System.Drawing.Point(446, 493)
        Me.GroupBox7.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Padding = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.GroupBox7.Size = New System.Drawing.Size(390, 344)
        Me.GroupBox7.TabIndex = 1881
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "Modules"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(55, 35)
        Me.Label13.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(223, 25)
        Me.Label13.TabIndex = 1884
        Me.Label13.Text = "Region Specific Settings"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Publish)
        Me.GroupBox3.Controls.Add(Me.NoPublish)
        Me.GroupBox3.Controls.Add(Me.PublishDefault)
        Me.GroupBox3.Location = New System.Drawing.Point(35, 77)
        Me.GroupBox3.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Padding = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.GroupBox3.Size = New System.Drawing.Size(405, 176)
        Me.GroupBox3.TabIndex = 1883
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Publicity"
        '
        'Publish
        '
        Me.Publish.AutoSize = True
        Me.Publish.Location = New System.Drawing.Point(20, 128)
        Me.Publish.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.Publish.Name = "Publish"
        Me.Publish.Size = New System.Drawing.Size(314, 29)
        Me.Publish.TabIndex = 1881
        Me.Publish.TabStop = True
        Me.Publish.Text = Global.Outworldz.My.Resources.Resources.Publish_Items
        Me.Publish.UseVisualStyleBackColor = True
        '
        'NoPublish
        '
        Me.NoPublish.AutoSize = True
        Me.NoPublish.Location = New System.Drawing.Point(20, 90)
        Me.NoPublish.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.NoPublish.Name = "NoPublish"
        Me.NoPublish.Size = New System.Drawing.Size(255, 29)
        Me.NoPublish.TabIndex = 1880
        Me.NoPublish.TabStop = True
        Me.NoPublish.Text = Global.Outworldz.My.Resources.Resources.No_Publish_Items
        Me.NoPublish.UseVisualStyleBackColor = True
        '
        'PublishDefault
        '
        Me.PublishDefault.AutoSize = True
        Me.PublishDefault.Location = New System.Drawing.Point(20, 48)
        Me.PublishDefault.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.PublishDefault.Name = "PublishDefault"
        Me.PublishDefault.Size = New System.Drawing.Size(138, 29)
        Me.PublishDefault.TabIndex = 1879
        Me.PublishDefault.TabStop = True
        Me.PublishDefault.Text = Global.Outworldz.My.Resources.Resources.Use_Default_word
        Me.PublishDefault.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.Gods_Use_Default)
        Me.GroupBox4.Controls.Add(Me.AllowGods)
        Me.GroupBox4.Controls.Add(Me.ManagerGod)
        Me.GroupBox4.Controls.Add(Me.RegionGod)
        Me.GroupBox4.Location = New System.Drawing.Point(35, 264)
        Me.GroupBox4.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Padding = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.GroupBox4.Size = New System.Drawing.Size(399, 212)
        Me.GroupBox4.TabIndex = 1882
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Permissions"
        '
        'Gods_Use_Default
        '
        Me.Gods_Use_Default.AutoSize = True
        Me.Gods_Use_Default.Location = New System.Drawing.Point(26, 48)
        Me.Gods_Use_Default.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.Gods_Use_Default.Name = "Gods_Use_Default"
        Me.Gods_Use_Default.Size = New System.Drawing.Size(139, 29)
        Me.Gods_Use_Default.TabIndex = 1859
        Me.Gods_Use_Default.Text = Global.Outworldz.My.Resources.Resources.Use_Default_word
        Me.Gods_Use_Default.UseVisualStyleBackColor = True
        '
        'MapBox
        '
        Me.MapBox.Controls.Add(Me.Maps_Use_Default)
        Me.MapBox.Controls.Add(Me.MapPicture)
        Me.MapBox.Controls.Add(Me.MapNone)
        Me.MapBox.Controls.Add(Me.MapSimple)
        Me.MapBox.Controls.Add(Me.MapBetter)
        Me.MapBox.Controls.Add(Me.MapBest)
        Me.MapBox.Controls.Add(Me.MapGood)
        Me.MapBox.Location = New System.Drawing.Point(446, 28)
        Me.MapBox.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.MapBox.Name = "MapBox"
        Me.MapBox.Padding = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.MapBox.Size = New System.Drawing.Size(390, 433)
        Me.MapBox.TabIndex = 1881
        Me.MapBox.TabStop = False
        Me.MapBox.Text = "Maps"
        '
        'Maps_Use_Default
        '
        Me.Maps_Use_Default.AutoSize = True
        Me.Maps_Use_Default.Location = New System.Drawing.Point(26, 41)
        Me.Maps_Use_Default.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.Maps_Use_Default.Name = "Maps_Use_Default"
        Me.Maps_Use_Default.Size = New System.Drawing.Size(138, 29)
        Me.Maps_Use_Default.TabIndex = 1858
        Me.Maps_Use_Default.TabStop = True
        Me.Maps_Use_Default.Text = Global.Outworldz.My.Resources.Resources.Use_Default_word
        Me.Maps_Use_Default.UseVisualStyleBackColor = True
        '
        'MapPicture
        '
        Me.MapPicture.InitialImage = CType(resources.GetObject("MapPicture.InitialImage"), System.Drawing.Image)
        Me.MapPicture.Location = New System.Drawing.Point(54, 259)
        Me.MapPicture.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.MapPicture.Name = "MapPicture"
        Me.MapPicture.Size = New System.Drawing.Size(198, 162)
        Me.MapPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.MapPicture.TabIndex = 138
        Me.MapPicture.TabStop = False
        '
        'MapNone
        '
        Me.MapNone.AutoSize = True
        Me.MapNone.Location = New System.Drawing.Point(22, 72)
        Me.MapNone.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.MapNone.Name = "MapNone"
        Me.MapNone.Size = New System.Drawing.Size(84, 29)
        Me.MapNone.TabIndex = 7
        Me.MapNone.TabStop = True
        Me.MapNone.Text = Global.Outworldz.My.Resources.Resources.None
        Me.MapNone.UseVisualStyleBackColor = True
        '
        'MapSimple
        '
        Me.MapSimple.AutoSize = True
        Me.MapSimple.Location = New System.Drawing.Point(22, 106)
        Me.MapSimple.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.MapSimple.Name = "MapSimple"
        Me.MapSimple.Size = New System.Drawing.Size(165, 29)
        Me.MapSimple.TabIndex = 8
        Me.MapSimple.TabStop = True
        Me.MapSimple.Text = Global.Outworldz.My.Resources.Resources.Simple_but_Fast_word
        Me.MapSimple.UseVisualStyleBackColor = True
        '
        'MapBetter
        '
        Me.MapBetter.AutoSize = True
        Me.MapBetter.Location = New System.Drawing.Point(26, 175)
        Me.MapBetter.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.MapBetter.Name = "MapBetter"
        Me.MapBetter.Size = New System.Drawing.Size(209, 29)
        Me.MapBetter.TabIndex = 10
        Me.MapBetter.TabStop = True
        Me.MapBetter.Text = Global.Outworldz.My.Resources.Resources.Better_Prims
        Me.MapBetter.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage
        Me.MapBetter.UseVisualStyleBackColor = True
        '
        'MapBest
        '
        Me.MapBest.AutoSize = True
        Me.MapBest.Location = New System.Drawing.Point(22, 212)
        Me.MapBest.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.MapBest.Name = "MapBest"
        Me.MapBest.Size = New System.Drawing.Size(314, 29)
        Me.MapBest.TabIndex = 11
        Me.MapBest.TabStop = True
        Me.MapBest.Text = Global.Outworldz.My.Resources.Resources.Best_Prims
        Me.MapBest.UseVisualStyleBackColor = True
        '
        'MapGood
        '
        Me.MapGood.AutoSize = True
        Me.MapGood.Location = New System.Drawing.Point(26, 140)
        Me.MapGood.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.MapGood.Name = "MapGood"
        Me.MapGood.Size = New System.Drawing.Size(177, 29)
        Me.MapGood.TabIndex = 9
        Me.MapGood.TabStop = True
        Me.MapGood.Text = Global.Outworldz.My.Resources.Resources.Good_Warp3D_word
        Me.MapGood.UseVisualStyleBackColor = True
        '
        'GroupBox5
        '
        Me.GroupBox5.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox5.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Padding = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.GroupBox5.Size = New System.Drawing.Size(350, 175)
        Me.GroupBox5.TabIndex = 1885
        Me.GroupBox5.TabStop = False
        '
        'DeregisterButton
        '
        Me.DeregisterButton.Location = New System.Drawing.Point(212, 385)
        Me.DeregisterButton.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.DeregisterButton.Name = "DeregisterButton"
        Me.DeregisterButton.Size = New System.Drawing.Size(132, 41)
        Me.DeregisterButton.TabIndex = 1880
        Me.DeregisterButton.Text = Global.Outworldz.My.Resources.Resources.Deregister_word
        Me.DeregisterButton.UseVisualStyleBackColor = True
        '
        'MenuStrip2
        '
        Me.MenuStrip2.GripMargin = New System.Windows.Forms.Padding(2, 2, 0, 2)
        Me.MenuStrip2.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.MenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem30})
        Me.MenuStrip2.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip2.Name = "MenuStrip2"
        Me.MenuStrip2.Padding = New System.Windows.Forms.Padding(7, 1, 0, 1)
        Me.MenuStrip2.Size = New System.Drawing.Size(1416, 36)
        Me.MenuStrip2.TabIndex = 1888
        Me.MenuStrip2.Text = "0"
        '
        'ToolStripMenuItem30
        '
        Me.ToolStripMenuItem30.Image = Global.Outworldz.My.Resources.Resources.about
        Me.ToolStripMenuItem30.Name = "ToolStripMenuItem30"
        Me.ToolStripMenuItem30.Size = New System.Drawing.Size(98, 34)
        Me.ToolStripMenuItem30.Text = Global.Outworldz.My.Resources.Resources.Help_word
        '
        'FormRegion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(168.0!, 168.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(1416, 969)
        Me.Controls.Add(Me.MenuStrip2)
        Me.Controls.Add(Me.DeregisterButton)
        Me.Controls.Add(Me.GroupBox6)
        Me.Controls.Add(Me.EnabledCheckBox)
        Me.Controls.Add(Me.DeleteButton)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Advanced)
        Me.Controls.Add(Me.RegionName)
        Me.Controls.Add(Me.NameTip)
        Me.Controls.Add(Me.GroupBox2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.MaximizeBox = False
        Me.Name = "FormRegion"
        Me.Text = "Regions"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Advanced.ResumeLayout(False)
        Me.Advanced.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.GroupBox8.ResumeLayout(False)
        Me.GroupBox8.PerformLayout()
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.MapBox.ResumeLayout(False)
        Me.MapBox.PerformLayout()
        CType(Me.MapPicture, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip2.ResumeLayout(False)
        Me.MenuStrip2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents Advanced As GroupBox
    Friend WithEvents Label6 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents UUID As TextBox
    Friend WithEvents CoordY As TextBox
    Friend WithEvents CoordX As TextBox
    Friend WithEvents RegionName As TextBox
    Friend WithEvents NameTip As Label
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents RadioButton4 As RadioButton
    Friend WithEvents RadioButton3 As RadioButton
    Friend WithEvents RadioButton2 As RadioButton
    Friend WithEvents RadioButton1 As RadioButton
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents DeleteButton As Button
    Friend WithEvents EnabledCheckBox As CheckBox
    Friend WithEvents MaxAgents As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents PhysicalPrimMax As TextBox
    Friend WithEvents Label12 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents NonphysicalPrimMax As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents MaxPrims As TextBox
    Friend WithEvents ClampPrimSize As CheckBox
    Friend WithEvents GroupBox6 As GroupBox
    Friend WithEvents Label13 As Label
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents NoPublish As RadioButton
    Friend WithEvents PublishDefault As RadioButton
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents Gods_Use_Default As CheckBox
    Friend WithEvents AllowGods As CheckBox
    Friend WithEvents ManagerGod As CheckBox
    Friend WithEvents RegionGod As CheckBox
    Friend WithEvents MapBox As GroupBox
    Friend WithEvents Maps_Use_Default As RadioButton
    Friend WithEvents MapPicture As PictureBox
    Friend WithEvents MapNone As RadioButton
    Friend WithEvents MapSimple As RadioButton
    Friend WithEvents MapBetter As RadioButton
    Friend WithEvents MapBest As RadioButton
    Friend WithEvents MapGood As RadioButton
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents Publish As RadioButton
    Friend WithEvents GroupBox7 As GroupBox
    Friend WithEvents BirdsCheckBox As CheckBox
    Friend WithEvents TidesCheckbox As CheckBox
    Friend WithEvents TPCheckBox1 As CheckBox
    Friend WithEvents DeregisterButton As Button
    Friend WithEvents RegionPort As TextBox
    Friend WithEvents SmartStartCheckBox As CheckBox
    Friend WithEvents MenuStrip2 As MenuStrip
    Friend WithEvents ToolStripMenuItem30 As ToolStripMenuItem
    Friend WithEvents Label14 As Label
    Friend WithEvents ScriptTimerTextBox As TextBox
    Friend WithEvents DisableGBCheckBox As CheckBox
    Friend WithEvents DisallowForeigners As CheckBox
    Friend WithEvents DisallowResidents As CheckBox
    Friend WithEvents Label15 As Label
    Friend WithEvents FrametimeBox As TextBox
    Friend WithEvents Label16 As Label
    Friend WithEvents SkipAutoCheckBox As CheckBox
    Friend WithEvents RadioButton8 As RadioButton
    Friend WithEvents RadioButton7 As RadioButton
    Friend WithEvents RadioButton6 As RadioButton
    Friend WithEvents RadioButton5 As RadioButton
    Friend WithEvents RadioButton16 As RadioButton
    Friend WithEvents RadioButton15 As RadioButton
    Friend WithEvents RadioButton14 As RadioButton
    Friend WithEvents RadioButton13 As RadioButton
    Friend WithEvents RadioButton12 As RadioButton
    Friend WithEvents RadioButton11 As RadioButton
    Friend WithEvents RadioButton10 As RadioButton
    Friend WithEvents RadioButton9 As RadioButton
    Friend WithEvents GroupBox8 As GroupBox
    Friend WithEvents ScriptDefaultButton As RadioButton
    Friend WithEvents XEngineButton As RadioButton
    Friend WithEvents YEngineButton As RadioButton
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Physics_Default As RadioButton
    Friend WithEvents Physics_Separate As RadioButton
    Friend WithEvents Physics_ubODE As RadioButton
    Friend WithEvents Physics_Hybrid As RadioButton
    Friend WithEvents Physics_Bullet As RadioButton
End Class
