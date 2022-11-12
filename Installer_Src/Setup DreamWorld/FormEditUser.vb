Public Class FormEditUser

    Private _UD As New UserData
    Private changed As Boolean
    Private initted As Boolean

    Private Property UD As UserData
        Get
            Return _UD
        End Get
        Set(value As UserData)
            _UD = value
        End Set
    End Property

    Public Sub Init(UUID As String)

        UD = MysqlGetUserData(UUID)
        TitleTextBox.Enabled = True
        LevelGroupBox.Enabled = True

        If UD.PrincipalID.Length > 0 Then
            UUIDTextBox.Text = UD.PrincipalID
            FnameTextBox.Text = UD.FirstName
            LastNameTextBox.Text = UD.LastName
            EmailTextBox.Text = UD.Email
            TitleTextBox.Text = UD.UserTitle

            If UD.Level < 0 Then
                RadioNologin.Checked = True
            ElseIf UD.Level >= 0 And UD.Level < 100 Then
                RadioLogin.Checked = True
            ElseIf UD.Level >= 100 And UD.Level < 200 Then
                RadioDiva.Checked = True
            ElseIf UD.Level >= 200 Then
                RadioGod.Checked = True
            End If
        Else
            FnameTextBox.Text = My.Resources.Not_Found
            LastNameTextBox.Text = My.Resources.Not_Found
        End If
        HelpOnce("Users")

        initted = True

    End Sub

    Private Sub EmailTextBox_TextChanged(sender As Object, e As EventArgs) Handles EmailTextBox.TextChanged
        If Not initted Then Return
        UD.Email = EmailTextBox.Text
        changed = True
    End Sub

    Private Sub FnameTextBox_TextChanged(sender As Object, e As EventArgs) Handles FnameTextBox.TextChanged

        If Not initted Then Return
        UD.FirstName = FnameTextBox.Text
        changed = True

    End Sub

    Private Sub Form1_Close(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Closing

        If changed Then
            Dim response = MsgBox(My.Resources.Save_changes_word, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground, My.Resources.Information_word)
            If response = vbYes Then
                MysqlSaveUserData(UD)
            End If
        End If

    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

        'TO DO translations

    End Sub

    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.Click

        HelpManual("Users")

    End Sub

    Private Sub LastNameTextBox_TextChanged(sender As Object, e As EventArgs) Handles LastNameTextBox.TextChanged

        If Not initted Then Return
        UD.LastName = LastNameTextBox.Text
        changed = True

    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioNologin.CheckedChanged

        If Not initted Then Return
        UD.Level = -1
        changed = True

    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioLogin.CheckedChanged

        If Not initted Then Return
        UD.Level = 0
        changed = True

    End Sub

    Private Sub RadioButton3_CheckedChanged(sender As Object, e As EventArgs) Handles RadioDiva.CheckedChanged

        If Not initted Then Return
        UD.Level = 100
        changed = True

    End Sub

    Private Sub RadioButton4_CheckedChanged(sender As Object, e As EventArgs) Handles RadioGod.CheckedChanged

        If Not initted Then Return
        UD.Level = 200
        changed = True

    End Sub

    Private Sub TitleTextBox_TextChanged(sender As Object, e As EventArgs) Handles TitleTextBox.TextChanged

        If Not initted Then Return
        UD.UserTitle = TitleTextBox.Text
        changed = True

    End Sub

    Private Sub UUIDTextBox_TextChanged(sender As Object, e As EventArgs) Handles UUIDTextBox.TextChanged

        If Not initted Then Return
        UD.PrincipalID = UUIDTextBox.Text
        changed = True

    End Sub

End Class
