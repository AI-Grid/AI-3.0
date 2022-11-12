Imports System.Globalization
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading

Module Speech

    Public SpeechBusyFlag As Boolean

    Public SpeechCounter As Integer

    Public SpeechList As New Queue(Of String)

    ''' <summary>
    ''' Starts the Chat Thread  to speak from a Que of arrivals
    ''' </summary>
    Public Sub Chat2Speech()

        If SpeechList.Count = 0 Then Return
        Dim WebThread = New Thread(AddressOf SpeakArrival)
        WebThread.SetApartmentState(ApartmentState.STA)
        WebThread.Start()

    End Sub

    Public Function GetMd5Hash(input As String) As String
        ' Create a new instance of the MD5 object.
#Disable Warning CA5351 ' Do Not Use Broken Cryptographic Algorithms
        Dim md5Hasher As MD5 = MD5.Create()
#Enable Warning CA5351 ' Do Not Use Broken Cryptographic Algorithms

        ' Convert the input string to a byte array and compute the hash.
        Dim data As Byte() = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input))

        ' Create a new String builder to collect the bytes
        ' and create a string.
        Dim sBuilder As New StringBuilder()

        ' Loop through each byte of the hashed data
        ' and format each one as a hexadecimal string.
        Dim i As Integer
        For i = 0 To data.Length - 1
            sBuilder.Append(data(i).ToString("x2", CultureInfo.InvariantCulture.NumberFormat))
        Next i

        md5Hasher.Dispose()

        ' Return the hexadecimal string.
        Return sBuilder.ToString()

    End Function

    Public Function Text2Speech(POST As String) As String

        Dim P = GetParam(POST, "Password")
        Dim M = Settings.MachineId
        If M <> P Then Return $"Bad Password {P}"
        Dim OutLoud = GetParam(POST, "Speak")
        Dim Save2File As Boolean
        If OutLoud.Length = 0 Then
            Save2File = True
        End If

        Dim Par = New SpeechParameters With {
            .Voice = GetParam(POST, "Voice"),
            .TTS = GetParam(POST, "TTS"),
            .Sex = GetParam(POST, "Sex"),
            .SaveWave = Save2File
        }

        Dim Synth As New ChatToSpeech
        Dim Filename = Synth.Speach(Par)
        Synth.Dispose()
        Return Filename

    End Function

    Private Function GetParam(POST As String, Param As String) As String

        ' Get Name=Value pairs
        Try
            Dim pattern = New Regex($"{Param}=(.*?)&|{Param}=(.*)", RegexOptions.IgnoreCase)
            Dim match As Match = pattern.Match(POST)
            If match.Success Then
                If match.Groups(1).Value.Length > 0 Then Return Uri.UnescapeDataString(match.Groups(1).Value)
                Return Uri.UnescapeDataString(match.Groups(2).Value)
            End If
        Catch
        End Try

        Return ""

    End Function

    Private Sub SpeakArrival()

        Using Synth = New ChatToSpeech
            While SpeechList.Count > 0
                If Settings.VoiceName = "No Speech" Then Return
                Try
                    Dim ProcessString As String = SpeechList.Dequeue

                    Dim Par = New SpeechParameters With {
                        .TTS = ProcessString,
                        .Voice = Settings.VoiceName
                    }
                    Synth.Speach(Par)
                Catch
                End Try

            End While
        End Using

    End Sub

End Module
