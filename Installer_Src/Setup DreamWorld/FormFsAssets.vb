﻿#Region "Copyright"

' Copyright 2014 Fred Beckhusen for outworldz.com https://opensource.org/licenses/AGPL

'Permission Is hereby granted, free Of charge, to any person obtaining a copy of this software
' And associated documentation files (the "Software"), to deal in the Software without restriction,
'including without limitation the rights To use, copy, modify, merge, publish, distribute, sublicense,
'And/Or sell copies Of the Software, And To permit persons To whom the Software Is furnished To
'Do so, subject To the following conditions:

'The above copyright notice And this permission notice shall be included In all copies Or '
'substantial portions Of the Software.

'THE SOFTWARE Is PROVIDED "AS IS", WITHOUT WARRANTY Of ANY KIND, EXPRESS Or IMPLIED,
' INCLUDING BUT Not LIMITED To THE WARRANTIES Of MERCHANTABILITY, FITNESS For A PARTICULAR
'PURPOSE And NONINFRINGEMENT.In NO Event SHALL THE AUTHORS Or COPYRIGHT HOLDERS BE LIABLE
'For ANY CLAIM, DAMAGES Or OTHER LIABILITY, WHETHER In AN ACTION Of CONTRACT, TORT Or
'OTHERWISE, ARISING FROM, OUT Of Or In CONNECTION With THE SOFTWARE Or THE USE Or OTHER
'DEALINGS IN THE SOFTWARE.Imports System

#End Region

Public Class FormFsAssets

#Region "Declarations"

    Dim _changed As Boolean = False
    Dim initted As Boolean = False

#End Region

#Region "ScreenSize"

    'The following detects  the location of the form in screen coordinates
    Private _screenPosition As ScreenPos

    Private Handler As New EventHandler(AddressOf Resize_page)

    Public Property ScreenPosition As ScreenPos
        Get
            Return _screenPosition
        End Get
        Set(value As ScreenPos)
            _screenPosition = value
        End Set
    End Property

    Private Sub Resize_page(ByVal sender As Object, ByVal e As System.EventArgs)
        'Me.Text = "Form screen position = " + Me.Location.ToString
        ScreenPosition.SaveXY(Me.Left, Me.Top)
    End Sub

    Private Sub SetScreen()
        Me.Show()
        ScreenPosition = New ScreenPos(Me.Name)
        AddHandler ResizeEnd, Handler
        Dim xy As List(Of Integer) = ScreenPosition.GetXY()
        Me.Left = xy.Item(0)
        Me.Top = xy.Item(1)
    End Sub

#End Region

#Region "Properties"

    Public Property Changed As Boolean
        Get
            Return _changed
        End Get
        Set(value As Boolean)
            _changed = value
        End Set
    End Property

    Public Property Initted1 As Boolean
        Get
            Return initted
        End Get
        Set(value As Boolean)
            initted = value
        End Set
    End Property

#End Region

#Region "LoadSave"

    Private Sub Form_exit() Handles Me.Closed

        If Changed Then

            Dim result As MsgBoxResult = MsgBox(My.Resources.Changes_Made, vbYesNo)
            If result = vbOK Then
                Form1.PropViewedSettings = True
                Form1.Settings.SaveSettings()
            End If
        End If

    End Sub

    Private Sub Loaded(sender As Object, e As EventArgs) Handles Me.Load

        Form1.HelpOnce("FSAssets")

        EnableFsAssetsCheckbox.Checked = Form1.Settings.FsAssetsEnabled
        DataFolder.Text = Form1.Settings.BaseDirectory
        ShowStatsCheckBox.Checked = CType(Form1.Settings.ShowConsoleStats, Boolean)
        SetScreen()
        Initted1 = True

    End Sub

#End Region

#Region "Subs"

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles EnableFsAssetsCheckbox.CheckedChanged
        If Not initted Then Return
        Form1.Settings.FsAssetsEnabled = EnableFsAssetsCheckbox.Checked
        Changed = True
    End Sub

    Private Sub CheckBox1_CheckedChanged_1(sender As Object, e As EventArgs) Handles ShowStatsCheckBox.CheckedChanged

        If Not initted Then Return
        Form1.Settings.ShowConsoleStats = ShowStatsCheckBox.Checked.ToString(Globalization.CultureInfo.InvariantCulture)
        Changed = True

    End Sub

    Private Sub HelpToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem1.Click
        Form1.Help("FSAssets")
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Form1.Help("FSAssets")
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click

        'Create an instance of the open file dialog box.
        Dim openFileDialog1 As FolderBrowserDialog = New FolderBrowserDialog With {
            .ShowNewFolderButton = True,
            .Description = My.Resources.Choose_a_Folder_word
        }
        Dim UserClickedOK As DialogResult = openFileDialog1.ShowDialog

        ' Process input if the user clicked OK.
        If UserClickedOK = DialogResult.OK Then
            Dim thing = openFileDialog1.SelectedPath
            If thing.Length > 0 Then
                Form1.Settings.BaseDirectory = thing
                Form1.Settings.SaveSettings()
                DataFolder.Text = thing
                Changed = True
            End If
        End If
        openFileDialog1.Dispose()

    End Sub

    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click

        Form1.Settings.SaveSettings()
        Me.Close()
    End Sub

#End Region

End Class
