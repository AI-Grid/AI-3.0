Imports System.Threading
Imports Ionic.Zip

Module Backup

    ''' <summary>
    ''' Waits until a file stops changing length so we can type again. Quits if DreamGrid  is stopped.
    ''' Waits for 5 minutes or the file stops growing for 30 seconds
    ''' </summary>
    ''' <param name="BackupName">Name of region to watch</param>
    Public Sub WaitforComplete(RegionUUID As String, FolderAndFileName As String)

        If SkipAutobackup(RegionUUID) = "True" Then Return

        If Not Settings.Smart_Start Or Not Smart_Start(RegionUUID) Then
            SequentialPause()
            Return
        End If

        ' pass the two parameters it needs as in object
        Dim Oar = New OarObject With {
            .RegionUUID = RegionUUID,
            .FolderAndFileName = FolderAndFileName
        }

        'if suspended, keep it alive
        If Not Settings.BootOrSuspend Then

            SequentialPause()

            Dim w = New WaitForOar()
            w.Data = Oar

            Dim threadDelegate = New ThreadStart(AddressOf w.Dowork)
            Dim newThread = New Thread(threadDelegate)
            newThread.Priority = ThreadPriority.BelowNormal
            newThread.Start()

        End If

    End Sub

#Region "Backups"

    ''' <summary>
    ''' Zip up Regions
    ''' </summary>
    Public Sub BackupINI()

        Dim Name = "Region INI"
        RunningBackupName.TryAdd($"{Name} {My.Resources.Starting_word}", "")

        Dim zipused As Boolean
        'used to zip it, zip it good
        Dim _folder = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\tmp\Region_" & DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss", Globalization.CultureInfo.InvariantCulture))
        FileIO.FileSystem.CreateDirectory(_folder)
        Dim whenrun As String = Date.Now().ToString("yyyy-MM-dd_HH_mm_ss", Globalization.CultureInfo.InvariantCulture)

        Dim FileName = "Region_" & whenrun ' Set default folder
        Dim Bak = IO.Path.Combine(_folder, FileName & ".zip")
        Dim f = IO.Path.Combine(BackupPath(), Date.Now().ToString("yyyy-MM-dd", Globalization.CultureInfo.InvariantCulture))
        FileIO.FileSystem.CreateDirectory(f)
        Dim NewFile = IO.Path.Combine(f, "Backup_" & Name & "_" & whenrun)

        Using Z = New ZipFile(NewFile) With {
                .UseZip64WhenSaving = Zip64Option.AsNecessary,
                .CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression
            }

            Try
                Dim sourcePath = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Opensim\bin\Regions")
                Dim sourceDirectoryInfo As New System.IO.DirectoryInfo(sourcePath)
                For Each fileSystemInfo In sourceDirectoryInfo.GetDirectories
                    Try
                        Dim folder = fileSystemInfo.FullName
                        Dim Regionpath = IO.Path.Combine(Settings.CurrentDirectory, folder & "\Region")
                        Dim RegionDirectoryInfo As New System.IO.DirectoryInfo(Regionpath)
                        For Each RegionName In RegionDirectoryInfo.GetFileSystemInfos
                            If RegionName.Name.EndsWith(".ini", StringComparison.OrdinalIgnoreCase) Then
                                Dim shortname = RegionName.Name.Replace(".ini", "")
                                Z.AddFile(IO.Path.Combine(Regionpath, RegionName.Name), $"\Regions\{shortname}\Region\")
                                zipused = True
                            End If
                        Next
                    Catch ex As Exception
                        BreakPoint.Print(ex.Message)
                    End Try
                Next
            Catch ex As Exception
                BreakPoint.Print(ex.Message)
            End Try

            Try
                If zipused = True Then
                    Z.Save()
                    Sleep(500)
                End If

                DeleteFolder(_folder)
            Catch ex As Exception
                BreakPoint.Print(ex.Message)
            End Try

        End Using

        RunningBackupName.TryAdd($"{Name} {My.Resources.Ok}", "")

    End Sub

    ''' <summary>
    ''' Get the path to the Autobackup folder. Make it if necessary
    ''' </summary>
    ''' <returns>file path</returns>
    Public Function BackupPath() As String

        'Autobackup must exist. if not create it
        ' if they set the folder somewhere else, it may have been deleted, so reset it to default
        BackupPath = Settings.BackupFolder
        BackupPath = BackupPath.Replace("\", "/")    ' because Opensim uses Unix-like slashes, that's why

        If Not IO.Directory.Exists(BackupPath) Then
            BackupPath = IO.Path.Combine(Settings.CurrentSlashDir, "OutworldzFiles/Autobackup")
            Try
                FileIO.FileSystem.CreateDirectory(BackupPath)
                Settings.BackupFolder = BackupPath
            Catch
            End Try
        End If

        Return BackupPath

    End Function

#End Region

#Region "Tasks"

    ''' <summary>
    ''' Background backup all OARS as a thread
    ''' </summary>
    Public Sub BackupAllRegions()

        If Not Settings.BackupOARs Then Return
        For Each RegionUUID In RegionUuids()
            If BackupAbort Then Return
            If Not RegionEnabled(RegionUUID) Then Continue For
            If SkipAutobackup(RegionUUID) = "True" Then Continue For

            RunningBackupName.TryAdd($"{Region_Name(RegionUUID)} {My.Resources.backup_running}", "")

            Dim f = IO.Path.Combine(BackupPath(), "AutoBackup-" & Date.Now().ToString("yyyy-MM-dd", Globalization.CultureInfo.InvariantCulture))
            FileIO.FileSystem.CreateDirectory(f)

            If Not System.IO.Directory.Exists(f & "/OAR") Then
                MakeFolder(f & "/OAR")
            End If

            Dim file = f & "/OAR/" & Region_Name(RegionUUID) & "_" & DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss", Globalization.CultureInfo.InvariantCulture) & ".oar"

            Dim Obj = New TaskObject With {
                .TaskName = TaskName.LaunchBackupper,
                .Command = file
            }
            RebootAndRunTask(RegionUUID, Obj)
            WaitforComplete(RegionUUID, file)
            ShowDOSWindow(RegionUUID, MaybeHideWindow())

        Next

    End Sub

    '' must use console as otherwise Smart Start will shutdown
    Public Sub Backupper(RegionUUID As String, file As String)
        PokeRegionTimer(RegionUUID)
        Dim Result = New WaitForFile(RegionUUID, "Finished writing out OAR", "Save OAR")
        RPC_Region_Command(RegionUUID, $"change region ""{Region_Name(RegionUUID)}"" ")
        RPC_Region_Command(RegionUUID, $"save oar ""{file}"" ")
        Result.Scan()
    End Sub

#End Region

End Module

Public Class OarObject

    Public FolderAndFileName As String
    Public RegionUUID As String

End Class

Public Class WaitForOar

    Public Data As New OarObject

    Public Sub Dowork()
        Dim RegionUUID = Data.RegionUUID
        Dim FolderAndFileName = Data.FolderAndFileName

        Dim Filectr As Integer = 0
        Dim s As Long = 0
        Dim oldsize As Long = 0

        Dim ctr = 0 ' wait two minutes at a given size and we call it done.

        While PropOpensimIsRunning
            PokeRegionTimer(RegionUUID)

            Debug.Print($"Waiting on {Region_Name(RegionUUID)} {CStr(s)}")
            Sleep(1000)
            Try
                Dim fi = New System.IO.FileInfo(FolderAndFileName)
                s = fi.Length
            Catch ex As Exception
                Filectr += 1
            End Try

            ' file does not exist, check for 2 minutes, and abort
            If s = 0 Then
                If Filectr < 60 Then
                    Continue While
                Else
                    ' 2 minutes - abort!
                    Log("Error", $"{Region_Name(RegionUUID)} failed to start saving")
                    Return
                End If
            End If

            ' See if OAR is growing, or not
            If s = oldsize Then
                ctr += 1 ' not growing, reset counter
            Else
                ctr = 0
            End If

            If ctr = 300 Then
                Log("Error", $"{Region_Name(RegionUUID)} timeout, took too long to save")
                Return
            End If

            oldsize = s

        End While
        RunningBackupName.TryAdd($"{My.Resources.Backup_word} {Region_Name(RegionUUID)} {My.Resources.Finished_with_backup_word}", "")

    End Sub

End Class
