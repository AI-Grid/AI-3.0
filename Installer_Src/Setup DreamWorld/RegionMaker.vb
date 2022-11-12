#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC. AGPL3.0 https://opensource.org/licenses/AGPL

#End Region

Imports System.Collections.Concurrent
Imports System.IO

Imports System.Text.RegularExpressions
Imports System.Threading
Imports MySqlConnector
Imports Newtonsoft.Json

Module RegionMaker

#Region "Public Fields"

    Private Class JSONresult

        Public login As String
        Public region_name As String

    End Class

#End Region

#Region "Declarations"

    Public ReadOnly WebserverList As New ConcurrentDictionary(Of String, String)
    Private ReadOnly _Grouplist As New Dictionary(Of String, Integer)
    ReadOnly Backup As New List(Of Region_data)
    Private ReadOnly RegionList As New ConcurrentDictionary(Of String, Region_data)

    Dim json As New JSONresult
    Private WriteRegionlock As New Object

    Public Enum SIMSTATUSENUM As Integer

        [Error] = -1
        Stopped = 0
        Booting = 1
        Booted = 2
        RecyclingUp = 3
        RecyclingDown = 4
        RestartPending = 5
        RetartingNow = 6
        [Resume] = 7
        Suspended = 8
        RestartStage2 = 9
        ShuttingDownForGood = 10
        NoLogin = 11
        NoError = 12
        NoShutdown = 13

    End Enum

    Private Class Region_Mapping

        Public Name As String
        Public X As Integer
        Public Y As Integer

    End Class

#End Region

#Region "Start/Stop"

    Public Sub ClearStack()

        WebserverList.Clear()

    End Sub

    Public Function Init(Verbose As Boolean) As Boolean

        If GetAlreadyUsedPorts() = 0 Then
            CreateRegionStruct("Welcome")
            Settings.WelcomeRegion = "Welcome"
            WriteRegionObject("Welcome", "Welcome")
            Settings.SaveSettings()
        End If

        If GetAllRegions(Verbose) = 0 Then Return False
        TextPrint($"Loaded {CStr(RegionCount)} Regions")

        Return True

    End Function

#End Region

