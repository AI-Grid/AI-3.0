Imports System.IO

Public Class FormSearchHelp

    Dim files As New List(Of IO.FileInfo)
    Dim initted As Boolean

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        Dim directory As New System.IO.DirectoryInfo(IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Help"))
        ' get each file's last modified date
        For Each FileName As System.IO.FileInfo In directory.GetFiles()
            If FileName.Name.EndsWith(".htm", StringComparison.OrdinalIgnoreCase) Then files.Add(FileName)
        Next

        SetupLayout()

        PopulateDataGridView("")

        HelpOnce("Search Help")
        initted = True

    End Sub

    Private Sub dataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick

        Dim cell = DataGridView1.Rows(e.RowIndex).Cells(0)
        If (cell.Value = "") Then Return
        HelpManual(CStr(cell.Value))

    End Sub

    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.Click

        HelpManual("Search Help")

    End Sub

    Private Sub PopulateDataGridView(text As String)
        If text.Length = 0 Then
            DataGridView1.Rows.Clear()
            For Each File In files
                Dim s = File.Name
                s = s.Replace(".htm", "")
                DataGridView1.Rows.Add({s})
            Next
        Else
            DataGridView1.Rows.Clear()
            searchHelp(text)
        End If

    End Sub

    Private Sub searchHelp(text As String)

        For Each file In files
            Dim fileReader As String
            fileReader = My.Computer.FileSystem.ReadAllText(file.FullName, System.Text.Encoding.ASCII)
            Dim Check As Boolean
            Try
                Check = fileReader Like "*" & text & "*"
            Catch
            End Try

            If file.Name Like text Then Check = True

            If Check Then
                Dim s = file.Name
                s = s.Replace(".htm", "")
                DataGridView1.Rows.Add({s})
                Continue For
            End If

        Next

    End Sub

    Private Sub SetupLayout()

        DataGridView1.Columns(0).Name = "File"
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.MultiSelect = False

    End Sub

    Private Sub TextBox1_Clicked(sender As Object, e As EventArgs) Handles TextBox1.Click

        TextBox1.Text = ""

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

        If Not initted Then Return
        PopulateDataGridView(TextBox1.Text)

    End Sub

End Class