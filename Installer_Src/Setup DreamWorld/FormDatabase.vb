#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.Text.RegularExpressions

Public Class FormDatabase

#Region "Private Fields"

    Dim changed As Boolean
    Dim DNSName As String = ""
    Dim initted As Boolean

#End Region

#Region "Properties"

    Public Property Changed1 As Boolean
        Get
            Return changed
        End Get
        Set(value As Boolean)
            changed = value
        End Set
    End Property

    Public Property DnsName1 As String
        Get
            Return DNSName
        End Get
        Set(value As String)
            DNSName = value
        End Set
    End Property

    Public Property Initted1 As Boolean
        Get
            Return initted
        End Get
        Set(value As Boolean)
            initted = value
        End Set
    End Property

#End Region

#Region "ScreenSize"

    Private ReadOnly Handler As New EventHandler(AddressOf Resize_page)

    'The following detects  the location of the form in screen coordinates
    Private _screenPosition As ClassScreenpos

    Public Property ScreenPosition As ClassScreenpos
        Get
            Return _screenPosition
        End Get
        Set(value As ClassScreenpos)
            _screenPosition = value
        End Set
    End Property

    Private Sub Resize_page(ByVal sender As Object, ByVal e As System.EventArgs)
        'Me.Text = "Form screen position = " + Me.Location.ToString
        ScreenPosition.SaveXY(Me.Left, Me.Top)
    End Sub

    Private Sub SetScreen()

        ScreenPosition = New ClassScreenpos(Me.Name)
        AddHandler ResizeEnd, Handler
        Dim xy As List(Of Integer) = ScreenPosition.GetXY()
        Me.Left = xy.Item(0)
        Me.Top = xy.Item(1)
    End Sub

#End Region

#Region "Load/Exit"

    Private Sub Form_exit() Handles Me.Closed
        If Changed1 Then
            SaveAll()
        End If
    End Sub

    Private Sub Loaded(sender As Object, e As EventArgs) Handles Me.Load

        ' Robust DB
        ConnectToMySqlToolStripMenuItem.Text = Global.Outworldz.My.Resources.Connect2Console
        Dbnameindex.Text = Global.Outworldz.My.Resources.DBName_word
        GridGroup.Text = Global.Outworldz.My.Resources.Robust_word
        HelpMenu.Image = Global.Outworldz.My.Resources.question_and_answer
        HelpMenu.Text = Global.Outworldz.My.Resources.Help_word
        Label1.Text = Global.Outworldz.My.Resources.Region_Server_word
        Label15.Text = Global.Outworldz.My.Resources.User_Name_word
        Label16.Text = Global.Outworldz.My.Resources.Robust_word
        Label2.Text = Global.Outworldz.My.Resources.MySqlPort_word
        Label20.Text = Global.Outworldz.My.Resources.Region_Database
        Label21.Text = Global.Outworldz.My.Resources.User_Name_word
        Label22.Text = Global.Outworldz.My.Resources.Password_word
        Label8.Text = Global.Outworldz.My.Resources.MySqlPort_word
        Label9.Text = Global.Outworldz.My.Resources.Password_word
        MenuStrip2.Text = Global.Outworldz.My.Resources._0
        MysqlRegionPort.Text = CStr(Settings.MySqlRegionDBPort)
        RegionDbName.Text = Settings.RegionDBName
        RegionDBUsername.Text = Settings.RegionDBUserName
        RegionMySqlPassword.Text = Settings.RegionDbPassword
        RegionServer.Text = Settings.RegionServer
        RobustDbName.Text = Settings.RobustDatabaseName
        RobustDBPassword.Text = Settings.RobustPassword
        RobustDBPassword.UseSystemPasswordChar = True
        RobustDbPort.Text = Settings.MySqlRobustDBPort.ToString(Globalization.CultureInfo.InvariantCulture)
        RobustDBUsername.Text = Settings.RobustUserName
        RobustServer.Text = Settings.RobustServerIP
        RootPassword.Text = Settings.RootMysqlPassword
        RunasaServiceCheckBox.Text = My.Resources.RunasaService_word
        StandaloneGroup.Text = Global.Outworldz.My.Resources.Region_Database
        Text = Global.Outworldz.My.Resources.Database_word
        ToolTip1.SetToolTip(RegionDbName, Global.Outworldz.My.Resources.Do_NotChange)
        ToolTip1.SetToolTip(RegionDBUsername, Global.Outworldz.My.Resources.Do_NotChange)
        ToolTip1.SetToolTip(RegionMySqlPassword, Global.Outworldz.My.Resources.Do_NotChange)
        ToolTip1.SetToolTip(RegionServer, Global.Outworldz.My.Resources.Region_ServerName)
        ToolTip1.SetToolTip(RobustDbName, Global.Outworldz.My.Resources.Do_NotChange)
        ToolTip1.SetToolTip(RobustDBPassword, Global.Outworldz.My.Resources.Do_NotChange)
        ToolTip1.SetToolTip(RobustDbPort, Global.Outworldz.My.Resources.MySQL_Port_Default)
        ToolTip1.SetToolTip(RobustDBUsername, Global.Outworldz.My.Resources.Do_NotChange)
        ToolTip1.SetToolTip(RobustServer, Global.Outworldz.My.Resources.Region_ServerName)

        RunasaServiceCheckBox.Checked = Settings.MysqlRunasaService

        SetScreen()

        Initted1 = True
        HelpOnce("Database")
        HelpOnce("ServerType")

    End Sub

    Private Sub SaveAll()

        Settings.SaveSettings()
        Changed1 = False ' do not trigger the save a second time

    End Sub

