#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC. AGPL3.0 https://opensource.org/licenses/AGPL

#End Region

Imports System.Globalization
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Management
Imports System.Threading
Imports IWshRuntimeLibrary

Public Class FormSetup

#Region "Vars"

    Public Event LinkClicked As System.Windows.Forms.LinkClickedEventHandler

#End Region

#Region "Private Declarations"

#Disable Warning CA2213 ' Disposable fields should be disposed
    ReadOnly BackupThread As New Backups

    Private ReadOnly HandlerSetup As New EventHandler(AddressOf Resize_page)
    Private ReadOnly CurrentLocation As New Dictionary(Of String, String)
    Private _Adv As FormSettings
    Private _ContentIAR As FormOAR
    Private _ContentOAR As FormOAR
    Private _DNSSTimer As Integer
    Private _IcecastCrashCounter As Integer
    Private _IceCastExited As Boolean
    Private _IPv4Address As String
    Private _KillSource As Boolean
    Private _regionForm As FormRegionlist
    Private _RestartApache As Boolean
    Private _RestartMysql As Boolean
    Private _speed As Double = 50
    Private cpu As New PerformanceCounter
    Private Graphs As New FormGraphs
    Private ScreenPosition As ClassScreenpos
    Private searcher As ManagementObjectSearcher
    Private speed As Double
    Private speed1 As Double
    Private speed2 As Double
    Private speed3 As Double
    Private TimerisBusy As Integer
    Private wql As New ObjectQuery("Select TotalVirtualMemorySize, FreeVirtualMemory ,TotalVisibleMemorySize, FreePhysicalMemory FROM Win32_OperatingSystem")
    Private ws As NetServer

#End Region

#Region "Properties"

    Public Property Adv1 As FormSettings
        Get
            Return _Adv
        End Get
        Set(value As FormSettings)
            _Adv = value
        End Set
    End Property

    Public Property ContentIar As FormOAR
        Get
            Return _ContentIAR
        End Get
        Set(value As FormOAR)
            _ContentIAR = value
        End Set
    End Property

    Public Property ContentOAR As FormOAR
        Get
            Return _ContentOAR
        End Get
        Set(value As FormOAR)
            _ContentOAR = value
        End Set
    End Property

    Public Property Cpu1 As PerformanceCounter
        Get
            Return cpu
        End Get
        Set(value As PerformanceCounter)
            cpu = value
        End Set
    End Property

    Public Property CPUAverageSpeed As Double
        Get
            Return _speed
        End Get
        Set(value As Double)
            _speed = value
        End Set
    End Property

    Public Property IcecastCrashCounter As Integer
        Get
            Return _IcecastCrashCounter
        End Get
        Set(value As Integer)
            _IcecastCrashCounter = value
        End Set
    End Property

    Public Property PropIceCastExited() As Boolean
        Get
            Return _IceCastExited
        End Get
        Set(ByVal Value As Boolean)
            _IceCastExited = Value
        End Set
    End Property

    Public Property PropIPv4Address() As String
        Get
            Return _IPv4Address
        End Get
        Set(ByVal Value As String)
            _IPv4Address = Value
        End Set
    End Property

    Public Property PropKillSource As Boolean
        Get
            Return _KillSource
        End Get
        Set(value As Boolean)
            _KillSource = value
        End Set
    End Property

    Public Property PropRegionForm As FormRegionlist
        Get
            Return _regionForm
        End Get
        Set(value As FormRegionlist)
            _regionForm = value
        End Set
    End Property

    Public Property PropRestartApache() As Boolean
        Get
            Return _RestartApache
        End Get
        Set(ByVal Value As Boolean)
            _RestartApache = Value
        End Set
    End Property

    Public Property PropRestartMySql() As Boolean
        Get
            Return _RestartMysql
        End Get
        Set(ByVal Value As Boolean)
            _RestartMysql = Value
        End Set
    End Property

    Public Property PropWebserver As NetServer
        Get
            Return ws
        End Get
        Set(value As NetServer)
            ws = value
        End Set
    End Property

    Public Property ScreenPosition1 As ClassScreenpos
        Get
            Return ScreenPosition
        End Get
        Set(value As ClassScreenpos)
            ScreenPosition = value
        End Set
    End Property

    Public Property Searcher1 As ManagementObjectSearcher
        Get
            Return searcher
        End Get
        Set(value As ManagementObjectSearcher)
            searcher = value
        End Set
    End Property

    Public Property SecondsTicker() As Integer
        Get
            Return _DNSSTimer
        End Get
        Set(ByVal Value As Integer)
            _DNSSTimer = Value
        End Set
    End Property

#End Region

#Region "Resize"

    Private Sub Resize_page(ByVal sender As Object, ByVal e As EventArgs)
        ScreenPosition1.SaveXY(Me.Left, Me.Top)
        ScreenPosition1.SaveHW(Me.Height, Me.Width)
    End Sub

#End Region

#Region "Errors"

    Public Shared Sub ErrorGroup(Groupname As String)

        For Each RegionUUID As String In RegionUuidListByName(Groupname)
            RegionStatus(RegionUUID) = SIMSTATUSENUM.Error
            PokeRegionTimer(RegionUUID)
        Next

    End Sub

#End Region

