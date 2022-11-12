<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormServerType
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.MetroRadioButton2 = New System.Windows.Forms.RadioButton()
        Me.GridRegionButton = New System.Windows.Forms.RadioButton()
        Me.osGridRadioButton1 = New System.Windows.Forms.RadioButton()
        Me.GridServerButton = New System.Windows.Forms.RadioButton()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupBox1.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.MetroRadioButton2)
        Me.GroupBox1.Controls.Add(Me.GridRegionButton)
        Me.GroupBox1.Controls.Add(Me.osGridRadioButton1)
        Me.GroupBox1.Controls.Add(Me.GridServerButton)
        Me.GroupBox1.Location = New System.Drawing.Point(16, 38)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(214, 145)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Server Type"
        '
        'MetroRadioButton2
        '
        Me.MetroRadioButton2.AutoSize = True
        Me.MetroRadioButton2.Location = New System.Drawing.Point(17, 99)
        Me.MetroRadioButton2.Name = "MetroRadioButton2"
        Me.MetroRadioButton2.Size = New System.Drawing.Size(146, 17)
        Me.MetroRadioButton2.TabIndex = 3
        Me.MetroRadioButton2.Text = "Metro.land Region Server"
        Me.MetroRadioButton2.UseVisualStyleBackColor = True
        Me.MetroRadioButton2.Visible = False
        '
        'GridRegionButton
        '
        Me.GridRegionButton.AutoSize = True
        Me.GridRegionButton.Location = New System.Drawing.Point(17, 55)
        Me.GridRegionButton.Name = "GridRegionButton"
        Me.GridRegionButton.Size = New System.Drawing.Size(93, 17)
        Me.GridRegionButton.TabIndex = 1
        Me.GridRegionButton.Text = Global.Outworldz.My.Resources.Resources.Region_Server_word
        Me.GridRegionButton.UseVisualStyleBackColor = True
        '
        'osGridRadioButton1
        '
        Me.osGridRadioButton1.AutoSize = True
        Me.osGridRadioButton1.Location = New System.Drawing.Point(17, 76)
        Me.osGridRadioButton1.Name = "osGridRadioButton1"
        Me.osGridRadioButton1.Size = New System.Drawing.Size(130, 17)
        Me.osGridRadioButton1.TabIndex = 2
        Me.osGridRadioButton1.Text = Global.Outworldz.My.Resources.Resources.OSGrid_Region_Server
        Me.osGridRadioButton1.UseVisualStyleBackColor = True
        '
        'GridServerButton
        '
        Me.GridServerButton.AutoSize = True
        Me.GridServerButton.Checked = True
        Me.GridServerButton.Location = New System.Drawing.Point(17, 31)
        Me.GridServerButton.Name = "GridServerButton"
        Me.GridServerButton.Size = New System.Drawing.Size(140, 17)
        Me.GridServerButton.TabIndex = 0
        Me.GridServerButton.TabStop = True
        Me.GridServerButton.Text = Global.Outworldz.My.Resources.Resources.Grid_Server_With_Robust_word
        Me.GridServerButton.UseVisualStyleBackColor = True
        '
        'MenuStrip1
        '
        Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(28, 28)
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Padding = New System.Windows.Forms.Padding(4, 2, 0, 2)
        Me.MenuStrip1.Size = New System.Drawing.Size(248, 32)
        Me.MenuStrip1.TabIndex = 0
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.Image = Global.Outworldz.My.Resources.Resources.about
        Me.HelpToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(68, 28)
        Me.HelpToolStripMenuItem.Text = Global.Outworldz.My.Resources.Resources.Help_word
        '
        'FormServerType
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(248, 194)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "FormServerType"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents MetroRadioButton2 As RadioButton
    Friend WithEvents GridRegionButton As RadioButton
    Friend WithEvents osGridRadioButton1 As RadioButton
    Friend WithEvents GridServerButton As RadioButton
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents HelpToolStripMenuItem As ToolStripMenuItem
End Class
