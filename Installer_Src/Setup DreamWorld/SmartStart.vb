#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC. AGPL3.0 https://opensource.org/licenses/AGPL

#End Region

Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Threading

Module SmartStart
    Public ReadOnly BootedList As New List(Of String)
    Public ReadOnly ProcessIdDict As New Dictionary(Of Integer, Process)
    Public MyCPUCollection As New List(Of Double)
    Public MyRAMCollection As New List(Of Double)
    Public ToDoList As New Dictionary(Of String, TaskObject)
    Public Visitor As New Dictionary(Of String, String)

    Private ToDoCount As New Dictionary(Of String, Integer)

    ''' <summary>
    ''' The list of commands
    ''' </summary>
    Public Enum TaskName As Integer

        None = 0
        LaunchBackupper = 1        ' run backups via XMLRPC
        TeleportClicked = 2     ' click the teleport button in the region pop up
        LoadOar = 3             ' for Loading a series of OARS
        LoadOneOarTask = 4      ' loading One Oar
        LoadOARContent = 5      ' From the map click
        SaveOneOAR = 6          ' Save this one OAR click
        RebuildTerrain = 7      ' Smart Terrain
        SaveTerrain = 8        ' Dump one region to disk
        ApplyTerrainEffect = 9 ' Change the terrain
        TerrainLoad = 10      ' Change one of them
        ApplyPlant = 11        ' Plant trees
        BakeTerrain = 12       ' save it permanently
        LoadAllFreeOARs = 13   ' the big Kaunas of all oars at once
        DeleteTree = 14        ' kill off all trees
        Revert = 15             ' revert terrain
        SaveAllIARS = 16        ' save all IARS

    End Enum

    Public Sub BuildLand(Avatars As List(Of AvatarObject))

        If Not Settings.AutoFill Then Return
        If Avatars.Count = 0 Then Return

        For Each AgentObject In Avatars

            Dim RegionUUID = AgentObject.RegionID
            Dim RegionName As String

            RegionName = Region_Name(RegionUUID)
            If RegionName Is Nothing Then Continue For

            If RegionName.Length > 0 Then
                Dim X = Coord_X(RegionUUID)
                Dim Y = Coord_Y(RegionUUID)
                If X = 0 Or Y = 0 Then Continue For

                Try
                    SurroundingLandMaker(RegionUUID)
                Catch ex As Exception
                    BreakPoint.Dump(ex)
                End Try

            End If
        Next

    End Sub

    ''' <summary>
    ''' Scan if any booted up, if so runs the futures task list
    ''' </summary>
    Public Sub CheckForBootedRegions()

        ' booted regions from web server
        Bench.Start("Booted list")
        Try
            Dim GroupName As String = ""

            While BootedList.Count > 0

                Dim RegionUUID As String = ""
                RegionUUID = BootedList(0)
                BootedList.RemoveAt(0)

                If PropAborting Then Return
                If Not PropOpensimIsRunning() Then Return
                If Not RegionEnabled(RegionUUID) Then Continue While

                Dim RegionName = Region_Name(RegionUUID)
                GroupName = Group_Name(RegionUUID)

                ' see how long it has been since we booted
                Dim seconds = DateAndTime.DateDiff(DateInterval.Second, Timer(RegionUUID), DateTime.Now)
                If seconds < 0 Then seconds = 0

                TextPrint($"{RegionName} {My.Resources.Boot_Time}:  {CStr(seconds)} {My.Resources.Seconds_word}")
                PokeRegionTimer(RegionUUID)

                SendToOpensimWorld(RegionUUID, 0) ' let opensim world know we are up.

                RegionStatus(RegionUUID) = SIMSTATUSENUM.Booted
                ShowDOSWindow(RegionUUID, MaybeHideWindow())

                If Settings.MapType = "None" AndAlso MapType(RegionUUID).Length = 0 Then
                    BootTime(RegionUUID) = CInt(seconds)
                Else
                    MapTime(RegionUUID) = CInt(seconds)
                End If

                TeleportAgents()

                If Estate(RegionUUID) = "SimSurround" Then
                    Landscape(RegionUUID, RegionName)
                End If

                PropUpdateView = True

            End While
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try
        Bench.Print("Booted list")

        Bench.Start("Scan Region State")
        Try

            For Each RegionUUID In RegionUuids()

                If PropAborting Then Continue For
                If Not PropOpensimIsRunning() Then Continue For

                Try
                    If Not RegionEnabled(RegionUUID) Then Continue For
                Catch ex As Exception
                    BreakPoint.Dump(ex)
                End Try

                Dim RegionName = Region_Name(RegionUUID)
                Dim GroupName = Group_Name(RegionUUID)
                Dim status = RegionStatus(RegionUUID)

                ' if anyone is in home stay alive
                If AvatarsIsInGroup(GroupName) Then
                    PokeRegionTimer(RegionUUID)
                End If

                RunTaskList(RegionUUID)

                If Settings.Smart_Start Then

                    Dim Nearby = AvatarIsNearby(RegionUUID)
                    ' If a region is stopped or suspended, boot it if someone is nearby
                    If status = SIMSTATUSENUM.Stopped _
                        Or status = SIMSTATUSENUM.Suspended Then
                        If Nearby Then
                            TextPrint($"{GroupName} {My.Resources.StartingNearby}")
                            ResumeRegion(RegionUUID)
                            Continue For
                        End If
                    End If

                    ' keep smart start regions alive if someone is near
                    If Nearby Then
                        PokeRegionTimer(RegionUUID)
                    End If

                    ' Smart Start Timer
                    If Smart_Start(RegionUUID) AndAlso status = SIMSTATUSENUM.Booted Then
                        Dim diff = DateAndTime.DateDiff(DateInterval.Second, Timer(RegionUUID), Date.Now)
                        If diff < 0 Then diff = 0

                        If diff > Settings.SmartStartTimeout AndAlso RegionName <> Settings.WelcomeRegion Then
                            BreakPoint.Print($"State Changed to ShuttingDown {GroupName} ")
                            If Settings.BootOrSuspend Then
                                ShutDown(RegionUUID, SIMSTATUSENUM.ShuttingDownForGood)
                            Else
                                PauseRegion(RegionUUID)
                                For Each UUID In RegionUuidListByName(GroupName)
                                    RegionStatus(UUID) = SIMSTATUSENUM.Suspended
                                Next
                            End If

                            PropUpdateView = True ' make form refresh
                            Application.DoEvents()
                            Continue For
                        End If
                    End If

                End If

                ' suspended timers need to run to auto restart
                If RegionStatus(RegionUUID) = SIMSTATUSENUM.Suspended Then
                    PokeRegionTimer(RegionUUID)
                End If

                ' auto restart timer

                Dim time2restart = Timer(RegionUUID).AddMinutes(Convert.ToDouble(Settings.AutoRestartInterval, Globalization.CultureInfo.InvariantCulture))
                Dim Expired As Integer = DateTime.Compare(Date.Now, time2restart)
                If Expired < 0 Then Expired = 0

                If RegionStatus(RegionUUID) = SIMSTATUSENUM.Booted _
                    AndAlso Expired > 0 _
                    AndAlso Settings.AutoRestartInterval() > 0 _
                    AndAlso Settings.AutoRestartEnabled Then

                    If AvatarsIsInGroup(GroupName) Then
                        ' keep smart start regions alive if someone is near
                        If AvatarIsNearby(RegionUUID) Then
                            PokeRegionTimer(RegionUUID)
                        End If
                        Continue For
                    Else

                        ' shut down the group when AutoRestartInterval has gone by.
                        BreakPoint.Print("State Is Time Exceeded, shutdown")

                        ShowDOSWindow(RegionUUID, MaybeShowWindow())
                        SequentialPause()
                        PokeRegionTimer(RegionUUID)
                        ' shut down all regions in the DOS box
                        ShutDown(RegionUUID, SIMSTATUSENUM.RecyclingDown)

                        BreakPoint.Print("State changed to ShuttingDownForGood")
                        TextPrint(GroupName & " " & Global.Outworldz.My.Resources.Exit__word)
                        PropUpdateView = True
                        Continue For
                    End If
                End If

                ' if a RestartPending is signaled, boot it up
                If status = SIMSTATUSENUM.RestartPending Then

                    'RestartPending = 6
                    If PropAborting Then Continue For
                    If Not PropOpensimIsRunning() Then Continue For

                    BreakPoint.Print("State Is RestartPending")
                    Dim GroupList As List(Of String) = RegionUuidListByName(GroupName)
                    For Each R As String In GroupList
                        PokeRegionTimer(RegionUUID)
                        Boot(RegionName)
                    Next

                    BreakPoint.Print("State Is now Booted")
                    PropUpdateView = True
                    Continue For
                End If

                If status = SIMSTATUSENUM.Resume Then
                    '[Resume] = 8
                    If PropAborting Then Continue For
                    If Not PropOpensimIsRunning() Then Continue For
                    ResumeRegion(RegionUUID)
                    BreakPoint.Print($"{GroupName} Is Resuming")
                    Dim GroupList As List(Of String) = RegionUuidListByName(GroupName)
                    For Each R As String In GroupList
                        If RegionEnabled(RegionUUID) Then
                            Boot(RegionName)
                        End If
                        RunTaskList(RegionUUID)
                    Next
                    PropUpdateView = True
                    Continue For
                End If

                If status = SIMSTATUSENUM.RestartStage2 Then
                    'RestartStage2 = 11
                    If PropAborting Then Continue For
                    If Not PropOpensimIsRunning() Then Continue For

                    TextPrint(GroupName & " " & Global.Outworldz.My.Resources.Restart_Pending_word)
                    Dim GroupList As List(Of String) = RegionUuidListByName(GroupName)
                    For Each R In GroupList
                        RegionStatus(R) = SIMSTATUSENUM.RestartPending
                        PokeRegionTimer(RegionUUID)
                        BreakPoint.Print($"State changed to RestartPending {Region_Name(R)}")
                    Next
                    PropUpdateView = True ' make form refresh
                    Continue For
                End If

                Application.DoEvents()
            Next
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

        Bench.Print("Scan Region State")

    End Sub

    Public Sub GetAllAgents()

        Bench.Start("GetAllAgents")
        ' Scan all the regions
        CachedAvatars.Clear()
        ' List of AvatarObject
        CachedAvatars = GetPresence() ' local usres

        Dim HGUsers = GetGridUsers() ' HG users
        For Each item In HGUsers
            CachedAvatars.Add(item)
        Next

        Return

    End Sub

    ''' <summary>
    ''' Queue a task to occur after a region is booted.
    ''' </summary>
    ''' <param name="RegionUUID">Region UUID</param>
    ''' <param name="Taskname">A Task Name</param>
    Public Sub RebootAndRunTask(RegionUUID As String, TObj As TaskObject)

        BreakPoint.Print($"{Region_Name(RegionUUID)} task {TObj.TaskName}")

        ' TODO add task queue
        ' so we can have more than one command
        'TaskQue.Add(TObj)
        If ToDoList.ContainsKey(RegionUUID) Then
            ToDoList(RegionUUID) = TObj
        Else
            ToDoList.Add(RegionUUID, TObj)
        End If

        ResumeRegion(RegionUUID)

        Application.DoEvents()
        If RegionStatus(RegionUUID) = SIMSTATUSENUM.Booted Then
            RunTaskList(RegionUUID)
        End If

    End Sub

    ''' <summary>
    ''' Run a task after region boots
    ''' </summary>
    ''' <param name="RegionUUID">RegionUUID</param>
    Public Sub RunTaskList(RegionUUID As String)

        If ToDoList.ContainsKey(RegionUUID) Then
            Try

                Dim Task = ToDoList.Item(RegionUUID)
                BreakPoint.Print($"Pending tasks for {Region_Name(RegionUUID)}")
                Try
                    ' stop trying after a period of time
                    ToDoCount(RegionUUID) = ToDoCount.Item(RegionUUID) + 1
                    If ToDoCount.Item(RegionUUID) > 120 Then
                        ToDoList.Remove(RegionUUID)
                        ToDoCount(RegionUUID) = 0
                        Return
                    End If
                Catch
                    ToDoCount(RegionUUID) = 1
                End Try

                If RegionStatus(RegionUUID) = SIMSTATUSENUM.Booted Then
                    BreakPoint.Print($"Running tasks for {Region_Name(RegionUUID)}")
                    ToDoList.Remove(RegionUUID)

                    ShowDOSWindow(RegionUUID, MaybeShowWindow())

                    Dim T = Task.TaskName
                    Select Case T
                        Case TaskName.LaunchBackupper      '1
                            Backupper(RegionUUID, Task.Command)
                        Case TaskName.TeleportClicked   '2
                            TeleportClicked(RegionUUID)
                        Case TaskName.LoadOar   '2
                            LoadOar(RegionUUID)
                        Case TaskName.LoadOneOarTask    '4
                            LoadOneOarTask(RegionUUID, Task)
                        Case TaskName.LoadOARContent    '5
                            LoadOARContent2(RegionUUID, Task)
                        Case TaskName.SaveOneOAR    '6
                            SaveOneOar(RegionUUID, Task)
                        Case TaskName.RebuildTerrain    '7
                            RebuildTerrain(RegionUUID)
                        Case TaskName.SaveTerrain  '8
                            Save_Terrain(RegionUUID)
                        Case TaskName.ApplyTerrainEffect    '9
                            ApplyTerrainEffect(RegionUUID)
                        Case TaskName.TerrainLoad       '10
                            Load_Save(RegionUUID)
                        Case TaskName.ApplyPlant       '11
                            Apply_Plant(RegionUUID)
                        Case TaskName.BakeTerrain      '12
                            Bake_Terrain(RegionUUID)
                        Case TaskName.LoadAllFreeOARs  '13
                            Load_AllFreeOARs(RegionUUID, Task)
                        Case TaskName.DeleteTree       '14
                            Delete_Tree(RegionUUID)
                        Case TaskName.Revert             '15
                            Revert(RegionUUID)
                        Case TaskName.SaveAllIARS        '16
                            SaveThreadIARS()
                        Case Else
                            BreakPoint.Print("Impossible task")
                    End Select
                End If
            Catch
                Debug.Print("weird error")
            End Try
        End If

    End Sub

    Public Sub SequentialPause()

        ''' <summary>
        ''' 0 for no waiting
        ''' 1 for Sequential
        ''' 2 for concurrent
        ''' ''' </summary>
        '''
        If Settings.SequentialMode = 0 Then
            Return
        End If

        Dim ctr = 5 * 60 ' 5 minute max to start a region at 100% CPU
        While True

            Dim wait As Boolean = False
            For Each RegionUUID In RegionUuids()
                Dim status = RegionStatus(RegionUUID)

                ' if we are a shutdown type region, we must wait

                If status = SIMSTATUSENUM.Booting And
                    Settings.Smart_Start And
                    Settings.BootOrSuspend And
                    Smart_Start(RegionUUID) Then
                    PokeRegionTimer(RegionUUID)
                    BreakPoint.Print($"Waiting On {Region_Name(RegionUUID)}")
                    wait = True
                    Exit For
                    ' could be a regular region so we wait
                ElseIf status = SIMSTATUSENUM.Booting And (Not Settings.Smart_Start Or Not Smart_Start(RegionUUID)) Then
                    PokeRegionTimer(RegionUUID)
                    BreakPoint.Print($"Waiting On {Region_Name(RegionUUID)}")
                    wait = True
                    Exit For
                End If
            Next

            If Settings.SequentialMode = 1 Then

                ' smart mode can wait on just CPU, not RAM
                If Not Settings.Smart_Start Then
                    If (FormSetup.CPUAverageSpeed > Settings.CpuMax Or Settings.Ramused > 90) Then
                        wait = True
                    End If
                Else
                    If (FormSetup.CPUAverageSpeed > Settings.CpuMax) Then
                        wait = True
                    Else
                        wait = False
                    End If
                End If

            End If

            If Not wait Then Exit While
            Application.DoEvents()

            ctr -= 1
            If ctr <= 0 Then
                Exit While
            End If
            Sleep(1000)
        End While

    End Sub

    Public Sub TeleportClicked(Regionuuid As String)

        Dim RegionName = Region_Name(Regionuuid)
        ResumeRegion(Regionuuid)
        PokeRegionTimer(Regionuuid)
        RegionName = System.Net.WebUtility.UrlEncode(RegionName)

        Dim link = $"hop://{Settings.PublicIP}:{Settings.HttpPort}/{RegionName}/128/128/35"
        Try
            System.Diagnostics.Process.Start(link)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

    End Sub

