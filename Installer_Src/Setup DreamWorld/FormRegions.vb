#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Option Explicit On

Imports System.Text.RegularExpressions

Public Class FormRegions

    Private _initialized As Boolean
    Private _StopLoading As String = "Stopped"

#Region "Forms"

#Disable Warning CA2213
    Private BlockifyForm As New FormBlockify
    Private RegionForm As New FormRegion
#Enable Warning CA2213

#End Region

#Region "ScreenSize"

    Private ReadOnly Handler As New EventHandler(AddressOf Resize_page)
    Private _screenPosition As ClassScreenpos

    Public Property ScreenPosition As ClassScreenpos
        Get
            Return _screenPosition
        End Get
        Set(value As ClassScreenpos)
            _screenPosition = value
        End Set
    End Property

    'The following detects  the location of the form in screen coordinates
    Private Sub Resize_page(ByVal sender As Object, ByVal e As System.EventArgs)
        'Me.Text = "Form screen position = " & Me.Location.ToString
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

#Region "Buttons"

    Private Sub AddRegion_Click(sender As Object, e As EventArgs) Handles Button_AddRegion.Click

        Try
            RegionForm.Dispose()
            RegionForm = New FormRegion
            RegionForm.Init("")
            RegionForm.Activate()
            RegionForm.Visible = True
            RegionForm.Select()
            RegionForm.BringToFront()
        Catch
        End Try

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button_Normalize.Click

        If PropOpensimIsRunning Then
            TextPrint("Opensim is Running!")
            Return
        End If

        Dim result As MsgBoxResult = MsgBox(My.Resources.This_Moves, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground)
        If result = vbYes Then

            Dim chosen = ChooseRegion(False) ' all regions, running or not

            ' Check for illegal stuff
            Dim RegionUUID As String = FindRegionByName(chosen)
            Dim X = Coord_X(RegionUUID)
            Dim Y = Coord_Y(RegionUUID)
            Dim Err As Boolean = False
            Dim Failed As String
            Dim DeltaX = 1000 - X
            Dim DeltaY = 1000 - Y
            For Each UUID In RegionUuids()

                If X + DeltaX <= 0 Then
                    Err = True
                    Failed = Region_Name(UUID)
                End If
                If Y + DeltaY <= 0 Then
                    Err = True
                    Failed = Region_Name(UUID)
                End If
            Next

            If (Err) Then
                MsgBox(My.Resources.Cannot_Normalize)
                Return
            End If

            For Each UUID In RegionUuids()
                Coord_X(UUID) = Coord_X(UUID) + DeltaX
                Coord_Y(UUID) = Coord_Y(UUID) + DeltaY
                WriteRegionObject(Group_Name(UUID), Region_Name(UUID))
            Next
            PropChangedRegionSettings = True
        End If

    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button_Clear.Click

        StartMySQL()
        MysqlInterface.DeregisterRegions(False)

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles WelcomeBox1.SelectedIndexChanged

        Dim value As String = TryCast(WelcomeBox1.SelectedItem, String)
        Settings.WelcomeRegion = value

        Debug.Print("Selected " & value)

    End Sub

    Private Sub ConciergeCheckbox_CheckedChanged(sender As Object, e As EventArgs) Handles CheckboxConcierge.CheckedChanged

        Settings.Concierge = CheckboxConcierge.Checked

    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs)

        Dim f = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Apache\htdocs\TTS\Audio")
        System.Diagnostics.Process.Start("explorer.exe", $"/open, {f}")

    End Sub

    Private Sub RegionBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RegionBox.SelectedIndexChanged

        Dim value As String = TryCast(RegionBox.SelectedItem, String)
#Disable Warning CA2000 ' Dispose objects before losing scope
        Dim RegionForm As New FormRegion
#Enable Warning CA2000 ' Dispose objects before losing scope
        RegionForm.Init(value)
        RegionForm.Activate()
        RegionForm.Visible = True
        RegionForm.Select()
        RegionForm.BringToFront()

    End Sub

    Private Sub RegionButton1_Click(sender As Object, e As EventArgs) Handles Button_Region.Click

        Dim X As Integer = 300
        Dim Y As Integer = 200
        Dim counter As Integer = 0

        For Each RegionUUID In RegionUuids()

            Dim RegionName = Region_Name(RegionUUID)
#Disable Warning CA2000 ' Dispose objects before losing scope
            Dim RegionForm As New FormRegion