#Region "CheckPost"

    ''' <summary>
    ''' see if anything arrived in the web server on port 8001
    ''' </summary>
    Public Sub CheckPost()

        For Each TKey In WebserverList

            Dim ProcessString As String = TKey.Key ' recover the PID as string
            WebserverList.TryRemove(TKey.Key, "")

            ' This search returns the substring between two strings, so the first index Is moved to the character just after the first string.
            Dim POST As String = Uri.UnescapeDataString(ProcessString)
            Dim first As Integer = POST.IndexOf("{", StringComparison.OrdinalIgnoreCase)
            Dim last As Integer = POST.LastIndexOf("}", StringComparison.OrdinalIgnoreCase)

            Dim rawJSON As String
            If first > -1 AndAlso last > -1 Then
                rawJSON = POST.Substring(first, last - first + 1)
            Else
                Logger("RegionReady", "Malformed Web request: " & POST, "Teleport")
                Continue For
            End If

            Try
                json = JsonConvert.DeserializeObject(Of JSONresult)(rawJSON)
            Catch ex As Exception
                Debug.Print(ex.Message)
                Logger("RegionReady", "Malformed JSON: " & ProcessString, "Teleport")
                Continue For
            End Try

            ' rawJSON "{""alert"":""region_ready"",""login"":""disabled"",""region_name"":""Welcome"",""region_id"":""365d804a-0df1-46cf-8acf-4320a3df3fca""}" String rawJSON
            ' "{""alert"":""region_ready"",""login"":""enabled"",""region_name"":""Welcome"",""region_id"":""365d804a-0df1-46cf-8acf-4320a3df3fca""}" String rawJSON
            ' "{""alert"":""region_ready"",""login"":""shutdown"",""region_name"":""Welcome"",""region_id"":""365d804a-0df1-46cf-8acf-4320a3df3fca""}" String

            '2020-12-27 04:31:52:RegionReady: Enabled:Aurora
            '2020-12-27 04:31:52:RegionReady Heard: Aurora
            '2020-12-27 04:31:52:Ready : Aurora
            '2020-12-27 04:31:52:RegionReady Booted: Aurora

            If json.login = "enabled" Then

                Logger("RegionReady: Enabled", json.region_name, "Teleport")
                Dim uuid As String = FindRegionByName(json.region_name)
                Dim out As New Guid
                If Not Guid.TryParse(uuid, out) Then
                    Logger("RegionReady Error, no UUID", json.region_name, "Teleport")
                    Continue For
                End If

                BootedList.Add(uuid)
                CheckForBootedRegions()

            ElseIf json.login = "shutdown" Then
                Continue For   ' this bit below interferes with restarting multiple regions in a DOS box
            ElseIf json.login = "disabled" Then
                Continue For
            Else
                Continue For
            End If

        Next

    End Sub

#End Region

#Region "Create Region"

    Private CreateRegionLock As New Object

    Public Function CreateRegionStruct(name As String, Optional UUID As String = "") As String

        SyncLock CreateRegionLock

            If String.IsNullOrEmpty(UUID) Then UUID = Guid.NewGuid().ToString

            Debug.Print("Create Region " + name)
            Dim r As New Region_data With {
                    ._AllowGods = "",
                    ._AvatarCount = 0,
                    ._AvatarsInRegion = 0,
                    ._Birds = "",
                    ._BootTime = 0,
                    ._ClampPrimSize = False,
                    ._Concierge = "",
                    ._CoordX = LargestX() + 8,
                    ._CoordY = LargestY() + 0,
                    ._Cores = 0,
                    ._CrashCounter = 0,
                    ._DisableGloebits = "",
                    ._FrameTime = "",
                    ._GodDefault = "",
                    ._Group = name,
                    ._GroupPort = 0,
                    ._ManagerGod = "",
                    ._MapType = "",
                    ._MaxAgents = "100",
                    ._MaxPrims = "15000",
                    ._MinTimerInterval = "",
                    ._NonPhysicalPrimMax = "1024",
                    ._PhysicalPrimMax = "64",
                    ._Priority = "",
                    ._ProcessID = 0,
                    ._RegionEnabled = True,
                    ._RegionGod = "",
                    ._RegionLandingSpot = "",
                    ._RegionName = name,
                    ._RegionPort = 0,
                    ._RegionSmartStart = "",
                    ._RegionSnapShot = "",
                    ._ScriptEngine = "",
                    ._SizeX = 256,
                    ._SizeY = 256,
                    ._SkipAutobackup = "",
                    ._Status = SIMSTATUSENUM.Stopped,
                    ._Teleport = "",
                    ._Tides = "",
                    ._Timer = Date.Now,
                    ._UUID = UUID
                }

            If Not RegionList.ContainsKey(r._UUID) Then
                RegionList.TryAdd(r._UUID, r)
            End If

            Debug.Print("Region count is " & CStr(RegionList.Count))

        End SyncLock
        Return UUID

    End Function

    Public Sub DeleteRegion(RegionUUID As String)

        If RegionList.ContainsKey(RegionUUID) Then

            ReleasePort(Region_Port(RegionUUID))
            ReleasePort(GroupPort(RegionUUID))

            Dim v As Region_data = Nothing
            Dim unused = RegionList.TryRemove(RegionUUID, v)
        End If

    End Sub

    ''' <summary>
    ''' Saves Region class to disk file
    ''' </summary>
    ''' <param name="Group">Dos Box name</param>
    ''' <param name="RegionName">Region Name</param>
    ''' <param name="Verbose">Be chatty on the console</param>
    Public Sub WriteRegionObject(Group As String, RegionName As String)

        SyncLock WriteRegionlock

            Dim pathtoRegion As String = IO.Path.Combine(Settings.OpensimBinPath, $"Regions\{Group}\Region\")
            Dim RegionUUID As String = FindRegionByName(RegionName)
            ' file paths
            RegionIniFilePath(RegionUUID) = IO.Path.Combine(Settings.OpensimBinPath, $"Regions\{Group}\Region\{RegionName}.ini")
            RegionIniFolderPath(RegionUUID) = IO.Path.Combine(Settings.OpensimBinPath, $"Regions\{Group}\Region")
            OpensimIniPath(RegionUUID) = IO.Path.Combine(Settings.OpensimBinPath, $"Regions\{Group}")

            CopyFileFast(IO.Path.Combine(pathtoRegion, $"{RegionName}.ini"), IO.Path.Combine(pathtoRegion, $"{RegionName}.bak"))
            DeleteFile(IO.Path.Combine(pathtoRegion, $"{RegionName}.ini"))
            If Not Directory.Exists(pathtoRegion) Then
                MakeFolder(pathtoRegion)
            End If

            ' Change estate for Endless Land, assuming its on
            Dim out As Integer
            If Integer.TryParse(Estate(RegionUUID), out) Then
                ErrorLog("No Region UUID for Estate")
            End If

            If Settings.AutoFill AndAlso Settings.Smart_Start AndAlso Smart_Start(RegionUUID) AndAlso out = 0 Then
                Estate(RegionUUID) = "SimSurround"
                SetEstate(RegionUUID, 1999)
            End If

            Dim proto = "; * Regions configuration file; " & vbCrLf _
        & "; Automatically changed and read by Dreamworld. Edits here are allowed and take effect on restart" & vbCrLf _
        & "; Rule1: The File name must match the RegionName in brackets exactly." & vbCrLf _
        & "; Rule2: Only one region per INI file." & vbCrLf _
        & ";" & vbCrLf _
        & "[" & RegionName & "]" & vbCrLf _
        & "RegionUUID=" & RegionUUID & vbCrLf _
        & "Location=" & Coord_X(RegionUUID).ToString(Globalization.CultureInfo.InvariantCulture) & "," & Coord_Y(RegionUUID).ToString(Globalization.CultureInfo.InvariantCulture) & vbCrLf _
        & "InternalAddress = 0.0.0.0" & vbCrLf _
        & "InternalPort=" & Region_Port(RegionUUID) & vbCrLf _
        & "GroupPort=" & GroupPort(RegionUUID) & vbCrLf _
        & "AllowAlternatePorts = False" & vbCrLf _
        & "ExternalHostName=" & Settings.ExternalHostName() & vbCrLf _
        & "SizeX=" & CStr(SizeX(RegionUUID)) & vbCrLf _
        & "SizeY=" & CStr(SizeY(RegionUUID)) & vbCrLf _
        & "Enabled=" & CStr(RegionEnabled(RegionUUID)) & vbCrLf _
        & "NonPhysicalPrimMax=" & CStr(NonPhysical_PrimMax(RegionUUID)) & vbCrLf _
        & "PhysicalPrimMax=" & CStr(Physical_PrimMax(RegionUUID)) & vbCrLf _
        & "ClampPrimSize=" & CStr(Clamp_PrimSize(RegionUUID)) & vbCrLf _
        & "MaxPrims=" & Max_Prims(RegionUUID) & vbCrLf _
        & "RegionType=" & Estate(RegionUUID) & vbCrLf _
        & "MaxAgents = 100" & vbCrLf & vbCrLf _
        & ";# Dreamgrid extended properties" & vbCrLf _
        & "RegionSnapShot=" & RegionSnapShot(RegionUUID) & vbCrLf _
        & "MapType=" & MapType(RegionUUID) & vbCrLf _
        & "Physics=" & RegionPhysics(RegionUUID) & vbCrLf _
        & "GodDefault=" & GodDefault(RegionUUID) & vbCrLf _
        & "AllowGods=" & AllowGods(RegionUUID) & vbCrLf _
        & "RegionGod=" & RegionGod(RegionUUID) & vbCrLf _
        & "ManagerGod=" & ManagerGod(RegionUUID) & vbCrLf _
        & "Birds=" & Birds(RegionUUID) & vbCrLf _
        & "Tides=" & Tides(RegionUUID) & vbCrLf _
        & "Teleport=" & CStr(Teleport_Sign(RegionUUID)) & vbCrLf _
        & "DisableGloebits=" & DisableGloebits(RegionUUID) & vbCrLf _
        & "DisallowForeigners=" & Disallow_Foreigners(RegionUUID) & vbCrLf _
        & "DisallowResidents=" & Disallow_Residents(RegionUUID) & vbCrLf _
        & "MinTimerInterval=" & MinTimerInterval(RegionUUID) & vbCrLf _
        & "Frametime=" & FrameTime(RegionUUID) & vbCrLf _
        & "ScriptEngine=" & ScriptEngine(RegionUUID) & vbCrLf _
        & "Publicity=" & GDPR(RegionUUID) & vbCrLf _
        & "Concierge=" & Concierge(RegionUUID) & vbCrLf _
        & "SmartStart=" & CStr(Smart_Start(RegionUUID)) & vbCrLf _
        & "LandingSpot=" & LandingSpot(RegionUUID) & vbCrLf _
        & "Cores=" & Cores(RegionUUID) & vbCrLf _
        & "Priority=" & Priority(RegionUUID) & vbCrLf _
        & "OpenSimWorldAPIKey=" & OpensimWorldAPIKey(RegionUUID) & vbCrLf _
        & "SkipAutoBackup=" & SkipAutobackup(RegionUUID) & vbCrLf

            Try
                Using outputFile As New StreamWriter(IO.Path.Combine(pathtoRegion, $"{RegionName}.ini"), False)
                    outputFile.WriteLine(proto)
                    outputFile.Flush()
                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try

            AddToRegionMap(RegionUUID)
        End SyncLock

    End Sub

#End Region

#Region "Functions"

    Private retry As Integer

    Public Sub AddToRegionMap(RegionUUID As String)

        ' add to the global map this entire DOS box
        Dim Xloc = Coord_X(RegionUUID)
        Dim Yloc = Coord_Y(RegionUUID)
        Dim Name = Region_Name(RegionUUID)

        ' draw a box at this size to the right and up plus the pull down size.
        Dim SimSize As Integer = CInt(SizeX(RegionUUID) / 256)
        For Xstep = 0 To SimSize - 1
            For Ystep = 0 To SimSize - 1
                Dim gr As String = $"{Xloc + Xstep},{Yloc + Ystep}"
                If Not Map.ContainsKey(gr) Then
                    Map.Add(gr, RegionUUID)
                End If
            Next
        Next

    End Sub

    Public Function AvatarIsNearby(RegionUUID As String) As Boolean

        ' no point looking if this is off
        If Settings.Skirtsize = 0 Then Return False

        Dim NameRegion = Region_Name(RegionUUID)
        'Debug.Print($"Looking for nearby to {NameRegion}")
        Dim Xloc = Coord_X(RegionUUID)
        Dim Yloc = Coord_Y(RegionUUID)

        Dim CenterSize As Integer = CInt(SizeX(RegionUUID) / 256)

        ' draw a square around the new sim
        Dim X1 = Xloc - Settings.Skirtsize
        Dim X2 = Xloc + Settings.Skirtsize - 1 + CenterSize
        Dim Y1 = Yloc - Settings.Skirtsize
        Dim Y2 = Yloc + Settings.Skirtsize - 1 + CenterSize

        For XPos As Integer = X1 To X2 Step 1
            For Ypos As Integer = Y1 To Y2 Step 1
                Dim gr As String = $"{XPos},{Ypos}"

                If Map.ContainsKey(gr) Then
                    Dim DestUUID = Map(gr)
                    Dim DestName = Region_Name(DestUUID)
                    ' do not look inside myself
                    If DestName = NameRegion Then
                        Continue For
                    End If

                    ' Debug.Print($"Looking in {Region_Name(DestUUID)}")

                    ' skip any offline regions, no one is in there
                    If RegionStatus(DestUUID) = SIMSTATUSENUM.Stopped _
                       Or RegionStatus(DestUUID) = SIMSTATUSENUM.Suspended _
                        Or Not RegionEnabled(DestUUID) Then Continue For

                    ' now see if anyone is in the surrounding sim
                    If IsAgentInRegion(DestUUID) Then
                        Debug.Print($"Avatar is detected near region {NameRegion} in {DestName}")
                        Return True
                    End If
                End If
            Next
        Next

        Return False

    End Function

    ''' <summary>
    ''' Detects if an avatars is in the DOS box
    ''' </summary>
    ''' <param name="groupname">Pass it the DOS box name</param>
    ''' <returns>true if avatar is present</returns>
    Public Function AvatarsIsInGroup(groupname As String) As Boolean

        For Each RegionUUID As String In RegionUuidListByName(groupname)

            If RegionStatus(RegionUUID) <> SIMSTATUSENUM.Booted Then Return False

            If IsAgentInRegion(RegionUUID) Then
                Return True
            End If
        Next
        Return False

    End Function

    Public Function CheckOverLap() As Boolean

        Dim FailedCheck As Boolean
        Dim Regions As New List(Of Region_Mapping)

        For Each RegionUUID In RegionUuids()

            Dim Name = Region_Name(RegionUUID)
            Dim Size As Integer = CInt(SizeX(RegionUUID) / 256)

            Dim X As Integer
            Dim Y As Integer
            ' make a box
            For X = 0 To Size - 1
                For Y = 0 To Size - 1
                    Dim map = New Region_Mapping With {
                        .Name = Name,
                        .X = Coord_X(RegionUUID) + X,
                        .Y = Coord_Y(RegionUUID) + Y
                    }
                    Regions.Add(map)
                    ' If (Name.Contains("MartinBassManSlad")) Or (Name.Contains("Maya")) Then Breakpoint.Print($"{Name} {map.X} {map.Y}") End If
                Next
            Next
        Next

        TextPrint($"-> {My.Resources.checking_word} {Regions.Count} {My.Resources.potential_overlap}")

        For Each Pass1 In Regions
            For Each Pass2 In Regions
                If Pass1.Name = Pass2.Name Then Continue For ' don't check itself

                If (Pass1.X = Pass2.X) AndAlso (Pass1.Y = Pass2.Y) Then
                    TextPrint($"-> Region {Pass1.Name} overlaps Region {Pass2.Name} at location {Pass1.X}, {Pass1.Y}")
                    FailedCheck = True
                End If
            Next
        Next
        If FailedCheck Then
            TextPrint($"-> ** {My.Resources.Error_word} **")
        Else
            TextPrint($"-> " & My.Resources.No_Overlaps)
        End If

        Return FailedCheck

    End Function

    ''' <summary>
    ''' Check Overlap of any region including Var
    ''' </summary>
    ''' <returns>True is a region overlaps another</returns>
    Public Function FindBackupByName(Name As String) As Integer

        Dim i As Integer = 0
        For Each obj As Region_data In Backup
            If Name = obj._RegionName Then
                ' Debug.Print("Current Backup Is " + obj._RegionName)
                Return i
            End If
            i += 1
        Next
        Return -1

    End Function

    ''' <summary>
    ''' Fetches all regions from Disk Into RAM
    ''' </summary>
    ''' <param name="Verbose"></param>
    ''' <returns>RegionList.Count</returns>
    Public Function GetAllRegions(Verbose As Boolean) As Integer

        Try

            Backup.Clear()
            Dim pair As KeyValuePair(Of String, Region_data)

            For Each pair In RegionList
                Backup.Add(pair.Value)
            Next

            RegionList.Clear()

            Dim uuid As String = ""
            Dim folders() = Directory.GetDirectories(Settings.OpensimBinPath + "Regions")
            System.Array.Sort(folders)

            For Each FolderName As String In folders

                Dim ThisGroup As Integer = 0

                Dim regionfolders = Directory.GetDirectories(FolderName)
                Application.DoEvents()

                For Each FileName As String In regionfolders

                    If FileName.EndsWith("DataSnapshot", StringComparison.OrdinalIgnoreCase) Then Continue For

                    Dim fName = ""
                    Try
                        Dim inis() As String = Nothing
                        Try
                            inis = Directory.GetFiles(FileName, "*.ini", SearchOption.TopDirectoryOnly)
                        Catch ex As Exception
                            BreakPoint.Dump(ex)
                        End Try

                        For Each file As String In inis
                            Application.DoEvents()
                            fName = Path.GetFileNameWithoutExtension(file)

                            Dim INI = New LoadIni(file, ";", System.Text.Encoding.ASCII)
                            Dim Group As String
                            uuid = CStr(INI.GetIni(fName, "RegionUUID", "", "String"))

                            Dim SomeUUID As New Guid
                            If Not Guid.TryParse(uuid, SomeUUID) Then
                                MsgBox("Cannot read UUID In INI file For " & fName, vbCritical Or MsgBoxStyle.MsgBoxSetForeground)
                                '  TODO Auto repair this error from a backup
                                Exit For
                            Else
                                If Verbose Then TextPrint("-> " & fName)
                                CreateRegionStruct(fName, uuid)

                                RegionEnabled(uuid) = CBool(INI.GetIni(fName, "Enabled", "True", "Boolean"))

                                RegionIniFilePath(uuid) = file ' save the path
                                RegionIniFolderPath(uuid) = System.IO.Path.GetDirectoryName(file)

                                Dim theEnd As Integer = RegionIniFolderPath(uuid).LastIndexOf("\", StringComparison.OrdinalIgnoreCase)
                                OpensimIniPath(uuid) = RegionIniFolderPath(uuid).Substring(0, theEnd + 1)

                                Dim DirName = Path.GetFileName(FolderName)
                                If DirName.Length > 0 Then
                                    Group_Name(uuid) = DirName
                                    Group = DirName
                                Else
                                    MsgBox("Cannot locate Dos Box name for " & fName, vbInformation Or MsgBoxStyle.MsgBoxSetForeground)
                                    Return 0
                                End If

                                SizeX(uuid) = CInt(INI.GetIni(fName, "SizeX", "256", "Integer"))
                                SizeY(uuid) = CInt(INI.GetIni(fName, "SizeY", "256", "Integer"))

                                ' extended props V2.1
                                NonPhysical_PrimMax(uuid) = CStr(INI.GetIni(fName, "NonPhysicalPrimMax", "1024", "Integer"))
                                Physical_PrimMax(uuid) = CStr(INI.GetIni(fName, "PhysicalPrimMax", "64", "Integer"))
                                Clamp_PrimSize(uuid) = CBool(INI.GetIni(fName, "ClampPrimSize", "False", "Boolean"))
                                Max_Agents(uuid) = CStr(INI.GetIni(fName, "MaxAgents", "100", "Integer"))

                                ' Location is int,int format.
                                Dim C As String = CStr(INI.GetIni(fName, "Location", RandomNumber.Between(980, 1020) & "," & RandomNumber.Between(980, 1020)))
                                Dim parts As String() = C.Split(New Char() {","c}) ' split at the comma
                                Coord_X(uuid) = CInt("0" & CStr(parts(0).Trim))
                                Coord_Y(uuid) = CInt("0" & CStr(parts(1).Trim))

                                ' options parameters coming from INI file can be blank!
                                MinTimerInterval(uuid) = CStr(INI.GetIni(fName, "MinTimerInterval", "", "String"))
                                FrameTime(uuid) = CStr(INI.GetIni(fName, "FrameTime", "", "String"))
                                RegionSnapShot(uuid) = CStr(INI.GetIni(fName, "RegionSnapShot", "", "String"))
                                MapType(uuid) = CStr(INI.GetIni(fName, "MapType", "", "String"))
                                RegionPhysics(uuid) = CStr(INI.GetIni(fName, "Physics", "", "String"))
                                Max_Prims(uuid) = CStr(INI.GetIni(fName, "MaxPrims", "", "String"))
                                GodDefault(uuid) = CStr(INI.GetIni(fName, "GodDefault", "True", "String"))
                                AllowGods(uuid) = CStr(INI.GetIni(fName, "AllowGods", "", "String"))
                                RegionGod(uuid) = CStr(INI.GetIni(fName, "RegionGod", "", "String"))
                                ManagerGod(uuid) = CStr(INI.GetIni(fName, "ManagerGod", "", "String"))
                                Birds(uuid) = CStr(INI.GetIni(fName, "Birds", "", "String"))
                                Tides(uuid) = CStr(INI.GetIni(fName, "Tides", "", "String"))
                                Teleport_Sign(uuid) = CBool(INI.GetIni(fName, "Teleport", "", "Boolean"))
                                DisableGloebits(uuid) = CStr(INI.GetIni(fName, "DisableGloebits", "", "String"))
                                Disallow_Foreigners(uuid) = CStr(INI.GetIni(fName, "DisallowForeigners", "", "String"))
                                Disallow_Residents(uuid) = CStr(INI.GetIni(fName, "DisallowResidents", "", "String"))
                                SkipAutobackup(uuid) = CStr(INI.GetIni(fName, "SkipAutoBackup", "", "String"))
                                Snapshot(uuid) = CStr(INI.GetIni(fName, "RegionSnapShot", "", "String"))
                                ScriptEngine(uuid) = CStr(INI.GetIni(fName, "ScriptEngine", "", "String"))
                                GDPR(uuid) = CStr(INI.GetIni(fName, "Publicity", "", "String"))
                                Concierge(uuid) = CStr(INI.GetIni(fName, "Concierge", "", "String"))
                                Smart_Start(uuid) = CBool(INI.GetIni(fName, "SmartStart", "False", "Boolean"))
                                LandingSpot(uuid) = CStr(INI.GetIni(fName, "DefaultLanding", "", "String"))
                                OpensimWorldAPIKey(uuid) = CStr(INI.GetIni(fName, "OpensimWorldAPIKey", "", "String"))
                                Cores(uuid) = CInt(0 & INI.GetIni(fName, "Cores", "", "String"))
                                Priority(uuid) = CStr(INI.GetIni(fName, "Priority", "", "String"))

                                ' Four  scenarios for ports
                                ' if the system was shut down safely ( default = true after an update), then
                                ' sequence them.
                                ' if not, read them from the INI files.
                                ' If the iNI files have Nothing, Then  go Max
                                ' if this is after boot up, use the backed up settings.
                                ' Adding a new region always uses

                                ' Get Next Port
                                If ThisGroup = 0 Then
                                    ThisGroup = GetPort(uuid)
                                    GroupPort(uuid) = ThisGroup
                                End If

                                Region_Port(uuid) = ThisGroup

                                Dim G = Group_Name(uuid)
                                If GetHwnd(G) = IntPtr.Zero Then
                                    Region_Port(uuid) = GetPort(uuid)
                                    Logger("Port", $"Assign Region Port {CStr(Region_Port(uuid))}  To {fName}", "Port")
                                    Logger("Port", $"Assign Group Port {CStr(GroupPort(uuid))} To {fName}", "Port")
                                Else
                                    Region_Port(uuid) = CInt("0" + INI.GetIni(fName, "InternalPort", "", "Integer"))
                                    If Region_Port(uuid) = 0 Then Region_Port(uuid) = LargestPort() + 1
                                    Logger("Port", $"Assign Region Port {CStr(Region_Port(uuid))} To {fName}", "Port")
                                    '
                                    GroupPort(uuid) = CInt("0" + INI.GetIni(fName, "GroupPort", "", "Integer"))
                                    BreakPoint.Print($"Assign Group Port {CStr(GroupPort(uuid))} To {fName}")
                                    '
                                    If GroupPort(uuid) = 0 Then
                                        GroupPort(uuid) = ThisGroup
                                        Logger("Port", $"Re-Assign Group Port {CStr(GroupPort(uuid))} To {fName}", "Port")
                                    End If

                                End If

                                ' If region Is already set, use its port as they cannot change while up.
                                ' restore backups of transient data
                                Dim o = FindBackupByName(fName)
                                If o >= 0 Then
                                    AvatarCount(uuid) = Backup(o)._AvatarCount
                                    ProcessID(uuid) = Backup(o)._ProcessID
                                    RegionStatus(uuid) = Backup(o)._Status
                                    Timer(uuid) = Backup(o)._Timer
                                    CrashCounter(uuid) = Backup(o)._CrashCounter
                                End If
                            End If

                            INI.SaveIni()
                            Debug.Print($"Adding {Region_Name(uuid)} To map")
                            AddToRegionMap(uuid)

                        Next
                    Catch ex As Exception
                        BreakPoint.Dump(ex)
                        MsgBox(My.Resources.Error_Region + fName + "  " + ex.Message, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Error_word)
                        ErrorLog("ErrParse file " + fName + ": " + ex.Message)
                        PropUpdateView = True ' make form refresh
                        Return 0
                    End Try
                Next
            Next
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

        PropChangedRegionSettings = False
        PropUpdateView = True ' make form refresh

        Return RegionList.Count

    End Function

    Public Function GetStateString(state As Integer) As String

        Dim statestring As String
        Select Case state
            Case -1
                statestring = "ERROR"
            Case 0
                statestring = "Stopped"
            Case 1
                statestring = "Booting"
            Case 2
                statestring = "Booted"
            Case 3
                statestring = "RecyclingUp"
            Case 4
                statestring = "RecyclingDown"
            Case 5
                statestring = "RestartPending"
            Case 6
                statestring = "RetartingNow"
            Case 7
                statestring = "Resume"
            Case 8
                statestring = "Suspended"
            Case 9
                statestring = "RestartStage2"
            Case 10
                statestring = "ShuttingDownForGood"
            Case 11
                statestring = "NoLogin"
            Case 12
                statestring = "No Error"
            Case Else
                statestring = "**** Unknown state ****"
                BreakPoint.Print($"**** Unknown state **** {CStr(state)}")
        End Select

        Return statestring

    End Function

    Public Function LargestX() As Integer

        ' locate largest global coordinates
        Dim Max As Integer
        Dim pair As KeyValuePair(Of String, Region_data)

        For Each pair In RegionList
            Dim val = pair.Value._CoordX
            If val > Max Then Max = val
        Next
        If Max = 0 Then
            Max = Between(990, 1010)
        End If
        Return Max

    End Function

    Public Function LargestY() As Integer

        ' locate largest global coordinates
        Dim Max As Integer
        Dim pair As KeyValuePair(Of String, Region_data)

        For Each pair In RegionList
            Dim val = pair.Value._CoordY
            If val > Max Then Max = val
        Next
        If Max = 0 Then
            Max = RandomNumber.Between(990, 1010)
        End If
        Return Max

    End Function

    Public Sub PokeGroupTimer(GroupName As String)

        For Each RegionUUID In RegionUuidListByName(GroupName)
            If Timer(RegionUUID) < Date.Now() Then
                Timer(RegionUUID) = Date.Now()
            End If
        Next

    End Sub

    Public Sub PokeRegionTimer(RegionUUID As String)

        PokeGroupTimer(Group_Name(RegionUUID))

    End Sub

    Public Function RobustName() As String

        Return "Robust " & Settings.PublicIP

    End Function

    Public Sub StopRegion(RegionUUID As String)

        Thaw(RegionUUID)

        If ShowDOSWindow(RegionUUID, SHOWWINDOWENUM.SWRESTORE) Then
            SequentialPause()
            ShutDown(RegionUUID, SIMSTATUSENUM.ShuttingDownForGood)
        Else
            ' shut down all regions in the DOS box
            For Each UUID As String In RegionUuidListByName(Group_Name(RegionUUID))
                RegionStatus(UUID) = SIMSTATUSENUM.Stopped ' already shutting down
            Next
        End If

        PropUpdateView = True ' make form refresh

    End Sub

    Public Function VarChooser(RegionName As String, Optional modal As Boolean = True, Optional Map As Boolean = True) As String

        Dim RegionUUID As String = FindRegionByName(RegionName)
        Dim size = SizeX(RegionUUID)

#Disable Warning CA2000 ' Dispose objects before losing scope
        Dim VarForm As New FormDisplacement ' form for choosing a region in  a var
#Enable Warning CA2000 ' Dispose objects before losing scope
        Dim span As Integer = CInt(Math.Ceiling(size / 256))
        ' Show Dialog as a modal dialog
        VarForm.Init(span, RegionUUID, Map)

        If modal Then
            VarForm.ShowDialog()
            VarForm.Dispose()
        Else
            VarForm.Show()
        End If

        Return PropSelectedBox

    End Function

#Region "Region_data"

    ' hold a copy of the Main region data on a per-form basis
    Private Class Region_data

        Public _AvatarCount As Integer
        Public _BootTime As Integer
        Public _ClampPrimSize As Boolean
        Public _CoordX As Integer = 1000
        Public _CoordY As Integer = 1000
        Public _Cores As Integer = 0
        Public _DisallowForeigners As String = ""
        Public _DisallowResidents As String = ""
        Public _Estate As String = ""
        Public _FolderPath As String = ""
        Public _Group As String = ""
        Public _IniPath As String = "" ' the path to the folder that holds the region ini
        Public _MapTime As Integer
        Public _Priority As String = "Normal"
        Public _ProcessID As Integer ' the folder name that holds the region(s), can be different named
        Public _RegionEnabled As Boolean = True
        Public _RegionName As String = ""
        Public _RegionPath As String = ""  ' The full path to the region ini file
        Public _RegionPort As Integer

        Public _SizeX As Integer = 256
        Public _SizeY As Integer = 256
        Public _Status As Integer
        Public _Timer As Date
        Public _UUID As String = ""

#End Region

#Region "OptionalStorage"

        Public _AllowGods As String = ""
        Public _AvatarsInRegion As Integer
        Public _Birds As String = ""
        Public _Concierge As String = ""
        Public _CrashCounter As Integer
        Public _DisableGloebits As String = ""
        Public _FrameTime As String = ""
        Public _GDPR As String = ""
        Public _GodDefault As String = ""
        Public _GroupPort As Integer
        Public _ManagerGod As String = ""
        Public _MapType As String = ""
        Public _MaxAgents As String = ""
        Public _MaxPrims As String = ""
        Public _MinTimerInterval As String = ""
        Public _NonPhysicalPrimMax As String = ""
        Public _OSWAPIKey As String = ""
        Public _PhysicalPrimMax As String = ""
        Public _Physics As String = "  "
        Public _RegionGod As String = ""
        Public _RegionLandingSpot As String = ""
        Public _RegionSmartStart As String = ""
        Public _RegionSnapShot As String = ""
        Public _ScriptEngine As String = ""
        Public _SkipAutobackup As String = ""
        Public _Snapshot As String = ""
        Public _Teleport As String = ""
        Public _Tides As String = ""

#End Region

    End Class

#End Region

#Region "Standard INI"

    Public ReadOnly Property RegionCount() As Integer
        Get
            Return RegionList.Count
        End Get
    End Property

    Public Property AvatarCount(uuid As String) As Integer
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._AvatarCount
            BadUUID(uuid)
            Return 0
        End Get
        Set(ByVal Value As Integer)
            RegionList(uuid)._AvatarCount = Value
        End Set
    End Property

    Public Property BootTime(uuid As String) As Integer
        Get
            Dim t As Integer = Settings.GetBootTime(uuid)
            If t > 0 Then Return t
            Return 0
        End Get
        Set(ByVal Value As Integer)
            If Value < 0 Then Value = 0
            RegionList(uuid)._BootTime = Value
            Settings.SaveBootTime(Value, uuid)
        End Set
    End Property

    Public Property Clamp_PrimSize(uuid As String) As Boolean
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._ClampPrimSize
            BadUUID(uuid)
            Return False
        End Get
        Set(ByVal Value As Boolean)
            RegionList(uuid)._ClampPrimSize = Value
        End Set
    End Property

    Public Property Coord_X(uuid As String) As Integer
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._CoordX
            BadUUID(uuid)
            Return 0
        End Get
        Set(ByVal Value As Integer)
            RegionList(uuid)._CoordX = Value
        End Set
    End Property

    Public Property Coord_Y(uuid As String) As Integer
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._CoordY
            BadUUID(uuid)
            Return 100
        End Get
        Set(ByVal Value As Integer)
            RegionList(uuid)._CoordY = Value
        End Set
    End Property

    Public Property Cores(uuid As String) As Integer
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._Cores
            BadUUID(uuid)
            Return 0
        End Get
        Set(ByVal Value As Integer)
            Try
                RegionList(uuid)._Cores = Value
            Catch
            End Try
        End Set
    End Property

    Public Property CrashCounter(uuid As String) As Integer
        Get
            If RegionList.ContainsKey(uuid) Then
                ErrorLog($"{Region_Name(uuid)} crashed")
                Return RegionList(uuid)._CrashCounter
            End If
            BadUUID(uuid)
            Return 0
        End Get
        Set(ByVal Value As Integer)
            RegionList(uuid)._CrashCounter = Value
        End Set
    End Property

    Public Property Estate(uuid As String) As String
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._Estate
            BadUUID(uuid)
            Return ""
        End Get
        Set(ByVal Value As String)
            RegionList(uuid)._Estate = Value
        End Set
    End Property

    Public Property Group_Name(uuid As String) As String
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._Group
            BadUUID(uuid)
            Return ""
        End Get
        Set(ByVal Value As String)
            RegionList(uuid)._Group = Value
        End Set
    End Property

    Public Property GroupPort(uuid As String) As Integer
        Get
            If RegionList.ContainsKey(uuid) Then
                Dim GN As String = Group_Name(uuid)
                If _Grouplist.ContainsKey(GN) Then Return _Grouplist.Item(GN)
                Return LargestPort()
            End If
            BadUUID(uuid)
            Return LargestPort()
        End Get
        Set(ByVal Value As Integer)
            Dim GN As String = Group_Name(uuid)
            If _Grouplist.ContainsKey(GN) Then
                _Grouplist.Item(GN) = Value
            Else
                _Grouplist.Add(GN, Value)
            End If
        End Set
    End Property

    Public Property MapTime(uuid As String) As Integer
        Get
            If RegionList.ContainsKey(uuid) Then
                Dim t As Integer = Settings.GetMapTime(uuid)
                If t > 0 Then Return t
                Return 0
            End If
            BadUUID(uuid)
            Return 0
        End Get
        Set(ByVal Value As Integer)
            If Value < 0 Then Value = 0
            RegionList(uuid)._MapTime = Value
            Settings.SaveMapTime(Value, uuid)
        End Set
    End Property

    Public Property Max_Agents(uuid As String) As String
        Get
            If RegionList.ContainsKey(uuid) Then
                If String.IsNullOrEmpty(RegionList(uuid)._MaxAgents) Then RegionList(uuid)._MaxAgents = "100"
                Return RegionList(uuid)._MaxAgents
            End If
            BadUUID(uuid)
            Return "100"
        End Get
        Set(ByVal Value As String)
            RegionList(uuid)._MaxAgents = Value
        End Set
    End Property

    Public Property Max_Prims(uuid As String) As String
        Get
            If RegionList.ContainsKey(uuid) Then
                If String.IsNullOrEmpty(RegionList(uuid)._MaxPrims) Then
                    RegionList(uuid)._MaxPrims = "45000"
                End If
                Return RegionList(uuid)._MaxPrims
            End If
            BadUUID(uuid)
            Return "45000"
        End Get
        Set(ByVal Value As String)
            RegionList(uuid)._MaxPrims = Value
        End Set
    End Property

    Public Property NonPhysical_PrimMax(uuid As String) As String
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._NonPhysicalPrimMax
            BadUUID(uuid)
            Return "1024"
        End Get
        Set(ByVal Value As String)
            RegionList(uuid)._NonPhysicalPrimMax = Value
        End Set
    End Property

    Public Property OpensimIniPath(uuid As String) As String
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._IniPath
            BadUUID(uuid)
            Return ""
        End Get
        Set(ByVal Value As String)
            RegionList(uuid)._IniPath = Value
        End Set
    End Property

    Public Property Physical_PrimMax(uuid As String) As String
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._PhysicalPrimMax
            BadUUID(uuid)
            Return "1024"
        End Get
        Set(ByVal Value As String)
            RegionList(uuid)._PhysicalPrimMax = Value
        End Set
    End Property

    Public Property Priority(uuid As String) As String
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._Priority
            BadUUID(uuid)
            Return "Normal"
        End Get
        Set(ByVal Value As String)
            RegionList(uuid)._Priority = Value
        End Set
    End Property

    ''' <summary>
    ''' Returns PID of any booted region
    ''' </summary>
    ''' <param name="uuid">UUID</param>
    ''' <returns>PID</returns>
    Public Property ProcessID(uuid As String) As Integer
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._ProcessID
            BadUUID(uuid)
            Return 0
        End Get
        Set(ByVal Value As Integer)
            RegionList(uuid)._ProcessID = Value
        End Set
    End Property

    Public Property Region_Name(uuid As String) As String
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._RegionName
            Return ""
        End Get
        Set(ByVal Value As String)
            RegionList(uuid)._RegionName = Value
        End Set
    End Property

    Public Property Region_Port(uuid As String) As Integer
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._RegionPort
            BadUUID(uuid)
            Return 0
        End Get
        Set(ByVal Value As Integer)
            RegionList(uuid)._RegionPort = Value

        End Set
    End Property

    Public Property RegionIniFilePath(uuid As String) As String
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._RegionPath
            BadUUID(uuid)
            Return ""
        End Get
        Set(ByVal Value As String)
            RegionList(uuid)._RegionPath = Value
        End Set
    End Property

    Public Property RegionIniFolderPath(uuid As String) As String
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._FolderPath
            BadUUID(uuid)
            Return ""
        End Get
        Set(ByVal Value As String)
            RegionList(uuid)._FolderPath = Value
        End Set
    End Property

    Public Property RegionStatus(RegionUUID As String) As Integer
        Get
            If RegionList.ContainsKey(RegionUUID) Then
                Return RegionList(RegionUUID)._Status
            End If

            BadUUID(RegionUUID)
            Return -1
        End Get
        Set(ByVal Value As Integer)
            If Debugger.IsAttached Then
                'Logger(Region_Name(RegionUUID), $"Status => {GetStateString(Value)}", "Status")
            End If
            RegionList(RegionUUID)._Status = Value
        End Set
    End Property

    Public Property SizeX(uuid As String) As Integer
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._SizeX
            BadUUID(uuid)
            Return 256
        End Get
        Set(ByVal Value As Integer)
            RegionList(uuid)._SizeX = Value
        End Set
    End Property

    Public Property SizeY(uuid As String) As Integer
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._SizeY
            BadUUID(uuid)
            Return 256
        End Get
        Set(ByVal Value As Integer)

            RegionList(uuid)._SizeY = Value
        End Set
    End Property

    Public Property Timer(uuid As String) As Date
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._Timer
            BadUUID(uuid)
            Return Date.Now
        End Get
        Set(ByVal Value As Date)
            RegionList(uuid)._Timer = Value
        End Set
    End Property

    Private Sub BadUUID(uuid As String)

        If Debugger.IsAttached Then
            ErrorLog($"Bad UUID [{uuid}]")
        End If

    End Sub

