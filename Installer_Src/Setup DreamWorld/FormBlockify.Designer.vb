<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormBlockify
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
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SpacingTextBox = New System.Windows.Forms.TextBox()
        Me.GoButton = New System.Windows.Forms.Button()
        Me.RowSizeTextbox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(217, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.Image = Global.Outworldz.My.Resources.Resources.about
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(60, 20)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'SpacingTextBox
        '
        Me.SpacingTextBox.Location = New System.Drawing.Point(15, 40)
        Me.SpacingTextBox.Name = "SpacingTextBox"
        Me.SpacingTextBox.Size = New System.Drawing.Size(43, 20)
        Me.SpacingTextBox.TabIndex = 1
        Me.SpacingTextBox.Text = "4"
        '
        'GoButton
        '
        Me.GoButton.Location = New System.Drawing.Point(12, 106)
        Me.GoButton.Name = "GoButton"
        Me.GoButton.Size = New System.Drawing.Size(111, 23)
        Me.GoButton.TabIndex = 2
        Me.GoButton.Text = "Go"
        Me.GoButton.UseVisualStyleBackColor = True
        '
        'RowSizeTextbox
        '
        Me.RowSizeTextbox.Location = New System.Drawing.Point(15, 66)
        Me.RowSizeTextbox.Name = "RowSizeTextbox"
        Me.RowSizeTextbox.Size = New System.Drawing.Size(43, 20)
        Me.RowSizeTextbox.TabIndex = 3
        Me.RowSizeTextbox.Text = "25"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(65, 46)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(39, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Label1"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(68, 72)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(39, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Label2"
        '
        'FormBlockify
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(217, 156)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.RowSizeTextbox)
        Me.Controls.Add(Me.GoButton)
        Me.Controls.Add(Me.SpacingTextBox)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "FormBlockify"
        Me.Text = "FormBlockify"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents HelpToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SpacingTextBox As TextBox
    Friend WithEvents GoButton As Button
    Friend WithEvents RowSizeTextbox As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents ToolTip1 As ToolTip
End Class
