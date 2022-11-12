Module FreezeThaw

#Region "FreezeThaw"

    ''' <summary>
    ''' Freeze a region
    ''' </summary>
    ''' <param name="RegionUUID">Region UUID</param>

    Public Sub Freeze(RegionUUID As String)

        If Smart_Start(RegionUUID) And RegionEnabled(RegionUUID) And Settings.Smart_Start And Settings.BootOrSuspend = False Then
            Dim PID = ProcessID(RegionUUID)
            ShowDOSWindow(RegionUUID, MaybeHideWindow())
            RegionStatus(RegionUUID) = SIMSTATUSENUM.Suspended
            NtSuspendProcess(CachedProcess(PID).Handle)
        End If

    End Sub

    ''' <summary>
    ''' Suspends region
    ''' </summary>
    ''' <param name="RegionUUID">RegionUUID</param>
    Public Sub PauseRegion(RegionUUID As String)

        If Settings.Smart_Start And Smart_Start(RegionUUID) And Settings.Smart_Start And Settings.BootOrSuspend = False Then
            Dim Groupname = Group_Name(RegionUUID)
            For Each UUID As String In RegionUuidListByName(Groupname)
                BreakPoint.Print($"Pausing {Region_Name(UUID)}")
                Freeze(UUID)
            Next

        End If

    End Sub

    ''' <summary>
    ''' Thaw a region
    ''' </summary>
    ''' <param name="RegionUUID">Region UUID</param>
    Public Sub Thaw(RegionUUID As String)

        Dim PID = ProcessID(RegionUUID)
        Try
            NtResumeProcess(CachedProcess(PID).Handle)
        Catch
        End Try

    End Sub

#End Region

End Module