#End Region

#Region "Options"

    Public Property AllowGods(uuid As String) As String
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._AllowGods
            BadUUID(uuid)
            Return ""
        End Get
        Set(ByVal Value As String)
            RegionList(uuid)._AllowGods = Value
        End Set
    End Property

    Public Property Birds(uuid As String) As String
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._Birds
            BadUUID(uuid)
            Return ""
        End Get
        Set(ByVal Value As String)
            RegionList(uuid)._Birds = Value
        End Set
    End Property

    Public Property Concierge(uuid As String) As String
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._Concierge
            BadUUID(uuid)
            Return ""
        End Get
        Set(ByVal Value As String)
            RegionList(uuid)._Concierge = Value
        End Set
    End Property

    Public Property DisableGloebits(uuid As String) As String
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._DisableGloebits
            BadUUID(uuid)
            Return ""
        End Get
        Set(ByVal Value As String)
            RegionList(uuid)._DisableGloebits = Value
        End Set
    End Property

    Public Property Disallow_Foreigners(uuid As String) As String
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._DisallowForeigners
            BadUUID(uuid)
            Return ""
        End Get
        Set(ByVal Value As String)
            RegionList(uuid)._DisallowForeigners = Value
        End Set
    End Property

    Public Property Disallow_Residents(uuid As String) As String
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._DisallowResidents
            BadUUID(uuid)
            Return ""
        End Get
        Set(ByVal Value As String)
            RegionList(uuid)._DisallowResidents = Value
        End Set
    End Property

    Public Property FrameTime(uuid As String) As String
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._FrameTime
            BadUUID(uuid)
            Return ""
        End Get
        Set(ByVal Value As String)
            Value = Value.Replace(",", ".")
            RegionList(uuid)._FrameTime = Value
        End Set
    End Property

    Public Property GDPR(uuid As String) As String
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._GDPR
            BadUUID(uuid)
            Return ""
        End Get
        Set(ByVal Value As String)
            RegionList(uuid)._GDPR = Value
        End Set
    End Property

    Public Property GodDefault(uuid As String) As String
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._GodDefault
            BadUUID(uuid)
            Return "True"
        End Get
        Set(ByVal Value As String)
            RegionList(uuid)._GodDefault = Value
        End Set
    End Property

    Public Property InRegion(uuid As String) As Integer
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._AvatarsInRegion
            BadUUID(uuid)
            Return 0
        End Get
        Set(ByVal Value As Integer)

            RegionList(uuid)._AvatarsInRegion = Value
        End Set
    End Property

    Public Property LandingSpot(uuid As String) As String
        Get
            If RegionList.ContainsKey(uuid) Then
                If RegionList(uuid)._RegionLandingSpot.Length = 0 Then
                    RegionList(uuid)._RegionLandingSpot = "<128,128,30>"
                End If
                Return RegionList(uuid)._RegionLandingSpot
            End If
            BadUUID(uuid)
            Return ""
        End Get
        Set(ByVal Value As String)
            RegionList(uuid)._RegionLandingSpot = Value
        End Set
    End Property

    Public Property ManagerGod(uuid As String) As String
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._ManagerGod
            BadUUID(uuid)
            Return ""
        End Get
        Set(ByVal Value As String)
            RegionList(uuid)._ManagerGod = Value
        End Set
    End Property

    Public Property MapType(uuid As String) As String
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._MapType
            BadUUID(uuid)
            Return ""
        End Get
        Set(ByVal Value As String)
            RegionList(uuid)._MapType = Value
        End Set
    End Property

    Public Property MinTimerInterval(uuid As String) As String
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._MinTimerInterval
            BadUUID(uuid)
            Return ""
        End Get
        Set(ByVal Value As String)
            If Value Is Nothing Then Return
            Value = Value.Replace(",", ".")
            RegionList(uuid)._MinTimerInterval = Value
        End Set
    End Property

    Public Property OpensimWorldAPIKey(uuid As String) As String
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._OSWAPIKey
            BadUUID(uuid)
            Return ""
        End Get
        Set(ByVal Value As String)
            RegionList(uuid)._OSWAPIKey = Value
        End Set
    End Property

    Public Property RegionEnabled(uuid As String) As Boolean
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._RegionEnabled
            BadUUID(uuid)
            Return False
        End Get
        Set(ByVal Value As Boolean)
            RegionList(uuid)._RegionEnabled = Value
        End Set
    End Property

    Public Property RegionGod(uuid As String) As String
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._RegionGod
            BadUUID(uuid)
            Return ""
        End Get
        Set(ByVal Value As String)
            RegionList(uuid)._RegionGod = Value
        End Set
    End Property

    Public Property RegionPhysics(uuid As String) As String
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._Physics
            BadUUID(uuid)
            Return ""
        End Get
        Set(ByVal Value As String)
            RegionList(uuid)._Physics = Value
        End Set
    End Property

    Public Property RegionSnapShot(uuid As String) As String
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._RegionSnapShot
            BadUUID(uuid)
            Return ""
        End Get
        Set(ByVal Value As String)
            RegionList(uuid)._RegionSnapShot = Value
        End Set
    End Property

    Public Property ScriptEngine(uuid As String) As String
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._ScriptEngine
            BadUUID(uuid)
            Return ""
        End Get
        Set(ByVal Value As String)
            RegionList(uuid)._ScriptEngine = Value
        End Set
    End Property

    Public Property SkipAutobackup(uuid As String) As String
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._SkipAutobackup
            BadUUID(uuid)
            Return ""
        End Get
        Set(ByVal Value As String)
            RegionList(uuid)._SkipAutobackup = Value
        End Set
    End Property

    ''' <summary>
    ''' Gets region Smart Start Type
    ''' </summary>
    ''' <param name="uuid"></param>
    ''' <returns>True if Smart Start</returns>
    Public Property Smart_Start(uuid As String) As Boolean
        Get
            Try
                If RegionList.ContainsKey(uuid) Then Return CBool(RegionList(uuid)._RegionSmartStart)
                BadUUID(uuid)
            Catch
            End Try
            Return False
        End Get
        Set(ByVal Value As Boolean)
            RegionList(uuid)._RegionSmartStart = CStr(Value)
        End Set
    End Property

    Public Property Snapshot(uuid As String) As String
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._Snapshot
            BadUUID(uuid)
            Return ""
        End Get
        Set(ByVal Value As String)
            RegionList(uuid)._Snapshot = Value
        End Set
    End Property

    Public Property Teleport_Sign(uuid As String) As Boolean
        Get
            Try
                If RegionList.ContainsKey(uuid) Then Return CBool(RegionList(uuid)._Teleport)
                BadUUID(uuid)
            Catch
            End Try
            Return False
        End Get
        Set(ByVal Value As Boolean)
            RegionList(uuid)._Teleport = CStr(Value)
        End Set
    End Property

    Public Property Tides(uuid As String) As String
        Get
            If RegionList.ContainsKey(uuid) Then Return RegionList(uuid)._Tides
            BadUUID(uuid)
            Return ""
        End Get
        Set(ByVal Value As String)
            RegionList(uuid)._Tides = Value
        End Set
    End Property

#End Region

#Region "Functions"

    Public Function FindRegionByName(name As String) As String

        Dim pair As KeyValuePair(Of String, Region_data)
        For Each pair In RegionList
            If name = pair.Value._RegionName Then               '
                Return pair.Value._UUID
            End If
        Next
        'RegionDump()
        Return ""

    End Function

    Public Function FindRegionUUIDByPID(PID As Integer) As String

        Dim pair As KeyValuePair(Of String, Region_data)
        For Each pair In RegionList
            If PID = pair.Value._ProcessID Then               '
                Return pair.Value._UUID
            End If
        Next
        Return ""

    End Function

    Public Function IsBooted(uuid As String) As Boolean

        If uuid Is Nothing Then Return False
        Return RegionStatus(uuid) = SIMSTATUSENUM.Booted

    End Function

    Public Function RegionPIDs() As List(Of Integer)

        Dim L As New List(Of Integer)
        For Each pair In RegionList
            L.Add(pair.Value._ProcessID)
        Next
        Return L

    End Function

    ''' <summary>
    ''' Returns a list of UUIDS of all regions in this group
    ''' </summary>
    ''' <param name="Gname">Group Name</param>
    ''' <returns>List of Region UUID's</returns>
    Public Function RegionUuidListByName(Gname As String) As List(Of String)

        Dim L As New List(Of String)
        Try
            Dim pair As KeyValuePair(Of String, Region_data)
            For Each pair In RegionList
                If pair.Value._Group = Gname Or Gname = "*" Then
                    L.Add(pair.Value._UUID)
                End If
            Next
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try
        Return L

    End Function

    ''' <summary>
    ''' Returns a list if region UUIDS sorted by name
    ''' </summary>
    ''' <returns>Alphabetized List of Region UUIDS</returns>
    Public Function RegionUuids() As List(Of String)

        Dim Tmp As New List(Of String)
        Dim Out As New List(Of String)
        Dim pair As KeyValuePair(Of String, Region_data)

        For Each pair In RegionList
            Tmp.Add(pair.Value._RegionName)
        Next

        Tmp.Sort()

        For Each name In Tmp
            Out.Add(FindRegionByName(name))
        Next

        Return Out

    End Function

