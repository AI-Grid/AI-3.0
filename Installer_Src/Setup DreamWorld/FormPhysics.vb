#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Public Class FormPhysics

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

#Region "Private Methods"

    Private Sub IsClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
        Settings.SaveSettings()
    End Sub

    Private Sub Physics_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        GroupBox1.Text = Global.Outworldz.My.Resources.Physics_Engine
        MenuStrip2.Text = Global.Outworldz.My.Resources._0
        Physics0_None.Text = Global.Outworldz.My.Resources.None
        Physics2_Bullet.Text = Global.Outworldz.My.Resources.Bullet_Physics_word
        Physics3_Separate.Text = Global.Outworldz.My.Resources.Bullet_Threaded_word
        Physics5_Hybrid.Text = Global.Outworldz.My.Resources.ubODE_Hybrid_word

        Text = Global.Outworldz.My.Resources.Physics_word
        ToolStripMenuItem30.Image = Global.Outworldz.My.Resources.question_and_answer
        ToolStripMenuItem30.Text = Global.Outworldz.My.Resources.Help_word

        SetScreen()

        Select Case Settings.Physics
            Case 0 : Physics0_None.Checked = True
            Case 2 : Physics2_Bullet.Checked = True
            Case 3 : Physics3_Separate.Checked = True
            Case 4 : Physics4_UbODE.Checked = True
            Case 5 : Physics5_Hybrid.Checked = True
            Case Else : Physics3_Separate.Checked = True
        End Select

        HelpOnce("Physics")
        initted = True

    End Sub

#End Region

#Region "Help"

    Private Sub ToolStripMenuItem30_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem30.Click
        HelpManual("Physics")
    End Sub

#End Region

#Region "Physics"

    Private Sub Physics5_Hybrid_CheckedChanged(sender As Object, e As EventArgs) Handles Physics5_Hybrid.CheckedChanged

        If Physics5_Hybrid.Checked Then
            Settings.Physics = 5
            Settings.SaveSettings()
        End If

    End Sub

    Private Sub PhysicsNone_CheckedChanged(sender As Object, e As EventArgs) Handles Physics0_None.CheckedChanged

        If Not initted Then Return
        If Physics0_None.Checked Then
            Settings.Physics = 0
            Settings.SaveSettings()
        End If

    End Sub

    Private Sub PhysicsSeparate_CheckedChanged(sender As Object, e As EventArgs) Handles Physics3_Separate.CheckedChanged

        If Not initted Then Return
        If Physics3_Separate.Checked Then
            Settings.Physics = 3
            Settings.SaveSettings()
        End If

    End Sub

    Private Sub PhysicsUbODE_CheckedChanged_1(sender As Object, e As EventArgs) Handles Physics4_UbODE.CheckedChanged

        If Physics4_UbODE.Checked Then
            Settings.Physics = 4
            Settings.SaveSettings()
        End If

    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles Physics2_Bullet.CheckedChanged

        If Not initted Then Return
        If Physics2_Bullet.Checked Then
            Settings.Physics = 2
            Settings.SaveSettings()
        End If

    End Sub

#End Region

End Class
