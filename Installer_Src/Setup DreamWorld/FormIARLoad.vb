Public Class FormIarLoad

#Region "Private"

    Private _gAvatar As String
    Private _gFolder As String

#End Region

#Region "Properties"

    Public Property GAvatar As String
        Get
            Return _gAvatar
        End Get
        Set(value As String)
            _gAvatar = value
        End Set
    End Property

    Public Property GFolder As String
        Get
            Return _gFolder
        End Get
        Set(value As String)
            _gFolder = value
        End Set
    End Property

#End Region

    Private Sub AvatarNameTextBox_TextChanged(sender As Object, e As EventArgs) Handles AviName.TextChanged
        If AviName.Text.Length > 0 Then
            AviName.BackColor = Color.White
        End If
        GAvatar = AviName.Text
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        DialogResult = DialogResult.OK

    End Sub

    Private Sub FormIARLoad_Load(sender As Object, e As EventArgs) Handles Me.Load

        Label1.Text = My.Resources.Enter_1_2
        Label2.Text = My.Resources.Folder_To_Save_To_word

        With AviName
            .AutoCompleteCustomSource = MysqlInterface.GetAvatarList()
            .AutoCompleteMode = AutoCompleteMode.Suggest
            .AutoCompleteSource = AutoCompleteSource.CustomSource
        End With

    End Sub

    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.Click
        HelpManual("Load IAR")
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles FolderTextbox.TextChanged
        GFolder = FolderTextbox.Text
    End Sub

End Class