#End Region

#Region "POST"

    Public Function CheckPassword(post As String, machine As String) As Boolean

        If machine Is Nothing Then Return False

        ' Returns true is password is blank or matching
        Dim pattern1 = New Regex("(?i)pw=(.*?)&", RegexOptions.IgnoreCase)
        Dim match1 As Match = pattern1.Match(post, RegexOptions.IgnoreCase)
        If match1.Success Then
            Dim p1 As String = match1.Groups(1).Value
            If p1.Length = 0 Then Return True
            If machine.ToUpper(Globalization.CultureInfo.InvariantCulture) = p1.ToUpper(Globalization.CultureInfo.InvariantCulture) Then
                Return True
            End If
        End If
        Return False

    End Function

    Public Function ParsePost(post As String) As String

        If post Is Nothing Then Return "<html><head></head><body>Error</html>"
        ' set Region.Booted to true if the POST from the region indicates it is online requires a section in Opensim.ini where [RegionReady] has this:

        '[RegionReady]

        '; Enable this module to get notified once all items And scripts in the region have been completely loaded And compiled
        'Enabled = True

        '; Channel on which to signal region readiness through a message
        '; formatted as follows: "{server_startup|oar_file_load},{0|1},n,[oar error]"
        '; - the first field indicating whether this Is an initial server startup
        '; - the second field Is a number indicating whether the OAR file loaded ok (1 == ok, 0 == error)
        '; - the third field Is a number indicating how many scripts failed to compile
        '; - "oar error" if supplied, provides the error message from the OAR load
        'channel_notify = -800

        '; - disallow logins while scripts are loading
        '; Instability can occur on regions with 100+ scripts if users enter before they have finished loading
        'login_disable = True

        '; - send an alert as json to a service
        'alert_uri = ${Const|BaseURL}:${Const|DiagnosticsPort}/${Const|RegionFolderName}

        ' POST = "GET Region name HTTP...{server_startup|oar_file_load},{0|1},n,[oar error]"
        '{"alert":"region_ready","login":"enabled","region_name":"Region 2","RegionId":"19f6adf0-5f35-4106-bcb8-dc3f2e846b89"}}
        'POST / Region%202 HTTP/1.1
        'Content-Type: Application/json
        'Host:   tea.outworldz.net : 8001
        'Content-Length:  118
        'Connection: Keep-Alive
        '
        '"{""alert""""region_ready"",""login"":""disabled"",""region_name"":""8021"",""region_id"":""c46ee5e5-5bb8-4cb5-8efd-eff44a0c7160""}"
        '"{"alert""region_ready","login":"enabled","region_name":"Region 2","region_id":"19f6adf0-5f35-4106-bcb8-dc3f2e846b89"}
        '"{""alert"":""region_ready"",""login"":""shutdown"",""region_name"":""8021"",""region_id"":""c46ee5e5-5bb8-4cb5-8efd-eff44a0c7160""}"
        ' we want region name, UUID and server_startup could also be a probe from the outworldz to check if ports are open.

        If post.Contains("""alert"":""region_ready""") Then
            WebserverList.TryAdd(post, "")
        ElseIf post.ToUpperInvariant.Contains("ALT=") Then
            Return SmartStartParse(post)
        ElseIf post.ToUpperInvariant.Contains("TOS") Then
            Return TOS(post)
        ElseIf post.ToUpperInvariant.Contains("SET_PARTNER") Then
            Return SetPartner(post)
        ElseIf post.ToUpperInvariant.Contains("GET_PARTNER") Then
            Return GetPartner(post)
        ElseIf post.ToUpperInvariant.Contains("TTS") Then
            Return Text2Speech(post)
        End If

        Return "Test Completed"

    End Function

