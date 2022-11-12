<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormEmailSetup
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.SMTPSendEmailAccountBox = New System.Windows.Forms.GroupBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.MaxMailSizeTextBox = New System.Windows.Forms.TextBox()
        Me.MaxMailSizeTextBoxLabel = New System.Windows.Forms.Label()
        Me.email_pause_timeTextBox = New System.Windows.Forms.TextBox()
        Me.EmailPauseTimeLabel = New System.Windows.Forms.Label()
        Me.enableEmailToSMTPCheckBox = New System.Windows.Forms.CheckBox()
        Me.enableEmailToExternalObjectsCheckBox = New System.Windows.Forms.CheckBox()
        Me.MailsToSMTPAddressPerHourTextBox = New System.Windows.Forms.TextBox()
        Me.MailsToSMTPAddressPerHourLabel = New System.Windows.Forms.Label()
        Me.SMTP_MailsPerDayTextBox = New System.Windows.Forms.TextBox()
        Me.MailsPerDayLabel = New System.Windows.Forms.Label()
        Me.MailsToPrimAddressPerHourTextBox = New System.Windows.Forms.TextBox()
        Me.LabelMailsToPrimAddressPerHour = New System.Windows.Forms.Label()
        Me.MailsFromPrimOwnerPerHourLabel = New System.Windows.Forms.Label()
        Me.MailsFromOwnerPerHourTextBox = New System.Windows.Forms.TextBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.RadioButtonStartTlsWhenAvailable = New System.Windows.Forms.RadioButton()
        Me.RadioButtonNone = New System.Windows.Forms.RadioButton()
        Me.RadioButtonStartTls = New System.Windows.Forms.RadioButton()
        Me.RadioButtonAuto = New System.Windows.Forms.RadioButton()
        Me.RadioButtonSslOnConnect = New System.Windows.Forms.RadioButton()
        Me.VerifyCertificateCheckBox = New System.Windows.Forms.CheckBox()
        Me.EmailEnabledCheckBox = New System.Windows.Forms.CheckBox()
        Me.SmtpPort = New System.Windows.Forms.TextBox()
        Me.EmailPortLabel = New System.Windows.Forms.Label()
        Me.SmtpHost = New System.Windows.Forms.TextBox()
        Me.EmailHostLabel = New System.Windows.Forms.Label()
        Me.EmailPassword = New System.Windows.Forms.TextBox()
        Me.EmailPasswordLabel = New System.Windows.Forms.Label()
        Me.UserNameLabel = New System.Windows.Forms.Label()
        Me.EmailUsername = New System.Windows.Forms.TextBox()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Button1 = New System.Windows.Forms.Button()
        Me.SMTPSendEmailAccountBox.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SMTPSendEmailAccountBox
        '
        Me.SMTPSendEmailAccountBox.Controls.Add(Me.GroupBox1)
        Me.SMTPSendEmailAccountBox.Controls.Add(Me.GroupBox2)
        Me.SMTPSendEmailAccountBox.Controls.Add(Me.EmailEnabledCheckBox)
        Me.SMTPSendEmailAccountBox.Controls.Add(Me.SmtpPort)
        Me.SMTPSendEmailAccountBox.Controls.Add(Me.EmailPortLabel)
        Me.SMTPSendEmailAccountBox.Controls.Add(Me.SmtpHost)
        Me.SMTPSendEmailAccountBox.Controls.Add(Me.EmailHostLabel)
        Me.SMTPSendEmailAccountBox.Controls.Add(Me.EmailPassword)
        Me.SMTPSendEmailAccountBox.Controls.Add(Me.EmailPasswordLabel)
        Me.SMTPSendEmailAccountBox.Controls.Add(Me.UserNameLabel)
        Me.SMTPSendEmailAccountBox.Controls.Add(Me.EmailUsername)
        Me.SMTPSendEmailAccountBox.Location = New System.Drawing.Point(34, 48)
        Me.SMTPSendEmailAccountBox.Name = "SMTPSendEmailAccountBox"
        Me.SMTPSendEmailAccountBox.Size = New System.Drawing.Size(662, 331)
        Me.SMTPSendEmailAccountBox.TabIndex = 1
        Me.SMTPSendEmailAccountBox.TabStop = False
        Me.SMTPSendEmailAccountBox.Text = "SMTP Send Email Account"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.MaxMailSizeTextBox)
        Me.GroupBox1.Controls.Add(Me.MaxMailSizeTextBoxLabel)
        Me.GroupBox1.Controls.Add(Me.email_pause_timeTextBox)
        Me.GroupBox1.Controls.Add(Me.EmailPauseTimeLabel)
        Me.GroupBox1.Controls.Add(Me.enableEmailToSMTPCheckBox)
        Me.GroupBox1.Controls.Add(Me.enableEmailToExternalObjectsCheckBox)
        Me.GroupBox1.Controls.Add(Me.MailsToSMTPAddressPerHourTextBox)
        Me.GroupBox1.Controls.Add(Me.MailsToSMTPAddressPerHourLabel)
        Me.GroupBox1.Controls.Add(Me.SMTP_MailsPerDayTextBox)
        Me.GroupBox1.Controls.Add(Me.MailsPerDayLabel)
        Me.GroupBox1.Controls.Add(Me.MailsToPrimAddressPerHourTextBox)
        Me.GroupBox1.Controls.Add(Me.LabelMailsToPrimAddressPerHour)
        Me.GroupBox1.Controls.Add(Me.MailsFromPrimOwnerPerHourLabel)
        Me.GroupBox1.Controls.Add(Me.MailsFromOwnerPerHourTextBox)
        Me.GroupBox1.Location = New System.Drawing.Point(344, 29)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(309, 288)
        Me.GroupBox1.TabIndex = 9
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Options"
        '
        'MaxMailSizeTextBox
        '
        Me.MaxMailSizeTextBox.Location = New System.Drawing.Point(9, 218)
        Me.MaxMailSizeTextBox.Name = "MaxMailSizeTextBox"
        Me.MaxMailSizeTextBox.Size = New System.Drawing.Size(45, 20)
        Me.MaxMailSizeTextBox.TabIndex = 15
        '
        'MaxMailSizeTextBoxLabel
        '
        Me.MaxMailSizeTextBoxLabel.AutoSize = True
        Me.MaxMailSizeTextBoxLabel.Location = New System.Drawing.Point(60, 221)
        Me.MaxMailSizeTextBoxLabel.Name = "MaxMailSizeTextBoxLabel"
        Me.MaxMailSizeTextBoxLabel.Size = New System.Drawing.Size(72, 13)
        Me.MaxMailSizeTextBoxLabel.TabIndex = 14
        Me.MaxMailSizeTextBoxLabel.Text = "Mail Max Size"
        '
        'email_pause_timeTextBox
        '
        Me.email_pause_timeTextBox.Location = New System.Drawing.Point(9, 191)
        Me.email_pause_timeTextBox.Name = "email_pause_timeTextBox"
        Me.email_pause_timeTextBox.Size = New System.Drawing.Size(45, 20)
        Me.email_pause_timeTextBox.TabIndex = 13
        '
        'EmailPauseTimeLabel
        '
        Me.EmailPauseTimeLabel.AutoSize = True
        Me.EmailPauseTimeLabel.Location = New System.Drawing.Point(60, 194)
        Me.EmailPauseTimeLabel.Name = "EmailPauseTimeLabel"
        Me.EmailPauseTimeLabel.Size = New System.Drawing.Size(85, 13)
        Me.EmailPauseTimeLabel.TabIndex = 12
        Me.EmailPauseTimeLabel.Text = "Mail Pause Time"
        '
        'enableEmailToSMTPCheckBox
        '
        Me.enableEmailToSMTPCheckBox.AutoSize = True
        Me.enableEmailToSMTPCheckBox.Location = New System.Drawing.Point(9, 57)
        Me.enableEmailToSMTPCheckBox.Name = "enableEmailToSMTPCheckBox"
        Me.enableEmailToSMTPCheckBox.Size = New System.Drawing.Size(154, 17)
        Me.enableEmailToSMTPCheckBox.TabIndex = 11
        Me.enableEmailToSMTPCheckBox.Text = "Email to the World Enabled"
        Me.enableEmailToSMTPCheckBox.UseVisualStyleBackColor = True
        '
        'enableEmailToExternalObjectsCheckBox
        '
        Me.enableEmailToExternalObjectsCheckBox.AutoSize = True
        Me.enableEmailToExternalObjectsCheckBox.Location = New System.Drawing.Point(9, 29)
        Me.enableEmailToExternalObjectsCheckBox.Name = "enableEmailToExternalObjectsCheckBox"
        Me.enableEmailToExternalObjectsCheckBox.Size = New System.Drawing.Size(148, 17)
        Me.enableEmailToExternalObjectsCheckBox.TabIndex = 10
        Me.enableEmailToExternalObjectsCheckBox.Text = "Email To Objects Enabled"
        Me.enableEmailToExternalObjectsCheckBox.UseVisualStyleBackColor = True
        '
        'MailsToSMTPAddressPerHourTextBox
        '
        Me.MailsToSMTPAddressPerHourTextBox.Location = New System.Drawing.Point(9, 165)
        Me.MailsToSMTPAddressPerHourTextBox.Name = "MailsToSMTPAddressPerHourTextBox"
        Me.MailsToSMTPAddressPerHourTextBox.Size = New System.Drawing.Size(45, 20)
        Me.MailsToSMTPAddressPerHourTextBox.TabIndex = 7
        '
        'MailsToSMTPAddressPerHourLabel
        '
        Me.MailsToSMTPAddressPerHourLabel.AutoSize = True
        Me.MailsToSMTPAddressPerHourLabel.Location = New System.Drawing.Point(60, 168)
        Me.MailsToSMTPAddressPerHourLabel.Name = "MailsToSMTPAddressPerHourLabel"
        Me.MailsToSMTPAddressPerHourLabel.Size = New System.Drawing.Size(166, 13)
        Me.MailsToSMTPAddressPerHourLabel.TabIndex = 6
        Me.MailsToSMTPAddressPerHourLabel.Text = "Mails To SMTP Address Per Hour"
        '
        'SMTP_MailsPerDayTextBox
        '
        Me.SMTP_MailsPerDayTextBox.Location = New System.Drawing.Point(9, 139)
        Me.SMTP_MailsPerDayTextBox.Name = "SMTP_MailsPerDayTextBox"
        Me.SMTP_MailsPerDayTextBox.Size = New System.Drawing.Size(45, 20)
        Me.SMTP_MailsPerDayTextBox.TabIndex = 5
        '
        'MailsPerDayLabel
        '
        Me.MailsPerDayLabel.AutoSize = True
        Me.MailsPerDayLabel.Location = New System.Drawing.Point(60, 141)
        Me.MailsPerDayLabel.Name = "MailsPerDayLabel"
        Me.MailsPerDayLabel.Size = New System.Drawing.Size(72, 13)
        Me.MailsPerDayLabel.TabIndex = 4
        Me.MailsPerDayLabel.Text = "Mails Per Day"
        '
        'MailsToPrimAddressPerHourTextBox
        '
        Me.MailsToPrimAddressPerHourTextBox.Location = New System.Drawing.Point(9, 113)
        Me.MailsToPrimAddressPerHourTextBox.Name = "MailsToPrimAddressPerHourTextBox"
        Me.MailsToPrimAddressPerHourTextBox.Size = New System.Drawing.Size(45, 20)
        Me.MailsToPrimAddressPerHourTextBox.TabIndex = 3
        '
        'LabelMailsToPrimAddressPerHour
        '
        Me.LabelMailsToPrimAddressPerHour.AutoSize = True
        Me.LabelMailsToPrimAddressPerHour.Location = New System.Drawing.Point(60, 115)
        Me.LabelMailsToPrimAddressPerHour.Name = "LabelMailsToPrimAddressPerHour"
        Me.LabelMailsToPrimAddressPerHour.Size = New System.Drawing.Size(156, 13)
        Me.LabelMailsToPrimAddressPerHour.TabIndex = 2
        Me.LabelMailsToPrimAddressPerHour.Text = "Mails To Prim Address Per Hour"
        '
        'MailsFromPrimOwnerPerHourLabel
        '
        Me.MailsFromPrimOwnerPerHourLabel.AutoSize = True
        Me.MailsFromPrimOwnerPerHourLabel.Location = New System.Drawing.Point(60, 89)
        Me.MailsFromPrimOwnerPerHourLabel.Name = "MailsFromPrimOwnerPerHourLabel"
        Me.MailsFromPrimOwnerPerHourLabel.Size = New System.Drawing.Size(136, 13)
        Me.MailsFromPrimOwnerPerHourLabel.TabIndex = 0
        Me.MailsFromPrimOwnerPerHourLabel.Text = "Mails From Owner Per Hour"
        '
        'MailsFromOwnerPerHourTextBox
        '
        Me.MailsFromOwnerPerHourTextBox.Location = New System.Drawing.Point(9, 85)
        Me.MailsFromOwnerPerHourTextBox.Name = "MailsFromOwnerPerHourTextBox"
        Me.MailsFromOwnerPerHourTextBox.Size = New System.Drawing.Size(45, 20)
        Me.MailsFromOwnerPerHourTextBox.TabIndex = 1
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Button1)
        Me.GroupBox2.Controls.Add(Me.RadioButtonStartTlsWhenAvailable)
        Me.GroupBox2.Controls.Add(Me.RadioButtonNone)
        Me.GroupBox2.Controls.Add(Me.RadioButtonStartTls)
        Me.GroupBox2.Controls.Add(Me.RadioButtonAuto)
        Me.GroupBox2.Controls.Add(Me.RadioButtonSslOnConnect)
        Me.GroupBox2.Controls.Add(Me.VerifyCertificateCheckBox)
        Me.GroupBox2.Location = New System.Drawing.Point(19, 168)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(318, 149)
        Me.GroupBox2.TabIndex = 15
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Security Options"
        '
        'RadioButtonStartTlsWhenAvailable
        '
        Me.RadioButtonStartTlsWhenAvailable.AutoSize = True
        Me.RadioButtonStartTlsWhenAvailable.Location = New System.Drawing.Point(10, 116)
        Me.RadioButtonStartTlsWhenAvailable.Name = "RadioButtonStartTlsWhenAvailable"
        Me.RadioButtonStartTlsWhenAvailable.Size = New System.Drawing.Size(90, 17)
        Me.RadioButtonStartTlsWhenAvailable.TabIndex = 14
        Me.RadioButtonStartTlsWhenAvailable.TabStop = True
        Me.RadioButtonStartTlsWhenAvailable.Text = "RadioButton5"
        Me.RadioButtonStartTlsWhenAvailable.UseVisualStyleBackColor = True
        '
        'RadioButtonNone
        '
        Me.RadioButtonNone.AutoSize = True
        Me.RadioButtonNone.Location = New System.Drawing.Point(6, 24)
        Me.RadioButtonNone.Name = "RadioButtonNone"
        Me.RadioButtonNone.Size = New System.Drawing.Size(90, 17)
        Me.RadioButtonNone.TabIndex = 10
        Me.RadioButtonNone.TabStop = True
        Me.RadioButtonNone.Text = "RadioButton1"
        Me.RadioButtonNone.UseVisualStyleBackColor = True
        '
        'RadioButtonStartTls
        '
        Me.RadioButtonStartTls.AutoSize = True
        Me.RadioButtonStartTls.Location = New System.Drawing.Point(10, 93)
        Me.RadioButtonStartTls.Name = "RadioButtonStartTls"
        Me.RadioButtonStartTls.Size = New System.Drawing.Size(90, 17)
        Me.RadioButtonStartTls.TabIndex = 13
        Me.RadioButtonStartTls.TabStop = True
        Me.RadioButtonStartTls.Text = "RadioButton3"
        Me.RadioButtonStartTls.UseVisualStyleBackColor = True
        '
        'RadioButtonAuto
        '
        Me.RadioButtonAuto.AutoSize = True
        Me.RadioButtonAuto.Location = New System.Drawing.Point(8, 47)
        Me.RadioButtonAuto.Name = "RadioButtonAuto"
        Me.RadioButtonAuto.Size = New System.Drawing.Size(90, 17)
        Me.RadioButtonAuto.TabIndex = 11
        Me.RadioButtonAuto.TabStop = True
        Me.RadioButtonAuto.Text = "RadioButton2"
        Me.RadioButtonAuto.UseVisualStyleBackColor = True
        '
        'RadioButtonSslOnConnect
        '
        Me.RadioButtonSslOnConnect.AutoSize = True
        Me.RadioButtonSslOnConnect.Location = New System.Drawing.Point(8, 70)
        Me.RadioButtonSslOnConnect.Name = "RadioButtonSslOnConnect"
        Me.RadioButtonSslOnConnect.Size = New System.Drawing.Size(90, 17)
        Me.RadioButtonSslOnConnect.TabIndex = 12
        Me.RadioButtonSslOnConnect.TabStop = True
        Me.RadioButtonSslOnConnect.Text = "RadioButton4"
        Me.RadioButtonSslOnConnect.UseVisualStyleBackColor = True
        '
        'VerifyCertificateCheckBox
        '
        Me.VerifyCertificateCheckBox.AutoSize = True
        Me.VerifyCertificateCheckBox.Location = New System.Drawing.Point(156, 25)
        Me.VerifyCertificateCheckBox.Name = "VerifyCertificateCheckBox"
        Me.VerifyCertificateCheckBox.Size = New System.Drawing.Size(108, 17)
        Me.VerifyCertificateCheckBox.TabIndex = 8
        Me.VerifyCertificateCheckBox.Text = "Verify Certificate?"
        Me.VerifyCertificateCheckBox.UseVisualStyleBackColor = True
        '
        'EmailEnabledCheckBox
        '
        Me.EmailEnabledCheckBox.AutoSize = True
        Me.EmailEnabledCheckBox.Location = New System.Drawing.Point(19, 29)
        Me.EmailEnabledCheckBox.Name = "EmailEnabledCheckBox"
        Me.EmailEnabledCheckBox.Size = New System.Drawing.Size(93, 17)
        Me.EmailEnabledCheckBox.TabIndex = 9
        Me.EmailEnabledCheckBox.Text = "Email Enabled"
        Me.EmailEnabledCheckBox.UseVisualStyleBackColor = True
        '
        'SmtpPort
        '
        Me.SmtpPort.Location = New System.Drawing.Point(20, 133)
        Me.SmtpPort.Name = "SmtpPort"
        Me.SmtpPort.Size = New System.Drawing.Size(33, 20)
        Me.SmtpPort.TabIndex = 7
        '
        'EmailPortLabel
        '
        Me.EmailPortLabel.AutoSize = True
        Me.EmailPortLabel.Location = New System.Drawing.Point(199, 140)
        Me.EmailPortLabel.Name = "EmailPortLabel"
        Me.EmailPortLabel.Size = New System.Drawing.Size(59, 13)
        Me.EmailPortLabel.TabIndex = 6
        Me.EmailPortLabel.Text = "SMTP Port"
        '
        'SmtpHost
        '
        Me.SmtpHost.Location = New System.Drawing.Point(20, 107)
        Me.SmtpHost.Name = "SmtpHost"
        Me.SmtpHost.Size = New System.Drawing.Size(164, 20)
        Me.SmtpHost.TabIndex = 5
        '
        'EmailHostLabel
        '
        Me.EmailHostLabel.AutoSize = True
        Me.EmailHostLabel.Location = New System.Drawing.Point(199, 113)
        Me.EmailHostLabel.Name = "EmailHostLabel"
        Me.EmailHostLabel.Size = New System.Drawing.Size(62, 13)
        Me.EmailHostLabel.TabIndex = 4
        Me.EmailHostLabel.Text = "SMTP Host"
        '
        'EmailPassword
        '
        Me.EmailPassword.Location = New System.Drawing.Point(19, 81)
        Me.EmailPassword.Name = "EmailPassword"
        Me.EmailPassword.Size = New System.Drawing.Size(165, 20)
        Me.EmailPassword.TabIndex = 3
        Me.EmailPassword.UseSystemPasswordChar = True
        '
        'EmailPasswordLabel
        '
        Me.EmailPasswordLabel.AutoSize = True
        Me.EmailPasswordLabel.Location = New System.Drawing.Point(197, 86)
        Me.EmailPasswordLabel.Name = "EmailPasswordLabel"
        Me.EmailPasswordLabel.Size = New System.Drawing.Size(86, 13)
        Me.EmailPasswordLabel.TabIndex = 2
        Me.EmailPasswordLabel.Text = "SMTP Password"
        '
        'UserNameLabel
        '
        Me.UserNameLabel.AutoSize = True
        Me.UserNameLabel.Location = New System.Drawing.Point(199, 62)
        Me.UserNameLabel.Name = "UserNameLabel"
        Me.UserNameLabel.Size = New System.Drawing.Size(60, 13)
        Me.UserNameLabel.TabIndex = 0
        Me.UserNameLabel.Text = "User Name"
        '
        'EmailUsername
        '
        Me.EmailUsername.Location = New System.Drawing.Point(19, 57)
        Me.EmailUsername.Name = "EmailUsername"
        Me.EmailUsername.Size = New System.Drawing.Size(165, 20)
        Me.EmailUsername.TabIndex = 1
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(712, 24)
        Me.MenuStrip1.TabIndex = 2
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.Image = Global.Outworldz.My.Resources.Resources.about
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(60, 20)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(163, 93)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(101, 23)
        Me.Button1.TabIndex = 16
        Me.Button1.Text = "Test"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'FormEmailSetup
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(712, 391)
        Me.Controls.Add(Me.SMTPSendEmailAccountBox)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "FormEmailSetup"
        Me.Text = "FormEmailSetup"
        Me.SMTPSendEmailAccountBox.ResumeLayout(False)
        Me.SMTPSendEmailAccountBox.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents SMTPSendEmailAccountBox As GroupBox
    Friend WithEvents SmtpPort As TextBox
    Friend WithEvents EmailPortLabel As Label
    Friend WithEvents SmtpHost As TextBox
    Friend WithEvents EmailHostLabel As Label
    Friend WithEvents EmailPassword As TextBox
    Friend WithEvents EmailPasswordLabel As Label
    Friend WithEvents UserNameLabel As Label
    Friend WithEvents EmailUsername As TextBox
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents HelpToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents VerifyCertificateCheckBox As CheckBox
    Friend WithEvents MailsToSMTPAddressPerHourTextBox As TextBox
    Friend WithEvents MailsToSMTPAddressPerHourLabel As Label
    Friend WithEvents SMTP_MailsPerDayTextBox As TextBox
    Friend WithEvents MailsPerDayLabel As Label
    Friend WithEvents MailsToPrimAddressPerHourTextBox As TextBox
    Friend WithEvents LabelMailsToPrimAddressPerHour As Label
    Friend WithEvents MailsFromPrimOwnerPerHourLabel As Label
    Friend WithEvents MailsFromOwnerPerHourTextBox As TextBox
    Friend WithEvents EmailEnabledCheckBox As CheckBox
    Friend WithEvents enableEmailToSMTPCheckBox As CheckBox
    Friend WithEvents enableEmailToExternalObjectsCheckBox As CheckBox
    Friend WithEvents MaxMailSizeTextBox As TextBox
    Friend WithEvents MaxMailSizeTextBoxLabel As Label
    Friend WithEvents email_pause_timeTextBox As TextBox
    Friend WithEvents EmailPauseTimeLabel As Label
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents RadioButtonStartTlsWhenAvailable As RadioButton
    Friend WithEvents RadioButtonNone As RadioButton
    Friend WithEvents RadioButtonStartTls As RadioButton
    Friend WithEvents RadioButtonAuto As RadioButton
    Friend WithEvents RadioButtonSslOnConnect As RadioButton
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents Button1 As Button
End Class
