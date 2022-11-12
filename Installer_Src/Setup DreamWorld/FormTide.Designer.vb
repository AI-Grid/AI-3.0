<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormTide
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
    <CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId:="System.Windows.Forms.Control.set_Text(System.String)")>
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormTide))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.TideInfoDebugCheckBox = New System.Windows.Forms.CheckBox()
        Me.BroadcastTideInfo = New System.Windows.Forms.CheckBox()
        Me.TideHiLoChannelLabel = New System.Windows.Forms.Label()
        Me.TideHiLoChannelTextBox = New System.Windows.Forms.TextBox()
        Me.TideInfoChannelLabel = New System.Windows.Forms.Label()
        Me.TideInfoChannelTextBox = New System.Windows.Forms.TextBox()
        Me.CycleTimeInSecondsLabel = New System.Windows.Forms.Label()
        Me.CycleTimeTextBox = New System.Windows.Forms.TextBox()
        Me.LowWaterLevelLabel = New System.Windows.Forms.Label()
        Me.TideLowLevelTextBox = New System.Windows.Forms.TextBox()
        Me.HighwaterLabel = New System.Windows.Forms.Label()
        Me.TideHighLevelTextBox = New System.Windows.Forms.TextBox()
        Me.TideEnabledCheckbox = New System.Windows.Forms.CheckBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MenuStrip2 = New System.Windows.Forms.MenuStrip()
        Me.ToolStripMenuItem30 = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupBox1.SuspendLayout()
        Me.MenuStrip2.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.TideInfoDebugCheckBox)
        Me.GroupBox1.Controls.Add(Me.BroadcastTideInfo)
        Me.GroupBox1.Controls.Add(Me.TideHiLoChannelLabel)
        Me.GroupBox1.Controls.Add(Me.TideHiLoChannelTextBox)
        Me.GroupBox1.Controls.Add(Me.TideInfoChannelLabel)
        Me.GroupBox1.Controls.Add(Me.TideInfoChannelTextBox)
        Me.GroupBox1.Controls.Add(Me.CycleTimeInSecondsLabel)
        Me.GroupBox1.Controls.Add(Me.CycleTimeTextBox)
        Me.GroupBox1.Controls.Add(Me.LowWaterLevelLabel)
        Me.GroupBox1.Controls.Add(Me.TideLowLevelTextBox)
        Me.GroupBox1.Controls.Add(Me.HighwaterLabel)
        Me.GroupBox1.Controls.Add(Me.TideHighLevelTextBox)
        Me.GroupBox1.Controls.Add(Me.TideEnabledCheckbox)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 37)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(294, 232)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Global Tide Settings"
        '
        'TideInfoDebugCheckBox
        '
        Me.TideInfoDebugCheckBox.AutoSize = True
        Me.TideInfoDebugCheckBox.Location = New System.Drawing.Point(27, 203)
        Me.TideInfoDebugCheckBox.Name = "TideInfoDebugCheckBox"
        Me.TideInfoDebugCheckBox.Size = New System.Drawing.Size(164, 17)
        Me.TideInfoDebugCheckBox.TabIndex = 6
        Me.TideInfoDebugCheckBox.Text = Global.Outworldz.My.Resources.Resources.Send_Debug_Info
        Me.ToolTip1.SetToolTip(Me.TideInfoDebugCheckBox, Global.Outworldz.My.Resources.Resources.Provide_Info)
        Me.TideInfoDebugCheckBox.UseVisualStyleBackColor = True
        '
        'BroadcastTideInfo
        '
        Me.BroadcastTideInfo.AutoSize = True
        Me.BroadcastTideInfo.Location = New System.Drawing.Point(125, 32)
        Me.BroadcastTideInfo.Name = "BroadcastTideInfo"
        Me.BroadcastTideInfo.Size = New System.Drawing.Size(119, 17)
        Me.BroadcastTideInfo.TabIndex = 7
        Me.BroadcastTideInfo.Text = Global.Outworldz.My.Resources.Resources.Broadcast_Tide_Info
        Me.ToolTip1.SetToolTip(Me.BroadcastTideInfo, Global.Outworldz.My.Resources.Resources.Broadcast_Tide_Chat)
        Me.BroadcastTideInfo.UseVisualStyleBackColor = True
        '
        'TideHiLoChannelLabel
        '
        Me.TideHiLoChannelLabel.AutoSize = True
        Me.TideHiLoChannelLabel.Location = New System.Drawing.Point(77, 170)
        Me.TideHiLoChannelLabel.Name = "TideHiLoChannelLabel"
        Me.TideHiLoChannelLabel.Size = New System.Drawing.Size(100, 13)
        Me.TideHiLoChannelLabel.TabIndex = 5
        Me.TideHiLoChannelLabel.Text = "Tide Hi/Lo Channel"
        '
        'TideHiLoChannelTextBox
        '
        Me.TideHiLoChannelTextBox.Location = New System.Drawing.Point(26, 166)
        Me.TideHiLoChannelTextBox.Name = "TideHiLoChannelTextBox"
        Me.TideHiLoChannelTextBox.Size = New System.Drawing.Size(48, 20)
        Me.TideHiLoChannelTextBox.TabIndex = 12
        '
        'TideInfoChannelLabel
        '
        Me.TideInfoChannelLabel.AutoSize = True
        Me.TideInfoChannelLabel.Location = New System.Drawing.Point(80, 143)
        Me.TideInfoChannelLabel.Name = "TideInfoChannelLabel"
        Me.TideInfoChannelLabel.Size = New System.Drawing.Size(91, 13)
        Me.TideInfoChannelLabel.TabIndex = 4
        Me.TideInfoChannelLabel.Text = "Tide Info Channel"
        '
        'TideInfoChannelTextBox
        '
        Me.TideInfoChannelTextBox.Location = New System.Drawing.Point(26, 140)
        Me.TideInfoChannelTextBox.Name = "TideInfoChannelTextBox"
        Me.TideInfoChannelTextBox.Size = New System.Drawing.Size(48, 20)
        Me.TideInfoChannelTextBox.TabIndex = 11
        '
        'CycleTimeInSecondsLabel
        '
        Me.CycleTimeInSecondsLabel.AutoSize = True
        Me.CycleTimeInSecondsLabel.Location = New System.Drawing.Point(80, 114)
        Me.CycleTimeInSecondsLabel.Name = "CycleTimeInSecondsLabel"
        Me.CycleTimeInSecondsLabel.Size = New System.Drawing.Size(113, 13)
        Me.CycleTimeInSecondsLabel.TabIndex = 3
        Me.CycleTimeInSecondsLabel.Text = "Cycle Time in seconds"
        '
        'CycleTimeTextBox
        '
        Me.CycleTimeTextBox.Location = New System.Drawing.Point(26, 111)
        Me.CycleTimeTextBox.Name = "CycleTimeTextBox"
        Me.CycleTimeTextBox.Size = New System.Drawing.Size(48, 20)
        Me.CycleTimeTextBox.TabIndex = 10
        Me.ToolTip1.SetToolTip(Me.CycleTimeTextBox, Global.Outworldz.My.Resources.Resources.Cycle_time_text)
        '
        'LowWaterLevelLabel
        '
        Me.LowWaterLevelLabel.AutoSize = True
        Me.LowWaterLevelLabel.Location = New System.Drawing.Point(80, 88)
        Me.LowWaterLevelLabel.Name = "LowWaterLevelLabel"
        Me.LowWaterLevelLabel.Size = New System.Drawing.Size(88, 13)
        Me.LowWaterLevelLabel.TabIndex = 2
        Me.LowWaterLevelLabel.Text = "Low Water Level"
        '
        'TideLowLevelTextBox
        '
        Me.TideLowLevelTextBox.Location = New System.Drawing.Point(26, 85)
        Me.TideLowLevelTextBox.Name = "TideLowLevelTextBox"
        Me.TideLowLevelTextBox.Size = New System.Drawing.Size(48, 20)
        Me.TideLowLevelTextBox.TabIndex = 9
        Me.ToolTip1.SetToolTip(Me.TideLowLevelTextBox, Global.Outworldz.My.Resources.Resources.Low_High)
        '
        'HighwaterLabel
        '
        Me.HighwaterLabel.AutoSize = True
        Me.HighwaterLabel.Location = New System.Drawing.Point(80, 61)
        Me.HighwaterLabel.Name = "HighwaterLabel"
        Me.HighwaterLabel.Size = New System.Drawing.Size(90, 13)
        Me.HighwaterLabel.TabIndex = 1
        Me.HighwaterLabel.Text = "High Water Level"
        '
        'TideHighLevelTextBox
        '
        Me.TideHighLevelTextBox.Location = New System.Drawing.Point(26, 58)
        Me.TideHighLevelTextBox.Name = "TideHighLevelTextBox"
        Me.TideHighLevelTextBox.Size = New System.Drawing.Size(48, 20)
        Me.TideHighLevelTextBox.TabIndex = 8
        Me.ToolTip1.SetToolTip(Me.TideHighLevelTextBox, Global.Outworldz.My.Resources.Resources.High_Water_Level_text)
        '
        'TideEnabledCheckbox
        '
        Me.TideEnabledCheckbox.AutoSize = True
        Me.TideEnabledCheckbox.Location = New System.Drawing.Point(27, 32)
        Me.TideEnabledCheckbox.Name = "TideEnabledCheckbox"
        Me.TideEnabledCheckbox.Size = New System.Drawing.Size(59, 17)
        Me.TideEnabledCheckbox.TabIndex = 0
        Me.TideEnabledCheckbox.Text = Global.Outworldz.My.Resources.Resources.Enable_word
        Me.TideEnabledCheckbox.UseVisualStyleBackColor = True
        '
        'ToolTip1
        '
        Me.ToolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info
        Me.ToolTip1.ToolTipTitle = Global.Outworldz.My.Resources.Resources.Tide_Enable
        '
        'MenuStrip2
        '
        Me.MenuStrip2.ImageScalingSize = New System.Drawing.Size(28, 28)
        Me.MenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem30})
        Me.MenuStrip2.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip2.Name = "MenuStrip2"
        Me.MenuStrip2.Padding = New System.Windows.Forms.Padding(4, 1, 0, 1)
        Me.MenuStrip2.Size = New System.Drawing.Size(318, 30)
        Me.MenuStrip2.TabIndex = 0
        Me.MenuStrip2.Text = "0"
        '
        'ToolStripMenuItem30
        '
        Me.ToolStripMenuItem30.Image = Global.Outworldz.My.Resources.Resources.about
        Me.ToolStripMenuItem30.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ToolStripMenuItem30.Name = "ToolStripMenuItem30"
        Me.ToolStripMenuItem30.Size = New System.Drawing.Size(68, 28)
        Me.ToolStripMenuItem30.Text = Global.Outworldz.My.Resources.Resources.Help_word
        '
        'FormTide
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoSize = True
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ClientSize = New System.Drawing.Size(318, 291)
        Me.Controls.Add(Me.MenuStrip2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "FormTide"
        Me.Text = "Tides"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.MenuStrip2.ResumeLayout(False)
        Me.MenuStrip2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents TideEnabledCheckbox As CheckBox
    Friend WithEvents LowWaterLevelLabel As Label
    Friend WithEvents TideLowLevelTextBox As TextBox
    Friend WithEvents HighwaterLabel As Label
    Friend WithEvents TideHighLevelTextBox As TextBox
    Friend WithEvents CycleTimeInSecondsLabel As Label
    Friend WithEvents CycleTimeTextBox As TextBox
    Friend WithEvents TideInfoChannelLabel As Label
    Friend WithEvents TideInfoChannelTextBox As TextBox
    Friend WithEvents TideHiLoChannelLabel As Label
    Friend WithEvents TideHiLoChannelTextBox As TextBox
    Friend WithEvents BroadcastTideInfo As CheckBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents TideInfoDebugCheckBox As CheckBox
    Friend WithEvents MenuStrip2 As MenuStrip
    Friend WithEvents ToolStripMenuItem30 As ToolStripMenuItem
End Class
