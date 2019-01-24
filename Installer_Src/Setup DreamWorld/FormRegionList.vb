﻿
Imports System.IO

Public Class RegionList

    Dim ViewNotBusy As Boolean
    Dim TheView As Integer = 0
    Private Shared FormExists As Boolean = False
    Dim pixels As Integer = 70
    Dim imageListSmall As New ImageList
    Dim imageListLarge As ImageList
    Dim ItemsAreChecked As Boolean = False
    Dim RegionClass As RegionMaker = RegionMaker.Instance(Form1.MysqlConn)
    Dim Timertick As Integer = 0

    <Flags()>
    Private Enum REGION_TIMER As Integer
        STOPPED = -3
        RESTARTING = -2
        RESTART_PENDING = -1
        START_COUNTING = 0
    End Enum


    Public Property UpdateView() As Boolean
        Get
            Return Form1.UpdateView
        End Get
        Set(ByVal Value As Boolean)
            Form1.UpdateView = Value
        End Set
    End Property
    ' property exposing FormExists
    Public Shared ReadOnly Property InstanceExists() As Boolean
        Get
            ' Access shared members through the Class name, not an instance.
            Return RegionList.FormExists
        End Get
    End Property

#Region "ScreenSize"
    Public ScreenPosition As ScreenPos
    Private Handler As New EventHandler(AddressOf resize_page)

    'The following detects  the location of the form in screen coordinates
    Private Sub resize_page(ByVal sender As Object, ByVal e As System.EventArgs)
        'Me.Text = "Form screen position = " + Me.Location.ToString
        ScreenPosition.SaveXY(Me.Left, Me.Top)
    End Sub
    Private Sub SetScreen()
        Me.Show()
        ScreenPosition = New ScreenPos(Me.Name)
        AddHandler ResizeEnd, Handler
        Dim xy As List(Of Integer) = ScreenPosition.GetXY()
        Me.Left = xy.Item(0)
        Me.Top = xy.Item(1)
    End Sub

#End Region

#Region "Layout"

    Private Sub Panel1_MouseWheel(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles ListView1.MouseWheel

        If TheView = 2 And ViewNotBusy Then
            ' Update the drawing based upon the mouse wheel scrolling.
            Dim numberOfTextLinesToMove As Integer = CInt(e.Delta * SystemInformation.MouseWheelScrollLines / 120)

            pixels = pixels + numberOfTextLinesToMove
            'Debug.Print(pixels.ToString)
            If pixels > 256 Then pixels = 256
            If pixels < 10 Then pixels = 10

            LoadMyListView()
        End If

    End Sub

    Private Sub RegionList_Layout(sender As Object, e As LayoutEventArgs) Handles Me.Layout

        Dim X = Me.Width - 45
        Dim Y = Me.Height - 125
        ListView1.Size = New System.Drawing.Size(X, Y)

    End Sub
#End Region

#Region "Loader"

    Private Sub _Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Size = New System.Drawing.Size(410, 410)

        pixels = 70

        RegionList.FormExists = True

        ' ListView Setup
        ListView1.AllowDrop = True
        ' Set the view to show details.
        ListView1.View = View.Details
        ' Allow the user to edit item text.
        ListView1.LabelEdit = True
        ' Allow the user to rearrange columns.
        ListView1.AllowColumnReorder = True
        ' Display check boxes.
        ListView1.CheckBoxes = True
        ' Select the item and subitems when selection is made.
        ListView1.FullRowSelect = True
        ' Display grid lines.
        ListView1.GridLines = True
        ' Sort the items in the list in ascending order.
        ListView1.Sorting = SortOrder.Ascending

        'Add the items to the ListView.
        ' Connect the ListView.ColumnClick event to the ColumnClick event handler.
        AddHandler Me.ListView1.ColumnClick, AddressOf ColumnClick

        ' Me.Controls.Add(ListView1)
        Me.Name = "Region List"
        Me.Text = "Region List"

        ' index of 0-4 to display icons
        imageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("navigate_up2"))   ' 0 booting up
        imageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("navigate_down2")) ' 1 shutting down
        imageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("check2")) ' 2 okay, up
        imageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("media_stop_red")) ' 3 disabled
        imageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("media_stop"))  ' 4 enabled, stopped
        imageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("replace2"))  ' 5 Restarting
        imageListSmall.Images.Add(My.Resources.ResourceManager.GetObject("warning"))  ' 6 Unknown

        Form1.UpdateView = True ' make form refresh
        LoadMyListView()
        ListView1.Show()
        Timer1.Interval = 1000 ' check for Form1.UpdateView every second
        Timer1.Start() 'Timer starts functioning
        SetScreen()

        Form1.HelpOnce("RegionList")

    End Sub


    Private Sub SingletonForm_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        RegionList.FormExists = False

    End Sub
