﻿Imports System.Threading
Imports System.IO
Imports System.IO.Compression

Module Backups

    Private _OpensimBackupRunning As Boolean
    Private _startDate As Date
    Private _initted As Boolean
    Private _Busy As Boolean
    Private _folder As String
    Private _filename As String

    Public Sub ClearFlags()

        _Busy = False
        _OpensimBackupRunning = False

    End Sub

    Private Sub Zipup()

        Dim f = _filename.Replace(".sql", ".zip")
        ZipFile.CreateFromDirectory(_folder & "\tmp\", _folder & "\" & f, CompressionLevel.Optimal, False)
        Thread.Sleep(5000)
        FileStuff.DeleteDirectory(_folder & "\tmp\", FileIO.DeleteDirectoryOption.DeleteAllContents)

    End Sub

    Public Sub SQLBackup()

        If _Busy = True Then
            FormSetup.Print("Backup is already running")
            Return
        End If
        _Busy = True
        FormSetup.Print(My.Resources.Slow_Backup)
        BackupMysql("opensim")

    End Sub

    Private Sub Exited(sender As Object, e As System.EventArgs)

        If Not _OpensimBackupRunning Then
            _OpensimBackupRunning = True
            Zipup()
            BackupMysql("robust")
        Else
            Zipup()
            _OpensimBackupRunning = False
            _Busy = False
        End If

    End Sub

    Private Sub ErrorHandler(sender As Object, e As System.EventArgs)

        Dim myProcess As New Process
        myProcess = DirectCast(sender, Process)
        BreakPoint.Show(CStr(myProcess.ExitCode))
        myProcess.Close()

        _Busy = False

    End Sub
    Public Function BackupPath() As String

        'Autobackup must exist. if not create it
        ' if they set the folder somewhere else, it may have been deleted, so reset it to default
        If Settings.BackupFolder.ToUpper(Globalization.CultureInfo.InvariantCulture) = "AUTOBACKUP" Then
            BackupPath = FormSetup.PropCurSlashDir & "/OutworldzFiles/AutoBackup/"
            If Not Directory.Exists(BackupPath) Then
                MkDir(BackupPath)
            End If
        Else
            BackupPath = Settings.BackupFolder & "/"
            BackupPath = BackupPath.Replace("\", "/")    ' because Opensim uses Unix-like slashes, that's why

            If Not Directory.Exists(BackupPath) Then
                BackupPath = FormSetup.PropCurSlashDir & "/OutworldzFiles/Autobackup/"

                If Not Directory.Exists(BackupPath) Then
                    MkDir(BackupPath)
                End If

                MsgBox(My.Resources.Autobackup_cannot_be_located & BackupPath)
                Settings.BackupFolder = BackupPath
                Settings.SaveSettings()
            End If
        End If
        Return BackupPath

    End Function
    Public Sub RunSQLBackup(OP As Object)

        Dim Name As String = OP.ToString

        Dim currentdatetime As Date = New DateTime()
        currentdatetime = Date.Now()
        Dim whenrun As String = currentdatetime.ToString("yyyy-MM-dd_HH_mm_ss", Globalization.CultureInfo.InvariantCulture)

        Dim what = Name & "_" & whenrun & ".sql"
        ' used to zip it, zip if good
        _folder = Settings.BackupFolder
        _filename = what

        ' make sure this is empty as we use it again and might have crashed
        FileStuff.DeleteDirectory(_folder & "\tmp\", FileIO.DeleteDirectoryOption.DeleteAllContents)
        Try
            MkDir(_folder & "\" & "tmp")
        Catch
        End Try

        ' we must write this to the file so it knows what database to use.
        Using outputFile As New StreamWriter(_folder & "\tmp\" & what)
            outputFile.Write("use " & Name & ";" + vbCrLf)
        End Using

        Dim ProcessSqlDump As Process = New Process With {
            .EnableRaisingEvents = True
        }
        AddHandler ProcessSqlDump.ErrorDataReceived, AddressOf ErrorHandler
        AddHandler ProcessSqlDump.Exited, AddressOf Exited

        Dim port As String = ""
        Dim host As String = ""
        If OP = "robust" Then
            port = CStr(Settings.MySqlRobustDBPort)
            host = Settings.RobustServer
        Else
            port = CStr(Settings.MySqlRegionDBPort)
            host = Settings.RegionServer
        End If

        Dim options = " --host=" & host & " --port=" & port _
        & " --opt --hex-blob --add-drop-table --allow-keywords  -uroot " _
        & " --verbose --log-error=Mysqldump.log " _
        & " --result-file=" & """" & Settings.BackupFolder & "\tmp\" & what & """" _
        & " " & Name
        Debug.Print(options)
        '--host=127.0.0.1 --port=3306 --opt --hex-blob --add-drop-table --allow-keywords  -uroot
        ' --verbose --log-error=Mysqldump.log
        ' --result-file="C:\Opensim\Outworldz_Dreamgrid\OutworldzFiles\AutoBackup\tmp\opensim_2020-12-09_00_25_24.sql"
        ' opensim

        Dim pi As ProcessStartInfo = New ProcessStartInfo With {
            .Arguments = options,
            .FileName = """" & IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\mysql\bin\mysqldump.exe") & """",
            .WorkingDirectory = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\mysql\bin"),
            .UseShellExecute = False,
            .RedirectStandardError = True,
            .RedirectStandardOutput = True,
            .CreateNoWindow = True
            }

        ProcessSqlDump.StartInfo = pi
        Try
            ProcessSqlDump.Start()
        Catch ex As Exception
            BreakPoint.Show(ex.Message)
            Return
        End Try

        ProcessSqlDump.WaitForExit()
        _Busy = False

    End Sub

    Public Sub BackupMysql(name As String)

        Dim WebThread = New Thread(AddressOf RunSQLBackup)
        WebThread.SetApartmentState(ApartmentState.STA)
        WebThread.Start(name)
        WebThread.Priority = ThreadPriority.BelowNormal

    End Sub

    Public Sub RunBackups()

        If Not _initted Then
            _initted = True
            _startDate = New DateTime()
            _startDate = Date.Now
        End If

        Dim currentdatetime As Date = New DateTime()
        currentdatetime = Date.Now

        Dim originalBoottime As Date = _startDate
        originalBoottime = originalBoottime.AddMinutes(CDbl(Settings.AutobackupInterval))

        Dim x = DateTime.Compare(currentdatetime, originalBoottime)
        If DateTime.Compare(currentdatetime, originalBoottime) > 0 Then

            _startDate = currentdatetime ' wait another interval

            If Settings.AutoBackup Then
                FormSetup.Print(currentdatetime.ToLocalTime & " Auto Backup Running")
                Dim WebThread = New Thread(AddressOf FullBackup)
                Try
                    WebThread.SetApartmentState(ApartmentState.STA)
                Catch ex As Exception
                    BreakPoint.Show(ex.Message)
                End Try
                WebThread.Start()
                WebThread.Priority = ThreadPriority.BelowNormal
            End If
        End If

        ' delete old files
        originalBoottime = _startDate

        Dim directory As New System.IO.DirectoryInfo(Backups.BackupPath)
        Dim File As System.IO.FileInfo() = directory.GetFiles()
        Dim File1 As System.IO.FileInfo

        ' get each file's last modified date
        For Each File1 In File
            If File1.Name.StartsWith("Full_Backup_", StringComparison.InvariantCultureIgnoreCase) Then
                Dim strLastModified As Date = System.IO.File.GetLastWriteTime(Backups.BackupPath & "\" & File1.Name)
                strLastModified = strLastModified.AddDays(CDbl(Settings.KeepForDays))
                Dim y = DateTime.Compare(currentdatetime, strLastModified)
                If DateTime.Compare(currentdatetime, strLastModified) > 0 Then
                    FileStuff.DeleteFile(File1.FullName)
                End If
            End If
        Next

    End Sub

    Private Sub FullBackup()

        Dim Foldername = "Full_backup_" + DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss", Globalization.CultureInfo.InvariantCulture)   ' Set default folder

        If Settings.BackupMysql Then
            Try
                My.Computer.FileSystem.CreateDirectory(Backups.BackupPath)
                My.Computer.FileSystem.CreateDirectory(Backups.BackupPath & "\Opensim_bin_Regions")
            Catch ex As Exception
            End Try

            FileStuff.CopyFolder(IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Opensim\bin\Regions"), IO.Path.Combine(Backups.BackupPath, "Opensim_bin_Regions"))
            Application.DoEvents()

        End If

        If Settings.BackupMysql Then
            Try
                My.Computer.FileSystem.CreateDirectory(Backups.BackupPath)
                My.Computer.FileSystem.CreateDirectory(IO.Path.Combine(Backups.BackupPath, "Mysql_Data"))
            Catch ex As Exception
                BreakPoint.Show(ex.Message)
            End Try
            FileStuff.CopyFolder(IO.Path.Combine(IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Mysql\Data\")), IO.Path.Combine(Backups.BackupPath, "Mysql_Data"))
            Application.DoEvents()
        End If

        If Settings.BackupFSAssets Then
            Try
                My.Computer.FileSystem.CreateDirectory(Backups.BackupPath)
                My.Computer.FileSystem.CreateDirectory(Backups.BackupPath + "\FSAssets")
            Catch ex As Exception
                BreakPoint.Show(ex.Message)
            End Try

            Dim folder As String = "./fsassets"
            If Settings.BaseDirectory = "./fsassets" Then
                folder = Settings.OpensimBinPath & "\FSAssets"
            Else
                folder = Settings.BaseDirectory
            End If
            FileStuff.CopyFolder(folder, IO.Path.Combine(Backups.BackupPath, "FSAssets"))
            Application.DoEvents()
        End If

        If Settings.BackupWifi Then
            Try
                My.Computer.FileSystem.CreateDirectory(Backups.BackupPath)
                My.Computer.FileSystem.CreateDirectory(Backups.BackupPath + "\Opensim_WifiPages-Custom")
                My.Computer.FileSystem.CreateDirectory(Backups.BackupPath + "\Opensim_bin_WifiPages-Custom")
            Catch ex As Exception
                BreakPoint.Show(ex.Message)
            End Try
            FileStuff.CopyFolder(IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Opensim\WifiPages\"), IO.Path.Combine(Backups.BackupPath, "Opensim_WifiPages-Custom"))
            FileStuff.CopyFolder(IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Opensim\bin\WifiPages\"), IO.Path.Combine(Backups.BackupPath, "Opensim_bin_WifiPages-Custom"))
            Application.DoEvents()
        End If

        FileStuff.CopyFile(IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Settings.ini"), IO.Path.Combine(Backups.BackupPath, "Settings.ini"), True)

        Dim Bak = Settings.BackupFolder & "\" & Foldername & ".zip"
        Dim counter As Integer = 10
        While counter > 0
            Try
                FileStuff.DeleteFile(Bak)
                ZipFile.CreateFromDirectory(Backups.BackupPath, Bak, CompressionLevel.Optimal, False)
                Thread.Sleep(1000)
                FileStuff.DeleteDirectory(Backups.BackupPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
                counter = 0
            Catch
                counter -= 1
            End Try
        End While

    End Sub

End Module
