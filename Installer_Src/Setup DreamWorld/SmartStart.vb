﻿Module SmartStart

    Private WithEvents BootProcess As New Process

    Public Sub DoSuspend_Resume(RegionName As String, Optional ResumeSwitch As Boolean = False)

        Dim RegionUUID As String = PropRegionClass.FindRegionByName(RegionName)
        Dim PID = PropRegionClass.ProcessID(RegionUUID)

        If True Then
            If ResumeSwitch Then
                Logger("State Changed to RestartPending", RegionName, "Restart")
                PropRegionClass.Status(RegionUUID) = RegionMaker.SIMSTATUSENUM.RestartPending
                PropRegionClass.Timer(RegionUUID) = Date.Now ' wait another interval
                PropUpdateView = True ' make form refresh
            Else
                Logger("State Changed to ShuttingDown", RegionName, "Restart")
                Dim GroupName = PropRegionClass.GroupName(RegionUUID)
                For Each UUID In PropRegionClass.RegionUuidListByName(GroupName)
                    PropRegionClass.Status(UUID) = RegionMaker.SIMSTATUSENUM.ShuttingDown
                    PropRegionClass.Timer(RegionUUID) = Date.Now ' wait another interval
                Next
                ShutDown(RegionUUID)
                Application.DoEvents()
                PropUpdateView = True ' make form refresh
            End If
        Else
            Dim R As String
            If ResumeSwitch Then
                R = " -rpid "
                TextPrint(My.Resources.Resuming_word & " " & RegionName)
            Else
                TextPrint(My.Resources.Suspending_word & " " & RegionName)
                R = " -pid "
            End If
            Dim SuspendProcess As New Process()
            Dim pi As ProcessStartInfo = New ProcessStartInfo With {
                  .Arguments = R & PID,
                  .FileName = """" & IO.Path.Combine(Settings.CurrentDirectory, "NtSuspendProcess64.exe") & """"
              }

            pi.CreateNoWindow = True
            pi.WindowStyle = ProcessWindowStyle.Minimized

            SuspendProcess.StartInfo = pi

            Try
                SuspendProcess.Start()
                SuspendProcess.WaitForExit()
                PropRegionClass.Timer(RegionUUID) = Date.Now ' wait another interval
            Catch ex As Exception
                BreakPoint.Show(ex.Message)
                TextPrint(My.Resources.NTSuspend)
            Finally
                SuspendProcess.Close()
            End Try

            Dim GroupName = PropRegionClass.GroupName(RegionUUID)
            For Each UUID In PropRegionClass.RegionUuidListByName(GroupName)
                If ResumeSwitch Then
                    PropRegionClass.Status(UUID) = RegionMaker.SIMSTATUSENUM.Booted
                Else
                    PropRegionClass.Status(UUID) = RegionMaker.SIMSTATUSENUM.Suspended
                End If
            Next
            PropUpdateView = True ' make form refresh
        End If

    End Sub

