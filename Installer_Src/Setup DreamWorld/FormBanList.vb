#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.Globalization
Imports System.Net

Public Class FormBanList

    Dim Saveneeded As Boolean

#Region "ScreenSize"

    Private ReadOnly colsize As New ClassScreenpos("BanList")

    Private ReadOnly Handler As New EventHandler(AddressOf Resize_page)

    'The following detects  the location of the form in screen coordinates
    Private _screenPosition As ClassScreenpos

    Public Property ScreenPosition As ClassScreenpos
        Get
            Return _screenPosition
        End Get
        Set(value As ClassScreenpos)
            _screenPosition = value
        End Set
    End Property

    Private Sub Resize_page(ByVal sender As Object, ByVal e As System.EventArgs)
        'Me.Text = "Form screen position = " + Me.Location.ToString
        ScreenPosition.SaveXY(Me.Left, Me.Top)
        ScreenPosition.SaveHW(Me.Height, Me.Width)
    End Sub

    Private Sub SetScreen()

        ScreenPosition = New ClassScreenpos(MyBase.Name)
        AddHandler ResizeEnd, Handler
        Dim xy As List(Of Integer) = ScreenPosition.GetXY()
        Me.Left = xy.Item(0)
        Me.Top = xy.Item(1)
        Dim hw As List(Of Integer) = ScreenPosition.GetHW()

        If hw.Item(0) = 0 Then
            Me.Height = 340
        Else
            Me.Height = hw.Item(0)
        End If
        If hw.Item(1) = 0 Then
            Me.Width = 680
        Else
            Me.Width = hw.Item(1)
        End If

    End Sub

#End Region

#Region "Start Stop"

    Public Sub LoadCollectionData() Handles Me.Load

        HelpToolStripMenuItem.Image = Global.Outworldz.My.Resources.question_and_answer
        HelpToolStripMenuItem.Text = Global.Outworldz.My.Resources.Help_word
        Text = Global.Outworldz.My.Resources.Ban_List_word

        SetScreen()
        GetData()
        BringToFront()

        HelpOnce("BanList")

    End Sub

    Private Sub Q() Handles Me.Closing

        If Saveneeded = False Then Return

        Dim BanListString As String = ""
        Try

            For Each row As DataGridViewRow In DataGridView1.Rows
                Dim s As String
                Dim t As String

                ' handle DBNull and empty lines
                Try
                    If row.Cells(0).Value Is Nothing Then
                        s = ""
                    Else
                        s = row.Cells(0).Value.ToString.Trim
                    End If
                    If row.Cells(1).Value Is Nothing Then
                        t = ""
                    Else
                        t = row.Cells(1).Value.ToString.Trim
                    End If

                    ' save back to Ban List
                    If (s.Length + t.Length) > 0 Then
                        BanListString += s & "=" & t & "|"
                    End If
                    Debug.Print(s)
                Catch
                End Try

            Next

            Settings.BanList = BanListString
            Settings.SaveSettings()

            If IsRobustRunning() Then
                Me.Hide()
                PropAborting = True
                StopRobust()
                StartRobust()
                PropAborting = False
            End If
        Catch ex As Exception
            BreakPoint.Dump(ex)
        Finally
            '"# Examples are below.=You can ban a viewer, an IP and/or a MAC address.|# 34.206.39.153=This IP from 'junk.com' is banned.|# 01:23:45:67:89:A=This MAC address is banned. Get MACs from the Robust log.|# HydraStorm=HydraStorm Copybotter|# SolarStorm=Singularity Solar Storm Copybot|#=# is a comment.|#=Delete or modify any rules then close this window. Robust will restart if running."
            BanListString = Mid(BanListString, 1, BanListString.Length - 1)
            Settings.BanList = BanListString
            Settings.SaveSettings()
        End Try

        Application.DoEvents()

    End Sub

#End Region

