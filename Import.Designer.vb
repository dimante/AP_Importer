<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Import
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Import))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DatabaseOperationsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CreateAPTestDefinitionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.InstallAdditionalTests = New System.Windows.Forms.ToolStripMenuItem()
        Me.ParametersMaintenanceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.grpAP = New System.Windows.Forms.GroupBox()
        Me.lblRProc = New System.Windows.Forms.Label()
        Me.lblType = New System.Windows.Forms.Label()
        Me.btnAP = New System.Windows.Forms.Button()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.chkTestOnly = New System.Windows.Forms.CheckBox()
        Me.filedes = New System.Windows.Forms.Label()
        Me.lblComplete = New System.Windows.Forms.Label()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.lblDatabaseLocation = New System.Windows.Forms.Label()
        Me.chkDataSource = New System.Windows.Forms.CheckBox()
        Me.OpacityTimer = New System.Windows.Forms.Timer(Me.components)
        Me.RT1 = New System.Windows.Forms.RichTextBox()
        Me.btnCTC = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.JMGBox = New System.Windows.Forms.PictureBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblDisErr = New System.Windows.Forms.Label()
        Me.pbUpdateNeeded = New System.Windows.Forms.PictureBox()
        Me.MenuStrip1.SuspendLayout()
        Me.grpAP.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        CType(Me.JMGBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        CType(Me.pbUpdateNeeded, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.DatabaseOperationsToolStripMenuItem, Me.ParametersMaintenanceToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Padding = New System.Windows.Forms.Padding(7, 2, 0, 2)
        Me.MenuStrip1.Size = New System.Drawing.Size(904, 24)
        Me.MenuStrip1.TabIndex = 8
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'DatabaseOperationsToolStripMenuItem
        '
        Me.DatabaseOperationsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CreateAPTestDefinitionToolStripMenuItem, Me.InstallAdditionalTests})
        Me.DatabaseOperationsToolStripMenuItem.Name = "DatabaseOperationsToolStripMenuItem"
        Me.DatabaseOperationsToolStripMenuItem.Size = New System.Drawing.Size(128, 20)
        Me.DatabaseOperationsToolStripMenuItem.Text = "Database Operations"
        '
        'CreateAPTestDefinitionToolStripMenuItem
        '
        Me.CreateAPTestDefinitionToolStripMenuItem.Name = "CreateAPTestDefinitionToolStripMenuItem"
        Me.CreateAPTestDefinitionToolStripMenuItem.Size = New System.Drawing.Size(204, 22)
        Me.CreateAPTestDefinitionToolStripMenuItem.Text = "Create AP Test Definition"
        '
        'InstallAdditionalTests
        '
        Me.InstallAdditionalTests.Name = "InstallAdditionalTests"
        Me.InstallAdditionalTests.Size = New System.Drawing.Size(204, 22)
        Me.InstallAdditionalTests.Text = "Install New Test(s)"
        '
        'ParametersMaintenanceToolStripMenuItem
        '
        Me.ParametersMaintenanceToolStripMenuItem.Name = "ParametersMaintenanceToolStripMenuItem"
        Me.ParametersMaintenanceToolStripMenuItem.Size = New System.Drawing.Size(145, 20)
        Me.ParametersMaintenanceToolStripMenuItem.Text = "Parameter Maintenance"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(38, 20)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'grpAP
        '
        Me.grpAP.Controls.Add(Me.lblRProc)
        Me.grpAP.Controls.Add(Me.lblType)
        Me.grpAP.Controls.Add(Me.btnAP)
        Me.grpAP.Location = New System.Drawing.Point(23, 181)
        Me.grpAP.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.grpAP.Name = "grpAP"
        Me.grpAP.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.grpAP.Size = New System.Drawing.Size(235, 123)
        Me.grpAP.TabIndex = 24
        Me.grpAP.TabStop = False
        Me.grpAP.Text = "AP Scores"
        '
        'lblRProc
        '
        Me.lblRProc.AutoSize = True
        Me.lblRProc.Location = New System.Drawing.Point(3, 36)
        Me.lblRProc.Name = "lblRProc"
        Me.lblRProc.Size = New System.Drawing.Size(0, 16)
        Me.lblRProc.TabIndex = 29
        '
        'lblType
        '
        Me.lblType.AutoSize = True
        Me.lblType.Location = New System.Drawing.Point(5, 20)
        Me.lblType.Name = "lblType"
        Me.lblType.Size = New System.Drawing.Size(0, 16)
        Me.lblType.TabIndex = 28
        '
        'btnAP
        '
        Me.btnAP.Location = New System.Drawing.Point(62, 75)
        Me.btnAP.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnAP.Name = "btnAP"
        Me.btnAP.Size = New System.Drawing.Size(113, 28)
        Me.btnAP.TabIndex = 1
        Me.btnAP.Text = "AP Test Import"
        Me.btnAP.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.chkTestOnly)
        Me.GroupBox3.Controls.Add(Me.filedes)
        Me.GroupBox3.Location = New System.Drawing.Point(22, 36)
        Me.GroupBox3.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GroupBox3.Size = New System.Drawing.Size(839, 71)
        Me.GroupBox3.TabIndex = 25
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "File to Process"
        '
        'chkTestOnly
        '
        Me.chkTestOnly.AutoSize = True
        Me.chkTestOnly.Location = New System.Drawing.Point(8, 18)
        Me.chkTestOnly.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.chkTestOnly.Name = "chkTestOnly"
        Me.chkTestOnly.Size = New System.Drawing.Size(301, 20)
        Me.chkTestOnly.TabIndex = 28
        Me.chkTestOnly.Text = "File Error Check Only (Do Not Write Database!)"
        Me.chkTestOnly.UseVisualStyleBackColor = True
        '
        'filedes
        '
        Me.filedes.AutoSize = True
        Me.filedes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.filedes.Location = New System.Drawing.Point(12, 46)
        Me.filedes.Name = "filedes"
        Me.filedes.Size = New System.Drawing.Size(0, 13)
        Me.filedes.TabIndex = 26
        '
        'lblComplete
        '
        Me.lblComplete.AutoSize = True
        Me.lblComplete.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblComplete.ForeColor = System.Drawing.Color.ForestGreen
        Me.lblComplete.Location = New System.Drawing.Point(345, 682)
        Me.lblComplete.Name = "lblComplete"
        Me.lblComplete.Size = New System.Drawing.Size(214, 20)
        Me.lblComplete.TabIndex = 30
        Me.lblComplete.Text = "**File Import Complete!!**"
        Me.lblComplete.Visible = False
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.lblDatabaseLocation)
        Me.GroupBox4.Controls.Add(Me.chkDataSource)
        Me.GroupBox4.Location = New System.Drawing.Point(22, 110)
        Me.GroupBox4.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GroupBox4.Size = New System.Drawing.Size(644, 64)
        Me.GroupBox4.TabIndex = 31
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Database"
        '
        'lblDatabaseLocation
        '
        Me.lblDatabaseLocation.AutoSize = True
        Me.lblDatabaseLocation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDatabaseLocation.Location = New System.Drawing.Point(216, 25)
        Me.lblDatabaseLocation.Name = "lblDatabaseLocation"
        Me.lblDatabaseLocation.Size = New System.Drawing.Size(0, 13)
        Me.lblDatabaseLocation.TabIndex = 32
        '
        'chkDataSource
        '
        Me.chkDataSource.AutoSize = True
        Me.chkDataSource.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDataSource.Location = New System.Drawing.Point(12, 22)
        Me.chkDataSource.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.chkDataSource.Name = "chkDataSource"
        Me.chkDataSource.Size = New System.Drawing.Size(174, 17)
        Me.chkDataSource.TabIndex = 31
        Me.chkDataSource.Text = "Process in LIVE Database"
        Me.chkDataSource.UseVisualStyleBackColor = True
        '
        'RT1
        '
        Me.RT1.Location = New System.Drawing.Point(22, 332)
        Me.RT1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.RT1.Name = "RT1"
        Me.RT1.ReadOnly = True
        Me.RT1.Size = New System.Drawing.Size(800, 345)
        Me.RT1.TabIndex = 36
        Me.RT1.Text = ""
        '
        'btnCTC
        '
        Me.btnCTC.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCTC.Enabled = False
        Me.btnCTC.Image = CType(resources.GetObject("btnCTC.Image"), System.Drawing.Image)
        Me.btnCTC.Location = New System.Drawing.Point(830, 632)
        Me.btnCTC.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnCTC.Name = "btnCTC"
        Me.btnCTC.Size = New System.Drawing.Size(42, 45)
        Me.btnCTC.TabIndex = 37
        Me.btnCTC.TabStop = False
        Me.btnCTC.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(711, 281)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(169, 14)
        Me.Label1.TabIndex = 47
        Me.Label1.Text = "https://www.jmgservices.org"
        '
        'JMGBox
        '
        Me.JMGBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.JMGBox.Image = CType(resources.GetObject("JMGBox.Image"), System.Drawing.Image)
        Me.JMGBox.Location = New System.Drawing.Point(729, 181)
        Me.JMGBox.Name = "JMGBox"
        Me.JMGBox.Size = New System.Drawing.Size(132, 91)
        Me.JMGBox.TabIndex = 46
        Me.JMGBox.TabStop = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblDisErr)
        Me.GroupBox1.Location = New System.Drawing.Point(270, 181)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(441, 123)
        Me.GroupBox1.TabIndex = 51
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "System Messages"
        '
        'lblDisErr
        '
        Me.lblDisErr.AutoSize = True
        Me.lblDisErr.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDisErr.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDisErr.ForeColor = System.Drawing.Color.Red
        Me.lblDisErr.Location = New System.Drawing.Point(6, 20)
        Me.lblDisErr.MaximumSize = New System.Drawing.Size(430, 0)
        Me.lblDisErr.Name = "lblDisErr"
        Me.lblDisErr.Size = New System.Drawing.Size(0, 16)
        Me.lblDisErr.TabIndex = 50
        '
        'pbUpdateNeeded
        '
        Me.pbUpdateNeeded.Image = CType(resources.GetObject("pbUpdateNeeded.Image"), System.Drawing.Image)
        Me.pbUpdateNeeded.Location = New System.Drawing.Point(71, 95)
        Me.pbUpdateNeeded.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.pbUpdateNeeded.Name = "pbUpdateNeeded"
        Me.pbUpdateNeeded.Size = New System.Drawing.Size(762, 524)
        Me.pbUpdateNeeded.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.pbUpdateNeeded.TabIndex = 55
        Me.pbUpdateNeeded.TabStop = False
        Me.pbUpdateNeeded.Visible = False
        '
        'Import
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(904, 714)
        Me.Controls.Add(Me.pbUpdateNeeded)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.JMGBox)
        Me.Controls.Add(Me.btnCTC)
        Me.Controls.Add(Me.RT1)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.lblComplete)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.grpAP)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "Import"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Test Score Import (7.1)"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.grpAP.ResumeLayout(False)
        Me.grpAP.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        CType(Me.JMGBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.pbUpdateNeeded, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Textbox1 As System.Windows.Forms.TextBox
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents grpAP As System.Windows.Forms.GroupBox
    Friend WithEvents btnAP As System.Windows.Forms.Button
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents filedes As System.Windows.Forms.Label
    Friend WithEvents chkTestOnly As System.Windows.Forms.CheckBox
    Friend WithEvents lblType As System.Windows.Forms.Label
    Friend WithEvents lblComplete As System.Windows.Forms.Label
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents lblDatabaseLocation As System.Windows.Forms.Label
    Friend WithEvents chkDataSource As System.Windows.Forms.CheckBox
    Friend WithEvents lblRProc As System.Windows.Forms.Label
    Friend WithEvents DatabaseOperationsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CreateAPTestDefinitionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpacityTimer As System.Windows.Forms.Timer
    Friend WithEvents InstallAdditionalTests As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RT1 As System.Windows.Forms.RichTextBox
    Friend WithEvents btnCTC As System.Windows.Forms.Button
    Friend WithEvents ParametersMaintenanceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents Label1 As Label
    Friend WithEvents JMGBox As PictureBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents lblDisErr As Label
    Friend WithEvents pbUpdateNeeded As PictureBox
End Class
