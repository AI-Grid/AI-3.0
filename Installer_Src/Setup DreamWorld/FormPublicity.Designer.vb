﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormPublicity
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormPublicity))
        Me.GroupBox11 = New System.Windows.Forms.GroupBox()
        Me.GDPRCheckBox = New System.Windows.Forms.CheckBox()
        Me.PictureBox9 = New System.Windows.Forms.PictureBox()
        Me.MenuStrip2 = New System.Windows.Forms.MenuStrip()
        Me.ToolStripMenuItem30 = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.CategoryCheckbox = New System.Windows.Forms.CheckedListBox()
        Me.FileSystemWatcher1 = New System.IO.FileSystemWatcher()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.DescriptionBox = New System.Windows.Forms.TextBox()
        Me.GroupBox11.SuspendLayout()
        CType(Me.PictureBox9, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.FileSystemWatcher1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox11
        '
        Me.GroupBox11.Controls.Add(Me.GDPRCheckBox)
        Me.GroupBox11.Controls.Add(Me.PictureBox9)
        Me.GroupBox11.Location = New System.Drawing.Point(21, 52)
        Me.GroupBox11.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.GroupBox11.Name = "GroupBox11"
        Me.GroupBox11.Padding = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.GroupBox11.Size = New System.Drawing.Size(387, 329)
        Me.GroupBox11.TabIndex = 1866
        Me.GroupBox11.TabStop = False
        Me.GroupBox11.Text = "Photo"
        '
        'GDPRCheckBox
        '
        Me.GDPRCheckBox.AutoSize = True
        Me.GDPRCheckBox.Location = New System.Drawing.Point(10, 33)
        Me.GDPRCheckBox.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.GDPRCheckBox.Name = "GDPRCheckBox"
        Me.GDPRCheckBox.Size = New System.Drawing.Size(288, 29)
        Me.GDPRCheckBox.TabIndex = 7
        Me.GDPRCheckBox.Text = Global.Outworldz.My.Resources.Resources.Publish_grid
        Me.GDPRCheckBox.UseVisualStyleBackColor = True
        '
        'PictureBox9
        '
        Me.PictureBox9.InitialImage = Global.Outworldz.My.Resources.Resources.ClicktoInsertPhoto
        Me.PictureBox9.Location = New System.Drawing.Point(10, 77)
        Me.PictureBox9.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.PictureBox9.Name = "PictureBox9"
        Me.PictureBox9.Size = New System.Drawing.Size(355, 215)
        Me.PictureBox9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox9.TabIndex = 1864
        Me.PictureBox9.TabStop = False
        '
        'MenuStrip2
        '
        Me.MenuStrip2.GripMargin = New System.Windows.Forms.Padding(2, 2, 0, 2)
        Me.MenuStrip2.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.MenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem30})
        Me.MenuStrip2.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip2.Name = "MenuStrip2"
        Me.MenuStrip2.Padding = New System.Windows.Forms.Padding(7, 2, 0, 2)
        Me.MenuStrip2.Size = New System.Drawing.Size(1227, 38)
        Me.MenuStrip2.TabIndex = 1889
        Me.MenuStrip2.Text = "0"
        '
        'ToolStripMenuItem30
        '
        Me.ToolStripMenuItem30.Image = Global.Outworldz.My.Resources.Resources.about
        Me.ToolStripMenuItem30.Name = "ToolStripMenuItem30"
        Me.ToolStripMenuItem30.Size = New System.Drawing.Size(98, 34)
        Me.ToolStripMenuItem30.Text = Global.Outworldz.My.Resources.Resources.Help_word
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.CategoryCheckbox)
        Me.GroupBox1.Location = New System.Drawing.Point(418, 52)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.GroupBox1.Size = New System.Drawing.Size(378, 329)
        Me.GroupBox1.TabIndex = 1892
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Category"
        '
        'CategoryCheckbox
        '
        Me.CategoryCheckbox.FormattingEnabled = True
        Me.CategoryCheckbox.Items.AddRange(New Object() {"Adult", "Art", "Charity", "Child Friendly", "Commercial", "Educational", "Education - School", "Education - College", "Experimental", "Fantasy", "Freebies", "Free Land", "Furry", "Hideout", "Hyperport", "Gaming", "LGBT", "Personal", "Newcomer Friendly", "Parks & Nature", "R-Rated", "Rental", "Residential", "Role play", "Romance", "Sandbox", "Sci-Fi", "Science", "Scripting", "Shopping", "Testing", "X-Rated"})
        Me.CategoryCheckbox.Location = New System.Drawing.Point(19, 33)
        Me.CategoryCheckbox.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.CategoryCheckbox.Name = "CategoryCheckbox"
        Me.CategoryCheckbox.Size = New System.Drawing.Size(354, 264)
        Me.CategoryCheckbox.TabIndex = 1897
        '
        'FileSystemWatcher1
        '
        Me.FileSystemWatcher1.EnableRaisingEvents = True
        Me.FileSystemWatcher1.SynchronizingObject = Me
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.DescriptionBox)
        Me.GroupBox2.Location = New System.Drawing.Point(805, 47)
        Me.GroupBox2.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Padding = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.GroupBox2.Size = New System.Drawing.Size(387, 335)
        Me.GroupBox2.TabIndex = 1896
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Description"
        '
        'DescriptionBox
        '
        Me.DescriptionBox.Location = New System.Drawing.Point(5, 37)
        Me.DescriptionBox.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.DescriptionBox.Multiline = True
        Me.DescriptionBox.Name = "DescriptionBox"
        Me.DescriptionBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.DescriptionBox.Size = New System.Drawing.Size(373, 268)
        Me.DescriptionBox.TabIndex = 1893
        '
        'FormPublicity
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(168.0!, 168.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1227, 402)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.MenuStrip2)
        Me.Controls.Add(Me.GroupBox11)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.MaximizeBox = False
        Me.Name = "FormPublicity"
        Me.Text = "Publicity"
        Me.GroupBox11.ResumeLayout(False)
        Me.GroupBox11.PerformLayout()
        CType(Me.PictureBox9, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip2.ResumeLayout(False)
        Me.MenuStrip2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.FileSystemWatcher1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents GroupBox11 As GroupBox
    Friend WithEvents GDPRCheckBox As CheckBox
    Friend WithEvents PictureBox9 As PictureBox
    Friend WithEvents MenuStrip2 As MenuStrip
    Friend WithEvents ToolStripMenuItem30 As ToolStripMenuItem
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents FileSystemWatcher1 As IO.FileSystemWatcher
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents DescriptionBox As TextBox
    Friend WithEvents CategoryCheckbox As CheckedListBox
End Class
