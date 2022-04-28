#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Module OAR
    Private _ForceMerge As Boolean
    Private _ForceParcel As Boolean = True
    Private _ForceTerrain As Boolean = True
    Private _UserName As String = ""

    Public Property PropForceMerge As Boolean
        Get
            Return _ForceMerge
        End Get
        Set(value As Boolean)
            _ForceMerge = value
        End Set
    End Property

    Public Property PropForceParcel As Boolean
        Get
            Return _ForceParcel
        End Get
        Set(value As Boolean)
            _ForceParcel = value
        End Set
    End Property

    Public Property PropForceTerrain As Boolean
        Get
            Return _ForceTerrain
        End Get
        Set(value As Boolean)
            _ForceTerrain = value
        End Set
    End Property

    Public Property PropUserName As String
        Get
            Return _UserName
        End Get
        Set(value As String)
            _UserName = value
        End Set
    End Property

    Public Sub LoadOar(RegionName As String)

        If RegionName Is Nothing Then Return

        HelpOnce("Load OAR")

        If PropOpensimIsRunning() Then
            If RegionName.Length = 0 Then
                RegionName = ChooseRegion(False)
                If RegionName.Length = 0 Then Return
            End If

            Dim RegionUUID As String = FindRegionByName(RegionName)

            ' Create an instance of the open file dialog box. Set filter options and filter index.
            Using openFileDialog1 = New OpenFileDialog With {
                .InitialDirectory = BackupPath(),
                .Filter = Global.Outworldz.My.Resources.OAR_Load_and_Save & "(*.OAR,*.GZ,*.TGZ)|*.oar;*.gz;*.tgz;*.OAR;*.GZ;*.TGZ|All Files (*.*)|*.*",
                .FilterIndex = 1,
                .Multiselect = False
                }

                ' Call the ShowDialog method to show the dialog box.
                Dim UserClickedOK As DialogResult = openFileDialog1.ShowDialog

                ' Process input if the user clicked OK.
                If UserClickedOK = DialogResult.OK Then

                    Dim offset = VarChooser(RegionName)

                    Dim thing = openFileDialog1.FileName
                    If thing.Length > 0 Then
                        thing = thing.Replace("\", "/")    ' because Opensim uses UNIX-like slashes, that's why

                        Dim Group = Group_Name(RegionUUID)

                        Dim ForceParcel As String = ""
                        If PropForceParcel() Then ForceParcel = " --force-parcels "
                        Dim ForceTerrain As String = ""
                        If PropForceTerrain Then ForceTerrain = " --force-terrain "
                        Dim ForceMerge As String = ""
                        If PropForceMerge Then ForceMerge = " --merge "
                        Dim UserName As String = ""
                        If Not PropForceMerge Then
                            Dim m = MsgBox(My.Resources.Erase_all, vbYesNoCancel Or MsgBoxStyle.MsgBoxSetForeground Or MsgBoxStyle.Critical, Global.Outworldz.My.Resources.Caution_word)
                            If m = vbNo Or m = vbCancel Then Return
                        End If

                        If PropUserName.Length > 0 Then UserName = $" --Default-user ""{PropUserName}"" "
                        Dim v As String = $"change region ""{RegionName}""{vbCrLf}load oar {UserName} {ForceMerge} {ForceTerrain} {ForceParcel} {offset} ""{thing}"""

                        Dim obj As New TaskObject With {
                            .TaskName = FormSetup.TaskName.LoadOneOarTask,
                            .Command = v
                        }
                        FormSetup.RebootAndRunTask(RegionUUID, obj)
                    End If
                End If

            End Using
        Else
            TextPrint(My.Resources.Not_Running)
        End If

    End Sub

    Public Sub LoadOARContent(thing As String)

        If Not PropOpensimIsRunning() Then
            TextPrint(My.Resources.Not_Running)
            Return
        End If

        thing = thing.Replace("https:", "http:")
        Dim RegionName = ChooseRegion(False)
        If RegionName.Length = 0 Then Return

        Dim offset = VarChooser(RegionName)
        If offset.Length = 0 Then Return

        ' Must be passed as a string in queue
        Dim backMeUp = MsgBox(My.Resources.Make_a_backup_word & " (" & RegionName & ")", vbYesNoCancel Or MsgBoxStyle.MsgBoxSetForeground Or MsgBoxStyle.Question, Global.Outworldz.My.Resources.Backup_word)
        Dim BackUpString As String = ""
        Select Case backMeUp
            Case vbCancel
                Return
            Case vbYes
                BackUpString = "Yes"
            Case vbNo
                BackUpString = "No"
        End Select

        Dim RegionUUID As String = FindRegionByName(RegionName)
        If RegionUUID.Length = 0 Then
            ErrorLog(My.Resources.Cannot_find_region_word)
        End If

        TextPrint(My.Resources.Opensimulator_is_loading & " " & thing)
        If thing IsNot Nothing Then thing = thing.Replace("\", "/")    ' because Opensim uses UNIX-like slashes, that's why

        Dim ForceParcel As String = ""
        If PropForceParcel() Then ForceParcel = " --force-parcels "
        Dim ForceTerrain As String = ""
        If PropForceTerrain Then ForceTerrain = " --force-terrain "
        Dim ForceMerge As String = ""

        Dim UserName As String = ""
        If PropUserName.Length > 0 Then UserName = " --default-user " & """" & PropUserName & """" & " "

        If PropForceMerge Then ForceMerge = " --merge "
        If Not PropForceMerge Then
            Dim m = MsgBox(My.Resources.Erase_all, vbYesNoCancel Or MsgBoxStyle.MsgBoxSetForeground Or MsgBoxStyle.Critical, Global.Outworldz.My.Resources.Caution_word)
            If m = vbNo Or m = vbCancel Then Return
        End If

        Dim LoadOarCmd = $"change region ""{RegionName}""{vbCrLf} load oar {UserName} {ForceMerge} {ForceTerrain} {ForceParcel} {offset} ""{thing}"""

        Dim obj As New TaskObject With {
            .TaskName = FormSetup.TaskName.LoadOARContent,
            .backMeUp = BackUpString,
            .Command = LoadOarCmd
        }

        FormSetup.RebootAndRunTask(RegionUUID, obj)

    End Sub

    ''' <summary>
    ''' Backs up a region by name after booting
    ''' </summary>

    Public Sub LoadOARContent2(RegionUUID As String, T As TaskObject)

        Dim RegionName = Region_Name(RegionUUID)
        Dim backMeUp = T.backMeUp
        Dim LoadOarStr = T.Command

        Try
            If backMeUp = "Yes" Then
                ConsoleCommand(RegionUUID, $"change region ""{RegionName}""", True)
                SendMessage(RegionUUID, Global.Outworldz.My.Resources.CPU_Intensive)
                ConsoleCommand(RegionUUID, $"save oar ""{BackupPath()}/{RegionName}_{DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss", Globalization.CultureInfo.InvariantCulture)}.oar""")
                SendMessage(RegionUUID, Global.Outworldz.My.Resources.New_Content)
            End If

            SendMessage(RegionUUID, Global.Outworldz.My.Resources.New_Content)
            ConsoleCommand(RegionUUID, $"change region ""{RegionName}""", True)
            ConsoleCommand(RegionUUID, LoadOarStr)
        Catch ex As Exception
            BreakPoint.Dump(ex)
            ErrorLog(My.Resources.Error_word & ":" & ex.Message)
        End Try

    End Sub

    ''' <summary>
    '''
    ''' </summary>

    Public Sub LoadOneOarTask(RegionUUID As String, T As TaskObject)

        SendMessage(RegionUUID, Global.Outworldz.My.Resources.New_Content)
        If Not PropForceParcel() Then
            ConsoleCommand(RegionUUID, "land clear", True)
        End If
        ConsoleCommand(RegionUUID, T.Command)

    End Sub

    Public Sub SaveOar(RegionName As String)

        If RegionName Is Nothing Then Return
        If Not PropOpensimIsRunning() Then
            TextPrint(My.Resources.Not_Running)
            Return
        End If

        If RegionName.Length = 0 Then
            RegionName = ChooseRegion(False)
            If RegionName.Length = 0 Then
                TextPrint(My.Resources.Cancelled_word)
                Return
            End If
        End If

        Dim RegionUUID As String = FindRegionByName(RegionName)

        Dim Message, title, defaultValue As String
        Dim myValue As String
        ' Set prompt.
        Message = Global.Outworldz.My.Resources.EnterName
        title = "Backup to OAR"
        defaultValue = RegionName & "_" & DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss", Globalization.CultureInfo.InvariantCulture) & ".oar"

        ' Display message, title, and default value.
        myValue = InputBox(Message, title, defaultValue)
        ' If user has clicked Cancel, set myValue to defaultValue
        If myValue.Length = 0 Then Return

        If myValue.EndsWith(".OAR", StringComparison.OrdinalIgnoreCase) Or myValue.EndsWith(".oar", StringComparison.OrdinalIgnoreCase) Then
            ' nothing
        Else
            myValue += ".oar"
        End If

        Dim obj As New TaskObject With {
                        .TaskName = FormSetup.TaskName.SaveOneOAR,
                        .Command = myValue
                    }
        FormSetup.RebootAndRunTask(RegionUUID, obj)

    End Sub

    Public Sub SaveOneOar(RegionUUID As String, Task As TaskObject)

        Dim RegionName = Region_Name(RegionUUID)
        Dim MyValue = Task.Command

        If IsBooted(RegionUUID) Then
            Dim Group = Group_Name(RegionUUID)
            SendMessage(RegionUUID, "CPU Intensive Backup Started")
            ConsoleCommand(RegionUUID, "change region " & """" & RegionName & """", True)
            ConsoleCommand(RegionUUID, "save oar " & """" & BackupPath() & "/" & MyValue & """")
        End If

        TextPrint(My.Resources.Saving_word & " " & BackupPath() & "/" & MyValue)

    End Sub

End Module
