#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.IO
Imports System.Text.RegularExpressions

Public Class FormLogging

#Region "Private Fields"

    Private _Avictr As Integer
    Dim _changed As Boolean
    Private _Err As Integer
    Dim _FileCounter As Integer
    Private _LineCounter As Integer
    Dim initted As Boolean

#End Region

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

#Region "Start/Stop"

    Private Sub Close_form(sender As Object, e As EventArgs) Handles Me.Closed

        Settings.SaveSettings()

        If _changed Then SendMsg(Settings.LogLevel.ToUpperInvariant)

    End Sub

    Private Sub Loaded(sender As Object, e As EventArgs) Handles Me.Load

        AnalyzeButton.Text = Global.Outworldz.My.Resources.AnalyzeLogButton
        Date_Time_Checkbox.Checked = Settings.ShowDateandTimeinLogs
        Date_Time_Checkbox.Text = Global.Outworldz.My.Resources.ShowDateTime
        DeleteOnBoot.Checked = Settings.DeleteByDate
        DeleteOnBoot.Text = Global.Outworldz.My.Resources.DeletebyAge
        GroupBox1.Text = Global.Outworldz.My.Resources.Log_Level
        HelpToolStripMenuItem.Image = Global.Outworldz.My.Resources.question_and_answer
        HelpToolStripMenuItem.Text = Global.Outworldz.My.Resources.Help_word
        KeepLog.Checked = Not Settings.DeleteByDate
        KeepLog.Text = Global.Outworldz.My.Resources.KeepAlways
        LogBenchmarks.Text = Global.Outworldz.My.Resources.LogBenchmarks
        NotePadButton.Text = My.Resources.NotePadButton
        RadioDebug.Text = Global.Outworldz.My.Resources.Debug_word
        RadioError.Text = Global.Outworldz.My.Resources.Error_word
        RadioFatal.Text = Global.Outworldz.My.Resources.Fatal_word
        RadioInfo.Text = Global.Outworldz.My.Resources.Info_word
        RadioOff.Text = Global.Outworldz.My.Resources.Off
        RadioWarn.Text = Global.Outworldz.My.Resources.Warn_word
        ViewLogButton.Text = Global.Outworldz.My.Resources.View_Logs

        LogBenchmarks.Checked = Settings.LogBenchmarks

        SetScreen()

        Select Case Settings.LogLevel.ToUpperInvariant
            Case "OFF"
                RadioOff.Checked = True
            Case "DEBUG"
                RadioDebug.Checked = True
            Case "INFO"
                RadioInfo.Checked = True
            Case "WARN"
                RadioWarn.Checked = True
            Case "ERROR"
                RadioError.Checked = True
            Case "FATAL"
                RadioFatal.Checked = True
            Case Else
                RadioInfo.Checked = True
        End Select

        If Settings.Logger = "Baretail" Then
            BaretailButton.Checked = True
        Else
            NotePadButton.Checked = True
        End If

        HelpOnce("Logging")
        initted = True

    End Sub

#End Region

#Region "Help"

    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.Click
        HelpManual("Logging")
    End Sub

#End Region

#Region "SetLogging"

    Private Sub Delete_CheckedChanged(sender As Object, e As EventArgs) Handles DeleteOnBoot.CheckedChanged

        If Not initted Then Return
        If DeleteOnBoot.Checked Then Settings.DeleteByDate = True

    End Sub

    Private Sub KeepLog_CheckedChanged(sender As Object, e As EventArgs) Handles KeepLog.CheckedChanged

        If Not initted Then Return
        If KeepLog.Checked Then Settings.DeleteByDate = False

    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioOff.CheckedChanged
        If Not initted Then Return
        Settings.LogLevel = "OFF"
        _changed = True
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioDebug.CheckedChanged
        If Not initted Then Return
        Settings.LogLevel = "DEBUG"
        _changed = True
    End Sub

    Private Sub RadioButton3_CheckedChanged(sender As Object, e As EventArgs) Handles RadioInfo.CheckedChanged
        If Not initted Then Return
        Settings.LogLevel = "INFO"
        _changed = True
    End Sub

    Private Sub RadioButton4_CheckedChanged(sender As Object, e As EventArgs) Handles RadioWarn.CheckedChanged
        If Not initted Then Return
        Settings.LogLevel = "WARN"
        _changed = True
    End Sub

    Private Sub RadioButton5_CheckedChanged(sender As Object, e As EventArgs) Handles RadioError.CheckedChanged
        If Not initted Then Return
        Settings.LogLevel = "ERROR"
        _changed = True
    End Sub

    Private Sub RadioButton6_CheckedChanged(sender As Object, e As EventArgs) Handles RadioFatal.CheckedChanged
        If Not initted Then Return
        Settings.LogLevel = "FATAL"
        _changed = True
    End Sub

#End Region

