#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.Text.RegularExpressions

Public Class FormDiva

#Region "Private Fields"

    Dim _SaveNeeded As Boolean
    Dim initted As Boolean
    Private path As String = ""
    Dim setpassword As Boolean

#End Region

#Region "FormPos"

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

    Private Sub Close_form(sender As Object, e As EventArgs) Handles Me.Closed

        If _SaveNeeded = True Then
            Settings.SaveSettings()
            If IsRobustRunning() Then
                PropAborting = True
                StopRobust()
                DoWifi()
                PropAborting = False
                StartRobust()
            End If
        End If

        If setpassword And PropOpensimIsRunning() And Settings.Password.Length > 0 Then
            ConsoleCommand(RobustName(), "reset user password " & Settings.AdminFirst & " " & Settings.AdminLast & " " & Settings.Password)
        End If

    End Sub

    Private Sub Loaded(sender As Object, e As EventArgs) Handles Me.Load

        AccountConfirmationRequired.Text = Global.Outworldz.My.Resources.AccountConfirm
        ApacheToolStripMenuItem.Image = Global.Outworldz.My.Resources.window_environment
        ApacheToolStripMenuItem.Text = Global.Outworldz.My.Resources.Apache_word
        BlackRadioButton.Text = Global.Outworldz.My.Resources.Black_word
        GroupBox1.Text = Global.Outworldz.My.Resources.SplashScreen

        HelpToolStripMenuItem.Image = Global.Outworldz.My.Resources.question_and_answer
        HelpToolStripMenuItem.Text = Global.Outworldz.My.Resources.Help_word
        HelpToolStripMenuItem1.Image = Global.Outworldz.My.Resources.about
        HelpToolStripMenuItem1.Text = Global.Outworldz.My.Resources.Diva_Panel_word
        Label1.Text = Global.Outworldz.My.Resources.Theme_word
        Label10.Text = Global.Outworldz.My.Resources.Password_word
        Label11.Text = Global.Outworldz.My.Resources.First_name_word
        Label12.Text = Global.Outworldz.My.Resources.Last_Name_Word

        Label17.Text = Global.Outworldz.My.Resources.Notify_Email
        Label19.Text = Global.Outworldz.My.Resources.SplashScreen
        Label2.Text = Global.Outworldz.My.Resources.Friendly
        Label4.Text = Global.Outworldz.My.Resources.Viewer_Greeting_word
        Text = Global.Outworldz.My.Resources.WebServerPanel
        ToolTip1.SetToolTip(AdminPassword, Global.Outworldz.My.Resources.Password_Text)
        Web.Text = Global.Outworldz.My.Resources.Wifi_interface
        WhiteRadioButton.Text = Global.Outworldz.My.Resources.White_word
        CustomRadioButton.Text = Global.Outworldz.My.Resources.Custom_word
        WifiEnabled.Text = Global.Outworldz.My.Resources.Diva_Wifi_Enabled_word

        SetScreen()

        'Wifi
        WifiEnabled.Checked = Settings.WifiEnabled
        AdminEmail.Text = Settings.AdminEmail
        AccountConfirmationRequired.Checked = Settings.AccountConfirmationRequired

        SplashPage.Text = Settings.SplashPage
        GridName.Text = Settings.SimName

        If Settings.Theme = "White" Then WhiteRadioButton.Checked = True
        If Settings.Theme = "Black" Then BlackRadioButton.Checked = True
        If Settings.Theme = "Custom" Then CustomRadioButton.Checked = True

        'passwords are asterisks
        AdminPassword.UseSystemPasswordChar = True

        ' ports
        AdminPassword.Text = Settings.Password
        AdminLast.Text = Settings.AdminLast
        AdminFirst.Text = Settings.AdminFirst

        If Settings.Theme = "White" Then
            WhiteRadioButton.Checked = True
        ElseIf Settings.Theme = "Black" Then
            BlackRadioButton.Checked = True
        ElseIf Settings.Theme = "Custom" Then
            CustomRadioButton.Checked = True
        End If

        If PropOpensimIsRunning Then
            AdminPassword.Enabled = True
        Else
            AdminPassword.Enabled = False
        End If

        GreetingTextBox.Text = Settings.WelcomeMessage
        HelpOnce("Diva")

        LoadPhoto()

        initted = True

    End Sub

#End Region

#Region "Photo"

    Private Sub LoadPhoto()

        'Black at first, then can save a NewBlack later date and we will use it

        If Settings.Theme = "Black" Then
            path = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles/NewBlack.png")
        ElseIf Settings.Theme = "White" Then
            path = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles/NewWhite.png")
        ElseIf Settings.Theme = "Custom" Then
            path = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles/NewCustom.png")
        Else
            path = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles/White.png")
        End If
        Dim newpath = path

        If Not System.IO.File.Exists(path) Then
            newpath = path.Replace("New", "")
        End If

        Dim img As Image
        Try
            Using bmpTemp As New Bitmap(newpath)
                img = New Bitmap(bmpTemp)
                PictureBox1.Image = img
            End Using
        Catch
        End Try

    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

        Using ofd As New OpenFileDialog With {
                .Filter = Global.Outworldz.My.Resources.picfilter,
                .FilterIndex = 1,
                .Multiselect = False
            }
            If ofd.ShowDialog = DialogResult.OK Then
                If ofd.FileName.Length > 0 Then

                    Dim pattern = New Regex("PNG$", RegexOptions.IgnoreCase)
                    Dim match As Match = pattern.Match(ofd.FileName)
                    If Not match.Success Then
                        MsgBox(My.Resources.Must_PNG, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground)
                        Return
                    End If

                    PictureBox1.Image = Nothing

                    PictureBox1.Image = Bitmap.FromFile(ofd.FileName)

                    DeleteFile(path)

                    Try
                        Using newBitmap As New Bitmap(PictureBox1.Image)
                            newBitmap.Save(path, Imaging.ImageFormat.Png)
                        End Using
                    Catch ex As Exception
                        BreakPoint.Dump(ex)
                    End Try

                    LoadPhoto()
                    CopyWifi()
                End If
            End If
        End Using

    End Sub

