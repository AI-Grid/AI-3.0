#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.IO
Imports System.Threading

Module Build
    Public NameList As New List(Of String)
    Public Terrains As New List(Of String)
    Public TreeList As New List(Of String)
    Private _ctr As Integer

#Region "Land"

    Public Function GenLand(R As Object) As Boolean

        If R Is Nothing Then Return False
        Dim regionUUID = R.RegionUUID.ToString
        Dim Name = R.RegionName.ToString

        If Not RPC_Region_Command(regionUUID, $"change region {Name}") Then Return False
        If Settings.TerrainType = "Flat" Then
            If Not RPC_Region_Command(regionUUID, $"terrain fill {Settings.FlatlandLevel}") Then Return False
        ElseIf Settings.TerrainType = "Water" Then
            If Not RPC_Region_Command(regionUUID, "terrain fill {Settings.FlatLandLevel}") Then Return False
        ElseIf Settings.TerrainType = "Random" Then
            Dim range = Between(0, Terrains.Count - 1)
            Dim Type As String = Terrains(range)

            Type = Type.Replace(".png", ".r32")
            If Not RPC_Region_Command(regionUUID, $"terrain load {Type}") Then Return False

            Dim Fortytwo = Between(1, 5)
            Select Case Fortytwo
                Case 1
                    If Not RPC_Region_Command(regionUUID, "terrain flip x") Then Return False
                Case 2
                    If Not RPC_Region_Command(regionUUID, "terrain flip y") Then Return False
                Case 3
                    If Not RPC_Region_Command(regionUUID, "terrain flip x") Then Return False
                    If Not RPC_Region_Command(regionUUID, "terrain flip y") Then Return False
            End Select

        ElseIf Settings.TerrainType = "AI" Then

            Dim min = Between(22, 40)
            Dim taper = Between(0, 135)

            Dim range = Between(0, 6)

            Select Case range
                Case 1
                    If Not RPC_Region_Command(regionUUID, $"terrain modify fill {Settings.LandStrength} -taper={taper}") Then Return False
                Case 2
                    If Not RPC_Region_Command(regionUUID, "terrain fill 21") Then Return False
                    If Not RPC_Region_Command(regionUUID, $"terrain modify noise 1 0") Then Return False
                    If Not RPC_Region_Command(regionUUID, $"terrain modify noise 1 0") Then Return False
                    If Not RPC_Region_Command(regionUUID, $"terrain modify noise 1 0") Then Return False
                Case 3
                    If Not RPC_Region_Command(regionUUID, $"terrain modify min {Between(20, 40)} -rec=128,128,120 -taper=-{taper}") Then Return False
                    If Not RPC_Region_Command(regionUUID, $"terrain modify min {Between(20, 40)} -rec=64,64,120 -taper=-{taper}") Then Return False
                    If Not RPC_Region_Command(regionUUID, $"terrain modify min {Between(20, 40)} -rec=64,64,32 -taper=-{taper}") Then Return False
                Case 4
                    If Not RPC_Region_Command(regionUUID, "terrain fill 0") Then Return False
                    If Not RPC_Region_Command(regionUUID, $"terrain modify min {min} -ell=128,128,120 -taper=-{Between(0, 55)}") Then Return False
                    If Not RPC_Region_Command(regionUUID, $"terrain modify noise 1 0") Then Return False
                Case 5
                    If Not RPC_Region_Command(regionUUID, "terrain fill 0") Then Return False
                    If Not RPC_Region_Command(regionUUID, $"terrain modify min {min} -ell=128,128,120 -taper=-{taper}") Then Return False
            End Select

        End If

        Modifiers(regionUUID)

        'force update - Force the region to send all clients updates about all objects.
        If Not RPC_Region_Command(regionUUID, "force update") Then Return False

        Return True

    End Function

    Public Sub SetCores(RegionUUID As String)
        ' bug 171036640
        Try
            If Cores(RegionUUID) = 0 Or Cores(RegionUUID) > Environment.ProcessorCount Then
                Cores(RegionUUID) = CInt(2 ^ Environment.ProcessorCount - 1)
            End If
        Catch
            Cores(RegionUUID) = &HFFFF
        End Try

    End Sub

    ''' <summary>
    ''' Makes a dos box surrounding a visitor
    ''' </summary>
    ''' <param name="RegionUUID">RegionUUID OF center</param>
    Public Sub SurroundingLandMaker(RegionUUID As String)

        ' Make a map of occupied areas
        Dim RegionXY As New Dictionary(Of String, String)

        For Each UUID In RegionUuids()
            Dim SimSize As Integer = CInt(SizeX(RegionUUID) / 256)
            For Xstep = 0 To SimSize - 1
                For Ystep = 0 To SimSize - 1
                    If Not RegionXY.ContainsKey($"{Coord_X(UUID) + Xstep}:{Coord_Y(UUID) + Ystep}") Then
                        RegionXY.Add($"{Coord_X(UUID) + Xstep}:{Coord_Y(UUID) + Ystep}", UUID)
                    End If
                Next
            Next
        Next

        Dim Xloc = Coord_X(RegionUUID)
        Dim Yloc = Coord_Y(RegionUUID)
        Dim CenterSize As Integer = CInt(SizeX(RegionUUID) / 256)
        Dim xy As New List(Of String)

        ' draw a square around the new sim
        Dim X1 = Xloc - Settings.Skirtsize
        Dim X2 = Xloc + Settings.Skirtsize - 1 + CenterSize
        Dim Y1 = Yloc - Settings.Skirtsize
        Dim Y2 = Yloc + Settings.Skirtsize - 1 + CenterSize

        For XPos As Integer = X1 To X2 Step 1
            For Ypos As Integer = Y1 To Y2 Step 1
                If Not xy.Contains($"{XPos}:{Ypos}") Then
                    xy.Add($"{XPos}:{Ypos}")
                End If
            Next
        Next

        Dim GroupName As String = ""

        Dim Bootable As New List(Of String)

        For Each possible As String In xy
            If Not RegionXY.ContainsKey(possible) Then

                Dim parts As String() = possible.Split(New Char() {":"c}) ' split at the space
                Dim nX = CInt(CStr(parts(0).Trim))
                Dim nY = CInt(CStr(parts(1).Trim))

                If GroupName.Length = 0 Then
                    GroupName = FantasyName()
                    TextPrint("Making Group " & GroupName)
                End If

                Dim NewName = MakeTempRegion(GroupName, nX, nY)
                If Not Bootable.Contains(NewName) Then
                    Bootable.Add(NewName)
                End If
            End If

        Next

        If Bootable.Count > 0 Then
            'PropChangedRegionSettings = True
            'GetAllRegions(False)

            For Each Name In Bootable
                If Name.Length > 0 Then
                    ResumeRegion(FindRegionByName(Name))
                End If
            Next
        End If

    End Sub

