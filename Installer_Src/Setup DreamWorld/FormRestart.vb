﻿#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.Text.RegularExpressions

Public Class FormRestart

#Region "Private Fields"

    Private ReadOnly Handler As New EventHandler(AddressOf Resize_page)
    Private _screenPosition As ClassScreenpos
    Dim initted As Boolean

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

#Region "Load and Close"

    Private Sub IsClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed

        Settings.AutoRestartEnabled = ARTimerBox.Checked
        Settings.Autostart = AutoStartCheckbox.Checked
        Try
            Settings.AutoRestartInterval = Convert.ToInt16(AutoRestartBox.Text, Globalization.CultureInfo.InvariantCulture)
        Catch ex As Exception
            BreakPoint.DUmp(ex)
            Settings.AutoRestartInterval = 0
        End Try

        Settings.RestartOnCrash = RestartOnCrash.Checked
        Settings.Sequential = SequentialCheckBox1.Checked

        Settings.SaveSettings()

    End Sub

    Private Sub Loaded(sender As Object, e As EventArgs) Handles Me.Load

        ARTimerBox.Text = Global.Outworldz.My.Resources.Restart_Periodically_word
        AutoStart.Text = Global.Outworldz.My.Resources.Auto_Startup_word
        AutoStartCheckbox.Text = Global.Outworldz.My.Resources.EnableOneClickStart_word
        Label25.Text = Global.Outworldz.My.Resources.Restart_Interval
        MenuStrip2.Text = Global.Outworldz.My.Resources._0
        RestartOnCrash.Text = Global.Outworldz.My.Resources.Restart_On_Crash
        SequentialCheckBox1.Text = Global.Outworldz.My.Resources.StartSequentially
        Text = Global.Outworldz.My.Resources.Restart_word
        ToolStripMenuItem30.Image = Global.Outworldz.My.Resources.question_and_answer
        ToolStripMenuItem30.Text = Global.Outworldz.My.Resources.Help_word
        ToolTip1.SetToolTip(ARTimerBox, Global.Outworldz.My.Resources.Restart_Periodically_Minutes)
        ToolTip1.SetToolTip(AutoRestartBox, Global.Outworldz.My.Resources.AutorestartBox)
        ToolTip1.SetToolTip(AutoStartCheckbox, Global.Outworldz.My.Resources.StartLaunch)
        ToolTip1.SetToolTip(RestartOnCrash, Global.Outworldz.My.Resources.Restart_On_Crash)
        ToolTip1.SetToolTip(SequentialCheckBox1, Global.Outworldz.My.Resources.Sequentially_text)

        AutoRestartBox.Text = Settings.AutoRestartInterval.ToString(Globalization.CultureInfo.InvariantCulture)
        ARTimerBox.Checked = Settings.AutoRestartEnabled
        AutoStartCheckbox.Checked = Settings.Autostart
        SequentialCheckBox1.Checked = Settings.Sequential
        RestartOnCrash.Checked = Settings.RestartOnCrash

        SetScreen()
        HelpOnce("Restart")

        initted = True ' suppress the install of the startup on formload

    End Sub

#End Region

#Region "SetScreen"

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

#Region "AutoStart"

    Private Sub ARTimerBox_CheckedChanged(sender As Object, e As EventArgs) Handles ARTimerBox.CheckedChanged

        If Not initted Then Return
        If ARTimerBox.Checked Then
            Dim BTime As Int16 = CType(Settings.AutobackupInterval, Int16)
            If Settings.AutoBackup And Settings.AutoRestartInterval > 0 And Settings.AutoRestartInterval < BTime Then
                Settings.AutoRestartInterval = BTime + 30
                AutoRestartBox.Text = (BTime + 30).ToString(Globalization.CultureInfo.InvariantCulture)
                MsgBox(My.Resources.Increasing_time_to_word & " " & BTime.ToString(Globalization.CultureInfo.InvariantCulture) & " + 30 Minutes for Autobackup to complete.", vbInformation Or MsgBoxStyle.MsgBoxSetForeground)
            End If
        Else
            Settings.AutoRestartEnabled = False
            Settings.AutoRestartInterval = 0
        End If
        Settings.SaveSettings()

    End Sub

    Private Sub AutoRestartBox_TextChanged(sender As Object, e As EventArgs) Handles AutoRestartBox.TextChanged

        If Not initted Then Return
        Dim digitsOnly = New Regex("[^\d]")
        AutoRestartBox.Text = digitsOnly.Replace(AutoRestartBox.Text, "")

    End Sub

    Private Sub RunOnBoot_Click_1(sender As Object, e As EventArgs)

        HelpManual("Restart")

    End Sub

    Private Sub ToolStripMenuItem30_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem30.Click
        HelpManual("Restart")
    End Sub

#End Region

End Class
