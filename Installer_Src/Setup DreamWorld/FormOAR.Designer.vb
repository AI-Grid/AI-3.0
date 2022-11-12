<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormOAR
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormOAR))
        Me.DataGridView = New System.Windows.Forms.DataGridView()
        Me.MenuStrip2 = New System.Windows.Forms.MenuStrip()
        Me.ToolStripMenuItem30 = New System.Windows.Forms.ToolStripMenuItem()
        Me.RefreshToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.NameRadioButton = New System.Windows.Forms.RadioButton()
        Me.DateRadioButton = New System.Windows.Forms.RadioButton()
        Me.AscendRadioButton = New System.Windows.Forms.RadioButton()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.DescendRadioButton = New System.Windows.Forms.RadioButton()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.ExclusiveCheckbox = New System.Windows.Forms.CheckBox()
        CType(Me.DataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip2.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'DataGridView
        '
        Me.DataGridView.AllowUserToDeleteRows = False
        Me.DataGridView.AllowUserToResizeColumns = False
        Me.DataGridView.AllowUserToResizeRows = False
        Me.DataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridView.BackgroundColor = System.Drawing.SystemColors.Control
        Me.DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView.ColumnHeadersVisible = False
        Me.DataGridView.Cursor = System.Windows.Forms.Cursors.Hand
        Me.DataGridView.Location = New System.Drawing.Point(11, 68)
        Me.DataGridView.Margin = New System.Windows.Forms.Padding(1)
        Me.DataGridView.MultiSelect = False
        Me.DataGridView.Name = "DataGridView"
        Me.DataGridView.RowHeadersWidth = 62
        Me.DataGridView.RowTemplate.Height = 3
        Me.DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.DataGridView.ShowCellErrors = False
        Me.DataGridView.Size = New System.Drawing.Size(665, 355)
        Me.DataGridView.TabIndex = 0
        '
        'MenuStrip2
        '
        Me.MenuStrip2.ImageScalingSize = New System.Drawing.Size(28, 28)
        Me.MenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem30, Me.RefreshToolStripMenuItem})
        Me.MenuStrip2.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip2.Name = "MenuStrip2"
        Me.MenuStrip2.Padding = New System.Windows.Forms.Padding(7, 1, 0, 1)
        Me.MenuStrip2.Size = New System.Drawing.Size(686, 34)
        Me.MenuStrip2.TabIndex = 0
        Me.MenuStrip2.Text = "0"
        '
        'ToolStripMenuItem30
        '
        Me.ToolStripMenuItem30.Image = Global.Outworldz.My.Resources.Resources.about
        Me.ToolStripMenuItem30.Name = "ToolStripMenuItem30"
        Me.ToolStripMenuItem30.Size = New System.Drawing.Size(72, 32)
        Me.ToolStripMenuItem30.Text = Global.Outworldz.My.Resources.Resources.Help_word
        '
        'RefreshToolStripMenuItem
        '
        Me.RefreshToolStripMenuItem.Image = Global.Outworldz.My.Resources.Resources.refresh
        Me.RefreshToolStripMenuItem.Name = "RefreshToolStripMenuItem"
        Me.RefreshToolStripMenuItem.Size = New System.Drawing.Size(86, 32)
        Me.RefreshToolStripMenuItem.Text = Global.Outworldz.My.Resources.Resources.Refresh_word
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(201, 9)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(207, 20)
        Me.TextBox1.TabIndex = 1
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.Outworldz.My.Resources.Resources.view
        Me.PictureBox1.Location = New System.Drawing.Point(166, 4)
        Me.PictureBox1.MinimumSize = New System.Drawing.Size(16, 16)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(29, 30)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 1892
        Me.PictureBox1.TabStop = False
        '
        'Timer1
        '
        '
        'NameRadioButton
        '
        Me.NameRadioButton.AutoSize = True
        Me.NameRadioButton.Location = New System.Drawing.Point(3, 3)
        Me.NameRadioButton.Name = "NameRadioButton"
        Me.NameRadioButton.Size = New System.Drawing.Size(89, 17)
        Me.NameRadioButton.TabIndex = 1893
        Me.NameRadioButton.TabStop = True
        Me.NameRadioButton.Text = "Sort by Name"
        Me.NameRadioButton.UseVisualStyleBackColor = True
        '
        'DateRadioButton
        '
        Me.DateRadioButton.AutoSize = True
        Me.DateRadioButton.Location = New System.Drawing.Point(124, 3)
        Me.DateRadioButton.Name = "DateRadioButton"
        Me.DateRadioButton.Size = New System.Drawing.Size(48, 17)
        Me.DateRadioButton.TabIndex = 1894
        Me.DateRadioButton.TabStop = True
        Me.DateRadioButton.Text = "Date"
        Me.DateRadioButton.UseVisualStyleBackColor = True
        '
        'AscendRadioButton
        '
        Me.AscendRadioButton.AutoSize = True
        Me.AscendRadioButton.Location = New System.Drawing.Point(3, 3)
        Me.AscendRadioButton.Name = "AscendRadioButton"
        Me.AscendRadioButton.Size = New System.Drawing.Size(75, 17)
        Me.AscendRadioButton.TabIndex = 1895
        Me.AscendRadioButton.TabStop = True
        Me.AscendRadioButton.Text = "Ascending"
        Me.AscendRadioButton.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.AutoSize = True
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.NameRadioButton, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.DateRadioButton, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(425, 7)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(243, 29)
        Me.TableLayoutPanel1.TabIndex = 1896
        '
        'DescendRadioButton
        '
        Me.DescendRadioButton.AutoSize = True
        Me.DescendRadioButton.Location = New System.Drawing.Point(124, 3)
        Me.DescendRadioButton.Name = "DescendRadioButton"
        Me.DescendRadioButton.Size = New System.Drawing.Size(82, 17)
        Me.DescendRadioButton.TabIndex = 1897
        Me.DescendRadioButton.TabStop = True
        Me.DescendRadioButton.Text = "Descending"
        Me.DescendRadioButton.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.AutoSize = True
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.DescendRadioButton, 1, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.AscendRadioButton, 0, 0)
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(425, 37)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(243, 28)
        Me.TableLayoutPanel2.TabIndex = 1897
        '
        'ExclusiveCheckbox
        '
        Me.ExclusiveCheckbox.AutoSize = True
        Me.ExclusiveCheckbox.Location = New System.Drawing.Point(198, 40)
        Me.ExclusiveCheckbox.Name = "ExclusiveCheckbox"
        Me.ExclusiveCheckbox.Size = New System.Drawing.Size(102, 17)
        Me.ExclusiveCheckbox.TabIndex = 1898
        Me.ExclusiveCheckbox.Text = "Exclusive OARs"
        Me.ExclusiveCheckbox.UseVisualStyleBackColor = True
        '
        'FormOAR
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(686, 433)
        Me.Controls.Add(Me.ExclusiveCheckbox)
        Me.Controls.Add(Me.TableLayoutPanel2)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.MenuStrip2)
        Me.Controls.Add(Me.DataGridView)
        Me.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(1)
        Me.Name = "FormOAR"
        CType(Me.DataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip2.ResumeLayout(False)
        Me.MenuStrip2.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents DataGridView As DataGridView
    Friend WithEvents MenuStrip2 As MenuStrip
    Friend WithEvents ToolStripMenuItem30 As ToolStripMenuItem
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents RefreshToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Timer1 As Timer
    Friend WithEvents NameRadioButton As RadioButton
    Friend WithEvents DateRadioButton As RadioButton
    Friend WithEvents AscendRadioButton As RadioButton
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents DescendRadioButton As RadioButton
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents ExclusiveCheckbox As CheckBox
End Class
