#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.IO
Imports System.Reflection
Imports System.Text.RegularExpressions

Public Class FormRegionlist

    '// Constants
    Const HWND_TOP As Integer = 0

    'Const HWND_TOPMOST As Integer = -1
    'Const HWND_NO_TOPMOST As Integer = -2
    Const NOMOVE As Long = &H2

    Const NOSIZE As Long = &H1
    Private ReadOnly colsize As New ClassScreenpos("Region List")
    Private ReadOnly Handler As New EventHandler(AddressOf Resize_page)
    Private ReadOnly SearchArray As New List(Of String)
    Private _ImageListSmall As New ImageList
    Dim _order As SortOrder
    Private _screenPosition As ClassScreenpos
    Private _SortColumn As Integer
    Private detailsinitted As Boolean
    Private initted As Boolean

    Dim RegionForm As New FormRegion
    Private TotalRam As Double
    Private UseMysql As Boolean

#Region "Declarations"

    Private SearchBusy As Boolean
    Private TheView As Integer = ViewType.Details

    ' icons image list layout
    Enum DGICON
        bootingup = 0
        shuttingdown = 1
        up = 2
        disabled = 3
        stopped = 4
        recyclingdown = 5
        recyclingup = 6
        warning = 7
        user1 = 8
        user2 = 9
        SmartStartStopped = 10
        Home = 11
        HomeOffline = 12
        Pending = 13
        Suspended = 14
        ErrorIcon = 15
        NoLogOn = 16
        NoError = 17
        NoEstate = 18
        icecube = 19
        icemelted = 20

    End Enum

    Private Enum ViewType As Integer
        Icons = 1
        Details = 2
        Avatars = 3
        Users = 4
    End Enum

#End Region

#Region "Properties"

    Public Property ImageListSmall As ImageList
        Get
            Return _ImageListSmall
        End Get
        Set(value As ImageList)
            _ImageListSmall = value
        End Set
    End Property

    Public Property ScreenPosition As ClassScreenpos
        Get
            Return _screenPosition
        End Get
        Set(value As ClassScreenpos)
            _screenPosition = value
        End Set
    End Property

    Public Property TheView1 As Integer
        Get
            Return TheView
        End Get
        Set(value As Integer)
            TheView = value
        End Set
    End Property

#End Region

#Region "ScreenSize"

#End Region