#Region "Maps"

    ''' <summary>Brings up a region chooser with no buttons, of all regions</summary>
    Public Shared Sub ShowRegionMap()

        Dim region = ChooseRegion(False)
        If region.Length = 0 Then Return

        VarChooser(region, False, False)

    End Sub

#End Region

#Region "Start/Stop"

    Public Shared Sub StopGroup(Groupname As String)

        For Each RegionUUID As String In RegionUuidListByName(Groupname)
            RegionStatus(RegionUUID) = SIMSTATUSENUM.Stopped
            PokeRegionTimer(RegionUUID)
        Next
        PropUpdateView = True

    End Sub

    Public Function DoStopActions() As Boolean

        TextPrint(My.Resources.Stopping_word)
        Buttons(BusyButton)

        If Not KillAll() Then Return False
        Buttons(StartButton)
        TextPrint(My.Resources.Stopped_word)

        Return True

    End Function

    Public Function KillAll() As Boolean

        If ScanAgents() > 0 Then
            Dim response = MsgBox(My.Resources.Avatars_in_World, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground, My.Resources.Agents_word)
            If response = vbNo Then Return False
        End If

        ToDoList.Clear()

        PropAborting = True

        ' close everything as gracefully as possible.

        StopIcecast()

        Dim n As Integer = RegionCount()

        ' only wait if the port 8001 is working

        If PropOpensimIsRunning Then TextPrint(My.Resources.Waiting_text)

        For Each RegionUUID In RegionUuids()
            If RegionEnabled(RegionUUID) And
            (RegionStatus(RegionUUID) = SIMSTATUSENUM.Booted Or
            RegionStatus(RegionUUID) = SIMSTATUSENUM.Suspended Or
            RegionStatus(RegionUUID) = SIMSTATUSENUM.ShuttingDownForGood Or
            RegionStatus(RegionUUID) = SIMSTATUSENUM.Booting) Then
                SequentialPause()

                If CBool(GetHwnd(Group_Name(RegionUUID))) Then
                    TextPrint(Group_Name(RegionUUID) & " " & Global.Outworldz.My.Resources.Stopping_word)
                    ReallyShutDown(RegionUUID, SIMSTATUSENUM.ShuttingDownForGood)
                Else
                    RegionStatus(RegionUUID) = SIMSTATUSENUM.Stopped
                End If
            End If
            Application.DoEvents()
        Next

        Dim LastCount As Integer = 0
        Dim counter As Integer = 3000 ' 5 minutes to quit all regions

        If PropOpensimIsRunning Then TextPrint(My.Resources.Waiting_text)

        While (counter > 0 AndAlso PropOpensimIsRunning())
            Application.DoEvents()
            counter -= 1

            Dim ListofPIDs = RegionPIDs()
            Dim CountisRunning As New List(Of Integer)
            Dim AllProcesses() = Process.GetProcessesByName("Opensim") ' cache of processes
            For Each P In AllProcesses
                If ListofPIDs.Contains(P.Id) Then
                    CountisRunning.Add(P.Id)
                End If
            Next

            If CountisRunning.Count <> LastCount Then
                If CountisRunning.Count = 1 Then
                    TextPrint(My.Resources.One_region)
                Else
                    TextPrint($"{CStr(CountisRunning.Count)} {Global.Outworldz.My.Resources.Regions_Are_Running}")
                End If
            End If

            LastCount = CountisRunning.Count

            If CountisRunning.Count = 0 Then
                counter = 0
            End If

            ProcessQuit()   '  check if any processes exited
            CheckPost()                 ' see if anything arrived in the web server

            For Each PID In CountisRunning
                For Each RegionUUID In RegionUuids()
                    If ProcessID(RegionUUID) = PID Then
                        ReallyShutDown(RegionUUID, SIMSTATUSENUM.ShuttingDownForGood)
                        Application.DoEvents()
                    End If
                Next
            Next

            Sleep(1000)
        End While
        PropUpdateView = True ' make form refresh

        ClearAllRegions()
        StopRobust()
        Zap("baretail")
        Zap("cports")

        TimerMain.Stop()

        ProcessIdDict.Clear()

        PropInstanceHandles.Clear()
        PropOpensimIsRunning() = False

        Settings.SaveSettings()

        Return True

    End Function

    Public Sub Language(sender As Object, e As EventArgs)
        Settings.SaveSettings()

        'For Each ci As CultureInfo In CultureInfo.GetCultures(CultureTypes.NeutralCultures)
        'Breakpoint.Print("")
        'Breakpoint.Print(ci.Name)
        'Breakpoint.Print(ci.TwoLetterISOLanguageName)
        'Breakpoint.Print(ci.ThreeLetterISOLanguageName)
        'Breakpoint.Print(ci.ThreeLetterWindowsLanguageName)
        'Breakpoint.Print(ci.DisplayName)
        'Breakpoint.Print(ci.EnglishName)
        'Next

        My.Application.ChangeUICulture(Settings.Language)
        My.Application.ChangeCulture(Settings.Language)
        Me.Controls.Clear() 'removes all the controls on the form
        InitializeComponent() 'load all the controls again
        FrmHome_Load(sender, e) 'Load everything in your form load event again
    End Sub

    Public Function StartOpensimulator() As Boolean

        Bench.Start("StartOpensim")
        StartTimer()

        PropOpensimIsRunning = True

        OpenPorts()

        Dim ini = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Opensim\bin\OpenSim.exe.config")
        Grep(ini, Settings.LogLevel)

        If Not Settings.DeregisteredOnce Then
            DeregisterRegions(True)
            Settings.DeregisteredOnce = True
        End If

        If Not PropOpensimIsRunning Then Return False
        PropAborting = False
        Buttons(BusyButton)

        DoEstates() ' has to be done after MySQL starts up.

        CheckOverLap()

        If Settings.ServerType = RobustServerName Then
            StartRobust()
            Dim ctr = 60
            While Not IsRobustRunning() AndAlso ctr > 0
                Sleep(1000)
                ctr -= 1
            End While

            Dim RegionName = Settings.WelcomeRegion
            Dim UUID As String = FindRegionByName(RegionName)
            Dim out As New Guid
            If Guid.TryParse(UUID, out) Then
                Boot(RegionName)
            End If
        End If

        If Settings.GraphVisible Then
            G()
        End If

        For Each RegionUUID In RegionUuids()

            If RegionEnabled(RegionUUID) Then

                Dim RegionName = Region_Name(RegionUUID)
                If Settings.WelcomeRegion = RegionName Then Continue For

                If PropOpensimIsRunning Then
                    Boot(RegionName)
                End If
            End If
            Application.DoEvents()
        Next

        Settings.SaveSettings()
        CalcCPU()
        Buttons(StopButton)
        TextPrint(My.Resources.Ready)
        Bench.Print("StartOpensim")
        Return True

    End Function

    Private Sub Form1_Closed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        Dim result = MsgBox(My.Resources.AreYouSure, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground Or MsgBoxStyle.Exclamation, My.Resources.Quit_Now_Word)
        If result <> vbYes Then
            Return
        End If

        If Not RunningBackupName.IsEmpty Then
            Dim response = MsgBox($"{My.Resources.backup_running} .  {My.Resources.Quit_Now_Word}?", MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground, My.Resources.Agents_word)
            If response = vbNo Then Return
        End If

        ReallyQuit()

    End Sub

    ''' <summary>Form Load is main() for all DreamGrid</summary>
    ''' <param name="sender">Unused</param>
    ''' <param name="e">Unused</param>
    Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

        Application.EnableVisualStyles()

        Dim _myFolder As String = My.Application.Info.DirectoryPath

        ' setup a debug path
        If Debugger.IsAttached Then
            ' for debugging when compiling
            _myFolder = _myFolder.Replace("\Installer_Src\Setup DreamWorld\bin\Debug", "")
            _myFolder = _myFolder.Replace("\Installer_Src\Setup DreamWorld\bin\Release", "")
            ' for testing, as the compiler buries itself in ../../../debug
        End If

        If Not System.IO.File.Exists(_myFolder & "\OutworldzFiles\Settings.ini") Then
            Create_ShortCut(_myFolder & "\Start.exe")
        End If

        Settings = New MySettings(_myFolder) With {
            .CurrentDirectory = _myFolder
        }

        Settings.CurrentSlashDir = _myFolder.Replace("\", "/")    ' because MySQL uses Unix like slashes, that's why

        Settings.OpensimBinPath() = _myFolder & "\OutworldzFiles\Opensim\bin\"

        Log("Startup", DisplayObjectInfo(Me))

        Dim cinfo() = System.Globalization.CultureInfo.GetCultures(CultureTypes.AllCultures)
        Try
            My.Application.ChangeUICulture(Settings.Language)
            My.Application.ChangeCulture(Settings.Language)
        Catch
            My.Application.ChangeUICulture("en")
            My.Application.ChangeCulture("en")
        End Try

        Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture

        Me.Controls.Clear() 'removes all the controls on the form
        InitializeComponent() 'load all the controls again
        Application.DoEvents()
        FrmHome_Load(sender, e) 'Load everything in your form load event again so it will be translated

    End Sub

    Private Sub FrmHome_Load(ByVal sender As Object, ByVal e As EventArgs)

        TextPrint("Language Is " & CultureInfo.CurrentCulture.Name)

        SetScreen()     ' move Form to fit screen from SetXY.ini

        AddUserToolStripMenuItem.Text = Global.Outworldz.My.Resources.Add_User_word
        AdvancedSettingsToolStripMenuItem.Image = Global.Outworldz.My.Resources.earth_network
        AdvancedSettingsToolStripMenuItem.Text = Global.Outworldz.My.Resources.Settings_word
        AdvancedSettingsToolStripMenuItem.ToolTipText = Global.Outworldz.My.Resources.All_Global_Settings_word
        AllUsersAllSimsToolStripMenuItem.Text = Global.Outworldz.My.Resources.All_Users_All_Sims_word
        BackupCriticalFilesToolStripMenuItem.Image = Global.Outworldz.My.Resources.disk_blue
        BackupCriticalFilesToolStripMenuItem.Text = Global.Outworldz.My.Resources.System_Backup_word
        BackupToolStripMenuItem1.Image = Global.Outworldz.My.Resources.disk_blue
        BackupToolStripMenuItem1.Text = Global.Outworldz.My.Resources.Backup_Databases

        BusyButton.Text = Global.Outworldz.My.Resources.Busy_word
        CHeckForUpdatesToolStripMenuItem.Image = Global.Outworldz.My.Resources.download
        CHeckForUpdatesToolStripMenuItem.Text = Global.Outworldz.My.Resources.Check_for_Updates_word
        ChangePasswordToolStripMenuItem.Text = Global.Outworldz.My.Resources.Change_Password_word
        FreezAllToolStripMenuItem.Text = My.Resources.FreezeAllRegions
        ThawAllToolStripMenuItem.Text = My.Resources.ThawAllRegions

        CheckAndRepairDatbaseToolStripMenuItem.Image = Global.Outworldz.My.Resources.Server_Client
        CheckAndRepairDatbaseToolStripMenuItem.Text = Global.Outworldz.My.Resources.Check_and_Repair_Database_word
        ClothingInventoryToolStripMenuItem.Image = Global.Outworldz.My.Resources.user1_into
        ClothingInventoryToolStripMenuItem.Text = Global.Outworldz.My.Resources.Load_Free_Avatar_Inventory_word
        ClothingInventoryToolStripMenuItem.ToolTipText = Global.Outworldz.My.Resources.Load_Free_Avatar_Inventory_text
        CommonConsoleCommandsToolStripMenuItem.Image = Global.Outworldz.My.Resources.text_marked
        CommonConsoleCommandsToolStripMenuItem.Text = Global.Outworldz.My.Resources.Issue_Commands

        ConnectToConsoleToolStripMenuItemMySQL.Text = Global.Outworldz.My.Resources.Connect2Console
        ConnectToIceCastToolStripMenuItemIcecast.Text = Global.Outworldz.My.Resources.Connect2Console
        ConnectToWebPageToolStripMenuItemApache.Text = Global.Outworldz.My.Resources.Connect2Console

        ConsoleCOmmandsToolStripMenuItem1.Image = Global.Outworldz.My.Resources.text_marked
        ConsoleCOmmandsToolStripMenuItem1.Text = Global.Outworldz.My.Resources.Help_Console
        ConsoleCOmmandsToolStripMenuItem1.ToolTipText = Global.Outworldz.My.Resources.Help_Console_text
        ConsoleToolStripMenuItem1.Image = Global.Outworldz.My.Resources.window_add
        ConsoleToolStripMenuItem1.Text = Global.Outworldz.My.Resources.Consoles_word
        ConsoleToolStripMenuItem1.ToolTipText = Global.Outworldz.My.Resources.Consoletext
        Debug.Text = Global.Outworldz.My.Resources.Debug_word
        DebugToolStripMenuItem.Text = Global.Outworldz.My.Resources.Set_Debug_Level_word
        DiagnosticsToolStripMenuItem.Image = Global.Outworldz.My.Resources.Server_Client
        DiagnosticsToolStripMenuItem.Text = Global.Outworldz.My.Resources.Network_Diagnostics
        DiagnosticsToolStripMenuItem.ToolTipText = Global.Outworldz.My.Resources.Network_Diagnostics_text
        ErrorToolStripMenuItem.Text = Global.Outworldz.My.Resources.Error_word
        Fatal1.Text = Global.Outworldz.My.Resources.Float
        FileToolStripMenuItem.Text = Global.Outworldz.My.Resources.File_word
        HelpOnIARSToolStripMenuItem.Image = Global.Outworldz.My.Resources.disks
        HelpOnIARSToolStripMenuItem.Text = Global.Outworldz.My.Resources.Help_On_IARS_word
        HelpOnIARSToolStripMenuItem.ToolTipText = Global.Outworldz.My.Resources.Help_IARS_text
        HelpOnOARsToolStripMenuItem.Image = Global.Outworldz.My.Resources.disks
        HelpOnOARsToolStripMenuItem.Text = Global.Outworldz.My.Resources.Help_OARS
        HelpOnOARsToolStripMenuItem.ToolTipText = Global.Outworldz.My.Resources.Help_OARS_text
        HelpOnSettingsToolStripMenuItem.Image = Global.Outworldz.My.Resources.gear
        HelpOnSettingsToolStripMenuItem.Text = Global.Outworldz.My.Resources.Help_Manuals_word
        HelpStartingUpToolStripMenuItem1.Image = Global.Outworldz.My.Resources.box_tall
        HelpStartingUpToolStripMenuItem1.Text = Global.Outworldz.My.Resources.Help_Startup
        HelpToolStripMenuItem.Text = Global.Outworldz.My.Resources.Help_word
        HelpToolStripMenuItem1.Image = Global.Outworldz.My.Resources.question_and_answer
        HelpToolStripMenuItem1.Text = Global.Outworldz.My.Resources.Help_word
        HelpToolStripMenuItem2.Image = Global.Outworldz.My.Resources.question_and_answer
        HelpToolStripMenuItem2.Text = Global.Outworldz.My.Resources.Help_word
        HelpToolStripMenuItem3.Image = Global.Outworldz.My.Resources.question_and_answer
        HelpToolStripMenuItem3.Text = Global.Outworldz.My.Resources.Help_word
        HelpToolStripMenuItem4.Image = Global.Outworldz.My.Resources.question_and_answer
        HelpToolStripMenuItem4.Text = Global.Outworldz.My.Resources.Help_word
        Info.Text = Global.Outworldz.My.Resources.Info_word
        IslandToolStripMenuItem.Image = Global.Outworldz.My.Resources.box_tall
        IslandToolStripMenuItem.Text = Global.Outworldz.My.Resources.Load_Free_DreamGrid_OARs_word
        JobEngineToolStripMenuItem.Text = Global.Outworldz.My.Resources.JobEngine_word
        JustOneRegionToolStripMenuItem.Text = Global.Outworldz.My.Resources.Just_one_region_word
        JustQuitToolStripMenuItem.Image = Global.Outworldz.My.Resources.exit_icon
        JustQuitToolStripMenuItem.Text = Global.Outworldz.My.Resources.Quit_Now_Word
        KeepOnTopToolStripMenuItem.Text = Global.Outworldz.My.Resources.Window_Word
        LanguageToolStripMenuItem1.Text = Global.Outworldz.My.Resources.Language
        LoadIARsToolMenuItem.Image = Global.Outworldz.My.Resources.user1_into
        LoadIARsToolMenuItem.Text = Global.Outworldz.My.Resources.Inventory_IAR_Load_and_Save_words
        LoadLocalOARSToolStripMenuItem.Image = Global.Outworldz.My.Resources.box_tall
        LoadLocalOARSToolStripMenuItem.Text = Global.Outworldz.My.Resources.Load_Local_OARs
        LoopBackToolStripMenuItem.Image = Global.Outworldz.My.Resources.refresh
        LoopBackToolStripMenuItem.Text = Global.Outworldz.My.Resources.Help_On_LoopBack_word
        LoopBackToolStripMenuItem.ToolTipText = Global.Outworldz.My.Resources.Help_Loopback_Text
        MinimizeAllToolStripMenuItem.Text = Global.Outworldz.My.Resources.Minimize_all
        ShowAllToolStripMenuItem.Text = Global.Outworldz.My.Resources.Show_all
        MnuContent.Text = Global.Outworldz.My.Resources.Content_word
        MoreFreeIslandsandPartsContentToolStripMenuItem.Image = Global.Outworldz.My.Resources.download
        MoreFreeIslandsandPartsContentToolStripMenuItem.Text = Global.Outworldz.My.Resources.More_Free_Islands_and_Parts_word
        MoreFreeIslandsandPartsContentToolStripMenuItem.ToolTipText = Global.Outworldz.My.Resources.Free_DLC_word
        RestartMysqlIcon.Image = Global.Outworldz.My.Resources.gear
        RestartMysqlIcon.Text = Global.Outworldz.My.Resources.Mysql_Word
        Off1.Text = Global.Outworldz.My.Resources.Off
        OnTopToolStripMenuItem.Text = Global.Outworldz.My.Resources.On_Top
        PDFManualToolStripMenuItem.Image = Global.Outworldz.My.Resources.pdf
        PDFManualToolStripMenuItem.Text = Global.Outworldz.My.Resources.PDF_Manual_word
        RegionsToolStripMenuItem.Image = Global.Outworldz.My.Resources.Server_Client
        RegionsToolStripMenuItem.Text = Global.Outworldz.My.Resources.Regions_word
        RestartApacheIcon.Image = Global.Outworldz.My.Resources.gear
        RestartApacheIcon.Text = Global.Outworldz.My.Resources.Apache_word
        RestartIceCastItem2.Image = Global.Outworldz.My.Resources.recycle
        RestartIceCastItem2.Text = Global.Outworldz.My.Resources.Restart_word
        RestartIcecastIcon.Image = Global.Outworldz.My.Resources.gear
        RestartIcecastIcon.Text = Global.Outworldz.My.Resources.Icecast_word
        RestartMysqlItem.Image = Global.Outworldz.My.Resources.recycle
        RestartMysqlItem.Text = Global.Outworldz.My.Resources.Restart_word
        RestartOneRegionToolStripMenuItem.Text = Global.Outworldz.My.Resources.Restart_one_region_word
        RestartRegionToolStripMenuItem.Text = Global.Outworldz.My.Resources.Restart_Region_word
        RestartRobustItem.Image = Global.Outworldz.My.Resources.recycle
        RestartRobustItem.Text = Global.Outworldz.My.Resources.Restart_word
        RestartTheInstanceToolStripMenuItem.Text = Global.Outworldz.My.Resources.Restart_one_instance_word
        RestartToolStripMenuItem2.Image = Global.Outworldz.My.Resources.recycle
        RestartToolStripMenuItem2.Text = Global.Outworldz.My.Resources.Restart_word
        RestoreToolStripMenuItem1.Image = Global.Outworldz.My.Resources.disk_blue
        RestoreToolStripMenuItem1.Text = Global.Outworldz.My.Resources.Restore_Database_word
        RevisionHistoryToolStripMenuItem.Image = Global.Outworldz.My.Resources.document_dirty
        RevisionHistoryToolStripMenuItem.Text = Global.Outworldz.My.Resources.Revision_History_word
        RestartRobustIcon.Image = Global.Outworldz.My.Resources.gear
        RestartRobustIcon.Text = Global.Outworldz.My.Resources.Robust_word
        ScriptsResumeToolStripMenuItem.Text = Global.Outworldz.My.Resources.Scripts_Resume_word
        ScriptsStartToolStripMenuItem.Text = Global.Outworldz.My.Resources.Scripts_Start_word
        ScriptsStopToolStripMenuItem.Text = Global.Outworldz.My.Resources.Scripts_Stop_word
        ScriptsSuspendToolStripMenuItem.Text = Global.Outworldz.My.Resources.Scripts_Suspend_word
        ScriptsToolStripMenuItem.Text = Global.Outworldz.My.Resources.Scripts_word
        SeePortsInUseToolStripMenuItem.Image = Global.Outworldz.My.Resources.server_connection
        SeePortsInUseToolStripMenuItem.Text = Global.Outworldz.My.Resources.See_Ports_In_Use_word
        SendAlertToAllUsersToolStripMenuItem.Text = Global.Outworldz.My.Resources.Send_Alert_Message_word
        ShowHyperGridAddressToolStripMenuItem.Image = Global.Outworldz.My.Resources.window_environment
        ShowHyperGridAddressToolStripMenuItem.Text = Global.Outworldz.My.Resources.Show_Grid_Address
        ShowHyperGridAddressToolStripMenuItem.ToolTipText = Global.Outworldz.My.Resources.Grid_Address_text
        ShowStatusToolStripMenuItem.Text = Global.Outworldz.My.Resources.Show_Status_word
        ShowUserDetailsToolStripMenuItem.Text = Global.Outworldz.My.Resources.Show_User_Details_word
        StartButton.Text = Global.Outworldz.My.Resources.Start_word
        StopButton.Text = Global.Outworldz.My.Resources.Stop_word
        TechnicalInfoToolStripMenuItem.Image = Global.Outworldz.My.Resources.document_dirty
        TechnicalInfoToolStripMenuItem.Text = Global.Outworldz.My.Resources.Help_Technical
        TechnicalInfoToolStripMenuItem.ToolTipText = Global.Outworldz.My.Resources.Help_Technical_text
        ThreadpoolsToolStripMenuItem.Text = Global.Outworldz.My.Resources.Thread_pools_word
        ToolStripMenuItem1.Image = Global.Outworldz.My.Resources.document_connection
        ToolStripMenuItem1.Text = Global.Outworldz.My.Resources.Help_Forward
        ToolStripMenuItem1.ToolTipText = Global.Outworldz.My.Resources.Help_Forward_text
        TroubleshootingToolStripMenuItem.Image = Global.Outworldz.My.Resources.document_view
        TroubleshootingToolStripMenuItem.Text = Global.Outworldz.My.Resources.Help_Troubleshooting_word
        UsersToolStripMenuItem.Text = Global.Outworldz.My.Resources.Users_word
        ViewGoogleMapToolStripMenuItem.Text = Global.Outworldz.My.Resources.ViewGoogleMap
        ViewLogsToolStripMenuItem.Image = Global.Outworldz.My.Resources.document_view
        ViewLogsToolStripMenuItem.Text = Global.Outworldz.My.Resources.View_Logs
        ViewVisitorMapsToolStripMenuItem.Text = Global.Outworldz.My.Resources.ViewVisitorMaps
        ViewWebUI.Image = Global.Outworldz.My.Resources.document_view
        ViewWebUI.Text = Global.Outworldz.My.Resources.View_Web_Interface
        ViewWebUI.ToolTipText = Global.Outworldz.My.Resources.View_Web_Interface_text
        Warn.Text = Global.Outworldz.My.Resources.Warn_word
        XengineToolStripMenuItem.Text = Global.Outworldz.My.Resources.XEngine_word
        mnuAbout.Image = Global.Outworldz.My.Resources.question_and_answer
        mnuAbout.Text = Global.Outworldz.My.Resources.About_word
        mnuExit.Image = Global.Outworldz.My.Resources.exit_icon
        mnuExit.Text = Global.Outworldz.My.Resources.Exit__word
        mnuHide.Image = Global.Outworldz.My.Resources.navigate_down
        mnuHide.Text = Global.Outworldz.My.Resources.Hide
        mnuHideAllways.Image = Global.Outworldz.My.Resources.navigate_down2
        mnuHideAllways.Text = Global.Outworldz.My.Resources.Hide_Allways_word
        mnuSettings.Text = Global.Outworldz.My.Resources.Setup_word
        mnuShow.Image = Global.Outworldz.My.Resources.navigate_up
        mnuShow.Text = Global.Outworldz.My.Resources.Show_word

        ' OAR AND IAR MENU
        SearchForObjectsMenuItem.Text = Global.Outworldz.My.Resources.Search_Events
        LoadInventoryIARToolStripMenuItem1.Text = Global.Outworldz.My.Resources.Load_Inventory_IAR
        SaveAllRunningRegiondsAsOARSToolStripMenuItem.Text = Global.Outworldz.My.Resources.Save_All_Regions
        LoadRegionOARToolStripMenuItem.Text = Global.Outworldz.My.Resources.Load_Region_OAR
        LoadLocalOARSToolStripMenuItem.Text = Global.Outworldz.My.Resources.OAR_load_save_backupp_word
        SaveInventoryIARToolStripMenuItem1.Text = Global.Outworldz.My.Resources.Save_Inventory_IAR_word
        SaveRegionOARToolStripMenuItem1.Text = Global.Outworldz.My.Resources.Save_Region_OAR_word

        'Search Help
        SearchHelpToolStripMenuItem.Text = Global.Outworldz.My.Resources.Search_Help

        ' show box styled nicely.
        Application.EnableVisualStyles()
        Buttons(BusyButton)

        If Settings.KeepOnTopMain Then
            Me.TopMost = True
            KeepOnTopToolStripMenuItem.Image = My.Resources.tables
            OnTopToolStripMenuItem.Image = My.Resources.table
        Else
            Me.TopMost = False
            KeepOnTopToolStripMenuItem.Image = My.Resources.table
            OnTopToolStripMenuItem.Image = My.Resources.tables
        End If

        TextBox1.BackColor = Me.BackColor

        TextBox1.SelectAll()
        TextBox1.SelectionIndent += 15 ' play With this values To match yours
        TextBox1.SelectionRightIndent += 15 ' this too
        TextBox1.SelectionLength = 0
        ' this Is a little hack because without this
        ' I've got the first line of my richTB selected anyway.
        TextBox1.SelectionBackColor = TextBox1.BackColor

        ' initialize the scrolling text box

        Adv1 = New FormSettings

        Me.Show()

        RunningBackupName.Clear()

        Dim v = Reflection.Assembly.GetExecutingAssembly().GetName().Version
        Dim buildDate = New DateTime(2000, 1, 1).AddDays(v.Build).AddSeconds(v.Revision * 2)
        Dim displayableVersion = $"{v} ({buildDate})"
        AssemblyV = "Assembly version " + displayableVersion
        TextPrint(AssemblyV)
        TextPrint($"{My.Resources.Version_word} {PropSimVersion}")
        Me.Text += " V" & PropMyVersion
        TextPrint($"DreamGrid {My.Resources.Version_word} {PropMyVersion}")

        UpgradeDotNet() ' one time run for Dot net prerequisites
        SetupPerl() ' Ditto
        SetupPerlModules() ' Perl Language interpreter
        Settings.DotnetUpgraded() = True

        TextPrint(My.Resources.Getting_regions_word)

        PropChangedRegionSettings = True

        CheckDefaultPorts()

        Init(True)  ' read all region data

        AddVoices() ' add eva and mark voices

        Application.DoEvents()

        ' Boot RAM Query
        Searcher1 = New ManagementObjectSearcher(wql)
        Application.DoEvents()

        CopyWifi() 'Make the two folders in Wifi and Wifi bin for Diva

        Cleanup() ' old files thread

        DeleteFile(IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Logs\Status.log"))

        'UPNP create if we need it
        PropMyUPnpMap = New UPnp()

        ' WebUI Menu
        ViewWebUI.Visible = Settings.WifiEnabled

        CheckForUpdates()
        Application.DoEvents()

        ' Get Opensimulator Scripts to date if needed
        If Settings.DeleteScriptsOnStartupLevel <> PropSimVersion Then
            WipeScripts(True)
            WipeBakes()
            WipeAssets()
            WipeMesh()
            TextPrint(My.Resources.All_Caches_Cleared_word)
            Settings.DeleteScriptsOnStartupLevel() = PropSimVersion ' we have scripts cleared to proper Opensim Version
        End If

        If Not IO.File.Exists(IO.Path.Combine(Settings.CurrentDirectory, "BareTail.udm")) Then
            CopyFileFast(IO.Path.Combine(Settings.CurrentDirectory, "BareTail.udm.bak"), IO.Path.Combine(Settings.CurrentDirectory, "BareTail.udm"))
        End If

        Using tmp As New ClassQuickedit
            tmp.SetQuickEditOff()
        End Using

        Using tmp As New ClassLoopback
            tmp.SetLoopback()
        End Using

        Application.DoEvents()

        ' collect all process windows
        Dim processes = Process.GetProcessesByName("Opensim")
        For Each p In processes
            If Not PropInstanceHandles.ContainsKey(p.Id) Then
                PropInstanceHandles.TryAdd(p.Id, p.MainWindowTitle)
            End If
        Next

        For Each RegionUUID In RegionUuids()
            If Not LogResults.ContainsKey(RegionUUID) Then LogResults.Add(RegionUUID, New LogReader(RegionUUID))
        Next

        'mnuShow shows the DOS box for Opensimulator
        Select Case Settings.ConsoleShow
            Case "True"
                mnuShow.Checked = True
                mnuHide.Checked = False
                mnuHideAllways.Checked = False
            Case "False"
                mnuShow.Checked = False
                mnuHide.Checked = True
                mnuHideAllways.Checked = False
            Case "None"
                mnuShow.Checked = False
                mnuHide.Checked = False
                mnuHideAllways.Checked = True
        End Select

        SkipSetup = False

        TextPrint(My.Resources.Setup_Network)
        SetPublicIP()
        SetServerType()

        If SetIniData() Then
            MsgBox("Failed to setup", MsgBoxStyle.Critical Or MsgBoxStyle.MsgBoxSetForeground, My.Resources.Error_word)
            Buttons(StartButton)
            TextPrint(My.Resources.Stopped_word)
            Return
        End If

        ' Boot Port 8001 Server
        TextPrint(My.Resources.Starting_WebServer_word)
        PropWebserver = NetServer.GetWebServer
        PropWebserver.StartServer(Settings.CurrentDirectory, Settings)
        Application.DoEvents()

        Sleep(100)
        If TestPrivateLoopback(True) Then
            ErrorLog("Diagnostic Listener port failed. Aborting")
            TextPrint("Diagnostic Listener port failed. Aborting")
            Return
        End If

        ' Run Diagnostics
        CheckDiagPort()

        Dim DB = IsMySqlRunning()

        If DB Then
            ' clear any temp regions on boot.
            For Each RegionUUID In RegionUuids()
                If Settings.TempRegion AndAlso EstateName(RegionUUID) = "SimSurround" Then
                    TextPrint($"{My.Resources.DeletingTempRegion} {Region_Name(RegionUUID)}")
                    DeleteAllRegionData(RegionUUID)
                    PropChangedRegionSettings = True
                End If
            Next
        End If

        With Cpu1
            .CategoryName = "Processor Information"
            .CounterName = "% Processor Utility"
            .InstanceName = "_Total"
        End With

        mnuSettings.Visible = True

        LoadHelp()      ' Help loads once
        FixUpdater()    ' replace DreamGridUpdater.exe with DreamGridUpdater.new

        If Settings.ShowMysqlStats Then
            OnToolStripMenuItem.Checked = True
            OffToolStripMenuItem.Checked = False
            MySQLSpeed.Text = ""
        Else
            OnToolStripMenuItem.Checked = False
            OffToolStripMenuItem.Checked = True
            MySQLSpeed.Text = ""
        End If

        If Settings.ShowRegionListOnBoot Then
            ShowRegionform()
        End If

        If Settings.Password = "secret" Or Settings.Password.Length = 0 Then
            Dim Password = New PassGen
            Settings.Password = Password.GeneratePass()
        End If

        TextPrint(My.Resources.RefreshingOAR)
        ContentOAR = New FormOAR
        ContentOAR.Init("OAR")
        TextPrint(My.Resources.RefreshingIAR)
        ContentIar = New FormOAR
        ContentIar.Init("IAR")

        Application.DoEvents()
        LoadLocalIAROAR() ' load IAR and OAR local content

        TextPrint(My.Resources.Setup_Ports_word)
        Application.DoEvents()
        Firewall.SetFirewall()

        ' Get the names of all the lands
        InitLand()
        InitTrees()

        HelpOnce("License") ' license on bottom
        HelpOnce("Startup")

        Joomla.CheckForjOpensimUpdate()

        ' Save a random machine ID - we don't want any data to be sent that's personal or identifiable, but it needs to be unique
        Randomize()
        If Settings.MachineId().Length = 0 Then Settings.MachineId() = RandomNumber.Random  ' a random machine ID may be generated.  Happens only once
        If Settings.APIKey().Length = 0 Then Settings.APIKey() = RandomNumber.Random  ' a random API Key may be generated.  Happens only once

        ' also turn on the lights for the other services.
        IsRobustRunning()
        IsApacheRunning()
        IsIceCastRunning()

        Settings.SaveSettings()

        OfflineIMEmailThread()  ' check for any offline emails.

        Dim n = Settings.DnsName
        If n.Length = 0 Then n = "(none)"
        TextPrint("--> WAN IP = " & Settings.WANIP)
        TextPrint("--> DNS = " & n)
        TextPrint("--> WAN = " & Settings.PublicIP)
        TextPrint("--> LAN IP = " & Settings.LANIP())
        TextPrint("--> Region IP= " & Settings.ExternalHostName)
        If Settings.ServerType = RobustServerName Then
            TextPrint("--> Login = " & "http://" & Settings.BaseHostName & ":" & Settings.HttpPort)
        ElseIf Settings.ServerType = RegionServerName Then
            TextPrint("--> Login = " & "http://" & Settings.BaseHostName & ":" & Settings.HttpPort)
        ElseIf Settings.ServerType = OsgridServer Then
            TextPrint("--> Login = " & "http://" & Settings.BaseHostName & ":80")
        ElseIf Settings.ServerType = MetroServer Then
            TextPrint("--> Login = " & "http://" & Settings.BaseHostName & ":80")
        End If

        Application.DoEvents() ' let timer run

        If Settings.Autostart Then
            TextPrint(My.Resources.Auto_Startup_word)
            Application.DoEvents()
            Startup()
        Else
            TextPrint(My.Resources.Ready_to_Launch & vbCrLf & "------------------" & vbCrLf & Global.Outworldz.My.Resources.Click_Start_2_Begin & vbCrLf)
            Buttons(StartButton)
        End If

        ToolBar(True)

    End Sub

    Private Sub Link_Clicked(ByVal sender As Object, ByVal e As System.Windows.Forms.LinkClickedEventArgs) Handles TextBox1.LinkClicked

        Try
            System.Diagnostics.Process.Start(e.LinkText)
        Catch
        End Try

    End Sub

#End Region

#Region "Exit Events"

    Public Shared Sub ProcessQuit()

        ' now look at the exit stack
        While Not ExitList.IsEmpty

            Dim GroupName = ExitList.Keys.First
            Dim Reason = ExitList.Item(GroupName) ' NoLogin or Exit

            TextPrint(GroupName & " " & Reason)
            Dim out As String = ""

            ' Need a region number and a Name. Name is either a region or a Group. For groups we need to get a region name from the group
            Dim GroupList As List(Of String) = RegionUuidListByName(GroupName)

            Dim PID As Integer
            Dim RegionUUID As String = ""
            If GroupList.Count > 0 Then
                RegionUUID = GroupList(0)

                PID = ProcessID(RegionUUID)
                ProcessID(RegionUUID) = 0
                If ProcessIdDict.ContainsKey(PID) Then
                    ProcessIdDict.Remove(PID)
                End If
                DelPidFile(RegionUUID) 'kill the disk PID

                If PropInstanceHandles.ContainsKey(PID) Then
                    PropInstanceHandles.TryRemove(PID, "")
                End If
            Else
                BreakPoint.Print("No UUID!")

                If PID > 0 Then
                    PropInstanceHandles.TryRemove(PID, "")
                End If

                Continue While
            End If

            If ToDoList.ContainsKey(RegionUUID) Then
                ToDoList.Remove(RegionUUID)
            End If

            If Reason = "NoLogin" Then
                RegionStatus(RegionUUID) = SIMSTATUSENUM.NoLogin
                PropUpdateView = True
                ExitList.TryRemove(RegionUUID, "")
                Continue While
            End If

            Dim Status = RegionStatus(RegionUUID)
            Dim RegionName = Region_Name(RegionUUID)

            BreakPoint.Print($"{RegionName} {GetStateString(Status)}")

            If Not RegionEnabled(RegionUUID) Then
                ExitList.TryRemove(GroupName, "")
                Continue While
            End If

            If Status = SIMSTATUSENUM.ShuttingDownForGood Then
                For Each UUID As String In RegionUuidListByName(GroupName)
                    RegionStatus(UUID) = SIMSTATUSENUM.Stopped
                Next

                If Settings.TempRegion AndAlso EstateName(RegionUUID) = "SimSurround" Then
                    DeleteAllRegionData(RegionUUID)
                End If

                PropUpdateView = True ' make form refresh
                ExitList.TryRemove(GroupName, "")
                Continue While

            ElseIf Status = SIMSTATUSENUM.RecyclingDown AndAlso Not PropAborting Then
                'RecyclingDown = 4

                TextPrint(GroupName & " " & Global.Outworldz.My.Resources.Restart_Queued_word)
                For Each R In GroupList
                    RegionStatus(R) = SIMSTATUSENUM.RestartStage2
                Next
                PropUpdateView = True
                ExitList.TryRemove(GroupName, "")
                Continue While

            ElseIf (Status = SIMSTATUSENUM.RecyclingUp Or
                Status = SIMSTATUSENUM.Booting Or
                Status = SIMSTATUSENUM.Booted Or
                Status = SIMSTATUSENUM.Suspended) And
                Not PropAborting Then

                ' Maybe we crashed during warm up or running. Skip prompt if auto restart on crash and restart the beast
                Status = SIMSTATUSENUM.Error
                PropUpdateView = True

                Logger("Crash", GroupName & " Crashed", "Status")
                If Settings.RestartOnCrash Then

                    If CrashCounter(RegionUUID) > 4 Then
                        Logger("Crash", $"{GroupName} Crashed 5 times", "Status")
                        TextPrint($"{GroupName} Crashed 5 times - Stopped with Errors")
                        ErrorGroup(GroupName)
                        RegionStatus(RegionUUID) = SIMSTATUSENUM.Error
                        Dim yesno = MsgBox(GroupName & " " & Global.Outworldz.My.Resources.Quit_unexpectedly & " " & Global.Outworldz.My.Resources.See_Log, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground Or MsgBoxStyle.Critical, Global.Outworldz.My.Resources.Error_word)
                        If (yesno = vbYes) Then
                            Baretail("""" & IO.Path.Combine(OpensimIniPath(RegionUUID), "Opensim.log") & """")
                        End If
                        ExitList.TryRemove(GroupName, "")
                        Continue While
                    End If

                    CrashCounter(RegionUUID) += 1

                    ' shut down all regions in the DOS box
                    TextPrint(GroupName & " " & Global.Outworldz.My.Resources.Quit_unexpectedly & " #" & CStr(CrashCounter(RegionUUID)))
                    StopGroup(GroupName)
                    TextPrint(GroupName & " " & Global.Outworldz.My.Resources.Restart_Queued_word)
                    For Each R In GroupList
                        RegionStatus(R) = SIMSTATUSENUM.RestartStage2
                    Next

                    ExitList.TryRemove(GroupName, "")
                    Continue While
                Else
                    If PropAborting Then
                        ExitList.TryRemove(GroupName, "")
                        Continue While
                    End If

                    TextPrint(GroupName & " " & Global.Outworldz.My.Resources.Quit_unexpectedly)
                    ErrorGroup(GroupName)

                    Dim yesno = MsgBox(GroupName & " " & Global.Outworldz.My.Resources.Quit_unexpectedly & " " & Global.Outworldz.My.Resources.See_Log, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground Or MsgBoxStyle.Critical, Global.Outworldz.My.Resources.Error_word)
                    If (yesno = vbYes) Then
                        Baretail("""" & IO.Path.Combine(OpensimIniPath(RegionUUID), "Opensim.log") & """")
                    End If
                End If
            Else
                StopGroup(GroupName)
            End If

            ExitList.TryRemove(GroupName, "")

        End While

    End Sub

    ''' <summary>Event handler for Icecast</summary>
    Public Sub IceCastExited(ByVal sender As Object, ByVal e As EventArgs)

        If PropAborting Then Return

        If Settings.RestartOnCrash AndAlso IcecastCrashCounter < 10 Then
            IcecastCrashCounter += 1
            PropIceCastExited = True
            Return
        End If
        IcecastCrashCounter = 0

        Dim yesno = MsgBox(My.Resources.Icecast_Exited, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Error_word)

        If (yesno = vbYes) Then
            Baretail("""" & IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Icecast\log\error.log") & """")
        End If

    End Sub

    Private Sub RestartDOSboxes()

        If PropRobustExited = True Then
            RobustIcon(False)
        End If

        If PropMysqlExited Then
            MySQLIcon(False)
        End If

        If PropApacheExited Then
            ApacheIcon(False)
        End If

        If PropIceCastExited Then
            IceCastIcon(False)
        End If

    End Sub

#End Region

#Region "Buttons"

    Public Sub Buttons(b As Control)

        If b Is Nothing Then Return
        ' Turns off all 3 stacked buttons, then enables one of them
        BusyButton.Visible = False
        StopButton.Visible = False
        StartButton.Visible = False

        b.Visible = True

    End Sub

#End Region

#Region "Scanner"

#Region "Booting"

    Public Sub RestartAllRegions()

        PropOpensimIsRunning() = True
        If Not StartMySQL() Then Return
        If Not StartRobust() Then Return

        StartTimer()

        For Each RegionUUID In RegionUuids()

            If PropAborting Then
                Return
            End If

            If Not RegionEnabled(RegionUUID) Then
                Continue For
            End If

            Dim GroupName = Group_Name(RegionUUID)
            Dim Status = RegionStatus(RegionUUID)

            If AvatarsIsInGroup(GroupName) Then
                TextPrint($"{My.Resources.Avatars_are_in} {GroupName}")
                Continue For
            End If

            If (Status = SIMSTATUSENUM.Booting Or
                Status = SIMSTATUSENUM.Booted Or
                Status = SIMSTATUSENUM.Suspended) Then

                ShowDOSWindow(RegionUUID, MaybeShowWindow())
                ShutDown(RegionUUID, SIMSTATUSENUM.RecyclingDown)
            End If
            Application.DoEvents()
        Next

    End Sub

#End Region

#Region "Start/Stop"

    ''' <summary>Startup() Starts opensimulator system Called by Start Button or by AutoStart</summary>
    Public Sub Startup()

        Buttons(BusyButton)

        Dim DefaultName As String = ""

        Dim RegionUUID As String = FindRegionByName(Settings.WelcomeRegion)
        If RegionUUID.Length = 0 AndAlso Settings.ServerType = RobustServerName Then
            MsgBox(My.Resources.Default_Welcome, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground Or MsgBoxStyle.Question, My.Resources.Information_word)
            TextPrint(My.Resources.Stopped_word)

#Disable Warning CA2000 ' Dispose objects before losing scope
            Dim FormRegions = New FormRegions
#Enable Warning CA2000 ' Dispose objects before losing scope
            FormRegions.Activate()
            FormRegions.Select()
            FormRegions.Visible = True
            FormRegions.BringToFront()
            Buttons(StartButton)
            Return
        End If

        TextPrint(My.Resources.Starting_word)

        PropAborting = False  ' suppress exit warning messages

        If Settings.Language.Length = 0 Then
            Settings.Language = "en-US"
        End If

        My.Application.ChangeUICulture(Settings.Language)
        My.Application.ChangeCulture(Settings.Language)

        If Settings.AutoBackup Then
            ' add 30 minutes to allow time to auto backup andalso then restart
            Dim BTime As Integer = CInt("0" & Settings.AutobackupInterval)
            If Settings.AutoRestartInterval > 0 AndAlso Settings.AutoRestartInterval < BTime Then
                Settings.AutoRestartInterval = BTime + 30
                TextPrint($"{My.Resources.AutorestartTime} {CStr(BTime)} + 30 min.")
            End If
        End If

        If SetIniData() Then
            MsgBox("Failed to setup", MsgBoxStyle.Critical Or MsgBoxStyle.MsgBoxSetForeground Or MsgBoxStyle.Critical, My.Resources.Error_word)
            Buttons(StartButton)
            TextPrint(My.Resources.Stopped_word)
            Return
        End If

        If Not StartMySQL() Then
            ToolBar(False)
            Buttons(StartButton)
            TextPrint(My.Resources.Stopped_word)
            Return
        End If

        If Not StartRobust() Then
            Buttons(StartButton)
            TextPrint(My.Resources.Stopped_word)
            Return
        End If

        ' clear any temp regions on boot.
        For Each RegionUUID In RegionUuids()
            If Settings.TempRegion AndAlso EstateName(RegionUUID) = "SimSurround" Then
                TextPrint($"{My.Resources.DeletingTempRegion} {Region_Name(RegionUUID)}")
                DeleteAllRegionData(RegionUUID)
                PropChangedRegionSettings = True
            End If
        Next

        ' create tables in case we need them
        SetupWordPress()    ' in case they want to use WordPress
        SetupSimStats()     ' Perl code
        SetupLocalSearch()  ' local search database
        SetupTOSTable()     ' local TOS table in Robust
        StartApache()
        StartIcecast()
        UploadPhoto()
        SetBirdsOnOrOff()

        If Not Settings.RunOnce AndAlso Settings.ServerType = RobustServerName Then
            Using InitialSetup As New FormInitialSetup ' form for use and password
                Dim ret = InitialSetup.ShowDialog()
                If ret = DialogResult.Cancel Then
                    Buttons(StartButton)
                    TextPrint(My.Resources.Stopped_word)
                    Return
                End If
                ' Read the chosen sim name
                ConsoleCommand(RobustName, $"create user {InitialSetup.FirstName} {InitialSetup.LastName} {InitialSetup.Password} {InitialSetup.Email}{vbCrLf}{vbCrLf}")
                Settings.RunOnce = True
                Settings.SaveSettings()
            End Using
        Else
            ForceBackupOnce()
        End If

        TextPrint($"{My.Resources.Grid_Address_is_word} http://{Settings.BaseHostName}:{Settings.HttpPort}")

        ' Launch the rockets
        TextPrint(My.Resources.Start_Regions_word)

        If Not StartOpensimulator() Then
            Buttons(StartButton)
            TextPrint(My.Resources.Stopped_word)
            Return
        End If

        Buttons(StopButton)
        TextPrint(My.Resources.Finished_word)
        ' done with boot up

    End Sub

    Private Shared Sub ForceBackupOnce()
        'once and only once, do a backup
        If Not Settings.DoSQLBackup Then
            Using Backup As New Backups
                Backup.SqlBackup()
            End Using
            Settings.DoSQLBackup = True
        End If
    End Sub

    Private Sub ReallyQuit()

        If Not KillAll() Then Return

        Try
            If cpu IsNot Nothing Then cpu.Dispose()
        Catch
        End Try
        Try
            If Searcher1 IsNot Nothing Then Searcher1.Dispose()
        Catch
        End Try
        Try
            If Graphs IsNot Nothing Then Graphs.Dispose()
        Catch
        End Try

        If PropWebserver IsNot Nothing Then PropWebserver.StopWebserver()

        PropAborting = True
        StopMysql()

        Settings.SaveSettings()

        TextPrint("Zzzz...")
        Thread.Sleep(1000)
        End

    End Sub

#End Region

#Region "Subs"

    Public Sub ToolBar(visible As Boolean)

        AvatarLabel.Visible = visible
        PercentCPU.Visible = visible
        PercentRAM.Visible = visible
        DiskSize.Visible = visible
        MySQLSpeed.Visible = visible

    End Sub

    Private Shared Sub AddorUpdateVisitor(Avatar As String, RegionName As String)

        If Not Visitor.ContainsKey(Avatar) Then
            Visitor.Add(Avatar, RegionName)
        Else
            Visitor.Item(Avatar) = RegionName
        End If

    End Sub

    Private Shared Sub ClearAllRegions()

        For Each RegionUUID In RegionUuids()
            If Settings.TempRegion AndAlso EstateName(RegionUUID) = "SimSurround" Then
                DeleteAllRegionData(RegionUUID)
                PropChangedRegionSettings = True
            End If

            RegionStatus(RegionUUID) = SIMSTATUSENUM.Stopped
            ProcessID(RegionUUID) = 0
            DelPidFile(RegionUUID)
        Next

        Try
            ExitList.Clear()

            ClearStack()
            PropInstanceHandles.Clear()
            WebserverList.Clear()
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

    End Sub

    Private Shared Sub Create_ShortCut(ByVal sTargetPath As String)
        ' Requires reference to Windows Script Host Object Model
        Dim WshShell = New WshShellClass
        Dim MyShortcut As IWshShortcut
        ' The shortcut will be created on the desktop
        Dim DesktopFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
        MyShortcut = CType(WshShell.CreateShortcut(DesktopFolder & "\Outworldz.lnk"), IWshShortcut)
        MyShortcut.TargetPath = sTargetPath
        MyShortcut.IconLocation = WshShell.ExpandEnvironmentStrings(CurDir() & "\Start.exe")
        MyShortcut.WorkingDirectory = CurDir()
        MyShortcut.Save()

    End Sub

    Private Shared Sub RunParser()

        If Settings.SearchOptions <> "Local" Then Return
        Using Parser As New Process
            Parser.StartInfo.UseShellExecute = True ' so we can redirect streams
            Parser.StartInfo.WorkingDirectory = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\PHP7\")
            Parser.StartInfo.FileName = "php.exe"
            Parser.StartInfo.Arguments = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Apache\htdocs\Search\parser.bat")
            If Debugger.IsAttached Then
                Parser.StartInfo.CreateNoWindow = False
                Parser.StartInfo.WindowStyle = ProcessWindowStyle.Normal
            Else
                Parser.StartInfo.CreateNoWindow = True
                Parser.StartInfo.WindowStyle = ProcessWindowStyle.Minimized
            End If

            Try
                Parser.Start()
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
        End Using

    End Sub

    Private Sub AddLog(name As String)
        Dim LogMenu As New ToolStripMenuItem With {
                .Text = name,
                .ToolTipText = Global.Outworldz.My.Resources.Click_to_View_this_word,
                .Size = New Size(269, 26),
                .Image = Global.Outworldz.My.Resources.document_view,
                .DisplayStyle = ToolStripItemDisplayStyle.Text
            }
        AddHandler LogMenu.Click, New EventHandler(AddressOf LogViewClick)
        ViewLogsToolStripMenuItem.Visible = True
        ViewLogsToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {LogMenu})

    End Sub

    Private Sub Chart()

        ' Graph https://github.com/sinairv/MSChartWrapper
        Try
            ' running average
            speed3 = speed2
            speed2 = speed1
            speed1 = speed
            Try
                speed = Me.Cpu1.NextValue()
                If speed > 100 Then speed = 100
            Catch ex As Exception
                BreakPoint.Dump(ex)
                If Not Settings.CpuPatched Then
                    Dim pUpdate = New Process()
                    Dim pi = New ProcessStartInfo With {
                        .Arguments = "/ R",
                        .FileName = "loadctr"
                    }
                    pUpdate.StartInfo = pi
                    pUpdate.Start()
                    pUpdate.WaitForExit()
                    pUpdate.Dispose()
                    Settings.CpuPatched = True
                End If

            End Try

            CPUAverageSpeed = (speed + speed1 + speed2 + speed3) / 4
            If CPUAverageSpeed > 100 Then
                CPUAverageSpeed = 100
            End If
            MyCPUCollection.Add(speed)

            If MyCPUCollection.Count > 180 Then MyCPUCollection.RemoveAt(0)

            PercentCPU.Text = $"CPU {speed / 100:P1}"
        Catch ex As Exception
            ErrorLog(ex.Message)
        End Try
        'RAM

        Try
            Dim results As ManagementObjectCollection = Searcher1.Get()
            For Each result In results
                Dim d As Double
                Dim f As Double
                Dim r As Double
                Dim v As Double
                Try
                    d = Convert.ToDouble(result("TotalVisibleMemorySize"), CultureInfo.InvariantCulture)
                    f = Convert.ToDouble(result("FreePhysicalMemory"), CultureInfo.InvariantCulture)
                    r = (d - f) / d * 100
                    d = Convert.ToDouble(result("TotalVirtualMemorySize"), CultureInfo.InvariantCulture)
                    f = Convert.ToDouble(result("FreeVirtualMemory"), CultureInfo.InvariantCulture)
                    v = (d - f) / 1024 / 1024
                Catch
                End Try

                MyRAMCollection.Add(r)
                If MyRAMCollection.Count > 180 Then MyRAMCollection.RemoveAt(0)
                r = Math.Round(r)
                v = Math.Round(v)
                Settings.Ramused = r
                PercentRAM.Text = $"{r / 100:p1} RAM"
                Virtual.Text = $"{v} MB VRAM"
            Next
            results.Dispose()
        Catch ex As Exception
            ErrorLog(ex.Message)
        End Try

    End Sub

    Private Sub G()

        Graphs.Close()
        Graphs.Dispose()
#Disable Warning CA2000 ' Dispose objects before losing scope
        Graphs = New FormGraphs With {
            .Visible = True
        }
#Enable Warning CA2000 ' Dispose objects before losing scope
        Graphs.Activate()
        Graphs.Select()
        Graphs.BringToFront()

    End Sub

    Private Sub LoadHelp()

        ' read help files for menu
        TextPrint(My.Resources.LoadHelp)
        Dim folders As Array = Nothing
        Try
            folders = Directory.GetFiles(IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Help"))
            For Each aline As String In folders
                If aline.EndsWith(".htm", StringComparison.OrdinalIgnoreCase) Then
                    aline = System.IO.Path.GetFileNameWithoutExtension(aline)
                    Dim HelpMenu As New ToolStripMenuItem With {
                    .Text = aline,
                    .ToolTipText = Global.Outworldz.My.Resources.Click_to_load,
                    .DisplayStyle = ToolStripItemDisplayStyle.Text,
                    .Image = Global.Outworldz.My.Resources.question_and_answer
                }
                    AddHandler HelpMenu.Click, New EventHandler(AddressOf HelpClick)
                    HelpOnSettingsToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {HelpMenu})
                End If
            Next
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

        AddLog("All Logs")
        AddLog("Robust")
        AddLog("Error")
        AddLog("Outworldz")
        AddLog("Icecast")
        AddLog("MySQL")
        AddLog("All Settings")
        AddLog("--- Regions ---")

        For Each RegionUUID In RegionUuids()
            Dim Name = Region_Name(RegionUUID)
            AddLog("Region " & Name)
        Next

    End Sub

    Private Sub LoadLocalIAROAR()

        ''' <summary>Loads OAR and IAR to the menu</summary>
        ''' <remarks>Handles both the IAR/OAR and Autobackup folders</remarks>

        LoadLocalOARToolStripMenuItem.DropDownItems.Clear()
        Dim MaxFileNum As Integer = 25
        Dim counter = MaxFileNum
        Dim Filename = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\OAR\")
        Dim OARs As Array = Nothing
        Try
            OARs = Directory.GetFiles(Filename, "*.OAR", SearchOption.TopDirectoryOnly)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

        Try
            For Each OAR As String In OARs
                counter -= 1

                If counter > 0 Then
                    Dim Name = Path.GetFileName(OAR)
                    Dim OarMenu As New ToolStripMenuItem With {
                        .Text = Name,
                        .ToolTipText = Global.Outworldz.My.Resources.Click_to_load,
                        .DisplayStyle = ToolStripItemDisplayStyle.Text,
                        .Image = My.Resources.box_new
                    }
                    AddHandler OarMenu.Click, New EventHandler(AddressOf LocalOarClick)
                    LoadLocalOARToolStripMenuItem.Visible = True
                    LoadLocalOARToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {OarMenu})
                End If

            Next
        Catch
        End Try

        Dim AutoOARs As Array = Nothing
        Try
            AutoOARs = Directory.GetFiles(BackupPath(), "*.OAR", SearchOption.TopDirectoryOnly)

            counter = MaxFileNum

            If AutoOARs IsNot Nothing Then
                For Each OAR As String In AutoOARs
                    counter -= 1
                    If counter > 0 Then
                        Dim Name = Path.GetFileName(OAR)
                        Dim OarMenu As New ToolStripMenuItem With {
                            .Text = Name,
                            .ToolTipText = Global.Outworldz.My.Resources.Click_to_load,
                            .DisplayStyle = ToolStripItemDisplayStyle.Text,
                            .Image = My.Resources.box_new
                        }
                        AddHandler OarMenu.Click, New EventHandler(AddressOf LoadOarClick)
                        LoadLocalOARToolStripMenuItem.Visible = True
                        LoadLocalOARToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {OarMenu})
                    End If
                Next
            End If
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

        Dim IARs As Array = Nothing
        ' now for the IARs
        Try
            Filename = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\IAR\")
            IARs = Directory.GetFiles(Filename, "*.IAR", SearchOption.TopDirectoryOnly)

            LoadLocalIARToolStripMenuItem.DropDownItems.Clear()
            counter = MaxFileNum
            For Each IAR As String In IARs
                counter -= 1
                If counter > 0 Then
                    Dim Name = Path.GetFileName(IAR)
                    Dim IarMenu As New ToolStripMenuItem With {
                        .Text = Name,
                        .ToolTipText = Global.Outworldz.My.Resources.Click_to_load,
                        .DisplayStyle = ToolStripItemDisplayStyle.Text,
                        .Image = My.Resources.box_new
                    }
                    AddHandler IarMenu.Click, New EventHandler(AddressOf LocalIarClick)
                    LoadLocalIARToolStripMenuItem.Visible = True
                    LoadLocalIARToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {IarMenu})

                End If
            Next
        Catch
        End Try

        Dim AutoIARs As Array = Nothing
        Try
            AutoIARs = Directory.GetFiles(BackupPath, "*.IAR", SearchOption.TopDirectoryOnly)

            If AutoIARs IsNot Nothing Then
                counter = MaxFileNum
                For Each IAR As String In AutoIARs
                    counter -= 1
                    If counter > 0 Then
                        Dim Name = Path.GetFileName(IAR)
                        Dim IarMenu As New ToolStripMenuItem With {
                            .Text = Name,
                            .ToolTipText = Global.Outworldz.My.Resources.Click_to_load,
                            .DisplayStyle = ToolStripItemDisplayStyle.Text
                        }
                        AddHandler IarMenu.Click, New EventHandler(AddressOf LoadIarClick)
                        LoadLocalIARToolStripMenuItem.Visible = True
                        LoadLocalIARToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {IarMenu})
                    End If

                Next
            End If
        Catch
        End Try

    End Sub

    Private Sub LoadOarClick(sender As Object, e As EventArgs) ' event handler

        Dim File As String = IO.Path.Combine(BackupPath, CStr(sender.Text)) 'make a real URL
        LoadOARContent(File)
        TextPrint($"{My.Resources.Opensimulator_is_loading} {CStr(sender.Text)}. {Global.Outworldz.My.Resources.Take_time}")

    End Sub

    Private Function ScanAgents() As Integer

        If Not MysqlInterface.IsMySqlRunning() Then Return 0
        Dim total As Integer
        Try

            Dim UUID2Name As New Dictionary(Of String, String)
            GetAllAgents()  ' to list (of Cachedavatars)

            ' start with zero avatars
            For Each RegionUUID In RegionUuids()
                AvatarCount(RegionUUID) = 0
            Next

            For Each AgentObject In CachedAvatars
                Dim Avatar = AgentObject.AgentName
                Dim AvatarKey = AgentObject.AvatarUUID
                UUID2Name.Add(AvatarKey, Avatar) ' stash this, we need it later
                Dim RegionUUID = AgentObject.RegionID
                If RegionUUID = "00000000-0000-0000-0000-000000000000" Then
                    Continue For
                End If
                Dim RegionName = Region_Name(RegionUUID)
                ' could be a tainted region UUID leading to a crash
                If RegionName.Length = 0 Then Continue For

                ' not seen before
                If Not CurrentLocation.ContainsKey(AvatarKey) Then
                    TextPrint($"{Avatar} {My.Resources.Arriving_word} {RegionName}")

                    Dim UUID = System.Guid.NewGuid.ToString

                    Dim URL = $"http://{Settings.PublicIP}:{Settings.DiagnosticPort}/TOS?uid={UUID}"
                    Dim Fname As String = ""
                    Dim Lname As String = ""
                    Dim pattern As New Regex("^(.*?) (.*?)$")
                    Dim match As Match = pattern.Match(Avatar)
                    If match.Success Then
                        Fname = match.Groups(1).Value
                        Lname = match.Groups(2).Value
                    End If

                    SpeechList.Enqueue($"{Avatar} {My.Resources.Arriving_word} {RegionName}")
                    CurrentLocation.Add(AvatarKey, RegionName)
                    AvatarCount(RegionUUID) += 1
                    AddorUpdateVisitor(Avatar, RegionName)
                    PropUpdateView = True

                    If Not IsTOSAccepted(AgentObject.AvatarUUID, Fname, Lname, UUID) Then
                        RPC_admin_dialog(AgentObject.AvatarUUID, $"{My.Resources.AgreeTOS}{vbCrLf}{URL}")
                    End If

                End If

                If Not CurrentLocation.Item(AvatarKey) = RegionName Then
                    TextPrint($"{Avatar} {My.Resources.Arriving_word} {RegionName}")
                    SpeechList.Enqueue($"{Avatar} {My.Resources.Arriving_word} {RegionName}")
                    CurrentLocation.Item(AvatarKey) = RegionName
                    AvatarCount(RegionUUID) += 1
                    PropUpdateView = True
                    AddorUpdateVisitor(Avatar, RegionName)
                Else
                    Try
                        AvatarCount(RegionUUID) += 1
                    Catch
                    End Try

                End If

            Next

            ' remove anyone who has left for good

            Dim Remove As New List(Of String)
            For Each NameValue In CurrentLocation
                Dim AvatarKey = NameValue.Key
                Dim RegionName = NameValue.Value
                Dim exists As Boolean
                For Each o In CachedAvatars
                    Diagnostics.Debug.Print(o.AgentName)
                    If o.AvatarUUID = AvatarKey Then
                        exists = True
                    End If
                Next
                If Not exists Then
                    TextPrint($"{UUID2Name.Item(AvatarKey)} {My.Resources.leaving_word} {RegionName}")
                    SpeechList.Enqueue($"{UUID2Name.Item(AvatarKey)} {My.Resources.leaving_word} {RegionName}")
                    Remove.Add(AvatarKey)
                    PropUpdateView = True
                End If
            Next

            For Each Avi In Remove
                CurrentLocation.Remove(Avi)
                If Visitor.ContainsKey(Avi) Then
                    Visitor.Remove(Avi)
                End If
            Next

            total = CachedAvatars.Count
            AvatarLabel.Text = $"{CStr(total)} {My.Resources.Avatars_word}"

            If CachedAvatars IsNot Nothing AndAlso CachedAvatars.Count > 0 Then
                BuildLand(CachedAvatars)
            End If
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

        Return total

    End Function

    Private Sub ScriptsResumeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ScriptsResumeToolStripMenuItem.Click
        SendScriptCmd("scripts resume")
    End Sub

    Private Sub ScriptsStartToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ScriptsStartToolStripMenuItem.Click
        SendScriptCmd("scripts start")
    End Sub

    Private Sub ScriptsStopToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ScriptsStopToolStripMenuItem.Click
        SendScriptCmd("scripts stop")
    End Sub

    Private Sub ScriptsSuspendToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ScriptsSuspendToolStripMenuItem.Click
        SendScriptCmd("scripts suspend")
    End Sub

    Private Sub SearchForOarsAtOutworldzToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SearchForOarsAtOutworldzToolStripMenuItem.Click

        Dim webAddress As String = PropHttpsDomain & "/Outworldz_Installer/OAR"
        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

    End Sub

    Private Sub SearchToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SearchForObjectsMenuItem.Click

        Dim webAddress As String = IO.Path.Combine(PropHttpsDomain, "Search/")
        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

    End Sub

    Private Sub SeePortsInUseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SeePortsInUseToolStripMenuItem.Click

        Using CPortsProcess As New Process
            CPortsProcess.StartInfo.UseShellExecute = True
            CPortsProcess.StartInfo.FileName = IO.Path.Combine(Settings.CurrentDirectory, "Cports.exe")
            CPortsProcess.StartInfo.CreateNoWindow = False
            CPortsProcess.StartInfo.Arguments = ""
            CPortsProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal
            Try
                CPortsProcess.Start()
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
        End Using

    End Sub

    ''' <summary>Sets H,W and pos of screen on load</summary>
    Private Sub SetScreen()
        '351, 200 default
        ScreenPosition1 = New ClassScreenpos("Form1")
        AddHandler ResizeEnd, HandlerSetup
        Dim xy As List(Of Integer) = ScreenPosition1.GetXY()
        Left = xy.Item(0)
        Top = xy.Item(1)

        Dim hw As List(Of Integer) = ScreenPosition1.GetHW()

        If hw.Item(0) = 0 Then
            Me.Height = 200
        Else
            Try
                Me.Height = hw.Item(0)
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
        End If

        If hw.Item(1) = 0 Then
            Me.Width = 351
        Else
            Me.Width = hw.Item(1)
        End If

        ScreenPosition1.SaveHW(Me.Height, Me.Width)

    End Sub

#End Region

#Region "Globals"

#End Region

#Region "Events"

#End Region

#Region "Public Properties"

#End Region

#Region "Public Function"

#End Region

#Region "ExitList"

#End Region

#Region "Misc"

#End Region

    Private Shared Sub PrintBackups()

        For Each k In RunningBackupName
            TextPrint(k.Key)
            RunningBackupName.TryRemove(k.Key, "")
        Next

    End Sub

    Private Sub ShowRegionform()

        Try
            If PropRegionForm IsNot Nothing Then
                PropRegionForm.Close()
                PropRegionForm.Dispose()
            End If
        Catch
        End Try

        Try
            PropRegionForm = New FormRegionlist
            PropRegionForm.Show()
            PropRegionForm.Activate()
            PropRegionForm.Select()
            PropRegionForm.BringToFront()
            PropRegionForm.Go()
        Catch
        End Try

    End Sub

#End Region

#Region "Timers"

    Public Sub StartTimer()

        TimerMain.Interval = 1000
        TimerMain.Start() 'Timer starts functioning
        PropOpensimIsRunning() = True

    End Sub

    ''' <summary>
    ''' Timer runs every second looks for web server stuff that arrives, restarts any sims , updates lists of agents builds teleports.html for older teleport checks for crashed regions
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As EventArgs) Handles TimerMain.Tick

        If Not PropOpensimIsRunning() Then
            Return
        End If

        If TimerisBusy > 0 And TimerisBusy < 30 Then
            TimerisBusy += 1
            Return
        Else
            TimerisBusy = 0
        End If

        TimerisBusy += 1

        Chart()                     ' do charts collection

        CheckPost()                 ' see if anything arrived in the web server
        CheckForBootedRegions()     ' task to scan for anything that just came online
        TeleportAgents()            ' send them onward
        ProcessQuit()               ' check if any processes exited
        PrintBackups()              ' print if backups are running

        Chat2Speech()               ' speak of the devil
        RestartDOSboxes()           ' Icons for failed Services

        If SecondsTicker Mod 5 = 0 AndAlso SecondsTicker > 0 Then
            Bench.Start("5 second + worker")

            ScanAgents()                ' update agent count

            CalcDiskFree()              ' check for free disk space

            If Settings.ShowMysqlStats Then
                MySQLSpeed.Text = (MysqlStats() / 5).ToString("0.0", Globalization.CultureInfo.CurrentCulture) & " Q/S"
            Else
                MySQLSpeed.Text = ""
            End If
            Bench.Print("5 second + worker")
        End If

        If SecondsTicker = 60 Then
            Bench.Start("60 second worker")
            DeleteOldWave()         ' clean up TTS cache
            RegionListHTML("Name")  ' create HTML for old teleport boards
            DeleteDirectoryTmp()    ' clean up old tmp folder
            MakeMaps()              ' Make all the large maps
            Bench.Print("60 second worker")
        End If

        If SecondsTicker Mod 60 = 0 AndAlso SecondsTicker > 0 Then
            Bench.Start("60 second + worker")
            NewUserTimeout()        ' see if a new users has read and agreed to the tos
            DeleteOldWave()         ' clean up TTS cache
            ScanOpenSimWorld(False) ' do not force an update unless avatar count changes
            RegionListHTML("Name") ' create HTML for old teleport boards
            VisitorCount()         ' For the large maps
            Bench.Print("60 second + worker")
        End If

        ' Run Search and events once at 5 minute mark
        If SecondsTicker = 300 Then
            Bench.Start("300 second worker")
            BackupThread.RunTimedBackups() ' run background thread once and also expire old backups
            RunParser()     ' PHP parse for Publicity
            GetEvents()     ' fetch events from Outworldz
            ScanOpenSimWorld(True)
            Bench.Print("300 second worker")
        End If

        If SecondsTicker Mod 300 = 0 AndAlso SecondsTicker > 0 Then
            Bench.Start("300 second + worker")
            BackupThread.RunTimedBackups() ' run background right now, assuming its been long enough
            If TestPrivateLoopback(False) Then
                ErrorLog("Diagnostic Listener port failed")
                TextPrint("Diagnostic Listener port failed")
            End If
            Bench.Print("300 second + worker")
        End If

        ' half hour
        If SecondsTicker Mod 1800 = 0 AndAlso SecondsTicker > 0 Then
            Bench.Start("Half hour worker")
            ScanOpenSimWorld(True)
            GetEvents()             ' fetch events from Outworldz
            RunParser()             ' PHP parse for Publicity
            MakeMaps()              ' Make all the large maps
            Bench.Print("Half hour worker")
        End If

        ' print hourly marks on console
        If SecondsTicker Mod 3600 = 0 AndAlso SecondsTicker > 0 Then
            Bench.Start("Hour worker")
            TextPrint($"{Global.Outworldz.My.Resources.Running_word} {CInt((SecondsTicker / 3600)).ToString(Globalization.CultureInfo.InvariantCulture)} {Global.Outworldz.My.Resources.Hours_word}")
            SetPublicIP()           ' Adjust to any IP changes
            ExpireLogsByAge()       ' clean up old logs
            DeleteOldVisitors()     ' can be pretty old

            ' Dynamically adjust Mysql for size of DB
            ' set mysql for amount of buffer to use now that it running.
            ' Will take effect next time Mysql is started.
            Settings.Total_InnoDB_GBytes = Total_InnoDB_Bytes()
            Bench.Print("Hour worker")
        End If

        ' Only runs once
        If SecondsTicker = 3600 Then
            ExportFsAssets()
        End If

        SecondsTicker += 1

        TimerisBusy = 0

    End Sub

#End Region

#Region "Clicks"

    Private Shared Sub CheckDiagPort()

        TextPrint(My.Resources.Check_Diag)
        Dim wsstarted = IsRegionReady(CType(Settings.DiagnosticPort, Integer))
        If wsstarted = False Then
            MsgBox($"{My.Resources.Diag_Port_word} {Settings.DiagnosticPort}  {Global.Outworldz.My.Resources.Diag_Broken}", MsgBoxStyle.Critical Or MsgBoxStyle.MsgBoxSetForeground, My.Resources.Error_word)
        End If

    End Sub

    Private Sub AddUserToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddUserToolStripMenuItem.Click

        ConsoleCommand(RobustName, "create user")

    End Sub

    Private Sub AdminUIToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ViewWebUI.Click

        If PropOpensimIsRunning() Then

            If Settings.ApacheEnable Then
                Dim webAddress As String = $"http://{Settings.LANIP}:{CStr(Settings.ApachePort)}"
                Try
                    Process.Start(webAddress)
                Catch ex As Exception
                    BreakPoint.Dump(ex)
                End Try
            Else
                Dim webAddress As String = $"http://{Settings.LANIP}:{CStr(Settings.HttpPort)}"
                Try
                    Process.Start(webAddress)
                Catch ex As Exception
                    BreakPoint.Dump(ex)
                End Try
                TextPrint($"{My.Resources.User_Name_word}:{Settings.AdminFirst} {Settings.AdminLast}")
                TextPrint($"{My.Resources.Password_word}:{Settings.Password}")
            End If
        Else
            If Settings.ApacheEnable Then
                Dim webAddress As String = $"http://{Settings.LANIP}:{CStr(Settings.ApachePort)}"
                Try
                    Process.Start(webAddress)
                Catch ex As Exception
                    BreakPoint.Dump(ex)
                End Try
            Else
                TextPrint(My.Resources.Not_Running)
            End If
        End If

    End Sub

    Private Sub AdvancedSettingsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AdvancedSettingsToolStripMenuItem.Click

        Try
            SkipSetup = False
            Adv1.Activate()
            Adv1.Visible = True
            Adv1.Select()
            Adv1.Init()
            Adv1.BringToFront()
        Catch
            SkipSetup = False
            Adv1 = New FormSettings
            Adv1.Activate()
            Adv1.Visible = True
            Adv1.Select()
            Adv1.Init()
            Adv1.BringToFront()

        End Try

    End Sub

    Private Sub AllUsersAllSimsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles JustOneRegionToolStripMenuItem.Click

        If Not PropOpensimIsRunning() Then
            TextPrint(My.Resources.Not_Running)
            Return
        End If
        Dim RegionName = ChooseRegion(True)
        If RegionName.Length > 0 Then
            Dim Message = InputBox(My.Resources.What_to_say_2_region)
            If Message.Length = 0 Then Return

            Dim RegionUUID As String = FindRegionByName(RegionName)
            If RegionUUID.Length > 0 Then
                SendMessage(RegionUUID, Message)
                SendAdminMessage(RegionUUID, Message)
            End If

        End If

    End Sub

    Private Sub BackupAllIARsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BackupAllIARsToolStripMenuItem.Click

        SaveIARTaskAll()

    End Sub

    Private Sub BackupCriticalFilesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BackupCriticalFilesToolStripMenuItem.Click

#Disable Warning CA2000 ' Dispose objects before losing scope
        Dim CriticalForm As New FormBackupBoxes
#Enable Warning CA2000 ' Dispose objects before losing scope

        CriticalForm.Activate()
        CriticalForm.Visible = True
        CriticalForm.Select()
        CriticalForm.BringToFront()

    End Sub

    Private Sub BackupDatabaseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BackupToolStripMenuItem1.Click

        Dim Log = IO.Path.Combine(Settings.CurrentDirectory, "Outworldzfiles\Mysql\bin\Mysqldump.log")
        DeleteFile(Log)
        Using Backup As New Backups
            Backup.SqlBackup()
        End Using

    End Sub

    Private Sub BusyButton_Click(sender As Object, e As EventArgs) Handles BusyButton.Click

        PropAborting = True

        PropUpdateView = True ' make form refresh

        PropOpensimIsRunning() = False
        TextPrint(My.Resources.Stopped_word)
        Buttons(StartButton)

    End Sub

    Private Sub ChangePasswordToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChangePasswordToolStripMenuItem.Click

        ConsoleCommand(RobustName, "reset user password")

    End Sub

    Private Sub CheckAndRepairDatbaseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CheckAndRepairDatbaseToolStripMenuItem.Click

        If Not StartMySQL() Then
            ToolBar(False)
            Buttons(StartButton)
            TextPrint(My.Resources.Stopped_word)
            Return
        End If

        Dim pi = New ProcessStartInfo()

        ChDir(IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\mysql\bin"))
        pi.WindowStyle = ProcessWindowStyle.Normal
        pi.Arguments = CStr(Settings.MySqlRobustDBPort)
        If Settings.RootMysqlPassword.Length > 0 Then
            pi.Arguments += $" {Settings.RootMysqlPassword}"
        End If

        pi.FileName = "CheckAndRepair.bat"
        Using pMySqlDiag1 = New Process With {
        .StartInfo = pi
    }
            Try
                pMySqlDiag1.Start()
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
            pMySqlDiag1.WaitForExit()
        End Using

        ChDir(Settings.CurrentDirectory)

    End Sub

    Private Sub CHeckForUpdatesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CHeckForUpdatesToolStripMenuItem.Click

        ShowUpdateForm()

    End Sub

    Private Sub ChooseAUserToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChooseAUserToolStripMenuItem.Click

        Dim User = InputBox("Enter a User Name", "UserName", "everyone")
        RunCheck(User)

    End Sub

    Private Sub ClothingInventoryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClothingInventoryToolStripMenuItem.Click

        ContentIar.Activate()
        ContentIar.ShowForm()
        ContentIar.Select()
        ContentIar.BringToFront()

    End Sub

    Private Sub ConnectToConsoleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConnectToConsoleToolStripMenuItemMySQL.Click
        MysqlConsole()
    End Sub

    Private Sub ConnectToIceCastToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConnectToIceCastToolStripMenuItemIcecast.Click

        If Settings.SCEnable Then
            Dim webAddress As String = "http://127.0.0.1:" & Convert.ToString(Settings.SCPortBase, Globalization.CultureInfo.InvariantCulture)
            Try
                Process.Start(webAddress)
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
        End If

    End Sub

    Private Sub ConnectToWebPageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConnectToWebPageToolStripMenuItemApache.Click

        If PropOpensimIsRunning() Then
            If Settings.ApacheEnable Then
                Dim webAddress As String = "http://127.0.0.1:" & Convert.ToString(Settings.ApachePort, Globalization.CultureInfo.InvariantCulture)
                Try
                    Process.Start(webAddress)
                Catch ex As Exception
                    BreakPoint.Dump(ex)
                End Try
            Else
                Dim webAddress As String = "http://127.0.0.1:" & Settings.HttpPort
                Try
                    Process.Start(webAddress)
                Catch ex As Exception
                    BreakPoint.Dump(ex)
                End Try
                TextPrint($"{My.Resources.User_Name_word}{Settings.AdminFirst} {Settings.AdminLast}")
                TextPrint($"{My.Resources.Password_word}{Settings.Password}")
            End If
        Else
            If Settings.ApacheEnable Then
                Dim webAddress As String = "http://127.0.0.1:" & Convert.ToString(Settings.ApachePort, Globalization.CultureInfo.InvariantCulture)
                Try
                    Process.Start(webAddress)
                Catch ex As Exception
                    BreakPoint.Dump(ex)
                End Try
            Else
                TextPrint(My.Resources.Not_Running)
            End If
        End If
    End Sub

    Private Sub ConsoleCOmmandsToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ConsoleCOmmandsToolStripMenuItem1.Click
        Dim webAddress As String = "http://opensimulator.org/wiki/Server_Commands"
        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try
    End Sub

    Private Sub Debug_Click(sender As Object, e As EventArgs) Handles Debug.Click

        Settings.LogLevel = "DEBUG"
        SendMsg(Settings.LogLevel)

    End Sub

    Private Sub DebugToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles DebugToolStripMenuItem1.Click

#Disable Warning CA2000 ' Dispose objects before losing scope
        Dim FormInput As New FormDebug
#Enable Warning CA2000 ' Dispose objects before losing scope
        FormInput.Activate()
        FormInput.Visible = True
        FormInput.Select()
        FormInput.BringToFront()

    End Sub

    Private Sub DeleteServiceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteServiceToolStripMenuItem.Click

        StopApache()
        Dim win = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "sc.exe")
        Dim pi = New ProcessStartInfo With {
            .WindowStyle = ProcessWindowStyle.Hidden,
            .CreateNoWindow = True,
            .FileName = win,
            .Arguments = "delete ApacheHTTPServer"
        }
        Using p As New Process
            p.StartInfo = pi
            Try
                p.Start()
                p.WaitForExit()
                ApacheIcon(False)
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
        End Using

    End Sub

    Private Sub DeleteServiceToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles DeleteServiceToolStripMenuItem1.Click

        StopMysql()
        Dim win = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "sc.exe")
        Dim pi = New ProcessStartInfo With {
    .WindowStyle = ProcessWindowStyle.Hidden,
    .CreateNoWindow = True,
    .FileName = win,
    .Arguments = "delete MySQLDreamGrid"
}
        Using p As New Process
            p.StartInfo = pi
            Try
                p.Start()
                p.WaitForExit()
                MySQLIcon(False)
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
        End Using
    End Sub

    Private Sub DiagnosticsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DiagnosticsToolStripMenuItem.Click

        If Not PropOpensimIsRunning() Then
            TextPrint(My.Resources.Click_Start)
            Return
        End If

        DoDiag()
        If Settings.DiagFailed = "True" Then
            TextPrint(My.Resources.HG_Failed)
        Else
            TextPrint(My.Resources.HG_Works)
        End If

    End Sub

    Private Sub ErrorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ErrorToolStripMenuItem.Click

        Settings.LogLevel = "ERROR"
        SendMsg(Settings.LogLevel)

    End Sub

    Private Sub EveryoneToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EveryoneToolStripMenuItem.Click

        RunCheck("everyone")

    End Sub

    Private Sub Fatal1_Click(sender As Object, e As EventArgs) Handles Fatal1.Click

        Settings.LogLevel = "FATAL"
        SendMsg(Settings.LogLevel)

    End Sub

    Private Sub FindIARsOnOutworldzToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FindIARsOnOutworldzToolStripMenuItem.Click

        Dim webAddress As String = PropHttpsDomain & "/outworldz_installer/IAR"
        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

    End Sub

    Private Sub FloatToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FloatToolStripMenuItem.Click

        Me.TopMost = False
        Settings.KeepOnTopMain = False
        KeepOnTopToolStripMenuItem.Image = My.Resources.table
        OnTopToolStripMenuItem.Checked = False
        FloatToolStripMenuItem.Checked = True

    End Sub

    Private Sub FreezAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FreezAllToolStripMenuItem.Click

        For Each RegionUUID In RegionUuids()
            FreezeThaw.Freeze(RegionUUID)
        Next

    End Sub

    Private Sub HelpClick(sender As Object, e As EventArgs)

        If sender Is Nothing Then Return
        If sender.ToString.ToUpper(Globalization.CultureInfo.InvariantCulture) <> "DreamGrid Manual.pdf".ToUpper(Globalization.CultureInfo.InvariantCulture) Then
            HelpManual(CStr(sender.Text))
        End If

    End Sub

    Private Sub HelpOnIARSToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpOnIARSToolStripMenuItem.Click
        Dim webAddress As String = "http://opensimulator.org/wiki/Inventory_Archives"
        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try
    End Sub

    Private Sub HelpOnOARsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpOnOARsToolStripMenuItem.Click
        Dim webAddress As String = "http://opensimulator.org/wiki/Load_Oar_0.9.0%2B"
        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try
    End Sub

    Private Sub HelpStartingUpToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles HelpStartingUpToolStripMenuItem1.Click

        HelpManual("Startup")

    End Sub

    Private Sub HelpToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem1.Click

        HelpManual("Database")

    End Sub

    Private Sub HelpToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem2.Click

        HelpManual("ServerType")

    End Sub

    Private Sub HelpToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem3.Click

        HelpManual("Apache")

    End Sub

    Private Sub HelpToolStripMenuItem4_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem4.Click

        HelpManual("Icecast")

    End Sub

    Private Sub Info_Click(sender As Object, e As EventArgs) Handles Info.Click

        Settings.LogLevel = "INFO"
        SendMsg(Settings.LogLevel)

    End Sub

    Private Sub JobEngineToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles JobEngineToolStripMenuItem.Click
        For Each RegionUUID As String In RegionUuids()
            ConsoleCommand(RegionUUID, "debug jobengine status")
        Next
    End Sub

    Private Sub JustOneRegionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AllUsersAllSimsToolStripMenuItem.Click

        If Not PropOpensimIsRunning() Then
            TextPrint(My.Resources.Not_Running)
            Return
        End If

        Dim HowManyAreOnline As Integer = 0
        Dim Message = InputBox(My.Resources.What_2_say_To_all)
        If Message.Length > 0 Then
            For Each RegionUUID In RegionUuids()
                If AvatarCount(RegionUUID) > 0 Then
                    HowManyAreOnline += 1
                    SendMessage(RegionUUID, Message)
                End If
            Next
            If HowManyAreOnline = 0 Then
                TextPrint(My.Resources.Nobody_Online)
            Else
                TextPrint($"{My.Resources.Message_sent_word} {CStr(HowManyAreOnline)} regions")
            End If
        End If

    End Sub

    ''' <summary>The main startup - done this way so languages can reload the entire form</summary>
    Private Sub JustQuitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles JustQuitToolStripMenuItem.Click

        TextPrint("Zzzz...")
        Thread.Sleep(100)
        End

    End Sub

    Private Sub LanguageToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles LanguageToolStripMenuItem1.Click

