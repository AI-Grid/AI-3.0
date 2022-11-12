<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormDiva
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormDiva))
        Me.Web = New System.Windows.Forms.GroupBox()
        Me.WifiEnabled = New System.Windows.Forms.CheckBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.AdminEmail = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.AccountConfirmationRequired = New System.Windows.Forms.CheckBox()
        Me.AdminLast = New System.Windows.Forms.TextBox()
        Me.AdminFirst = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.AdminPassword = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.GridName = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.SplashPage = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.CustomRadioButton = New System.Windows.Forms.RadioButton()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.GreetingTextBox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.WhiteRadioButton = New System.Windows.Forms.RadioButton()
        Me.BlackRadioButton = New System.Windows.Forms.RadioButton()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ApacheToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Web.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Web
        '
        Me.Web.Controls.Add(Me.WifiEnabled)
        Me.Web.Controls.Add(Me.Label17)
        Me.Web.Controls.Add(Me.AdminEmail)
        Me.Web.Controls.Add(Me.Label12)
        Me.Web.Controls.Add(Me.AccountConfirmationRequired)
        Me.Web.Controls.Add(Me.AdminLast)
        Me.Web.Controls.Add(Me.AdminFirst)
        Me.Web.Controls.Add(Me.Label11)
        Me.Web.Controls.Add(Me.Label10)
        Me.Web.Controls.Add(Me.AdminPassword)
        Me.Web.Location = New System.Drawing.Point(5, 37)
        Me.Web.Name = "Web"
        Me.Web.Size = New System.Drawing.Size(370, 196)
        Me.Web.TabIndex = 3
        Me.Web.TabStop = False
        Me.Web.Text = "Wifi Interface Admin"
        '
        'WifiEnabled
        '
        Me.WifiEnabled.AutoSize = True
        Me.WifiEnabled.Location = New System.Drawing.Point(19, 28)
        Me.WifiEnabled.Name = "WifiEnabled"
        Me.WifiEnabled.Size = New System.Drawing.Size(111, 17)
        Me.WifiEnabled.TabIndex = 0
        Me.WifiEnabled.Text = Global.Outworldz.My.Resources.Resources.Diva_Wifi_Enabled_word
        Me.WifiEnabled.UseVisualStyleBackColor = True
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(4, 139)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(62, 13)
        Me.Label17.TabIndex = 7
        Me.Label17.Text = "Notify Email"
        '
        'AdminEmail
        '
        Me.AdminEmail.Location = New System.Drawing.Point(161, 134)
        Me.AdminEmail.Name = "AdminEmail"
        Me.AdminEmail.Size = New System.Drawing.Size(164, 20)
        Me.AdminEmail.TabIndex = 8
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(4, 89)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(58, 13)
        Me.Label12.TabIndex = 3
        Me.Label12.Text = "Last Name"
        '
        'AccountConfirmationRequired
        '
        Me.AccountConfirmationRequired.AutoSize = True
        Me.AccountConfirmationRequired.Location = New System.Drawing.Point(19, 173)
        Me.AccountConfirmationRequired.Name = "AccountConfirmationRequired"
        Me.AccountConfirmationRequired.Size = New System.Drawing.Size(264, 17)
        Me.AccountConfirmationRequired.TabIndex = 9
        Me.AccountConfirmationRequired.Text = Global.Outworldz.My.Resources.Resources.AccountConfirm
        Me.AccountConfirmationRequired.UseVisualStyleBackColor = True
        '
        'AdminLast
        '
        Me.AdminLast.Location = New System.Drawing.Point(160, 88)
        Me.AdminLast.Name = "AdminLast"
        Me.AdminLast.Size = New System.Drawing.Size(165, 20)
        Me.AdminLast.TabIndex = 4
        '
        'AdminFirst
        '
        Me.AdminFirst.Location = New System.Drawing.Point(160, 64)
        Me.AdminFirst.Name = "AdminFirst"
        Me.AdminFirst.Size = New System.Drawing.Size(165, 20)
        Me.AdminFirst.TabIndex = 2
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(4, 67)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(57, 13)
        Me.Label11.TabIndex = 1
        Me.Label11.Text = "First Name"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(4, 113)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(53, 13)
        Me.Label10.TabIndex = 6
        Me.Label10.Text = "Password"
        '
        'AdminPassword
        '
        Me.AdminPassword.Location = New System.Drawing.Point(160, 110)
        Me.AdminPassword.Name = "AdminPassword"
        Me.AdminPassword.Size = New System.Drawing.Size(165, 20)
        Me.AdminPassword.TabIndex = 5
        Me.ToolTip1.SetToolTip(Me.AdminPassword, Global.Outworldz.My.Resources.Resources.Password_Text)
        Me.AdminPassword.UseSystemPasswordChar = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 21)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(126, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "This Grid's Friendly Name"
        '
        'GridName
        '
        Me.GridName.Location = New System.Drawing.Point(15, 37)
        Me.GridName.Name = "GridName"
        Me.GridName.Size = New System.Drawing.Size(243, 20)
        Me.GridName.TabIndex = 1
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(13, 59)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(76, 13)
        Me.Label19.TabIndex = 2
        Me.Label19.Text = "Splash Screen"
        '
        'SplashPage
        '
        Me.SplashPage.Location = New System.Drawing.Point(13, 75)
        Me.SplashPage.Name = "SplashPage"
        Me.SplashPage.Size = New System.Drawing.Size(245, 20)
        Me.SplashPage.TabIndex = 3
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.PictureBox1)
        Me.GroupBox1.Controls.Add(Me.CustomRadioButton)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.GreetingTextBox)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.GridName)
        Me.GroupBox1.Controls.Add(Me.Label19)
        Me.GroupBox1.Controls.Add(Me.WhiteRadioButton)
        Me.GroupBox1.Controls.Add(Me.SplashPage)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.BlackRadioButton)
        Me.GroupBox1.Location = New System.Drawing.Point(393, 37)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(267, 337)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Splash Screen"
        '
        'PictureBox1
        '
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox1.Location = New System.Drawing.Point(15, 179)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(231, 149)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 186740
        Me.PictureBox1.TabStop = False
        '
        'CustomRadioButton
        '
        Me.CustomRadioButton.AutoSize = True
        Me.CustomRadioButton.Location = New System.Drawing.Point(183, 149)
        Me.CustomRadioButton.Name = "CustomRadioButton"
        Me.CustomRadioButton.Size = New System.Drawing.Size(60, 17)
        Me.CustomRadioButton.TabIndex = 9
        Me.CustomRadioButton.TabStop = True
        Me.CustomRadioButton.Text = "Custom"
        Me.CustomRadioButton.UseVisualStyleBackColor = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(13, 97)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(82, 13)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Viewer Greeting"
        '
        'GreetingTextBox
        '
        Me.GreetingTextBox.Location = New System.Drawing.Point(13, 113)
        Me.GreetingTextBox.Name = "GreetingTextBox"
        Me.GreetingTextBox.Size = New System.Drawing.Size(245, 20)
        Me.GreetingTextBox.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(16, 149)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(43, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Theme:"
        '
        'WhiteRadioButton
        '
        Me.WhiteRadioButton.AutoSize = True
        Me.WhiteRadioButton.Location = New System.Drawing.Point(67, 149)
        Me.WhiteRadioButton.Name = "WhiteRadioButton"
        Me.WhiteRadioButton.Size = New System.Drawing.Size(53, 17)
        Me.WhiteRadioButton.TabIndex = 7
        Me.WhiteRadioButton.TabStop = True
        Me.WhiteRadioButton.Text = Global.Outworldz.My.Resources.Resources.White_word
        Me.WhiteRadioButton.UseVisualStyleBackColor = True
        '
        'BlackRadioButton
        '
        Me.BlackRadioButton.AutoSize = True
        Me.BlackRadioButton.Location = New System.Drawing.Point(125, 149)
        Me.BlackRadioButton.Name = "BlackRadioButton"
        Me.BlackRadioButton.Size = New System.Drawing.Size(52, 17)
        Me.BlackRadioButton.TabIndex = 8
        Me.BlackRadioButton.TabStop = True
        Me.BlackRadioButton.Text = Global.Outworldz.My.Resources.Resources.Black_word
        Me.BlackRadioButton.UseVisualStyleBackColor = True
        '
        'MenuStrip1
        '
        Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(28, 28)
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Padding = New System.Windows.Forms.Padding(4, 1, 0, 1)
        Me.MenuStrip1.Size = New System.Drawing.Size(698, 34)
        Me.MenuStrip1.TabIndex = 0
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HelpToolStripMenuItem1, Me.ApacheToolStripMenuItem})
        Me.HelpToolStripMenuItem.Image = Global.Outworldz.My.Resources.Resources.about
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(72, 32)
        Me.HelpToolStripMenuItem.Text = Global.Outworldz.My.Resources.Resources.Help_word
        '
        'HelpToolStripMenuItem1
        '
        Me.HelpToolStripMenuItem1.Image = Global.Outworldz.My.Resources.Resources.about
        Me.HelpToolStripMenuItem1.Name = "HelpToolStripMenuItem1"
        Me.HelpToolStripMenuItem1.Size = New System.Drawing.Size(129, 22)
        Me.HelpToolStripMenuItem1.Text = Global.Outworldz.My.Resources.Resources.Diva_Panel_word
        '
        'ApacheToolStripMenuItem
        '
        Me.ApacheToolStripMenuItem.Image = Global.Outworldz.My.Resources.Resources.window_environment
        Me.ApacheToolStripMenuItem.Name = "ApacheToolStripMenuItem"
        Me.ApacheToolStripMenuItem.Size = New System.Drawing.Size(129, 22)
        Me.ApacheToolStripMenuItem.Text = Global.Outworldz.My.Resources.Resources.Apache_word
        '
        'FormDiva
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(698, 391)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Web)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "FormDiva"
        Me.Text = "Web Server Panel"
        Me.Web.ResumeLayout(False)
        Me.Web.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Web As GroupBox
    Friend WithEvents WifiEnabled As CheckBox
    Friend WithEvents Label17 As Label
    Friend WithEvents AdminEmail As TextBox
    Friend WithEvents Label12 As Label
    Friend WithEvents AccountConfirmationRequired As CheckBox
    Friend WithEvents AdminLast As TextBox
    Friend WithEvents AdminFirst As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents AdminPassword As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents GridName As TextBox
    Friend WithEvents Label19 As Label
    Friend WithEvents SplashPage As TextBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents WhiteRadioButton As RadioButton
    Friend WithEvents BlackRadioButton As RadioButton
    Friend WithEvents Label1 As Label
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents Label4 As Label
    Friend WithEvents GreetingTextBox As TextBox
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents HelpToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ApacheToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents CustomRadioButton As RadioButton
End Class