#Region "Data"

    Public Shared Sub WriteDataTable(ByVal sourceTable As DataTable, ByVal writer As TextWriter, ByVal includeHeaders As Boolean)

        If sourceTable Is Nothing Then Return
        If writer Is Nothing Then Return

        If (includeHeaders) Then
            Dim headerValues As IEnumerable(Of String) = sourceTable.Columns.OfType(Of DataColumn).Select(Function(column) QuoteValue(column.ColumnName))
            writer.WriteLine(String.Join(",", headerValues))
        End If

        Dim items As IEnumerable(Of String) = Nothing
        For Each row As DataRow In sourceTable.Rows
            items = row.ItemArray.Select(Function(obj) QuoteValue(If(obj?.ToString(), String.Empty)))
            writer.WriteLine(String.Join(",", items))
        Next

        writer.Flush()

    End Sub

    Private Shared Sub DoubleBuff(ByVal control As Control, ByVal enable As Boolean)
        Dim doubleBufferPropertyInfo = control.[GetType]().GetProperty("DoubleBuffered", BindingFlags.Instance Or BindingFlags.NonPublic)
        doubleBufferPropertyInfo.SetValue(control, enable, Nothing)
    End Sub

    Private Shared Function GetRegionsName(Region As String) As String

        Dim p1 As String = ""
        Using reader = New StreamReader(Region)
            While reader.Peek <> -1 And p1.Length = 0
                Dim line = reader.ReadLine
                Dim pattern1 = New Regex("^ *\[(.*?)\] *$")
                Dim match1 As Match = pattern1.Match(line)
                If match1.Success Then
                    p1 = match1.Groups(1).Value
                End If
            End While
        End Using
        Return p1

    End Function

    Private Shared Function LoadImage(S As String) As Image

        Dim bmp As Bitmap = Nothing
        Dim u As New Uri(S)
        Dim request As System.Net.WebRequest = Net.WebRequest.Create(u)
        Dim response As System.Net.WebResponse = Nothing
        Try
            response = request.GetResponse()
        Catch ex As Exception
            ' BreakPoint.Dump(ex)
        End Try

        Dim responseStream As System.IO.Stream = Nothing
        Try
            responseStream = response.GetResponseStream()
        Catch ex As Exception
            'BreakPoint.Dump(ex)
        End Try

        If responseStream IsNot Nothing Then
            bmp = New Bitmap(responseStream)
            responseStream.Dispose()
        End If

        Return bmp

    End Function

    Private Shared Function PickGroup() As String

        Dim Chooseform As New FormChooser ' form for choosing a set of regions
        ' Show testDialog as a modal dialog and determine if DialogResult = OK.

        Chooseform.FillGrid("Group")
        Chooseform.BringToFront()
        Dim chosen As String
        Chooseform.ShowDialog()
        Try
            ' Read the chosen GROUP name
            chosen = Chooseform.DataGridView.CurrentCell.Value.ToString()
        Catch ex As Exception
            BreakPoint.Dump(ex)
            chosen = ""
        End Try

        Chooseform.Dispose()
        Return chosen

    End Function

    Private Shared Function QuoteValue(ByVal value As String) As String
        Return String.Concat("""", value.Replace("""", """"""), """")
    End Function

    Private Shared Sub SetWindowOnTop(ByVal lhWnd As Int32)

        On Error GoTo SetWindowOnTop_Err

        SetWindowPos(lhWnd, HWND_TOP, 0, 0, 0, 0, NOMOVE Or NOSIZE)

SetWindowOnTop_Exit:
        Exit Sub

SetWindowOnTop_Err:
        Resume SetWindowOnTop_Exit

    End Sub

    Private Shared Sub StartStopEdit(RegionUUID As String, RegionName As String)

        Dim Choices As New FormRegionPopup
        Dim chosen As String = ""
        Choices.Init(RegionName)
        Choices.BringToFront()
        Choices.ShowDialog()

        ' Read the chosen sim name
        chosen = Choices.Choice()
        Choices.Dispose()

        If chosen = "Start" Then
            ResumeRegion(RegionUUID)
            If RegionStatus(RegionUUID) = SIMSTATUSENUM.Suspended Then
                RegionStatus(RegionUUID) = SIMSTATUSENUM.Resume
                Return
            End If

            FormSetup.Buttons(FormSetup.BusyButton)

            If Not StartMySQL() Then
                TextPrint(My.Resources.Stopped_word)
            End If
            If Not StartRobust() Then
                TextPrint(My.Resources.Stopped_word)
            End If

            Log("Starting", Region_Name(RegionUUID))

            PropAborting = False

            If CBool(GetHwnd(Group_Name(RegionUUID))) Then
                RegionStatus(RegionUUID) = SIMSTATUSENUM.Booted
            Else
                RegionStatus(RegionUUID) = SIMSTATUSENUM.Resume
            End If
            PropUpdateView() = True
            FormSetup.StartTimer()

            FormSetup.Buttons(FormSetup.StopButton)
            PropOpensimIsRunning() = True

        ElseIf chosen = "Stop" Then
            ResumeRegion(RegionUUID)
            ' if any avatars in any region, give them a choice.
            Dim StopIt As Boolean = True
            For Each num In RegionUuidListByName(Group_Name(RegionUUID))
                ' Ask before killing any people
                If AvatarCount(num) > 0 Then
                    Dim response As MsgBoxResult
                    If AvatarCount(num) = 1 Then
                        response = MsgBox(My.Resources.OneAvatar & " " & Region_Name(num) & " " & Global.Outworldz.My.Resources.Do_you_still_want_to_Stop_word, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground)
                    Else
                        response = MsgBox(AvatarCount(num).ToString(Globalization.CultureInfo.InvariantCulture) + " " & Global.Outworldz.My.Resources.Avatars_are_in & " " + Region_Name(num) + ". " & Global.Outworldz.My.Resources.Do_you_still_want_to_Stop_word, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground)
                    End If
                    If response = vbNo Then
                        StopIt = False
                    End If
                End If
            Next

            If (StopIt) Then
                StopRegion(RegionUUID)
            End If

        ElseIf chosen = "Console" Then

            ResumeRegion(RegionUUID)
            Dim hwnd = GetHwnd(Group_Name(RegionUUID))
            If hwnd = IntPtr.Zero Then
                ' shut down all regions in the DOS box
                For Each UUID As String In RegionUuidListByName(Group_Name(RegionUUID))
                    RegionStatus(UUID) = SIMSTATUSENUM.Stopped ' already shutting down
                Next
                DelPidFile(RegionUUID)
                PropUpdateView = True ' make form refresh
            Else
                Dim tmp As String = Settings.ConsoleShow
                'temp show console
                Settings.ConsoleShow = "True"
                If Not ShowDOSWindow(hwnd, SHOWWINDOWENUM.SWRESTORE) Then
                    ' shut down all regions in the DOS box
                    For Each UUID As String In RegionUuidListByName(Group_Name(RegionUUID))
                        RegionStatus(UUID) = SIMSTATUSENUM.Stopped ' already shutting down
                    Next
                    DelPidFile(RegionUUID)
                    Return
                End If

                SetWindowOnTop(hwnd.ToInt32)
                Settings.ConsoleShow = tmp
            End If

        ElseIf chosen = "Edit" Then

#Disable Warning CA2000 ' Dispose objects before losing scope
            Dim RegionForm As New FormRegion
#Enable Warning CA2000 ' Dispose objects before losing scope
            RegionForm.BringToFront()
            RegionForm.Init(RegionName)
            RegionForm.Activate()
            RegionForm.Visible = True
            RegionForm.Select()

        ElseIf chosen = "Restart" Then
            ResumeRegion(RegionUUID)
            FormSetup.Buttons(FormSetup.BusyButton)

            ' shut down all regions in the DOS box
            Dim GroupName = Group_Name(RegionUUID)

            ShutDown(RegionUUID, SIMSTATUSENUM.RecyclingDown)
            FormSetup.Buttons(FormSetup.StopButton)

            TextPrint(My.Resources.Recycle1 & "  " + Group_Name(RegionUUID))

            PropUpdateView = True ' make form refresh

        ElseIf chosen = "Teleport" Then
            ResumeRegion(RegionUUID)
            Dim Obj = New TaskObject With {
                        .TaskName = FormSetup.TaskName.TeleportClicked,
                        .Command = ""
                    }
            FormSetup.RebootAndRunTask(RegionUUID, Obj)

        ElseIf chosen = "Load" Then
            ResumeRegion(RegionUUID)
            LoadOar(RegionName)

        ElseIf chosen = "Save" Then
            ResumeRegion(RegionUUID)
            SaveOar(RegionName)

        End If

    End Sub

    Private Sub Addregion_Click(sender As Object, e As EventArgs) Handles AddRegionButton.Click

        Try
            RegionForm.Dispose()
            RegionForm = New FormRegion
            RegionForm.BringToFront()
            RegionForm.Init("")
            RegionForm.Activate()
            RegionForm.Visible = True
            RegionForm.Select()
        Catch
        End Try

    End Sub

    Private Sub AllButton_CheckedChanged(sender As Object, e As EventArgs) Handles AllButton.CheckedChanged

        If Not initted Then Return
        LoadMyListView()

    End Sub

    Private Sub AllNone_CheckedChanged(sender As Object, e As EventArgs) Handles AllNone.CheckedChanged

        If Not initted Then Return

        detailsinitted = False ' don't toggle the codes

        If TheView1 = ViewType.Users Then

            For Each X As ListViewItem In UserView.Items

                If AllNone.Checked Then
                    If X.ForeColor = Color.Black Then
                        X.Checked = AllNone.Checked
                    End If
                Else
                    X.Checked = False
                End If
            Next

        ElseIf TheView1 = ViewType.Details Then

            For Each X As ListViewItem In ListView1.Items
                Dim RegionUUID As String

                Dim name = X.Text
                If name.Length > 0 Then
                    RegionUUID = FindRegionByName(name)

                    If OnButton.Checked And Not RegionEnabled(RegionUUID) Then Continue For
                    If OffButton.Checked And RegionEnabled(RegionUUID) Then Continue For
                    If SmartButton.Checked And Not Smart_Start(RegionUUID) = "True" Then Continue For

                    RegionEnabled(RegionUUID) = AllNone.Checked

                    Dim INI = New LoadIni(RegionIniFilePath(RegionUUID), ";", System.Text.Encoding.UTF8)
                    INI.SetIni(Region_Name(RegionUUID), "Enabled", CStr(RegionEnabled(RegionUUID)))
                    INI.SaveINI()
                    Application.DoEvents()
                End If
            Next
        End If

        PropUpdateView = True ' make form refresh
        detailsinitted = True

    End Sub

    Private Sub AvatarView_Click(sender As Object, e As EventArgs) Handles AvatarView.Click

        Dim regions As ListView.SelectedListViewItemCollection = Me.AvatarView.SelectedItems
        Dim item As ListViewItem

        For Each item In regions
            Dim RegionName = item.SubItems(1).Text
            Dim RegionUUID As String = FindRegionByName(RegionName)
            If RegionUUID.Length > 0 Then
                ' TODO: Needs to be HGV3?
                Dim webAddress As String = "hop://" & Settings.DNSName & ":" & Settings.HttpPort & "/" & RegionName
                Try
                    Dim result = Process.Start(webAddress)
                Catch ex As Exception
                End Try
            End If
        Next
        PropUpdateView() = True

    End Sub

    Private Sub Avatarview_ColumnWidthChanged(sender As Object, e As ColumnWidthChangedEventArgs) Handles AvatarView.ColumnWidthChanged

        Dim w = AvatarView.Columns(e.ColumnIndex).Width
        Dim name = AvatarView.Columns(e.ColumnIndex).Name
        If name.Length = 0 Or w = 0 Then Return

        ScreenPosition.PutSize(name, w)
        ScreenPosition.SaveFormSettings()

    End Sub

    Private Sub Bootedbutton_CheckedChanged(sender As Object, e As EventArgs) Handles Bootedbutton.CheckedChanged

        LoadMyListView()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles RefreshButton.Click

        PropChangedRegionSettings = True
        GetAllRegions(False)

        LoadMyListView()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles ImportButton.Click

        Using ofd As New OpenFileDialog With {
            .InitialDirectory = "\",
            .Filter = Global.Outworldz.My.Resources.INI_Filter,
            .FilterIndex = 1,
            .RestoreDirectory = True,
            .Multiselect = True
        }

            If ofd.ShowDialog = DialogResult.OK Then
                If ofd.CheckFileExists Then

                    Dim dirpathname = PickGroup()
                    If dirpathname.Length = 0 Then
                        TextPrint(My.Resources.Aborted_word)
                        Return
                    End If

                    If dirpathname = "! Add New Name" Then
                        dirpathname = InputBox(My.Resources.Enter_Dos_Name)
                    End If
                    If dirpathname.Length = 0 Then
                        Return
                    End If

                    For Each ofdFilename As String In ofd.FileNames

                        Dim noquotes = New Regex("'")
                        dirpathname = noquotes.Replace(dirpathname, "")

                        Dim extension As String = Path.GetExtension(ofdFilename)
                        extension = Mid(extension, 2, 5)
                        If extension.ToUpper(Globalization.CultureInfo.InvariantCulture) = "INI" Then

                            Dim filename = GetRegionsName(ofdFilename)
                            Dim RegionUUID As String = FindRegionByName(filename)

                            If RegionUUID.Length > 0 Then
                                MsgBox(My.Resources.Region_Already_Exists, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Info_word)
                                Return
                            End If

                            If dirpathname.Length = 0 Then dirpathname = filename

                            Dim NewFilepath = Settings.OpensimBinPath & "Regions\" + dirpathname + "\Region\"
                            If Not Directory.Exists(NewFilepath) Then
                                MakeFolder(Settings.OpensimBinPath & "Regions\" + dirpathname + "\Region")
                            End If
                            File.Copy(ofdFilename, Settings.OpensimBinPath & "Regions\" + dirpathname + "\Region\" + filename + ".ini")
                        Else
                            TextPrint(My.Resources.Unrecognized & " " & extension & ". ")
                        End If
                    Next

                    PropChangedRegionSettings = True
                    LoadMyListView()
                End If
            End If
        End Using

    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles RestartButton.Click

        FormSetup.RestartAllRegions()

    End Sub

    ' ColumnClick event handler.
    Private Sub ColumnClick(ByVal o As Object, ByVal e As ColumnClickEventArgs)

        '// Determine if clicked column Is already the column that is being sorted.
        If e.Column = _SortColumn Then
            ' // Reverse the current sort direction for this column.
            If (_order = SortOrder.Ascending) Then
                _order = SortOrder.Descending
            Else
                _order = SortOrder.Ascending
            End If
        Else
            _order = SortOrder.Ascending
        End If

        If TheView1 = ViewType.Details Then
            ListView1.ListViewItemSorter = New ListViewColumnSorter(e.Column, _order)
            _SortColumn = e.Column

            ListView1.Sort()
            ListView1.ResumeLayout()
        End If

        If TheView1 = ViewType.Users Then
            UserView.ListViewItemSorter = New ListViewColumnSorter(e.Column, _order)
            _SortColumn = e.Column

            UserView.Sort()
            UserView.ResumeLayout()
        End If

        If TheView1 = ViewType.Avatars Then
            UserView.ListViewItemSorter = New ListViewColumnSorter(e.Column, _order)
            _SortColumn = e.Column

            AvatarView.Sort()
            UserView.ResumeLayout()
        End If

    End Sub

    Private Sub ExportButton_Click(sender As Object, e As EventArgs) Handles ExportButton.Click

        Dim BaseFolder As String
        Dim f = Settings.BackupFolder.Replace("/", "\")
        'Create an instance of the open file dialog box.
        Using openFileDialog1 = New FolderBrowserDialog With {
            .ShowNewFolderButton = True,
            .Description = Global.Outworldz.My.Resources.Choose_folder,
            .SelectedPath = f
        }
            Dim UserClickedOK As DialogResult = openFileDialog1.ShowDialog
            ' Process input if the user clicked OK.
            If UserClickedOK = DialogResult.OK Then
                Dim thing = openFileDialog1.SelectedPath
                If thing.Length > 0 Then
                    BaseFolder = thing
                    Using sourceTable As New DataTable()

                        sourceTable.Locale = Globalization.CultureInfo.CurrentCulture

                        sourceTable.Columns.AddRange(New DataColumn() {
                    New DataColumn("Name", GetType(String)),
                    New DataColumn("Dos Box", GetType(String)),
                    New DataColumn("Agents", GetType(Integer)),
                    New DataColumn("Status", GetType(String)),
                    New DataColumn("RAM", GetType(String)),
                    New DataColumn("CPU %", GetType(String)),
                    New DataColumn("X", GetType(String)),
                    New DataColumn("Y", GetType(String)),
                    New DataColumn("Size", GetType(String)),
                    New DataColumn("Estate", GetType(String)),
                    New DataColumn("Prims", GetType(String)),
                    New DataColumn("Region Port", GetType(String)),
                    New DataColumn("Group Port", GetType(String)),
                    New DataColumn("Scripts", GetType(String)),
                    New DataColumn("Maps", GetType(String)),
                    New DataColumn("Physics", GetType(String)),
                    New DataColumn("Birds", GetType(String)),
                    New DataColumn("Tides", GetType(String)),
                    New DataColumn("Teleport", GetType(String)),
                    New DataColumn("Smart Start", GetType(String)),
                    New DataColumn("Level Gods", GetType(String)),
                    New DataColumn("Owner Gods", GetType(String)),
                    New DataColumn("Manager Gods", GetType(String)),
                    New DataColumn("OAR Backup", GetType(String)),
                    New DataColumn("Publicity", GetType(String)),
                    New DataColumn("Frame Rate", GetType(String)),
                    New DataColumn("Script Rate", GetType(String)),
                    New DataColumn("Boot Time", GetType(String)),
                    New DataColumn("Map Boot Time", GetType(String))
                })

                        For Each i In ListView1.Items
                            sourceTable.Rows.Add(i.SubItems(0).Text.Trim,
                                         i.SubItems(1).Text.Trim,
                                         i.SubItems(2).Text.Trim,
                                         i.SubItems(3).Text.Trim,
                                         i.SubItems(4).Text.Trim,
                                         i.SubItems(5).Text.Trim,
                                         i.SubItems(6).Text.Trim,
                                         i.SubItems(7).Text.Trim,
                                         i.SubItems(8).Text.Trim,
                                         i.SubItems(9).Text.Trim,
                                         i.SubItems(10).Text.Trim,
                                         i.SubItems(11).Text.Trim,
                                         i.SubItems(12).Text.Trim,
                                         i.SubItems(13).Text.Trim,
                                         i.SubItems(14).Text.Trim,
                                         i.SubItems(15).Text.Trim,
                                         i.SubItems(16).Text.Trim,
                                         i.SubItems(17).Text.Trim,
                                         i.SubItems(18).Text.Trim,
                                         i.SubItems(19).Text.Trim,
                                         i.SubItems(20).Text.Trim,
                                         i.SubItems(21).Text.Trim,
                                         i.SubItems(22).Text.Trim,
                                         i.SubItems(23).Text.Trim,
                                         i.SubItems(24).Text.Trim,
                                         i.SubItems(25).Text.Trim,
                                         i.SubItems(26).Text.Trim,
                                         i.SubItems(27).Text.Trim,
                                         i.SubItems(28).Text.Trim)
                        Next

                        DeleteFile(IO.Path.Combine($"{BaseFolder}, RegionList.csv"))
                        Sleep(100)
                        Try
                            Using writer = New StreamWriter(IO.Path.Combine(BaseFolder, "RegionList.csv"))
                                WriteDataTable(sourceTable, writer, True)
                                writer.Flush()
                            End Using
                        Catch
                        End Try
                    End Using
                    MsgBox($"{My.Resources.Saved_Word} RegionList.csv ==> {BaseFolder}", MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground, "RegionList.csv " + My.Resources.Saved_Word)

                    Dim response = MsgBox($"{My.Resources.Open_word} RegionList.csv?", MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground, "RegionList.csv")
                    If response = vbYes Then
                        Try
                            System.Diagnostics.Process.Start(IO.Path.Combine(BaseFolder, "RegionList.csv"))
                        Catch ex As Exception
                            BreakPoint.Dump(ex)
                        End Try
                    End If

                End If
            End If
        End Using

    End Sub

    Private Sub FloatToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FloatToolStripMenuItem.Click

        FloatToolStripMenuItem.Checked = True
        OnTopToolStripMenuItem.Checked = False
        Me.TopMost = False
        Settings.KeepOnTop = False
        Settings.SaveSettings()

    End Sub

    Private Sub Form_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Settings.RegionListVisible = False

        _ImageListSmall.Dispose()
        colsize.Dispose()

    End Sub

    Private Function GetStatus(RegionUUID As String, ByRef Num As Integer, ByRef Letter As String) As Integer

        Dim Status As Integer = RegionStatus(RegionUUID)

        ' Get Estate name (cached)
        Dim MyEstate = Estate(RegionUUID)
        If TheView1 = ViewType.Details Then
            If MyEstate.Length = 0 Then
                If UseMysql Then
                    MyEstate = EstateName(RegionUUID)
                    Estate(RegionUUID) = MyEstate
                End If
            End If
        End If

        If Not RegionEnabled(RegionUUID) Then
            Letter = "Disabled"
            If Region_Name(RegionUUID) = Settings.WelcomeRegion Then
                Num = DGICON.HomeOffline
            Else
                Num = DGICON.disabled
            End If
            '   ElseIf Estate.Length = 0 Then
            '      Letter = My.Resources.No_Estate_Word
            '     Num = DGICON.NoEstate
        ElseIf Status = SIMSTATUSENUM.Stopped And Smart_Start(RegionUUID) = "True" And Settings.Smart_Start And Settings.BootOrSuspend Then
            Letter = My.Resources.Waiting
            Num = DGICON.SmartStartStopped
        ElseIf Status = SIMSTATUSENUM.Stopped And Smart_Start(RegionUUID) = "True" And Settings.Smart_Start And Not Settings.BootOrSuspend Then
            Letter = My.Resources.Frozen
            Num = DGICON.icemelted
        ElseIf Status = SIMSTATUSENUM.Suspended And Smart_Start(RegionUUID) = "True" And Settings.Smart_Start And Not Settings.BootOrSuspend Then
            Letter = My.Resources.Frozen
            Num = DGICON.icecube
        ElseIf Status = SIMSTATUSENUM.Stopped And Smart_Start(RegionUUID) = "True" And Not Settings.Smart_Start Then
            Letter = My.Resources.Stopped_word
            Num = DGICON.stopped
        ElseIf Status = SIMSTATUSENUM.Stopped And Smart_Start(RegionUUID) <> "True" Then
            Letter = My.Resources.Stopped_word
            Num = DGICON.stopped
        ElseIf Status = SIMSTATUSENUM.Error Then
            Letter = My.Resources.Error_word
            Num = DGICON.ErrorIcon
        ElseIf Status = SIMSTATUSENUM.Suspended Then
            Letter = My.Resources.Suspended_word
            Num = DGICON.Suspended
        ElseIf Status = SIMSTATUSENUM.RecyclingDown Then
            Letter = My.Resources.Recycling_Down_word
            Num = DGICON.recyclingdown
        ElseIf Status = SIMSTATUSENUM.RecyclingUp Then
            Letter = My.Resources.Recycling_Up_word
            Num = DGICON.recyclingup
        ElseIf Status = SIMSTATUSENUM.RestartPending Then
            Letter = My.Resources.Restart_Pending_word
            Num = DGICON.recyclingup
        ElseIf Status = SIMSTATUSENUM.RetartingNow Then
            Letter = My.Resources.Restarting_Now_word
            Num = DGICON.recyclingup
        ElseIf Status = SIMSTATUSENUM.Resume Then
            Letter = "Restarting Now"
            Num = DGICON.recyclingup
        ElseIf Status = SIMSTATUSENUM.Booting Then
            Letter = My.Resources.Booting_word
            Num = DGICON.bootingup
        ElseIf Status = SIMSTATUSENUM.NoLogin Then
            Letter = My.Resources.NoLogin_word
            Num = DGICON.NoLogOn
        ElseIf Status = SIMSTATUSENUM.ShuttingDownForGood Then
            Letter = My.Resources.Quitting_word
            Num = DGICON.shuttingdown
        ElseIf Status = SIMSTATUSENUM.ShuttingDown Then
            Letter = My.Resources.Stopping_word
            Num = DGICON.shuttingdown
        ElseIf Status = SIMSTATUSENUM.RestartStage2 Then
            Letter = My.Resources.Pending_word
        ElseIf Status = SIMSTATUSENUM.NoError Then
            Letter = My.Resources.Stopped_word
            Num = DGICON.NoError
        ElseIf Status = SIMSTATUSENUM.Booted And AvatarCount(RegionUUID) = 1 Then
            Letter = My.Resources.Running_word
            Num = DGICON.user1
        ElseIf Status = SIMSTATUSENUM.Booted And AvatarCount(RegionUUID) > 1 Then
            Letter = CStr(AvatarCount(RegionUUID) & " " & My.Resources.Avatars_word)
            Num = DGICON.user2
        ElseIf Status = SIMSTATUSENUM.Booted Then
            If Region_Name(RegionUUID) = Settings.WelcomeRegion Then
                Num = DGICON.Home
                Letter = My.Resources.Home_word
            Else
                Letter = My.Resources.Running_word
                Num = DGICON.up
            End If
        Else
            Num = DGICON.warning ' warning
        End If

        Return Status

    End Function

    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.Click

        HelpManual("RegionList")

    End Sub

    Private Sub IconClick(sender As Object, e As EventArgs) Handles UserView.Click
        If Not initted Then Return
        Dim User As ListView.SelectedListViewItemCollection = Me.UserView.SelectedItems
        Dim item As ListViewItem
        For Each item In User
            Dim Username = item.SubItems(0).Text.Trim
            Dim UUID = item.SubItems(7).Text.Trim
            If Username.Length > 0 Then
#Disable Warning CA2000
                Dim UserData As New FormEditUser
#Enable Warning CA2000
                UserData.init(UUID)
                UserData.BringToFront()
                UserData.Activate()
                UserData.Visible = True
                UserData.Select()
            End If
        Next

    End Sub

    Private Sub IconViewClick(sender As Object, e As EventArgs) Handles IconView.Click

        Dim regions As ListView.SelectedListViewItemCollection = Me.IconView.SelectedItems
        Dim item As ListViewItem
        For Each item In regions
            Dim RegionName = item.SubItems(0).Text.Trim
            Dim RegionUUID As String = FindRegionByName(RegionName)
            If RegionUUID.Length > 0 Then
                StartStopEdit(RegionUUID, RegionName)
            End If
        Next

    End Sub

    Private Sub ListClick(sender As Object, e As EventArgs) Handles ListView1.Click

        Dim regions As ListView.SelectedListViewItemCollection = Me.ListView1.SelectedItems
        Dim item As ListViewItem
        For Each item In regions
            Dim RegionName = item.SubItems(0).Text.Trim
            Dim RegionUUID As String = FindRegionByName(RegionName)
            If RegionUUID.Length > 0 Then
                StartStopEdit(RegionUUID, RegionName)
            End If
        Next

    End Sub

    Private Sub ListView1_ColumnWidthChanged(sender As Object, e As ColumnWidthChangedEventArgs) Handles ListView1.ColumnWidthChanged

        Dim w = ListView1.Columns(e.ColumnIndex).Width
        Dim name = ListView1.Columns(e.ColumnIndex).Name
        If name.Length = 0 Or w = 0 Then Return

        ScreenPosition.PutSize(name, w)
        ScreenPosition.SaveFormSettings()

    End Sub

    Private Sub ListView1_ItemCheck1(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles ListView1.ItemCheck

        If Not detailsinitted Then Return

        Dim Item As ListViewItem = Nothing

        Try
            Item = ListView1.Items.Item(e.Index)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try
        If Item.Text.Length = 0 Then Return
        If Item.Text = "New Region" Then Return

        Dim UUID As String = FindRegionByName(Item.Text)
        Dim out As New Guid
        If Not Guid.TryParse(UUID, out) Then Return
        Dim GroupName = Group_Name(UUID)

        For Each RegionUUID In RegionUuidListByName(GroupName)

            If (e.NewValue = CheckState.Unchecked) Then
                RegionEnabled(RegionUUID) = False
            Else
                RegionEnabled(RegionUUID) = True
            End If

            If RegionIniFilePath(RegionUUID).Length > 0 Then
                Dim INI = New LoadIni(RegionIniFilePath(RegionUUID), ";", System.Text.Encoding.UTF8)
                INI.SetIni(Region_Name(RegionUUID), "Enabled", CStr(RegionEnabled(RegionUUID)))
                INI.SaveINI()
            Else
                BreakPoint.Print("cannot locate region in group " & GroupName)
            End If

        Next

    End Sub

    Private Sub LoadForm(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' Set the view to show whatever
        TheView1 = Settings.RegionListView()

        SetScreen(TheView1)

        Try
            AllButton.Text = Global.Outworldz.My.Resources.All_word
            AddRegionButton.Text = Global.Outworldz.My.Resources.Add_word
            AllNone.Text = Global.Outworldz.My.Resources.AllNone_word
            AvatarsButton.Text = Global.Outworldz.My.Resources.Avatars_word
            Bootedbutton.Text = Global.Outworldz.My.Resources.Running_word
            DetailsButton.Text = Global.Outworldz.My.Resources.Details_word
            ExportButton.Text = Global.Outworldz.My.Resources.Export_word
            FloatToolStripMenuItem.Text = Global.Outworldz.My.Resources.Float
            HelpToolStripMenuItem.Image = Global.Outworldz.My.Resources.question_and_answer
            HelpToolStripMenuItem.Text = Global.Outworldz.My.Resources.Help_word
            IconsButton.Text = Global.Outworldz.My.Resources.Icons_word
            ImportButton.Text = Global.Outworldz.My.Resources.Import_word
            KOT.Text = Global.Outworldz.My.Resources.Window_Word
            OnButton.Text = Global.Outworldz.My.Resources.On_word
            OffButton.Text = Global.Outworldz.My.Resources.Off
            OnTopToolStripMenuItem.Text = Global.Outworldz.My.Resources.On_Top
            RefreshButton.Text = Global.Outworldz.My.Resources.Refresh_word
            RestartButton.Text = Global.Outworldz.My.Resources.Restart_word
            RunAllButton.Text = Global.Outworldz.My.Resources.Run_All_word
            SearchBox.Text = Global.Outworldz.My.Resources.Search_word
            StopAllButton.Text = Global.Outworldz.My.Resources.Stop_All_word
            StoppedButton.Text = Global.Outworldz.My.Resources.Stopped_word
            SmartButton.Text = Global.Outworldz.My.Resources.Smart_Start_word
            Users.Text = Global.Outworldz.My.Resources.Users_word
            Emails.Text = Global.Outworldz.My.Resources.Email_word

            IconView.SmallImageList = ImageListSmall
            ImageListSmall.ImageSize = New Drawing.Size(16, 16)
            ListView1.SmallImageList = ImageListSmall

            ToolTip1.SetToolTip(AddRegionButton, Global.Outworldz.My.Resources.Add_Region_word)
            ToolTip1.SetToolTip(AllNone, Global.Outworldz.My.Resources.Selectallnone)
            ToolTip1.SetToolTip(AvatarsButton, Global.Outworldz.My.Resources.ListAvatars)
            ToolTip1.SetToolTip(DetailsButton, Global.Outworldz.My.Resources.View_Details)
            ToolTip1.SetToolTip(ExportButton, Global.Outworldz.My.Resources.Export_list)
            ToolTip1.SetToolTip(IconsButton, Global.Outworldz.My.Resources.View_as_Icons)
            ToolTip1.SetToolTip(ImportButton, Global.Outworldz.My.Resources.Importtext)
            ToolTip1.SetToolTip(ListView1, Global.Outworldz.My.Resources.ClickStartStoptxt)
            ToolTip1.SetToolTip(RefreshButton, Global.Outworldz.My.Resources.Reload)
            ToolTip1.SetToolTip(RestartButton, Global.Outworldz.My.Resources.Restart_All_Checked)
            ToolTip1.SetToolTip(RunAllButton, Global.Outworldz.My.Resources.StartAll)
            ToolTip1.SetToolTip(StopAllButton, Global.Outworldz.My.Resources.Stopsall)
            ToolTip1.ToolTipTitle = Global.Outworldz.My.Resources.Row_note

            UseMysql = False
            If MysqlInterface.IsMySqlRunning() Then
                UseMysql = True
            End If

            AllNone.Checked = False
            AllNone.Visible = False

            AllButton.Checked = True

            DoubleBuff(ListView1, True)
            DoubleBuff(IconView, True)
            DoubleBuff(UserView, True)

            Settings.RegionListVisible = True

            Me.Name = "Region List"
            Me.Text = Global.Outworldz.My.Resources.Region_List

            AvatarView.CheckBoxes = False
            AvatarView.TabIndex = 0
            AvatarView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.None)
            AvatarView.GridLines = False
            AvatarView.ShowItemToolTips = True

            ListView1.Visible = False
            ListView1.LabelWrap = True
            ListView1.AutoArrange = True
            ListView1.TabIndex = 0
            ListView1.CheckBoxes = True
            ListView1.View = View.Details
            ListView1.LabelEdit = True
            ListView1.AllowColumnReorder = True
            ListView1.FullRowSelect = False
            ListView1.GridLines = True
            ListView1.AllowColumnReorder = True
            ListView1.Sorting = SortOrder.Ascending
            ListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.None)
            ListView1.ShowItemToolTips = True

            IconView.TabIndex = 0
            IconView.View = View.SmallIcon
            IconView.CheckBoxes = False
            IconView.FullRowSelect = False
            IconView.GridLines = True
            IconView.AllowColumnReorder = False
            IconView.Sorting = SortOrder.Ascending
            IconView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.None)
            IconView.ShowItemToolTips = True

            UserView.TabIndex = 0
            UserView.View = View.Details
            UserView.CheckBoxes = True
            UserView.FullRowSelect = True
            UserView.AllowColumnReorder = True
            UserView.GridLines = True
            UserView.AllowColumnReorder = True
            UserView.Sorting = SortOrder.Ascending
            UserView.ShowItemToolTips = True

            If Settings.KeepOnTop Then
                Me.TopMost = True
                OnTopToolStripMenuItem.Checked = True
                FloatToolStripMenuItem.Checked = False
            Else
                Me.TopMost = False
                OnTopToolStripMenuItem.Checked = False
                FloatToolStripMenuItem.Checked = True
            End If

            Dim ctr = 0

            'icons
            IconView.Columns.Add(New ColumnHeader)
            IconView.Columns.Add(My.Resources.Enable_word, colsize.ColumnWidth("Icon" & ctr & "_" & CStr(ViewType.Icons), 400), HorizontalAlignment.Left)
            IconView.Columns(ctr).Name = "Icon" & ctr & "_" & CStr(ViewType.Icons)

            'details
            ListView1.Columns.Add(My.Resources.Enable_word, colsize.ColumnWidth("Column" & ctr & "_" & CStr(ViewType.Details), 120), HorizontalAlignment.Left)
            ListView1.Columns(ctr).Name = "Column" & ctr & "_" & CStr(ViewType.Details)
            ctr += 1
            ListView1.Columns.Add(My.Resources.DOS_Box_word, colsize.ColumnWidth("Column" & ctr & "_" & CStr(ViewType.Details), 120), HorizontalAlignment.Left)
            ListView1.Columns(ctr).Name = "Column" & ctr & "_" & CStr(ViewType.Details)
            ctr += 1
            ListView1.Columns.Add(My.Resources.Agents_word, colsize.ColumnWidth("Column" & ctr & "_" & CStr(ViewType.Details), 50), HorizontalAlignment.Center)
            ListView1.Columns(ctr).Name = "Column" & ctr & "_" & CStr(ViewType.Details)
            ctr += 1
            ListView1.Columns.Add(My.Resources.Status_word, colsize.ColumnWidth("Column" & ctr & "_" & CStr(ViewType.Details), 120), HorizontalAlignment.Center)
            ListView1.Columns(ctr).Name = "Column" & ctr & "_" & CStr(ViewType.Details)
            ctr += 1
            ListView1.Columns.Add(My.Resources.RAM_Word, colsize.ColumnWidth("Column" & ctr & "_" & CStr(ViewType.Details), 80), HorizontalAlignment.Center)
            ListView1.Columns(ctr).Name = "Column" & ctr & "_" & CStr(ViewType.Details)
            ctr += 1
            ListView1.Columns.Add(My.Resources.CPU_word, colsize.ColumnWidth("Column" & ctr & "_" & CStr(ViewType.Details), 60), HorizontalAlignment.Center)
            ListView1.Columns(ctr).Name = "Column" & ctr & "_" & CStr(ViewType.Details)
            ctr += 1
            ListView1.Columns.Add("X".ToUpperInvariant, colsize.ColumnWidth("Column" & ctr & "_" & CStr(ViewType.Details), 50), HorizontalAlignment.Center)
            ListView1.Columns(ctr).Name = "Column" & ctr & "_" & CStr(ViewType.Details)
            ctr += 1
            ListView1.Columns.Add("Y".ToUpperInvariant, colsize.ColumnWidth("Column" & ctr & "_" & CStr(ViewType.Details), 50), HorizontalAlignment.Center)
            ListView1.Columns(ctr).Name = "Column" & ctr & "_" & CStr(ViewType.Details)
            ctr += 1
            ListView1.Columns.Add(My.Resources.Size_word, colsize.ColumnWidth("Column" & ctr & "_" & CStr(ViewType.Details), 40), HorizontalAlignment.Center)
            ListView1.Columns(ctr).Name = "Column" & ctr & "_" & CStr(ViewType.Details)
            ctr += 1
            ListView1.Columns.Add(My.Resources.Estate_word, colsize.ColumnWidth("Column" & ctr & "_" & CStr(ViewType.Details), 100), HorizontalAlignment.Left)
            ListView1.Columns(ctr).Name = "Column" & ctr & "_" & CStr(ViewType.Details)
            ctr += 1
            ListView1.Columns.Add("Prims", colsize.ColumnWidth("Column" & ctr & "_" & CStr(ViewType.Details), 80), HorizontalAlignment.Left)
            ListView1.Columns(ctr).Name = "Column" & ctr & "_" & CStr(ViewType.Details)
            ctr += 1
            ListView1.Columns.Add(My.Resources.Region_Ports_word, colsize.ColumnWidth("Column" & ctr & "_" & CStr(ViewType.Details), 50), HorizontalAlignment.Center)
            ListView1.Columns(ctr).Name = "Column" & ctr & "_" & CStr(ViewType.Details)
            ctr += 1
            ListView1.Columns.Add(My.Resources.Group_Ports_word, colsize.ColumnWidth("Column" & ctr & "_" & CStr(ViewType.Details), 50), HorizontalAlignment.Center)
            ListView1.Columns(ctr).Name = "Column" & ctr & "_" & CStr(ViewType.Details)
            ctr += 1
            ' optional
            ListView1.Columns.Add(My.Resources.Scripts_word, colsize.ColumnWidth("Column" & ctr & "_" & CStr(ViewType.Details), 80), HorizontalAlignment.Center)
            ListView1.Columns(ctr).Name = "Column" & ctr & "_" & CStr(ViewType.Details)
            ctr += 1
            ListView1.Columns.Add(My.Resources.Maps_word, colsize.ColumnWidth("Column" & ctr & "_" & CStr(ViewType.Details), 80), HorizontalAlignment.Center)
            ListView1.Columns(ctr).Name = "Column" & ctr & "_" & CStr(ViewType.Details)
            ctr += 1
            ListView1.Columns.Add(My.Resources.Physics_word, colsize.ColumnWidth("Column" & ctr & "_" & CStr(ViewType.Details), 120), HorizontalAlignment.Center)
            ListView1.Columns(ctr).Name = "Column" & ctr & "_" & CStr(ViewType.Details)
            ctr += 1
            ListView1.Columns.Add(My.Resources.Birds_word, colsize.ColumnWidth("Column" & ctr & "_" & CStr(ViewType.Details) & "_" & CStr(TheView), 60), HorizontalAlignment.Center)
            ListView1.Columns(ctr).Name = "Column" & ctr & "_" & CStr(ViewType.Details)
            ctr += 1
            ListView1.Columns.Add(My.Resources.Tides_word, colsize.ColumnWidth("Column" & ctr & "_" & CStr(ViewType.Details), 60), HorizontalAlignment.Center)
            ListView1.Columns(ctr).Name = "Column" & ctr & "_" & CStr(ViewType.Details)
            ctr += 1
            ListView1.Columns.Add(My.Resources.Teleport_word, colsize.ColumnWidth("Column" & ctr, 65), HorizontalAlignment.Center)
            ListView1.Columns(ctr).Name = "Column" & ctr & "_" & CStr(ViewType.Details)
            ctr += 1
            ListView1.Columns.Add(My.Resources.Smart_Start_word, colsize.ColumnWidth("Column" & ctr & "_" & CStr(ViewType.Details), 80), HorizontalAlignment.Center)
            ListView1.Columns(ctr).Name = "Column" & ctr & "_" & CStr(ViewType.Details)
            ctr += 1
            ListView1.Columns.Add(My.Resources.Allow_Or_Disallow_Gods_word, colsize.ColumnWidth("Column" & ctr & "_" & CStr(ViewType.Details), 75), HorizontalAlignment.Center)
            ListView1.Columns(ctr).Name = "Column" & ctr & "_" & CStr(ViewType.Details)
            ctr += 1
            ListView1.Columns.Add(My.Resources.Owner_God, colsize.ColumnWidth("Column" & ctr & "_" & CStr(ViewType.Details), 75), HorizontalAlignment.Center)
            ListView1.Columns(ctr).Name = "Column" & ctr & "_" & CStr(ViewType.Details)
            ctr += 1
            ListView1.Columns.Add(My.Resources.Manager_God_word, colsize.ColumnWidth("Column" & ctr & "_" & CStr(ViewType.Details), 80), HorizontalAlignment.Center)
            ListView1.Columns(ctr).Name = "Column" & ctr & "_" & CStr(ViewType.Details)
            ctr += 1
            ListView1.Columns.Add(My.Resources.No_Autobackup, colsize.ColumnWidth("Column" & ctr & "_" & CStr(ViewType.Details), 90), HorizontalAlignment.Center)
            ListView1.Columns(ctr).Name = "Column" & ctr & "_" & CStr(ViewType.Details)
            ctr += 1
            ListView1.Columns.Add(My.Resources.Publicity_Word, colsize.ColumnWidth("Column" & ctr & "_" & CStr(ViewType.Details), 80), HorizontalAlignment.Center)
            ListView1.Columns(ctr).Name = "Column" & ctr & "_" & CStr(ViewType.Details)
            ctr += 1
            ListView1.Columns.Add(My.Resources.Script_Rate_word, colsize.ColumnWidth("Column" & ctr & "_" & CStr(ViewType.Details), 80), HorizontalAlignment.Center)
            ListView1.Columns(ctr).Name = "Column" & ctr & "_" & CStr(ViewType.Details)
            ctr += 1
            ListView1.Columns.Add(My.Resources.Frame_Rate_word, colsize.ColumnWidth("Column" & ctr & "_" & CStr(ViewType.Details), 80), HorizontalAlignment.Center)
            ListView1.Columns(ctr).Name = "Column" & ctr & "_" & CStr(ViewType.Details)
            ' Timers
            ctr += 1
            ListView1.Columns.Add(My.Resources.Boot_Time, colsize.ColumnWidth("Column" & ctr & "_" & CStr(ViewType.Details), 150), HorizontalAlignment.Center)
            ListView1.Columns(ctr).Name = "Column" & ctr & "_" & CStr(ViewType.Details)
            ctr += 1
            ListView1.Columns.Add(My.Resources.Map_Time, colsize.ColumnWidth("Column" & ctr & "_" & CStr(ViewType.Details), 150), HorizontalAlignment.Center)
            ListView1.Columns(ctr).Name = "Column" & ctr & "_" & CStr(ViewType.Details)

            'Avatars
            ctr = 0
            AvatarView.Columns.Add(My.Resources.Agents_word, colsize.ColumnWidth("Avatar" & ctr & "_" & CStr(ViewType.Details), 150), HorizontalAlignment.Center)
            AvatarView.Columns(ctr).Name = "Avatars" & ctr & "_" & CStr(ViewType.Avatars)
            ctr += 1
            AvatarView.Columns.Add(My.Resources.Region_word, colsize.ColumnWidth("Avatar" & ctr & "_" & CStr(ViewType.Details), 150), HorizontalAlignment.Center)
            AvatarView.Columns(ctr).Name = "Avatars" & ctr & "_" & CStr(ViewType.Avatars)
            ctr += 1
            AvatarView.Columns.Add(My.Resources.Click_Avatar_to_Teleport, colsize.ColumnWidth("Avatar" & ctr & "_" & CStr(ViewType.Details), 150), HorizontalAlignment.Left)
            AvatarView.Columns(ctr).Name = "Avatars" & ctr & "_" & CStr(ViewType.Avatars)

            'Users
            ctr = 0
            UserView.Columns.Add(My.Resources.Avatar_Name_word, colsize.ColumnWidth("User" & ctr & "_" & CStr(ViewType.Users), 250), HorizontalAlignment.Left)
            UserView.Columns(ctr).Name = "User" & ctr & "_" & CStr(ViewType.Users)
            ctr += 1
            UserView.Columns.Add(My.Resources.Email_word, colsize.ColumnWidth("Email" & ctr & "_" & CStr(ViewType.Users), 250), HorizontalAlignment.Left)
            UserView.Columns(ctr).Name = "User" & ctr & "_" & CStr(ViewType.Users)
            ctr += 1
            UserView.Columns.Add(My.Resources.Title_word, colsize.ColumnWidth("Title" & ctr & "_" & CStr(ViewType.Users), 90), HorizontalAlignment.Left)
            UserView.Columns(ctr).Name = "User" & ctr & "_" & CStr(ViewType.Users)
            ctr += 1
            UserView.Columns.Add(My.Resources.Items_word, colsize.ColumnWidth("Items" & ctr & "_" & CStr(ViewType.Users), 90), HorizontalAlignment.Left)
            UserView.Columns(ctr).Name = "User" & ctr & "_" & CStr(ViewType.Users)
            ctr += 1
            UserView.Columns.Add(My.Resources.Level_word, colsize.ColumnWidth("Level" & ctr & "_" & CStr(ViewType.Users), 90), HorizontalAlignment.Left)
            UserView.Columns(ctr).Name = "User" & ctr & "_" & CStr(ViewType.Users)
            ctr += 1
            UserView.Columns.Add(My.Resources.Birthday_word, colsize.ColumnWidth("Birthday" & ctr & "_" & CStr(ViewType.Users), 120), HorizontalAlignment.Left)
            UserView.Columns(ctr).Name = "User" & ctr & "_" & CStr(ViewType.Users)
            ctr += 1
            UserView.Columns.Add(My.Resources.Age, colsize.ColumnWidth("Age" & ctr & "_" & CStr(ViewType.Users), 90), HorizontalAlignment.Left)
            UserView.Columns(ctr).Name = "User" & ctr & "_" & CStr(ViewType.Users)
            ctr += 1
            UserView.Columns.Add(My.Resources.UUID, colsize.ColumnWidth("UUID" & ctr & "_" & CStr(ViewType.Users), 250), HorizontalAlignment.Left)
            UserView.Columns(ctr).Name = "UserUUID" & ctr & "_" & CStr(ViewType.Users)

            ' Connect the ListView.ColumnClick event to the ColumnClick event handler.
            AddHandler ListView1.ColumnClick, AddressOf ColumnClick
            AddHandler IconView.ColumnClick, AddressOf ColumnClick
            AddHandler UserView.ColumnClick, AddressOf ColumnClick

            ' index to display icons
            ImageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("navigate_up2", Globalization.CultureInfo.InvariantCulture))   ' 0 booting up
            ImageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("navigate_down2", Globalization.CultureInfo.InvariantCulture)) ' 1 shutting down
            ImageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("check2", Globalization.CultureInfo.InvariantCulture)) ' 2 okay, up
            ImageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("navigate_cross", Globalization.CultureInfo.InvariantCulture)) ' 3 disabled
            ImageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("cube_green", Globalization.CultureInfo.InvariantCulture))  ' 4 enabled, stopped
            ImageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("navigate_down", Globalization.CultureInfo.InvariantCulture))  ' 5 Recycling down
            ImageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("navigate_up", Globalization.CultureInfo.InvariantCulture))  ' 6 Recycling Up
            ImageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("warning", Globalization.CultureInfo.InvariantCulture))  ' 7 Unknown
            ImageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("user2", Globalization.CultureInfo.InvariantCulture))  ' 8 - 1 User
            ImageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("users1", Globalization.CultureInfo.InvariantCulture))  ' 9 - 2 user
            ImageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("media_pause", Globalization.CultureInfo.InvariantCulture))  ' 10 - 2 user
            ImageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("home", Globalization.CultureInfo.InvariantCulture))  '  11- home
            ImageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("home_02", Globalization.CultureInfo.InvariantCulture))  '  12- home _offline
            ImageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("refresh", Globalization.CultureInfo.InvariantCulture))  '  13- Pending
            ImageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("media_pause", Globalization.CultureInfo.InvariantCulture))  '  14- Suspended
            ImageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("package_error", Globalization.CultureInfo.InvariantCulture))  '  15- Error
            ImageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("gear_stop", Globalization.CultureInfo.InvariantCulture))  '  16 - NoLogon
            ImageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("redo", Globalization.CultureInfo.InvariantCulture))  '  17 - NOError
            ImageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("navigate_minus", Globalization.CultureInfo.InvariantCulture))  '  17 - NoEstate
            ImageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("Icecastpic", Globalization.CultureInfo.InvariantCulture))  '  18 - icecube
            ImageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("Icemelted", Globalization.CultureInfo.InvariantCulture))  '  19 - icecube

            If TheView1 = ViewType.Details Or TheView1 = ViewType.Icons Then
                Timer1.Interval = 1000 ' check for Form1.PropUpdateView immediately
                Timer1.Start() 'Timer starts functioning
            End If

            Timer1.Start()
            LoadMyListView()

            initted = True
        Catch
        End Try

    End Sub

    Private Sub LoadMyListView()

        If SearchBusy = True Then Return
        SearchBusy = True
        BringToFront()

        SearchArray.Clear()

        Select Case TheView1
            Case ViewType.Avatars

                ShowAvatars()

            Case ViewType.Users

                ShowUsers()

            Case ViewType.Details

                Dim L = RegionUuids()
                L.Sort()

                For Each RegionUUID In L
                    If SearchBox.Text.Length > 0 And SearchBox.Text <> My.Resources.Search_word Then
                        If Region_Name(RegionUUID).ToUpper(Globalization.CultureInfo.InvariantCulture).Contains(SearchBox.Text.ToUpper(Globalization.CultureInfo.InvariantCulture)) Then
                            SearchArray.Add(RegionUUID)
                        End If
                    Else
                        SearchArray.Add(RegionUUID)
                    End If
                Next

                ShowDetails()

            Case ViewType.Icons

                Dim L = RegionUuids()
                L.Sort()

                For Each RegionUUID In L
                    If SearchBox.Text.Length > 0 And SearchBox.Text <> My.Resources.Search_word Then
                        If Region_Name(RegionUUID).ToUpper(Globalization.CultureInfo.InvariantCulture).Contains(SearchBox.Text.ToUpper(Globalization.CultureInfo.InvariantCulture)) Then
                            SearchArray.Add(RegionUUID)
                        End If
                    Else
                        SearchArray.Add(RegionUUID)
                    End If
                Next

                ShowIcons()

        End Select

        SearchBusy = False

    End Sub

    Private Sub MyListView_AfterLabelEdit(sender As Object, e As System.Windows.Forms.LabelEditEventArgs) Handles ListView1.AfterLabelEdit

        Debug.Print(e.Label)

    End Sub

    Private Sub OffButton_CheckedChanged(sender As Object, e As EventArgs) Handles OffButton.CheckedChanged

        If Not initted Then Return
        LoadMyListView()

    End Sub

    Private Sub OnButton_CheckedChanged(sender As Object, e As EventArgs) Handles OnButton.CheckedChanged

        If Not initted Then Return
        LoadMyListView()

    End Sub

    Private Sub OnTopToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OnTopToolStripMenuItem.Click

        OnTopToolStripMenuItem.Checked = True
        FloatToolStripMenuItem.Checked = False
        Me.TopMost = True
        Settings.KeepOnTop = True
        Settings.SaveSettings()

    End Sub

    'The following detects  the location of the form in screen coordinates
    Private Sub Resize_page(ByVal sender As Object, ByVal e As System.EventArgs)

        ScreenPosition.SaveXY(Me.Left, Me.Top)
        ScreenPosition.SaveHW(Me.Height, Me.Width)

    End Sub

    Private Sub RunAllButton_Click(sender As Object, e As EventArgs) Handles RunAllButton.Click

        FormSetup.StartOpensimulator()

    End Sub

    Private Sub SearchBox_TextChanged(sender As Object, e As EventArgs) Handles SearchBox.Click
        If Not initted Then Return
        If SearchBox.Text = My.Resources.Search_word Then
            SearchBox.Text = ""
        End If
    End Sub

    Private Sub SetScreen(View As Integer)

        Try
            ScreenPosition = New ClassScreenpos(MyBase.Name & "_View_" & CStr(View))
            AddHandler ResizeEnd, Handler
            Dim xy As List(Of Integer) = ScreenPosition.GetXY()
            Me.Left = xy.Item(0)
            Me.Top = xy.Item(1)
            Dim hw As List(Of Integer) = ScreenPosition.GetHW()
            '1106, 460
            If hw.Item(0) = 0 Then
                Me.Height = 460
            Else
                Me.Height = hw.Item(0)
            End If
            If hw.Item(1) = 0 Then
                Me.Width = 1800
            Else
                Me.Width = hw.Item(1)
            End If
        Catch ex As Exception
            BreakPoint.Dump(ex)
            Me.Height = 460
            Me.Width = 1800
            Me.Left = 100
            Me.Top = 100
        End Try

    End Sub

