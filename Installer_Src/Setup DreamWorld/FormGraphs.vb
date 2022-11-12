Public Class FormGraphs

#Region "ScreenSize"

    Private ReadOnly Handler As New EventHandler(AddressOf Resize_page)

    Private _screenPosition As ClassScreenpos

    Public Property ScreenPosition As ClassScreenpos
        Get
            Return _screenPosition
        End Get
        Set(value As ClassScreenpos)
            _screenPosition = value
        End Set
    End Property

    'The following detects  the location of the form in screen coordinates
    Private Sub Resize_page(ByVal sender As Object, ByVal e As System.EventArgs)

        ScreenPosition.SaveXY(Me.Left, Me.Top)
        ScreenPosition.SaveHW(Me.Height, Me.Width)

    End Sub

    Private Sub SetScreen()

        ScreenPosition = New ClassScreenpos(MyBase.Name)
        AddHandler ResizeEnd, Handler
        Dim xy As List(Of Integer) = ScreenPosition.GetXY()
        Me.Left = xy.Item(0)
        Me.Top = xy.Item(1)
        Dim hw As List(Of Integer) = ScreenPosition.GetHW()
        '1106, 460
        If hw.Item(0) = 0 Then
            Me.Height = 480
        Else
            Me.Height = hw.Item(0)
        End If
        If hw.Item(1) = 0 Then
            Me.Width = 640
        Else
            Me.Width = hw.Item(1)
        End If

    End Sub

#End Region

    Private Sub CPU() Handles Me.Load

        SetScreen()

        TimerGraph.Interval = 1000
        TimerGraph.Enabled = True

        ChartWrapper1.AxisXTitle = Global.Outworldz.My.Resources.AxisXTitle
        ChartWrapper2.AxisXTitle = Global.Outworldz.My.Resources.AxisXTitle

        Dim msChart = ChartWrapper1.TheChart
        msChart.ChartAreas(0).AxisX.Maximum = 180
        msChart.ChartAreas(0).AxisX.Minimum = 0
        msChart.ChartAreas(0).AxisY.Maximum = 100
        msChart.ChartAreas(0).AxisY.Minimum = 0
        msChart.ChartAreas(0).AxisY.LabelStyle.Enabled = True
        msChart.ChartAreas(0).AxisX.LabelStyle.Enabled = False
        ChartWrapper1.AddMarkers = False
        ChartWrapper1.MarkerFreq = 60

        msChart = ChartWrapper2.TheChart
        msChart.ChartAreas(0).AxisX.Maximum = 180
        msChart.ChartAreas(0).AxisX.Minimum = 0
        msChart.ChartAreas(0).AxisY.Maximum = 100
        msChart.ChartAreas(0).AxisY.Minimum = 0
        msChart.ChartAreas(0).AxisX.LabelStyle.Enabled = False
        msChart.ChartAreas(0).AxisY.LabelStyle.Enabled = True
        ChartWrapper2.AddMarkers = False
        ChartWrapper2.MarkerFreq = 60

        ChartWrapper1.AxisXTitle = My.Resources.OneMinute
        ChartWrapper2.AxisXTitle = My.Resources.OneMinute

        ChartWrapper1.ClearChart()
        Dim CPU1() As Double = MyCPUCollection.ToArray()
        ChartWrapper1.AddLinePlot("CPU", CPU1)

        ChartWrapper2.ClearChart()
        Dim RAM() As Double = MyRAMCollection.ToArray()
        ChartWrapper2.AddLinePlot("RAM", RAM)

        Settings.GraphVisible = True
        Settings.SaveSettings()

    End Sub

    Private Sub Form_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Settings.GraphVisible = False
        Settings.SaveSettings()

    End Sub

    Private Sub Timer() Handles TimerGraph.Tick
        ChartWrapper1.ClearChart()
        Dim CPU1() As Double = MyCPUCollection.ToArray()
        ChartWrapper1.AddLinePlot("CPU", CPU1)
        ChartWrapper2.ClearChart()
        Dim RAM() As Double = MyRAMCollection.ToArray()
        ChartWrapper2.AddLinePlot("RAM", RAM)

        If CPU1.Length > 0 Then
            Me.Text = $"{My.Resources.CPU_word} {Format(CPU1(UBound(CPU1)) / 100, "0.0%")} {My.Resources.RAM_Word} {Format(RAM(UBound(RAM)) / 100, "0.0%") }%"
        End If

    End Sub

End Class