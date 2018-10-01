﻿
Imports System.IO
Imports System.ComponentModel

Public Class FormRegion

#Region "Declarations"
    Dim Big As Boolean = False
    Dim n As Integer
    Dim oldname As String = ""
    Dim initted As Boolean = False ' needed a flag to see if we are initted as the dialogs change on start.
    Dim changed As Boolean    ' true if we need to save a form
    Dim isNew As Boolean = False
    Dim RegionClass As RegionMaker

    Dim RName As String

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
#End Region

#Region "Start_Stop"

    Public Sub Init(Name As String)

        Me.Size = New System.Drawing.Size(275, 335)
        Advanced.Visible = False
        Big = True
        Me.Focus()

        RegionClass = RegionMaker.Instance(Form1.MysqlConn)
        If Name = "" Then
            isNew = True
            RegionName.Text = Name
            UUID.Text = Guid.NewGuid().ToString
            SizeX.Text = 256.ToString
            SizeY.Text = 256.ToString
            CoordX.Text = (RegionClass.LargestX() + 4).ToString
            CoordY.Text = (RegionClass.LargestY() + 0).ToString
            RegionPort.Text = (RegionClass.LargestPort() + 1).ToString
            EnabledCheckBox.Checked = True
            RadioButton1.Checked = True
            NonphysicalPrimMax.Text = 1024.ToString
            PhysicalPrimMax.Text = 64.ToString
            ClampPrimSize.Checked = False
            MaxPrims.Text = 45000.ToString
            MaxAgents.Text = 100.ToString

            n = RegionClass.CreateRegion("")

        Else
            isNew = False
            n = RegionClass.FindRegionByName(Name)
            oldname = RegionClass.RegionName(n) ' backup in case of rename
            EnabledCheckBox.Checked = RegionClass.RegionEnabled(n)
            RegionName.Text = Name
            Me.Text = Name ' on screen
            RegionName.Text = RegionClass.RegionName(n) ' on form
            UUID.Text = RegionClass.UUID(n)   ' on screen
            NonphysicalPrimMax.Text = RegionClass.NonPhysicalPrimMax(n).ToString
            PhysicalPrimMax.Text = RegionClass.PhysicalPrimMax(n).ToString
            ClampPrimSize.Checked = RegionClass.ClampPrimSize(n)
            MaxPrims.Text = RegionClass.MaxPrims(n).ToString
            MaxAgents.Text = RegionClass.MaxAgents(n).ToString

            Me.Show() ' time to show the results
            Me.Activate()
            Application.DoEvents()

            ' Size buttons
            If RegionClass.SizeY(n) = 256 And RegionClass.SizeX(n) = 256 Then
                RadioButton1.Checked = True
                RadioButton2.Checked = False
                RadioButton3.Checked = False
                RadioButton4.Checked = False
                SizeX.Text = "256"
                SizeY.Text = "256"
            ElseIf RegionClass.SizeY(n) = 512 And RegionClass.SizeX(n) = 512 Then
                RadioButton1.Checked = False
                RadioButton2.Checked = True
                RadioButton3.Checked = False
                RadioButton4.Checked = False
                SizeX.Text = "512"
                SizeY.Text = "512"
            ElseIf RegionClass.SizeY(n) = 768 And RegionClass.SizeX(n) = 768 Then
                RadioButton1.Checked = False
                RadioButton2.Checked = False
                RadioButton3.Checked = True
                RadioButton4.Checked = False
                SizeX.Text = "768"
                SizeY.Text = "768"
            ElseIf RegionClass.SizeY(n) = 1024 And RegionClass.SizeX(n) = 1024 Then
                RadioButton1.Checked = False
                RadioButton2.Checked = False
                RadioButton3.Checked = False
                RadioButton4.Checked = True
                SizeX.Text = "1024"
                SizeY.Text = "1024"
            Else
                RadioButton1.Checked = False
                RadioButton2.Checked = False
                RadioButton3.Checked = False
                RadioButton4.Checked = False
                SizeX.Text = Convert.ToString(RegionClass.SizeX(n))
                SizeY.Text = Convert.ToString(RegionClass.SizeY(n))
            End If

            ' global coords
            If RegionClass.CoordX(n) <> 0 Then
                CoordX.Text = RegionClass.CoordX(n).ToString
            End If

            If RegionClass.CoordY(n) <> 0 Then
                CoordY.Text = RegionClass.CoordY(n).ToString
            End If

            ' and port
            If RegionClass.RegionPort(n) <> 0 Then
                RegionPort.Text = RegionClass.RegionPort(n).ToString
            End If
        End If

        RName = Name

        '''''''''''''''''''''''''''''  DREAMGRID REGION LOAD '''''''''''''''''

        If RegionClass.MapType(n) = "" Then
            Maps_Use_Default.Checked = True

            If Form1.MySetting.MapType = "None" Then
                MapPicture.Image = My.Resources.blankbox
            ElseIf Form1.MySetting.MapType = "Simple" Then
                MapPicture.Image = My.Resources.Simple
            ElseIf Form1.MySetting.MapType = "Good" Then
                MapPicture.Image = My.Resources.Good
            ElseIf Form1.MySetting.MapType = "Better" Then
                MapPicture.Image = My.Resources.Better
            ElseIf Form1.MySetting.MapType = "Best" Then
                MapPicture.Image = My.Resources.Best
            End If

        ElseIf RegionClass.MapType(n) = "None" Then
            MapNone.Checked = True
            MapPicture.Image = My.Resources.blankbox
        ElseIf RegionClass.MapType(n) = "Simple" Then
            MapSimple.Checked = True
            MapPicture.Image = My.Resources.Simple
        ElseIf RegionClass.MapType(n) = "Good" Then
            MapGood.Checked = True
            MapPicture.Image = My.Resources.Good
        ElseIf RegionClass.MapType(n) = "Better" Then
            MapBetter.Checked = True
            MapPicture.Image = My.Resources.Better
        ElseIf RegionClass.MapType(n) = "Best" Then
            MapBest.Checked = True
            MapPicture.Image = My.Resources.Best
        End If

        Select Case RegionClass.Physics(n)
            Case "" : Physics_Default.Checked = True
            Case "0" : PhysicsNone.Checked = True
            Case "1" : PhysicsODE.Checked = True
            Case "2" : PhysicsBullet.Checked = True
            Case "3" : PhysicsSeparate.Checked = True
            Case "4" : PhysicsubODE.Checked = True
            Case Else : Physics_Default.Checked = True
        End Select

        MaxPrims.Text = RegionClass.MaxPrims(n).ToString

        If RegionClass.AllowGods(n) = "" And RegionClass.RegionGod(n) = "" And RegionClass.ManagerGod(n) = "" Then
            Gods_Use_Default.Checked = True
            AllowGods.Checked = False
            RegionGod.Checked = False
            ManagerGod.Checked = False
        Else
            AllowGods.Checked = CBool(RegionClass.AllowGods(n))
            RegionGod.Checked = CBool(RegionClass.RegionGod(n))
            ManagerGod.Checked = CBool(RegionClass.ManagerGod(n))
        End If

        If RegionClass.RegionSnapShot(n) = "" Then
            PublishDefault.Checked = True
        Else
            Publish.Checked = CBool(RegionClass.RegionSnapShot(n))
        End If

        If RegionClass.Birds(n) = "True" Then
            BirdsCheckBox.Checked = True
        End If

        If RegionClass.Tides(n) = "True" Then
            TidesCheckbox.Checked = True
        End If

        If RegionClass.Teleport(n) = "True" Then
            TPCheckBox1.Checked = True
        End If


        Me.Focus()
        initted = True

    End Sub

    Private Sub FormRegion_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing

        If changed Then
            Dim v = MsgBox("Save changes?", vbYesNo, "Region Save")
            If v = vbYes Then
                Dim message = RegionValidate()
                If Len(message) > 0 Then
                    v = MsgBox(message + vbCrLf + "Discard all changes And Exit anyway?", vbYesNo, "Info")
                    If v = vbYes Then
                        Me.Close()
                    End If
                Else
                    WriteRegion()
                    Form1.RegionClass.UpdateAllRegionPorts()
                    Form1.CopyOpensimProto()
                    RegionClass.GetAllRegions()
                End If
            End If
        End If

    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim message = RegionValidate()
        If Len(message) > 0 Then
            Dim v = MsgBox(message + vbCrLf + "Discard all changes And Exit anyway?", vbYesNo, "Info")
            If v = vbYes Then
                Me.Close()
            End If
        Else

            WriteRegion()
            Form1.RegionClass.UpdateAllRegionPorts()
            RegionClass.GetAllRegions()
            Form1.CopyOpensimProto()

            changed = False
            Me.Close()
        End If

    End Sub