#End Region

#Region "Timer"
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        If UpdateView() Or Timertick Mod 120 = 0 Then ' force a refresh
            LoadMyListView()
        End If
        Timertick = Timertick + 1

    End Sub
#End Region

#Region "Load List View"
    Public Sub LoadMyListView()

        ViewNotBusy = False

        ListView1.BeginUpdate()

        imageListLarge = New ImageList()
        If pixels = 0 Then pixels = 20
        imageListLarge.ImageSize = New Size(pixels, pixels)
        ListView1.Clear()
        ListView1.Items.Clear()

        ' Create columns for the items and subitems.
        ' Width of -2 indicates auto-size.
        ListView1.Columns.Add("Enabled", 120, HorizontalAlignment.Center)
        ListView1.Columns.Add("DOS Box", 100, HorizontalAlignment.Center)
        ListView1.Columns.Add("Agents", 60, HorizontalAlignment.Center)
        ListView1.Columns.Add("Status", 120, HorizontalAlignment.Center)

        Dim Num As Integer = 0

        ' have to get maps by http port + region UUID, not region port + uuid
        ' RegionClass.DebugGroup() ' show the list of groups and http ports.

        For Each X In RegionClass.RegionNumbers

            Dim Letter As String = ""
            If RegionClass.Timer(X) = REGION_TIMER.RESTART_PENDING Then
                Letter = "Recycling Down"
                Num = 5
            ElseIf RegionClass.Timer(X) = REGION_TIMER.RESTARTING Then
                Letter = "Recycling Up"
                Num = 5
            ElseIf RegionClass.WarmingUp(X) Then
                Letter = "Booting"
                Num = 0
            ElseIf RegionClass.ShuttingDown(X) Then
                Letter = "Stopping"
                Num = 1
            ElseIf RegionClass.Booted(X) Then
                Letter = "Running"
                Num = 2
            ElseIf Not RegionClass.RegionEnabled(X) Then
                Letter = "Disabled"
                Num = 3
            ElseIf RegionClass.RegionEnabled(X) Then
                Letter = "Stopped"
                Num = 4
            Else
                Num = 6 ' warning
            End If

            If TheView = 2 Then

                If RegionClass.Booted(X) Then
                    Dim img As String = "http://127.0.0.1:" + RegionClass.GroupPort(X).ToString + "/" + "index.php?method=regionImage" + RegionClass.UUID(X).Replace("-", "")
                    Debug.Print(img)

                    Dim bmp As Image = LoadImage(img)
                    If bmp Is Nothing Then
                        imageListLarge.Images.Add(My.Resources.ResourceManager.GetObject("OfflineMap"))
                    Else
                        imageListLarge.Images.Add(bmp)

                    End If
                Else
                    imageListLarge.Images.Add(My.Resources.ResourceManager.GetObject("OfflineMap"))
                End If
                Num = X
            End If

            ' Create  items and subitems for each item.
            Dim item1 As New ListViewItem(RegionClass.RegionName(X), Num)
            ' Place a check mark next to the item.
            item1.Checked = RegionClass.RegionEnabled(X)
            item1.SubItems.Add(RegionClass.GroupName(X).ToString)
            item1.SubItems.Add(RegionClass.AvatarCount(X).ToString)

            item1.SubItems.Add(Letter)
            ListView1.Items.AddRange(New ListViewItem() {item1})


        Next

        'Assign the ImageList objects to the ListView.
        ListView1.LargeImageList = imageListLarge
        ListView1.SmallImageList = imageListSmall

        Me.ListView1.TabIndex = 0

        ListView1.EndUpdate()
        ListView1.Show()
        ViewNotBusy = True

        For i As Integer = 0 To ListView1.Items.Count - 1
            If ListView1.Items(i).Checked Then
                ListView1.Items(i).ForeColor = SystemColors.ControlText
            Else
                ListView1.Items(i).ForeColor = SystemColors.GrayText
            End If
        Next i

        UpdateView() = False

    End Sub 'listView1

    Private Function LoadImage(url As String) As Image
        Dim bmp As Bitmap = Nothing
        Dim request As System.Net.WebRequest = System.Net.WebRequest.Create(url)
        Try
            Dim response As System.Net.WebResponse = request.GetResponse()
            Dim responseStream As System.IO.Stream = response.GetResponseStream()
            bmp = New Bitmap(responseStream)

            'Dim s = bmp.Size
            'Debug.Print(s.Width.ToString + ":" + s.Height.ToString)

            responseStream.Dispose()
        Catch ex As Exception
            Form1.Log("Maps: " + ex.Message + ":" + url)
        End Try

        Return bmp

    End Function


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        RegionClass.GetAllRegions()
        LoadMyListView()

    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged

        Dim regions As ListView.SelectedListViewItemCollection = Me.ListView1.SelectedItems
        Dim item As ListViewItem

        For Each item In regions
            Dim RegionName = item.SubItems(0).Text
            Dim checked As Boolean = item.Checked
            Debug.Print("Clicked row " + RegionName)
            Dim R = RegionClass.FindRegionByName(RegionName)
            If R >= 0 Then
                StartStopEdit(checked, R, RegionName)
            End If
        Next

        UpdateView() = True
    End Sub


    Private Declare Function ShowWindow Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal nCmdShow As SHOW_WINDOW) As Boolean

    <Flags()>
    Private Enum SHOW_WINDOW As Integer
        SW_HIDE = 0
        SW_SHOWNORMAL = 1
        SW_NORMAL = 1
        SW_SHOWMINIMIZED = 2
        SW_SHOWMAXIMIZED = 3
        SW_MAXIMIZE = 3
        SW_SHOWNOACTIVATE = 4
        SW_SHOW = 5
        SW_MINIMIZE = 6
        SW_SHOWMINNOACTIVE = 7
        SW_SHOWNA = 8
        SW_RESTORE = 9
        SW_SHOWDEFAULT = 10
        SW_FORCEMINIMIZE = 11
        SW_MAX = 11
    End Enum


    Private Sub StartStopEdit(checked As Boolean, n As Integer, RegionName As String)

        ' show it, stop it, start it, or edit it
        Dim hwnd = Form1.getHwnd(RegionClass.GroupName(n))
        If hwnd <> IntPtr.Zero Then ShowWindow(hwnd, SHOW_WINDOW.SW_RESTORE)

        Dim Choices As New FormRegionPopup
        Dim chosen As String
        Choices.init(RegionName)
        Choices.ShowDialog()
        Try
            ' Read the chosen sim name
            chosen = Choices.Choice()
            If chosen = "Start" Then

                ' it was stopped, and off, so we start up
                If Not Form1.StartMySQL() Then
                    Form1.ProgressBar1.Value = 0
                    Form1.ProgressBar1.Visible = True
                    Form1.Print("Stopped")
                    Return
                End If
                Form1.Start_Robust()
                Form1.Log("Starting " + RegionClass.RegionName(n))
                Form1.CopyOpensimProto() ' !!! make this region specific for speed
                Form1.Boot(RegionClass.RegionName(n))
                UpdateView() = True ' force a refresh
                Return

            ElseIf chosen = "Stop" Then


                For Each num In RegionClass.RegionListByGroupNum(RegionClass.GroupName(n))
                        ' Ask before killing any people
                        If RegionClass.AvatarCount(num) > 0 Then
                            Dim response As MsgBoxResult

                            If RegionClass.AvatarCount(num) = 1 Then
                                response = MsgBox("There is one avatar in " + RegionClass.RegionName(num) + ".  Do you still want to stop it?", vbYesNo)
                            Else
                                response = MsgBox("There are " + RegionClass.AvatarCount(num).ToString + " avatars in " + RegionClass.RegionName(num) + ".  Do you still want to stop it?", vbYesNo)
                            End If
                            If response = vbYes Then
                                StopRegionNum(num)
                            End If
                        Else
                            StopRegionNum(num)
                        End If
                    Next
                    UpdateView = True ' make form refresh
                    Return

                ElseIf chosen = "Edit" Then

                    Dim RegionForm As New FormRegion
                    RegionForm.Init(RegionClass.RegionName(n))
                    RegionForm.Activate()
                    RegionForm.Visible = True
                    RegionForm.Select()
                    UpdateView = True ' make form refresh
                    Return

                ElseIf chosen = "Recycle" Then
                    RegionClass.Timer(n) = REGION_TIMER.RESTART_PENDING  ' request a recycle.
                UpdateView = True ' make form refresh
                Return
            End If
            If chosen.Length > 0 Then
                Choices.Dispose()
            End If
        Catch ex As Exception
            chosen = ""
        End Try


    End Sub

    Private Sub StopRegionNum(num As Integer)


        If Form1.ConsoleCommand(RegionClass.GroupName(num), "q{ENTER}q{ENTER}") Then
            RegionClass.Booted(num) = False
            RegionClass.WarmingUp(num) = False
            RegionClass.ShuttingDown(num) = True
        Else
            RegionClass.Booted(num) = False
            RegionClass.WarmingUp(num) = False
            RegionClass.ShuttingDown(num) = False
        End If
        Form1.Log("Region:Stopping Region " + RegionClass.RegionName(num))
    End Sub

    Private Sub ListView1_ItemCheck1(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles ListView1.ItemCheck

        Dim Item As ListViewItem = ListView1.Items.Item(e.Index)
        Dim n As Integer = RegionClass.FindRegionByName(Item.Text)
        If n = -1 Then Return
        Dim GroupName = RegionClass.GroupName(n)
        For Each X In RegionClass.RegionListByGroupNum(GroupName)
            If ViewNotBusy Then
                If (e.CurrentValue = CheckState.Unchecked) Then
                    RegionClass.RegionEnabled(X) = True
                    ' and region file on disk
                    Form1.MySetting.LoadOtherIni(RegionClass.RegionPath(X), ";")
                    Form1.MySetting.SetOtherIni(RegionClass.RegionName(X), "Enabled", "true")
                    Form1.MySetting.SaveOtherINI()
                ElseIf (e.CurrentValue = CheckState.Checked) Then
                    RegionClass.RegionEnabled(X) = False
                    ' and region file on disk
                    Form1.MySetting.LoadOtherIni(RegionClass.RegionPath(X), ";")
                    Form1.MySetting.SetOtherIni(RegionClass.RegionName(X), "Enabled", "false")
                    Form1.MySetting.SaveOtherINI()
                End If
            End If
        Next


        UpdateView() = True ' force a refresh

    End Sub

    ' ColumnClick event handler.
    Private Sub ColumnClick(ByVal o As Object, ByVal e As ColumnClickEventArgs)

        ListView1.SuspendLayout()
        Me.ListView1.Sorting = SortOrder.None
        ' Set the ListViewItemSorter property to a new ListViewItemComparer 
        ' object. Setting this property immediately sorts the 
        ' ListView using the ListViewItemComparer object.
        Me.ListView1.ListViewItemSorter = New ListViewItemComparer(e.Column)
        ListView1.ResumeLayout()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles ViewButton.Click

        ' It may be busy refreshing so lets wait
        While Not ViewNotBusy
            Form1.Sleep(100)
            Application.DoEvents()
        End While

        Debug.Print(Form1.MySetting.MapType)
        If Form1.MySetting.MapType = "None" And TheView = 1 Then TheView = 2

        If TheView = 0 Then
            ListView1.CheckBoxes = False
            ListView1.View = View.List
            Timer1.Start()
        ElseIf TheView = 1 Then
            ListView1.CheckBoxes = False
            ListView1.View = View.LargeIcon
            Timer1.Stop()
        ElseIf TheView = 2 Then
            ListView1.CheckBoxes = True
            ListView1.View = View.Details
            Timer1.Start()
        End If

        TheView = TheView + 1
        If TheView > 2 Then TheView = 0
        LoadMyListView()

    End Sub

    Private Sub Addregion_Click(sender As Object, e As EventArgs) Handles Addregion.Click

        Dim RegionForm As New FormRegion
        RegionForm.Init("")
        RegionForm.Activate()
        RegionForm.Visible = True

    End Sub

#End Region

#Region "DragDrop"

    Private Sub ListView1_DragEnter(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles ListView1.DragEnter

        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If

    End Sub

    Private Sub ListView1_DragDrop(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles ListView1.DragDrop

        Dim files() As String = CType(e.Data.GetData(DataFormats.FileDrop), String())

        Dim dirpathname = ""
        'Dim yesNo As MsgBoxResult = MsgBox("New regions can be combined with other regions in an existing DOS box (Yes), or run in their own Dos Box (No)", vbYesNo, "Grouping Regions")
        'If yesNo = vbYes Then
        dirpathname = PickGroup()
            If dirpathname = "" Then
                Form1.PrintFast("Aborted")
                Return
            End If
        'End If



        For Each pathname As String In files
            pathname = pathname.Replace("\", "/")
            Dim extension As String = Path.GetExtension(pathname)
            extension = Mid(extension, 2, 5)
            If extension.ToLower = "ini" Then
                Dim filename = Path.GetFileNameWithoutExtension(pathname)
                Dim i = RegionClass.FindRegionByName(filename)
                If i >= 0 Then
                    MsgBox("Region name " + filename + " already exists", vbInformation, "Info")
                    Return
                End If

                If dirpathname = "" Then dirpathname = filename

                Dim NewFilepath = Form1.gPath & "bin\Regions\" + dirpathname + "\Region\"
                If Not Directory.Exists(NewFilepath) Then
                    Directory.CreateDirectory(Form1.gPath & "bin\Regions\" + dirpathname + "\Region")
                End If

                File.Copy(pathname, Form1.gPath & "bin\Regions\" + dirpathname + "\Region\" + filename + ".ini")

            Else
                Form1.Print("Unrecognized file type" + extension + ". Drag and drop any Region.ini files to add them to the system.")
            End If
        Next
        RegionClass.GetAllRegions()
        LoadMyListView()

    End Sub

    Private Function PickGroup() As String

        Dim Chooseform As New Choice ' form for choosing a set of regions
        ' Show testDialog as a modal dialog and determine if DialogResult = OK.

        Chooseform.FillGrid("Group")

        Dim chosen As String
        Chooseform.ShowDialog()
        Try
            ' Read the chosen GROUP name
            chosen = Chooseform.DataGridView.CurrentCell.Value.ToString()
            If chosen.Length > 0 Then
                Chooseform.Dispose()
            End If
        Catch ex As Exception
            chosen = ""
        End Try
        Return chosen

    End Function

    Private Sub RegionHelp_Click(sender As Object, e As EventArgs) Handles RegionHelp.Click

        Form1.Help("RegionList")

    End Sub

    Private Sub AllNome_CheckedChanged(sender As Object, e As EventArgs) Handles AllNome.CheckedChanged

        For Each X As ListViewItem In ListView1.Items
            If ItemsAreChecked Then
                X.Checked = CType(CheckState.Unchecked, Boolean)
            Else
                X.Checked = CType(CheckState.Checked, Boolean)
            End If
        Next

        If ItemsAreChecked Then
            ItemsAreChecked = False
        Else
            ItemsAreChecked = True
        End If

    End Sub


#End Region

End Class

' Implements the manual sorting of items by columns.
Class ListViewItemComparer
    Implements IComparer
    Private col As Integer

    Public Sub New()
        col = 0
    End Sub

    Public Sub New(ByVal column As Integer)
        col = column
    End Sub

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare

        Return [String].Compare(CType(x, ListViewItem).SubItems(col).Text, CType(y, ListViewItem).SubItems(col).Text)

    End Function

End Class
