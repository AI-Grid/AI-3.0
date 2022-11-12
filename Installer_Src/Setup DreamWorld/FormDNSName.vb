#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.Net
Imports System.Text.RegularExpressions

Public Class FormDnsName

#Region "Private Fields"

    Dim initted As Boolean

#End Region

#Region "ScreenSize"

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

#Region "Load"

    Private Sub DNS_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        PictureBox.Visible = False
        EnableHypergrid.Text = Global.Outworldz.My.Resources.Enable_Hypergrid_word
        HelpToolStripMenuItem.Image = Global.Outworldz.My.Resources.question_and_answer
        HelpToolStripMenuItem.Text = Global.Outworldz.My.Resources.Help_word
        Label1.Text = Global.Outworldz.My.Resources.DynDNS_password_word
        Label2.Text = Global.Outworldz.My.Resources.DNSNameText
        Label3.Text = Global.Outworldz.My.Resources.DNSAltWords
        Me.Text = Global.Outworldz.My.Resources.Dynamic_DNS_word
        NextNameButton.Text = Global.Outworldz.My.Resources.Next_Name
        SaveButton1.Text = Global.Outworldz.My.Resources.Save_word
        SuitcaseCheckbox.Text = Global.Outworldz.My.Resources.Suitcase_enable
        TestButton1.Text = Global.Outworldz.My.Resources.Test_DNS_word
        Text = Global.Outworldz.My.Resources.DNS_HG_Name
        ToolTip1.SetToolTip(DNSAliasTextBox, Global.Outworldz.My.Resources.DNSAlt)
        ToolTip1.SetToolTip(DNSNameBox, Global.Outworldz.My.Resources.AlphaNum)
        ToolTip1.SetToolTip(EnableHypergrid, Global.Outworldz.My.Resources.Enable_Hypergrid_word)
        ToolTip1.SetToolTip(NextNameButton, Global.Outworldz.My.Resources.FreeName)
        ToolTip1.SetToolTip(SaveButton1, Global.Outworldz.My.Resources.Save_word)
        ToolTip1.SetToolTip(SuitcaseCheckbox, Global.Outworldz.My.Resources.Disable_Suitcase_txt)
        ToolTip1.SetToolTip(TestButton1, Global.Outworldz.My.Resources.Test_DNS_word)
        ToolTip1.SetToolTip(UniqueId, Global.Outworldz.My.Resources.Reserve_Password)

        SetScreen()

        If PropOpensimIsRunning Then
            UniqueId.ReadOnly = True
        Else
            UniqueId.ReadOnly = False
        End If

        DNSNameBox.Text = Settings.DnsName
        UniqueId.Text = Settings.MachineId()
        EnableHypergrid.Checked = Settings.EnableHypergrid
        SuitcaseCheckbox.Checked = Settings.Suitcase
        NextNameButton.Enabled = True
        DNSAliasTextBox.Text = Settings.AltDnsName
        HelpOnce("DNS")
        initted = True

    End Sub

#End Region