#Enable Warning CA2000 ' Dispose objects before losing scope
            RegionForm.Init(RegionName)
            RegionForm.Activate()
            RegionForm.Visible = True
            RegionForm.Select()
            RegionForm.BringToFront()

            Application.DoEvents()
            counter += 1
            Y += 100
            X += 100

        Next

    End Sub

    Private Sub RegionsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RegionsToolStripMenuItem.Click
        HelpManual("Regions")
    End Sub

    Private Sub ToolStripMenuItem30_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem30.Click
        HelpManual("Regions")
    End Sub

#End Region

#Region "Startup"

    Private Sub Form1_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed

        RegionForm.Dispose()
        Settings.SaveSettings()

    End Sub

    Private Sub Loaded(sender As Object, e As EventArgs) Handles Me.Load

        AviName.Text = Settings.BulkLoadOwner
        BulkLoadButton.Text = My.Resources.BulkLoad
        Button_AddRegion.Text = Global.Outworldz.My.Resources.Add_Region_word
        Button_Clear.Text = Global.Outworldz.My.Resources.ClearReg
        Button_Normalize.Text = Global.Outworldz.My.Resources.NormalizeRegions
        Button_Region.Text = Global.Outworldz.My.Resources.Configger
        CheckboxConcierge.Checked = Settings.Concierge
        CheckboxConcierge.Checked = Settings.Concierge
        ClearFarmButton.Text = Global.Outworldz.My.Resources.CleanSatyrFarm
        CheckboxConcierge.Text = Global.Outworldz.My.Resources.Announce_visitors
        GroupBoxChat.Text = Global.Outworldz.My.Resources.Chat_Channel_word
        GroupBoxConcierge.Text = Global.Outworldz.My.Resources.Concierge_word
        GroupBoxRegion.Text = Global.Outworldz.My.Resources.Default_Region_word
        Label_whisper_distance.Text = Global.Outworldz.My.Resources.Whisper_distance
        LabelEditRegion.Text = Global.Outworldz.My.Resources.EditRegion
        LabelNewUser.Text = Global.Outworldz.My.Resources.New_User_Home
        labelSay.Text = Global.Outworldz.My.Resources.Say_distance
        LabelShout.Text = Global.Outworldz.My.Resources.Shout_distance
        MenuStrip2.Text = Global.Outworldz.My.Resources._0
        OwnerLabel.Text = My.Resources.OwnerofNewRegions
        RegionBox.Items.AddRange(New Object() {Global.Outworldz.My.Resources.Choose_Region_word})
        TextBox_Whisper_distance.Text = Settings.WhisperDistance
        TextBox_Say_Distance.Text = Settings.SayDistance
        TextBox_Shout_Distance.Text = Settings.ShoutDistance
        Text = Global.Outworldz.My.Resources.Region_word
        TextBoxX.Name = Global.Outworldz.My.Resources.X
        TextBoxX.Text = Settings.HomeVectorX
        TextBoxY.Name = Global.Outworldz.My.Resources.Y
        TextBoxY.Text = Settings.HomeVectorY
        TextBoxZ.Name = Global.Outworldz.My.Resources.Z
        TextBoxZ.Text = Settings.HomeVectorZ
        BlockButton.Text = Global.Outworldz.My.Resources.RearrangeRegions
        ToolStripMenuItem30.Image = Global.Outworldz.My.Resources.question_and_answer
        ToolStripMenuItem30.Text = Global.Outworldz.My.Resources.Help_word
        ToolStripMenuItem30.Text = Global.Outworldz.My.Resources.Help_word

        LoadWelcomeBox()
        LoadRegionBox()

        If AviName.Text.Length = 0 Then
            AviName.BackColor = Color.Red
        End If
        With AviName
            .AutoCompleteCustomSource = MysqlInterface.GetAvatarList()
            .AutoCompleteMode = AutoCompleteMode.Suggest
            .AutoCompleteSource = AutoCompleteSource.CustomSource
        End With

        HelpOnce("Regions")
        SetScreen()
        _initialized = True

    End Sub

    Private Sub LoadRegionBox()
        ' All region load
        RegionBox.Items.Clear()

        For Each RegionUUID In RegionUuids()
            RegionBox.Items.Add(Region_Name(RegionUUID))
        Next

    End Sub

    Private Sub LoadWelcomeBox()

        ' Default welcome region load
        WelcomeBox1.Items.Clear()

        For Each RegionUUID In RegionUuids()
            WelcomeBox1.Items.Add(Region_Name(RegionUUID))
        Next

        Dim s = WelcomeBox1.FindString(Settings.WelcomeRegion)
        If s > -1 Then
            WelcomeBox1.SelectedIndex = s
        Else
            MsgBox(My.Resources.Choose_Welcome, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Choose_Region_word)
            Dim chosen = ChooseRegion(False)
            Dim Index = WelcomeBox1.FindString(chosen)
            WelcomeBox1.SelectedIndex = Index
        End If

    End Sub

