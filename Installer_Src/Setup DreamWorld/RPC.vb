#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports Nwc.XmlRpc

Module RPC
    Public ReadOnly SavedAlready As New List(Of String)

    ''' <summary>
    ''' Returns count of ALL agents + NPC in region
    ''' </summary>
    ''' <param name="RegionUUID"></param>
    ''' <returns>Object</returns>
    '''
    'http://opensimulator.org/wiki/Remoteadmin:admin_get_agents
    Public Function Admin_get_agents(RegionUUID As String) As Integer

        If Not RegionStatus(RegionUUID) = SIMSTATUSENUM.Booted Then
            Return 0
        End If

        Dim ht = New Hashtable From {
           {"password", Settings.MachineId},
           {"region_id", RegionUUID}
        }
        Dim o As Object = GetRPCAsObject(RegionUUID, "admin_get_agents", ht)
        If o Is Nothing Then Return 0

        Dim regions As Object = o.Root
        Dim c As Integer
        For Each agent As Object In CType(regions, IEnumerable(Of Object))
            For Each agents As Object In CType(agent, IEnumerable(Of Object))
                c += 1
            Next
        Next
        Return c

    End Function

    Public Function GetRPCAsObject(FromRegionUUID As String, cmd As String, ht As Hashtable) As Object

        If Not RegionStatus(FromRegionUUID) = SIMSTATUSENUM.Booted Then
            Return Nothing
        End If

        Dim RegionPort = GroupPort(FromRegionUUID)
        Dim url = $"http://{Settings.LANIP}:{RegionPort}"

        Dim parameters = New List(Of Hashtable) From {ht}
        Try
            Dim RPC = New XmlRpcRequest(cmd, parameters)
            Return RPC.Send(url, 2000).Value
        Catch ex As Exception
        End Try
        Return Nothing

    End Function

    Public Sub ReallyShutDown(RegionUUID As String, nextstate As SIMSTATUSENUM)

        Thaw(RegionUUID)
        RPC_Region_Command(RegionUUID, "quit")

        Dim Group = Group_Name(RegionUUID)

        For Each RegionUUID In RegionUuidListByName(Group)
            RegionStatus(RegionUUID) = nextstate
        Next

    End Sub

    ''TODO logoff_user

    Public Function RPC_admin_dialog(agentId As String, text As String) As Boolean

        Dim RegionUUID As String = GetRegionFromAgentId(agentId)
        Dim ht = New Hashtable From {
           {"password", Settings.MachineId},
           {"message", text}
        }
        Return SendRPC(RegionUUID, "admin_dialog", ht)

    End Function

    Public Function RPC_admin_get_agent_list(RegionUUID As String) As List(Of AvatarData)

        Dim result As New List(Of AvatarData)

        If Not RegionStatus(RegionUUID) = SIMSTATUSENUM.Booted Then
            Return result
        End If

        Dim ht = New Hashtable From {
           {"password", Settings.MachineId},
           {"region_id", RegionUUID}
        }

        Try
            Dim o = GetRPCAsObject(RegionUUID, "admin_get_agents", ht)
            If o Is Nothing Then Return result
            Dim data As Hashtable = CType(o, Hashtable)

            If data.Item("success") <> True Then Return result

            Dim regions As ArrayList = CType(data.Item("regions"), ArrayList)
            For Each region In regions
                'Dim name = region.item("name")
                Dim agents = region.item("agents")
                Dim ag As ArrayList = CType(agents, ArrayList)
                For Each agent In ag
                    If agent.item("type") = "User" Then
                        Dim avi = New AvatarData With {
                            .AvatarName = CStr(agent.Item("name")),
                            .X = CSng(agent.Item("pos_x")),
                            .Y = CSng(agent.item("pos_y"))
                        }
                        result.Add(avi)
                    End If
                Next
            Next
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try
        Return result

    End Function

    Public Function RPC_Region_Command(RegionUUID As String, Message As String) As Boolean

        Dim ht = New Hashtable From {
           {"password", Settings.MachineId},
           {"command", Message}
        }
        Debug.Print($"admin_console_command {Message}")
        Return SendRPC(RegionUUID, "admin_console_command", ht)

    End Function

    Public Function RPC_Save_OAR(RegionUUID As String, Filename As String, Region_Name As String, Optional timeout As Integer = 2000) As Boolean

        If Not RegionStatus(RegionUUID) = SIMSTATUSENUM.Booted Then
            Return False
        End If

        Dim ht = New Hashtable From {
           {"password", Settings.MachineId},
           {"filename", Filename},
           {"region_name", Region_Name}
        }
        Debug.Print($"admin_save_oar {Region_Name}")
        Application.DoEvents()
        Dim status = SendRPC(RegionUUID, "admin_save_oar", ht, timeout)

        Return status

    End Function

    Public Function SendAdminMessage(RegionUUID As String, Message As String) As Boolean

        If Not RegionStatus(RegionUUID) = SIMSTATUSENUM.Booted Then
            Return False
        End If

        'http://opensimulator.org/wiki/RemoteAdmin:admin_dialog

        Dim ht = New Hashtable From {
           {"password", Settings.MachineId},
           {"message", Message}
        }
        Log("Info", "Message to " & Region_Name(RegionUUID) & " of " & Message)
        Return SendRPC(RegionUUID, "admin_dialog", ht)

    End Function

    Public Function SendMessage(RegionUUID As String, Message As String) As Boolean

        If Not RegionStatus(RegionUUID) = SIMSTATUSENUM.Booted Then
            Return False
        End If

        'http://opensimulator.org/wiki/RemoteAdmin:admin_broadcast

        Dim ht = New Hashtable From {
           {"password", Settings.MachineId},
           {"message", Message}
       }
        Log("Info", "Message to " & Region_Name(RegionUUID) & " of " & Message)
        Return SendRPC(RegionUUID, "admin_broadcast", ht)

    End Function

    Public Sub ShutDown(RegionUUID As String, nextstate As SIMSTATUSENUM)

        Thaw(RegionUUID)
        ConsoleCommand(RegionUUID, "q")

        Dim Group = Group_Name(RegionUUID)

        For Each RegionUUID In RegionUuidListByName(Group)
            RegionStatus(RegionUUID) = nextstate
        Next

    End Sub

    Public Function TeleportTo(FromRegionUUID As String, ToRegionName As String, AgentID As String) As Boolean

        'http://opensimulator.org/wiki/Remoteadmin:admin_teleport_agent

        Debug.Print("Teleport To:" & ToRegionName)

        Dim ht = New Hashtable From {
            {"password", Settings.MachineId},
            {"region_name", ToRegionName},
            {"agent_id", AgentID}
        }

        If FromRegionUUID.Length > 0 Then
            Dim Status = SendRPC(FromRegionUUID, "admin_teleport_agent", ht)
            Return Status
        End If
        Return False

    End Function

    Private Function SendRPC(RegionUUID As String, cmd As String, ht As Hashtable, Optional Timeout As Integer = 1000) As Boolean

        If RegionUUID.Length = 0 Then Return False

        Dim RegionPort = GroupPort(RegionUUID)
        Dim url = $"http://{Settings.LANIP}:{RegionPort}"

        Dim parameters = New List(Of Hashtable) From {ht}
        Dim RPC = New XmlRpcRequest(cmd, parameters)

        Try
            Dim r = RPC.Send(url, Timeout)
            If r.Value Is Nothing Then
                Return True
            End If
#Disable Warning BC42016 ' Implicit conversion

            For Each s In r.Value
                'Log("Info", s.Key & ":" & s.Value)
                If s.Key = "saved" AndAlso s.Value = "True" Then
                    Debug.Print("Oar Saved")
                    Return True
                End If
                If s.Key = "success" AndAlso s.Value = "True" Then
                    Debug.Print($"{cmd} Sent")
                    Return True
                End If
                If s.Key = "faultCode" Then
                    Debug.Print($"Fault: {CStr(s.Value)}")
                End If
                If s.Key = "faultString" Then
                    Debug.Print($"Fault: {CStr(s.Value)}")
                End If
            Next
#Enable Warning BC42016 ' Implicit conversion
        Catch ex As Exception
            Return False
        End Try
        Return True

    End Function

    Public Class AvatarData

        Public AvatarName As String
        Public X As Single
        Public Y As Single

    End Class

End Module
