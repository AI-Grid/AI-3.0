#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC. AGPL3.0 https://opensource.org/licenses/AGPL

#End Region

Imports System.IO

Imports System.Text.RegularExpressions
Imports MySql.Data.MySqlClient
Imports Newtonsoft.Json

Public Class RegionMaker

#Region "Public Fields"

    Private Class JSONresult
        Public alert As String
        Public login As String
        Public region_id As String
        Public region_name As String
    End Class

#End Region

#Region "Declarations"

    Public WebserverList As New List(Of String)
    Private Shared FInstance As RegionMaker
    Private ReadOnly _Grouplist As New Dictionary(Of String, Integer)
    ReadOnly Backup As New List(Of RegionMaker.Region_data)
    Private ReadOnly Map As New Dictionary(Of String, String)
    Private ReadOnly RegionList As New Dictionary(Of String, Region_data)
    Private _GetAllRegionsIsBusy As Boolean
    Private _RegionListIsInititalized As Boolean
    Dim json As New JSONresult

    Public Enum SIMSTATUSENUM As Integer

        Stopped = 0
        Booting = 1
        Booted = 2
        RecyclingUp = 3
        RecyclingDown = 4
        ShuttingDown = 5
        RestartPending = 6
        RetartingNow = 7
        [Resume] = 8
        Suspended = 9
        [Error] = 10
        RestartStage2 = 11
        ShuttingDownForGood = 12 ' Not possible to change state in exit logic, only entry logic
        NoLogin = 13
        NoError = 14

    End Enum

    Private Class Region_Mapping

        Public Name As String
        Public X As Integer
        Public Y As Integer

    End Class

#End Region

#Region "Instance"

    Public Shared ReadOnly Property Instance() As RegionMaker
        Get
            If (FInstance Is Nothing) Then
                FInstance = New RegionMaker()
                SkipSetup = False
            End If
            Return FInstance
        End Get
    End Property

#End Region

#Region "Public Properties"

    Public Property GetAllRegionsIsBusy As Boolean
        Get
            Return _GetAllRegionsIsBusy
        End Get
        Set(value As Boolean)
            _GetAllRegionsIsBusy = value
        End Set
    End Property

#End Region

#Region "Start/Stop"

    Public Sub ClearStack()

        WebserverList.Clear()

    End Sub

    Public Function Init() As Boolean

        GetAllRegions()
        If RegionCount() = 0 Then
            CreateRegion("Welcome")
            Settings.WelcomeRegion = "Welcome"
            WriteRegionObject("Welcome", "Welcome")
            Settings.SaveSettings()
            If GetAllRegions() = -1 Then Return False
        End If
        TextPrint($"Loaded {CStr(RegionCount)} Regions")

        Return CheckOverLap()

    End Function

#End Region

#Region "CheckPost"

    Public Sub CheckPost()

        If WebserverList.Count = 0 Then Return

        While WebserverList.Count > 0

            Try
                Dim ProcessString As String = WebserverList(0) ' recover the PID as string
                WebserverList.RemoveAt(0)

                ' This search returns the substring between two strings, so the first index Is moved to the character just after the first string.
                Dim POST As String = Uri.UnescapeDataString(ProcessString)
                Dim first As Integer = POST.IndexOf("{", StringComparison.InvariantCulture)
                Dim last As Integer = POST.LastIndexOf("}", StringComparison.InvariantCulture)

                Dim rawJSON As String = ""
                If first > -1 And last > -1 Then
                    rawJSON = POST.Substring(first, last - first + 1)
                Else
                    Logger("RegionReady", "Malformed Web request: " & POST, "Teleport")
                    Continue While
                End If

                Try
                    json = JsonConvert.DeserializeObject(Of JSONresult)(rawJSON)
                Catch ex As Exception
                    BreakPoint.Show(ex.Message)
                    Debug.Print(ex.Message)
                    Logger("RegionReady", "Malformed JSON: " & ProcessString, "Teleport")
                    Continue While
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
                        Continue While
                    End If

                    Dim GroupList = RegionUuidListByName(GroupName(uuid))
                    For Each RUUID As String In GroupList
                        If Not FormSetup.BootedList1.Contains(RUUID) Then
                            FormSetup.BootedList1.Add(RUUID)
                        End If
                    Next

                ElseIf json.login = "shutdown" Then
                    Continue While   ' this bit below interferes with restarting multiple regions in a DOS box
                ElseIf json.login = "disabled" Then
                    Continue While
                Else
                    Continue While
                End If
            Catch ex As Exception
                BreakPoint.Show(ex.Message)
                Logger("RegionReady", "Exception:" & ex.Message, "Teleport")
                Try
                    WebserverList.Clear()
                Catch
                End Try
            End Try
        End While

        PropUpdateView() = True

    End Sub

#End Region

