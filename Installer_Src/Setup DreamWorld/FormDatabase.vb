﻿Imports System.Text.RegularExpressions

Public Class FormDatabase

    Dim initted As Boolean

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

#Region "Load/Exit"

    Private Sub Loaded(sender As Object, e As EventArgs) Handles Me.Load

        'Database 
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''
        RegionDbName.Text = Form1.MySetting.RegionDBName
        RegionDBUsername.Text = Form1.MySetting.RegionDBUsername
        RegionMySqlPassword.Text = Form1.MySetting.RegionDbPassword

        ' Robust DB
        RobustServer.Text = Form1.MySetting.RobustServer
        RobustDbName.Text = Form1.MySetting.RobustDataBaseName
        RobustDBPassword.Text = Form1.MySetting.RobustPassword
        RobustDBUsername.Text = Form1.MySetting.RobustUsername
        RobustDbPort.Text = Form1.MySetting.MySqlPort.ToString

        RobustDBPassword.UseSystemPasswordChar = True

        SetScreen()

        initted = True

        MsgBox("Changes to this area require special changes to MySQL.  If you change these, you will probaly break things.", vbInformation)

    End Sub

#End Region


#Region "Database"

    Private Sub RobustServer_TextChanged(sender As Object, e As EventArgs) Handles RobustServer.TextChanged

        If Not initted Then Return
        Form1.MySetting.RobustServer = RobustServer.Text
        Form1.MySetting.SaveSettings()

    End Sub

    Private Sub DatabaseNameUser_TextChanged(sender As Object, e As EventArgs) Handles RegionDbName.TextChanged

        If Not initted Then Return
        Form1.MySetting.RegionDBName = RegionDbName.Text
        Form1.MySetting.SaveSettings()

    End Sub

    Private Sub DbUsername_TextChanged(sender As Object, e As EventArgs) Handles RegionDBUsername.TextChanged
        If Not initted Then Return
        Form1.MySetting.RegionDBUsername = RegionDBUsername.Text
        Form1.MySetting.SaveSettings()

    End Sub

    Private Sub DbPassword_Clicked(sender As Object, e As EventArgs) Handles RegionMySqlPassword.Click

        RegionMySqlPassword.UseSystemPasswordChar = False

    End Sub
    
    Private Sub DbPassword_TextChanged(sender As Object, e As EventArgs) Handles RegionMySqlPassword.TextChanged

        If Not initted Then Return
        Form1.MySetting.RegionDbPassword = RegionMySqlPassword.Text
        Form1.MySetting.SaveSettings()

    End Sub

    Private Sub TextBox1_TextChanged_1(sender As Object, e As EventArgs) Handles RobustDbName.TextChanged
        If Not initted Then Return
        Form1.MySetting.RobustDataBaseName = RobustDbName.Text
        Form1.MySetting.SaveSettings()

    End Sub

    Private Sub RobustUsernameTextBox_TextChanged(sender As Object, e As EventArgs) Handles RobustDBUsername.TextChanged

        If Not initted Then Return
        Form1.MySetting.RobustUsername = RobustDBUsername.Text
        Form1.MySetting.SaveSettings()

    End Sub

    Private Sub RobustPasswordTextBox_TextChanged(sender As Object, e As EventArgs) Handles RobustDBPassword.TextChanged

        If Not initted Then Return
        Form1.MySetting.RobustPassword = RobustDBPassword.Text
        Form1.MySetting.SaveSettings()

    End Sub

    Private Sub RobustDbPortTextbox_click(sender As Object, e As EventArgs) Handles RobustDBPassword.Click

        RobustDBPassword.UseSystemPasswordChar = False

    End Sub


    Private Sub RobustDbPortTextbox_TextChanged(sender As Object, e As EventArgs) Handles RobustDbPort.TextChanged
        If Not initted Then Return
        Form1.MySetting.MySqlPort = RobustDbPort.Text
        Form1.MySetting.SaveSettings()

    End Sub


    Private Sub BirdHelp_Click(sender As Object, e As EventArgs) Handles BirdHelp.Click
        Form1.Help("Database")
    End Sub


    Private Sub RobustDbPort_TextChanged(sender As Object, e As EventArgs) Handles RobustDbPort.TextChanged

        Dim digitsOnly As Regex = New Regex("[^\d]")
        RobustDbPort.Text = digitsOnly.Replace(RobustDbPort.Text, "")
        Form1.MySetting.MySqlPort = RobustDbPort.Text
        Form1.MySetting.SaveSettings()

    End Sub


#End Region


End Class