#End Region

#Region "Properties"

    Public Property StopLoading As String
        Get
            Return _StopLoading
        End Get
        Set(value As String)
            _StopLoading = value
        End Set
    End Property

#End Region

#Region "Loading"

    Public Sub StartLoading()

        StopLoading = "Stopped"

        If Settings.OarCount = 0 Then Return ' sanity check  as web server may be gone

        Dim Caution = MsgBox($"{My.Resources.CautionOARs2} {CStr(Settings.OarCount)}", vbYesNo Or MsgBoxStyle.MsgBoxSetForeground Or MsgBoxStyle.Critical, My.Resources.Caution_word)
        If Caution <> MsgBoxResult.Yes Then Return

        gEstateName = InputBox(My.Resources.WhatEstateName, My.Resources.WhatEstate, "Outworldz")

        If gEstateName.Length = 0 Then
            MsgBox(My.Resources.TryAgain)
            Return
        End If

        If Settings.BulkLoadOwner.Length = 0 Then
            MsgBox(My.Resources.TryAgain)
            Return
        End If

        gEstateOwner = Settings.SurroundOwner

        Dim CoordX = CStr(LargestX())
        Dim CoordY = CStr(LargestY() + 18)
        Dim coord = InputBox(My.Resources.WheretoStart, My.Resources.StartingLocation, CoordX & "," & CoordY)

        Dim pattern = New Regex("(\d+),\s?(\d+)")
        Dim match As Match = pattern.Match(coord)
        If Not match.Success Then
            MsgBox(My.Resources.BadCoordinates, MsgBoxStyle.Exclamation Or MsgBoxStyle.MsgBoxSetForeground, My.Resources.Error_word)
            Return
        End If

        Dim X As Integer = 0
        Dim Y As Integer = 0
        Try
            X = CInt("0" & match.Groups(1).Value)
            Y = CInt("0" & match.Groups(2).Value)
        Catch
        End Try

        If X <= 1 Or Y < 32 Then
            MsgBox($"{My.Resources.BadCoordinates} : X > 1 And Y > 32", MsgBoxStyle.Exclamation Or MsgBoxStyle.MsgBoxSetForeground, My.Resources.Error_word)
            Return
        End If

        If Not PropOpensimIsRunning() Then
            MysqlInterface.DeregisterRegions(False)
        End If

        FormSetup.Buttons(FormSetup.BusyButton)
        If Not StartMySQL() Then Return
        If Not StartRobust() Then Return
        FormSetup.StartTimer()

        If StopLoading = "StopRequested" Then
            ResetRun()
            Return
        End If

        ' setup parameters for the load
        Dim StartX = X ' loop begin
        Dim MaxSizeThisRow As Integer  ' the largest region in a row
        Dim SizeRegion As Integer = 1 ' (1X1)

        StopLoading = "Running"
        Dim regionList As New Dictionary(Of String, String)
        Try
            For Each J In FormSetup.ContentOAR.GetJson
                If StopLoading = "StopRequested" Then
                    ResetRun()
                    Return
                End If

                Application.DoEvents()

                ' Get name from web site JSON
                Dim Name = J.Name
                Dim shortname = IO.Path.GetFileNameWithoutExtension(Name)
                Dim Index = shortname.IndexOf("(", StringComparison.OrdinalIgnoreCase)
                If (Index >= 0) Then
                    shortname = shortname.Substring(0, Index)
                End If

                If shortname.Length = 0 Then Return
                If shortname = Settings.WelcomeRegion Then Continue For

                Dim RegionUUID As String

                ' it may already exists
                Dim p = IO.Path.Combine(Settings.OpensimBinPath, $"Regions\{shortname}\Region\{shortname}.ini")
                If IO.File.Exists(p) Then
                    ' if so, check that it has prims
                    RegionUUID = FindRegionByName(shortname)

                    Dim o As New Guid
                    If Guid.TryParse(RegionUUID, o) Then
                        Dim Prims = GetPrimCount(RegionUUID)
                        If Prims > 0 Then
                            'TextPrint($"{J.Name} {My.Resources.Ok} ")
                            Continue For
                        Else
                            TextPrint($"{J.Name} needs content")
                            regionList.Add(J.Name, RegionUUID)
                        End If
                    Else
                        BreakPoint.Print("Bad Region UUID " & RegionUUID)
                        ResetRun()
                        Return
                    End If
                Else
                    ' its a new region
                    TextPrint($"{My.Resources.Add_Region_word} {Name} ")

                    If StopLoading = "StopRequested" Then
                        ResetRun()
                        Return
                    End If
                    RegionUUID = CreateRegionStruct(shortname)

                    ' bump across 50 regions, then move up the Max size of that row +1
                    If SizeRegion > MaxSizeThisRow Then
                        MaxSizeThisRow = SizeRegion ' remember the height
                    End If

                    ' read the size from the file name (1X1), (2X2)
                    Dim pattern1 = New Regex("(.*?)\((\d+)[xX](\d+)\)\.")

                    Dim match1 As Match = pattern1.Match(Name)
                    If match1.Success Then
                        SizeRegion = CInt("0" & match1.Groups(2).Value.Trim)
                        If SizeRegion = 0 Then
                            ErrorLog($"Cannot load OAR - bad size in {J.Name}")
                            ResetRun()
                            Return
                        End If
                    Else
                        ErrorLog($"Cannot load OAR {J.Name}")
                        ResetRun()
                        Return
                    End If

                    Coord_X(RegionUUID) = X
                    Coord_Y(RegionUUID) = Y

                    Smart_Start(RegionUUID) = True
                    Teleport_Sign(RegionUUID) = True

                    SizeX(RegionUUID) = SizeRegion * 256
                    SizeY(RegionUUID) = SizeRegion * 256

                    Group_Name(RegionUUID) = shortname

                    RegionIniFilePath(RegionUUID) = IO.Path.Combine(Settings.OpensimBinPath, $"Regions\{shortname}\Region\{shortname}.ini")
                    RegionIniFolderPath(RegionUUID) = IO.Path.Combine(Settings.OpensimBinPath, $"Regions\{shortname}\Region")
                    OpensimIniPath(RegionUUID) = IO.Path.Combine(Settings.OpensimBinPath, $"Regions\{shortname}")

                    Dim port = GetPort(RegionUUID)
                    GroupPort(RegionUUID) = port
                    Region_Port(RegionUUID) = port
                    WriteRegionObject(shortname, shortname)
                    regionList.Add(J.Name, RegionUUID)
                    If X > (StartX + 50) Then   ' if past right border,
                        X = StartX              ' go back to left border
                        Y += MaxSizeThisRow + 1  ' Add the largest size +1 and move up
                        SizeRegion = 256            ' and reset the max height
                    Else     ' we move right the size of the last sim + 1
                        X += SizeRegion + 1
                    End If

                End If
                If StopLoading = "StopRequested" Then
                    ResetRun()
                    Return
                End If
            Next
        Catch ex As Exception
            ResetRun()
            BreakPoint.Print(ex.Message)
        End Try

        Dim keys As List(Of String) = regionList.Keys.ToList

        ' Sort the keys.
        keys.Sort()

        Dim regionList2 As New Dictionary(Of String, String)
        ' Loop over the sorted keys.
        For Each str As String In keys
            regionList2.Add(str, regionList.Item(str))
        Next

        PropUpdateView = True ' make form refresh

        Settings.Smart_Start = True
        Settings.BootOrSuspend = True

        PropChangedRegionSettings = True
        'GetAllRegions(True)
        Firewall.SetFirewall()

        Try
            For Each line In regionList2
                Application.DoEvents()

                If StopLoading = "StopRequested" Then
                    ResetRun()
                    Return
                End If

                If Not PropOpensimIsRunning Then
                    ResetRun()
                    Return
                End If

                Dim Region_Name = line.Key
                Dim RegionUUID = line.Value

                If RegionEnabled(RegionUUID) Then
                    TextPrint($"{My.Resources.Start_word} {Region_Name}")
                    Dim File = $"{PropDomain}/Outworldz_Installer/OAR/{Region_Name}"
                    Dim obj As New TaskObject With {
                        .TaskName = TaskName.LoadAllFreeOARs,
                        .Command = File
                    }
                    RebootAndRunTask(RegionUUID, obj)
                    AddToRegionMap(RegionUUID)

                End If

                If StopLoading = "StopRequested" Then
                    ResetRun()
                    Return
                End If

            Next
        Catch ex As Exception
            BreakPoint.Print(ex.Message)

        End Try

        ResetRun()

    End Sub

    Private Shared Sub ResetRun()

        gEstateName = ""
        FormSetup.Buttons(FormSetup.StopButton)

    End Sub