#Region "Create Region"

    Public Function CreateRegion(name As String, Optional UUID As String = "") As String

        If String.IsNullOrEmpty(UUID) Then UUID = Guid.NewGuid().ToString

        Debug.Print("Create Region " + name)
        Dim r As New Region_data With {
            ._AllowGods = "",
            ._AvatarCount = 0,
            ._Birds = "",
            ._ClampPrimSize = False,
            ._Concierge = "",
            ._CoordX = LargestX() + 8,
            ._CoordY = LargestY() + 0,
            ._CrashCounter = 0,
            ._DisableGloebits = "",
            ._FrameTime = "",
            ._GodDefault = "",
            ._GroupPort = 0,
            ._ManagerGod = "",
            ._MapType = "",
            ._MaxAgents = "100",
            ._MaxPrims = "15000",
            ._MinTimerInterval = "",
            ._AvatarsInRegion = 0,
            ._NonPhysicalPrimMax = "1024",
            ._PhysicalPrimMax = "64",
            ._ProcessID = 0,
            ._RegionEnabled = True,
            ._RegionGod = "",
            ._RegionName = name,
            ._RegionPort = 0,
            ._RegionSmartStart = "",
            ._RegionLandingSpot = "",
            ._RegionSnapShot = "",
            ._ScriptEngine = "",
            ._SizeX = 256,
            ._SizeY = 256,
            ._SkipAutobackup = "",
            ._Status = SIMSTATUSENUM.Stopped,
            ._BootTime = 0,
            ._Teleport = "",
            ._Tides = "",
            ._Timer = Date.Now,
            ._UUID = UUID
        }

        RegionList.Add(r._UUID, r)
        'RegionDump()
        Debug.Print("Region count is " & CStr(RegionList.Count - 1))
        Return r._UUID

    End Function

    Public Sub DeleteRegion(UUID As String)

        If RegionList.ContainsKey(UUID) Then
            RegionList.Remove(UUID)
        End If

    End Sub

    ''' <summary>
    ''' Saves Region class to disk file
    ''' </summary>
    ''' <param name="Group">Dos Box name</param>
    ''' <param name="Newname">New Name</param>
    ''' <param name="OldName">Old Name to change</param>
    ''' <param name="Estate">Optional Estate Name</param>
    Public Sub WriteRegionObject(Group As String, Newname As String)

        Dim pathtoWelcome As String = IO.Path.Combine(Settings.OpensimBinPath, $"Regions\{Group}\Region\")
        Dim RegionUUID As String = FindRegionByName(Newname)
        DeleteFile(IO.Path.Combine(pathtoWelcome, $"Region\{Newname}.ini"))

        Dim fname = pathtoWelcome + Newname + ".ini"
        If Not Directory.Exists(pathtoWelcome) Then
            Try
                Directory.CreateDirectory(pathtoWelcome)
            Catch ex As Exception
                BreakPoint.Show(ex.Message)
            End Try
        End If

        ' Change estate for Smart Start
        Dim Estate = "Estate"
        If SmartStart(RegionUUID) = "True" Then
            Estate = "SmartStart"
        End If

        Dim proto = "; * Regions configuration file; " & vbCrLf _
        & "; Automatically changed and read by Dreamworld. Edits are allowed" & vbCrLf _
        & "; Rule1: The File name must match the RegionName" & vbCrLf _
        & "; Rule2: Only one region per INI file." & vbCrLf _
        & ";" & vbCrLf _
        & "[" & Newname & "]" & vbCrLf _
        & "RegionUUID = " & RegionUUID & vbCrLf _
        & "Location = " & CoordX(RegionUUID).ToString(Globalization.CultureInfo.InvariantCulture) & "," & CoordY(RegionUUID).ToString(Globalization.CultureInfo.InvariantCulture) & vbCrLf _
        & "InternalAddress = 0.0.0.0" & vbCrLf _
        & "InternalPort = " & RegionPort(RegionUUID) & vbCrLf _
        & "GroupPort = " & GroupPort(RegionUUID) & vbCrLf _
        & "AllowAlternatePorts = False" & vbCrLf _
        & "ExternalHostName = " & Settings.ExternalHostName() & vbCrLf _
        & "SizeX = " & CStr(SizeX(RegionUUID)) & vbCrLf _
        & "SizeY = " & CStr(SizeY(RegionUUID)) & vbCrLf _
        & "Enabled = " & CStr(RegionEnabled(RegionUUID)) & vbCrLf _
        & "NonPhysicalPrimMax = " & CStr(NonPhysicalPrimMax(RegionUUID)) & vbCrLf _
        & "PhysicalPrimMax = " & CStr(PhysicalPrimMax(RegionUUID)) & vbCrLf _
        & "ClampPrimSize = " & CStr(ClampPrimSize(RegionUUID)) & vbCrLf _
        & "MaxPrims = " & MaxPrims(RegionUUID) & vbCrLf _
        & "RegionType = " & Estate & vbCrLf _
        & "MaxAgents = 100" & vbCrLf & vbCrLf _
        & ";# Dreamgrid extended properties" & vbCrLf _
        & "RegionSnapShot = " & RegionSnapShot(RegionUUID) & vbCrLf _
        & "MapType = " & MapType(RegionUUID) & vbCrLf _
        & "Physics = " & Physics(RegionUUID) & vbCrLf _
        & "GodDefault = " & GodDefault(RegionUUID) & vbCrLf _
        & "AllowGods = " & AllowGods(RegionUUID) & vbCrLf _
        & "RegionGod = " & RegionGod(RegionUUID) & vbCrLf _
        & "ManagerGod = " & ManagerGod(RegionUUID) & vbCrLf _
        & "Birds = " & Birds(RegionUUID) & vbCrLf _
        & "Tides = " & Tides(RegionUUID) & vbCrLf _
        & "Teleport = " & Teleport(RegionUUID) & vbCrLf _
        & "DisableGloebits = " & DisableGloebits(RegionUUID) & vbCrLf _
        & "DisallowForeigners = " & DisallowForeigners(RegionUUID) & vbCrLf _
        & "DisallowResidents = " & DisallowResidents(RegionUUID) & vbCrLf _
        & "MinTimerInterval =" & MinTimerInterval(RegionUUID) & vbCrLf _
        & "Frametime =" & FrameTime(RegionUUID) & vbCrLf _
        & "ScriptEngine =" & ScriptEngine(RegionUUID) & vbCrLf _
        & "Publicity =" & GDPR(RegionUUID) & vbCrLf _
        & "Concierge =" & Concierge(RegionUUID) & vbCrLf _
        & "SmartStart =" & SmartStart(RegionUUID) & vbCrLf _
        & "LandingSpot =" & LandingSpot(RegionUUID) & vbCrLf _
        & "OpenSimWorldAPIKey = " & OpensimWorldAPIKey(RegionUUID)

        DeleteFile(fname)

        Try
            Using outputFile As New StreamWriter(fname, True)
                outputFile.WriteLine(proto)
            End Using
        Catch ex As Exception
            BreakPoint.Show(ex.Message)
        End Try

        Add_To_Region_Map(RegionUUID)

    End Sub

#End Region

#Region "Functions"

    Public Sub Add_To_Region_Map(RegionUUID As String)

        ' add to the global map this entire DOS box
        Dim Xloc = CoordX(RegionUUID)
        Dim Yloc = CoordY(RegionUUID)
        Dim Name = RegionName(RegionUUID)

        ' draw a box at this size plus the pull down size.
        For Each UUID In RegionUuidListByName(Name)
            Dim SimSize As Integer = CInt(SizeX(RegionUUID) / 256)
            For Xstep = 0 To SimSize - 1
                For Ystep = 0 To SimSize - 1
                    Dim gr As String = $"{Xloc + Xstep},{Yloc + Ystep}"
                    If Not Map.ContainsKey(gr) Then Map.Add(gr, UUID)
                Next
            Next
        Next

    End Sub

    Public Function AvatarIsNearby(RegionUUID As String) As Boolean

        Dim Xloc = CoordX(RegionUUID)
        Dim Yloc = CoordY(RegionUUID)

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
                    If IsAgentInRegion(Map.Item(gr)) Then
                        Return True
                    End If
                End If
            Next
        Next

        Return False

    End Function

    Public Function AvatarsIsInGroup(groupname As String) As Boolean

        Dim present As Integer = 0
        For Each RegionUUID As String In RegionUuidListByName(groupname)
            If IsAgentInRegion(RegionUUID) Then
                present += 1
            End If
        Next
        Return CType(present, Boolean)

    End Function

    Public Function CheckOverLap() As Boolean

        Dim FailedCheck As Boolean
        Dim Regions As New List(Of Region_Mapping)

        For Each RegionUUID In RegionUuids()

            Dim Name = RegionName(RegionUUID)
            Dim Size As Integer = CInt(SizeX(RegionUUID) / 256)

            Dim X As Integer
            Dim Y As Integer
            ' make a box
            For X = 0 To Size - 1
                For Y = 0 To Size - 1
                    Dim map = New Region_Mapping With {
                        .Name = Name,
                        .X = CoordX(RegionUUID) + X,
                        .Y = CoordY(RegionUUID) + Y
                    }
                    Regions.Add(map)
                    ' If (Name.Contains("MartinBassManSlad")) Or (Name.Contains("Maya")) Then Diagnostics.Debug.Print($"{Name} {map.X} {map.Y}") End If
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

    Public Sub Delete_Region_Map(RegionUUID As String)

        ' add to the global map this entire DOS box
        Dim Xloc = CoordX(RegionUUID)
        Dim Yloc = CoordY(RegionUUID)

        ' draw a box at this size plus the pull down size.
        For Each UUID In RegionUuidListByName(GroupName(RegionUUID))
            Dim SimSize As Integer = CInt(SizeX(RegionUUID) / 256)
            For Xstep = 0 To SimSize - 1
                For Ystep = 0 To SimSize - 1
                    Dim gr As String = $"{Xloc + Xstep},{Yloc + Ystep}"
                    If Map.ContainsKey(gr) Then Map.Remove(gr)
                Next
            Next
        Next

    End Sub

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

    Public Function GetAllRegions() As Integer

        If PropOpensimIsRunning Then Return 0

        Dim ctr = 600
        While GetAllRegionsIsBusy And ctr > 0
            Sleep(100)
            ctr -= 1
        End While

        GetAllRegionsIsBusy = True
        Try
            Backup.Clear()
            Dim pair As KeyValuePair(Of String, Region_data)
            Dim UsedPorts As New List(Of Integer)

            For Each pair In RegionList
                Backup.Add(pair.Value)
            Next

            RegionList.Clear()

            Dim uuid As String = ""
            Dim folders() = Directory.GetDirectories(Settings.OpensimBinPath + "Regions")
            System.Array.Sort(folders)

            For Each FolderName As String In folders

                Dim regionfolders = Directory.GetDirectories(FolderName)
                For Each FileName As String In regionfolders

                    If FileName.EndsWith("DataSnapshot", StringComparison.InvariantCulture) Then Continue For

                    Dim fName = ""
                    Try
                        Dim inis() As String = Nothing
                        Try
                            inis = Directory.GetFiles(FileName, "*.ini", SearchOption.TopDirectoryOnly)
                        Catch ex As Exception
                            BreakPoint.Show(ex.Message)
                        End Try

                        For Each file As String In inis
                            fName = System.IO.Path.GetFileNameWithoutExtension(file)

                            Dim INI = New LoadIni(file, ";", System.Text.Encoding.ASCII)

                            uuid = CStr(INI.GetIni(fName, "RegionUUID", "", "String"))

                            Dim SomeUUID As New Guid
                            If Not Guid.TryParse(uuid, SomeUUID) Then
                                MsgBox("Cannot read uuid In INI file For " & fName, vbCritical Or MsgBoxStyle.MsgBoxSetForeground)
                                '  TODO Auto repair this error from a backup
                                Return -1
                            End If

                            CreateRegion(fName, uuid)

                            RegionEnabled(uuid) = CBool(INI.GetIni(fName, "Enabled", "True", "Boolean"))

                            RegionIniFilePath(uuid) = file ' save the path
                            RegionIniFolderPath(uuid) = System.IO.Path.GetDirectoryName(file)

                            Dim theEnd As Integer = RegionIniFolderPath(uuid).LastIndexOf("\", StringComparison.InvariantCulture)
                            OpensimIniPath(uuid) = RegionIniFolderPath(uuid).Substring(0, theEnd + 1)

                            Dim M As Match = Regex.Match(FolderName, ".*\\(.*)$")
                            If M.Groups(1).Success Then
                                Debug.Print(M.Groups(1).Value)
                                GroupName(uuid) = M.Groups(1).Value
                            Else
                                MsgBox("Cannot locate Dos Box name for  " & fName, vbInformation Or MsgBoxStyle.MsgBoxSetForeground)
                                Return 0
                            End If

                            SizeX(uuid) = CInt(INI.GetIni(fName, "SizeX", "256", "Integer"))
                            SizeY(uuid) = CInt(INI.GetIni(fName, "SizeY", "256", "Integer"))

                            ' extended props V2.1
                            NonPhysicalPrimMax(uuid) = CStr(INI.GetIni(fName, "NonPhysicalPrimMax", "1024", "Integer"))
                            PhysicalPrimMax(uuid) = CStr(INI.GetIni(fName, "PhysicalPrimMax", "64", "Integer"))
                            ClampPrimSize(uuid) = CBool(INI.GetIni(fName, "ClampPrimSize", "False", "Boolean"))
                            MaxPrims(uuid) = CStr(INI.GetIni(fName, "MaxPrims", "15000", "Integer"))
                            MaxAgents(uuid) = CStr(INI.GetIni(fName, "MaxAgents", "100", "Integer"))

                            ' Location is int,int format.
                            Dim C As String = CStr(INI.GetIni(fName, "Location", RandomNumber.Between(1020, 980) & "," & RandomNumber.Between(1020, 980)))
                            Dim parts As String() = C.Split(New Char() {","c}) ' split at the comma
                            CoordX(uuid) = CInt("0" & CStr(parts(0).Trim))
                            CoordY(uuid) = CInt("0" & CStr(parts(1).Trim))

                            ' options parameters coming from INI file can be blank!
                            MinTimerInterval(uuid) = CStr(INI.GetIni(fName, "MinTimerInterval", "", "String"))
                            FrameTime(uuid) = CStr(INI.GetIni(fName, "FrameTime", "", "String"))
                            RegionSnapShot(uuid) = CStr(INI.GetIni(fName, "RegionSnapShot", "", "String"))
                            MapType(uuid) = CStr(INI.GetIni(fName, "MapType", "", "String"))
                            Physics(uuid) = CStr(INI.GetIni(fName, "Physics", "", "String"))
                            MaxPrims(uuid) = CStr(INI.GetIni(fName, "MaxPrims", "", "String"))
                            GodDefault(uuid) = CStr(INI.GetIni(fName, "GodDefault", "True", "String"))
                            AllowGods(uuid) = CStr(INI.GetIni(fName, "AllowGods", "", "String"))
                            RegionGod(uuid) = CStr(INI.GetIni(fName, "RegionGod", "", "String"))
                            ManagerGod(uuid) = CStr(INI.GetIni(fName, "ManagerGod", "", "String"))
                            Birds(uuid) = CStr(INI.GetIni(fName, "Birds", "", "String"))
                            Tides(uuid) = CStr(INI.GetIni(fName, "Tides", "", "String"))
                            Teleport(uuid) = CStr(INI.GetIni(fName, "Teleport", "", "String"))
                            DisableGloebits(uuid) = CStr(INI.GetIni(fName, "DisableGloebits", "", "String"))
                            DisallowForeigners(uuid) = CStr(INI.GetIni(fName, "DisallowForeigners", "", "String"))
                            DisallowResidents(uuid) = CStr(INI.GetIni(fName, "DisallowResidents", "", "String"))
                            SkipAutobackup(uuid) = CStr(INI.GetIni(fName, "SkipAutoBackup", "", "String"))
                            Snapshot(uuid) = CStr(INI.GetIni(fName, "RegionSnapShot", "", "String"))
                            ScriptEngine(uuid) = CStr(INI.GetIni(fName, "ScriptEngine", "", "String"))
                            GDPR(uuid) = CStr(INI.GetIni(fName, "Publicity", "", "String"))
                            Concierge(uuid) = CStr(INI.GetIni(fName, "Concierge", "", "String"))
                            SmartStart(uuid) = CStr(INI.GetIni(fName, "SmartStart", "False", "String"))
                            LandingSpot(uuid) = CStr(INI.GetIni(fName, "LandingSpot", "", "String"))
                            OpensimWorldAPIKey(uuid) = CStr(INI.GetIni(fName, "OpensimWorldAPIKey", "", "String"))

                            Dim p = LargestPort() + 1
                            RegionPort(uuid) = p
                            GroupPort(uuid) = p

                            'Diagnostics.Debug.Print("Assign Port:" & CStr(GroupPort(uuid)))

                            If _RegionListIsInititalized Then
                                ' restore backups of transient data
                                Dim o = FindBackupByName(fName)
                                If o >= 0 Then
                                    AvatarCount(uuid) = Backup(o)._AvatarCount
                                    ProcessID(uuid) = Backup(o)._ProcessID
                                    Status(uuid) = Backup(o)._Status
                                    Timer(uuid) = Backup(o)._Timer
                                    CrashCounter(uuid) = Backup(o)._CrashCounter

                                    ' If region Is Running, use its port as they cannot chenge while up.

                                    If Backup(o)._RegionPort > 0 Then
                                        RegionPort(uuid) = Backup(o)._RegionPort
                                    Else
                                        BreakPoint.Show("oops")
                                    End If
                                    If Backup(o)._GroupPort > 0 Then
                                        GroupPort(uuid) = Backup(o)._GroupPort
                                    Else
                                        BreakPoint.Show("No Group Port yet")
                                    End If

                                End If

                            End If
                            INI.SaveINI()
                            Add_To_Region_Map(uuid)
                            Application.DoEvents()
                        Next
                    Catch ex As Exception
                        BreakPoint.Show(ex.Message)
                        MsgBox(My.Resources.Error_Region + fName + " : " + ex.Message, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Error_word)
                        ErrorLog("Err:Parse file " + fName + ":" + ex.Message)
                        GetAllRegionsIsBusy = False
                        PropUpdateView = True ' make form refresh
                        Return -1
                    End Try

                Next
            Next

            _RegionListIsInititalized = True
        Catch ex As Exception
            BreakPoint.Show(ex.Message)
            GetAllRegionsIsBusy = False
            PropUpdateView = True ' make form refresh
            Return -1
        End Try
        GetAllRegionsIsBusy = False
        PropUpdateView = True ' make form refresh

        Return RegionList.Count

    End Function

    Public Function LargestPort() As Integer

        ' locate largest port
        Dim MaxNum As Integer = Settings.FirstRegionPort - 1
        Dim pair As KeyValuePair(Of String, Region_data)

        For Each pair In RegionList
            If pair.Value._RegionPort > MaxNum Then
                MaxNum = pair.Value._RegionPort
            End If
        Next

        Return MaxNum

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
            Max = Between(1010, 990)
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
            Max = RandomNumber.Between(1010, 990)
        End If
        Return Max

    End Function

    ''' <summary>Self setting Region Ports Iterate over all regions and set the ports from the starting value</summary>
    Public Sub UpdateAllRegionPorts()

        TextPrint(My.Resources.Updating_Ports_word)
        Dim used As New List(Of Integer)
        ' Do not want to reuse any ports.
        ' Get all the running regions so we can collect their ports.
        Dim runningRegions As New List(Of String)
        For Each RegionUUID As String In RegionUuids()
            Dim Name = RegionName(RegionUUID)

            If CBool(GetHwnd(GroupName(RegionUUID))) Then
                'TextPrint($"-->{Name} {My.Resources.Running_word}")
                runningRegions.Add(RegionUUID)

                Dim INI = New LoadIni(RegionIniFilePath(RegionUUID), ";", System.Text.Encoding.UTF8)
                Dim Rp = INI.GetIni(Name, "InternalPort", "0")
                Dim Gp = INI.GetIni(Name, "GroupPort", "0")

                RegionPort(RegionUUID) = CInt("0" & Rp)
                GroupPort(RegionUUID) = CInt("0" & Gp)

                If Not used.Contains(CInt("0" & Rp)) Then
                    used.Add(CInt("0" & Rp))
                End If
            Else
                'TextPrint($"-->{My.Resources.Region_word} {Name} {My.Resources.Stopped_word}")
            End If
        Next

        Dim port = Settings.FirstRegionPort
        For Each RegionUUID As String In RegionUuids()
            If runningRegions.Contains(RegionUUID) Then Continue For

            If used.Contains(port) Then
                port = LargestPort() + 1
            End If
            RegionPort(RegionUUID) = port
            GroupPort(RegionUUID) = port

            'Diagnostics.Debug.Print("Assign Port:" & CStr(GroupPort(RegionUUID)))
            port += 1
        Next

        TextPrint(My.Resources.Setup_Firewall_word)
        Firewall.SetFirewall()   ' must be after UpdateAllRegionPorts

    End Sub

