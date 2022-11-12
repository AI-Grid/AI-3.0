Imports System.Text.RegularExpressions

Public Class FormBlockify

    Private Sizer As Integer = 10
    Private Spacer As Integer = 1

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
        'Me.Text = "Form screen position = " & Me.Location.ToString
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

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles GoButton.Click

        If PropOpensimIsRunning() Then
            MsgBox(My.Resources.OpensimNeedstoStop, MsgBoxStyle.Exclamation Or MsgBoxStyle.MsgBoxSetForeground Or MsgBoxStyle.Information, My.Resources.Cancelled_word)
            Return
        End If

        MakeBlock()

    End Sub

    Private Sub FormBlockify_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        GoButton.Text = My.Resources.Packregions
        Label1.Text = My.Resources.Spacing
        Label2.Text = My.Resources.Width
        Me.Text = Global.Outworldz.My.Resources.Pack_Word
        ToolTip1.SetToolTip(GoButton, Global.Outworldz.My.Resources.PackRegionTT)
        ToolTip1.SetToolTip(Label1, Global.Outworldz.My.Resources.SpacingTT)
        ToolTip1.SetToolTip(RowSizeTextbox, Global.Outworldz.My.Resources.RowSizeTT)
        ToolTip1.SetToolTip(RowSizeTextbox, Global.Outworldz.My.Resources.RowSizeTT)
        ToolTip1.SetToolTip(SpacingTextBox, Global.Outworldz.My.Resources.SpacingTT)

        SetScreen()

        HelpOnce("Rearrange")

    End Sub

    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.Click
        HelpManual("Rearrange")
    End Sub

    Private Sub MakeBlock()

        TextPrint(My.Resources.BackingUp)
        BackupINI()
        TextPrint(My.Resources.Finished_with_backup_word)

        Dim Caution = MsgBox(My.Resources.CautionMoving, vbYesNo Or MsgBoxStyle.MsgBoxSetForeground Or MsgBoxStyle.Critical, My.Resources.Caution_word)
        If Caution <> MsgBoxResult.Yes Then Return

        TextPrint(My.Resources.Packing)

        Dim CoordX = CStr(LargestX())
        Dim CoordY = CStr(LargestY() + 18)

        Dim coord = InputBox(My.Resources.WheretoStart, My.Resources.StartingLocation, CoordX & "," & CoordY)

        Dim pattern = New Regex("(\d+),\s?(\d+)")
        Dim match As Match = pattern.Match(coord)
        If Not match.Success Then
            MsgBox(My.Resources.BadCoordinates, MsgBoxStyle.Exclamation Or MsgBoxStyle.MsgBoxSetForeground, My.Resources.Error_word)
            Return
        End If

        GoButton.Text = My.Resources.Busy_word

        Dim X As Integer = 0
        Dim Y As Integer = 0
        Try
            X = CInt("0" & match.Groups(1).Value)
            Y = CInt("0" & match.Groups(2).Value)
        Catch
        End Try

        If X <= 1 Or Y < 32 Then
            MsgBox($"{My.Resources.BadCoordinates} : X > 1 And Y > 32", MsgBoxStyle.Exclamation Or MsgBoxStyle.MsgBoxSetForeground, My.Resources.Error_word)
            Return
        End If

        ' setup parameters for the load
        Dim StartX = X ' loop begin
        Dim MaxSizeThisRow As Integer = 1 ' the largest region in a row
        Dim SizeRegion As Integer = 1 ' (1X1)

        Dim ListRegions As New List(Of String)

        'sort alphabetically
        For Each RegionUUID In RegionUuids()
            ListRegions.Add(Region_Name(RegionUUID))
        Next
        ListRegions.Sort()

        For Each RegionName In ListRegions

            Dim RegionUUID = FindRegionByName(RegionName)

            If RegionEnabled(RegionUUID) Then

                Coord_X(RegionUUID) = X
                Coord_Y(RegionUUID) = Y

                SizeRegion = CInt(SizeX(RegionUUID) / 256)

                TextPrint(Region_Name(RegionUUID))
                WriteRegionObject(Group_Name(RegionUUID), Region_Name(RegionUUID))

                If MaxSizeThisRow <= SizeRegion Then
                    MaxSizeThisRow = SizeRegion
                End If

                X += SizeRegion + Spacer

                If X >= StartX + Sizer Then   ' if past right border,
                    X = StartX              ' go back to left border
                    Y += MaxSizeThisRow + Spacer ' Add the largest size +1 and move up
                    MaxSizeThisRow = 1
                End If

                Delete_Region_Map(RegionUUID)
            End If

        Next

        PropChangedRegionSettings = True
        PropUpdateView = True ' make form refresh

        DeregisterRegions(False)
        TextPrint(My.Resources.Finished_word)
        GoButton.Text = My.Resources.Finished_word
        Sleep(2000)
        Me.Close()

    End Sub

    Private Sub RowSize_TextChanged(sender As Object, e As EventArgs) Handles RowSizeTextbox.LostFocus

        Dim digitsOnly = New Regex("[^\d]")
        RowSizeTextbox.Text = digitsOnly.Replace(RowSizeTextbox.Text, "")

        Sizer = CInt(RowSizeTextbox.Text)
        If Sizer < 1 Then
            Sizer = 1
#Disable Warning CA1303 ' Do not pass literals as localized parameters
            RowSizeTextbox.Text = "1"
#Enable Warning CA1303 ' Do not pass literals as localized parameters
        End If

    End Sub

    Private Sub Spacing_TextChanged(sender As Object, e As EventArgs) Handles SpacingTextBox.TextChanged

        Dim digitsOnly = New Regex("[^\d]")
        SpacingTextBox.Text = digitsOnly.Replace(SpacingTextBox.Text, "")

        Spacer = CInt("0" & SpacingTextBox.Text)
        SpacingTextBox.Text = CStr(Spacer)
    End Sub

End Class