#End Region

#Region "Database"

    Private Sub Database_Click(sender As Object, e As EventArgs)

        HelpManual("Database")

    End Sub

    Private Sub DatabaseNameUser_TextChanged(sender As Object, e As EventArgs) Handles RegionDbName.TextChanged

        If Not Initted1 Then Return
        If Settings.RobustDatabaseName <> RegionDbName.Text Then
            Dim result1 = MsgBox("Renaming a database will connect to this new name when DreamGrid starts. Are you sure? ", MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground)
            If result1 = vbNo Then
                RegionDbName.Text = Settings.RegionDBName
                Return
            End If
        End If

        Settings.RegionDBName = RegionDbName.Text
        Settings.SaveSettings()

    End Sub

    Private Sub DbPassword_Clicked(sender As Object, e As EventArgs) Handles RegionMySqlPassword.Click

        RegionMySqlPassword.UseSystemPasswordChar = False

    End Sub

    Private Sub DbPassword_TextChanged(sender As Object, e As EventArgs) Handles RegionMySqlPassword.LostFocus

        If Not Initted1 Then Return

        If Settings.RegionDbPassword <> RegionMySqlPassword.Text Then
            Dim Q = UpdateMysqlPassword(RegionMySqlPassword.Text, Settings.RegionDBUserName)
            If Q.Length = 0 Then
                Settings.RegionDbPassword = RegionMySqlPassword.Text
                Settings.SaveSettings()
            Else
                MsgBox($"{My.Resources.Failpassword} : {Q} ", MsgBoxStyle.Critical Or MsgBoxStyle.MsgBoxSetForeground, My.Resources.Error_word)
            End If

        End If

    End Sub

    Private Sub DbUsername_TextChanged(sender As Object, e As EventArgs) Handles RegionDBUsername.TextChanged

        If Not Initted1 Then Return
        Settings.RegionDBUserName = RegionDBUsername.Text
        Settings.SaveSettings()

    End Sub

    Private Sub RobustDbPort_TextChanged(sender As Object, e As EventArgs) Handles RobustDbPort.TextChanged

        If Not Initted1 Then Return
        Dim digitsOnly = New Regex("[^\d]")
        RobustDbPort.Text = digitsOnly.Replace(RobustDbPort.Text, "")
        Settings.MySqlRobustDBPort = CInt("0" & RobustDbPort.Text)
        Settings.SaveSettings()

    End Sub

    Private Sub RobustDbPortTextbox_click(sender As Object, e As EventArgs) Handles RobustDBPassword.Click

        RobustDBPassword.UseSystemPasswordChar = False

    End Sub

    Private Sub RobustPasswordTextBox_TextChanged(sender As Object, e As EventArgs) Handles RobustDBPassword.LostFocus

        If Not Initted1 Then Return

        If Settings.RobustPassword <> RobustDBPassword.Text Then
            Dim Q = UpdateMysqlPassword(RobustDBPassword.Text, Settings.RobustUserName)
            If Q.Length = 0 Then
                Settings.RobustPassword = RobustDBPassword.Text
                Settings.SaveSettings()
            Else
                MsgBox($"{My.Resources.Failpassword} : Q", MsgBoxStyle.Critical Or MsgBoxStyle.MsgBoxSetForeground, My.Resources.Error_word)
            End If
        End If

    End Sub

    Private Sub RobustServer_TextChanged(sender As Object, e As EventArgs) Handles RobustServer.TextChanged

        If Not Initted1 Then Return
        Settings.RobustServerIP = RobustServer.Text
        Settings.SaveSettings()

    End Sub

    Private Sub RobustUsernameTextBox_TextChanged(sender As Object, e As EventArgs) Handles RobustDBUsername.TextChanged

        If Not Initted1 Then Return
        Settings.RobustUserName = RobustDBUsername.Text
        Settings.SaveSettings()

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles RegionServer.TextChanged

        If Not Initted1 Then Return
        Settings.RegionServer = RegionServer.Text
        Settings.SaveSettings()

    End Sub

    Private Sub TextBox1_TextChanged_1(sender As Object, e As EventArgs) Handles RobustDbName.TextChanged
        If Not Initted1 Then Return
        If Settings.RobustDatabaseName <> RobustDbName.Text Then
            Dim result1 = MsgBox("Renaming a database connect to this new name when DreamGrid starts. Are you sure? ", MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground)
            If result1 = vbNo Then
                RobustDbName.Text = Settings.RobustDatabaseName
                Return
            End If
        End If

        Settings.RobustDatabaseName = RobustDbName.Text
        Settings.SaveSettings()

    End Sub

    Private Sub TextBox1_TextChanged_2(sender As Object, e As EventArgs) Handles MysqlRegionPort.TextChanged

        If Not Initted1 Then Return
        Dim digitsOnly = New Regex("[^\d]")
        MysqlRegionPort.Text = digitsOnly.Replace(MysqlRegionPort.Text, "")

        Settings.MySqlRegionDBPort = CInt("0" & MysqlRegionPort.Text)
        Settings.SaveSettings()

    End Sub

