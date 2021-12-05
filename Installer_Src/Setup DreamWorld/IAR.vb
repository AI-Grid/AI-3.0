﻿#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.Threading

Module IAR

    Public Class Params
        Public itemName As String
        Public opt As String
        Public RegionName As String
    End Class

#Region "Load"

    Public Sub LoadIAR()

        HelpOnce("Load IAR")

        If PropOpensimIsRunning() Then
            ' Create an instance of the open file dialog box. Set filter options and filter index.
            Dim openFileDialog1 = New OpenFileDialog With {
                            .InitialDirectory = """" & IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles") & """",
                            .Filter = Global.Outworldz.My.Resources.IAR_Load_and_Save_word & " (*.iar)|*.iar|All Files (*.*)|*.*",
                            .FilterIndex = 1,
                            .Multiselect = False
                        }

            ' Call the ShowDialog method to show the dialog box.
            Dim UserClickedOK As DialogResult = openFileDialog1.ShowDialog

            ' Process input if the user clicked OK.
            If UserClickedOK = DialogResult.OK Then
                Dim thing = openFileDialog1.FileName
                If thing.Length > 0 Then
                    thing = thing.Replace("\", "/")    ' because Opensim uses Unix-like slashes, that's why
                    If LoadIARContent(thing) Then
                        TextPrint(My.Resources.isLoading & " " & thing)
                    End If
                End If
            End If
            openFileDialog1.Dispose()
        Else
            TextPrint(My.Resources.Not_Running)
        End If

    End Sub

    Public Function LoadIARContent(thing As String) As Boolean

        ' handles IARS clicks
        If Not PropOpensimIsRunning() Then
            TextPrint(My.Resources.Not_Running)
            Return False
        End If

        Dim UUID As String = ""

        Try
            ' find one that is running
            For Each RegionUUID As String In RegionUuids()

                If IsBooted(RegionUUID) Then
                    UUID = RegionUUID
                    Exit For
                End If
                Application.DoEvents()
            Next
        Catch
        End Try

        Dim out As New Guid
        If Not Guid.TryParse(UUID, out) Then
            MsgBox(My.Resources.No_Regions_Ready, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Info_word)
            Return False
        End If

        Using LoadIAR As New FormIARLoad
            LoadIAR.ShowDialog()
            Dim chosen = LoadIAR.DialogResult()
            If chosen = DialogResult.OK Then
                Dim p As String = LoadIAR.GFolder
                If p.Length = 0 Then p = "/"
                Dim u As String = LoadIAR.GAvatar
                If u Is Nothing Then
                    TextPrint(My.Resources.Canceled_IAR)
                    Return False
                End If
                If u.Length > 0 Then
                    ConsoleCommand(UUID, "load iar --merge " & u & " " & p & " " & """" & thing & """")
                    SendMessage(UUID, "IAR content is loading")
                    TextPrint(My.Resources.isLoading & vbCrLf & p)
                Else
                    TextPrint(My.Resources.Canceled_IAR)
                End If
            End If

        End Using
        Return True

    End Function

    Public Sub SaveIARTask()

        If PropOpensimIsRunning() Then

            Using SaveIAR As New FormIARSave
                SaveIAR.ShowDialog()
                Dim chosen = SaveIAR.DialogResult()
                If chosen = DialogResult.OK Then

                    Dim itemName = SaveIAR.GObject
                    If itemName = "/=everything, /Objects/Folder, etc." Then
                        itemName = "/"
                    End If

                    If itemName.Length = 0 Then
                        MsgBox(My.Resources.MustHaveName, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground)
                        Return
                    End If

                    Dim ToBackup As String
                    Dim BackupName = SaveIAR.GBackupName

                    If Not BackupName.EndsWith(".iar", StringComparison.OrdinalIgnoreCase) Then
                        BackupName += ".iar"
                    End If

                    If String.IsNullOrEmpty(SaveIAR.GBackupPath) Or SaveIAR.GBackupPath = "AutoBackup" Then
                        ToBackup = IO.Path.Combine(BackupPath(), BackupName)
                    Else
                        ToBackup = BackupName
                    End If

                    Dim Name = SaveIAR.GAvatarName

                    Dim opt As String = "  "
                    If Settings.DNSName.Length > 0 Then
                        opt += " -h " & Settings.DNSName & " "
                    End If

                    Dim Perm As String = ""
                    If Not SaveIAR.GCopy Then
                        Perm += "C"
                    End If

                    If Not SaveIAR.GTransfer Then
                        Perm += "T"
                    End If

                    If Not SaveIAR.GModify Then
                        Perm += "M"
                    End If

                    If Perm.Length > 0 Then
                        opt += " --perm=" & Perm & " "
                    End If

                    For Each RegionUUID As String In RegionUuids()

                        Try
                            If IsBooted(RegionUUID) Then
                                ConsoleCommand(RegionUUID, "save iar " & opt & Name & " " & """" & itemName & """" & " " & """" & ToBackup & """")
                                TextPrint(My.Resources.Saving_word & " " & BackupPath() & "\" & BackupName & ", Region " & Region_Name(RegionUUID))
                                Exit For
                            End If
                        Catch
                        End Try
                    Next
                End If
            End Using
        Else
            TextPrint(My.Resources.Not_Running)
        End If
    End Sub

    Public Sub SaveIARTaskAll()

        If PropOpensimIsRunning() Then

            Dim RegionName = ChooseRegion(False)
            If RegionName.Length = 0 Then Return

            Using SaveIAR As New FormIARSaveAll
                SaveIAR.ShowDialog()

                Dim chosen = SaveIAR.DialogResult()
                If chosen = DialogResult.OK Then
                    Dim itemName = SaveIAR.GObject
                    If itemName = "/=everything, /Objects/Folder, etc." Then
                        itemName = "/"
                    End If

                    If itemName.Length = 0 Then
                        MsgBox(My.Resources.MustHaveName, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground)
                        Return
                    End If

                    Dim opt As String = "  "
                    If Settings.DNSName.Length > 0 Then
                        opt += " -h " & Settings.DNSName & ":" & Settings.HttpPort & " "
                    End If

                    Dim Perm As String = ""
                    If Not SaveIAR.GCopy Then
                        Perm += "C"
                    End If

                    If Not SaveIAR.GTransfer Then
                        Perm += "T"
                    End If

                    If Not SaveIAR.GModify Then
                        Perm += "M"
                    End If

                    If Perm.Length > 0 Then
                        opt += " --perm=" & Perm & " "
                    End If

                    Dim p As New Params With {
                        .RegionName = RegionName,
                        .opt = opt,
                        .itemName = itemName
                    }

                    ' start a thread to see if a region has crashed, if so, add it to an exit list
#Disable Warning BC42016 ' Implicit conversion
                    Dim start As ParameterizedThreadStart = AddressOf DoIARBackground
#Enable Warning BC42016 ' Implicit conversion
                    Dim SaveIARThread = New Thread(start)
                    SaveIARThread.SetApartmentState(ApartmentState.STA)
                    SaveIARThread.Priority = ThreadPriority.Lowest ' UI gets priority
                    SaveIARThread.Start(p)
                End If
            End Using
        Else
            TextPrint(My.Resources.Not_Running)
        End If
    End Sub

    Private Function DoIARBackground(o As Params) As Boolean

        Dim RegionName As String = o.RegionName
        Dim opt As String = o.opt
        Dim itemName As String = o.itemName

        Dim ToBackup As String
        Dim UserList = GetAvatarList()

        Dim RegionUUID = FindRegionByName(RegionName)
        If Not IsBooted(RegionUUID) Then Return False
        For Each k As String In UserList
            Dim newname = k.Replace(" ", "_")
            Dim BackupName = $"{newname}_{DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss", Globalization.CultureInfo.InvariantCulture)}.iar"
            If Not System.IO.Directory.Exists(BackupPath() & "/IAR") Then
                Try
                    System.IO.Directory.CreateDirectory(BackupPath() & "/IAR")
                Catch ex As Exception
                    BreakPoint.Show(ex.Message)
                End Try
            End If

            ToBackup = IO.Path.Combine(BackupPath() & "/IAR", BackupName)
            ConsoleCommand(RegionUUID, $"save iar {opt} {k} / ""{ToBackup}""")
            WaitforComplete(ToBackup)
        Next
        Return True

    End Function

    Private Sub WaitforComplete(BackupName As String)

        Dim s As Long
        Dim oldsize As Long = 0
        Dim same As Integer = 0
        While same < 10
            Dim fi = New System.IO.FileInfo(BackupName)
            Try
                s = fi.Length
            Catch ex As Exception
                'BreakPoint.Show(ex.Message)
            End Try
            If s = oldsize And s > 0 Then
                same += 1
            Else
                same = 0
            End If
            Sleep(1000)
            oldsize = s
        End While

    End Sub

#End Region

End Module
