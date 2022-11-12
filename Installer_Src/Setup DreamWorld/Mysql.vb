#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.Globalization
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Threading
Imports Ionic.Zip
Imports MySqlConnector

#Region "MysqlInterface"

Public Module MysqlInterface
    Public WithEvents ProcessMySql As Process = New Process()

    Private _MysqlCrashCounter As Integer
    Private _MysqlExited As Boolean
    Private _MysqlLPM As Integer

#End Region

#Region "Properties"

    Public Property MysqlCrashCounter As Integer
        Get
            Return _MysqlCrashCounter
        End Get
        Set(value As Integer)
            _MysqlCrashCounter = value
        End Set
    End Property

    Public Property PropMysqlExited() As Boolean
        Get
            Return _MysqlExited
        End Get
        Set(ByVal Value As Boolean)
            _MysqlExited = Value
        End Set
    End Property

#End Region

#Region "Stats"

    ' TODO

    'Select Case COUNT(*), SUBSTRING( argument, 1, 7), SUBSTRING_INDEX(SUBSTRING_INDEX(argument, "values", -1), "'", 3) As arg    FROM mysql.general_log    WHERE SUBSTRING( argument, 1, 21) = "insert into primitems"    GROUP BY arg    ORDER BY 1 DESC

    'SELECT COUNT(), SUBSTRING( argument, 1, 7), SUBSTRING_INDEX(SUBSTRING_INDEX(SUBSTRING_INDEX( argument, "values", -1),"'", 4),"'", -1) AS arg    FROM mysql.general_log    WHERE SUBSTRING( argument, 1, 19) = "replace into prims "GROUP BY arg ORDER BY 1 DESC

    Public Function MysqlStats() As Double

        QuerySuper("SET GLOBAL general_log = 'OFF'")

        Using MysqlConn As New MySqlConnection(Settings.RootMysqlConnection)
            Try
                MysqlConn.Open()
                Dim stm = "Select count(*) from mysql.general_log"
                Using cmd = New MySqlCommand(stm, MysqlConn)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            _MysqlLPM = reader.GetInt32(0)
                        End If
                    End Using
                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
        End Using

        QuerySuper("TRUNCATE table mysql.general_log;")
        QuerySuper("SET GLOBAL general_log = 'ON'; set GLOBAL log_output = 'TABLE';")

        Return _MysqlLPM

    End Function

#End Region

