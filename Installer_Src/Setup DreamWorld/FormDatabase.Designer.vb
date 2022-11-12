<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormDatabase
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormDatabase))
        Me.StandaloneGroup = New System.Windows.Forms.GroupBox()
        Me.MysqlRegionPort = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.RegionServer = New System.Windows.Forms.TextBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.RegionDbName = New System.Windows.Forms.TextBox()
        Me.RegionDBUsername = New System.Windows.Forms.TextBox()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.RegionMySqlPassword = New System.Windows.Forms.TextBox()
        Me.GridGroup = New System.Windows.Forms.GroupBox()
        Me.RobustServer = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Dbnameindex = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.RobustDbPort = New System.Windows.Forms.TextBox()
        Me.RobustDbName = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.RobustDBPassword = New System.Windows.Forms.TextBox()
        Me.RobustDBUsername = New System.Windows.Forms.TextBox()
        Me.MenuStrip2 = New System.Windows.Forms.MenuStrip()
        Me.HelpMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConnectToMySqlToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StartToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StopToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConsoleToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.RootPassword = New System.Windows.Forms.TextBox()
        Me.RunasaServiceCheckBox = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.StandaloneGroup.SuspendLayout()
        Me.GridGroup.SuspendLayout()
        Me.MenuStrip2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'StandaloneGroup
        '
        Me.StandaloneGroup.Controls.Add(Me.MysqlRegionPort)
        Me.StandaloneGroup.Controls.Add(Me.Label2)
        Me.StandaloneGroup.Controls.Add(Me.Label1)
        Me.StandaloneGroup.Controls.Add(Me.RegionServer)
        Me.StandaloneGroup.Controls.Add(Me.Label22)
        Me.StandaloneGroup.Controls.Add(Me.Label20)
        Me.StandaloneGroup.Controls.Add(Me.RegionDbName)
        Me.StandaloneGroup.Controls.Add(Me.RegionDBUsername)
        Me.StandaloneGroup.Controls.Add(Me.Label21)
        Me.StandaloneGroup.Controls.Add(Me.RegionMySqlPassword)
        Me.StandaloneGroup.Location = New System.Drawing.Point(315, 152)
        Me.StandaloneGroup.Name = "StandaloneGroup"
        Me.StandaloneGroup.Size = New System.Drawing.Size(277, 177)
        Me.StandaloneGroup.TabIndex = 2
        Me.StandaloneGroup.TabStop = False
        Me.StandaloneGroup.Text = "Region Database"
        '
        'MysqlRegionPort
        '
        Me.MysqlRegionPort.Location = New System.Drawing.Point(77, 135)
        Me.MysqlRegionPort.Name = "MysqlRegionPort"
        Me.MysqlRegionPort.Size = New System.Drawing.Size(47, 20)
        Me.MysqlRegionPort.TabIndex = 9
        Me.ToolTip1.SetToolTip(Me.MysqlRegionPort, "3306")
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(135, 135)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(64, 13)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "MySQL Port"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(133, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Region Server"
        '
        'RegionServer
        '
        Me.RegionServer.Location = New System.Drawing.Point(17, 25)
        Me.RegionServer.Name = "RegionServer"
        Me.RegionServer.Size = New System.Drawing.Size(107, 20)
        Me.RegionServer.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.RegionServer, Global.Outworldz.My.Resources.Resources.Region_ServerName)
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(135, 108)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(53, 13)
        Me.Label22.TabIndex = 7
        Me.Label22.Text = "Password"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(133, 55)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(90, 13)
        Me.Label20.TabIndex = 5
        Me.Label20.Text = "Region Database"
        '
        'RegionDbName
        '
        Me.RegionDbName.Location = New System.Drawing.Point(17, 52)
        Me.RegionDbName.Name = "RegionDbName"
        Me.RegionDbName.Size = New System.Drawing.Size(107, 20)
        Me.RegionDbName.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.RegionDbName, Global.Outworldz.My.Resources.Resources.Do_NotChange)
        '
        'RegionDBUsername
        '
        Me.RegionDBUsername.Location = New System.Drawing.Point(17, 77)
        Me.RegionDBUsername.Name = "RegionDBUsername"
        Me.RegionDBUsername.Size = New System.Drawing.Size(107, 20)
        Me.RegionDBUsername.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.RegionDBUsername, Global.Outworldz.My.Resources.Resources.Do_NotChange)
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(135, 81)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(60, 13)
        Me.Label21.TabIndex = 6
        Me.Label21.Text = "User Name"
        '
        'RegionMySqlPassword
        '
        Me.RegionMySqlPassword.Location = New System.Drawing.Point(17, 107)
        Me.RegionMySqlPassword.Name = "RegionMySqlPassword"
        Me.RegionMySqlPassword.Size = New System.Drawing.Size(107, 20)
        Me.RegionMySqlPassword.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.RegionMySqlPassword, Global.Outworldz.My.Resources.Resources.Do_NotChange)
        Me.RegionMySqlPassword.UseSystemPasswordChar = True
        '
        'GridGroup
        '
        Me.GridGroup.Controls.Add(Me.RobustServer)
        Me.GridGroup.Controls.Add(Me.Label16)
        Me.GridGroup.Controls.Add(Me.Dbnameindex)
        Me.GridGroup.Controls.Add(Me.Label9)
        Me.GridGroup.Controls.Add(Me.RobustDbPort)
        Me.GridGroup.Controls.Add(Me.RobustDbName)
        Me.GridGroup.Controls.Add(Me.Label15)
        Me.GridGroup.Controls.Add(Me.Label8)
        Me.GridGroup.Controls.Add(Me.RobustDBPassword)
        Me.GridGroup.Controls.Add(Me.RobustDBUsername)
        Me.GridGroup.Location = New System.Drawing.Point(22, 152)
        Me.GridGroup.Name = "GridGroup"
        Me.GridGroup.Size = New System.Drawing.Size(287, 177)
        Me.GridGroup.TabIndex = 1
        Me.GridGroup.TabStop = False
        Me.GridGroup.Text = "Robust"
        '
        'RobustServer
        '
        Me.RobustServer.Location = New System.Drawing.Point(6, 25)
        Me.RobustServer.Name = "RobustServer"
        Me.RobustServer.Size = New System.Drawing.Size(107, 20)
        Me.RobustServer.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.RobustServer, Global.Outworldz.My.Resources.Resources.Region_ServerName)
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(138, 26)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(41, 13)
        Me.Label16.TabIndex = 5
        Me.Label16.Text = "Robust"
        '
        'Dbnameindex
        '
        Me.Dbnameindex.AutoSize = True
        Me.Dbnameindex.Location = New System.Drawing.Point(138, 50)
        Me.Dbnameindex.Name = "Dbnameindex"
        Me.Dbnameindex.Size = New System.Drawing.Size(84, 13)
        Me.Dbnameindex.TabIndex = 6
        Me.Dbnameindex.Text = "Database Name"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(138, 102)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(53, 13)
        Me.Label9.TabIndex = 8
        Me.Label9.Text = "Password"
        '
        'RobustDbPort
        '
        Me.RobustDbPort.Location = New System.Drawing.Point(66, 125)
        Me.RobustDbPort.Name = "RobustDbPort"
        Me.RobustDbPort.Size = New System.Drawing.Size(47, 20)
        Me.RobustDbPort.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.RobustDbPort, Global.Outworldz.My.Resources.Resources.MySQL_Port_Default)
        '
        'RobustDbName
        '
        Me.RobustDbName.Location = New System.Drawing.Point(6, 50)
        Me.RobustDbName.Name = "RobustDbName"
        Me.RobustDbName.Size = New System.Drawing.Size(107, 20)
        Me.RobustDbName.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.RobustDbName, Global.Outworldz.My.Resources.Resources.Do_NotChange)
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(138, 79)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(60, 13)
        Me.Label15.TabIndex = 7
        Me.Label15.Text = "User Name"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(138, 129)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(64, 13)
        Me.Label8.TabIndex = 9
        Me.Label8.Text = "MySQL Port"
        '
        'RobustDBPassword
        '
        Me.RobustDBPassword.Location = New System.Drawing.Point(6, 102)
        Me.RobustDBPassword.Name = "RobustDBPassword"
        Me.RobustDBPassword.Size = New System.Drawing.Size(107, 20)
        Me.RobustDBPassword.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.RobustDBPassword, Global.Outworldz.My.Resources.Resources.Do_NotChange)
        Me.RobustDBPassword.UseSystemPasswordChar = True
        '
        'RobustDBUsername
        '
        Me.RobustDBUsername.Location = New System.Drawing.Point(6, 77)
        Me.RobustDBUsername.Name = "RobustDBUsername"
        Me.RobustDBUsername.Size = New System.Drawing.Size(107, 20)
        Me.RobustDBUsername.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.RobustDBUsername, Global.Outworldz.My.Resources.Resources.Do_NotChange)
        '
        'MenuStrip2
        '
        Me.MenuStrip2.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.MenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HelpMenu, Me.ConnectToMySqlToolStripMenuItem})
        Me.MenuStrip2.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip2.Name = "MenuStrip2"
        Me.MenuStrip2.Padding = New System.Windows.Forms.Padding(4, 1, 0, 1)
        Me.MenuStrip2.Size = New System.Drawing.Size(613, 30)
        Me.MenuStrip2.TabIndex = 0
        Me.MenuStrip2.Text = "0"
        '
        'HelpMenu
        '
        Me.HelpMenu.Image = Global.Outworldz.My.Resources.Resources.about
        Me.HelpMenu.Name = "HelpMenu"
        Me.HelpMenu.Size = New System.Drawing.Size(68, 28)
        Me.HelpMenu.Text = Global.Outworldz.My.Resources.Resources.Help_word
        '
        'ConnectToMySqlToolStripMenuItem
        '
        Me.ConnectToMySqlToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StartToolStripMenuItem, Me.StopToolStripMenuItem, Me.ConsoleToolStripMenuItem})
        Me.ConnectToMySqlToolStripMenuItem.Image = Global.Outworldz.My.Resources.Resources.data
        Me.ConnectToMySqlToolStripMenuItem.Name = "ConnectToMySqlToolStripMenuItem"
        Me.ConnectToMySqlToolStripMenuItem.Size = New System.Drawing.Size(138, 28)
        Me.ConnectToMySqlToolStripMenuItem.Text = "Connect to MySql"
        '
        'StartToolStripMenuItem
        '
        Me.StartToolStripMenuItem.Image = Global.Outworldz.My.Resources.Resources.gear
        Me.StartToolStripMenuItem.Name = "StartToolStripMenuItem"
        Me.StartToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.StartToolStripMenuItem.Text = "Start"
        '
        'StopToolStripMenuItem
        '
        Me.StopToolStripMenuItem.Image = Global.Outworldz.My.Resources.Resources.gear_stop
        Me.StopToolStripMenuItem.Name = "StopToolStripMenuItem"
        Me.StopToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.StopToolStripMenuItem.Text = "Stop"
        '
        'ConsoleToolStripMenuItem
        '
        Me.ConsoleToolStripMenuItem.Image = Global.Outworldz.My.Resources.Resources.document_text
        Me.ConsoleToolStripMenuItem.Name = "ConsoleToolStripMenuItem"
        Me.ConsoleToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.ConsoleToolStripMenuItem.Text = "Console"
        '
        'RootPassword
        '
        Me.RootPassword.Location = New System.Drawing.Point(20, 23)
        Me.RootPassword.Name = "RootPassword"
        Me.RootPassword.Size = New System.Drawing.Size(165, 20)
        Me.RootPassword.TabIndex = 10
        Me.ToolTip1.SetToolTip(Me.RootPassword, Global.Outworldz.My.Resources.Resources.Do_NotChange)
        Me.RootPassword.UseSystemPasswordChar = True
        '
        'RunasaServiceCheckBox
        '
        Me.RunasaServiceCheckBox.AutoSize = True
        Me.RunasaServiceCheckBox.Location = New System.Drawing.Point(28, 43)
        Me.RunasaServiceCheckBox.Name = "RunasaServiceCheckBox"
        Me.RunasaServiceCheckBox.Size = New System.Drawing.Size(108, 17)
        Me.RunasaServiceCheckBox.TabIndex = 3
        Me.RunasaServiceCheckBox.Text = "Run as a Service"
        Me.RunasaServiceCheckBox.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.RootPassword)
        Me.GroupBox1.Location = New System.Drawing.Point(22, 85)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(564, 49)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Root User"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(201, 26)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(53, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Password"
        '
        'FormDatabase
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(613, 341)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.RunasaServiceCheckBox)
        Me.Controls.Add(Me.MenuStrip2)
        Me.Controls.Add(Me.StandaloneGroup)
        Me.Controls.Add(Me.GridGroup)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "FormDatabase"
        Me.Text = "Database"
        Me.StandaloneGroup.ResumeLayout(False)
        Me.StandaloneGroup.PerformLayout()
        Me.GridGroup.ResumeLayout(False)
        Me.GridGroup.PerformLayout()
        Me.MenuStrip2.ResumeLayout(False)
        Me.MenuStrip2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents StandaloneGroup As GroupBox
    Friend WithEvents Label22 As Label
    Friend WithEvents Label20 As Label
    Friend WithEvents RegionDbName As TextBox
    Friend WithEvents RegionDBUsername As TextBox
    Friend WithEvents Label21 As Label
    Friend WithEvents RegionMySqlPassword As TextBox
    Friend WithEvents GridGroup As GroupBox
    Friend WithEvents RobustServer As TextBox
    Friend WithEvents Label16 As Label
    Friend WithEvents Dbnameindex As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents RobustDbPort As TextBox
    Friend WithEvents RobustDbName As TextBox
    Friend WithEvents Label15 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents RobustDBPassword As TextBox
    Friend WithEvents RobustDBUsername As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents RegionServer As TextBox
    Friend WithEvents MysqlRegionPort As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents MenuStrip2 As MenuStrip
    Friend WithEvents HelpMenu As ToolStripMenuItem
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents ConnectToMySqlToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents StartToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents StopToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ConsoleToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RunasaServiceCheckBox As CheckBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label3 As Label
    Friend WithEvents RootPassword As TextBox
End Class
