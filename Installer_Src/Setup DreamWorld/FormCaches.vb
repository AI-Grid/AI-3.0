#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.Text.RegularExpressions

Public Class FormCaches

#Region "Private Fields"

    Private gInitted As Boolean

#End Region

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

    Private Sub B_Click(sender As Object, e As EventArgs) Handles Button1.Click

        PictureBox.Visible = True

        If GridCacheCheckbox.Checked Then
            DeleteOnlineUsers()
        End If

        If ScriptCheckBox1.Checked Then
            WipeScripts(False)
        End If

        If AvatarCheckBox2.Checked Then
            WipeBakes()
        End If

        If AssetCheckBox3.Checked Then
            WipeAssets()
        End If

        If ImageCheckBox4.Checked Then
            WipeImage()
        End If

        If MeshCheckBox5.Checked Then
            WipeMesh()
        End If

        If Not PropOpensimIsRunning() Then
            TextPrint(My.Resources.All_Caches_Cleared_word)
        Else
            TextPrint(My.Resources.Cache_Most_Cleared)
        End If

        PictureBox.Visible = False
        Me.Close()

    End Sub

    Private Sub CacheEnabledBox_CheckedChanged(sender As Object, e As EventArgs)
        If Not gInitted Then Return

    End Sub

    Private Sub CacheTimeout_TextChanged(sender As Object, e As EventArgs)
        If Not gInitted Then Return
        Dim digitsOnly = New Regex("[^\d\.]")
        CacheTimeout.Text = digitsOnly.Replace(CacheTimeout.Text, "")

    End Sub

    Private Sub Form_unload() Handles Me.Closing

        Settings.CacheLogLevel = LogLevelBox.SelectedIndex.ToString(Globalization.CultureInfo.InvariantCulture)
        Settings.CacheFolder = CacheFolder.Text
        Settings.CacheTimeout = CacheTimeout.Text

        Settings.SaveSettings()

    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Button1.Text = Global.Outworldz.My.Resources.Clear_Selected_Caches_word
        ScriptCheckBox1.Text = Global.Outworldz.My.Resources.Script_cache_word
        AvatarCheckBox2.Text = Global.Outworldz.My.Resources.Avatar_Bakes_Cache_word
        AssetCheckBox3.Text = Global.Outworldz.My.Resources.Asset_Cache_word
        ImageCheckBox4.Text = Global.Outworldz.My.Resources.Image_Cache_word
        MeshCheckBox5.Text = Global.Outworldz.My.Resources.Mesh_Cache_word
        GridUsers.Text = Global.Outworldz.My.Resources.Choose_Cache ' "Choose which cache to empty"
        GroupBox2.Text = Global.Outworldz.My.Resources.Asset_Cache_word

        HelpToolStripMenuItem.Image = Global.Outworldz.My.Resources.question_and_answer
        HelpToolStripMenuItem.Text = Global.Outworldz.My.Resources.Help_word
        Label1.Text = Global.Outworldz.My.Resources.Cache_Directory_word
        Label2.Text = Global.Outworldz.My.Resources.Log_Level

        Label5.Text = Global.Outworldz.My.Resources.Timeout_in_hours_word
        LogLevelBox.Items.AddRange(New Object() {Global.Outworldz.My.Resources.ErrorLevel0, Global.Outworldz.My.Resources.ErrorLevel1, Global.Outworldz.My.Resources.ErrorLevel2})
        PictureBox1.BackgroundImage = Global.Outworldz.My.Resources.folder
        Text = Global.Outworldz.My.Resources.Cache_Control_word

        ToolTip1.SetToolTip(CacheTimeout, Global.Outworldz.My.Resources.Timeout_in_hours_word)
        PictureBox.Visible = False

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

        gInitted = True

        HelpOnce("Cache")
    End Sub

    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.Click
        HelpManual("Cache")
        HelpManual("Flotsam Cache")
    End Sub

    Private Sub PictureBox1_Click_1(sender As Object, e As EventArgs) Handles PictureBox1.Click

        'Create an instance of the open file dialog box.
        Dim openFileDialog1 = New FolderBrowserDialog With {
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

            End If
        End If

    End Sub

#End Region

End Class
