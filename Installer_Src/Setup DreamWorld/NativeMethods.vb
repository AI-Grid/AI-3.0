#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.Runtime.InteropServices

Friend Module NativeMethods

    '// Constants
    Const HWND_TOP As Integer = 0

    'Const HWND_TOPMOST As Integer = -1
    'Const HWND_NO_TOPMOST As Integer = -2
    Const NOMOVE As Long = &H2

    Const NOSIZE As Long = &H1

    <DllImport("ntdll.dll")>
    Public Function GetLastError() As Integer
    End Function

    <DllImport("ntdll.dll", PreserveSig:=False, SetLastError:=True)>
    Public Sub NtResumeProcess(ByVal ProcessHandle As IntPtr)
    End Sub

    <DllImport("ntdll.dll", SetLastError:=False)>
    Public Function NtSuspendProcess(ByVal ProcessHandle As IntPtr) As IntPtr
    End Function

    <DllImport("kernel32.dll")>
    Public Function OpenThread(ByVal dwDesiredAccess As ThreadAccess, ByVal bInheritHandle As Boolean, ByVal dwThreadId As UInteger) As IntPtr
    End Function

    <DllImport("kernel32.dll")>
    Public Function ResumeThread(ByVal hThread As IntPtr) As Integer
    End Function

    <DllImport("iphlpapi.dll", ExactSpelling:=True)>
    Public Function SendARP(ByVal DestIP As UInteger, ByVal SrcIP As UInteger, ByVal pMacAddr As Byte(), ByRef PhyAddrLen As UInteger) As Integer
    End Function

    <DllImport("user32.dll")>
    Public Function SetForegroundWindow(hWnd As IntPtr) As Boolean
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Unicode)>
    Public Function SetWindowText(ByVal hwnd As IntPtr, ByVal windowName As String) As Boolean
    End Function

    Public Declare Function ShowWindow Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal nCmdShow As SHOWWINDOWENUM) As Boolean

    Public Declare Function SetWindowPos Lib "user32" _
            (ByVal hWnd As Integer, ByVal hWndInsertAfter As Integer,
            ByVal x As Integer, ByVal y As Integer, ByVal cX As Integer,
            ByVal cY As Integer, ByVal wFlags_ As Integer) As Long

    Public Sub SetWindowOnTop(ByVal lhWnd As Int32)

        On Error GoTo SetWindowOnTop_Err

        SetWindowPos(lhWnd, HWND_TOP, 0, 0, 0, 0, NOMOVE Or NOSIZE)

SetWindowOnTop_Exit:
        Exit Sub

SetWindowOnTop_Err:
        Resume SetWindowOnTop_Exit

    End Sub

End Module
