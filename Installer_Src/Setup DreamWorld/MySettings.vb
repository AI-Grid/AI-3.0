#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.IO

Public Class MySettings

#Region "Private Fields"

    Private Const DreamGrid As String = "DreamGrid"
    Private Const JOpensim As String = "JOpensim"
    Private _CurSlashDir As String = ""
    Private _DeleteTreesFirst As Boolean
    Private _ExternalHostName As String
    Private _LANIP As String
    Private _MacAddress As String
    Private _OarCount As Integer
    Private _RamUsed As Double
    Private _Settings As LoadIni

#End Region

#Region "New"

    Public Sub New(Folder As String)

        Dim _myINI = Folder + "\OutworldzFiles\Settings.ini"
        If File.Exists(_myINI) Then
            Settings = New LoadIni(_myINI, ";", System.Text.Encoding.UTF8)
        Else
            Dim contents = "[Data]" + vbCrLf
            Try
                Using outputFile As New StreamWriter(_myINI, False)
                    outputFile.WriteLine(contents)
                    outputFile.Flush()
                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try

            Settings = New LoadIni(_myINI, ";", System.Text.Encoding.UTF8)

        End If

    End Sub

#End Region

#Region "Functions And Subs"

    ''' <summary>
    ''' Seconds to Boot until region Ready with maps off
    ''' </summary>
    ''' <param name="UUID"></param>
    ''' <returns>integer</returns>
    Public Function GetBootTime(UUID As String) As Integer

        Return CInt("0" & GetMySetting("BootTime_" & Region_Name(UUID), "0"))

    End Function

    ''' <summary>
    ''' Seconds to Boot until region Ready with maps on
    ''' </summary>
    ''' <param name="UUID"></param>
    ''' <returns>integer</returns>
    Public Function GetMapTime(UUID As String) As Integer

        Return CInt("0" & GetMySetting("MapTime_" & Region_Name(UUID), "0"))

    End Function

    Public Function GetMySetting(key As String, Optional D As String = "") As String

        Return CStr(Settings.GetIni("Data", key, D, "String"))

    End Function

    ''' <summary>
    ''' Saves the time it took to boot w/o maps
    ''' </summary>
    ''' <param name="DT">Time in Seconds to boot</param>
    ''' <param name="UUID">UUID of region</param>
    Public Sub SaveBootTime(DT As Integer, UUID As String)

        SetMySetting("BootTime_" & Region_Name(UUID), CStr(DT))

    End Sub

    ''' <summary>
    ''' Saves the time it took to boot with maps
    ''' </summary>
    ''' <param name="DT">Time in Seconds to boot</param>
    ''' <param name="UUID">UUID of region</param>
    Public Sub SaveMapTime(DT As Integer, UUID As String)

        SetMySetting("MapTime_" & Region_Name(UUID), CStr(DT))

    End Sub

    Public Sub SaveSettings()

        Settings.SaveIni()

    End Sub

    Public Sub SetMySetting(key As String, value As String)

        If value Is Nothing Then Return
        Settings.SetIni("Data", key, value.ToString(Globalization.CultureInfo.InvariantCulture))

    End Sub

#End Region

