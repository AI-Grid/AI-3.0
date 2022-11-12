<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormDnsName
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormDnsName))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.SuitcaseCheckbox = New System.Windows.Forms.CheckBox()
        Me.EnableHypergrid = New System.Windows.Forms.CheckBox()
        Me.UniqueId = New System.Windows.Forms.TextBox()
        Me.TestButton1 = New System.Windows.Forms.Button()
        Me.NextNameButton = New System.Windows.Forms.Button()
        Me.DNSNameBox = New System.Windows.Forms.TextBox()
        Me.SaveButton1 = New System.Windows.Forms.Button()
        Me.DNSAliasTextBox = New System.Windows.Forms.TextBox()
        Me.OsGridButton = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PictureBox = New System.Windows.Forms.PictureBox()
        Me.OsGridButton.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        CType(Me.PictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SuitcaseCheckbox
        '
        Me.SuitcaseCheckbox.AutoSize = True
        Me.SuitcaseCheckbox.Location = New System.Drawing.Point(16, 42)
        Me.SuitcaseCheckbox.Name = "SuitcaseCheckbox"
        Me.SuitcaseCheckbox.Size = New System.Drawing.Size(150, 17)
        Me.SuitcaseCheckbox.TabIndex = 1
        Me.SuitcaseCheckbox.Text = Global.Outworldz.My.Resources.Resources.Suitcase_enable
        Me.ToolTip1.SetToolTip(Me.SuitcaseCheckbox, Global.Outworldz.My.Resources.Resources.Disable_Suitcase_txt)
        Me.SuitcaseCheckbox.UseVisualStyleBackColor = True
        '
        'EnableHypergrid
        '
        Me.EnableHypergrid.AutoSize = True
        Me.EnableHypergrid.Location = New System.Drawing.Point(16, 19)
        Me.EnableHypergrid.Name = "EnableHypergrid"
        Me.EnableHypergrid.Size = New System.Drawing.Size(107, 17)
        Me.EnableHypergrid.TabIndex = 0
        Me.EnableHypergrid.Text = Global.Outworldz.My.Resources.Resources.Enable_Hypergrid_word
        Me.ToolTip1.SetToolTip(Me.EnableHypergrid, Global.Outworldz.My.Resources.Resources.Enable_Hypergrid_word)
        Me.EnableHypergrid.UseVisualStyleBackColor = True
        '
        'UniqueId
        '
        Me.UniqueId.Location = New System.Drawing.Point(17, 185)
        Me.UniqueId.Name = "UniqueId"
        Me.UniqueId.Size = New System.Drawing.Size(132, 20)
        Me.UniqueId.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.UniqueId, Global.Outworldz.My.Resources.Resources.Reserve_Password)
        '
        'TestButton1
        '
        Me.TestButton1.Location = New System.Drawing.Point(143, 232)
        Me.TestButton1.Name = "TestButton1"
        Me.TestButton1.Size = New System.Drawing.Size(97, 23)
        Me.TestButton1.TabIndex = 6
        Me.TestButton1.Text = Global.Outworldz.My.Resources.Resources.Test_DNS_word
        Me.ToolTip1.SetToolTip(Me.TestButton1, Global.Outworldz.My.Resources.Resources.Test_DNS_word)
        Me.TestButton1.UseVisualStyleBackColor = True
        '
        'NextNameButton
        '
        Me.NextNameButton.Location = New System.Drawing.Point(25, 232)
        Me.NextNameButton.Name = "NextNameButton"
        Me.NextNameButton.Size = New System.Drawing.Size(102, 23)
        Me.NextNameButton.TabIndex = 7
        Me.NextNameButton.Text = Global.Outworldz.My.Resources.Resources.Next_Name
        Me.ToolTip1.SetToolTip(Me.NextNameButton, Global.Outworldz.My.Resources.Resources.FreeName)
        Me.NextNameButton.UseVisualStyleBackColor = True
        '
        'DNSNameBox
        '
        Me.DNSNameBox.Location = New System.Drawing.Point(16, 87)
        Me.DNSNameBox.Name = "DNSNameBox"
        Me.DNSNameBox.Size = New System.Drawing.Size(296, 20)
        Me.DNSNameBox.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.DNSNameBox, Global.Outworldz.My.Resources.Resources.AlphaNum)
        '
        'SaveButton1
        '
        Me.SaveButton1.Location = New System.Drawing.Point(255, 232)
        Me.SaveButton1.Name = "SaveButton1"
        Me.SaveButton1.Size = New System.Drawing.Size(91, 23)
        Me.SaveButton1.TabIndex = 5
        Me.SaveButton1.Text = Global.Outworldz.My.Resources.Resources.Save_word
        Me.ToolTip1.SetToolTip(Me.SaveButton1, Global.Outworldz.My.Resources.Resources.Save_word)
        Me.SaveButton1.UseVisualStyleBackColor = True
        '
        'DNSAliasTextBox
        '
        Me.DNSAliasTextBox.Location = New System.Drawing.Point(17, 134)
        Me.DNSAliasTextBox.Name = "DNSAliasTextBox"
        Me.DNSAliasTextBox.Size = New System.Drawing.Size(296, 20)
        Me.DNSAliasTextBox.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.DNSAliasTextBox, Global.Outworldz.My.Resources.Resources.DNSAlt)
        '
        'OsGridButton
        '
        Me.OsGridButton.Controls.Add(Me.PictureBox)
        Me.OsGridButton.Controls.Add(Me.Label3)
        Me.OsGridButton.Controls.Add(Me.DNSAliasTextBox)
        Me.OsGridButton.Controls.Add(Me.SaveButton1)
        Me.OsGridButton.Controls.Add(Me.Label2)
        Me.OsGridButton.Controls.Add(Me.SuitcaseCheckbox)
        Me.OsGridButton.Controls.Add(Me.Label1)
        Me.OsGridButton.Controls.Add(Me.EnableHypergrid)
        Me.OsGridButton.Controls.Add(Me.UniqueId)
        Me.OsGridButton.Controls.Add(Me.TestButton1)
        Me.OsGridButton.Controls.Add(Me.NextNameButton)
        Me.OsGridButton.Controls.Add(Me.DNSNameBox)
        Me.OsGridButton.Location = New System.Drawing.Point(21, 34)
        Me.OsGridButton.Name = "OsGridButton"
        Me.OsGridButton.Size = New System.Drawing.Size(357, 261)
        Me.OsGridButton.TabIndex = 1
        Me.OsGridButton.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(17, 118)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(224, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "DNS Name or SomeName.Outworldz.net or IP"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(16, 71)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(224, 13)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "DNS Name or SomeName.Outworldz.net or IP"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(22, 169)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(122, 13)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Dynamic DNS password"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(28, 28)
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Padding = New System.Windows.Forms.Padding(4, 1, 0, 1)
        Me.MenuStrip1.Size = New System.Drawing.Size(387, 34)
        Me.MenuStrip1.TabIndex = 0
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.Image = Global.Outworldz.My.Resources.Resources.about
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(72, 32)
        Me.HelpToolStripMenuItem.Text = Global.Outworldz.My.Resources.Resources.Help_word
        '
        'PictureBox
        '
        Me.PictureBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox.Image = Global.Outworldz.My.Resources.Resources.loader
        Me.PictureBox.Location = New System.Drawing.Point(174, 192)
        Me.PictureBox.Name = "PictureBox"
        Me.PictureBox.Size = New System.Drawing.Size(38, 34)
        Me.PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox.TabIndex = 18621
        Me.PictureBox.TabStop = False
        '
        'FormDnsName
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(387, 307)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.OsGridButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FormDnsName"
        Me.Text = "DNS Name & Hypergrid"
        Me.ToolTip1.SetToolTip(Me, Global.Outworldz.My.Resources.Resources.Help_word)
        Me.OsGridButton.ResumeLayout(False)
        Me.OsGridButton.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        CType(Me.PictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents OsGridButton As GroupBox
    Friend WithEvents Label2 As Label
    Friend WithEvents SuitcaseCheckbox As CheckBox
    Friend WithEvents Label1 As Label
    Friend WithEvents EnableHypergrid As CheckBox
    Friend WithEvents UniqueId As TextBox
    Friend WithEvents TestButton1 As Button
    Friend WithEvents NextNameButton As Button
    Friend WithEvents DNSNameBox As TextBox
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents SaveButton1 As Button
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents HelpToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Label3 As Label
    Friend WithEvents DNSAliasTextBox As TextBox
    Friend WithEvents PictureBox As PictureBox
End Class
