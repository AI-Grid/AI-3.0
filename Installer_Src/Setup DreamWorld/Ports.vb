Imports System.Collections.Concurrent
Imports System.IO
Imports System.Text.RegularExpressions

Module Ports

    Private ReadOnly RegionPortList As New ConcurrentDictionary(Of Integer, String)

    Public Function GetAlreadyUsedPorts() As Integer

        Dim folders() As String
        Try
            folders = Directory.GetDirectories(Settings.OpensimBinPath + "Regions")
        Catch
            Return 0
        End Try

        System.Array.Sort(folders)
        Dim Ctr As Integer = 0
        For Each FolderName As String In folders
            Dim regionfolders = Directory.GetDirectories(FolderName)
            For Each FileName As String In regionfolders

                If FileName.EndsWith("DataSnapshot", StringComparison.OrdinalIgnoreCase) Then Continue For

                Try
                    Dim inis() As String = Nothing
                    Try
                        inis = Directory.GetFiles(FileName, "*.ini", SearchOption.TopDirectoryOnly)
                    Catch ex As Exception
                        BreakPoint.Dump(ex)
                    End Try

                    For Each file As String In inis
                        Ctr += 1
                        Dim fName = Path.GetFileNameWithoutExtension(file)
                        Debug.Print($"Loading {fName}")

                        Dim pattern1 = New Regex("\\Regions\\(.*?)\\Region", RegexOptions.IgnoreCase)
                        Dim match1 As Match = pattern1.Match(file, RegexOptions.IgnoreCase)
                        Dim Groupname As String
                        If match1.Success Then
                            Groupname = match1.Groups(1).Value
                        Else
                            MsgBox("Cannot read INI file For " & fName, vbCritical Or MsgBoxStyle.MsgBoxSetForeground)
                            Exit For
                        End If

                        Dim INI = New LoadIni(file, ";", System.Text.Encoding.ASCII)

                        Dim RegionUUID = CStr(INI.GetIni(fName, "RegionUUID", "", "String"))

                        ' check it
                        Dim SomeUUID As New Guid
                        If Not Guid.TryParse(RegionUUID, SomeUUID) Then
                            MsgBox("Cannot read UUID In INI file For " & fName, vbCritical Or MsgBoxStyle.MsgBoxSetForeground)
                            Exit For
                        End If

                        If CBool(GetHwnd(Groupname)) Then
                            Dim RegionPort = CInt("0" + INI.GetIni(fName, "InternalPort", "", "Integer"))
                            If RegionPortList.ContainsKey(RegionPort) Then
                                RegionPortList.Item(RegionPort) = RegionUUID  ' update
                            Else
                                RegionPortList.TryAdd(RegionPort, RegionUUID) ' add
                            End If
                        End If
                    Next
                Catch ex As Exception
                    BreakPoint.Dump(ex)
                End Try
            Next
            Application.DoEvents()
        Next

        Debug.Print($"RegionPortList count is {RegionPortList.Count}")
        Return Ctr

    End Function

    Public Function GetPort(regionUUID As String) As Integer

        For Each kp In RegionPortList
            If kp.Value = regionUUID Then
                Return kp.Key
            End If
        Next

        Return GetFreePort(regionUUID)

    End Function

    '' Get the larget Region Port
    Public Function LargestPort() As Integer

        Dim retval As Integer = Settings.FirstRegionPort ' start at 8004

        For Each kp In RegionPortList
            If kp.Key > retval Then
                retval = kp.Key
            End If
        Next

        Return retval

    End Function

    ''' <summary>
    ''' Removes a port from use when a region is deleted
    ''' </summary>
    ''' <param name="Port">Region or Group Port</param>
    ''' <returns>True if it is removed, false if it fails to be removed</returns>
    Public Function ReleasePort(Port As Integer) As Boolean
        If RegionPortList.ContainsKey(Port) Then
            Return RegionPortList.TryRemove(Port, "")
        End If
        Return True
    End Function

    Private Function GetFreePort(RegionUUID As String) As Integer

        Dim StartPort = Settings.FirstRegionPort

        While (True)
            If RegionPortList.ContainsKey(StartPort) Then
                StartPort += 1
            Else
                RegionPortList.TryAdd(StartPort, RegionUUID)
                Exit While
            End If
        End While
        Return StartPort

    End Function

End Module