#Disable Warning CA2000
        Dim Lang As New Language
#Enable Warning CA2000
        Lang.Activate()
        Lang.Visible = True
        Lang.Select()
        Lang.BringToFront()

    End Sub

    Private Sub LoadFreeDreamGridOARsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles IslandToolStripMenuItem.Click

        Try
            ContentOAR.Activate()
            ContentOAR.ShowForm()
            ContentOAR.Select()
            ContentOAR.BringToFront()
        Catch
        End Try

    End Sub

    Private Sub LoadIarClick(sender As Object, e As EventArgs) ' event handler

        Dim File As String = IO.Path.Combine(BackupPath, CStr(sender.Text)) 'make a real URL
        If LoadIARContent(File) Then
            TextPrint($"{My.Resources.Opensimulator_is_loading} {CStr(sender.Text)}. {Global.Outworldz.My.Resources.Take_time}")
        End If

    End Sub

    Private Sub LoadInventoryIARToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles LoadInventoryIARToolStripMenuItem1.Click

        LoadIAR()

    End Sub

    Private Sub LoadOARToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoadRegionOARToolStripMenuItem.Click

        LoadOar("")

    End Sub

    Private Sub LocalIarClick(sender As Object, e As EventArgs) ''''

        Dim thing As String = sender.text.ToString

        Dim File As String = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles/IAR/" & CStr(thing)) 'make a real URL
        If LoadIARContent(File) Then
            TextPrint(My.Resources.Opensimulator_is_loading & CStr(thing))
        End If

    End Sub

    Private Sub LocalOarClick(sender As Object, e As EventArgs)

        Dim thing As String = sender.text.ToString
        Dim File As String = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles/OAR/" & thing) 'make a real URL
        LoadOARContent(File)
        TextPrint(My.Resources.Opensimulator_is_loading & CStr(sender.Text))

    End Sub

    Private Sub LogViewClick(sender As Object, e As EventArgs)

        Viewlog(CStr(sender.Text))

    End Sub

    Private Sub LoopBackToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoopBackToolStripMenuItem.Click

        HelpManual("Loopback Fixes")

    End Sub

    Private Sub MinimizeAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MinimizeAllToolStripMenuItem.Click

        For Each RegionUuid In RegionUuids()
            ShowDOSWindow(RegionUuid, SHOWWINDOWENUM.SWMINIMIZE)
        Next

    End Sub

    Private Sub MnuAbout_Click(sender As System.Object, e As EventArgs) Handles mnuAbout.Click

#Disable Warning CA2000 ' Dispose objects before losing scope
        Dim HelpAbout = New FormCopyright
#Enable Warning CA2000 ' Dispose objects before losing scope
        HelpAbout.Show()
        HelpAbout.Activate()
        HelpAbout.Select()
        HelpAbout.BringToFront()

    End Sub

    Private Sub MnuExit_Click(sender As System.Object, e As EventArgs) Handles mnuExit.Click

        Dim result = MsgBox(My.Resources.AreYouSure, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground Or MsgBoxStyle.Exclamation, My.Resources.Quit_Now_Word)
        If result = vbYes Then
            ReallyQuit()
        End If

    End Sub

    Private Sub MnuHide_Click(sender As System.Object, e As EventArgs) Handles mnuHide.Click

        TextPrint(My.Resources.Not_Shown)
        mnuShow.Checked = False
        mnuHide.Checked = True
        mnuHideAllways.Checked = False

        Settings.ConsoleShow = "False"
        Settings.SaveSettings()

    End Sub

    Private Sub MnuHideAllways_Click(sender As Object, e As EventArgs) Handles mnuHideAllways.Click
        TextPrint(My.Resources.Not_Shown)
        mnuShow.Checked = False
        mnuHide.Checked = False
        mnuHideAllways.Checked = True

        Settings.ConsoleShow = "None"
        Settings.SaveSettings()

    End Sub

    Private Sub MoreContentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MoreFreeIslandsandPartsContentToolStripMenuItem.Click

        Dim webAddress As String = PropHttpsDomain & "/cgi/freesculpts.plx"
        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

    End Sub

    Private Sub MysqlPictureBox_Click(sender As Object, e As EventArgs)

        If MysqlInterface.IsMySqlRunning() Then
            StopMysql()
        Else
            StartMySQL()
        End If

    End Sub

    Private Sub Off1_Click(sender As Object, e As EventArgs) Handles Off1.Click

        Settings.LogLevel = "OFF"
        SendMsg(Settings.LogLevel)

    End Sub

    Private Sub OffToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OffToolStripMenuItem.Click
        OnToolStripMenuItem.Checked = False
        OffToolStripMenuItem.Checked = True
        Settings.ShowMysqlStats = False
    End Sub

    Private Sub OnToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OnToolStripMenuItem.Click
        OnToolStripMenuItem.Checked = True
        OffToolStripMenuItem.Checked = False
        Settings.ShowMysqlStats = True
    End Sub

    Private Sub OnTopToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OnTopToolStripMenuItem.Click

        Me.TopMost = True
        Settings.KeepOnTopMain = True
        KeepOnTopToolStripMenuItem.Image = My.Resources.tables
        OnTopToolStripMenuItem.Checked = True
        FloatToolStripMenuItem.Checked = False

    End Sub

    Private Sub PercentCPU_Click(sender As Object, e As EventArgs) Handles PercentCPU.Click
        G()
    End Sub

    Private Sub PercentRAM_Click(sender As Object, e As EventArgs) Handles PercentRAM.Click
        G()
    End Sub

    Private Sub RegionsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RegionsToolStripMenuItem.Click

        ShowRegionform()

    End Sub

    Private Sub RestartAllRegionsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RestartAllRegionsToolStripMenuItem.Click

        RestartAllRegions()

    End Sub

    Private Sub RestartOneRegionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RestartOneRegionToolStripMenuItem.Click

        If Not PropOpensimIsRunning() Then
            TextPrint(My.Resources.Not_Running)
            Return
        End If
        Dim name = ChooseRegion(True)
        Dim RegionUUID As String = FindRegionByName(name)

        If RegionUUID.Length > 0 Then
            ShutDown(RegionUUID, SIMSTATUSENUM.RecyclingDown)
        End If

    End Sub

    Private Sub RestartTheInstanceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RestartTheInstanceToolStripMenuItem.Click

        If Not PropOpensimIsRunning() Then
            TextPrint(My.Resources.Not_Running)
            Return
        End If
        Dim name = ChooseRegion(True)
        Dim RegionUUID As String = FindRegionByName(name)
        If RegionUUID.Length > 0 Then
            ShutDown(RegionUUID, SIMSTATUSENUM.RecyclingDown)
        End If

    End Sub

    Private Sub RestartToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RestartMysqlItem.Click

        PropAborting = True
        StopMysql()
        StartMySQL()
        PropAborting = False

    End Sub

    Private Sub RestartToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles RestartRobustItem.Click

        PropAborting = True
        StopRobust()
        StartRobust()
        PropAborting = False

    End Sub

    Private Sub RestartToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles RestartToolStripMenuItem2.Click

        If Not Settings.ApacheEnable Then
            ApacheIcon(False)
            TextPrint(My.Resources.Apache_Disabled)
        End If
        StopApache()
        StartApache()
        PropAborting = False

    End Sub

    Private Sub RestartToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles RestartIceCastItem2.Click

        If Not Settings.SCEnable Then
            Settings.SCEnable = True
        End If

        PropAborting = True
        StopIcecast()
        StartIcecast()
        PropAborting = False

    End Sub

    Private Sub RestoreDatabaseToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles RestoreToolStripMenuItem1.Click

        If PropOpensimIsRunning() Then
            TextPrint($"{My.Resources.OpensimNeedstoStop}, {My.Resources.Aborted_word}")
            Return
        End If

        If Not StartMySQL() Then
            ToolBar(False)
            Buttons(StartButton)
            TextPrint(My.Resources.Stopped_word)
            Return
        End If

        ' Create an instance of the open file dialog box. Set filter options and filter index.
        Dim openFileDialog1 = New OpenFileDialog With {
            .InitialDirectory = BackupPath(),
            .Filter = Global.Outworldz.My.Resources.Backup_Folder & "(*.sql)|*.sql|All Files (*.*)|*.*",
            .FilterIndex = 1,
            .Multiselect = False
        }

        ' Call the ShowDialog method to show the dialogbox.
        ' Call the ShowDialog method to show the dialogbox.
        Dim UserClickedOK As DialogResult = openFileDialog1.ShowDialog
        openFileDialog1.Dispose()
        ' Process input if the user clicked OK.
        If UserClickedOK = DialogResult.OK Then
            Dim thing = openFileDialog1.FileName
            If thing.Length > 0 Then

                Dim db As String
                If thing.ToUpper(Globalization.CultureInfo.InvariantCulture).Contains("ROBUST") Then
                    db = Settings.RobustDatabaseName
                Else
                    db = Settings.RegionDBName
                End If

                Dim yesno = MsgBox(My.Resources.Are_You_Sure, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground Or MsgBoxStyle.Exclamation, Global.Outworldz.My.Resources.Restore_word)
                If yesno = vbYes Then

                    DeleteFile(IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\mysql\bin\RestoreMysql.bat"))

                    Dim opt As String = ""
                    If Settings.RootMysqlPassword.Length > 0 Then
                        opt = $"-p{Settings.RootMysqlPassword}"
                    End If

                    Try
                        Dim filename As String = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\mysql\bin\RestoreMysql.bat")
                        Using outputFile As New StreamWriter(filename, False)
                            outputFile.WriteLine($"@REM A program to restore MySQL from a backup{vbCrLf}mysql -u root {db} {opt} < ""{thing}""{vbCrLf} @pause{vbCrLf}")
                        End Using
                    Catch ex As Exception
                        ErrorLog(" Failed to create restore file:" & ex.Message)
                        Return
                    End Try

                    Using pMySqlRestore = New Process()
                        ' pi.Arguments = thing
                        Dim pi = New ProcessStartInfo With {
                            .WindowStyle = ProcessWindowStyle.Normal,
                            .WorkingDirectory = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\mysql\bin\"),
                            .FileName = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\mysql\bin\RestoreMysql.bat")
                        }
                        pMySqlRestore.StartInfo = pi
                        TextPrint(My.Resources.Do_Not_Interrupt_word)
                        Try
                            pMySqlRestore.Start()
                        Catch ex As Exception
                            BreakPoint.Dump(ex)
                        End Try
                    End Using
                End If
            Else
                TextPrint(My.Resources.Cancelled_word)
            End If
        End If
    End Sub

    Private Sub RevisionHistoryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RevisionHistoryToolStripMenuItem.Click

        HelpManual("Revisions")

    End Sub

    Private Sub RobustPictureBox_Click(sender As Object, e As EventArgs)

        If Not IsRobustRunning() Then
            StartRobust()
        Else
            StopRobust()
        End If

    End Sub

    Private Sub RunCheck(type As String)
        Using p = New Process()
            Dim pi = New ProcessStartInfo With {
                .Arguments = $"check_inventory.php {type}",
                .FileName = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles/php7/php.exe"),
                .WorkingDirectory = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles/php7/"),
                .UseShellExecute = True, ' so we can redirect streams and minimize
                .WindowStyle = ProcessWindowStyle.Normal,
                .CreateNoWindow = False
            }
            p.StartInfo = pi
            Try
                p.Start()
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
        End Using
    End Sub

    Private Sub SaveAllRunningRegiondsAsOARSToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveAllRunningRegiondsAsOARSToolStripMenuItem.Click

        If Not PropOpensimIsRunning() Then
            TextPrint(My.Resources.Not_Running)
            Return
        End If

        BackupAllRegions()

    End Sub

    Private Sub SaveInventoryIARToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles SaveInventoryIARToolStripMenuItem1.Click

        SaveIARTask()

    End Sub

    Private Sub SaveRegionOARToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles SaveRegionOARToolStripMenuItem1.Click

        SaveOar("")

    End Sub

    Private Sub SearchHelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SearchHelpToolStripMenuItem.Click

#Disable Warning CA2000 ' Dispose objects before losing scope
        Dim HelpAbout = New FormSearchHelp
#Enable Warning CA2000 ' Dispose objects before losing scope
        HelpAbout.Show()
        HelpAbout.Activate()
        HelpAbout.Select()
        HelpAbout.BringToFront()

    End Sub

    Private Sub ShowAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowAllToolStripMenuItem.Click
        For Each RegionUuid In RegionUuids()
            ShowDOSWindow(RegionUuid, SHOWWINDOWENUM.SWRESTORE)
        Next
    End Sub

    Private Sub ShowHyperGridAddressToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowHyperGridAddressToolStripMenuItem.Click

        TextPrint($"{My.Resources.Grid_Address_is_word} http://{Settings.PublicIP}:{Settings.HttpPort}")

    End Sub

    Private Sub ShowStatsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowStatsToolStripMenuItem.Click

        If Settings.ShowMysqlStats Then
            Settings.ShowMysqlStats = False
            OnToolStripMenuItem.Checked = False
            OffToolStripMenuItem.Checked = True
        Else
            Settings.ShowMysqlStats = True
            OnToolStripMenuItem.Checked = True
            OffToolStripMenuItem.Checked = False
        End If

    End Sub

    Private Sub ShowToolStripMenuItem_Click(sender As System.Object, e As EventArgs) Handles mnuShow.Click

        TextPrint(My.Resources.Is_Shown)
        mnuShow.Checked = True
        mnuHide.Checked = False
        mnuHideAllways.Checked = False

        Settings.ConsoleShow = "True"
        Settings.SaveSettings()

    End Sub

    Private Sub ShowUserDetailsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowUserDetailsToolStripMenuItem.Click
        Dim person = InputBox(My.Resources.Enter_1_2)
        If person.Length > 0 Then
            ConsoleCommand(RobustName, "show account " & person)
        End If
    End Sub

    Private Sub SimulatorStatsToolStripMenuItem_Click(sender As Object, e As EventArgs)

        If Not PropOpensimIsRunning Then
            TextPrint(My.Resources.Not_Running)
            Return
        End If

        Dim RegionPort = GroupPort(FindRegionByName(Settings.WelcomeRegion))
        Dim webAddress As String = $"http://{Settings.PublicIP}:{CType(RegionPort, String)}/SStats/"
        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

    End Sub

    Private Sub StartButton_Click(sender As System.Object, e As EventArgs) Handles StartButton.Click
        Startup()
    End Sub

    Private Sub StartToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StartToolStripMenuItem.Click

        Settings.ApacheEnable = True
        Settings.SaveSettings()
        StartApache()

    End Sub

    Private Sub StartToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles StartToolStripMenuItem1.Click

        StartMySQL()

    End Sub

    Private Sub StartToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles StartToolStripMenuItem2.Click

        StartRobust()

    End Sub

    Private Sub StartToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles StartToolStripMenuItem3.Click

        Settings.SCEnable = True
        Settings.SaveSettings()
        StartIcecast()

    End Sub

    Private Sub Statmenu(sender As Object, e As EventArgs)

        If PropOpensimIsRunning() Then
            Dim RegionUUID As String = FindRegionByName(CStr(sender.Text))
            Dim port As String = CStr(Region_Port(RegionUUID))
            Dim webAddress As String = "http://127.0.0.1:" & Settings.HttpPort & "/bin/data/sim.html?port=" & port
            Try
                Process.Start(webAddress)
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
        Else
            TextPrint(My.Resources.Not_Running)
        End If

    End Sub

    Private Sub StopButton_Click_1(sender As System.Object, e As EventArgs) Handles StopButton.Click

        DoStopActions()

    End Sub

    Private Sub StopToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StopToolStripMenuItem.Click

        StopApache()

    End Sub

    Private Sub StopToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles StopToolStripMenuItem1.Click

        StopMysql()

    End Sub

    Private Sub StopToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles StopToolStripMenuItem2.Click

        StopRobust()

    End Sub

    Private Sub StopToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles StopToolStripMenuItem3.Click

        PropAborting = True
        StopIcecast()
        Sleep(2000)
        PropAborting = False

    End Sub

    Private Sub TechnicalInfoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TechnicalInfoToolStripMenuItem.Click

        Dim webAddress As String = PropHttpsDomain & "/Outworldz_installer/technical.htm"
        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

    End Sub

    Private Sub ThawAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ThawAllToolStripMenuItem.Click

        For Each RegionUUID In RegionUuids()
            Thaw(RegionUUID)
            Application.DoEvents()
            Timer(RegionUUID) = DateAdd("n", 5, Date.Now) ' Add  5 minutes for console to do things
        Next

    End Sub

    Private Sub ThreadpoolsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ThreadpoolsToolStripMenuItem.Click

        For Each RegionUUID As String In RegionUuids()
            ConsoleCommand(RegionUUID, "show threads")
        Next

    End Sub

    Private Sub TodoManualToolStripMenuItem_Click_1(sender As Object, e As EventArgs)

        Dim webAddress As String = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Help\To Do List.pdf")
        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click

        Dim webAddress As String = PropHttpsDomain & "/Outworldz_Installer/PortForwarding.htm"
        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

    End Sub

    Private Sub TroubleshootingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TroubleshootingToolStripMenuItem.Click

        HelpManual("TroubleShooting")

    End Sub

    Private Sub ViewGoogleMapToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewGoogleMapToolStripMenuItem.Click

        Dim WelcomeUUID = FindRegionByName(Settings.WelcomeRegion)
        Dim Str = "/wifi/map.html?X=" & CStr(Coord_X(WelcomeUUID) - 15) &
                "&Y=" & CStr(Coord_Y(WelcomeUUID) + 5)

        If Settings.ApacheEnable Then
            Dim webAddress As String = $"http://{Settings.PublicIP}:{Settings.HttpPort}{Str}"

            Try
                Process.Start(webAddress)
            Catch ex As Exception
            End Try
        Else
            TextPrint(My.Resources.Apache_Disabled)
        End If

    End Sub

    Private Sub ViewIcecastWebPageToolStripMenuItem_Click(sender As Object, e As EventArgs)

        If PropOpensimIsRunning() AndAlso Settings.SCEnable Then
            Dim webAddress As String = "http://" & Settings.PublicIP & ":" & CStr(Settings.SCPortBase)
            TextPrint($"{My.Resources.Icecast_Desc}{vbCrLf}{webAddress}/stream")
            Try
                Process.Start(webAddress)
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
        ElseIf Settings.SCEnable = False Then
            TextPrint(My.Resources.Shoutcast_Disabled)
        Else
            TextPrint(My.Resources.Not_Running)
        End If

    End Sub

    Private Sub ViewVisitorMapsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewVisitorMapsToolStripMenuItem.Click

        Dim webAddress As String
        If Settings.PublicVisitorMaps Then
            webAddress = $"http://{Settings.LANIP}:{CStr(Settings.ApachePort)}/Stats?r={Random()}"
        Else
            webAddress = $"http://127.0.0.1:{CStr(Settings.ApachePort)}/Stats"
        End If

        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

    End Sub

    Private Sub Virtual_Click(sender As Object, e As EventArgs) Handles Virtual.Click
        G()
    End Sub

    Private Sub Warn_Click(sender As Object, e As EventArgs) Handles Warn.Click

        Settings.LogLevel = "WARN"
        SendMsg(Settings.LogLevel)

    End Sub

    Private Sub XengineToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles XengineToolStripMenuItem.Click

        For Each RegionUUID As String In RegionUuids()
            ConsoleCommand(RegionUUID, "xengine status")
        Next

    End Sub

#End Region

End Class
