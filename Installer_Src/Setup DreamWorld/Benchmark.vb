Public Class Benchmark

    Private stash As New Dictionary(Of String, Stopwatch)

    Public Sub Print(Name As String)

        If Not Settings.LogBenchmarks Then Return
        If Not stash.ContainsKey(Name) Then
            Return
        End If

        If Settings.LogBenchmarks Then
            Logger("Benchmark", $"{Name}:{CStr(stash.Item(Name).Elapsed.TotalMilliseconds / 1000)} {My.Resources.Seconds_word}", "Benchmark")
        End If
        stash.Remove(Name)

    End Sub

    Public Sub Start(name As String)

        If Not Settings.LogBenchmarks Then Return
        If stash.ContainsKey(name) Then
            Return
        End If
        stash.Add(name, New Stopwatch())
        stash.Item(name).Start()

    End Sub

End Class