#End Region

#Region "Layout"

    Private Sub ShowAvatars()
        Try
            Me.Text = ""
            AllNone.Visible = False

            AvatarView.Show()
            AvatarView.Visible = True
            ListView1.Visible = False
            UserView.Visible = False
            IconView.Visible = False
            ListView1.Hide()
            UserView.Hide()
            IconView.Hide()

            AvatarView.BeginUpdate()
            AvatarView.Items.Clear()

            Dim Index = 0

            ' Create items and sub items for each item.
            Dim ListOfAgents As New Dictionary(Of String, String)

            If MysqlInterface.IsMySqlRunning() Then
                ListOfAgents = GetAllAgents()
            End If

            For Each Agent In ListOfAgents
                If Region_Name(Agent.Value).Contains(SearchBox.Text) Or SearchBox.Text.Length = 0 Or SearchBox.Text = My.Resources.Search_word Then
                    Dim item1 As New ListViewItem(Agent.Key, Index)
                    item1.SubItems.Add(Region_Name(Agent.Value))
                    item1.SubItems.Add("-".ToUpperInvariant)
                    AvatarView.Items.AddRange(New ListViewItem() {item1})
                    Index += 1
                End If

            Next

            If ListOfAgents.Count = 0 Then
                Dim item1 As New ListViewItem(My.Resources.No_Avatars, Index)
                item1.SubItems.Add("-".ToUpperInvariant)
                item1.SubItems.Add("-".ToUpperInvariant)
                AvatarView.Items.AddRange(New ListViewItem() {item1})
                Index += 1
            End If

            For Each col In AvatarView.Columns
                Using csize As New ClassScreenpos(MyBase.Name & "ColumnSize")
                    Dim w = csize.ColumnWidth(CStr(col.name))
                    If w > 0 Then
                        col.Width = w
                    Else
                        col.width = 150
                    End If
                End Using
            Next

            AvatarView.EndUpdate()
            AvatarView.Show()
        Catch ex As Exception
            BreakPoint.Dump(ex)
            Log(My.Resources.Error_word, " RegionList " & ex.Message)
        Finally
            PropUpdateView() = False

        End Try

    End Sub

    Private Sub ShowDetails()

        ShowTitle()

        AllNone.Visible = True
        detailsinitted = False
        ListView1.Show()
        ListView1.Visible = True
        IconView.Hide()
        AvatarView.Hide()
        UserView.Hide()

        ListView1.TabIndex = 0

        ListView1.BeginUpdate()
        ListView1.Items.Clear()

        Dim p As PerformanceCounter = Nothing

        Try
            For Each RegionUUID As String In SearchArray

                If OnButton.Checked And Not RegionEnabled(RegionUUID) Then Continue For
                If OffButton.Checked And RegionEnabled(RegionUUID) Then Continue For
                If SmartButton.Checked And Not Smart_Start(RegionUUID) = "True" Then Continue For
                If Bootedbutton.Checked And RegionStatus(RegionUUID) <> SIMSTATUSENUM.Booted Then Continue For
                If StoppedButton.Checked And RegionStatus(RegionUUID) <> SIMSTATUSENUM.Stopped Then Continue For

                Dim Num As Integer = 0
                Dim Letter As String = ""
                Dim status = GetStatus(RegionUUID, Num, Letter)

                ' Create items and sub items for each item. Place a check mark next to the item.
                Dim item1 As New ListViewItem(Region_Name(RegionUUID), Num) With
                    {
                        .Checked = RegionEnabled(RegionUUID)
                    }
                item1.SubItems.Add(Group_Name(RegionUUID).ToString(Globalization.CultureInfo.CurrentCulture))
                item1.SubItems.Add(AvatarCount(RegionUUID).ToString(Globalization.CultureInfo.CurrentCulture))

                item1.SubItems.Add(Letter)
                Dim fmtXY = "00000" ' 65536
                Dim fmtRam = "0.0" ' 9999 MB
                ' RAM

                If status = SIMSTATUSENUM.Booting _
                        Or (status = SIMSTATUSENUM.Suspended And Settings.Smart_Start And Smart_Start(RegionUUID) = "True") _
                        Or status = SIMSTATUSENUM.Booted _
                        Or status = SIMSTATUSENUM.RecyclingUp _
                        Or status = SIMSTATUSENUM.RecyclingDown _
                        Then

                    Try
                        Dim PID = ProcessID(RegionUUID)
                        Dim component1 As Process = CachedProcess(PID)
                        Dim Memory As Double = (component1.WorkingSet64 / 1024) / 1024
                        TotalRam += Memory
                        item1.SubItems.Add(Memory.ToString("0.0", Globalization.CultureInfo.CurrentCulture))
                    Catch ex As Exception
                        item1.SubItems.Add("0")
                    End Try
                Else
                    item1.SubItems.Add("0")
                End If

                Dim cpupercent As Double = 0
                If Not status = SIMSTATUSENUM.Stopped And Not status = SIMSTATUSENUM.Error Then
                    Dim Groupname As String = Group_Name(RegionUUID)
                    CPUValues.TryGetValue(Groupname, cpupercent)
                End If

                item1.SubItems.Add(CStr(cpupercent))
                Dim c As Color = SystemColors.ControlText
                If cpupercent > 1 Then
                    c = Color.Red
                End If

                item1.SubItems.Add(Coord_X(RegionUUID).ToString(fmtXY, Globalization.CultureInfo.CurrentCulture))
                item1.SubItems.Add(Coord_Y(RegionUUID).ToString(fmtXY, Globalization.CultureInfo.CurrentCulture))

                ' Size of region
                Dim s As Double = SizeX(RegionUUID) / 256
                Dim size As String = CStr(s) & "X" & CStr(s)
                item1.SubItems.Add(size)
                item1.SubItems.Add(Estate(RegionUUID))

                If UseMysql Then
                    item1.SubItems.Add(GetPrimCount(RegionUUID).ToString("00000", Globalization.CultureInfo.CurrentCulture))
                Else
                    item1.SubItems.Add("N/A")
                End If

                item1.SubItems.Add(Region_Port(RegionUUID).ToString(Globalization.CultureInfo.CurrentCulture))
                item1.SubItems.Add(GroupPort(RegionUUID).ToString(Globalization.CultureInfo.CurrentCulture))

                'Scripts XEngine or YEngine
                Select Case ScriptEngine(RegionUUID)
                    Case "YEngine"
                        item1.SubItems.Add(My.Resources.YEngine_word)
                    Case "XEngine"
                        item1.SubItems.Add(My.Resources.XEngine_word)
                    Case "Off"
                        item1.SubItems.Add("Off".ToUpperInvariant)
                    Case Else
                        item1.SubItems.Add("-".ToUpperInvariant)
                End Select

                'Map
                If MapType(RegionUUID).Length > 0 Then
                    item1.SubItems.Add(MapType(RegionUUID))
                Else
                    item1.SubItems.Add("-".ToUpperInvariant)
                End If

                ' physics
                Select Case RegionPhysics(RegionUUID)
                    Case ""
                        item1.SubItems.Add("-".ToUpperInvariant)
                    Case "0"
                        item1.SubItems.Add(My.Resources.None)
                    Case "1"
                        item1.SubItems.Add(My.Resources.ODE_word_NT)
                    Case "2"
                        item1.SubItems.Add(My.Resources.Bullet_word_NT)
                    Case "3"
                        item1.SubItems.Add(My.Resources.Bullet_Threaded_word)
                    Case "4"
                        item1.SubItems.Add(My.Resources.ubODE_word)
                    Case "5"
                        item1.SubItems.Add(My.Resources.ubODE_Hybrid_word)
                    Case Else
                        item1.SubItems.Add("-".ToUpperInvariant)
                End Select

                'birds

                If Birds(RegionUUID) = "True" Then
                    item1.SubItems.Add(My.Resources.Yes_word)
                Else
                    item1.SubItems.Add("-".ToUpperInvariant)
                End If

                'Tides
                If Tides(RegionUUID) = "True" Then
                    item1.SubItems.Add(My.Resources.Yes_word)
                Else
                    item1.SubItems.Add("-".ToUpperInvariant)
                End If

                'teleport
                If Teleport_Sign(RegionUUID) = "True" Then
                    item1.SubItems.Add(My.Resources.Yes_word)
                Else
                    item1.SubItems.Add("-".ToUpperInvariant)
                End If

                If Smart_Start(RegionUUID) = "True" And Settings.Smart_Start Then
                    item1.SubItems.Add(My.Resources.Yes_word)
                Else
                    item1.SubItems.Add("-".ToUpperInvariant)
                End If

                If AllowGods(RegionUUID).Length > 0 Then
                    item1.SubItems.Add(AllowGods(RegionUUID))
                Else
                    item1.SubItems.Add("-".ToUpperInvariant)
                End If

                If RegionGod(RegionUUID).Length > 0 Then
                    item1.SubItems.Add(RegionGod(RegionUUID))
                Else
                    item1.SubItems.Add("-".ToUpperInvariant)
                End If

                If ManagerGod(RegionUUID).Length > 0 Then
                    item1.SubItems.Add(ManagerGod(RegionUUID))
                Else
                    item1.SubItems.Add("-".ToUpperInvariant)
                End If

                If SkipAutobackup(RegionUUID).Length > 0 Then
                    item1.SubItems.Add(SkipAutobackup(RegionUUID))
                Else
                    item1.SubItems.Add("-".ToUpperInvariant)
                End If

                If RegionSnapShot(RegionUUID).Length > 0 Then
                    item1.SubItems.Add(RegionSnapShot(RegionUUID))
                Else
                    item1.SubItems.Add("-".ToUpperInvariant)
                End If

                If MinTimerInterval(RegionUUID).Length > 0 Then
                    item1.SubItems.Add(MinTimerInterval(RegionUUID))
                Else
                    item1.SubItems.Add("-".ToUpperInvariant)
                End If

                If FrameTime(RegionUUID).Length > 0 Then
                    item1.SubItems.Add(FrameTime(RegionUUID))
                Else
                    item1.SubItems.Add("-".ToUpperInvariant)
                End If

                item1.SubItems.Add(BootTime(RegionUUID).ToString("0.0", Globalization.CultureInfo.CurrentCulture))
                item1.SubItems.Add(MapTime(RegionUUID).ToString("0.0", Globalization.CultureInfo.CurrentCulture))

                item1.ForeColor = c
                ListView1.Items.AddRange(New ListViewItem() {item1})

            Next
        Catch ex As Exception
            BreakPoint.Dump(ex)
            Log(My.Resources.Error_word, " RegionList " & ex.Message)
        End Try

        For Each col In ListView1.Columns
            Using csize As New ClassScreenpos(MyBase.Name & "ColumnSize")
                Dim w = csize.ColumnWidth(CStr(col.name))
                If w > 0 Then col.Width = w
            End Using
        Next

        ListView1.EndUpdate()
        detailsinitted = True

        PropUpdateView() = False

    End Sub

    Private Sub ShowIcons()

        ShowTitle()

        AllNone.Visible = False
        IconView.TabIndex = 0
        IconView.Show()
        UserView.Visible = True
        ListView1.Hide()
        AvatarView.Hide()
        UserView.Hide()

        IconView.BeginUpdate()
        IconView.Items.Clear()
        Dim max_length As Integer
        For Each RegionUUID As String In SearchArray

            If OnButton.Checked And Not RegionEnabled(RegionUUID) Then Continue For
            If OffButton.Checked And RegionEnabled(RegionUUID) Then Continue For
            If SmartButton.Checked And Not Smart_Start(RegionUUID) = "True" Then Continue For
            If Bootedbutton.Checked And RegionStatus(RegionUUID) <> SIMSTATUSENUM.Booted Then Continue For
            If StoppedButton.Checked And RegionStatus(RegionUUID) <> SIMSTATUSENUM.Stopped Then Continue For

            Try
                Dim Num As Integer = 0
                Dim Letter As String = ""
                GetStatus(RegionUUID, Num, Letter)
                Dim name = Region_Name(RegionUUID)
                If name.Length > max_length Then max_length = name.Length
                ' Create items and sub items for each item. Place a check mark next to the item.
                Dim item1 As New ListViewItem(name, Num) With
                {
                    .Checked = RegionEnabled(RegionUUID)
                }
                IconView.Items.AddRange(New ListViewItem() {item1})
            Catch ex As Exception
                BreakPoint.Dump(ex)
                Log(My.Resources.Error_word, " RegionList " & ex.Message)
            End Try
        Next

        For Each part In IconView.Items
            part.Text = part.Text.PadRight(max_length)
        Next

        PropUpdateView() = False
        IconView.EndUpdate()

    End Sub

    Private Sub ShowTitle()

        Dim TotalSize As Double
        Dim RegionCount As Integer
        Dim TotalRegionCount As Integer
        Dim SSRegionCount As Integer

        For Each RegionUUID As String In SearchArray
            TotalSize += SizeX(RegionUUID) / 256 * SizeY(RegionUUID) / 256
            If RegionEnabled(RegionUUID) Then
                RegionCount += 1
            End If
            If RegionEnabled(RegionUUID) And Smart_Start(RegionUUID) = "True" Then
                SSRegionCount += 1
            End If
            TotalRegionCount += 1
        Next
        Me.Text = $"{CStr(TotalRegionCount)} {My.Resources.Regions_word}.  {CStr(RegionCount)} {My.Resources.Enabled_word} {My.Resources.Regions_word}. {CStr(SSRegionCount)} {My.Resources.Smart_Start_word} {My.Resources.Regions_word}. {My.Resources.TotalArea_word}: {CStr(TotalSize)} {My.Resources.Regions_word} RAM: {TotalRam.ToString("0.0", Globalization.CultureInfo.CurrentCulture)}"

    End Sub

    Private Sub ShowUsers()

        AllNone.Visible = True
        UserView.TabIndex = 0

        UserView.Show()
        UserView.Visible = True
        ListView1.Hide()
        AvatarView.Hide()
        IconView.Hide()

        UserView.BeginUpdate()
        UserView.Items.Clear()
        UserView.CheckBoxes = True
        Dim Index = 0

        Try

            ' Create items and sub items for each item.
            If Not IsMySqlRunning() Then Return

            Dim Mail = GetEmailList()

            For Each Agent In Mail

                Dim k = Agent.Key
                Dim O = Agent.Value

                If O.firstname.Contains(SearchBox.Text) Or O.LastName.Contains(SearchBox.Text) Or O.Email.Contains(SearchBox.Text) Or SearchBox.Text.Length = 0 Or SearchBox.Text = My.Resources.Search_word Then

                    Dim item1 As New ListViewItem(O.firstname & " " & O.LastName, Index)

                    If O.Email.Length = 0 Then
                        item1.BackColor = Color.DarkGray
                        item1.ForeColor = Color.White
                    Else
                        item1.BackColor = Color.White
                        item1.ForeColor = Color.Black
                    End If

                    ' Build output string

                    item1.SubItems.Add(O.Email)
                    item1.SubItems.Add(O.Title)
                    item1.SubItems.Add(O.DiffDays)
                    item1.SubItems.Add(O.userlevel)
                    item1.SubItems.Add(O.Datestring)
                    item1.SubItems.Add(O.Assets)
                    item1.SubItems.Add(O.principalid)
                    UserView.Items.AddRange(New ListViewItem() {item1})

                    Index += 1
                End If

            Next

            Me.Text = Mail.Count & " " & My.Resources.Users_word

            If Index = 0 Then
                Dim item1 As New ListViewItem(My.Resources.No_Avatars, Index)
                item1.SubItems.Add("-".ToUpperInvariant)
                item1.SubItems.Add(My.Resources.Hypergrid_word)
                UserView.Items.AddRange(New ListViewItem() {item1})
            End If

            For Each col In UserView.Columns
                Using csize As New ClassScreenpos(MyBase.Name & "ColumnSize")
                    Dim w = csize.ColumnWidth(CStr(col.Name))
                    If w > 0 Then
                        col.Width = w
                    End If
                End Using
            Next
        Catch ex As Exception
            BreakPoint.Dump(ex)
            Log(My.Resources.Error_word, " RegionList " & ex.Message)
        End Try

        UserView.EndUpdate()

        PropUpdateView() = False

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        If TheView1 = ViewType.Users Then
            Timer1.Stop()
            Return
        End If

        If PropUpdateView() Then ' force a refresh

            LoadMyListView()

        End If

    End Sub

    Private Sub Userview_ColumnWidthChanged(sender As Object, e As ColumnWidthChangedEventArgs) Handles UserView.ColumnWidthChanged

        Dim w = UserView.Columns(e.ColumnIndex).Width
        Dim name = UserView.Columns(e.ColumnIndex).Name
        If name.Length = 0 Or w = 0 Then Return

        ScreenPosition.PutSize(name, w)
        ScreenPosition.SaveFormSettings()

    End Sub