#Region "StartStart"

    ''' <summary>
    ''' Stops and deletes a region and all the DB things it came with
    ''' </summary>
    ''' <param name="RegionUUID"></param>
    Public Sub DeleteAllRegionData(RegionUUID As String)

        Dim RegionName = Region_Name(RegionUUID)
        Dim GroupName = Group_Name(RegionUUID)
        PropAborting = True
        ShutDown(RegionUUID, SIMSTATUSENUM.ShuttingDownForGood)
        ' wait 2 minute for the region to quit
        Dim ctr = 120

        While PropOpensimIsRunning AndAlso
            RegionStatus(RegionUUID) <> SIMSTATUSENUM.Stopped AndAlso
            RegionStatus(RegionUUID) <> SIMSTATUSENUM.Error AndAlso
            RegionStatus(RegionUUID) <> SIMSTATUSENUM.ShuttingDownForGood
            Sleep(1000)
            ctr -= 1
            If ctr = 0 Then Exit While
        End While

        DeleteAllContents(RegionUUID)
        PropAborting = False
        PropChangedRegionSettings = True
        PropUpdateView = True

    End Sub

    Private Function AddEm(RegionUUID As String, AgentID As String, Teleport As Boolean) As Boolean

        If RegionUUID = "00000000-0000-0000-0000-000000000000" Then
            BreakPoint.Print("UUID Zero")
            Logger("Addem", "Bad Region UUID", "Teleport")
            Return True
        End If

        Dim result As New Guid
        If Not Guid.TryParse(RegionUUID, result) Then
            Logger("Addem", "Bad Region UUID", "Teleport")
            Return False
        End If

        Logger("Teleport Request", Region_Name(RegionUUID) & ":" & AgentID, "Teleport")

        If Teleport Then
            If TeleportAvatarDict.ContainsKey(AgentID) Then
                TeleportAvatarDict.TryRemove(AgentID, "")
            End If
            TeleportAvatarDict(AgentID) = RegionUUID
        End If

        ResumeRegion(RegionUUID) ' Wait for it to start booting

        Application.DoEvents()
        Return False

    End Function

