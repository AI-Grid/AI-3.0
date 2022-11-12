#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.Text.RegularExpressions

Public Class FormIcecast

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

    Private Sub AdminPassword_Click(sender As Object, e As EventArgs) Handles AdminPassword.Click

        AdminPassword.UseSystemPasswordChar = False

    End Sub

    Private Sub AdminPassword_TextChanged(sender As Object, e As EventArgs) Handles AdminPassword.TextChanged

        Settings.SCAdminPassword = AdminPassword.Text

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If Not Settings.SCEnable Then
            Settings.SCEnable = True
        End If

        PropAborting = True
        StopIcecast()
        StartIcecast()
        PropAborting = False

    End Sub

    Private Sub FormisClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed

        Settings.SaveSettings()
        DoIceCast()

    End Sub

    Private Sub HelpToolStripMenuItem4_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem4.Click
        HelpManual("Icecast")
    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub

    Private Sub LoadURL_Click(sender As Object, e As EventArgs) Handles LoadURL.Click

        Dim webAddress As String = "http://" + Settings.PublicIP + ":" + ShoutcastPort.Text
        TextPrint(My.Resources.Icecast_Desc & " " + webAddress)
        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

        If Settings.SCEnable = False Then
            TextPrint(My.Resources.IceCast_disabled)
        End If

    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs)

        HelpManual("Icecast")

    End Sub

    Private Sub SC_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        AdminPassword.Text = Settings.SCAdminPassword
        Button1.Text = Global.Outworldz.My.Resources.Start_word
        GroupBox1.Text = Global.Outworldz.My.Resources.IceCast_Server_word
        HelpToolStripMenuItem4.Text = Global.Outworldz.My.Resources.Help_word
        Label1.Text = Global.Outworldz.My.Resources.Password_word
        Label2.Text = Global.Outworldz.My.Resources.Port1
        Label3.Text = Global.Outworldz.My.Resources.Admin_Password_word
        Label4.Text = Global.Outworldz.My.Resources.port2
        LoadURL.Text = Global.Outworldz.My.Resources.Admin_Web_Page_word
        MenuStrip2.Text = Global.Outworldz.My.Resources._0
        ShoutcastEnable.Checked = Settings.SCEnable
        ShoutcastEnable.Text = Global.Outworldz.My.Resources.Enable_word
        ShoutcastPassword.Text = Settings.SCPassword
        ShoutcastPort.Text = Settings.SCPortBase.ToString(Globalization.CultureInfo.InvariantCulture)
        ShoutcastPort1.Text = Settings.SCPortBase1.ToString(Globalization.CultureInfo.InvariantCulture)
        Text = Global.Outworldz.My.Resources.Icecast_word
        ToolStripMenuItem30.Image = Global.Outworldz.My.Resources.question_and_answer
        ToolStripMenuItem30.Text = Global.Outworldz.My.Resources.Help_word

        AdminPassword.UseSystemPasswordChar = True
        ShoutcastPassword.UseSystemPasswordChar = True
        SetScreen()
        HelpOnce("Icecast")

    End Sub

    Private Sub ShoutcastEnable_CheckedChanged(sender As Object, e As EventArgs) Handles ShoutcastEnable.CheckedChanged

        Settings.SCEnable = ShoutcastEnable.Checked

    End Sub

    Private Sub ShoutcastPassword_CLickChanged(sender As Object, e As EventArgs) Handles ShoutcastPassword.Click

        ShoutcastPassword.UseSystemPasswordChar = False

    End Sub

    Private Sub ShoutcastPassword_TextChanged(sender As Object, e As EventArgs) Handles ShoutcastPassword.TextChanged

        Settings.SCPassword = ShoutcastPassword.Text

    End Sub

    Private Sub ShoutcastPortTextChanged(sender As Object, e As EventArgs) Handles ShoutcastPort.TextChanged

        Dim digitsOnly = New Regex("[^\d]")
        ShoutcastPort.Text = digitsOnly.Replace(ShoutcastPort.Text, "")

        If Not Integer.TryParse(ShoutcastPort.Text, Settings.SCPortBase) Then
            MsgBox(My.Resources.Must_be_A_Number, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground)
        End If

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles ShoutcastPort1.TextChanged

        Dim digitsOnly = New Regex("[^\d]")
        ShoutcastPort1.Text = digitsOnly.Replace(ShoutcastPort1.Text, "")

        If Not Integer.TryParse(ShoutcastPort1.Text, Settings.SCPortBase1) Then
            MsgBox(My.Resources.Must_be_A_Number, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground)
        End If

    End Sub

#End Region

End Class
