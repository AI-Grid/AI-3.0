Public Class FormIarSaveAll

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

        Label1.Text = Global.Outworldz.My.Resources.Object_Path_and_name
        Text = Global.Outworldz.My.Resources.Save_Inventory_IAR_word

        GModify = ModifyCheckBox.Checked
        GTransfer = TransferCheckBox.Checked
        GCopy = CopyCheckBox.Checked

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

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
    End Sub

    Private Sub CopyCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles CopyCheckBox.CheckedChanged
        GCopy = CopyCheckBox.Checked
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

    Private Sub TransferCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles TransferCheckBox.CheckedChanged
        GTransfer = TransferCheckBox.Checked
    End Sub

#End Region

End Class
