﻿#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.IO
Imports System.Threading
Imports Ionic.Zip

Public Class Backups

    Private _folder As String
    Private _initted As Boolean
    Private _startDate As Date
    Private _WebThread1 As Thread
    Private _WebThread2 As Thread
    Private _WebThread3 As Thread

    Private Shared Sub Break(msg As String)
        Diagnostics.Debug.Print(msg)
    End Sub

#Region "Public"

    Public Sub New()

    End Sub

#End Region

#Region "SQL Backup"

    Public Sub BackupSQLDB(DBName As String)

        If Not StartMySQL() Then
            FormSetup.ToolBar(False)
            FormSetup.Buttons(FormSetup.StartButton)
            TextPrint(My.Resources.Stopped_word)
            Return
        End If

        SQLBackup(DBName)

    End Sub

    Public Sub RunSQLBackup(OP As Object)

        If OP Is Nothing Then Return

        Dim Name As String = OP.ToString
        Try
            Dim currentdatetime As Date = Date.Now()
            Dim whenrun As String = currentdatetime.ToString("yyyy-MM-dd_HH_mm_ss", Globalization.CultureInfo.InvariantCulture)

            'used to zip it, zip it good
            _folder = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\tmp\Backup_" & DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss", Globalization.CultureInfo.InvariantCulture))
            If Not System.IO.Directory.Exists(_folder) Then MkDir(_folder)

            Dim _filename = "Backup_" & Name & "_" & whenrun & ".sql"
            Dim SQLFile = IO.Path.Combine(_folder, _filename)

            ' we must write this to the file so it knows what database to use.
            Using outputFile As New StreamWriter(SQLFile)
                outputFile.Write("use " & Name & ";" + vbCrLf)
            End Using

            Dim ProcessSqlDump = New Process With {
                .EnableRaisingEvents = True
            }

            Dim port As String
            Dim host As String
            Dim password As String
            Dim user As String
            Dim dbname As String
            If OP = Settings.RobustDataBaseName Then
                port = CStr(Settings.MySqlRobustDBPort)
                host = Settings.RobustServerIP
                user = Settings.RobustUsername
                password = Settings.RobustPassword
                dbname = Settings.RobustDataBaseName
            Else
                port = CStr(Settings.MySqlRegionDBPort)
                host = Settings.RegionServer
                user = Settings.RegionDBUsername
                password = Settings.RegionDbPassword
                dbname = Settings.RegionDBName
            End If

            Dim options = " --host=" & host & " --port=" & port _
                & " --opt --hex-blob --add-drop-table --allow-keywords  --skip-lock-tables --compress --quick " _
                & " -u" & user _
                & " -p" & password _
                & " --verbose --log-error=Mysqldump.log " _
                & " --result-file=" & """" & SQLFile & """" _
                & " " & dbname
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
                Break(ex.Message)
            End Try

            ProcessSqlDump.WaitForExit()
            ProcessSqlDump.Close()

            Dim Bak = IO.Path.Combine(_folder, _filename & ".zip")
            DeleteFile(Bak)

            Using Zip As ZipFile = New ZipFile(Bak)
                Zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression
                Zip.AddFile(SQLFile, "/")
                Zip.Save()
            End Using
            Sleep(1000)
            MoveFile(Bak, IO.Path.Combine(BackupPath(), _filename & ".zip"))
            DeleteFile(SQLFile)
            DeleteFolder(_folder)
        Catch ex As Exception
            Break(ex.Message)
        End Try

        If Name = Settings.RegionDBName And Settings.RegionDBName <> Settings.RobustDataBaseName Then
            BackupSQLDB(Settings.RobustDataBaseName)
        End If

    End Sub

    Public Sub SQLBackup(DBName As String)

        Dim currentdatetime As Date = Date.Now()

        _WebThread1 = New Thread(AddressOf RunSQLBackup)
        _WebThread1.SetApartmentState(ApartmentState.STA)
        _WebThread1.Priority = ThreadPriority.BelowNormal
        _WebThread1.Start(DBName)

    End Sub

#End Region

