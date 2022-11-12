Imports System.IO
Imports System.Net
Imports System.Threading
Imports MySqlConnector

Module Events
    ''' <summary>
    ''' Delete all events from search so it can be rebuilt
    ''' Always uses Robust DB, even on Region Servers
    ''' </summary>

    Public Sub DeleteEvents()

        Try
            Dim conn = Settings.RegionMySqlConnection
            Using osconnection = New MySqlConnection(conn)
                Dim stm = "delete from ossearch.events"
                Using cmd = New MySqlCommand(stm, osconnection)
                    Dim rowsdeleted = cmd.ExecuteNonQuery()
                    BreakPoint.Print($"Rows: rowsdeleted.ToString(Globalization.CultureInfo.InvariantCulture)")
                End Using
            End Using
        Catch
        End Try

    End Sub

    '' start a background thread for fetching all events
    Public Sub GetEvents()

        If Settings.SearchOptions <> "Local" Then Return

        Dim start As ParameterizedThreadStart = AddressOf Events
        Dim T = New Thread(start)
        T.SetApartmentState(ApartmentState.STA)
        T.Priority = ThreadPriority.Lowest ' UI gets priority
        T.Start()

        Return
    End Sub

    ''' <summary>
    ''' Delete all events from search so it can be rebuilt
    ''' rebuilds the events db from Outworldz.com json
    ''' Always uses Robust DB, even on Region Servers
    ''' </summary>
    Private Sub Events()

        DeleteEvents()

        Try
            Using osconnection = New MySqlConnection(Settings.RobustMysqlConnection())
                osconnection.Open()
                Dim stm = "insert into ossearch.events (simname,category,creatoruuid, owneruuid,name, description, dateUTC,duration,covercharge,coveramount,parcelUUID, globalPos,gateway,eventflags) "
                stm += " values (@simname,@category,@creatoruuid,@owneruuid,@name,@description,@dateUTC,@duration,@covercharge,@coveramount,@parcelUUID,@globalPos,@gateway,@eventflags)"

                Dim Simevent As New Dictionary(Of String, String)
                Try
                    Using client As New WebClient()
                        Dim Stream = client.OpenRead(PropHttpsDomain() & "/events.txt?r=" & RandomNumber.Random)
                        Using reader = New StreamReader(Stream)
                            Application.DoEvents()

                            While reader.Peek <> -1
                                Dim s = reader.ReadLine
                                ' Split line on comma.
                                Dim array As String() = s.Split("|".ToCharArray())
                                Simevent.Clear()
                                ' Loop over each string received.
                                Dim part As String
                                For Each part In array
                                    ' Display to console.
                                    Dim a As String() = part.Split("^".ToCharArray())
                                    If a.Length = 2 Then
                                        a(1) = a(1).Replace("'", "\'")
                                        a(1) = a(1).Replace("`", vbLf)
                                        'Breakpoint.Print("{0}:{1}", a(0), a(1))
                                        If Not Simevent.ContainsKey(a(0)) Then
                                            Simevent.Add(a(0), a(1))
                                        End If
                                    End If
                                Next

                                BreakPoint.Print(Simevent.Item("description"))

                                Using cmd1 = New MySqlCommand(stm, osconnection)

                                    cmd1.Parameters.AddWithValue("@owneruuid", Simevent.Item("owneruuid"))
                                    cmd1.Parameters.AddWithValue("@name", Simevent.Item("name"))
                                    cmd1.Parameters.AddWithValue("@simname", Simevent.Item("simname"))
                                    cmd1.Parameters.AddWithValue("@category", Simevent.Item("category"))
                                    cmd1.Parameters.AddWithValue("@creatoruuid", Simevent.Item("creatoruuid"))
                                    cmd1.Parameters.AddWithValue("@description", Simevent.Item("description"))
                                    cmd1.Parameters.AddWithValue("@dateUTC", Simevent.Item("dateUTC"))
                                    cmd1.Parameters.AddWithValue("@duration", Simevent.Item("duration"))
                                    cmd1.Parameters.AddWithValue("@covercharge", Simevent.Item("covercharge"))
                                    cmd1.Parameters.AddWithValue("@coveramount", Simevent.Item("coveramount"))
                                    cmd1.Parameters.AddWithValue("@parcelUUID", Simevent.Item("parcelUUID"))
                                    cmd1.Parameters.AddWithValue("@globalPos", Simevent.Item("globalPos"))
                                    cmd1.Parameters.AddWithValue("@gateway", Simevent.Item("gateway"))
                                    cmd1.Parameters.AddWithValue("@eventflags", Simevent.Item("eventflags"))

                                    cmd1.ExecuteNonQuery()
                                    Application.DoEvents()
                                End Using ' command
                            End While
                        End Using ' reader
                    End Using ' client
                Catch
                End Try
            End Using ' connection
        Catch ex As Exception

        End Try

    End Sub

End Module
