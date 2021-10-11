﻿Module Services

    Public Function ServiceExists(name As String) As Boolean

        Using ServiceProcess As New Process()
            ServiceProcess.StartInfo.RedirectStandardOutput = True
            ServiceProcess.StartInfo.RedirectStandardError = True
            ServiceProcess.StartInfo.RedirectStandardInput = True
            ServiceProcess.StartInfo.UseShellExecute = False
            ServiceProcess.StartInfo.FileName = "sc.exe"
            ServiceProcess.StartInfo.Arguments = "query " & name
            ServiceProcess.StartInfo.CreateNoWindow = True
            ServiceProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden

            Dim console As String = ""
            Try
                ServiceProcess.Start()
                console = ServiceProcess.StandardOutput.ReadToEnd()
                ServiceProcess.WaitForExit()
            Catch ex As Exception
                BreakPoint.Show(ex.Message)
            End Try
            If console.Contains("does not exist") Then Return False
            Return True

        End Using

    End Function

    Public Function StopMysql() As Boolean

        If Not MysqlInterface.IsMySqlRunning() Then
            Application.DoEvents()
            MySQLIcon(False)
            Return True
        End If

        PropAborting = True

        TextPrint($"MySQL {Global.Outworldz.My.Resources.Stopping_word}")

        If Settings.MysqlRunasaService Then

            Using MysqlProcess As New Process With {
                       .EnableRaisingEvents = False
                   }
                MysqlProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                MysqlProcess.StartInfo.FileName = "net"
                MysqlProcess.StartInfo.Arguments = "stop MySQLDreamGrid"
                MysqlProcess.StartInfo.UseShellExecute = False
                MysqlProcess.StartInfo.CreateNoWindow = True
                MysqlProcess.StartInfo.RedirectStandardError = True
                MysqlProcess.StartInfo.RedirectStandardOutput = True
                Dim response As String = ""

                Try
                    MysqlProcess.Start()
                    response = MysqlProcess.StandardOutput.ReadToEnd() & MysqlProcess.StandardError.ReadToEnd()
                    MysqlProcess.WaitForExit()
                Catch ex As Exception
                    BreakPoint.Show(ex.Message)
                End Try
                Application.DoEvents()

                If MysqlProcess.ExitCode <> 0 Then
                    TextPrint(My.Resources.Error_word & ":" & CStr(MysqlProcess.ExitCode) & ":" & response)
                    MsgBox(My.Resources.MySQLDidNotStop, vbCritical Or vbMsgBoxSetForeground)
                    MySQLIcon(True)
                    Return False
                Else
                    TextPrint(My.Resources.Mysql_Word & " " & My.Resources.Stopped_word)
                    MySQLIcon(False)
                    Return True
                End If
            End Using

        End If

        Using p As Process = New Process()
            Dim pi As ProcessStartInfo = New ProcessStartInfo With {
                .Arguments = "--port " & CStr(Settings.MySqlRobustDBPort) & " -u root shutdown",
                .FileName = """" & IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\mysql\bin\mysqladmin.exe") & """",
                .UseShellExecute = True, ' so we can redirect streams and minimize
                .WindowStyle = ProcessWindowStyle.Hidden
            }
            p.StartInfo = pi
            Try
                p.Start()
                Application.DoEvents()
                p.WaitForExit()
            Catch
            End Try
        End Using
        MySQLIcon(False)
        If MysqlInterface.IsMySqlRunning() Then
            MySQLIcon(True)
            MsgBox(My.Resources.MySQLDidNotStop, vbCritical Or vbMsgBoxSetForeground)
            Return False
        End If
        Return True

    End Function

End Module
