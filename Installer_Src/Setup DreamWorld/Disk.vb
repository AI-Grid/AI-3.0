﻿Imports System.IO
Imports System.Threading

Module Disk

#Region "Disk"

    Private ReadOnly Sleeping As New List(Of String)

    Public Function CalcDiskFree() As Long

        Dim d = DriveInfo.GetDrives()
        Dim c = CurDir()
        Dim Free As Long
        Try
            For Each drive As DriveInfo In d
                Dim x = Mid(c, 1, 1)
                If x = Mid(drive.Name, 1, 1) Then
                    Dim Percent = drive.AvailableFreeSpace / drive.TotalSize
                    Dim FreeDisk = Percent * 100
                    Free = drive.AvailableFreeSpace

                    If Sleeping.Count = 0 Then
                        If Free < FreeDiskSpaceWarn Then
                            Dim SThread = New Thread(AddressOf FreezeAll)
                            SThread.SetApartmentState(ApartmentState.STA)
                            SThread.Priority = ThreadPriority.BelowNormal ' UI gets priority
                            SThread.Start()
                            Busy = True
                            MsgBox(My.Resources.Diskspacelow & $" {Free:n0} Bytes", vbInformation Or MsgBoxStyle.MsgBoxSetForeground)
                        End If
                    End If

                    Dim tt = My.Resources.Available
                    Dim Text = $"{Percent:P1} {tt}"

                    FormSetup.DiskSize.Text = $"Disk {x}: {Text} "
                    Exit For
                End If

            Next
        Catch
        End Try

        Return Free

    End Function

    Public Sub FreezeAll()

        Dim running As Boolean
        For Each RegionUUID In RegionUuids()
            Dim status = RegionStatus(RegionUUID)
            If Not Sleeping.Contains(RegionUUID) Then
                Sleeping.Add(RegionUUID)
                Select Case status
                    Case SIMSTATUSENUM.Booted
                        PauseRegion(RegionUUID)
                        running = True
                    Case SIMSTATUSENUM.Booting
                        PauseRegion(RegionUUID)
                        running = True
                    Case SIMSTATUSENUM.Error
                    Case SIMSTATUSENUM.NoLogin
                        PauseRegion(RegionUUID)
                        running = True
                    Case SIMSTATUSENUM.RecyclingDown
                        PauseRegion(RegionUUID)
                        running = True
                    Case SIMSTATUSENUM.RecyclingUp
                        PauseRegion(RegionUUID)
                        running = True
                    Case SIMSTATUSENUM.RestartPending
                    Case SIMSTATUSENUM.RestartStage2
                        PauseRegion(RegionUUID)
                        running = True
                    Case SIMSTATUSENUM.RetartingNow
                        PauseRegion(RegionUUID)
                        running = True
                    Case SIMSTATUSENUM.Stopped
                    Case SIMSTATUSENUM.Suspended
                End Select
            End If
        Next

        PropUpdateView = True ' make form refresh
        If Not running Then
            Busy = False
            Sleeping.Clear()
            Return
        End If

        While CalcDiskFree() < FreeDiskSpaceWarn AndAlso PropOpensimIsRunning
            Sleep(1000)
        End While

        For Each RegionUUID In Sleeping
            Dim RegionName = Region_Name(RegionUUID)

            If ProcessID(RegionUUID) > 0 Then
                ResumeRegion(RegionUUID)
            End If

            RegionStatus(RegionUUID) = SIMSTATUSENUM.Booted
        Next

        Sleeping.Clear()
        Busy = False

        PropUpdateView = True ' make form refresh
        'Application.ExitThread()

    End Sub

#End Region

#Region "FreeThaw"

    ''' <summary>
    ''' Suspends region
    ''' </summary>
    ''' <param name="RegionUUID">RegionUUID</param>
    Public Sub PauseRegion(RegionUUID As String)

        Diagnostics.Debug.Print($"Pausing {Region_Name(RegionUUID)}")

        FreezeThaw(RegionUUID, "-pid " & ProcessID(RegionUUID))
        RegionStatus(RegionUUID) = SIMSTATUSENUM.Suspended
        PropUpdateView = True ' make form refresh

    End Sub

    ''' <summary>
    ''' Resumes Region from frozen state
    ''' </summary>
    ''' <param name="RegionUUID">RegionUUID</param>
    ''' <returns>0 if success</returns>
    Public Function ResumeRegion(RegionUUID As String) As Boolean

        If Settings.Smart_Start AndAlso Smart_Start(RegionUUID) = "" Or Smart_Start(RegionUUID) = "False" Then
            Return True
        End If
        If ProcessID(RegionUUID) = 0 Then
            Return True
        End If

        Diagnostics.Debug.Print($"Resume {Region_Name(RegionUUID)}")

        FreezeThaw(RegionUUID, "-rpid " & ProcessID(RegionUUID))
        If CBool(GetHwnd(Group_Name(RegionUUID))) Then
            RegionStatus(RegionUUID) = SIMSTATUSENUM.Booted
            PropUpdateView = True ' make form refresh
            TeleportAgents()
        Else
            ReBoot(RegionUUID)
            TeleportAgents()
        End If

        Return True

    End Function

    Private Function FreezeThaw(RegionUUID As String, Arg As String) As Boolean

        Dim result As Boolean
        Using SuspendProcess As New Process()
            Dim pi = New ProcessStartInfo With {
                .Arguments = Arg,
                .FileName = """" & IO.Path.Combine(Settings.CurrentDirectory, "NtSuspendProcess64.exe") & """"
            }

            pi.CreateNoWindow = True
            pi.WindowStyle = ProcessWindowStyle.Minimized

            SuspendProcess.StartInfo = pi

            Try
                SuspendProcess.Start()
                SuspendProcess.WaitForExit()
                PokeRegionTimer(RegionUUID)
            Catch ex As Exception
                BreakPoint.Dump(ex)
                result = True
            End Try
        End Using
        Return result

    End Function

#End Region

End Module
