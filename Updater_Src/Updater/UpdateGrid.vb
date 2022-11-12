Imports System.Net
Imports System.IO

Imports System.Threading
Imports System.IO.Compression


' Copyright 2019 Fred Beckhusen
' AGPL 3.0 Licensed
' Redistribution and use in binary and source form is permitted provided
' that ALL the licenses in the text files are followed and included in all copies

Public Class UpdateGrid
    Private WithEvents ApacheProcess As New Process()

#Region "Private Fields"

    ReadOnly Filename As String = "DreamGrid.zip"
    Dim MyFolder As String = ""

#End Region

#Region "Public Methods"

    Public Sub TextPrint(ByVal str As String)
        Label1.Text = str
    End Sub

#End Region

#Region "Private Methods"

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        EnsureInitialized()

        Label1.Text = "DreamGrid is downloading"
        Me.Text = "DreamGrid Updater/Installer"

        Me.Left = 100
        Me.Top = 100
        Me.Show()
        Application.DoEvents()
        MyFolder = My.Application.Info.DirectoryPath
        If Debugger.IsAttached = True Then
            MyFolder = "C:\Users\Fred\Downloads\dg"
            If Not IO.Directory.Exists(MyFolder) Then
                IO.Directory.CreateDirectory(MyFolder)
            End If
            ' for testing, as the compiler buries itself in ../../../debug
        End If

        ChDir(MyFolder)
        ProgressBar1.Enabled = True
        ProgressBar1.Maximum = 100
        ProgressBar1.Value = 10

        Dim f1 = New Downloader

        Dim ret = Downloader.ShowDialog()
        If ret = DialogResult.OK Then

            If Not File.Exists(MyFolder & "\" & Filename) Then
                MsgBox("File not found. Aborting.")
                End
            Else

                Application.DoEvents()

                Label1.Text = "Stopping MySQL"
                StopMYSQL()
                ProgressBar1.Value = 20
                Application.DoEvents()
                Thread.Sleep(1000)

                Label1.Text = "Stopping Apache"
                StopApache()
                ProgressBar1.Value = 30
                Application.DoEvents()
                Thread.Sleep(1000)

                Label1.Text = "Stopping Robust"
                For Each p As Process In Process.GetProcessesByName("Robust")
                    p.Kill()
                Next
                ProgressBar1.Value = 40
                Application.DoEvents()
                Thread.Sleep(1000)

                Label1.Text = "Stopping Opensim"
                For Each p As Process In Process.GetProcessesByName("Opensim")
                    p.Kill()
                Next
                ProgressBar1.Value = 50
                Application.DoEvents()
                Thread.Sleep(1000)

                Label1.Text = "Stopping Icecast"
                For Each p As Process In Process.GetProcessesByName("Icecast")
                    p.Kill()
                Next
                ProgressBar1.Value = 60
                Application.DoEvents()
                Thread.Sleep(1000)

                Label1.Text = ""
                Try
                    My.Computer.FileSystem.DeleteDirectory(MyFolder & "\Outworldzfiles\opensim\bin\addin-db-002", FileIO.DeleteDirectoryOption.DeleteAllContents)
                Catch ex As Exception
                End Try
                ProgressBar1.Value = 70
                Application.DoEvents()

                Label1.Text = "Stopping Dreamgrid"
                For Each p As Process In Process.GetProcessesByName("Start")
                    p.Kill()
                Next
                ProgressBar1.Value = 80
                Application.DoEvents()


                Dim err As Integer = 0
                Dim fname As String = ""

                Dim extractPath = Path.GetFullPath(MyFolder)
                If (Not extractPath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal)) Then
                    extractPath += Path.DirectorySeparatorChar
                End If

                Dim ZipContains As Integer = 0
                Dim counter As Integer = 0
                Try
                    Using zip As ZipArchive = ZipFile.Open(MyFolder & "\" & Filename, ZipArchiveMode.Read)
                        ZipContains = zip.Entries.Count

                        Label1.Text = "Unzipping"
                        ProgressBar1.Value = 100
                        Thread.Sleep(1000)
                        Application.DoEvents()

                        For Each ZipEntry In zip.Entries
                            counter += 1
                            Dim percent As Double = counter / ZipContains

                            If percent * 100 > 100 Then percent = 1
                            ProgressBar1.Value = percent * 100
                            Application.DoEvents()
                            fname = ZipEntry.Name
                            If fname.Length = 0 Then
                                Continue For
                            End If
                            If ZipEntry.Name <> "DreamGridUpdater.exe" Then
                                TextPrint(CStr(counter) & " of " & CStr(ZipContains) & ":" & Path.GetFileName(ZipEntry.Name))
                                Application.DoEvents()

                                Dim destinationPath As String = Path.GetFullPath(Path.Combine(extractPath, ZipEntry.FullName))
                                If File.Exists(destinationPath) Then
                                    File.Delete(destinationPath)
                                End If
                                Dim folder = Path.GetDirectoryName(destinationPath)
                                Directory.CreateDirectory(folder)
                                ZipEntry.ExtractToFile(folder & "\" & ZipEntry.Name)
                                Log($"Extracted {ZipEntry.Name}")
                            End If
                        Next
                    End Using


                Catch ex As Exception
                    Log("Unable to extract file: " & fname & ":" & ex.Message)
                    err += 1
                End Try

                If counter <> ZipContains Then
                    err += 1
                    TextPrint($"Aborting, did not extract all files. Stopped at {fname}")
                    Log("Aborting, did not extract all files.")
                    Application.DoEvents()
                    Thread.Sleep(3000)
                    Dim Logname As String = MyFolder & "\Updater.log"
                    Try
                        Dim dest As String = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "Wordpad.exe")

                        Using ApacheProcess As New Process()
                            ApacheProcess.StartInfo.FileName = "notepad.exe"
                            ApacheProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal
                            ApacheProcess.StartInfo.Arguments = Logname
                            ApacheProcess.Start()
                            Application.DoEvents()
                        End Using


                    Catch ex As Exception
                    End Try
                    End
                End If

                If Not err Then
                    ProgressBar1.Value = 100
                    TextPrint("Completed!")
                    Log("Completed!")
                End If

                Application.DoEvents()
                Thread.Sleep(1000)

                ApacheProcess.StartInfo.FileName = "Start.exe"
                ApacheProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal

                Try
                    ApacheProcess.Start()
                Catch ex As Exception
                    TextPrint("Could not start DreamGrid!")
                    Thread.Sleep(5000)
                End Try

                End
            End If
        Else
            TextPrint("Canceled")
            ProgressBar1.Value = 0
        End If

        End

    End Sub

    Private Sub StopApache()

        Using ApacheProcess As New Process()
            ApacheProcess.StartInfo.FileName = "net"
            ApacheProcess.StartInfo.CreateNoWindow = True
            ApacheProcess.StartInfo.WindowStyle = ProcessWindowStyle.Minimized
            ApacheProcess.StartInfo.Arguments = "stop " & "ApacheHTTPServer"
            ApacheProcess.Start()
            Application.DoEvents()
            ApacheProcess.WaitForExit()
            Log("Stopped Apache")
        End Using

        Zap("httpd")
        Zap("rotatelogs")

    End Sub

    Private Sub StopMYSQL()

        Dim p = New Process()
        Dim pi = New ProcessStartInfo With {
            .Arguments = "-u root shutdown",
            .FileName = MyFolder + "\OutworldzFiles\mysql\bin\mysqladmin.exe",
            .WindowStyle = ProcessWindowStyle.Minimized
        }
        p.StartInfo = pi
        Try
            p.Start()
        Catch
        End Try

        pi.Arguments = "-u root -port=3306 shutdown"
        p.StartInfo = pi
        Try
            p.Start()
            p.WaitForExit()
            Log($"Stopped Mysql")
        Catch
            Log($"Failed to stop Mysql")
        End Try


    End Sub

    Private Sub Zap(processName As String)

        ' Kill process by name
        For Each P As Process In System.Diagnostics.Process.GetProcessesByName(processName)
            Try
                P.Kill()
            Catch
                Log($"Failed to stop {processName}")
            End Try
        Next

    End Sub
    Public Sub Log(message As String)
        Try
            Using outputFile As New StreamWriter(MyFolder & "\Updater.log", True)
                outputFile.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + message)
            End Using
        Catch
        End Try

    End Sub
#End Region
End Class
