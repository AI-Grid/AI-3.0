#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Public Class FormBackupBoxes

    Private initted As Boolean

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

#Region "Backup Start "

    ''' <summary>
    ''' Backup Run Button
    ''' </summary>
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles BackupButton.Click

        If BackupAbort = True Then
            RunningBackupName.Clear()
            BackupAbort = False
        End If

        FormSetup.StartTimer()

        If Not RunningBackupName.IsEmpty Then
            Dim namelist As String = ""
            For Each N In RunningBackupName
                namelist += N.Key + ","
            Next

            Dim cancel = MsgBox($"{My.Resources.BusyWith} {namelist} {My.Resources.Cancel_word}? ", MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground, My.Resources.Busy_word)
            If cancel = vbYes Then
                BackupAbort = True
                RunningBackupName.Clear()
                TextPrint($"{My.Resources.Backup_word} {My.Resources.Aborted_word}")
                Return
            End If
            Me.Close()
        End If

        BackupButton.Text = My.Resources.Running_word
#Disable Warning CA2000 ' Dispose objects before losing scope
        Dim b As New Backups()
#Enable Warning CA2000 ' Dispose objects before losing scope

        ' Settings.ini, BackupW Wifi and Backup Region ini
        b.RunFullBackupThread() 'run backup right now instead of on a timer

        Me.Close()

    End Sub

#End Region

#Region "Load"

    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.Click

        HelpManual("Backup Manually")

    End Sub

    Private Sub Loaded(sender As Object, e As EventArgs) Handles Me.Load

        BackupButton.Text = Global.Outworldz.My.Resources.System_Backup_Now_word
        BackupIARsCheckBox.Text = Global.Outworldz.My.Resources.Backup_IARs
        BackupOarsCheckBox.Text = Global.Outworldz.My.Resources.Backup_OARs
        BackupSQlCheckBox.Text = Global.Outworldz.My.Resources.Backup_Mysql
        CustomCheckBox.Text = Global.Outworldz.My.Resources.Backup_Custom
        FSAssetsCheckBox.Text = Global.Outworldz.My.Resources.Backup_FSAssets
        GroupBox1.Text = Global.Outworldz.My.Resources.Backup_word
        HelpToolStripMenuItem.Image = Global.Outworldz.My.Resources.question_and_answer
        HelpToolStripMenuItem.Text = Global.Outworldz.My.Resources.Help_word
        RegionCheckBox.Text = Global.Outworldz.My.Resources.Backup_Region
        SettingsCheckbox.Text = Global.Outworldz.My.Resources.Backup_Settings_word
        ShowFsassets.Text = Global.Outworldz.My.Resources.Show_Status_word
        Me.Text = Global.Outworldz.My.Resources.System_Backup_word

        ' tool tips
        ToolTip1.SetToolTip(BackupButton, Global.Outworldz.My.Resources.tt_Backup_Now)
        ToolTip1.SetToolTip(BackupIARsCheckBox, Global.Outworldz.My.Resources.tt_Backup_IARs)
        ToolTip1.SetToolTip(BackupOarsCheckBox, Global.Outworldz.My.Resources.tt_Backup_OARs)
        ToolTip1.SetToolTip(BackupSQlCheckBox, Global.Outworldz.My.Resources.tt_Backup_SQL)
        ToolTip1.SetToolTip(CustomCheckBox, Global.Outworldz.My.Resources.tt_Backup_Custom)
        ToolTip1.SetToolTip(FSAssetsCheckBox, Global.Outworldz.My.Resources.tt_Backup_Fsassets)
        ToolTip1.SetToolTip(RegionCheckBox, Global.Outworldz.My.Resources.tt_Backup_Regions)
        ToolTip1.SetToolTip(SettingsCheckbox, Global.Outworldz.My.Resources.tt_Backup_Settings)
        ToolTip1.SetToolTip(ShowFsassets, Global.Outworldz.My.Resources.tt_Backup_Fsassets)

        HelpOnce("Backup Manually")

        If Settings.FSAssetsEnabled Then
            FSAssetsCheckBox.Enabled = True
            FSAssetsCheckBox.Checked = True
        Else
            FSAssetsCheckBox.Enabled = False
            FSAssetsCheckBox.Checked = False
            Settings.BackupFSAssets = False
        End If

        RegionCheckBox.Checked = Settings.BackupRegion
        SettingsCheckbox.Checked = Settings.BackupSettings
        FSAssetsCheckBox.Checked = Settings.BackupFSAssets
        CustomCheckBox.Checked = Settings.BackupWifi
        BackupOarsCheckBox.Checked = Settings.BackupOARs
        BackupIARsCheckBox.Checked = Settings.BackupIars
        BackupSQlCheckBox.Checked = Settings.BackupSQL

        initted = True

    End Sub

#End Region

#Region "CheckBoxes"

    Private Sub AbortButton_Click(sender As Object, e As EventArgs) Handles AbortButton.Click

        BackupAbort = True
        BackupButton.Text = Global.Outworldz.My.Resources.System_Backup_Now_word
        TextPrint($"{My.Resources.Backup_word} {My.Resources.Aborted_word}")

    End Sub

    Private Sub BackupSQlCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles BackupSQlCheckBox.CheckedChanged

        If Not initted Then Return
        Settings.BackupSQL = BackupSQlCheckBox.Checked
        Settings.SaveSettings()

    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles BackupOarsCheckBox.CheckedChanged

        If Not initted Then Return
        Settings.BackupOARs = BackupOarsCheckBox.Checked
        Settings.SaveSettings()

    End Sub

    Private Sub CheckBox1_CheckedChanged_1(sender As Object, e As EventArgs) Handles BackupIARsCheckBox.CheckedChanged

        If Not initted Then Return
        Settings.BackupIars = BackupIARsCheckBox.Checked
        Settings.SaveSettings()

    End Sub

    Private Sub CustomCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles CustomCheckBox.CheckedChanged

        If Not initted Then Return
        Settings.BackupWifi = CustomCheckBox.Checked
        Settings.SaveSettings()

    End Sub

    Private Sub FSAssetsCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles FSAssetsCheckBox.CheckedChanged

        If Not initted Then Return
        Settings.BackupFSAssets = FSAssetsCheckBox.Checked
        Settings.SaveSettings()

    End Sub

    Private Sub RegionCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles RegionCheckBox.CheckedChanged

        If Not initted Then Return
        Settings.BackupRegion = RegionCheckBox.Checked
        Settings.SaveSettings()

    End Sub

    Private Sub SettingsCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles SettingsCheckbox.CheckedChanged

        If Not initted Then Return
        Settings.BackupSettings = SettingsCheckbox.Checked
        Settings.SaveSettings()

    End Sub

    Private Sub ShowFsassets_CheckedChanged(sender As Object, e As EventArgs) Handles ShowFsassets.CheckedChanged

        If Not initted Then Return
        Settings.ShowFSAssetBackup = ShowFsassets.Checked
        Settings.SaveSettings()

    End Sub

#End Region

End Class
