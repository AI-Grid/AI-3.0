Imports System.Speech.AudioFormat
Imports System.Speech.Synthesis
Imports System.Speech.Synthesis.VoiceGender
Imports System.Text.RegularExpressions

Public Class ChatToSpeech

    Implements IDisposable

    Public WithEvents Speaker As New SpeechSynthesizer

    ReadOnly Interlock As New Object

    Private disposedValue As Boolean

    Public Shared Function SpeechBusy() As Boolean
        Return SpeechBusyFlag
    End Function

    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub

    Public Function GetVoices() As ObjectModel.ReadOnlyCollection(Of InstalledVoice)

#Disable Warning CA1304 ' Specify CultureInfo
        Return Speaker.GetInstalledVoices()
#Enable Warning CA1304 ' Specify CultureInfo

    End Function

    Public Sub SelectVoice(v As String)

        Speaker.SelectVoice(v)

    End Sub

    ''' <summary>
    ''' Speaks the chosen voice (if any) on the server
    ''' </summary>
    ''' <param name="texttospeak">String to speak</param>
    ''' <param name="SaveWave">True: return a path to a wave file, False: speak the text</param>
    ''' <returns>path to wave file</returns>
    Public Function Speach(Params As SpeechParameters) As String

        If Params Is Nothing Then Return ""

        Dim HttpPathInfo As String = ""
        If Settings.VoiceName = "No Speech" Then Return ""
        If Settings.VoiceName = "No Speech" Then Return ""
        If Params.TTS Is Nothing Then Return ""
        If Params.TTS.Length = 0 Then Return ""

        SyncLock Interlock

            While SpeechBusyFlag
                Sleep(1000)
            End While

            Dim fname As String = ""
            Try

                Dim DiskFilepath = IO.Path.Combine(Settings.CurrentDirectory, $"OutworldzFiles\Apache\htdocs\TTS\Audio")

                ' Numbers if from form, else counter for security
                If Params.FileName IsNot Nothing Then
                    fname = $"TTS_{Params.FileName}.wav"
                Else
                    fname = $"TTS_{SpeechCounter:D10}.wav"
                    SpeechCounter += 1
                End If

                ' Make path to Cache
                HttpPathInfo = $"http://{Settings.PublicIP}:{Convert.ToString(Settings.ApachePort, Globalization.CultureInfo.InvariantCulture)}/TTS/Audio"
                HttpPathInfo = IO.Path.Combine(HttpPathInfo, fname)
                HttpPathInfo = HttpPathInfo.Replace("\", "/")
                HttpPathInfo = HttpPathInfo.Replace(".wav", ".mp3")

                If Not FileIO.FileSystem.FileExists(DiskFilepath) Then
                    FileIO.FileSystem.CreateDirectory(DiskFilepath)
                End If

                DiskFilepath = IO.Path.Combine(DiskFilepath, fname)
                If DiskFilepath Is Nothing Then Return ""
                If fname Is Nothing Then Return ""
                Debug.Print(DiskFilepath)

                ' check if it is in cache
                Dim Mp3FilePath = DiskFilepath.Replace(".wav", ".mp3")
                If FileIO.FileSystem.FileExists(Mp3FilePath) Then
                    Return HttpPathInfo
                End If

                If Params.SaveWave Then
                    Speaker.SetOutputToWaveFile(DiskFilepath, New SpeechAudioFormatInfo(44100, AudioBitsPerSample.Sixteen, AudioChannel.Mono))
                Else
                    Speaker.SetOutputToDefaultAudioDevice()
                End If

                'M: text , F: text will choose the first  Male or female voice
                If CBool(Params.TTS.StartsWith("M:", System.StringComparison.OrdinalIgnoreCase)) Then
                    Speaker.SelectVoiceByHints(Male, System.Speech.Synthesis.VoiceAge.Adult, 0, System.Globalization.CultureInfo.CurrentCulture)
                    Params.TTS = Params.TTS.Replace("M:", "")

                ElseIf CBool(Params.TTS.StartsWith("F:", System.StringComparison.OrdinalIgnoreCase)) Then
                    Speaker.SelectVoiceByHints(Female, System.Speech.Synthesis.VoiceAge.Adult, 0, System.Globalization.CultureInfo.CurrentCulture)
                    Params.TTS = Params.TTS.Replace("F:", "")

                ElseIf Params.Sex = "M" Then
                    Speaker.SelectVoiceByHints(Male, System.Speech.Synthesis.VoiceAge.Adult, 0, System.Globalization.CultureInfo.CurrentCulture)

                ElseIf Params.Sex = "F" Then
                    Speaker.SelectVoiceByHints(Female, System.Speech.Synthesis.VoiceAge.Adult, 0, System.Globalization.CultureInfo.CurrentCulture)

                ElseIf Params.TTS.Contains(":") Then
                    SpecificVoice(Params)

                ElseIf Params.Voice.Length > 0 Then
                    ' Code can call for a specific voice
#Disable Warning CA1304
                    For Each voice In Speaker.GetInstalledVoices()
                        If voice.VoiceInfo.Name.Contains(Params.Voice) Then
                            Speaker.SelectVoice(voice.VoiceInfo.Name)
                            Exit For
                        End If
                    Next
#Enable Warning CA1304

                    ' Lines can begin with a subset of a voice name, such as "Zira:'
                Else
                    Speaker.SelectVoice(Settings.VoiceName)
                End If

                SpeechBusyFlag = True
                Speaker.SpeakAsync(Params.TTS)
            Catch ex As Exception

            End Try

            While SpeechBusyFlag
                Sleep(10)
            End While

            If Params.SaveWave Then
                ConvertWavMP3(fname, True)
            End If

        End SyncLock

        Return HttpPathInfo

    End Function

    ''' <summary>
    ''' Line with a subset of the voice name and a colon will choose that voice
    ''' Zira: This line will be spoken using Zira's voice
    ''' </summary>

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' dispose managed state (managed objects)
                Speaker.Dispose()
            End If

            ' free unmanaged resources (unmanaged objects) and override finalizer
            ' set large fields to null
            disposedValue = True
        End If
    End Sub

    Private Shared Sub ConvertWavMP3(fileName As String, waitFlag As Boolean)
        Dim psi = New System.Diagnostics.ProcessStartInfo With {
            .FileName = IO.Path.Combine(Settings.CurrentDirectory, "Outworldzfiles\Opensim\lame.exe"),
            .Arguments = $"-b 128 --resample 44.1 {fileName} {fileName.Replace(".wav", ".mp3")}",
            .WorkingDirectory = IO.Path.Combine(Settings.CurrentDirectory, "Outworldzfiles\Apache\htdocs\TTS\Audio"),
            .WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
        }
        Dim p As New Process()
        Try
            p = Process.Start(psi)
        Catch ex As Exception
            BreakPoint.Print(ex.Message)
        End Try

        If waitFlag Then
            p.WaitForExit()
        End If
        If p IsNot Nothing Then p.Dispose()

    End Sub

    Private Shared Sub EventHandler() Handles Speaker.SpeakCompleted

        SpeechBusyFlag = False

    End Sub

    Private Sub SpecificVoice(ByVal Param As SpeechParameters)

        Dim pattern = New Regex("(.*?):", RegexOptions.IgnoreCase)
        Dim match As Match = pattern.Match(Param.TTS)
        If match.Success Then
            Dim Str = match.Groups(1).Value
            If Str.Length > 0 Then

#Disable Warning CA1304
                For Each voice In Speaker.GetInstalledVoices()
#Enable Warning CA1304
                    If voice.VoiceInfo.Name.Contains(Str) Then
                        Speaker.SelectVoice(voice.VoiceInfo.Name)
                        Param.TTS = Param.TTS.Replace(Str & ":", "")
                        Exit For
                    End If
                Next
            End If
        End If

    End Sub

End Class

Public Class SpeechParameters
    Public FileName As String
    Public SaveWave As Boolean
    Public Sex As String
    Public TTS As String
    Public Voice As String
End Class
