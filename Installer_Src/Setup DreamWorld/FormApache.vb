#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.Text.RegularExpressions

Public Class FormApache

#Region "Private Fields"

    Private Const DreamGrid As String = "DreamGrid"
    Private Const JOpensim As String = "JOpensim"
    Private Const WordPress As String = "WordPress"
    Dim initted As Boolean
#Disable Warning CA2213
    Dim SSL As New FormSSL
#Enable Warning CA2213

#End Region

#Region "FormPos"

    Private ReadOnly Handler As New EventHandler(AddressOf Resize_page)
    Private _screenPosition As ClassScreenpos

    Public Property ScreenPosition As ClassScreenpos
        Get
            Return _screenPosition
        End Get
        Set(value As ClassScreenpos)
            _screenPosition = value
        End Set
    End Property

    'The following detects  the location of the form in screen coordinates
    Private Sub Resize_page(ByVal sender As Object, ByVal e As System.EventArgs)
        'Me.Text = "Form screen position = " + Me.Location.ToString
        ScreenPosition.SaveXY(Me.Left, Me.Top)
    End Sub

    Private Sub SetScreen()

        ScreenPosition = New ClassScreenpos(Me.Name)
        AddHandler ResizeEnd, Handler
        Dim xy As List(Of Integer) = ScreenPosition.GetXY()
        Me.Left = xy.Item(0)
        Me.Top = xy.Item(1)
    End Sub

#End Region

