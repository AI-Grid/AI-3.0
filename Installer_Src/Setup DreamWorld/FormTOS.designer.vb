﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class TosForm
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
    <CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId:="System.Windows.Forms.ButtonBase.set_Text(System.String)")>
    <CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId:="System.Windows.Forms.Control.set_Text(System.String)")>
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TosForm))
        Me.PreviewButton = New System.Windows.Forms.Button()
        Me.Editor1 = New LiveSwitch.TextControl.Editor()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.ApplyButton = New System.Windows.Forms.Button()
        Me.MenuStrip2 = New System.Windows.Forms.MenuStrip()
        Me.ToolStripMenuItem30 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip2.SuspendLayout()
        Me.SuspendLayout()
        '
        'PreviewButton
        '
        Me.PreviewButton.Location = New System.Drawing.Point(376, 734)
        Me.PreviewButton.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.PreviewButton.Name = "PreviewButton"
        Me.PreviewButton.Size = New System.Drawing.Size(182, 34)
        Me.PreviewButton.TabIndex = 5
        Me.PreviewButton.Text = Global.Outworldz.My.Resources.Resources.Preview_in_Browser
        Me.PreviewButton.UseVisualStyleBackColor = True
        '
        'Editor1
        '
        Me.Editor1.BodyBackgroundColor = System.Drawing.Color.White
        Me.Editor1.BodyHtml = Nothing
        Me.Editor1.BodyText = Nothing
        Me.Editor1.DocumentText = resources.GetString("Editor1.DocumentText")
        Me.Editor1.EditorBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Editor1.EditorForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Editor1.FontSize = LiveSwitch.TextControl.FontSize.Three
        Me.Editor1.Html = Nothing
        Me.Editor1.Location = New System.Drawing.Point(3, 72)
        Me.Editor1.Margin = New System.Windows.Forms.Padding(9, 9, 9, 9)
        Me.Editor1.Name = "Editor1"
        Me.Editor1.Size = New System.Drawing.Size(954, 622)
        Me.Editor1.TabIndex = 7
        '
        'SaveButton
        '
        Me.SaveButton.Location = New System.Drawing.Point(15, 734)
        Me.SaveButton.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(112, 34)
        Me.SaveButton.TabIndex = 8
        Me.SaveButton.Text = Global.Outworldz.My.Resources.Resources.Ok
        Me.SaveButton.UseVisualStyleBackColor = True
        '
        'ApplyButton
        '
        Me.ApplyButton.Location = New System.Drawing.Point(164, 734)
        Me.ApplyButton.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.ApplyButton.Name = "ApplyButton"
        Me.ApplyButton.Size = New System.Drawing.Size(112, 34)
        Me.ApplyButton.TabIndex = 9
        Me.ApplyButton.Text = Global.Outworldz.My.Resources.Resources.Apply_word
        Me.ApplyButton.UseVisualStyleBackColor = True
        '
        'MenuStrip2
        '
        Me.MenuStrip2.GripMargin = New System.Windows.Forms.Padding(2, 2, 0, 2)
        Me.MenuStrip2.ImageScalingSize = New System.Drawing.Size(28, 28)
        Me.MenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem30})
        Me.MenuStrip2.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip2.Name = "MenuStrip2"
        Me.MenuStrip2.Padding = New System.Windows.Forms.Padding(5, 2, 0, 2)
        Me.MenuStrip2.Size = New System.Drawing.Size(990, 36)
        Me.MenuStrip2.TabIndex = 1887
        Me.MenuStrip2.Text = "0"
        '
        'ToolStripMenuItem30
        '
        Me.ToolStripMenuItem30.Image = Global.Outworldz.My.Resources.Resources.about
        Me.ToolStripMenuItem30.Name = "ToolStripMenuItem30"
        Me.ToolStripMenuItem30.Size = New System.Drawing.Size(93, 32)
        Me.ToolStripMenuItem30.Text = Global.Outworldz.My.Resources.Resources.Help_word
        '
        'TosForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(144.0!, 144.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(990, 812)
        Me.Controls.Add(Me.MenuStrip2)
        Me.Controls.Add(Me.ApplyButton)
        Me.Controls.Add(Me.SaveButton)
        Me.Controls.Add(Me.Editor1)
        Me.Controls.Add(Me.PreviewButton)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.MaximizeBox = False
        Me.Name = "TosForm"
        Me.Text = "Terms of Service"
        Me.MenuStrip2.ResumeLayout(False)
        Me.MenuStrip2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PreviewButton As Button
    Friend WithEvents Editor1 As LiveSwitch.TextControl.Editor
    Friend WithEvents SaveButton As Button
    Friend WithEvents ApplyButton As Button
    Friend WithEvents MenuStrip2 As MenuStrip
    Friend WithEvents ToolStripMenuItem30 As ToolStripMenuItem
End Class
