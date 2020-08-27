Imports WPFGrowlNotification
Imports System.Windows

Public Class Parameters

    Private Sub Parameters_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Get user values and apply them to the text boxes!

        'District
        txtDistrict.Text = My.Settings.District.ToString

        'Database Server Name
        txtDatabaseServerName.Text = My.Settings.DBServerName.ToString

        'Database Name
        txtDatabaseName.Text = My.Settings.DBServerDatabaseName.ToString

        'DB Username
        txtUserName.Text = My.Settings.DBUserName.ToString

        'DB Password
        txtPassword.Text = My.Settings.DBPass.ToString

        'SSPI SETTING
        If Trim(My.Settings.IntegratedSecurity.ToString) = "" Then
            cmbSSPI.SelectedItem = "False"
        Else
            cmbSSPI.SelectedItem = My.Settings.IntegratedSecurity.ToString
        End If

        'DEV Database Server Name
        txtDEVDatabaseServerName.Text = My.Settings.DBDevServerName.ToString

        'DEV Database Name
        txtDEVDatabaseName.Text = My.Settings.DBDevServerDatabaseName.ToString

        'DEV DB Username
        txtDEVUserName.Text = My.Settings.DBDevUserName.ToString

        'DEV DB Password
        txtDEVPassword.Text = My.Settings.DBDevPass.ToString


    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click

        Me.Close()

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        'Update Values
        Dim indicator As Boolean = False

        'District
        My.Settings.District = Trim(txtDistrict.Text)

        'Database Server Name
        My.Settings.DBServerName = Trim(txtDatabaseServerName.Text)

        'Database Name
        My.Settings.DBServerDatabaseName = Trim(txtDatabaseName.Text)

        'DB Username
        My.Settings.DBUserName = Trim(txtUserName.Text)

        'DB Password
        My.Settings.DBPass = Trim(txtPassword.Text)

        'SSPI
        If Trim(cmbSSPI.SelectedItem.ToString) <> "" Then
            My.Settings.IntegratedSecurity = cmbSSPI.SelectedItem.ToString
        Else
            My.Settings.IntegratedSecurity = "False"
        End If


        'DEV Database Server Name
        My.Settings.DBDevServerName = Trim(txtDEVDatabaseServerName.Text)

        'DEV Database Name
        My.Settings.DBDevServerDatabaseName = Trim(txtDEVDatabaseName.Text)

        'DEV DB Username
        My.Settings.DBDevUserName = Trim(txtDEVUserName.Text)

        'DEV DB Password
        My.Settings.DBDevPass = Trim(txtDEVPassword.Text)

        'Save the changed settings
        My.Settings.Save()

        'District 
        If My.Settings.District.Length >= 1 Then
            strDistrict = My.Settings.District
        Else
            'This is a required field so this is a problem.  Display form for updating the information!

        End If

        'DB Servername
        If My.Settings.DBServerName.Length > 1 Then
            DBServerNM = My.Settings.DBServerName
        Else
            'Field is required show update form!
        End If

        'DB Server Database Name
        If My.Settings.DBServerDatabaseName.Length > 1 Then
            DBServerDBNM = My.Settings.DBServerDatabaseName
        Else
            'Field is required show form for update!

        End If

        'DB Username 
        If My.Settings.DBUserName.Length > 1 Then
            DBUN = My.Settings.DBUserName
        Else
            'Required if missing need to display update form!
        End If

        'DB Password
        If My.Settings.DBPass.Length > 1 Then
            DBPassword = My.Settings.DBPass
        Else
            'No value, flag this is a required field!
        End If

        'SSPI 
        IntegratedSecurity = My.Settings.IntegratedSecurity

        'The remaining fields are optional-------------------------------------------------------------------------------------------------------------------

        'DB Dev Server Name (Optional)
        DBDevServerNM = My.Settings.DBDevServerName

        'DB Dev server database name (Optional)
        DBDevServerDBNM = My.Settings.DBDevServerDatabaseName

        'DB Dev Username (Optional) 
        DBDEVUN = My.Settings.DBDevUserName

        'DB Dev Password (Optional)
        DBDevPassword = My.Settings.DBDevPass

        'Update Database connection items!
        'Need to make a decision on Integrated Security=SSPI or Integrated Security=False SSPI assumes the person running the app has database privs... 'User can decide now
        'Integrated Security = SSPI prevents uses without permissions from getting to the database -Removed!
        strConnLive = "Data Source=" & DBServerNM & ";Initial Catalog=" & DBServerDBNM & ";user Id=" & DBUN & ";Password=" & DBPassword & ";Integrated Security=" & IntegratedSecurity.ToString

        'Add development ini

        strConnDev = "Data Source=" & DBDevServerNM & ";Initial Catalog=" & DBDevServerDBNM & ";user Id=" & DBDEVUN & ";Password=" & DBDevPassword & ";Integrated Security=" & IntegratedSecurity.ToString

        Dim connection As String = ""

        If Import.chkDataSource.Checked = True Then

            connection = strConnLive

        Else

            connection = strConnDev

        End If

        Dim DBDisplayString As Array

        DBDisplayString = Split(connection, ";")

        Import.lblDatabaseLocation.Text = "(Current Database: " & Replace(DBDisplayString(1), "Initial Catalog=", "") & ")"

        If Not Replace(DBDisplayString(0), "Data Source=", "") = "[SQLServerName]" Then

            '3.1.001
            'Check for problem test score import spec.  Fix if issue is present 
            'Dim cmdBase As New SqlCommand("update ltdb_subtest set SUBTEST = '7' where TEST_CODE = 'AP' and SUBTEST = '07' Update ltdb_subtest_score set SUBTEST = '7' where TEST_CODE = 'AP' and SUBTEST = '07'", New SqlConnection(connection))

            'MsgBox(cmdBase.CommandText)

            'cmdBase.Connection.Open()
            'cmdBase.ExecuteNonQuery()
            'cmdBase.Connection.Close()
            'cmdBase.Connection.Dispose()

            'End ' 3.1.001
        End If

        'Import.Refresh()

        'MsgBox(strConnLive & vbCrLf & strConnDev)

        Import.lblDisErr.Text = "The parameters were saved successfully!"

        Close()

        Import.RefreshDatasource()
        Import.Refresh()

    End Sub

    Private Sub btnShowMainPW_Click(sender As Object, e As EventArgs) Handles btnShowMainPW.Click

        If txtPassword.UseSystemPasswordChar = True Then

            txtPassword.UseSystemPasswordChar = False
            txtPassword.PasswordChar = ""
            'Me.Refresh()

        Else

            txtPassword.UseSystemPasswordChar = True
            txtPassword.PasswordChar = "*"
            'Me.Refresh()
        End If


    End Sub

    Private Sub btnShowDevPW_Click(sender As Object, e As EventArgs) Handles btnShowDevPW.Click

        If txtDEVPassword.UseSystemPasswordChar = True Then

            txtDEVPassword.UseSystemPasswordChar = False
            txtDEVPassword.PasswordChar = ""
            'Me.Refresh()

        Else

            txtDEVPassword.UseSystemPasswordChar = True
            txtDEVPassword.PasswordChar = "*"
            'Me.Refresh()
        End If


    End Sub

End Class