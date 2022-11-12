<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormBackupBoxes
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormBackupBoxes))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.ShowFsassets = New System.Windows.Forms.CheckBox()
        Me.BackupIARsCheckBox = New System.Windows.Forms.CheckBox()
        Me.BackupSQlCheckBox = New System.Windows.Forms.CheckBox()
        Me.BackupOarsCheckBox = New System.Windows.Forms.CheckBox()
        Me.BackupButton = New System.Windows.Forms.Button()
        Me.CustomCheckBox = New System.Windows.Forms.CheckBox()
        Me.FSAssetsCheckBox = New System.Windows.Forms.CheckBox()
        Me.SettingsCheckbox = New System.Windows.Forms.CheckBox()
        Me.RegionCheckBox = New System.Windows.Forms.CheckBox()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.AbortButton = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.AbortButton)
        Me.GroupBox1.Controls.Add(Me.ShowFsassets)
        Me.GroupBox1.Controls.Add(Me.BackupIARsCheckBox)
        Me.GroupBox1.Controls.Add(Me.BackupSQlCheckBox)
        Me.GroupBox1.Controls.Add(Me.BackupOarsCheckBox)
        Me.GroupBox1.Controls.Add(Me.BackupButton)
        Me.GroupBox1.Controls.Add(Me.CustomCheckBox)
        Me.GroupBox1.Controls.Add(Me.FSAssetsCheckBox)
        Me.GroupBox1.Controls.Add(Me.SettingsCheckbox)
        Me.GroupBox1.Controls.Add(Me.RegionCheckBox)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 33)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(346, 222)
        Me.GroupBox1.TabIndex = 6
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Backup"
        '
        'ShowFsassets
        '
        Me.ShowFsassets.AutoSize = True
        Me.ShowFsassets.Checked = True
        Me.ShowFsassets.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ShowFsassets.Location = New System.Drawing.Point(211, 68)
        Me.ShowFsassets.Name = "ShowFsassets"
        Me.ShowFsassets.Size = New System.Drawing.Size(53, 17)
        Me.ShowFsassets.TabIndex = 12
        Me.ShowFsassets.Text = "Show"
        Me.ShowFsassets.UseVisualStyleBackColor = True
        '
        'BackupIARsCheckBox
        '
        Me.BackupIARsCheckBox.AutoSize = True
        Me.BackupIARsCheckBox.Checked = True
        Me.BackupIARsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.BackupIARsCheckBox.Location = New System.Drawing.Point(23, 137)
        Me.BackupIARsCheckBox.Name = "BackupIARsCheckBox"
        Me.BackupIARsCheckBox.Size = New System.Drawing.Size(89, 17)
        Me.BackupIARsCheckBox.TabIndex = 11
        Me.BackupIARsCheckBox.Text = "Backup IARs"
        Me.BackupIARsCheckBox.UseVisualStyleBackColor = True
        '
        'BackupSQlCheckBox
        '
        Me.BackupSQlCheckBox.AutoSize = True
        Me.BackupSQlCheckBox.Checked = True
        Me.BackupSQlCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.BackupSQlCheckBox.Location = New System.Drawing.Point(23, 158)
        Me.BackupSQlCheckBox.Name = "BackupSQlCheckBox"
        Me.BackupSQlCheckBox.Size = New System.Drawing.Size(87, 17)
        Me.BackupSQlCheckBox.TabIndex = 6
        Me.BackupSQlCheckBox.Text = "Backup SQL"
        Me.BackupSQlCheckBox.UseVisualStyleBackColor = True
        '
        'BackupOarsCheckBox
        '
        Me.BackupOarsCheckBox.AutoSize = True
        Me.BackupOarsCheckBox.Checked = True
        Me.BackupOarsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.BackupOarsCheckBox.Location = New System.Drawing.Point(23, 114)
        Me.BackupOarsCheckBox.Name = "BackupOarsCheckBox"
        Me.BackupOarsCheckBox.Size = New System.Drawing.Size(94, 17)
        Me.BackupOarsCheckBox.TabIndex = 5
        Me.BackupOarsCheckBox.Text = "Backup OARs"
        Me.BackupOarsCheckBox.UseVisualStyleBackColor = True
        '
        'BackupButton
        '
        Me.BackupButton.Location = New System.Drawing.Point(23, 193)
        Me.BackupButton.Name = "BackupButton"
        Me.BackupButton.Size = New System.Drawing.Size(110, 23)
        Me.BackupButton.TabIndex = 10
        Me.BackupButton.Text = Global.Outworldz.My.Resources.Resources.Backup_word
        Me.BackupButton.UseVisualStyleBackColor = True
        '
        'CustomCheckBox
        '
        Me.CustomCheckBox.AutoSize = True
        Me.CustomCheckBox.Checked = True
        Me.CustomCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CustomCheckBox.Location = New System.Drawing.Point(23, 90)
        Me.CustomCheckBox.Name = "CustomCheckBox"
        Me.CustomCheckBox.Size = New System.Drawing.Size(160, 17)
        Me.CustomCheckBox.TabIndex = 4
        Me.CustomCheckBox.Text = Global.Outworldz.My.Resources.Resources.Backup_Custom
        Me.CustomCheckBox.UseVisualStyleBackColor = True
        '
        'FSAssetsCheckBox
        '
        Me.FSAssetsCheckBox.AutoSize = True
        Me.FSAssetsCheckBox.Checked = True
        Me.FSAssetsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.FSAssetsCheckBox.Location = New System.Drawing.Point(23, 68)
        Me.FSAssetsCheckBox.Name = "FSAssetsCheckBox"
        Me.FSAssetsCheckBox.Size = New System.Drawing.Size(139, 17)
        Me.FSAssetsCheckBox.TabIndex = 3
        Me.FSAssetsCheckBox.Text = Global.Outworldz.My.Resources.Resources.Backup_FSAssets
        Me.FSAssetsCheckBox.UseVisualStyleBackColor = True
        '
        'SettingsCheckbox
        '
        Me.SettingsCheckbox.AutoSize = True
        Me.SettingsCheckbox.Checked = True
        Me.SettingsCheckbox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.SettingsCheckbox.Location = New System.Drawing.Point(23, 20)
        Me.SettingsCheckbox.Name = "SettingsCheckbox"
        Me.SettingsCheckbox.Size = New System.Drawing.Size(104, 17)
        Me.SettingsCheckbox.TabIndex = 1
        Me.SettingsCheckbox.Text = "Backup Settings"
        Me.SettingsCheckbox.UseVisualStyleBackColor = True
        '
        'RegionCheckBox
        '
        Me.RegionCheckBox.AutoSize = True
        Me.RegionCheckBox.Checked = True
        Me.RegionCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.RegionCheckBox.Location = New System.Drawing.Point(23, 44)
        Me.RegionCheckBox.Name = "RegionCheckBox"
        Me.RegionCheckBox.Size = New System.Drawing.Size(138, 17)
        Me.RegionCheckBox.TabIndex = 2
        Me.RegionCheckBox.Text = Global.Outworldz.My.Resources.Resources.Backup_Region
        Me.RegionCheckBox.UseVisualStyleBackColor = True
        '
        'MenuStrip1
        '
        Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Padding = New System.Windows.Forms.Padding(4, 1, 0, 1)
        Me.MenuStrip1.Size = New System.Drawing.Size(377, 30)
        Me.MenuStrip1.TabIndex = 18602
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.Image = Global.Outworldz.My.Resources.Resources.about
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(68, 28)
        Me.HelpToolStripMenuItem.Text = Global.Outworldz.My.Resources.Resources.Help_word
        '
        'AbortButton
        '
        Me.AbortButton.Location = New System.Drawing.Point(174, 193)
        Me.AbortButton.Name = "AbortButton"
        Me.AbortButton.Size = New System.Drawing.Size(110, 23)
        Me.AbortButton.TabIndex = 13
        Me.AbortButton.Text = "Abort"
        Me.AbortButton.UseVisualStyleBackColor = True
        '
        'FormBackupBoxes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(377, 270)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FormBackupBoxes"
        Me.RightToLeftLayout = True
        Me.Text = "System Backup"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents BackupButton As Button
    Friend WithEvents CustomCheckBox As CheckBox
    Friend WithEvents FSAssetsCheckBox As CheckBox
    Friend WithEvents SettingsCheckbox As CheckBox
    Friend WithEvents RegionCheckBox As CheckBox
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents HelpToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BackupOarsCheckBox As CheckBox
    Friend WithEvents BackupSQlCheckBox As CheckBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents BackupIARsCheckBox As CheckBox
    Friend WithEvents ShowFsassets As CheckBox
    Friend WithEvents AbortButton As Button
End Class
