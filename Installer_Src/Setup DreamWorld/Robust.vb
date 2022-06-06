#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.IO
Imports System.Net
Imports System.Text.RegularExpressions

Module Robust

    Private WithEvents RobustProcess As New Process()
    Private _RobustCrashCounter As Integer
    Private _RobustExited As Boolean
    Private _RobustProcID As Integer

    Public Property PropRobustExited() As Boolean
        Get
            Return _RobustExited
        End Get
        Set(ByVal Value As Boolean)
            _RobustExited = Value
        End Set
    End Property

    Public Property PropRobustProcID As Integer
        Get
            Return _RobustProcID
        End Get
        Set(value As Integer)
            _RobustProcID = value
        End Set
    End Property

    Public Property RobustCrashCounter As Integer
        Get
            Return _RobustCrashCounter
        End Get
        Set(value As Integer)
            _RobustCrashCounter = value
        End Set
    End Property

#Region "Robust"

    ''' <summary>
    '''  Shows a Region picker
    ''' </summary>
    ''' <param name="JustRunning">True = only running regions</param>
    ''' <returns>Name</returns>
    Public Function ChooseRegion(Optional JustRunning As Boolean = False) As String

        ' Show testDialog as a modal dialog and determine if DialogResult = OK.
        Dim chosen As String = ""
        Using Chooseform As New FormChooser ' form for choosing a set of regions
            Chooseform.FillGrid("Region", JustRunning)  ' populate the grid with either Group or RegionName
            Dim ret = Chooseform.ShowDialog()
            If ret = DialogResult.Cancel Then Return ""
            Try
                ' Read the chosen sim name
                chosen = Chooseform.DataGridView.CurrentCell.Value.ToString()
            Catch ex As Exception
                BreakPoint.Dump(ex)
                ErrorLog("Warn: Could not choose a displayed region. " & ex.Message)
            End Try
        End Using
        Return chosen

    End Function

    ''' <summary>
    ''' Log and show that Robust is offline. PID = 0
    ''' </summary>
    Public Sub MarkRobustOffline()

        Log("INFO", "Robust is not running")
        RobustIcon(False)
        PropRobustProcID = 0

    End Sub

    Public Sub RobustIcon(Running As Boolean)

        If Not Running Then
            FormSetup.RestartRobustIcon.Image = Global.Outworldz.My.Resources.gear
        Else
            FormSetup.RestartRobustIcon.Image = Global.Outworldz.My.Resources.check2
        End If

    End Sub

    Public Function StartRobust() As Boolean

        If Not StartMySQL() Then Return False ' prerequsite

        For Each p In Process.GetProcessesByName("Robust")
            Try
                If p.MainWindowTitle = RobustName() Then

                    If Not IsRobustRunning() Then
                        Sleep(10000)
                    End If

                    PropRobustProcID = p.Id
                    Log(My.Resources.Info_word, Global.Outworldz.My.Resources.DosBoxRunning)
                    RobustIcon(True)
                    p.EnableRaisingEvents = True
                    AddHandler p.Exited, AddressOf RobustProcess_Exited
                    PropOpensimIsRunning = True
                    Return True

                End If
            Catch ex As Exception
            End Try
        Next

        ' Check the HTTP port
        If IsRobustRunning() Then
            RobustIcon(True)
            PropOpensimIsRunning = True
            Return True
        End If

        If Settings.ServerType <> RobustServerName Then
            RobustIcon(True)
            PropOpensimIsRunning = True
            Return True
        End If

        SetServerType()
        PropRobustProcID = 0

        If DoRobust() Then Return False

        TextPrint("Robust " & Global.Outworldz.My.Resources.Starting_word)

        RobustProcess.EnableRaisingEvents = True
        RobustProcess.StartInfo.UseShellExecute = False
        RobustProcess.StartInfo.Arguments = "-inifile Robust.HG.ini"

        RobustProcess.StartInfo.FileName = Settings.OpensimBinPath & "robust.exe"
        RobustProcess.StartInfo.CreateNoWindow = False
        RobustProcess.StartInfo.WorkingDirectory = Settings.OpensimBinPath

        RobustProcess.StartInfo.RedirectStandardOutput = False

        Select Case Settings.ConsoleShow
            Case "True"
                RobustProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal
            Case "False"
                RobustProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal
            Case "None"
                RobustProcess.StartInfo.WindowStyle = ProcessWindowStyle.Minimized
        End Select

        Try
            RobustProcess.Start()
        Catch ex As Exception
            BreakPoint.Dump(ex)
            TextPrint($"Robust {Global.Outworldz.My.Resources.did_not_start_word} {ex.Message}")
            FormSetup.KillAll()
            FormSetup.Buttons(FormSetup.StartButton)
            MarkRobustOffline()
            Return False
        End Try
        PropRobustProcID = WaitForPID(RobustProcess)
        If PropRobustProcID = 0 Then
            MarkRobustOffline()
            Log("Error", Global.Outworldz.My.Resources.Robust_failed_to_start)
            Return False
        End If

        SetWindowTextCall(RobustProcess, RobustName)

        ' Wait for Robust to start listening
        Dim counter = 0
        While Not IsRobustRunning() And PropOpensimIsRunning
            Dim sleeptime = 5    ' seconds
            TextPrint("Robust " & Global.Outworldz.My.Resources.isBooting)
            counter += 1
            ' 2 minutes to boot on bad hardware at 5 sec per spin
            If counter > 60 * 2 / sleeptime Then
                TextPrint(My.Resources.Robust_failed_to_start)
                FormSetup.Buttons(FormSetup.StartButton)
                Dim yesno = MsgBox(My.Resources.See_Log, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Error_word)
                If (yesno = vbYes) Then
                    Baretail("""" & Settings.OpensimBinPath & "Robust.log" & """")
                End If
                FormSetup.Buttons(FormSetup.StartButton)
                MarkRobustOffline()
                Return False
            End If
            Sleep(sleeptime * 1000) ' in ms
        End While
        Sleep(2000)
        Log(My.Resources.Info_word, Global.Outworldz.My.Resources.Robust_running)
        ShowDOSWindow(GetHwnd(RobustName), MaybeHideWindow())
        RobustIcon(True)
        TextPrint(Global.Outworldz.My.Resources.Robust_running)
        PropRobustExited = False

        Return True

    End Function

    Public Sub StopRobust()

        If Settings.ServerType <> RobustServerName Then Return
        If IsRobustRunning() Then

            TextPrint("Robust " & Global.Outworldz.My.Resources.Stopping_word)

            ConsoleCommand(RobustName, "q", True)

            Dim ctr As Integer = 0
            ' wait 30 seconds for robust to quit
            While IsRobustRunning() And ctr < 30
                Application.DoEvents()
                Sleep(1000)
                ConsoleCommand(RobustName, "q", True)
                ctr += 1
            End While
            If ctr = 30 Then Zap("Robust")
        End If

        MarkRobustOffline()

    End Sub

#End Region

    Public Sub DoBanList(INI As LoadIni)

        Dim MACString As String = ""
        Dim ViewerString As String = ""
        Dim GridString As String = ""
        Dim Bans As String = Settings.BanList
        Dim filename As String
        If Bans.Length = 0 Then
            filename = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles/Opensim/BanListProto.txt")
            Bans = ReadBanList(filename)
        End If

        Dim Banlist As String()
        Banlist = Bans.Split("|".ToCharArray())
        For Each str As String In Banlist

            Dim a() = str.Split("=".ToCharArray())
            Dim s = a(0)

            Dim pattern1 = New Regex("^#")
            Dim match1 As Match = pattern1.Match(s)
            If match1.Success Then
                Continue For
            End If

            ' ban grid Addresses
            Dim pattern2 = New Regex("^http", RegexOptions.IgnoreCase)
            Dim match2 As Match = pattern2.Match(s)
            If match2.Success Then
                GridString += s & ","   ' delimiter is a comma for grids
                Continue For
            End If

            ' Ban IP's

            Dim pattern3 = New Regex("^\d+\.\d+\.\d+\.\d+")
            Dim match3 As Match = pattern3.Match(s)
            If match3.Success Then
                Firewall.BlockIP(s)
                Continue For
            End If

            ' ban MAC Addresses with and without caps and :
            Dim result As Guid
            If Guid.TryParse(s, result) Then
                s = s.ToUpperInvariant
                MACString += s & " " ' delimiter is a " " and  not a pipe
                Continue For
            End If

            ' none of the above
            If s.Length > 0 Then
                ViewerString += s & "|"
            End If

        Next

        ' Ban grids
        If GridString.Length > 0 Then
            GridString = Mid(GridString, 1, GridString.Length - 1)
        End If

        INI.SetIni("GatekeeperService", "AllowExcept", GridString)

        ' Ban Macs
        If MACString.Length > 0 Then
            MACString = Mid(MACString, 1, MACString.Length - 1)
        End If
        INI.SetIni("LoginService", "DeniedMacs", MACString)
        INI.SetIni("GatekeeperService", "DeniedMacs", MACString)

        'Ban Viewers
        If ViewerString.Length > 0 Then
            ViewerString = Mid(ViewerString, 1, ViewerString.Length - 1)
        End If

        INI.SetIni("AccessControl", "DeniedClients", ViewerString)

    End Sub

    Public Function DoRobust() As Boolean

        TextPrint("->Set Robust")

        CopyFileFast(IO.Path.Combine(Settings.OpensimBinPath, "Robust.HG.ini.proto"),
                     IO.Path.Combine(Settings.OpensimBinPath, "Robust.HG.ini"))

        ' set the defaults in the INI for the viewer to use. Painful  as it's a Left hand side edit must be done before other edits to Robust.HG.ini as this makes the actual Robust.HG.ifile
        ' add this sim name as a default to the file as HG regions, and add the other regions as fallback it may have been deleted
        Dim WelcomeUUID As String = FindRegionByName(Settings.WelcomeRegion)
        Dim DefaultName = Settings.WelcomeRegion

        ' Replace the block with a list of regions with the Region_Name = DefaultRegion, DefaultHGRegion is Welcome
        ' Region_Name = FallbackRegion,  if non Smart Start region
        ' and is SS is enabled Region_Name = FallbackRegion,FallbackRegion

        Dim Welcome As String = Settings.WelcomeRegion
        Welcome = DefaultName.Replace(" ", "_")    ' because this is a screwy thing they did in the INI file
        Dim RegionSetting As String = ""

        ' make a long list of the various regions with region_ at the start
        For Each RegionUUID As String In RegionUuids()

            Dim RegionName = Region_Name(RegionUUID)
            RegionName = RegionName.Replace(" ", "_")    ' because this is a screwy thing they did in the INI file
            If Region_Name(RegionUUID) = DefaultName Then
                RegionSetting += $"Region_{Welcome}=DefaultRegion,DefaultHGRegion{vbCrLf}"
            Else
                RegionSetting += $"Region_{RegionName}=Persistent,FallbackRegion{vbCrLf}"
            End If

        Next

        Using outputFile As New StreamWriter(Settings.OpensimBinPath & "Robust.HG.ini", False)
            Using reader = System.IO.File.OpenText(Settings.OpensimBinPath & "Robust.HG.ini.proto")
                'now loop through each line
                Dim line As String

                While reader.Peek <> -1
                    line = reader.ReadLine()
                    Dim Output As String = Nothing
                    'Breakpoint.Print(line)
                    If line.StartsWith("; START", StringComparison.OrdinalIgnoreCase) Then
                        Output += line ' add back on the ; START
                        Output += vbCrLf & RegionSetting
                    Else
                        Output += line
                    End If

                    outputFile.WriteLine(Output)
                End While
                outputFile.Flush()
            End Using
        End Using

        Dim INI = New LoadIni(Settings.OpensimBinPath & "Robust.HG.ini", ";", System.Text.Encoding.UTF8)

        If WelcomeUUID.Length = 0 And Settings.ServerType = RobustServerName Then
            MsgBox(My.Resources.Cannot_locate, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground)
            Dim RegionName = ChooseRegion(False)

            If RegionName.Length = 0 Then
                Return False
            End If
            Settings.WelcomeRegion = RegionName
            Settings.SaveSettings()
        End If

        'For GetTexture Service
        If Settings.FsAssetsEnabled Then
            INI.SetIni("CapsService", "AssetService", """" & "OpenSim.Services.FSAssetService.dll:FSAssetConnector" & """")
        Else
            INI.SetIni("CapsService", "AssetService", """" & "OpenSim.Services.AssetService.dll:AssetService" & """")
        End If

        If Settings.AltDnsName.Length > 0 Then
            INI.SetIni("Hypergrid", "HomeURIAlias", Settings.AltDnsName)
            INI.SetIni("Hypergrid", "GatekeeperURIAlias", Settings.AltDnsName)
        End If

        INI.SetIni("Const", "GridName", Settings.SimName)
        INI.SetIni("Const", "BaseURL", "http://" & Settings.PublicIP)

        DoBanList(INI)

        ' Smart Start cannot boot a HG region so send them to welcome.
        INI.SetIni("GatekeeperService", "AllowTeleportsToAnyRegion", CStr(Settings.Smart_Start))

        INI.SetIni("Const", "DiagnosticsPort", CStr(Settings.DiagnosticPort))
        INI.SetIni("Const", "PrivURL", "http://" & Settings.LANIP())
        INI.SetIni("Const", "PublicPort", CStr(Settings.HttpPort)) ' 8002
        INI.SetIni("Const", "PrivatePort", CStr(Settings.PrivatePort))
        INI.SetIni("Const", "http_listener_port", CStr(Settings.HttpPort))
        INI.SetIni("Const", "ApachePort", CStr(Settings.ApachePort))
        INI.SetIni("Const", "MachineID", CStr(Settings.MachineID))

        If Settings.Suitcase() Then
            INI.SetIni("HGInventoryService", "LocalServiceModule", "OpenSim.Services.HypergridService.dll:HGSuitcaseInventoryService")
        Else
            INI.SetIni("HGInventoryService", "LocalServiceModule", "OpenSim.Services.InventoryService.dll:XInventoryService")
        End If

        ' LSL emails
        If INI.SetIni("SMTP", "SMTP_SERVER_HOSTNAME", Settings.SmtpHost) Then Return True
        If INI.SetIni("SMTP", "SMTP_SERVER_PORT", CStr(Settings.SmtpPort)) Then Return True
        If INI.SetIni("SMTP", "SMTP_SERVER_LOGIN", Settings.SmtPropUserName) Then Return True

        ' Some SMTP servers require a known From email address or will give Error 500 - Envelope from address is not authorized
        '; set to a valid email address that SMTP will accept (in some cases must be Like SMTP_SERVER_LOGIN)

        If Settings.AdminEmail.Length > 0 Then
            If INI.SetIni("SMTP", "SMTP_SERVER_FROM", Settings.AdminEmail) Then Return True
        Else
            If INI.SetIni("SMTP", "SMTP_SERVER_FROM", Settings.SmtPropUserName) Then Return True
        End If

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

        If Settings.SmtpSecure Then
            If INI.SetIni("SMTP", "SMTP_SERVER_TLS", CStr(Settings.SmtpPassword)) Then Return True
        End If

        If INI.SetIni("SMTP", "host_domain_header_from", Settings.BaseHostName) Then Return True

        SetupRobustSearchINI(INI)

        SetupMoney(INI)

        INI.SetIni("LoginService", "WelcomeMessage", Settings.WelcomeMessage)

        'FSASSETS
        If Settings.FsAssetsEnabled Then
            INI.SetIni("AssetService", "LocalServiceModule", "OpenSim.Services.FSAssetService.dll:FSAssetConnector")
            INI.SetIni("HGAssetService", "LocalServiceModule", "OpenSim.Services.HypergridService.dll:HGFSAssetService")
        Else
            INI.SetIni("AssetService", "LocalServiceModule", "OpenSim.Services.AssetService.dll:AssetService")
            INI.SetIni("HGAssetService", "LocalServiceModule", "OpenSim.Services.HypergridService.dll:HGAssetService")
        End If

        INI.SetIni("AssetService", "BaseDirectory", Settings.BaseDirectory & "/data")
        INI.SetIni("AssetService", "SpoolDirectory", Settings.BaseDirectory & "/tmp")
        INI.SetIni("AssetService", "ShowConsoleStats", Settings.ShowConsoleStats)

        INI.SetIni("ServiceList", "GetTextureConnector", """" & "${Const|PublicPort}/Opensim.Capabilities.Handlers.dll:GetTextureServerConnector" & """")

        If Settings.CMS = JOpensim Then
            INI.SetIni("ServiceList", "UserProfilesServiceConnector", "")
            INI.SetIni("UserProfilesService", "Enabled", "False")
            INI.SetIni("GridInfoService", "welcome", "${Const|BaseURL}:${Const|ApachePort}/jOpensim/index.php?option=com_opensim")
            INI.SetIni("GridInfoService", "economy", "${Const|BaseURL}:${Const|ApachePort}/jOpensim/components/com_opensim/")
        Else
            INI.SetIni("ServiceList", "UserProfilesServiceConnector", "${Const|PublicPort}/OpenSim.Server.Handlers.dll:UserProfilesConnector")
            INI.SetIni("UserProfilesService", "Enabled", "True")
            INI.SetIni("GridInfoService", "welcome", Settings.SplashPage)

            If Settings.GloebitsEnable Then
                INI.SetIni("GridInfoService", "economy", "${Const|BaseURL}:${Const|PublicPort}")
            Else
                ' use Landtool.php
                INI.SetIni("GridInfoService", "economy", "${Const|BaseURL}:${Const|ApachePort}/Land")
            End If
        End If

        INI.SetIni("DatabaseService", "ConnectionString", Settings.RobustDBConnection)

        ' SmartStart. Add own entries for DreamGrid host and port
        ' In future that may need to be more clever, as per machine in a servers cluster
        If Settings.Smart_Start Then
            INI.SetIni("SmartStart", "Enabled", "True")
            INI.SetIni("SmartStart", "URL", "http://" & Settings.LANIP() + ":" & CStr(Settings.DiagnosticPort))
            INI.SetIni("SmartStart", "MachineID", CStr(Settings.MachineID))
        Else
            INI.SetIni("SmartStart", "Enabled", "False")
            INI.SetIni("SmartStart", "URL", "")
            INI.SetIni("SmartStart", "MachineID", "")
        End If

        INI.SaveINI()

        Dim src = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Opensim\bin\Robust.exe.config.proto")
        Dim Dest = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Opensim\bin\Robust.exe.config")
        CopyFileFast(src, Dest)
        Dim anini = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Opensim\bin\Robust.exe.config")
        Grep(anini, Settings.LogLevel)

        Return False

    End Function

    ''' <summary>Check is Robust port 8002 is up</summary>
    ''' <returns>boolean</returns>
    Public Function IsRobustRunning() As Boolean

        Log("INFO", "Checking Robust")

        Dim Up As String = ""

        Using TimedClient As New TimedWebClient With {
                .Timeout = 2000
            }
            Dim url As String
            If Settings.ServerType = "Robust" Then
                url = "http://" & Settings.LANIP & ":" & Settings.HttpPort & "/index.php?version"
            Else
                url = "http://" & Settings.PublicIP & ":" & Settings.HttpPort & "/index.php?version"
            End If

            Try
                Up = TimedClient.DownloadString(url)
            Catch ex As Exception
                If ex.Message.Contains("404") Then
                    Log("INFO", "Robust is running")
                    RobustIcon(True)
                    Return True
                End If
            End Try

        End Using

        If Up = "" Then
            MarkRobustOffline()
            Return False
        ElseIf Up.Contains("OpenSim") Then
            Log("INFO", "Robust is running")
            RobustIcon(True)
            Return True
        End If
        MarkRobustOffline()
        Return False

    End Function

    Public Function ReadBanList(filename As String) As String

        Dim output As String = ""

        If filename.Length > 0 Then
            Using reader As IO.StreamReader = System.IO.File.OpenText(filename)
                'now loop through each line
                While reader.Peek <> -1
                    Dim line = reader.ReadLine()
                    Dim words() = line.Split("|".ToCharArray)
                    output += words(0) & "|"
                End While
            End Using
        End If

        Return output

    End Function

    Private Sub RobustProcess_Exited(ByVal sender As Object, ByVal e As EventArgs) Handles RobustProcess.Exited

        ' Handle Exited event and display process information.
        PropRobustProcID = Nothing
        If PropAborting Then Return

        If Settings.RestartOnCrash And RobustCrashCounter < 10 Then
            PropRobustExited = True
            MarkRobustOffline()
            RobustCrashCounter += 1
            Return
        End If

        RobustCrashCounter = 0
        MarkRobustOffline()

        Dim yesno = MsgBox(My.Resources.Robust_exited, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Error_word)
        If (yesno = vbYes) Then
            Baretail("""" & Settings.OpensimBinPath & "Robust.log" & """")
        End If

    End Sub

    Private Sub SetupMoney(INI As LoadIni)

        DeleteFile(IO.Path.Combine(Settings.OpensimBinPath, "jOpenSim.Money.dll"))
        If Settings.GCG Then
            INI.SetIni("LoginService", "Currency", "MC$")
            CopyFileFast(IO.Path.Combine(Settings.OpensimBinPath, "Gloebit.dll.bak"), IO.Path.Combine(Settings.OpensimBinPath, "Gloebit.dll"))
        ElseIf Settings.GloebitsEnable Then
            INI.SetIni("LoginService", "Currency", "G$")
            CopyFileFast(IO.Path.Combine(Settings.OpensimBinPath, "Gloebit.dll.bak"), IO.Path.Combine(Settings.OpensimBinPath, "Gloebit.dll"))
        ElseIf Settings.GloebitsEnable = False And Settings.CMS = JOpensim Then
            INI.SetIni("LoginService", "Currency", "jO$")
            DeleteFile(IO.Path.Combine(Settings.OpensimBinPath, "Gloebit.dll"))
        Else
            INI.SetIni("LoginService", "Currency", "$")
            DeleteFile(IO.Path.Combine(Settings.OpensimBinPath, "Gloebit.dll"))
        End If

    End Sub

    Private Sub SetupRobustSearchINI(INI As LoadIni)

        If Settings.CMS = JOpensim And Settings.SearchOptions = JOpensim Then
            Dim SearchURL = "http://" & Settings.PublicIP & ":" & Settings.ApachePort & "/jOpensim/index.php?option=com_opensim&view=inworldsearch&task=viewersearch&tmpl=component&"
            INI.SetIni("LoginService", "SearchURL", SearchURL)
            INI.SetIni("LoginService", "DestinationGuide", "http://" & Settings.PublicIP & ":" & Settings.ApachePort & "/jOpensim/index.php?option=com_opensim&view=guide&tmpl=component")
        ElseIf Settings.SearchOptions = Hyperica Then
            INI.SetIni("LoginService", "SearchURL", PropDomain & "/Search/query.php")
            INI.SetIni("LoginService", "DestinationGuide", PropDomain & "/destination-guide")
        ElseIf Settings.SearchOptions = "Local" Then
            INI.SetIni("LoginService", "SearchURL", $"http://{Settings.PublicIP}:{Settings.ApachePort}/Search/query.php")
            INI.SetIni("LoginService", "DestinationGuide", PropDomain & "/destination-guide")
        Else
            INI.SetIni("LoginService", "SearchURL", "")
            INI.SetIni("LoginService", "DestinationGuide", "")
        End If

    End Sub

    Public Class TimedWebClient
        Inherits WebClient

        Public Sub New()
            Me.Timeout = 2000
        End Sub

        Public Property Timeout As Integer

        Protected Overrides Function GetWebRequest(ByVal address As Uri) As WebRequest
            Dim objWebRequest = MyBase.GetWebRequest(address)
            objWebRequest.Timeout = Me.Timeout
            Return objWebRequest
        End Function

    End Class

End Module
