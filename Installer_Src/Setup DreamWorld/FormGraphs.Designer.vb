<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormGraphs
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
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.ChartWrapper1 = New MSChartWrapper.ChartWrapper()
        Me.ChartWrapper2 = New MSChartWrapper.ChartWrapper()
        Me.TimerGraph = New System.Windows.Forms.Timer(Me.components)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.ChartWrapper1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.ChartWrapper2)
        Me.SplitContainer1.Size = New System.Drawing.Size(800, 450)
        Me.SplitContainer1.SplitterDistance = 218
        Me.SplitContainer1.TabIndex = 18610
        '
        'ChartWrapper1
        '
        Me.ChartWrapper1.AddMarkers = True
        Me.ChartWrapper1.AutoSize = True
        Me.ChartWrapper1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ChartWrapper1.AxisXTitle = "1 minute"
        Me.ChartWrapper1.AxisYTitle = "% CPU"
        Me.ChartWrapper1.BackColor = System.Drawing.SystemColors.Control
        Me.ChartWrapper1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ChartWrapper1.LegendVisible = False
        Me.ChartWrapper1.Location = New System.Drawing.Point(0, 0)
        Me.ChartWrapper1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.ChartWrapper1.MarkerCount = 15
        Me.ChartWrapper1.MarkerFreq = 0
        Me.ChartWrapper1.MarkerSize = 2
        Me.ChartWrapper1.Name = "ChartWrapper1"
        Me.ChartWrapper1.SideLegendVisible = True
        Me.ChartWrapper1.Size = New System.Drawing.Size(800, 218)
        Me.ChartWrapper1.TabIndex = 0
        Me.ChartWrapper1.Title = ""
        '
        'ChartWrapper2
        '
        Me.ChartWrapper2.AddMarkers = True
        Me.ChartWrapper2.AutoSize = True
        Me.ChartWrapper2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ChartWrapper2.AxisXTitle = "10 Minutes"
        Me.ChartWrapper2.AxisYTitle = "% Memory"
        Me.ChartWrapper2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ChartWrapper2.LegendVisible = False
        Me.ChartWrapper2.Location = New System.Drawing.Point(0, 0)
        Me.ChartWrapper2.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.ChartWrapper2.MarkerCount = 15
        Me.ChartWrapper2.MarkerFreq = 0
        Me.ChartWrapper2.MarkerSize = 1
        Me.ChartWrapper2.Name = "ChartWrapper2"
        Me.ChartWrapper2.SideLegendVisible = True
        Me.ChartWrapper2.Size = New System.Drawing.Size(800, 228)
        Me.ChartWrapper2.TabIndex = 0
        Me.ChartWrapper2.Title = ""
        '
        'TimerGraph
        '
        '
        'FormGraphs
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "FormGraphs"
        Me.Text = "FormGraphs"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents ChartWrapper1 As MSChartWrapper.ChartWrapper
    Friend WithEvents ChartWrapper2 As MSChartWrapper.ChartWrapper
    Friend WithEvents TimerGraph As Timer
End Class