#End Region

#Region "Wifi"

    Private Sub AccountConfirmationRequired_CheckedChanged(sender As Object, e As EventArgs) Handles AccountConfirmationRequired.CheckedChanged

        If Not initted Then Return
        Settings.AccountConfirmationRequired = AccountConfirmationRequired.Checked
        _SaveNeeded = True

    End Sub

    Private Sub PictureBox4_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

        HelpManual("Diva")

    End Sub

    Private Sub WifiEnabled_CheckedChanged(sender As Object, e As EventArgs) Handles WifiEnabled.CheckedChanged

        If Not initted Then Return
        Settings.WifiEnabled = WifiEnabled.Checked
        _SaveNeeded = True

        If WifiEnabled.Checked Then
            AdminFirst.Enabled = True
            AdminLast.Enabled = True
            AdminPassword.Enabled = True
            AdminEmail.Enabled = True
            AccountConfirmationRequired.Enabled = True
        Else
            AdminFirst.Enabled = False
            AdminLast.Enabled = False
            AdminPassword.Enabled = False
            AdminEmail.Enabled = False
            AccountConfirmationRequired.Enabled = False
        End If

    End Sub

#End Region

#Region "Gmail"

    Private Sub AdminFirst_TextChanged_2(sender As Object, e As EventArgs) Handles AdminFirst.TextChanged

        If Not initted Then Return
        Settings.AdminFirst = AdminFirst.Text
        _SaveNeeded = True

    End Sub

    Private Sub AdminLast_TextChanged(sender As Object, e As EventArgs) Handles AdminLast.TextChanged

        If Not initted Then Return
        Settings.AdminLast = AdminLast.Text
        _SaveNeeded = True

    End Sub

    Private Sub AdminPassword_Click(sender As Object, e As EventArgs) Handles AdminPassword.Click

        AdminPassword.UseSystemPasswordChar = False

    End Sub

    Private Sub GmailPassword_TextChanged(sender As Object, e As EventArgs)

        If Not initted Then Return

        _SaveNeeded = True

    End Sub

    Private Sub GmailUsername_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub Password_TextChanged(sender As Object, e As EventArgs) Handles AdminPassword.TextChanged

        If Not initted Then Return

        Settings.Password = AdminPassword.Text
        _SaveNeeded = True
        setpassword = True

    End Sub

    Private Sub TextBox1_TextChanged_3(sender As Object, e As EventArgs) Handles AdminEmail.TextChanged

        If Not initted Then Return
        Settings.AdminEmail = AdminEmail.Text
        _SaveNeeded = True

    End Sub

#End Region

#Region "Splash"

    Private Sub BlackRadioButton_CheckedChanged(sender As Object, e As EventArgs) Handles BlackRadioButton.CheckedChanged

        If Not initted Then Return
        If BlackRadioButton.Checked Then
            Settings.Theme = "Black"
            Settings.SaveSettings()
            LoadPhoto()
            CopyWifi()
            TextPrint(My.Resources.Theme_Black)
        End If

    End Sub

    Private Sub CustomRadioButton_CheckedChanged(sender As Object, e As EventArgs) Handles CustomRadioButton.CheckedChanged

        If Not initted Then Return

        If CustomRadioButton.Checked Then
            Settings.Theme = "Custom"
            Settings.SaveSettings()
            LoadPhoto()
            CopyWifi()
            TextPrint(My.Resources.Theme_Custom)
        End If

    End Sub

    Private Sub GreetingTextBox_TextChanged(sender As Object, e As EventArgs) Handles GreetingTextBox.TextChanged

        If Not initted Then Return
        Settings.WelcomeMessage = GreetingTextBox.Text
        _SaveNeeded = True

    End Sub

    Private Sub GridName_TextChanged(sender As Object, e As EventArgs) Handles GridName.TextChanged
        If Not initted Then Return
        Settings.SimName = GridName.Text
        _SaveNeeded = True

    End Sub

    Private Sub HelpToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem1.Click
        HelpManual("Diva")
    End Sub

    Private Sub SplashPage_TextChanged(sender As Object, e As EventArgs) Handles SplashPage.TextChanged

        If Not initted Then Return
        _SaveNeeded = True

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles SplashPage.LostFocus

        If Not initted Then Return

        If Not SplashPage.Text.StartsWith("http://", System.StringComparison.OrdinalIgnoreCase) And Not SplashPage.Text.StartsWith("https://", System.StringComparison.OrdinalIgnoreCase) Then
            SplashPage.Text = "http://" & SplashPage.Text
        End If
        Settings.SplashPage = SplashPage.Text
        _SaveNeeded = True

    End Sub

    Private Sub WhiteRadioButton_CheckedChanged(sender As Object, e As EventArgs) Handles WhiteRadioButton.CheckedChanged

        If Not initted Then Return

        If WhiteRadioButton.Checked Then
            Settings.Theme = "White"
            Settings.SaveSettings()
            LoadPhoto()
            CopyWifi()
            TextPrint(My.Resources.Theme_White)
        End If

    End Sub

#End Region

End Class