#End Region

#Region "Landscaper"

    Public Function GenTrees(R As Object) As Boolean

        Dim regionUUID = R.RegionUUID.ToString
        Dim regionName = R.RegionName.ToString

        Dim UseTree As New List(Of String)
        For Each t As String In TreeList
            If GetSetting(t) Then
                UseTree.Add(t)
            End If
        Next

        If Not RPC_Region_Command(regionUUID, $"change region {regionName}") Then Return False
        If Not RPC_Region_Command(regionUUID, "tree active true") Then Return False

        If Settings.DeleteTreesFirst Then
            For Each TT As String In TreeList
                If Not RPC_Region_Command(regionUUID, $"tree remove {TT}") Then Return False
            Next
        End If

        If UseTree.Count = 0 Then
            If Not RPC_Region_Command(regionUUID, "tree active false") Then Return False
        End If

        Debug.Print($"Planting {regionName}")

        For Each NewType In UseTree
            If Not RPC_Region_Command(regionUUID, $"tree load Trees/{NewType}.xml") Then Return False
            If Not RPC_Region_Command(regionUUID, $"tree plant {NewType}") Then Return False
            If Not RPC_Region_Command(regionUUID, $"tree rate 1000") Then Return False
            If Not RPC_Region_Command(regionUUID, "tree active true") Then Return False
            Sleep(1500)
            If Not RPC_Region_Command(regionUUID, $"tree freeze {NewType} true") Then Return False
            If Not RPC_Region_Command(regionUUID, "force update") Then Return False
        Next

        If Not RPC_Region_Command(regionUUID, "tree active false") Then Return False

        'If Not RPC_Region_Command(RegionUUID, $"tree statistics") Then Return
        'force update - Force the region to send all clients updates about all objects.
        If Not RPC_Region_Command(regionUUID, "force update") Then Return False

        BreakPoint.Print("GenTrees success")
        Return True

    End Function

    Public Sub InitTrees()

        Try
            Dim TreeDirectoryInfo As New System.IO.DirectoryInfo(IO.Path.Combine(Settings.OpensimBinPath, "Trees"))
            For Each fileSystemInfo In TreeDirectoryInfo.GetFileSystemInfos
                Dim n = fileSystemInfo.Name
                If n.EndsWith(".xml", StringComparison.OrdinalIgnoreCase) Then
                    Dim part = IO.Path.GetFileName(n)
                    part = part.Replace(".xml", "")
                    TreeList.Add(part)
                End If
            Next
            Debug.Print($"{TreeList.Count} Trees")
        Catch
        End Try

    End Sub

    ''' <summary>
    ''' Background task to make land in any given region.
    ''' </summary>
    ''' <param name="RegionUUID"></param>
    ''' <param name="RegionName"></param>
    Sub Landscape(RegionUUID As String, RegionName As String)

        Dim Threaded = False

        ' pass the two parameters it needs as in object
        Dim c As New RegionEssentials With {
                .RegionUUID = RegionUUID,
                .RegionName = RegionName
        }

        If Threaded Then
            Dim start As ParameterizedThreadStart = AddressOf MakeLand
            Dim MakeLandthread = New Thread(start)
            MakeLandthread.SetApartmentState(ApartmentState.STA)
            MakeLandthread.Priority = ThreadPriority.BelowNormal ' UI gets priority
            MakeLandthread.Start(c)
        Else
            MakeLand(c)
        End If

    End Sub

    Private Function GetSetting(Box As String) As Boolean
        Dim b As Boolean
        Select Case Settings.GetMySetting(Box)
            Case ""
                b = True
            Case "True"
                b = True
            Case "False"
                b = False
            Case Else
                b = False
        End Select

        Return b

    End Function

    Private Sub MakeLand(R As Object)

        If GenLand(R) Then Return
        If GenTrees(R) Then Return

    End Sub

#End Region

#Region "JSON"

    Public Sub InitLand()

        Try
            Dim TerrainDirectoryInfo As New System.IO.DirectoryInfo(IO.Path.Combine(Settings.OpensimBinPath, "Terrains"))
            Dim fileSystemInfo As System.IO.FileSystemInfo
            For Each fileSystemInfo In TerrainDirectoryInfo.GetFileSystemInfos
                Dim n = fileSystemInfo.Name
                If n.EndsWith(".r32", StringComparison.OrdinalIgnoreCase) Then
                    Terrains.Add(fileSystemInfo.FullName)
                End If
            Next
            Debug.Print($"{Terrains.Count} Terrains")

            Dim l As New List(Of String)
            Dim names = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\RegionNames.txt")
            If System.IO.File.Exists(names) Then
                Using reader As StreamReader = System.IO.File.OpenText(names)
                    While reader.Peek <> -1
                        l.Add(reader.ReadLine.Trim)
                    End While
                End Using
            End If
            NameList = l
            Return
        Catch
        End Try

    End Sub

    Private Function FantasyName() As String

        Try
            Dim Existing As New List(Of String)
            For Each UUID In RegionUuids()
                Existing.Add(Region_Name(UUID))
            Next

            ' if no names, make up a number
            If NameList.Count = 0 Then
                _ctr += 1
                Return "SimSurround " & CStr(_ctr)
            End If

            Dim Retry = 300
            While Retry > 0
                Dim index = RandomNumber.Between(1, NameList.Count - 1)
                Dim proposedName = NameList.Item(index)
                If Not Existing.Contains(proposedName) Then
                    Return proposedName
                End If
                Retry -= 1
            End While
            If Retry = 0 Then BreakPoint.Print("Retry Random Name exceeded")
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

        Return ""

    End Function

    Private Function MakeTempRegion(Group As String, X As Integer, Y As Integer) As String

        Dim shortname = FantasyName()

        If IO.File.Exists(IO.Path.Combine(Settings.OpensimBinPath, $"Regions\{Group}\Region\{shortname}.ini")) Then
            BreakPoint.Print("Trying to make a region that exists!")
            Return shortname
        End If

        'kill it
        DeregisterPosition(X, Y)

        ' build it
        Dim RegionUUID = CreateRegionStruct(shortname, "")
        ' Set the defaults for a Landfill
        CrashCounter(RegionUUID) = 0
        Coord_X(RegionUUID) = X
        Coord_Y(RegionUUID) = Y
        Smart_Start(RegionUUID) = True
        Teleport_Sign(RegionUUID) = True
        SizeX(RegionUUID) = 256
        SizeY(RegionUUID) = 256
        Group_Name(RegionUUID) = Group

        SetCores(RegionUUID)

        GDPR(RegionUUID) = CStr(Settings.Gdpr)

        Dim port = LargestPort() + 1
        GroupPort(RegionUUID) = port
        Region_Port(RegionUUID) = port

        WriteRegionObject(Group, shortname)

        PropChangedRegionSettings = True
        Return shortname

    End Function

    Private Sub Modifiers(RegionUUID As String)

        If Settings.LandSmooth Then
            If Not RPC_Region_Command(RegionUUID, $"terrain modify smooth {Settings.LandSmoothValue} -taper={Settings.LandTaper}") Then Return
            If Not RPC_Region_Command(RegionUUID, $"terrain modify smooth {Settings.LandSmoothValue} -taper={Settings.LandTaper}") Then Return
        End If

        If Settings.LandNoise Then
            If Not RPC_Region_Command(RegionUUID, "terrain modify noise 1") Then Return
            If Not RPC_Region_Command(RegionUUID, "terrain modify noise 1") Then Return
        End If

    End Sub

#End Region

    Public Class RegionEssentials
        Public RegionName As String
        Public RegionUUID As String
    End Class

End Module