#End Region

#Region "Button"

    Private Sub AviName_TextChanged(sender As Object, e As EventArgs) Handles AviName.TextChanged

        If Not _initialized Then Return

        Settings.BulkLoadOwner = AviName.Text
        Settings.SaveSettings()
        If AviName.Text.Length = 0 Then
            AviName.BackColor = Color.Red
            Return
        End If

        If IsMySqlRunning() Then
            Dim AvatarUUID As String = ""
            Try
                AvatarUUID = GetAviUUUD(AviName.Text)
            Catch
            End Try
            If AvatarUUID.Length > 0 Then
                AviName.BackColor = Color.White
            End If
        End If

    End Sub

    Private Sub BlockButton_Click(sender As Object, e As EventArgs) Handles BlockButton.Click
        Try
            BlockifyForm.Dispose()
            BlockifyForm = New FormBlockify
            BlockifyForm.Activate()
            BlockifyForm.Visible = True
            BlockifyForm.Select()
            BlockifyForm.BringToFront()
        Catch
        End Try
    End Sub

    Private Sub Button1_Click_2(sender As Object, e As EventArgs) Handles ClearFarmButton.Click

        Dim chosen = ChooseRegion(True) ' all regions, running or not
        If chosen.Length > 0 Then
            Dim RegionUUID = FindRegionByName(chosen)
            Dim File = IO.Path.Combine(Settings.OpensimBinPath, "SFCleanup.txt")
            Dim fileReader = My.Computer.FileSystem.ReadAllText(File, System.Text.Encoding.ASCII)
            Dim Commands As String() = fileReader.Split("\n".ToCharArray())

            For Each line In Commands
                RPC_Region_Command(RegionUUID, line)
            Next
        End If

    End Sub

    Private Sub Button1_Click_3(sender As Object, e As EventArgs) Handles BulkLoadButton.Click

        If BulkLoadButton.Text = My.Resources.Abort Then
            BulkLoadButton.Text = My.Resources.Aborted_word
            StopLoading = "StopRequested"
            TextPrint(My.Resources.Aborting)
            Return
        End If

        BulkLoadButton.Text = My.Resources.Abort
        StartLoading()

        If BulkLoadButton.Text = My.Resources.Aborting Then
            TextPrint(My.Resources.Aborted_word)
        End If

        BulkLoadButton.Text = My.Resources.BulkLoad
        TextPrint("Stopped")
        gEstateName = ""

    End Sub

    Private Sub Button1_Click_4(sender As Object, e As EventArgs) Handles Button1.Click

        If (MsgBox(My.Resources.rezrights, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground, "Information") = Windows.Forms.DialogResult.Yes) Then
            DoLand()
        End If

    End Sub

    Private Sub Form1_FormClosing(sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        If StopLoading = "Running" Then
            If (MsgBox("Abort Loading?", MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground, "") = Windows.Forms.DialogResult.No) Then
                e.Cancel = True
            End If
        End If
        Settings.SaveSettings()

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBoxX.TextChanged
        Dim digitsOnly = New Regex("[^\d]")
        TextBoxX.Text = digitsOnly.Replace(TextBoxX.Text, "")
        Settings.HomeVectorX = TextBoxX.Text
    End Sub

    Private Sub TextBox1_TextChanged_1(sender As Object, e As EventArgs) Handles TextBox_Say_Distance.TextChanged

        Dim digitsOnly = New Regex("[^\d]")
        TextBox_Say_Distance.Text = digitsOnly.Replace(TextBox_Say_Distance.Text, "")
        Settings.SayDistance = TextBox_Say_Distance.Text

    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox_Shout_Distance.TextChanged

        Dim digitsOnly = New Regex("[^\d]")
        TextBox_Shout_Distance.Text = digitsOnly.Replace(TextBox_Shout_Distance.Text, "")
        Settings.ShoutDistance = TextBox_Shout_Distance.Text

    End Sub

    Private Sub Whisper_distance_TextChanged(sender As Object, e As EventArgs) Handles TextBox_Whisper_distance.TextChanged

        Dim digitsOnly = New Regex("[^\d]")
        TextBox_Whisper_distance.Text = digitsOnly.Replace(TextBox_Whisper_distance.Text, "")
        Settings.WhisperDistance = TextBox_Whisper_distance.Text

    End Sub

    Private Sub Y_TextChanged(sender As Object, e As EventArgs) Handles TextBoxY.TextChanged
        Dim digitsOnly = New Regex("[^\d]")
        TextBoxY.Text = digitsOnly.Replace(TextBoxY.Text, "")
        Settings.HomeVectorY = TextBoxY.Text
    End Sub

    Private Sub Z_TextChanged(sender As Object, e As EventArgs) Handles TextBoxZ.TextChanged
        Dim digitsOnly = New Regex("[^\d]")
        TextBoxZ.Text = digitsOnly.Replace(TextBoxZ.Text, "")
        Settings.HomeVectorZ = TextBoxZ.Text
    End Sub

#End Region

End Class
