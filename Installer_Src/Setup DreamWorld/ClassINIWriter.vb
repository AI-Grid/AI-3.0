Imports System.IO

Public Class IniWriter

    Private _Filename As String
    Dim _In As New List(Of String)

    Public Sub New(File As String)

        If File Is Nothing Then Return
        _Filename = File

        Try
            Using Reader As New StreamReader(_Filename, System.Text.Encoding.UTF8)
                While Reader.EndOfStream = False
                    _In.Add(Reader.ReadLine())
                End While
            End Using
        Catch ex As Exception
            ErrorLog(ex.Message)
        End Try

        Return
    End Sub

#Region "Loader"

    Public Sub Save(name As String)

        ' make a backup
        DeleteFile($"{_Filename}.bak")
        Sleep(10)
        Try
            My.Computer.FileSystem.RenameFile(_Filename, $"{name}.bak")
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

        Try
            Dim file As System.IO.StreamWriter
            file = My.Computer.FileSystem.OpenTextFileWriter(_Filename, False)
            For Each Item As String In _In
                file.WriteLine(Item)
            Next
            file.Flush()
            file.Close()
        Catch ex As Exception
            ErrorLog($"{_Filename} {ex.Message}")
        End Try
        _In.Clear()

    End Sub

    Public Sub Write(Name As String, value As String)

        Dim _Out As New List(Of String)
        Dim found As Boolean = False
        For Each Item As String In _In
            If Item.StartsWith(Name, StringComparison.OrdinalIgnoreCase) Then
                _Out.Add(value)
                found = True
            Else
                _Out.Add(Item)
            End If
        Next

        _In.Clear()
        For Each item In _Out
            _In.Add(item)
        Next

    End Sub

#End Region

End Class