#Region "Region_data"

    ' hold a copy of the Main region data on a per-form basis
    Private Class Region_data

        Public _AvatarCount As Integer
        Public _BootTime As Integer
        Public _ClampPrimSize As Boolean
        Public _CoordX As Integer = 1000
        Public _CoordY As Integer = 1000
        Public _DisallowForeigners As String = ""
        Public _DisallowResidents As String = ""
        Public _Estate As String = ""
        Public _FolderPath As String = ""
        Public _Group As String = ""
        Public _IniPath As String = "" ' the path to the folder that holds the region ini
        Public _MapTime As Integer
        Public _ProcessID As Integer ' the folder name that holds the region(s), can be different named
        Public _RegionEnabled As Boolean = True
        Public _RegionName As String = ""
        Public _RegionPath As String = ""  ' The full path to the region ini file
        Public _RegionPort As Integer
        Public _SimName As String = ""
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

    Public Property AvatarCount(uuid As String) As Integer
        Get
            If uuid Is Nothing Then Return 0
            If Bad(uuid) Then Return 0
            Return RegionList(uuid)._AvatarCount
        End Get
        Set(ByVal Value As Integer)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._AvatarCount = Value
        End Set
    End Property

    Public Property BootTime(uuid As String) As Integer
        Get
            If uuid Is Nothing Then Return 0
            If Bad(uuid) Then Return 0
            Dim t As Integer = CInt(Settings.GetBootTime(uuid))
            If t > 0 Then Return t
            Return RegionList(uuid)._BootTime
        End Get
        Set(ByVal Value As Integer)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._BootTime = Value
            Settings.SaveBootTime(Value, uuid)
        End Set
    End Property

    Public Property ClampPrimSize(uuid As String) As Boolean
        Get
            If uuid Is Nothing Then Return False
            If Bad(uuid) Then Return False
            Return RegionList(uuid)._ClampPrimSize
        End Get
        Set(ByVal Value As Boolean)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._ClampPrimSize = Value
        End Set
    End Property

    Public Property CoordX(uuid As String) As Integer
        Get
            If uuid Is Nothing Then Return 0
            If Bad(uuid) Then Return 0
            Return RegionList(uuid)._CoordX
        End Get
        Set(ByVal Value As Integer)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._CoordX = Value
        End Set
    End Property

    Public Property CoordY(uuid As String) As Integer
        Get
            If uuid Is Nothing Then Return 0
            If Bad(uuid) Then Return 0
            Return RegionList(uuid)._CoordY
        End Get
        Set(ByVal Value As Integer)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._CoordY = Value
        End Set
    End Property

    Public Property CrashCounter(uuid As String) As Integer
        Get
            If uuid Is Nothing Then Return -1
            If Bad(uuid) Then Return -1
            Return RegionList(uuid)._CrashCounter
        End Get
        Set(ByVal Value As Integer)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._CrashCounter = Value
        End Set
    End Property

    Public Property Estate(uuid As String) As String
        Get
            If uuid Is Nothing Then Return ""
            If Bad(uuid) Then Return ""
            Return RegionList(uuid)._Estate
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._Estate = Value
        End Set
    End Property

    Public Property MapTime(uuid As String) As Integer
        Get
            If uuid Is Nothing Then Return 0
            If Bad(uuid) Then Return 0
            Dim t As Integer = CInt(Settings.GetMapTime(uuid))
            If t > 0 Then Return t
            Return RegionList(uuid)._MapTime
        End Get
        Set(ByVal Value As Integer)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._MapTime = Value
            Settings.SaveMapTime(Value, uuid)
        End Set
    End Property

    Public Property MaxAgents(uuid As String) As String
        Get
            If uuid Is Nothing Then Return "100"
            If Bad(uuid) Then Return "100"
            If String.IsNullOrEmpty(RegionList(uuid)._MaxAgents) Then RegionList(uuid)._MaxAgents = "100"

            Return RegionList(uuid)._MaxAgents
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._MaxAgents = Value
        End Set
    End Property

    Public Property MaxPrims(uuid As String) As String
        Get
            If uuid Is Nothing Then Return "45000"
            If Bad(uuid) Then Return "45000"
            If String.IsNullOrEmpty(RegionList(uuid)._MaxPrims) Then
                RegionList(uuid)._MaxPrims = "45000"
            End If
            Return RegionList(uuid)._MaxPrims
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._MaxPrims = Value
        End Set
    End Property

    Public Property NonPhysicalPrimMax(uuid As String) As String
        Get
            If uuid Is Nothing Then Return "1024"
            If Bad(uuid) Then Return "1024"
            Return RegionList(uuid)._NonPhysicalPrimMax
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._NonPhysicalPrimMax = Value
        End Set
    End Property

    Public Property PhysicalPrimMax(uuid As String) As String
        Get
            If uuid Is Nothing Then Return "1024"
            If Bad(uuid) Then Return "1024"
            Return RegionList(uuid)._PhysicalPrimMax
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._PhysicalPrimMax = Value
        End Set
    End Property

    Public Property ProcessID(uuid As String) As Integer
        Get
            If uuid Is Nothing Then Return 0
            If Bad(uuid) Then Return 0
            Return RegionList(uuid)._ProcessID
        End Get
        Set(ByVal Value As Integer)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._ProcessID = Value
        End Set
    End Property

    Public Property SizeX(uuid As String) As Integer
        Get
            If uuid Is Nothing Then Return 256
            If Bad(uuid) Then Return 256
            Return RegionList(uuid)._SizeX
        End Get
        Set(ByVal Value As Integer)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._SizeX = Value
        End Set
    End Property

    Public Property SizeY(uuid As String) As Integer
        Get
            If uuid Is Nothing Then Return 256
            If Bad(uuid) Then Return 256
            Return RegionList(uuid)._SizeY
        End Get
        Set(ByVal Value As Integer)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._SizeY = Value
        End Set
    End Property

    Public Property Status(uuid As String) As Integer

        Get
            If uuid Is Nothing Then
                Return -1
            End If
            If Bad(uuid) Then
                Return -1
            End If
            Return RegionList(uuid)._Status
        End Get
        Set(ByVal Value As Integer)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._Status = Value

        End Set
    End Property

    Public Property Timer(uuid As String) As Date
        Get
            Return RegionList(uuid)._Timer
        End Get
        Set(ByVal Value As Date)
            RegionList(uuid)._Timer = Value
        End Set
    End Property

