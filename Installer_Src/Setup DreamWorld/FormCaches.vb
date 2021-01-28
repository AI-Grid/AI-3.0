#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

'Permission Is hereby granted, free Of charge, to any person obtaining a copy of this software
' And associated documentation files (the "Software"), to deal in the Software without restriction,
'including without limitation the rights To use, copy, modify, merge, publish, distribute, sublicense,
'And/Or sell copies Of the Software, And To permit persons To whom the Software Is furnished To
'Do so, subject To the following conditions:

'The above copyright notice And this permission notice shall be included In all copies Or '
'substantial portions Of the Software.

'THE SOFTWARE Is PROVIDED "AS IS", WITHOUT WARRANTY Of ANY KIND, EXPRESS Or IMPLIED,
' INCLUDING BUT Not LIMITED To THE WARRANTIES Of MERCHANTABILITY, FITNESS For A PARTICULAR
'PURPOSE And NONINFRINGEMENT.In NO Event SHALL THE AUTHORS Or COPYRIGHT HOLDERS BE LIABLE
'For ANY CLAIM, DAMAGES Or OTHER LIABILITY, WHETHER In AN ACTION Of CONTRACT, TORT Or
'OTHERWISE, ARISING FROM, OUT Of Or In CONNECTION With THE SOFTWARE Or THE USE Or OTHER
'DEALINGS IN THE SOFTWARE.Imports System

#End Region

Imports System.Text.RegularExpressions

Public Class FormCaches

#Region "Private Fields"

    Private gInitted As Boolean

#End Region

#Region "ScreenSize"

    Private _screenPosition As ScreenPos
    Private Handler As New EventHandler(AddressOf Resize_page)

    Public Property ScreenPosition As ScreenPos
        Get
            Return _screenPosition
        End Get
        Set(value As ScreenPos)
            _screenPosition = value
        End Set
    End Property

    'The following detects  the location of the form in screen coordinates
    Private Sub Resize_page(ByVal sender As Object, ByVal e As System.EventArgs)
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

