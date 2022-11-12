Imports System.Net.NetworkInformation

Public Class ClassLoopback
    Implements IDisposable

    Public Shared Sub SetLoopback()

        Dim Adapters = NetworkInterface.GetAllNetworkInterfaces()
        For Each adapter As NetworkInterface In Adapters

            If adapter.Name = "Loopback" Then
                TextPrint(My.Resources.Setting_Loopback)
                Using LoopbackProcess As New Process
                    LoopbackProcess.StartInfo.UseShellExecute = True ' so we can redirect streams
                    LoopbackProcess.StartInfo.FileName = IO.Path.Combine(Settings.CurrentDirectory, "NAT_Loopback_Tool.bat")
                    LoopbackProcess.StartInfo.CreateNoWindow = True
                    LoopbackProcess.StartInfo.Arguments = "Loopback"
                    LoopbackProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                    Try
                        LoopbackProcess.Start()
                    Catch ex As Exception
                        BreakPoint.Dump(ex)
                    End Try
                End Using
            End If
        Next

    End Sub

#Region "IDisposable Support"

    Private disposedValue As Boolean ' To detect redundant calls

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code. Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)

        GC.SuppressFinalize(Me)
    End Sub

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then

            End If
        End If
        disposedValue = True
    End Sub

#End Region

End Class