#End Region

#Region "Functions"
    Private Function IsPowerOf256(x As Integer) As Boolean

        Dim y As Single = Convert.ToSingle(x)
        While y > 0
            y = y - 256
        End While
        If y = 0 Then
            Return True
        End If
        Return False

    End Function

    Public Shared Function FilenameIsOK(ByVal fileName As String) As Boolean
        ' check for invalid chars in file name for INI file

        Dim value As Boolean = False
        Try
            value = Not fileName.Intersect(Path.GetInvalidFileNameChars()).Any()
        Catch
        End Try

        Return value

    End Function

    Private Function RegionValidate() As String

        Dim Message As String

        If Len(RegionName.Text) = 0 Then
            Message = "Region name must Not be blank"
            Form1.Log(Message)
            Return Message
        End If

        ' UUID
        Dim result As Guid
        If Not Guid.TryParse(UUID.Text, result) Then
            Message = "Region UUID Is invalid " + UUID.Text
            Form1.Log(Message)
            Return Message
        End If

        ' global coords
        If Convert.ToInt16(CoordX.Text) = 0 Then
            Message = "Region Coordinate X cannot be zero"
            Form1.Log(Message)
            Return Message
        ElseIf Convert.ToInt16(CoordX.Text) > 65536 Then
            Message = "Region Coordinate X Is too large"
            Form1.Log(Message)
            Return Message
        End If

        If Convert.ToInt16(CoordY.Text) = 0 Then
            Message = "Region CoordY cannot be zero"
            Form1.Log(Message)
            Return Message
        ElseIf Convert.ToInt16(CoordY.Text) > 65536 Then
            Message = "Region CoordY Is too large"
            Form1.Log(Message)
            Return Message
        End If

        If Convert.ToInt16(RegionPort.Text) = 0 Then
            Message = "Region Port cannot be zero Or undefined"
            Form1.Log(Message)
            Return Message
        End If

        Dim aresult As Guid
        If Not Guid.TryParse(UUID.Text, aresult) Then
            Message = "Not a valid UUID"
            Form1.Log(Message)
            Return Message
        End If

        If (NonphysicalPrimMax.Text = "") Or (CType(NonphysicalPrimMax.Text, Integer) <= 0) Then
            Message = "Not a valid Non-Physical Prim Max Value. Must be greater than 0."
            Form1.Log(Message)
            Return Message
        End If

        If (PhysicalPrimMax.Text = "") Or (CType(PhysicalPrimMax.Text, Integer) <= 0) Then
            Message = "Not a valid Physical Prim Max Value. Must be greater than 0."
            Form1.Log(Message)
            Return Message
        End If

        If (MaxPrims.Text = "") Or (CType(MaxPrims.Text, Integer) <= 0) Then
            Message = "Not a valid MaxPrims Value. Must be greater than 0."
            Form1.Log(Message)
            Return Message
        End If

        If (MaxAgents.Text = "") Or (CType(MaxAgents.Text, Integer) <= 0) Then
            Message = "Not a valid MaxAgents Value. Must be greater than 0."
            Form1.Log(Message)
            Return Message
        End If

        Return ""
    End Function

    Private Sub WriteRegion()

        ' save the Region File, choose an existing DOS box to put it in, or make a new one

        Dim Filepath = RegionClass.RegionPath(n)
        Dim Folderpath = RegionClass.FolderPath(n)

        ' rename is possible
        If oldname <> RegionName.Text And Not isNew Then
            Try
                My.Computer.FileSystem.RenameFile(Filepath, RegionName.Text + ".bak")
            Catch ex As Exception
                Debug.Print(ex.Message)
            End Try

            Try
                Dim NewFilepath = Form1.gPath & "bin\Regions\" + RegionName.Text + "\Region\"
                Directory.CreateDirectory(NewFilepath)
                Filepath = NewFilepath + RegionName.Text + ".ini"
                RegionClass.RegionPath(n) = Filepath
                Form1.CopyOpensimProto()
            Catch
                MsgBox("Cannot create New region. It seems to already exist", vbInformation, "Info")
                Form1.PrintFast("Aborted")
                Return
            End Try
        End If

        ' might be a new region, so give them a choice

        If isNew Then
            Dim NewGroup As String = RegionName.Text
            Dim yesNo As MsgBoxResult = MsgBox("New regions can can be combined with other regions in an existing DOS box (Yes), Or run in their own Dos Box (No)", vbYesNo, "Combine Regions?")
            If yesNo = vbYes Then
                NewGroup = RegionChosen()
                If NewGroup = "" Then
                    Form1.PrintFast("Aborted")
                    Return
                End If
            End If

            If Not Directory.Exists(Filepath) Or Filepath = "" Then
                Directory.CreateDirectory(dir & "bin\Regions\" + NewGroup + "\Region")
            End If

            RegionClass.RegionPath(n) = dir & "bin\Regions\" + NewGroup + "\Region\" + RegionName.Text + ".ini"

        End If

        Dim Snapshot As String = ""
        If PublishDefault.Checked Then
            Snapshot = ""
        ElseIf NoPublish.Checked Then
            Snapshot = "False"
        ElseIf Publish.Checked Then
            Snapshot = "True"
        End If

        RegionClass.RegionSnapShot(n) = Snapshot

        Dim Map As String = ""
        If MapNone.Checked Then
            Map = ""
        ElseIf MapNone.checked Then
            Map = "None"
        ElseIf MapSimple.Checked Then
            Map = "Simple"
        ElseIf MapGood.Checked Then
            Map = "Good"
        ElseIf MapBetter.Checked Then
            Map = "Better"
        ElseIf MapBest.Checked Then
            Map = "Best"
        End If

        RegionClass.MapType(n) = Map

        Dim Phys As String = ""
        If Physics_Default.Checked Then
            Phys = ""
        ElseIf PhysicsNone.Checked Then
            Phys = "0"
        ElseIf PhysicsODE.Checked Then
            Phys = "1"
        ElseIf PhysicsBullet.Checked Then
            Phys = "2"
        ElseIf PhysicsSeparate.Checked Then
            Phys = "3"
        ElseIf PhysicsubODE.Checked Then
            Phys = "4"
        End If

        RegionClass.Physics(n) = Phys

        Dim AllowAGod As String = ""
        If AllowGods.Checked Then
            AllowAGod = "True"
            RegionClass.AllowGods(n) = "True"
        End If

        Dim ARegionGod As String = ""
        If RegionGod.Checked Then
            ARegionGod = "True"
            RegionClass.RegionGod(n) = "True"
        End If

        Dim AManagerGod As String = ""
        If ManagerGod.Checked Then
            AManagerGod = "True"
            RegionClass.ManagerGod(n) = "True"
        End If

        Dim Region = "; * Regions configuration file" &
                        "; * This Is Your World. See Common Settings->[Region Settings]." & vbCrLf &
                        "; Automatically changed by Dreamworld" & vbCrLf &
                        "[" & RegionName.Text & "]" & vbCrLf &
                        "RegionUUID = " & UUID.Text & vbCrLf &
                        "Location = " & CoordX.Text & "," & CoordY.Text & vbCrLf &
                        "InternalAddress = 0.0.0.0" & vbCrLf &
                        "InternalPort = " & RegionPort.Text & vbCrLf &
                        "AllowAlternatePorts = False" & vbCrLf &
                        "ExternalHostName = " & Form1.MySetting.PublicIP & vbCrLf &
                        "SizeX = " & SizeX.Text & vbCrLf &
                        "SizeY = " & SizeY.Text & vbCrLf &
                        "Enabled = " & EnabledCheckBox.Checked.ToString & vbCrLf &
                        "NonPhysicalPrimMax = " & NonphysicalPrimMax.Text & vbCrLf &
                        "PhysicalPrimMax = " & PhysicalPrimMax.Text & vbCrLf &
                        "ClampPrimSize = " & ClampPrimSize.Checked.ToString & vbCrLf &
                        "MaxAgents = " & MaxAgents.Text & vbCrLf &
                        "MaxPrims = " & MaxPrims.Text & vbCrLf & vbCrLf &
                        ";# Extended region properties from Dreamgrid" & vbCrLf &
                        "RegionSnapShot = " & Snapshot & vbCrLf &
                        "MapType = " & Map & vbCrLf &
                        "Physics = " & Phys & vbCrLf &
                        "AllowGods = " & AllowAGod & vbCrLf &
                        "RegionGod = " & ARegionGod & vbCrLf &
                        "ManagerGod = " & AManagerGod & vbCrLf &
                        "Birds = " & BirdsCheckBox.Checked.ToString & vbCrLf &
                        "Tides = " & TidesCheckbox.Checked.ToString & vbCrLf &
                        "Teleport = " & TPCheckBox1.Checked.ToString

        Debug.Print(Region)

        Try
            Using outputFile As New StreamWriter(RegionClass.RegionPath(n), False)
                outputFile.Write(Region)
            End Using
        Catch
        End Try

        oldname = RegionName.Text

    End Sub

    Private Function RegionChosen() As String

        Dim Chooseform As New Choice ' form for choosing a set of regions
        ' Show testDialog as a modal dialog and determine if DialogResult = OK.

        Chooseform.FillGrid("Group")  ' populate the grid with either Group or RegionName

        Dim chosen As String
        Chooseform.ShowDialog()
        Try
            ' Read the chosen sim name
            chosen = Chooseform.DataGridView.CurrentCell.Value.ToString()
            If chosen.Length > 0 Then
                Chooseform.Dispose()
            End If
        Catch ex As Exception
            chosen = ""
        End Try
        Return chosen

    End Function
#End Region

#Region "Default"

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        '663, 613
        If Big Then
            Me.Size = New System.Drawing.Size(716, 555)
            Advanced.Visible = True
            Big = False
        Else
            Me.Size = New System.Drawing.Size(275, 290)
            Advanced.Visible = False
            Big = True
        End If
    End Sub

    Private Sub DeleteButton_Click(sender As Object, e As EventArgs) Handles DeleteButton.Click

        Dim msg = MsgBox("Are you sure you want To delete this region? ", vbYesNo, "Delete?")
        If msg = vbYes Then
            Try
                My.Computer.FileSystem.DeleteFile(Form1.gPath & "bin\Regions\" + RegionName.Text + "\Region\" + RegionName.Text + ".bak")
            Catch
            End Try

            Try
                My.Computer.FileSystem.RenameFile(RegionClass.RegionPath(n), RegionName.Text + ".bak")
                RegionClass.GetAllRegions()

            Catch ex As Exception
            End Try
        End If
        Me.Close()

    End Sub

    Private Sub RLostFocus(sender As Object, e As EventArgs) Handles RegionName.TextChanged

        If Len(RegionName.Text) > 0 And initted Then
            If Not FilenameIsOK(RegionName.Text) Then
                MsgBox("Region name can't use special characters such as < > : """" / \ | ? *", vbInformation, "Info")
                Return
        End If
        changed = True
        End If

    End Sub
#End Region

#Region "More"

    Private Sub Coordy_TextChanged(sender As Object, e As EventArgs) Handles CoordY.TextChanged
        If initted And CoordY.Text <> "" Then
            Try
                CoordY.Text = CoordY.Text
            Catch
            End Try
            changed = True
        End If

    End Sub

    Private Sub CoordX_TextChanged(sender As Object, e As EventArgs) Handles CoordX.TextChanged

        If initted And CoordX.Text <> "" Then
            Try
                CoordX.Text = CoordX.Text
            Catch

            End Try
            changed = True
        End If

    End Sub

    Private Sub RegionPort_TextChanged(sender As Object, e As EventArgs) Handles RegionPort.TextChanged

        If initted Then
            Try
                RegionPort.Text = RegionPort.Text
            Catch

            End Try
            changed = True
        End If

    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged

        If initted And RadioButton1.Checked Then
            SizeX.Text = "256"
            SizeY.Text = "256"
            changed = True
        End If

    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged

        If initted And RadioButton2.Checked Then
            SizeX.Text = "512"
            SizeY.Text = "512"
            changed = True
        End If

    End Sub

    Private Sub RadioButton3_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton3.CheckedChanged

        If initted And RadioButton3.Checked Then
            SizeX.Text = "768"
            SizeY.Text = "768"
            changed = True
        End If

    End Sub

    Private Sub RadioButton4_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton4.CheckedChanged

        If initted And RadioButton4.Checked Then
            SizeX.Text = "1024"
            SizeY.Text = "1024"
            changed = True
        End If

    End Sub

    Private Sub UUID_LostFocus(sender As Object, e As EventArgs) Handles UUID.LostFocus

        If UUID.Text <> UUID.Text And initted Then
            Dim resp = MsgBox("Changing the UUID will lose all data in the old sim and create a new, empty sim. Are you sure you wish to change the UUID?", vbYesNo, "Info")
            If resp = vbYes Then
                changed = True
                Dim result As Guid
                If Guid.TryParse(UUID.Text, result) Then

                Else
                    Dim ok = MsgBox("Not a valid UUID. Do you want a new, Random UUID?", vbOKCancel, "Info")
                    If ok = vbOK Then
                        UUID.Text = System.Guid.NewGuid.ToString
                    End If
                End If
            End If
        End If

    End Sub

    Private Sub SizeX_Changed(sender As Object, e As EventArgs) Handles SizeX.LostFocus

        If initted And SizeX.Text <> "" Then
            If Not IsPowerOf256(CType(SizeX.Text, Integer)) Then
                MsgBox("Must be a multiple of 256: 256,512,768,1024,1280,1536,1792,2048,2304,2560, ..", vbInformation, "Size X,Y")
            Else
                If CType(SizeX.Text, Double) > 1024 Then
                    RadioButton1.Checked = False
                    RadioButton2.Checked = False
                    RadioButton3.Checked = False
                    RadioButton4.Checked = False
                End If

                SizeY.Text = SizeX.Text
                changed = True

            End If
        End If

    End Sub

#End Region

#Region "MoreExtras"

    Private Sub Maps_Use_Default_changed(sender As Object, e As EventArgs) Handles Maps_Use_Default.CheckedChanged
        If Maps_Use_Default.Checked Then
            Form1.Log("Region " + Name + " Map is set to Default")
            MapNone.Checked = False
            MapSimple.Checked = False
            MapGood.Checked = False
            MapBetter.Checked = False
            MapBest.Checked = False

            If Form1.MySetting.MapType = "None" Then
                MapPicture.Image = My.Resources.blankbox
            ElseIf Form1.MySetting.MapType = "Simple" Then
                MapPicture.Image = My.Resources.Simple
            ElseIf Form1.MySetting.MapType = "Good" Then
                MapPicture.Image = My.Resources.Good
            ElseIf Form1.MySetting.MapType = "Better" Then
                MapPicture.Image = My.Resources.Better
            ElseIf Form1.MySetting.MapType = "Best" Then
                Form1.MySetting.MapType = "Best"
            End If
        End If

        If initted Then changed = True
    End Sub

    Private Sub MapNone_CheckedChanged(sender As Object, e As EventArgs) Handles MapNone.CheckedChanged
        If MapNone.Checked Then
            Form1.Log("Region " + Name + " Map is set to None")
            MapPicture.Image = My.Resources.blankbox
        End If
        If initted Then changed = True
    End Sub

    Private Sub MapSimple_CheckedChanged(sender As Object, e As EventArgs) Handles MapSimple.CheckedChanged
        If MapSimple.Checked Then
            Form1.Log("Region " + Name + " Map is set to Simple")
            MapPicture.Image = My.Resources.Simple
        End If
        If initted Then changed = True
    End Sub

    Private Sub MapGood_CheckedChanged(sender As Object, e As EventArgs) Handles MapGood.CheckedChanged
        If MapGood.Checked Then
            Form1.Log("Region " + Name + " Map is set to Good")
            MapPicture.Image = My.Resources.Good
        End If
        If initted Then changed = True
    End Sub

    Private Sub MapBetter_CheckedChanged(sender As Object, e As EventArgs) Handles MapBetter.CheckedChanged
        If MapBetter.Checked Then
            Form1.Log("Region " + Name + " Map is set to Better")
            MapPicture.Image = My.Resources.Better
        End If
        If initted Then changed = True
    End Sub

    Private Sub MapBest_CheckedChanged(sender As Object, e As EventArgs) Handles MapBest.CheckedChanged
        If MapBest.Checked Then
            Form1.Log("Region " + Name + " Map is set to Best")
            MapPicture.Image = My.Resources.Best
        End If
        If initted Then changed = True
    End Sub


    Private Sub Physics_Default_CheckedChanged(sender As Object, e As EventArgs) Handles Physics_Default.CheckedChanged

        If Physics_Default.Checked Then
            Form1.Log("Region " + Name + " Physics is set to default")
            PhysicsNone.Checked = False
            PhysicsODE.Checked = False
            PhysicsubODE.Checked = False
            PhysicsBullet.Checked = False
            PhysicsSeparate.Checked = False
        End If

        If initted Then changed = True
    End Sub

    Private Sub PhysicsNone_CheckedChanged(sender As Object, e As EventArgs) Handles PhysicsNone.CheckedChanged
        If PhysicsNone.Checked Then
            Form1.Log("Region " + Name + " Physics is set to None")
        End If
        If initted Then changed = True
    End Sub

    Private Sub PhysicsODE_CheckedChanged(sender As Object, e As EventArgs) Handles PhysicsODE.CheckedChanged
        If PhysicsODE.Checked Then
            Form1.Log("Region " + Name + " Physics is set to ODE")
        End If
        If initted Then changed = True
    End Sub

    Private Sub PhysicsBullet_CheckedChanged(sender As Object, e As EventArgs) Handles PhysicsBullet.CheckedChanged
        If PhysicsBullet.Checked Then
            Form1.Log("Region " + Name + " Physics is set to Bullet")
        End If
        If initted Then changed = True
    End Sub

    Private Sub PhysicsSeparate_CheckedChanged(sender As Object, e As EventArgs) Handles PhysicsSeparate.CheckedChanged
        If PhysicsSeparate.Checked Then
            Form1.Log("Region " + Name + " Physics is set to Bullet in a Thread")
        End If
        If initted Then changed = True
    End Sub

    Private Sub PhysicsubODE_CheckedChanged(sender As Object, e As EventArgs) Handles PhysicsubODE.CheckedChanged
        If PhysicsubODE.Checked Then
            Form1.Log("Region " + Name + " Physics is set to Ubit's ODE")
        End If
        If initted Then changed = True
    End Sub

    Private Sub EnableMaxPrims_text(sender As Object, e As EventArgs) Handles MaxPrims.TextChanged
        If initted Then changed = True
    End Sub

    Private Sub Gods_Use_Default_CheckedChanged(sender As Object, e As EventArgs) Handles Gods_Use_Default.CheckedChanged
        If Gods_Use_Default.Checked Then
            AllowGods.Checked = False
            RegionGod.Checked = False
            ManagerGod.Checked = False
            Form1.Log("Region " + Name + " is set to default for Gods")
        End If

        If initted Then changed = True
    End Sub

    Private Sub AllowGods_CheckedChanged(sender As Object, e As EventArgs) Handles AllowGods.CheckedChanged

        If AllowGods.Checked Then
            Gods_Use_Default.Checked = False
            Form1.Log("Region " + Name + " is allowing Gods")
        Else
            Form1.Log("Region " + Name + " is not allowing Gods")
        End If

        If initted Then changed = True
    End Sub

    Private Sub RegionGod_CheckedChanged(sender As Object, e As EventArgs) Handles RegionGod.CheckedChanged

        If RegionGod.Checked Then
            Gods_Use_Default.Checked = False
            Form1.Log("Region " + Name + " is allowing Region Gods")
        Else
            Form1.Log("Region " + Name + " is not allowing Region Gods")
        End If

        If initted Then changed = True
    End Sub

    Private Sub ManagerGod_CheckedChanged(sender As Object, e As EventArgs) Handles ManagerGod.CheckedChanged

        If ManagerGod.Checked Then
            Gods_Use_Default.Checked = False
            Form1.Log("Region " + Name + " is allowing Manager Gods")
        Else
            Form1.Log("Region " + Name + " is not allowing Manager Gods")
        End If
        If initted Then changed = True
    End Sub

    Private Sub PublishDefault_CheckedChanged(sender As Object, e As EventArgs) Handles PublishDefault.CheckedChanged
        If PublishDefault.Checked Then
            Form1.Log("Region " + Name + " is set to default for snapshots")
        End If
        If initted Then changed = True
    End Sub

    Private Sub NoPublish_CheckedChanged(sender As Object, e As EventArgs) Handles NoPublish.CheckedChanged
        If NoPublish.Checked Then
            Form1.Log("Region " + Name + " is not set to publish snapshots")
        End If
        If initted Then changed = True
    End Sub

    Private Sub Publish_CheckedChanged(sender As Object, e As EventArgs) Handles Publish.CheckedChanged
        If Publish.Checked Then
            Form1.Log("Region " + Name + " is publishing snapshots")
        Else
            Form1.Log("Region " + Name + " is not publishing snapshots")
        End If
        If initted Then changed = True
    End Sub

    Private Sub BirdsCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles BirdsCheckBox.CheckedChanged
        If BirdsCheckBox.Checked Then
            Form1.Log("Region " + Name + " has birds enabled")
        End If
        If initted Then changed = True
    End Sub

    Private Sub TidesCheckbox_CheckedChanged(sender As Object, e As EventArgs) Handles TidesCheckbox.CheckedChanged
        If TidesCheckbox.Checked Then
            Form1.Log("Region " + Name + " has tides enabled")
        End If
        If initted Then changed = True
    End Sub

    Private Sub TPCheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles TPCheckBox1.CheckedChanged
        If TPCheckBox1.Checked Then
            Form1.Log("Region " + Name + " has Teleport Board enabled")
        End If
        If initted Then changed = True
    End Sub


#End Region
End Class
