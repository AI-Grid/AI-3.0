
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormRegionlist
    Inherits System.Windows.Forms.Form


    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim ListViewItem1 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("")
        Dim ListViewItem2 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("")
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormRegionlist))
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.IconView = New System.Windows.Forms.ListView()
        Me.RefreshButton = New System.Windows.Forms.Button()
        Me.ExportButton = New System.Windows.Forms.Button()
        Me.AllNone = New System.Windows.Forms.CheckBox()
        Me.AvatarView = New System.Windows.Forms.ListView()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.UserView = New System.Windows.Forms.ListView()
        Me.OffButton = New System.Windows.Forms.RadioButton()
        Me.OnButton = New System.Windows.Forms.RadioButton()
        Me.SmartButton = New System.Windows.Forms.RadioButton()
        Me.AllButton = New System.Windows.Forms.RadioButton()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SearchBox = New System.Windows.Forms.TextBox()
        Me.Bootedbutton = New System.Windows.Forms.RadioButton()
        Me.StoppedButton = New System.Windows.Forms.RadioButton()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.KOT = New System.Windows.Forms.ToolStripMenuItem()
        Me.OnTopToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FloatToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShowUponBootToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RestartButton = New System.Windows.Forms.Button()
        Me.DetailsButton = New System.Windows.Forms.Button()
        Me.StopAllButton = New System.Windows.Forms.Button()
        Me.RunAllButton = New System.Windows.Forms.Button()
        Me.AddRegionButton = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.AvatarsButton = New System.Windows.Forms.Button()
        Me.IconsButton = New System.Windows.Forms.Button()
        Me.Emails = New System.Windows.Forms.Button()
        Me.Users = New System.Windows.Forms.Button()
        Me.ImportButton = New System.Windows.Forms.Button()
        Me.MenuStrip1.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Timer1
        '
        '
        'ListView1
        '
        Me.ListView1.AllowColumnReorder = True
        Me.ListView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView1.FullRowSelect = True
        Me.ListView1.GridLines = True
        Me.ListView1.HideSelection = False
        Me.ListView1.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem1})
        Me.ListView1.Location = New System.Drawing.Point(12, 160)
        Me.ListView1.MultiSelect = False
        Me.ListView1.Name = "ListView1"
        Me.ListView1.ShowItemToolTips = True
        Me.ListView1.Size = New System.Drawing.Size(1371, 154)
        Me.ListView1.TabIndex = 5
        Me.ToolTip1.SetToolTip(Me.ListView1, Global.Outworldz.My.Resources.Resources.ClickStartStoptxt)
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'ToolTip1
        '
        Me.ToolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info
        '
        'IconView
        '
        Me.IconView.AllowColumnReorder = True
        Me.IconView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.IconView.Font = New System.Drawing.Font("Lucida Console", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.IconView.FullRowSelect = True
        Me.IconView.HideSelection = False
        Me.IconView.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem2})
        Me.IconView.Location = New System.Drawing.Point(13, 160)
        Me.IconView.MultiSelect = False
        Me.IconView.Name = "IconView"
        Me.IconView.ShowItemToolTips = True
        Me.IconView.Size = New System.Drawing.Size(1370, 154)
        Me.IconView.TabIndex = 18609
        Me.ToolTip1.SetToolTip(Me.IconView, "Icons")
        Me.IconView.UseCompatibleStateImageBehavior = False
        Me.IconView.View = System.Windows.Forms.View.SmallIcon
        '
        'RefreshButton
        '
        Me.RefreshButton.AutoSize = True
        Me.RefreshButton.Image = Global.Outworldz.My.Resources.Resources.refresh
        Me.RefreshButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.RefreshButton.Location = New System.Drawing.Point(200, 3)
        Me.RefreshButton.MinimumSize = New System.Drawing.Size(83, 26)
        Me.RefreshButton.Name = "RefreshButton"
        Me.RefreshButton.Padding = New System.Windows.Forms.Padding(2)
        Me.RefreshButton.Size = New System.Drawing.Size(144, 34)
        Me.RefreshButton.TabIndex = 0
        Me.RefreshButton.Text = Global.Outworldz.My.Resources.Resources.Refresh_word
        Me.ToolTip1.SetToolTip(Me.RefreshButton, Global.Outworldz.My.Resources.Resources.Reload)
        Me.RefreshButton.UseVisualStyleBackColor = True
        '
        'ExportButton
        '
        Me.ExportButton.AutoSize = True
        Me.ExportButton.Image = Global.Outworldz.My.Resources.Resources.xls_icon_3398
        Me.ExportButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ExportButton.Location = New System.Drawing.Point(500, 83)
        Me.ExportButton.MinimumSize = New System.Drawing.Size(83, 26)
        Me.ExportButton.Name = "ExportButton"
        Me.ExportButton.Padding = New System.Windows.Forms.Padding(2)
        Me.ExportButton.Size = New System.Drawing.Size(144, 34)
        Me.ExportButton.TabIndex = 10
        Me.ExportButton.Text = "Export"
        Me.ToolTip1.SetToolTip(Me.ExportButton, "Export this list")
        Me.ExportButton.UseVisualStyleBackColor = True
        '
        'AllNone
        '
        Me.AllNone.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.AllNone.AutoSize = True
        Me.AllNone.Location = New System.Drawing.Point(3, 94)
        Me.AllNone.Name = "AllNone"
        Me.AllNone.Size = New System.Drawing.Size(68, 17)
        Me.AllNone.TabIndex = 4
        Me.AllNone.Text = "All/None"
        Me.AllNone.UseVisualStyleBackColor = True
        '
        'AvatarView
        '
        Me.AvatarView.AllowColumnReorder = True
        Me.AvatarView.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AvatarView.FullRowSelect = True
        Me.AvatarView.GridLines = True
        Me.AvatarView.HideSelection = False
        Me.AvatarView.Location = New System.Drawing.Point(13, 160)
        Me.AvatarView.MultiSelect = False
        Me.AvatarView.Name = "AvatarView"
        Me.AvatarView.ShowItemToolTips = True
        Me.AvatarView.Size = New System.Drawing.Size(1370, 154)
        Me.AvatarView.TabIndex = 18597
        Me.AvatarView.UseCompatibleStateImageBehavior = False
        Me.AvatarView.View = System.Windows.Forms.View.Details
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label1.Location = New System.Drawing.Point(271, 93)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(0, 25)
        Me.Label1.TabIndex = 18606
        '
        'UserView
        '
        Me.UserView.AllowColumnReorder = True
        Me.UserView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UserView.FullRowSelect = True
        Me.UserView.GridLines = True
        Me.UserView.HideSelection = False
        Me.UserView.Location = New System.Drawing.Point(13, 160)
        Me.UserView.MultiSelect = False
        Me.UserView.Name = "UserView"
        Me.UserView.ShowItemToolTips = True
        Me.UserView.Size = New System.Drawing.Size(1370, 154)
        Me.UserView.TabIndex = 18608
        Me.UserView.UseCompatibleStateImageBehavior = False
        Me.UserView.View = System.Windows.Forms.View.Details
        '
        'OffButton
        '
        Me.OffButton.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.OffButton.AutoSize = True
        Me.OffButton.Checked = True
        Me.OffButton.Location = New System.Drawing.Point(77, 51)
        Me.OffButton.Name = "OffButton"
        Me.OffButton.Size = New System.Drawing.Size(39, 17)
        Me.OffButton.TabIndex = 18610
        Me.OffButton.TabStop = True
        Me.OffButton.Text = "Off"
        Me.OffButton.UseVisualStyleBackColor = True
        '
        'OnButton
        '
        Me.OnButton.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.OnButton.AutoSize = True
        Me.OnButton.Location = New System.Drawing.Point(77, 11)
        Me.OnButton.Name = "OnButton"
        Me.OnButton.Size = New System.Drawing.Size(39, 17)
        Me.OnButton.TabIndex = 18611
        Me.OnButton.Text = "On"
        Me.OnButton.UseVisualStyleBackColor = True
        '
        'SmartButton
        '
        Me.SmartButton.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.SmartButton.AutoSize = True
        Me.SmartButton.Location = New System.Drawing.Point(3, 51)
        Me.SmartButton.Name = "SmartButton"
        Me.SmartButton.Size = New System.Drawing.Size(52, 17)
        Me.SmartButton.TabIndex = 18612
        Me.SmartButton.Text = "Smart"
        Me.SmartButton.UseVisualStyleBackColor = True
        '
        'AllButton
        '
        Me.AllButton.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.AllButton.AutoSize = True
        Me.AllButton.Location = New System.Drawing.Point(3, 11)
        Me.AllButton.Name = "AllButton"
        Me.AllButton.Size = New System.Drawing.Size(36, 17)
        Me.AllButton.TabIndex = 18613
        Me.AllButton.Text = "All"
        Me.AllButton.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(61, 4)
        '
        'SearchBox
        '
        Me.SearchBox.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.TableLayoutPanel1.SetColumnSpan(Me.SearchBox, 2)
        Me.SearchBox.Location = New System.Drawing.Point(77, 93)
        Me.SearchBox.Name = "SearchBox"
        Me.SearchBox.Size = New System.Drawing.Size(117, 20)
        Me.SearchBox.TabIndex = 18615
        '
        'Bootedbutton
        '
        Me.Bootedbutton.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Bootedbutton.AutoSize = True
        Me.Bootedbutton.Location = New System.Drawing.Point(122, 11)
        Me.Bootedbutton.Name = "Bootedbutton"
        Me.Bootedbutton.Size = New System.Drawing.Size(65, 17)
        Me.Bootedbutton.TabIndex = 18616
        Me.Bootedbutton.Text = "Running"
        Me.Bootedbutton.UseVisualStyleBackColor = True
        '
        'StoppedButton
        '
        Me.StoppedButton.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.StoppedButton.AutoSize = True
        Me.StoppedButton.Location = New System.Drawing.Point(122, 51)
        Me.StoppedButton.Name = "StoppedButton"
        Me.StoppedButton.Size = New System.Drawing.Size(65, 17)
        Me.StoppedButton.TabIndex = 18617
        Me.StoppedButton.Text = "Stopped"
        Me.StoppedButton.UseVisualStyleBackColor = True
        '
        'MenuStrip1
        '
        Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HelpToolStripMenuItem, Me.KOT})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Padding = New System.Windows.Forms.Padding(4, 1, 0, 1)
        Me.MenuStrip1.Size = New System.Drawing.Size(1404, 30)
        Me.MenuStrip1.TabIndex = 18618
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.Image = Global.Outworldz.My.Resources.Resources.about
        Me.HelpToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(68, 28)
        Me.HelpToolStripMenuItem.Text = Global.Outworldz.My.Resources.Resources.Help_word
        '
        'KOT
        '
        Me.KOT.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OnTopToolStripMenuItem, Me.FloatToolStripMenuItem, Me.ShowUponBootToolStripMenuItem})
        Me.KOT.Image = Global.Outworldz.My.Resources.Resources.tables
        Me.KOT.Name = "KOT"
        Me.KOT.Size = New System.Drawing.Size(83, 28)
        Me.KOT.Text = Global.Outworldz.My.Resources.Resources.Window
        '
        'OnTopToolStripMenuItem
        '
        Me.OnTopToolStripMenuItem.Image = Global.Outworldz.My.Resources.Resources.tables
        Me.OnTopToolStripMenuItem.Name = "OnTopToolStripMenuItem"
        Me.OnTopToolStripMenuItem.Size = New System.Drawing.Size(162, 22)
        Me.OnTopToolStripMenuItem.Text = Global.Outworldz.My.Resources.Resources.On_Top
        '
        'FloatToolStripMenuItem
        '
        Me.FloatToolStripMenuItem.Image = Global.Outworldz.My.Resources.Resources.table
        Me.FloatToolStripMenuItem.Name = "FloatToolStripMenuItem"
        Me.FloatToolStripMenuItem.Size = New System.Drawing.Size(162, 22)
        Me.FloatToolStripMenuItem.Text = Global.Outworldz.My.Resources.Resources.Float
        '
        'ShowUponBootToolStripMenuItem
        '
        Me.ShowUponBootToolStripMenuItem.Name = "ShowUponBootToolStripMenuItem"
        Me.ShowUponBootToolStripMenuItem.Size = New System.Drawing.Size(162, 22)
        Me.ShowUponBootToolStripMenuItem.Text = "Show upon Boot"
        '
        'RestartButton
        '
        Me.RestartButton.AutoSize = True
        Me.RestartButton.Image = Global.Outworldz.My.Resources.Resources.refresh
        Me.RestartButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.RestartButton.Location = New System.Drawing.Point(200, 83)
        Me.RestartButton.MinimumSize = New System.Drawing.Size(83, 26)
        Me.RestartButton.Name = "RestartButton"
        Me.RestartButton.Padding = New System.Windows.Forms.Padding(2)
        Me.RestartButton.Size = New System.Drawing.Size(144, 34)
        Me.RestartButton.TabIndex = 8
        Me.RestartButton.Text = "Restart"
        Me.RestartButton.UseVisualStyleBackColor = True
        '
        'DetailsButton
        '
        Me.DetailsButton.AutoSize = True
        Me.DetailsButton.Image = Global.Outworldz.My.Resources.Resources.text_marked
        Me.DetailsButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.DetailsButton.Location = New System.Drawing.Point(350, 3)
        Me.DetailsButton.MinimumSize = New System.Drawing.Size(83, 26)
        Me.DetailsButton.Name = "DetailsButton"
        Me.DetailsButton.Padding = New System.Windows.Forms.Padding(2)
        Me.DetailsButton.Size = New System.Drawing.Size(144, 34)
        Me.DetailsButton.TabIndex = 1
        Me.DetailsButton.Text = "Details"
        Me.DetailsButton.UseVisualStyleBackColor = True
        '
        'StopAllButton
        '
        Me.StopAllButton.AutoSize = True
        Me.StopAllButton.Image = Global.Outworldz.My.Resources.Resources.media_stop
        Me.StopAllButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.StopAllButton.Location = New System.Drawing.Point(350, 83)
        Me.StopAllButton.MinimumSize = New System.Drawing.Size(83, 26)
        Me.StopAllButton.Name = "StopAllButton"
        Me.StopAllButton.Padding = New System.Windows.Forms.Padding(2)
        Me.StopAllButton.Size = New System.Drawing.Size(144, 34)
        Me.StopAllButton.TabIndex = 7
        Me.StopAllButton.Text = "Stop"
        Me.StopAllButton.UseVisualStyleBackColor = True
        '
        'RunAllButton
        '
        Me.RunAllButton.AutoSize = True
        Me.RunAllButton.Image = Global.Outworldz.My.Resources.Resources.media_play
        Me.RunAllButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.RunAllButton.Location = New System.Drawing.Point(350, 43)
        Me.RunAllButton.MinimumSize = New System.Drawing.Size(83, 26)
        Me.RunAllButton.Name = "RunAllButton"
        Me.RunAllButton.Padding = New System.Windows.Forms.Padding(2)
        Me.RunAllButton.Size = New System.Drawing.Size(144, 34)
        Me.RunAllButton.TabIndex = 6
        Me.RunAllButton.Text = "Run"
        Me.RunAllButton.UseVisualStyleBackColor = True
        '
        'AddRegionButton
        '
        Me.AddRegionButton.AutoSize = True
        Me.AddRegionButton.Image = Global.Outworldz.My.Resources.Resources.package_add
        Me.AddRegionButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.AddRegionButton.Location = New System.Drawing.Point(200, 43)
        Me.AddRegionButton.MinimumSize = New System.Drawing.Size(83, 26)
        Me.AddRegionButton.Name = "AddRegionButton"
        Me.AddRegionButton.Padding = New System.Windows.Forms.Padding(2)
        Me.AddRegionButton.Size = New System.Drawing.Size(144, 34)
        Me.AddRegionButton.TabIndex = 5
        Me.AddRegionButton.Text = Global.Outworldz.My.Resources.Resources.Add_word
        Me.AddRegionButton.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.AutoSize = True
        Me.TableLayoutPanel1.ColumnCount = 11
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.Controls.Add(Me.DetailsButton, 4, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.AllButton, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Bootedbutton, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.OnButton, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.RefreshButton, 3, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.SmartButton, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.SearchBox, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.OffButton, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.StoppedButton, 2, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.AddRegionButton, 3, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.RunAllButton, 4, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.StopAllButton, 4, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.RestartButton, 3, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.AllNone, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.PictureBox1, 10, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.AvatarsButton, 9, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.IconsButton, 5, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ExportButton, 5, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Emails, 9, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Users, 9, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.ImportButton, 5, 1)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(22, 34)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 3
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(913, 126)
        Me.TableLayoutPanel1.TabIndex = 2
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.Image = Global.Outworldz.My.Resources.Resources.loader
        Me.PictureBox1.Location = New System.Drawing.Point(800, 43)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(38, 34)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 18619
        Me.PictureBox1.TabStop = False
        '
        'AvatarsButton
        '
        Me.AvatarsButton.AutoSize = True
        Me.AvatarsButton.Image = Global.Outworldz.My.Resources.Resources.users2
        Me.AvatarsButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.AvatarsButton.Location = New System.Drawing.Point(650, 3)
        Me.AvatarsButton.MinimumSize = New System.Drawing.Size(83, 26)
        Me.AvatarsButton.Name = "AvatarsButton"
        Me.AvatarsButton.Padding = New System.Windows.Forms.Padding(2)
        Me.AvatarsButton.Size = New System.Drawing.Size(144, 34)
        Me.AvatarsButton.TabIndex = 3
        Me.AvatarsButton.Text = "Avatars"
        Me.AvatarsButton.UseVisualStyleBackColor = True
        '
        'IconsButton
        '
        Me.IconsButton.AutoSize = True
        Me.IconsButton.Image = Global.Outworldz.My.Resources.Resources.transform
        Me.IconsButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.IconsButton.Location = New System.Drawing.Point(500, 3)
        Me.IconsButton.MinimumSize = New System.Drawing.Size(83, 26)
        Me.IconsButton.Name = "IconsButton"
        Me.IconsButton.Padding = New System.Windows.Forms.Padding(2)
        Me.IconsButton.Size = New System.Drawing.Size(144, 34)
        Me.IconsButton.TabIndex = 2
        Me.IconsButton.Text = "Icons"
        Me.IconsButton.UseVisualStyleBackColor = True
        '
        'Emails
        '
        Me.Emails.Image = Global.Outworldz.My.Resources.Resources.mail_into
        Me.Emails.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Emails.Location = New System.Drawing.Point(649, 82)
        Me.Emails.Margin = New System.Windows.Forms.Padding(2)
        Me.Emails.Name = "Emails"
        Me.Emails.Size = New System.Drawing.Size(145, 34)
        Me.Emails.TabIndex = 11
        Me.Emails.Text = "Email"
        Me.Emails.UseVisualStyleBackColor = True
        '
        'Users
        '
        Me.Users.Image = Global.Outworldz.My.Resources.Resources.users3
        Me.Users.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Users.Location = New System.Drawing.Point(649, 42)
        Me.Users.Margin = New System.Windows.Forms.Padding(2)
        Me.Users.Name = "Users"
        Me.Users.Size = New System.Drawing.Size(145, 34)
        Me.Users.TabIndex = 4
        Me.Users.Text = "Users"
        Me.Users.UseVisualStyleBackColor = True
        '
        'ImportButton
        '
        Me.ImportButton.AutoSize = True
        Me.ImportButton.Image = Global.Outworldz.My.Resources.Resources.package
        Me.ImportButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ImportButton.Location = New System.Drawing.Point(500, 43)
        Me.ImportButton.MinimumSize = New System.Drawing.Size(83, 26)
        Me.ImportButton.Name = "ImportButton"
        Me.ImportButton.Padding = New System.Windows.Forms.Padding(2)
        Me.ImportButton.Size = New System.Drawing.Size(144, 34)
        Me.ImportButton.TabIndex = 9
        Me.ImportButton.Text = "Import INI"
        Me.ImportButton.UseVisualStyleBackColor = True
        '
        'FormRegionlist
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1404, 338)
        Me.Controls.Add(Me.AvatarView)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.IconView)
        Me.Controls.Add(Me.UserView)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.ListView1)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "FormRegionlist"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Region List"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Timer1 As Timer
    Friend WithEvents ListView1 As ListView
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents AllNone As CheckBox
    Friend WithEvents AvatarView As ListView
    Friend WithEvents HelpToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Label1 As Label
    Friend WithEvents KOT As ToolStripMenuItem
    Friend WithEvents OnTopToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FloatToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents UserView As ListView
    Friend WithEvents IconView As ListView
    Friend WithEvents OffButton As RadioButton
    Friend WithEvents OnButton As RadioButton
    Friend WithEvents SmartButton As RadioButton
    Friend WithEvents AllButton As RadioButton
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents SearchBox As TextBox
    Friend WithEvents Bootedbutton As RadioButton
    Friend WithEvents StoppedButton As RadioButton
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents ShowUponBootToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RestartButton As Button
    Friend WithEvents DetailsButton As Button
    Friend WithEvents RefreshButton As Button
    Friend WithEvents StopAllButton As Button
    Friend WithEvents RunAllButton As Button
    Friend WithEvents AddRegionButton As Button
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents AvatarsButton As Button
    Friend WithEvents ImportButton As Button
    Friend WithEvents ExportButton As Button
    Friend WithEvents IconsButton As Button
    Friend WithEvents Users As Button
    Friend WithEvents Emails As Button
    Friend WithEvents PictureBox1 As PictureBox
End Class
