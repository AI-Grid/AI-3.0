#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Namespace My

    ' The following events are available for MyApplication:
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed. This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active.
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication

#Region "Private Methods"

        'Private Sub AppStart(ByVal sender As Object,      ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
        'AddHandler AppDomain.CurrentDomain.AssemblyResolve, AddressOf ResolveAssemblies
        'End Sub

        ''' <summary>
        ''' UnhandledException: Raised if the application encounters an unhandled exception.
        ''' </summary>

        Private Sub MyApplication_UnhandledException(
                            ByVal sender As Object,
                    ByVal e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs
         ) Handles Me.UnhandledException

            Dim ex = e.Exception

            e.ExitApplication = False

            Dim Result As String
            Dim hr As Integer = Runtime.InteropServices.Marshal.GetHRForException(ex)
            Result = ex.GetType.ToString() & "(0x" & hr.ToString("X8", Globalization.CultureInfo.InvariantCulture) & "): " & ex.Message & Environment.NewLine & ex.StackTrace & Environment.NewLine
            Dim st As StackTrace = New StackTrace(ex, True)
            For Each sf As StackFrame In st.GetFrames
                If sf.GetFileLineNumber() > 0 Then
                    Result &= "Line:" & sf.GetFileLineNumber() & " Filename: " & IO.Path.GetFileName(sf.GetFileName) & Environment.NewLine
                End If
            Next

            Logger("Error", AssemblyV, "Error")
            Logger("Error", "Git Version: #" & GitVersion(), "Error")
            Logger("Error", "Version: " & PropMyVersion(), "Error")
            Logger("Error", Result, "Error")
            Logger("Error", DisplayObjectInfo(sender), "Error")

            Using Logform As New FormErrorLogger
                Logform.ShowDialog()
            End Using

        End Sub

        ''' <summary>
        ''' Sample of embedded DLL
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <returns></returns>
        'Private Function ResolveAssemblies(sender As Object, e As System.ResolveEventArgs) As Reflection.Assembly
        'Dim desiredAssembly = New Reflection.AssemblyName(e.Name)
        '   Breakpoint.Print("Loading Assembly " & desiredAssembly.Name)
        'If desiredAssembly.Name = "Ionic.Zip" Then
        'Return Nothing
        '     Return Reflection.Assembly.Load("") 'replace with your assembly's resource name
        'Else
        'Return Nothing
        'End If
        'End Function

    End Class

#End Region

End Namespace
