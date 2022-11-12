Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports MailKit.Net.Smtp
Imports MailKit.Security
Imports MimeKit

Namespace MailKit.SSL
    Module SslCertificateValidation

        Public Function SendMessage(ByVal message As MimeMessage) As String
            Try
                Using client = New SmtpClient()
                    client.ServerCertificateValidationCallback = AddressOf MySslCertificateValidationCallback

                    Dim SSLType As SecureSocketOptions
                    Select Case Settings.SSLType
                        Case 0
                            SSLType = SecureSocketOptions.None
                        Case 1
                            SSLType = SecureSocketOptions.Auto
                        Case 2
                            SSLType = SecureSocketOptions.SslOnConnect
                        Case 3
                            SSLType = SecureSocketOptions.StartTls
                        Case 4
                            SSLType = SecureSocketOptions.StartTlsWhenAvailable
                    End Select

                    client.Connect(Settings.SmtpHost, Settings.SmtpPort, SSLType)
                    client.Authenticate(Settings.SmtPropUserName, Settings.SmtpPassword)
                    client.Send(message)
                    client.Disconnect(True)
                End Using
            Catch ex As Exception
                Return (ex.Message)
            End Try

            Return My.Resources.Ok

        End Function

        Private Function MySslCertificateValidationCallback(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
            If sslPolicyErrors = SslPolicyErrors.None Then Return True
            Dim host = CStr(sender)

            If (sslPolicyErrors And SslPolicyErrors.RemoteCertificateNotAvailable) <> 0 Then
                TextPrint($"The SSL certificate was not available for {host}")
                Return False
            End If

            If (sslPolicyErrors And SslPolicyErrors.RemoteCertificateNameMismatch) <> 0 Then
                Dim certificate2 = TryCast(certificate, X509Certificate2)
                Dim cn = If(certificate2 IsNot Nothing, certificate2.GetNameInfo(X509NameType.SimpleName, False), certificate.Subject)
                TextPrint($"The Common Name for the SSL certificate did not match {host}. Instead, it was {cn}.")
                Return False
            End If

            TextPrint("The SSL certificate for the server could not be validated for the following reasons:")

            For Each element In chain.ChainElements
                If element.ChainElementStatus.Length = 0 Then Continue For
                TextPrint(element.Certificate.Subject)

                For Each E In element.ChainElementStatus
                    TextPrint(vbTab & $"• {E.StatusInformation}")
                Next
            Next

            Return False
        End Function

    End Module
End Namespace
