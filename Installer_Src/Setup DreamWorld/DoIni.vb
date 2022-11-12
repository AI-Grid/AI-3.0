#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.IO
Imports System.Text.RegularExpressions

Module DoIni

#Region "Apache"

    Public Function DoApache() As Boolean

        If Not Settings.ApacheEnable Then Return False
        TextPrint("->Set Apache")

        ' lean rightward paths for Apache
        Dim ini = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Apache\conf\httpd.conf")
        Dim Apache = New IniWriter(ini)
        Apache.Write("Listen", $"Listen {Settings.ApachePort}")
        Apache.Write("Define SRVROOT", $"Define SRVROOT ""{Settings.CurrentSlashDir}/OutworldzFiles/Apache""")
        Apache.Write("DocumentRoot", $"DocumentRoot ""{Settings.CurrentSlashDir}/OutworldzFiles/Apache/htdocs""")
        Apache.Write("Use VDir", $"Use VDir ""{Settings.CurrentSlashDir}/OutworldzFiles/Apache/htdocs""")
        Apache.Write("PHPIniDir", $"PHPIniDir ""{Settings.CurrentSlashDir}/OutworldzFiles/PHP7""")
        Apache.Write("ServerName", "ServerName " & Settings.PublicIP)
        Apache.Write("ServerAdmin", "ServerAdmin " & Settings.AdminEmail)
        Apache.Write("<VirtualHost", $"<VirtualHost  *:{Convert.ToString(Settings.ApachePort, Globalization.CultureInfo.InvariantCulture)}>")
        Apache.Write("ErrorLog", $"ErrorLog ""|bin/rotatelogs.exe  -l \""{Settings.CurrentSlashDir}/OutworldzFiles/Logs/Apache/Error-%Y-%m-%d.log\"" 86400""")
        Apache.Write("CustomLog", $"CustomLog ""|bin/rotatelogs.exe -l \""{Settings.CurrentSlashDir}/OutworldzFiles/Logs/Apache/access-%Y-%m-%d.log\"" 86400"" common env=!dontlog")
        Apache.Write("LoadModule php7_module", $"LoadModule php7_module ""{Settings.CurrentSlashDir}/OutworldzFiles/PHP7/php7apache2_4.dll""")

        If Settings.SSLIsInstalled And Settings.SSLEnabled Then
            Apache.Write("UnDefine SSL", "Define SSL")
        Else
            Apache.Write("Define SSL", "UnDefine SSL")
        End If

        Apache.Save("httpd.conf")

        DeleteFolder(IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\PHP5"))
        FileIO.FileSystem.CreateDirectory(IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Logs\Apache"))

        ' lean rightward paths for Apache
        ini = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Apache\conf\extra\httpd-ssl.conf")
        Dim SSL = New IniWriter(ini)
        SSL.Write("Listen", $"Listen {Settings.LANIP()}:443")
        SSL.Write("ServerName", "ServerName " & Settings.PublicIP)
        SSL.Write("Define SRVROOT", $"Define SRVROOT ""{Settings.CurrentSlashDir}/OutworldzFiles/Apache""")
        SSL.Write("DocumentRoot", $"DocumentRoot ""{Settings.CurrentSlashDir}/OutworldzFiles/Apache/htdocs""")
        SSL.Write("Use VDir", $"Use VDir ""{Settings.CurrentSlashDir}/OutworldzFiles/Apache/htdocs""")
        SSL.Write("ServerName", "ServerName " & Settings.PublicIP)
        SSL.Write("ServerAdmin", "ServerAdmin " & Settings.AdminEmail)

        ' Install Certificates
        SSL.Write("SSLCertificateFile", $"SSLCertificateFile ""{Settings.CurrentSlashDir}/Outworldzfiles/Apache/Certs/{Settings.DnsName}-chain.pem""")
        SSL.Write("SSLCertificateKeyFile", $"SSLCertificateKeyFile ""{Settings.CurrentSlashDir}/Outworldzfiles/Apache/Certs/{Settings.DnsName}-key.pem""")
        SSL.Write("SSLCertificateChainFile", $"SSLCertificateChainFile ""{Settings.CurrentSlashDir}/Outworldzfiles/Apache/Certs/{Settings.DnsName}-crt.pem""")
        SSL.Write("SSLCACertificateFile", $"SSLCACertificateFile ""{Settings.CurrentSlashDir}/Outworldzfiles/Apache/Certs/lets-encrypt-r3.pem""")

        SSL.Save("httpd-ssl.conf")

        Return False

    End Function

#End Region

#Region "Birds"

    Public Function DoBirds() As Boolean

        If Not Settings.BirdsModuleStartup Then Return False
        TextPrint("->Set Birds")
        Dim BirdFile = Settings.OpensimBinPath & "addon-modules\OpenSimBirds\config\OpenSimBirds.ini"

        DeleteFile(BirdFile)

        Dim BirdData As String = ""

        ' Birds setup per region
        For Each RegionUUID In RegionUuids()
            Application.DoEvents()
            Dim Name = Region_Name(RegionUUID)

            If Settings.BirdsModuleStartup And Birds(RegionUUID) = "True" Then

                BirdData = BirdData & "[" & Name & "]" & vbCrLf &
            ";this Is the default And determines whether the module does anything" & vbCrLf &
            "BirdsModuleStartup = True" & vbCrLf & vbCrLf &
            ";set to false to disable the birds from appearing in this region" & vbCrLf &
            "BirdsEnabled = True" & vbCrLf & vbCrLf &
            ";which channel do we listen on for in world commands" & vbCrLf &
            "BirdsChatChannel = " & CStr(Settings.BirdsChatChannel) & vbCrLf & vbCrLf &
            ";the number of birds to flock" & vbCrLf &
            "BirdsFlockSize = " & CStr(Settings.BirdsFlockSize) & vbCrLf & vbCrLf &
            ";how far each bird can travel per update" & vbCrLf &
            "BirdsMaxSpeed = " & Settings.BirdsMaxSpeed.ToString(Globalization.CultureInfo.InvariantCulture) & vbCrLf & vbCrLf &
            ";the maximum acceleration allowed to the current velocity of the bird" & vbCrLf &
            "BirdsMaxForce = " & Settings.BirdsMaxForce.ToString(Globalization.CultureInfo.InvariantCulture) & vbCrLf & vbCrLf &
            ";max distance for other birds to be considered in the same flock as us" & vbCrLf &
            "BirdsNeighbourDistance = " & Settings.BirdsNeighbourDistance.ToString(Globalization.CultureInfo.InvariantCulture) & vbCrLf & vbCrLf &
            ";how far away from other birds we would Like To stay" & vbCrLf &
            "BirdsDesiredSeparation = " & Settings.BirdsDesiredSeparation.ToString(Globalization.CultureInfo.InvariantCulture) & vbCrLf & vbCrLf &
            ";how close To the edges Of things can we Get without being worried" & vbCrLf &
            "BirdsTolerance = " & Settings.BirdsTolerance.ToString(Globalization.CultureInfo.InvariantCulture) & vbCrLf & vbCrLf &
            ";how close To the edge Of a region can we Get?" & vbCrLf &
            "BirdsBorderSize = " & Settings.BirdsBorderSize.ToString(Globalization.CultureInfo.InvariantCulture) & vbCrLf & vbCrLf &
            ";how high are we allowed To flock" & vbCrLf &
            "BirdsMaxHeight = " & Settings.BirdsMaxHeight.ToString(Globalization.CultureInfo.InvariantCulture) & vbCrLf & vbCrLf &
            ";By Default the Module will create a flock Of plain wooden spheres," & vbCrLf &
            ";however this can be overridden To the name Of an existing prim that" & vbCrLf &
            ";needs To already exist In the scene - i.e. be rezzed In the region." & vbCrLf &
            "BirdsPrim = " & Settings.BirdsPrim & vbCrLf & vbCrLf &
            ";who Is allowed to send commands via chat Or script List of UUIDs Or ESTATE_OWNER Or ESTATE_MANAGER" & vbCrLf &
            ";Or everyone if Not specified" & vbCrLf &
            "BirdsAllowedControllers = ESTATE_OWNER, ESTATE_MANAGER" & vbCrLf & vbCrLf & vbCrLf

            End If

        Next
        IO.File.WriteAllText(BirdFile, BirdData, System.Text.Encoding.Default) 'The text file will be created if it does not already exist

        Return False

    End Function

#End Region

#Region "Estates"

    Public Function DoEstates() As Boolean

        If Settings.ServerType = RobustServerName Then
            Dim INI = New LoadIni(IO.Path.Combine(Settings.OpensimBinPath, "Estates\Estates.ini"), ";", System.Text.Encoding.UTF8)
            Dim AvatarUUID As String = GetAviUUUD(Settings.SurroundOwner)
            INI.SetIni("SimSurround", "Owner", AvatarUUID)
            INI.SaveIni()
        Else
            Dim INI = New LoadIni(IO.Path.Combine(Settings.OpensimBinPath, "Estates\Estates.ini"), ";", System.Text.Encoding.UTF8)
            INI.SetIni("SimSurround", "Owner", "")
            INI.SaveIni()
        End If
        Return False

    End Function

#End Region

#Region "Money"

    Public Function DoCurrency() As Boolean

        If DoGloebit() Then Return True ' error = true
        If DoAnotherCurrency() Then Return True

        ' No error
        Return False

    End Function

    Private Function DoAnotherCurrency() As Boolean

        Return False

    End Function

    Private Function DoGloebit() As Boolean

        'Gloebit.ini
        Dim INI = New LoadIni(Settings.OpensimBinPath & "config-addon-opensim\Gloebit.ini", ";", System.Text.Encoding.UTF8)
        ' always select the money symbol and the DLL's
        SelectMoneySymbol(INI)

        If Not Settings.GloebitsEnable Then Return False

        INI.SetIni("Gloebit", "Enabled", CStr(Settings.GloebitsEnable))
        INI.SetIni("Gloebit", "GLBShowNewSessionAuthIM", CStr(Settings.GlbShowNewSessionAuthIM))
        INI.SetIni("Gloebit", "GLBShowNewSessionPurchaseIM", CStr(Settings.GlbShowNewSessionPurchaseIM))
        INI.SetIni("Gloebit", "GLBShowWelcomeMessage", CStr(Settings.GlbShowWelcomeMessage))

        INI.SetIni("Gloebit", "GLBEnvironment", "production")
        INI.SetIni("Gloebit", "GLBKey", Settings.GLProdKey)
        INI.SetIni("Gloebit", "GLBSecret", Settings.GLProdSecret)

        INI.SetIni("Gloebit", "GLBOwnerName", Settings.GlbOwnerName)
        INI.SetIni("Gloebit", "GLBOwnerEmail", Settings.GlbOwnerEmail)

        If Settings.ServerType = RobustServerName Then
            INI.SetIni("Gloebit", "GLBSpecificConnectionString", Settings.RobustDBConnection)
        Else
            INI.SetIni("Gloebit", "GLBSpecificConnectionString", Settings.RegionDBConnection)
        End If

        INI.SaveIni()

        Return False

    End Function

    Private Sub SelectMoneySymbol(INI As LoadIni)

        DeleteFile(IO.Path.Combine(Settings.OpensimBinPath, "jOpenSim.Money.dll"))
        If Settings.GloebitsEnable Then
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

#End Region

    Public Function DoGrid() As Boolean

        Try
            TextPrint("->Set Grid.ini")

            ' Put that grid.ini file in place
            Dim Src = IO.Path.Combine(Settings.OpensimBinPath & "config-include\Proto")
            Src = IO.Path.Combine(Src, Settings.ServerType)
            Src = IO.Path.Combine(Src, "Grid.ini")

            Dim dest = IO.Path.Combine(Settings.OpensimBinPath, "config-include\Grid.ini")

            CopyFileFast(Src, dest)
            Return False
        Catch
            Return True
        End Try

    End Function

    Public Function DoGridCommon() As Boolean

        Try
            TextPrint("->Set GridCommon.ini")

            'Choose a GridCommon.ini to use.
            Dim GridCommon As String = ""

            Select Case Settings.ServerType
                Case RobustServerName
                    If Settings.CMS = JOpensim Then
                        GridCommon = "Joomla\Gridcommon.ini"
                    Else
                        GridCommon = "Robust\Gridcommon.ini"
                    End If
                Case RegionServerName
                    GridCommon = "Region\Gridcommon.ini"
                Case OsgridServer
                    GridCommon = "OsGrid\Gridcommon.ini"
                    Settings.HttpPort = 80
                Case MetroServer
                    GridCommon = "Metro\Gridcommon.ini"
            End Select

            ' Put that gridcommon.ini file in place
            Dim s = IO.Path.Combine(Settings.OpensimBinPath & "config-include\Proto")
            s = IO.Path.Combine(s, GridCommon)

            Dim d = IO.Path.Combine(Settings.OpensimBinPath, "config-include\")
            d = IO.Path.Combine(d, "GridCommon.ini")

            ' set the defaults in the INI for the viewer to use. Painful as it's a Left hand side edit
            Dim reader As IO.StreamReader
            Dim line As String

            Try
                ' make a long list of the various regions with region_ at the start
                Dim Authorizationlist As String = ""
                For Each RegionUUID In RegionUuids()
                    Dim RegionName = Region_Name(RegionUUID)

                    RegionName = RegionName.Replace(" ", "_")

                    If Disallow_Foreigners(RegionUUID) = "True" Then
                        Authorizationlist += $"Region_{RegionName}=DisallowForeigners{vbLf}"
                    ElseIf Disallow_Residents(RegionUUID) = "True" Then
                        Authorizationlist += $"Region_{RegionName}=DisallowResidents{vbLf}"
                    End If
                Next

                Using outputFile As New StreamWriter(d, False)
                    outputFile.AutoFlush = True
                    reader = System.IO.File.OpenText(s)
                    'now loop through each line
                    While reader.Peek <> -1
                        line = reader.ReadLine()
                        Dim Output As String = Nothing
                        'Breakpoint.Print(line)
                        If line.StartsWith("; START", StringComparison.OrdinalIgnoreCase) Then
                            Output += line & vbLf ' add back on the ; START
                            Output += Authorizationlist
                        Else
                            Output += line
                        End If
                        outputFile.WriteLine(Output)
                    End While
                    outputFile.Flush()
                End Using
                'close your reader
                reader.Close()
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try

            Dim INI = New LoadIni(d, ";", System.Text.Encoding.UTF8)

            INI.SetIni("HGInventoryAccessModule", "OutboundPermission", CStr(Settings.OutboundPermissions))
            INI.SetIni("DatabaseService", "ConnectionString", Settings.RegionDBConnection)

            ' ;; Send visual reminder to local users that their inventories are unavailable while they are traveling ;; and available when they return. True by default.
            If Settings.Suitcase Then
                INI.SetIni("HGInventoryAccessModule", "RestrictInventoryAccessAbroad", "True")
            Else
                INI.SetIni("HGInventoryAccessModule", "RestrictInventoryAccessAbroad", "False")
            End If

            INI.SaveIni()
            Return False
        Catch
            Return True
        End Try

    End Function

    Public Function DoGridHyperGrid() As Boolean

        Try
            TextPrint("->Set GridHypergrid.ini")

            Dim src = IO.Path.Combine(Settings.OpensimBinPath & "config-include\Proto")
            src = IO.Path.Combine(src, Settings.ServerType)
            src = IO.Path.Combine(src, "GridHypergrid.ini")

            Dim dest = IO.Path.Combine(Settings.OpensimBinPath, "config-include\GridHypergrid.ini")
            'Put that gridhypergrid.ini file in place
            CopyFileFast(src, dest)
            Return False
        Catch
            Return True
        End Try

    End Function

    Public Function DoIceCast() As Boolean

        If Not Settings.SCEnable Then Return False
        Try

            TextPrint("->Set IceCast")
            Dim rgx As New Regex("[^a-zA-Z0-9 ]")
            Dim name As String = rgx.Replace(Settings.SimName, "")

            Dim icecast As String = "<icecast>" & vbCrLf +
                           "<hostname>" & Settings.PublicIP & "</hostname>" & vbCrLf +
                            "<location>" & name & "</location>" & vbCrLf +
                            "<admin>" & Settings.AdminEmail & "</admin>" & vbCrLf +
                            "<shoutcast-mount>/stream</shoutcast-mount>" & vbCrLf +
                            "<listen-socket>" & vbCrLf +
                            "    <port>" & CStr(Settings.SCPortBase) & "</port>" & vbCrLf +
                            "</listen-socket>" & vbCrLf +
                            "<listen-socket>" & vbCrLf +
                            "   <port>" & CStr(Settings.SCPortBase1) & "</port>" & vbCrLf +
                            "   <shoutcast-compat>1</shoutcast-compat>" & vbCrLf +
                            "</listen-socket>" & vbCrLf +
                             "<limits>" & vbCrLf +
                              "   <clients>20</clients>" & vbCrLf +
                              "    <sources>4</sources>" & vbCrLf +
                              "    <queue-size>524288</queue-size>" & vbCrLf +
                              "     <client-timeout>30</client-timeout>" & vbCrLf +
                              "    <header-timeout>15</header-timeout>" & vbCrLf +
                              "    <source-timeout>10</source-timeout>" & vbCrLf +
                              "    <burst-On-connect>1</burst-On-connect>" & vbCrLf +
                              "    <burst-size>65535</burst-size>" & vbCrLf +
                              "</limits>" & vbCrLf +
                              "<authentication>" & vbCrLf +
                                  "<source-password>" & Settings.SCPassword & "</source-password>" & vbCrLf +
                                  "<relay-password>" & Settings.SCPassword & "</relay-password>" & vbCrLf +
                                  "<admin-user>admin</admin-user>" & vbCrLf +
                                  "<admin-password>" & Settings.SCAdminPassword & "</admin-password>" & vbCrLf +
                              "</authentication>" & vbCrLf +
                              "<http-headers>" & vbCrLf +
                              "    <header name=" & """" & "Access-Control-Allow-Origin" & """" & " value=" & """" & "*" & """" & "/>" & vbCrLf +
                              "</http-headers>" & vbCrLf +
                              "<fileserve>1</fileserve>" & vbCrLf +
                              "<paths>" & vbCrLf +
                                  "<logdir>./log</logdir>" & vbCrLf +
                                  "<webroot>./web</webroot>" & vbCrLf +
                                  "<adminroot>./admin</adminroot>" & vbCrLf &  '
                                   "<alias source=" & """" & "/" & """" & " destination=" & """" & "/status.xsl" & """" & "/>" & vbCrLf +
                              "</paths>" & vbCrLf +
                              "<logging>" & vbCrLf +
                                  "<accesslog>access.log</accesslog>" & vbCrLf +
                                  "<errorlog>Error.log</errorlog>" & vbCrLf +
                                  "<loglevel>3</loglevel>" & vbCrLf +
                                  "<logsize>10000</logsize>" & vbCrLf +
                              "</logging>" & vbCrLf +
                          "</icecast>" & vbCrLf

            Using outputFile As New IO.StreamWriter(IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Icecast\icecast_run.xml"), False)
                outputFile.WriteLine(icecast)
                outputFile.Flush()
            End Using

            Return False
        Catch
            Return True
        End Try

    End Function

    Public Function DoPerlDBSetup() As Boolean

        Try
            Dim perltext = $"DSN={Settings.RobustDatabaseName}:localhost:{Settings.MySqlRobustDBPort};UID={Settings.RobustUserName};PWD={Settings.RobustPassword};"
            Using outputFile As New StreamWriter(IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Apache\MySQL.txt"), False)
                outputFile.WriteLine(perltext)
                outputFile.Flush()
            End Using
        Catch ex As Exception
            Return True
        End Try

        Return False

    End Function

    Public Function DoPHPDBSetup() As Boolean

        Try

            TextPrint("->Set PHP7")
            Dim phptext = "<?php " & vbCrLf &
    "/* General Domain */" & vbCrLf &
    "$CONF_domain        = " & """" & Settings.PublicIP & """" & "; " & vbCrLf &
    "$CONF_port          = " & """" & Settings.HttpPort & """" & "; " & vbCrLf &
    "$CONF_sim_domain    = " & """" & "http://" & Settings.PublicIP & "/" & """" & ";" & vbCrLf &
    "$CONF_install_path  = " & """" & "/Metromap" & """" & ";   // Installation path " & vbCrLf & "/* MySQL Database */ " & vbCrLf &
    "$CONF_db_server     = " & """" & Settings.RobustServerIP & """" & "; // Address Of Robust Server " & vbCrLf &
    "$CONF_db_port       = " & """" & CStr(Settings.MySqlRobustDBPort) & """" & "; // Robust port " & vbCrLf &
    "$CONF_db_user       = " & """" & Settings.RobustUserName & """" & ";  // login " & vbCrLf &
    "$CONF_db_pass       = " & """" & Settings.RobustPassword & """" & ";  // password " & vbCrLf &
    "$CONF_db_database   = " & """" & Settings.RobustDatabaseName & """" & ";     // Name Of Robust Server " & vbCrLf &
    "/* The Coordinates Of the Grid-Center */ " & vbCrLf &
    "$CONF_center_coord_x = " & """" & CStr(Settings.MapCenterX) & """" & ";		// the Center-X-Coordinate " & vbCrLf &
    "$CONF_center_coord_y = " & """" & CStr(Settings.MapCenterY) & """" & ";		// the Center-Y-Coordinate " & vbCrLf &
    "// style-sheet items" & vbCrLf &
    "$CONF_style_sheet     = " & """" & "/css/stylesheet.css" & """" & ";          //Link To your StyleSheet" & vbCrLf &
    "$CONF_HOME            = " & """/" & Settings.CMS & "/""" & ";          //Link To your Home Folder in htdocs.  WordPress, DreamGrid, JOpensim/jOpensim or user assigned folder" & vbCrLf &
    "?>"

            Try
                Using outputFile As New StreamWriter(IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Apache\htdocs\MetroMap\includes\config.php"), False)
                    outputFile.WriteLine(phptext)
                    outputFile.Flush()
                End Using
            Catch ex As Exception
                MsgBox(ex.Message, vbCritical Or MsgBoxStyle.MsgBoxSetForeground)
            End Try

            Try
                phptext = "<?php " & vbCrLf &
    "$DB_GRIDNAME = " & """" & Settings.PublicIP & ":" & Settings.HttpPort & """" & ";" & vbCrLf &
    "$DB_HOST = " & """" & Settings.RobustServerIP & """" & ";" & vbCrLf &
    "$DB_PORT = " & """" & CStr(Settings.MySqlRobustDBPort) & """" & "; // Robust port " & vbCrLf &
    "$DB_USER = " & """" & Settings.RobustUserName & """" & ";" & vbCrLf &
    "$DB_PASSWORD = " & """" & Settings.RobustPassword & """" & ";" & vbCrLf &
    "$DB_NAME = " & """" & "ossearch" & """" & ";" & vbCrLf &
    "$ROBUST_NAME = " & """" & Settings.RobustDatabaseName & """" & ";" & vbCrLf &
    "$OPENSIM_NAME = " & """" & Settings.RegionDBName & """" & ";" & vbCrLf &
    "?>"

                Using outputFile As New StreamWriter(IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\PHP7\databaseinfo.php"), False)
                    outputFile.WriteLine(phptext)
                    outputFile.Flush()
                End Using
            Catch ex As Exception
                MsgBox(ex.Message, vbCritical Or MsgBoxStyle.MsgBoxSetForeground)
            End Try

            Return False
        Catch
            Return True
        End Try

    End Function

    Public Function DoTos() As Boolean

        If Not IO.File.Exists(IO.Path.Combine(Settings.CurrentDirectory, "tos.html")) Then
            CopyFileFast(IO.Path.Combine(Settings.CurrentDirectory, "tos.proto"), IO.Path.Combine(Settings.CurrentDirectory, "tos.html"))
        End If

        CopyFileFast(IO.Path.Combine(Settings.CurrentDirectory, "tos.html"), IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Opensim\bin\WifiPages\tos.html"))

        Return False

    End Function

    Public Function DoWhoGotWhat() As Boolean

        Try
            Dim INI = New LoadIni(Settings.OpensimBinPath & "config-addon-opensim\WhoGotWhat.ini", ";", System.Text.Encoding.UTF8)
            INI.SetIni("WhoGotWhat", "MachineID", Settings.MachineId)
            INI.SaveIni()
            Return False
        Catch
            Return True
        End Try

    End Function

    Public Function DoWifi() As Boolean

        ' There are two Wifi's so search will work
        Try
            Dim INI = New LoadIni(Settings.OpensimBinPath & "config-addon-opensim\Wifi.ini", ";", System.Text.Encoding.UTF8)
            TextPrint("->Set Diva Wifi page")
            INI.SetIni("DatabaseService", "ConnectionString", Settings.RobustDBConnection)
            INI.SaveIni()

            INI = New LoadIni(IO.Path.Combine(Settings.OpensimBinPath, "Wifi.ini"), ";", System.Text.Encoding.UTF8)

            INI.SetIni("DatabaseService", "ConnectionString", Settings.RobustDBConnection)

            If Settings.ServerType = RobustServerName Then ' wifi could be on or off
                If (Settings.WifiEnabled) Then
                    INI.SetIni("WifiService", "Enabled", "True")
                Else
                    INI.SetIni("WifiService", "Enabled", "False")
                End If
            Else ' it is always off
                ' shutdown wifi in Attached mode
                INI.SetIni("WifiService", "Enabled", "False")
            End If

            INI.SetIni("WifiService", "GridName", Settings.SimName)
            INI.SetIni("WifiService", "LoginURL", "http://" & Settings.PublicIP & ":" & Settings.HttpPort)
            INI.SetIni("WifiService", "WebAddress", "http://" & Settings.PublicIP & ":" & Settings.HttpPort)

            ' Wifi Admin'
            INI.SetIni("WifiService", "AdminFirst", Settings.AdminFirst)    ' Wifi
            INI.SetIni("WifiService", "AdminLast", Settings.AdminLast)      ' Admin
            INI.SetIni("WifiService", "AdminPassword", Settings.Password)   ' secret
            INI.SetIni("WifiService", "AdminEmail", Settings.AdminEmail)    ' send notifications to this person

            'Gmail and other SMTP mailers
            ' Gmail requires you set to set low security access
            INI.SetIni("WifiService", "SmtpHost", Settings.SmtpHost)
            INI.SetIni("WifiService", "SmtpPort", Convert.ToString(Settings.SmtpPort, Globalization.CultureInfo.InvariantCulture))
            INI.SetIni("WifiService", "SmtpUsername", Settings.SmtPropUserName)
            INI.SetIni("WifiService", "SmtpPassword", Settings.SmtpPassword)

            INI.SetIni("WifiService", "HomeLocation", Settings.WelcomeRegion & "/" & Settings.HomeVectorX & "/" & Settings.HomeVectorY & "/" & Settings.HomeVectorZ)

            If Settings.AccountConfirmationRequired Then
                INI.SetIni("WifiService", "AccountConfirmationRequired", "True")
            Else
                INI.SetIni("WifiService", "AccountConfirmationRequired", "False")
            End If

            INI.SaveIni()
            Return False
        Catch
            Return True
        End Try

    End Function

    Public Function GetOpensimProto() As String
        ''' <summary>Loads the INI file for the proper grid type for parsing</summary>
        ''' <returns>Returns the path to the proper Opensim.ini prototype.</returns>

        Dim Path = IO.Path.Combine(Settings.OpensimBinPath, "config-include\Proto")
        Path = IO.Path.Combine(Path, Settings.ServerType)
        Path = IO.Path.Combine(Path, "Opensim.ini")
        Return Path

    End Function

    ''' <summary>Set up all INI files</summary>
    ''' <returns>true if it fails</returns>
    Public Function SetIniData() As Boolean

        If SkipSetup Then Return False
        SkipSetup = True

        TextPrint(My.Resources.Creating_INI_Files_word)

        If DoRobust() Then Return True  ' Robust
        If DoGrid() Then Return True ' Grid.ini
        If DoGridCommon() Then Return True ' GridCommon.ini
        If DoGridHyperGrid() Then Return True ' GridHypergrid.ini
        If DoTos() Then Return True         ' term of service
        If DoFlotsamINI() Then Return True  ' cache
        If DoWifi() Then Return True        ' Diva Wifi
        If DoCurrency() Then Return True    ' Gloebit
        If DoWhoGotWhat() Then Return True  ' add on for scripts to save events to a CSV file
        If DoTides() Then Return True       ' tides
        If DoBirds() Then Return True       ' birds
        If DoPHPDBSetup() Then Return True  ' PHP Database
        If DoPHP() Then Return True         ' PHP.ini
        If DoPerlDBSetup() Then Return True  ' Perl
        If DoApache() Then Return True      ' Apache.conf
        If DoIceCast() Then Return True     ' Icecast

        Return False

    End Function

    Public Function Stripqq(input As String) As String

        ' remove double quotes and any comments ";"
        Return Replace(input, """", "")

    End Function

    Private Function DoFlotsamINI() As Boolean

        Dim INI = New LoadIni(Settings.OpensimBinPath & "config-include\FlotsamCache.ini", ";", System.Text.Encoding.UTF8)
        Try
            TextPrint("->Set Flotsam Cache")
            INI.SetIni("AssetCache", "LogLevel", Settings.CacheLogLevel)
            INI.SetIni("AssetCache", "CacheDirectory", Settings.CacheFolder)
            INI.SetIni("AssetCache", "FileCacheEnabled", CStr(Settings.CacheEnabled))
            INI.SetIni("AssetCache", "FileCacheTimeout", Settings.CacheTimeout)
            INI.SaveIni()
            Return False
        Catch
            Return True
        End Try

    End Function

    Private Function DoPHP() As Boolean

        Dim ini = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\PHP7\php.ini")

        Try
            Dim PHP = New IniWriter(ini)
            PHP.Write("extension_dir", "extension_dir = " & """" & Settings.CurrentSlashDir & "/OutworldzFiles/PHP7/ext/""")
            PHP.Write("doc_root", "doc_root = """ & Settings.CurrentSlashDir & "/OutworldzFiles/Apache/htdocs/""")
            PHP.Save("php.ini")
        Catch
            Return True
        End Try

        Return False

    End Function

    Private Function DoTides() As Boolean

        Try
            Dim TideData As String = ""
            Dim TideFile = Settings.OpensimBinPath & "addon-modules\OpenSimTide\config\OpenSimTide.ini"

            DeleteFile(TideFile)

            For Each RegionUUID In RegionUuids()
                Dim RegionName = Region_Name(RegionUUID)
                'Tides Setup per region
                If Settings.TideEnabled And Tides(RegionUUID) = "True" Then

                    TideData = TideData & ";; Set the Tide settings per named region" & vbCrLf &
                        "[" & RegionName & "]" & vbCrLf &
                    ";this determines whether the Module does anything In this region" & vbCrLf &
                    ";# {TideEnabled} {} {Enable the tide To come In And out?} {True False} False" & vbCrLf &
                    "TideEnabled = True" & vbCrLf &
                        vbCrLf &
                    ";; Tides currently only work On Single regions And varregions (non megaregions) " & vbCrLf &
                    ";# surrounded completely by water" & vbCrLf &
                    ";; Anything Else will produce weird results where you may see a big" & vbCrLf &
                    ";; vertical 'step' in the ocean" & vbCrLf &
                    ";; update the tide every x simulator frames" & vbCrLf &
                    "TideUpdateRate = 50" & vbCrLf &
                        vbCrLf &
                    ";; low And high water marks in metres" & vbCrLf &
                    "TideHighWater = " & Convert.ToString(Settings.TideHighLevel(), Globalization.CultureInfo.InvariantCulture) & vbCrLf &
                    "TideLowWater = " & Convert.ToString(Settings.TideLowLevel(), Globalization.CultureInfo.InvariantCulture) & vbCrLf &
                    vbCrLf &
                    ";; how long in seconds for a complete cycle time low->high->low" & vbCrLf &
                    "TideCycleTime = " & Convert.ToString(Settings.CycleTime(), Globalization.CultureInfo.InvariantCulture) & vbCrLf &
                        vbCrLf &
                    ";; provide tide information on the console?" & vbCrLf &
                    "TideInfoDebug = " & CStr(Settings.TideInfoDebug) & vbCrLf &
                        vbCrLf &
                    ";; chat tide info to the whole region?" & vbCrLf &
                    "TideInfoBroadcast = " & Settings.BroadcastTideInfo() & vbCrLf &
                        vbCrLf &
                    ";; which channel to region chat on for the full tide info" & vbCrLf &
                    "TideInfoChannel = " & CStr(Settings.TideInfoChannel) & vbCrLf &
                    vbCrLf &
                    ";; which channel to region chat on for just the tide level in metres" & vbCrLf &
                    "TideLevelChannel = " & Convert.ToString(Settings.TideLevelChannel(), Globalization.CultureInfo.InvariantCulture) & vbCrLf &
                        vbCrLf &
                    ";; How many times to repeat Tide Warning messages at high/low tide" & vbCrLf &
                    "TideAnnounceCount = 1" & vbCrLf & vbCrLf & vbCrLf & vbCrLf
                End If

            Next
            IO.File.WriteAllText(TideFile, TideData, System.Text.Encoding.Default) 'The text file will be created if it does not already exist

            Return False
        Catch
            Return True
        End Try

    End Function

End Module
