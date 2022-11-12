#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Public Class FormTide

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
        'Me.Text = "Form screen position = " + Me.Location.ToString
        ScreenPosition.SaveXY(Me.Left, Me.Top)
    End Sub

    Private Sub SetScreen()

        ScreenPosition = New ClassScreenpos(Me.Name)
        AddHandler ResizeEnd, Handler
        Dim xy As List(Of Integer) = ScreenPosition.GetXY()
        Me.Left = xy.Item(0)
        Me.Top = xy.Item(1)
    End Sub

#End Region

#Region "Private Methods"

    Private Sub BroadcastTideInfo_CheckedChanged(sender As Object, e As EventArgs) Handles BroadcastTideInfo.CheckedChanged
        Settings.BroadcastTideInfo = BroadcastTideInfo.Checked
    End Sub

    Private Sub CycleTimeTextBox_TextChanged(sender As Object, e As EventArgs) Handles CycleTimeTextBox.TextChanged
        Settings.CycleTime = CType("0" & CycleTimeTextBox.Text, Integer)
    End Sub

    Private Sub IsClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed

        Settings.SaveSettings()

    End Sub

    Private Sub Loaded(sender As Object, e As EventArgs) Handles Me.Load

        BroadcastTideInfo.Text = Global.Outworldz.My.Resources.Broadcast_Tide_Info
        TideEnabledCheckbox.Text = Global.Outworldz.My.Resources.Enable_word
        TideInfoDebugCheckBox.Text = Global.Outworldz.My.Resources.Send_Debug_Info
        ToolStripMenuItem30.Image = Global.Outworldz.My.Resources.question_and_answer
        ToolStripMenuItem30.Text = Global.Outworldz.My.Resources.Help_word
        ToolTip1.SetToolTip(BroadcastTideInfo, Global.Outworldz.My.Resources.Broadcast_Tide_Chat)
        ToolTip1.SetToolTip(CycleTimeTextBox, Global.Outworldz.My.Resources.Cycle_time_text)
        ToolTip1.SetToolTip(TideHighLevelTextBox, Global.Outworldz.My.Resources.High_Water_Level_text)
        ToolTip1.SetToolTip(TideInfoDebugCheckBox, Global.Outworldz.My.Resources.Provide_Info)
        ToolTip1.SetToolTip(TideLowLevelTextBox, Global.Outworldz.My.Resources.Low_High)
        ToolTip1.ToolTipTitle = Global.Outworldz.My.Resources.Tide_Enable
        TideEnabledCheckbox.Checked = CType(Settings.TideEnabled, Boolean)
        TideHighLevelTextBox.Text = Convert.ToString(Settings.TideHighLevel(), Globalization.CultureInfo.InvariantCulture)
        TideLowLevelTextBox.Text = Convert.ToString(Settings.TideLowLevel(), Globalization.CultureInfo.InvariantCulture)
        CycleTimeTextBox.Text = Settings.CycleTime.ToString(Globalization.CultureInfo.InvariantCulture)
        BroadcastTideInfo.Checked = CType(Settings.BroadcastTideInfo, Boolean)
        TideInfoChannelTextBox.Text = CStr(Settings.TideInfoChannel)
        TideHiLoChannelTextBox.Text = CStr(Settings.TideLevelChannel)
        TideInfoDebugCheckBox.Checked = Settings.TideInfoDebug
        GroupBox1.Text = My.Resources.GlobalTideSettings
        HighwaterLabel.Text = My.Resources.HighwaterLevel
        LowWaterLevelLabel.Text = My.Resources.LowWaterLevel
        TideInfoChannelLabel.Text = My.Resources.TideInfoChannel
        TideHiLoChannelLabel.Text = My.Resources.TideHiLoChannelLabel

        SetScreen()
        HelpOnce("Tides")

    End Sub

    Private Sub RunOnBoot_Click(sender As Object, e As EventArgs)
        HelpManual("Tides")
    End Sub

    Private Sub TideEnabledCheckbox_CheckedChanged(sender As Object, e As EventArgs) Handles TideEnabledCheckbox.CheckedChanged
        Settings.TideEnabled = TideEnabledCheckbox.Checked
    End Sub

    Private Sub TideHghLevelTextBox_TextChanged(sender As Object, e As EventArgs) Handles TideHighLevelTextBox.TextChanged
        Settings.TideHighLevel() = CType("0" & TideHighLevelTextBox.Text, Single)
    End Sub

    Private Sub TideHiLoChannelTextBox_TextChanged(sender As Object, e As EventArgs) Handles TideHiLoChannelTextBox.TextChanged
        Settings.TideLevelChannel = CType("0" & TideHiLoChannelTextBox.Text, Integer)
    End Sub

    Private Sub TideInfoChannelTextBox_TextChanged(sender As Object, e As EventArgs) Handles TideInfoChannelTextBox.TextChanged
        Settings.TideInfoChannel = CType("0" & TideInfoChannelTextBox.Text, Integer)
    End Sub

    Private Sub TideInfoDebugCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles TideInfoDebugCheckBox.CheckedChanged
        Settings.TideInfoDebug = TideInfoDebugCheckBox.Checked
    End Sub

    Private Sub TideLowLevelTextBox_TextChanged(sender As Object, e As EventArgs) Handles TideLowLevelTextBox.TextChanged
        Settings.TideLowLevel() = CType("0" & TideLowLevelTextBox.Text, Single)
    End Sub

    Private Sub ToolStripMenuItem30_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem30.Click
        HelpManual("Tides")
    End Sub

#End Region

End Class