#End Region

#Region "Clicks"

    Private Sub Email_Click(sender As Object, e As EventArgs) Handles Emails.Click

#Disable Warning CA2000
        Dim EmailForm = New FormEmail
#Enable Warning CA2000
        EmailForm.BringToFront()
        EmailForm.Init(UserView)
        Try
            EmailForm.Visible = True
            EmailForm.Select()
            EmailForm.Activate()
        Catch
        End Try

    End Sub

    Private Sub SmartButton_CheckedChanged(sender As Object, e As EventArgs) Handles SmartButton.CheckedChanged

        LoadMyListView()

    End Sub

    Private Sub StopAllButton_Click(sender As Object, e As EventArgs) Handles StopAllButton.Click

        FormSetup.DoStopActions()
        LoadMyListView()

    End Sub

    Private Sub StoppedButton_CheckedChanged(sender As Object, e As EventArgs) Handles StoppedButton.CheckedChanged

        LoadMyListView()

    End Sub

    Private Sub TbSecurity_KeyPress(sender As System.Object, e As System.EventArgs) Handles SearchBox.KeyUp

        LoadMyListView()

    End Sub

    Private Sub Users_Click(sender As Object, e As EventArgs) Handles Users.Click

        Settings.RegionListView() = ViewType.Users
        Settings.SaveSettings()
        TheView1 = ViewType.Users
        SetScreen(TheView1)
        ListView1.View = View.List
        ListView1.Hide()
        UserView.Show()
        AvatarView.Hide()
        LoadMyListView()

    End Sub

    Private Sub ViewAvatars_Click(sender As Object, e As EventArgs) Handles AvatarsButton.Click

        Settings.RegionListView() = ViewType.Avatars
        Settings.SaveSettings()
        TheView1 = ViewType.Avatars

        SetScreen(TheView1)
        ListView1.View = View.Details

        ListView1.Hide()
        UserView.Hide()
        IconView.Hide()
        AvatarView.Show()
        LoadMyListView()
        Timer1.Start()

    End Sub

    Private Sub ViewCompact_Click(sender As Object, e As EventArgs) Handles IconsButton.Click

        Settings.RegionListView() = ViewType.Icons
        Settings.SaveSettings()
        TheView1 = ViewType.Icons
        SetScreen(TheView1)
        ListView1.View = View.SmallIcon
        IconView.Show()
        AvatarView.Hide()
        UserView.Hide()

        ListView1.CheckBoxes = False
        Timer1.Start()
        LoadMyListView()

    End Sub

    Private Sub ViewDetail_Click(sender As Object, e As EventArgs) Handles DetailsButton.Click

        Settings.RegionListView() = ViewType.Details
        Settings.SaveSettings()
        TheView1 = ViewType.Details
        SetScreen(TheView1)
        ListView1.View = View.Details
        ListView1.Show()
        AvatarView.Hide()
        UserView.Hide()

        ListView1.CheckBoxes = True
        Timer1.Start()
        LoadMyListView()

    End Sub

#End Region

End Class
