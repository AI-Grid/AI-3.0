﻿Imports System.IO

Public Class TosForm

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

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Load file MyFolder + "TOS.txt"
        Dim reader As System.IO.StreamReader
        reader = System.IO.File.OpenText(Form1.MyFolder + "\tos.html")
        'now loop through each line
        Dim HTML As String = ""
        While reader.Peek <> -1
            HTML = HTML + reader.ReadLine() + vbCrLf
        End While
        reader.Close()
        Editor1.BodyHtml = HTML

        ShowToLocalUsersCheckbox.Checked = Form1.MySetting.ShowToLocalUsers
        ShowToHGUsersCheckbox.Checked = Form1.MySetting.ShowToForeignUsers
        TOSEnable.Checked = Form1.MySetting.TOSEnabled
        SetScreen()

    End Sub
    Private Sub Form1_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed

        'nothing

    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles ShowToLocalUsersCheckbox.CheckedChanged

        Form1.MySetting.ShowToLocalUsers = ShowToLocalUsersCheckbox.Checked
        Form1.MySetting.SaveSettings()

    End Sub

    Private Sub ShowToHGUsersCheckbox_CheckedChanged(sender As Object, e As EventArgs) Handles ShowToHGUsersCheckbox.CheckedChanged

        Form1.MySetting.ShowToForeignUsers = ShowToHGUsersCheckbox.Checked
        Form1.MySetting.SaveSettings()

    End Sub

    Private Sub TOSEnable_CheckedChanged(sender As Object, e As EventArgs) Handles TOSEnable.CheckedChanged

        Form1.MySetting.TOSEnabled = TOSEnable.Checked
        Form1.MySetting.SaveSettings()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If Form1.OpensimIsRunning() Then
            Dim webAddress As String = "http://" + Form1.MySetting.PublicIP + ":" + Form1.MySetting.HttpPort + "/wifi/termsofservice.html"
            Process.Start(webAddress)
        Else
            MsgBox("Opensim must be running to show you the TOS.")
        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Dim response = MsgBox("Clicking Yes will force all users to re-agree to the TOS on next login or visit.", vbYesNo)
        If response = vbYes Then
            Dim m As New Mysql(Form1.gRobustConnStr)
            If m.IsMySqlRunning() Is Nothing Then
                MsgBox("MySql is not running, so I cannot save the re-validate data. Start Opensim or Mysql and try again.")
            Else
                m.QueryString("update opensim.griduser set TOS = '';")
            End If
        End If

    End Sub

    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click

        Using outputFile As New StreamWriter(Form1.MyFolder + "\tos.html")
            outputFile.WriteLine(Editor1.BodyHtml)
        End Using

        Using outputFile As New StreamWriter(Form1.MyFolder + "\Outworldzfiles\opensim\bin\WifiPages\tos.html")
            outputFile.WriteLine(Editor1.BodyHtml)
        End Using

        Me.Close()

    End Sub

    Private Sub ApplyButton_Click(sender As Object, e As EventArgs) Handles ApplyButton.Click

        Using outputFile As New StreamWriter(Form1.MyFolder + "\tos.html")
            outputFile.WriteLine(Editor1.BodyHtml)
        End Using

        Using outputFile As New StreamWriter(Form1.MyFolder + "\Outworldzfiles\opensim\bin\WifiPages\tos.html")
            outputFile.WriteLine(Editor1.BodyHtml)
        End Using
    End Sub

End Class