#Region "ExamineLogs"

    Private Shared Sub LookatMac(line As String, outputfile As StreamWriter)

        '2021-04-19 07:05:46,389 INFO  (99) - OpenSim.Services.HypergridService.GatekeeperService [GATEKEEPER SERVICE]: Login failed, reason: client with mac (.*?) is denied

        Dim pattern = New Regex("^(.*?),.*?INFO.*?mac (.*?) is denied")
        Dim match As Match = pattern.Match(line)
        If match.Success Then
            Dim DateTime = match.Groups(1).Value
            Dim MAC = match.Groups(2).Value

            outputfile.WriteLine($"<tr bgcolor=""gray""><td>{DateTime}</td><td>MAC BANNED</td><td></td><td></td><td></td><td></td><td>{MAC}</td><td></td><td></td><td></td></tr>")
        End If

    End Sub

    Private Shared Sub LookatYengine(line As String, outputfile As StreamWriter, RegionName As String)

        Dim pattern = New Regex("^(.*?)(\[YEngine\]\:.*)")
        Dim match As Match = pattern.Match(line)
        If match.Success Then
            Dim DateTime1 As String = ""
            Try
                DateTime1 = match.Groups(1).Value
            Catch
            End Try
            outputfile.WriteLine($"<tr><td>{DateTime1}</td><td>{RegionName}</td><td>{line}</td></tr>")
        End If
        Return

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles AnalyzeButton.Click

        AnalyzeButton.Text = Global.Outworldz.My.Resources.Busy_word

        Dim TMPFolder = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\tmp")
        FileIO.FileSystem.CreateDirectory(TMPFolder)
        Dim Out = IO.Path.Combine(TMPFolder, "Avatars.htm")
        DeleteFile(Out)
        ExamineAvatars(Out)
        Process.Start(Out)

        Out = IO.Path.Combine(TMPFolder, "Regions.htm")
        DeleteFile(Out)
        _Err = 0

        Try
            Using outputFile As New StreamWriter(Out, False)
                outputFile.WriteLine("<style>table, th, td {border: 1px solid black; padding: 5px; text-align: left;border-spacing: 0px;border-collapse: collapse;}</style>")
                outputFile.WriteLine("<style>#t01 tr:nth-child(even) {  background-color: #eee;}#t01 tr:nth-child(odd) { background-color: #fff;}#t01 th {  background-color: black;  color: white;}</style>")
                outputFile.WriteLine("<table id=""t01"">")
                outputFile.WriteLine("<tr><td>DateType</td><td>Region</td><td>Message</td></tr>")

                For Each UUID In RegionUuids()
                    Application.DoEvents()
                    Dim GroupName = Group_Name(UUID)
                    ExamineOpensim(outputFile, GroupName, Region_Name(UUID))
                Next
                outputFile.WriteLine("</table>")
            End Using
        Catch
        End Try

        If _Err > 0 Then Process.Start(Out)

        AnalyzeButton.Text = Global.Outworldz.My.Resources.AnalyzeLogButton

    End Sub

    Private Sub Date_Time_Checkbox_CheckedChanged_1(sender As Object, e As EventArgs) Handles Date_Time_Checkbox.CheckedChanged

        Settings.ShowDateandTimeinLogs = Date_Time_Checkbox.Checked

    End Sub

    Private Sub ExamineAvatars(Log As String)

        Try
            Using outputFile As New StreamWriter(Log, True)
                outputFile.WriteLine("<style>table, th, td {border: 1px solid black; padding: 5px; text-align: left;border-spacing: 0px;border-collapse: collapse;}</style>")
                outputFile.WriteLine("<style>#t01 tr:nth-child(even) {  background-color: #eee;}#t01 tr:nth-child(odd) { background-color: #fff;}#t01 th {  background-color: black;  color: white;}</style>")
                outputFile.WriteLine("<table id=""t01"">")

                outputFile.WriteLine("<tr><td>Date</td><td>Avatar</td><td>IP</td><td>UUID</td><td>Viewer</td><td>Channel</td><td>IP</td><td>MAC</td><td>ID0</td></tr>")

                Dim Robust = IO.Path.Combine(Settings.OpensimBinPath, "Robust.log")
                If System.IO.File.Exists(Robust) Then
                    Using F As New FileStream(Robust, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                        Using S = New StreamReader(F)
                            'now loop through each line
                            While S.Peek <> -1
                                Application.DoEvents()
                                Dim l = S.ReadLine
                                Lookat(l, outputFile)
                                LookatMac(l, outputFile)
                            End While
                        End Using
                    End Using
                End If
                outputFile.WriteLine("</table>")
            End Using
            If _Avictr > 0 Then Process.Start(IO.Path.Combine(Log))
        Catch ex As Exception
            MsgBox(ex.Message, vbInformation Or MsgBoxStyle.MsgBoxSetForeground)
        End Try

    End Sub

    Private Sub ExamineOpensim(outputfile As StreamWriter, GroupName As String, RegionName As String)

        Try
            Dim Region = IO.Path.Combine(Settings.OpensimBinPath, $"Regions\{GroupName}\Opensim.log")
            If System.IO.File.Exists(Region) Then
                Using F = New FileStream(Region, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                    Using S = New StreamReader(F)
                        'now loop through each line
                        While S.Peek <> -1
                            _LineCounter += 1
                            Dim line = S.ReadLine()
                            LookatOpensim(line, outputfile, RegionName)
                            LookatYengine(line, outputfile, RegionName)
                            Application.DoEvents()
                        End While
                    End Using
                End Using
            End If
        Catch ex As Exception
            MsgBox(ex.Message, vbInformation Or MsgBoxStyle.MsgBoxSetForeground)
        End Try
        _FileCounter += 1

    End Sub

    Private Sub Lookat(line As String, outputfile As StreamWriter)

        '2021-06-27 19:50:17,103 INFO  (31) - OpenSim.Services.HypergridService.GatekeeperService [GATEKEEPER SERVICE]:
        ''Login request for Test User @ http: //192.168.2.100:8002/ (44bc90a1-407a-48e8-9cc3-9fcf0a6d32fa)
        ''at 74159a1f-1019-4947-943c-9686a1ccf466 using viewer Firestorm-Releasex64 6.4.12.62831, channel Firestorm-Releasex64,
        ''IP 192.168.2.100, Mac 1cd06720d0cb1737ad6e4f159e25a0db, Id0 a5e2d9cb0462874d43f24feeb248d6e7, Teleport Flags: ViaLogin, ViaRegionID.From region Unknown

        Dim pattern = New Regex("^(.*?),.*?Login request for (.*?) \@ (.*?) \((.*?)\).*?viewer (.*?), channel (.*?), IP (.*?), Mac (.*?), Id0 (.*?,)")
        Dim match As Match = pattern.Match(line)
        If match.Success Then
            Dim DateTime = match.Groups(1).Value
            Dim Avatar = match.Groups(2) '.Value.Replace(" ", " ")
            Dim UUID = match.Groups(3).Value
            Dim Viewer = match.Groups(4).Value
            Dim Channel = match.Groups(5).Value
            Dim IP = match.Groups(6).Value
            Dim MAC = match.Groups(7).Value
            Dim Id0 = match.Groups(8).Value
            Dim Region = match.Groups(9).Value
            _Avictr += 1

            outputfile.WriteLine($"<tr><td>{DateTime}</td><td>{Avatar}</td><td>{UUID}</td><td>{Viewer}</td><td>{Channel}</td><td>{IP}</td><td>{MAC}</td><td>{Id0}</td><td>{Region}</td></tr>")
        End If

    End Sub

    Private Sub LookatOpensim(line As String, outputfile As StreamWriter, RegionName As String)

        Dim pattern = New Regex("^(.*?),.*?ERROR(.*?)(<.*?,.*?,.*?>)(.*)")
        Dim match As Match = pattern.Match(line)
        If match.Success Then
            Dim DateTime = match.Groups(1).Value
            Dim Preamble = match.Groups(2).Value
            Dim Vector = match.Groups(3).Value
            Dim Last = match.Groups(4).Value
            Dim v = Vector
            v = v.Replace(",", "/")
            v = v.Replace(" ", "")
            v = v.Replace("<", "")
            v = v.Replace(">", "")
            outputfile.WriteLine($"<tr><td>{DateTime}</td><td>{RegionName}</td><td>{Preamble} <a href=""hop://{Settings.PublicIP}:{Settings.HttpPort}/{RegionName}/{v}""> {RegionName}/{v}</a> {Last}</td></tr>")
            _Err += 1
        End If
        Return

    End Sub

    Private Sub OutputViewerButton_CheckedChanged(sender As Object, e As EventArgs) Handles NotePadButton.CheckedChanged

        Settings.Logger = "NotePad"
        Settings.SaveSettings()

    End Sub

#End Region

#Region "Buttons"

    Private Sub BaretailButton_CheckedChanged(sender As Object, e As EventArgs) Handles BaretailButton.CheckedChanged

        Settings.Logger = "Baretail"
        Settings.SaveSettings()

    End Sub

    Private Sub BaretailPictureBox_Click(sender As Object, e As EventArgs)
        Dim webAddress As String = "https://www.baremetalsoft.com/baretail/"
        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles ViewLogButton.Click
        Process.Start("explorer.exe", IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Logs"))
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles LogBenchmarks.CheckedChanged

        Settings.LogBenchmarks = LogBenchmarks.Checked

    End Sub

    Private Sub QuickmanagerPictureBox_Click(sender As Object, e As EventArgs)
        Dim webAddress As String = "https://github.com/itlezy/ITLezyTools#outputviewer"
        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try
    End Sub

#End Region

End Class
