<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormPhysics
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormPhysics))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Physics4_UbODE = New System.Windows.Forms.RadioButton()
        Me.Physics2_Bullet = New System.Windows.Forms.RadioButton()
        Me.Physics3_Separate = New System.Windows.Forms.RadioButton()
        Me.Physics0_None = New System.Windows.Forms.RadioButton()
        Me.MenuStrip2 = New System.Windows.Forms.MenuStrip()
        Me.ToolStripMenuItem30 = New System.Windows.Forms.ToolStripMenuItem()
        Me.Physics5_Hybrid = New System.Windows.Forms.RadioButton()
        Me.GroupBox1.SuspendLayout()
        Me.MenuStrip2.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Physics5_Hybrid)
        Me.GroupBox1.Controls.Add(Me.Physics4_UbODE)
        Me.GroupBox1.Controls.Add(Me.Physics2_Bullet)
        Me.GroupBox1.Controls.Add(Me.Physics3_Separate)
        Me.GroupBox1.Controls.Add(Me.Physics0_None)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 33)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(215, 138)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Physics Engine"
        '
        'Physics4_UbODE
        '
        Me.Physics4_UbODE.AutoSize = True
        Me.Physics4_UbODE.Location = New System.Drawing.Point(6, 43)
        Me.Physics4_UbODE.Name = "Physics4_UbODE"
        Me.Physics4_UbODE.Size = New System.Drawing.Size(153, 17)
        Me.Physics4_UbODE.TabIndex = 2
        Me.Physics4_UbODE.TabStop = True
        Me.Physics4_UbODE.Text = Global.Outworldz.My.Resources.Resources.UBODE_words
        Me.Physics4_UbODE.UseVisualStyleBackColor = True
        '
        'Physics2_Bullet
        '
        Me.Physics2_Bullet.AutoSize = True
        Me.Physics2_Bullet.Location = New System.Drawing.Point(6, 88)
        Me.Physics2_Bullet.Name = "Physics2_Bullet"
        Me.Physics2_Bullet.Size = New System.Drawing.Size(92, 17)
        Me.Physics2_Bullet.TabIndex = 3
        Me.Physics2_Bullet.TabStop = True
        Me.Physics2_Bullet.Text = "Bullet physics "
        Me.Physics2_Bullet.UseVisualStyleBackColor = True
        '
        'Physics3_Separate
        '
        Me.Physics3_Separate.AutoSize = True
        Me.Physics3_Separate.Location = New System.Drawing.Point(6, 109)
        Me.Physics3_Separate.Name = "Physics3_Separate"
        Me.Physics3_Separate.Size = New System.Drawing.Size(177, 17)
        Me.Physics3_Separate.TabIndex = 4
        Me.Physics3_Separate.TabStop = True
        Me.Physics3_Separate.Text = Global.Outworldz.My.Resources.Resources.BP
        Me.Physics3_Separate.UseVisualStyleBackColor = True
        '
        'Physics0_None
        '
        Me.Physics0_None.AutoSize = True
        Me.Physics0_None.Location = New System.Drawing.Point(6, 20)
        Me.Physics0_None.Name = "Physics0_None"
        Me.Physics0_None.Size = New System.Drawing.Size(51, 17)
        Me.Physics0_None.TabIndex = 0
        Me.Physics0_None.TabStop = True
        Me.Physics0_None.Text = Global.Outworldz.My.Resources.Resources.None
        Me.Physics0_None.UseVisualStyleBackColor = True
        '
        'MenuStrip2
        '
        Me.MenuStrip2.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.MenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem30})
        Me.MenuStrip2.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip2.Name = "MenuStrip2"
        Me.MenuStrip2.Padding = New System.Windows.Forms.Padding(4, 1, 0, 1)
        Me.MenuStrip2.Size = New System.Drawing.Size(236, 30)
        Me.MenuStrip2.TabIndex = 0
        Me.MenuStrip2.Text = "0"
        '
        'ToolStripMenuItem30
        '
        Me.ToolStripMenuItem30.Image = Global.Outworldz.My.Resources.Resources.about
        Me.ToolStripMenuItem30.Name = "ToolStripMenuItem30"
        Me.ToolStripMenuItem30.Size = New System.Drawing.Size(68, 28)
        Me.ToolStripMenuItem30.Text = Global.Outworldz.My.Resources.Resources.Help_word
        '
        'Physics5_Hybrid
        '
        Me.Physics5_Hybrid.AutoSize = True
        Me.Physics5_Hybrid.Location = New System.Drawing.Point(6, 66)
        Me.Physics5_Hybrid.Name = "Physics5_Hybrid"
        Me.Physics5_Hybrid.Size = New System.Drawing.Size(55, 17)
        Me.Physics5_Hybrid.TabIndex = 5
        Me.Physics5_Hybrid.TabStop = True
        Me.Physics5_Hybrid.Text = "Hybrid"
        Me.Physics5_Hybrid.UseVisualStyleBackColor = True
        '
        'FormPhysics
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(236, 189)
        Me.Controls.Add(Me.MenuStrip2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "FormPhysics"
        Me.Text = "Physics"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.MenuStrip2.ResumeLayout(False)
        Me.MenuStrip2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Physics3_Separate As RadioButton
    Friend WithEvents Physics0_None As RadioButton
    Friend WithEvents MenuStrip2 As MenuStrip
    Friend WithEvents ToolStripMenuItem30 As ToolStripMenuItem
    Friend WithEvents Physics2_Bullet As RadioButton
    Friend WithEvents Physics4_UbODE As RadioButton
    Friend WithEvents Physics5_Hybrid As RadioButton
End Class
