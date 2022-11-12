Public Class FormSearch

#Region "ScreenSize"

    Private ReadOnly Handler As New EventHandler(AddressOf Resize_page)

    'The following detects  the location of the form in screen coordinates
    Private _screenPosition As ClassScreenpos

    Public Property ScreenPosition As ClassScreenpos
        Get
            Return _screenPosition
        End Get
        Set(value As ClassScreenpos)
            _screenPosition = value
        End Set
    End Property

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

    Private Sub FormSearch_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        OutworldzRadioButton.Text = Global.Outworldz.My.Resources.HypericaSearch_word
        JOpensimRadioButton.Text = Global.Outworldz.My.Resources.JOpensimSearch_word
        SearchBox.Text = Global.Outworldz.My.Resources.SearchOptions_word
        LocalButton.Text = Global.Outworldz.My.Resources.Local_Search

        Dim installed As Boolean = Joomla.IsjOpensimInstalled()
        If installed Then
            JOpensimRadioButton.Enabled = True
        Else
            JOpensimRadioButton.Enabled = False
            JOpensimRadioButton.Checked = False
        End If

        HelpOnce("Search Help")

        Select Case Settings.SearchOptions
            Case "None"
                NoneButton.Checked = True
            Case JOpensim
                LocalButton.Checked = True
            Case "Local"
                LocalButton.Checked = True
            Case Outworldz
                OutworldzRadioButton.Checked = True
        End Select
        SetScreen()

    End Sub

    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToolStripLabel1.Click

        HelpManual("Search Help")

    End Sub

    Private Sub LocalButton_CheckedChanged(sender As Object, e As EventArgs) Handles LocalButton.CheckedChanged

        If LocalButton.Checked Then
            Settings.SearchOptions = "Local"
            Settings.SaveSettings()
        End If

    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles OutworldzRadioButton.CheckedChanged

        If OutworldzRadioButton.Checked Then
            Settings.SearchOptions = Outworldz
            Settings.SaveSettings()
        End If

    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles JOpensimRadioButton.CheckedChanged

        If JOpensimRadioButton.Checked Then
            Settings.SearchOptions = JOpensim
            Settings.SaveSettings()
        End If

    End Sub

    Private Sub RadioButton2_CheckedChanged_1(sender As Object, e As EventArgs) Handles NoneButton.CheckedChanged

        If NoneButton.Checked Then
            Settings.SearchOptions = "None"
            Settings.SaveSettings()
        End If

    End Sub

End Class