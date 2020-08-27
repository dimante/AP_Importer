<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Parameters
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.btnShowMainPW = New System.Windows.Forms.Button()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.txtUserName = New System.Windows.Forms.TextBox()
        Me.txtDatabaseName = New System.Windows.Forms.TextBox()
        Me.txtDatabaseServerName = New System.Windows.Forms.TextBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.btnShowDevPW = New System.Windows.Forms.Button()
        Me.txtDEVPassword = New System.Windows.Forms.TextBox()
        Me.txtDEVUserName = New System.Windows.Forms.TextBox()
        Me.txtDEVDatabaseName = New System.Windows.Forms.TextBox()
        Me.txtDEVDatabaseServerName = New System.Windows.Forms.TextBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtDistrict = New System.Windows.Forms.TextBox()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.cmbSSPI = New System.Windows.Forms.ComboBox()
        Me.lblSSPI = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(18, 51)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(183, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "District Number (Required): "
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnShowMainPW)
        Me.GroupBox1.Controls.Add(Me.txtPassword)
        Me.GroupBox1.Controls.Add(Me.txtUserName)
        Me.GroupBox1.Controls.Add(Me.txtDatabaseName)
        Me.GroupBox1.Controls.Add(Me.txtDatabaseServerName)
        Me.GroupBox1.Controls.Add(Me.TextBox1)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.ForeColor = System.Drawing.Color.DarkRed
        Me.GroupBox1.Location = New System.Drawing.Point(15, 86)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(505, 199)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Main Database (Required)"
        '
        'btnShowMainPW
        '
        Me.btnShowMainPW.Location = New System.Drawing.Point(321, 114)
        Me.btnShowMainPW.Name = "btnShowMainPW"
        Me.btnShowMainPW.Size = New System.Drawing.Size(161, 23)
        Me.btnShowMainPW.TabIndex = 8
        Me.btnShowMainPW.TabStop = False
        Me.btnShowMainPW.Text = "Show / Hide Password"
        Me.btnShowMainPW.UseVisualStyleBackColor = True
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(185, 115)
        Me.txtPassword.MaxLength = 255
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.Size = New System.Drawing.Size(132, 22)
        Me.txtPassword.TabIndex = 5
        Me.txtPassword.UseSystemPasswordChar = True
        '
        'txtUserName
        '
        Me.txtUserName.Location = New System.Drawing.Point(185, 86)
        Me.txtUserName.MaxLength = 255
        Me.txtUserName.Name = "txtUserName"
        Me.txtUserName.Size = New System.Drawing.Size(132, 22)
        Me.txtUserName.TabIndex = 4
        '
        'txtDatabaseName
        '
        Me.txtDatabaseName.Location = New System.Drawing.Point(185, 57)
        Me.txtDatabaseName.MaxLength = 255
        Me.txtDatabaseName.Name = "txtDatabaseName"
        Me.txtDatabaseName.Size = New System.Drawing.Size(280, 22)
        Me.txtDatabaseName.TabIndex = 3
        '
        'txtDatabaseServerName
        '
        Me.txtDatabaseServerName.Location = New System.Drawing.Point(185, 28)
        Me.txtDatabaseServerName.MaxLength = 255
        Me.txtDatabaseServerName.Name = "txtDatabaseServerName"
        Me.txtDatabaseServerName.Size = New System.Drawing.Size(280, 22)
        Me.txtDatabaseServerName.TabIndex = 2
        '
        'TextBox1
        '
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox1.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(9, 147)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(456, 42)
        Me.TextBox1.TabIndex = 7
        Me.TextBox1.TabStop = False
        Me.TextBox1.Text = "The user defined above must have network connection rights and update access to t" &
    "he database!"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Black
        Me.Label6.Location = New System.Drawing.Point(7, 121)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(168, 16)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Database User Password:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Black
        Me.Label5.Location = New System.Drawing.Point(34, 91)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(140, 16)
        Me.Label5.TabIndex = 5
        Me.Label5.Text = "Database UserName:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(32, 61)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(142, 16)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "eSP Database Name:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Black
        Me.Label3.Location = New System.Drawing.Point(21, 31)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(158, 16)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Database Server Name:"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.btnShowDevPW)
        Me.GroupBox2.Controls.Add(Me.txtDEVPassword)
        Me.GroupBox2.Controls.Add(Me.txtDEVUserName)
        Me.GroupBox2.Controls.Add(Me.txtDEVDatabaseName)
        Me.GroupBox2.Controls.Add(Me.txtDEVDatabaseServerName)
        Me.GroupBox2.Controls.Add(Me.TextBox2)
        Me.GroupBox2.Controls.Add(Me.Label7)
        Me.GroupBox2.Controls.Add(Me.Label8)
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Controls.Add(Me.Label10)
        Me.GroupBox2.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.ForeColor = System.Drawing.Color.Goldenrod
        Me.GroupBox2.Location = New System.Drawing.Point(15, 363)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(505, 199)
        Me.GroupBox2.TabIndex = 4
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Development Database (Optional)"
        '
        'btnShowDevPW
        '
        Me.btnShowDevPW.Location = New System.Drawing.Point(341, 118)
        Me.btnShowDevPW.Name = "btnShowDevPW"
        Me.btnShowDevPW.Size = New System.Drawing.Size(158, 23)
        Me.btnShowDevPW.TabIndex = 9
        Me.btnShowDevPW.TabStop = False
        Me.btnShowDevPW.Text = "Show / Hide Password"
        Me.btnShowDevPW.UseVisualStyleBackColor = True
        '
        'txtDEVPassword
        '
        Me.txtDEVPassword.Location = New System.Drawing.Point(212, 118)
        Me.txtDEVPassword.MaxLength = 255
        Me.txtDEVPassword.Name = "txtDEVPassword"
        Me.txtDEVPassword.Size = New System.Drawing.Size(127, 22)
        Me.txtDEVPassword.TabIndex = 9
        Me.txtDEVPassword.UseSystemPasswordChar = True
        '
        'txtDEVUserName
        '
        Me.txtDEVUserName.Location = New System.Drawing.Point(212, 89)
        Me.txtDEVUserName.MaxLength = 255
        Me.txtDEVUserName.Name = "txtDEVUserName"
        Me.txtDEVUserName.Size = New System.Drawing.Size(127, 22)
        Me.txtDEVUserName.TabIndex = 8
        '
        'txtDEVDatabaseName
        '
        Me.txtDEVDatabaseName.Location = New System.Drawing.Point(212, 60)
        Me.txtDEVDatabaseName.MaxLength = 255
        Me.txtDEVDatabaseName.Name = "txtDEVDatabaseName"
        Me.txtDEVDatabaseName.Size = New System.Drawing.Size(256, 22)
        Me.txtDEVDatabaseName.TabIndex = 7
        '
        'txtDEVDatabaseServerName
        '
        Me.txtDEVDatabaseServerName.Location = New System.Drawing.Point(212, 31)
        Me.txtDEVDatabaseServerName.MaxLength = 255
        Me.txtDEVDatabaseServerName.Name = "txtDEVDatabaseServerName"
        Me.txtDEVDatabaseServerName.Size = New System.Drawing.Size(256, 22)
        Me.txtDEVDatabaseServerName.TabIndex = 6
        '
        'TextBox2
        '
        Me.TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox2.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox2.Location = New System.Drawing.Point(9, 149)
        Me.TextBox2.Multiline = True
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.Size = New System.Drawing.Size(456, 42)
        Me.TextBox2.TabIndex = 7
        Me.TextBox2.TabStop = False
        Me.TextBox2.Text = "The user defined above must have network connection rights and update access to t" &
    "he database!"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Black
        Me.Label7.Location = New System.Drawing.Point(5, 121)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(196, 16)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Dev Database User Password:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.Black
        Me.Label8.Location = New System.Drawing.Point(36, 91)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(168, 16)
        Me.Label8.TabIndex = 5
        Me.Label8.Text = "Dev Database UserName:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.Black
        Me.Label9.Location = New System.Drawing.Point(34, 61)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(170, 16)
        Me.Label9.TabIndex = 4
        Me.Label9.Text = "eSP Dev Database Name:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.Black
        Me.Label10.Location = New System.Drawing.Point(23, 31)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(186, 16)
        Me.Label10.TabIndex = 3
        Me.Label10.Text = "Dev Database Server Name:"
        '
        'txtDistrict
        '
        Me.txtDistrict.Location = New System.Drawing.Point(199, 51)
        Me.txtDistrict.MaxLength = 255
        Me.txtDistrict.Name = "txtDistrict"
        Me.txtDistrict.Size = New System.Drawing.Size(132, 22)
        Me.txtDistrict.TabIndex = 1
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(126, 581)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 10
        Me.btnSave.TabStop = False
        Me.btnSave.Text = "Save Changes"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(311, 581)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 15
        Me.btnCancel.TabStop = False
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.cmbSSPI)
        Me.GroupBox3.Controls.Add(Me.lblSSPI)
        Me.GroupBox3.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox3.ForeColor = System.Drawing.Color.DarkRed
        Me.GroupBox3.Location = New System.Drawing.Point(15, 295)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(505, 58)
        Me.GroupBox3.TabIndex = 17
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Integrated Security (Required)"
        '
        'cmbSSPI
        '
        Me.cmbSSPI.FormattingEnabled = True
        Me.cmbSSPI.Items.AddRange(New Object() {"False", "SSPI"})
        Me.cmbSSPI.Location = New System.Drawing.Point(395, 17)
        Me.cmbSSPI.Name = "cmbSSPI"
        Me.cmbSSPI.Size = New System.Drawing.Size(68, 24)
        Me.cmbSSPI.TabIndex = 19
        '
        'lblSSPI
        '
        Me.lblSSPI.AutoSize = True
        Me.lblSSPI.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSSPI.ForeColor = System.Drawing.Color.Black
        Me.lblSSPI.Location = New System.Drawing.Point(12, 20)
        Me.lblSSPI.Name = "lblSSPI"
        Me.lblSSPI.Size = New System.Drawing.Size(377, 16)
        Me.lblSSPI.TabIndex = 18
        Me.lblSSPI.Text = "User Running Application Has Full Database Rights (SSPI):"
        '
        'Parameters
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(542, 623)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.txtDistrict)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "Parameters"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Parameter Maintenance"
        Me.TopMost = True
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtUserName As System.Windows.Forms.TextBox
    Friend WithEvents txtDatabaseName As System.Windows.Forms.TextBox
    Friend WithEvents txtDatabaseServerName As System.Windows.Forms.TextBox
    Friend WithEvents txtDEVPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtDEVUserName As System.Windows.Forms.TextBox
    Friend WithEvents txtDEVDatabaseName As System.Windows.Forms.TextBox
    Friend WithEvents txtDEVDatabaseServerName As System.Windows.Forms.TextBox
    Friend WithEvents txtDistrict As System.Windows.Forms.TextBox
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents cmbSSPI As System.Windows.Forms.ComboBox
    Friend WithEvents lblSSPI As System.Windows.Forms.Label
    Friend WithEvents btnShowMainPW As System.Windows.Forms.Button
    Friend WithEvents btnShowDevPW As System.Windows.Forms.Button
End Class
