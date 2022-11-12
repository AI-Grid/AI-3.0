<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
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
        Me.NonPhysPrimLabel = New System.Windows.Forms.Label()
        Me.PhysPrimLabel = New System.Windows.Forms.Label()
        Me.PhysicalPrimMax = New System.Windows.Forms.TextBox()
        Me.MaxNPrimsLabel = New System.Windows.Forms.Label()
        Me.MaxPrims = New System.Windows.Forms.TextBox()
        Me.NonphysicalPrimMax = New System.Windows.Forms.TextBox()
        Me.MaxMAvatarsLabel = New System.Windows.Forms.Label()
        Me.BirdsCheckBox = New System.Windows.Forms.CheckBox()
        Me.TidesCheckbox = New System.Windows.Forms.CheckBox()
        Me.TPCheckBox1 = New System.Windows.Forms.CheckBox()
        Me.GodLevel = New System.Windows.Forms.CheckBox()
        Me.GodManager = New System.Windows.Forms.CheckBox()
        Me.GodEstate = New System.Windows.Forms.CheckBox()
        Me.SmartStartCheckBox = New System.Windows.Forms.CheckBox()
        Me.DisableGBCheckBox = New System.Windows.Forms.CheckBox()
        Me.DisallowForeigners = New System.Windows.Forms.CheckBox()
        Me.DisallowResidents = New System.Windows.Forms.CheckBox()
        Me.SkipAutoCheckBox = New System.Windows.Forms.CheckBox()
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
        Me.FrameRateLabel = New System.Windows.Forms.Label()
        Me.FrametimeBox = New System.Windows.Forms.TextBox()
        Me.ScriptRateLabel = New System.Windows.Forms.Label()
        Me.ScriptTimerTextBox = New System.Windows.Forms.TextBox()
        Me.ClampPrimSize = New System.Windows.Forms.CheckBox()
        Me.ClampPrimLabel = New System.Windows.Forms.Label()
        Me.RegionPort = New System.Windows.Forms.TextBox()
        Me.RegionsGroupbox = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.YLabel = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.XLabel = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.UUID = New System.Windows.Forms.TextBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.DeleteButton = New System.Windows.Forms.Button()
        Me.EnabledCheckBox = New System.Windows.Forms.CheckBox()
        Me.ScriptsGroupbox = New System.Windows.Forms.GroupBox()
        Me.ScriptOffButton = New System.Windows.Forms.RadioButton()
        Me.ScriptDefaultButton = New System.Windows.Forms.RadioButton()
        Me.XEngineButton = New System.Windows.Forms.RadioButton()
        Me.YEngineButton = New System.Windows.Forms.RadioButton()
        Me.ModulesGroupBox = New System.Windows.Forms.GroupBox()
        Me.ConciergeCheckBox = New System.Windows.Forms.CheckBox()
        Me.RichTextBoxModules = New System.Windows.Forms.RichTextBox()
        Me.PublicityGroupBox = New System.Windows.Forms.GroupBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Hyperica = New System.Windows.Forms.LinkLabel()
        Me.APIKey = New System.Windows.Forms.TextBox()
        Me.Publish = New System.Windows.Forms.RadioButton()
        Me.Opensimworld = New System.Windows.Forms.LinkLabel()
        Me.NoPublish = New System.Windows.Forms.RadioButton()
        Me.PublishDefault = New System.Windows.Forms.RadioButton()
        Me.PermissionsGroupbox = New System.Windows.Forms.GroupBox()
        Me.Gods_Use_Default = New System.Windows.Forms.CheckBox()
        Me.RichTextBoxPermissions = New System.Windows.Forms.RichTextBox()
        Me.MapGroupBox = New System.Windows.Forms.GroupBox()
        Me.LandingSpotLabel = New System.Windows.Forms.Label()
        Me.LandingSpotTextBox = New System.Windows.Forms.TextBox()
        Me.Maps_Use_Default = New System.Windows.Forms.RadioButton()
        Me.MapPicture = New System.Windows.Forms.PictureBox()
        Me.MapNone = New System.Windows.Forms.RadioButton()
        Me.MapSimple = New System.Windows.Forms.RadioButton()
        Me.MapBetter = New System.Windows.Forms.RadioButton()
        Me.MapBest = New System.Windows.Forms.RadioButton()
        Me.MapGood = New System.Windows.Forms.RadioButton()
        Me.RichTextBoxMap = New System.Windows.Forms.RichTextBox()
        Me.DeregisterButton = New System.Windows.Forms.Button()
        Me.MenuStrip2 = New System.Windows.Forms.MenuStrip()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BasicsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MapsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PhysicsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ScriptsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PermissionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PublicityToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ModulesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CPUTab = New System.Windows.Forms.TabControl()
        Me.Basics = New System.Windows.Forms.TabPage()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Options = New System.Windows.Forms.TabPage()
        Me.RichTextBoxOptions = New System.Windows.Forms.RichTextBox()
        Me.Maps = New System.Windows.Forms.TabPage()
        Me.Physics = New System.Windows.Forms.TabPage()
        Me.RichTextBoxPhysics = New System.Windows.Forms.RichTextBox()
        Me.PhysicsGroupbox = New System.Windows.Forms.GroupBox()
        Me.Physics_Hybrid = New System.Windows.Forms.RadioButton()
        Me.Physics_Bullet = New System.Windows.Forms.RadioButton()
        Me.Physics_Default = New System.Windows.Forms.RadioButton()
        Me.Physics_Separate = New System.Windows.Forms.RadioButton()
        Me.Physics_ubODE = New System.Windows.Forms.RadioButton()
        Me.Scripts = New System.Windows.Forms.TabPage()
        Me.RichTextBoxScripts = New System.Windows.Forms.RichTextBox()
        Me.Permissions = New System.Windows.Forms.TabPage()
        Me.Publicity = New System.Windows.Forms.TabPage()
        Me.RichTextBoxPublicity = New System.Windows.Forms.RichTextBox()
        Me.Modules = New System.Windows.Forms.TabPage()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.BelowNormal = New System.Windows.Forms.RadioButton()
        Me.Normal = New System.Windows.Forms.RadioButton()
        Me.AboveNormal = New System.Windows.Forms.RadioButton()
        Me.High = New System.Windows.Forms.RadioButton()
        Me.RealTime = New System.Windows.Forms.RadioButton()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Core16Button = New System.Windows.Forms.CheckBox()
        Me.Core6Button = New System.Windows.Forms.CheckBox()
        Me.Core15Button = New System.Windows.Forms.CheckBox()
        Me.Core14Button = New System.Windows.Forms.CheckBox()
        Me.Core13Button = New System.Windows.Forms.CheckBox()
        Me.Core12Button = New System.Windows.Forms.CheckBox()
        Me.Core11Button = New System.Windows.Forms.CheckBox()
        Me.Core10Button = New System.Windows.Forms.CheckBox()
        Me.Core9Button = New System.Windows.Forms.CheckBox()
        Me.Core8Button = New System.Windows.Forms.CheckBox()
        Me.Core7Button = New System.Windows.Forms.CheckBox()
        Me.Core5Button = New System.Windows.Forms.CheckBox()
        Me.Core4Button = New System.Windows.Forms.CheckBox()
        Me.Core3Button = New System.Windows.Forms.CheckBox()
        Me.Core2Button = New System.Windows.Forms.CheckBox()
        Me.Core1Button = New System.Windows.Forms.CheckBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.HelpProvider1 = New System.Windows.Forms.HelpProvider()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.SearchLabel = New System.Windows.Forms.Label()
        Me.RegionsGroupbox.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.ScriptsGroupbox.SuspendLayout()
        Me.ModulesGroupBox.SuspendLayout()
        Me.PublicityGroupBox.SuspendLayout()
        Me.PermissionsGroupbox.SuspendLayout()
        Me.MapGroupBox.SuspendLayout()
        CType(Me.MapPicture, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip2.SuspendLayout()
        Me.CPUTab.SuspendLayout()
        Me.Basics.SuspendLayout()
        Me.Options.SuspendLayout()
        Me.Maps.SuspendLayout()
        Me.Physics.SuspendLayout()
        Me.PhysicsGroupbox.SuspendLayout()
        Me.Scripts.SuspendLayout()
        Me.Permissions.SuspendLayout()
        Me.Publicity.SuspendLayout()
        Me.Modules.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'CoordY
        '
        Me.CoordY.Location = New System.Drawing.Point(78, 22)
        Me.CoordY.Name = "CoordY"
        Me.CoordY.Size = New System.Drawing.Size(43, 20)
        Me.CoordY.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.CoordY, Global.Outworldz.My.Resources.Resources.CoordY)
        '
        'CoordX
        '
        Me.CoordX.Location = New System.Drawing.Point(12, 22)
        Me.CoordX.Name = "CoordX"
        Me.CoordX.Size = New System.Drawing.Size(40, 20)
        Me.CoordX.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.CoordX, Global.Outworldz.My.Resources.Resources.CoordX)
        '
        'RegionName
        '
        Me.RegionName.Location = New System.Drawing.Point(16, 28)
        Me.RegionName.Name = "RegionName"
        Me.RegionName.Size = New System.Drawing.Size(219, 20)
        Me.RegionName.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.RegionName, Global.Outworldz.My.Resources.Resources.Region_Name)
        '
        'RadioButton4
        '
        Me.RadioButton4.AutoSize = True
        Me.RadioButton4.Location = New System.Drawing.Point(20, 85)
        Me.RadioButton4.Name = "RadioButton4"
        Me.RadioButton4.Size = New System.Drawing.Size(50, 17)
        Me.RadioButton4.TabIndex = 3
        Me.RadioButton4.TabStop = True
        Me.RadioButton4.Text = "4 X 4"
        Me.ToolTip1.SetToolTip(Me.RadioButton4, "1024 X 1024")
        Me.RadioButton4.UseVisualStyleBackColor = True
        '
        'RadioButton3
        '
        Me.RadioButton3.AutoSize = True
        Me.RadioButton3.Location = New System.Drawing.Point(20, 63)
        Me.RadioButton3.Name = "RadioButton3"
        Me.RadioButton3.Size = New System.Drawing.Size(50, 17)
        Me.RadioButton3.TabIndex = 2
        Me.RadioButton3.TabStop = True
        Me.RadioButton3.Text = "3 X 3"
        Me.ToolTip1.SetToolTip(Me.RadioButton3, "768 X 768")
        Me.RadioButton3.UseVisualStyleBackColor = True
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Location = New System.Drawing.Point(20, 39)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(50, 17)
        Me.RadioButton2.TabIndex = 1
        Me.RadioButton2.TabStop = True
        Me.RadioButton2.Text = "2 X 2"
        Me.ToolTip1.SetToolTip(Me.RadioButton2, "512 X 512")
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Location = New System.Drawing.Point(20, 16)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(50, 17)
        Me.RadioButton1.TabIndex = 0
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "1 X 1"
        Me.ToolTip1.SetToolTip(Me.RadioButton1, "256 X 256")
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'MaxAgents
        '
        Me.MaxAgents.Location = New System.Drawing.Point(12, 130)
        Me.MaxAgents.Name = "MaxAgents"
        Me.MaxAgents.Size = New System.Drawing.Size(40, 20)
        Me.MaxAgents.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.MaxAgents, Global.Outworldz.My.Resources.Resources.Max_Agents)
        '
        'NonPhysPrimLabel
        '
        Me.NonPhysPrimLabel.AutoSize = True
        Me.NonPhysPrimLabel.Location = New System.Drawing.Point(65, 57)
        Me.NonPhysPrimLabel.Name = "NonPhysPrimLabel"
        Me.NonPhysPrimLabel.Size = New System.Drawing.Size(111, 13)
        Me.NonPhysPrimLabel.TabIndex = 9
        Me.NonPhysPrimLabel.Text = "Nonphysical Prim Size"
        Me.ToolTip1.SetToolTip(Me.NonPhysPrimLabel, Global.Outworldz.My.Resources.Resources.Max_NonPhys)
        '
        'PhysPrimLabel
        '
        Me.PhysPrimLabel.AutoSize = True
        Me.PhysPrimLabel.Location = New System.Drawing.Point(64, 82)
        Me.PhysPrimLabel.Name = "PhysPrimLabel"
        Me.PhysPrimLabel.Size = New System.Drawing.Size(115, 13)
        Me.PhysPrimLabel.TabIndex = 11
        Me.PhysPrimLabel.Text = "Physical Prim Max Size"
        Me.ToolTip1.SetToolTip(Me.PhysPrimLabel, Global.Outworldz.My.Resources.Resources.Max_Phys)
        '
        'PhysicalPrimMax
        '
        Me.PhysicalPrimMax.Location = New System.Drawing.Point(11, 78)
        Me.PhysicalPrimMax.Name = "PhysicalPrimMax"
        Me.PhysicalPrimMax.Size = New System.Drawing.Size(40, 20)
        Me.PhysicalPrimMax.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.PhysicalPrimMax, Global.Outworldz.My.Resources.Resources.Max_Phys)
        '
        'MaxNPrimsLabel
        '
        Me.MaxNPrimsLabel.AutoSize = True
        Me.MaxNPrimsLabel.Location = New System.Drawing.Point(65, 108)
        Me.MaxNPrimsLabel.Name = "MaxNPrimsLabel"
        Me.MaxNPrimsLabel.Size = New System.Drawing.Size(160, 13)
        Me.MaxNPrimsLabel.TabIndex = 15
        Me.MaxNPrimsLabel.Text = "Max Number of Prims in a Parcel"
        Me.ToolTip1.SetToolTip(Me.MaxNPrimsLabel, Global.Outworldz.My.Resources.Resources.Viewer_Stops_Counting)
        '
        'MaxPrims
        '
        Me.MaxPrims.Location = New System.Drawing.Point(12, 104)
        Me.MaxPrims.Name = "MaxPrims"
        Me.MaxPrims.Size = New System.Drawing.Size(40, 20)
        Me.MaxPrims.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.MaxPrims, Global.Outworldz.My.Resources.Resources.Not_Normal)
        '
        'NonphysicalPrimMax
        '
        Me.NonphysicalPrimMax.Location = New System.Drawing.Point(12, 53)
        Me.NonphysicalPrimMax.Name = "NonphysicalPrimMax"
        Me.NonphysicalPrimMax.Size = New System.Drawing.Size(40, 20)
        Me.NonphysicalPrimMax.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.NonphysicalPrimMax, Global.Outworldz.My.Resources.Resources.Normal_Prim)
        '
        'MaxMAvatarsLabel
        '
        Me.MaxMAvatarsLabel.AutoSize = True
        Me.MaxMAvatarsLabel.Location = New System.Drawing.Point(65, 134)
        Me.MaxMAvatarsLabel.Name = "MaxMAvatarsLabel"
        Me.MaxMAvatarsLabel.Size = New System.Drawing.Size(155, 13)
        Me.MaxMAvatarsLabel.TabIndex = 17
        Me.MaxMAvatarsLabel.Text = "Max number of Avatars + NPCs"
        Me.ToolTip1.SetToolTip(Me.MaxMAvatarsLabel, Global.Outworldz.My.Resources.Resources.Max_Agents)
        '
        'BirdsCheckBox
        '
        Me.BirdsCheckBox.AutoSize = True
        Me.BirdsCheckBox.Location = New System.Drawing.Point(15, 20)
        Me.BirdsCheckBox.Name = "BirdsCheckBox"
        Me.BirdsCheckBox.Size = New System.Drawing.Size(82, 17)
        Me.BirdsCheckBox.TabIndex = 0
        Me.BirdsCheckBox.Text = Global.Outworldz.My.Resources.Resources.Bird_Module_word
        Me.ToolTip1.SetToolTip(Me.BirdsCheckBox, Global.Outworldz.My.Resources.Resources.GBoids)
        Me.BirdsCheckBox.UseVisualStyleBackColor = True
        '
        'TidesCheckbox
        '
        Me.TidesCheckbox.AutoSize = True
        Me.TidesCheckbox.Location = New System.Drawing.Point(15, 40)
        Me.TidesCheckbox.Name = "TidesCheckbox"
        Me.TidesCheckbox.Size = New System.Drawing.Size(194, 17)
        Me.TidesCheckbox.TabIndex = 1
        Me.TidesCheckbox.Text = Global.Outworldz.My.Resources.Resources.Tide_Enable
        Me.ToolTip1.SetToolTip(Me.TidesCheckbox, Global.Outworldz.My.Resources.Resources.GTide)
        Me.TidesCheckbox.UseVisualStyleBackColor = True
        '
        'TPCheckBox1
        '
        Me.TPCheckBox1.AutoSize = True
        Me.TPCheckBox1.Location = New System.Drawing.Point(15, 60)
        Me.TPCheckBox1.Name = "TPCheckBox1"
        Me.TPCheckBox1.Size = New System.Drawing.Size(125, 17)
        Me.TPCheckBox1.TabIndex = 2
        Me.TPCheckBox1.Text = Global.Outworldz.My.Resources.Resources.Teleporter_Enable_word
        Me.ToolTip1.SetToolTip(Me.TPCheckBox1, Global.Outworldz.My.Resources.Resources.Teleport_Tooltip)
        Me.TPCheckBox1.UseVisualStyleBackColor = True
        '
        'GodLevel
        '
        Me.GodLevel.AutoSize = True
        Me.GodLevel.Location = New System.Drawing.Point(15, 52)
        Me.GodLevel.Name = "GodLevel"
        Me.GodLevel.Size = New System.Drawing.Size(138, 17)
        Me.GodLevel.TabIndex = 1
        Me.GodLevel.Text = Global.Outworldz.My.Resources.Resources.Allow_Or_Disallow_Gods_word
        Me.ToolTip1.SetToolTip(Me.GodLevel, Global.Outworldz.My.Resources.Resources.AllowGodsTooltip)
        Me.GodLevel.UseVisualStyleBackColor = True
        '
        'GodManager
        '
        Me.GodManager.AutoSize = True
        Me.GodManager.Location = New System.Drawing.Point(15, 99)
        Me.GodManager.Name = "GodManager"
        Me.GodManager.Size = New System.Drawing.Size(132, 17)
        Me.GodManager.TabIndex = 3
        Me.GodManager.Text = Global.Outworldz.My.Resources.Resources.EstateManagerIsGod_word
        Me.ToolTip1.SetToolTip(Me.GodManager, Global.Outworldz.My.Resources.Resources.EMGod)
        Me.GodManager.UseVisualStyleBackColor = True
        '
        'GodEstate
        '
        Me.GodEstate.AutoSize = True
        Me.GodEstate.Location = New System.Drawing.Point(15, 75)
        Me.GodEstate.Name = "GodEstate"
        Me.GodEstate.Size = New System.Drawing.Size(127, 17)
        Me.GodEstate.TabIndex = 2
        Me.GodEstate.Text = Global.Outworldz.My.Resources.Resources.Region_Owner_Is_God_word
        Me.ToolTip1.SetToolTip(Me.GodEstate, Global.Outworldz.My.Resources.Resources.Region_Owner_Is_God_word)
        Me.GodEstate.UseVisualStyleBackColor = True
        '
        'SmartStartCheckBox
        '
        Me.SmartStartCheckBox.AutoSize = True
        Me.SmartStartCheckBox.Location = New System.Drawing.Point(16, 78)
        Me.SmartStartCheckBox.Name = "SmartStartCheckBox"
        Me.SmartStartCheckBox.Size = New System.Drawing.Size(78, 17)
        Me.SmartStartCheckBox.TabIndex = 2
        Me.SmartStartCheckBox.Text = Global.Outworldz.My.Resources.Resources.Smart_Start_word
        Me.ToolTip1.SetToolTip(Me.SmartStartCheckBox, Global.Outworldz.My.Resources.Resources.GTide)
        Me.SmartStartCheckBox.UseVisualStyleBackColor = True
        '
        'DisableGBCheckBox
        '
        Me.DisableGBCheckBox.AutoSize = True
        Me.DisableGBCheckBox.Location = New System.Drawing.Point(15, 80)
        Me.DisableGBCheckBox.Name = "DisableGBCheckBox"
        Me.DisableGBCheckBox.Size = New System.Drawing.Size(111, 17)
        Me.DisableGBCheckBox.TabIndex = 3
        Me.DisableGBCheckBox.Text = Global.Outworldz.My.Resources.Resources.Disable_Gloebits_word
        Me.ToolTip1.SetToolTip(Me.DisableGBCheckBox, Global.Outworldz.My.Resources.Resources.Disable_Gloebits_text)
        Me.DisableGBCheckBox.UseVisualStyleBackColor = True
        '
        'DisallowForeigners
        '
        Me.DisallowForeigners.AutoSize = True
        Me.DisallowForeigners.Location = New System.Drawing.Point(15, 100)
        Me.DisallowForeigners.Name = "DisallowForeigners"
        Me.DisallowForeigners.Size = New System.Drawing.Size(135, 17)
        Me.DisallowForeigners.TabIndex = 4
        Me.DisallowForeigners.Text = Global.Outworldz.My.Resources.Resources.Disable_Foreigners_word
        Me.ToolTip1.SetToolTip(Me.DisallowForeigners, Global.Outworldz.My.Resources.Resources.No_HG)
        Me.DisallowForeigners.UseVisualStyleBackColor = True
        '
        'DisallowResidents
        '
        Me.DisallowResidents.AutoSize = True
        Me.DisallowResidents.Location = New System.Drawing.Point(15, 120)
        Me.DisallowResidents.Name = "DisallowResidents"
        Me.DisallowResidents.Size = New System.Drawing.Size(128, 17)
        Me.DisallowResidents.TabIndex = 5
        Me.DisallowResidents.Text = Global.Outworldz.My.Resources.Resources.Disable_Residents
        Me.ToolTip1.SetToolTip(Me.DisallowResidents, Global.Outworldz.My.Resources.Resources.Only_Owners)
        Me.DisallowResidents.UseVisualStyleBackColor = True
        '
        'SkipAutoCheckBox
        '
        Me.SkipAutoCheckBox.AutoSize = True
        Me.SkipAutoCheckBox.Location = New System.Drawing.Point(15, 140)
        Me.SkipAutoCheckBox.Name = "SkipAutoCheckBox"
        Me.SkipAutoCheckBox.Size = New System.Drawing.Size(162, 17)
        Me.SkipAutoCheckBox.TabIndex = 6
        Me.SkipAutoCheckBox.Text = Global.Outworldz.My.Resources.Resources.Skip_Autobackup_word
        Me.ToolTip1.SetToolTip(Me.SkipAutoCheckBox, Global.Outworldz.My.Resources.Resources.WillNotSave)
        Me.SkipAutoCheckBox.UseVisualStyleBackColor = True
        '
        'RadioButton8
        '
        Me.RadioButton8.AutoSize = True
        Me.RadioButton8.Location = New System.Drawing.Point(20, 176)
        Me.RadioButton8.Name = "RadioButton8"
        Me.RadioButton8.Size = New System.Drawing.Size(50, 17)
        Me.RadioButton8.TabIndex = 7
        Me.RadioButton8.TabStop = True
        Me.RadioButton8.Text = "8 X 8"
        Me.ToolTip1.SetToolTip(Me.RadioButton8, "2,048 X 2,048")
        Me.RadioButton8.UseVisualStyleBackColor = True
        '
        'RadioButton7
        '
        Me.RadioButton7.AutoSize = True
        Me.RadioButton7.Location = New System.Drawing.Point(20, 153)
        Me.RadioButton7.Name = "RadioButton7"
        Me.RadioButton7.Size = New System.Drawing.Size(50, 17)
        Me.RadioButton7.TabIndex = 6
        Me.RadioButton7.TabStop = True
        Me.RadioButton7.Text = "7 X 7"
        Me.ToolTip1.SetToolTip(Me.RadioButton7, "1792 X 1792")
        Me.RadioButton7.UseVisualStyleBackColor = True
        '
        'RadioButton6
        '
        Me.RadioButton6.AutoSize = True
        Me.RadioButton6.Location = New System.Drawing.Point(20, 130)
        Me.RadioButton6.Name = "RadioButton6"
        Me.RadioButton6.Size = New System.Drawing.Size(50, 17)
        Me.RadioButton6.TabIndex = 5
        Me.RadioButton6.TabStop = True
        Me.RadioButton6.Text = "6 X 6"
        Me.ToolTip1.SetToolTip(Me.RadioButton6, "1536 X 1536")
        Me.RadioButton6.UseVisualStyleBackColor = True
        '
        'RadioButton5
        '
        Me.RadioButton5.AutoSize = True
        Me.RadioButton5.Location = New System.Drawing.Point(20, 108)
        Me.RadioButton5.Name = "RadioButton5"
        Me.RadioButton5.Size = New System.Drawing.Size(50, 17)
        Me.RadioButton5.TabIndex = 4
        Me.RadioButton5.TabStop = True
        Me.RadioButton5.Text = "5 X 5"
        Me.ToolTip1.SetToolTip(Me.RadioButton5, "1280 X 1280")
        Me.RadioButton5.UseVisualStyleBackColor = True
        '
        'RadioButton10
        '
        Me.RadioButton10.AutoSize = True
        Me.RadioButton10.Location = New System.Drawing.Point(92, 38)
        Me.RadioButton10.Name = "RadioButton10"
        Me.RadioButton10.Size = New System.Drawing.Size(62, 17)
        Me.RadioButton10.TabIndex = 9
        Me.RadioButton10.TabStop = True
        Me.RadioButton10.Text = "10 X 10"
        Me.ToolTip1.SetToolTip(Me.RadioButton10, "2560 X 2560")
        Me.RadioButton10.UseVisualStyleBackColor = True
        '
        'RadioButton9
        '
        Me.RadioButton9.AutoSize = True
        Me.RadioButton9.Location = New System.Drawing.Point(92, 16)
        Me.RadioButton9.Name = "RadioButton9"
        Me.RadioButton9.Size = New System.Drawing.Size(50, 17)
        Me.RadioButton9.TabIndex = 8
        Me.RadioButton9.TabStop = True
        Me.RadioButton9.Text = "9 X 9"
        Me.ToolTip1.SetToolTip(Me.RadioButton9, "2304 X 2304")
        Me.RadioButton9.UseVisualStyleBackColor = True
        '
        'RadioButton12
        '
        Me.RadioButton12.AutoSize = True
        Me.RadioButton12.Location = New System.Drawing.Point(94, 84)
        Me.RadioButton12.Name = "RadioButton12"
        Me.RadioButton12.Size = New System.Drawing.Size(62, 17)
        Me.RadioButton12.TabIndex = 11
        Me.RadioButton12.TabStop = True
        Me.RadioButton12.Text = "12 X 12"
        Me.ToolTip1.SetToolTip(Me.RadioButton12, "3072 X 3072")
        Me.RadioButton12.UseVisualStyleBackColor = True
        '
        'RadioButton11
        '
        Me.RadioButton11.AutoSize = True
        Me.RadioButton11.Location = New System.Drawing.Point(94, 61)
        Me.RadioButton11.Name = "RadioButton11"
        Me.RadioButton11.Size = New System.Drawing.Size(62, 17)
        Me.RadioButton11.TabIndex = 10
        Me.RadioButton11.TabStop = True
        Me.RadioButton11.Text = "11 X 11"
        Me.ToolTip1.SetToolTip(Me.RadioButton11, "2816 X 2816")
        Me.RadioButton11.UseVisualStyleBackColor = True
        '
        'RadioButton16
        '
        Me.RadioButton16.AutoSize = True
        Me.RadioButton16.Location = New System.Drawing.Point(94, 176)
        Me.RadioButton16.Name = "RadioButton16"
        Me.RadioButton16.Size = New System.Drawing.Size(62, 17)
        Me.RadioButton16.TabIndex = 15
        Me.RadioButton16.TabStop = True
        Me.RadioButton16.Text = "16 X 16"
        Me.ToolTip1.SetToolTip(Me.RadioButton16, "4096 X 4096")
        Me.RadioButton16.UseVisualStyleBackColor = True
        '
        'RadioButton15
        '
        Me.RadioButton15.AutoSize = True
        Me.RadioButton15.Location = New System.Drawing.Point(94, 153)
        Me.RadioButton15.Name = "RadioButton15"
        Me.RadioButton15.Size = New System.Drawing.Size(62, 17)
        Me.RadioButton15.TabIndex = 14
        Me.RadioButton15.TabStop = True
        Me.RadioButton15.Text = "15 X 15"
        Me.ToolTip1.SetToolTip(Me.RadioButton15, "3840 X 3840")
        Me.RadioButton15.UseVisualStyleBackColor = True
        '
        'RadioButton14
        '
        Me.RadioButton14.AutoSize = True
        Me.RadioButton14.Location = New System.Drawing.Point(93, 130)
        Me.RadioButton14.Name = "RadioButton14"
        Me.RadioButton14.Size = New System.Drawing.Size(62, 17)
        Me.RadioButton14.TabIndex = 13
        Me.RadioButton14.TabStop = True
        Me.RadioButton14.Text = "14 X 14"
        Me.ToolTip1.SetToolTip(Me.RadioButton14, "3584 X 3584")
        Me.RadioButton14.UseVisualStyleBackColor = True
        '
        'RadioButton13
        '
        Me.RadioButton13.AutoSize = True
        Me.RadioButton13.Location = New System.Drawing.Point(93, 108)
        Me.RadioButton13.Name = "RadioButton13"
        Me.RadioButton13.Size = New System.Drawing.Size(62, 17)
        Me.RadioButton13.TabIndex = 12
        Me.RadioButton13.TabStop = True
        Me.RadioButton13.Text = "13 X 13"
        Me.ToolTip1.SetToolTip(Me.RadioButton13, "3328 X 3328")
        Me.RadioButton13.UseVisualStyleBackColor = True
        '
        'FrameRateLabel
        '
        Me.FrameRateLabel.AutoSize = True
        Me.FrameRateLabel.Location = New System.Drawing.Point(71, 153)
        Me.FrameRateLabel.Name = "FrameRateLabel"
        Me.FrameRateLabel.Size = New System.Drawing.Size(104, 13)
        Me.FrameRateLabel.TabIndex = 25
        Me.FrameRateLabel.Text = "Frame Rate (0.0909)"
        Me.ToolTip1.SetToolTip(Me.FrameRateLabel, Global.Outworldz.My.Resources.Resources.FRText)
        '
        'FrametimeBox
        '
        Me.FrametimeBox.Location = New System.Drawing.Point(14, 153)
        Me.FrametimeBox.Name = "FrametimeBox"
        Me.FrametimeBox.Size = New System.Drawing.Size(40, 20)
        Me.FrametimeBox.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.FrametimeBox, Global.Outworldz.My.Resources.Resources.FrameTime)
        '
        'ScriptRateLabel
        '
        Me.ScriptRateLabel.AutoSize = True
        Me.ScriptRateLabel.Location = New System.Drawing.Point(71, 125)
        Me.ScriptRateLabel.Name = "ScriptRateLabel"
        Me.ScriptRateLabel.Size = New System.Drawing.Size(113, 13)
        Me.ScriptRateLabel.TabIndex = 23
        Me.ScriptRateLabel.Text = "Script Timer Rate (0.2)"
        Me.ToolTip1.SetToolTip(Me.ScriptRateLabel, Global.Outworldz.My.Resources.Resources.Script_Timer_Text)
        '
        'ScriptTimerTextBox
        '
        Me.ScriptTimerTextBox.Location = New System.Drawing.Point(14, 125)
        Me.ScriptTimerTextBox.Name = "ScriptTimerTextBox"
        Me.ScriptTimerTextBox.Size = New System.Drawing.Size(40, 20)
        Me.ScriptTimerTextBox.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.ScriptTimerTextBox, Global.Outworldz.My.Resources.Resources.STComment)
        '
        'ClampPrimSize
        '
        Me.ClampPrimSize.AutoSize = True
        Me.ClampPrimSize.Location = New System.Drawing.Point(36, 157)
        Me.ClampPrimSize.Name = "ClampPrimSize"
        Me.ClampPrimSize.Size = New System.Drawing.Size(15, 14)
        Me.ClampPrimSize.TabIndex = 5
        Me.ToolTip1.SetToolTip(Me.ClampPrimSize, Global.Outworldz.My.Resources.Resources.ClampSize)
        Me.ClampPrimSize.UseVisualStyleBackColor = True
        '
        'ClampPrimLabel
        '
        Me.ClampPrimLabel.AutoSize = True
        Me.ClampPrimLabel.Location = New System.Drawing.Point(65, 157)
        Me.ClampPrimLabel.Name = "ClampPrimLabel"
        Me.ClampPrimLabel.Size = New System.Drawing.Size(82, 13)
        Me.ClampPrimLabel.TabIndex = 24
        Me.ClampPrimLabel.Text = "Clamp Prim Size"
        Me.ToolTip1.SetToolTip(Me.ClampPrimLabel, Global.Outworldz.My.Resources.Resources.ClampSize)
        '
        'RegionPort
        '
        Me.RegionPort.Location = New System.Drawing.Point(162, 22)
        Me.RegionPort.Name = "RegionPort"
        Me.RegionPort.ReadOnly = True
        Me.RegionPort.Size = New System.Drawing.Size(43, 20)
        Me.RegionPort.TabIndex = 25
        Me.ToolTip1.SetToolTip(Me.RegionPort, Global.Outworldz.My.Resources.Resources.CoordY)
        '
        'RegionsGroupbox
        '
        Me.RegionsGroupbox.BackColor = System.Drawing.SystemColors.Window
        Me.RegionsGroupbox.Controls.Add(Me.Label4)
        Me.RegionsGroupbox.Controls.Add(Me.RegionPort)
        Me.RegionsGroupbox.Controls.Add(Me.ClampPrimSize)
        Me.RegionsGroupbox.Controls.Add(Me.ClampPrimLabel)
        Me.RegionsGroupbox.Controls.Add(Me.YLabel)
        Me.RegionsGroupbox.Controls.Add(Me.MaxMAvatarsLabel)
        Me.RegionsGroupbox.Controls.Add(Me.NonphysicalPrimMax)
        Me.RegionsGroupbox.Controls.Add(Me.MaxNPrimsLabel)
        Me.RegionsGroupbox.Controls.Add(Me.PhysicalPrimMax)
        Me.RegionsGroupbox.Controls.Add(Me.Label6)
        Me.RegionsGroupbox.Controls.Add(Me.PhysPrimLabel)
        Me.RegionsGroupbox.Controls.Add(Me.MaxPrims)
        Me.RegionsGroupbox.Controls.Add(Me.NonPhysPrimLabel)
        Me.RegionsGroupbox.Controls.Add(Me.MaxAgents)
        Me.RegionsGroupbox.Controls.Add(Me.XLabel)
        Me.RegionsGroupbox.Controls.Add(Me.Label1)
        Me.RegionsGroupbox.Controls.Add(Me.UUID)
        Me.RegionsGroupbox.Controls.Add(Me.CoordY)
        Me.RegionsGroupbox.Controls.Add(Me.CoordX)
        Me.RegionsGroupbox.Location = New System.Drawing.Point(8, 11)
        Me.RegionsGroupbox.Name = "RegionsGroupbox"
        Me.RegionsGroupbox.Size = New System.Drawing.Size(263, 207)
        Me.RegionsGroupbox.TabIndex = 26
        Me.RegionsGroupbox.TabStop = False
        Me.RegionsGroupbox.Text = "Regions"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(211, 25)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(26, 13)
        Me.Label4.TabIndex = 26
        Me.Label4.Text = "Port"
        '
        'YLabel
        '
        Me.YLabel.AutoSize = True
        Me.YLabel.Location = New System.Drawing.Point(127, 25)
        Me.YLabel.Name = "YLabel"
        Me.YLabel.Size = New System.Drawing.Size(14, 13)
        Me.YLabel.TabIndex = 22
        Me.YLabel.Text = "Y"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(226, 184)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(34, 13)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "UUID"
        '
        'XLabel
        '
        Me.XLabel.AutoSize = True
        Me.XLabel.Location = New System.Drawing.Point(58, 25)
        Me.XLabel.Name = "XLabel"
        Me.XLabel.Size = New System.Drawing.Size(14, 13)
        Me.XLabel.TabIndex = 0
        Me.XLabel.Text = "X"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(153, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(0, 13)
        Me.Label1.TabIndex = 3
        '
        'UUID
        '
        Me.UUID.Location = New System.Drawing.Point(11, 181)
        Me.UUID.Name = "UUID"
        Me.UUID.ReadOnly = True
        Me.UUID.Size = New System.Drawing.Size(209, 20)
        Me.UUID.TabIndex = 6
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
        Me.GroupBox2.Location = New System.Drawing.Point(289, 12)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(170, 206)
        Me.GroupBox2.TabIndex = 4
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Sim Size"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(148, 47)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(0, 13)
        Me.Label3.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(148, 25)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(0, 13)
        Me.Label2.TabIndex = 2
        '
        'SaveButton
        '
        Me.SaveButton.Location = New System.Drawing.Point(16, 107)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(102, 23)
        Me.SaveButton.TabIndex = 3
        Me.SaveButton.Text = Global.Outworldz.My.Resources.Resources.Save_word
        Me.SaveButton.UseVisualStyleBackColor = True
        '
        'DeleteButton
        '
        Me.DeleteButton.Location = New System.Drawing.Point(16, 165)
        Me.DeleteButton.Name = "DeleteButton"
        Me.DeleteButton.Size = New System.Drawing.Size(101, 23)
        Me.DeleteButton.TabIndex = 5
        Me.DeleteButton.Text = Global.Outworldz.My.Resources.Resources.Delete_word
        Me.DeleteButton.UseVisualStyleBackColor = True
        '
        'EnabledCheckBox
        '
        Me.EnabledCheckBox.AutoSize = True
        Me.EnabledCheckBox.Location = New System.Drawing.Point(16, 55)
        Me.EnabledCheckBox.Name = "EnabledCheckBox"
        Me.EnabledCheckBox.Size = New System.Drawing.Size(65, 17)
        Me.EnabledCheckBox.TabIndex = 1
        Me.EnabledCheckBox.Text = Global.Outworldz.My.Resources.Resources.Enabled_word
        Me.EnabledCheckBox.UseVisualStyleBackColor = True
        '
        'ScriptsGroupbox
        '
        Me.ScriptsGroupbox.Controls.Add(Me.ScriptOffButton)
        Me.ScriptsGroupbox.Controls.Add(Me.FrameRateLabel)
        Me.ScriptsGroupbox.Controls.Add(Me.FrametimeBox)
        Me.ScriptsGroupbox.Controls.Add(Me.ScriptRateLabel)
        Me.ScriptsGroupbox.Controls.Add(Me.ScriptTimerTextBox)
        Me.ScriptsGroupbox.Controls.Add(Me.ScriptDefaultButton)
        Me.ScriptsGroupbox.Controls.Add(Me.XEngineButton)
        Me.ScriptsGroupbox.Controls.Add(Me.YEngineButton)
        Me.ScriptsGroupbox.Location = New System.Drawing.Point(8, 13)
        Me.ScriptsGroupbox.Name = "ScriptsGroupbox"
        Me.ScriptsGroupbox.Size = New System.Drawing.Size(265, 200)
        Me.ScriptsGroupbox.TabIndex = 4
        Me.ScriptsGroupbox.TabStop = False
        Me.ScriptsGroupbox.Text = "Script Engine"
        '
        'ScriptOffButton
        '
        Me.ScriptOffButton.AutoSize = True
        Me.ScriptOffButton.Location = New System.Drawing.Point(15, 44)
        Me.ScriptOffButton.Name = "ScriptOffButton"
        Me.ScriptOffButton.Size = New System.Drawing.Size(39, 17)
        Me.ScriptOffButton.TabIndex = 26
        Me.ScriptOffButton.TabStop = True
        Me.ScriptOffButton.Text = "Off"
        Me.ScriptOffButton.UseVisualStyleBackColor = True
        '
        'ScriptDefaultButton
        '
        Me.ScriptDefaultButton.AutoSize = True
        Me.ScriptDefaultButton.Location = New System.Drawing.Point(16, 19)
        Me.ScriptDefaultButton.Name = "ScriptDefaultButton"
        Me.ScriptDefaultButton.Size = New System.Drawing.Size(81, 17)
        Me.ScriptDefaultButton.TabIndex = 0
        Me.ScriptDefaultButton.TabStop = True
        Me.ScriptDefaultButton.Text = Global.Outworldz.My.Resources.Resources.Use_Default_word
        Me.ScriptDefaultButton.UseVisualStyleBackColor = True
        '
        'XEngineButton
        '
        Me.XEngineButton.AutoSize = True
        Me.XEngineButton.Location = New System.Drawing.Point(16, 67)
        Me.XEngineButton.Name = "XEngineButton"
        Me.XEngineButton.Size = New System.Drawing.Size(68, 17)
        Me.XEngineButton.TabIndex = 1
        Me.XEngineButton.TabStop = True
        Me.XEngineButton.Text = Global.Outworldz.My.Resources.Resources.XEngine_word
        Me.XEngineButton.UseVisualStyleBackColor = True
        '
        'YEngineButton
        '
        Me.YEngineButton.AutoSize = True
        Me.YEngineButton.Location = New System.Drawing.Point(16, 90)
        Me.YEngineButton.Name = "YEngineButton"
        Me.YEngineButton.Size = New System.Drawing.Size(68, 17)
        Me.YEngineButton.TabIndex = 2
        Me.YEngineButton.TabStop = True
        Me.YEngineButton.Text = Global.Outworldz.My.Resources.Resources.YEngine_word
        Me.YEngineButton.UseVisualStyleBackColor = True
        '
        'ModulesGroupBox
        '
        Me.ModulesGroupBox.Controls.Add(Me.ConciergeCheckBox)
        Me.ModulesGroupBox.Controls.Add(Me.SkipAutoCheckBox)
        Me.ModulesGroupBox.Controls.Add(Me.DisallowResidents)
        Me.ModulesGroupBox.Controls.Add(Me.DisallowForeigners)
        Me.ModulesGroupBox.Controls.Add(Me.DisableGBCheckBox)
        Me.ModulesGroupBox.Controls.Add(Me.TPCheckBox1)
        Me.ModulesGroupBox.Controls.Add(Me.TidesCheckbox)
        Me.ModulesGroupBox.Controls.Add(Me.BirdsCheckBox)
        Me.ModulesGroupBox.Location = New System.Drawing.Point(8, 13)
        Me.ModulesGroupBox.Name = "ModulesGroupBox"
        Me.ModulesGroupBox.Size = New System.Drawing.Size(265, 200)
        Me.ModulesGroupBox.TabIndex = 0
        Me.ModulesGroupBox.TabStop = False
        Me.ModulesGroupBox.Text = "Modules"
        '
        'ConciergeCheckBox
        '
        Me.ConciergeCheckBox.AutoSize = True
        Me.ConciergeCheckBox.Location = New System.Drawing.Point(15, 160)
        Me.ConciergeCheckBox.Name = "ConciergeCheckBox"
        Me.ConciergeCheckBox.Size = New System.Drawing.Size(111, 17)
        Me.ConciergeCheckBox.TabIndex = 7
        Me.ConciergeCheckBox.Text = "Announce Visitors"
        Me.ConciergeCheckBox.UseVisualStyleBackColor = True
        '
        'RichTextBoxModules
        '
        Me.RichTextBoxModules.Location = New System.Drawing.Point(287, 23)
        Me.RichTextBoxModules.Margin = New System.Windows.Forms.Padding(2)
        Me.RichTextBoxModules.Name = "RichTextBoxModules"
        Me.RichTextBoxModules.ReadOnly = True
        Me.RichTextBoxModules.Size = New System.Drawing.Size(200, 190)
        Me.RichTextBoxModules.TabIndex = 8
        Me.RichTextBoxModules.Text = ""
        '
        'PublicityGroupBox
        '
        Me.PublicityGroupBox.Controls.Add(Me.SearchLabel)
        Me.PublicityGroupBox.Controls.Add(Me.LinkLabel1)
        Me.PublicityGroupBox.Controls.Add(Me.Label5)
        Me.PublicityGroupBox.Controls.Add(Me.Hyperica)
        Me.PublicityGroupBox.Controls.Add(Me.APIKey)
        Me.PublicityGroupBox.Controls.Add(Me.Publish)
        Me.PublicityGroupBox.Controls.Add(Me.Opensimworld)
        Me.PublicityGroupBox.Controls.Add(Me.NoPublish)
        Me.PublicityGroupBox.Controls.Add(Me.PublishDefault)
        Me.PublicityGroupBox.Location = New System.Drawing.Point(8, 13)
        Me.PublicityGroupBox.Name = "PublicityGroupBox"
        Me.PublicityGroupBox.Size = New System.Drawing.Size(265, 200)
        Me.PublicityGroupBox.TabIndex = 1
        Me.PublicityGroupBox.TabStop = False
        Me.PublicityGroupBox.Text = "Publicity"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(16, 140)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(25, 13)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Key"
        '
        'Hyperica
        '
        Me.Hyperica.AutoSize = True
        Me.Hyperica.Location = New System.Drawing.Point(115, 27)
        Me.Hyperica.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Hyperica.Name = "Hyperica"
        Me.Hyperica.Size = New System.Drawing.Size(0, 13)
        Me.Hyperica.TabIndex = 3
        Me.Hyperica.TabStop = True
        '
        'APIKey
        '
        Me.APIKey.Location = New System.Drawing.Point(20, 156)
        Me.APIKey.Name = "APIKey"
        Me.APIKey.Size = New System.Drawing.Size(203, 20)
        Me.APIKey.TabIndex = 5
        '
        'Publish
        '
        Me.Publish.AutoSize = True
        Me.Publish.Location = New System.Drawing.Point(11, 73)
        Me.Publish.Name = "Publish"
        Me.Publish.Size = New System.Drawing.Size(175, 17)
        Me.Publish.TabIndex = 2
        Me.Publish.TabStop = True
        Me.Publish.Text = Global.Outworldz.My.Resources.Resources.Publish_Items
        Me.Publish.UseVisualStyleBackColor = True
        '
        'Opensimworld
        '
        Me.Opensimworld.AutoSize = True
        Me.Opensimworld.Location = New System.Drawing.Point(78, 140)
        Me.Opensimworld.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Opensimworld.Name = "Opensimworld"
        Me.Opensimworld.Size = New System.Drawing.Size(130, 13)
        Me.Opensimworld.TabIndex = 6
        Me.Opensimworld.TabStop = True
        Me.Opensimworld.Text = "https://opensimworld.com"
        '
        'NoPublish
        '
        Me.NoPublish.AutoSize = True
        Me.NoPublish.Location = New System.Drawing.Point(11, 51)
        Me.NoPublish.Name = "NoPublish"
        Me.NoPublish.Size = New System.Drawing.Size(144, 17)
        Me.NoPublish.TabIndex = 1
        Me.NoPublish.TabStop = True
        Me.NoPublish.Text = Global.Outworldz.My.Resources.Resources.No_Publish_Items
        Me.NoPublish.UseVisualStyleBackColor = True
        '
        'PublishDefault
        '
        Me.PublishDefault.AutoSize = True
        Me.PublishDefault.Location = New System.Drawing.Point(11, 27)
        Me.PublishDefault.Name = "PublishDefault"
        Me.PublishDefault.Size = New System.Drawing.Size(81, 17)
        Me.PublishDefault.TabIndex = 0
        Me.PublishDefault.TabStop = True
        Me.PublishDefault.Text = Global.Outworldz.My.Resources.Resources.Use_Default_word
        Me.PublishDefault.UseVisualStyleBackColor = True
        '
        'PermissionsGroupbox
        '
        Me.PermissionsGroupbox.Controls.Add(Me.Gods_Use_Default)
        Me.PermissionsGroupbox.Controls.Add(Me.GodLevel)
        Me.PermissionsGroupbox.Controls.Add(Me.GodManager)
        Me.PermissionsGroupbox.Controls.Add(Me.GodEstate)
        Me.PermissionsGroupbox.Location = New System.Drawing.Point(8, 13)
        Me.PermissionsGroupbox.Name = "PermissionsGroupbox"
        Me.PermissionsGroupbox.Size = New System.Drawing.Size(265, 200)
        Me.PermissionsGroupbox.TabIndex = 2
        Me.PermissionsGroupbox.TabStop = False
        Me.PermissionsGroupbox.Text = "Permissions"
        '
        'Gods_Use_Default
        '
        Me.Gods_Use_Default.AutoSize = True
        Me.Gods_Use_Default.Location = New System.Drawing.Point(15, 27)
        Me.Gods_Use_Default.Name = "Gods_Use_Default"
        Me.Gods_Use_Default.Size = New System.Drawing.Size(82, 17)
        Me.Gods_Use_Default.TabIndex = 0
        Me.Gods_Use_Default.Text = Global.Outworldz.My.Resources.Resources.Use_Default_word
        Me.Gods_Use_Default.UseVisualStyleBackColor = True
        '
        'RichTextBoxPermissions
        '
        Me.RichTextBoxPermissions.Location = New System.Drawing.Point(288, 13)
        Me.RichTextBoxPermissions.Margin = New System.Windows.Forms.Padding(2)
        Me.RichTextBoxPermissions.Name = "RichTextBoxPermissions"
        Me.RichTextBoxPermissions.ReadOnly = True
        Me.RichTextBoxPermissions.Size = New System.Drawing.Size(200, 190)
        Me.RichTextBoxPermissions.TabIndex = 6
        Me.RichTextBoxPermissions.Text = ""
        '
        'MapGroupBox
        '
        Me.MapGroupBox.Controls.Add(Me.LandingSpotLabel)
        Me.MapGroupBox.Controls.Add(Me.LandingSpotTextBox)
        Me.MapGroupBox.Controls.Add(Me.Maps_Use_Default)
        Me.MapGroupBox.Controls.Add(Me.MapPicture)
        Me.MapGroupBox.Controls.Add(Me.MapNone)
        Me.MapGroupBox.Controls.Add(Me.MapSimple)
        Me.MapGroupBox.Controls.Add(Me.MapBetter)
        Me.MapGroupBox.Controls.Add(Me.MapBest)
        Me.MapGroupBox.Controls.Add(Me.MapGood)
        Me.MapGroupBox.Location = New System.Drawing.Point(8, 13)
        Me.MapGroupBox.Name = "MapGroupBox"
        Me.MapGroupBox.Size = New System.Drawing.Size(265, 200)
        Me.MapGroupBox.TabIndex = 5
        Me.MapGroupBox.TabStop = False
        Me.MapGroupBox.Text = "Maps"
        '
        'LandingSpotLabel
        '
        Me.LandingSpotLabel.AutoSize = True
        Me.LandingSpotLabel.Location = New System.Drawing.Point(116, 174)
        Me.LandingSpotLabel.Name = "LandingSpotLabel"
        Me.LandingSpotLabel.Size = New System.Drawing.Size(70, 13)
        Me.LandingSpotLabel.TabIndex = 143
        Me.LandingSpotLabel.Text = "Landing Spot"
        '
        'LandingSpotTextBox
        '
        Me.LandingSpotTextBox.Location = New System.Drawing.Point(23, 171)
        Me.LandingSpotTextBox.Name = "LandingSpotTextBox"
        Me.LandingSpotTextBox.Size = New System.Drawing.Size(87, 20)
        Me.LandingSpotTextBox.TabIndex = 139
        '
        'Maps_Use_Default
        '
        Me.Maps_Use_Default.AutoSize = True
        Me.Maps_Use_Default.Checked = True
        Me.Maps_Use_Default.Location = New System.Drawing.Point(15, 20)
        Me.Maps_Use_Default.Name = "Maps_Use_Default"
        Me.Maps_Use_Default.Size = New System.Drawing.Size(81, 17)
        Me.Maps_Use_Default.TabIndex = 0
        Me.Maps_Use_Default.TabStop = True
        Me.Maps_Use_Default.Text = Global.Outworldz.My.Resources.Resources.Use_Default_word
        Me.Maps_Use_Default.UseVisualStyleBackColor = True
        '
        'MapPicture
        '
        Me.MapPicture.InitialImage = CType(resources.GetObject("MapPicture.InitialImage"), System.Drawing.Image)
        Me.MapPicture.Location = New System.Drawing.Point(145, 20)
        Me.MapPicture.Name = "MapPicture"
        Me.MapPicture.Size = New System.Drawing.Size(94, 90)
        Me.MapPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.MapPicture.TabIndex = 138
        Me.MapPicture.TabStop = False
        '
        'MapNone
        '
        Me.MapNone.AutoSize = True
        Me.MapNone.Location = New System.Drawing.Point(15, 43)
        Me.MapNone.Name = "MapNone"
        Me.MapNone.Size = New System.Drawing.Size(51, 17)
        Me.MapNone.TabIndex = 1
        Me.MapNone.Text = Global.Outworldz.My.Resources.Resources.None
        Me.MapNone.UseVisualStyleBackColor = True
        '
        'MapSimple
        '
        Me.MapSimple.AutoSize = True
        Me.MapSimple.Location = New System.Drawing.Point(15, 67)
        Me.MapSimple.Name = "MapSimple"
        Me.MapSimple.Size = New System.Drawing.Size(94, 17)
        Me.MapSimple.TabIndex = 2
        Me.MapSimple.Text = Global.Outworldz.My.Resources.Resources.Simple_but_Fast_word
        Me.MapSimple.UseVisualStyleBackColor = True
        '
        'MapBetter
        '
        Me.MapBetter.AutoSize = True
        Me.MapBetter.Location = New System.Drawing.Point(15, 116)
        Me.MapBetter.Name = "MapBetter"
        Me.MapBetter.Size = New System.Drawing.Size(116, 17)
        Me.MapBetter.TabIndex = 4
        Me.MapBetter.Text = Global.Outworldz.My.Resources.Resources.Better_Prims
        Me.MapBetter.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage
        Me.MapBetter.UseVisualStyleBackColor = True
        '
        'MapBest
        '
        Me.MapBest.AutoSize = True
        Me.MapBest.Location = New System.Drawing.Point(15, 139)
        Me.MapBest.Name = "MapBest"
        Me.MapBest.Size = New System.Drawing.Size(171, 17)
        Me.MapBest.TabIndex = 5
        Me.MapBest.Text = Global.Outworldz.My.Resources.Resources.Best_Prims
        Me.MapBest.UseVisualStyleBackColor = True
        '
        'MapGood
        '
        Me.MapGood.AutoSize = True
        Me.MapGood.Location = New System.Drawing.Point(15, 93)
        Me.MapGood.Name = "MapGood"
        Me.MapGood.Size = New System.Drawing.Size(100, 17)
        Me.MapGood.TabIndex = 3
        Me.MapGood.Text = Global.Outworldz.My.Resources.Resources.Good_Warp3D_word
        Me.MapGood.UseVisualStyleBackColor = True
        '
        'RichTextBoxMap
        '
        Me.RichTextBoxMap.Location = New System.Drawing.Point(288, 23)
        Me.RichTextBoxMap.Margin = New System.Windows.Forms.Padding(2)
        Me.RichTextBoxMap.Name = "RichTextBoxMap"
        Me.RichTextBoxMap.ReadOnly = True
        Me.RichTextBoxMap.Size = New System.Drawing.Size(200, 190)
        Me.RichTextBoxMap.TabIndex = 139
        Me.RichTextBoxMap.Text = ""
        '
        'DeregisterButton
        '
        Me.DeregisterButton.Location = New System.Drawing.Point(16, 136)
        Me.DeregisterButton.Name = "DeregisterButton"
        Me.DeregisterButton.Size = New System.Drawing.Size(102, 23)
        Me.DeregisterButton.TabIndex = 4
        Me.DeregisterButton.Text = Global.Outworldz.My.Resources.Resources.Deregister_word
        Me.DeregisterButton.UseVisualStyleBackColor = True
        '
        'MenuStrip2
        '
        Me.MenuStrip2.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.MenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HelpToolStripMenuItem})
        Me.MenuStrip2.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip2.Name = "MenuStrip2"
        Me.MenuStrip2.Padding = New System.Windows.Forms.Padding(4, 1, 0, 1)
        Me.MenuStrip2.Size = New System.Drawing.Size(512, 30)
        Me.MenuStrip2.TabIndex = 0
        Me.MenuStrip2.Text = "0"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BasicsToolStripMenuItem, Me.OptionsToolStripMenuItem, Me.MapsToolStripMenuItem, Me.PhysicsToolStripMenuItem, Me.ScriptsToolStripMenuItem, Me.PermissionsToolStripMenuItem, Me.PublicityToolStripMenuItem, Me.ModulesToolStripMenuItem})
        Me.HelpToolStripMenuItem.Image = Global.Outworldz.My.Resources.Resources.about
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(68, 28)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'BasicsToolStripMenuItem
        '
        Me.BasicsToolStripMenuItem.Image = Global.Outworldz.My.Resources.Resources.cube_green
        Me.BasicsToolStripMenuItem.Name = "BasicsToolStripMenuItem"
        Me.BasicsToolStripMenuItem.Size = New System.Drawing.Size(188, 30)
        Me.BasicsToolStripMenuItem.Text = "Basics"
        '
        'OptionsToolStripMenuItem
        '
        Me.OptionsToolStripMenuItem.Image = Global.Outworldz.My.Resources.Resources.cube_green
        Me.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem"
        Me.OptionsToolStripMenuItem.Size = New System.Drawing.Size(188, 30)
        Me.OptionsToolStripMenuItem.Text = "Options"
        '
        'MapsToolStripMenuItem
        '
        Me.MapsToolStripMenuItem.Image = Global.Outworldz.My.Resources.Resources.earth_view
        Me.MapsToolStripMenuItem.Name = "MapsToolStripMenuItem"
        Me.MapsToolStripMenuItem.Size = New System.Drawing.Size(188, 30)
        Me.MapsToolStripMenuItem.Text = "Maps"
        '
        'PhysicsToolStripMenuItem
        '
        Me.PhysicsToolStripMenuItem.Image = Global.Outworldz.My.Resources.Resources.transform
        Me.PhysicsToolStripMenuItem.Name = "PhysicsToolStripMenuItem"
        Me.PhysicsToolStripMenuItem.Size = New System.Drawing.Size(188, 30)
        Me.PhysicsToolStripMenuItem.Text = "Physics"
        '
        'ScriptsToolStripMenuItem
        '
        Me.ScriptsToolStripMenuItem.Image = Global.Outworldz.My.Resources.Resources.text_marked
        Me.ScriptsToolStripMenuItem.Name = "ScriptsToolStripMenuItem"
        Me.ScriptsToolStripMenuItem.Size = New System.Drawing.Size(188, 30)
        Me.ScriptsToolStripMenuItem.Text = "Scripts"
        '
        'PermissionsToolStripMenuItem
        '
        Me.PermissionsToolStripMenuItem.Image = Global.Outworldz.My.Resources.Resources.spy
        Me.PermissionsToolStripMenuItem.Name = "PermissionsToolStripMenuItem"
        Me.PermissionsToolStripMenuItem.Size = New System.Drawing.Size(188, 30)
        Me.PermissionsToolStripMenuItem.Text = "Permissions"
        '
        'PublicityToolStripMenuItem
        '
        Me.PublicityToolStripMenuItem.Image = Global.Outworldz.My.Resources.Resources.microsoft_edge
        Me.PublicityToolStripMenuItem.Name = "PublicityToolStripMenuItem"
        Me.PublicityToolStripMenuItem.Size = New System.Drawing.Size(188, 30)
        Me.PublicityToolStripMenuItem.Text = "Publicity"
        '
        'ModulesToolStripMenuItem
        '
        Me.ModulesToolStripMenuItem.Image = Global.Outworldz.My.Resources.Resources.window_add
        Me.ModulesToolStripMenuItem.Name = "ModulesToolStripMenuItem"
        Me.ModulesToolStripMenuItem.Size = New System.Drawing.Size(188, 30)
        Me.ModulesToolStripMenuItem.Text = "Modules"
        '
        'CPUTab
        '
        Me.CPUTab.Controls.Add(Me.Basics)
        Me.CPUTab.Controls.Add(Me.Options)
        Me.CPUTab.Controls.Add(Me.Maps)
        Me.CPUTab.Controls.Add(Me.Physics)
        Me.CPUTab.Controls.Add(Me.Scripts)
        Me.CPUTab.Controls.Add(Me.Permissions)
        Me.CPUTab.Controls.Add(Me.Publicity)
        Me.CPUTab.Controls.Add(Me.Modules)
        Me.CPUTab.Controls.Add(Me.TabPage1)
        Me.CPUTab.Location = New System.Drawing.Point(0, 32)
        Me.CPUTab.Margin = New System.Windows.Forms.Padding(2)
        Me.CPUTab.Name = "CPUTab"
        Me.CPUTab.SelectedIndex = 0
        Me.CPUTab.Size = New System.Drawing.Size(512, 254)
        Me.CPUTab.TabIndex = 1
        '
        'Basics
        '
        Me.Basics.Controls.Add(Me.Label8)
        Me.Basics.Controls.Add(Me.EnabledCheckBox)
        Me.Basics.Controls.Add(Me.DeregisterButton)
        Me.Basics.Controls.Add(Me.GroupBox2)
        Me.Basics.Controls.Add(Me.DeleteButton)
        Me.Basics.Controls.Add(Me.RegionName)
        Me.Basics.Controls.Add(Me.SmartStartCheckBox)
        Me.Basics.Controls.Add(Me.SaveButton)
        Me.Basics.Location = New System.Drawing.Point(4, 22)
        Me.Basics.Margin = New System.Windows.Forms.Padding(2)
        Me.Basics.Name = "Basics"
        Me.Basics.Padding = New System.Windows.Forms.Padding(2)
        Me.Basics.Size = New System.Drawing.Size(504, 228)
        Me.Basics.TabIndex = 0
        Me.Basics.Text = "Region Basics"
        Me.Basics.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(13, 12)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(38, 13)
        Me.Label8.TabIndex = 8
        Me.Label8.Text = "Name:"
        '
        'Options
        '
        Me.Options.Controls.Add(Me.RichTextBoxOptions)
        Me.Options.Controls.Add(Me.RegionsGroupbox)
        Me.Options.Location = New System.Drawing.Point(4, 22)
        Me.Options.Margin = New System.Windows.Forms.Padding(2)
        Me.Options.Name = "Options"
        Me.Options.Padding = New System.Windows.Forms.Padding(2)
        Me.Options.Size = New System.Drawing.Size(504, 228)
        Me.Options.TabIndex = 1
        Me.Options.Text = "Optional"
        Me.Options.UseVisualStyleBackColor = True
        '
        'RichTextBoxOptions
        '
        Me.RichTextBoxOptions.Location = New System.Drawing.Point(287, 22)
        Me.RichTextBoxOptions.Margin = New System.Windows.Forms.Padding(2)
        Me.RichTextBoxOptions.Name = "RichTextBoxOptions"
        Me.RichTextBoxOptions.ReadOnly = True
        Me.RichTextBoxOptions.Size = New System.Drawing.Size(200, 190)
        Me.RichTextBoxOptions.TabIndex = 140
        Me.RichTextBoxOptions.Text = ""
        '
        'Maps
        '
        Me.Maps.Controls.Add(Me.RichTextBoxMap)
        Me.Maps.Controls.Add(Me.MapGroupBox)
        Me.Maps.Location = New System.Drawing.Point(4, 22)
        Me.Maps.Margin = New System.Windows.Forms.Padding(2)
        Me.Maps.Name = "Maps"
        Me.Maps.Size = New System.Drawing.Size(504, 228)
        Me.Maps.TabIndex = 2
        Me.Maps.Text = "Maps"
        Me.Maps.UseVisualStyleBackColor = True
        '
        'Physics
        '
        Me.Physics.Controls.Add(Me.RichTextBoxPhysics)
        Me.Physics.Controls.Add(Me.PhysicsGroupbox)
        Me.Physics.Location = New System.Drawing.Point(4, 22)
        Me.Physics.Margin = New System.Windows.Forms.Padding(2)
        Me.Physics.Name = "Physics"
        Me.Physics.Size = New System.Drawing.Size(504, 228)
        Me.Physics.TabIndex = 8
        Me.Physics.Text = "Physics"
        Me.Physics.UseVisualStyleBackColor = True
        '
        'RichTextBoxPhysics
        '
        Me.RichTextBoxPhysics.Location = New System.Drawing.Point(287, 23)
        Me.RichTextBoxPhysics.Margin = New System.Windows.Forms.Padding(2)
        Me.RichTextBoxPhysics.Name = "RichTextBoxPhysics"
        Me.RichTextBoxPhysics.ReadOnly = True
        Me.RichTextBoxPhysics.Size = New System.Drawing.Size(200, 190)
        Me.RichTextBoxPhysics.TabIndex = 6
        Me.RichTextBoxPhysics.Text = ""
        '
        'PhysicsGroupbox
        '
        Me.PhysicsGroupbox.Controls.Add(Me.Physics_Hybrid)
        Me.PhysicsGroupbox.Controls.Add(Me.Physics_Bullet)
        Me.PhysicsGroupbox.Controls.Add(Me.Physics_Default)
        Me.PhysicsGroupbox.Controls.Add(Me.Physics_Separate)
        Me.PhysicsGroupbox.Controls.Add(Me.Physics_ubODE)
        Me.PhysicsGroupbox.Location = New System.Drawing.Point(8, 13)
        Me.PhysicsGroupbox.Name = "PhysicsGroupbox"
        Me.PhysicsGroupbox.Size = New System.Drawing.Size(265, 200)
        Me.PhysicsGroupbox.TabIndex = 4
        Me.PhysicsGroupbox.TabStop = False
        Me.PhysicsGroupbox.Text = "Physics"
        '
        'Physics_Hybrid
        '
        Me.Physics_Hybrid.AutoSize = True
        Me.Physics_Hybrid.Location = New System.Drawing.Point(15, 70)
        Me.Physics_Hybrid.Name = "Physics_Hybrid"
        Me.Physics_Hybrid.Size = New System.Drawing.Size(55, 17)
        Me.Physics_Hybrid.TabIndex = 5
        Me.Physics_Hybrid.TabStop = True
        Me.Physics_Hybrid.Text = "Hybrid"
        Me.Physics_Hybrid.UseVisualStyleBackColor = True
        '
        'Physics_Bullet
        '
        Me.Physics_Bullet.AutoSize = True
        Me.Physics_Bullet.Location = New System.Drawing.Point(15, 93)
        Me.Physics_Bullet.Name = "Physics_Bullet"
        Me.Physics_Bullet.Size = New System.Drawing.Size(92, 17)
        Me.Physics_Bullet.TabIndex = 3
        Me.Physics_Bullet.TabStop = True
        Me.Physics_Bullet.Text = "Bullet physics "
        Me.Physics_Bullet.UseVisualStyleBackColor = True
        '
        'Physics_Default
        '
        Me.Physics_Default.AutoSize = True
        Me.Physics_Default.Location = New System.Drawing.Point(15, 20)
        Me.Physics_Default.Name = "Physics_Default"
        Me.Physics_Default.Size = New System.Drawing.Size(81, 17)
        Me.Physics_Default.TabIndex = 0
        Me.Physics_Default.TabStop = True
        Me.Physics_Default.Text = Global.Outworldz.My.Resources.Resources.Use_Default_word
        Me.Physics_Default.UseVisualStyleBackColor = True
        '
        'Physics_Separate
        '
        Me.Physics_Separate.AutoSize = True
        Me.Physics_Separate.Location = New System.Drawing.Point(15, 116)
        Me.Physics_Separate.Name = "Physics_Separate"
        Me.Physics_Separate.Size = New System.Drawing.Size(177, 17)
        Me.Physics_Separate.TabIndex = 4
        Me.Physics_Separate.TabStop = True
        Me.Physics_Separate.Text = Global.Outworldz.My.Resources.Resources.BP
        Me.Physics_Separate.UseVisualStyleBackColor = True
        '
        'Physics_ubODE
        '
        Me.Physics_ubODE.AutoSize = True
        Me.Physics_ubODE.Location = New System.Drawing.Point(15, 46)
        Me.Physics_ubODE.Name = "Physics_ubODE"
        Me.Physics_ubODE.Size = New System.Drawing.Size(153, 17)
        Me.Physics_ubODE.TabIndex = 2
        Me.Physics_ubODE.TabStop = True
        Me.Physics_ubODE.Text = Global.Outworldz.My.Resources.Resources.UBODE_words
        Me.Physics_ubODE.UseVisualStyleBackColor = True
        '
        'Scripts
        '
        Me.Scripts.Controls.Add(Me.RichTextBoxScripts)
        Me.Scripts.Controls.Add(Me.ScriptsGroupbox)
        Me.Scripts.Location = New System.Drawing.Point(4, 22)
        Me.Scripts.Margin = New System.Windows.Forms.Padding(2)
        Me.Scripts.Name = "Scripts"
        Me.Scripts.Size = New System.Drawing.Size(504, 228)
        Me.Scripts.TabIndex = 3
        Me.Scripts.Text = "Scripts"
        Me.Scripts.UseVisualStyleBackColor = True
        '
        'RichTextBoxScripts
        '
        Me.RichTextBoxScripts.Location = New System.Drawing.Point(287, 23)
        Me.RichTextBoxScripts.Margin = New System.Windows.Forms.Padding(2)
        Me.RichTextBoxScripts.Name = "RichTextBoxScripts"
        Me.RichTextBoxScripts.ReadOnly = True
        Me.RichTextBoxScripts.Size = New System.Drawing.Size(200, 190)
        Me.RichTextBoxScripts.TabIndex = 5
        Me.RichTextBoxScripts.Text = ""
        '
        'Permissions
        '
        Me.Permissions.Controls.Add(Me.RichTextBoxPermissions)
        Me.Permissions.Controls.Add(Me.PermissionsGroupbox)
        Me.Permissions.Location = New System.Drawing.Point(4, 22)
        Me.Permissions.Margin = New System.Windows.Forms.Padding(2)
        Me.Permissions.Name = "Permissions"
        Me.Permissions.Size = New System.Drawing.Size(504, 228)
        Me.Permissions.TabIndex = 5
        Me.Permissions.Text = "Permissions"
        Me.Permissions.UseVisualStyleBackColor = True
        '
        'Publicity
        '
        Me.Publicity.Controls.Add(Me.RichTextBoxPublicity)
        Me.Publicity.Controls.Add(Me.PublicityGroupBox)
        Me.Publicity.Location = New System.Drawing.Point(4, 22)
        Me.Publicity.Margin = New System.Windows.Forms.Padding(2)
        Me.Publicity.Name = "Publicity"
        Me.Publicity.Size = New System.Drawing.Size(504, 228)
        Me.Publicity.TabIndex = 7
        Me.Publicity.Text = "Publicity"
        Me.Publicity.UseVisualStyleBackColor = True
        '
        'RichTextBoxPublicity
        '
        Me.RichTextBoxPublicity.Location = New System.Drawing.Point(287, 23)
        Me.RichTextBoxPublicity.Margin = New System.Windows.Forms.Padding(2)
        Me.RichTextBoxPublicity.Name = "RichTextBoxPublicity"
        Me.RichTextBoxPublicity.ReadOnly = True
        Me.RichTextBoxPublicity.Size = New System.Drawing.Size(200, 190)
        Me.RichTextBoxPublicity.TabIndex = 7
        Me.RichTextBoxPublicity.Text = ""
        '
        'Modules
        '
        Me.Modules.Controls.Add(Me.RichTextBoxModules)
        Me.Modules.Controls.Add(Me.ModulesGroupBox)
        Me.Modules.Location = New System.Drawing.Point(4, 22)
        Me.Modules.Margin = New System.Windows.Forms.Padding(2)
        Me.Modules.Name = "Modules"
        Me.Modules.Size = New System.Drawing.Size(504, 228)
        Me.Modules.TabIndex = 6
        Me.Modules.Text = "Modules"
        Me.Modules.UseVisualStyleBackColor = True
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.GroupBox3)
        Me.TabPage1.Controls.Add(Me.GroupBox1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(504, 228)
        Me.TabPage1.TabIndex = 9
        Me.TabPage1.Text = "CPU"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.BelowNormal)
        Me.GroupBox3.Controls.Add(Me.Normal)
        Me.GroupBox3.Controls.Add(Me.AboveNormal)
        Me.GroupBox3.Controls.Add(Me.High)
        Me.GroupBox3.Controls.Add(Me.RealTime)
        Me.GroupBox3.Location = New System.Drawing.Point(214, 8)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(154, 177)
        Me.GroupBox3.TabIndex = 6
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Priority"
        '
        'BelowNormal
        '
        Me.BelowNormal.AutoSize = True
        Me.BelowNormal.Location = New System.Drawing.Point(22, 130)
        Me.BelowNormal.Name = "BelowNormal"
        Me.BelowNormal.Size = New System.Drawing.Size(87, 17)
        Me.BelowNormal.TabIndex = 4
        Me.BelowNormal.TabStop = True
        Me.BelowNormal.Text = "BelowNormal"
        Me.BelowNormal.UseVisualStyleBackColor = True
        '
        'Normal
        '
        Me.Normal.AutoSize = True
        Me.Normal.Location = New System.Drawing.Point(22, 107)
        Me.Normal.Name = "Normal"
        Me.Normal.Size = New System.Drawing.Size(58, 17)
        Me.Normal.TabIndex = 3
        Me.Normal.TabStop = True
        Me.Normal.Text = "Normal"
        Me.Normal.UseVisualStyleBackColor = True
        '
        'AboveNormal
        '
        Me.AboveNormal.AutoSize = True
        Me.AboveNormal.Location = New System.Drawing.Point(22, 84)
        Me.AboveNormal.Name = "AboveNormal"
        Me.AboveNormal.Size = New System.Drawing.Size(89, 17)
        Me.AboveNormal.TabIndex = 2
        Me.AboveNormal.TabStop = True
        Me.AboveNormal.Text = "AboveNormal"
        Me.AboveNormal.UseVisualStyleBackColor = True
        '
        'High
        '
        Me.High.AutoSize = True
        Me.High.Location = New System.Drawing.Point(22, 61)
        Me.High.Name = "High"
        Me.High.Size = New System.Drawing.Size(47, 17)
        Me.High.TabIndex = 1
        Me.High.TabStop = True
        Me.High.Text = "High"
        Me.High.UseVisualStyleBackColor = True
        '
        'RealTime
        '
        Me.RealTime.AutoSize = True
        Me.RealTime.Location = New System.Drawing.Point(22, 36)
        Me.RealTime.Name = "RealTime"
        Me.RealTime.Size = New System.Drawing.Size(70, 17)
        Me.RealTime.TabIndex = 0
        Me.RealTime.TabStop = True
        Me.RealTime.Text = "RealTime"
        Me.RealTime.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Core16Button)
        Me.GroupBox1.Controls.Add(Me.Core6Button)
        Me.GroupBox1.Controls.Add(Me.Core15Button)
        Me.GroupBox1.Controls.Add(Me.Core14Button)
        Me.GroupBox1.Controls.Add(Me.Core13Button)
        Me.GroupBox1.Controls.Add(Me.Core12Button)
        Me.GroupBox1.Controls.Add(Me.Core11Button)
        Me.GroupBox1.Controls.Add(Me.Core10Button)
        Me.GroupBox1.Controls.Add(Me.Core9Button)
        Me.GroupBox1.Controls.Add(Me.Core8Button)
        Me.GroupBox1.Controls.Add(Me.Core7Button)
        Me.GroupBox1.Controls.Add(Me.Core5Button)
        Me.GroupBox1.Controls.Add(Me.Core4Button)
        Me.GroupBox1.Controls.Add(Me.Core3Button)
        Me.GroupBox1.Controls.Add(Me.Core2Button)
        Me.GroupBox1.Controls.Add(Me.Core1Button)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Location = New System.Drawing.Point(19, 6)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(122, 206)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Cores"
        '
        'Core16Button
        '
        Me.Core16Button.AutoSize = True
        Me.Core16Button.Location = New System.Drawing.Point(61, 183)
        Me.Core16Button.Name = "Core16Button"
        Me.Core16Button.Size = New System.Drawing.Size(38, 17)
        Me.Core16Button.TabIndex = 23
        Me.Core16Button.Text = "16"
        Me.Core16Button.UseVisualStyleBackColor = True
        '
        'Core6Button
        '
        Me.Core6Button.AutoSize = True
        Me.Core6Button.Location = New System.Drawing.Point(6, 138)
        Me.Core6Button.Name = "Core6Button"
        Me.Core6Button.Size = New System.Drawing.Size(32, 17)
        Me.Core6Button.TabIndex = 22
        Me.Core6Button.Text = "6"
        Me.Core6Button.UseVisualStyleBackColor = True
        '
        'Core15Button
        '
        Me.Core15Button.AutoSize = True
        Me.Core15Button.Location = New System.Drawing.Point(61, 161)
        Me.Core15Button.Name = "Core15Button"
        Me.Core15Button.Size = New System.Drawing.Size(38, 17)
        Me.Core15Button.TabIndex = 21
        Me.Core15Button.Text = "15"
        Me.Core15Button.UseVisualStyleBackColor = True
        '
        'Core14Button
        '
        Me.Core14Button.AutoSize = True
        Me.Core14Button.Location = New System.Drawing.Point(61, 138)
        Me.Core14Button.Name = "Core14Button"
        Me.Core14Button.Size = New System.Drawing.Size(38, 17)
        Me.Core14Button.TabIndex = 20
        Me.Core14Button.Text = "14"
        Me.Core14Button.UseVisualStyleBackColor = True
        '
        'Core13Button
        '
        Me.Core13Button.AutoSize = True
        Me.Core13Button.Location = New System.Drawing.Point(61, 116)
        Me.Core13Button.Name = "Core13Button"
        Me.Core13Button.Size = New System.Drawing.Size(38, 17)
        Me.Core13Button.TabIndex = 19
        Me.Core13Button.Text = "13"
        Me.Core13Button.UseVisualStyleBackColor = True
        '
        'Core12Button
        '
        Me.Core12Button.AutoSize = True
        Me.Core12Button.Location = New System.Drawing.Point(61, 93)
        Me.Core12Button.Name = "Core12Button"
        Me.Core12Button.Size = New System.Drawing.Size(38, 17)
        Me.Core12Button.TabIndex = 18
        Me.Core12Button.Text = "12"
        Me.Core12Button.UseVisualStyleBackColor = True
        '
        'Core11Button
        '
        Me.Core11Button.AutoSize = True
        Me.Core11Button.Location = New System.Drawing.Point(61, 70)
        Me.Core11Button.Name = "Core11Button"
        Me.Core11Button.Size = New System.Drawing.Size(38, 17)
        Me.Core11Button.TabIndex = 17
        Me.Core11Button.Text = "11"
        Me.Core11Button.UseVisualStyleBackColor = True
        '
        'Core10Button
        '
        Me.Core10Button.AutoSize = True
        Me.Core10Button.Location = New System.Drawing.Point(61, 47)
        Me.Core10Button.Name = "Core10Button"
        Me.Core10Button.Size = New System.Drawing.Size(38, 17)
        Me.Core10Button.TabIndex = 16
        Me.Core10Button.Text = "10"
        Me.Core10Button.UseVisualStyleBackColor = True
        '
        'Core9Button
        '
        Me.Core9Button.AutoSize = True
        Me.Core9Button.Location = New System.Drawing.Point(61, 25)
        Me.Core9Button.Name = "Core9Button"
        Me.Core9Button.Size = New System.Drawing.Size(32, 17)
        Me.Core9Button.TabIndex = 15
        Me.Core9Button.Text = "9"
        Me.Core9Button.UseVisualStyleBackColor = True
        '
        'Core8Button
        '
        Me.Core8Button.AutoSize = True
        Me.Core8Button.Location = New System.Drawing.Point(6, 184)
        Me.Core8Button.Name = "Core8Button"
        Me.Core8Button.Size = New System.Drawing.Size(32, 17)
        Me.Core8Button.TabIndex = 13
        Me.Core8Button.Text = "8"
        Me.Core8Button.UseVisualStyleBackColor = True
        '
        'Core7Button
        '
        Me.Core7Button.AutoSize = True
        Me.Core7Button.Location = New System.Drawing.Point(6, 161)
        Me.Core7Button.Name = "Core7Button"
        Me.Core7Button.Size = New System.Drawing.Size(32, 17)
        Me.Core7Button.TabIndex = 12
        Me.Core7Button.Text = "7"
        Me.Core7Button.UseVisualStyleBackColor = True
        '
        'Core5Button
        '
        Me.Core5Button.AutoSize = True
        Me.Core5Button.Location = New System.Drawing.Point(6, 115)
        Me.Core5Button.Name = "Core5Button"
        Me.Core5Button.Size = New System.Drawing.Size(32, 17)
        Me.Core5Button.TabIndex = 11
        Me.Core5Button.Text = "5"
        Me.Core5Button.UseVisualStyleBackColor = True
        '
        'Core4Button
        '
        Me.Core4Button.AutoSize = True
        Me.Core4Button.Location = New System.Drawing.Point(6, 92)
        Me.Core4Button.Name = "Core4Button"
        Me.Core4Button.Size = New System.Drawing.Size(32, 17)
        Me.Core4Button.TabIndex = 10
        Me.Core4Button.Text = "4"
        Me.Core4Button.UseVisualStyleBackColor = True
        '
        'Core3Button
        '
        Me.Core3Button.AutoSize = True
        Me.Core3Button.Location = New System.Drawing.Point(6, 69)
        Me.Core3Button.Name = "Core3Button"
        Me.Core3Button.Size = New System.Drawing.Size(32, 17)
        Me.Core3Button.TabIndex = 9
        Me.Core3Button.Text = "3"
        Me.Core3Button.UseVisualStyleBackColor = True
        '
        'Core2Button
        '
        Me.Core2Button.AutoSize = True
        Me.Core2Button.Location = New System.Drawing.Point(6, 46)
        Me.Core2Button.Name = "Core2Button"
        Me.Core2Button.Size = New System.Drawing.Size(32, 17)
        Me.Core2Button.TabIndex = 8
        Me.Core2Button.Text = "2"
        Me.Core2Button.UseVisualStyleBackColor = True
        '
        'Core1Button
        '
        Me.Core1Button.AutoSize = True
        Me.Core1Button.Location = New System.Drawing.Point(6, 24)
        Me.Core1Button.Name = "Core1Button"
        Me.Core1Button.Size = New System.Drawing.Size(32, 17)
        Me.Core1Button.TabIndex = 7
        Me.Core1Button.Text = "1"
        Me.Core1Button.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(148, 47)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(0, 13)
        Me.Label7.TabIndex = 3
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(148, 25)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(0, 13)
        Me.Label9.TabIndex = 2
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(61, 4)
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.Location = New System.Drawing.Point(78, 111)
        Me.LinkLabel1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(150, 13)
        Me.LinkLabel1.TabIndex = 7
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "https://outworldz.com/Search"
        '
        'SearchLabel
        '
        Me.SearchLabel.AutoSize = True
        Me.SearchLabel.Location = New System.Drawing.Point(17, 111)
        Me.SearchLabel.Name = "SearchLabel"
        Me.SearchLabel.Size = New System.Drawing.Size(41, 13)
        Me.SearchLabel.TabIndex = 8
        Me.SearchLabel.Text = "Search"
        '
        'FormRegion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoSize = True
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(512, 278)
        Me.Controls.Add(Me.CPUTab)
        Me.Controls.Add(Me.MenuStrip2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "FormRegion"
        Me.Text = "Regions"
        Me.RegionsGroupbox.ResumeLayout(False)
        Me.RegionsGroupbox.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ScriptsGroupbox.ResumeLayout(False)
        Me.ScriptsGroupbox.PerformLayout()
        Me.ModulesGroupBox.ResumeLayout(False)
        Me.ModulesGroupBox.PerformLayout()
        Me.PublicityGroupBox.ResumeLayout(False)
        Me.PublicityGroupBox.PerformLayout()
        Me.PermissionsGroupbox.ResumeLayout(False)
        Me.PermissionsGroupbox.PerformLayout()
        Me.MapGroupBox.ResumeLayout(False)
        Me.MapGroupBox.PerformLayout()
        CType(Me.MapPicture, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip2.ResumeLayout(False)
        Me.MenuStrip2.PerformLayout()
        Me.CPUTab.ResumeLayout(False)
        Me.Basics.ResumeLayout(False)
        Me.Basics.PerformLayout()
        Me.Options.ResumeLayout(False)
        Me.Maps.ResumeLayout(False)
        Me.Physics.ResumeLayout(False)
        Me.PhysicsGroupbox.ResumeLayout(False)
        Me.PhysicsGroupbox.PerformLayout()
        Me.Scripts.ResumeLayout(False)
        Me.Permissions.ResumeLayout(False)
        Me.Publicity.ResumeLayout(False)
        Me.Modules.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents RegionsGroupbox As GroupBox
    Friend WithEvents Label6 As Label
    Friend WithEvents XLabel As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents UUID As TextBox
    Friend WithEvents CoordY As TextBox
    Friend WithEvents CoordX As TextBox
    Friend WithEvents RegionName As TextBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents RadioButton4 As RadioButton
    Friend WithEvents RadioButton3 As RadioButton
    Friend WithEvents RadioButton2 As RadioButton
    Friend WithEvents RadioButton1 As RadioButton
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents SaveButton As Button
    Friend WithEvents DeleteButton As Button
    Friend WithEvents EnabledCheckBox As CheckBox
    Friend WithEvents MaxAgents As TextBox
    Friend WithEvents NonPhysPrimLabel As Label
    Friend WithEvents PhysPrimLabel As Label
    Friend WithEvents PhysicalPrimMax As TextBox
    Friend WithEvents MaxMAvatarsLabel As Label
    Friend WithEvents NonphysicalPrimMax As TextBox
    Friend WithEvents MaxNPrimsLabel As Label
    Friend WithEvents MaxPrims As TextBox
    Friend WithEvents PublicityGroupBox As GroupBox
    Friend WithEvents NoPublish As RadioButton
    Friend WithEvents PublishDefault As RadioButton
    Friend WithEvents PermissionsGroupbox As GroupBox
    Friend WithEvents Gods_Use_Default As CheckBox
    Friend WithEvents GodLevel As CheckBox
    Friend WithEvents GodManager As CheckBox
    Friend WithEvents GodEstate As CheckBox
    Friend WithEvents MapGroupBox As GroupBox
    Friend WithEvents Maps_Use_Default As RadioButton
    Friend WithEvents MapPicture As PictureBox
    Friend WithEvents MapNone As RadioButton
    Friend WithEvents MapSimple As RadioButton
    Friend WithEvents MapBetter As RadioButton
    Friend WithEvents MapBest As RadioButton
    Friend WithEvents MapGood As RadioButton
    Friend WithEvents Publish As RadioButton
    Friend WithEvents ModulesGroupBox As GroupBox
    Friend WithEvents BirdsCheckBox As CheckBox
    Friend WithEvents TidesCheckbox As CheckBox
    Friend WithEvents TPCheckBox1 As CheckBox
    Friend WithEvents DeregisterButton As Button
    Friend WithEvents SmartStartCheckBox As CheckBox
    Friend WithEvents MenuStrip2 As MenuStrip
    Friend WithEvents DisableGBCheckBox As CheckBox
    Friend WithEvents DisallowForeigners As CheckBox
    Friend WithEvents DisallowResidents As CheckBox
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
    Friend WithEvents ScriptsGroupbox As GroupBox
    Friend WithEvents ScriptDefaultButton As RadioButton
    Friend WithEvents XEngineButton As RadioButton
    Friend WithEvents YEngineButton As RadioButton
    Friend WithEvents CPUTab As TabControl
    Friend WithEvents Basics As TabPage
    Friend WithEvents Options As TabPage
    Friend WithEvents Maps As TabPage
    Friend WithEvents Scripts As TabPage
    Friend WithEvents Permissions As TabPage
    Friend WithEvents Modules As TabPage
    Friend WithEvents Publicity As TabPage
    Friend WithEvents Hyperica As LinkLabel
    Friend WithEvents Physics As TabPage
    Friend WithEvents PhysicsGroupbox As GroupBox
    Friend WithEvents Physics_Bullet As RadioButton
    Friend WithEvents Physics_Default As RadioButton
    Friend WithEvents Physics_Separate As RadioButton
    Friend WithEvents Physics_ubODE As RadioButton
    Friend WithEvents Label5 As Label
    Friend WithEvents APIKey As TextBox
    Friend WithEvents Opensimworld As LinkLabel
    Friend WithEvents YLabel As Label
    Friend WithEvents RichTextBoxModules As RichTextBox
    Friend WithEvents RichTextBoxPermissions As RichTextBox
    Friend WithEvents RichTextBoxMap As RichTextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents RichTextBoxPhysics As RichTextBox
    Friend WithEvents RichTextBoxScripts As RichTextBox
    Friend WithEvents HelpToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RichTextBoxPublicity As RichTextBox
    Friend WithEvents ClampPrimSize As CheckBox
    Friend WithEvents ClampPrimLabel As Label
    Friend WithEvents FrameRateLabel As Label
    Friend WithEvents FrametimeBox As TextBox
    Friend WithEvents ScriptRateLabel As Label
    Friend WithEvents ScriptTimerTextBox As TextBox
    Friend WithEvents RichTextBoxOptions As RichTextBox
    Friend WithEvents BasicsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OptionsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MapsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PhysicsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ScriptsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PermissionsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PublicityToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ModulesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HelpProvider1 As HelpProvider
    Friend WithEvents Label4 As Label
    Friend WithEvents RegionPort As TextBox
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label7 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents BelowNormal As RadioButton
    Friend WithEvents Normal As RadioButton
    Friend WithEvents AboveNormal As RadioButton
    Friend WithEvents High As RadioButton
    Friend WithEvents RealTime As RadioButton
    Friend WithEvents Core1Button As CheckBox
    Friend WithEvents Core16Button As CheckBox
    Friend WithEvents Core6Button As CheckBox
    Friend WithEvents Core15Button As CheckBox
    Friend WithEvents Core14Button As CheckBox
    Friend WithEvents Core13Button As CheckBox
    Friend WithEvents Core12Button As CheckBox
    Friend WithEvents Core11Button As CheckBox
    Friend WithEvents Core10Button As CheckBox
    Friend WithEvents Core9Button As CheckBox
    Friend WithEvents Core8Button As CheckBox
    Friend WithEvents Core7Button As CheckBox
    Friend WithEvents Core5Button As CheckBox
    Friend WithEvents Core4Button As CheckBox
    Friend WithEvents Core3Button As CheckBox
    Friend WithEvents Core2Button As CheckBox
    Friend WithEvents ConciergeCheckBox As CheckBox
    Friend WithEvents ScriptOffButton As RadioButton
    Friend WithEvents LandingSpotLabel As Label
    Friend WithEvents LandingSpotTextBox As TextBox
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents Physics_Hybrid As RadioButton
    Friend WithEvents SearchLabel As Label
    Friend WithEvents LinkLabel1 As LinkLabel
End Class
