﻿Public Class FormRegionPopup

    Dim gPick As String = ""


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

    Private Sub Popup_load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetScreen()
    End Sub
    Public Sub init(RegionName As String)

        Dim X = Form1.RegionClass.FindRegionByName(RegionName)
        GroupBox1.Text = RegionName & vbCrLf & "in " & Form1.RegionClass.GroupName(X)

        If Not Form1.RegionClass.RegionEnabled(X) Then
            StartButton3.Enabled = False
            StopButton1.Enabled = False
            RecycleButton2.Enabled = False
        Else
            If Form1.RegionClass.Booted(X) Then
                StartButton3.Enabled = False
                StopButton1.Enabled = True
                RecycleButton2.Enabled = True
            End If

            If Form1.RegionClass.ShuttingDown(X) Then
                StartButton3.Enabled = False
                StopButton1.Enabled = True
                RecycleButton2.Enabled = False
            End If

            If Form1.RegionClass.WarmingUp(X) Then
                StartButton3.Enabled = False
                StopButton1.Enabled = True
                RecycleButton2.Enabled = False
            End If

            ' stopped
            If Not Form1.RegionClass.ShuttingDown(X) And Not Form1.RegionClass.Booted(X) And Not Form1.RegionClass.WarmingUp(X) Then
                StartButton3.Enabled = True
                StopButton1.Enabled = False
                RecycleButton2.Enabled = False
            End If
        End If


    End Sub

    Public Function Choice() As String
        Return gPick
    End Function
    Private Sub StopButton1_Click(sender As Object, e As EventArgs) Handles StopButton1.Click
        gPick = "Stop"
        DialogResult = DialogResult.OK
    End Sub

    Private Sub StartButton3_Click(sender As Object, e As EventArgs) Handles StartButton3.Click
        gPick = "Start"
        DialogResult = DialogResult.OK
    End Sub

    Private Sub RecycleButton2_Click(sender As Object, e As EventArgs) Handles RecycleButton2.Click
        gPick = "Recycle"
        DialogResult = DialogResult.OK
    End Sub

    Private Sub EditButton1_Click(sender As Object, e As EventArgs) Handles EditButton1.Click
        gPick = "Edit"
        DialogResult = DialogResult.OK
    End Sub

End Class