#End Region

#Region "Properties"

    Public ReadOnly Property RegionCount() As Integer
        Get
            Return RegionList.Count
        End Get
    End Property

    Public Property GroupName(uuid As String) As String
        Get
            If uuid Is Nothing Then Return ""
            If Bad(uuid) Then Return ""
            Return RegionList(uuid)._Group
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._Group = Value
        End Set
    End Property

    Public Property GroupPort(uuid As String) As Integer
        Get
            Dim GN As String = GroupName(uuid)
            If _Grouplist.ContainsKey(GN) Then
                Return _Grouplist.Item(GN)
            End If
            Return LargestPort()
        End Get

        Set(ByVal Value As Integer)

            Dim GN As String = GroupName(uuid)
            If _Grouplist.ContainsKey(GN) Then
                _Grouplist.Item(GN) = Value
            Else
                _Grouplist.Add(GN, Value)
            End If

        End Set
    End Property

    Public Property OpensimIniPath(uuid As String) As String
        Get
            If uuid Is Nothing Then Return ""
            If Bad(uuid) Then Return ""
            Return RegionList(uuid)._IniPath
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._IniPath = Value
        End Set
    End Property

    Public Property RegionIniFilePath(uuid As String) As String
        Get
            If uuid Is Nothing Then Return ""
            If Bad(uuid) Then Return ""
            Return RegionList(uuid)._RegionPath
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._RegionPath = Value
        End Set
    End Property

    Public Property RegionIniFolderPath(uuid As String) As String
        Get
            If uuid Is Nothing Then Return ""
            If Bad(uuid) Then Return ""
            Return RegionList(uuid)._FolderPath
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._FolderPath = Value
        End Set
    End Property

    Public Property RegionName(uuid As String) As String
        Get
            If uuid Is Nothing Then Return ""
            If Bad(uuid) Then Return ""
            Return RegionList(uuid)._RegionName
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._RegionName = Value
        End Set
    End Property

    Public Property RegionPort(uuid As String) As Integer
        Get
            If uuid Is Nothing Then Return -1
            If Bad(uuid) Then Return -1
            Return RegionList(uuid)._RegionPort
        End Get
        Set(ByVal Value As Integer)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._RegionPort = Value
        End Set
    End Property

#End Region

#Region "Options"

    Public Property AllowGods(uuid As String) As String
        Get
            If uuid Is Nothing Then Return ""
            If Bad(uuid) Then Return ""
            Return RegionList(uuid)._AllowGods
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._AllowGods = Value
        End Set
    End Property

    Public Property Birds(uuid As String) As String
        Get
            If uuid Is Nothing Then Return ""
            If Bad(uuid) Then Return ""
            Return RegionList(uuid)._Birds
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._Birds = Value
        End Set
    End Property

    Public Property Concierge(uuid As String) As String
        Get
            If uuid Is Nothing Then Return ""
            If Bad(uuid) Then Return ""
            Return RegionList(uuid)._Concierge
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._Concierge = Value
        End Set
    End Property

    Public Property DisableGloebits(uuid As String) As String
        Get
            If uuid Is Nothing Then Return ""
            If Bad(uuid) Then Return ""
            Return RegionList(uuid)._DisableGloebits
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._DisableGloebits = Value
        End Set
    End Property

    Public Property DisallowForeigners(uuid As String) As String
        Get
            If uuid Is Nothing Then Return ""
            If Bad(uuid) Then Return ""
            Return RegionList(uuid)._DisallowForeigners
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._DisallowForeigners = Value
        End Set
    End Property

    Public Property DisallowResidents(uuid As String) As String
        Get
            If uuid Is Nothing Then Return ""
            If Bad(uuid) Then Return ""
            Return RegionList(uuid)._DisallowResidents
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._DisallowResidents = Value
        End Set
    End Property

    Public Property FrameTime(uuid As String) As String
        Get
            If uuid Is Nothing Then Return ""
            If Bad(uuid) Then Return ""
            Return RegionList(uuid)._FrameTime
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            If Value Is Nothing Then Return
            Value = Value.Replace(",", ".")
            RegionList(uuid)._FrameTime = Value
        End Set
    End Property

    Public Property GDPR(uuid As String) As String
        Get
            If uuid Is Nothing Then Return ""
            If Bad(uuid) Then Return ""
            Return RegionList(uuid)._GDPR
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._GDPR = Value
        End Set
    End Property

    Public Property GodDefault(uuid As String) As String
        Get
            If uuid Is Nothing Then Return "True"
            If Bad(uuid) Then Return "True"
            Return RegionList(uuid)._GodDefault
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._GodDefault = Value
        End Set
    End Property

    Public Property InRegion(uuid As String) As Integer
        Get
            If uuid Is Nothing Then Return 0
            If Bad(uuid) Then Return 0
            Return CInt(RegionList(uuid)._AvatarsInRegion)
        End Get
        Set(ByVal Value As Integer)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._AvatarsInRegion = Value
        End Set
    End Property

    Public Property LandingSpot(uuid As String) As String

        Get
            If uuid Is Nothing Then Return ""
            If Bad(uuid) Then Return ""
            Return RegionList(uuid)._RegionLandingSpot
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._RegionLandingSpot = Value
        End Set

    End Property

    Public Property ManagerGod(uuid As String) As String
        Get
            If uuid Is Nothing Then Return ""
            If Bad(uuid) Then Return ""
            Return RegionList(uuid)._ManagerGod
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._ManagerGod = Value
        End Set
    End Property

    Public Property MapType(uuid As String) As String
        Get
            If uuid Is Nothing Then Return ""
            If Bad(uuid) Then Return ""
            Return RegionList(uuid)._MapType
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._MapType = Value
        End Set
    End Property

    Public Property MinTimerInterval(uuid As String) As String
        Get
            If uuid Is Nothing Then Return ""
            If Bad(uuid) Then Return ""
            Return RegionList(uuid)._MinTimerInterval
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            If Value Is Nothing Then Return
            Value = Value.Replace(",", ".")
            RegionList(uuid)._MinTimerInterval = Value
        End Set
    End Property

    Public Property OpensimWorldAPIKey(uuid As String) As String
        Get
            If uuid Is Nothing Then Return ""
            If Bad(uuid) Then Return ""
            Return RegionList(uuid)._OSWAPIKey
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._OSWAPIKey = Value
        End Set
    End Property

    Public Property Physics(uuid As String) As String
        Get
            If uuid Is Nothing Then Return ""
            If Bad(uuid) Then Return ""
            Return RegionList(uuid)._Physics
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._Physics = Value
        End Set
    End Property

    Public Property RegionEnabled(uuid As String) As Boolean
        Get
            If uuid Is Nothing Then Return False
            If Bad(uuid) Then Return False
            Return RegionList(uuid)._RegionEnabled
        End Get
        Set(ByVal Value As Boolean)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._RegionEnabled = Value
        End Set
    End Property

    Public Property RegionGod(uuid As String) As String
        Get
            If uuid Is Nothing Then Return ""
            If Bad(uuid) Then Return ""
            Return RegionList(uuid)._RegionGod
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._RegionGod = Value
        End Set
    End Property

    Public Property RegionSnapShot(uuid As String) As String
        Get
            If uuid Is Nothing Then Return ""
            If Bad(uuid) Then Return ""
            Return RegionList(uuid)._RegionSnapShot
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._RegionSnapShot = Value
        End Set
    End Property

    Public Property ScriptEngine(uuid As String) As String
        Get
            If uuid Is Nothing Then Return ""
            If Bad(uuid) Then Return ""
            Return RegionList(uuid)._ScriptEngine
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._ScriptEngine = Value
        End Set
    End Property

    Public Property SkipAutobackup(uuid As String) As String
        Get
            If uuid Is Nothing Then Return ""
            If Bad(uuid) Then Return ""
            Return RegionList(uuid)._SkipAutobackup
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._SkipAutobackup = Value
        End Set
    End Property

    Public Property SmartStart(uuid As String) As String

        Get
            If uuid Is Nothing Then Return ""
            If Bad(uuid) Then Return ""
            Return RegionList(uuid)._RegionSmartStart
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._RegionSmartStart = Value
        End Set

    End Property

    Public Property Snapshot(uuid As String) As String
        Get
            If uuid Is Nothing Then Return ""
            If Bad(uuid) Then Return ""
            Return RegionList(uuid)._Snapshot
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._Snapshot = Value
        End Set
    End Property

    Public Property Teleport(uuid As String) As String
        Get
            If uuid Is Nothing Then Return ""
            If Bad(uuid) Then Return ""
            Return RegionList(uuid)._Teleport
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._Teleport = Value
        End Set
    End Property

    Public Property Tides(uuid As String) As String
        Get
            If uuid Is Nothing Then Return ""
            If Bad(uuid) Then Return ""
            Return RegionList(uuid)._Tides
        End Get
        Set(ByVal Value As String)
            If uuid Is Nothing Then Return
            If Bad(uuid) Then Return
            RegionList(uuid)._Tides = Value
        End Set
    End Property