#Region "StartMysql"

    Public Function StartMySQL() As Boolean

        PropAborting = False

        Log("INFO", "Checking MySQL")
        If MysqlInterface.IsMySqlRunning() Then
            Return True
        End If

        Log("INFO", "MySQL is not running")
        ' Build data folder if it does not exist
        MakeMysql()

        MySQLIcon(False)
        ' Start MySql in background.

        ' SAVE INI file
        Dim INI = New LoadIni(IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\mysql\my.ini"), "#", System.Text.Encoding.ASCII)

        INI.SetIni("mysqld", "innodb_buffer_pool_size", $"{Settings.Total_InnoDB_GBytes()}G")

        If Settings.MysqlRunasaService Then
            INI.SetIni("mysqld", "innodb_doublewrite", "0")
            INI.SetIni("mysqld", "innodb_max_dirty_pages_pct", "75")
            INI.SetIni("mysqld", "innodb_flush_log_at_trx_commit", "2")
        Else
            ' when we are a service we can wait until we have 75 % of the buffer full before we flush
            ' if not, too dangerous, so we always write at 0% for ACID behavior.
            ' InnoDB tries to flush data from the buffer pool so that the percentage of dirty pages does Not exceed this value. The default value Is 75.
            ' The innodb_max_dirty_pages_pct setting establishes a target for flushing activity. It does Not affect the rate of flushing.

            INI.SetIni("mysqld", "innodb_max_dirty_pages_pct", "0")

            ' The doublewrite buffer is a storage area where InnoDB writes pages flushed from the buffer
            ' pool before writing the pages To their proper positions In the InnoDB data files.
            ' If there Is an operating system, storage subsystem, or unexpected mysqld process
            ' Exit In the middle Of a page write, InnoDB can find a good copy Of the page from the
            ' doublewrite Buffer during crash recovery.
            INI.SetIni("mysqld", "innodb_doublewrite", "1")

            ' If set To 1, InnoDB will flush (fsync) the transaction logs To the
            ' disk at Each commit, which offers full ACID behavior. If you are
            ' willing To compromise this safety, And you are running small
            ' transactions, you may Set this To 0 Or 2 To reduce disk I/O To the
            ' logs. Value 0 means that the log Is only written To the log file And
            ' the log file flushed To disk approximately once per second. Value 2
            ' means the log Is written To the log file at Each commit, but the log
            ' file Is only flushed To disk approximately once per second.

            INI.SetIni("mysqld", "innodb_flush_log_at_trx_commit", "1")
        End If

        INI.SetIni("mysqld", "basedir", $"""{Settings.CurrentSlashDir}/OutworldzFiles/MySQL""")
        INI.SetIni("mysqld", "datadir", $"""{Settings.CurrentSlashDir}/OutworldzFiles/MySQL/Data""")
        INI.SetIni("mysqld", "port", CStr(Settings.MySqlRobustDBPort))
        INI.SetIni("client", "port", CStr(Settings.MySqlRobustDBPort))

        INI.SaveIni()

        ' create test program slants the other way:
        Dim testProgram As String = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\MySQL\bin\StartManually.bat")
        DeleteFile(testProgram)

        Try
            Using outputFile As New StreamWriter(testProgram, False)
                outputFile.WriteLine("@REM A program to run MySQL manually for troubleshooting." & vbCrLf _
                             & "mysqld.exe --defaults-file=" &
                             """" & Settings.CurrentSlashDir & "/OutworldzFiles/mysql/my.ini" & """"
                             )
                outputFile.Flush()
            End Using
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

        CreateStopMySql()

        If Settings.MysqlRunasaService Then

            CreateService()

            If Settings.CurrentDirectory <> Settings.MysqlLastDirectory Or Not ServiceExists("MySQLDreamGrid") Then
                Using MysqlProcess As New Process With {
                .EnableRaisingEvents = False
            }
                    MysqlProcess.StartInfo.UseShellExecute = True ' so we can redirect streams
                    MysqlProcess.StartInfo.FileName = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\MySQL\bin\mysqld.exe")
                    MysqlProcess.StartInfo.Arguments = $"--install MySQLDreamGrid --defaults-file=""{Settings.CurrentSlashDir}/OutworldzFiles/mysql/my.ini"""
                    MysqlProcess.StartInfo.CreateNoWindow = True
                    MysqlProcess.StartInfo.WorkingDirectory = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\MySQL\bin\")
                    MysqlProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden

                    Try
                        MysqlProcess.Start()
                        MysqlProcess.WaitForExit()
                    Catch ex As Exception
                        BreakPoint.Dump(ex)
                        MySQLIcon(False)
                    End Try
                    Application.DoEvents()

                    If MysqlProcess.ExitCode <> 0 Then
                        TextPrint(My.Resources.Mysql_Failed)
                        MySQLIcon(False)
                        Return False
                    Else
                        Settings.MysqlLastDirectory = Settings.CurrentDirectory
                        Settings.SaveSettings()
                    End If

                    Application.DoEvents()
                End Using

            End If

            TextPrint(My.Resources.Mysql_Starting)

            Using MysqlProcess As New Process With {
                        .EnableRaisingEvents = False
                    }
                MysqlProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                MysqlProcess.StartInfo.FileName = "net"
                MysqlProcess.StartInfo.Arguments = "start MySQLDreamGrid"
                MysqlProcess.StartInfo.UseShellExecute = False
                MysqlProcess.StartInfo.CreateNoWindow = True
                MysqlProcess.StartInfo.RedirectStandardError = True
                MysqlProcess.StartInfo.RedirectStandardOutput = True
                Dim response As String = ""

                Try
                    MysqlProcess.Start()
                    response = MysqlProcess.StandardOutput.ReadToEnd() & MysqlProcess.StandardError.ReadToEnd()
                    MysqlProcess.WaitForExit()
                Catch ex As Exception
                    BreakPoint.Dump(ex)
                    TextPrint(My.Resources.Mysql_Failed & ":" & ex.Message)
                End Try
                Application.DoEvents()

                If MysqlProcess.ExitCode <> 0 Then
                    If response.Contains("has already been started") Then
                        TextPrint(My.Resources.Mysql_is_Running & ":" & Settings.MySqlRobustDBPort)
                        MySQLIcon(True)
                        Return True
                    End If
                    TextPrint(My.Resources.Mysql_Failed & ":" & response)
                    MySQLIcon(False)
                    Return False
                Else
                    TextPrint(My.Resources.Mysql_is_Running & ":" & Settings.MySqlRobustDBPort)
                    MySQLIcon(True)
                End If

            End Using
        Else

            Application.DoEvents()
            ' MySQL was not running, so lets start it up.
            Dim pi = New ProcessStartInfo With {
                .Arguments = $"--defaults-file=""{Settings.CurrentSlashDir}/OutworldzFiles/mysql/my.ini""",
                .WindowStyle = ProcessWindowStyle.Hidden,
                .CreateNoWindow = True,
                .FileName = $"""{IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\mysql\bin\mysqld.exe")}"""
            }
            ProcessMySql.StartInfo = pi
            ProcessMySql.EnableRaisingEvents = True
            Try
                ProcessMySql.Start()
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try

            ' wait for MySql to come up
            Dim MysqlOk As Boolean
            Dim ctr As Integer = 0
            While Not MysqlOk

                Dim MysqlLog As String = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\mysql\data")
                If ctr = 60 Then ' about 60 seconds when it fails

                    Dim yesno = MsgBox(My.Resources.Mysql_Failed, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Error_word)
                    If (yesno = vbYes) Then
                        Dim files As Array = Nothing
                        Try
                            files = Directory.GetFiles(MysqlLog, "*.err", SearchOption.TopDirectoryOnly)
                        Catch ex As Exception
                            BreakPoint.Dump(ex)
                        End Try

                        For Each FileName As String In files
                            Baretail("""" & FileName & """")
                            Application.DoEvents()
                        Next
                    End If
                    FormSetup.Buttons(FormSetup.StartButton)
                    Return False
                End If
                ctr += 1
                ' check again
                Sleep(1000)
                Application.DoEvents()
                MysqlOk = MysqlInterface.IsMySqlRunning()
            End While

            If Not MysqlOk Then Return False

        End If

        UpgradeMysql()

        TextPrint(Global.Outworldz.My.Resources.Mysql_is_Running)
        MySQLIcon(True)

        PropMysqlExited = False

        Return True

    End Function

#End Region

    ''' <summary>
    ''' Delete all regions data in various tables when a region is deleted bu GUI
    ''' ''' </summary>
    ''' <param name="primUuid">UUID</param>
    ''' <param name="tablename"></param>
    ''' <param name="uuidname"></param>

#Region "Email"

    ''' <summary>
    ''' Gets first name, last name, email from user account
    ''' </summary>
    ''' <param name="UUID">avatar UUID</param>
    ''' <returns>Person class</returns>
    Public Function AvatarEmailData(UUID As String) As UserData

        Dim Avi As New UserData

        Using MysqlConn As New MySqlConnection(Settings.RobustMysqlConnection)
            Try
                MysqlConn.Open()
                Dim stm = "Select firstname, lastname, email from useraccounts where principalid = @UUID"

                Using cmd = New MySqlCommand(stm, MysqlConn)
                    cmd.Parameters.AddWithValue("@UUID", UUID)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Avi.FirstName = reader.GetString(0)
                            Avi.LastName = reader.GetString(1)
                            Avi.Email = reader.GetString(2)
                        End If
                    End Using
                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
        End Using
        Return Avi

    End Function

#End Region



#Region "Delete Stuff"

    Public Sub DeleteContent(RegionUuid As String, tablename As String, uuidname As String)

        Using MysqlConn As New MySqlConnection(Settings.RegionMySqlConnection)
            Try
                MysqlConn.Open()
                Dim stm = $"delete from {tablename} WHERE = "
#Disable Warning CA2100
                Using cmd = New MySqlCommand(stm, MysqlConn)
                    cmd.Parameters.AddWithValue("@UUID", RegionUuid)
                    cmd.ExecuteNonQuery()
                End Using
#Enable Warning CA2100
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
        End Using

    End Sub

    ''' <summary>
    ''' Deletes all IMS from a user
    ''' </summary>
    ''' <param name="id">Avatar uuid</param>
    '''
    Public Sub DeleteIM(id As Integer)

        Dim stm = $"delete from im_offline where id = @param"
        Dim v As String = ""

        Dim conn As String
        If (Settings.ServerType = RobustServerName) Then
            conn = Settings.RobustMysqlConnection
        Else
            conn = Settings.RegionMySqlConnection
        End If

        Using MysqlConn As New MySqlConnection(conn)
            Try
                MysqlConn.Open()
#Disable Warning CA2100
                Using cmd As New MySqlCommand(stm, MysqlConn)
#Enable Warning
                    cmd.Parameters.AddWithValue("@param", id)
                    v = Convert.ToString(cmd.ExecuteScalar(), Globalization.CultureInfo.InvariantCulture)
                End Using
            Catch ex As Exception
                BreakPoint.Print(ex.Message)
            End Try
        End Using

    End Sub

    ''' <summary>
    ''' Delete old visitors and regions that no longer exist from the stats table
    ''' </summary>
    Public Sub DeleteOldVisitors()

        Dim stm = $"delete from visitor WHERE dateupdated < NOW() - INTERVAL {Settings.KeepVisits} DAY "

        QueryString(stm) ' todo fix sql

        ' make a list of  'uuid1', 'uuid2' etc
        Dim list2 As New List(Of String)
        For Each item In RegionUuids()
            list2.Add($"'{item}'")
        Next
        Dim arr As String() = list2.ToArray
        Dim clause = Join(arr, ",")

        stm = $"delete from stats where UUID not in ({clause})"
        QueryString(stm)

        ' make a list of 'Welcome', 'Virunga' etc
        list2.Clear()
        For Each item In RegionUuids()
            Dim r = Replace(Region_Name(item), "'", "''")  ' escape single quotes with ''
            list2.Add($"'{r}'")
        Next
        arr = list2.ToArray
        clause = Join(arr, ",")

        stm = $"delete from visitor where regionname not in ({clause})" ' todo fix sql
        QueryString(stm)

    End Sub

    '''
    ''' logs out any users when we clear caches
    '''
    Public Sub DeleteOnlineUsers()

        If PropOpensimIsRunning Then
            Return
        End If

        ' If we are booting and nothing is running, we clear the registered regions and people
        Dim OpensimRunning() = Process.GetProcessesByName("Opensim")
        If IsMySqlRunning() And OpensimRunning.Length = 0 And Settings.ServerType = RobustServerName Then
            MysqlInterface.DeregisterRegions(False)
            FixPresence()
            QueryString("update griduser set online = 'false';")
        End If

    End Sub

    ''' <summary>
    ''' Delete old visitors and regions that no longer exist from the stats table
    ''' </summary>
    Public Sub DeleteVisitorMap(RegionUUID As String)

        'todo clear up SQL
        Dim stm = $"delete from stats where UUID = {RegionUUID}"
        QueryString(stm)

        ''todo clear up SQL
        stm = $"delete from visitors where regionname = {Region_Name(RegionUUID)}"
        QueryString(stm)

    End Sub

    ''' <summary>
    ''' Delete useless fsasset data
    ''' </summary>
    Public Sub DelRobustMaps()

        Dim q = "delete from fsassets WHERE name LIKE 'terrainImage_%';"
        QueryString(q)
        q = "delete from assets WHERE name LIKE 'terrainImage_%';"
        QueryString(q)

    End Sub

    ''' <summary>
    ''' DeregisterPosition removes a region registration at a given X, Y for temp region creation
    ''' </summary>
    ''' <param name="X"></param>
    ''' <param name="Y"></param>
    Public Sub DeregisterPosition(X As Integer, Y As Integer)

        Using MysqlConn As New MySqlConnection(Settings.RobustMysqlConnection)
            Try
                MysqlConn.Open()

                Dim stm = "delete from regions where LocX=@X And LocY=@Y"
                Using cmd = New MySqlCommand(stm, MysqlConn)
                    cmd.Parameters.AddWithValue("@X", X * 256)
                    cmd.Parameters.AddWithValue("@Y", Y * 256)
                    cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try

        End Using

    End Sub

    ''' <summary>
    ''' deletes all regions from robust regions
    ''' </summary>
    ''' <param name="force">if true, deletes even if running, used as part of update process</param>
    Public Sub DeregisterRegions(force As Boolean)

        If PropOpensimIsRunning And Not force Then
            MsgBox("Opensim Is running. Cannot clear the list Of registered regions", MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground)
            Return
        End If

        If MysqlInterface.IsMySqlRunning() Then
            QueryString($"delete from regions;")
            TextPrint(My.Resources.Deregister_All)
        End If

    End Sub

    ''' <summary>
    ''' deletes one region from robust.regions
    ''' </summary>
    ''' <param name="UUID">UUID of region</param>
    Public Sub DeregisterRegionUUID(RegionUUID As String)

        Using MysqlConn As New MySqlConnection(Settings.RobustMysqlConnection)
            Try
                MysqlConn.Open()

                Dim stm = "delete from regions where uuid = @UUID;"
                Using cmd = New MySqlCommand(stm, MysqlConn)
                    cmd.Parameters.AddWithValue("@UUID", RegionUUID)
                    cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try

        End Using

    End Sub

#End Region

#Region "TOS"

    Public Sub Add2Tos(AvatarUUID As String, token As String, avatarname As String)

        Using NewSQLConn As New MySqlConnection(Settings.RobustMysqlConnection)

            Dim stm = "insert into tosauth (avataruuid, avatarname, agreed, token) values (@AVATARUUID,@AVATARNAME,0,@TOKEN)"
            Using cmd As New MySqlCommand(stm, NewSQLConn)
                Try
                    NewSQLConn.Open()
                    cmd.Parameters.AddWithValue("@AVATARUUID", AvatarUUID)
                    cmd.Parameters.AddWithValue("@AVATARNAME", avatarname)
                    cmd.Parameters.AddWithValue("@TOKEN", token)
                    cmd.ExecuteScalar()
                Catch ex As Exception
                    BreakPoint.Dump(ex)
                End Try
            End Using
        End Using

    End Sub

    Public Sub Agree2Tos(uuid As Guid)

        Using NewSQLConn As New MySqlConnection(Settings.RobustMysqlConnection)

            Dim stm = "update tosauth set agreed=1, confirmtime = now() where token = @TOKEN; "
            Using cmd As New MySqlCommand(stm, NewSQLConn)
                Try
                    NewSQLConn.Open()
                    cmd.Parameters.AddWithValue("@TOKEN", uuid.ToString())
                    cmd.ExecuteScalar()
                Catch ex As Exception
                    BreakPoint.Dump(ex)
                End Try
            End Using
        End Using

    End Sub

    Public Function IsTOSAccepted(AvatarUUID As String, Fname As String, LName As String, UUID As String) As Boolean

        If Not Settings.TOSEnabled Then Return True

        If Not InAuth(AvatarUUID) Then
            Add2Tos(AvatarUUID, UUID, Fname & " " & LName)
            Return False
        End If

        Using NewSQLConn As New MySqlConnection(Settings.RobustMysqlConnection)
            Try
                NewSQLConn.Open()
                Dim stm As String = "SELECT agreed FROM tosauth where avataruuid = @UUID"
                Using cmd As New MySqlCommand(stm, NewSQLConn)
                    cmd.Parameters.AddWithValue("@UUID", AvatarUUID)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Return CBool(reader.GetInt32(0))
                        End While
                    End Using
                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
        End Using

        Return False

    End Function

    ''' <summary>
    ''' Check if an avatar has been here 5 minutes and not agreed to a TOS. If so, it logs them out
    ''' </summary>
    Public Sub NewUserTimeout()

        If Not Settings.TOSEnabled Then Return


        Using Connection As New MySqlConnection(Settings.RobustMysqlConnection)
            Try
                Connection.Open()
                Dim stm = "Select avatarname, avataruuid from tosauth where TIMESTAMPDIFF(minute,createtime,now()) > 3 and agreed = 0; "
                Using cmd = New MySqlCommand(stm, Connection)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim Val = reader.GetString("avataruuid")
                            Dim aviname = reader.GetString("avatarname")
                            TextPrint($"{aviname} {My.Resources.DidNotAccept}.")
                            ' TODO
                            'LogoutAvatar(Val)
                            DropAvatarFromTOS(Val)
                        End While
                    End Using
                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try

        End Using

    End Sub

    Private Sub DropAvatarFromTOS(AvatarUUID As String)

        ' T
        Using NewSQLConn As New MySqlConnection(Settings.RobustMysqlConnection)
            Try
                NewSQLConn.Open()
                Dim stm As String = "update tosauth set agreed = -1 where avataruuid = @UUID"
                Using cmd As New MySqlCommand(stm, NewSQLConn)
                    cmd.Parameters.AddWithValue("@UUID", AvatarUUID)
                    cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
        End Using

    End Sub

    Private Sub LogoutAvatar(AvatarUUID As String)

        For Each Agent In CachedAvatars
            If Agent.AvatarUUID = AvatarUUID Then
                For Each Avatar In CachedAvatars
                    Dim RegionUUID = Avatar.RegionID
                    If IsAgentInRegion(RegionUUID) Then
                        RPC_Region_Command(RegionUUID, $"kick user {Avatar.FirstName} {Avatar.LastName} You are logged out for not agreeing to the Terms and Conditions of this Grid. ")
                        Exit For
                    End If
                Next
            End If
        Next

    End Sub

#End Region

#Region "Land"

    Public Sub DoLand()

        Dim stm = "update land set landflags = (landflags & ! 64);" ' Rez
        DoOpensim(stm)
        stm = "update land set landflags = (landflags & ! 16);" ' Land editing
        DoOpensim(stm)

    End Sub

    Public Sub DoLandOneRegion(regionUUID As String)

        Dim stm = $"update land set landflags = (landflags & ! 64) where regionUUID = '{regionUUID}';" ' Rez
        DoOpensim(stm)
        stm = "update land set landflags = (landflags & ! 16) where regionUUID = '{regionUUID}';" ' Land editing
        DoOpensim(stm)

    End Sub

#End Region

#Region "Needs fixup"

    Public Sub DoOpensim(stm As String)

        Using MysqlConn As New MySqlConnection(Settings.RegionMySqlConnection)
            Try
                MysqlConn.Open()
#Disable Warning CA2100
                Using cmd1 = New MySqlCommand(stm, MysqlConn)
#Enable Warning CA2100
                    cmd1.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
        End Using

    End Sub

    ' TODO model for one param queries
    Public Function Queryparam(SQL As String, param As String) As String

        Dim v As String = ""

        Dim conn As String
        If (Settings.ServerType = RobustServerName) Then
            conn = Settings.RobustMysqlConnection
        Else
            conn = Settings.RegionMySqlConnection
        End If

        Using MysqlConn As New MySqlConnection(conn)
            Try
                MysqlConn.Open()
#Disable Warning CA2100
                Using cmd As New MySqlCommand(SQL, MysqlConn)
#Enable Warning
                    cmd.Parameters.AddWithValue("@param", param)
                    v = Convert.ToString(cmd.ExecuteScalar(), Globalization.CultureInfo.InvariantCulture)
                End Using
            Catch ex As Exception
                BreakPoint.Print(ex.Message)
            End Try
        End Using

        Return v

    End Function

    <CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100Review Sql queries for security vulnerabilities")>
    Public Function QueryString(SQL As String) As String

        Dim v As String = ""

        Dim conn As String
        If (Settings.ServerType = RobustServerName) Then
            conn = Settings.RobustMysqlConnection
        Else
            conn = Settings.RegionMySqlConnection
        End If

        Using MysqlConn As New MySqlConnection(conn)
            Try
                MysqlConn.Open()
#Disable Warning CA2100
                Using cmd As New MySqlCommand(SQL, MysqlConn)
#Enable Warning
                    v = Convert.ToString(cmd.ExecuteScalar(), Globalization.CultureInfo.InvariantCulture)
                End Using
            Catch ex As Exception
                BreakPoint.Print(ex.Message)
            End Try
        End Using

        Return v

    End Function

    <CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100Review Sql queries for security vulnerabilities")>
    Public Sub QuerySuper(SQL As String)

        Dim conn As String
        conn = Settings.RootMysqlConnection
        Using MysqlConn As New MySqlConnection(conn)
            Try
                MysqlConn.Open()
#Disable Warning CA2100
                Using cmd As New MySqlCommand(SQL, MysqlConn)
#Enable Warning
                    cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                BreakPoint.Print(ex.Message)
            End Try
        End Using

    End Sub

#End Region

#Region "Estates"

    ''' <summary>
    ''' Gets Estate ID number 101, 102, etc
    ''' </summary>
    ''' <param name="UUID">Region UUID</param>
    ''' <returns>int</returns>
    Public Function EstateId(UUID As String) As Integer

        Dim Val = 0
        Using EstateConnection As New MySqlConnection(Settings.RegionMySqlConnection)
            Try
                EstateConnection.Open()
                Dim stm = "Select EstateID from estate_map where RegionID=@UUID"
                Using cmd = New MySqlCommand(stm, EstateConnection)
                    cmd.Parameters.AddWithValue("@UUID", UUID)

                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Val = reader.GetInt32("EstateID")
                        End If
                    End Using
                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try

        End Using

        Return Val

    End Function

    ''' <summary>Returns Estate Name give an Estate UUID</summary>
    ''' <param name="UUID">Region UUID</param>
    ''' <returns>Estate Name as string</returns>
    Public Function EstateName(UUID As String) As String

        Dim Val As String = ""

        Using EstateConnection As New MySqlConnection(Settings.RegionMySqlConnection)
            Try
                EstateConnection.Open()

                Dim stm = "Select estate_settings.EstateName FROM estate_settings estate_settings INNER JOIN estate_map estate_map On (estate_settings.EstateID = estate_map.EstateID) where regionid = @UUID"

                Using cmd = New MySqlCommand(stm, EstateConnection)
                    cmd.Parameters.AddWithValue("@UUID", UUID)

                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            'Debug.Print("ID = {0}", reader.GetString(0))
                            Val = reader.GetString(0)
                        End If
                    End Using

                End Using
            Catch ex As Exception
                BreakPoint.Print(ex.Message)
            End Try
        End Using
        Return Val

    End Function

#End Region

#Region "Run Once"

    ''' <summary>
    ''' Runs once at the hour tick mark to move assets to file system
    ''' </summary>
    Public Sub ExportFsAssets()

        If Settings.ExportAssetsOnce Then Return
        Dim export = $"import ""Data Source=localhost;Port={Settings.MySqlRobustDBPort};Database={Settings.RobustDatabaseName};User ID={Settings.RobustUserName};Password={Settings.RobustPassword};Old Guids=True;Command Timeout=300;"" assets"
        ConsoleCommand(RobustName, export)
        Settings.ExportAssetsOnce = True
        Settings.SaveSettings()

    End Sub

#End Region

#Region "Presence"

    ''' <summary>
    ''' This deletes Presence rows where the corresponding GridUser row does Not exist and is online
    ''' </summary>
    Public Sub FixPresence()

        Dim q = "Delete From Presence Where Not exists (Select * from GridUser  where Presence.UserID = GridUser.UserID  And GridUser.Online = 'True');"
        QueryString(q)

    End Sub

#End Region

#Region "Avatar Counts"

    ''' <summary>
    ''' Gets user count from user accounts
    ''' </summary>
    ''' <returns>integer count of agents in this region</returns>
    Public Function GetAgentsInRegion(RegionUUID As String) As Integer

        Dim RegionName = Region_Name(RegionUUID)

        Using NewSQLConn As New MySqlConnection(Settings.RobustMysqlConnection)
            Try
                NewSQLConn.Open()
                Dim stm As String = "Select count(*) FROM (presence INNER JOIN useraccounts On presence.UserID = useraccounts.PrincipalID) where regionid = @UUID "
                Using cmd As New MySqlCommand(stm, NewSQLConn)
                    cmd.Parameters.AddWithValue("@UUID", RegionUUID)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Try
                                Return reader.GetInt32(0)
                            Catch
                                BreakPoint.Print("Cannot read MySQL!")
                            End Try
                        End If
                    End Using
                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
        End Using

        Return 0

    End Function

    ''' <summary>
    ''' return List of all users
    ''' </summary>
    ''' <returns> auto complete collection</returns>
    Public Function GetAvatarList() As AutoCompleteStringCollection

        Dim A As New AutoCompleteStringCollection
        Using MysqlConn As New MySqlConnection(Settings.RobustMysqlConnection)
            Try
                MysqlConn.Open()

                Dim stm = "Select firstname, lastname FROM useraccounts "

                Using cmd = New MySqlCommand(stm, MysqlConn)

                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim f = reader.GetString(0)
                            Dim l = reader.GetString(1)
                            '  Debug.Print("ID = {0} {1}", f, l)
                            If f <> "GRID" And l <> "SERVICES" Then
                                A.Add(f & " " & l)
                            End If
                        End While
                    End Using

                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try

        End Using

        Return A

    End Function

    ''' <summary>
    ''' Returns a local avatar UUID give a First and Last name
    ''' </summary>
    ''' <param name="avatarName"></param>
    ''' <returns>Avatar UUID</returns>
    Public Function GetAviUUUD(avatarname As String) As String

        If avatarname Is Nothing Then Return ""

        StartMySQL()

        If avatarname.Length = 0 Then Return ""
        Dim Val As String = ""
        Dim stm = "Select PrincipalID  from useraccounts where FirstName Like CONCAT('%', @Fname, '%')"
        Dim parts As String() = avatarname.Split(" ".ToCharArray())
        Dim Fname = parts(0).Trim
        Dim LName As String = ""
        If parts.Length = 2 Then
            LName = parts(1).Trim
            stm = "Select PrincipalID  from useraccounts where FirstName= @Fname And LastName like CONCAT('%', @Lname, '%')"
        End If

        Using MysqlConn As New MySqlConnection(Settings.RobustMysqlConnection)
            Try
                MysqlConn.Open()

                Using cmd = New MySqlCommand(stm, MysqlConn)

                    cmd.Parameters.AddWithValue("@Fname", Fname)
                    If parts.Length = 2 Then
                        cmd.Parameters.AddWithValue("@Lname", LName)
                    End If

                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            'Debug.Print("ID = {0}", reader.GetString(0))\
                            Val = reader.GetString(0)
                        End If
                    End Using

                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try

        End Using

        Return Val

    End Function

    Public Function GetEmailList() As Dictionary(Of String, MailList)

        Dim result = New Dictionary(Of String, MailList)

        Using MysqlConn As New MySqlConnection(Settings.RobustMysqlConnection)
            Try
                MysqlConn.Open()

                Dim stm = "Select firstname, lastname, email, usertitle, principalid, userlevel, created from useraccounts"

                Using cmd = New MySqlCommand(stm, MysqlConn)

                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim Output As New MailList With {
                            .Firstname = reader.GetString(0),
                            .LastName = reader.GetString(1),
                            .Email = reader.GetString(2),
                            .Title = reader.GetString(3),
                            .Principalid = reader.GetString(4)
                            }

                            Dim Level = reader.GetInt32(5)

                            If Level < 0 Then
                                Output.Userlevel = "Disabled"
                            ElseIf Level >= 0 And Level < 100 Then
                                Output.Userlevel = "Enabled"
                            ElseIf Level >= 100 And Level < 200 Then
                                Output.Userlevel = "Wifi"
                            ElseIf Level >= 200 Then
                                Output.Userlevel = "God"
                            End If

                            Dim created = reader.GetInt32(6)
                            Dim datecreated = UnixTimestampToDateTime(created)
                            Output.Datestring = datecreated.ToString(CultureInfo.CurrentCulture)
                            Output.DiffDays = DateDiff(DateInterval.Day, datecreated, DateTime.Now).ToString("000000", Globalization.CultureInfo.CurrentCulture)
                            'Output.Assets = MysqlInterface.AssetCount(Output.Principalid).ToString("000000", Globalization.CultureInfo.CurrentCulture)

                            If Output.Firstname <> "GRID" And Output.LastName <> "SERVICES" Then
                                result.Add(Output.Principalid, Output)
                            End If
                        End While
                    End Using

                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try

        End Using

        Return result

    End Function

    ''' <summary>
    ''' Returns list of people and region UUID
    ''' </summary>
    Public Function GetGridUsers() As List(Of AvatarObject)

        '6f285c43-e656-42d9-b0e9-a78684fee15c;http://outworldz.com:9000/;Ferd Frederix

        Dim UserStmt = "Select UserID, LastRegionID from GridUser where  lastregionid <> '00000000-0000-0000-0000-000000000000'"
        Dim pattern As String = "(.*?);(.*?);(.*)$"

        Dim L As New List(Of AvatarObject)

        '0a44c403-93e4-4519-b620-bde38883d874;http://hg.osgrid.org:80/;andressa loren

        Using NewSQLConn As New MySqlConnection(Settings.RobustMysqlConnection)
            Try
                NewSQLConn.Open()
                Using cmd = New MySqlCommand(UserStmt, NewSQLConn)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            ' Debug.Print(reader.GetString(0))
                            Dim LongName = reader.GetString(0)
                            Dim UUID = reader.GetString(1)

                            For Each m In Regex.Matches(LongName, pattern)
                                Dim grid = m.Groups(2).Value.ToString
                                grid = grid.Replace("http://", "")
                                grid = grid.Replace("/", "")

                                Dim AvatarUUID = m.Groups(1).Value.ToString
                                Dim Avatar = m.Groups(3).Value.ToString & "@" & grid
                                Dim HGVisitors As New AvatarObject
                                HGVisitors.RegionID = UUID
                                HGVisitors.AvatarUUID = AvatarUUID
                                Dim parts As String() = m.Groups(3).Value.ToString.Split(" ".ToCharArray())
                                HGVisitors.FirstName = parts(0).Trim
                                HGVisitors.LastName = parts(1).Trim
                                HGVisitors.AgentName = m.Groups(3).Value.ToString
                                If IsInPresence(AvatarUUID) Then
                                    L.Add(HGVisitors)
                                End If

                            Next
                        End While
                    End Using
                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
        End Using
        Return L

    End Function

    Public Function GetInventoryList(AvatarUUID As String) As Dictionary(Of Integer, Integer)

        Dim result = New Dictionary(Of Integer, Integer)

        Using MysqlConn As New MySqlConnection(Settings.RobustMysqlConnection)
            Try
                MysqlConn.Open()

                Dim stm = "Select count(*), invtype from inventoryitems  where avatarid = @UUID group by invtype"

                Using cmd = New MySqlCommand(stm, MysqlConn)
                    cmd.Parameters.AddWithValue("@UUID", AvatarUUID)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim count = reader.GetInt32(0)
                            Dim type = reader.GetInt32(1)
                            result.Add(type, count)
                        End While
                    End Using

                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try

        End Using

        Return result

    End Function
    ''' <summary>
    ''' Check if a given key is in presence without a join.
    ''' </summary>
    ''' <param name="AvatarUUID"></param>
    ''' <returns></returns>
    Public Function IsInPresence(AvatarUUID As String) As Boolean

        Using NewSQLConn As New MySqlConnection(Settings.RobustMysqlConnection)

            Try
                NewSQLConn.Open()
                Dim stm As String = "SELECT count(*) from presence where presence.UserID = @UUID"
                Using cmd As New MySqlCommand(stm, NewSQLConn)
                    cmd.Parameters.AddWithValue("@UUID", AvatarUUID)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            If reader.GetInt32(0) > 0 Then
                                Return True
                            End If
                        End While
                    End Using
                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
        End Using

        Return False

    End Function

    ''' <summary>
    ''' Returns User name and region they are in, if any
    ''' </summary>
    ''' <returns>First name, Last Name, RegionUUID</returns>
    Public Function GetPresence() As List(Of AvatarObject)

        Dim L As New List(Of AvatarObject)

        Using NewSQLConn As New MySqlConnection(Settings.RobustMysqlConnection)

            Try
                NewSQLConn.Open()

                Dim stm As String = "SELECT useraccounts.PrincipalID, useraccounts.FirstName, useraccounts.LastName, RegionID FROM (presence INNER JOIN useraccounts ON presence.UserID = useraccounts.PrincipalID) where presence.regionid <> '00000000-0000-0000-0000-000000000000' "
                Using cmd As New MySqlCommand(stm, NewSQLConn)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            If reader.GetString(0).Length > 0 Then
                                Dim Avatar As New AvatarObject
                                Avatar.AvatarUUID = reader.GetString("PrincipalID")
                                Avatar.FirstName = reader.GetString("Firstname")
                                Avatar.LastName = reader.GetString("LastName")
                                Avatar.RegionID = reader.GetString("RegionID")
                                Avatar.AgentName = Avatar.FirstName & " " & Avatar.LastName
                                L.Add(Avatar)
                            End If
                        End While
                    End Using
                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
        End Using

        Return L

    End Function

    ''' <summary>
    ''' Finds where an avatar is
    ''' </summary>
    ''' <param name="AgentID"></param>
    ''' <returns>RegionID</returns>
    Public Function GetRegionFromAgentId(AgentID As String) As String

        Dim Val As String = ""
        If Settings.ServerType <> RobustServerName Then Return Val

        Using MysqlConn As New MySqlConnection(Settings.RobustMysqlConnection)
            Try
                MysqlConn.Open()
            Catch ex As Exception
                BreakPoint.Dump(ex)
                Return Val
            End Try

            Dim stm As String = "SELECT presence.RegionID FROM presence where presence.userid = @ID;"
            Try
                Using cmd As New MySqlCommand(stm, MysqlConn)
                    Try
                        cmd.Parameters.AddWithValue("@ID", AgentID)
                        Using reader As MySqlDataReader = cmd.ExecuteReader()
                            While reader.Read()
                                Val = reader.GetString(0)
                            End While
                        End Using
                    Catch ex As Exception
                        BreakPoint.Dump(ex)
                    End Try
                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try

        End Using

        Return Val

    End Function

    Public Function InAuth(AvatarUUID As String) As Boolean

        Using NewSQLConn As New MySqlConnection(Settings.RobustMysqlConnection)
            Try
                NewSQLConn.Open()
                Dim stm As String = "SELECT COUNT(*) as C FROM tosauth where avataruuid = @UUID"
                Using cmd As New MySqlCommand(stm, NewSQLConn)
                    cmd.Parameters.AddWithValue("@UUID", AvatarUUID)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Return CBool(reader.GetInt32(0))
                        End While
                    End Using
                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
        End Using
        Return False

    End Function

    ''' <summary>
    ''' Locates if an Agent is in a region
    ''' </summary>
    ''' <param name="regionuuid"></param>
    ''' <returns>boolean</returns>
    Public Function IsAgentInRegion(regionuuid As String) As Boolean

        If Settings.ServerType <> RobustServerName Then Return False
        If Not RegionEnabled(regionuuid) Then Return False
        For Each AgentObject In CachedAvatars
            If AgentObject.RegionID = regionuuid Then
                Return True
            End If
        Next
        Return False

    End Function

    Public Function MysqlGetUserData(uuid As String) As UserData

        Dim UD As New UserData
        Using MysqlConn As New MySqlConnection(Settings.RobustMysqlConnection)
            Try
                MysqlConn.Open()
                Dim stm = "select FirstName, LastName, Email, UserLevel, UserTitle from useraccounts where principalID = @UUID"
                Using cmd = New MySqlCommand(stm, MysqlConn)
                    cmd.Parameters.AddWithValue("@UUID", uuid)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            UD.FirstName = reader.GetString(0)
                            UD.LastName = reader.GetString(1)
                            UD.Email = reader.GetString(2)
                            UD.Level = reader.GetInt32(3)
                            UD.UserTitle = reader.GetString(4)
                            UD.PrincipalID = uuid
                        Else
                            UD.FirstName = "No record"
                        End If
                    End Using
                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
        End Using

        Return UD

    End Function

    Public Sub MysqlSaveUserData(ud As UserData)

        If ud Is Nothing Then Return

        Using MysqlConn As New MySqlConnection(Settings.RobustMysqlConnection)
            Try
                MysqlConn.Open()

                Dim stm = "update useraccounts set email=@email,usertitle=@utitle,userlevel=@level,firstname=@fname,lastname=@lname where PrincipalID=@UUID;"
                Using cmd = New MySqlCommand(stm, MysqlConn)
                    cmd.Parameters.AddWithValue("@level", ud.Level)
                    cmd.Parameters.AddWithValue("@UUID", ud.PrincipalID)
                    cmd.Parameters.AddWithValue("@fname", ud.FirstName)
                    cmd.Parameters.AddWithValue("@lname", ud.LastName)
                    cmd.Parameters.AddWithValue("@email", ud.Email)
                    cmd.Parameters.AddWithValue("@utitle", ud.UserTitle)
                    cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try

        End Using

    End Sub

#End Region

#Region "Prims"

    ''' <summary>
    ''' Number of prims in this region
    ''' </summary>
    ''' <param name="UUID">Region UUID</param>
    ''' <returns>integer</returns>
    Public Function GetPrimCount(UUID As String) As Integer

        Dim count As Integer
        Using MysqlConn = New MySqlConnection(Settings.RegionMySqlConnection)

            Try
                MysqlConn.Open()

                Dim stm = "select count(*) from prims where regionuuid = @UUID"

                Using cmd = New MySqlCommand(stm, MysqlConn)
                    cmd.Parameters.AddWithValue("@UUID", UUID)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            count = reader.GetInt32(0)
                        End If
                    End Using
                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try

        End Using

        Return count

    End Function

#End Region

#Region "Mysql Overhead"

    ''' <summary>
    ''' Queries Mysql to see if its up
    ''' </summary>
    ''' <returns>True if up</returns>
    Public Function IsMySqlRunning() As Boolean

        Dim version = QueryString("Select VERSION()")
        If version.Length > 0 Then
            MySqlRev = version
            PropMysqlExited = False
            MySQLIcon(True)
            Return True
        End If

        MySQLIcon(False)
        Return False

    End Function

    Public Sub MysqlConsole()

        StartMySQL()

        Dim QS As String
        If Settings.RootMysqlPassword.Length > 0 Then
            QS = $" -u root --port={Settings.MySqlRegionDBPort} -p{Settings.RootMysqlPassword}"
        Else
            QS = $" -u root --port={Settings.MySqlRegionDBPort}"
        End If

        Using p = New Process()
            Dim pi = New ProcessStartInfo With {
                .Arguments = QS,
                .FileName = """" & IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\mysql\bin\mysql.exe") & """",
                .UseShellExecute = True, ' so we can redirect streams and minimize
                .WindowStyle = ProcessWindowStyle.Normal
            }
            p.StartInfo = pi
            Try
                p.Start()
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
        End Using
    End Sub

    Public Sub MySQLIcon(running As Boolean)

        If Not running Then
            FormSetup.RestartMysqlIcon.Image = Global.Outworldz.My.Resources.nav_plain_red
        Else
            FormSetup.RestartMysqlIcon.Image = Global.Outworldz.My.Resources.check2
        End If

    End Sub

#End Region

#Region "Partners"

    Public Function MysqlGetPartner(p1 As String, mysetting As MySettings) As String

        If mysetting Is Nothing Then
            Return ""
        End If

        Dim answer As String = ""
        Using MysqlConn As New MySqlConnection(mysetting.RobustMysqlConnection)
            Try
                MysqlConn.Open()

                Dim Query1 = "Select profilePartner from userprofile where userUUID=@p1;"
                Using myCommand1 = New MySqlCommand(Query1) With {
                        .Connection = MysqlConn
                    }
                    myCommand1.Parameters.AddWithValue("@p1", p1)
                    answer = CStr(myCommand1.ExecuteScalar())
                    ' Debug.Print($"User={p1}, Partner={answer}")
                    If answer Is Nothing Or answer.Length = 0 Then
                        answer = "00000000-0000-0000-0000-000000000000"
                    End If
                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try

        End Using

        Return answer

    End Function

#End Region

#Region "Region Flags"

    Public Sub MysqlSetRegionFlagOnline(RegionUUID As String)

        Dim RegionFlag = GetFlag(RegionUUID)
        ' no need to update if its enabled
        If RegionFlag > 0 And RegionFlag Mod 20 = 0 Then Return

        RegionFlag += 20

        Using MysqlConn As New MySqlConnection(Settings.RobustMysqlConnection)
            Try
                MysqlConn.Open()

                Dim stm = "update regions set flags = @flag where uuid = @UUID;"
                Using cmd = New MySqlCommand(stm, MysqlConn)
                    cmd.Parameters.AddWithValue("@flag", RegionFlag)
                    cmd.Parameters.AddWithValue("@UUID", RegionUUID)
                    cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
        End Using

    End Sub

    ''' <summary>
    ''' Gets any parcels names that have rez rights on for everyone
    ''' </summary>
    ''' <param name="RegionUUID">Region UUID</param>
    ''' <returns>list of string</returns>
    Public Function ParcelPermissionsCheck(RegionUUID As String) As String

        Dim str As String = ""
        Using MysqlConn As New MySqlConnection(Settings.RegionMySqlConnection)
            Try
                MysqlConn.Open()
                Dim stm = "SELECT name, landflags FROM land where regionuuid = @UUID and ((landflags & 64) or (landflags & 16) );"
#Disable Warning CA2100
                Using cmd = New MySqlCommand(stm, MysqlConn)
#Enable Warning

                    cmd.Parameters.AddWithValue("@UUID", RegionUUID)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Dim ParcelName = reader.GetString(0)
                            Dim flag = reader.GetInt32(1)

                            If CBool((flag And 64) Or (flag And 16)) Then
                                str = $"{ParcelName} ("
                            Else
                                str = "-"
                                Return str
                            End If
                            If CBool(flag And 64) Then
                                str += "Build "
                            End If
                            If CBool(flag And 16) Then
                                str += "Land "
                            End If

                            str += ")"

                        End If
                    End Using
                End Using
            Catch ex As Exception
                BreakPoint.Print(ex.Message)
            End Try
        End Using
        Return str

    End Function

    ''' <summary>
    ''' Disable on-line flag for suspended regions
    ''' </summary>
    ''' <param name="RegionUUID">Region UUID</param>
    ''' <summary>
    ''' Returns boolean if a region exists in the regions table
    ''' </summary>
    ''' <param name="UUID">Region UUID</param>
    ''' <returns>True is region is in table</returns>
    Public Function RegionIsRegistered(UUID As String) As Boolean

        Dim count As Integer

        Using MysqlConn As New MySqlConnection(Settings.RobustMysqlConnection)
            Try
                MysqlConn.Open()

                Dim stm = "Select count(*) as cnt from regions where uuid = @UUID"

                Using cmd = New MySqlCommand(stm, MysqlConn)
                    cmd.Parameters.AddWithValue("@UUID", UUID)

                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            count = CInt(reader.GetInt16("cnt"))
                        End If
                    End Using

                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
        End Using

        Return CBool(count)

    End Function

    ''' <summary>
    ''' Returns boolean if a region exists in the regions table
    ''' </summary>
    ''' <param name="UUID">Region UUID</param>
    ''' <returns>True is region is in table</returns>
    Public Function RegionIsRegisteredOnline(UUID As String) As Boolean

        Dim count As Integer

        Using MysqlConn As New MySqlConnection(Settings.RobustMysqlConnection)
            Try
                MysqlConn.Open()
                Dim stm = "Select count(*) as cnt from regions where uuid = @UUID And flags & 4 = 4 "

                Using cmd = New MySqlCommand(stm, MysqlConn)
                    cmd.Parameters.AddWithValue("@UUID", UUID)

                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            count = CInt(reader.GetInt16("cnt"))
                        End If
                    End Using

                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
        End Using

        Return CBool(count)

    End Function

    Public Sub SetEstate(UUID As String, EstateID As Integer)

        If Not IsMySqlRunning() Then Return
        Dim exists As Boolean

        Using MysqlConn As New MySqlConnection(Settings.RegionMySqlConnection)
            Try
                MysqlConn.Open()
                Dim stm = "Select EstateID from estate_map where regionid = @UUID"

                Using cmd = New MySqlCommand(stm, MysqlConn)
                    cmd.Parameters.AddWithValue("@UUID", UUID)

                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            exists = True
                        End If
                    End Using

                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try

        End Using

        If Not exists Then

            Using MysqlConn1 As New MySqlConnection(Settings.RegionMySqlConnection)
                Try
                    MysqlConn1.Open()
                    Dim stm1 = "insert into estate_map (RegionID, EstateID) values (@UUID, @EID)"
                    Try
                        Using cmd1 = New MySqlCommand(stm1, MysqlConn1)
                            cmd1.Parameters.AddWithValue("@UUID", UUID)
                            cmd1.Parameters.AddWithValue("@EID", EstateID)
                            cmd1.ExecuteNonQuery()
                        End Using
                    Catch ex As Exception
                        BreakPoint.Dump(ex)
                    End Try
                Catch ex As Exception
                    BreakPoint.Dump(ex)
                End Try
            End Using
        End If
    End Sub

    ''' <summary>
    ''' Sets region flag to offline for one region
    ''' </summary>
    ''' <param name="RegionUUID">RegionUUID</param>
    Public Sub SetRegionOffline(RegionUUID As String)

        ' bit 4 is the on-line bit
        Using MysqlConn As New MySqlConnection(Settings.RobustMysqlConnection)
            Try
                MysqlConn.Open()
                Dim stm = "update regions set flags = (flags) & ~4 where uuid = @UUID;"
#Disable Warning CA2100
                Using cmd = New MySqlCommand(stm, MysqlConn)
#Enable Warning
                    cmd.Parameters.AddWithValue("@UUID", RegionUUID)
                    cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                BreakPoint.Print(ex.Message)
            End Try
        End Using

    End Sub

    Public Sub SetRegionOnline(RegionUUID As String)

        ' bit 4 is the on-line bit
        Using MysqlConn As New MySqlConnection(Settings.RobustMysqlConnection)
            Try
                MysqlConn.Open()
                Dim stm = "update regions set flags = (flags) | 4 where uuid = @UUID;"
#Disable Warning CA2100
                Using cmd = New MySqlCommand(stm, MysqlConn)
#Enable Warning
                    cmd.Parameters.AddWithValue("@UUID", RegionUUID)
                    cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                BreakPoint.Print(ex.Message)
            End Try
        End Using

    End Sub

    Private Function GetFlag(RegionUUID As String) As Integer
        Dim Val = 0
        Using Flags As New MySqlConnection(Settings.RobustMysqlConnection)
            Try
                Flags.Open()
                Dim stm = "select flags from regions where uuid=@UUID"
                Using cmd = New MySqlCommand(stm, Flags)
                    cmd.Parameters.AddWithValue("@UUID", RegionUUID)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Val = reader.GetInt32("flags")
                        End If
                    End Using
                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try

        End Using

        Return Val
    End Function

#End Region

#Region "Offline Email"

    Public Function OfflineEmails() As List(Of EmailData)

        Dim emails = New List(Of EmailData)
        Dim result = New EmailData
        Using MysqlConn As New MySqlConnection(Settings.RobustMysqlConnection)
            Try
                MysqlConn.Open()
                Dim stm = "Select id, principalId, fromid, message from im_offline"

                Using cmd = New MySqlCommand(stm, MysqlConn)

                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            result.Id = reader.GetInt32(0)
                            result.Principalid = reader.GetString(1)
                            result.Fromid = reader.GetString(2)
                            result.Message = reader.GetString(3)
                            emails.Add(result)
                        End If
                    End Using
                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
        End Using

        Return emails

    End Function

#End Region

#Region "Search"

    Public Sub SetupLocalSearch()

        If Settings.ServerType <> "Robust" Then Return

        ' modify this to migrate search database upwards a rev
        If Settings.SearchMigration < 3 Then

            MysqlInterface.DeleteSearchDatabase()

            TextPrint(My.Resources.Setup_search)
            Dim pi = New ProcessStartInfo()

            FileIO.FileSystem.CurrentDirectory = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\mysql\bin")
            pi.FileName = "Create_OsSearch.bat"
            pi.UseShellExecute = True
            pi.CreateNoWindow = False
            pi.WindowStyle = ProcessWindowStyle.Hidden
            Using ProcessMysql = New Process With {
                    .StartInfo = pi
                }

                Try
                    ProcessMysql.Start()
                    ProcessMysql.WaitForExit()
                Catch ex As Exception
                    ErrorLog("Error ProcessMysql failed to launch " & ex.Message)
                    FileIO.FileSystem.CurrentDirectory = Settings.CurrentDirectory
                    Return
                End Try
            End Using

            FileIO.FileSystem.CurrentDirectory = Settings.CurrentDirectory

            Settings.SearchMigration = 3
            Settings.SaveSettings()

        End If

    End Sub

#End Region

#Region "Stats"

    Public Sub SetupSimStats()

        Dim pi = New ProcessStartInfo With {
                .FileName = "Create_Simstats.bat",
                .UseShellExecute = True,
                .CreateNoWindow = True,
                .WindowStyle = ProcessWindowStyle.Minimized,
                .WorkingDirectory = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\mysql\bin\")
            }
        Using Mutelist = New Process With {
                .StartInfo = pi
            }

            Try
                Mutelist.Start()
                Mutelist.WaitForExit()
            Catch ex As Exception
                BreakPoint.Dump(ex)
                ErrorLog("Could Not create SimStats Database " & ex.Message)
                FileIO.FileSystem.CurrentDirectory = Settings.CurrentDirectory
            End Try
        End Using

    End Sub

#End Region

#Region "Setup Database"

    Public Sub SetupTOSTable()
        If Settings.ServerType <> "Robust" Then Return

        Dim pi = New ProcessStartInfo()

        FileIO.FileSystem.CurrentDirectory = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\mysql\bin")
        pi.FileName = "Create_TOS.bat"
        pi.UseShellExecute = True
        pi.CreateNoWindow = False
        pi.WindowStyle = ProcessWindowStyle.Hidden
        Using ProcessMysql = New Process With {
                .StartInfo = pi
            }

            Try
                ProcessMysql.Start()
                ProcessMysql.WaitForExit()
            Catch ex As Exception
                ErrorLog("Error Setting TOS failed to launch " & ex.Message)
                FileIO.FileSystem.CurrentDirectory = Settings.CurrentDirectory
                Return
            End Try
        End Using

        FileIO.FileSystem.CurrentDirectory = Settings.CurrentDirectory

    End Sub

    Public Sub SetupWordPress()

        If Not Settings.ApacheEnable Then Return
        Dim pi = New ProcessStartInfo With {
            .FileName = "Create_WordPress.bat",
            .UseShellExecute = True,
            .CreateNoWindow = True,
            .WindowStyle = ProcessWindowStyle.Minimized,
            .WorkingDirectory = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\mysql\bin")
        }
        Using MysqlWordpress = New Process With {
            .StartInfo = pi
        }

            Try
                MysqlWordpress.Start()
                MysqlWordpress.WaitForExit()
            Catch ex As Exception
                BreakPoint.Dump(ex)
                ErrorLog("Could Not create WordPress Database " & ex.Message)
                FileIO.FileSystem.CurrentDirectory = Settings.CurrentDirectory
                Return
            End Try
        End Using

    End Sub

    Public Function UnixTimestampToDateTime(unixTimestamp As Double) As DateTime

        ' Unix time stamp Is seconds past epoch
        Dim dtDateTime = New DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)
        dtDateTime = dtDateTime.AddSeconds(unixTimestamp).ToLocalTime()
        Return dtDateTime

    End Function

    ''' <summary>
    ''' Updates a Mysql Password
    ''' </summary>
    ''' <param name="NewPassword">New Password</param>
    ''' <param name="Username">User Name</param>
    Public Function UpdateMysqlPassword(NewPassword As String, Username As String) As String

        StartMySQL()

        Using MysqlConn As New MySqlConnection(Settings.RootMysqlConnection)
            Try
                MysqlConn.Open()

                Dim stm = $"Update mysql.user Set Password=PASSWORD(@NewPassword) WHERE User=@User;FLUSH PRIVILEGES;"
                Using cmd = New MySqlCommand(stm, MysqlConn)
                    cmd.Parameters.AddWithValue("@user", Username)
                    cmd.Parameters.AddWithValue("@NewPassword", NewPassword)
                    cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
                Return ex.Message
            End Try
        End Using
        Return ""

    End Function

    Private Sub CreateService()

        ' create test program slants the other way:
        Dim testProgram As String = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\MySQL\bin\InstallAsAService.bat")
        DeleteFile(testProgram)

        Try
            Using outputFile As New StreamWriter(testProgram, False)
                outputFile.WriteLine("@REM Program to run MySQL as a Service" & vbCrLf +
            "mysqld.exe --install MySQL --defaults-file=" & """" & Settings.CurrentSlashDir & "/OutworldzFiles/mysql/my.ini" & """" & vbCrLf & "net start MySQL" & vbCrLf)
                outputFile.Flush()
            End Using
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

    End Sub

    Private Sub CreateStopMySql()

        ' create test program slants the other way:
        Dim testProgram As String = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\MySQL\bin\StopMySQL.bat")
        DeleteFile(testProgram)
        Try
            Dim QS As String
            If Settings.RootMysqlPassword.Length > 0 Then
                QS = $" -u root --port={Settings.MySqlRobustDBPort} -p{Settings.RootMysqlPassword}"
            Else
                QS = $" -u root --port={Settings.MySqlRobustDBPort}"
            End If

            Using outputFile As New StreamWriter(testProgram, False)
                outputFile.WriteLine($"@REM Program to stop MySQL{vbCrLf}mysqladmin.exe -u root {QS} shutdown{vbCrLf}@pause{vbCrLf}")
                outputFile.Flush()
            End Using
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

    End Sub

    Private Sub DeleteSearchDatabase()

        Using MysqlConn As New MySqlConnection(Settings.RobustMysqlConnection)
            Try
                MysqlConn.Open()
                Dim stm = "drop database ossearch"
                Using cmd = New MySqlCommand(stm, MysqlConn)
                    cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
        End Using

    End Sub

    Private Sub MakeMysql()

        Dim fname As String = ""
        Dim m As String = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\MySQL")
        If Not System.IO.File.Exists(IO.Path.Combine(m, "Data\ibdata1")) Then
            TextPrint(My.Resources.Create_DB)
            Try
                Using zip = New ZipFile(IO.Path.Combine(m, "Blank-MySQL-Data-folder.zip"))
                    zip.UseZip64WhenSaving = Zip64Option.AsNecessary
                    Dim extractPath = $"{Path.GetFullPath(Settings.CurrentDirectory)}\OutworldzFiles\MySQL"
                    If (Not extractPath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal)) Then
                        extractPath += Path.DirectorySeparatorChar
                    End If
                    zip.ExtractAll(extractPath)
                End Using
            Catch ex As Exception
                TextPrint("Unable to extract file: " & fname & ":" & ex.Message)
                Thread.Sleep(3000)
                Application.DoEvents()
            End Try

        End If

    End Sub

    Private Sub Mysql_Exited(ByVal sender As Object, ByVal e As EventArgs) Handles ProcessMySql.Exited

        FormSetup.RestartMysqlIcon.Image = Global.Outworldz.My.Resources.nav_plain_red

        If PropAborting Then Return

        If Settings.RestartOnCrash And MysqlCrashCounter < 10 Then
            MysqlCrashCounter += 1
            PropMysqlExited = True
            Return
        End If
        MysqlCrashCounter = 0
        Dim MysqlLog As String = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\mysql\data")
        Dim files As Array = Nothing
        Try
            files = Directory.GetFiles(MysqlLog, "*.err", SearchOption.TopDirectoryOnly)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

        If files IsNot Nothing Then
            Dim yesno = MsgBox(My.Resources.MySql_Exited, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Error_word)
            If (yesno = vbYes) Then

                For Each FileName As String In files
                    Baretail("""" & FileName & """")
                Next
            End If
        Else
            PropAborting = True
            MsgBox(My.Resources.Error_word, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Error_word)
        End If

    End Sub

#End Region

#Region "Visitors"

    ''' <summary>
    ''' Adds visitor X and Y to Visitor database each minute
    ''' </summary>
    ''' <param name="AvatarName"></param>
    ''' <param name="RegionName"></param>
    ''' <param name="LocX"></param>
    ''' <param name="LocY"></param>
    Public Sub VisitorCount()

        If Visitor.Count > 0 Then
            Using MysqlConn1 As New MySqlConnection(Settings.RobustMysqlConnection)
                Try
                    Dim stm1 = "insert into visitor (name, regionname, locationX, locationY) values (@NAME, @REGIONNAME, @LOCX, @LOCY)"
                    MysqlConn1.Open()
                    For Each Visit As KeyValuePair(Of String, String) In Visitor
                        Application.DoEvents()
                        Dim RegionName = Visit.Value
                        Dim RegionUUID = FindRegionByName(RegionName)
                        Dim result As List(Of AvatarData) = RPC_admin_get_agent_list(RegionUUID)
                        For Each Avi In result
                            Using cmd1 = New MySqlCommand(stm1, MysqlConn1)
                                cmd1.Parameters.AddWithValue("@NAME", Avi.AvatarName)
                                cmd1.Parameters.AddWithValue("@REGIONNAME", RegionName)
                                cmd1.Parameters.AddWithValue("@LOCX", Avi.X)
                                cmd1.Parameters.AddWithValue("@LOCY", Avi.Y)
                                cmd1.ExecuteNonQuery()
                                Statrecord(RegionName)
                            End Using
                        Next
                    Next
                Catch ex As Exception
                    BreakPoint.Dump(ex)
                End Try
            End Using
        End If

    End Sub

    Private Sub Statrecord(RegionName As String)

        Dim UUID = FindRegionByName(RegionName)
        Dim val As String = ""
        Using MysqlConn As New MySqlConnection(Settings.RobustMysqlConnection)
            Try
                Dim stm = "select regionname from stats where UUID=@UUID;"
                MysqlConn.Open()
                Using cmd = New MySqlCommand(stm, MysqlConn)
                    cmd.Parameters.AddWithValue("@UUID", UUID)

                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            val = reader.GetString(0)
                        End If
                    End Using
                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
        End Using

        If val.Length > 0 Then
            Try
                Using MysqlConn As New MySqlConnection(Settings.RobustMysqlConnection)
                    Dim stm = "update stats set regionname=@REGIONNAME,regionsize=@REGIONSIZE,locationx=@LOCX,locationy=@LOCY where UUID=@UUID"

                    MysqlConn.Open()
                    Using cmd = New MySqlCommand(stm, MysqlConn)
                        cmd.Parameters.AddWithValue("@UUID", UUID)
                        cmd.Parameters.AddWithValue("@REGIONNAME", RegionName)
                        cmd.Parameters.AddWithValue("@REGIONSIZE", SizeX(UUID))
                        cmd.Parameters.AddWithValue("@LOCX", Coord_X(UUID))
                        cmd.Parameters.AddWithValue("@LOCY", Coord_Y(UUID))
                        cmd.ExecuteNonQuery()
                    End Using
                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
        Else
            Try
                Using MysqlConn As New MySqlConnection(Settings.RobustMysqlConnection)
                    Dim stm = "insert into stats (regionname,regionsize,locationx,locationy,UUID) values (@REGIONNAME,@REGIONSIZE,@LOCX,@LOCY,@UUID)"

                    MysqlConn.Open()
                    Using cmd = New MySqlCommand(stm, MysqlConn)
                        cmd.Parameters.AddWithValue("@UUID", UUID)
                        cmd.Parameters.AddWithValue("@REGIONNAME", RegionName)
                        cmd.Parameters.AddWithValue("@REGIONSIZE", SizeX(UUID))
                        cmd.Parameters.AddWithValue("@LOCX", Coord_X(UUID))
                        cmd.Parameters.AddWithValue("@LOCY", Coord_Y(UUID))
                        cmd.ExecuteNonQuery()
                    End Using

                    Make_Region_Map(UUID)

                End Using
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
        End If

    End Sub

#End Region

#Region "Tuning"

    ''' <summary>
    ''' Dynamically adjust Mysql for size of DB
    ''' </summary>
    ''' <returns></returns>
    Public Function Total_InnoDB_Bytes() As Double

        Dim Bytes As Double
        Using MysqlConn As New MySqlConnection(Settings.RobustMysqlConnection)
            Try
                MysqlConn.Open()
                Dim stm = "SELECT CEILING(Total_InnoDB_Bytes*1.6/POWER(1024,3)) RIBPS FROM
    (SELECT SUM(data_length+index_length) Total_InnoDB_Bytes
    FROM information_schema.tables WHERE engine='InnoDB') A;"

                Using cmd = New MySqlCommand(stm, MysqlConn)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Bytes = reader.GetDouble(0)
                        End If
                    End Using
                End Using
            Catch ex As Exception
                Bytes = 1
            End Try
        End Using
        If Bytes < 1 Then Bytes = 1
        If Bytes > 4 Then Bytes = 4
        Return Bytes

    End Function

#End Region

End Module

#Region "Classes"

Public Class EmailData

    Private _fromid As String = ""
    Private _id As Integer
    Private _message As String = ""
    Private _principalid As String = ""

    Public Property Fromid As String
        Get
            Return _fromid
        End Get
        Set(value As String)
            _fromid = value
        End Set
    End Property

    Public Property Id As Integer
        Get
            Return _id
        End Get
        Set(value As Integer)
            _id = value
        End Set
    End Property

    Public Property Message As String
        Get
            Return _message
        End Get
        Set(value As String)
            _message = value
        End Set
    End Property

    Public Property Principalid As String
        Get
            Return _principalid
        End Get
        Set(value As String)
            _principalid = value
        End Set
    End Property

End Class

Public Class MailList

    Private _assets As String = ""
    Private _datestring As String = ""
    Private _diffDays As String = ""
    Private _email As String = ""
    Private _firstname As String = ""
    Private _lastName As String = ""
    Private _principalid As String = ""
    Private _title As String = ""
    Private _userlevel As String = ""

    Public Property Assets As String
        Get
            Return _assets
        End Get
        Set(value As String)
            _assets = value
        End Set
    End Property

    Public Property Datestring As String
        Get
            Return _datestring
        End Get
        Set(value As String)
            _datestring = value
        End Set
    End Property

    Public Property DiffDays As String
        Get
            Return _diffDays
        End Get
        Set(value As String)
            _diffDays = value
        End Set
    End Property

    Public Property Email As String
        Get
            Return _email
        End Get
        Set(value As String)
            _email = value
        End Set
    End Property

    Public Property Firstname As String
        Get
            Return _firstname
        End Get
        Set(value As String)
            _firstname = value
        End Set
    End Property

    Public Property LastName As String
        Get
            Return _lastName
        End Get
        Set(value As String)
            _lastName = value
        End Set
    End Property

    Public Property Principalid As String
        Get
            Return _principalid
        End Get
        Set(value As String)
            _principalid = value
        End Set
    End Property

    Public Property Title As String
        Get
            Return _title
        End Get
        Set(value As String)
            _title = value
        End Set
    End Property

    Public Property Userlevel As String
        Get
            Return _userlevel
        End Get
        Set(value As String)
            _userlevel = value
        End Set
    End Property

    Public Overrides Function Equals(obj As Object) As Boolean
        Dim list = TryCast(obj, MailList)
        Return list IsNot Nothing AndAlso
               Assets = list.Assets
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return HashCode.Combine(Assets)
    End Function

End Class

Public Class UserData

    Private _email As String = ""
    Private _firstName As String = ""
    Private _lastName As String = ""
    Private _level As Integer = -1
    Private _principalID As String = ""
    Private _userTitle As String = ""
    Private _RegionUUID As String = ""

    Public Property RegionUUID As String
        Get
            Return _RegionUUID
        End Get
        Set(value As String)
            _RegionUUID = value
        End Set
    End Property

    Public Property Email As String
        Get
            Return _email
        End Get
        Set(value As String)
            _email = value
        End Set
    End Property

    Public Property FirstName As String
        Get
            Return _firstName
        End Get
        Set(value As String)
            _firstName = value
        End Set
    End Property

    Public Property LastName As String
        Get
            Return _lastName
        End Get
        Set(value As String)
            _lastName = value
        End Set
    End Property

    Public Property Level As Integer
        Get
            Return _level
        End Get
        Set(value As Integer)
            _level = value
        End Set
    End Property

    Public Property PrincipalID As String
        Get
            Return _principalID
        End Get
        Set(value As String)
            _principalID = value
        End Set
    End Property

    Public Property UserTitle As String
        Get
            Return _userTitle
        End Get
        Set(value As String)
            _userTitle = value
        End Set
    End Property

#End Region

End Class
