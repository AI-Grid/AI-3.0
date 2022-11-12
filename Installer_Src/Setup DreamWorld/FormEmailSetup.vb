Imports System.Text.RegularExpressions

Public Class FormEmailSetup

    Dim initted As Boolean

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

#Region "Startup"

    Private Sub Form_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed

        Settings.SaveSettings()

    End Sub

    Private Sub Loaded(sender As Object, e As EventArgs) Handles Me.Load

        Me.Text = Global.Outworldz.My.Resources.SMTP
        EmailPassword.Text = Settings.SmtpPassword
        EmailPassword.UseSystemPasswordChar = True
        EmailPasswordLabel.Text = Global.Outworldz.My.Resources.SMTPPassword_word
        ToolTip1.SetToolTip(EmailPassword, Global.Outworldz.My.Resources.tt_Click_to_reveal_password)
        HelpToolStripMenuItem.Text = My.Resources.Help_word
        UserNameLabel.Text = Global.Outworldz.My.Resources.User_Name_word
        EmailUsername.Text = Settings.SmtPropUserName
        ToolTip1.SetToolTip(UserNameLabel, Global.Outworldz.My.Resources.tt_EmailUserName)
        ToolTip1.SetToolTip(EmailUsername, Global.Outworldz.My.Resources.tt_EmailUserName)
        SMTPSendEmailAccountBox.Text = My.Resources.enableEmailToSMTP
        EmailEnabledCheckBox.Checked = Settings.EmailEnabled
        EmailEnabledCheckBox.Text = Global.Outworldz.My.Resources.EmailEnabled
        ToolTip1.SetToolTip(EmailEnabledCheckBox, Global.Outworldz.My.Resources.tt_EmailEnabled)

        EmailHostLabel.Text = Global.Outworldz.My.Resources.SMTPHost_word
        SmtpHost.Text = Settings.SmtpHost
        ToolTip1.SetToolTip(EmailHostLabel, Global.Outworldz.My.Resources.tt_SMTP_Host_name)
        ToolTip1.SetToolTip(SmtpHost, Global.Outworldz.My.Resources.tt_SMTP_Host_name)

        EmailPortLabel.Text = Global.Outworldz.My.Resources.SMTPPort_word
        SmtpPort.Text = CStr(Settings.SmtpPort)
        ToolTip1.SetToolTip(EmailPortLabel, Global.Outworldz.My.Resources.tt_EmailPort)
        ToolTip1.SetToolTip(SmtpPort, Global.Outworldz.My.Resources.tt_EmailPort)

        'extended factors

        VerifyCertificateCheckBox.Checked = Settings.VerifyCertCheckBox
        VerifyCertificateCheckBox.Text = Global.Outworldz.My.Resources.VerifyCertificate
        ToolTip1.SetToolTip(VerifyCertificateCheckBox, Global.Outworldz.My.Resources.tt_VerifyCertificate)

        ';# {enabled} {[Startup]emailmoduleDefaultEmailModule} {Enable send to objects to regions Not on instance?} {true false} true
        ';; Enable sending email to regions Not on current opensimulator instance. Currently does Not work
        enableEmailToExternalObjectsCheckBox.Text = Global.Outworldz.My.Resources.EmailToObjectsEnabled
        enableEmailToExternalObjectsCheckBox.Checked = Settings.EnableEmailToExternalObjects
        ToolTip1.SetToolTip(enableEmailToExternalObjectsCheckBox, Global.Outworldz.My.Resources.tt_enableEmailToExternalObjects)

        enableEmailToSMTPCheckBox.Checked = Settings.EnableEmailToSMTPCheckBox
        enableEmailToSMTPCheckBox.Text = Global.Outworldz.My.Resources.enableEmailToSMTP
        ToolTip1.SetToolTip(enableEmailToSMTPCheckBox, Global.Outworldz.My.Resources.tt_enableEmailToSMTPCheckBox)

        ';# {enabled} {[Startup]emailmoduleDefaultEmailModule} {maximum number of emails from a object owner per hour} {} 500
        MailsFromOwnerPerHourTextBox.Text = CStr(Settings.MailsFromOwnerPerHour)
        MailsFromPrimOwnerPerHourLabel.Text = Global.Outworldz.My.Resources.EmailsFromOwnerPerHour
        ToolTip1.SetToolTip(MailsFromOwnerPerHourTextBox, Global.Outworldz.My.Resources.tt_MailsFromOwnerPerHour)
        ToolTip1.SetToolTip(MailsFromPrimOwnerPerHourLabel, Global.Outworldz.My.Resources.tt_MailsFromOwnerPerHour)

        ';# {enabled} {[Startup]emailmoduleDefaultEmailModule} {maximum number of emails to a prim address per hour} {} 50
        MailsToPrimAddressPerHourTextBox.Text = CStr(Settings.MailsToPrimAddressPerHour)
        LabelMailsToPrimAddressPerHour.Text = Global.Outworldz.My.Resources.EmailsToPrimAddressPerHour
        ToolTip1.SetToolTip(MailsToPrimAddressPerHourTextBox, Global.Outworldz.My.Resources.tt_MailsToPrimAddressPerHour)
        ToolTip1.SetToolTip(LabelMailsToPrimAddressPerHour, Global.Outworldz.My.Resources.tt_MailsToPrimAddressPerHour)

        ';# {enabled} {[Startup]emailmoduleDefaultEmailModule} {maximum number of emails via smtp per day} {} 100
        '; MailsToAddressPerHour = 500  ' UBIT MANTIS
        SMTP_MailsPerDayTextBox.Text = CStr(Settings.MailsPerDay)
        MailsPerDayLabel.Text = Global.Outworldz.My.Resources.EmailsPerDay
        ToolTip1.SetToolTip(SMTP_MailsPerDayTextBox, Global.Outworldz.My.Resources.tt_MailsPerDay)
        ToolTip1.SetToolTip(MailsPerDayLabel, Global.Outworldz.My.Resources.tt_MailsPerDay)

        ';# {enabled} {[Startup]emailmoduleDefaultEmailModule} {maximum number of emails to a SMTP address per hour} {} 10
        MailsToSMTPAddressPerHourTextBox.Text = CStr(Settings.EmailsToSMTPAddressPerHour)
        MailsToSMTPAddressPerHourLabel.Text = Global.Outworldz.My.Resources.EmailsToSMTPAddressPerHour
        ToolTip1.SetToolTip(MailsToSMTPAddressPerHourTextBox, Global.Outworldz.My.Resources.tt_MailsToSMTPAddressPerHour)
        ToolTip1.SetToolTip(MailsToSMTPAddressPerHourLabel, Global.Outworldz.My.Resources.tt_MailsToSMTPAddressPerHour)

        ';# {email_pause_time} {[Startup]emailmoduleDefaultEmailModule Enabled: true} {Period in seconds to delay after an email Is sent.} {} 20
        email_pause_timeTextBox.Text = CStr(Settings.Email_pause_time)
        EmailPauseTimeLabel.Text = Global.Outworldz.My.Resources.Emailpausetime
        ToolTip1.SetToolTip(email_pause_timeTextBox, Global.Outworldz.My.Resources.tt_EmailPauseTime)
        ToolTip1.SetToolTip(EmailPauseTimeLabel, Global.Outworldz.My.Resources.tt_EmailPauseTime)

        ';# {email_max_size} {[Startup]emailmoduleDefaultEmailModule Enabled: true} {Maximum total size of email in bytes.} {} 4096
        MaxMailSizeTextBox.Text = CStr(Settings.MaxMailSize)
        MaxMailSizeTextBoxLabel.Text = Global.Outworldz.My.Resources.EmailMaxMailSize
        ToolTip1.SetToolTip(MaxMailSizeTextBox, Global.Outworldz.My.Resources.tt_MaxMailSize)
        ToolTip1.SetToolTip(MaxMailSizeTextBoxLabel, Global.Outworldz.My.Resources.tt_MaxMailSize)

        Select Case Settings.SSLType
            Case 0
                RadioButtonNone.Checked = True
            Case 1
                RadioButtonAuto.Checked = True
            Case 2
                RadioButtonSslOnConnect.Checked = True
            Case 3
                RadioButtonStartTls.Checked = True
            Case 4
                RadioButtonStartTlsWhenAvailable.Checked = True
        End Select

        RadioButtonNone.Text = Global.Outworldz.My.Resources.None
        ToolTip1.SetToolTip(RadioButtonNone, Global.Outworldz.My.Resources.tt_NoSecurity)

        RadioButtonAuto.Text = Global.Outworldz.My.Resources.Automatic
        ToolTip1.SetToolTip(RadioButtonAuto, Global.Outworldz.My.Resources.tt_AutoMaticSecurity)

        RadioButtonSslOnConnect.Text = Global.Outworldz.My.Resources.SslOnConnect
        ToolTip1.SetToolTip(RadioButtonSslOnConnect, Global.Outworldz.My.Resources.tt_SslOnConnect)

        RadioButtonStartTls.Text = Global.Outworldz.My.Resources.StartTls
        ToolTip1.SetToolTip(RadioButtonStartTls, Global.Outworldz.My.Resources.tt_RadioButtonStartTls)

        RadioButtonStartTlsWhenAvailable.Text = Global.Outworldz.My.Resources.StartTlsWhenAvailable
        ToolTip1.SetToolTip(RadioButtonStartTlsWhenAvailable, Global.Outworldz.My.Resources.tt_StartTlsWhenAvailable)

        Button1.Text = My.Resources.Test_word
        HelpOnce("Email")

        SetScreen()
        initted = True

    End Sub

