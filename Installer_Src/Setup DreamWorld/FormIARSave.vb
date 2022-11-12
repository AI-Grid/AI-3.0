#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Public Class FormIarSave

#Region "Private Fields"

    Private _gAvatarName As String = ""
    Private _gBackupName As String = ""
    Private _gBackupPath As String = ""
    Private _GCopy As Boolean
    Private _GModify As Boolean
    Private _gObject As String = "/"
    Private _GTransfer As Boolean

#End Region

#Region "Public Constructors"

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        Button1.Text = Global.Outworldz.My.Resources.Save_IAR_word

        ModifyCheckBox.Text = Global.Outworldz.My.Resources.Modify_word
        TransferCheckBox.Text = Global.Outworldz.My.Resources.Transfer_word
        CopyCheckBox.Text = Global.Outworldz.My.Resources.Copy_Word

        GroupBox1.Text = Global.Outworldz.My.Resources.Save_Inventory_IAR_word '"Save Inventory IAR"
        HelpToolStripMenuItem.Image = Global.Outworldz.My.Resources.question_and_answer
        HelpToolStripMenuItem.Text = Global.Outworldz.My.Resources.Help_word
        Label1.Text = Global.Outworldz.My.Resources.Object_Path_and_name
        Label2.Text = Global.Outworldz.My.Resources.Backup_Name
        Label3.Text = Global.Outworldz.My.Resources.Avatar_Name_word
        PictureBox1.Image = Global.Outworldz.My.Resources.folder
        Text = Global.Outworldz.My.Resources.Save_Inventory_IAR_word
        ToolTip1.SetToolTip(AviName, Global.Outworldz.My.Resources.Avatar_First_and_Last_Name_word)
        ToolTip1.SetToolTip(ObjectNameBox, Global.Outworldz.My.Resources.Enter_Name)

        BackupNameTextBox.Text = "Backup_" + DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss", Globalization.CultureInfo.InvariantCulture) + ".iar"

        GModify = ModifyCheckBox.Checked
        GTransfer = TransferCheckBox.Checked
        GCopy = CopyCheckBox.Checked

        With AviName
            .AutoCompleteCustomSource = MysqlInterface.GetAvatarList()
            .AutoCompleteMode = AutoCompleteMode.Suggest
            .AutoCompleteSource = AutoCompleteSource.CustomSource
        End With
    End Sub

    Public Sub Init()

        ObjectNameBox.Text = My.Resources.Slash
        BackupNameTextBox.Visible = False
        Label2.Visible = False
        AviName.Visible = False
        Label3.Visible = False
        GAvatarName = "*"

    End Sub

#End Region

#Region "Public Properties"

    Public Property GAvatarName As String
        Get
            Return _gAvatarName
        End Get
        Set(value As String)
            _gAvatarName = value
        End Set
    End Property

    Public Property GBackupName As String
        Get
            Return _gBackupName
        End Get
        Set(value As String)
            _gBackupName = value
        End Set
    End Property

    Public Property GBackupPath As String
        Get
            Return _gBackupPath
        End Get
        Set(value As String)
            _gBackupPath = value
        End Set
    End Property

    Public Property GCopy As Boolean
        Get
            Return _GCopy
        End Get
        Set(value As Boolean)
            _GCopy = value
        End Set
    End Property

    Public Property GModify As Boolean
        Get
            Return _GModify
        End Get
        Set(value As Boolean)
            _GModify = value
        End Set
    End Property

    Public Property GObject As String
        Get
            Return _gObject
        End Get
        Set(value As String)
            _gObject = value
        End Set
    End Property

    Public Property GTransfer As Boolean
        Get
            Return _GTransfer
        End Get
        Set(value As Boolean)
            _GTransfer = value
        End Set
    End Property

#End Region

#Region "Private Methods"

    Private Sub AviName_TextChanged(sender As Object, e As EventArgs) Handles AviName.TextChanged
        If AviName.Text.Length > 0 Then
            AviName.BackColor = Color.White
        End If
        GAvatarName = AviName.Text
    End Sub

    Private Sub BackupNameTextBox_TextChanged_1(sender As Object, e As EventArgs) Handles BackupNameTextBox.TextChanged
        GBackupName = BackupNameTextBox.Text
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        If GAvatarName.Length = 0 Or Not GAvatarName.Contains(" ") Then
            AviName.BackColor = Color.Red
            Return
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
    End Sub

    Private Sub CopyCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles CopyCheckBox.CheckedChanged
        GCopy = CopyCheckBox.Checked
    End Sub

    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.Click
        HelpManual("SaveIar")
    End Sub

    Private Sub ModifyCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles ModifyCheckBox.CheckedChanged
        GModify = ModifyCheckBox.Checked
    End Sub

    Private Sub ObjectNameBox_TextChanged_1(sender As Object, e As EventArgs) Handles ObjectNameBox.TextChanged
        GObject = ObjectNameBox.Text
    End Sub

    Private Sub ObjectNameBox_TextClicks(sender As Object, e As EventArgs) Handles ObjectNameBox.Click
        If ObjectNameBox.Text = "/=everything, /Objects/Folder, etc." Then
            ObjectNameBox.Text = My.Resources.Slash
        End If
    End Sub

    Private Sub PictureBox1_Click_1(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Dim ofd As New FolderBrowserDialog
        If ofd.ShowDialog = DialogResult.OK Then
            If ofd.SelectedPath.Length > 0 Then
                GBackupPath = ofd.SelectedPath
                BackupNameTextBox.Text = ofd.SelectedPath
            End If
        End If
        ofd.Dispose()
    End Sub

    Private Sub TransferCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles TransferCheckBox.CheckedChanged
        GTransfer = TransferCheckBox.Checked
    End Sub

#End Region

End Class
