#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.Collections.Concurrent

Module Teleport

    Public Fin As New List(Of String)
    Public TeleportAvatarDict As New ConcurrentDictionary(Of String, String)

    ''' <summary>Check if region is ready for login</summary>
    ''' <returns>true if up</returns>
    Public Function IsRegionReady(Port As Integer) As Boolean

        Return CheckPort(Settings.LANIP, Port)

    End Function

    Public Sub TeleportAgents()

        Bench.Start("TeleportAgents")
        Try
            For Each Keypair In TeleportAvatarDict
                Dim AgentID = Keypair.Key
                Dim RegionToUUID = Keypair.Value
                Dim status = RegionStatus(RegionToUUID)
                Dim Port As Integer = GroupPort(RegionToUUID)

                If status = SIMSTATUSENUM.Stopped Then
                    Fin.Add(AgentID) ' cancel this, the region went away

                ElseIf status = SIMSTATUSENUM.Booted Then
                    ShowDOSWindow(RegionToUUID, MaybeShowWindow())
                    If IsRegionReady(Port) And RegionIsRegisteredOnline(RegionToUUID) Then
                        Dim DestinationName = Region_Name(RegionToUUID)
                        Dim FromRegionUUID As String = GetRegionFromAgentId(AgentID)
                        Dim fromName = Region_Name(FromRegionUUID)
                        If fromName Is Nothing Then Fin.Add(AgentID)
                        If fromName.Length > 0 Then
                            If Settings.TeleportSleepTime = 0 And Not Settings.BootOrSuspend Then
                                'nothing
                            ElseIf Settings.TeleportSleepTime > 0 And Not Settings.BootOrSuspend Then
                                RPC_admin_dialog(AgentID, $"{ Region_Name(RegionToUUID)} will be ready in {CStr(Settings.TeleportSleepTime)} seconds.")
                                Sleep(Settings.TeleportSleepTime * 1000)
                            End If

                            If TeleportTo(FromRegionUUID, DestinationName, AgentID) Then
                                Logger("Teleport", $"{DestinationName} teleport command sent", "Teleport")
                                Fin.Add(AgentID)
                            Else
                                Logger("Teleport", $"{DestinationName} failed to receive teleport", "Teleport")
                                BreakPoint.Print($"{DestinationName} failed to receive teleport")
                                RPC_admin_dialog(AgentID, $"Unable to locate region { Region_Name(RegionToUUID)}.")
                                Fin.Add(AgentID) ' cancel this, the agent is not anywhere  we can get to
                            End If
                        Else
                            Fin.Add(AgentID) ' cancel this, the agent is not anywhere  we can get to
                        End If
                    End If
                    ShowDOSWindow(RegionToUUID, MaybeHideWindow())
                End If
            Next
        Catch
        End Try
        ' rem from the to list as they have moved on
        For Each str As String In Fin
            Logger("Teleport Done", str, "Teleport")
            If TeleportAvatarDict.ContainsKey(str) Then
                TeleportAvatarDict.TryRemove(str, "")
            End If
        Next
        Fin.Clear()
        Bench.Print("TeleportAgents")

    End Sub

End Module