#End Region

#Region "TOS"

    Private Function TOS(post As String) As String
        ' currently unused as is only in standalone
        Debug.Print("UUID:" + post)
        '"POST /TOS HTTP/1.1" & vbCrLf & "Host: mach.outworldz.net:8001" & vbCrLf & "Connection: keep-alive" & vbCrLf & "Content-Length: 102" & vbCrLf & "Cache-Control: max-age=0" & vbCrLf & "Upgrade-Insecure-Requests: 1" & vbCrLf & "User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36" & vbCrLf & "Origin: http://mach.outworldz.net:9201" & vbCrLf & "Content-Type: application/x-www-form-urlencoded" & vbCrLf & "DNT: 1" & vbCrLf & "Accept: text/html,application/xhtml+xml,application/xml;q=0.0909,image/webp,image/apng,*/*;q=0.8" & vbCrLf & "Referer: http://mach.outworldz.net:9200/wifi/termsofservice.html?uid=acb8fd92-c725-423f-b750-5fd971d73182&sid=40c5b80a-5377-4b97-820c-a0952782a701" & vbCrLf & "Accept-Encoding: gzip, deflate" & vbCrLf & "Accept-Language: en-US,en;q=0.0909" & vbCrLf & vbCrLf &
        '"action-accept=Accept&uid=acb8fd92-c725-423f-b750-5fd971d73182&sid=40c5b80a-5377-4b97-820c-a0952782a701"

        Dim sid As Guid

        Try
            Dim webpage = ""
            Dim pattern = New Regex("uid=([0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12})")
            Dim match As Match = pattern.Match(post)
            If match.Success Then

                Dim include = tosinclude(match.Groups(1).Value)

                Dim Header = IO.Path.Combine(Settings.CurrentDirectory, "termsofservice.html")
                Using streamReader As New System.IO.FileStream(Header, FileMode.Open, FileAccess.Read, FileShare.Read)
                    Using S = New StreamReader(streamReader)
                        'now loop through each line and append the css to web page
                        While S.Peek <> -1
                            Dim line = S.ReadLine
                            line = line.Replace("[GRIDNAME]", Settings.SimName)
                            line = line.Replace("[TOS]", include & vbCrLf)
                            webpage += line & vbCrLf
                        End While
                    End Using
                End Using
                webpage += "</style>"
                webpage += "</head>"
                Return webpage
            End If

            ' detect and print a response to an ACCEPT
            Dim pattern2 = New Regex("sid=([0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12})")
            Dim match2 As Match = pattern2.Match(post)
            If match2.Success Then
                sid = Guid.Parse(match2.Groups(1).Value)
            End If

            If match2.Success Then
                Agree2Tos(sid)  ' detect and print a response to an ACCEPT
                Dim include = "Welcome! You can close this window."
                Dim Header = IO.Path.Combine(Settings.CurrentDirectory, "termsofservice.html")
                Using streamReader As New System.IO.FileStream(Header, FileMode.Open, FileAccess.Read, FileShare.Read)
                    Using S = New StreamReader(streamReader)
                        'now loop through each line and append the css to web page
                        While S.Peek <> -1
                            Dim line = S.ReadLine
                            line = line.Replace("[GRIDNAME]", Settings.SimName)
                            line = line.Replace("[TOS]", include & vbCrLf)
                            webpage += line & vbCrLf
                        End While
                    End Using
                End Using
                webpage += "</style>"
                webpage += "</head>"
                Return webpage
            Else
                Return "<html><head></head><body>Test Passed</html>"
            End If
        Catch ex As Exception
            BreakPoint.Dump(ex)
            Return "<html><head></head><body>Error</html>"
        End Try

    End Function

    Private Function tosinclude(uid As String) As String

        ' Print a TOS webpage to the client
        Dim include = ""
        Dim From = IO.Path.Combine(Settings.CurrentDirectory, "tos.html")
        Using streamRead As New System.IO.FileStream(From, FileMode.Open, FileAccess.Read, FileShare.Read)
            Using S = New StreamReader(streamRead)
                'now loop through each line
                While S.Peek <> -1
                    Dim data = S.ReadLine
                    data = data.Replace("[GRIDNAME]", Settings.SimName)
                    include += data
                End While
            End Using
            include += "<form>"
            include += "<input class='button' type='Submit' name='agree' value='Agree wth TOS'>"
            include += $"<input type='hidden' name='sid' value='{uid.ToString}'>"
            include += "</form>"
            include += "</body>"
            include += "</html>"
        End Using

        Return include
    End Function

#End Region

#Region "Partners"

    Public Function SetPartner(post As String) As String

        Debug.Print("set Partner")
        Dim PWok As Boolean = CheckPassword(post, CStr(Settings.MachineId()))
        If Not PWok Then Return ""

        Dim pattern1 = New Regex("User=(.*?)&", RegexOptions.IgnoreCase)
        Dim match1 As Match = pattern1.Match(post)
        If match1.Success Then

            Dim p2 As String = ""
            Dim p1 = match1.Groups(1).Value
            Dim pattern2 = New Regex("Partner=(.*)", RegexOptions.IgnoreCase)
            Dim match2 As Match = pattern2.Match(post)
            If match2.Success Then
                p2 = match2.Groups(1).Value
            End If
            Dim result1 As New Guid
            Dim result2 As New Guid
            If Guid.TryParse(p1, result1) AndAlso Guid.TryParse(p2, result2) Then
                Try

                    Dim Partner = MysqlGetPartner(p1, Settings)
                    If Partner = "00000000-0000-0000-0000-000000000000" Then
                        Partner = ""
                    End If

                    Using myConnection = New MySqlConnection(Settings.RobustMysqlConnection)
                        myConnection.Open()
                        Dim Query1 = "update userprofile set profilePartner=@p2 where useruuid=@p1; "
                        Using myCommand1 = New MySqlCommand(Query1) With {
                                .Connection = myConnection
                            }

                            myCommand1.Parameters.AddWithValue("@p1", result1.ToString)
                            myCommand1.Parameters.AddWithValue("@p2", result2.ToString)
                            Dim x = myCommand1.ExecuteNonQuery()
                            If x <> 1 Then
                                BreakPoint.Print($"Failed to return Partner rowcount={x}")
                            End If
                        End Using
                    End Using
                    Return Partner
                Catch ex As Exception
                    BreakPoint.Dump(ex)
                End Try

            End If
        End If
        Debug.Print("NULL response")
        Return ""

    End Function

    Private Function GetPartner(post As String) As String

        Debug.Print("Get Partner")
        Dim PWok As Boolean = CheckPassword(post, Settings.MachineId())
        If Not PWok Then Return ""

        Dim pattern1 = New Regex("User=(.*)", RegexOptions.IgnoreCase)
        Dim match1 As Match = pattern1.Match(post)
        Dim p1 As String
        If match1.Success Then
            p1 = match1.Groups(1).Value
            Dim s = MysqlGetPartner(p1, Settings)
            Debug.Print(s)
            Return s
        Else
            Debug.Print("No partner")

            Return ""
            '"00000000-0000-0000-0000-000000000000"
        End If

    End Function

#End Region