#Region "Private Subs"

    Private Sub DataGridView1_CellContentAdded(sender As Object, e As DataGridViewRowEventArgs) Handles DataGridView1.UserAddedRow

        Saveneeded = True

    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellEndEdit

        Saveneeded = True

    End Sub

    Private Sub DataGridView1_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles DataGridView1.ColumnWidthChanged

        Dim w As Integer = e.Column.Width
        Dim name As String = e.Column.Name.ToString(Globalization.CultureInfo.CurrentCulture)

        colsize.PutSize(name, w)

        ScreenPosition.SaveFormSettings()

    End Sub

    Private Sub DataGridView1_Delrow(sender As Object, e As DataGridViewRowEventArgs) Handles DataGridView1.UserDeletedRow

        Saveneeded = True

    End Sub

    Private Sub GetData()

        Try

            ' Populate a New data table And bind it to the BindingSource.
            Dim table = New DataTable

            'Create column.
            Dim column1 = New DataColumn With {
                .DataType = System.Type.GetType("System.String"),
                .ColumnName = Global.Outworldz.My.Resources.Banned_word,
                .AutoIncrement = False,
                .Caption = Global.Outworldz.My.Resources.Banned_word,
                .ReadOnly = False,
                .Unique = False
            }
            ' Add the column to the table.
            table.Columns.Add(column1)

            Dim column2 = New DataColumn With {
                .DataType = System.Type.GetType("System.String"),
                .ColumnName = Global.Outworldz.My.Resources.Comment_or_Notes_Word,
                .AutoIncrement = False,
                .Caption = Global.Outworldz.My.Resources.Comment_or_Notes_Word,
                .ReadOnly = False,
                .Unique = False
            }
            ' Add the column to the table.
            table.Columns.Add(column2)

            'i.e, Firestorm-Release 4.6.7.42398

            table.Locale = CultureInfo.InvariantCulture

            Dim filename As String = ""

            If Settings.BanList.Length > 0 Then
                Dim words() = Settings.BanList.Split("|".ToCharArray)
                For index As Integer = 0 To words.Length - 1
                    BreakPoint.Print(words(index))
                    Dim elems() As String = words(index).Split("="c)
                    If elems.Length = 1 Then
                        table.Rows.Add(elems(0).Trim, "")
                    ElseIf elems.Length = 2 Then
                        table.Rows.Add(elems(0).Trim, elems(1).Trim)
                    End If
                    ' remove all IPs from firewall as they are read - new ones or edited ones will be saved back on clode
                    Dim I As System.Net.IPAddress = Nothing
                    If IPAddress.TryParse(elems(0).Trim, I) Then
                        Firewall.ReleaseIp(elems(0).Trim)
                        Saveneeded = True
                    End If
                Next
            Else
                filename = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles/Opensim/BanListProto.txt")
            End If

            If filename.Length > 0 Then
                Dim line As String
                Using reader As IO.StreamReader = System.IO.File.OpenText(filename)
                    'now loop through each line
                    While reader.Peek <> -1
                        line = reader.ReadLine()
                        If line.Length > 1 Then
                            Dim words() = line.Split("|".ToCharArray)
                            If words(0) = "469947894f9e298a7726b4a58ff7bf9f" Then
                                words(0) = "#469947894f9e298a7726b4a58ff7bf9f"
                                words(1) = "Should not be banned as it a loopback adapter. Use Disk ID instead."
                            End If
                            Try
                                table.Rows.Add(words(0), words(1))
                            Catch
                                table.Rows.Add(words(0), "")
                            End Try

                            ' remove all IPs from firewall as they are read - new ones or edited ones will be saved back on close
                            Dim I As System.Net.IPAddress = Nothing
                            If IPAddress.TryParse(words(0), I) Then Firewall.ReleaseIp(words(0))
                        End If
                    End While
                End Using
            End If

            DataGridView1.DataSource = table
            DataGridView1.Columns(0).Width = colsize.ColumnWidth(My.Resources.Banned_word, 240)
            DataGridView1.Columns(1).Width = colsize.ColumnWidth(My.Resources.Comment_or_Notes_Word, 500)
            ' Do not catch general exception types
        Catch ex As Exception
            ' Do not catch general exception types
            BreakPoint.Dump(ex)
            ErrorLog("Banlist:" & ex.Message)
        End Try

    End Sub

    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.Click

        HelpManual("BanList")

    End Sub

#End Region

End Class
