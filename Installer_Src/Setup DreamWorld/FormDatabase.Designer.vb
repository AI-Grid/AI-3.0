﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormDatabase
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormDatabase))
        Me.StandaloneGroup = New System.Windows.Forms.GroupBox()
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
        Me.BirdHelp = New System.Windows.Forms.PictureBox()
        Me.StandaloneGroup.SuspendLayout()
        Me.GridGroup.SuspendLayout()
        CType(Me.BirdHelp, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'StandaloneGroup
        '
        Me.StandaloneGroup.Controls.Add(Me.Label22)
        Me.StandaloneGroup.Controls.Add(Me.Label20)
        Me.StandaloneGroup.Controls.Add(Me.RegionDbName)
        Me.StandaloneGroup.Controls.Add(Me.RegionDBUsername)
        Me.StandaloneGroup.Controls.Add(Me.Label21)
        Me.StandaloneGroup.Controls.Add(Me.RegionMySqlPassword)
        Me.StandaloneGroup.Location = New System.Drawing.Point(272, 51)
        Me.StandaloneGroup.Name = "StandaloneGroup"
        Me.StandaloneGroup.Size = New System.Drawing.Size(243, 119)
        Me.StandaloneGroup.TabIndex = 56
        Me.StandaloneGroup.TabStop = False
        Me.StandaloneGroup.Text = "Local Region Database"
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(17, 79)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(53, 13)
        Me.Label22.TabIndex = 17
        Me.Label22.Text = "Password"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(16, 26)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(53, 13)
        Me.Label20.TabIndex = 13
        Me.Label20.Text = "DB Name"
        '
        'RegionDbName
        '
        Me.RegionDbName.Location = New System.Drawing.Point(120, 26)
        Me.RegionDbName.Name = "RegionDbName"
        Me.RegionDbName.Size = New System.Drawing.Size(107, 20)
        Me.RegionDbName.TabIndex = 42
        '
        'RegionDBUsername
        '
        Me.RegionDBUsername.Location = New System.Drawing.Point(120, 52)
        Me.RegionDBUsername.Name = "RegionDBUsername"
        Me.RegionDBUsername.Size = New System.Drawing.Size(107, 20)
        Me.RegionDBUsername.TabIndex = 43
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(17, 52)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(55, 13)
        Me.Label21.TabIndex = 16
        Me.Label21.Text = "Username"
        '
        'RegionMySqlPassword
        '
        Me.RegionMySqlPassword.Location = New System.Drawing.Point(120, 81)
        Me.RegionMySqlPassword.Name = "RegionMySqlPassword"
        Me.RegionMySqlPassword.Size = New System.Drawing.Size(107, 20)
        Me.RegionMySqlPassword.TabIndex = 44
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
        Me.GridGroup.Location = New System.Drawing.Point(8, 10)
        Me.GridGroup.Name = "GridGroup"
        Me.GridGroup.Size = New System.Drawing.Size(243, 162)
        Me.GridGroup.TabIndex = 55
        Me.GridGroup.TabStop = False
        Me.GridGroup.Text = "Robust Database"
        '
        'RobustServer
        '
        Me.RobustServer.Location = New System.Drawing.Point(119, 25)
        Me.RobustServer.Name = "RobustServer"
        Me.RobustServer.Size = New System.Drawing.Size(107, 20)
        Me.RobustServer.TabIndex = 37
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(17, 27)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(75, 13)
        Me.Label16.TabIndex = 38
        Me.Label16.Text = "Robust Server"
        '
        'Dbnameindex
        '
        Me.Dbnameindex.AutoSize = True
        Me.Dbnameindex.Location = New System.Drawing.Point(17, 51)
        Me.Dbnameindex.Name = "Dbnameindex"
        Me.Dbnameindex.Size = New System.Drawing.Size(72, 13)
        Me.Dbnameindex.TabIndex = 35
        Me.Dbnameindex.Text = "Robust Name"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(15, 103)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(53, 13)
        Me.Label9.TabIndex = 35
        Me.Label9.Text = "Password"
        '
        'RobustDbPort
        '
        Me.RobustDbPort.Location = New System.Drawing.Point(123, 129)
        Me.RobustDbPort.Name = "RobustDbPort"
        Me.RobustDbPort.Size = New System.Drawing.Size(47, 20)
        Me.RobustDbPort.TabIndex = 41
        '
        'RobustDbName
        '
        Me.RobustDbName.Location = New System.Drawing.Point(119, 51)
        Me.RobustDbName.Name = "RobustDbName"
        Me.RobustDbName.Size = New System.Drawing.Size(107, 20)
        Me.RobustDbName.TabIndex = 38
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(15, 77)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(55, 13)
        Me.Label15.TabIndex = 34
        Me.Label15.Text = "Username"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(17, 129)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(58, 13)
        Me.Label8.TabIndex = 32
        Me.Label8.Text = "MySql Port"
        '
        'RobustDBPassword
        '
        Me.RobustDBPassword.Location = New System.Drawing.Point(121, 103)
        Me.RobustDBPassword.Name = "RobustDBPassword"
        Me.RobustDBPassword.Size = New System.Drawing.Size(107, 20)
        Me.RobustDBPassword.TabIndex = 40
        Me.RobustDBPassword.UseSystemPasswordChar = True
        '
        'RobustDBUsername
        '
        Me.RobustDBUsername.Location = New System.Drawing.Point(121, 77)
        Me.RobustDBUsername.Name = "RobustDBUsername"
        Me.RobustDBUsername.Size = New System.Drawing.Size(107, 20)
        Me.RobustDBUsername.TabIndex = 39
        '
        'BirdHelp
        '
        Me.BirdHelp.Image = Global.Outworldz.My.Resources.Resources.about
        Me.BirdHelp.Location = New System.Drawing.Point(291, 10)
        Me.BirdHelp.Name = "BirdHelp"
        Me.BirdHelp.Size = New System.Drawing.Size(28, 32)
        Me.BirdHelp.TabIndex = 1859
        Me.BirdHelp.TabStop = False
        '
        'FormDatabase
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(543, 182)
        Me.Controls.Add(Me.BirdHelp)
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
        CType(Me.BirdHelp, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

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
    Friend WithEvents BirdHelp As PictureBox
End Class