#Region "Start/Stop"

    Private Sub Close_form(sender As Object, e As EventArgs) Handles MyBase.Closed

        Settings.SaveSettings()
        SSL.Dispose()

    End Sub

    Private Sub Loaded(sender As Object, e As EventArgs) Handles MyBase.Load

        ApacheCheckbox.Text = Global.Outworldz.My.Resources.EnableApache
        EnableDiva.Text = Global.Outworldz.My.Resources.EnableDiva
        EnableJOpensim.Text = Global.Outworldz.My.Resources.JOpensim_word
        EnableOther.Text = Global.Outworldz.My.Resources.EnableOther_Word
        EnableWP.Text = My.Resources.WordPress_Word
        GroupBox2.Text = My.Resources.Apache_word
        GroupBox3.Text = My.Resources.Content_Manager_Word
        HelpToolStripMenuItem.Image = Global.Outworldz.My.Resources.question_and_answer
        HelpToolStripMenuItem.Text = Global.Outworldz.My.Resources.Help_word
        Label3.Text = My.Resources.Web_Port
        Text = My.Resources.Apache_word
        Sitemap.Text = Global.Outworldz.My.Resources.Automatic_Site_map_word

        'Tool tips
        ToolTip1.SetToolTip(Me.ApachePort, Global.Outworldz.My.Resources.Resources.tt_Apache_Port)
        ToolTip1.SetToolTip(Me.ApacheCheckbox, Global.Outworldz.My.Resources.Resources.tt_Apache_Enable)
        ToolTip1.SetToolTip(Me.Sitemap, Global.Outworldz.My.Resources.Resources.tt_Apache_Sitemap)
        ToolTip1.SetToolTip(Me.EnableJOpensim, Global.Outworldz.My.Resources.Resources.tt_Apache_EnableJOpensim)
        ToolTip1.SetToolTip(Me.EnableDiva, Global.Outworldz.My.Resources.Resources.tt_Apache_EnableDiva)
        ToolTip1.SetToolTip(Me.EnableWP, Global.Outworldz.My.Resources.Resources.tt_Apache_EnableWP)
        ToolTip1.SetToolTip(Me.EnableOther, Global.Outworldz.My.Resources.Resources.tt_Apache_EnableOther)
        ToolTip1.SetToolTip(Me.Other, Global.Outworldz.My.Resources.Resources.tt_Apache_Other)

        SetScreen()

        ApacheCheckbox.Checked = Settings.ApacheEnable
        ApachePort.Text = CType(Settings.ApachePort, String)

        '''' set the other box and the radios for Different CMS systems.
        ''' This is used to redirect all access to apache / to the folder listed below
        '''
        Other.Text = Settings.OtherCMS

        If Settings.CMS = DreamGrid Then
            EnableDiva.Checked = True
        ElseIf Settings.CMS = WordPress Then
            EnableWP.Checked = True
        ElseIf Settings.CMS = JOpensim Then
            EnableJOpensim.Checked = True
        Else
            EnableOther.Checked = True
        End If

        Sitemap.Checked = Settings.SiteMap

        HelpOnce("Apache")
        initted = True

    End Sub

#End Region

#Region "Clickers"

    Private Shared Sub RemoveApache()

        Using ApacheProcess As New Process()
            ApacheProcess.StartInfo.FileName = "sc"
            ApacheProcess.StartInfo.Arguments = "stop " & "ApacheHTTPServer"
            ApacheProcess.StartInfo.CreateNoWindow = True
            Try
                ApacheProcess.Start()
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
            Application.DoEvents()
            ApacheProcess.WaitForExit()
            Sleep(1000)
            ApacheProcess.StartInfo.Arguments = " delete  " & "ApacheHTTPServer"
            ApacheProcess.StartInfo.CreateNoWindow = True
            Try
                ApacheProcess.Start()
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try
            Application.DoEvents()
            ApacheProcess.WaitForExit()
            TextPrint(My.Resources.Apache_has_been_removed)
        End Using

    End Sub

    Private Sub ApacheCheckbox_CheckedChanged(sender As Object, e As EventArgs) Handles ApacheCheckbox.CheckedChanged

        If Not initted Then Return
        Settings.ApacheEnable = ApacheCheckbox.Checked

    End Sub

    Private Sub ApachePort_TextChanged(sender As Object, e As EventArgs) Handles ApachePort.TextChanged

        If Not initted Then Return

        Dim digitsOnly = New Regex("[^\d]")
        ApachePort.Text = digitsOnly.Replace(ApachePort.Text, "")
        If ApachePort.Text.Length > 0 Then
            Settings.ApachePort = CType(ApachePort.Text, Integer)
        End If
        If Settings.ApachePort <> 80 Then
            SSLButton.Enabled = False
        Else
            SSLButton.Enabled = True
        End If
    End Sub

    Private Sub EnableDiva_CheckedChanged(sender As Object, e As EventArgs) Handles EnableDiva.CheckedChanged

        If Not initted Then Return
        If EnableDiva.Checked Then Settings.CMS = DreamGrid

    End Sub

    Private Sub EnableJOpensim_CheckedChanged(sender As Object, e As EventArgs) Handles EnableJOpensim.CheckedChanged

        If Not initted Then Return

        If Not EnableJOpensim.Checked Then Return

        Dim installed As Boolean = Joomla.IsjOpensimInstalled()

        If Not installed Then
            MsgBox("That folder has no content. Install Joomla and JOpensim first, then enable this.", vbInformation Or MsgBoxStyle.MsgBoxSetForeground)
            EnableDiva.Checked = True
            Return
        End If

        If EnableJOpensim.Checked Then Settings.CMS = JOpensim

    End Sub

    Private Sub EnableOther_CheckedChanged(sender As Object, e As EventArgs) Handles EnableOther.CheckedChanged

        If Not initted Then Return
        If Not EnableOther.Checked Then Return

        Dim Exist As Boolean
        Try
            If Other.Text.Length > 0 Then
                Dim folders() = IO.Directory.GetFiles(IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Apache\htdocs\" & Other.Text))
                If folders.Length > 1 Then
                    Exist = True
                End If
            End If
        Catch
        End Try

        If Not Exist Then
            MsgBox("That folder has no content. Install a CMS and then enable 'Other'.", vbInformation Or MsgBoxStyle.MsgBoxSetForeground)
            EnableDiva.Checked = True
            Return
        End If

        If EnableOther.Checked Then
            Settings.CMS = Other.Text
        End If

    End Sub

    Private Sub EnableWP_CheckedChanged(sender As Object, e As EventArgs) Handles EnableWP.CheckedChanged

        If Not initted Then Return
        If Not EnableWP.Checked Then Return

        Dim Exist As Boolean
        Try
            Dim folders() = IO.Directory.GetFiles(IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Apache\htdocs\WordPress"))
            If folders.Length > 1 Then
                Exist = True
            End If
        Catch
        End Try

        If Not Exist Then
            MsgBox("That folder has no content. Install WordPress then enable WordPress.", vbInformation Or MsgBoxStyle.MsgBoxSetForeground)
            EnableDiva.Checked = True
            Return
        End If

        If EnableWP.Checked Then Settings.CMS = WordPress

    End Sub

    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.Click
        HelpManual("Apache")
    End Sub

    Private Sub Other_TextChanged(sender As Object, e As EventArgs) Handles Other.TextChanged

        If Not initted Then Return
        Settings.CMS = Other.Text

    End Sub

    Private Sub Sitemap_CheckedChanged(sender As Object, e As EventArgs) Handles Sitemap.CheckedChanged

        If Not initted Then Return
        Settings.SiteMap = Sitemap.Checked

    End Sub

    Private Sub SSLButton_Click(sender As Object, e As EventArgs) Handles SSLButton.Click

        SSL.Dispose()
        SSL = New FormSSL
        SSL.Activate()
        SSL.Visible = True
        SSL.Select()
        SSL.BringToFront()

    End Sub

#End Region

End Class
