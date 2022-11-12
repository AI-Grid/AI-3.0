Imports System.Text.RegularExpressions
Imports System.Threading
Imports EmailValidation
Imports MimeKit

Module OfflineIM

    Public Sub OfflineIMEmailThread()

        Dim WebThread = New Thread(AddressOf OfflineIMEmail)
        WebThread.SetApartmentState(ApartmentState.STA)
        WebThread.Priority = ThreadPriority.BelowNormal ' UI gets priority
        WebThread.Start()

    End Sub

    Public Function SendEmail(FromName As String, ToEmail As String, Subject As String, Text As String) As String

        If Not Settings.EmailEnabled Then Return My.Resources.EmailDisabled


        If FromName.Length = 0 Then
            Return My.Resources.NoFromEmail
        End If

        If ToEmail.Length = 0 Then
            Return My.Resources.NoToEmail
        End If

        If Text.Length = 0 Then
            Return My.Resources.NoBody
        End If

        Using Message As New MimeMessage()
            Message.From.Add(New MailboxAddress("", Settings.SmtPropUserName))
            Message.To.Add(New MailboxAddress("", Settings.SmtPropUserName))
            Message.Subject = Subject

            Dim builder = New BodyBuilder With {
                .TextBody = Text
            }
            Message.Body = builder.ToMessageBody()
            Return MailKit.SSL.SendMessage(Message)

        End Using

    End Function

    Private Sub OfflineIMEmail()

        While (True)
            While PropOpensimIsRunning

                If Settings.EmailEnabled And Settings.EnableEmailToSMTPCheckBox Then
                    Dim Mails = OfflineEmails()
                    For Each email In Mails
                        Dim FromPerson = AvatarEmailData(email.Fromid)
                        Dim ToPerson = AvatarEmailData(email.Principalid)
                        If ToPerson.Email.Length > 0 Then
                            '<?xml version="1.0" encoding="utf-8"?><GridInstantMessage xmlns:xsd = "http://www.w3.org/2001/XMLSchema" xmlns:xsi = "http://www.w3.org/2001/XMLSchema-instance" <> fromAgentID > fa378d99 - c4d5 - 4cc0-93E1-92E70ca49355</fromAgentID><fromAgentName>test user</fromAgentName><toAgentID>ed8813ef-361d-4005-8Fdd-a1f89908300e</toAgentID><dialog>0</dialog><fromGroup>False</fromGroup><message>test 2</message><imSessionID>17bf9e76-f2c8-0cc5-1c3c-331F95aca35b</imSessionID><offline>0</offline><Position><X>0</X><Y>0</Y><Z>0</Z></Position><binaryBucket>AA==</binaryBucket><ParentEstateID>1</ParentEstateID><RegionID>33aeeb96-c012-41E4-8edd-d78aa22e6444</RegionID><timestamp>1657327580</timestamp></GridInstantMessage>

                            Dim pattern1 = New Regex("<message>(.*?)</message>")
                            Dim match1 As Match = pattern1.Match(email.Message)
                            Dim msg As String = ""
                            If match1.Success Then
                                msg = match1.Groups(1).Value
                            End If

                            If (msg.Length > Settings.MaxMailSize) Then
                                msg = msg.Substring(0, Settings.MaxMailSize)
                            End If

                            Dim result = SendEmail($"{FromPerson.FirstName} {FromPerson.LastName}", $"{ToPerson.FirstName} {ToPerson.LastName}", $"IM from {FromPerson.FirstName} {FromPerson.LastName}", msg)
                            If result = My.Resources.Ok Then
                                Logger("Email", $"Offline IM Email sent from {FromPerson.FirstName} {FromPerson.LastName} to {ToPerson.FirstName} {ToPerson.LastName}", "Outworldz")
                                DeleteIM(email.Id)
                            End If
                        End If
                    Next
                End If

                Sleep(Settings.Email_pause_time * 1000)
            End While
            Sleep(Settings.Email_pause_time * 1000)
        End While

    End Sub

End Module
