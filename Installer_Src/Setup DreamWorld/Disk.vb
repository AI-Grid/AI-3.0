Imports System.IO
Imports System.Threading

Module Disk

#Region "Disk"

    Private ReadOnly Sleeping As New List(Of String)

#Region "Freeze"

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
                        PauseRegion(RegionUUID)
                        running = True
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
            ResumeRegion(RegionUUID)
            RegionStatus(RegionUUID) = SIMSTATUSENUM.Booted
        Next

        Sleeping.Clear()
        Busy = False

        PropUpdateView = True ' make form refresh        

    End Sub

#End Region

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

                    FormSetup.DiskSize.Text = $"{x}: {Text} "
                    Exit For
                End If

            Next
        Catch
        End Try

        Return Free

    End Function

#End Region

End Module
