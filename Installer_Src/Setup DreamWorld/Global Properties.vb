Imports System.Collections.Concurrent
Imports System.IO
Imports System.Threading
Imports IniParser.Model

Public Class AvatarObject
    Public AgentName As String
    Public AvatarUUID As String
    Public FirstName As String
    Public LastName As String
    Public RegionID As String
End Class

Module Global_Properties

#Region "Globals"

    Public ReadOnly LogResults As New Dictionary(Of String, LogReader)
    Public _Data As IniParser.Model.IniData
    Public BackupAbort As Boolean
    Public Bench As New Benchmark()
    Public CachedAvatars As New List(Of AvatarObject)
    Public gEstateName As String = ""
    Public gEstateOwner As String = ""
    Public MapX As Integer = 100
    Public MapY As Integer = 100
    Public RunningBackupName As New ConcurrentDictionary(Of String, String)

#End Region

#Region "Private"

    Private _IsRunning As Boolean
    Private _mySetting As MySettings
    Private _PropAborting As Boolean
    Private _RegionFilesChanged As Boolean
    Private _SelectedBox As String = ""
    Private _SkipSetup As Boolean = True
    Private _UpdateView As Boolean = True
    Private _XYINI As String ' global XY INI

#End Region

#Region "Subs"

    Private TextLock As New Object

    Public Sub Sleep(value As Integer)
        ''' <summary>Sleep(ms)</summary>
        ''' <param name="value">millseconds</param>
        ' value is in milliseconds, but we do it in 10 passes a second so we can doevents() to free up console
        Dim slices As Integer = 10

        While value > 0
            Application.DoEvents()
            Dim t = CInt(1000 / slices)
            Thread.Sleep(t)
            value -= t
        End While

    End Sub

    Public Sub TextPrint(Value As String)

        SyncLock TextLock
            Log(My.Resources.Info_word, Value)
            Dim dt = Date.Now.ToString(Globalization.CultureInfo.CurrentCulture)
            If Settings.ShowDateandTimeinLogs Then
                FormSetup.TextBox1.Text += $"{dt} {Value}{vbCrLf}"
                Log(My.Resources.Info_word, $"{dt} {Value}{vbCrLf}")
            Else
                FormSetup.TextBox1.Text += $"{Value}{vbCrLf}"
                Log(My.Resources.Info_word, $"{dt} {Value}{vbCrLf}")
            End If

            Dim ln As Integer = FormSetup.TextBox1.Text.Length
            FormSetup.TextBox1.SelectionStart = ln
            'FormSetup.TextBox1.ScrollToCaret()
            Dim Le As Integer = 29000
            Dim L = FormSetup.TextBox1.Text.Length - Le
            If L > 0 Then
                FormSetup.TextBox1.Text = FormSetup.TextBox1.Text.Substring(FormSetup.TextBox1.Text.Length - Le, Le)
            End If
            FormSetup.TextBox1.SelectionStart = FormSetup.TextBox1.Text.Length
            FormSetup.TextBox1.ScrollToCaret()

        End SyncLock

    End Sub

#End Region

#Region "Classes and Enums"

    Public Class PRIEnumClass
        Public AboveNormal As ProcessPriorityClass = ProcessPriorityClass.AboveNormal
        Public BelowNormal As ProcessPriorityClass = ProcessPriorityClass.BelowNormal
        Public High As ProcessPriorityClass = ProcessPriorityClass.High
        Public Normal As ProcessPriorityClass = ProcessPriorityClass.Normal
        Public RealTime As ProcessPriorityClass = ProcessPriorityClass.RealTime
    End Class

#End Region

#Region "Properties"

    Public ReadOnly Property GitVersion As String
        ' output of  git rev-parse --short HEAD   from Perl
        Get
            Dim line As String = "None"
            Dim fname = IO.Path.Combine(Settings.CurrentDirectory, "GitVersion")
            If System.IO.File.Exists(fname) Then
                Using reader As StreamReader = System.IO.File.OpenText(fname)
                    'now loop through each line
                    While reader.Peek <> -1
                        line = reader.ReadLine()
                    End While
                End Using
            End If
            Return line
        End Get

    End Property

    Public Property PropAborting() As Boolean
        Get
            Return _PropAborting
        End Get
        Set(ByVal Value As Boolean)
            _PropAborting = Value
        End Set
    End Property

    ''' <summary>
    ''' Set when the RegionList should be refreshed
    ''' </summary>
    ''' <returns>true if it need refresh</returns>
    Public Property PropChangedRegionSettings As Boolean
        Get
            Return _RegionFilesChanged
        End Get
        Set(value As Boolean)
            _RegionFilesChanged = value
        End Set
    End Property

    Public ReadOnly Property PropDomain As String
        Get
            Return _Domain
        End Get

    End Property

    Public ReadOnly Property PropHttpsDomain As String
        Get
            Return _httpsDomain
        End Get

    End Property

    Public ReadOnly Property PropMyVersion As String
        Get
            Return _MyVersion
        End Get
    End Property

    ''' <summary>
    ''' Property set if Opensim when supposed to be running
    ''' </summary>
    ''' <returns>True if running</returns>
    Public Property PropOpensimIsRunning() As Boolean
        Get
            Return _IsRunning
        End Get
        Set(ByVal Value As Boolean)
            _IsRunning = Value
        End Set
    End Property

    Public Property PropSelectedBox As String
        Get
            Return _SelectedBox
        End Get
        Set(value As String)
            _SelectedBox = value
        End Set
    End Property

    Public ReadOnly Property PropSimVersion As String
        Get
            Return _SimVersion
        End Get
    End Property

    Public Property PropUpdateView() As Boolean
        Get
            Return _UpdateView
        End Get
        Set(ByVal Value As Boolean)
            'If Debugger.IsAttached Then Breakpoint.Print("View refresh")
            _UpdateView = Value
        End Set
    End Property

    Public Property Settings As MySettings
        Get
            Return _mySetting
        End Get
        Set(value As MySettings)
            _mySetting = value
        End Set
    End Property

    Public Property SkipSetup() As Boolean
        Get
            Return _SkipSetup
        End Get
        Set(ByVal Value As Boolean)
            _SkipSetup = Value
        End Set
    End Property

    Public Property XYData As IniData
        Get
            Return _Data
        End Get
        Set(value As IniData)
            _Data = value
        End Set
    End Property

    Public Property XYINI As String
        Get
            Return _XYINI
        End Get
        Set(value As String)
            _XYINI = value
        End Set
    End Property

#End Region

End Module
