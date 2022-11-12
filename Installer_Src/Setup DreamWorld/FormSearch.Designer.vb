<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormSearch
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormSearch))
        Me.SearchBox = New System.Windows.Forms.GroupBox()
        Me.LocalButton = New System.Windows.Forms.RadioButton()
        Me.NoneButton = New System.Windows.Forms.RadioButton()
        Me.JOpensimRadioButton = New System.Windows.Forms.RadioButton()
        Me.OutworldzRadioButton = New System.Windows.Forms.RadioButton()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.SearchBox.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SearchBox
        '
        Me.SearchBox.Controls.Add(Me.LocalButton)
        Me.SearchBox.Controls.Add(Me.NoneButton)
        Me.SearchBox.Controls.Add(Me.JOpensimRadioButton)
        Me.SearchBox.Controls.Add(Me.OutworldzRadioButton)
        Me.SearchBox.Location = New System.Drawing.Point(16, 39)
        Me.SearchBox.Margin = New System.Windows.Forms.Padding(2)
        Me.SearchBox.Name = "SearchBox"
        Me.SearchBox.Padding = New System.Windows.Forms.Padding(2)
        Me.SearchBox.Size = New System.Drawing.Size(170, 150)
        Me.SearchBox.TabIndex = 1
        Me.SearchBox.TabStop = False
        Me.SearchBox.Text = "Search Options"
        '
        'LocalButton
        '
        Me.LocalButton.AutoSize = True
        Me.LocalButton.Location = New System.Drawing.Point(22, 54)
        Me.LocalButton.Margin = New System.Windows.Forms.Padding(2)
        Me.LocalButton.Name = "LocalButton"
        Me.LocalButton.Size = New System.Drawing.Size(88, 17)
        Me.LocalButton.TabIndex = 3
        Me.LocalButton.TabStop = True
        Me.LocalButton.Text = "Local Search"
        Me.LocalButton.UseVisualStyleBackColor = True
        '
        'NoneButton
        '
        Me.NoneButton.AutoSize = True
        Me.NoneButton.Location = New System.Drawing.Point(22, 33)
        Me.NoneButton.Margin = New System.Windows.Forms.Padding(2)
        Me.NoneButton.Name = "NoneButton"
        Me.NoneButton.Size = New System.Drawing.Size(76, 17)
        Me.NoneButton.TabIndex = 0
        Me.NoneButton.TabStop = True
        Me.NoneButton.Text = "No Search"
        Me.NoneButton.UseVisualStyleBackColor = True
        '
        'JOpensimRadioButton
        '
        Me.JOpensimRadioButton.AutoSize = True
        Me.JOpensimRadioButton.Location = New System.Drawing.Point(22, 97)
        Me.JOpensimRadioButton.Margin = New System.Windows.Forms.Padding(2)
        Me.JOpensimRadioButton.Name = "JOpensimRadioButton"
        Me.JOpensimRadioButton.Size = New System.Drawing.Size(108, 17)
        Me.JOpensimRadioButton.TabIndex = 2
        Me.JOpensimRadioButton.TabStop = True
        Me.JOpensimRadioButton.Text = Global.Outworldz.My.Resources.Resources.JOpensimSearch_word
        Me.JOpensimRadioButton.UseVisualStyleBackColor = True
        Me.JOpensimRadioButton.Visible = False
        '
        'OutworldzRadioButton
        '
        Me.OutworldzRadioButton.AutoSize = True
        Me.OutworldzRadioButton.Location = New System.Drawing.Point(22, 75)
        Me.OutworldzRadioButton.Margin = New System.Windows.Forms.Padding(2)
        Me.OutworldzRadioButton.Name = "OutworldzRadioButton"
        Me.OutworldzRadioButton.Size = New System.Drawing.Size(92, 17)
        Me.OutworldzRadioButton.TabIndex = 1
        Me.OutworldzRadioButton.TabStop = True
        Me.OutworldzRadioButton.Text = Global.Outworldz.My.Resources.Resources.HypericaSearch_word
        Me.OutworldzRadioButton.UseVisualStyleBackColor = True
        '
        'ToolStrip1
        '
        Me.ToolStrip1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton1, Me.ToolStripLabel1})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(194, 31)
        Me.ToolStrip1.TabIndex = 0
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton1.Image = Global.Outworldz.My.Resources.Resources.about
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Size = New System.Drawing.Size(28, 28)
        Me.ToolStripButton1.Text = "ToolStripButton1"
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(32, 28)
        Me.ToolStripLabel1.Text = "Help"
        '
        'FormSearch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(194, 200)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.SearchBox)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "FormSearch"
        Me.SearchBox.ResumeLayout(False)
        Me.SearchBox.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents SearchBox As GroupBox
    Friend WithEvents NoneButton As RadioButton
    Friend WithEvents JOpensimRadioButton As RadioButton
    Friend WithEvents OutworldzRadioButton As RadioButton
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents ToolStripButton1 As ToolStripButton
    Friend WithEvents ToolStripLabel1 As ToolStripLabel
    Friend WithEvents LocalButton As RadioButton
End Class