#Region "Properties"

    ''' <summary>
    ''' Diva will set Logins to disabled (-1) if this switch is set
    ''' </summary>
    ''' <returns>AccountConfirmationRequired as boolean</returns>
    Public Property AccountConfirmationRequired() As Boolean
        Get
            Return CType(GetMySetting("AccountConfirmationRequired", "False"), Boolean)
        End Get
        Set
            SetMySetting("AccountConfirmationRequired", CStr(Value))
        End Set
    End Property

    ''' <summary>
    ''' Diva Wifi Email address
    ''' </summary>
    ''' <returns>AdminEmail</returns>
    Public Property AdminEmail() As String
        Get
            Dim mail As String = GetMySetting("AdminEmail", "not@set.yet")
            Return mail
        End Get
        Set
            SetMySetting("AdminEmail", Value)
        End Set
    End Property

    ''' <summary>
    ''' Diva Wifi User Name (Wifi)
    ''' </summary>
    ''' <returns>Wifi</returns>
    Public Property AdminFirst() As String
        Get
            Return GetMySetting("AdminFirst", "Wifi")
        End Get
        Set
            SetMySetting("AdminFirst", Value)
        End Set
    End Property

    ''' <summary>
    ''' Diva Wifi User Name (Wifi)
    ''' </summary>
    ''' <returns>Admin</returns>
    Public Property AdminLast() As String
        Get
            Return GetMySetting("AdminLast", "Admin")
        End Get
        Set
            SetMySetting("AdminLast", Value)
        End Set
    End Property

    ''' <summary>
    ''' Allow Gods - without this no gods are possible
    ''' </summary>
    ''' <returns>Boolean</returns>
    Public Property AllowGridGods() As Boolean
        Get
            Return CType(GetMySetting("Allow_grid_gods", "False"), Boolean)
        End Get
        Set
            SetMySetting("Allow_grid_gods", CStr(Value))
        End Set

    End Property

    ''' <summary>
    ''' Allow all Plants to be set
    ''' </summary>
    ''' <returns>Boolean</returns>
    Public Property AllPlants() As Boolean
        Get
            Return CType(GetMySetting("AllPlants", "False"), Boolean)
        End Get
        Set
            SetMySetting("AllPlants", CStr(Value))
        End Set
    End Property

    ''' <summary>
    ''' Show the Robo copy window
    ''' </summary>
    Public Property AltDnsName() As String
        Get
            Return GetMySetting("AltDnsName", "")
        End Get
        Set
            SetMySetting("AltDnsName", Value)
        End Set
    End Property

    ''' <summary>
    '''True or False that Apache will run
    ''' </summary>
    ''' <returns></returns>
    Public Property ApacheEnable() As Boolean
        Get
            Return CType(GetMySetting("ApacheEnabled", "False"), Boolean)
        End Get
        Set
            SetMySetting("ApacheEnabled", CStr(Value))
        End Set
    End Property

    ''' <summary>
    ''' Apache Port
    ''' </summary>
    ''' <returns>80 by default</returns>
    Public Property ApachePort() As Integer
        Get
            Return CInt("0" & GetMySetting("ApachePort", "80".ToUpperInvariant))
        End Get
        Set
            SetMySetting("ApachePort", CStr(Value))
        End Set
    End Property

    ''' <summary>
    ''' Last Apache Revision
    ''' </summary>
    '''
    Public Property ApacheRev() As String
        Get
            Return GetMySetting("ApacheRev", "")
        End Get
        Set
            SetMySetting("ApacheRev", Value)
        End Set
    End Property

    ''' <summary>
    ''' Does Apache run as a service?
    ''' </summary>
    ''' <returns>Boolean</returns>
    Public Property ApacheService() As Boolean
        Get
            Return CType(GetMySetting("ApacheService", "False"), Boolean)
        End Get
        Set
            SetMySetting("ApacheService", CStr(Value))
        End Set
    End Property

    ''' <summary>
    ''' Text to Speech API key for security
    ''' </summary>
    ''' <returns>A large random number</returns>
    Public Property APIKey() As String
        Get
            Dim Key = Random()
            Return GetMySetting("APIKey")
        End Get
        Set
            SetMySetting("APIKey", Value)
        End Set
    End Property

    ''' <summary>
    ''' Sort Ascent or Descend in OARS
    ''' </summary>
    ''' <returns>Boolean</returns>
    Public Property AscendOrDescend() As Boolean
        Get
            Return CType(GetMySetting("AscendOrDescend", "True"), Boolean)
        End Get
        Set
            SetMySetting("AscendOrDescend", CStr(Value))
        End Set

    End Property

    ''' <summary>
    ''' Run Autobackup?
    ''' </summary>
    ''' <returns>Boolean</returns>
    Public Property AutoBackup() As Boolean
        Get
            Return CType(GetMySetting("AutoBackup", "True"), Boolean)
        End Get
        Set
            SetMySetting("AutoBackup", CStr(Value))
        End Set
    End Property

    ''' <summary>
    ''' How often to run autobackup in minutes
    ''' </summary>
    ''' <returns>1/2 (720 minutes) as a default</returns>
    Public Property AutobackupInterval() As String
        Get
            Return GetMySetting("AutobackupInterval", "720")
        End Get
        Set
            SetMySetting("AutobackupInterval", Value)
        End Set
    End Property

    ''' <summary>
    ''' Auto fill regions surrounding where an avatar is
    ''' </summary>
    ''' <returns>Boolean</returns>
    Public Property AutoFill() As Boolean
        Get
            Return CType(GetMySetting("AutoFill", "False"), Boolean)
        End Get
        Set
            SetMySetting("AutoFill", CStr(Value))
        End Set
    End Property

    ''' <summary>
    ''' AutoRestart a crashed region is Enabled?
    ''' </summary>
    ''' <returns>Boolean</returns>
    Public Property AutoRestartEnabled() As Boolean
        Get
            Return CType(GetMySetting("AutoRestartEnabled", "False"), Boolean)
        End Get
        Set
            SetMySetting("AutoRestartEnabled", CStr(Value))
        End Set
    End Property

    ''' <summary>
    ''' Auto Restart Interval in minutes
    ''' </summary>
    ''' <returns>0 (off) as a default</returns>
    Public Property AutoRestartInterval() As Integer
        Get
            Return CInt("0" & GetMySetting("AutoRestartInterval", "0".ToUpperInvariant))
        End Get
        Set
            SetMySetting("AutoRestartInterval", CStr(Value))
        End Set
    End Property

    ''' <summary>
    ''' Auto click the Start button  when booted
    ''' </summary>
    ''' <returns>boolean</returns>
    Public Property Autostart() As Boolean
        Get
            Return CType(GetMySetting("Autostart", "False"), Boolean)
        End Get
        Set
            SetMySetting("Autostart", CStr(Value))
        End Set
    End Property

    ''' <summary>
    ''' TYhe folder to save Auto Backups to
    ''' </summary>
    ''' <returns>Default: AutoBackup folder</returns>
    Public Property BackupFolder() As String
        Get
            Return GetMySetting("BackupFolder", "AutoBackup")
        End Get
        Set
            SetMySetting("BackupFolder", Value)
        End Set
    End Property

    ''' <summary>
    ''' Run robocopy to back up assets?
    ''' </summary>
    ''' <returns>Boolean</returns>
    Public Property BackupFSAssets() As Boolean
        Get
            Return CType(GetMySetting("BackupFSAssets", "False"), Boolean)
        End Get
        Set
            SetMySetting("BackupFSAssets", CStr(Value))
        End Set
    End Property

    ''' <summary>
    ''' Backup IARS?
    ''' </summary>
    ''' <returns>Boolean</returns>
    Public Property BackupIars() As Boolean
        Get
            Return CType(GetMySetting("BackupIARs", "False"), Boolean)
        End Get
        Set
            SetMySetting("BackupIARs", CStr(Value))
        End Set
    End Property

    ''' <summary>
    ''' Backup Oars?
    ''' </summary>
    ''' <returns>Boolean</returns>
    Public Property BackupOARs() As Boolean
        Get
            Return CType(GetMySetting("BackupOARs", "True"), Boolean)
        End Get
        Set
            SetMySetting("BackupOARs", CStr(Value))
        End Set
    End Property

    ''' <summary>
    ''' Backup Region INIs?
    ''' </summary>
    ''' <returns>Boolean</returns>
    Public Property BackupRegion() As Boolean
        Get
            Return CType(GetMySetting("BackupRegion", "True"), Boolean)
        End Get
        Set
            SetMySetting("BackupRegion", CStr(Value))
        End Set
    End Property

    ''' <summary>
    ''' Backup Settings.ini and XYSettings.ini?
    ''' </summary>
    ''' <returns>Boolean</returns>
    Public Property BackupSettings() As Boolean
        Get
            Return CType(GetMySetting("BackupSettings", "True"), Boolean)
        End Get
        Set
            SetMySetting("BackupSettings", CStr(Value))
        End Set
    End Property

    Public Property BackupSQL() As Boolean
        Get
            Return CType(GetMySetting("BackupSQL", "True"), Boolean)
        End Get
        Set
            SetMySetting("BackupSQL", CStr(Value))
        End Set
    End Property

    Public Property BackupWifi() As Boolean
        Get
            Return CType(GetMySetting("BackupWifi", "True"), Boolean)
        End Get
        Set
            SetMySetting("BackupWifi", CStr(Value))
        End Set
    End Property

    Public Property BanList() As String
        Get
            Return GetMySetting("BanList", "")
        End Get
        Set
            SetMySetting("BanList", Value)
        End Set
    End Property

    Public Property BaseDirectory() As String
        Get
            Return GetMySetting("BaseDirectory", "./fsassets")
        End Get
        Set
            SetMySetting("BaseDirectory", Value)
        End Set
    End Property

    Public Property BaseHostName() As String
        Get
            Return GetMySetting("BaseHostName", DnsName)
        End Get
        Set
            SetMySetting("BaseHostName", Value)
        End Set
    End Property

    ''' <summary>how close to the edge of a region can we get?</summary>
    ''' <returns></returns>
    Public Property BirdsBorderSize() As Double
        Get
            Try
                Return Convert.ToDouble(GetMySetting("BirdsBorderSize", "25"), Globalization.CultureInfo.InvariantCulture)
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
            Return 25
        End Get
        Set
            SetMySetting("BirdsBorderSize", CStr(Value))
        End Set
    End Property

    ''' <summary>which channel do we listen on for in world commands</summary>
    ''' <returns></returns>
    Public Property BirdsChatChannel() As Integer
        Get
            Return CInt("0" & GetMySetting("BirdsChatChannel", "118"))
        End Get
        Set
            SetMySetting("BirdsChatChannel", CStr(Value))
        End Set
    End Property

    ''' <summary>how far away from other birds we would like to stay</summary>
    ''' <returns></returns>
    Public Property BirdsDesiredSeparation() As Double
        Get
            Try
                Return Convert.ToDouble(GetMySetting("BirdsDesiredSeparation", "5"), Globalization.CultureInfo.InvariantCulture)
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
            Return 5
        End Get
        Set
            SetMySetting("BirdsDesiredSeparation", CStr(Value))
        End Set
    End Property

    Public Property BirdsEnabled() As Boolean
        Get
            Return CType(GetMySetting("BirdsEnabled", "False"), Boolean)
        End Get
        Set
            SetMySetting("BirdsEnabled", CStr(Value))
        End Set
    End Property

    Public Property BirdsFlockSize() As Integer
        Get
            Return CInt("0" & GetMySetting("BirdsFlockSize", "25"))
        End Get
        Set
            SetMySetting("BirdsFlockSize", CStr(Value))
        End Set
    End Property

    Public Property BirdsMaxForce() As Double
        Get
            Try
                Return Convert.ToDouble(GetMySetting("BirdsMaxForce", "0.2"), Globalization.CultureInfo.InvariantCulture)
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
            Return 0.2
        End Get
        Set
            SetMySetting("BirdsMaxForce", CStr(Value))
        End Set
    End Property

    ''' <summary>how high are we allowed to flock</summary>
    ''' <returns></returns>
    Public Property BirdsMaxHeight() As Double
        Get
            Try
                Return Convert.ToDouble(GetMySetting("BirdsMaxHeight", "25"), Globalization.CultureInfo.InvariantCulture)
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
            Return 25
        End Get
        Set
            SetMySetting("BirdsMaxHeight", CStr(Value))
        End Set
    End Property

    ''' <summary>'how far each bird can travel per update</summary>
    ''' <returns></returns>
    Public Property BirdsMaxSpeed() As Double
        Get
            Try
                Return Convert.ToDouble(GetMySetting("BirdsMaxSpeed", "1.0"), Globalization.CultureInfo.InvariantCulture)
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
            Return 1.0
        End Get
        Set
            SetMySetting("BirdsMaxSpeed", CStr(Value))
        End Set
    End Property

    Public Property BirdsModuleStartup() As Boolean
        Get
            Return CType(GetMySetting("BirdsModuleStartup", "False"), Boolean)
        End Get
        Set
            SetMySetting("BirdsModuleStartup", CStr(Value))
        End Set
    End Property

    ''' <summary>max distance for other birds to be considered in the same flock as us</summary>
    ''' <returns></returns>
    Public Property BirdsNeighbourDistance() As Double
        Get
            Try
                Return Convert.ToDouble(GetMySetting("BirdsNeighbourDistance", "25"), Globalization.CultureInfo.InvariantCulture)
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
            Return 25
        End Get
        Set
            SetMySetting("BirdsNeighbourDistance", CStr(Value))
        End Set
    End Property

    ''' <summary>
    ''' By default the module will create a flock of plain wooden spheres, however this can be overridden to the name of an existing prim that needs to already exist in the scene - i.e. be rezzed in
    ''' the region.
    ''' </summary>
    ''' <returns></returns>
    Public Property BirdsPrim() As String
        Get
            Return GetMySetting("BirdsPrim", "SeaGull1")
        End Get
        Set
            SetMySetting("BirdsPrim", Value)
        End Set
    End Property

    ''' <summary>how close to the edges of things can we get without being worried</summary>
    ''' <returns></returns>
    Public Property BirdsTolerance() As Double
        Get
            Try
                Return Convert.ToDouble(GetMySetting("BirdsTolerance", "25"), Globalization.CultureInfo.InvariantCulture)
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
            Return 25
        End Get
        Set
            SetMySetting("BirdsTolerance", CStr(Value))
        End Set
    End Property

    Public Property BootOrSuspend() As Boolean
        Get
            Return CType(GetMySetting("BootOrSuspend", "False"), Boolean)
        End Get
        Set
            SetMySetting("BootOrSuspend", CStr(Value))
        End Set
    End Property

    Public Property BroadcastTideInfo() As Boolean
        Get
            Return CType(GetMySetting("BroadcastTideInfo", "True"), Boolean)
        End Get
        Set
            SetMySetting("BroadcastTideInfo", CStr(Value))
        End Set
    End Property

    ''' <summary>
    ''' BulkLoadOwner User Name
    ''' </summary>
    ''' <returns></returns>
    Public Property BulkLoadOwner() As String
        Get
            Return GetMySetting("BulkLoadOwner", "")
        End Get
        Set
            SetMySetting("BulkLoadOwner", Value)
        End Set
    End Property

    Public Property CacheEnabled() As Boolean
        Get
            Return CType(GetMySetting("CacheEnabled", "True"), Boolean)
        End Get
        Set
            SetMySetting("CacheEnabled", CStr(Value))
        End Set
    End Property

    Public Property CacheFolder() As String
        Get
            Return GetMySetting("CacheFolder", ".\assetcache")
        End Get
        Set
            SetMySetting("CacheFolder", Value)
        End Set
    End Property

    Public Property CacheLogLevel() As String
        Get
            Return GetMySetting("CacheLogLevel", "0".ToUpperInvariant)
        End Get
        Set
            SetMySetting("CacheLogLevel", Value)
        End Set
    End Property

    Public Property CacheTimeout() As String
        Get
            Return GetMySetting("CacheTimeout", "4")
        End Get
        Set
            SetMySetting("CacheTimeout", Value)
        End Set
    End Property

    Public Property Categories() As String
        Get
            Return GetMySetting("Categories")
        End Get
        Set
            SetMySetting("Categories", Value)
        End Set
    End Property

    Public Property CMS() As String
        Get
            Dim var = GetMySetting("CMS", DreamGrid)
            If var = "Joomla" Then var = JOpensim
            Dim installed As Boolean = Joomla.IsjOpensimInstalled()
            If (Not installed) & SearchOptions = JOpensim Then
                Return DreamGrid
            End If
            Return var
        End Get
        Set
            SetMySetting("CMS", CStr(Value))
        End Set
    End Property

    Public Property Concierge() As Boolean
        Get
            Return CType(GetMySetting("Concierge", "True"), Boolean)
        End Get
        Set
            SetMySetting("Concierge", CStr(Value))
        End Set
    End Property

    Public Property ConsolePass() As String
        Get
            Return GetMySetting("ConsolePass")
        End Get
        Set
            SetMySetting("ConsolePass", Value)
        End Set
    End Property

    Public Property ConsoleShow() As String
        Get
            Return GetMySetting("ConsoleShow", "False")
        End Get
        Set
            SetMySetting("ConsoleShow", Value)
        End Set
    End Property

    Public Property ConsoleUser() As String
        Get
            Return GetMySetting("ConsoleUser")
        End Get
        Set
            SetMySetting("ConsoleUser", Value)
        End Set
    End Property

    Public Property CoordX() As Integer
        Get
            Return CInt("0" & GetMySetting("CoordX", CStr(RandomNumber.Between(990, 1010))))
        End Get
        Set
            SetMySetting("CoordX", CStr(Value))
        End Set
    End Property

    Public Property CoordY() As Integer
        Get
            Return CInt("0" & GetMySetting("CoordY", CStr(RandomNumber.Between(990, 1010))))
        End Get
        Set
            SetMySetting("CoordY", CStr(Value))
        End Set
    End Property

    Public Property CpuMax As Single
        Get
            Return CType(GetMySetting("CPUMax", "90"), Single)
        End Get
        Set(value As Single)
            SetMySetting("CPUMax", CStr(value))
        End Set
    End Property

    Public Property CpuPatched() As Boolean
        Get
            Return CType(GetMySetting("CPUPatched", "False"), Boolean)
        End Get
        Set
            SetMySetting("CPUPatched", CStr(Value))
        End Set
    End Property

    Public Property CurrentDirectory() As String
        Get
            Return GetMySetting("Myfolder") ' no default
        End Get
        Set
            SetMySetting("Myfolder", Value) ' DEBUG
        End Set
    End Property

    ''' <summary>
    ''' Mysql requires slashes in the "/" Unix style
    ''' </summary>
    ''' <returns>The current directory with '/'</returns>
    Public Property CurrentSlashDir As String
        Get
            Return _CurSlashDir
        End Get
        Set(value As String)
            _CurSlashDir = value
        End Set
    End Property

    Public Property CycleTime() As Integer
        Get
            Return CInt("0" & GetMySetting("CycleTime", "900"))
        End Get
        Set
            SetMySetting("CycleTime", CStr(Value))
        End Set
    End Property

    Public Property DeleteByDate() As Boolean
        Get
            Return CType(GetMySetting("DeleteByDate", "True"), Boolean)
        End Get
        Set
            SetMySetting("DeleteByDate", CStr(Value))
        End Set
    End Property

    Public Property DeleteScriptsOnStartupLevel() As String
        Get
            Return GetMySetting("DeleteScriptsOnStartupLevel", "")
        End Get
        Set
            SetMySetting("DeleteScriptsOnStartupLevel", Value)
        End Set
    End Property

    Public Property DeleteTreesFirst() As Boolean
        Get
            Return _DeleteTreesFirst
        End Get
        Set
            _DeleteTreesFirst = Value
        End Set
    End Property

    Public Property Density() As Double
        Get
            Try
                Return Convert.ToDouble(GetMySetting("Density", "0.5"), Globalization.CultureInfo.InvariantCulture)
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
            Return 0.5
        End Get
        Set
            SetMySetting("Density", CStr(Value))
        End Set
    End Property

    Public Property DeregisteredOnce() As Boolean
        Get
            Return CType(GetMySetting("DeregisteredOnce", "False"), Boolean)
        End Get
        Set
            SetMySetting("DeregisteredOnce", CStr(Value))
        End Set
    End Property

    Public Property Description() As String
        Get
            Return GetMySetting("Description")
        End Get
        Set
            SetMySetting("Description", Value)
        End Set
    End Property

    Public Property DiagFailed() As String
        Get
            Return GetMySetting("DiagFailed", "False")
        End Get
        Set
            SetMySetting("DiagFailed", Value)
        End Set
    End Property

    Public Property DiagnosticPort() As Integer
        Get
            Return CInt("0" & GetMySetting("DiagnosticPort", "8001"))
        End Get
        Set
            SetMySetting("DiagnosticPort", CStr(Value))
        End Set
    End Property

    Public Property DnsName() As String
        Get
            Return GetMySetting("DnsName")
        End Get
        Set
            SetMySetting("DnsName", Value)
        End Set
    End Property

    Public Property DoSQLBackup() As Boolean
        Get
            Return CType(GetMySetting("DoSQLBackup", "False"), Boolean)
        End Get
        Set
            SetMySetting("DoSQLBackup", CStr(Value))
        End Set

    End Property

    Public Property DotnetUpgraded() As Boolean
        Get
            Return CType(GetMySetting("DotnetUpgraded", "False"), Boolean)
        End Get
        Set
            SetMySetting("DotnetUpgraded", CStr(Value))
        End Set
    End Property

    ''' <summary>
    ''' Period in seconds to delay after an email Is sent.
    ''' </summary>
    ''' <returns>integer, default of 20</returns>
    Public Property Email_pause_time() As Integer
        Get
            Return CInt("0" & GetMySetting("Email_pause_time", "20"))
        End Get
        Set
            SetMySetting("Email_pause_time", CStr(Value))
        End Set
    End Property

    ''' <summary>
    ''' If on, emails are enabled.
    ''' </summary>
    ''' <returns>boolean</returns>
    Public Property EmailEnabled() As Boolean
        Get
            Return CType(GetMySetting("EmailEnabled", "False"), Boolean)
        End Get
        Set
            SetMySetting("EmailEnabled", CStr(Value))
        End Set

    End Property

    ''' <summary>
    ''' maximum number of emails via smtp per hour
    ''' </summary>
    ''' <returns></returns>
    Public Property EmailsToSMTPAddressPerHour() As Integer
        Get
            Return CInt("0" & GetMySetting("EmailsToSMTPAddressPerHour", "10"))
        End Get
        Set
            SetMySetting("EmailsToSMTPAddressPerHour", CStr(Value))
        End Set
    End Property

    ''' <summary>
    ''' Enable sending email to regions not on current opensimulator instance.
    ''' </summary>
    ''' <returns></returns>
    Public Property EnableEmailToExternalObjects() As Boolean
        Get
            Return CType(GetMySetting("enableEmailToExternalObjects", "False"), Boolean)
        End Get
        Set
            SetMySetting("enableEmailToExternalObjects", CStr(Value))
        End Set

    End Property

    ''' <summary>
    ''' Enable sending email to the world
    ''' </summary>
    ''' <returns></returns>
    Public Property EnableEmailToSMTPCheckBox() As Boolean
        Get
            Return CType(GetMySetting("enableEmailToSMTPCheckBox", "False"), Boolean)
        End Get
        Set
            SetMySetting("enableEmailToSMTPCheckBox", CStr(Value))
        End Set

    End Property

    Public Property EnableHypergrid() As Boolean
        Get
            Return CType(GetMySetting("EnableHypergrid", "True"), Boolean)
        End Get
        Set
            SetMySetting("EnableHypergrid", CStr(Value))
        End Set

    End Property

    ''' <summary>
    ''' Exports all asserts to fsassets, once
    ''' </summary>
    ''' <returns>True is exported as boolean</returns>
    Public Property ExportAssetsOnce() As Boolean
        Get
            Return CType(GetMySetting("ExportAssetsOnce", "False"), Boolean)
        End Get
        Set
            SetMySetting("ExportAssetsOnce", CStr(Value))
        End Set
    End Property

    Public Property ExternalHostName() As String
        Get
            Return _ExternalHostName
        End Get
        Set
            _ExternalHostName = Value
        End Set
    End Property

    Public Property FirewallMigrated() As Integer
        Get
            Try
                Return CType("0" & GetMySetting("FirewallMigrated", "0"), Integer)
            Catch
                Return 1
            End Try
        End Get
        Set
            SetMySetting("FirewallMigrated", CStr(Value))
        End Set
    End Property

    Public Property FirstRegionPort() As Integer
        Get
            Return CInt("0" & GetMySetting("FirstRegionPort", "8004"))
        End Get
        Set
            SetMySetting("FirstRegionPort", CStr(Value))
        End Set
    End Property

    Public Property FlatlandLevel() As Double
        Get
            Try
                Return Convert.ToDouble(GetMySetting("FlatLandLevel", "21"), Globalization.CultureInfo.InvariantCulture)
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
            Return 21
        End Get
        Set
            SetMySetting("FlatLandLevel", CStr(Value))
        End Set
    End Property

    ' fsassets
    Public Property FSAssetsEnabled() As Boolean
        Get
            Return CType(GetMySetting("FsAssetsEnabled", "True"), Boolean)
        End Get
        Set
            SetMySetting("FsAssetsEnabled", CStr(Value))
        End Set
    End Property

    Public Property Gdpr() As Boolean
        Get
            Return CType(GetMySetting("GDPR", "False"), Boolean)
        End Get
        Set
            SetMySetting("GDPR", CStr(Value))
        End Set
    End Property

    Public Property GlbOwnerEmail() As String
        Get
            Return GetMySetting("GLBOwnerEmail")
        End Get
        Set
            SetMySetting("GLBOwnerEmail", Value)
        End Set
    End Property

    Public Property GlbOwnerName() As String
        Get
            Return GetMySetting("GLBOwnerName")
        End Get
        Set
            SetMySetting("GLBOwnerName", Value)
        End Set
    End Property

    ''' <summary>Show authorization Instant Message to user at session start?</summary>
    ''' <returns>False</returns>
    Public Property GlbShowNewSessionAuthIM() As Boolean
        Get
            Return CType(GetMySetting("GLBShowNewSessionAuthIM", "False"), Boolean)
        End Get
        Set
            SetMySetting("GLBShowNewSessionAuthIM", CStr(Value))
        End Set
    End Property

    ''' <summary>Show purchase Gloebit Instant Message to user at session start?</summary>
    ''' <returns>False</returns>
    Public Property GlbShowNewSessionPurchaseIM() As Boolean
        Get
            Return CType(GetMySetting("GLBShowNewSessionPurchaseIM", "False"), Boolean)
        End Get
        Set
            SetMySetting("GLBShowNewSessionPurchaseIM", CStr(Value))
        End Set
    End Property

    ''' <summary>Show welcome message when entering a region?</summary>
    ''' <returns>True</returns>
    Public Property GlbShowWelcomeMessage() As Boolean
        Get
            Return CType(GetMySetting("GLBShowWelcomeMessage", "True"), Boolean)
        End Get
        Set
            SetMySetting("GLBShowWelcomeMessage", CStr(Value))
        End Set
    End Property

    Public Property GloebitsEnable() As Boolean
        Get
            Return CType(GetMySetting("GloebitsEnable", "False"), Boolean)
        End Get
        Set
            SetMySetting("GloebitsEnable", CStr(Value))
        End Set
    End Property

    Public Property GLProdKey() As String
        Get
            Return GetMySetting("GLProdKey")
        End Get
        Set
            SetMySetting("GLProdKey", Value)
        End Set
    End Property

    Public Property GLProdSecret() As String
        Get
            Return GetMySetting("GLProdSecret")
        End Get
        Set
            SetMySetting("GLProdSecret", Value)
        End Set
    End Property

    Public Property GraphVisible() As Boolean
        Get
            Return CType(GetMySetting("GraphVisible", "True"), Boolean)
        End Get
        Set
            SetMySetting("GraphVisible", CStr(Value))
        End Set
    End Property

    Public Property HomeVectorX() As String
        Get
            Return GetMySetting("HomeVectorX", "128")
        End Get
        Set
            SetMySetting("HomeVectorX", Value)
        End Set
    End Property

    Public Property HomeVectorY() As String
        Get
            Return GetMySetting("HomeVectorY", "128")
        End Get
        Set
            SetMySetting("HomeVectorY", Value)
        End Set
    End Property

    Public Property HomeVectorZ() As String
        Get
            Return GetMySetting("HomeVectorZ", "24")
        End Get
        Set
            SetMySetting("HomeVectorZ", Value)
        End Set
    End Property

    Public Property HttpPort() As Integer
        Get
            Return CInt("0" & GetMySetting("HttpPort", "8002"))
        End Get
        Set
            SetMySetting("HttpPort", CStr(Value))
        End Set
    End Property

    Public Property InstalledRuntime() As Boolean
        Get
            Return CType(GetMySetting("InstalledRuntime", "True"), Boolean)
        End Get
        Set
            SetMySetting("InstalledRuntime", CStr(Value))
        End Set

    End Property

    Public Property KeepForDays() As Integer
        Get
            Return CInt("0" & GetMySetting("KeepForDays", "3"))
        End Get
        Set
            SetMySetting("KeepForDays", CStr(Value))
        End Set
    End Property

    Public Property KeepOnTop() As Boolean
        Get
            Return CType(GetMySetting("KeepOnTopRegionList", "False"), Boolean)
        End Get
        Set
            SetMySetting("KeepOnTopRegionList", CStr(Value))
        End Set
    End Property

    Public Property KeepOnTopMain() As Boolean
        Get
            Return CType(GetMySetting("KeepOnTopMain", "False"), Boolean)
        End Get
        Set
            SetMySetting("KeepOnTopMain", CStr(Value))
        End Set
    End Property

    Public Property KeepVisits() As Integer
        Get
            Return CInt("0" & GetMySetting("KeepVisits", "365"))
        End Get
        Set
            SetMySetting("KeepVisits", CStr(Value))
        End Set
    End Property

    Public Property LandNoise() As Boolean
        Get
            Return CType(GetMySetting("LandNoise", "False"), Boolean)
        End Get
        Set
            SetMySetting("LandNoise", CStr(Value))
        End Set
    End Property

    Public Property LandSmooth() As Boolean
        Get
            Return CType(GetMySetting("LandSmooth", "False"), Boolean)
        End Get
        Set
            SetMySetting("LandSmooth", CStr(Value))
        End Set
    End Property

    Public Property LandSmoothValue() As Double
        Get
            Try

                Return Convert.ToDouble(GetMySetting("LandSmoothValue", "0.5"), Globalization.CultureInfo.InvariantCulture)
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
            Return 0.5
        End Get
        Set
            SetMySetting("LandSmoothValue", CStr(Value))
        End Set
    End Property

    Public Property LandStrength() As Double
        Get
            Try
                Return Convert.ToDouble(GetMySetting("LandStrength", "1.0"), Globalization.CultureInfo.InvariantCulture)
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
            Return 10
        End Get
        Set
            SetMySetting("LandStrength", CStr(Value))
        End Set
    End Property

    Public Property LandTaper() As Double
        Get
            Try
                Return Convert.ToDouble(GetMySetting("LandTaper", "0.6"), Globalization.CultureInfo.InvariantCulture)
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
            Return 0.6
        End Get
        Set
            SetMySetting("LandTaper", CStr(Value))
        End Set
    End Property

    Public Property Language() As String
        Get
            Return GetMySetting("Language", "en")
        End Get
        Set
            SetMySetting("Language", Value)
        End Set
    End Property

    Public Property LANIP() As String
        Get
            Return _LANIP
        End Get
        Set
            _LANIP = Value
        End Set
    End Property

    Public Property LastDirectory() As String
        Get
            Return GetMySetting("LastDirectory") ' no default
        End Get
        Set
            SetMySetting("LastDirectory", Value)
        End Set
    End Property

    'LocalServiceModule
    Public Property LocalServiceModule() As String
        Get
            Return GetMySetting("LocalServiceModule", "OpenSim.Services.AssetService.dll: AssetService")
        End Get
        Set
            SetMySetting("LocalServiceModule", Value)
        End Set
    End Property

    Public Property LogBenchmarks() As Boolean
        Get
            Return CType(GetMySetting("LogBenchmarks", "False"), Boolean)
        End Get
        Set
            SetMySetting("LogBenchmarks", CStr(Value))
        End Set
    End Property

    Public Property Logger() As String
        Get
            Return GetMySetting("Logger", "Baretail")
        End Get
        Set
            SetMySetting("Logger", Value)
        End Set
    End Property

    Public Property LogLevel() As String
        Get
            Return GetMySetting("LogLevel", "INFO")
        End Get
        Set
            SetMySetting("LogLevel", Value)
        End Set
    End Property

    Public Property LoopbackDiag() As Boolean
        Get
            Return CType(GetMySetting("LoopBackDiag", "True"), Boolean)
        End Get
        Set
            SetMySetting("LoopBackDiag", CStr(Value))
        End Set
    End Property

    Public Property LSLHTTP() As Boolean
        Get
            Return CType(GetMySetting("LSL_HTTP", "False"), Boolean)
        End Get
        Set
            SetMySetting("LSL_HTTP", CStr(Value))
        End Set
    End Property

    Public Property MacAddress() As String
        Get
            Return _MacAddress
        End Get
        Set
            _MacAddress = Value
        End Set
    End Property

    Public Property MachineId() As String
        Get
            Return GetMySetting("MachineID")
        End Get
        Set
            SetMySetting("MachineID", Value)
        End Set
    End Property

    ''' <summary>
    ''' maximum number of emails from a object owner per hour
    ''' </summary>
    Public Property MailsFromOwnerPerHour() As Integer
        Get
            Return CInt("0" & GetMySetting("MailsFromOwnerPerHour", "500"))
        End Get
        Set
            SetMySetting("MailsFromOwnerPerHour", CStr(Value))
        End Set
    End Property

    ''' <summary>
    ''' maximum number of emails via smtp per day
    ''' </summary>
    ''' <returns></returns>
    Public Property MailsPerDay() As Integer
        Get
            Return CInt("0" & GetMySetting("MailsPerDay", "100"))
        End Get
        Set
            SetMySetting("MailsPerDay", CStr(Value))
        End Set
    End Property

    Public Property MailsToPrimAddressPerHour() As Integer
        Get
            Return CInt("0" & GetMySetting("MailsToPrimAddressPerHour", "20"))
        End Get
        Set
            SetMySetting("MailsToPrimAddressPerHour", CStr(Value))
        End Set
    End Property

    Public Property MapCenterX() As Integer
        Get
            If ServerType = RobustServerName Then
                Dim RegionUUID As String = FindRegionByName(WelcomeRegion)
                Dim Center As String = CStr(Coord_X(RegionUUID))
                Return CInt("0" & GetMySetting("MapCenterX", Center))
            Else
                Return 1000
            End If

        End Get
        Set
            SetMySetting("MapCenterX", CStr(Value))
        End Set
    End Property

    Public Property MapCenterY() As Integer
        Get
            If ServerType = RobustServerName Then
                Dim RegionUUID As String = FindRegionByName(WelcomeRegion)
                Dim Center As String = CStr(Coord_Y(RegionUUID))
                Return CInt("0" & GetMySetting("MapCenterY", Center))
            Else
                Return 1000
            End If

        End Get
        Set
            SetMySetting("MapCenterY", CStr(Value))
        End Set
    End Property

    Public Property MapType() As String
        Get
            Return GetMySetting("MapType", "Simple")
        End Get
        Set
            SetMySetting("MapType", Value)
        End Set
    End Property

    ''' <summary>
    ''' Maximum total size of email in bytes.
    ''' </summary>
    Public Property MaxMailSize() As Integer
        Get
            Return CInt("0" & GetMySetting("MaxMailSize", "4096"))
        End Get
        Set
            SetMySetting("MaxMailSize", CStr(Value))
        End Set
    End Property

    Public Property MinTimerInterval() As Double
        Get
            Try
                Dim value = Convert.ToDouble(GetMySetting("MinTimerInterval", "0.2"), Globalization.CultureInfo.InvariantCulture)
                If value < 0.05 Or value > 1 Then value = 0.2
                Return value
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
            Return 0.2
        End Get
        Set
            SetMySetting("MinTimerInterval", CStr(Value))
        End Set
    End Property

    Public Property MysqlLastDirectory() As String
        Get
            Return GetMySetting("MysqlLastDirectory", "")
        End Get
        Set
            SetMySetting("MysqlLastDirectory", Value)
        End Set
    End Property

    Public Property MySqlRegionDBPort() As Integer
        Get
            Return CInt("0" & GetMySetting("MySqlRegionDBPort", "3306"))
        End Get
        Set
            SetMySetting("MySqlRegionDBPort", CStr(Value))
        End Set
    End Property

    Public Property MysqlRev() As String
        Get
            Dim mail As String = GetMySetting("MysqlRev", "")
            Return mail
        End Get
        Set
            SetMySetting("MysqlRev", Value)
        End Set
    End Property

    Public Property MySqlRobustDBPort() As Integer
        Get
            Return CInt("0" & GetMySetting("MySqlRobustDBPort", "3306"))
        End Get
        Set
            SetMySetting("MySqlRobustDBPort", CStr(Value))
        End Set
    End Property

    Public Property MysqlRunasaService() As Boolean
        Get
            Return CType(GetMySetting("MysqlRunasaService", "False"), Boolean)
        End Get
        Set
            SetMySetting("MysqlRunasaService", CStr(Value))
        End Set
    End Property

    Public Property MyX() As Integer
        Get
            Return CInt("0" & GetMySetting("MyX", "0".ToUpperInvariant))
        End Get
        Set
            SetMySetting("MyX", CStr(Value))
        End Set
    End Property

    Public Property MyY() As Integer
        Get
            Return CInt("0" & GetMySetting("MyY", "0".ToUpperInvariant))
        End Get
        Set
            SetMySetting("MyY", CStr(Value))
        End Set
    End Property

    ''' <summary>
    ''' Sort Name or Date in OARS
    ''' </summary>
    ''' <returns>Boolean</returns>
    Public Property NameOrDate() As Boolean
        Get
            Return CType(GetMySetting("NameOrDate", "False"), Boolean)
        End Get
        Set
            SetMySetting("NameOrDate", CStr(Value))
        End Set

    End Property

    Public Property NinjaRagdoll() As Boolean
        Get
            Return CType(GetMySetting("NinjaRagdoll", "False"), Boolean)
        End Get
        Set
            SetMySetting("NinjaRagdoll", CStr(Value))
        End Set
    End Property

    Public Property NoPlants() As Boolean
        Get
            Return CType(GetMySetting("NoPlants", "False"), Boolean)
        End Get
        Set
            SetMySetting("NoPlants", CStr(Value))
        End Set
    End Property

    Public Property OarCount() As Integer
        Get
            Return _OarCount
        End Get
        Set
            _OarCount = Value
        End Set
    End Property

    Public Property OpensimBinPath() As String
        Get
            Return GetMySetting("OpensimBinPath")
        End Get
        Set
            SetMySetting("OpensimBinPath", Value)
        End Set
    End Property

    Public Property OperatingSystem() As String
        Get
            Return GetMySetting("OperatingSystem", "")
        End Get
        Set
            SetMySetting("OperatingSystem", Value)
        End Set
    End Property

    Public Property OtherCMS() As String
        Get
            Dim other As String = GetMySetting("OtherCMS", "Other")
            Return other
        End Get
        Set
            SetMySetting("OtherCMS", Value)
        End Set
    End Property

    Public Property OutboundPermissions() As Boolean
        Get
            Return CType(GetMySetting("OutBoundPermissions", "True"), Boolean)
        End Get
        Set
            SetMySetting("OutBoundPermissions", CStr(Value))
        End Set
    End Property

    Public Property OverrideName() As String
        Get
            Return GetMySetting("OverrideName", "")
        End Get
        Set
            SetMySetting("OverrideName", Value)
        End Set
    End Property

    Public Property ParkingLot() As String
        Get
            Return GetMySetting("ParkingLot", WelcomeRegion)
        End Get
        Set
            SetMySetting("ParkingLot", Value)
        End Set
    End Property

    Public Property Password() As String
        Get
            Return GetMySetting("Password")
        End Get
        Set
            SetMySetting("Password", Value)
        End Set
    End Property

    Public Property PemKey() As String
        Get
            Return CType(GetMySetting("pemKey", ""), String)
        End Get
        Set
            SetMySetting("pemKey", Value)
        End Set
    End Property

    Public Property Physics() As Integer
        Get
            Return CInt("0" & GetMySetting("Physics", "3"))
        End Get
        Set
            SetMySetting("Physics", CType(Value, String))
        End Set
    End Property

    Public Property Primlimits() As Boolean
        Get
            Return CType(GetMySetting("Primlimits", "False"), Boolean)
        End Get
        Set
            SetMySetting("Primlimits", CStr(Value))
        End Set
    End Property

    Public Property PrivatePort() As Integer
        Get
            Return CInt(Settings.GetIni("Data", "PrivatePort", "8003", "Integer"))
        End Get
        Set
            SetMySetting("PrivatePort", CStr(Value))
        End Set
    End Property

    Public Property PublicIP() As String
        Get
            Return GetMySetting("PublicIP")
        End Get
        Set
            SetMySetting("PublicIP", Value)
        End Set
    End Property

    Public Property PublicVisitorMaps() As Boolean
        Get
            Return CType(GetMySetting("PublicVisitorMaps", "False"), Boolean)
        End Get
        Set
            SetMySetting("PublicVisitorMaps", CStr(Value))
        End Set

    End Property

    Public Property QuikEditOff() As Boolean
        Get
            Return CType(GetMySetting("QuikEditOff", "False"), Boolean)
        End Get
        Set
            SetMySetting("QuikEditOff", CStr(Value))
        End Set
    End Property

    Public Property Ramused() As Double
        Get
            Return _RamUsed
        End Get
        Set
            _RamUsed = Value
        End Set
    End Property

    Public Property RanAllDiags() As Boolean
        Get
            Return CType(GetMySetting("RanAllDiags", "False"), Boolean)
        End Get
        Set
            SetMySetting("RanAllDiags", CStr(Value))
        End Set
    End Property

    Public Property RegionDBName() As String
        Get
            Return GetMySetting("RegionDBName", "opensim")
        End Get
        Set
            SetMySetting("RegionDBName", Value)
        End Set
    End Property

    Public Property RegionDbPassword() As String
        Get
            Return GetMySetting("RegionDbPassword", "opensimpassword")
        End Get
        Set
            SetMySetting("RegionDbPassword", Value)
        End Set
    End Property

    Public Property RegionDBUserName() As String
        Get
            Return GetMySetting("RegionDBUsername", "opensimuser")
        End Get
        Set
            SetMySetting("RegionDBUsername", Value)
        End Set
    End Property

    Public Property RegionListView() As Integer
        Get
            Return CInt("0" & GetMySetting("RegionListView", "2"))
        End Get
        Set
            SetMySetting("RegionListView", CStr(Value))
        End Set
    End Property

    Public Property RegionManagerIsGod() As Boolean
        Get
            Return CType(GetMySetting("Region_manager_is_god", "False"), Boolean)
        End Get
        Set
            SetMySetting("Region_manager_is_god", CStr(Value))
        End Set
    End Property

    Public Property RegionOwnerIsGod() As Boolean
        Get
            Return CType(GetMySetting("Region_owner_is_god", "False"), Boolean)
        End Get
        Set
            SetMySetting("Region_owner_is_god", CStr(Value))
        End Set
    End Property

    Public Property RegionServer() As String
        Get
            Return GetMySetting("RegionServer", "127.0.0.1")
        End Get
        Set
            SetMySetting("RegionServer", Value)
        End Set
    End Property

    Public Property RenderMaxHeight() As Double
        Get
            Try
                Return Convert.ToDouble(GetMySetting("RenderMaxHeight", "4096"), Globalization.CultureInfo.InvariantCulture)
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
            Return 4096

        End Get
        Set
            SetMySetting("RenderMaxHeight", CStr(Value))
        End Set
    End Property

    Public Property RenderMinHeight() As Integer
        Get
            Return CInt(GetMySetting("RenderMinHeight", "-100"))
        End Get
        Set
            SetMySetting("RenderMinHeight", CStr(Value))
        End Set
    End Property



    Public Property RestartOnCrash() As Boolean
        Get
            Return CType(GetMySetting("RestartOnCrash", "False"), Boolean)
        End Get
        Set
            SetMySetting("RestartOnCrash", CStr(Value))
        End Set
    End Property

    Public Property RestartonPhysics() As Boolean
        Get
            Return CType(GetMySetting("RestartonPhysics", "False"), Boolean)
        End Get
        Set
            SetMySetting("RestartonPhysics", CStr(Value))
        End Set
    End Property

    Public Property RobustDatabaseName() As String
        Get
            Return GetMySetting("RobustMySqlName", "Robust")
        End Get
        Set
            SetMySetting("RobustMySqlName", Value)
        End Set
    End Property

    Public Property RobustPassword() As String
        Get
            Return GetMySetting("RobustMySqlPassword", "robustpassword")
        End Get
        Set
            SetMySetting("RobustMySqlPassword", Value)
        End Set
    End Property

    Public Property RobustServerIP() As String
        Get
            Return GetMySetting("RobustServer", "127.0.0.1")
        End Get
        Set
            SetMySetting("RobustServer", CStr(Value))
        End Set
    End Property

    Public Property RobustUserName() As String
        Get
            Return GetMySetting("RobustMySqlUsername", "robustuser")
        End Get
        Set
            SetMySetting("RobustMySqlUsername", Value)
        End Set
    End Property

    ''' <summary>
    ''' Holds the mysql root user password
    ''' </summary>
    Public Property RootMysqlPassword() As String
        Get
            Return GetMySetting("RootMysqlPassword", "")
        End Get
        Set
            SetMySetting("RootMysqlPassword", Value)
        End Set
    End Property

    Public Property RunOnce() As Boolean
        Get
            Return CType(GetMySetting("RunOnce", "False"), Boolean)
        End Get
        Set
            SetMySetting("RunOnce", CStr(Value))
        End Set
    End Property

    Public Property SayDistance() As String
        Get
            Return GetMySetting("SayDistance", "20")
        End Get
        Set
            SetMySetting("SayDistance", Value)
        End Set
    End Property

    Public Property SCAdminPassword() As String
        Get
            Return GetMySetting("SC_AdminPassword", SCAdmin())
        End Get
        Set
            SetMySetting("SC_AdminPassword", Value)
        End Set
    End Property

    Public Property SCEnable() As Boolean
        Get
            Return CType(GetMySetting("SC_Enable", "False"), Boolean)
        End Get
        Set
            SetMySetting("SC_Enable", CStr(Value))
        End Set
    End Property

    Public Property SCPassword() As String
        Get
            Return GetMySetting("SC_Password", SCPass())
        End Get
        Set
            SetMySetting("SC_Password", Value)
        End Set
    End Property

    Public Property SCPortBase() As Integer
        Get
            Return CType("0" & GetMySetting("SC_PortBase", "8100"), Integer)
        End Get
        Set
            SetMySetting("SC_PortBase", CStr(Value))
        End Set
    End Property

    Public Property SCPortBase1() As Integer
        Get
            Return CType("0" & GetMySetting("SC_PortBase1", "8101"), Integer)
        End Get
        Set
            SetMySetting("SC_PortBase1", CStr(Value))
        End Set
    End Property

    Public Property ScriptEngine() As String
        Get
            Return GetMySetting("ScriptEngine", "YEngine")
        End Get
        Set
            SetMySetting("ScriptEngine", Value)
        End Set
    End Property

    Public Property SearchEnabled() As Boolean
        Get
            Return CType(GetMySetting("SearchEnabled", "True"), Boolean)
        End Get
        Set
            SetMySetting("SearchEnabled", CStr(Value))
        End Set
    End Property

    Public Property SearchMigration() As Integer
        Get
            Return CType("0" & GetMySetting("SearchMigration", "0"), Integer)
        End Get
        Set
            SetMySetting("SearchMigration", CStr(Value))
        End Set
    End Property

    Public Property SearchOptions() As String
        Get
            Return GetMySetting("OpensimSearch", "Outworldz")
        End Get
        Set
            SetMySetting("OpensimSearch", Value)
        End Set
    End Property

    ''' <summary>
    ''' 0 for no waiting
    ''' 1 for Sequential
    ''' 2 for concurrent
    ''' ''' </summary>
    Public Property SequentialMode() As Integer
        Get
            Return CInt("0" & GetMySetting("SequentialMode", "0"))
        End Get
        Set
            SetMySetting("SequentialMode", CType(Value, String))
        End Set
    End Property

    Public Property ServerType() As String
        Get
            Return GetMySetting("ServerType", "Robust")
        End Get
        Set
            SetMySetting("ServerType", Value)
        End Set
    End Property

    Public Property Settings As LoadIni
        Get
            Return _Settings
        End Get
        Set(value As LoadIni)
            _Settings = value
        End Set
    End Property

    Public Property ShoutDistance() As String
        Get
            Return GetMySetting("ShoutDistance", "100")
        End Get
        Set
            SetMySetting("ShoutDistance", Value)
        End Set
    End Property

    'ShowConsoleStats
    Public Property ShowConsoleStats() As String
        Get
            Return GetMySetting("ShowConsoleStats", "false")
        End Get
        Set
            SetMySetting("ShowConsoleStats", Value)
        End Set
    End Property

    Public Property ShowDateandTimeinLogs() As Boolean
        Get
            Return CType(GetMySetting("ShowDateandTimeinLogs", "True"), Boolean)
        End Get
        Set
            SetMySetting("ShowDateandTimeinLogs", CStr(Value))
        End Set
    End Property

    Public Property ShowFSAssetBackup() As Boolean
        Get
            Return CType(GetMySetting("ShowFsAssetBackup", "False"), Boolean)
        End Get
        Set
            SetMySetting("ShowFsAssetBackup", CStr(Value))
        End Set
    End Property

    ''' <summary>
    ''' SHows Mysql stats is enabled, which is laggy
    ''' </summary>
    ''' <returns>ShowMysqlStats as boolean</returns>
    Public Property ShowMysqlStats() As Boolean
        Get
            Return CType(GetMySetting("ShowMysqlStats", "False"), Boolean)
        End Get
        Set
            SetMySetting("ShowMysqlStats", CStr(Value))
        End Set
    End Property

    ''' <summary>
    ''' ShowRegionListOnBoot brings up the Region list if enabled
    ''' </summary>
    ''' <returns>T/F</returns>
    Public Property ShowRegionListOnBoot() As Boolean
        Get
            Return CBool(GetMySetting("ShowRegionListOnBoot", "True"))
        End Get
        Set
            SetMySetting("ShowRegionListOnBoot", CStr(Value))
        End Set
    End Property

    Public Property ShowToForeignUsers() As Boolean
        Get
            Return CType(GetMySetting("ShowToForeignUsers", "False"), Boolean)
        End Get
        Set
            SetMySetting("ShowToForeignUsers", CStr(Value))
        End Set
    End Property

    Public Property ShowToLocalUsers() As Boolean
        Get
            Return CType(GetMySetting("ShowToLocalUsers", "False"), Boolean)
        End Get
        Set
            SetMySetting("ShowToLocalUsers", CStr(Value))
        End Set
    End Property

    Public Property SimName() As String
        Get
            Return GetMySetting("SimName", DreamGrid)
        End Get
        Set
            SetMySetting("SimName", Value)
        End Set
    End Property

    Public Property SiteMap() As Boolean
        Get
            Return CType(GetMySetting("SiteMap", "True"), Boolean)
        End Get
        Set
            SetMySetting("SiteMap", CStr(Value))
        End Set
    End Property

    Public Property SizeX() As String
        Get
            Return GetMySetting("SizeX", "256")
        End Get
        Set
            SetMySetting("SizeX", Value)
        End Set
    End Property

    Public Property SizeY() As String
        Get
            Return GetMySetting("SizeY", "256")
        End Get
        Set
            SetMySetting("SizeY", Value)
        End Set
    End Property

    Public Property SkipUpdateCheck() As Double
        Get
            Try
                Return Convert.ToDouble(GetMySetting("SkipUpdateCheck", "0"), Globalization.CultureInfo.InvariantCulture)
            Catch ex As Exception
            End Try
            Return 0
        End Get
        Set
            SetMySetting("SkipUpdateCheck", CStr(Value))
        End Set
    End Property

    Public Property Skirtsize() As Integer
        Get
            Return CInt("0" & GetMySetting("Skirtsize", "0"))
        End Get
        Set
            SetMySetting("Skirtsize", CType(Value, String))
        End Set
    End Property

    ''' <summary>
    ''' Enable the SmartStart Module
    ''' </summary>
    Public Property Smart_Start() As Boolean
        Get
            Return CType(GetMySetting("SmartStart", "False"), Boolean)
        End Get
        Set
            SetMySetting("SmartStart", CStr(Value))
        End Set

    End Property

    Public Property SmartStartTimeout() As Integer
        Get
            Return CInt("0" & GetMySetting("SmartStartTimeout", "20"))
        End Get
        Set
            SetMySetting("SmartStartTimeout", CType(Value, String))
        End Set
    End Property

    Public Property SmtpHost() As String
        Get
            Return GetMySetting("SmtpHost", "smtp.gmail.com")
        End Get
        Set
            SetMySetting("SmtpHost", Value)
        End Set
    End Property

    Public Property SmtpPassword() As String
        Get
            Return GetMySetting("SmtpPassword", "Some Password")
        End Get
        Set
            SetMySetting("SmtpPassword", Value)
        End Set
    End Property

    Public Property SmtpPort() As Integer
        Get
            Return CInt("0" & GetMySetting("SmtpPort", "587"))
        End Get
        Set
            SetMySetting("SmtpPort", CStr(Value))
        End Set
    End Property

    Public Property SmtPropUserName() As String
        Get
            Return GetMySetting("SmtPropUserName", "LoginName@gmail.com")
        End Get
        Set
            SetMySetting("SmtPropUserName", Value)
        End Set
    End Property

    Public Property SmtpSecure() As Boolean
        Get
            Return CType(GetMySetting("SmtpSecure", "True"), Boolean)
        End Get
        Set
            SetMySetting("SmtpSecure", CStr(Value))
        End Set
    End Property

    Public Property SplashPage() As String
        Get
            Return GetMySetting("SplashPage", PropHttpsDomain & "/Outworldz_installer/Welcome.htm")
        End Get
        Set
            SetMySetting("SplashPage", Value)
        End Set
    End Property

    Public Property SSLEmail() As String
        Get
            Return GetMySetting("SSLEmail", "")
        End Get
        Set
            SetMySetting("SSLEmail", Value)
        End Set
    End Property

    Public Property SSLEnabled() As Boolean
        Get
            Return CType(GetMySetting("SSLEnabled", "False"), Boolean)
        End Get
        Set
            SetMySetting("SSLEnabled", CStr(Value))
        End Set
    End Property

    Public Property SSLIsInstalled() As Boolean
        Get
            Return CType(GetMySetting("SSLIsInstalled", "False"), Boolean)
        End Get
        Set
            SetMySetting("SSLIsInstalled", CStr(Value))
        End Set
    End Property

    Public Property SSLType() As Integer
        Get
            Return CInt("0" & GetMySetting("SSLType", "3")) ' StartTLS is default
        End Get
        Set
            SetMySetting("SSLType", CStr(Value))
        End Set
    End Property

    Public Property StartDate() As DateTime
        Get
            Dim parsedDate As DateTime
            If DateTime.TryParse(GetMySetting("StartDate", ""), parsedDate) Then
                Return parsedDate
            End If
            Return Now
        End Get
        Set
            Dim Datestring = CStr(Value)
            SetMySetting("StartDate", Datestring)
        End Set
    End Property

    Public Property StatusInterval() As Integer
        Get
            Return CType("0" & GetMySetting("StatusInterval", "0"), Integer)
        End Get
        Set
            SetMySetting("StatusInterval", CStr(Value))
        End Set
    End Property

    ''' <summary>
    ''' A flag in opensim.ini that prints a dump of all on all regions status values every 10 minutes from bin\debug.txt
    ''' </summary>
    ''' <returns>true/false</returns>
    Public Property Suitcase() As Boolean
        Get
            Return CType(GetMySetting("Suitcase", "True"), Boolean)
        End Get
        Set
            SetMySetting("Suitcase", CStr(Value))
        End Set
    End Property

    Public Property SupportViewerObjectsCache() As Boolean
        Get
            Return CType(GetMySetting("SupportViewerObjectsCache", "True"), Boolean)
        End Get
        Set
            SetMySetting("SupportViewerObjectsCache", CStr(Value))
        End Set
    End Property

    Public Property SurroundOwner() As String
        Get
            Dim other As String = GetMySetting("SurroundOwner", "")
            Return other
        End Get
        Set
            SetMySetting("SurroundOwner", Value)
        End Set
    End Property

    Public Property TeleportSleepTime() As Integer
        Get
            Return CInt("0" & GetMySetting("TeleportSleepTime", "15"))
        End Get
        Set
            SetMySetting("TeleportSleepTime", CType(Value, String))
        End Set
    End Property

    Public Property TempRegion() As Boolean
        Get
            Return CType(GetMySetting("TempRegion", "True"), Boolean)
        End Get
        Set
            SetMySetting("TempRegion", CStr(Value))
        End Set
    End Property

    Public Property TerrainType() As String
        Get
            Return GetMySetting("TerrainType", "Random")
        End Get
        Set
            SetMySetting("TerrainType", Value)
        End Set
    End Property

    Public Property Theme() As String
        Get
            Return GetMySetting("Theme", "White") ' no default, so we copy it.
        End Get
        Set
            SetMySetting("Theme", Value)
        End Set
    End Property

    Public Property TideEnabled() As Boolean
        Get
            Return CType(GetMySetting("TideEnabled", "False"), Boolean)
        End Get
        Set
            SetMySetting("TideEnabled", CStr(Value))
        End Set
    End Property

    Public Property TideHighLevel() As Single
        Get
            Return CSng("0" & GetMySetting("TideHighLevel", "20"))
        End Get
        Set
            SetMySetting("TideHighLevel", CStr(Value))
        End Set
    End Property

    Public Property TideInfoChannel() As Integer
        Get
            Return CInt("0" & GetMySetting("TideInfoChannel", "5555"))
        End Get
        Set
            SetMySetting("TideInfoChannel", CStr(Value))
        End Set
    End Property

    ' Tides
    Public Property TideInfoDebug() As Boolean
        Get
            Return CType(GetMySetting("TideInfoDebug", "False"), Boolean)
        End Get
        Set
            SetMySetting("TideInfoDebug", CStr(Value))
        End Set
    End Property

    Public Property TideLevelChannel() As Integer
        Get
            Return CInt("0" & GetMySetting("TideLevelChannel", "5556"))
        End Get
        Set
            SetMySetting("TideLevelChannel", CStr(Value))
        End Set
    End Property

    Public Property TideLowLevel() As Single
        Get
            Return CInt("0" & GetMySetting("TideLowLevel", "17"))
        End Get
        Set
            SetMySetting("TideLowLevel", CStr(Value))
        End Set
    End Property

    Public Property TOSEnabled() As Boolean
        Get
            Return CType(GetMySetting("TOSEnabled", "False"), Boolean)
        End Get
        Set
            SetMySetting("TOSEnabled", CStr(Value))
        End Set
    End Property

    Public Property Total_InnoDB_GBytes() As Double
        Get
            Dim amount = Convert.ToDouble("0" & GetMySetting("Total_InnoDB_GBytes", "1"), Globalization.CultureInfo.InvariantCulture)
            If amount > 2 Then amount = 2
            Return amount
        End Get
        Set
            If Value > 2 Then Value = 2
            SetMySetting("Total_InnoDB_GBytes", CType(Value, String))
        End Set
    End Property

    ''' <summary>Hours that TTS/Audio keeps files before expiring</summary>
    Public Property TTSHours() As Double
        Get
            Try
                Return Convert.ToDouble(GetMySetting("TTSHours", "1"), Globalization.CultureInfo.InvariantCulture)
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
            Return 1
        End Get
        Set
            If Value < 1 Then Value = 1
            SetMySetting("TTSHours", CStr(Value))
        End Set
    End Property

    Public Property UPnpDiag() As Boolean
        Get
            Return CType(GetMySetting("UPnpDiag", "False"), Boolean)
        End Get
        Set
            SetMySetting("UPnpDiag", CStr(Value))
        End Set
    End Property

    Public Property UPnPEnabled() As Boolean
        Get
            Return CType(GetMySetting("UPnPEnabled", "True"), Boolean)
        End Get
        Set
            SetMySetting("UPnPEnabled", CStr(Value))
        End Set
    End Property

    ''' <summary>
    ''' using TLS and a smtp server with a self signed certificate you will need to set this option false
    ''' </summary>
    ''' <returns></returns>
    Public Property VerifyCertCheckBox() As Boolean
        Get
            Return CType(GetMySetting("VerifyCertCheckBox", "True"), Boolean)
        End Get
        Set
            SetMySetting("VerifyCertCheckBox", CStr(Value))
        End Set
    End Property

    Public Property VisitorsEnabled() As Boolean
        Get
            Return CType(GetMySetting("VisitorsEnabled", "False"), Boolean)
        End Get
        Set
            SetMySetting("VisitorsEnabled", CStr(Value))
        End Set

    End Property

    Public Property VisitorsEnabledModules() As Boolean
        Get
            Return CType(GetMySetting("VisitorMapsEnabled", "False"), Boolean)
        End Get
        Set
            SetMySetting("VisitorMapsEnabled", CStr(Value))
        End Set

    End Property

    Public Property VivoxEnabled() As Boolean
        Get
            Return CType(GetMySetting("VivoxEnabled", "False"), Boolean)
        End Get
        Set
            SetMySetting("VivoxEnabled", CStr(Value))
        End Set
    End Property

    Public Property VivoxPassword() As String
        Get
            Return GetMySetting("Vivox_password", "Passord")
        End Get
        Set
            SetMySetting("Vivox_password", Value)
        End Set
    End Property

    Public Property VivoxUserName() As String
        Get
            Return GetMySetting("Vivox_username", "User Name")
        End Get
        Set
            SetMySetting("Vivox_username", Value)
        End Set
    End Property

    Public Property VoiceName() As String
        Get
            Return GetMySetting("VoiceName", "Microsoft Zira Desktop")
        End Get
        Set
            SetMySetting("VoiceName", Value)
        End Set
    End Property

    Public Property VoicesInstalled() As Boolean
        Get
            Return CType(GetMySetting("VoicesInstalled", "False"), Boolean)
        End Get
        Set
            SetMySetting("VoicesInstalled", CStr(Value))
        End Set
    End Property

    Public Property WANIP() As String

    Public Property WelcomeMessage() As String
        Get
            Return GetMySetting("WelcomeMessage", "Welcome to " & SimName() & ", <USERNAME>")
        End Get
        Set
            SetMySetting("WelcomeMessage", Value)
        End Set

    End Property

    Public Property WelcomeRegion() As String
        Get
            Return GetMySetting("WelcomeRegion", "Welcome")
        End Get
        Set
            SetMySetting("WelcomeRegion", Value)
        End Set
    End Property

    Public Property WhisperDistance() As String
        Get
            Return GetMySetting("WhisperDistance", "10")
        End Get
        Set
            SetMySetting("WhisperDistance", Value)
        End Set
    End Property

    Public Property WifiEnabled() As Boolean
        Get
            Return CType(GetMySetting("WifiEnabled", "True"), Boolean)
        End Get
        Set
            SetMySetting("WifiEnabled", CStr(Value))
        End Set
    End Property

    Public Shared Function SCPass() As String
        Dim SCPasswordAdmin = New PassGen
        Return SCPasswordAdmin.GeneratePass()
    End Function

    Private Shared Function SCAdmin() As String
        Dim SCPasswordAdmin As New PassGen()
        Return SCPasswordAdmin.GeneratePass()
    End Function

#End Region

#Region "Robust"

    Public Function RegionDBConnection() As String

        Return """" _
        & "Data Source=" & RegionServer _
        & ";Database=" & RegionDBName _
        & ";Port=" & CStr(MySqlRegionDBPort) _
        & ";User ID=" & RegionDBUserName _
        & ";Password=" & RegionDbPassword _
        & ";Old Guids=true;Allow Zero Datetime=true" _
        & ";Convert Zero Datetime=True" _       ' thanks, Hairy Thor!
        & ";Connect Timeout=28800;Command Timeout=28800;" & """"

    End Function

    Public Function RegionMySqlConnection() As String

        Return "server=" & RegionServer _
        & ";database=" & RegionDBName _
        & ";port=" & CStr(MySqlRegionDBPort) _
        & ";user=" & RegionDBUserName _
        & ";password=" & RegionDbPassword

    End Function

    Public Function RobustDBConnection() As String

        Return """" _
            & "Data Source=" & RobustServerIP _
            & ";Database=" & RobustDatabaseName _
            & ";Port=" & CStr(MySqlRobustDBPort) _
            & ";User ID=" & RobustUserName _
            & ";Password=" & RobustPassword _
            & ";Old Guids=true;Allow Zero Datetime=true" _
            & ";Convert Zero Datetime=True" _
            & ";Connect Timeout=28800;Command Timeout=28800" & """"

    End Function

    Public Function RobustMysqlConnection() As String

        Return "server=" & RobustServerIP _
            & ";database=" & RobustDatabaseName _
            & ";port=" & CStr(MySqlRobustDBPort) _
            & ";user=" & RobustUserName _
            & ";password=" & RobustPassword _
            & ";Old Guids=true;Allow Zero Datetime=true" _
            & ";Convert Zero Datetime=True" _
            & ";Connect Timeout=28800;Command Timeout=28800;"

    End Function

    Public Function RootMysqlConnection() As String

        Return "server=" & RobustServerIP _
            & ";database=mysql" _
            & ";port=" & CStr(MySqlRobustDBPort) _
            & ";user=root" _
            & ";password=" & RootMysqlPassword _
            & ";Old Guids=true;Allow Zero Datetime=true" _
            & ";Connect Timeout=28800;Command Timeout=28800;"

    End Function

#End Region

End Class
