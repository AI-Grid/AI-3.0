Imports EmailValidation
Imports MimeKit

Public Class FormEmail

    Private ReadOnly Contacts As New Dictionary(Of String, String)

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

    Public Sub Init(L As ListView)
        If L Is Nothing Then Return
        SetScreen()
        If Settings.SmtPropUserName = "LoginName@gmail.com" Then
            MsgBox(My.Resources.No_Email, vbInformation Or MsgBoxStyle.MsgBoxSetForeground, "Oops")
#Disable Warning CA2000
            Dim FormEmail As New FormEmailSetup
#Enable Warning CA2000
            FormEmail.Activate()
            FormEmail.Visible = True
            FormEmail.Select()
            FormEmail.BringToFront()
            Me.Close()
            Return
        End If

        Dim counter = 0
        For Each X As ListViewItem In L.Items
            If X.Checked Then
                If Not Contacts.ContainsKey(X.Text) Then
                    Contacts.Add(X.Text, X.SubItems(1).Text)
                    counter += 1
                End If
            End If
        Next

        Me.Text = CStr(counter) & " " & My.Resources.Emails_Selected

        If counter = 0 Then
            MsgBox(My.Resources.No_Emails_Selected, vbInformation Or MsgBoxStyle.MsgBoxSetForeground, "Oops")
            Close()
        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles SendButton.Click

        BringToFront()

        If Not Settings.EmailEnabled Then
            MsgBox(My.Resources.tt_Email_Disabled, vbInformation Or MsgBoxStyle.MsgBoxSetForeground, "Oops")
#Disable Warning CA2000
            Dim FormEmail As New FormEmailSetup
#Enable Warning CA2000
            FormEmail.Activate()
            FormEmail.Visible = True
            FormEmail.Select()
            FormEmail.BringToFront()
            Me.Close()
            Return
        End If

        If SubjectTextBox.TextLength = 0 Then
            MsgBox(My.Resources.No_Subject, vbInformation Or MsgBoxStyle.MsgBoxSetForeground)
            Return
        End If

        Using Message As New MimeMessage()

            Message.From.Add(New MailboxAddress("", Settings.SmtPropUserName))

            For Each Contact In Contacts
                If EmailValidator.Validate(Contact.Value) Then
                    Message.Bcc.Add(New MailboxAddress(Contact.Key, Contact.Value))
                    TextPrint($"{My.Resources.Email_word} {Contact.Key} <{Contact.Value}>")
                End If
            Next

            Message.Subject = SubjectTextBox.Text

            Dim builder = New BodyBuilder With {
                .TextBody = EditorBox.BodyText,
                .HtmlBody = EditorBox.BodyHtml
            }
            Message.Body = builder.ToMessageBody()
            MailKit.SSL.SendMessage(Message)

        End Using
        Me.Close()
    End Sub

    Private Sub Email_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        EditorBox.DocumentText = "<html><body></body></html>"
        SubjectLabel.Text = My.Resources.Subject_word
        SendButton.Text = My.Resources.Send_word

    End Sub

End Class