#End Region

#Region "Grid type"

    Private Sub ConsoleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConsoleToolStripMenuItem.Click

        MysqlConsole()

    End Sub

    Private Sub HelpMenu_Click(sender As Object, e As EventArgs) Handles HelpMenu.Click
        HelpManual("Database")
    End Sub

    Private Sub RootPassword_click(sender As Object, e As EventArgs) Handles RootPassword.Click

        RootPassword.UseSystemPasswordChar = False

    End Sub

    Private Sub RootPassword_TextChanged_3(sender As Object, e As EventArgs) Handles RootPassword.LostFocus

        If Not Initted1 Then Return

        If Settings.RootMysqlPassword <> RootPassword.Text Then
            Dim Q = UpdateMysqlPassword(RootPassword.Text, "root")
            If Q.Length = 0 Then
                Settings.RootMysqlPassword = RootPassword.Text
                Settings.SaveSettings()
            Else
                MsgBox($"{My.Resources.Failpassword} : {Q}", MsgBoxStyle.Critical Or MsgBoxStyle.MsgBoxSetForeground, My.Resources.Error_word)
            End If
        End If

    End Sub

    Private Sub RunasaServiceCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles RunasaServiceCheckBox.CheckedChanged

        If Not initted Then Return
        If RunasaServiceCheckBox.Checked Then
            Dim result1 = MsgBox("Is this PC in a data center with a UPS and Backup Generator?", MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground)
            If result1 = vbYes Then
                Settings.MysqlRunasaService = RunasaServiceCheckBox.Checked
                Settings.SaveSettings()
                StopMysql()
                StartMySQL()
                Return
            End If
            Dim result2 = MsgBox("Is this PC connected to a UPS with a cable to automatically shut off the PC in a power failure?", MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground)
            If result2 = vbYes Then
                Settings.MysqlRunasaService = RunasaServiceCheckBox.Checked
                Settings.SaveSettings()
                StopMysql()
                StartMySQL()
                Return
            End If
            MsgBox("You should not be running MySQL as a service", vbInformation Or MsgBoxStyle.MsgBoxSetForeground)
            Settings.MysqlRunasaService = False
            RunasaServiceCheckBox.Checked = False
        Else
            Settings.MysqlRunasaService = False
        End If

    End Sub

    Private Sub StartToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StartToolStripMenuItem.Click

        StartMySQL()

    End Sub

    Private Sub StopToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StopToolStripMenuItem.Click

        StopMysql()

    End Sub

#End Region

End Class
