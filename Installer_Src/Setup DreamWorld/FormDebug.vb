Imports System.Net

Public Class FormDebug

    Private _backup As Boolean
    Private _command As String
    Private _value As Boolean

#Region "FormPos"

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

#Region "Properties"

    Public Property Backup As Boolean
        Get
            Return _backup
        End Get
        Set(value As Boolean)
            _backup = value
        End Set
    End Property

    Public Property Command As String
        Get
            Return _command
        End Get
        Set(value As String)
            _command = value
        End Set
    End Property

    Public Property Value As Boolean
        Get
            Return _value
        End Get
        Set(value As Boolean)
            _value = value
        End Set
    End Property

#End Region

#Region "Scrolling text box"

    Public Sub ProgressPrint(Value As String)
        Log(My.Resources.Info_word, Value)
        TextBox1.Text += Value & vbCrLf
        If TextBox1.Text.Length > TextBox1.MaxLength - 1000 Then
            TextBox1.Text = Mid(TextBox1.Text, 1000)
        End If
    End Sub

    Private Sub TextBox1_Changed(sender As System.Object, e As EventArgs) Handles TextBox1.TextChanged
        Dim ln As Integer = TextBox1.Text.Length
        TextBox1.SelectionStart = ln
        TextBox1.ScrollToCaret()
    End Sub

#End Region

#Region "Set"

    Private Shared Sub MakeMap()

        Try
#Disable Warning CA2000 ' Dispose objects before losing scope
            Dim NewMap = New FormGlobalMap
#Enable Warning CA2000 ' Dispose objects before losing scope
            NewMap.Show()
            NewMap.Activate()
            NewMap.Select()
            NewMap.BringToFront()
        Catch
        End Try

    End Sub

    Private Shared Sub Poketest()
        For Each RegionUUID In RegionUuids()
            Thaw(RegionUUID)
            ConsoleCommand(RegionUUID, "show stats")
        Next
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles ApplyButton.Click

        If Command = My.Resources.MakeNewMap Then
            MakeMap()

        ElseIf Command = My.Resources.TeleportAPI Then

            TPAPITest()

        ElseIf Command = "All region stats" Then

            Poketest()

        ElseIf Command = $"{My.Resources.Debug_word} {My.Resources.Off}" Then

            If Value Then
                Settings.StatusInterval = 0
                ProgressPrint($"{My.Resources.Debug_word} {My.Resources.Off}")
            Else
                ProgressPrint(My.Resources.Unchanged)
            End If
            Settings.SaveSettings()

        ElseIf Command = $"{My.Resources.Debug_word} 1 {My.Resources.Minute}" Then

            If Value Then
                Settings.StatusInterval = 60
                ProgressPrint($"{My.Resources.Debug_word} 1 {My.Resources.Minute}")
            Else
                Settings.StatusInterval = 0
                ProgressPrint($"{My.Resources.Debug_word} {My.Resources.Off}")
            End If
            Settings.SaveSettings()

        ElseIf Command = $"{My.Resources.Debug_word} 10 {My.Resources.Minutes}" Then

            If Value Then
                Settings.StatusInterval = 600
                ProgressPrint($"{My.Resources.Debug_word} 10 {My.Resources.Minutes}")
            Else
                Settings.StatusInterval = 0
                ProgressPrint($"{My.Resources.Debug_word} Off")
            End If
            Settings.SaveSettings()

        ElseIf Command = $"{My.Resources.Debug_word} 60 {My.Resources.Minutes}" Then

            If Value Then
                Settings.StatusInterval = 3600
                ProgressPrint($"{My.Resources.Debug_word} 60 {My.Resources.Minutes}")
            Else
                Settings.StatusInterval = 0
                ProgressPrint($"{My.Resources.Debug_word} {My.Resources.Off}")
            End If
            Settings.SaveSettings()

        ElseIf Command = $"{My.Resources.Debug_word} 24 {My.Resources.Hours}" Then

            If Value Then
                Settings.StatusInterval = 60 * 60 * 24
                ProgressPrint($"{My.Resources.Debug_word} 24 {My.Resources.Hours}")
            Else
                Settings.StatusInterval = 0
                ProgressPrint($"{My.Resources.Debug_word} {My.Resources.Off}")
            End If
            Settings.SaveSettings()

        End If

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

        Command = CStr(ComboBox1.SelectedItem)
        Value = RadioTrue.Checked

    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

        RadioTrue.Checked = False
        RadioFalse.Checked = True

        RadioTrue.Text = My.Resources.True_word
        RadioFalse.Text = My.Resources.False_word

        ComboBox1.Items.Add("All region stats")
        ComboBox1.Items.Add(My.Resources.TeleportAPI)
        ComboBox1.Items.Add($"{My.Resources.Debug_word} {My.Resources.Off}")
        ComboBox1.Items.Add($"{My.Resources.Debug_word} 1 {My.Resources.Minute}")
        ComboBox1.Items.Add($"{My.Resources.Debug_word} 10 {My.Resources.Minutes}")
        ComboBox1.Items.Add($"{My.Resources.Debug_word} 60 {My.Resources.Minutes}")
        ComboBox1.Items.Add($"{My.Resources.Debug_word} 24 {My.Resources.Hours}")

        ComboBox1.Items.Add(My.Resources.MakeNewMap)

        SetScreen()

        HelpOnce("Debug")

    End Sub

    Private Sub TPAPITest()

        If Value Then
            Dim region = ChooseRegion(False)
            Dim UUID = Guid.NewGuid().ToString
            Dim AviName = InputBox("Avatar Name?")
            Dim AviUUID As String = ""
            If AviName.Length > 0 Then
                AviUUID = Uri.EscapeDataString(MysqlInterface.GetAviUUUD(AviName))
                If AviUUID.Length > 0 Then
                    Dim url = $"http://{Settings.PublicIP}:{Settings.DiagnosticPort}/alt={region}&agent=AviName&AgentID={AviUUID}&password={Settings.MachineId}"
                    ProgressPrint(url)
                    Using client As New WebClient ' download client for web pages
                        Dim r As String = ""
                        Try
                            r = client.DownloadString(url)
                        Catch ex As Exception
                            ProgressPrint(ex.Message)
                        End Try
                        ProgressPrint(r)
                    End Using
                Else
                    ProgressPrint("Avatar Not located")
                End If
            Else
                ProgressPrint($"{My.Resources.Aborted_word} ")
            End If
        End If

    End Sub

#End Region

#Region "Radio"

    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.Click
        HelpManual("Debug")

    End Sub

    Private Sub RadioTrue_CheckedChanged(sender As Object, e As EventArgs) Handles RadioTrue.CheckedChanged

        Value = RadioTrue.Checked

    End Sub

#End Region

End Class