#End Region

#Region "HTML"

    Public Function RegionListHTML(Data As String) As String

        ' there is only a 16KB capability in Opensim for reading a web page.
        ' so we have to ask for a 1-32, 2-64 size chunks for larger grids

        ' Added Start and End integers
        '  ?Start=1&End=32

        Dim startRegion As Integer = 1
        Dim Count As Integer = 256 ' a default for older signs

        Dim pattern = New Regex("Start=(\d+?)&Count=(\d+)", RegexOptions.IgnoreCase)
        Dim match As Match = pattern.Match(Data)
        If match.Success Then
            Integer.TryParse(Uri.UnescapeDataString(match.Groups(1).Value), startRegion)
            Integer.TryParse(Uri.UnescapeDataString(match.Groups(2).Value), Count)
        End If

        ' http://localhost:8001/teleports.htm
        ' http://YourURL:8001/teleports.htm
        'Outworldz|Welcome||outworldz.com:9000:Welcome|128,128,96|
        '*|Welcome||outworldz.com9000Welcome|128,128,96|
        Dim HTML As String = ""

        ' whole lotta sorting going on as the RegionUUID list is not sorted.
        Dim ToSort As New List(Of String)
        For Each RegionUUID In RegionUuids()
            ToSort.Add(Region_Name(RegionUUID))
        Next
        ToSort.Sort() ' not it is sorted

        ' but we want Welcome at the very beginning of the 1st sign
        Dim NewSort As New List(Of String)
        If startRegion = 1 Then        ' first sign
            NewSort.Add(Settings.WelcomeRegion)
        Else
            startRegion -= 1
        End If

        For Each item In ToSort
            If item <> Settings.WelcomeRegion Then
                NewSort.Add(item)
            End If
        Next

        Dim ctr = 1
        Dim used = 1
        For Each RegionName In NewSort

            Dim RegionUUID = FindRegionByName(RegionName)

            ' only print the ones inclusive between startRegion and lastRegion
            If ctr >= startRegion And used <= Count Then
                If Teleport_Sign(RegionUUID) AndAlso RegionEnabled(RegionUUID) Then
                    HTML += $"*|{RegionName}||{Settings.PublicIP}:{Settings.HttpPort}:{RegionName}||{RegionName}|{vbCrLf}"
                    used += 1
                End If
            End If
            ctr += 1
        Next

        Dim HTMLFILE = Settings.OpensimBinPath & "data\teleports.htm"
        DeleteFile(HTMLFILE)

        Try
            Using outputFile As New StreamWriter(HTMLFILE, False)
                outputFile.WriteLine(HTML)
            End Using
        Catch ex As Exception
        End Try

        Return HTML

    End Function

    ''' <summary>
    ''' Parses web server input
    ''' </summary>
    ''' <param name="post"></param>
    ''' <returns></returns>
    Public Function SmartStartParse(post As String) As String

        ' Smart Start AutoStart Region mode
        Logger("Teleport", "Smart Start:" + post, "Teleport")

        'Smart Start:http://192.168.2.140:8999/?alt=Deliverance_of_JarJar_Binks__Fred_Beckhusen_1X1&agent=Ferd%20Frederix&AgentID=6f285c43-e656-42d9-b0e9-a78684fee15d&password=XYZZY

        Dim pattern = New Regex("alt=(.*?)&agent=(.*?)&agentid=(.*?)&password=(.*)", RegexOptions.IgnoreCase)
        Dim match As Match = pattern.Match(post)
        If match.Success Then
            Dim Name As String = Uri.UnescapeDataString(match.Groups(1).Value)
            'Logger("Teleport", $"Name={Name}", "Teleport")

            Dim TeleportType As String = Uri.UnescapeDataString(match.Groups(2).Value)
            'Logger("Teleport", $"TeleportType={TeleportType}", "Teleport")

            Dim AgentID As String = Uri.UnescapeDataString(match.Groups(3).Value)
            'Logger("Teleport", $"AgentID={AgentID}", "Teleport")

            Dim Password As String = Uri.UnescapeDataString(match.Groups(4).Value)
            'Logger("Teleport", $"Password={Password}", "Teleport")

            If Password <> Settings.MachineId Then
                Logger("ERROR", $"Bad Password {Password} for Teleport system. Should be the Dyn DNS password.", "Outworldz")
                Return ""
            End If

            ' Region may be a name or a Region UUID
            Dim RegionUUID = FindRegionByName(Name)
            If RegionUUID.Length = 0 Then
                RegionUUID = Name ' Its a UUID
            Else
                Name = Region_Name(RegionUUID)
                If Name.Length = 0 Then
                    Return RegionUUID
                End If
            End If
            'Debug.Print("Teleport to " & Name)

            ' Smart Start below here

            If Smart_Start(RegionUUID) AndAlso Settings.Smart_Start Then

                If RegionEnabled(RegionUUID) Then

                    ' smart, and up
                    If RegionStatus(RegionUUID) = SIMSTATUSENUM.Booted Then
                        If TeleportType.ToUpperInvariant = "UUID" Then
                            'Logger("Already Booted UUID Teleport", Name & ":" & AgentID, "Teleport")
                            Return RegionUUID
                        ElseIf TeleportType.ToUpperInvariant = "REGIONNAME" Then
                            'Logger("Already Booted Named Teleport", Name & ":" & AgentID, "Teleport")
                            Return Name
                        Else ' Its a sign!
                            'Logger("Already Booted TP Sign Teleport", Name & ":" & AgentID, "Teleport")
                            Return Name & "|0"
                        End If
                    Else  ' requires booting
                        If TeleportType.ToUpperInvariant = "UUID" Then
                            If Settings.BootOrSuspend Then
                                AddEm(RegionUUID, AgentID, True)
                                'Logger("Boot Type UUID Teleport", Name & ":" & AgentID, "Teleport")
                                RPC_admin_dialog(AgentID, $"Booting your region {Region_Name(RegionUUID)}.{vbCrLf}Region will be ready in {CStr(BootTime(RegionUUID) + Settings.TeleportSleepTime)} seconds. Please wait in this region.")
                                Dim uuid = FindRegionByName(Settings.ParkingLot)
                                Return uuid
                            Else
                                AddEm(RegionUUID, AgentID, True)
                                'Logger("Suspend type UUID Teleport", Name & ":" & AgentID, "Teleport")
                                Return RegionUUID
                            End If

                        ElseIf TeleportType.ToUpperInvariant = "REGIONNAME" Then
                            If Settings.BootOrSuspend Then
                                AddEm(RegionUUID, AgentID, True)
                                'Logger("Boot Type Named Teleport", Name & ":" & AgentID, "Teleport")
                                RPC_admin_dialog(AgentID, $"Booting your region { Region_Name(RegionUUID)}.{vbCrLf}Region will be ready in {CStr(BootTime(RegionUUID) + Settings.TeleportSleepTime)} seconds. Please wait in this region.")
                                Return Settings.ParkingLot
                            Else
                                'Logger("Suspend Type Named Teleport", Name & ":" & AgentID, "Teleport")
                                AddEm(RegionUUID, AgentID, True)
                                Return Name
                            End If
                        Else ' Its a v4 sign
                            Dim time As String
                            If Settings.MapType = "None" AndAlso MapType(RegionUUID).Length = 0 Then
                                time = "|" & CStr(BootTime(RegionUUID) + Settings.TeleportSleepTime)
                            Else
                                time = "|" & CStr(MapTime(RegionUUID) + Settings.TeleportSleepTime)
                            End If
                            If Settings.BootOrSuspend Then
                                RPC_admin_dialog(AgentID, $"Booting your region { Region_Name(RegionUUID)}.{vbCrLf}Region will be ready in {CStr(time)} seconds.")
                                'Logger("Sign Boot, Agent ", Name & ":" & AgentID, "Teleport")
                                Return Settings.ParkingLot
                            Else
                                'Logger("Sign Suspend,Agent ", Name & ":" & AgentID, "Teleport")
                                Return RegionUUID
                            End If
                        End If
                    End If
                Else
                    ' not enabled
                    RPC_admin_dialog(AgentID, $"Destination {Region_Name(RegionUUID)} is disabled.")
                    Debug.Print($"Destination {Region_Name(RegionUUID)} is disabled.")
                    Return Settings.ParkingLot
                End If
            Else ' Non Smart Start
                If TeleportType.ToUpperInvariant = "UUID" Then
                    Logger("Teleport Non Smart", Name & ":" & AgentID, "Teleport")
                    Return RegionUUID
                ElseIf TeleportType.ToUpperInvariant = "REGIONNAME" Then
                    Logger("Teleport Non Smart", Name & ":" & AgentID, "Teleport")
                    Return Name
                Else     ' Its a sign!
                    'Logger("Teleport Sign ", Name & ":" & AgentID, "Teleport")
                    AddEm(RegionUUID, AgentID, False)
                    Return Name
                End If
            End If
        End If

        Return FindRegionByName(Settings.WelcomeRegion)

    End Function

#End Region

#Region "BootUp"

    ReadOnly BootupLock As New Object

    ''' <summary>Starts Opensim for a given name</summary>
    ''' <param name="BootName">Name of region to start</param>
    ''' <returns>success = true</returns>
    '''
    Public Function Boot(BootName As String) As Boolean

        Application.DoEvents()

        SyncLock BootupLock

            PropOpensimIsRunning() = True
            If PropAborting Then Return True

            ' stop if disabled
            Dim RegionUUID As String = FindRegionByName(BootName)

            ' must be real
            If String.IsNullOrEmpty(RegionUUID) Then
                ErrorLog("Cannot find " & BootName & " to boot!")
                Return False
            End If

            ' must be enabled
            If Not RegionEnabled(RegionUUID) Then
                ShutDown(RegionUUID, SIMSTATUSENUM.ShuttingDownForGood)
                Return True
            End If

            Dim GroupName = Group_Name(RegionUUID)
            Dim PID As Integer = GetPIDofWindow(GroupName)

            ' Detect if a region Window is already running
            ' needs to be captured into the event handler
            If CBool(GetHwnd(Group_Name(RegionUUID))) Then

                Try
                    Dim P = Process.GetProcessById(PID)
                    P.EnableRaisingEvents = True
                    AddHandler P.Exited, AddressOf OpensimExited ' Registering event handler
                Catch ex As Exception
                    RegionStatus(RegionUUID) = SIMSTATUSENUM.Error
                    Return False
                End Try

                ' scan over all regions in the DOS box
                For Each UUID As String In RegionUuidListByName(GroupName)
                    ProcessID(UUID) = PID
                    Thaw(UUID)
                    RegionStatus(UUID) = SIMSTATUSENUM.Booted
                    SendToOpensimWorld(UUID, 0)
                    TextPrint($"{BootName} {My.Resources.Ready}")
                Next

                ShowDOSWindow(RegionUUID, MaybeHideWindow())
                PropUpdateView = True ' make form refresh
                Return True

            End If

            TextPrint(BootName & " " & Global.Outworldz.My.Resources.Starting_word)

            DoCurrency()
            SetCores(RegionUUID)

            If CopyOpensimProto(RegionUUID) Then Return False

#Disable Warning CA2000 ' Dispose objects before losing scope
            Dim BootProcess = New Process With {
                .EnableRaisingEvents = True
            }
#Enable Warning CA2000 ' Dispose objects before losing scope
            AddHandler BootProcess.Exited, AddressOf OpensimExited ' Registering event handler

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
            If Not LogResults.ContainsKey(RegionUUID) Then LogResults.Add(RegionUUID, New LogReader(RegionUUID))
            Dim ok As Boolean = False
            Try
                ok = BootProcess.Start
            Catch ex As Exception
                PropUpdateView = True ' make form refresh
                Logger("Failed to boot ", BootName, "Outworldz")
                TextPrint("Failed to boot region " & BootName)
                Return False
            End Try

            If ok Then

                PID = WaitForPID(BootProcess)      ' check if it gave us a PID, if not, it failed.

                If PID > 0 Then
                    If ProcessIdDict.ContainsKey(PID) Then
                        ProcessIdDict.Item(PID) = CachedProcess(PID)
                    Else
                        ProcessIdDict.Add(PID, BootProcess)
                    End If
                    ' 0 is all cores
                    Try
                        If Cores(RegionUUID) > 0 Then
                            BootProcess.ProcessorAffinity = CType(Cores(RegionUUID), IntPtr)
                        End If
                    Catch ex As Exception
                        PropUpdateView = True ' make form refresh
                        Logger("Failed to boot ", BootName, "Outworldz")
                        TextPrint("Failed to boot region " & BootName)
                        Return False
                    End Try

                    Try
                        Dim Pri = Priority(RegionUUID)

                        Dim E = New PRIEnumClass
                        Dim P As ProcessPriorityClass
                        If Pri = "RealTime" Then
                            P = E.RealTime
                        ElseIf Pri = "High" Then
                            P = E.High
                        ElseIf Pri = "AboveNormal" Then
                            P = E.AboveNormal
                        ElseIf Pri = "Normal" Then
                            P = E.Normal
                        ElseIf Pri = "BelowNormal" Then
                            P = E.BelowNormal
                        Else
                            P = E.Normal
                        End If

                        BootProcess.PriorityClass = P
                    Catch ex As Exception
                        PropUpdateView = True ' make form refresh
                        Logger("Failed to boot ", BootName, "Outworldz")
                        TextPrint("Failed to boot region " & BootName)
                        Return False
                    End Try

                    SetWindowTextCall(BootProcess, GroupName)

                    If Not PropInstanceHandles.ContainsKey(PID) Then
                        PropInstanceHandles.TryAdd(PID, GroupName)
                    End If

                    ' Mark them before we boot as a crash will immediately trigger the event that it exited
                    For Each UUID As String In RegionUuidListByName(GroupName)
                        RegionStatus(UUID) = SIMSTATUSENUM.Booting
                        PokeRegionTimer(RegionUUID)
                    Next

                    AddCPU(PID, GroupName) ' get a list of running opensim processes
                    For Each UUID As String In RegionUuidListByName(GroupName)
                        ProcessID(UUID) = PID
                    Next
                Else
                    PropUpdateView = True ' make form refresh
                    Logger("Failed to boot ", BootName, "Outworldz")
                    TextPrint("Failed to boot region " & BootName)
                    Return False
                End If

                PropUpdateView = True ' make form refresh
                FormSetup.Buttons(FormSetup.StopButton)
                SequentialPause()   ' wait for previous region to give us some CPU
                Return True
            End If
            PropUpdateView = True ' make form refresh
            Logger("Failed to boot ", BootName, "Teleport")
            TextPrint("Failed to boot region " & BootName)
            Return False
        End SyncLock

    End Function

    ''' <summary>
    ''' Resumes Region from stopped, frozen or error  state
    ''' </summary>
    ''' <param name="RegionUUID">RegionUUID</param>
    ''' <returns>0 if success</returns>
    Public Sub ResumeRegion(RegionUUID As String)

        ' smart boot Freeze/Thaw type
        For Each RegionUUID In RegionUuidListByName(Group_Name(RegionUUID))
            Thaw(RegionUUID)
            PokeRegionTimer(RegionUUID)
        Next

        If IsRegionReady(GroupPort(RegionUUID)) Then
            RunTaskList(RegionUUID)
            For Each RegionUUID In RegionUuidListByName(Group_Name(RegionUUID))
                RegionStatus(RegionUUID) = SIMSTATUSENUM.Booted
            Next
        End If
        TeleportAgents()

        ' not smart boot Freeze/Thaw
        If RegionStatus(RegionUUID) = SIMSTATUSENUM.Stopped Or
                 RegionStatus(RegionUUID) = SIMSTATUSENUM.Error Or
                 RegionStatus(RegionUUID) = SIMSTATUSENUM.ShuttingDownForGood Then

            PokeRegionTimer(RegionUUID)
            For Each RegionUUID In RegionUuidListByName(Group_Name(RegionUUID))
                RegionStatus(RegionUUID) = SIMSTATUSENUM.Resume
            Next
            PropUpdateView = True ' make form refresh
        End If

        RunTaskList(RegionUUID)

    End Sub

    ''' <summary>
    ''' Sets status to Resume if stopped or paused
    ''' </summary>

#End Region

#Region "Pass2"

    Public Sub Apply_Plant(RegionUUID As String)

        RPC_Region_Command(RegionUUID, $"change region ""{Region_Name(RegionUUID)}""")

        If RegionUUID.Length > 0 Then
            Dim R As New RegionEssentials With {
             .RegionUUID = RegionUUID,
             .RegionName = Region_Name(RegionUUID)
             }

            GenTrees(R)
        End If

    End Sub

    Public Sub ApplyTerrainEffect(RegionUUID As String)

        If Not RPC_Region_Command(RegionUUID, $"change region ""{Region_Name(RegionUUID)}""") Then Return

        Dim backupname = IO.Path.Combine(Settings.OpensimBinPath, "Terrains")
        If Not RPC_Region_Command(RegionUUID, $"terrain save ""{backupname}\{Region_Name(RegionUUID)}-Backup.r32""") Then Return
        If Not RPC_Region_Command(RegionUUID, $"terrain save ""{backupname}\{Region_Name(RegionUUID)}-Backup.raw""") Then Return
        If Not RPC_Region_Command(RegionUUID, $"terrain save ""{backupname}\{Region_Name(RegionUUID)}-Backup.jpg""") Then Return
        If Not RPC_Region_Command(RegionUUID, $"terrain save ""{backupname}\{Region_Name(RegionUUID)}-Backup.png""") Then Return
        If Not RPC_Region_Command(RegionUUID, $"terrain save ""{backupname}\{Region_Name(RegionUUID)}-Backup.ter""") Then Return

        Dim R As New RegionEssentials With {
             .RegionUUID = RegionUUID,
             .RegionName = Region_Name(RegionUUID)
             }

        GenLand(R)

    End Sub

    Public Sub Bake_Terrain(RegionUUID As String)

        RPC_Region_Command(RegionUUID, $"change region ""{Region_Name(RegionUUID)}""")
        RPC_Region_Command(RegionUUID, "terrain bake")

    End Sub

    Public Sub Delete_Tree(RegionUUID As String)

        For Each TT As String In TreeList
            If Not RPC_Region_Command(RegionUUID, $"tree remove {TT}") Then Return
        Next

    End Sub

    Public Sub Load_AllFreeOARs(RegionUUID As String, obj As TaskObject)

        Dim File = obj.Command
        PokeRegionTimer(RegionUUID)
        TextPrint($"{Region_Name(RegionUUID)}: load oar {File}")

        Dim Result = New WaitForFile(RegionUUID, "Start scripts done", "Load OAR")
        ConsoleCommand(RegionUUID, $"change region ""{Region_Name(RegionUUID)}""{vbCrLf}load oar --force-terrain --force-parcels ""{File}""{vbCrLf}backup{vbCrLf}")
        Result.Scan()

        If Not AvatarsIsInGroup(Group_Name(RegionUUID)) Then
            PokeRegionTimer(RegionUUID)
            RegionStatus(RegionUUID) = SIMSTATUSENUM.ShuttingDownForGood
            ShutDown(RegionUUID, SIMSTATUSENUM.ShuttingDownForGood)
        End If

    End Sub

    Public Sub Load_Save(RegionUUID As String)

        RPC_Region_Command(RegionUUID, $"change region ""{Region_Name(RegionUUID)}""")

        Dim Terrainfolder = IO.Path.Combine(Settings.OpensimBinPath, "Terrains")
        ' Create an instance of the open file dialog box. Set filter options and filter index.
        Using openFileDialog1 = New OpenFileDialog With {
            .InitialDirectory = Terrainfolder,
            .Filter = Global.Outworldz.My.Resources.OAR_Load_and_Save & "(*.png,*.r32,*.raw, *.ter)|*.png;*.r32;*.raw;*.ter|All Files (*.*)|*.*",
            .FilterIndex = 1,
            .Multiselect = False
            }

            ' Call the ShowDialog method to show the dialog box.
            Dim UserClickedOK As DialogResult = openFileDialog1.ShowDialog

            ' Process input if the user clicked OK.
            If UserClickedOK = DialogResult.OK Then

                Dim thing = openFileDialog1.FileName
                If thing.Length > 0 Then
                    RPC_Region_Command(RegionUUID, $"terrain save ""{Terrainfolder}\{Region_Name(RegionUUID)}-Backup.r32""")
                    RPC_Region_Command(RegionUUID, $"terrain save ""{Terrainfolder}\{Region_Name(RegionUUID)}-Backup.raw""")
                    RPC_Region_Command(RegionUUID, $"terrain save ""{Terrainfolder}\{Region_Name(RegionUUID)}-Backup.jpg""")
                    RPC_Region_Command(RegionUUID, $"terrain save ""{Terrainfolder}\{Region_Name(RegionUUID)}-Backup.png""")
                    RPC_Region_Command(RegionUUID, $"terrain save ""{Terrainfolder}\{Region_Name(RegionUUID)}-Backup.ter""")
                    RPC_Region_Command(RegionUUID, $"terrain load ""{thing}""")
                End If
            End If

        End Using

    End Sub

    Public Sub RebuildTerrain(RegionUUId As String)

        Dim Terrainfolder = IO.Path.Combine(Settings.OpensimBinPath, "Terrains")
        Dim exts As New List(Of String) From {
                "*.r32",
                "*.raw",
                "*.ter",
                "*.png"
            }

        For Each extension In exts
            Dim Files = System.IO.Directory.EnumerateFiles(Terrainfolder, extension, SearchOption.TopDirectoryOnly)
            For Each File In Files
                Maketypes(extension, File, RegionUUId)
            Next
        Next

    End Sub

    Public Sub Revert(RegionUUID As String)

        RPC_Region_Command(RegionUUID, $"change region ""{Region_Name(RegionUUID)}""")
        RPC_Region_Command(RegionUUID, "terrain revert")

    End Sub

    Public Sub Save_Terrain(RegionUUID As String)

        Dim Terrainfolder = IO.Path.Combine(Settings.OpensimBinPath, "Terrains")
        Dim S As Double = SizeX(RegionUUID)
        S /= 256
        If S > 1 Then
            Dim path = $"{Terrainfolder}\{S}x{S}"
            ' If the destination folder don't exist then create it
            If Not System.IO.Directory.Exists(path) Then
                MakeFolder(path)
            End If
            Terrainfolder = path
        End If

        If Not RPC_Region_Command(RegionUUID, $"change region {Region_Name(RegionUUID)}") Then Return
        RPC_Region_Command(RegionUUID, $"terrain save ""{Terrainfolder}\{Region_Name(RegionUUID)}.r32""")
        RPC_Region_Command(RegionUUID, $"terrain save ""{Terrainfolder}\{Region_Name(RegionUUID)}.raw""")
        RPC_Region_Command(RegionUUID, $"terrain save ""{Terrainfolder}\{Region_Name(RegionUUID)}.jpg""")
        RPC_Region_Command(RegionUUID, $"terrain save ""{Terrainfolder}\{Region_Name(RegionUUID)}.png""")
        RPC_Region_Command(RegionUUID, $"terrain save ""{Terrainfolder}\{Region_Name(RegionUUID)}.ter""")

    End Sub

    Private Sub Maketypes(startWith As String, Filename As String, RegionUUID As String)

        Dim Terrainfolder = IO.Path.Combine(Settings.OpensimBinPath, "Terrains")
        Dim extension = IO.Path.GetExtension(Filename)

        RPC_Region_Command(RegionUUID, $"change region ""{Region_Name(RegionUUID)}""")

        Dim RegionName = Filename
        RegionName = RegionName.Replace($"{extension}", startWith)
        RegionName = RegionName.Replace("*", "")
        RPC_Region_Command(RegionUUID, $"terrain load ""{Filename}""")

        RegionName = RegionName.Replace($"{extension}", "")

        Save(RegionUUID, $"terrain save ""{RegionName}.r32""")
        Save(RegionUUID, $"terrain save ""{RegionName}.raw""")
        Save(RegionUUID, $"terrain save ""{RegionName}.jpg""")
        Save(RegionUUID, $"terrain save ""{RegionName}.png""")
        Save(RegionUUID, $"terrain save ""{RegionName}.ter""")

    End Sub

    Private Sub Save(RegionUUID As String, S As String)

        If SavedAlready.Contains(S) Then
            Return
        End If
        RPC_Region_Command(RegionUUID, S)
        SavedAlready.Add(S)

    End Sub

#End Region

    Public Class TaskObject

        Public backMeUp As String
        Public Command As String    ' text to send in sequence to the task
        Public Str As String
        Public TaskName As TaskName
        Public Type As String

    End Class

End Module