#End Region

#Region "TextBoxes"

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If Not Settings.EmailEnabled Then
            MsgBox(My.Resources.EmailDisabled, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Error_word)
            Return
        End If

        If Not Settings.EnableEmailToSMTPCheckBox Then
            MsgBox(My.Resources.SMTPDisabled, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Error_word)
            Return
        End If

        Dim result = SendEmail(Settings.SmtPropUserName, Settings.SmtPropUserName, "Test Message from Dreamgrid", "Test Message from Dreamgrid")
        If result = My.Resources.Ok Then
            Button1.Text = My.Resources.Ok
            Sleep(3000)
            Button1.Text = My.Resources.Send_word
        End If

    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles VerifyCertificateCheckBox.CheckedChanged

        If Not initted Then Return
        Settings.VerifyCertCheckBox = VerifyCertificateCheckBox.Checked

    End Sub

    Private Sub email_pause_timeTextBox_TextChanged(sender As Object, e As EventArgs) Handles email_pause_timeTextBox.TextChanged

        If Not initted Then Return
        Dim digitsOnly = New Regex("[^\d]")
        email_pause_timeTextBox.Text = digitsOnly.Replace(email_pause_timeTextBox.Text, "")
        Settings.Email_pause_time = CInt("0" & email_pause_timeTextBox.Text)

    End Sub

    Private Sub EmailEnabledCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles EmailEnabledCheckBox.CheckedChanged

        If Not initted Then Return
        Settings.EmailEnabled = EmailEnabledCheckBox.Checked

    End Sub

    Private Sub EmailPassword_Click(sender As Object, e As EventArgs) Handles EmailPassword.Click
        EmailPassword.UseSystemPasswordChar = False
    End Sub

    Private Sub EmailPassword_TextChanged(sender As Object, e As EventArgs) Handles EmailPassword.TextChanged

        If Not initted Then Return
        Settings.SmtpPassword = EmailPassword.Text

    End Sub

    Private Sub EmailUsername_TextChanged(sender As Object, e As EventArgs) Handles EmailUsername.TextChanged

        If Not initted Then Return
        Settings.SmtPropUserName = EmailUsername.Text

    End Sub

    Private Sub enableEmailToExternalObjectsCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles enableEmailToExternalObjectsCheckBox.CheckedChanged

        If Not initted Then Return
        Settings.EnableEmailToExternalObjects = enableEmailToExternalObjectsCheckBox.Checked

    End Sub

    Private Sub enableEmailToSMTPCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles enableEmailToSMTPCheckBox.CheckedChanged

        If Not initted Then Return
        Settings.EnableEmailToSMTPCheckBox = enableEmailToSMTPCheckBox.Checked

    End Sub

    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.Click

        HelpManual("Email")

    End Sub

    Private Sub MailsFromOwnerPerHourTextBox_TextChanged(sender As Object, e As EventArgs) Handles MailsFromOwnerPerHourTextBox.TextChanged

        If Not initted Then Return
        Dim digitsOnly = New Regex("[^\d]")
        MailsFromOwnerPerHourTextBox.Text = digitsOnly.Replace(MailsFromOwnerPerHourTextBox.Text, "")
        Settings.MailsFromOwnerPerHour = CInt("0" & MailsFromOwnerPerHourTextBox.Text)

    End Sub

    Private Sub MailsToPrimAddressPerHourTextBox_TextChanged(sender As Object, e As EventArgs) Handles MailsToPrimAddressPerHourTextBox.TextChanged

        If Not initted Then Return
        Dim digitsOnly = New Regex("[^\d]")
        MailsToPrimAddressPerHourTextBox.Text = digitsOnly.Replace(MailsToPrimAddressPerHourTextBox.Text, "")
        Settings.MailsToPrimAddressPerHour = CInt("0" & MailsToPrimAddressPerHourTextBox.Text)

    End Sub

    Private Sub MailsToSMTPAddressPerHourTextBox_TextChanged(sender As Object, e As EventArgs) Handles MailsToSMTPAddressPerHourTextBox.TextChanged

        If Not initted Then Return
        Dim digitsOnly = New Regex("[^\d]")
        MailsToSMTPAddressPerHourTextBox.Text = digitsOnly.Replace(MailsToSMTPAddressPerHourTextBox.Text, "")
        Settings.EmailsToSMTPAddressPerHour = CInt("0" & MailsToSMTPAddressPerHourTextBox.Text)

    End Sub

    Private Sub RadioButtonAuto_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonAuto.CheckedChanged

        Settings.SmtpSecure = True
        Settings.SSLType = 1

    End Sub

    Private Sub RadioButtonNone_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonNone.CheckedChanged

        Settings.SmtpSecure = False
        Settings.SSLType = 0

    End Sub

    Private Sub RadioButtonSslOnConnect_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonSslOnConnect.CheckedChanged

        Settings.SmtpSecure = True
        Settings.SSLType = 2

    End Sub

    Private Sub RadioButtonStartTls_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonStartTls.CheckedChanged

        Settings.SmtpSecure = True
        Settings.SSLType = 3

    End Sub

    Private Sub RadioButtonStartTlsWhenAvailable_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonStartTlsWhenAvailable.CheckedChanged

        Settings.SmtpSecure = True
        Settings.SSLType = 4

    End Sub

    Private Sub SMTP_MailsPerDayTextBox_TextChanged(sender As Object, e As EventArgs) Handles SMTP_MailsPerDayTextBox.TextChanged

        If Not initted Then Return
        Dim digitsOnly = New Regex("[^\d]")
        SMTP_MailsPerDayTextBox.Text = digitsOnly.Replace(SMTP_MailsPerDayTextBox.Text, "")
        Settings.MailsPerDay = CInt("0" & SMTP_MailsPerDayTextBox.Text)

    End Sub

    Private Sub SmtpHost_TextChanged(sender As Object, e As EventArgs) Handles SmtpHost.TextChanged

        If Not initted Then Return
        Settings.SmtpHost = SmtpHost.Text

    End Sub

    Private Sub SmtpPort_TextChanged(sender As Object, e As EventArgs) Handles SmtpPort.TextChanged

        If Not initted Then Return

        Dim digitsOnly = New Regex("[^\d]")
        SmtpPort.Text = digitsOnly.Replace(SmtpPort.Text, "")
        Settings.SmtpPort = CInt("0" & SmtpPort.Text)

    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles MaxMailSizeTextBox.TextChanged

        If Not initted Then Return
        Dim digitsOnly = New Regex("[^\d]")
        MaxMailSizeTextBox.Text = digitsOnly.Replace(MaxMailSizeTextBox.Text, "")
        Settings.MaxMailSize = CInt("0" & MaxMailSizeTextBox.Text)

    End Sub

#End Region

End Class