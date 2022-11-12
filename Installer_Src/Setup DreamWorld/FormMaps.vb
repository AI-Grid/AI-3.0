#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.Text.RegularExpressions

Public Class FormMaps

#Region "Private Fields"

    Private ReadOnly Handler As New EventHandler(AddressOf Resize_page)
    Private _screenPosition As ClassScreenpos
    Private initted As Boolean

#End Region

#Region "Public Properties"

    Public Property ScreenPosition As ClassScreenpos
        Get
            Return _screenPosition
        End Get
        Set(value As ClassScreenpos)
            _screenPosition = value
        End Set
    End Property

#End Region

#Region "Private Methods"

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles DelMapButton.Click

        TextPrint(My.Resources.Clearing_Map_tiles_word)
        DelRobustMaps()
        Dim f As String = Settings.OpensimBinPath & "Maptiles\00000000-0000-0000-0000-000000000000"
        Try
            DeleteDirectory(f, FileIO.DeleteDirectoryOption.DeleteAllContents)
            My.Computer.FileSystem.CreateDirectory(f)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

        TextPrint(My.Resources.Maps_Erased)

    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles SmallMapButton.Click
        Dim webAddress As String = "http://" & Settings.PublicIP & ":" & CStr(Settings.ApachePort) & "/Metromap/index.php"
        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try
    End Sub

    Private Sub Button1_Click_2(sender As Object, e As EventArgs) Handles ViewVisitorMapButton.Click

        Dim webAddress As String
        If Settings.PublicVisitorMaps Then
            webAddress = $"http://{Settings.LANIP}:{CStr(Settings.ApachePort)}/Stats?r={Random()}"
        Else
            webAddress = $"http://127.0.0.1:{CStr(Settings.ApachePort)}/Stats"
        End If

        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles ViewRegionMapsButton.Click

        Dim X As Integer = Settings.MapCenterX
        Dim Y As Integer = Settings.MapCenterY

        Dim webAddress As String = "http://" & CStr(Settings.PublicIP) & ":" & CStr(Settings.HttpPort) & "/wifi/map.html?X=" & CStr(X - 10) & "&Y=" & CStr(Y + 5)

        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try
    End Sub

    Private Sub DeleteAMapButton_Click(sender As Object, e As EventArgs) Handles DeleteAMapButton.Click

        Dim regionname = ChooseRegion()
        ' check it
        Dim RegionUUID As String = FindRegionByName(regionname)
        If RegionUUID.Length = 0 Then Return

        DeleteMapTile(RegionUUID)
        TextPrint($"{regionname} {My.Resources.maphasbeendeleted}")

    End Sub

    Private Sub ExportAllMaps_Click(sender As Object, e As EventArgs) Handles ExportAllMaps.Click

        'export-map [<path>] - Save an image of the world map (default name is exportmap.jpg)

        'Create an instance of the open file dialog box.
        Using openFileDialog1 = New FolderBrowserDialog With {
            .ShowNewFolderButton = True,
            .Description = Global.Outworldz.My.Resources.Choose_a_Folder_word
        }
            Dim UserClickedOK As DialogResult = openFileDialog1.ShowDialog
            ' Process input if the user clicked OK.
            If UserClickedOK = DialogResult.OK Then
                Dim thing = openFileDialog1.SelectedPath
                If thing.Length > 0 Then
                    For Each RegionUUID In RegionUuids()

                        If RegionStatus(RegionUUID) = SIMSTATUSENUM.Booted Then
                            thing = IO.Path.Combine(thing, Region_Name(RegionUUID))
                            thing += ".jpg"
                            TextPrint($"{ Region_Name(RegionUUID)} map exported")
                            RPC_Region_Command(RegionUUID, $"export-map ""{thing}""")
                            Application.DoEvents()
                        End If
                    Next
                End If
            End If
        End Using

    End Sub

    Private Sub IsClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed

        Settings.SaveSettings()

    End Sub

    Private Sub LargeMapButton_Click(sender As Object, e As EventArgs) Handles LargeMapButton.Click
        Dim webAddress As String = "http://" + Settings.PublicIP & ":" & Settings.ApachePort & "/Metromap/indexmax.php"
        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try
    End Sub

    Private Sub Loaded(sender As Object, e As EventArgs) Handles Me.Load

        ViewRegionMapsButton.Text = Global.Outworldz.My.Resources.View_Maps
        GroupBox2.Text = Global.Outworldz.My.Resources.Maps_word
        Label1.Text = Global.Outworldz.My.Resources.Map_Center_Location_word
        Label2.Text = Global.Outworldz.My.Resources.X
        Label3.Text = Global.Outworldz.My.Resources.Y
        Label4.Text = Global.Outworldz.My.Resources.RenderMax
        Label5.Text = Global.Outworldz.My.Resources.RenderMin
        LargeMapButton.Text = Global.Outworldz.My.Resources.LargeMap
        MapBest.Text = Global.Outworldz.My.Resources.Best_Prims
        MapBetter.Text = Global.Outworldz.My.Resources.Better_Prims
        MapBox.Text = Global.Outworldz.My.Resources.Maps_word
        MapGood.Text = Global.Outworldz.My.Resources.Good_Warp3D_word
        ViewVisitorMapButton.Text = Global.Outworldz.My.Resources.ViewVisitorMaps
        K4Days.Text = Global.Outworldz.My.Resources.Keep_for_Days_word
        MapNone.Text = Global.Outworldz.My.Resources.None
        MapSimple.Text = Global.Outworldz.My.Resources.Simple_but_Fast_word
        MenuStrip2.Text = Global.Outworldz.My.Resources._0
        SmallMapButton.Text = Global.Outworldz.My.Resources.Small_Map
        Text = Global.Outworldz.My.Resources.Maps_word
        ToolStripMenuItem30.Image = Global.Outworldz.My.Resources.question_and_answer
        ToolStripMenuItem30.Text = Global.Outworldz.My.Resources.Help_word
        ToolTip1.SetToolTip(ViewRegionMapsButton, Global.Outworldz.My.Resources.WifiMap)
        ToolTip1.SetToolTip(MapXStart, Global.Outworldz.My.Resources.CenterMap)
        ToolTip1.SetToolTip(MapYStart, Global.Outworldz.My.Resources.CenterMap)
        ToolTip1.SetToolTip(RenderMaxH, Global.Outworldz.My.Resources.Max4096)
        ToolTip1.SetToolTip(DelMapButton, Global.Outworldz.My.Resources.Regen_Map)
        DelMapButton.Text = Global.Outworldz.My.Resources.DelMaps
        VieweAllMaps.Text = Global.Outworldz.My.Resources.ViewAllMaps
        ExportAllMaps.Text = Global.Outworldz.My.Resources.ExportAllMaps

        PublicMapsCheckbox.Checked = Settings.PublicVisitorMaps

        If Settings.ApacheEnable Then
            VisitorGroup.Enabled = True
            ApacheRunning.Text = ""
        Else
            VisitorGroup.Enabled = False
            ApacheRunning.Text = My.Resources.Apache_Disabled
        End If

        If Settings.MapType = "None" Then
            MapNone.Checked = True
            MapPicture.Image = Global.Outworldz.My.Resources.blankbox
        ElseIf Settings.MapType = "Simple" Then
            MapSimple.Checked = True
            MapPicture.Image = Global.Outworldz.My.Resources.Simple
        ElseIf Settings.MapType = "Good" Then
            MapGood.Checked = True
            MapPicture.Image = Global.Outworldz.My.Resources.Good
        ElseIf Settings.MapType = "Better" Then
            MapBetter.Checked = True
            MapPicture.Image = Global.Outworldz.My.Resources.Better
        ElseIf Settings.MapType = "Best" Then
            MapBest.Checked = True
            MapPicture.Image = Global.Outworldz.My.Resources.Best
        Else
            MapNone.Checked = True
            MapPicture.Image = Global.Outworldz.My.Resources.blankbox
        End If

        If IsMySqlRunning() Then
            ViewRegionMapsButton.Enabled = True
        Else
            ViewRegionMapsButton.Enabled = False
        End If

        If Settings.ApacheEnable Then
            MapYStart.Text = CStr(Settings.MapCenterY)
            MapXStart.Text = CStr(Settings.MapCenterX)
            MapYStart.Enabled = True
            MapXStart.Enabled = True
            LargeMapButton.Enabled = True
            SmallMapButton.Enabled = True
        Else
            MapYStart.Enabled = False
            MapXStart.Enabled = False
            LargeMapButton.Enabled = False
            SmallMapButton.Enabled = False
        End If

        RenderMaxH.Text = CStr(Settings.RenderMaxHeight)
        RenderMinH.Text = CStr(Settings.RenderMinHeight)

        Days2KeepBox.Text = CStr(Settings.KeepVisits)

        DelMapButton.Visible = True

        initted = True

        HelpOnce("Maps")
        SetScreen()

    End Sub

    Private Sub MapBest_CheckedChanged(sender As Object, e As EventArgs) Handles MapBest.CheckedChanged

        If Not initted Then Return
        Settings.MapType = "Best"
        MapPicture.Image = Global.Outworldz.My.Resources.Best

    End Sub

    Private Sub MapBetter_CheckedChanged(sender As Object, e As EventArgs) Handles MapBetter.CheckedChanged

        If Not initted Then Return
        Settings.MapType = "Better"
        MapPicture.Image = Global.Outworldz.My.Resources.Better

    End Sub

    Private Sub MapGood_CheckedChanged(sender As Object, e As EventArgs) Handles MapGood.CheckedChanged

        If Not initted Then Return
        Settings.MapType = "Good"
        MapPicture.Image = Global.Outworldz.My.Resources.Good

    End Sub

    Private Sub MapNone_CheckedChanged(sender As Object, e As EventArgs) Handles MapNone.CheckedChanged

        If Not initted Then Return
        Settings.MapType = "None"
        MapPicture.Image = Global.Outworldz.My.Resources.blankbox

    End Sub

    Private Sub MapSimple_CheckedChanged(sender As Object, e As EventArgs) Handles MapSimple.CheckedChanged

        If Not initted Then Return
        Settings.MapType = "Simple"
        MapPicture.Image = Global.Outworldz.My.Resources.Simple

    End Sub

    Private Sub MapXStart_TextChanged(sender As Object, e As EventArgs) Handles MapXStart.TextChanged

        If Not initted Then Return
        Dim digitsOnly = New Regex("[^\d]")
        MapXStart.Text = digitsOnly.Replace(MapXStart.Text, "")
        If Not Integer.TryParse(MapXStart.Text, Settings.MapCenterX) Then
            MsgBox(My.Resources.Must_be_A_Number, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground)
        End If

    End Sub

    Private Sub MapYStart_TextChanged(sender As Object, e As EventArgs) Handles MapYStart.TextChanged

        If Not initted Then Return
        Dim digitsOnly = New Regex("[^\d]")
        MapYStart.Text = digitsOnly.Replace(MapYStart.Text, "")
        If Not Integer.TryParse(MapYStart.Text, Settings.MapCenterY) Then
            MsgBox(My.Resources.Must_be_A_Number, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground)
        End If

    End Sub

    Private Sub PublicMapsCheckbox_CheckedChanged(sender As Object, e As EventArgs) Handles PublicMapsCheckbox.CheckedChanged

        If Not initted Then Return
        Settings.PublicVisitorMaps = PublicMapsCheckbox.Checked
        Settings.SaveSettings()

    End Sub

    Private Sub RenderMaxH_TextChanged(sender As Object, e As EventArgs) Handles RenderMaxH.TextChanged

        If Not initted Then Return
        Dim digitsOnly = New Regex("[^-\d]")
        RenderMaxH.Text = digitsOnly.Replace(RenderMaxH.Text, "")
        If Not Double.TryParse(RenderMaxH.Text, Settings.RenderMaxHeight) Then
            MsgBox(My.Resources.Must_be_A_Number, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground)
        End If

    End Sub

    Private Sub RenderMinH_TextChanged(sender As Object, e As EventArgs) Handles RenderMinH.TextChanged

        If Not initted Then Return
        Dim digitsOnly = New Regex("[^-\d]")
        RenderMinH.Text = digitsOnly.Replace(RenderMinH.Text, "")
        If Not Integer.TryParse(RenderMinH.Text, Settings.RenderMinHeight) Then
            MsgBox(My.Resources.Must_be_A_Number, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground)
        End If

    End Sub

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

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles Days2KeepBox.TextChanged

        If Not initted Then Return
        Dim digitsOnly = New Regex("[^\d]")
        Days2KeepBox.Text = digitsOnly.Replace(Days2KeepBox.Text, "")

        If Not Integer.TryParse(Days2KeepBox.Text, Settings.KeepVisits) Then
            MsgBox(My.Resources.Must_be_A_Number, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground)
            Settings.SaveSettings()
        End If
    End Sub

    Private Sub ToolStripMenuItem30_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem30.Click
        HelpManual("Maps")
    End Sub

    Private Sub VieweAllMaps_Click(sender As Object, e As EventArgs) Handles VieweAllMaps.Click

        For Each RegionUUID In RegionUuids()
            VarChooser(Region_Name(RegionUUID), False, False)
            Application.DoEvents()
        Next

    End Sub

#End Region

End Class
