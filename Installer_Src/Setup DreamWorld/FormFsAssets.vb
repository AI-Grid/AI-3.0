#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Public Class FormFSAssets

#Region "Declarations"

    Dim _changed As Boolean
    Dim initted As Boolean

#End Region

#Region "ScreenSize"

    Private ReadOnly Handler As New EventHandler(AddressOf Resize_page)

    'The following detects  the location of the form in screen coordinates
    Private _screenPosition As ClassScreenpos

    Public Property ScreenPosition As ClassScreenpos
        Get
            Return _screenPosition
        End Get
        Set(value As ClassScreenpos)
            _screenPosition = value
        End Set
    End Property

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

    Public Property Changed As Boolean
        Get
            Return _changed
        End Get
        Set(value As Boolean)
            _changed = value
        End Set
    End Property

    Public Property Initted1 As Boolean
        Get
            Return initted
        End Get
        Set(value As Boolean)
            initted = value
        End Set
    End Property

#End Region

#Region "LoadSave"

    Private Sub Form_exit() Handles Me.Closed

        If Changed Then

            Dim result As MsgBoxResult = MsgBox(My.Resources.Changes_Made, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground)
            If result = vbOK Then

                Settings.SaveSettings()
            End If
        End If

    End Sub

    Private Sub Loaded(sender As Object, e As EventArgs) Handles Me.Load

        EnableFsAssetsCheckbox.Text = Global.Outworldz.My.Resources.Enable_word
        HelpToolStripMenuItem.Image = Global.Outworldz.My.Resources.question_and_answer
        HelpToolStripMenuItem.Text = Global.Outworldz.My.Resources.Help_word
        Label6.Text = Global.Outworldz.My.Resources.Data_Folder_word
        PictureBox2.BackgroundImage = Global.Outworldz.My.Resources.folder
        SaveButton.Text = Global.Outworldz.My.Resources.Save_word
        ShowStatsCheckBox.Text = Global.Outworldz.My.Resources.Show_Stats
        Text = Global.Outworldz.My.Resources.FSassets_Server_word
        FSAssetsGroupBox.Text = Global.Outworldz.My.Resources.FSassets_Server_word

        HelpOnce("FSAssets")

        EnableFsAssetsCheckbox.Checked = Settings.FSAssetsEnabled
        DataFolder.Text = Settings.BaseDirectory
        ShowStatsCheckBox.Checked = CType(Settings.ShowConsoleStats, Boolean)
        SetScreen()
        Initted1 = True

    End Sub

#End Region

#Region "Subs"

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles EnableFsAssetsCheckbox.CheckedChanged
        If Not initted Then Return
        Settings.FSAssetsEnabled = EnableFsAssetsCheckbox.Checked
        Changed = True
    End Sub

    Private Sub CheckBox1_CheckedChanged_1(sender As Object, e As EventArgs) Handles ShowStatsCheckBox.CheckedChanged

        If Not initted Then Return
        Settings.ShowConsoleStats = ShowStatsCheckBox.Checked.ToString(Globalization.CultureInfo.InvariantCulture)
        Changed = True

    End Sub

    Private Sub DataFolder_TextChanged(sender As Object, e As EventArgs) Handles DataFolder.TextChanged
        If Not initted Then Return
        Changed = True
    End Sub

    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.Click
        HelpManual("FSAssets")
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click

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
                Settings.BaseDirectory = thing
                Settings.SaveSettings()
                DataFolder.Text = thing
                Changed = True
            End If
        End If

    End Sub

    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click

        Settings.SaveSettings()
        Me.Close()
    End Sub

#End Region

End Class
