#Region "Copyright AGPL3.0"

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
                        TextPrint($"{My.Resources.isLoading} {thing}")
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

        thing = thing.Replace("https:", "http:")
        Dim UUID As String = ""
        Try
            ' find one that is running
            For Each RegionUUID In RegionUuids()

                If IsBooted(RegionUUID) And Not Smart_Start(RegionUUID) Then
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

        Using LoadIAR As New FormIarLoad
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
                    ConsoleCommand(UUID, $"load iar --merge {u} ""{p}"" ""{thing}""")
                    SendMessage(UUID, "IAR content is loading")
                    TextPrint($"{My.Resources.isLoading} {vbCrLf}{p}")
                Else
                    TextPrint(My.Resources.Canceled_IAR)
                End If
            End If

        End Using
        Return True

    End Function

    Public Sub SaveIARTask()

        If PropOpensimIsRunning() Then

            Using SaveIAR As New FormIarSave
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

                    Dim Newpath = BackupPath().Replace("/", "\")
                    Dim RegionUUID = FindRegionByName(Settings.WelcomeRegion)

                    Dim Result = New WaitForFile(RegionUUID, "Saved archive", "Save IAR")
                    ConsoleCommand(RegionUUID, "save iar " & opt & Name & " " & """" & itemName & """" & " " & """" & ToBackup & """")
                    Result.Scan()

                    TextPrint(My.Resources.Saving_word & " " & Newpath & "\" & BackupName & ", Region " & Region_Name(RegionUUID))

                End If
            End Using
        Else
            TextPrint(My.Resources.Not_Running)
        End If
    End Sub

    Public Sub SaveIARTaskAll()

        If PropOpensimIsRunning() Then

            Dim RegionName = Settings.WelcomeRegion
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

                    Dim opt As String = " "

                    If Perm.Length > 0 Then
                        opt += " --perm=" & Perm & " "
                    End If

                    Dim p As New Params With {
                        .RegionName = Settings.WelcomeRegion,
                        .opt = opt,
                        .itemName = itemName
                    }

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

    Public Sub SaveThreadIARS()

        Dim opt As String = "   "
        If Settings.DnsName.Length > 0 Then
            opt += $" -h {Settings.DnsName}:{Settings.HttpPort} "    ' needs leading and trailing spaces
        End If

        Dim p As New Params With {
                            .RegionName = Settings.WelcomeRegion,
                            .opt = opt,
                            .itemName = "/"
                        }

#Disable Warning BC42016 ' Implicit conversion
        Dim start As ParameterizedThreadStart = AddressOf DoIARBackground
#Enable Warning BC42016 ' Implicit conversion
        Dim SaveIARThread = New Thread(start)
        SaveIARThread.SetApartmentState(ApartmentState.STA)
        SaveIARThread.Priority = ThreadPriority.Lowest ' UI gets priority
        SaveIARThread.Start(p)

    End Sub

    Private Sub DoIARBackground(o As Params)

        Dim RegionName As String = o.RegionName
        Dim opt As String = o.opt
        Dim itemName As String = o.itemName

        Dim ToBackup As String
        Dim UserList = GetAvatarList()

        Dim RegionUUID = FindRegionByName(RegionName)
        If Not IsBooted(RegionUUID) Then Return
        For Each k As String In UserList
            If BackupAbort Then Return
            RunningBackupName.TryAdd($"{My.Resources.Backup_IAR} {k} {My.Resources.Starting_word}", "")

            Dim newname = k.Replace(" ", "_")
            Dim BackupName = $"{newname}_{DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss", Globalization.CultureInfo.InvariantCulture)}.iar"

            Dim f = IO.Path.Combine(BackupPath(), "AutoBackup-" & Date.Now().ToString("yyyy-MM-dd", Globalization.CultureInfo.InvariantCulture))
            FileIO.FileSystem.CreateDirectory(f)

            If Not System.IO.Directory.Exists(f & "/IAR") Then
                MakeFolder(f & "/IAR")
            End If

            ToBackup = IO.Path.Combine(f & "/IAR", BackupName)
            RPC_Region_Command(RegionUUID, $"save iar {opt} {k} / ""{ToBackup}""")
            WaitforComplete(RegionUUID, ToBackup)
            RunningBackupName.TryAdd($"{My.Resources.Backup_IAR} {k} {My.Resources.Ok}", "")

        Next

    End Sub

#End Region

End Module
