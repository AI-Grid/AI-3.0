﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormBackups
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
        Me.components = New System.ComponentModel.Container()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.AutoBackupHelp = New System.Windows.Forms.PictureBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.BackupFolder = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.AutoBackupKeepFilesForDays = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.AutoBackupInterval = New System.Windows.Forms.ComboBox()
        Me.AutoBackup = New System.Windows.Forms.CheckBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox3.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AutoBackupHelp, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.PictureBox1)
        Me.GroupBox3.Controls.Add(Me.AutoBackupHelp)
        Me.GroupBox3.Controls.Add(Me.Label6)
        Me.GroupBox3.Controls.Add(Me.BackupFolder)
        Me.GroupBox3.Controls.Add(Me.Label9)
        Me.GroupBox3.Controls.Add(Me.AutoBackupKeepFilesForDays)
        Me.GroupBox3.Controls.Add(Me.Label8)
        Me.GroupBox3.Controls.Add(Me.AutoBackupInterval)
        Me.GroupBox3.Controls.Add(Me.AutoBackup)
        Me.GroupBox3.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(253, 193)
        Me.GroupBox3.TabIndex = 1863
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Auto Backup"
        '
        'PictureBox1
        '
        Me.PictureBox1.BackgroundImage = Global.Outworldz.My.Resources.Resources.folder
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.PictureBox1.Location = New System.Drawing.Point(178, 120)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(42, 42)
        Me.PictureBox1.TabIndex = 1858
        Me.PictureBox1.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PictureBox1, "Click to change the folder")
        '
        'AutoBackupHelp
        '
        Me.AutoBackupHelp.Image = Global.Outworldz.My.Resources.Resources.about
        Me.AutoBackupHelp.Location = New System.Drawing.Point(154, 17)
        Me.AutoBackupHelp.Name = "AutoBackupHelp"
        Me.AutoBackupHelp.Size = New System.Drawing.Size(28, 32)
        Me.AutoBackupHelp.TabIndex = 1857
        Me.AutoBackupHelp.TabStop = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 110)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(83, 13)
        Me.Label6.TabIndex = 16
        Me.Label6.Text = "Save To Folder:"
        '
        'BackupFolder
        '
        Me.BackupFolder.Location = New System.Drawing.Point(9, 133)
        Me.BackupFolder.Name = "BackupFolder"
        Me.BackupFolder.Size = New System.Drawing.Size(163, 20)
        Me.BackupFolder.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.BackupFolder, "Normally set to ""AutoBackup"" folder in OutworldzFiles.  Click to change the folde" &
        "r")
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(6, 88)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(74, 13)
        Me.Label9.TabIndex = 14
        Me.Label9.Text = "Keep for Days"
        '
        'AutoBackupKeepFilesForDays
        '
        Me.AutoBackupKeepFilesForDays.Location = New System.Drawing.Point(125, 85)
        Me.AutoBackupKeepFilesForDays.Name = "AutoBackupKeepFilesForDays"
        Me.AutoBackupKeepFilesForDays.Size = New System.Drawing.Size(47, 20)
        Me.AutoBackupKeepFilesForDays.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.AutoBackupKeepFilesForDays, "How long to keep the backups. After this time in days, the oldest get deleted.")
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(6, 58)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(45, 13)
        Me.Label8.TabIndex = 12
        Me.Label8.Text = "Interval:"
        '
        'AutoBackupInterval
        '
        Me.AutoBackupInterval.AutoCompleteCustomSource.AddRange(New String() {"1 Hour", "4 Hour", "12 Hour", "Daily", "Weekly"})
        Me.AutoBackupInterval.FormattingEnabled = True
        Me.AutoBackupInterval.Items.AddRange(New Object() {"Hourly", "12 Hour", "Daily", "2 days", " 3 days", "4 days", "5 days", "6 days", "Weekly"})
        Me.AutoBackupInterval.Location = New System.Drawing.Point(80, 55)
        Me.AutoBackupInterval.Name = "AutoBackupInterval"
        Me.AutoBackupInterval.Size = New System.Drawing.Size(121, 21)
        Me.AutoBackupInterval.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.AutoBackupInterval, "How long Openim runs before an Auto backup occurs")
        '
        'AutoBackup
        '
        Me.AutoBackup.AutoSize = True
        Me.AutoBackup.Location = New System.Drawing.Point(22, 24)
        Me.AutoBackup.Name = "AutoBackup"
        Me.AutoBackup.Size = New System.Drawing.Size(65, 17)
        Me.AutoBackup.TabIndex = 1
        Me.AutoBackup.Text = "Enabled"
        Me.ToolTip1.SetToolTip(Me.AutoBackup, "If enabled, Openim will save OARS  after running this long")
        Me.AutoBackup.UseVisualStyleBackColor = True
        '
        'FormBackups
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(278, 218)
        Me.Controls.Add(Me.GroupBox3)
        Me.Name = "FormBackups"
        Me.Text = "Backups"
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AutoBackupHelp, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents AutoBackupHelp As PictureBox
    Friend WithEvents Label6 As Label
    Friend WithEvents BackupFolder As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents AutoBackupKeepFilesForDays As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents AutoBackupInterval As ComboBox
    Friend WithEvents AutoBackup As CheckBox
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents ToolTip1 As ToolTip
End Class
