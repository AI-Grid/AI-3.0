#Region "Copyright"

' Copyright 2014 Fred Beckhusen for www.Outworldz.com https://opensource.org/licenses/AGPL

'Permission Is hereby granted, free Of charge, to any person obtaining a copy of this software
' And associated documentation files (the "Software"), to deal in the Software without restriction,
'including without limitation the rights To use, copy, modify, merge, publish, distribute, sublicense,
'And/Or sell copies Of the Software, And To permit persons To whom the Software Is furnished To
'Do so, subject To the following conditions:

'The above copyright notice And this permission notice shall be included In all copies Or '
'substantial portions Of the Software.

'THE SOFTWARE Is PROVIDED "AS IS", WITHOUT WARRANTY Of ANY KIND, EXPRESS Or IMPLIED,
' INCLUDING BUT Not LIMITED To THE WARRANTIES Of MERCHANTABILITY, FITNESS For A PARTICULAR
'PURPOSE And NONINFRINGEMENT.In NO Event SHALL THE AUTHORS Or COPYRIGHT HOLDERS BE LIABLE
'For ANY CLAIM, DAMAGES Or OTHER LIABILITY, WHETHER In AN ACTION Of CONTRACT, TORT Or
'OTHERWISE, ARISING FROM, OUT Of Or In CONNECTION With THE SOFTWARE Or THE USE Or OTHER
'DEALINGS IN THE SOFTWARE.

#End Region

Imports System.IO

Public Class ClrCache

#Region "Public Constructors"

    Public Sub New()
    End Sub

#End Region

#Region "Protected Destructors"

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

#End Region

#Region "Public Methods"

    Public Shared Sub WipeAssets()

        If Form1.PropOpensimIsRunning Then
            For Each regionnum In Form1.PropRegionClass.RegionNumbers
                If Form1.PropRegionClass.IsBooted(regionnum) Then
                    Dim Box = Form1.PropRegionClass.GroupName(regionnum)
                    Form1.ConsoleCommand(Box, "fcache clear{ENTER}")
                End If
            Next
            Return
        End If

        Form1.Print(My.Resources.Clearing_Assets)
        Dim folders() = Directory.GetDirectories(Form1.PropOpensimBinPath & "bin\Assetcache\", "*", SearchOption.AllDirectories)
        Dim ctr As Integer = 0
        For Each folder As String In folders
            FileStuff.DeleteDirectory(folder, FileIO.DeleteDirectoryOption.DeleteAllContents)
            ctr += 1
            If ctr Mod 100 = 0 Then Form1.Print(My.Resources.Deleted & " " & CStr(ctr))
            Application.DoEvents()
        Next

    End Sub

    Public Shared Sub WipeBakes()

        Form1.Print(My.Resources.Clearing_Bake)
        FileStuff.DeleteDirectory(Form1.PropOpensimBinPath & "bin\bakes\", FileIO.DeleteDirectoryOption.DeleteAllContents)

    End Sub

    Public Shared Sub WipeImage()

        Form1.Print(My.Resources.Clearing_Image)
        Dim folders() = Nothing
        Try
            folders = IO.Directory.GetDirectories(Form1.PropOpensimBinPath & "bin\j2kDecodeCache\")
        Catch ex As UnauthorizedAccessException
        Catch ex As ArgumentNullException
        Catch ex As ArgumentException
        Catch ex As PathTooLongException
        Catch ex As IOException
        End Try

        Dim ctr = 0
        For Each folder As String In folders
            FileStuff.DeleteDirectory(folder, FileIO.DeleteDirectoryOption.DeleteAllContents)
            ctr += 1
            If ctr Mod 100 = 0 Then Form1.Print(My.Resources.Deleted & " " & CStr(ctr))
            Application.DoEvents()
        Next

    End Sub

    Public Shared Sub WipeMesh()

        Form1.Print(My.Resources.Clearing_Mesh)
        Dim fCount As Integer

        Dim folders As Array = Nothing
        Try
            fCount = Directory.GetFiles(Form1.PropOpensimBinPath & "bin\MeshCache\", "*", SearchOption.AllDirectories).Length
        Catch ex As ArgumentException
        Catch ex As UnauthorizedAccessException
        Catch ex As DirectoryNotFoundException
        Catch ex As PathTooLongException
        Catch ex As IOException
        End Try

        Try
            folders = Directory.GetFiles(Form1.PropOpensimBinPath & "bin\MeshCache\", "*", SearchOption.AllDirectories)
        Catch ex As ArgumentException
        Catch ex As UnauthorizedAccessException
        Catch ex As DirectoryNotFoundException
        Catch ex As PathTooLongException
        Catch ex As IOException
        End Try

        Dim ctr As Integer = 0
        For Each folder As String In folders
            FileStuff.DeleteDirectory(Form1.PropOpensimBinPath & "bin\MeshCache\", FileIO.DeleteDirectoryOption.DeleteAllContents)
        Next

    End Sub

    Public Shared Sub WipeScripts()

        If Not Form1.PropOpensimIsRunning() Then
            Dim folders() = Directory.GetFiles(Form1.PropOpensimBinPath & "bin\ScriptEngines\", "*", SearchOption.AllDirectories)
            Form1.Print(My.Resources.Clearing_Script)
            Dim ctr As Integer = 0
            For Each script As String In folders
                Dim ext = Path.GetExtension(script)
                If ext.ToLower(Form1.Invarient) <> ".state" And ext.ToLower(Form1.Invarient) <> ".keep" Then
                    FileStuff.DeleteFile(script)
                    ctr += 1
                    If ctr Mod 100 = 0 Then
                        Form1.Print(My.Resources.Updated & " " & CStr(ctr) & " scripts")
                    End If
                    Application.DoEvents()
                End If
            Next
        End If

    End Sub

#End Region

End Class
