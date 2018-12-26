﻿Public Class FormBackups

#Region "FormPos"


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

#Region "Load"

#End Region
    Private Sub Loaded(sender As Object, e As EventArgs) Handles Me.Load

        AutoBackupKeepFilesForDays.Text = Form1.MySetting.KeepForDays.ToString

        '0 = Hourly
        '1 = 12 Hour
        '2 = Daily
        '3 = 2 days
        '4 = 3 days
        '5 = 4 days
        '6 = 6 days
        '7 = 7 days
        '8 = Weekly
        ' default= 1

        If CType(Form1.MySetting.AutobackupInterval, Double) = 60 Then
            AutoBackupInterval.SelectedIndex = 0
        ElseIf CType(Form1.MySetting.AutobackupInterval, Double) = 12 * 60 Then
            AutoBackupInterval.SelectedIndex = 1
        ElseIf CType(Form1.MySetting.AutobackupInterval, Double) = 24 * 60 Then
            AutoBackupInterval.SelectedIndex = 2
        ElseIf CType(Form1.MySetting.AutobackupInterval, Double) = 2 * 24 * 60 Then
            AutoBackupInterval.SelectedIndex = 3
        ElseIf CType(Form1.MySetting.AutobackupInterval, Double) = 3 * 24 * 60 Then
            AutoBackupInterval.SelectedIndex = 4
        ElseIf CType(Form1.MySetting.AutobackupInterval, Double) = 4 * 24 * 60 Then
            AutoBackupInterval.SelectedIndex = 5
        ElseIf CType(Form1.MySetting.AutobackupInterval, Double) = 5 * 24 * 60 Then
            AutoBackupInterval.SelectedIndex = 6
        ElseIf CType(Form1.MySetting.AutobackupInterval, Double) = 6 * 24 * 60 Then
            AutoBackupInterval.SelectedIndex = 8
        ElseIf CType(Form1.MySetting.AutobackupInterval, Double) = 7 * 60 Then
            AutoBackupInterval.SelectedIndex = 8
        Else
            AutoBackupInterval.SelectedIndex = 1
        End If

        BackupFolder.Text = Form1.MySetting.BackupFolder
        AutoBackup.Checked = Form1.MySetting.AutoBackup

    End Sub

#Region "Changes"
    Private Sub ABEnabled_CheckedChanged(sender As Object, e As EventArgs) Handles AutoBackup.CheckedChanged

        Form1.MySetting.AutoBackup = AutoBackup.Checked
        Form1.MySetting.SaveSettings()
    End Sub

    Private Sub AutoBackupInterval_SelectedIndexChanged(sender As Object, e As EventArgs) Handles AutoBackupInterval.SelectedIndexChanged
        Dim text = AutoBackupInterval.SelectedItem.ToString()
        '0 = Hourly
        '1 = 12 Hour
        '2 = Daily
        '3 = 2 days
        '4 = 3 days
        '5 = 4 days
        '6 = 6 days
        '7 = 7 days
        '8 = Weekly
        ' default= 1

        Dim Interval As Integer
        If text = "Hourly" Then Interval = 60
        If text = "12 Hour" Then Interval = 60 * 12
        If text = "Daily" Then Interval = 60 * 24
        If text = "2 days" Then Interval = 2 * 60 * 24
        If text = "3 days" Then Interval = 3 * 60 * 24
        If text = "4 days" Then Interval = 4 * 60 * 24
        If text = "5 days" Then Interval = 5 * 60 * 24
        If text = "6 days" Then Interval = 6 * 60 * 24
        If text = "Weekly" Then Interval = 7 * 60 * 24
        Form1.MySetting.AutobackupInterval = Interval.ToString

        Form1.MySetting.SaveSettings()
    End Sub

    Private Sub AutoBackupKeepFilesForDays_TextChanged(sender As Object, e As EventArgs) Handles AutoBackupKeepFilesForDays.TextChanged

        Try
            If Convert.ToInt32(AutoBackupKeepFilesForDays.Text) > 0 Then
                Form1.MySetting.KeepForDays = Convert.ToInt32(AutoBackupKeepFilesForDays.Text)
                Form1.MySetting.SaveSettings()
            End If
        Catch
            MsgBox("Must be a number of days", vbInformation)
            Form1.MySetting.KeepForDays = 30
            Form1.MySetting.SaveSettings()
        End Try

    End Sub
    Private Sub BackupFolder_clicked(sender As Object, e As EventArgs) Handles BackupFolder.Click

        Backup()

    End Sub

#End Region

#Region "Help"

    Private Sub AutoBackupHelp_Click(sender As Object, e As EventArgs) Handles AutoBackupHelp.Click
        Dim webAddress As String = Form1.gDomain + "/Outworldz_installer/technical.htm#Backup"
        Process.Start(webAddress)
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Backup()
    End Sub

    Private Sub Backup()

        'Create an instance of the open file dialog box.
        Dim openFileDialog1 As FolderBrowserDialog = New FolderBrowserDialog

        openFileDialog1.ShowNewFolderButton = True
        openFileDialog1.Description = "Pick folder for backups"
        Dim UserClickedOK As DialogResult = openFileDialog1.ShowDialog

        ' Process input if the user clicked OK.
        If UserClickedOK = DialogResult.OK Then
            Dim thing = openFileDialog1.SelectedPath
            If thing.Length > 0 Then
                Form1.MySetting.BackupFolder = thing
                Form1.MySetting.SaveSettings()
                BackupFolder.Text = thing
            End If
        End If

    End Sub
#End Region


End Class