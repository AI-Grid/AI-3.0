﻿Public Class Gloebits
    Implements IDisposable
#Region "Globals"
    Dim Initted As Boolean = False
#End Region

#Region "Load/Quit"


    Private Sub Loaded(sender As Object, e As EventArgs) Handles Me.Load

        ContactEmailTextBox.Text = Form1.MySetting.GLBOwnerEmail
        OwnerNameTextbox.Text = Form1.MySetting.GLBOwnerName

        SandboxButton.Checked = Not Form1.MySetting.GloebitsMode
        ProductionButton.Checked = Form1.MySetting.GloebitsMode

        SandKeyTextBox.Text = Form1.MySetting.GLSandKey
        SandSecretTextBox.UseSystemPasswordChar = True
        SandSecretTextBox.Text = Form1.MySetting.GLSandSecret

        ProdKeyTextBox.Text = Form1.MySetting.GLProdKey
        ProdSecretTextBox.UseSystemPasswordChar = True
        ProdSecretTextBox.Text = Form1.MySetting.GLProdSecret

        GloebitsEnabled.Checked = Form1.MySetting.GloebitsEnable

        Initted = True
    End Sub

    Private Sub FormisClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed

        Form1.DoGloebits()

    End Sub

#End Region


#Region "Mode"
    Private Sub SandboxButton_CheckedChanged_1(sender As Object, e As EventArgs) Handles SandboxButton.CheckedChanged
        If SandboxButton.Checked = True Then
            ProductionButton.Checked = False
            Form1.MySetting.GloebitsMode = False
            Form1.MySetting.SaveSettings()
        End If
    End Sub

    Private Sub ProductionButton_CheckedChanged_1(sender As Object, e As EventArgs) Handles ProductionButton.CheckedChanged
        If ProductionButton.Checked = True Then
            SandboxButton.Checked = False
            Form1.MySetting.GloebitsMode = True
            Form1.MySetting.SaveSettings()
        End If
    End Sub


#End Region

#Region "Sandbox"

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles SandKeyTextBox.TextChanged
        Form1.MySetting.GLSandKey = SandKeyTextBox.Text
        Form1.MySetting.SaveSettings()
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles SandSecretTextBox.TextChanged
        Form1.MySetting.GLSandSecret = SandSecretTextBox.Text
        Form1.MySetting.SaveSettings()
    End Sub

    Private Sub TextBox2_click(sender As Object, e As EventArgs) Handles SandSecretTextBox.Click
        SandSecretTextBox.UseSystemPasswordChar = False
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles SandBoxSignUpButton.Click
        Dim webAddress As String = "https://sandbox.gloebit.com/signup/"
        Process.Start(webAddress)
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles SandBoxReqAppButton.Click
        Dim webAddress As String = "https://sandbox.gloebit.com/merchant-signup/"
        Process.Start(webAddress)
    End Sub

    Private Sub CreateAppButton2_Click(sender As Object, e As EventArgs) Handles SandBoxCreateAppButton.Click
        Dim webAddress As String = "https://www.gloebit.com"
        Process.Start(webAddress)
    End Sub
#End Region

#Region "Production"

    Private Sub ProdSecretTextBox_Click(sender As Object, e As EventArgs) Handles ProdSecretTextBox.Click
        ProdSecretTextBox.UseSystemPasswordChar = False
    End Sub
    Private Sub ProdSecretTextBox_TextChanged(sender As Object, e As EventArgs) Handles ProdSecretTextBox.TextChanged
        Form1.MySetting.GLProdSecret = ProdSecretTextBox.Text
        Form1.MySetting.SaveSettings()
    End Sub
    Private Sub ProdKeyTextBox_Click(sender As Object, e As EventArgs) Handles ProdKeyTextBox.Click
        ProdKeyTextBox.UseSystemPasswordChar = False
    End Sub
    Private Sub ProdKeyTextBox_TextChanged(sender As Object, e As EventArgs) Handles ProdKeyTextBox.TextChanged
        Form1.MySetting.GLProdKey = ProdKeyTextBox.Text
        Form1.MySetting.SaveSettings()
    End Sub

    Private Sub ProductionCreateButton_Click(sender As Object, e As EventArgs) Handles ProductionCreateButton.Click
        Dim webAddress As String = "https://www.gloebit.com/signup/"
        Process.Start(webAddress)
    End Sub

    Private Sub ProductionReqAppButton_Click(sender As Object, e As EventArgs) Handles ProductionReqAppButton.Click
        Dim webAddress As String = "https://www.gloebit.com/merchant-signup/"
        Process.Start(webAddress)
    End Sub

    Private Sub ProductionCreateAppButton_Click(sender As Object, e As EventArgs) Handles ProductionCreateAppButton.Click
        Dim webAddress As String = "https://www.gloebit.com/merchant-tools/"
        Process.Start(webAddress)
    End Sub

#End Region

#Region "OwnerInfo"

    Private Sub OwnerNameTextbox_TextChanged(sender As Object, e As EventArgs) Handles OwnerNameTextbox.TextChanged
        Form1.MySetting.GLBOwnerName = OwnerNameTextbox.Text
        Form1.MySetting.SaveSettings()

    End Sub

    Private Sub ContactEmailTextBox_TextChanged(sender As Object, e As EventArgs) Handles ContactEmailTextBox.TextChanged

        Form1.MySetting.GLBOwnerEmail = ContactEmailTextBox.Text
        Form1.MySetting.SaveSettings()

    End Sub

    Private Sub GloebitsEnabled_CheckedChanged(sender As Object, e As EventArgs) Handles GloebitsEnabled.CheckedChanged

        Form1.MySetting.GloebitsEnable = GloebitsEnabled.Checked
        Form1.MySetting.SaveSettings()

    End Sub

#End Region

#Region "Help"

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim webAddress As String = "http://dev.gloebit.com/opensim/"
        Process.Start(webAddress)
    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        Dim webAddress As String = "http://dev.gloebit.com/opensim/"
        Process.Start(webAddress)
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub

    Private Sub GroupBox3_Enter(sender As Object, e As EventArgs) Handles GroupBox3.Enter

    End Sub

    Private Sub GroupBox2_Enter(sender As Object, e As EventArgs) Handles GroupBox2.Enter

    End Sub

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter

    End Sub




#End Region


End Class