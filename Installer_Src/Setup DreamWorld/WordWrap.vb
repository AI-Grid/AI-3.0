﻿#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Module Word_Wrap

    Public Function WrapText(ByVal Text As String, ByVal LineLength As Integer) As List(Of String)

        Dim ReturnValue As New List(Of String)
        ' Remove leading and trailing spaces
        Text = Trim(Text)
        Text = Text.Replace("--", "")

#Disable Warning BC42016 ' Implicit conversion
        Dim Words As String() = Text.Split(" ")
#Enable Warning BC42016 ' Implicit conversion

        If Words.Length = 1 And Words(0).Length > LineLength Then

            ' Text is just one big word that is longer than one line
            ' Split it mercilessly
            Dim lines As Double = (Int(Text.Length / LineLength) + 1)
            Text = Text.PadRight(CInt(lines) * LineLength)
            For i = 0 To lines - 1
                Dim SliceStart As Double = i * LineLength
                ReturnValue.Add(Text.Substring(CInt(SliceStart), LineLength))
            Next
        Else
            Dim CurrentLine As New System.Text.StringBuilder
            For Each Word As String In Words
                ' will this word fit on the current line?
                If CurrentLine.Length + Word.Length <
                LineLength Then
                    CurrentLine.Append(Word & " ")
                Else
                    ' is the word too long for one line
                    If Word.Length > LineLength Then
                        '
                        Dim Slice As String = Word.Substring(0, LineLength - CurrentLine.Length)
                        CurrentLine.Append(Slice)
                        ReturnValue.Add(CurrentLine.ToString)

                        CurrentLine = New System.Text.StringBuilder()

                        ' Remove the first slice from the word
                        Word = Word.Substring(Slice.Length,
                        Word.Length - Slice.Length)

                        ' How many more lines do we need for this word?
                        Dim RemainingSlices As Double = (Word.Length / LineLength) + 1
                        For LineNumber = 1 To RemainingSlices

                            If LineNumber = RemainingSlices Then

                                'this is the last slice
                                CurrentLine.Append(Word & " ")
                            Else
                                ' this is not the last slice hack off a slice that is one line long, add that slice to the output as a line and start a new line

                                Slice = Word ' .Substring(0, LineLength)
                                CurrentLine.Append(Slice)

                                ReturnValue.Add(CurrentLine.ToString)
                                CurrentLine = New System.Text.StringBuilder()

                                ' Remove the slice from the

                                Word = Word.Substring(Slice.Length, Word.Length - Slice.Length)
                            End If
                        Next
                    Else
                        ' finish the current line and start a new one with the wrapped word
                        ReturnValue.Add(CurrentLine.ToString)
                        CurrentLine = New System.Text.StringBuilder(Word & " ")
                    End If
                End If
            Next

            ' Write the last line to the output
            If CurrentLine.Length > 0 Then
                ReturnValue.Add(CurrentLine.ToString)
            End If
        End If
        Return ReturnValue

    End Function

End Module
