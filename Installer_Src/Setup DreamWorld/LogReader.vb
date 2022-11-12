Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Threading

Public Class LogReader

    Private Errors As New List(Of String) From {
                "Couldn't start script"
            }

    Public Sub New(RegionUUID As String)

        Dim o As New Seeker With {
            .RegionUUID = RegionUUID
        }

#Disable Warning BC42016 ' Implicit conversion
        Dim start As ParameterizedThreadStart = AddressOf Dowork
#Enable Warning BC42016 ' Implicit conversion
        Dim T = New Thread(start)
        T.SetApartmentState(ApartmentState.STA)
        T.Priority = ThreadPriority.Lowest ' UI gets priority
        T.Start(o)

    End Sub

    Public Sub Dowork(o As Seeker)

        Dim RegionUUID As String = o.RegionUUID
        Dim lastMaxOffset As Long = 0

        Try
            Dim filename = IO.Path.Combine(Settings.OpensimBinPath, $"Regions/{Group_Name(RegionUUID)}/Opensim.log")
            If File.Exists(filename) Then

                Using reader = New StreamReader(New FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))

                    'start at the end of the file                    
                    reader.BaseStream.Seek(0, SeekOrigin.End)
                    lastMaxOffset = reader.BaseStream.Length

                    While True
                        Sleep(10000)
                        'if the file size has not changed, idle
                        If reader.BaseStream.Length = lastMaxOffset Then Continue While

                        'seek to the last max offset
                        reader.BaseStream.Seek(lastMaxOffset, SeekOrigin.Begin)
                        'Dim line As String = ""
                        While reader.BaseStream.Length <> lastMaxOffset
                            ScanIssues(reader.ReadLine(), RegionUUID)
                            'update the last max offset
                            lastMaxOffset = reader.BaseStream.Position
                            Application.DoEvents()
                        End While
                    End While
                End Using
            End If
        Catch ex As Exception
                Return
            End Try


    End Sub

    Private Sub ScanIssues(line As String, RegionUUID As String)

        For Each thing In Errors
            If line.Length > 0 Then
                'Debug.Print(line)
                Dim pattern = New Regex(thing, RegexOptions.IgnoreCase)
                Dim match = pattern.Match(line)
                If match.Success Then
                    Logger(Region_Name(RegionUUID), line, "Opensim Error")
                End If
            End If
        Next

    End Sub

End Class

Public Class Seeker
    Public RegionUUID As String
    Public seek As Boolean
End Class
