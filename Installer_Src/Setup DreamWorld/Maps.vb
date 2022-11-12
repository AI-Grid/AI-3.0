
Imports System.Threading

Module Maps
    Public ReadOnly Map As New Dictionary(Of String, String)

#Region "MapMaking"

    ReadOnly MakeMapLock As New Object

    Public Sub MakeMaps()

        Dim Mapthread As Thread
        Mapthread = New Thread(AddressOf BuildMap)
        Mapthread.SetApartmentState(ApartmentState.STA)
        Mapthread.Priority = ThreadPriority.BelowNormal
        Mapthread.Start()

    End Sub

    Private Sub BuildMap()

        Delete_all_visitor_maps()

        For Each RegionUUID In RegionUuids()
            Application.DoEvents()
            Make_Region_Map(RegionUUID)
            Sleep(100)
        Next

    End Sub

#End Region

    Public Sub Make_Region_Map(regionUUID As String)

        Dim SavePath = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Apache\htdocs\Stats\Maps")
        Try
            FileIO.FileSystem.CreateDirectory(SavePath)
        Catch ex As Exception
            BreakPoint.Dump(ex)
            Return
        End Try

        SyncLock MakeMapLock
            Dim MapPath = IO.Path.Combine(Settings.OpensimBinPath, "maptiles\00000000-0000-0000-0000-000000000000")

            Dim Name = Region_Name(regionUUID)
            Dim SimSize As Integer = CInt(SizeX(regionUUID))
            If SimSize = 0 Then Return
            Using bmp As New Bitmap(SimSize, SimSize)
                Dim X = 0
                Dim Y = 0

                ' Loop through the images pixels to reset color.

                'If False Then
                'For X = 0 To bmp.Width - 1
                'For Y = 0 To bmp.Height - 1
                'Dim newColor = Color.FromArgb(230, 230, 230)
                'Try
                'bmp.SetPixel(X, Y, newColor)
                'Catch ex As Exception
                'BreakPoint.Dump(ex)
                'End Try
                '
                'Next
                'Next
                'End If

                Dim Out As Image = bmp
                Dim Src As Image = bmp
                Dim XS = SimSize / 256

                X = 0
                For Xstep = 0 To XS - 1
                    Y = CInt(SimSize - (SimSize / XS))
                    For Ystep = 0 To XS - 1
                        Dim MapImage = $"map-1-{Coord_X(regionUUID) + Xstep }-{Coord_Y(regionUUID) + Ystep  }-objects.jpg"
                        '  BreakPoint.Print(Name)
                        '  BreakPoint.Print(MapImage)

                        ' images plot at up[per left, Opensim is lower left
                        ' for a 2X2 this is the value
                        'X:Y = 0:256
                        'X:Y = 0:0
                        'X:Y = 256:256
                        'X:Y = 256:0

                        Dim RegionSrc = IO.Path.Combine(MapPath, MapImage)
                        If IO.File.Exists(RegionSrc) Then
                            Src = Image.FromFile(RegionSrc)
                            Using g As Graphics = Graphics.FromImage(Out)
                                'Breakpoint.Print(CStr(X) & ":" & CStr(Y))
                                Try
                                    g.DrawImage(Src, New System.Drawing.Rectangle(X, Y, 256, 256))
                                    Out.Save(IO.Path.Combine(SavePath, $"{Name}.png"))
                                Catch ex As Exception
                                End Try
                            End Using
                        Else
                            Return
                        End If
                        Y -= 256
                    Next
                    X += 256
                Next

            End Using
        End SyncLock

    End Sub

#Region "Map Deleting"

    Public Sub Delete_all_visitor_maps()

        Dim SavePath = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Apache\htdocs\Stats\Maps")
        DeleteDirectory(SavePath, FileIO.DeleteDirectoryOption.DeleteAllContents)

    End Sub

    Public Sub Delete_Region_Map(RegionUUID As String)

        Dim Xloc = Coord_X(RegionUUID)
        Dim Yloc = Coord_Y(RegionUUID)

        Dim SimSize As Integer = CInt(SizeX(RegionUUID) / 256)
        For Xstep = 0 To SimSize - 1
            For Ystep = 0 To SimSize - 1
                Dim gr As String = $"{Xloc + Xstep},{Yloc + Ystep}"
                If Map.ContainsKey(gr) Then Map.Remove(gr)
            Next
        Next

    End Sub

    Public Sub DeleteMapTile(RegionUUID As String)

        Dim path = IO.Path.Combine(Settings.OpensimBinPath(), "maptiles\00000000-0000-0000-0000-000000000000")

        For i = 1 To 8
            For XX = 0 To SizeX(RegionUUID) / 256
                For YY = 0 To SizeY(RegionUUID) / 256
                    Dim x1 = Coord_X(RegionUUID) + XX
                    Dim y1 = Coord_Y(RegionUUID) + YY
                    Dim file = IO.Path.Combine(path, $"map-{i}-{x1}-{y1}-objects.jpg")
                    Debug.Print($"Delete map-{i}-{x1}-{y1}-objects.jpg")
                    DeleteFile(file)
                Next
            Next
        Next

    End Sub

#End Region

End Module