#Region "Opensim.ini writers"

    Public Function CopyOpensimProto(uuid As String) As Boolean

        Try
            '============== Opensim.ini =====================
            Dim OpensimPathName = OpensimIniPath(uuid)
            Dim Name = Region_Name(uuid)
            Dim Group = Group_Name(uuid)

            ' copy the prototype to the regions Opensim.ini

            CopyFileFast(GetOpensimProto(), IO.Path.Combine(OpensimPathName, "Opensim.ini"))
            Thread.Sleep(10)

            Dim INI = New LoadIni(IO.Path.Combine(OpensimPathName, "Opensim.ini"), ";", System.Text.Encoding.UTF8)
            If INI Is Nothing Then Return True

            If INI.SetIni("Const", "MachineID", Settings.MachineId) Then Return True

            If Settings.StatusInterval > 0 Then
                If INI.SetIni("Startup", "timer_Script", "debug.txt") Then Return True
                If INI.SetIni("Startup", "timer_Interval", CStr(Settings.StatusInterval)) Then Return True
            Else
                If INI.SetIni("Startup", "timer_Script", "") Then Return True
                If INI.SetIni("Startup", "timer_Interval", "1200") Then Return True
            End If

            If INI.SetIni("RemoteAdmin", "port", CStr(GroupPort(uuid))) Then Return True
            If INI.SetIni("RemoteAdmin", "access_password", Settings.MachineId) Then Return True
            If INI.SetIni("Const", "PrivatePort", CStr(Settings.PrivatePort)) Then Return True
            If INI.SetIni("Const", "RegionFolderName", Group_Name(uuid)) Then Return True
            If INI.SetIni("Const", "BaseHostname", Settings.BaseHostName) Then Return True
            If INI.SetIni("Const", "PublicPort", CStr(Settings.HttpPort)) Then Return True ' 8002
            If INI.SetIni("Const", "PrivURL", "http://" & CStr(Settings.LANIP())) Then Return True ' local IP
            If INI.SetIni("Const", "http_listener_port", CStr(GroupPort(uuid))) Then Return True ' varies with region

            If INI.SetIni("Chat", "whisper_distance", Settings.WhisperDistance) Then Return True
            If INI.SetIni("Chat", "say_distance", Settings.SayDistance) Then Return True
            If INI.SetIni("Chat", "shout_distance", Settings.ShoutDistance) Then Return True

            Select Case Settings.ServerType
                Case RobustServerName
                    SetupOpensimSearchINI(INI)
                    If INI.SetIni("Const", "PrivURL", "http://" & Settings.LANIP()) Then Return True
                    If INI.SetIni("Const", "GridName", Settings.SimName) Then Return True
                    SetupOpensimIM(INI)
                Case RegionServerName
                    SetupOpensimSearchINI(INI)
                    SetupOpensimIM(INI)
                Case OsgridServer
                Case MetroServer
            End Select

            If Settings.CMS = JOpensim Then
                If INI.SetIni("UserProfiles", "ProfileServiceURL", "") Then Return True
                If INI.SetIni("Groups", "Module", "GroupsModule") Then Return True
                If INI.SetIni("Groups", "ServicesConnectorModule", """" & "XmlRpcGroupsServicesConnector" & """") Then Return True
                If INI.SetIni("Groups", "MessagingModule", "GroupsMessagingModule") Then Return True
                If INI.SetIni("Groups", "GroupsServerURI", "http://" & Settings.PublicIP & ":" & Settings.ApachePort & "/jOpensim/index.php?option=com_opensim&view=interface") Then Return True
            End If

            If INI.SetIni("Const", "ApachePort", CStr(Settings.ApachePort)) Then Return True

            ' Support viewers object cache, default true users may need to reduce viewer bandwidth if some prims Or terrain parts fail to rez. change to false if you need to use old viewers that do Not
            ' support this feature

            If INI.SetIni("ClientStack.LindenUDP", "SupportViewerObjectsCache", CStr(Settings.SupportViewerObjectsCache)) Then Return True

            'ScriptEngine
            If INI.SetIni("Startup", "DefaultScriptEngine", Settings.ScriptEngine) Then Return True
            If Settings.ScriptEngine = "XEngine" Then
                If INI.SetIni("Startup", "DefaultScriptEngine", "XEngine") Then Return True
                If INI.SetIni("XEngine", "Enabled", "True") Then Return True
                If INI.SetIni("YEngine", "Enabled", "False") Then Return True
            ElseIf Settings.ScriptEngine = "YEngine" Then
                If INI.SetIni("Startup", "DefaultScriptEngine", "YEngine") Then Return True
                If INI.SetIni("XEngine", "Enabled", "False") Then Return True
                If INI.SetIni("YEngine", "Enabled", "True") Then Return True
            Else
                If INI.SetIni("Startup", "DefaultScriptEngine", "YEngine") Then Return True
                If INI.SetIni("XEngine", "Enabled", "False") Then Return True
                If INI.SetIni("YEngine", "Enabled", "False") Then Return True
            End If

            'Script Override
            If ScriptEngine(uuid) = "XEngine" Then
                If INI.SetIni("XEngine", "Enabled", "True") Then Return True
                If INI.SetIni("YEngine", "Enabled", "False") Then Return True
                If INI.SetIni("Startup", "DefaultScriptEngine", "XEngine") Then Return True
            ElseIf ScriptEngine(uuid) = "YEngine" Then
                If INI.SetIni("XEngine", "Enabled", "False") Then Return True
                If INI.SetIni("YEngine", "Enabled", "True") Then Return True
                If INI.SetIni("Startup", "DefaultScriptEngine", "YEngine") Then Return True
            ElseIf ScriptEngine(uuid) = "Off" Then
                If INI.SetIni("Startup", "DefaultScriptEngine", "YEngine") Then Return True
                If INI.SetIni("XEngine", "Enabled", "False") Then Return True
                If INI.SetIni("YEngine", "Enabled", "False") Then Return True
            End If

            ' Script timers
            ' set new Min Timer Interval for how fast a script can go.
            If INI.SetIni("XEngine", "MinTimerInterval", CStr(Settings.MinTimerInterval)) Then Return True
            If INI.SetIni("YEngine", "MinTimerInterval", CStr(Settings.MinTimerInterval)) Then Return True

            ' set new Min Timer Interval for how fast a script can go. Can be set in region files as a float, or nothing
            Dim Xtime As Double = 1 / 11   '1/11 of a second is as fast as she can go
            If MinTimerInterval(uuid).Length > 0 Then
                If Not Double.TryParse(MinTimerInterval(uuid), Xtime) Then
                    Xtime = 0.0909
                End If
            End If

            If MinTimerInterval(uuid).Length > 0 Then
                If INI.SetIni("YEngine", "MinTimerInterval", CStr(MinTimerInterval(uuid))) Then Return True
                If INI.SetIni("XEngine", "MinTimerInterval", CStr(MinTimerInterval(uuid))) Then Return True
            End If

            ' Main Frame time
            ' This defines the rate of several simulation events.
            ' Default value should meet most needs.
            ' It can be reduced To improve the simulation Of moving objects, with possible increase of CPU and network loads.
            'FrameTime = 0.0909

            If INI.SetIni("Startup", "FrameTime", Convert.ToString(1 / 11, Globalization.CultureInfo.InvariantCulture)) Then Return True

            If FrameTime(uuid).Length > 0 Then
                If INI.SetIni("Startup", "FrameTime", CStr(FrameTime(uuid))) Then Return True
            End If

            ' God
            Select Case Settings.AllowGridGods
                Case True
                    If INI.SetIni("Permissions", "allow_grid_gods", "True") Then Return True
                Case False
                    If INI.SetIni("Permissions", "allow_grid_gods", "False") Then Return True
            End Select

            Select Case Settings.RegionOwnerIsGod
                Case True
                    If INI.SetIni("Permissions", "region_owner_is_god", "True") Then Return True
                Case False
                    If INI.SetIni("Permissions", "region_owner_is_god", "False") Then Return True
            End Select

            Select Case Settings.RegionManagerIsGod
                Case True
                    If INI.SetIni("Permissions", "region_manager_is_god", "True") Then Return True
                Case False
                    If INI.SetIni("Permissions", "region_manager_is_god", "False") Then Return True
            End Select

            ' God Overrides
            Select Case AllowGods(uuid)
                Case "True"
                    If INI.SetIni("Permissions", "allow_grid_gods", "True") Then Return True
            End Select

            Select Case RegionGod(uuid)
                Case "True"
                    If INI.SetIni("Permissions", "region_owner_is_god", "True") Then Return True
            End Select

            Select Case ManagerGod(uuid)
                Case "True"
                    If INI.SetIni("Permissions", "region_manager_is_god", "True") Then Return True
            End Select

            ' all grids requires these setting in Opensim.ini
            If INI.SetIni("Const", "DiagnosticsPort", CStr(Settings.DiagnosticPort)) Then Return True

            If INI.SetIni("XEngine", "DeleteScriptsOnStartup", "False") Then Return True

            If Not Settings.LSLHTTP Then
                If INI.SetIni("Network", "OutboundDisallowForUserScriptsExcept",
                               $"{Settings.PublicIP}:{Settings.DiagnosticPort}|127.0.0.1:{Settings.DiagnosticPort}|localhost:{Settings.DiagnosticPort}|{Settings.LANIP()}:{Settings.DiagnosticPort}|{Settings.LANIP()}:{Settings.HttpPort}|{Settings.PublicIP()}:{Settings.HttpPort}") Then Return True
            End If

            If INI.SetIni("PrimLimitsModule", "EnforcePrimLimits", CStr(Settings.Primlimits)) Then Return True
            If Settings.Primlimits Then
                If INI.SetIni("Permissions", "permissionmodules", "DefaultPermissionsModule, PrimLimitsModule") Then Return True
            Else
                If INI.SetIni("Permissions", "permissionmodules", "DefaultPermissionsModule") Then Return True
            End If

            If Settings.GloebitsEnable Then
                If INI.SetIni("Startup", "economymodule", "Gloebit") Then Return True
                If INI.SetIni("Economy", "CurrencyURL", "") Then Return True
            ElseIf Settings.CMS = JOpensim Then
                If INI.SetIni("Startup", "economymodule", "jOpenSimMoneyModule") Then Return True
                If INI.SetIni("Economy", "CurrencyURL", "{$Const|BaseURL}:${Const|ApachePort}/jOpensim/index.php?option=com_opensim&view=interface") Then Return True
            Else
                If INI.SetIni("Startup", "economymodule", "BetaGridLikeMoneyModule") Then Return True
                ' Any old URL will do for any amount of money
                If INI.SetIni("Economy", "CurrencyURL", $"{Settings.PublicIP}:{Settings.DiagnosticPort}") Then Return True
            End If

            If INI.SetIni("SMTP", "enabled", CStr(Settings.EmailEnabled)) Then Return True

            ' LSL emails
            If INI.SetIni("SMTP", "SMTP_SERVER_HOSTNAME", Settings.SmtpHost) Then Return True
            If INI.SetIni("SMTP", "SMTP_SERVER_PORT", CStr(Settings.SmtpPort)) Then Return True
            If INI.SetIni("SMTP", "SMTP_SERVER_LOGIN", Settings.SmtPropUserName) Then Return True

            ' Some SMTP servers require a known From email address or will give Error 500 - Envelope from address is not authorized
            '; set to a valid email address that SMTP will accept (in some cases must be Like SMTP_SERVER_LOGIN)

            If INI.SetIni("SMTP", "SMTP_SERVER_FROM", Settings.SmtPropUserName) Then Return True
            If INI.SetIni("SMTP", "SMTP_SERVER_PASSWORD", Settings.SmtpPassword) Then Return True
            If INI.SetIni("SMTP", "SMTP_VerifyCertNames", CStr(Settings.VerifyCertCheckBox)) Then Return True
            If INI.SetIni("SMTP", "SMTP_VerifyCertChain", CStr(Settings.VerifyCertCheckBox)) Then Return True
            If INI.SetIni("SMTP", "enableEmailToExternalObjects", CStr(Settings.EnableEmailToExternalObjects)) Then Return True
            If INI.SetIni("SMTP", "enableEmailToSMTP", CStr(Settings.EnableEmailToSMTPCheckBox)) Then Return True
            If INI.SetIni("SMTP", "MailsFromOwnerPerHour", CStr(Settings.MailsFromOwnerPerHour)) Then Return True
            If INI.SetIni("SMTP", "MailsToPrimAddressPerHour", CStr(Settings.MailsToPrimAddressPerHour)) Then Return True
            If INI.SetIni("SMTP", "SMTP_MailsPerDay", CStr(Settings.MailsPerDay)) Then Return True
            If INI.SetIni("SMTP", "MailsToSMTPAddressPerHour", CStr(Settings.EmailsToSMTPAddressPerHour)) Then Return True
            If INI.SetIni("SMTP", "email_pause_time", CStr(Settings.Email_pause_time)) Then Return True
            If INI.SetIni("SMTP", "email_max_size", CStr(Settings.MaxMailSize)) Then Return True
            If INI.SetIni("SMTP", "SMTP_SERVER_TLS", CStr(Settings.SmtpSecure)) Then Return True
            If INI.SetIni("SMTP", "host_domain_header_from", Settings.BaseHostName) Then Return True

            ' Physics choices for meshmerizer, where ODE requires a special one ZeroMesher meshing = Meshmerizer meshing = ubODEMeshmerizer 0 = none 1 = OpenDynamicsEngine 2 = BulletSim 3 = BulletSim with
            ' threads 4 = ubODE

            Select Case Settings.Physics
                Case 0 'Physics_Default
                    If INI.SetIni("Startup", "meshing", "ZeroMesher") Then Return True
                    If INI.SetIni("Startup", "physics", "basicphysics") Then Return True
                    If INI.SetIni("Startup", "UseSeparatePhysicsThread", "False") Then Return True
                    If INI.SetIni("ODEPhysicsSettings", "use_NINJA_physics_joints", "False") Then Return True
                Case 2 'Physics_Bullet
                    If INI.SetIni("Startup", "meshing", "Meshmerizer") Then Return True
                    If INI.SetIni("Startup", "physics", "BulletSim") Then Return True
                    If INI.SetIni("Startup", "UseSeparatePhysicsThread", "False") Then Return True
                    If INI.SetIni("ODEPhysicsSettings", "use_NINJA_physics_joints", "False") Then Return True
                Case 3 ' Physics_Separate thread
                    If INI.SetIni("Startup", "meshing", "Meshmerizer") Then Return True
                    If INI.SetIni("Startup", "physics", "BulletSim") Then Return True
                    If INI.SetIni("Startup", "UseSeparatePhysicsThread", "True") Then Return True
                    If INI.SetIni("ODEPhysicsSettings", "use_NINJA_physics_joints", "False") Then Return True
                Case 4 ' Physics_ubODE
                    If INI.SetIni("Startup", "meshing", "ubODEMeshmerizer") Then Return True
                    If INI.SetIni("Startup", "physics", "ubODE") Then Return True
                    If INI.SetIni("Startup", "UseSeparatePhysicsThread", "False") Then Return True
                    If INI.SetIni("ODEPhysicsSettings", "use_NINJA_physics_joints", CStr(Settings.NinjaRagdoll)) Then Return True
                Case 5 'Physics_Hybrid
                    If INI.SetIni("Startup", "meshing", "Meshmerizer") Then Return True
                    If INI.SetIni("Startup", "physics", "ubODE") Then Return True
                    If INI.SetIni("Startup", "UseSeparatePhysicsThread", "False") Then Return True
                    If INI.SetIni("ODEPhysicsSettings", "use_NINJA_physics_joints", CStr(Settings.NinjaRagdoll)) Then Return True
                Case Else
                    If INI.SetIni("Startup", "meshing", "Meshmerizer") Then Return True
                    If INI.SetIni("Startup", "physics", "BulletSim") Then Return True
                    If INI.SetIni("Startup", "UseSeparatePhysicsThread", "True") Then Return True
                    If INI.SetIni("ODEPhysicsSettings", "use_NINJA_physics_joints", "False") Then Return True
            End Select

            'Override Physics
            Select Case RegionPhysics(uuid)
                Case "0" 'Physics_Default
                    If INI.SetIni("Startup", "meshing", "ZeroMesher") Then Return True
                    If INI.SetIni("Startup", "physics", "basicphysics") Then Return True
                    If INI.SetIni("Startup", "UseSeparatePhysicsThread", "False") Then Return True
                Case "1" ' default
                    If INI.SetIni("Startup", "meshing", "Meshmerizer") Then Return True
                    If INI.SetIni("Startup", "physics", "BulletSim") Then Return True
                    If INI.SetIni("Startup", "UseSeparatePhysicsThread", "False") Then Return True
                Case "2"  'Physics_Bullet
                    If INI.SetIni("Startup", "meshing", "Meshmerizer") Then Return True
                    If INI.SetIni("Startup", "physics", "BulletSim") Then Return True
                    If INI.SetIni("Startup", "UseSeparatePhysicsThread", "False") Then Return True
                Case "3" ' Physics_Separate thread
                    If INI.SetIni("Startup", "meshing", "Meshmerizer") Then Return True
                    If INI.SetIni("Startup", "physics", "BulletSim") Then Return True
                    If INI.SetIni("Startup", "UseSeparatePhysicsThread", "True") Then Return True
                Case "4" ' Physics_ubODE
                    If INI.SetIni("Startup", "meshing", "ubODEMeshmerizer") Then Return True
                    If INI.SetIni("Startup", "physics", "ubODE") Then Return True
                    If INI.SetIni("Startup", "UseSeparatePhysicsThread", "False") Then Return True
                Case "5" 'Physics_Hybrid
                    If INI.SetIni("Startup", "meshing", "Meshmerizer") Then Return True
                    If INI.SetIni("Startup", "physics", "ubODE") Then Return True
                    If INI.SetIni("Startup", "UseSeparatePhysicsThread", "False") Then Return True
                Case Else
                    ' do nothing
            End Select

            ' Maps
            If INI.SetIni("Map", "RenderMaxHeight", CStr(Settings.RenderMaxHeight)) Then Return True
            If INI.SetIni("Map", "RenderMinHeight", CStr(Settings.RenderMinHeight)) Then Return True

            If Settings.MapType = "None" Then
                If INI.SetIni("Map", "GenerateMaptiles", "False") Then Return True
            ElseIf Settings.MapType = "Simple" Then
                If INI.SetIni("Map", "GenerateMaptiles", "True") Then Return True
                If INI.SetIni("Map", "MapImageModule", "MapImageModule") Then Return True  ' versus Warp3DImageModule
                If INI.SetIni("Map", "TextureOnMapTile", "False") Then Return True         ' versus true
                If INI.SetIni("Map", "DrawPrimOnMapTile", "False") Then Return True
                If INI.SetIni("Map", "TexturePrims", "False") Then Return True
                If INI.SetIni("Map", "RenderMeshes", "False") Then Return True
            ElseIf Settings.MapType = "Good" Then
                If INI.SetIni("Map", "GenerateMaptiles", "True") Then Return True
                If INI.SetIni("Map", "MapImageModule", "Warp3DImageModule") Then Return True  ' versus MapImageModule
                If INI.SetIni("Map", "TextureOnMapTile", "False") Then Return True         ' versus true
                If INI.SetIni("Map", "DrawPrimOnMapTile", "False") Then Return True
                If INI.SetIni("Map", "TexturePrims", "False") Then Return True
                If INI.SetIni("Map", "RenderMeshes", "False") Then Return True
            ElseIf Settings.MapType = "Better" Then
                If INI.SetIni("Map", "GenerateMaptiles", "True") Then Return True
                If INI.SetIni("Map", "MapImageModule", "Warp3DImageModule") Then Return True  ' versus MapImageModule
                If INI.SetIni("Map", "TextureOnMapTile", "True") Then Return True         ' versus true
                If INI.SetIni("Map", "DrawPrimOnMapTile", "True") Then Return True
                If INI.SetIni("Map", "TexturePrims", "False") Then Return True
                If INI.SetIni("Map", "RenderMeshes", "False") Then Return True
            ElseIf Settings.MapType = "Best" Then
                If INI.SetIni("Map", "GenerateMaptiles", "True") Then Return True
                If INI.SetIni("Map", "MapImageModule", "Warp3DImageModule") Then Return True  ' versus MapImageModule
                If INI.SetIni("Map", "TextureOnMapTile", "True") Then Return True      ' versus true
                If INI.SetIni("Map", "DrawPrimOnMapTile", "True") Then Return True
                If INI.SetIni("Map", "TexturePrims", "True") Then Return True
                If INI.SetIni("Map", "RenderMeshes", "True") Then Return True
            End If

            'Override Map
            If MapType(uuid) = "None" Then
                If INI.SetIni("Map", "GenerateMaptiles", "False") Then Return True
            ElseIf MapType(uuid) = "Simple" Then
                If INI.SetIni("Map", "GenerateMaptiles", "True") Then Return True
                If INI.SetIni("Map", "MapImageModule", "MapImageModule") Then Return True  ' versus Warp3DImageModule
                If INI.SetIni("Map", "TextureOnMapTile", "False") Then Return True         ' versus true
                If INI.SetIni("Map", "DrawPrimOnMapTile", "False") Then Return True
                If INI.SetIni("Map", "TexturePrims", "False") Then Return True
                If INI.SetIni("Map", "RenderMeshes", "False") Then Return True
            ElseIf MapType(uuid) = "Good" Then
                If INI.SetIni("Map", "GenerateMaptiles", "True") Then Return True
                If INI.SetIni("Map", "MapImageModule", "Warp3DImageModule") Then Return True  ' versus MapImageModule
                If INI.SetIni("Map", "TextureOnMapTile", "False") Then Return True         ' versus true
                If INI.SetIni("Map", "DrawPrimOnMapTile", "False") Then Return True
                If INI.SetIni("Map", "TexturePrims", "False") Then Return True
                If INI.SetIni("Map", "RenderMeshes", "False") Then Return True
            ElseIf MapType(uuid) = "Better" Then
                If INI.SetIni("Map", "GenerateMaptiles", "True") Then Return True
                If INI.SetIni("Map", "MapImageModule", "Warp3DImageModule") Then Return True  ' versus MapImageModule
                If INI.SetIni("Map", "TextureOnMapTile", "True") Then Return True         ' versus true
                If INI.SetIni("Map", "DrawPrimOnMapTile", "True") Then Return True
                If INI.SetIni("Map", "TexturePrims", "False") Then Return True
                If INI.SetIni("Map", "RenderMeshes", "False") Then Return True
            ElseIf MapType(uuid) = "Best" Then
                If INI.SetIni("Map", "GenerateMaptiles", "True") Then Return True
                If INI.SetIni("Map", "MapImageModule", "Warp3DImageModule") Then Return True  ' versus MapImageModule
                If INI.SetIni("Map", "TextureOnMapTile", "True") Then Return True      ' versus true
                If INI.SetIni("Map", "DrawPrimOnMapTile", "True") Then Return True
                If INI.SetIni("Map", "TexturePrims", "True") Then Return True
                If INI.SetIni("Map", "RenderMeshes", "True") Then Return True
            End If

            ' Voice
            If Settings.VivoxEnabled Then
                If INI.SetIni("VivoxVoice", "enabled", "True") Then Return True
            Else
                If INI.SetIni("VivoxVoice", "enabled", "False") Then Return True
            End If

            If INI.SetIni("VivoxVoice", "vivox_admin_user", Settings.VivoxUserName) Then Return True
            If INI.SetIni("VivoxVoice", "vivox_admin_password", Settings.VivoxPassword) Then Return True

            ' Gloebit
            If INI.SetIni("Gloebit", "Enabled", CStr(Settings.GloebitsEnable)) Then Return True
            If INI.SetIni("Gloebit", "GLBShowNewSessionAuthIM", CStr(Settings.GlbShowNewSessionAuthIM)) Then Return True
            If INI.SetIni("Gloebit", "GLBShowNewSessionPurchaseIM", CStr(Settings.GlbShowNewSessionPurchaseIM)) Then Return True
            If INI.SetIni("Gloebit", "GLBShowWelcomeMessage", CStr(Settings.GlbShowWelcomeMessage)) Then Return True

            If INI.SetIni("Gloebit", "GLBEnvironment", "production") Then Return True
            If INI.SetIni("Gloebit", "GLBKey", Settings.GLProdKey) Then Return True
            If INI.SetIni("Gloebit", "GLBSecret", Settings.GLProdSecret) Then Return True

            If INI.SetIni("Gloebit", "GLBOwnerName", Settings.GlbOwnerName) Then Return True
            If INI.SetIni("Gloebit", "GLBOwnerEmail", Settings.GlbOwnerEmail) Then Return True

            If Settings.ServerType = RobustServerName Then
                If INI.SetIni("Gloebit", "GLBSpecificConnectionString", Settings.RobustDBConnection) Then Return True
            Else
                If INI.SetIni("Gloebit", "GLBSpecificConnectionString", Settings.RegionDBConnection) Then Return True
            End If

            ' Autobackup
            If INI.SetIni("AutoBackupModule", "AutoBackup", "True") Then Return True
            If Settings.AutoBackup AndAlso String.IsNullOrEmpty((uuid)) Then Return True
            If INI.SetIni("AutoBackupModule", "AutoBackup", "True") Then Return True

            If Settings.AutoBackup AndAlso SkipAutobackup(uuid) = "True" Then
                If INI.SetIni("AutoBackupModule", "AutoBackup", "False") Then Return True
            End If
            If Not Settings.AutoBackup Then
                If INI.SetIni("AutoBackupModule", "AutoBackup", "False") Then Return True
            End If

            If Not Settings.BackupOARs Then
                If INI.SetIni("AutoBackupModule", "AutoBackup", "False") Then Return True
            End If

            If INI.SetIni("AutoBackupModule", "AutoBackupInterval", Settings.AutobackupInterval) Then Return True
            If INI.SetIni("AutoBackupModule", "AutoBackupKeepFilesForDays", CStr(Settings.KeepForDays)) Then Return True
            If INI.SetIni("AutoBackupModule", "AutoBackupDir", BackupPath()) Then Return True

            If NonPhysical_PrimMax(uuid).Length > 0 Then
                If INI.SetIni("Startup", "NonPhysicalPrimMax", CStr(NonPhysical_PrimMax(uuid))) Then Return True
            End If

            If Physical_PrimMax(uuid).Length > 0 Then
                If INI.SetIni("Startup", "PhysicalPrimMax", CStr(Physical_PrimMax(uuid))) Then Return True
            End If

            If Disallow_Foreigners(uuid) = "True" Then
                If INI.SetIni("DisallowForeigners", "Enabled", CStr(Disallow_Foreigners(uuid))) Then Return True
            End If

            If Disallow_Residents(uuid) = "True" Then
                If INI.SetIni("DisallowResidents", "Enabled", CStr(Disallow_Residents(uuid))) Then Return True
            End If

            ' TODO replace with a PHP module?
            If DisableGloebits(uuid) = "True" Then
                If INI.SetIni("Startup", "economymodule", "BetaGridLikeMoneyModule") Then Return True
            End If

            ' Search
            Select Case Snapshot(uuid)
                Case ""
                    If INI.SetIni("DataSnapshot", "index_sims", CStr(Settings.SearchEnabled)) Then Return True
                Case "True"
                    If INI.SetIni("DataSnapshot", "index_sims", "True") Then Return True
                Case "False"
                    If INI.SetIni("DataSnapshot", "index_sims", "False") Then Return True
            End Select

            If Settings.Concierge Then
                Select Case Concierge(uuid)
                    Case ""
                        If INI.SetIni("Concierge", "enabled", "False") Then Return True
                    Case "True"
                        If INI.SetIni("Concierge", "enabled", Concierge(uuid)) Then Return True
                    Case "False"
                        If INI.SetIni("Concierge", "enabled", "False") Then Return True
                End Select
            End If

            If Settings.Smart_Start Then
                INI.SetIni("SmartStart", "Enabled", "True")
                INI.SetIni("SmartStart", "URL", $"http://{Settings.LANIP}:{Settings.DiagnosticPort}")
                INI.SetIni("SmartStart", "MachineID", CStr(Settings.MachineId))
            Else
                INI.SetIni("SmartStart", "Enabled", "False")
                'nope
                INI.SetIni("SmartStart", "URL", "")
                INI.SetIni("SmartStart", "MachineID", "")
            End If

            If INI.SetIni("Estates", "DefaultEstateName", gEstateName) Then Return True
            If INI.SetIni("Estates", "DefaultEstateOwnerName", gEstateOwner) Then Return True
            INI.SaveIni()

            '============== Region.ini =====================
            ' Region.ini in Region Folder specific to this region

            For Each uuid In RegionUuidListByName(Group)

                Dim regionINI = New LoadIni(RegionIniFilePath(uuid), ";", System.Text.Encoding.UTF8)

                ' Need the filename from this INI

                Name = Region_Name(uuid)

                If regionINI.SetIni(Name, "DefaultLanding", LandingSpot(uuid)) Then Return True

                If regionINI.SetIni(Name, "InternalPort", CStr(Region_Port(uuid))) Then Return True
                If regionINI.SetIni(Name, "GroupPort", CStr(GroupPort(uuid))) Then Return True
                If regionINI.SetIni(Name, "ExternalHostName", Settings.ExternalHostName()) Then Return True
                If regionINI.SetIni(Name, "ClampPrimSize", CStr(Clamp_PrimSize(uuid))) Then Return True

                ' not a standard only use by the Dreamers
                If RegionEnabled(uuid) Then
                    If regionINI.SetIni(Name, "Enabled", "True") Then Return True
                Else
                    If regionINI.SetIni(Name, "Enabled", "False") Then Return True
                End If

                Select Case NonPhysical_PrimMax(uuid)
                    Case ""
                        If regionINI.SetIni(Name, "NonPhysicalPrimMax", CStr(1024)) Then Return True
                    Case Else
                        If regionINI.SetIni(Name, "NonPhysicalPrimMax", NonPhysical_PrimMax(uuid)) Then Return True
                End Select

                Select Case Physical_PrimMax(uuid)
                    Case ""
                        If regionINI.SetIni(Name, "PhysicalPrimMax", CStr(64)) Then Return True
                    Case Else
                        If regionINI.SetIni(Name, "PhysicalPrimMax", Physical_PrimMax(uuid)) Then Return True
                End Select

                If Settings.Primlimits Then
                    Select Case Max_Prims(uuid)
                        Case ""
                            If regionINI.SetIni(Name, "MaxPrims", CStr(45000)) Then Return True
                        Case Else
                            If regionINI.SetIni(Name, "MaxPrims", Max_Prims(uuid)) Then Return True
                    End Select
                Else
                    Select Case Max_Prims(uuid)
                        Case ""
                            If regionINI.SetIni(Name, "MaxPrims", CStr(45000)) Then Return True
                        Case Else
                            If regionINI.SetIni(Name, "MaxPrims", Max_Prims(uuid)) Then Return True
                    End Select
                End If

                Select Case Max_Agents(uuid)
                    Case ""
                        If regionINI.SetIni(Name, "MaxAgents", CStr(100)) Then Return True
                    Case Else
                        If regionINI.SetIni(Name, "MaxAgents", Max_Agents(uuid)) Then Return True
                End Select

                ' Maps
                If MapType(uuid) = "None" Then
                    If regionINI.SetIni(Name, "GenerateMaptiles", "False") Then Return True
                ElseIf MapType(uuid) = "Simple" Then
                    If regionINI.SetIni(Name, "GenerateMaptiles", "True") Then Return True
                    If regionINI.SetIni(Name, "MapImageModule", "MapImageModule") Then Return True ' versus Warp3DImageModule
                    If regionINI.SetIni(Name, "TextureOnMapTile", "False") Then Return True        ' versus True
                    If regionINI.SetIni(Name, "DrawPrimOnMapTile", "False") Then Return True
                    If regionINI.SetIni(Name, "TexturePrims", "False") Then Return True
                    If regionINI.SetIni(Name, "RenderMeshes", "False") Then Return True
                ElseIf MapType(uuid) = "Good" Then
                    If regionINI.SetIni(Name, "GenerateMaptiles", "True") Then Return True
                    If regionINI.SetIni(Name, "MapImageModule", "Warp3DImageModule") Then Return True ' versus MapImageModule
                    If regionINI.SetIni(Name, "TextureOnMapTile", "False") Then Return True        ' versus True
                    If regionINI.SetIni(Name, "DrawPrimOnMapTile", "False") Then Return True
                    If regionINI.SetIni(Name, "TexturePrims", "False") Then Return True
                    If regionINI.SetIni(Name, "RenderMeshes", "False") Then Return True
                ElseIf MapType(uuid) = "Better" Then
                    If regionINI.SetIni(Name, "GenerateMaptiles", "True") Then Return True
                    If regionINI.SetIni(Name, "MapImageModule", "Warp3DImageModule") Then Return True ' versus MapImageModule
                    If regionINI.SetIni(Name, "TextureOnMapTile", "True") Then Return True        ' versus True
                    If regionINI.SetIni(Name, "DrawPrimOnMapTile", "True") Then Return True
                    If regionINI.SetIni(Name, "TexturePrims", "False") Then Return True
                    If regionINI.SetIni(Name, "RenderMeshes", "False") Then Return True
                ElseIf MapType(uuid) = "Best" Then
                    If regionINI.SetIni(Name, "GenerateMaptiles", "True") Then Return True
                    If regionINI.SetIni(Name, "MapImageModule", "Warp3DImageModule") Then Return True ' versus MapImageModule
                    If regionINI.SetIni(Name, "TextureOnMapTile", "True") Then Return True     ' versus True
                    If regionINI.SetIni(Name, "DrawPrimOnMapTile", "True") Then Return True
                    If regionINI.SetIni(Name, "TexturePrims", "True") Then Return True
                    If regionINI.SetIni(Name, "RenderMeshes", "True") Then Return True
                End If

                'Options and overrides

                If regionINI.SetIni(Name, "Concierge", Concierge(uuid)) Then Return True
                If regionINI.SetIni(Name, "DisableGloebits", DisableGloebits(uuid)) Then Return True
                If regionINI.SetIni(Name, "RegionSnapShot", RegionSnapShot(uuid)) Then Return True
                If regionINI.SetIni(Name, "Birds", Birds(uuid)) Then Return True
                If regionINI.SetIni(Name, "Tides", Tides(uuid)) Then Return True
                If regionINI.SetIni(Name, "Teleport", CStr(Teleport_Sign(uuid))) Then Return True
                If regionINI.SetIni(Name, "DisallowForeigners", Disallow_Foreigners(uuid)) Then Return True
                If regionINI.SetIni(Name, "DisallowResidents", Disallow_Residents(uuid)) Then Return True
                If regionINI.SetIni(Name, "SkipAutoBackup", SkipAutobackup(uuid)) Then Return True
                If regionINI.SetIni(Name, "Physics", RegionPhysics(uuid)) Then Return True
                If regionINI.SetIni(Name, "FrameTime", FrameTime(uuid)) Then Return True

                regionINI.SaveIni()

            Next

            Settings.SaveSettings()
        Catch ex As Exception
            ErrorLog(ex.Message)
            Return True
        End Try

        Return False

    End Function

    Private Sub SetupOpensimIM(INI As LoadIni)

        Dim URL = "http://" & Settings.PublicIP & ":" & Settings.ApachePort
        If Settings.CMS = JOpensim Then
            INI.SetIni("Messaging", "OfflineMessageModule", "OfflineMessageModule")
            INI.SetIni("Messaging", "OfflineMessageURL", URL & "/jOpensim/index.php?option=com_opensim&view=interface&messaging=")
            INI.SetIni("Messaging", "MuteListURL", URL & "/jOpensim/index.php?option=com_opensim&view=interface&messaging=")
        Else
            INI.SetIni("Messaging", "OfflineMessageModule", "Offline Message Module V2")
            INI.SetIni("Messaging", "OfflineMessageURL", "")
            INI.SetIni("Messaging", "MuteListURL", "http://" & Settings.PublicIP & ":" & Settings.HttpPort)
        End If
    End Sub

    Private Sub SetupOpensimSearchINI(INI As LoadIni)

        ' RegionSnapShot
        INI.SetIni("DataSnapshot", "index_sims", "True")
        If Settings.CMS = JOpensim Then
            Settings.CMS = "Local"
        End If

        If Settings.CMS = JOpensim AndAlso Settings.SearchOptions = JOpensim Then

            INI.SetIni("DataSnapshot", "data_services", "")
            Dim SearchURL = "http://" & Settings.PublicIP & ":" & Settings.ApachePort &
                "/jOpensim/index.php?option=com_opensim&view=interface"
            INI.SetIni("Search", "SearchURL", SearchURL)
            INI.SetIni("LoginService", "SearchURL", SearchURL)
            CopyFileFast(IO.Path.Combine(Settings.OpensimBinPath, "jOpensimSearch.Modules.dll.bak"),
                         IO.Path.Combine(Settings.OpensimBinPath, "jOpensimSearch.Modules.dll"))

        ElseIf Settings.SearchOptions = Outworldz Then

            INI.SetIni("DataSnapshot", "data_services", PropDomain & "/Search/register.php")
            Dim SearchURL = PropDomain & "/Search/query.php"
            INI.SetIni("Search", "SearchURL", SearchURL)
            INI.SetIni("LoginService", "SearchURL", SearchURL)
            DeleteFile(IO.Path.Combine(Settings.OpensimBinPath, "jOpensimSearch.Modules.dll"))
            DeleteFile(IO.Path.Combine(Settings.OpensimBinPath, "jOpensimProfile.Modules.dll"))

        ElseIf Settings.SearchOptions = "Local" Then

            INI.SetIni("DataSnapshot", "data_services", $"http://{Settings.LANIP}:{Settings.ApachePort}/Search/register.php")
            Dim SearchURL = $"http://{Settings.PublicIP}:{Settings.ApachePort}/Search/query.php"
            INI.SetIni("Search", "SearchURL", SearchURL)
            INI.SetIni("LoginService", "SearchURL", SearchURL)
            DeleteFile(IO.Path.Combine(Settings.OpensimBinPath, "jOpensimSearch.Modules.dll"))
            DeleteFile(IO.Path.Combine(Settings.OpensimBinPath, "jOpensimProfile.Modules.dll"))
        Else
            INI.SetIni("DataSnapshot", "data_services", "")
            DeleteFile(IO.Path.Combine(Settings.OpensimBinPath, "jOpensimProfile.Modules.dll"))
            INI.SetIni("Search", "SearchURL", "")
            INI.SetIni("LoginService", "SearchURL", "")
            DeleteFile(IO.Path.Combine(Settings.OpensimBinPath, "jOpensimSearch.Modules.dll"))
        End If

    End Sub

#End Region

End Module