#Region "Private Methods"

    Private Sub B_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If ScriptCheckBox1.Checked Then
            ClrCache.WipeScripts()
        End If

        If AvatarCheckBox2.Checked Then
            ClrCache.WipeBakes()
        End If

        If AssetCheckBox3.Checked Then
            ClrCache.WipeAssets()
        End If

        If ImageCheckBox4.Checked Then
            ClrCache.WipeImage()
        End If

        If MeshCheckBox5.Checked Then
            ClrCache.WipeMesh()
        End If

        If Not PropOpensimIsRunning() Then
            TextPrint(My.Resources.All_Caches_Cleared_word)
        Else
            TextPrint(My.Resources.Cache_Most_Cleared)
        End If

        Me.Close()

    End Sub

    Private Sub CacheEnabledBox_CheckedChanged(sender As Object, e As EventArgs) Handles CacheEnabledBox.CheckedChanged
        If Not gInitted Then Return
        FormSetup.PropViewedSettings = True
    End Sub

    Private Sub CacheTimeout_TextChanged(sender As Object, e As EventArgs)
        If Not gInitted Then Return
        Dim digitsOnly As Regex = New Regex("[^\d\.]")
        CacheTimeout.Text = digitsOnly.Replace(CacheTimeout.Text, "")
        FormSetup.PropViewedSettings = True
    End Sub

    Private Sub Form_unload() Handles Me.Closing

        Settings.CacheLogLevel = LogLevelBox.SelectedIndex.ToString(Globalization.CultureInfo.InvariantCulture)
        Settings.CacheFolder = CacheFolder.Text
        Settings.CacheEnabled = CacheEnabledBox.Checked
        Settings.CacheTimeout = CacheTimeout.Text
        Settings.SupportViewerObjectsCache = ViewerCacheCheckbox.Checked

        FormSetup.PropViewedSettings = True
        Settings.SaveSettings()

    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Button1.Text = Global.Outworldz.My.Resources.Clear_Selected_Caches_word
        ScriptCheckBox1.Text = Global.Outworldz.My.Resources.Script_cache_word
        AvatarCheckBox2.Text = Global.Outworldz.My.Resources.Avatar_Bakes_Cache_word
        AssetCheckBox3.Text = Global.Outworldz.My.Resources.Asset_Cache_word
        ImageCheckBox4.Text = Global.Outworldz.My.Resources.Image_Cache_word
        MeshCheckBox5.Text = Global.Outworldz.My.Resources.Mesh_Cache_word
        GroupBox1.Text = Global.Outworldz.My.Resources.Choose_Cache ' "Choose which cache to empty"
        GroupBox2.Text = Global.Outworldz.My.Resources.Asset_Cache_word
        GroupBox3.Text = Global.Outworldz.My.Resources.Viewer_Cache_word
        HelpToolStripMenuItem.Image = Global.Outworldz.My.Resources.question_and_answer
        HelpToolStripMenuItem.Text = Global.Outworldz.My.Resources.Help_word
        HelpToolStripMenuItem1.Image = Global.Outworldz.My.Resources.about
        HelpToolStripMenuItem1.Text = Global.Outworldz.My.Resources.Help_word
        Label1.Text = Global.Outworldz.My.Resources.Cache_Directory_word
        Label2.Text = Global.Outworldz.My.Resources.Log_Level
        Label4.Text = Global.Outworldz.My.Resources.Cache_Enabled_word
        Label5.Text = Global.Outworldz.My.Resources.Timeout_in_hours_word
        LogLevelBox.Items.AddRange(New Object() {Global.Outworldz.My.Resources.ErrorLevel0, Global.Outworldz.My.Resources.ErrorLevel1, Global.Outworldz.My.Resources.ErrorLevel2})
        MapHelp.Image = Global.Outworldz.My.Resources.about
        PictureBox1.BackgroundImage = Global.Outworldz.My.Resources.folder
        PictureBox2.Image = Global.Outworldz.My.Resources.about
        Text = Global.Outworldz.My.Resources.Cache_Control_word
        ToolTip1.SetToolTip(CacheEnabledBox, Global.Outworldz.My.Resources.Default_Checked_word)
        ToolTip1.SetToolTip(CacheTimeout, Global.Outworldz.My.Resources.Timeout_in_hours_word)
        ToolTip1.SetToolTip(MapHelp, Global.Outworldz.My.Resources.Click_For_Help)
        ToolTip1.SetToolTip(PictureBox2, Global.Outworldz.My.Resources.Click_For_Help)
        ToolTip1.SetToolTip(ViewerCacheCheckbox, Global.Outworldz.My.Resources.Viewer_Cache_text)
        ViewerCacheCheckbox.Text = Global.Outworldz.My.Resources.Enabled_word

        SetScreen()

        If Not PropOpensimIsRunning() Then
            ScriptCheckBox1.Enabled = True
            AvatarCheckBox2.Enabled = True

            ScriptCheckBox1.Checked = True
            AvatarCheckBox2.Checked = True
            AssetCheckBox3.Checked = True
            ImageCheckBox4.Checked = True
            MeshCheckBox5.Checked = True
        Else
            ScriptCheckBox1.Enabled = False
            AvatarCheckBox2.Enabled = False
            ScriptCheckBox1.Checked = False
            AvatarCheckBox2.Checked = False
            AssetCheckBox3.Checked = True
            ImageCheckBox4.Checked = True
            MeshCheckBox5.Checked = True
        End If
        HelpOnce("Flotsam Cache")

        CacheFolder.Text = Settings.CacheFolder
        CacheEnabledBox.Checked = Settings.CacheEnabled
        CacheTimeout.Text = Settings.CacheTimeout
        LogLevelBox.SelectedIndex = CType(Settings.CacheLogLevel, Integer)

        Dim tmp = CacheFolder.Text
        If tmp = ".\assetcache" Then
            tmp = Settings.OpensimBinPath & "assetcache"
        End If

        If Not IO.Directory.Exists(tmp) Then
            MsgBox(My.Resources.No_Locate_FSassets & tmp & Global.Outworldz.My.Resources.Reset_To_Default, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground)
            CacheFolder.Text = ".\assetcache".ToString(Globalization.CultureInfo.InvariantCulture)
        End If

        ViewerCacheCheckbox.Checked = Settings.SupportViewerObjectsCache

        gInitted = True

        HelpOnce("Cache")
    End Sub

    Private Sub HelpToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem1.Click
        HelpManual("Cache")
    End Sub

    Private Sub MapHelp_Click(sender As Object, e As EventArgs) Handles MapHelp.Click
        HelpManual("Cache")
    End Sub

    Private Sub PictureBox1_Click_1(sender As Object, e As EventArgs) Handles PictureBox1.Click

        'Create an instance of the open file dialog box.
        Dim openFileDialog1 As FolderBrowserDialog = New FolderBrowserDialog With {
            .ShowNewFolderButton = True,
            .Description = Global.Outworldz.My.Resources.Choose_a_Folder_word
        }
        Dim UserClickedOK As DialogResult = openFileDialog1.ShowDialog
        openFileDialog1.Dispose()
        ' Process input if the user clicked OK.
        If UserClickedOK = DialogResult.OK Then
            Dim thing = openFileDialog1.SelectedPath
            If thing.Length > 0 Then
                Settings.CacheFolder = thing
                Settings.SaveSettings()
                CacheFolder.Text = thing
                FormSetup.PropViewedSettings = True
            End If
        End If

    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        HelpManual("Flotsam Cache")
    End Sub

    Private Sub ViewerCacheCheckbox_CheckedChanged(sender As Object, e As EventArgs) Handles ViewerCacheCheckbox.CheckedChanged

        ' Support viewers object cache, default true Users may need to reduce viewer bandwidth if
        ' some prims Or terrain parts fail to rez. Change to false if you need to use old viewers
        ' that do not support this feature
        Settings.SupportViewerObjectsCache = ViewerCacheCheckbox.Checked

    End Sub

#End Region

End Class