#Region "BootUp"

    Public Function Boot(BootName As String) As Boolean
        ''' <summary>Starts Opensim for a given name</summary>
        ''' <param name="BootName">Name of region to start</param>
        ''' <returns>success = true</returns>


        FormSetup.Timer1.Interval = 1000
        FormSetup.Timer1.Start() 'Timer starts functioning

        PropOpensimIsRunning() = True

        If PropAborting Then Return True

        Dim RegionUUID As String = PropRegionClass.FindRegionByName(BootName)
        Dim GroupName = PropRegionClass.GroupName(RegionUUID)

        If String.IsNullOrEmpty(RegionUUID) Then
            ErrorLog("Cannot find " & BootName & " to boot!")
            Logger("Cannot find", BootName, "Restart")
            Return False
        End If
        Logger("Info", "Region: Starting Region " & BootName, "Restart")

        DoGloebits()

        PropRegionClass.CopyOpensimProto(RegionUUID)

        Dim GP = PropRegionClass.GroupPort(RegionUUID)
        Diagnostics.Debug.Print("Group port =" & CStr(GP))
        Application.DoEvents()
        Dim isRegionRunning As Boolean = CheckPort("127.0.0.1", GP)
        If isRegionRunning Then
            If PropRegionClass.Status(RegionUUID) = RegionMaker.SIMSTATUSENUM.Suspended Then
                Logger("Suspended, Resuming it", BootName, "Restart")

                Dim PID As Integer = GetPIDofWindow(GroupName)
                If Not FormSetup.PropInstanceHandles.ContainsKey(PID) Then FormSetup.PropInstanceHandles.Add(PID, GroupName)
                For Each UUID As String In PropRegionClass.RegionUuidListByName(GroupName)
                    PropRegionClass.Status(UUID) = RegionMaker.SIMSTATUSENUM.Resume
                    PropRegionClass.ProcessID(UUID) = PID
                Next

                Logger("Info", "Region " & BootName & " skipped as it is Suspended, Resuming it instead", "Restart")
                PropUpdateView = True ' make form refresh
                Return True
            Else    ' needs to be captured into the event handler
                Log(My.Resources.Info_word, "Region " & BootName & " skipped as it is already up")

                Dim PID As Integer = GetPIDofWindow(GroupName)
                If Not FormSetup.PropInstanceHandles.ContainsKey(PID) Then FormSetup.PropInstanceHandles.Add(PID, GroupName)
                For Each UUID As String In PropRegionClass.RegionUuidListByName(GroupName)
                    PropRegionClass.Status(UUID) = RegionMaker.SIMSTATUSENUM.Booted
                    PropRegionClass.Timer(UUID) = Date.Now
                    PropRegionClass.ProcessID(UUID) = PID
                Next

                PropUpdateView = True ' make form refresh
                Return True
            End If
        End If

        TextPrint(BootName & " " & Global.Outworldz.My.Resources.Starting_word)
        Application.DoEvents()
        BootProcess.EnableRaisingEvents = True
        BootProcess.StartInfo.UseShellExecute = True
        BootProcess.StartInfo.WorkingDirectory = Settings.OpensimBinPath()

        BootProcess.StartInfo.FileName = """" & Settings.OpensimBinPath() & "OpenSim.exe" & """"
        BootProcess.StartInfo.CreateNoWindow = False

        Select Case Settings.ConsoleShow
            Case "True"
                BootProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal
            Case "False"
                BootProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal
            Case "None"
                BootProcess.StartInfo.WindowStyle = ProcessWindowStyle.Minimized
        End Select

        BootProcess.StartInfo.Arguments = " -inidirectory=" & """" & "./Regions/" & GroupName & """"

        Environment.SetEnvironmentVariable("OSIM_LOGPATH", Settings.OpensimBinPath() & "Regions\" & GroupName)

        FormSetup.SequentialPause()   ' wait for previous region to give us some CPU
        Logger("Booting", GroupName, "Restart")

        Dim ok As Boolean = False
        Try
            ok = BootProcess.Start
            Application.DoEvents()
        Catch ex As Exception
            BreakPoint.Show(ex.Message)
            ErrorLog(ex.Message)
        End Try

        If ok Then
            Dim PID = WaitForPID(BootProcess)           ' check if it gave us a PID, if not, it failed.

            If PID > 0 Then
                SetWindowTextCall(BootProcess, GroupName)
                If Not FormSetup.PropInstanceHandles.ContainsKey(PID) Then FormSetup.PropInstanceHandles.Add(PID, GroupName)
                ' Mark them before we boot as a crash will immediately trigger the event that it exited
                For Each UUID As String In PropRegionClass.RegionUuidListByName(GroupName)
                    PropRegionClass.Status(UUID) = RegionMaker.SIMSTATUSENUM.Booting
                    PropRegionClass.Timer(RegionUUID) = Date.Now()
                Next
            Else
                BreakPoint.Show("No PID for " & GroupName)
            End If

            For Each UUID As String In PropRegionClass.RegionUuidListByName(GroupName)
                PropRegionClass.ProcessID(UUID) = PID
            Next
            PropUpdateView = True ' make form refresh
            FormSetup.Buttons(FormSetup.StopButton)
            Return True
        End If
        PropUpdateView = True ' make form refresh
        Logger("Failed to boot ", BootName, "Restart")
        TextPrint("Failed to boot region " & BootName)
        Return False

    End Function

#End Region

End Module
