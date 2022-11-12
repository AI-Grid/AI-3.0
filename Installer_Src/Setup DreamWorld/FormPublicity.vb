#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.IO
Imports System.Text.RegularExpressions

Public Class FormPublicity

#Region "GLobals"

    Private initted As Boolean

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

#Region "Publicity Checkbox"

    Private Sub GDPRCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles GDPRCheckBox.CheckedChanged

        If initted Then
            Settings.Gdpr() = GDPRCheckBox.Checked
            Settings.SaveSettings()
        End If

    End Sub

#End Region

#Region "Start/Stop"

    Private Sub Publicity_Close(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed

        Dim category As String = ""
        For Each i In CategoryCheckbox.CheckedItems
            If i.ToString.Length > 0 Then category += CStr(i) & ","
        Next

        GroupBoxPhoto.Text = My.Resources.Photo_Word
        GDPRCheckBox.Text = My.Resources.PublishToOutworldz
        GroupBoxCategory.Text = My.Resources.Category_word
        GroupBoxDescription.Text = My.Resources.Description_word
        ViewOutworldz.Text = My.Resources.View_word

        Settings.Categories = category

        Dim tmp = DescriptionBox.Text.Replace(vbCrLf, "<br>")
        Settings.Description = tmp

        UploadCategory()
        Settings.SaveSettings()

    End Sub

    Private Sub Publicity_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        GDPRCheckBox.Text = Global.Outworldz.My.Resources.Publish_grid
        GroupBoxCategory.Text = Global.Outworldz.My.Resources.Category_word
        GroupBoxPhoto.Text = Global.Outworldz.My.Resources.Photo_Word
        GroupBoxDescription.Text = Global.Outworldz.My.Resources.Description_word
        MenuStrip2.Text = Global.Outworldz.My.Resources._0
        PictureBox9.Image = Global.Outworldz.My.Resources.ClicktoInsertPhoto
        Text = Global.Outworldz.My.Resources.Publicity_Word
        ToolStripMenuItem30.Text = Global.Outworldz.My.Resources.Help_word
        ViewOutworldz.Text = Global.Outworldz.My.Resources.View_word

        SetScreen()

        GDPRCheckBox.Checked = Settings.Gdpr()

        Dim p = IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Photo.png")
        Try
            If File.Exists(p) Then
                Using stream As New IO.FileStream(p, FileMode.Open, FileAccess.Read)
                    PictureBox9.Image = Image.FromStream(stream)
                End Using
            End If
        Catch ex As Exception
            BreakPoint.Dump(ex)
            PictureBox9.Image = Global.Outworldz.My.Resources.ClicktoInsertPhoto
        End Try
        Dim tmp = Settings.Description
        tmp = tmp.Replace("<br>", vbCrLf)
        DescriptionBox.Text = tmp

        Dim cats = Settings.Categories.Split(",".ToCharArray())

        For Each itemname In cats
            Dim Index = CategoryCheckbox.FindStringExact(itemname)
            If Index > -1 Then
                CategoryCheckbox.SetSelected(Index, True)
            End If
        Next

        HelpOnce("Publicity")
        initted = True

    End Sub

#End Region

#Region "Clicks"

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles ViewOutworldz.Click

        Dim webAddress As String = IO.Path.Combine(PropHttpsDomain, "Hyperica/GridsWide.htm")
        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

    End Sub

    Private Sub CategoryCheckbox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CategoryCheckbox.SelectedIndexChanged

        Dim Index = CategoryCheckbox.SelectedIndex

        If CategoryCheckbox.GetItemChecked(Index) Then
            CategoryCheckbox.SetItemCheckState(Index, CheckState.Unchecked)
        Else
            CategoryCheckbox.SetItemCheckState(Index, CheckState.Checked)
        End If

    End Sub

    Private Sub PictureBox9_Click(sender As Object, e As EventArgs) Handles PictureBox9.Click
        Using ofd As New OpenFileDialog With {
            .Filter = Global.Outworldz.My.Resources.picfilter,
            .FilterIndex = 1,
            .Multiselect = False
        }
            If ofd.ShowDialog = DialogResult.OK Then
                If ofd.FileName.Length > 0 Then

                    Dim pattern = New Regex("PNG$", RegexOptions.IgnoreCase)
                    Dim match As Match = pattern.Match(ofd.FileName)
                    If Not match.Success Then
                        MsgBox(My.Resources.Must_PNG, MsgBoxStyle.Information Or MsgBoxStyle.MsgBoxSetForeground)
                        Return
                    End If

                    PictureBox9.Image = Nothing
                    Try

                        PictureBox9.Image = Bitmap.FromFile(ofd.FileName)

                        DeleteFile(IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Photo.png"))
                        Dim newBitmap = New Bitmap(PictureBox9.Image)
                        newBitmap.Save(IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Photo.png"), Imaging.ImageFormat.Png)
                        newBitmap.Dispose()
                    Catch ex As Exception
                        BreakPoint.Dump(ex)
                        ErrorLog("Save Photo " & ex.Message)
                        Return
                    End Try

                    UploadPhoto()
                End If
            End If
        End Using

    End Sub

#Region "Upload"

#End Region

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles DescriptionBox.TextChanged

        If (DescriptionBox.Text.Length > 1024) Then
            DescriptionBox.Text = Mid(DescriptionBox.Text, 1024)
        End If

    End Sub

    Private Sub ToolStripMenuItem30_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem30.Click
        HelpManual("Publicity")
    End Sub

#End Region

End Class