#Region "File Backup"

    Public Sub RunAllBackups(RunNow As Boolean)

        Dim currentdatetime As Date = Date.Now
        If RunNow Then
            _WebThread2 = New Thread(AddressOf FullBackupThread)
            _WebThread2.SetApartmentState(ApartmentState.STA)
            _WebThread2.Priority = ThreadPriority.BelowNormal
            _WebThread2.Start()
            Return
        End If

        If Not _initted Then
            _initted = True
            _startDate = New DateTime()
            _startDate = Date.Now
            Settings.StartDate = _startDate
            Settings.SaveSettings()
        End If

        Dim originalBoottime As Date = Settings.StartDate
        originalBoottime = originalBoottime.AddMinutes(CDbl(Settings.AutobackupInterval))

        If DateTime.Compare(currentdatetime, originalBoottime) > 0 Then
            Settings.StartDate = currentdatetime ' wait another interval
            Settings.SaveSettings()
            If Settings.AutoBackup Then
                TextPrint(currentdatetime.ToLocalTime & " auto backup running")
                _WebThread3 = New Thread(AddressOf FullBackupThread)
                _WebThread3.SetApartmentState(ApartmentState.STA)
                _WebThread3.Priority = ThreadPriority.BelowNormal
                _WebThread3.Start()

            End If
        End If

        Try
            ' delete old files
            originalBoottime = Settings.StartDate

            Dim directory As New System.IO.DirectoryInfo(BackupPath)
            Dim File As System.IO.FileInfo() = directory.GetFiles()
            Dim File1 As System.IO.FileInfo

            ' get each file's last modified date
            For Each File1 In File
                If File1.Name.StartsWith("Backup_", StringComparison.InvariantCultureIgnoreCase) Then
                    Dim strLastModified As Date = System.IO.File.GetLastWriteTime(BackupPath() & "\" & File1.Name)
                    strLastModified = strLastModified.AddDays(CDbl(Settings.KeepForDays))
                    Dim y = DateTime.Compare(currentdatetime, strLastModified)
                    If DateTime.Compare(currentdatetime, strLastModified) > 0 Then
                        DeleteFile(File1.FullName)
                    End If
                End If
            Next
        Catch
        End Try

    End Sub

    Private Sub FullBackupThread()

        'used to zip it, zip it good
        _folder = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\tmp\Backup_" & DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss", Globalization.CultureInfo.InvariantCulture))
        If Not System.IO.Directory.Exists(_folder) Then MkDir(_folder)

        Dim Foldername = "Backup_" + DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss", Globalization.CultureInfo.InvariantCulture)   ' Set default folder
        Dim Bak = IO.Path.Combine(_folder, Foldername & ".zip")
        Dim zipused As Boolean
        Using Z As ZipFile = New ZipFile(Bak) With {
            .CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression
        }

            Try
                If Settings.BackupWifi Then
                    Z.AddDirectory(IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Opensim\WifiPages-Custom\"), "WifiPages-Custom")
                    zipused = True
                End If
            Catch ex As Exception
                Break(ex.Message)
            End Try

            If Settings.BackupSettings Then
                Try
                    Z.AddFile(IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Settings.ini"), "Settings")
                    Z.AddFile(IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\XYSettings.ini"), "Settings")

                    Dim fs = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\OpPensim\bin\LocalUserStatistics.db")
                    If File.Exists(fs) Then Z.AddFile(fs, "Stats Database in bin")
                    fs = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Photo.png")
                    If File.Exists(fs) Then Z.AddFile(fs, "Photos")
                    fs = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\NewBlack.png")
                    If File.Exists(fs) Then Z.AddFile(fs, "Photos")
                    fs = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\NewWhite.png")
                    If File.Exists(fs) Then Z.AddFile(fs, "Photos")
                    fs = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\NewCustom.png")
                    If File.Exists(fs) Then Z.AddFile(fs, "Photos")
                    zipused = True
                Catch
                End Try
            End If

            If Settings.BackupRegion Then
                Dim sourcePath = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Opensim\bin\Regions")
                Dim sourceDirectoryInfo As New System.IO.DirectoryInfo(sourcePath)
                For Each fileSystemInfo In sourceDirectoryInfo.GetDirectories
                    Try
                        Dim folder = fileSystemInfo.FullName
                        Dim Regionpath = IO.Path.Combine(Settings.CurrentDirectory, folder & "\Region")
                        Dim RegionDirectoryInfo As New System.IO.DirectoryInfo(Regionpath)
                        For Each Region In RegionDirectoryInfo.GetFileSystemInfos
                            Dim name = Region.Name
                            If name.EndsWith(".ini", StringComparison.InvariantCulture) Then
                                Dim shortname = name.Replace("ini", "")
                                Z.AddFile(IO.Path.Combine(Regionpath, Region.Name), $"\Regions\{shortname}\Region\")
                                zipused = True
                            End If
                        Next
                    Catch ex As Exception
                        Break(ex.Message)
                    End Try

                Next
            End If

            Try
                If zipused = True Then
                    Z.Save()
                    Sleep(1000)
                    MoveFile(Bak, IO.Path.Combine(BackupPath, Foldername & ".zip"))
                    Sleep(1000)
                End If

                DeleteFolder(_folder)
            Catch
            End Try

            Try
                If Settings.BackupFSAssets Then
                    Dim f As String
                    If Settings.BaseDirectory.ToUpper(Globalization.CultureInfo.InvariantCulture) = "./FSASSETS" Then
                        f = IO.Path.Combine(Settings.OpensimBinPath, "FSAssets")
                    Else
                        f = Settings.BaseDirectory
                    End If
                    If Directory.Exists(f) Then
                        Dim dest = IO.Path.Combine(BackupPath, "FSassets")
                        Dim args = """" & f & """" & " " & """" & dest & """" & " /MIR /TBD /LFSM:50M /IM /M  /J "
                        Dim win = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "robocopy.exe")

                        Using ProcessRobocopy As New Process With {
                            .EnableRaisingEvents = True
                        }
                            Dim pi As ProcessStartInfo = New ProcessStartInfo With {
                                .Arguments = args,
                                .FileName = win,
                                .UseShellExecute = True
                            }
                            ProcessRobocopy.StartInfo = pi
                            ProcessRobocopy.StartInfo.WindowStyle = ProcessWindowStyle.Minimized
                            ProcessRobocopy.StartInfo.CreateNoWindow = True
                            Try
                                ProcessRobocopy.Start()
                            Catch ex As Exception
                                Break(ex.Message)
                            End Try
                        End Using
                    End If
                End If
            Catch ex As Exception
                Break(ex.Message)
            End Try

            Try
                If Settings.BackupSQL Then
                    BackupSQLDB(Settings.RegionDBName)
                End If
            Catch ex As Exception
                Break(ex.Message)
            End Try

        End Using

    End Sub

#End Region

End Class
