#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.Net
Imports System.Text.RegularExpressions
Imports System.Threading
Imports Newtonsoft.Json

#Region "JSON"

Public Class FormOAR

    Private SearchBusy As Boolean
    Private WebThread As Thread

#Region "Private Fields"

    Private ReadOnly initSize As Integer = 512
    Private ReadOnly k As Integer = 50
    Private _initted As Boolean
    Private _type As String
    Private NumColumns As Integer

#End Region

#Region "Public Fields"

    Private json() As JSONResult
    Private SearchArray() As JSONResult

#End Region

#Region "Draw"

    Public Sub Redraw(jsonresult As JSONResult())

        Dim gdImageColumn As New DataGridViewImageColumn
        DataGridView.Columns.Add(gdImageColumn)
        DataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
        DataGridView.ScrollBars = ScrollBars.Vertical
        DataGridView.AutoSize = False

        'add 0 px padding to bottom
        DataGridView.RowTemplate.DefaultCellStyle.Padding = New Padding(10, 0, 10, 0)
        DataGridView.GridColor = Color.White
        DataGridView.BackgroundColor = Color.White
        DataGridView.RowHeadersVisible = False
        DataGridView.Width = initSize
        DataGridView.ShowCellToolTips = True
        DataGridView.AllowUserToAddRows = False
        DataGridView.SelectionMode = DataGridViewSelectionMode.CellSelect
        DataGridView.ScrollBars = ScrollBars.Both
        DataGridView.Enabled = True

        NumColumns = CInt(Math.Ceiling(Me.Width / 256) - 1.0)
        If NumColumns = 0 Then
            NumColumns = 1
        End If

        DataGridView.Height = Me.Height - 50
        'DataGridView.RowTemplate.Height = 256  ' Math.Ceiling((Me.Width - k) / NumColumns)
        DataGridView.RowTemplate.MinimumHeight = CInt(Math.Ceiling((Me.Width - k) / NumColumns))

        DataGridView.Width = Me.Width - 50
        DataGridView.Columns(0).Width = Me.Width - k
        'DataGridView.ColumnHeadersHeight = CInt(Math.Ceiling((Me.Width - k) / NumColumns))
        DataGridView.Columns.Clear()
        DataGridView.Rows.Clear()
        DataGridView.ClearSelection()

        For index = 1 To NumColumns 'How many do you want?
            Using col As New DataGridViewImageColumn
                With col
                    .Width = 256 '(Me.Width - k) / NumColumns
                    .Name = "Details" & CStr(index)
                    .Frozen = False
                    .ImageLayout = DataGridViewImageCellLayout.Zoom
                End With
                DataGridView.Columns.Insert(0, col)
            End Using
        Next
        Dim column = 0
        Dim rowcounter = 0
        If jsonresult IsNot Nothing Then
            Dim cnt = jsonresult.Length
            Me.Text = CStr(cnt) & " " & Global.Outworldz.My.Resources.Items_word

            For Each item In jsonresult
                Application.DoEvents()
                Debug.Print("Item:" & item.Name)

                If column = 0 Then DataGridView.Rows.Add()
                Save(item, rowcounter, column)

                column += 1
                If column = NumColumns Then
                    rowcounter += 1
                    column = 0
                End If
            Next
        Else
            DataGridView.Rows.Add()
            While column < NumColumns
                DataGridView.Rows(rowcounter).Cells(column).Value = Global.Outworldz.My.Resources.NoImage
                column += 1
            End While
        End If
        Try
            While column < NumColumns And column > 0
                DataGridView.Rows(rowcounter).Cells(column).Value = Global.Outworldz.My.Resources.Blank256
                column += 1
            End While
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try
        DataGridView.AutoResizeRows()
        DataGridView.PerformLayout()
        DataGridView.Show()

    End Sub

    Private Shared Sub SaveLicenseFile(O As FileData)

        'FileData.Result = Result
        'FileData.Name = Name
        'FileData.Text = item.License

        Dim Path = IO.Path.Combine(Settings.CurrentDirectory, "Licenses_to_Content")

        Using file As New System.IO.StreamWriter(Path & $"\{O.Name}.txt", False)
            file.Write(O.Text)
        End Using

    End Sub

    Private Sub DataGridView_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView.CellClick

        Try
            Dim File As String = SearchArray(e.ColumnIndex + (NumColumns * e.RowIndex)).Name
            File = $"{PropHttpsDomain}/Outworldz_Installer/{_type}/{File}" 'make a real URL
            If File.EndsWith(".oar", StringComparison.OrdinalIgnoreCase) Or
                File.EndsWith(".gz", StringComparison.OrdinalIgnoreCase) Or
                File.EndsWith(".tgz", StringComparison.OrdinalIgnoreCase) Then

                Dim O = CCBY(File)
                If O.Result = MsgBoxResult.No Then Return

                SaveLicenseFile(O)

                Me.Hide()
                LoadOARContent(File)
            ElseIf File.EndsWith(".iar", StringComparison.OrdinalIgnoreCase) Then
                LoadIARContent(File)
            End If
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

    End Sub