#Region "Buttons"

    Private Sub EnableHypergrid_CheckedChanged(sender As Object, e As EventArgs) Handles EnableHypergrid.CheckedChanged

        If Not initted Then Return
        Settings.EnableHypergrid = EnableHypergrid.Checked

    End Sub

    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.Click
        HelpManual("DNS")
    End Sub

    Private Sub NextNameButton_Click(sender As Object, e As EventArgs) Handles NextNameButton.Click


        NextNameButton.Text = Global.Outworldz.My.Resources.Busy_word
        DNSNameBox.Text = String.Empty
        Application.DoEvents()
        Dim newname = GetNewDnsName()
        NextNameButton.Text = Global.Outworldz.My.Resources.Next1
        If newname.Length = 0 Then
            MsgBox(My.Resources.Please_enter, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Info_word)
            NextNameButton.Enabled = False
        Else
            NextNameButton.Enabled = True
            DNSNameBox.Text = newname
        End If


    End Sub

    Private Sub SaveAll()


        NextNameButton.Text = Global.Outworldz.My.Resources.Saving_word

        Settings.SaveSettings()
        SetPublicIP()

        If UniqueId.Text.Length > 0 Then Me.Close()

    End Sub

    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton1.Click

        SaveAll()
        Close()

    End Sub

    Private Sub SuitcaseCheckbox_CheckedChanged(sender As Object, e As EventArgs) Handles SuitcaseCheckbox.CheckedChanged

        If Not initted Then Return
        Settings.Suitcase() = SuitcaseCheckbox.Checked

        If Not SuitcaseCheckbox.Checked Then
            MsgBox(My.Resources.Disabling_Suitcase)
        End If

    End Sub

    Private Sub TestButton1_Click(sender As Object, e As EventArgs) Handles TestButton1.Click

        PictureBox.Visible = True
        Application.DoEvents()
        NextNameButton.Text = Global.Outworldz.My.Resources.Busy_word

        Dim address As System.Net.IPAddress = Nothing
        If DNSNameBox.Text.Length = 0 Then
            Settings.PublicIP = WANIP()
        Else
            Settings.PublicIP = DNSNameBox.Text
            RegisterName(Settings.PublicIP)    ' force it to register

            Try
                If IPAddress.TryParse(DNSNameBox.Text, address) Then
                    Dim IP = GetHostAddresses(DNSNameBox.Text)
                    If IP.Length = 0 Then
                        PictureBox.Visible = False
                        MsgBox(My.Resources.Cannot_resolve_word & " " & DNSNameBox.Text, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Error_word)
                    Else
                        MsgBox(DNSNameBox.Text + " " & Global.Outworldz.My.Resources.resolved & " " & IP, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Info_word)
                    End If
                Else
                    Dim IP = GetHostAddresses(DNSNameBox.Text)
                    If IP.Length = 0 Then
                        PictureBox.Visible = False
                        MsgBox(My.Resources.Cannot_resolve_word & " " & DNSNameBox.Text, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Error_word)
                    Else
                        MsgBox(DNSNameBox.Text + " " & Global.Outworldz.My.Resources.resolved & " " & IP, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Info_word)
                    End If
                End If
            Catch ex As Exception
                BreakPoint.Dump(ex)
            End Try

            If DNSAliasTextBox.Text.Length > 0 Then
                Try

                    Dim array As String() = Settings.AltDnsName.Split(",".ToCharArray())
                    For Each part As String In array

                        RegisterName(part)
                        PictureBox.Visible = False
                        If IPAddress.TryParse(part, address) Then
                            MsgBox(Global.Outworldz.My.Resources.resolved & " " & part, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Info_word)
                        Else
                            Dim IP = GetHostAddresses(part)
                            If IP.Length = 0 Then
                                MsgBox(My.Resources.Cannot_resolve_word & " " & part, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Error_word)
                            Else
                                MsgBox(part + " " & Global.Outworldz.My.Resources.resolved & " " & IP, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Info_word)
                            End If
                        End If
                    Next
                Catch ex As Exception
                    BreakPoint.Dump(ex)
                End Try

            End If
        End If

        NextNameButton.Text = Global.Outworldz.My.Resources.Next1

        PictureBox.Visible = False

    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")>
    Private Sub TextBox1_LostFocus(sender As Object, e As EventArgs) Handles DNSNameBox.TextChanged

        If DNSNameBox.Text.Length > 0 Then

            DNSNameBox.Text = Regex.Replace(DNSNameBox.Text, ":\d+", "") ' no :8002 on end.

            Dim rgx As New Regex("[^a-zA-Z0-9\.\-]")
            DNSNameBox.Text = rgx.Replace(DNSNameBox.Text, "")

            DNSNameBox.Text = DNSNameBox.Text.ToLower(Globalization.CultureInfo.InvariantCulture)
            DNSNameBox.Text = DNSNameBox.Text.Replace("http://", "")
            DNSNameBox.Text = DNSNameBox.Text.Replace("https://", "")

        End If
        Settings.DnsName = DNSNameBox.Text

    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")>
    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles DNSAliasTextBox.LostFocus

        If Not initted Then Return

        Text = DNSAliasTextBox.Text.Replace("http://", "")
        DNSAliasTextBox.Text = DNSAliasTextBox.Text.Replace("https://", "")
        DNSAliasTextBox.Text = Regex.Replace(DNSAliasTextBox.Text, ":\d+", "") ' no :8002 on end.
        Dim rgx As New Regex("[^a-zA-Z0-9\.\-,]")
        DNSAliasTextBox.Text = rgx.Replace(DNSAliasTextBox.Text, "")
        DNSAliasTextBox.Text = DNSAliasTextBox.Text.ToLower(Globalization.CultureInfo.InvariantCulture)
        Settings.AltDnsName = DNSAliasTextBox.Text

    End Sub

    Private Sub UniqueId_TextChanged_1(sender As Object, e As EventArgs) Handles UniqueId.TextChanged

        If Not initted Then Return
        Settings.MachineId() = UniqueId.Text

    End Sub

#End Region

End Class