#End Region

#Region "Functions"

    Public Sub DebugGroup()

        For Each pair In _Grouplist
            Debug.Print("Group name: {0}, http port: {1}", pair.Key, pair.Value)
        Next

    End Sub

    Public Sub DebugRegions(region As String)

        Log("uuid", CStr(region) & vbCrLf &
            " PID:" & RegionList(region)._ProcessID & vbCrLf &
            " Group:" & RegionList(region)._Group & vbCrLf &
            " Region:" & RegionList(region)._RegionName & vbCrLf &
            " Status=" & CStr(RegionList(region)._Status) & vbCrLf &
            " Crashes=" & CStr(RegionList(region)._CrashCounter) & vbCrLf &
           " RegionEnabled=" & RegionList(region)._RegionEnabled & vbCrLf &
           " Timer=" & CStr(RegionList(region)._Timer))

    End Sub

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

    Public Function FindRegionUUIDByName(name As String) As String

        Dim pair As KeyValuePair(Of String, Region_data)

        For Each pair In RegionList
            If name.ToUpperInvariant = pair.Value._RegionName.ToUpperInvariant Then
                Return pair.Value._UUID
            End If
        Next

        Return ""

    End Function

    Public Function IsBooted(uuid As String) As Boolean
        If uuid Is Nothing Then Return False
        If Bad(uuid) Then Return False
        If Status(uuid) = SIMSTATUSENUM.Booted Then
            Return True
        End If
        Return False

    End Function

    Public Sub RegionDump()

        If Not Debugger.IsAttached Then Return
        Dim pair As KeyValuePair(Of String, Region_data)
        For Each pair In RegionList
            DebugRegions(pair.Value._UUID)
        Next

    End Sub

    ''' <summary>
    ''' Returns a list if UUIDS of all regions in this group
    ''' </summary>
    ''' <param name="Gname">Group Name</param>
    ''' <returns>List of Region UUID's</returns>
    Public Function RegionUuidListByName(Gname As String) As List(Of String)

        Dim L As New List(Of String)

        Dim pair As KeyValuePair(Of String, Region_data)
        For Each pair In RegionList
            If pair.Value._Group = Gname Or Gname = "*" Then
                L.Add(pair.Value._UUID)
            End If
        Next

        Return L

    End Function

    Public Function RegionUuids() As List(Of String)

        Dim L As New List(Of String)
        Dim pair As KeyValuePair(Of String, Region_data)

        For Each pair In RegionList
            L.Add(pair.Value._UUID)
        Next

        Return L

    End Function

#End Region

#Region "POST"

    Shared Function CheckPassword(post As String, machine As String) As Boolean

        If machine Is Nothing Then Return False

        ' Returns true is password is blank or matching
        Dim pattern1 As Regex = New Regex("(?i)pw=(.*?)&", RegexOptions.IgnoreCase)
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

    Public Function ParsePost(post As String, settings As MySettings) As String

        If settings Is Nothing Then Return "<html><head></head><body>Error</html>"
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
        'Content-Type: Application/ json
        'Host:   tea.outworldz.net : 8001
        'Content-Length:  118
        'Connection: Keep-Alive
        '
        '"{""alert"":""region_ready"",""login"":""disabled"",""region_name"":""8021"",""region_id"":""c46ee5e5-5bb8-4cb5-8efd-eff44a0c7160""}"
        '"{"alert":"region_ready","login":"enabled","region_name":"Region 2","region_id":"19f6adf0-5f35-4106-bcb8-dc3f2e846b89"}
        '"{""alert"":""region_ready"",""login"":""shutdown"",""region_name"":""8021"",""region_id"":""c46ee5e5-5bb8-4cb5-8efd-eff44a0c7160""}"
        ' we want region name, UUID and server_startup could also be a probe from the outworldz to check if ports are open.

        ' WarmingUp(0) = True ShuttingDown(1) = True

        ' alerts need to be fast so we stash them on a list and process them on a 10 second timer.

        If post.Contains("/broker/") Then
            '{0} avatar name, {1} region name, {2} number of avatars
            BreakPoint.Show(post)
        ElseIf post.Contains("""alert"":""region_ready""") Then
            WebserverList.Add(post)
        ElseIf post.ToUpperInvariant.Contains("ALT=") Then
            Return SmartStartParse(post)
        ElseIf post.ToUpperInvariant.Contains("TOS") Then
            Return TOS(post)
        ElseIf post.ToUpperInvariant.Contains("SET_PARTNER") Then
            Return SetPartner(post)
        ElseIf post.ToUpperInvariant.Contains("GET_PARTNER") Then
            Return GetPartner(post)
        End If

        Return "Test Completed"

    End Function

#End Region

#Region "TOS"

    Private Shared Function TOS(post As String) As String

        ' currently unused as is only in stand alones
        Debug.Print("UUID:" + post)
        '"POST /TOS HTTP/1.1" & vbCrLf & "Host: mach.outworldz.net:9201" & vbCrLf & "Connection: keep-alive" & vbCrLf & "Content-Length: 102" & vbCrLf & "Cache-Control: max-age=0" & vbCrLf & "Upgrade-Insecure-Requests: 1" & vbCrLf & "User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36" & vbCrLf & "Origin: http://mach.outworldz.net:9201" & vbCrLf & "Content-Type: application/x-www-form-urlencoded" & vbCrLf & "DNT: 1" & vbCrLf & "Accept: text/html,application/xhtml+xml,application/xml;q=0.0909,image/webp,image/apng,*/*;q=0.8" & vbCrLf & "Referer: http://mach.outworldz.net:9200/wifi/termsofservice.html?uid=acb8fd92-c725-423f-b750-5fd971d73182&sid=40c5b80a-5377-4b97-820c-a0952782a701" & vbCrLf & "Accept-Encoding: gzip, deflate" & vbCrLf & "Accept-Language: en-US,en;q=0.0909" & vbCrLf & vbCrLf &
        '"action-accept=Accept&uid=acb8fd92-c725-423f-b750-5fd971d73182&sid=40c5b80a-5377-4b97-820c-a0952782a701"

        Return "<html><head></head><body>Error</html>"

        Dim uid As Guid
        Dim sid As Guid

        Try
            post = post.Replace("{ENTER}", "")
            post = post.Replace("\r", "")

            Dim pattern As Regex = New Regex("uid=([0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12})")
            Dim match As Match = pattern.Match(post)
            If match.Success Then
                uid = Guid.Parse(match.Groups(1).Value)
            End If

            Dim pattern2 As Regex = New Regex("sid=([0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12})")
            Dim match2 As Match = pattern2.Match(post)
            If match2.Success Then
                sid = Guid.Parse(match2.Groups(1).Value)
            End If

            If match.Success And match2.Success Then

                ' Only works in Standalone, anyway. Not implemented at all in Grid mode - the Diva DLL Diva is stubbed off.
                Dim result As Integer = 1

                Dim myConnection As MySqlConnection = New MySqlConnection(Settings.RobustMysqlConnection)

                Dim Query1 = "update opensim.griduser set TOS = 1 where UserID = @p1; "
                Dim myCommand1 As MySqlCommand = New MySqlCommand(Query1) With {
                    .Connection = myConnection
                }
                myConnection.Open()
                myCommand1.Prepare()
                myCommand1.Parameters.AddWithValue("p1", uid.ToString())
                myCommand1.ExecuteScalar()
                myConnection.Close()
                myConnection.Dispose()
                Return "<html><head></head><body>Welcome! You can close this window.</html>"
            Else
                Return "<html><head></head><body>Test Passed</html>"
            End If
        Catch ex As Exception
            BreakPoint.Show(ex.Message)
            Return "<html><head></head><body>Error</html>"
        End Try

    End Function

#End Region

#Region "Partners"

    Private Shared Function GetPartner(post As String) As String

        Debug.Print("Get Partner")
        Dim PWok As Boolean = CheckPassword(post, Settings.MachineID())
        If Not PWok Then Return ""

        Dim pattern1 As Regex = New Regex("User=(.*)", RegexOptions.IgnoreCase)
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

    Private Shared Function SetPartner(post As String) As String

        Debug.Print("set Partner")
        Dim PWok As Boolean = CheckPassword(post, CStr(Settings.MachineID()))
        If Not PWok Then Return ""

        Dim pattern1 As Regex = New Regex("User=(.*?)&", RegexOptions.IgnoreCase)
        Dim match1 As Match = pattern1.Match(post)
        If match1.Success Then

            Dim p2 As String = ""
            Dim p1 = match1.Groups(1).Value
            Dim pattern2 As Regex = New Regex("Partner=(.*)", RegexOptions.IgnoreCase)
            Dim match2 As Match = pattern2.Match(post)
            If match2.Success Then
                p2 = match2.Groups(1).Value
            End If
            Dim result1 As New Guid
            Dim result2 As New Guid
            If Guid.TryParse(p1, result1) And Guid.TryParse(p2, result2) Then
                Try

                    Dim Partner = MysqlGetPartner(p1, Settings)
                    If Partner = "00000000-0000-0000-0000-000000000000" Then
                        Partner = ""
                    End If

                    Using myConnection As MySqlConnection = New MySqlConnection(Settings.RobustMysqlConnection)
                        myConnection.Open()
                        Dim Query1 = "update userprofile set profilePartner=@p2 where useruuid=@p1; "
                        Using myCommand1 As MySqlCommand = New MySqlCommand(Query1) With {
                                .Connection = myConnection
                            }

                            myCommand1.Parameters.AddWithValue("@p1", result1.ToString)
                            myCommand1.Parameters.AddWithValue("@p2", result2.ToString)
                            Dim x = myCommand1.ExecuteNonQuery()
                            If x <> 1 Then
                                BreakPoint.Show($"Failed to return Partner rowcount={x}")
                            End If
                        End Using
                    End Using
                    Return Partner
                Catch ex As Exception
                    BreakPoint.Show(ex.Message)
                End Try

            End If
        End If
        Debug.Print("NULL response")
        Return ""

    End Function

#End Region

#Region "Opensim.ini writers"

    Public Function CopyOpensimProto(uuid As String) As Boolean

        Try
            '============== Opensim.ini =====================
            Dim OpensimPathName = OpensimIniPath(uuid)
            Dim Name = RegionName(uuid)
            Dim Group = GroupName(uuid)

            ' copy the prototype to the regions Opensim.ini

            CopyFileFast(GetOpensimProto(), IO.Path.Combine(OpensimPathName, "Opensim.ini"))
            Sleep(10) ' this should not be necessary but it is on some file systames

            Dim INI = New LoadIni(IO.Path.Combine(OpensimPathName, "Opensim.ini"), ";", System.Text.Encoding.UTF8)
            If INI Is Nothing Then Return True

            If INI.SetIni("Const", "MachineID", Settings.MachineID) Then Return True

            If Settings.StatusInterval > 0 Then
                If INI.SetIni("Startup", "timer_Script", "debug.txt") Then Return True
                If INI.SetIni("Startup", "timer_Interval", CStr(Settings.StatusInterval)) Then Return True
            Else
                If INI.SetIni("Startup", "timer_Script", "") Then Return True
                If INI.SetIni("Startup", "timer_Interval", "1200") Then Return True
            End If

            If INI.SetIni("RemoteAdmin", "port", CStr(GroupPort(uuid))) Then Return True
            If INI.SetIni("RemoteAdmin", "access_password", Settings.MachineID) Then Return True
            If INI.SetIni("Const", "PrivatePort", CStr(Settings.PrivatePort)) Then Return True
            If INI.SetIni("Const", "RegionFolderName", GroupName(uuid)) Then Return True
            If INI.SetIni("Const", "BaseHostname", Settings.BaseHostName) Then Return True
            If INI.SetIni("Const", "PublicPort", CStr(Settings.HttpPort)) Then Return True ' 8002
            If INI.SetIni("Const", "PrivURL", "http://" & CStr(Settings.LANIP())) Then Return True ' local IP
            If INI.SetIni("Const", "http_listener_port", CStr(GroupPort(uuid))) Then Return True ' varies with region

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
            Else
                If INI.SetIni("Startup", "DefaultScriptEngine", "YEngine") Then Return True
                If INI.SetIni("XEngine", "Enabled", "False") Then Return True
                If INI.SetIni("YEngine", "Enabled", "True") Then Return True
            End If

            ' set new Min Timer Interval for how fast a script can go.
            If INI.SetIni("XEngine", "MinTimerInterval", CStr(Settings.MinTimerInterval)) Then Return True
            If INI.SetIni("YEngine", "MinTimerInterval", CStr(Settings.MinTimerInterval)) Then Return True

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
                If INI.SetIni("Economy", "CurrencyURL", "${Const|BaseURL}:${Const|ApachePort}/jOpensim/index.php?option=com_opensim&view=interface") Then Return True
            Else
                If INI.SetIni("Startup", "economymodule", "BetaGridLikeMoneyModule") Then Return True
                If INI.SetIni("Economy", "CurrencyURL", "") Then Return True
            End If

            ' Main Frame time
            ' This defines the rate of several simulation events.
            ' Default value should meet most needs.
            ' It can be reduced To improve the simulation Of moving objects, with possible increase of CPU and network loads.
            'FrameTime = 0.0909

            If INI.SetIni("Startup", "FrameTime", Convert.ToString(1 / 11, Globalization.CultureInfo.InvariantCulture)) Then Return True

            ' LSL emails
            If INI.SetIni("SMTP", "SMTP_SERVER_HOSTNAME", Settings.SmtpHost) Then Return True
            If INI.SetIni("SMTP", "SMTP_SERVER_PORT", CStr(Settings.SmtpPort)) Then Return True
            If INI.SetIni("SMTP", "SMTP_SERVER_LOGIN", Settings.SmtPropUserName) Then Return True
            If INI.SetIni("SMTP", "SMTP_SERVER_PASSWORD", Settings.SmtpPassword) Then Return True
            If INI.SetIni("SMTP", "host_domain_header_from", Settings.BaseHostName) Then Return True

            ' the old Clouds
            If Settings.Clouds Then
                If INI.SetIni("Cloud", "enabled", "True") Then Return True
                If INI.SetIni("Cloud", "density", CStr(Settings.Density)) Then Return True
            Else
                If INI.SetIni("Cloud", "enabled", "False") Then Return True
            End If

            ' Physics choices for meshmerizer, where ODE requires a special one ZeroMesher meshing = Meshmerizer meshing = ubODEMeshmerizer 0 = none 1 = OpenDynamicsEngine 2 = BulletSim 3 = BulletSim with
            ' threads 4 = ubODE

            Select Case Settings.Physics
                Case 0
                    If INI.SetIni("Startup", "meshing", "ZeroMesher") Then Return True
                    If INI.SetIni("Startup", "physics", "basicphysics") Then Return True
                    If INI.SetIni("Startup", "UseSeparatePhysicsThread", "False") Then Return True
                    If INI.SetIni("ODEPhysicsSettings", "use_NINJA_physics_joints", "False") Then Return True
                Case 1
                    If INI.SetIni("Startup", "meshing", "Meshmerizer") Then Return True
                    If INI.SetIni("Startup", "physics", "OpenDynamicsEngine") Then Return True
                    If INI.SetIni("Startup", "UseSeparatePhysicsThread", "False") Then Return True
                    If INI.SetIni("ODEPhysicsSettings", "use_NINJA_physics_joints", CStr(Settings.NinjaRagdoll)) Then Return True
                Case 2
                    If INI.SetIni("Startup", "meshing", "Meshmerizer") Then Return True
                    If INI.SetIni("Startup", "physics", "BulletSim") Then Return True
                    If INI.SetIni("Startup", "UseSeparatePhysicsThread", "False") Then Return True
                    If INI.SetIni("ODEPhysicsSettings", "use_NINJA_physics_joints", "False") Then Return True
                Case 3
                    If INI.SetIni("Startup", "meshing", "Meshmerizer") Then Return True
                    If INI.SetIni("Startup", "physics", "BulletSim") Then Return True
                    If INI.SetIni("Startup", "UseSeparatePhysicsThread", "True") Then Return True
                    If INI.SetIni("ODEPhysicsSettings", "use_NINJA_physics_joints", "False") Then Return True
                Case 4
                    If INI.SetIni("Startup", "meshing", "ubODEMeshmerizer") Then Return True
                    If INI.SetIni("Startup", "physics", "ubODE") Then Return True
                    If INI.SetIni("Startup", "UseSeparatePhysicsThread", "False") Then Return True
                    If INI.SetIni("ODEPhysicsSettings", "use_NINJA_physics_joints", CStr(Settings.NinjaRagdoll)) Then Return True
                Case 5
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

            ' Voice
            If Settings.VivoxEnabled Then
                If INI.SetIni("VivoxVoice", "enabled", "True") Then Return True
            Else
                If INI.SetIni("VivoxVoice", "enabled", "False") Then Return True
            End If

            If INI.SetIni("VivoxVoice", "vivox_admin_user", Settings.VivoxUserName) Then Return True
            If INI.SetIni("VivoxVoice", "vivox_admin_password", Settings.VivoxPassword) Then Return True

            ' set new Min Timer Interval for how fast a script can go. Can be set in region files as a float, or nothing
            Dim Xtime As Double = 1 / 11   '1/11 of a second is as fast as she can go
            If MinTimerInterval(uuid).Length > 0 Then
                If Not Double.TryParse(MinTimerInterval(uuid), Xtime) Then
                    Xtime = 0.0909
                End If
            End If

            If INI.SetIni("XEngine", "MinTimerInterval", Convert.ToString(Xtime, Globalization.CultureInfo.InvariantCulture)) Then Return True
            If INI.SetIni("YEngine", "MinTimerInterval", Convert.ToString(Xtime, Globalization.CultureInfo.InvariantCulture)) Then Return True

            ' Gloebit
            If INI.SetIni("Gloebit", "Enabled", CStr(Settings.GloebitsEnable)) Then Return True
            If INI.SetIni("Gloebit", "GLBShowNewSessionAuthIM", CStr(Settings.GLBShowNewSessionAuthIM)) Then Return True
            If INI.SetIni("Gloebit", "GLBShowNewSessionPurchaseIM", CStr(Settings.GLBShowNewSessionPurchaseIM)) Then Return True
            If INI.SetIni("Gloebit", "GLBShowWelcomeMessage", CStr(Settings.GLBShowWelcomeMessage)) Then Return True

            If Settings.GloebitsMode Then
                If INI.SetIni("Gloebit", "GLBEnvironment", "production") Then Return True
                If INI.SetIni("Gloebit", "GLBKey", Settings.GLProdKey) Then Return True
                If INI.SetIni("Gloebit", "GLBSecret", Settings.GLProdSecret) Then Return True
            Else
                If INI.SetIni("Gloebit", "GLBEnvironment", "sandbox") Then Return True
                If INI.SetIni("Gloebit", "GLBKey", Settings.GLSandKey) Then Return True
                If INI.SetIni("Gloebit", "GLBSecret", Settings.GLSandSecret) Then Return True
            End If

            If INI.SetIni("Gloebit", "GLBOwnerName", Settings.GLBOwnerName) Then Return True
            If INI.SetIni("Gloebit", "GLBOwnerEmail", Settings.GLBOwnerEmail) Then Return True
            If Settings.ServerType = RobustServerName Then
                If INI.SetIni("Gloebit", "GLBSpecificConnectionString", Settings.RobustDBConnection) Then Return True
            Else
                If INI.SetIni("Gloebit", "GLBSpecificConnectionString", Settings.RegionDBConnection) Then Return True
            End If

            ' Autobackup
            If INI.SetIni("AutoBackupModule", "AutoBackup", "True") Then Return True
            If Settings.AutoBackup And String.IsNullOrEmpty((uuid)) Then Return True
            If INI.SetIni("AutoBackupModule", "AutoBackup", "True") Then Return True

            If Settings.AutoBackup And SkipAutobackup(uuid) = "True" Then
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

            Select Case Physics(uuid)
                Case "0"
                    If INI.SetIni("Startup", "meshing", "ZeroMesher") Then Return True
                    If INI.SetIni("Startup", "physics", "basicphysics") Then Return True
                    If INI.SetIni("Startup", "UseSeparatePhysicsThread", "False") Then Return True
                Case "1"
                    If INI.SetIni("Startup", "meshing", "Meshmerizer") Then Return True
                    If INI.SetIni("Startup", "physics", "OpenDynamicsEngine") Then Return True
                    If INI.SetIni("Startup", "UseSeparatePhysicsThread", "False") Then Return True
                Case "2"
                    If INI.SetIni("Startup", "meshing", "Meshmerizer") Then Return True
                    If INI.SetIni("Startup", "physics", "BulletSim") Then Return True
                    If INI.SetIni("Startup", "UseSeparatePhysicsThread", "False") Then Return True
                Case "3"
                    If INI.SetIni("Startup", "meshing", "Meshmerizer") Then Return True
                    If INI.SetIni("Startup", "physics", "BulletSim") Then Return True
                    If INI.SetIni("Startup", "UseSeparatePhysicsThread", "True") Then Return True
                Case "4"
                    If INI.SetIni("Startup", "meshing", "ubODEMeshmerizer") Then Return True
                    If INI.SetIni("Startup", "physics", "ubODE") Then Return True
                    If INI.SetIni("Startup", "UseSeparatePhysicsThread", "False") Then Return True
                Case "5"
                    If INI.SetIni("Startup", "meshing", "Meshmerizer") Then Return True
                    If INI.SetIni("Startup", "physics", "ubODE") Then Return True
                    If INI.SetIni("Startup", "UseSeparatePhysicsThread", "False") Then Return True
                Case Else
                    ' do nothing
            End Select

            'Gods
            If String.IsNullOrEmpty(GodDefault(uuid)) Or GodDefault(uuid) = "False" Then

                Select Case AllowGods(uuid)
                    Case ""
                        If INI.SetIni("Permissions", "allow_grid_gods", CStr(Settings.AllowGridGods)) Then Return True
                    Case "False"
                        If INI.SetIni("Permissions", "allow_grid_gods", "False") Then Return True
                    Case "True"
                        If INI.SetIni("Permissions", "allow_grid_gods", "True") Then Return True
                End Select

                Select Case RegionGod(uuid)
                    Case ""
                        If INI.SetIni("Permissions", "region_owner_is_god", CStr(Settings.RegionOwnerIsGod)) Then Return True
                    Case "False"
                        If INI.SetIni("Permissions", "region_owner_is_god", "False") Then Return True
                    Case "True"
                        If INI.SetIni("Permissions", "region_owner_is_god", "True") Then Return True
                End Select

                Select Case ManagerGod(uuid)
                    Case ""
                        If INI.SetIni("Permissions", "region_manager_is_god", CStr(Settings.RegionManagerIsGod)) Then Return True
                    Case "False"
                        If INI.SetIni("Permissions", "region_manager_is_god", "False") Then Return True
                    Case "True"
                        If INI.SetIni("Permissions", "region_manager_is_god", "True") Then Return True
                End Select
            Else
                If INI.SetIni("Permissions", "allow_grid_gods", "False") Then Return True
                If INI.SetIni("Permissions", "region_manager_is_god", "False") Then Return True
                If INI.SetIni("Permissions", "allow_grid_gods", "True") Then Return True
            End If

            If NonPhysicalPrimMax(uuid).Length > 0 Then
                If INI.SetIni("Startup", "NonPhysicalPrimMax", CStr(NonPhysicalPrimMax(uuid))) Then Return True
            End If

            If PhysicalPrimMax(uuid).Length > 0 Then
                If INI.SetIni("Startup", "PhysicalPrimMax", CStr(PhysicalPrimMax(uuid))) Then Return True
            End If

            If MinTimerInterval(uuid).Length > 0 Then
                If INI.SetIni("XEngine", "MinTimerInterval", CStr(MinTimerInterval(uuid))) Then Return True
            End If

            If FrameTime(uuid).Length > 0 Then
                If INI.SetIni("Startup", "FrameTime", CStr(FrameTime(uuid))) Then Return True
            End If

            If DisallowForeigners(uuid) = "True" Then
                If INI.SetIni("DisallowForeigners", "Enabled", CStr(DisallowForeigners(uuid))) Then Return True
            End If

            If DisallowResidents(uuid) = "True" Then
                If INI.SetIni("DisallowResidents", "Enabled", CStr(DisallowResidents(uuid))) Then Return True
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

            'ScriptEngine Overrides
            If ScriptEngine(uuid) = "XEngine" Then
                If INI.SetIni("Startup", "DefaultScriptEngine", "XEngine") Then Return True
                If INI.SetIni("XEngine", "Enabled", "True") Then Return True
                If INI.SetIni("YEngine", "Enabled", "False") Then Return True
            ElseIf ScriptEngine(uuid) = "YEngine" Then
                If INI.SetIni("Startup", "DefaultScriptEngine", "YEngine") Then Return True
                If INI.SetIni("XEngine", "Enabled", "False") Then Return True
                If INI.SetIni("YEngine", "Enabled", "True") Then Return True
            End If

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

            If Settings.SmartStart Then
                If INI.SetIni("Startup", "SmartStart", SmartStart(uuid)) Then Return True
            End If

            If INI.SetIni("Estates", "DefaultEstateName", gEstateName) Then Return True
            If INI.SetIni("Estates", "DefaultEstateOwnerName", gEstateOwner) Then Return True

            INI.SaveINI()

            '============== Region.ini =====================
            ' Region.ini in Region Folder specific to this region

            INI = New LoadIni(RegionIniFilePath(uuid), ";", System.Text.Encoding.UTF8)

            If INI.SetIni(Name, "InternalPort", CStr(RegionPort(uuid))) Then Return True
            If INI.SetIni(Name, "GroupPort", CStr(GroupPort(uuid))) Then Return True
            If INI.SetIni(Name, "ExternalHostName", Settings.ExternalHostName()) Then Return True
            If INI.SetIni(Name, "ClampPrimSize", CStr(ClampPrimSize(uuid))) Then Return True

            ' not a standard only use by the Dreamers
            If RegionEnabled(uuid) Then
                If INI.SetIni(Name, "Enabled", "True") Then Return True
            Else
                If INI.SetIni(Name, "Enabled", "False") Then Return True
            End If

            Select Case NonPhysicalPrimMax(uuid)
                Case ""
                    If INI.SetIni(Name, "NonPhysicalPrimMax", CStr(1024)) Then Return True
                Case Else
                    If INI.SetIni(Name, "NonPhysicalPrimMax", NonPhysicalPrimMax(uuid)) Then Return True
            End Select

            Select Case PhysicalPrimMax(uuid)
                Case ""
                    If INI.SetIni(Name, "PhysicalPrimMax", CStr(64)) Then Return True
                Case Else
                    If INI.SetIni(Name, "PhysicalPrimMax", PhysicalPrimMax(uuid)) Then Return True
            End Select

            If Settings.Primlimits Then
                Select Case MaxPrims(uuid)
                    Case ""
                        If INI.SetIni(Name, "MaxPrims", CStr(45000)) Then Return True
                    Case Else
                        If INI.SetIni(Name, "MaxPrims", MaxPrims(uuid)) Then Return True
                End Select
            Else
                Select Case MaxPrims(uuid)
                    Case ""
                        If INI.SetIni(Name, "MaxPrims", CStr(45000)) Then Return True
                    Case Else
                        If INI.SetIni(Name, "MaxPrims", MaxPrims(uuid)) Then Return True
                End Select
            End If

            Select Case MaxAgents(uuid)
                Case ""
                    If INI.SetIni(Name, "MaxAgents", CStr(100)) Then Return True
                Case Else
                    If INI.SetIni(Name, "MaxAgents", MaxAgents(uuid)) Then Return True
            End Select

            ' Maps
            If MapType(uuid) = "None" Then
                If INI.SetIni(Name, "GenerateMaptiles", "False") Then Return True
            ElseIf MapType(uuid) = "Simple" Then
                If INI.SetIni(Name, "GenerateMaptiles", "True") Then Return True
                If INI.SetIni(Name, "MapImageModule", "MapImageModule") Then Return True ' versus Warp3DImageModule
                If INI.SetIni(Name, "TextureOnMapTile", "False") Then Return True        ' versus True
                If INI.SetIni(Name, "DrawPrimOnMapTile", "False") Then Return True
                If INI.SetIni(Name, "TexturePrims", "False") Then Return True
                If INI.SetIni(Name, "RenderMeshes", "False") Then Return True
            ElseIf MapType(uuid) = "Good" Then
                If INI.SetIni(Name, "GenerateMaptiles", "True") Then Return True
                If INI.SetIni(Name, "MapImageModule", "Warp3DImageModule") Then Return True ' versus MapImageModule
                If INI.SetIni(Name, "TextureOnMapTile", "False") Then Return True        ' versus True
                If INI.SetIni(Name, "DrawPrimOnMapTile", "False") Then Return True
                If INI.SetIni(Name, "TexturePrims", "False") Then Return True
                If INI.SetIni(Name, "RenderMeshes", "False") Then Return True
            ElseIf MapType(uuid) = "Better" Then
                If INI.SetIni(Name, "GenerateMaptiles", "True") Then Return True
                If INI.SetIni(Name, "MapImageModule", "Warp3DImageModule") Then Return True ' versus MapImageModule
                If INI.SetIni(Name, "TextureOnMapTile", "True") Then Return True        ' versus True
                If INI.SetIni(Name, "DrawPrimOnMapTile", "True") Then Return True
                If INI.SetIni(Name, "TexturePrims", "False") Then Return True
                If INI.SetIni(Name, "RenderMeshes", "False") Then Return True
            ElseIf MapType(uuid) = "Best" Then
                If INI.SetIni(Name, "GenerateMaptiles", "True") Then Return True
                If INI.SetIni(Name, "MapImageModule", "Warp3DImageModule") Then Return True ' versus MapImageModule
                If INI.SetIni(Name, "TextureOnMapTile", "True") Then Return True     ' versus True
                If INI.SetIni(Name, "DrawPrimOnMapTile", "True") Then Return True
                If INI.SetIni(Name, "TexturePrims", "True") Then Return True
                If INI.SetIni(Name, "RenderMeshes", "True") Then Return True
            Else
                If INI.SetIni(Name, "GenerateMaptiles", "") Then Return True
                If INI.SetIni(Name, "MapImageModule", "") Then Return True  ' versus MapImageModule
                If INI.SetIni(Name, "TextureOnMapTile", "") Then Return True      ' versus True
                If INI.SetIni(Name, "DrawPrimOnMapTile", "") Then Return True
                If INI.SetIni(Name, "TexturePrims", "") Then Return True
                If INI.SetIni(Name, "RenderMeshes", "") Then Return True
            End If

            'Options and overrides

            If INI.SetIni(Name, "Concierge", Concierge(uuid)) Then Return True
            If INI.SetIni(Name, "DisableGloebits", DisableGloebits(uuid)) Then Return True
            If INI.SetIni(Name, "RegionSnapShot", RegionSnapShot(uuid)) Then Return True
            If INI.SetIni(Name, "Birds", Birds(uuid)) Then Return True
            If INI.SetIni(Name, "Tides", Tides(uuid)) Then Return True
            If INI.SetIni(Name, "Teleport", Teleport(uuid)) Then Return True
            If INI.SetIni(Name, "DisallowForeigners", DisallowForeigners(uuid)) Then Return True
            If INI.SetIni(Name, "DisallowResidents", DisallowResidents(uuid)) Then Return True
            If INI.SetIni(Name, "SkipAutoBackup", SkipAutobackup(uuid)) Then Return True
            If INI.SetIni(Name, "Physics", Physics(uuid)) Then Return True
            If INI.SetIni(Name, "FrameTime", FrameTime(uuid)) Then Return True

            INI.SaveINI()

            Settings.SaveSettings()
        Catch ex As Exception
            ErrorLog(ex.Message)
            Return True
        End Try

        Return False

    End Function

    Private Shared Sub SetupOpensimIM(INI As LoadIni)

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

    Private Function Bad(uuid As String) As Boolean

        If RegionList.ContainsKey(uuid) Then
            Return False
        End If

        Return True

    End Function

    Private Sub SetupOpensimSearchINI(INI As LoadIni)

        ' RegionSnapShot
        INI.SetIni("DataSnapshot", "index_sims", "True")
        If Settings.CMS = JOpensim And Settings.SearchOptions = JOpensim Then
            INI.SetIni("DataSnapshot", "data_services", "")
        ElseIf Settings.SearchOptions = Hyperica Then
            INI.SetIni("DataSnapshot", "data_services", "http://hyperica.com/Search/register.php")
        Else
            INI.SetIni("DataSnapshot", "data_services", "")
        End If

        If Settings.CMS = JOpensim Then
            CopyFileFast(IO.Path.Combine(Settings.OpensimBinPath, "jOpensimProfile.Modules.dll.bak"),
                         IO.Path.Combine(Settings.OpensimBinPath, "jOpensimProfile.Modules.dll"))
        Else
            DeleteFile(IO.Path.Combine(Settings.OpensimBinPath, "jOpensimProfile.Modules.dll"))
        End If

        If Settings.SearchOptions = JOpensim Then
            Dim SearchURL = "http://" & Settings.PublicIP & ":" & Settings.ApachePort &
                "/jOpensim/index.php?option=com_opensim&view=interface"
            INI.SetIni("Search", "SearchURL", SearchURL)
            INI.SetIni("LoginService", "SearchURL", SearchURL)
            CopyFileFast(IO.Path.Combine(Settings.OpensimBinPath, "jOpensimSearch.Modules.dll.bak"),
                         IO.Path.Combine(Settings.OpensimBinPath, "jOpensimSearch.Modules.dll"))
        ElseIf Settings.SearchOptions = Hyperica Then
            Dim SearchURL = "http://hyperica.com/Search/query.php"
            INI.SetIni("Search", "SearchURL", SearchURL)
            INI.SetIni("LoginService", "SearchURL", SearchURL)
            DeleteFile(IO.Path.Combine(Settings.OpensimBinPath, "jOpensimSearch.Modules.dll"))
        Else
            INI.SetIni("Search", "SearchURL", "")
            INI.SetIni("LoginService", "SearchURL", "")
            DeleteFile(IO.Path.Combine(Settings.OpensimBinPath, "jOpensimSearch.Modules.dll"))
        End If

    End Sub

#End Region

End Class