#End Region

#Region "IO"

    Private Shared Function GetImageFromURL(ByVal url As Uri) As Image

        Dim req As WebRequest = System.Net.WebRequest.Create(url)
        Try
            Using request As System.Net.WebResponse = req.GetResponse
                Using stream As IO.Stream = request.GetResponseStream
                    Return New Bitmap(System.Drawing.Image.FromStream(stream))
                End Using
            End Using
        Catch ex As Exception
            Log("Warn", ex.Message)
        End Try

        Return Nothing

    End Function

    Private Sub Save(item As JSONResult, row As Integer, col As Integer)
        Try
            If item Is Nothing Then Return
            If item.Cache.Width > 0 Then
                DataGridView.Rows(row).Cells(col).Value = item.Cache
                DataGridView.Rows(row).Cells(col).ToolTipText = item.Str
            Else
                DataGridView.Rows(row).Cells(col).Value = NoImage(item)
            End If
        Catch ex As Exception
            BreakPoint.Dump(ex)
            Log("Error", ex.Message)
        End Try

    End Sub

    Private Sub ToolStripMenuItem30_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem30.Click

        If _type = "IAR" Then HelpManual("Load IAR")
        If _type = "OAR" Then HelpManual("Load OAR")

    End Sub

#End Region

#Region "ScreenSize"

    Private ReadOnly Handler As New EventHandler(AddressOf Resize_page)
    Private _screenPosition As ClassScreenpos
    Dim aHeight As Integer
    Dim aWidth As Integer

    Public Property ScreenPosition As ClassScreenpos
        Get
            Return _screenPosition
        End Get
        Set(value As ClassScreenpos)
            _screenPosition = value
        End Set
    End Property

    Public Function GetJson() As JSONResult()
        Return json
    End Function

    'The following detects  the location of the form in screen coordinates
    Private Sub Resize_page(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not _initted Then Return
        ScreenPosition.SaveXY(Me.Left, Me.Top)
        ScreenPosition.SaveHW(Me.Height, Me.Width)

        If Height <> aHeight Or Width <> aWidth Then
            aHeight = Height
            aWidth = Width
            Redraw(SearchArray)
        End If

    End Sub

    Private Sub SetScreen()

        ScreenPosition = New ClassScreenpos(Me.Name)
        AddHandler ResizeEnd, Handler
        Dim xy As List(Of Integer) = ScreenPosition.GetXY()
        Me.Left = xy.Item(0)
        Me.Top = xy.Item(1)
        Dim hw As List(Of Integer) = ScreenPosition.GetHW()

        If hw.Item(0) = 0 Then
            Me.Height = initSize * 2
        Else
            Me.Height = hw.Item(0)
        End If
        If hw.Item(1) = 0 Then
            Me.Width = initSize * 2
        Else
            Me.Width = hw.Item(1)
        End If

    End Sub

#End Region

#Region "Start/Stop"

    Public Sub Init(type As String)

        _type = type

        InitiateThread()

    End Sub

    Public Sub ShowForm()
        TextBox1.Text = ""
        Me.Show()
        Search()
        'Redraw(SearchArray)
        If _type = "OAR" Then HelpOnce("Load OAR")
        If _type = "IAR" Then HelpOnce("Load IAR")
    End Sub

    Private Function CCBY(file As String) As FileData

        Dim Out As New FileData

        If SearchArray IsNot Nothing Then
            Dim Str As String = ""
            Dim pattern = New Regex(".*/(.*?)$", RegexOptions.IgnoreCase)
            Dim match As Match = pattern.Match(file)
            If match.Success Then
                Dim name = match.Groups(1).Value
                For Each item In SearchArray

                    If item.Name = name Then
                        Dim terms As String = ""
                        Try
                            If item.Exclusive = "yes" Then terms = " " & My.Resources.exclusive
                        Catch
                        End Try

                        Dim R = MsgBox($"{item.Author}'s {name} {My.Resources.islicensedas} {item.CCBY} {terms}. {My.Resources.Terms}", MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground, My.Resources.Info_word)
                        Out.Result = R
                        Out.Name = name
                        Out.Text = item.License
                        Return Out
                    End If
                Next
            End If
        End If

        Return Out

    End Function

    Private Function DoWork() As JSONResult

        SearchBusy = True
        json = GetData()
        If json IsNot Nothing Then
            If _type = "OAR" Then
                Settings.OarCount = json.Length
            End If
            json = ImageToJson(json)
            SearchArray = json
        End If

        _initted = True
        SearchBusy = False
        Return Nothing

    End Function

    Private Sub Form_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        MenuStrip2.Text = Global.Outworldz.My.Resources._0
        PictureBox1.Image = Global.Outworldz.My.Resources.view
        RefreshToolStripMenuItem.Image = Global.Outworldz.My.Resources.refresh
        RefreshToolStripMenuItem.Text = Global.Outworldz.My.Resources.Refresh_word
        ToolStripMenuItem30.Image = Global.Outworldz.My.Resources.question_and_answer
        ToolStripMenuItem30.Text = Global.Outworldz.My.Resources.Help_word

        NameRadioButton.Text = My.Resources.SortbyName
        DateRadioButton.Text = My.Resources.SortbyDate
        AscendRadioButton.Text = My.Resources.Ascending
        DescendRadioButton.Text = My.Resources.Descending

        SetScreen()

        NameRadioButton.Checked = Settings.NameOrDate
        DateRadioButton.Checked = Not Settings.NameOrDate
        AscendRadioButton.Checked = Settings.AscendOrDescend
        DescendRadioButton.Checked = Not Settings.AscendOrDescend
        Application.DoEvents()
        Search()

    End Sub

    Private Sub Form1_Closed(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing

        Me.Hide()
        e.Cancel = True

    End Sub

    Private Sub InitiateThread()

        SearchBusy = True
        WebThread = New Thread(DirectCast(Function() DoWork(), ThreadStart))

        Try
            WebThread.SetApartmentState(ApartmentState.STA)
        Catch ex As Exception
            BreakPoint.Dump(ex)
            Log(My.Resources.Error_word, ex.Message)
        End Try
        WebThread.Start()

    End Sub

#End Region

#Region "Data"

    Private Function GetData() As JSONResult()

        Dim result As String = Nothing
        Using client As New WebClient ' download client for web pages
            Try
                Dim str = $"{PropHttpsDomain}/outworldz_installer/JSON/{_type}2.json?r={CStr(Random())}"
                result = client.DownloadString(str)
            Catch ex As Exception
                BreakPoint.Dump(ex)
                ErrorLog(My.Resources.Wrong & " " & ex.Message)
                Return Nothing
            End Try
        End Using
        Try
            json = JsonConvert.DeserializeObject(Of JSONResult())(result)
        Catch ex As Exception
            Return json
        End Try
        Return json

    End Function

#End Region

#Region "Imagery"

    Private Shared Function DrawTextOnImage(item As String, photo As Image, color As Color) As Image

        ' Create solid brush.
        Using blueBrush = New SolidBrush(color)
            ' Create rectangle.
            Dim rect = New Rectangle(0, 0, 256, 20)
            Dim bmp = photo
            Dim newImage As New Bitmap(256, 256)
            Try

                Using drawFont = New Font("Arial", 8)
                    Using gr As Graphics = Graphics.FromImage(newImage)
                        gr.DrawImageUnscaled(bmp, 0, 0)
                        gr.FillRectangle(blueBrush, rect)
                        gr.DrawString(item, drawFont, Brushes.White, 10, 5)
                    End Using
                End Using
            Catch
            End Try
            Return newImage

        End Using

    End Function

    Private Shared Function NoImage(item As JSONResult) As Image

        Dim bmp = Global.Outworldz.My.Resources.Blank256
        Using drawFont = New Font("Arial", 12)
            Dim newImage = New Bitmap(256, 256)
            Try
                Dim gr = Graphics.FromImage(newImage)
                gr.DrawImageUnscaled(bmp, 0, 0)
                gr.DrawString(item.Name, drawFont, Brushes.Black, 30, 100)
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
            Return newImage
        End Using
        Return Nothing

    End Function

    Private Function ImageToJson(ByVal jsonarray() As JSONResult) As JSONResult()

        If jsonarray IsNot Nothing Then

            For Each item In jsonarray
                Application.DoEvents()
                Debug.Print("Item:" & item.Name)

                Dim bmp = New Bitmap(256, 276)
                If item.Cache IsNot Nothing Then
                    Using g As Graphics = Graphics.FromImage(bmp)

                        Try
                            g.DrawImage(item.Cache, 0, 0, bmp.Width, bmp.Height)
                        Catch
                        End Try

                    End Using
                Else
                    Dim img As Image = Nothing
                    If item.Photo.Length > 0 Then
                        Dim link As New Uri($"{PropHttpsDomain}/Outworldz_installer/{_type}/{item.Photo}?r={CStr(Random())}")
#Disable Warning CA2000
                        img = GetImageFromURL(link)
#Enable Warning CA2000

                    End If

                    If img Is Nothing Then
#Disable Warning CA2000
                        img = NoImage(item)
#Enable Warning CA2000
                    End If

                    Dim colorcode As Color = Color.FromArgb(84, 115, 159)
                    Dim txt = item.Name
                    If item.Exclusive = "yes" Then
                        txt += " ** EXCLUSIVE **"
                    End If
                    img = DrawTextOnImage(txt, img, colorcode)

                    Try
                        Using g As Graphics = Graphics.FromImage(bmp)
                            g.DrawImage(img, 0, 0, bmp.Width, bmp.Height)
                        End Using
                    Catch ex As Exception
                    End Try
                    img.Dispose()

                End If
                item.Cache = bmp
                Try
                    item.Size = Format(CDbl(item.Size) / (1024 * 1024), "###0.00")
                Catch
                End Try
                item.Str = item.Name & vbCrLf & item.Size & "MB" & vbCrLf & item.License
            Next
        End If

        Return json

    End Function

#End Region

#Region "Search"

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles ExclusiveCheckbox.CheckedChanged

        Search()

    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

        Search()

    End Sub

    Private Sub RadioDateRadioButton_CheckedChanged(sender As Object, e As EventArgs) Handles DateRadioButton.CheckedChanged
        Settings.NameOrDate = DateRadioButton.Checked
        If DateRadioButton.Checked Then Search()
    End Sub

    Private Sub RadioDecendRadioButton_CheckedChanged(sender As Object, e As EventArgs) Handles DescendRadioButton.CheckedChanged
        Settings.AscendOrDescend = DescendRadioButton.Checked
        If DescendRadioButton.Checked Then Search()
    End Sub

    Private Sub RadioNameRadioButton_CheckedChanged(sender As Object, e As EventArgs) Handles NameRadioButton.CheckedChanged

        Settings.NameOrDate = NameRadioButton.Checked
        If NameRadioButton.Checked Then Search()
    End Sub

    Private Sub RadioOldestRadioButton_CheckedChanged(sender As Object, e As EventArgs) Handles AscendRadioButton.CheckedChanged
        Settings.AscendOrDescend = AscendRadioButton.Checked
        If AscendRadioButton.Checked Then Search()
    End Sub

    Private Sub RefreshToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RefreshToolStripMenuItem.Click

        SearchBusy = True
        json = GetData()
        If _type = "OAR" Then
            Settings.OarCount = json.Length
        End If
        json = ImageToJson(json)
        SearchArray = json
        _initted = True
        SearchBusy = False
        Search()

    End Sub

    Private Sub Search()

        If SearchBusy = True Then Return
        SearchBusy = True
        Dim searchterm = TextBox1.Text
        Try
            'If searchterm.Length > 0 Then
            Erase SearchArray
            ' search
            If ExclusiveCheckbox.Checked Then
                For Each item In json
                    If searchterm.Length = 0 Or
                        item.License.ToUpper(Globalization.CultureInfo.InvariantCulture).Contains(searchterm.ToUpper(Globalization.CultureInfo.InvariantCulture)) Or
                        item.Name.ToUpper(Globalization.CultureInfo.InvariantCulture).Contains(searchterm.ToUpper(Globalization.CultureInfo.InvariantCulture)) Or
                        item.Author.ToUpper(Globalization.CultureInfo.InvariantCulture).Contains(searchterm.ToUpper(Globalization.CultureInfo.InvariantCulture)) Then
                        If item.Exclusive <> "yes" Then
                            Continue For
                        End If
                        Dim l As Integer
                        If SearchArray Is Nothing Then
                            l = 0
                        Else
                            l = SearchArray.Length
                        End If
                        Array.Resize(SearchArray, l + 1)
                        SearchArray(SearchArray.Length - 1) = item
                    End If
                Next
            Else
                If json IsNot Nothing Then
                    For Each item In json
                        If searchterm.Length = 0 Or
                        item.License.ToUpper(Globalization.CultureInfo.InvariantCulture).Contains(searchterm.ToUpper(Globalization.CultureInfo.InvariantCulture)) Or
                        item.Name.ToUpper(Globalization.CultureInfo.InvariantCulture).Contains(searchterm.ToUpper(Globalization.CultureInfo.InvariantCulture)) Or
                        item.Author.ToUpper(Globalization.CultureInfo.InvariantCulture).Contains(searchterm.ToUpper(Globalization.CultureInfo.InvariantCulture)) Then
                            Dim l As Integer
                            If SearchArray Is Nothing Then
                                l = 0
                            Else
                                l = SearchArray.Length
                            End If
                            Array.Resize(SearchArray, l + 1)
                            SearchArray(SearchArray.Length - 1) = item
                        End If
                    Next
                End If
            End If

            If DateRadioButton.Checked And AscendRadioButton.Checked Then
                Dim NewArray = From thing In SearchArray
                               Order By thing.Date Descending
                SearchArray = NewArray.ToArray()
            ElseIf DateRadioButton.Checked And Not AscendRadioButton.Checked Then
                Dim NewArray = From thing In SearchArray
                               Order By thing.Date Ascending
                SearchArray = NewArray.ToArray()
            ElseIf NameRadioButton.Checked And Not AscendRadioButton.Checked Then
                Dim NewArray = From thing In SearchArray
                               Order By thing.Name Descending
                SearchArray = NewArray.ToArray()
            ElseIf NameRadioButton.Checked And Not AscendRadioButton.Checked Then
                Dim NewArray = From thing In SearchArray
                               Order By thing.Name Ascending
                SearchArray = NewArray.ToArray()
            Else
                Dim NewArray = From thing In SearchArray
                               Order By thing.Date Ascending
                SearchArray = NewArray.ToArray()
            End If

            Redraw(SearchArray)
        Catch ex As Exception
            ErrorLog(ex.Message)
        End Try

        SearchBusy = False
    End Sub

    Private Sub TbSecurity_KeyPress(sender As System.Object, e As System.EventArgs) Handles TextBox1.KeyUp

        Search()

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As EventArgs) Handles Timer1.Tick

        If WebThread.IsAlive Then Return
        Timer1.Stop()
        Search()

    End Sub

#End Region

End Class

Public Class JSONResult
    Private _author As String
    Private _cache As Image
    Private _copyright As String
    Private _date As String
    Private _exclusive As String
    Private _license As String
    Private _name As String
    Private _photo As String
    Private _size As String
    Private _str As String

#End Region

#Region "Properties"

    Public Property [Date] As String
        Get
            If _date Is Nothing Then Return ""
            Return _date
        End Get
        Set(value As String)
            _date = value
        End Set
    End Property

    Public Property Author As String
        Get
            If _author Is Nothing Then Return ""
            Return _author
        End Get
        Set(value As String)
            _author = value
        End Set
    End Property

    Public Property Cache As Image
        Get
            Return _cache
        End Get
        Set(value As Image)
            _cache = value
        End Set
    End Property

    Public Property CCBY As String
        Get
            If _copyright Is Nothing Then Return ""
            Return _copyright
        End Get
        Set(value As String)
            _copyright = value
        End Set
    End Property

    Public Property Exclusive As String
        Get
            If _exclusive Is Nothing Then Return "no"
            Return _exclusive
        End Get
        Set(value As String)
            _exclusive = value
        End Set
    End Property

    Public Property License As String
        Get
            'Return _license
            If _license Is Nothing Then Return ""
            _license = Mid(_license, 1, 1024)
            Dim i As List(Of String) = Word_Wrap.WrapText(_license, 70)
            Dim str As String = ""
            For Each line In i
                str += line & vbCrLf
                If str.Length >= 1024 Then
                    Exit For
                End If
            Next
            Return str
        End Get
        Set(value As String)
            _license = value
        End Set
    End Property

    Public Property Name As String
        Get
            If _name Is Nothing Then Return ""
            Return _name
        End Get
        Set(value As String)
            _name = value
        End Set
    End Property

    Public Property Photo As String
        Get
            If _photo Is Nothing Then Return ""
            Return _photo
        End Get
        Set(value As String)
            _photo = value
        End Set
    End Property

    Public Property Size As String
        Get
            If _license Is Nothing Then Return "0"
            Return _size
        End Get
        Set(value As String)
            _size = value
        End Set
    End Property

    Public Property Str As String
        Get
            If _str Is Nothing Then Return ""
            Return _str
        End Get
        Set(value As String)
            _str = value
        End Set
    End Property

End Class

Public Class FileData
    Private _name As String = ""
    Private _result As MsgBoxResult = MsgBoxResult.No
    Private _text As String = ""

    Public Property Name As String
        Get
            Return _name
        End Get
        Set(value As String)
            _name = value
        End Set
    End Property

    Public Property Result As MsgBoxResult
        Get
            Return _result
        End Get
        Set(value As MsgBoxResult)
            _result = value
        End Set
    End Property

    Public Property Text As String
        Get
            Return _text
        End Get
        Set(value As String)
            _text = value
        End Set
    End Property

End Class

#End Region
