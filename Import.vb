#Region "Comment Header"
'----------------------------------------------------------------------------
' GITHUB Release Version
'----------------------------------------------------------------------------
' System Name:  AP Test Score Importer
'----------------------------------------------------------------------------
' REV       DATE        AUTHOR                  NOTES
' =======   ==========  ======================  ================================
' 7.1.001   8/27/2020   Gates                   GITHub Release Version
'----------------------------------------------------------------------------

#End Region

Option Infer On
Imports System
Imports System.Security
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.ComponentModel
Imports System.Linq
Imports System.Xml
Imports System.Xml.Linq
Imports System.Xml.XPath
Imports System.Net
Imports System.Windows

Public Class Import
    Dim Procfile As String = ""
    Dim chkFlag As Boolean = False
    Dim flgDBChkDisabled As Boolean = False
    Dim flgACTEnabled As Boolean = False
    Dim FontBold As Boolean = False
    Dim connection As String = strConnLive
    Dim objConnection As SqlConnection

    Private Sub FileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FileToolStripMenuItem.Click

        Dim fdlg As OpenFileDialog = New OpenFileDialog With {
            .Title = "Choose file to import("")",
            .InitialDirectory = Environment.CurrentDirectory & "\(A1) Import files", '"."
            .Filter = "All files (*.*)|*.*|All files (*.*)|*.*",
            .FilterIndex = 2,
            .RestoreDirectory = True
        }



        If fdlg.ShowDialog() = DialogResult.OK Then
            filedes.Text = fdlg.FileName
            Procfile = fdlg.FileName
        End If

    End Sub

    Private Sub fileD()

        Dim fdlg As OpenFileDialog = New OpenFileDialog With {
            .Title = "Choose file to import("")",
            .InitialDirectory = Environment.CurrentDirectory & "\(A1) Import files", '"."
            .Filter = "All files (*.*)|*.*|All files (*.*)|*.*",
            .FilterIndex = 2,
            .RestoreDirectory = True
        }


        If fdlg.ShowDialog() = DialogResult.OK Then
            filedes.Text = fdlg.FileName
            Procfile = fdlg.FileName
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub Import_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        'See if application has been updated.  If it has copy the user settings to the new application version!
        UpgradeMySetings()

        'Get district information from configuration files!
        GetPreferences()

        Me.Text = Me.Text & " Version: " & My.Application.Info.Version.Major & "." & My.Application.Info.Version.Minor & "." & My.Application.Info.Version.Build & "." & My.Application.Info.Version.Revision

        connection = strConnLive

        chkDataSource.Checked = True

        If DBDevServerDBNM = "" Then
            chkDataSource.Enabled = False
            flgDBChkDisabled = True
        End If

        '4.0.001
        ToolTip1.SetToolTip(btnCTC, "Copy text to clipboard")

        Try

            'Check and display a message if there is an AP score format and it is missing any of the new codes!
            Dim connect2 As New SqlConnection(connection)
            Dim command2 As New SqlCommand
            'Dim TestKey As String = ""
            Dim TKey As Integer = 0
            'Check LTDB_TEST for existing record
            connect2 = New SqlConnection(connection)

            connect2.Open()
            'command.Cancel()

            command2 = New SqlCommand("SELECT TOP (1) * FROM LTDB_TEST WHERE District = '" & strDistrict & "' AND TEST_CODE = 'AP' ", connect2)

            Dim reader1 As SqlDataReader =
                          command2.ExecuteReader(
                          CommandBehavior.CloseConnection)

            'Console.WriteLine(reader2.HasRows)
            If reader1.HasRows = False Then

            Else

                '28 Chinese Language and Culture
                Dim ConnectInit As SqlConnection
                Dim Command As SqlCommand
                Dim rowcount As Integer = 0
                ConnectInit = New SqlConnection(connection)

                ConnectInit.Open()
                'command.Cancel()

                Command = New SqlCommand("SELECT Count(*) AS [RowCount]  FROM LTDB_SUBTEST WHERE District = '" & strDistrict & "' AND TEST_CODE = 'AP' AND SUBTEST IN ('28','22','83','84','23') ", ConnectInit)

                Dim reader As SqlDataReader =
                                  Command.ExecuteReader(
                                  CommandBehavior.CloseConnection)

                'Console.WriteLine(reader2.HasRows)
                If reader.HasRows = False Then
                    'Then a spec update is needed.  Let the user know!
                    pbUpdateNeeded.Visible = True
                Else
                    While reader.Read
                        rowcount = reader.Item("RowCount")
                        Exit While
                    End While

                    If rowcount = 5 Then
                        'We have all elements
                    Else
                        pbUpdateNeeded.Visible = True

                    End If

                End If

                reader.Close()
                reader.Dispose()
                ConnectInit.Close()
                ConnectInit.Dispose()

                'For testing visibility
                'pbUpdateNeeded.Visible = True

            End If

            reader1.Close()
            reader1.Dispose()
            connect2.Close()
            connect2.Dispose()
        Catch ex As Exception

            MsgBox("Error encountered on initiation.  Please click ok and make sure your user account information defined in the parameters is correct. " & vbCrLf & "Additional Information:" & vbCrLf & "------->" & vbCrLf & ex.Message.ToString & vbCrLf & "<-------")
            Parameters.ShowDialog()
            Exit Sub

        End Try

    End Sub

    Private Sub chkDataSource_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkDataSource.CheckedChanged

        If chkDataSource.Checked = True Then

            connection = strConnLive

        Else

            connection = strConnDev

        End If

        Dim DBDisplayString As Array

        DBDisplayString = Split(connection, ";")

        If DBDisplayString.Length = 1 Then

            'something is not initialized properly!

        Else

            lblDatabaseLocation.Text = "(Current Database: " & Replace(DBDisplayString(1), "Initial Catalog=", "") & ")"

        End If


    End Sub

    Public Sub GetPreferences()

        'Check for user values and populate.  If none are present then prompt for them to be filled out!
        Dim MissingParam As Boolean = False
        'Check for a value then assign

        'District 
        If My.Settings.District.Length >= 1 Then
            strDistrict = My.Settings.District
        Else
            'This is a required field so this is a problem.  Display form for updating the information!
            MissingParam = True
            End If

            'DB Servername
            If My.Settings.DBServerName.Length > 1 Then
                DBServerNM = My.Settings.DBServerName
            Else
                'Field is required show update form!
                MissingParam = True
            End If

        'DB Server Database Name
        If My.Settings.DBServerDatabaseName.Length > 1 Then
                DBServerDBNM = My.Settings.DBServerDatabaseName
            Else
                'Field is required show form for update!
                MissingParam = True
            End If

            'DB Username 
            If My.Settings.DBUserName.Length > 1 Then
                DBUN = My.Settings.DBUserName
            Else
                'Required if missing need to display update form!
                MissingParam = True
            End If

            'DB Password
            If My.Settings.DBPass.Length > 1 Then
                DBPassword = My.Settings.DBPass
            Else
                'No value, flag this is a required field!
                MissingParam = True
            End If

            'SSPI
            If My.Settings.IntegratedSecurity.Length > 1 Then
                IntegratedSecurity = My.Settings.IntegratedSecurity
            Else
                'Default to False )Or the program will fail!
                My.Settings.IntegratedSecurity = "False"
                My.Settings.Save()
            End If

            'The remaining fields are optional-------------------------------------------------------------------------------------------------------------------

            'DB Dev Server Name (Optional)
            DBDevServerNM = My.Settings.DBDevServerName

            'DB Dev server database name (Optional)
            DBDevServerDBNM = My.Settings.DBDevServerDatabaseName

            'DB Dev Username (Optional) 
            DBDEVUN = My.Settings.DBDevUserName

            'DB Dev Password (Optional)
            DBDevPassword = My.Settings.DBDevPass

            'End Optional----------------------------------------------------------------------------------------------------------------------------------------

            If MissingParam = True Then
                'we need to show the dialog the program does not have the information to run :-(
                Parameters.ShowDialog()
                Exit Sub
            End If
            'Exit Sub

            'Need to make a decision on Integrated Security=SSPI or Integrated Security=False SSPI assumes the person running the app has database privs... *User can now choose!
            'Integrated Security = SSPI prevents uses without permissions from getting to the database -Removed!
            strConnLive = "Data Source=" & DBServerNM & ";Initial Catalog=" & DBServerDBNM & ";user Id=" & DBUN & ";Password=" & DBPassword & ";Integrated Security=" & IntegratedSecurity.ToString

            'Add development ini

            strConnDev = "Data Source=" & DBDevServerNM & ";Initial Catalog=" & DBDevServerDBNM & ";user Id=" & DBDEVUN & ";Password=" & DBDevPassword & ";Integrated Security=" & IntegratedSecurity.ToString

            If File.Exists("./Init.ini") Then
                ' File.Delete("./Init.ini")   'Enable this only after complete usercode is done.
            End If

            ' MsgBox(My.Settings.ACTEnabled.ToString + vbCrLf + My.Settings.DBDevPass.ToString + vbCrLf + My.Settings.DBDevServerDatabaseName.ToString + vbCrLf + My.Settings.DBDevServerName + vbCrLf + My.Settings.DBDevUserName + vbCrLf + My.Settings.DBPass + vbCrLf + My.Settings.DBServerDatabaseName + vbCrLf + My.Settings.DBServerName + vbCrLf + My.Settings.DBUserName + vbCrLf + My.Settings.District + vbCrLf)



    End Sub

    Public Sub CreateTestDefinition()

        Try

            'Look into the specified database and pull the AP test key to script the rest of the process!
            Dim connect2 As New SqlConnection(connection)
            Dim command2 As New SqlCommand
            'Dim TestKey As String = ""
            Dim TKey As Integer = 0
            'Check LTDB_TEST for existing record
            connect2 = New SqlConnection(connection)

            connect2.Open()
            'command.Cancel()

            command2 = New SqlCommand("SELECT TOP (1) * FROM LTDB_TEST WHERE District = '" & strDistrict & "' AND TEST_CODE = 'AP' ", connect2)

            Dim reader As SqlDataReader =
                          command2.ExecuteReader(
                          CommandBehavior.CloseConnection)

            'Console.WriteLine(reader2.HasRows)
            If reader.HasRows = False Then

                'We are good to continue with creation!
                reader.Close()
                reader.Dispose()

                'Create Base Test Definition by checking LTDB_TEST and getting the highest Test_Key then add 1
                connect2.Open()

                command2 = New SqlCommand("SELECT TOP (1) * FROM LTDB_TEST WHERE District = '" & strDistrict & "' ORDER BY TEST_KEY DESC ", connect2)

                reader = command2.ExecuteReader(
                              CommandBehavior.CloseConnection)

                'Console.WriteLine(reader2.HasRows)
                If reader.HasRows = False Then
                    'We are in trouble cannot create the record let the user know!
                    'MsgBox("Cannot determine next test key value!")


                    lblDisErr.Text = "An error occurred - Cannot create test key value!"

                    Exit Sub

                Else
                    'Grab the test key value
                    While reader.Read

                        TKey = CInt(reader.Item("Test_Key")) + 1

                    End While

                    reader.Close()
                    reader.Dispose()

                End If

                reader.Close()
                reader.Dispose()

                'Build base AP record

                'MsgBox(TKey)

                Dim cmdBase As New SqlCommand("SET NOCOUNT ON; " &
                                              "SET XACT_ABORT ON; " &
                                              "BEGIN TRANSACTION; " &
                                              "INSERT INTO [dbo].[LTDB_TEST]([DISTRICT], [TEST_CODE], [TEST_LEVEL], [TEST_FORM], [TEST_KEY], [DESCRIPTION], [DISPLAY], [SEC_PACKAGE], [SEC_SUBPACKAGE], [SEC_FEATURE], [TEACHER_DISPLAY], [SUB_DISPLAY], [INCLUDE_PERFPLUS], [PESC_CODE], [CHANGE_DATE_TIME], [CHANGE_UID]) " &
                                              "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'Advanced Placement Tests', N'N', N'LTDB      ', N'REPORTS   ', N'RUNRPT    ', N'R', N'N', N'N', NULL, '20101117 11:40:48.000', N'TEST.IMPORTER' " &
                                              "COMMIT; ", New SqlConnection(connection))

                'MsgBox(cmdBase.CommandText)

                cmdBase.Connection.Open()
                cmdBase.ExecuteNonQuery()
                cmdBase.Connection.Close()
                cmdBase.Connection.Dispose()

                'So the base record is now added.  Let's add the subtest and subtest scores and we are good to start importing!

                Dim cmdSUBTEST As New SqlCommand("SET NOCOUNT ON; " &
                                            "SET XACT_ABORT ON; " &
                                            "BEGIN TRANSACTION; " &
                                            "INSERT INTO [dbo].[LTDB_SUBTEST]([DISTRICT], [TEST_CODE], [TEST_LEVEL], [TEST_FORM], [TEST_KEY], [SUBTEST], [DESCRIPTION], [SUBTEST_ORDER], [DISPLAY], [STATE_CODE_EQUIV], [PESC_CODE], [CHANGE_DATE_TIME], [CHANGE_UID]) " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'7         ', N'United States History', 37, N'Y', NULL, NULL, '20101108 12:12:36.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'14        ', N'Art: Studio Art-Drawing', 4, N'Y', NULL, NULL, '20101108 13:26:15.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'15        ', N'Art: Studio Art-2-D Design', 2, N'Y', NULL, NULL, '20101108 13:26:15.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'16        ', N'Art: Studio Art-3-D Design', 3, N'Y', NULL, NULL, '20101108 13:26:15.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'20        ', N'Biology', 5, N'Y', NULL, NULL, '20101108 13:26:15.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'25        ', N'Chemistry', 9, N'Y', NULL, NULL, '20121116 09:50:29.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'31        ', N'Computer Science A', 11, N'Y', NULL, NULL, '20101108 13:26:15.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'34        ', N'Economics: Microeconomics', 13, N'Y', NULL, NULL, '20101109 12:48:48.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'35        ', N'Economics: Macroeconomics', 12, N'Y', NULL, NULL, '20101109 12:48:48.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'36        ', N'English Language and Composition', 14, N'Y', NULL, NULL, '20101109 12:48:48.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'37        ', N'English Literature and Composition', 15, N'Y', NULL, NULL, '20101109 12:48:48.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'40        ', N'Environmental Science', 16, N'Y', NULL, NULL, '20101118 08:23:01.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'43        ', N'European History', 17, N'Y', NULL, NULL, '20101118 08:23:01.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'48        ', N'French Language', 18, N'Y', NULL, NULL, '20101118 08:23:01.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'53        ', N'Human Geography', 22, N'Y', NULL, NULL, '20101118 08:23:01.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'55        ', N'German Language', 19, N'Y', NULL, NULL, '20101118 08:23:01.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'57        ', N'United States:Government and Politics', 21, N'Y', NULL, NULL, '20101118 08:23:01.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'58        ', N'Comparative:Government and Politics', 20, N'Y', NULL, NULL, '20101118 08:23:01.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'62        ', N'Italian Language and Culture', 23, N'Y', NULL, NULL, '20101118 08:23:01.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'66        ', N'Calculus AB', 6, N'Y', NULL, NULL, '20101118 08:23:01.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'68        ', N'Calculus BC', 7, N'Y', NULL, NULL, '20101118 08:23:01.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'69        ', N'Calculus BC - Calculus AB Subscore', 8, N'Y', NULL, NULL, '20121116 09:53:10.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'76        ', N'Music Theory - Aural Subscore', 27, N'Y', NULL, NULL, '20121116 09:53:46.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'77        ', N'Music Theory - Non-Aural Subscore', 28, N'Y', NULL, NULL, '20121116 09:55:13.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'80        ', N'Physics C - Mechanics', 32, N'Y', NULL, NULL, '20101118 08:23:01.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'85        ', N'Psychology', 33, N'Y', NULL, NULL, '20101118 08:23:01.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'87        ', N'Spanish Language', 34, N'Y', NULL, NULL, '20101118 08:23:01.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'89        ', N'Spanish Literature', 35, N'Y', NULL, NULL, '20101118 08:23:01.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'90        ', N'Statistics', 36, N'Y', NULL, NULL, '20101118 08:23:01.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'93        ', N'World History', 38, N'Y', NULL, NULL, '20101118 08:23:01.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'75        ', N'Music Theory', 26, N'Y', NULL, NULL, '20121116 09:56:34.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'13        ', N'Art History', 1, N'Y', NULL, NULL, '20121116 11:28:26.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'64        ', N'Japanese Language and Culture', 24, N'Y', NULL, NULL, '20121116 11:28:26.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'60        ', N'Latin', 25, N'Y', NULL, NULL, '20121116 11:28:26.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'82        ', N'Physics C - Electricity and Magnetism', 31, N'Y', NULL, NULL, '20121116 11:28:27.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'22        ', N'Seminar', 39, N'Y', NULL, NULL, '20150811 12:12:36.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'28        ', N'Chinese Language and Culture', 10, N'Y', NULL, NULL, '20150811 12:12:36.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'83        ', N'Physics 1', 29, N'Y', NULL, NULL, '20150811 12:12:36.000', N'TEST.IMPORTER' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'84        ', N'Physics 2', 30, N'Y', NULL, NULL, '20150811 12:12:36.000', N'TEST.IMPORTER' " &
                                            "COMMIT;", New SqlConnection(connection))

                'MsgBox(cmdBase.CommandText)

                cmdSUBTEST.Connection.Open()
                cmdSUBTEST.ExecuteNonQuery()
                cmdSUBTEST.Connection.Close()
                cmdSUBTEST.Connection.Dispose()

                'Now Add Sub Score items!

                Dim cmdSUBTESTSC As New SqlCommand("SET NOCOUNT ON; " &
                                            "SET XACT_ABORT ON; " &
                                            "BEGIN TRANSACTION; " &
                                            "INSERT INTO [dbo].[LTDB_SUBTEST_SCORE]([DISTRICT], [TEST_CODE], [TEST_LEVEL], [TEST_FORM], [TEST_KEY], [SUBTEST], [SCORE_CODE], [SCORE_ORDER], [SCORE_LABEL], [REQUIRED], [FIELD_TYPE], [DATA_TYPE], [NUMBER_TYPE], [DATA_LENGTH], [FIELD_SCALE], [FIELD_PRECISION], [DEFAULT_VALUE], [VALIDATION_LIST], [VALIDATION_TABLE], [CODE_COLUMN], [DESCRIPTION_COLUMN], [DISPLAY], [INCLUDE_DASHBOARD], [MONTHS_TO_INCLUDE], [RANGE1_HIGH_LIMIT], [RANGE2_HIGH_LIMIT], [STATE_CODE_EQUIV], [SCORE_TYPE], [PERFPLUS_GROUP], [PESC_CODE], [CHANGE_DATE_TIME], [CHANGE_UID]) " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'7        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 10:12:27.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'40        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 08:25:37.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'14        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:15:47.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'15        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:16:43.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'16        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:17:03.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'20        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:17:29.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'25        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:17:42.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'31        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:17:56.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'34        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:18:30.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'35        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:19:19.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'40        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:20:17.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'48        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:20:45.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'55        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:21:33.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'58        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:22:48.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'62        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:23:35.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'66        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:23:51.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'25        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 08:01:42.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'14        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 10:13:21.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'68        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:24:05.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'69        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:24:21.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'76        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:24:37.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'77        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:24:54.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'80        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:25:08.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'85        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:25:25.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'87        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:25:42.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'89        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:26:03.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'93        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:26:36.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'15        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 10:13:32.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'16        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 08:23:31.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'20        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 08:23:47.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'31        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 08:24:12.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'34        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 08:24:44.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'35        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 08:24:59.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'36        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 08:25:10.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'37        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 08:25:22.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'43        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 08:25:51.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'48        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 08:26:06.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'53        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 08:27:10.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'55        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 08:27:24.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'57        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 08:27:36.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'58        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 08:27:49.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'62        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 08:28:15.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'66        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 08:28:29.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'68        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 08:28:42.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'69        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 08:28:55.000', N'JGATES' " &
                                            "COMMIT; " &
                                            "BEGIN TRANSACTION; " &
                                            "INSERT INTO [dbo].[LTDB_SUBTEST_SCORE]([DISTRICT], [TEST_CODE], [TEST_LEVEL], [TEST_FORM], [TEST_KEY], [SUBTEST], [SCORE_CODE], [SCORE_ORDER], [SCORE_LABEL], [REQUIRED], [FIELD_TYPE], [DATA_TYPE], [NUMBER_TYPE], [DATA_LENGTH], [FIELD_SCALE], [FIELD_PRECISION], [DEFAULT_VALUE], [VALIDATION_LIST], [VALIDATION_TABLE], [CODE_COLUMN], [DESCRIPTION_COLUMN], [DISPLAY], [INCLUDE_DASHBOARD], [MONTHS_TO_INCLUDE], [RANGE1_HIGH_LIMIT], [RANGE2_HIGH_LIMIT], [STATE_CODE_EQUIV], [SCORE_TYPE], [PERFPLUS_GROUP], [PESC_CODE], [CHANGE_DATE_TIME], [CHANGE_UID]) " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'76        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 08:29:10.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'77        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 08:29:25.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'80        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 08:29:37.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'85        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 08:29:49.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'87        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 08:30:06.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'89        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 08:30:19.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'90        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 08:30:31.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'93        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 08:30:44.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'7        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:12:19.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'36        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:19:36.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'37        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:20:02.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'43        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:20:31.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'53        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:21:14.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'57        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:21:50.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'90        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:26:22.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'7        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:42:25.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'7        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 07:42:53.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'14        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:43:34.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'15        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:44:02.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'20        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:44:45.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'25        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:45:03.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'31        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:45:24.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'34        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:46:08.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'35        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:46:26.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'36        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:46:43.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'37        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:47:00.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'40        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:47:16.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'43        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:47:41.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'16        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:44:26.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'48        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:47:58.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'53        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:48:30.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'62        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:50:23.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'69        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:51:15.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'76        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:51:35.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'77        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:51:54.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'80        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:52:15.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'85        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:52:35.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'87        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:52:59.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'89        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:53:15.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'90        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:53:32.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'93        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:53:50.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'14        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 07:54:42.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'15        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 07:54:58.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'16        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 07:55:15.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'20        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 07:55:30.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'25        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 07:56:24.000', N'JGATES' " &
                                            "COMMIT; " &
                                            "BEGIN TRANSACTION; " &
                                            "INSERT INTO [dbo].[LTDB_SUBTEST_SCORE]([DISTRICT], [TEST_CODE], [TEST_LEVEL], [TEST_FORM], [TEST_KEY], [SUBTEST], [SCORE_CODE], [SCORE_ORDER], [SCORE_LABEL], [REQUIRED], [FIELD_TYPE], [DATA_TYPE], [NUMBER_TYPE], [DATA_LENGTH], [FIELD_SCALE], [FIELD_PRECISION], [DEFAULT_VALUE], [VALIDATION_LIST], [VALIDATION_TABLE], [CODE_COLUMN], [DESCRIPTION_COLUMN], [DISPLAY], [INCLUDE_DASHBOARD], [MONTHS_TO_INCLUDE], [RANGE1_HIGH_LIMIT], [RANGE2_HIGH_LIMIT], [STATE_CODE_EQUIV], [SCORE_TYPE], [PERFPLUS_GROUP], [PESC_CODE], [CHANGE_DATE_TIME], [CHANGE_UID]) " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'35        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 07:59:19.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'55        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:48:55.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'57        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:49:11.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'58        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:49:35.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'66        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:50:40.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'68        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:50:57.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'31        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 07:56:43.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'34        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 07:59:03.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'36        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 08:02:21.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'37        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 08:02:48.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'40        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 08:03:07.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'43        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 09:20:24.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'48        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 09:20:43.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'53        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 09:21:25.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'55        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 09:21:55.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'57        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 09:22:13.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'58        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 09:22:32.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'62        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 09:23:13.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'66        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 09:23:34.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'68        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 09:24:00.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'69        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 09:24:54.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'76        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 09:25:13.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'77        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 09:25:28.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'80        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 09:25:44.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'85        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 09:26:05.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'87        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 09:26:21.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'89        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 09:26:36.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'90        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 09:26:57.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'93        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 09:27:13.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'75        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20121116 09:58:13.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'75        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20121116 09:58:43.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'75        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20121116 09:59:18.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'75        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20121116 09:59:42.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'13        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20121116 11:30:02.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'13        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20121116 11:31:04.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'13        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20121116 11:31:46.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'13        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20121116 11:32:17.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'60        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20121116 11:32:55.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'60        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20121116 11:33:13.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'60        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20121116 11:33:32.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'60        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20121116 11:33:54.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'64        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20121116 11:34:21.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'64        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20121116 11:34:37.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'64        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20121116 11:34:55.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'64        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20121116 11:35:16.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'82        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20121116 12:14:08.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'82        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20121116 12:14:37.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'82        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20121116 12:15:45.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'82        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20121116 11:38:04.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'28        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:15:47.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'28        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 10:12:27.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'28        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:42:25.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'28        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 07:42:53.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'22        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:15:47.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'22        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 10:12:27.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'22        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:42:25.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'22        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 07:42:53.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'83        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:15:47.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'83        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 10:12:27.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'83        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:42:25.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'83        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 07:42:53.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'84        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:15:47.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'84        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 10:12:27.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'84        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:42:25.000', N'JGATES' UNION ALL " &
                                            "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'84        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 07:42:53.000', N'JGATES' " &
                                            "COMMIT;", New SqlConnection(connection))

                'MsgBox(cmdSUBTESTSC.CommandText)

                cmdSUBTESTSC.Connection.Open()
                cmdSUBTESTSC.ExecuteNonQuery()
                cmdSUBTESTSC.Connection.Close()
                cmdSUBTESTSC.Connection.Dispose()

                'MsgBox("Complete!")

                'New notification display!  

                lblDisErr.Text = "The AP Test Score Definition was created successfully!"



            Else

                'Trouble time to exit!


                Dim DBDisplayString As Array
                Dim DBString As String = ""

                DBDisplayString = Split(connection, ";")

                DBString = "(Current Database: " & Replace(DBDisplayString(1), "Initial Catalog=", "") & ")"

                lblDisErr.Text = "Base AP test record already exists in " & DBString & " !!  Cannot re-create it!!"

                Exit Sub

            End If

        Catch ex As Exception

            lblDisErr.Text = "An error occurred and the test import specification was not created!"

        End Try


    End Sub

    Private Sub CreateAPTestDefinitionToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CreateAPTestDefinitionToolStripMenuItem.Click

        Call CreateTestDefinition()

    End Sub

    Private Sub btnAP_Click(sender As System.Object, e As System.EventArgs) Handles btnAP.Click

        'AP SCORE IMPORT

        'Try 
        btnAP.Enabled = False
        RT1.Text = ""
        lblDisErr.Text = ""
        lblComplete.Visible = False
        chkTestOnly.Enabled = False
        chkDataSource.Enabled = False

        'Now let's read the file contents and put it in the database!
        Dim FullPath As String = ""
        FullPath = (Procfile)
        Dim testdate As String = ""
        Dim objDate As Date
        Dim delimiter As String = ""

        delimiter = ","

        If FullPath = "" Then
            MsgBox("A file must be selected for import!")
            Call fileD()
            btnAP.Enabled = True
            Exit Sub
        End If

        'Clear text box for run!
        RT1.Clear()

        Dim Counter As Integer
        Dim LastName As String = ""
        Dim FirstName As String = ""
        Dim SSex As String = ""
        Dim s As String = Nothing
        Dim sites As String() = Nothing
        Dim RouteInd As String = Nothing
        Dim sr As New StreamReader(FullPath)
        Dim strID As String = ""
        Dim examDate As String = ""
        Dim examDate1 As String = ""
        Dim examDate2 As String = ""
        Dim examDate3 As String = ""
        Dim examDate4 As String = ""
        Dim examDate5 As String = ""
        Dim examDate6 As String = ""
        Dim examDate7 As String = ""
        Dim examDate8 As String = ""
        Dim examDate9 As String = ""
        Dim examDate10 As String = ""
        Dim examDate11 As String = ""
        Dim examDate12 As String = ""
        Dim examDate13 As String = ""
        Dim examDate14 As String = ""
        Dim examDate15 As String = ""
        Dim examDate16 As String = ""
        Dim examDate17 As String = ""
        Dim examDate18 As String = ""
        Dim examDate19 As String = ""
        Dim examDate20 As String = ""
        Dim examDate21 As String = ""
        Dim examDate22 As String = ""
        Dim examDate23 As String = ""
        Dim examDate24 As String = ""
        Dim examDate25 As String = ""
        Dim examDate26 As String = ""
        Dim examDate27 As String = ""
        Dim examDate28 As String = ""
        Dim examDate29 As String = ""
        Dim examDate30 As String = ""
        Dim testCode As String = ""
        Dim testKey As String = ""
        Dim examCode1 As String = ""
        Dim examYear1 As String = ""
        Dim examScore1 As String = ""
        Dim examCode2 As String = ""
        Dim examYear2 As String = ""
        Dim examScore2 As String = ""
        Dim examCode3 As String = ""
        Dim examYear3 As String = ""
        Dim examScore3 As String = ""
        Dim examCode4 As String = ""
        Dim examYear4 As String = ""
        Dim examScore4 As String = ""
        Dim examCode5 As String = ""
        Dim examYear5 As String = ""
        Dim examScore5 As String = ""
        Dim examCode6 As String = ""
        Dim examYear6 As String = ""
        Dim examScore6 As String = ""
        Dim examCode7 As String = ""
        Dim examYear7 As String = ""
        Dim examScore7 As String = ""
        Dim examCode8 As String = ""
        Dim examYear8 As String = ""
        Dim examScore8 As String = ""
        Dim examCode9 As String = ""
        Dim examYear9 As String = ""
        Dim examScore9 As String = ""
        Dim examCode10 As String = ""
        Dim examYear10 As String = ""
        Dim examScore10 As String = ""
        Dim examCode11 As String = ""
        Dim examYear11 As String = ""
        Dim examScore11 As String = ""
        Dim examCode12 As String = ""
        Dim examYear12 As String = ""
        Dim examScore12 As String = ""
        Dim examCode13 As String = ""
        Dim examYear13 As String = ""
        Dim examScore13 As String = ""
        Dim examCode14 As String = ""
        Dim examYear14 As String = ""
        Dim examScore14 As String = ""
        Dim examCode15 As String = ""
        Dim examYear15 As String = ""
        Dim examScore15 As String = ""
        Dim examCode16 As String = ""
        Dim examYear16 As String = ""
        Dim examScore16 As String = ""
        Dim examCode17 As String = ""
        Dim examYear17 As String = ""
        Dim examScore17 As String = ""
        Dim examCode18 As String = ""
        Dim examYear18 As String = ""
        Dim examScore18 As String = ""
        Dim examCode19 As String = ""
        Dim examYear19 As String = ""
        Dim examScore19 As String = ""
        Dim examCode20 As String = ""
        Dim examYear20 As String = ""
        Dim examScore20 As String = ""
        Dim examCode21 As String = ""
        Dim examYear21 As String = ""
        Dim examScore21 As String = ""
        Dim examCode22 As String = ""
        Dim examYear22 As String = ""
        Dim examScore22 As String = ""
        Dim examCode23 As String = ""
        Dim examYear23 As String = ""
        Dim examScore23 As String = ""
        Dim examCode24 As String = ""
        Dim examYear24 As String = ""
        Dim examScore24 As String = ""
        Dim examCode25 As String = ""
        Dim examYear25 As String = ""
        Dim examScore25 As String = ""
        Dim examCode26 As String = ""
        Dim examYear26 As String = ""
        Dim examScore26 As String = ""
        Dim examCode27 As String = ""
        Dim examYear27 As String = ""
        Dim examScore27 As String = ""
        Dim examCode28 As String = ""
        Dim examYear28 As String = ""
        Dim examScore28 As String = ""
        Dim examCode29 As String = ""
        Dim examYear29 As String = ""
        Dim examScore29 As String = ""
        Dim examCode30 As String = ""
        Dim examYear30 As String = ""
        Dim examScore30 As String = ""
        Dim adminYear1 As String = ""
        Dim adminYear2 As String = ""
        Dim adminYear3 As String = ""
        Dim adminYear4 As String = ""
        Dim adminYear5 As String = ""
        Dim adminYear6 As String = ""
        Dim adminYear7 As String = ""
        Dim adminYear8 As String = ""
        Dim adminYear9 As String = ""
        Dim adminYear10 As String = ""
        Dim adminYear11 As String = ""
        Dim adminYear12 As String = ""
        Dim adminYear13 As String = ""
        Dim adminYear14 As String = ""
        Dim adminYear15 As String = ""
        Dim adminYear16 As String = ""
        Dim adminYear17 As String = ""
        Dim adminYear18 As String = ""
        Dim adminYear19 As String = ""
        Dim adminYear20 As String = ""
        Dim adminYear21 As String = ""
        Dim adminYear22 As String = ""
        Dim adminYear23 As String = ""
        Dim adminYear24 As String = ""
        Dim adminYear25 As String = ""
        Dim adminYear26 As String = ""
        Dim adminYear27 As String = ""
        Dim adminYear28 As String = ""
        Dim adminYear29 As String = ""
        Dim adminYear30 As String = ""
        Dim birthdate As String = ""
        Dim IReg11 As String = ""
        Dim IReg12 As String = ""
        Dim IReg21 As String = ""
        Dim IReg22 As String = ""
        Dim IReg31 As String = ""
        Dim IReg32 As String = ""
        Dim IReg41 As String = ""
        Dim IReg42 As String = ""
        Dim IReg51 As String = ""
        Dim IReg52 As String = ""
        Dim IReg61 As String = ""
        Dim IReg62 As String = ""
        Dim IReg71 As String = ""
        Dim IReg72 As String = ""
        Dim IReg81 As String = ""
        Dim IReg82 As String = ""
        Dim IReg91 As String = ""
        Dim IReg92 As String = ""
        Dim IReg101 As String = ""
        Dim IReg102 As String = ""
        Dim IReg111 As String = ""
        Dim IReg112 As String = ""
        Dim IReg121 As String = ""
        Dim IReg122 As String = ""
        Dim IReg131 As String = ""
        Dim IReg132 As String = ""
        Dim IReg141 As String = ""
        Dim IReg142 As String = ""
        Dim IReg151 As String = ""
        Dim IReg152 As String = ""
        Dim IReg161 As String = ""
        Dim IReg162 As String = ""
        Dim IReg171 As String = ""
        Dim IReg172 As String = ""
        Dim IReg181 As String = ""
        Dim IReg182 As String = ""
        Dim IReg191 As String = ""
        Dim IReg192 As String = ""
        Dim IReg201 As String = ""
        Dim IReg202 As String = ""
        Dim IReg211 As String = ""
        Dim IReg212 As String = ""
        Dim IReg221 As String = ""
        Dim IReg222 As String = ""
        Dim IReg231 As String = ""
        Dim IReg232 As String = ""
        Dim IReg241 As String = ""
        Dim IReg242 As String = ""
        Dim IReg251 As String = ""
        Dim IReg252 As String = ""
        Dim IReg261 As String = ""
        Dim IReg262 As String = ""
        Dim IReg271 As String = ""
        Dim IReg272 As String = ""
        Dim IReg281 As String = ""
        Dim IReg282 As String = ""
        Dim IReg291 As String = ""
        Dim IReg292 As String = ""
        Dim IReg301 As String = ""
        Dim IReg302 As String = ""

        Dim cmpAge As String = ""
        Dim stuBuilding As String = ""
        Dim recProc As Integer = 0
        Dim errFlag As Boolean = False
        Dim connect2 As New SqlConnection(connection)
        Dim command2 As New SqlCommand
        lblRProc.Text = ""

        RT1.BackColor = (Drawing.Color.White)
        RT1.SelectionFont = New Font("Arial", 10, Drawing.FontStyle.Regular)

        '7.1.001 - New file read logic

        Dim afile As FileIO.TextFieldParser = New FileIO.TextFieldParser(FullPath)
            Dim CurrentRecord As String() ' this array will hold each line of data
            afile.TextFieldType = FileIO.FieldType.Delimited
            afile.Delimiters = New String() {","}
            afile.HasFieldsEnclosedInQuotes = True

            ' parse the actual file
            Do While Not afile.EndOfData
                Try

                recProc = recProc + 1

                If RunInd = True Then
                        Exit Do
                        Exit Sub
                        Me.Close()
                    End If

                    Counter = Counter + 1

                    CurrentRecord = afile.ReadFields

                'MsgBox(CurrentRecord(0).ToString)

                If CurrentRecord.Length = "244" Then ' 3.0.003
                        lblType.Text = "AP Test Scores"
                        testCode = "AP"
                        'testKey = "13"
                        Me.Refresh()
                    Else
                        lblType.Text = "Unknown"

                        If FontBold = False Then
                            FontBold = True
                            RT1.SelectionFont = New Font("Arial", 10, Drawing.FontStyle.Bold)
                            RT1.SelectionColor = Color.DarkSeaGreen
                            'RT1.SelectionFont.ForeColor.ToString()
                        Else
                            FontBold = False
                            RT1.SelectionFont = New Font("Arial", 10, Drawing.FontStyle.Bold)
                            RT1.SelectionFont = New Font(RT1.SelectionFont, Drawing.FontStyle.Bold + Drawing.FontStyle.Italic)
                            RT1.SelectionColor = Color.DarkSeaGreen
                        End If
                        RT1.AppendText("Record Number: " & recProc & " Record Length: " & CurrentRecord.Length.ToString & " | " & "Is not the proper length for an AP test record.  The line will be ignored." & vbNewLine)
                        'RT1.ForeColor = (Drawing.Color.Black)
                        'RT1.AppendText("____________________________________________________________________________________________________________________________________________")
                        RT1.AppendText(vbNewLine)

                        errFlag = True
                        GoTo 9
                        ' Exit Sub

                    End If

                    lblRProc.Text = "Processing Record #: " & recProc.ToString

                'Test code

                ' Dim input As String = arrContents.ToString

                'strContents = Replace(strContents, "'", "''") 'Close any database quotes!
                FirstName = Trim(CurrentRecord(2).ToString) '.Replace("'", "''"))
                LastName = Trim(CurrentRecord(1).ToString) '.Replace("'", "''"))
                SSex = Trim(CurrentRecord(12).ToString)
                'MsgBox(FirstName)
                'Is student id present in the file?  If so attempt to get information!
                'MsgBox(Trim(arrContents(241).ToString))
                '2.1.003 - Start
                If Trim(CurrentRecord(241).ToString.Length) > 0 Then

                    If Trim(CurrentRecord(241).ToString) = "Student Identifier" Then
                        recProc = recProc - 1 ' Header record, take it out of the count!
                        Continue Do 'This is the header record, time to move to the next record.
                    End If

                    strID = Trim(CurrentRecord(241).ToString)

                        connect2 = New SqlConnection(connection)

                        Try

                            connect2.Open()

                        Catch ex As Exception
                            MsgBox("Unable to connect to database.  Make sure you have the proper user credentials set up on the parameters screen.  If your connection parameters are correct, please insure you have connectivity to the database server. If you are still encountering problems, please email me at dimante@jmgservices.org and let me know the error code 0x000801 has occurred.  The parameters window will open when you click ok.")
                            btnAP.Enabled = True
                            chkTestOnly.Enabled = True
                            lblRProc.Text = ""
                            Parameters.ShowDialog()
                            Exit Sub

                        End Try

                        command2 = New SqlCommand("SELECT Student_ID, Building, CONVERT(char(10), BIRTHDATE, 101) AS Birth, LAST_NAME FROM REG WHERE DISTRICT = '" & strDistrict & "' AND Student_ID = '" & strID & "' ", connect2)

                        Dim reader2 As SqlDataReader =
                              command2.ExecuteReader(
                              CommandBehavior.CloseConnection)

                        'Console.WriteLine(reader2.HasRows)
                        If reader2.HasRows = False Then
                            'Student was not found with the id.  Let's try the conventional way!

                            connect2.Close()
                            connect2.Dispose()
                            reader2.Close()
                            reader2.Dispose()

                            birthdate = Trim(CurrentRecord(13).ToString)

                            If birthdate.Length = 5 Then
                            'Month has 0 truncated add it in!!
                            birthdate = "0" & birthdate
                        End If

                        If birthdate.Length = 6 Then

                            If birthdate.Substring(2, 3) = "19" Then
                                birthdate = birthdate.Substring(0, 1) & "/" & birthdate.Substring(1, 1) & "/" & birthdate.Substring(2, 4)
                            Else
                                'Four digit year
                                birthdate = birthdate.Substring(0, 2) & "/" & birthdate.Substring(2, 2) & "/" & birthdate.Substring(4, 2)
                                End If

                            End If

                            If birthdate = "00/00/00" Or birthdate = "" Then
                                'skip
                                birthdate = ""
                            Else
                                objDate = Date.Parse(birthdate).ToShortDateString

                                birthdate = objDate
                            End If

                            'Go through eSchool and see if you can find a student match.

                            Dim connect2nd As New SqlConnection(connection)

                            connect2nd.Open()
                            'command.Cancel()

                            Dim command2nd As New SqlCommand("SELECT Student_ID, Building, CONVERT(char(10), BIRTHDATE, 101) AS Birth FROM REG WHERE DISTRICT = '" & strDistrict & "' AND Last_Name = '" & LastName.ToString.Replace("'", "''") & "' AND First_Name = '" & FirstName.ToString.Replace("'", "''") & "' AND Birthdate = '" & birthdate & "' AND Gender = '" & SSex & "' ", connect2nd)

                            Dim reader2nd As SqlDataReader =
                                  command2nd.ExecuteReader(
                                  CommandBehavior.CloseConnection)

                            'Console.WriteLine(reader2.HasRows))
                            If reader2nd.HasRows = False Then
                                'Student was not found

                                If FontBold = False Then
                                    FontBold = True
                                    RT1.SelectionFont = New Font("Arial", 10, Drawing.FontStyle.Bold)
                                    RT1.SelectionColor = Color.DodgerBlue
                                    'RT1.SelectionFont.ForeColor.ToString()
                                Else
                                    FontBold = False
                                    RT1.SelectionFont = New Font("Arial", 10, Drawing.FontStyle.Bold)
                                    RT1.SelectionFont = New Font(RT1.SelectionFont, Drawing.FontStyle.Bold + Drawing.FontStyle.Italic)
                                    RT1.SelectionColor = Color.DodgerBlue
                                End If
                                RT1.AppendText("Student information was not found for record number and both attempts to find student failed line: " & Counter & " LastName: " & LastName & " | " & " FirstName: " & FirstName & " | Gender: " & SSex & vbNewLine)
                                'RT1.ForeColor = (Drawing.Color.Black)
                                'RT1.AppendText("______________________________________________________________________________________________________________________________________________")
                                'RT1.AppendText(vbNewLine)
                                RT1.AppendText(vbNewLine)

                                connect2nd.Close()
                                connect2nd.Dispose()
                                reader2nd.Close()
                                reader2nd.Dispose()

                                Continue Do

                            Else

                                While reader2nd.Read()

                                    'If (reader2.Item("HIGHEST_ID_USED")) = "0" Then
                                    'We have no id setup in the system... time to let the user know and exit.
                                    'Else

                                    strID = Trim(reader2nd.Item("Student_ID").ToString)
                                    'Need the building number for the matched record also!!! 
                                    stuBuilding = Trim(reader2nd.Item("Building").ToString)
                                    birthdate = Trim(reader2nd.Item("Birth").ToString)
                                    'End If

                                End While

                                If birthdate.Length = 5 Then
                                    'Month has 0 truncated add it in!!
                                    birthdate = "0" + birthdate
                                End If

                                If birthdate.Length = 6 Then

                                    ' MsgBox(birthdate.Substring(2, 2).ToString)
                                    'If birthdate.Substring(2, 3) = "19" And birthdate.Substring(0, 1) = "0" Then
                                    'Two digit year
                                    'birthdate = birthdate.Substring(0, 1) & "/" & birthdate.Substring(1, 1) & "/" & birthdate.Substring(4, 2)

                                    If birthdate.Substring(2, 3) = "19" Then
                                        birthdate = birthdate.Substring(0, 1) & "/" & birthdate.Substring(1, 1) & "/" & birthdate.Substring(2, 4)
                                    Else
                                        'Four digit year
                                        birthdate = birthdate.Substring(0, 2) & "/" & birthdate.Substring(2, 2) & "/" & birthdate.Substring(4, 2)
                                    End If

                                End If


                                If birthdate = "00/00/00" Or birthdate = "" Then
                                    'skip
                                    birthdate = ""
                                Else
                                    objDate = Date.Parse(birthdate).ToShortDateString

                                    birthdate = objDate
                                End If

                                connect2nd.Close()
                                connect2nd.Dispose()
                                reader2nd.Close()
                                reader2nd.Dispose()
                                'Continue

                            End If


                        Else

                            Dim LNC As String = ""

                            While reader2.Read()

                                'Need the building number for the matched record also!!! -Gates 1/7/2013
                                stuBuilding = Trim(reader2.Item("Building").ToString)
                                birthdate = Trim(reader2.Item("Birth").ToString)
                                LNC = Trim(reader2.Item("LAST_NAME").ToString)
                            End While

                            'birthdate = Trim(arrContents(13).ToString)

                            If birthdate.Length = 5 Then
                                'Month has 0 truncated add it in!!
                                birthdate = "0" + birthdate
                            End If

                            If birthdate.Length = 6 Then

                                ' MsgBox(birthdate.Substring(2, 2).ToString)
                                'If birthdate.Substring(2, 3) = "19" And birthdate.Substring(0, 1) = "0" Then
                                'Two digit year
                                'birthdate = birthdate.Substring(0, 1) & "/" & birthdate.Substring(1, 1) & "/" & birthdate.Substring(4, 2)

                                If birthdate.Substring(2, 3) = "19" Then
                                    birthdate = birthdate.Substring(0, 1) & "/" & birthdate.Substring(1, 1) & "/" & birthdate.Substring(2, 4)
                                Else
                                    'Four digit year
                                    birthdate = birthdate.Substring(0, 2) & "/" & birthdate.Substring(2, 2) & "/" & birthdate.Substring(4, 2)
                                End If

                            End If


                            If birthdate = "00/00/00" Or birthdate = "" Then
                                'skip
                                birthdate = ""
                            Else
                                objDate = Date.Parse(birthdate).ToShortDateString

                                birthdate = objDate
                            End If

                            connect2.Close()
                            connect2.Dispose()
                            reader2.Close()
                            reader2.Dispose()


                            'does the last name match for the record retrieved?  If it does great, if not log it and continue!

                            If LCase(LastName) = LCase(LNC) Then

                                'we are clear

                            Else


                                If FontBold = False Then
                                    FontBold = True
                                    RT1.SelectionFont = New Font("Arial", 10, Drawing.FontStyle.Bold)
                                    RT1.SelectionColor = Color.Chocolate
                                    'RT1.SelectionFont.ForeColor.ToString()
                                Else
                                    FontBold = False
                                    RT1.SelectionFont = New Font("Arial", 10, Drawing.FontStyle.Bold)
                                    RT1.SelectionFont = New Font(RT1.SelectionFont, Drawing.FontStyle.Bold + Drawing.FontStyle.Italic)
                                    RT1.SelectionColor = Color.Chocolate
                                End If
                                RT1.AppendText("Student id does not match last name of student in test record.  Check Line: " & Counter & " LastName: " & LastName & " | " & " FirstName: " & FirstName & " | Gender: " & SSex & vbNewLine)
                                'RT1.ForeColor = (Drawing.Color.Black)
                                'RT1.AppendText("______________________________________________________________________________________________________________________________________________")
                                'RT1.AppendText(vbNewLine)
                                RT1.AppendText(vbNewLine)

                                Continue Do

                            End If

                            'Continue

                        End If


                    Else

                        'We don't have an id provided or it does not match, let's try to grab the id based on the other file elements!

                        'Get Birthdate from the file to process a match! ' 3.0.003
                        birthdate = Trim(CurrentRecord(13).ToString)

                        If birthdate.Length = 5 Then
                            'Month has 0 truncated add it in!!
                            birthdate = "0" + birthdate
                        End If

                        If birthdate.Length = 6 Then

                            ' MsgBox(birthdate.Substring(2, 2).ToString)
                            'If birthdate.Substring(2, 3) = "19" And birthdate.Substring(0, 1) = "0" Then
                            'Two digit year
                            'birthdate = birthdate.Substring(0, 1) & "/" & birthdate.Substring(1, 1) & "/" & birthdate.Substring(4, 2)

                            If birthdate.Substring(2, 3) = "19" Then
                                birthdate = birthdate.Substring(0, 1) & "/" & birthdate.Substring(1, 1) & "/" & birthdate.Substring(2, 4)
                            Else
                                'Four digit year
                                birthdate = birthdate.Substring(0, 2) & "/" & birthdate.Substring(2, 2) & "/" & birthdate.Substring(4, 2)
                            End If

                        End If


                        If birthdate = "00/00/00" Or birthdate = "" Then
                            'skip
                            birthdate = ""
                        Else
                            objDate = Date.Parse(birthdate).ToShortDateString

                            birthdate = objDate
                        End If

                        'Go through eSchool and see if you can find a student match.

                        connect2 = New SqlConnection(connection)

                        connect2.Open()
                        'command.Cancel()

                        command2 = New SqlCommand("SELECT Student_id, Building, CONVERT(char(10), BIRTHDATE, 101) AS Birth FROM REG WHERE DISTRICT = '" & strDistrict & "' AND Last_Name = '" & LastName.ToString.Replace("'", "''") & "' AND First_Name = '" & FirstName.ToString.Replace("'", "''") & "' AND Birthdate = '" & birthdate & "' AND Gender = '" & SSex & "' ", connect2)

                        Dim reader2 As SqlDataReader =
                              command2.ExecuteReader(
                              CommandBehavior.CloseConnection)

                        'Console.WriteLine(reader2.HasRows)
                        If reader2.HasRows = False Then
                            'Student was not found


                            If FontBold = False Then
                                FontBold = True
                                RT1.SelectionFont = New Font("Arial", 10, Drawing.FontStyle.Bold)
                                RT1.SelectionColor = Color.Crimson
                                'RT1.SelectionFont.ForeColor.ToString()
                            Else
                                FontBold = False
                                RT1.SelectionFont = New Font("Arial", 10, Drawing.FontStyle.Bold)
                                RT1.SelectionFont = New Font(RT1.SelectionFont, Drawing.FontStyle.Bold + Drawing.FontStyle.Italic)
                                RT1.SelectionColor = Color.Crimson
                            End If
                            'RT1.ForeColor = (Drawing.Color.Violet)
                            RT1.AppendText("Student Id was not found for record number.  No student id is supplied in file so unable to process line: " & Counter & " LastName: " & LastName & " | " & " FirstName: " & FirstName & " | " & "Birthdate: " & birthdate & " | Gender: " & SSex & vbNewLine)
                            'RT1.ForeColor = (Drawing.Color.Black)
                            'RT1.AppendText("______________________________________________________________________________________________________________________________________________")
                            'RT1.AppendText(vbNewLine)
                            RT1.AppendText(vbNewLine)

                            Continue Do ' move to the next student

                        Else

                            While reader2.Read()

                                strID = Trim(reader2.Item("Student_ID").ToString)
                                'Need the building number for the matched record also!!! -Gates 1/7/2013
                                stuBuilding = Trim(reader2.Item("Building").ToString)
                                birthdate = Trim(reader2.Item("Birth").ToString)

                            End While

                            If birthdate.Length = 5 Then
                                'Month has 0 truncated add it in!!
                                birthdate = "0" + birthdate
                            End If

                            If birthdate.Length = 6 Then

                                ' MsgBox(birthdate.Substring(2, 2).ToString)
                                'If birthdate.Substring(2, 3) = "19" And birthdate.Substring(0, 1) = "0" Then
                                'Two digit year
                                'birthdate = birthdate.Substring(0, 1) & "/" & birthdate.Substring(1, 1) & "/" & birthdate.Substring(4, 2)

                                If birthdate.Substring(2, 3) = "19" Then
                                    birthdate = birthdate.Substring(0, 1) & "/" & birthdate.Substring(1, 1) & "/" & birthdate.Substring(2, 4)
                                Else
                                    'Four digit year
                                    birthdate = birthdate.Substring(0, 2) & "/" & birthdate.Substring(2, 2) & "/" & birthdate.Substring(4, 2)
                                End If

                            End If


                            If birthdate = "00/00/00" Or birthdate = "" Then
                                'skip
                                birthdate = ""
                            Else
                                objDate = Date.Parse(birthdate).ToShortDateString

                                birthdate = objDate
                            End If

                            connect2.Close()
                            connect2.Dispose()
                            reader2.Close()
                            reader2.Dispose()
                            'Continue


                        End If

                    End If
                    '2.1.003 End

                    'RecordLength = arrContents.Length.ToString

                    If CurrentRecord.Length <> "244" And testCode = "AP" Then
                        'There is a problem with the AP Layout!

                        If FontBold = False Then
                            FontBold = True
                            RT1.SelectionFont = New Font("Arial", 10, Drawing.FontStyle.Bold)
                            RT1.SelectionColor = Color.DarkSeaGreen
                            'RT1.SelectionFont.ForeColor.ToString()
                        Else
                            FontBold = False
                            RT1.SelectionFont = New Font("Arial", 10, Drawing.FontStyle.Bold)
                            RT1.SelectionFont = New Font(RT1.SelectionFont, Drawing.FontStyle.Bold + Drawing.FontStyle.Italic)
                            RT1.SelectionColor = Color.DarkSeaGreen
                        End If
                        RT1.AppendText("Student Id has an invalid length: " & CurrentRecord.Length.ToString & " | " & "Line: " & Counter & " | " & "LastName: " & LastName & "|" & "Birthdate: " & birthdate & "| Gender: " & SSex & vbNewLine)
                        'RT1.ForeColor = (Drawing.Color.Black)
                        'RT1.AppendText("______________________________________________________________________________________________________________________________________________")
                        'RT1.AppendText(vbNewLine)
                        RT1.AppendText(vbNewLine)

                    Else

                        ' If strID = "163312" Then
                        'Debug.Print("Stop!")
                        'End If

                        '1st Score
                        examYear1 = Trim(CurrentRecord(58).ToString)
                        examCode1 = Trim(CurrentRecord(59).ToString)
                        examScore1 = Trim(CurrentRecord(60).ToString)
                        IReg11 = Trim(CurrentRecord(61).ToString) 'Irregularity Code 1
                        IReg12 = Trim(CurrentRecord(62).ToString) 'Irregularity Code 2

                        '2nd Score
                        examYear2 = Trim(CurrentRecord(64).ToString)
                        examCode2 = Trim(CurrentRecord(65).ToString)
                        examScore2 = Trim(CurrentRecord(66).ToString)
                        IReg21 = Trim(CurrentRecord(67).ToString) 'Irregularity Code 1
                        IReg22 = Trim(CurrentRecord(68).ToString) 'Irregularity Code 2

                        '3rd Score
                        examYear3 = Trim(CurrentRecord(70).ToString)
                        examCode3 = Trim(CurrentRecord(71).ToString)
                        examScore3 = Trim(CurrentRecord(72).ToString)
                        IReg31 = Trim(CurrentRecord(73).ToString) 'Irregularity Code 1
                        IReg32 = Trim(CurrentRecord(74).ToString) 'Irregularity Code 2

                        '4th Score
                        examYear4 = Trim(CurrentRecord(76).ToString)
                        examCode4 = Trim(CurrentRecord(77).ToString)
                        examScore4 = Trim(CurrentRecord(78).ToString)
                        IReg41 = Trim(CurrentRecord(79).ToString) 'Irregularity Code 1
                        IReg42 = Trim(CurrentRecord(80).ToString) 'Irregularity Code 2

                        '5th Score
                        examYear5 = Trim(CurrentRecord(82).ToString)
                        examCode5 = Trim(CurrentRecord(83).ToString)
                        examScore5 = Trim(CurrentRecord(84).ToString)
                        IReg51 = Trim(CurrentRecord(85).ToString) 'Irregularity Code 1
                        IReg52 = Trim(CurrentRecord(86).ToString) 'Irregularity Code 2

                        '6th Score
                        examYear6 = Trim(CurrentRecord(88).ToString)
                        examCode6 = Trim(CurrentRecord(89).ToString)
                        examScore6 = Trim(CurrentRecord(90).ToString)
                        IReg61 = Trim(CurrentRecord(91).ToString) 'Irregularity Code 1
                        IReg62 = Trim(CurrentRecord(92).ToString) 'Irregularity Code 2

                        '7th Score
                        examYear7 = Trim(CurrentRecord(94).ToString)
                        examCode7 = Trim(CurrentRecord(95).ToString)
                        examScore7 = Trim(CurrentRecord(96).ToString)
                        IReg71 = Trim(CurrentRecord(97).ToString) 'Irregularity Code 1
                        IReg72 = Trim(CurrentRecord(98).ToString) 'Irregularity Code 2

                        '8th Score
                        examYear8 = Trim(CurrentRecord(100).ToString)
                        examCode8 = Trim(CurrentRecord(101).ToString)
                        examScore8 = Trim(CurrentRecord(102).ToString)
                        IReg81 = Trim(CurrentRecord(103).ToString) 'Irregularity Code 1
                        IReg82 = Trim(CurrentRecord(104).ToString) 'Irregularity Code 2

                        '9th Score
                        examYear9 = Trim(CurrentRecord(106).ToString)
                        examCode9 = Trim(CurrentRecord(107).ToString)
                        examScore9 = Trim(CurrentRecord(108).ToString)
                        IReg91 = Trim(CurrentRecord(109).ToString) 'Irregularity Code 1
                        IReg92 = Trim(CurrentRecord(110).ToString) 'Irregularity Code 2

                        '10th Score
                        examYear10 = Trim(CurrentRecord(112).ToString)
                        examCode10 = Trim(CurrentRecord(113).ToString)
                        examScore10 = Trim(CurrentRecord(114).ToString)
                        IReg101 = Trim(CurrentRecord(115).ToString) 'Irregularity Code 1
                        IReg102 = Trim(CurrentRecord(116).ToString) 'Irregularity Code 2

                        '11th Score
                        examYear11 = Trim(CurrentRecord(118).ToString)
                        examCode11 = Trim(CurrentRecord(119).ToString)
                        examScore11 = Trim(CurrentRecord(120).ToString)
                        IReg111 = Trim(CurrentRecord(121).ToString) 'Irregularity Code 1
                        IReg112 = Trim(CurrentRecord(122).ToString) 'Irregularity Code 2

                        '12th Score
                        examYear12 = Trim(CurrentRecord(124).ToString)
                        examCode12 = Trim(CurrentRecord(125).ToString)
                        examScore12 = Trim(CurrentRecord(126).ToString)
                        IReg121 = Trim(CurrentRecord(127).ToString) 'Irregularity Code 1
                        IReg122 = Trim(CurrentRecord(128).ToString) 'Irregularity Code 2


                        '13th Score
                        examYear13 = Trim(CurrentRecord(130).ToString)
                        examCode13 = Trim(CurrentRecord(131).ToString)
                        examScore13 = Trim(CurrentRecord(132).ToString)
                        IReg131 = Trim(CurrentRecord(133).ToString) 'Irregularity Code 1
                        IReg132 = Trim(CurrentRecord(134).ToString) 'Irregularity Code 2

                        '14th Score
                        examYear14 = Trim(CurrentRecord(136).ToString)
                        examCode14 = Trim(CurrentRecord(137).ToString)
                        examScore14 = Trim(CurrentRecord(138).ToString)
                        IReg141 = Trim(CurrentRecord(139).ToString) 'Irregularity Code 1
                        IReg142 = Trim(CurrentRecord(140).ToString) 'Irregularity Code 2

                        '15th Score
                        examYear15 = Trim(CurrentRecord(142).ToString)
                        examCode15 = Trim(CurrentRecord(143).ToString)
                        examScore15 = Trim(CurrentRecord(144).ToString)
                        IReg151 = Trim(CurrentRecord(145).ToString) 'Irregularity Code 1
                        IReg152 = Trim(CurrentRecord(146).ToString) 'Irregularity Code 2

                        '16th Score
                        examYear16 = Trim(CurrentRecord(148).ToString)
                        examCode16 = Trim(CurrentRecord(149).ToString)
                        examScore16 = Trim(CurrentRecord(150).ToString)
                        IReg161 = Trim(CurrentRecord(151).ToString) 'Irregularity Code 1
                        IReg162 = Trim(CurrentRecord(152).ToString) 'Irregularity Code 2

                        '17th Score
                        examYear17 = Trim(CurrentRecord(154).ToString)
                        examCode17 = Trim(CurrentRecord(155).ToString)
                        examScore17 = Trim(CurrentRecord(156).ToString)
                        IReg171 = Trim(CurrentRecord(157).ToString) 'Irregularity Code 1
                        IReg172 = Trim(CurrentRecord(158).ToString) 'Irregularity Code 2

                        '18th Score
                        examYear18 = Trim(CurrentRecord(160).ToString)
                        examCode18 = Trim(CurrentRecord(161).ToString)
                        examScore18 = Trim(CurrentRecord(162).ToString)
                        IReg181 = Trim(CurrentRecord(163).ToString) 'Irregularity Code 1
                        IReg182 = Trim(CurrentRecord(164).ToString) 'Irregularity Code 2

                        '19th Score 
                        examYear19 = Trim(CurrentRecord(166).ToString)
                        examCode19 = Trim(CurrentRecord(167).ToString)
                        examScore19 = Trim(CurrentRecord(168).ToString)
                        IReg191 = Trim(CurrentRecord(169).ToString) 'Irregularity Code 1
                        IReg192 = Trim(CurrentRecord(170).ToString) 'Irregularity Code 2

                        '20th Score
                        examYear20 = Trim(CurrentRecord(172).ToString)
                        examCode20 = Trim(CurrentRecord(173).ToString)
                        examScore20 = Trim(CurrentRecord(174).ToString)
                        IReg201 = Trim(CurrentRecord(175).ToString) 'Irregularity Code 1
                        IReg202 = Trim(CurrentRecord(176).ToString) 'Irregularity Code 2

                        '21st score
                        examYear21 = Trim(CurrentRecord(178).ToString)
                        examCode21 = Trim(CurrentRecord(179).ToString)
                        examScore21 = Trim(CurrentRecord(180).ToString)
                        IReg211 = Trim(CurrentRecord(181).ToString) 'Irregularity Code 1
                        IReg212 = Trim(CurrentRecord(182).ToString) 'Irregularity Code 2

                        '22nd score
                        examYear22 = Trim(CurrentRecord(184).ToString)
                        examCode22 = Trim(CurrentRecord(185).ToString)
                        examScore22 = Trim(CurrentRecord(186).ToString)
                        IReg221 = Trim(CurrentRecord(187).ToString) 'Irregularity Code 1
                        IReg222 = Trim(CurrentRecord(188).ToString) 'Irregularity Code 2

                        '23rd score
                        examYear23 = Trim(CurrentRecord(190).ToString)
                        examCode23 = Trim(CurrentRecord(191).ToString)
                        examScore23 = Trim(CurrentRecord(192).ToString)
                        IReg231 = Trim(CurrentRecord(193).ToString) 'Irregularity Code 1
                        IReg232 = Trim(CurrentRecord(194).ToString) 'Irregularity Code 2

                        '24th score
                        examYear24 = Trim(CurrentRecord(196).ToString)
                        examCode24 = Trim(CurrentRecord(197).ToString)
                        examScore24 = Trim(CurrentRecord(198).ToString)
                        IReg241 = Trim(CurrentRecord(199).ToString) 'Irregularity Code 1
                        IReg242 = Trim(CurrentRecord(200).ToString) 'Irregularity Code 2

                        '25th score
                        examYear25 = Trim(CurrentRecord(202).ToString)
                        examCode25 = Trim(CurrentRecord(203).ToString)
                        examScore25 = Trim(CurrentRecord(204).ToString)
                        IReg251 = Trim(CurrentRecord(205).ToString) 'Irregularity Code 1
                        IReg252 = Trim(CurrentRecord(206).ToString) 'Irregularity Code 2

                        '26th score
                        examYear26 = Trim(CurrentRecord(208).ToString)
                        examCode26 = Trim(CurrentRecord(209).ToString)
                        examScore26 = Trim(CurrentRecord(210).ToString)
                        IReg261 = Trim(CurrentRecord(211).ToString) 'Irregularity Code 1
                        IReg262 = Trim(CurrentRecord(212).ToString) 'Irregularity Code 2

                        '27th score
                        examYear27 = Trim(CurrentRecord(214).ToString)
                        examCode27 = Trim(CurrentRecord(215).ToString)
                        examScore27 = Trim(CurrentRecord(216).ToString)
                        IReg271 = Trim(CurrentRecord(217).ToString) 'Irregularity Code 1
                        IReg272 = Trim(CurrentRecord(218).ToString) 'Irregularity Code 2

                        '28th score
                        examYear28 = Trim(CurrentRecord(220).ToString)
                        examCode28 = Trim(CurrentRecord(221).ToString)
                        examScore28 = Trim(CurrentRecord(222).ToString)
                        IReg281 = Trim(CurrentRecord(223).ToString) 'Irregularity Code 1
                        IReg282 = Trim(CurrentRecord(224).ToString) 'Irregularity Code 2

                        '29th score
                        examYear29 = Trim(CurrentRecord(226).ToString)
                        examCode29 = Trim(CurrentRecord(227).ToString)
                        examScore29 = Trim(CurrentRecord(228).ToString)
                        IReg291 = Trim(CurrentRecord(229).ToString) 'Irregularity Code 1
                        IReg292 = Trim(CurrentRecord(230).ToString) 'Irregularity Code 2

                        '30th score
                        examYear30 = Trim(CurrentRecord(232).ToString)
                        examCode30 = Trim(CurrentRecord(233).ToString)
                        examScore30 = Trim(CurrentRecord(234).ToString)
                        IReg301 = Trim(CurrentRecord(235).ToString) 'Irregularity Code 1
                        IReg302 = Trim(CurrentRecord(236).ToString) 'Irregularity Code 2

                        'Assign variables and write test records.

                        If Trim(CurrentRecord(239)).Length < 6 Then

                            CurrentRecord(239) = "0" + CurrentRecord(239)

                            ' MsgBox(arrContents(239).ToString)

                            examDate = Trim(CurrentRecord(239).ToString.Substring(0, 2) & "/" & Trim(CurrentRecord(239).ToString.Substring(2, 2) & "/20" & Trim(CurrentRecord(239).ToString.Substring(4, 2))))

                        Else

                            examDate = Trim(CurrentRecord(239).ToString.Substring(0, 2) & "/" & Trim(CurrentRecord(239).ToString.Substring(2, 2) & "/20" & Trim(CurrentRecord(239).ToString.Substring(4, 2))))


                        End If


                        Dim examYr As String = ""

                        'MsgBox("Exam years: " & examYear1 & " | " & examYear2 & " | " & examYear3 & " | " & examYear4 & " | " & examYear5 & " | " & examYear6 & " | " & examYear7 & " | " & examYear8 & " | " & examYear9 & " | " & examYear10 & " | " & examYear11)


                        'Set test dates

                        'Dim x As Integer

                        'Dim years(21) As String

                        If examYear1.Length < 2 Then
                            adminYear1 = "20" & "0" & examYear1
                            examDate1 = "05/09/" & adminYear1
                        Else
                            adminYear1 = "20" & examYear1
                            examDate1 = "05/09/" & adminYear1
                        End If

                        If examYear2.Length < 2 Then
                            adminYear2 = "20" & "0" & examYear2
                            examDate2 = "05/09/" & adminYear2
                        Else
                            adminYear2 = "20" & examYear2
                            examDate2 = "05/09/" & adminYear2
                        End If

                        If examYear3.Length < 2 Then
                            adminYear3 = "20" & "0" & examYear3
                            examDate3 = "05/09/" & adminYear3
                        Else
                            adminYear3 = "20" & examYear3
                            examDate3 = "05/09/" & adminYear3
                        End If

                        If examYear4.Length < 2 Then
                            adminYear4 = "20" & "0" & examYear4
                            examDate4 = "05/09/" & adminYear4
                        Else
                            adminYear4 = "20" & examYear4
                            examDate4 = "05/09/" & adminYear4
                        End If
                        If examYear5.Length < 2 Then
                            adminYear5 = "20" & "0" & examYear5
                            examDate5 = "05/09/" & adminYear5
                        Else
                            adminYear5 = "20" & examYear5
                            examDate5 = "05/09/" & adminYear5
                        End If

                        If examYear6.Length < 2 Then
                            adminYear6 = "20" & "0" & examYear6
                            examDate6 = "05/09/" & adminYear6
                        Else
                            adminYear6 = "20" & examYear6
                            examDate6 = "05/09/" & adminYear6
                        End If
                        If examYear7.Length < 2 Then
                            adminYear7 = "20" & "0" & examYear7
                            examDate7 = "05/09/" & adminYear7
                        Else
                            adminYear7 = "20" & examYear7
                            examDate7 = "05/09/" & adminYear7
                        End If
                        If examYear8.Length < 2 Then
                            adminYear8 = "20" & "0" & examYear8
                            examDate8 = "05/09/" & adminYear8
                        Else
                            adminYear8 = "20" & examYear8
                            examDate8 = "05/09/" & adminYear8
                        End If
                        If examYear9.Length < 2 Then
                            adminYear9 = "20" & "0" & examYear9
                            examDate9 = "05/09/" & adminYear9
                        Else
                            adminYear9 = "20" & examYear9
                            examDate9 = "05/09/" & adminYear9
                        End If
                        If examYear10.Length < 2 Then
                            adminYear10 = "20" & "0" & examYear10
                            examDate10 = "05/09/" & adminYear10
                        Else
                            adminYear10 = "20" & examYear10
                            examDate10 = "05/09/" & adminYear10
                        End If
                        If examYear11.Length < 2 Then
                            adminYear11 = "20" & "0" & examYear11
                            examDate11 = "05/09/" & adminYear11
                        Else
                            adminYear11 = "20" & examYear11
                            examDate11 = "05/09/" & adminYear11
                        End If

                        If examYear12.Length < 2 Then
                            adminYear12 = "20" & "0" & examYear12
                            examDate12 = "05/09/" & adminYear12
                        Else
                            adminYear12 = "20" & examYear12
                            examDate12 = "05/09/" & adminYear12
                        End If

                        If examYear13.Length < 2 Then
                            adminYear13 = "20" & "0" & examYear13
                            examDate13 = "05/09/" & adminYear13
                        Else
                            adminYear13 = "20" & examYear13
                            examDate13 = "05/09/" & adminYear13
                        End If

                        If examYear14.Length < 2 Then
                            adminYear14 = "20" & "0" & examYear14
                            examDate14 = "05/09/" & adminYear14
                        Else
                            adminYear14 = "20" & examYear14
                            examDate14 = "05/09/" & adminYear14
                        End If

                        If examYear15.Length < 2 Then
                            adminYear15 = "20" & "0" & examYear15
                            examDate15 = "05/09/" & adminYear15
                        Else
                            adminYear15 = "20" & examYear15
                            examDate15 = "05/09/" & adminYear15
                        End If

                        If examYear16.Length < 2 Then
                            adminYear16 = "20" & "0" & examYear16
                            examDate16 = "05/09/" & adminYear16
                        Else
                            adminYear16 = "20" & examYear16
                            examDate16 = "05/09/" & adminYear16
                        End If

                        If examYear17.Length < 2 Then
                            adminYear17 = "20" & "0" & examYear17
                            examDate17 = "05/09/" & adminYear17
                        Else
                            adminYear17 = "20" & examYear17
                            examDate17 = "05/09/" & adminYear17
                        End If

                        If examYear18.Length < 2 Then
                            adminYear18 = "20" & "0" & examYear18
                            examDate18 = "05/09/" & adminYear18
                        Else
                            adminYear18 = "20" & examYear18
                            examDate18 = "05/09/" & adminYear18
                        End If
                        If examYear19.Length < 2 Then
                            adminYear19 = "20" & "0" & examYear19
                            examDate19 = "05/09/" & adminYear19
                        Else
                            adminYear19 = "20" & examYear19
                            examDate19 = "05/09/" & adminYear19
                        End If
                        If examYear20.Length < 2 Then
                            adminYear20 = "20" & "0" & examYear20
                            examDate20 = "05/09/" & adminYear20
                        Else
                            adminYear20 = "20" & examYear20
                            examDate20 = "05/09/" & adminYear20
                        End If
                        If examYear21.Length < 2 Then
                            adminYear21 = "20" & "0" & examYear21
                            examDate21 = "05/09/" & adminYear21
                        Else
                            adminYear21 = "20" & examYear21
                            examDate21 = "05/09/" & adminYear21
                        End If
                        If examYear22.Length < 2 Then
                            adminYear22 = "20" & "0" & examYear22
                            examDate22 = "05/09/" & adminYear22
                        Else
                            adminYear22 = "20" & examYear22
                            examDate22 = "05/09/" & adminYear22
                        End If


                        'Calculate age on test date

                        cmpAge = DateDiff(DateInterval.Year, CDate(birthdate), CDate(examDate))

                        'Check for no write!


                        If chkTestOnly.Checked = True Then

                            'DO NOT WRITE DATABASE!!!


                        Else

                            'Get test key from the database!!

                            Dim connectTChk = New SqlConnection(connection)

                            connectTChk.Open()

                            Dim commandTChk As New SqlCommand("SELECT TOP (1) * FROM LTDB_TEST WHERE District = '" & strDistrict & "' AND TEST_CODE = 'AP'", connectTChk)

                            Dim readerTChk As SqlDataReader

                            readerTChk =
                                  commandTChk.ExecuteReader(
                                  CommandBehavior.CloseConnection)

                            If readerTChk.HasRows = False Then

                            'This would not be an ideal situation exit is the only solution!

                            'MsgBox("A test key was not found for the 'AP' test type.  Please check your setups and try again!")

                            lblDisErr.Text = "A test key was not found for the 'AP' test type.  Please check your setups and try again!"

                            Exit Sub
                        Else

                                While readerTChk.Read()


                                    testKey = Trim(readerTChk.Item("Test_Key"))

                                End While

                            End If

                            readerTChk.Close()
                            readerTChk.Dispose()

                            'Check for code '07' and convert to '7' if found!

                            If Trim(examCode1) = "07" Then
                                examCode1 = "7"
                            End If

                            If Trim(examCode2) = "07" Then
                                examCode2 = "7"
                            End If

                            If Trim(examCode3) = "07" Then
                                examCode3 = "7"
                            End If

                            If Trim(examCode4) = "07" Then
                                examCode4 = "7"
                            End If

                            If Trim(examCode5) = "07" Then
                                examCode5 = "7"
                            End If
                            If Trim(examCode6) = "07" Then
                                examCode6 = "7"
                            End If
                            If Trim(examCode7) = "07" Then
                                examCode7 = "7"
                            End If
                            If Trim(examCode8) = "07" Then
                                examCode8 = "7"
                            End If
                            If Trim(examCode9) = "07" Then
                                examCode9 = "7"
                            End If
                            If Trim(examCode10) = "07" Then
                                examCode10 = "7"
                            End If
                            If Trim(examCode11) = "07" Then
                                examCode11 = "7"
                            End If
                            If Trim(examCode12) = "07" Then
                                examCode12 = "7"
                            End If
                            If Trim(examCode13) = "07" Then
                                examCode13 = "7"
                            End If
                            If Trim(examCode14) = "07" Then
                                examCode14 = "7"
                            End If
                            If Trim(examCode15) = "07" Then
                                examCode15 = "7"
                            End If
                            If Trim(examCode16) = "07" Then
                                examCode16 = "7"
                            End If
                            If Trim(examCode17) = "07" Then
                                examCode17 = "7"
                            End If
                            If Trim(examCode18) = "07" Then
                                examCode18 = "7"
                            End If
                            If Trim(examCode19) = "07" Then
                                examCode19 = "7"
                            End If
                            If Trim(examCode20) = "07" Then
                                examCode20 = "7"
                            End If
                            If Trim(examCode21) = "07" Then
                                examCode21 = "7"
                            End If
                            If Trim(examCode22) = "07" Then
                                examCode22 = "7"
                            End If
                            If Trim(examCode23) = "07" Then
                                examCode23 = "7"
                            End If
                            If Trim(examCode24) = "07" Then
                                examCode24 = "7"
                            End If
                            If Trim(examCode25) = "07" Then
                                examCode25 = "7"
                            End If
                            If Trim(examCode26) = "07" Then
                                examCode26 = "7"
                            End If
                            If Trim(examCode27) = "07" Then
                                examCode27 = "7"
                            End If
                            If Trim(examCode28) = "07" Then
                                examCode28 = "7"
                            End If
                            If Trim(examCode29) = "07" Then
                                examCode29 = "7"
                            End If
                            If Trim(examCode30) = "07" Then
                                examCode30 = "7"
                            End If
                            'End Convert
                            'Check for existing test record for this student for AP scores.  If there is one then update instead of creating another one! -Gates 7/19/2012

                            'Check LTDB_STU_TEST for existing record
                            connect2 = New SqlConnection(connection)

                            connect2.Open()
                            'command.Cancel()

                            command2 = New SqlCommand("SELECT TOP (1) * FROM LTDB_STU_TEST WHERE District = '" & strDistrict & "' AND TEST_CODE = 'AP' AND Student_ID = '" & strID & "' ", connect2)

                            Dim reader As SqlDataReader =
                                  command2.ExecuteReader(
                                  CommandBehavior.CloseConnection)

                            'Console.WriteLine(reader2.HasRows)
                            If reader.HasRows = False Then
                                'A previous AP test was not found create as new!
                                'Write the test record !
                                Dim cmdU As New SqlCommand("INSERT INTO LTDB_STU_TEST (DISTRICT, TEST_CODE, TEST_LEVEL, TEST_FORM, TEST_KEY, STUDENT_ID, TEST_DATE, AGE, TRANSCRIPT_PRINT, BUILDING, CHANGE_DATE_TIME, CHANGE_UID)VALUES('" & strDistrict & "', '" & testCode & "', '1','1','" & testKey & "','" & strID & "','" & examDate & "','" & cmpAge & "', 'Y','" & stuBuilding & "','" & Date.Now.ToShortDateString & " " & Date.Now.ToShortTimeString & "'" & ",'Test.Import')", New SqlConnection(connection))

                                cmdU.Connection.Open()
                                cmdU.ExecuteNonQuery()
                                cmdU.Connection.Close()
                                cmdU.Connection.Dispose()

                                'Enter Tests!

                                'Subtest 1
                                APTestInsertUpdate(connection, strID, examCode1, testCode, testKey, examDate, examScore1, adminYear1, IReg11, IReg12)


                                'Subtest 2
                                If examCode2.Length < 1 Then

                                Else
                                    APTestInsertUpdate(connection, strID, examCode2, testCode, testKey, examDate, examScore2, adminYear2, IReg21, IReg22)

                                End If

                                'End Subtest 2

                                'Subtest 3
                                If examCode3.Length < 1 Then

                                Else
                                    APTestInsertUpdate(connection, strID, examCode3, testCode, testKey, examDate, examScore3, adminYear3, IReg31, IReg32)

                                End If

                                'End Subtest 3

                                'Subtest 4
                                If examCode4.Length < 1 Then

                                Else
                                    APTestInsertUpdate(connection, strID, examCode4, testCode, testKey, examDate, examScore4, adminYear4, IReg41, IReg42)

                                End If

                                'End Subtest 4

                                'Subtest 5
                                If examCode5.Length < 1 Then

                                Else
                                    APTestInsertUpdate(connection, strID, examCode5, testCode, testKey, examDate, examScore5, adminYear5, IReg51, IReg52)

                                End If

                                'End Subtest 5

                                'Subtest 6
                                If examCode6.Length < 1 Then

                                Else
                                    APTestInsertUpdate(connection, strID, examCode6, testCode, testKey, examDate, examScore6, adminYear6, IReg61, IReg62)

                                End If

                                'End Subtest 6

                                'Subtest 7
                                If examCode7.Length < 1 Then

                                Else
                                    APTestInsertUpdate(connection, strID, examCode7, testCode, testKey, examDate, examScore7, adminYear7, IReg71, IReg72)

                                End If

                                'End Subtest 7

                                'Subtest 8
                                If examCode8.Length < 1 Then

                                Else
                                    APTestInsertUpdate(connection, strID, examCode8, testCode, testKey, examDate, examScore8, adminYear8, IReg81, IReg82)

                                End If

                                'End Subtest 8

                                'Subtest 9
                                If examCode9.Length < 1 Then

                                Else
                                    APTestInsertUpdate(connection, strID, examCode9, testCode, testKey, examDate, examScore9, adminYear9, IReg91, IReg92)

                                End If

                                'End Subtest 9


                                'Subtest 10
                                If examCode10.Length < 1 Then

                                Else
                                    APTestInsertUpdate(connection, strID, examCode10, testCode, testKey, examDate, examScore10, adminYear10, IReg101, IReg92)

                                End If

                                'End Subtest 10

                                'Subtest 11
                                If examCode11.Length < 1 Then

                                Else
                                    APTestInsertUpdate(connection, strID, examCode11, testCode, testKey, examDate, examScore11, adminYear11, IReg111, IReg112)

                                End If

                                'End Subtest 11


                                'Subtest 12
                                If examCode12.Length < 1 Then

                                Else
                                    APTestInsertUpdate(connection, strID, examCode12, testCode, testKey, examDate, examScore12, adminYear12, IReg121, IReg122)

                                End If

                                'End Subtest 12

                                'Subtest 13
                                If examCode13.Length < 1 Then

                                Else
                                    APTestInsertUpdate(connection, strID, examCode13, testCode, testKey, examDate, examScore13, adminYear13, IReg131, IReg132)

                                End If

                                'End Subtest 13

                                'Subtest 14
                                If examCode14.Length < 1 Then

                                Else
                                    APTestInsertUpdate(connection, strID, examCode14, testCode, testKey, examDate, examScore14, adminYear14, IReg141, IReg142)

                                End If

                                'End Subtest 14

                                'Subtest 15
                                If examCode15.Length < 1 Then

                                Else
                                    APTestInsertUpdate(connection, strID, examCode15, testCode, testKey, examDate, examScore15, adminYear15, IReg151, IReg152)

                                End If

                                'End Subtest 15

                                'Subtest 16
                                If examCode16.Length < 1 Then

                                Else
                                    APTestInsertUpdate(connection, strID, examCode16, testCode, testKey, examDate, examScore16, adminYear16, IReg161, IReg162)

                                End If

                                'End Subtest 16

                                'Subtest 17
                                If examCode17.Length < 1 Then

                                Else
                                    APTestInsertUpdate(connection, strID, examCode17, testCode, testKey, examDate, examScore17, adminYear17, IReg171, IReg172)

                                End If

                                'End Subtest 17

                                'Subtest 18
                                If examCode18.Length < 1 Then

                                Else
                                    APTestInsertUpdate(connection, strID, examCode18, testCode, testKey, examDate, examScore18, adminYear18, IReg181, IReg182)

                                End If

                                'End Subtest 18

                                'Subtest 19
                                If examCode19.Length < 1 Then

                                Else
                                    APTestInsertUpdate(connection, strID, examCode19, testCode, testKey, examDate, examScore19, adminYear19, IReg191, IReg192)

                                End If

                                'End Subtest 19

                                'Subtest 20
                                If examCode20.Length < 1 Then

                                Else
                                    APTestInsertUpdate(connection, strID, examCode20, testCode, testKey, examDate, examScore20, adminYear20, IReg201, IReg202)

                                End If

                                'End Subtest 20

                                'Subtest 21
                                If examCode21.Length < 1 Then

                                Else
                                    APTestInsertUpdate(connection, strID, examCode21, testCode, testKey, examDate, examScore21, adminYear21, IReg211, IReg212)

                                End If

                                'End Subtest 21

                                'Subtest 22
                                If examCode22.Length < 1 Then

                                Else
                                    APTestInsertUpdate(connection, strID, examCode22, testCode, testKey, examDate, examScore22, adminYear22, IReg221, IReg222)

                                End If

                                'End Subtest 22

                                'Subtest 23
                                If examCode23.Length < 1 Then

                                Else
                                    APTestInsertUpdate(connection, strID, examCode23, testCode, testKey, examDate, examScore23, adminYear23, IReg231, IReg232)

                                End If

                                'End Subtest 23

                                'Subtest 24
                                If examCode24.Length < 1 Then

                                Else
                                    APTestInsertUpdate(connection, strID, examCode24, testCode, testKey, examDate, examScore24, adminYear24, IReg241, IReg242)

                                End If

                                'End Subtest 24

                                'Subtest 25
                                If examCode25.Length < 1 Then

                                Else
                                    APTestInsertUpdate(connection, strID, examCode25, testCode, testKey, examDate, examScore25, adminYear25, IReg251, IReg252)

                                End If

                                'End Subtest 25

                                'Subtest 26
                                If examCode26.Length < 1 Then

                                Else
                                    APTestInsertUpdate(connection, strID, examCode26, testCode, testKey, examDate, examScore26, adminYear26, IReg261, IReg262)

                                End If

                                'End Subtest 26

                                'Subtest 27
                                If examCode27.Length < 1 Then

                                Else
                                    APTestInsertUpdate(connection, strID, examCode27, testCode, testKey, examDate, examScore27, adminYear27, IReg271, IReg272)

                                End If

                                'End Subtest 27

                                'Subtest 28
                                If examCode28.Length < 1 Then

                                Else
                                    APTestInsertUpdate(connection, strID, examCode28, testCode, testKey, examDate, examScore28, adminYear28, IReg281, IReg282)

                                End If

                                'End Subtest 28

                                'Subtest 29
                                If examCode29.Length < 1 Then

                                Else
                                    APTestInsertUpdate(connection, strID, examCode29, testCode, testKey, examDate, examScore29, adminYear29, IReg291, IReg292)

                                End If

                                'End Subtest 29

                                'Subtest 30
                                If examCode30.Length < 1 Then

                                Else
                                    APTestInsertUpdate(connection, strID, examCode30, testCode, testKey, examDate, examScore30, adminYear30, IReg301, IReg302)

                                End If

                                'End Subtest 30




                                '<-------------------------------------------------------------------------------------end new record----------------------------------------------------------------------------------------------------->


                            Else

                                '<---------------------------------------------------------------------------------------existing record-------------------------------------------------------------------------------------------------->
                                'We have a previous test get the information so that we can update the record!
                                While reader.Read()

                                    testdate = Trim(reader.Item("Test_Date")) ' Test date needs to match the original AP test import date.  This is why admin year is so important!

                                    'Then update AP test record!
                                    Dim cmdU As New SqlCommand("UPDATE LTDB_STU_TEST SET AGE = '" & cmpAge & "' , TRANSCRIPT_PRINT = 'Y', CHANGE_DATE_TIME = '" & Date.Now.ToShortDateString & " " & Date.Now.ToShortTimeString & "', CHANGE_UID = 'Test.Import' WHERE  Student_id = '" & strID & "' AND TEST_DATE = '" & reader.Item("TEST_DATE").ToString & "' AND TEST_CODE = 'AP'", New SqlConnection(connection))

                                    cmdU.Connection.Open()
                                    cmdU.ExecuteNonQuery()
                                    cmdU.Connection.Close()
                                    cmdU.Connection.Dispose()

                                    'Now update subtests!!!

                                    If examCode1.Length < 1 Then

                                    Else

                                        'Subtest 1
                                        APTestInsertUpdate(connection, strID, examCode1, testCode, testKey, testdate, examScore1, adminYear1, IReg11, IReg12)

                                        'Subtest 2
                                        If examCode2.Length < 1 Then

                                        Else
                                            APTestInsertUpdate(connection, strID, examCode2, testCode, testKey, testdate, examScore2, adminYear2, IReg21, IReg22)

                                        End If

                                        'End Subtest 2

                                        'Subtest 3
                                        If examCode3.Length < 1 Then

                                        Else
                                            APTestInsertUpdate(connection, strID, examCode3, testCode, testKey, testdate, examScore3, adminYear3, IReg31, IReg32)

                                        End If

                                        'End Subtest 3

                                        'Subtest 4
                                        If examCode4.Length < 1 Then

                                        Else
                                            APTestInsertUpdate(connection, strID, examCode4, testCode, testKey, testdate, examScore4, adminYear4, IReg41, IReg42)

                                        End If

                                        'End Subtest 4

                                        'Subtest 5
                                        If examCode5.Length < 1 Then

                                        Else
                                            APTestInsertUpdate(connection, strID, examCode5, testCode, testKey, testdate, examScore5, adminYear5, IReg51, IReg52)

                                        End If

                                        'End Subtest 5

                                        'Subtest 6
                                        If examCode6.Length < 1 Then

                                        Else
                                            APTestInsertUpdate(connection, strID, examCode6, testCode, testKey, testdate, examScore6, adminYear6, IReg61, IReg62)

                                        End If

                                        'End Subtest 6

                                        'Subtest 7
                                        If examCode7.Length < 1 Then

                                        Else
                                            APTestInsertUpdate(connection, strID, examCode7, testCode, testKey, testdate, examScore7, adminYear7, IReg71, IReg72)

                                        End If

                                        'End Subtest 7

                                        'Subtest 8
                                        If examCode8.Length < 1 Then

                                        Else
                                            APTestInsertUpdate(connection, strID, examCode8, testCode, testKey, testdate, examScore8, adminYear8, IReg81, IReg82)

                                        End If

                                        'End Subtest 8

                                        'Subtest 9
                                        If examCode9.Length < 1 Then

                                        Else
                                            APTestInsertUpdate(connection, strID, examCode9, testCode, testKey, testdate, examScore9, adminYear9, IReg91, IReg92)

                                        End If

                                        'End Subtest 9


                                        'Subtest 10
                                        If examCode10.Length < 1 Then

                                        Else
                                            APTestInsertUpdate(connection, strID, examCode10, testCode, testKey, testdate, examScore10, adminYear10, IReg101, IReg92)

                                        End If

                                        'End Subtest 10

                                        'Subtest 11
                                        If examCode11.Length < 1 Then

                                        Else
                                            APTestInsertUpdate(connection, strID, examCode11, testCode, testKey, testdate, examScore11, adminYear11, IReg111, IReg112)

                                        End If

                                        'End Subtest 11


                                        'Subtest 12
                                        If examCode12.Length < 1 Then

                                        Else
                                            APTestInsertUpdate(connection, strID, examCode12, testCode, testKey, testdate, examScore12, adminYear12, IReg121, IReg122)

                                        End If

                                        'End Subtest 12

                                        'Subtest 13
                                        If examCode13.Length < 1 Then

                                        Else
                                            APTestInsertUpdate(connection, strID, examCode13, testCode, testKey, testdate, examScore13, adminYear13, IReg131, IReg132)

                                        End If

                                        'End Subtest 13

                                        'Subtest 14
                                        If examCode14.Length < 1 Then

                                        Else
                                            APTestInsertUpdate(connection, strID, examCode14, testCode, testKey, testdate, examScore14, adminYear14, IReg141, IReg142)

                                        End If

                                        'End Subtest 14

                                        'Subtest 15
                                        If examCode15.Length < 1 Then

                                        Else
                                            APTestInsertUpdate(connection, strID, examCode15, testCode, testKey, testdate, examScore15, adminYear15, IReg151, IReg152)

                                        End If

                                        'End Subtest 15

                                        'Subtest 16
                                        If examCode16.Length < 1 Then

                                        Else
                                            APTestInsertUpdate(connection, strID, examCode16, testCode, testKey, testdate, examScore16, adminYear16, IReg161, IReg162)

                                        End If

                                        'End Subtest 16

                                        'Subtest 17
                                        If examCode17.Length < 1 Then

                                        Else
                                            APTestInsertUpdate(connection, strID, examCode17, testCode, testKey, testdate, examScore17, adminYear17, IReg171, IReg172)

                                        End If

                                        'End Subtest 17

                                        'Subtest 18
                                        If examCode18.Length < 1 Then

                                        Else
                                            APTestInsertUpdate(connection, strID, examCode18, testCode, testKey, testdate, examScore18, adminYear18, IReg181, IReg182)

                                        End If

                                        'End Subtest 18

                                        'Subtest 19
                                        If examCode19.Length < 1 Then

                                        Else
                                            APTestInsertUpdate(connection, strID, examCode19, testCode, testKey, testdate, examScore19, adminYear19, IReg191, IReg192)

                                        End If

                                        'End Subtest 19

                                        'Subtest 20
                                        If examCode20.Length < 1 Then

                                        Else
                                            APTestInsertUpdate(connection, strID, examCode20, testCode, testKey, testdate, examScore20, adminYear20, IReg201, IReg202)

                                        End If

                                        'End Subtest 20

                                        'Subtest 21
                                        If examCode21.Length < 1 Then

                                        Else
                                            APTestInsertUpdate(connection, strID, examCode21, testCode, testKey, testdate, examScore21, adminYear21, IReg211, IReg212)

                                        End If

                                        'End Subtest 21

                                        'Subtest 22
                                        If examCode22.Length < 1 Then

                                        Else
                                            APTestInsertUpdate(connection, strID, examCode22, testCode, testKey, testdate, examScore22, adminYear22, IReg221, IReg222)

                                        End If

                                        'End Subtest 22

                                        'Subtest 23
                                        If examCode23.Length < 1 Then

                                        Else
                                            APTestInsertUpdate(connection, strID, examCode23, testCode, testKey, testdate, examScore23, adminYear23, IReg231, IReg232)

                                        End If

                                        'End Subtest 23

                                        'Subtest 24
                                        If examCode24.Length < 1 Then

                                        Else
                                            APTestInsertUpdate(connection, strID, examCode24, testCode, testKey, testdate, examScore24, adminYear24, IReg241, IReg242)

                                        End If

                                        'End Subtest 24

                                        'Subtest 25
                                        If examCode25.Length < 1 Then

                                        Else
                                            APTestInsertUpdate(connection, strID, examCode25, testCode, testKey, testdate, examScore25, adminYear25, IReg251, IReg252)

                                        End If

                                        'End Subtest 25

                                        'Subtest 26
                                        If examCode26.Length < 1 Then

                                        Else
                                            APTestInsertUpdate(connection, strID, examCode26, testCode, testKey, testdate, examScore26, adminYear26, IReg261, IReg262)

                                        End If

                                        'End Subtest 26

                                        'Subtest 27
                                        If examCode27.Length < 1 Then

                                        Else
                                            APTestInsertUpdate(connection, strID, examCode27, testCode, testKey, testdate, examScore27, adminYear27, IReg271, IReg272)

                                        End If

                                        'End Subtest 27

                                        'Subtest 28
                                        If examCode28.Length < 1 Then

                                        Else
                                            APTestInsertUpdate(connection, strID, examCode28, testCode, testKey, testdate, examScore28, adminYear28, IReg281, IReg282)

                                        End If

                                        'End Subtest 28

                                        'Subtest 29
                                        If examCode29.Length < 1 Then

                                        Else
                                            APTestInsertUpdate(connection, strID, examCode29, testCode, testKey, testdate, examScore29, adminYear29, IReg291, IReg292)

                                        End If

                                        'End Subtest 29

                                        'Subtest 30
                                        If examCode30.Length < 1 Then

                                        Else
                                            APTestInsertUpdate(connection, strID, examCode30, testCode, testKey, testdate, examScore30, adminYear30, IReg301, IReg302)

                                        End If

                                        'End Subtest 30


                                        'End subtest update

                                    End If

                                End While
                                connect2.Close()
                                connect2.Dispose()
                                reader.Close()
                                reader.Dispose()
                                'Continue


                            End If

                            'End Processing checkbox
                        End If

                        'End record length check
                    End If

                    'End check student routine


                    'Clear variables for next student!
9:
                    SqlClient.SqlConnection.ClearAllPools()

                    'Core Student
                    LastName = ""
                    FirstName = ""
                    SSex = ""
                    strID = ""
                    birthdate = ""


                    'Student Age
                    cmpAge = ""

                    'Core Exams
                    examCode1 = ""
                    examYear1 = ""
                    examScore1 = ""
                    examCode2 = ""
                    examYear2 = ""
                    examScore2 = ""
                    examCode3 = ""
                    examYear3 = ""
                    examScore3 = ""
                    examCode4 = ""
                    examYear4 = ""
                    examScore4 = ""
                    examCode5 = ""
                    examYear5 = ""
                    examScore5 = ""
                    examCode6 = ""
                    examYear6 = ""
                    examScore6 = ""
                    examCode7 = ""
                    examYear7 = ""
                    examScore7 = ""
                    examCode8 = ""
                    examYear8 = ""
                    examScore8 = ""
                    examCode9 = ""
                    examYear9 = ""
                    examScore9 = ""
                    examCode10 = ""
                    examYear10 = ""
                    examScore10 = ""
                    examCode11 = ""
                    examYear11 = ""
                    examScore11 = ""
                    examCode12 = ""
                    examYear12 = ""
                    examScore12 = ""
                    examCode13 = ""
                    examYear13 = ""
                    examScore13 = ""
                    examCode14 = ""
                    examYear14 = ""
                    examScore14 = ""
                    examCode15 = ""
                    examYear15 = ""
                    examScore15 = ""
                    examCode16 = ""
                    examYear16 = ""
                    examScore16 = ""
                    examCode17 = ""
                    examYear17 = ""
                    examScore17 = ""
                    examCode18 = ""
                    examYear18 = ""
                    examScore18 = ""
                    examCode19 = ""
                    examYear19 = ""
                    examScore19 = ""
                    examCode20 = ""
                    examYear20 = ""
                    examScore20 = ""
                    examCode21 = ""
                    examYear21 = ""
                    examScore21 = ""
                    examCode22 = ""
                    examYear22 = ""
                    examScore22 = ""
                    examCode23 = ""
                    examYear23 = ""
                    examScore23 = ""
                    examCode24 = ""
                    examYear24 = ""
                    examScore24 = ""
                    examCode25 = ""
                    examYear25 = ""
                    examScore25 = ""
                    examCode26 = ""
                    examYear26 = ""
                    examScore26 = ""
                    examCode27 = ""
                    examYear27 = ""
                    examScore27 = ""
                    examCode28 = ""
                    examYear28 = ""
                    examScore28 = ""
                    examCode29 = ""
                    examYear29 = ""
                    examScore29 = ""
                    examCode30 = ""
                    examYear30 = ""
                    examScore30 = ""

                    'Exam Full dates
                    examDate = ""
                    examDate1 = ""
                    examDate2 = ""
                    examDate3 = ""
                    examDate4 = ""
                    examDate5 = ""
                    examDate6 = ""
                    examDate7 = ""
                    examDate8 = ""
                    examDate9 = ""
                    examDate10 = ""
                    examDate11 = ""
                    examDate12 = ""
                    examDate13 = ""
                    examDate14 = ""
                    examDate15 = ""
                    examDate16 = ""
                    examDate17 = ""
                    examDate18 = ""
                    examDate19 = ""
                    examDate20 = ""
                    examDate21 = ""
                    examDate22 = ""
                    examDate23 = ""
                    examDate24 = ""
                    examDate25 = ""
                    examDate26 = ""
                    examDate27 = ""
                    examDate28 = ""
                    examDate29 = ""
                    examDate30 = ""

                    'Exam Years
                    adminYear1 = ""
                    adminYear2 = ""
                    adminYear3 = ""
                    adminYear4 = ""
                    adminYear5 = ""
                    adminYear6 = ""
                    adminYear7 = ""
                    adminYear8 = ""
                    adminYear9 = ""
                    adminYear10 = ""
                    adminYear11 = ""
                    adminYear12 = ""
                    adminYear13 = ""
                    adminYear14 = ""
                    adminYear15 = ""
                    adminYear16 = ""
                    adminYear17 = ""
                    adminYear18 = ""
                    adminYear19 = ""
                    adminYear20 = ""
                    adminYear21 = ""
                    adminYear22 = ""
                    adminYear23 = ""
                    adminYear24 = ""
                    adminYear25 = ""
                    adminYear26 = ""
                    adminYear27 = ""
                    adminYear28 = ""
                    adminYear29 = ""
                    adminYear30 = ""
                    stuBuilding = ""

                    System.Windows.Forms.Application.DoEvents()

                Catch ex As FileIO.MalformedLineException
                    Stop
                End Try
            Loop


        'Loop



10:     If chkTestOnly.Checked = True Then
            lblComplete.Text = "File Test Complete!!"
        Else
            lblComplete.Text = "AP Test Import Complete!"

        End If

        lblComplete.Visible = True

        sr.Close()
        sr.Dispose()


        If RunInd = True Then
            Exit Sub
            Me.Close()
        End If

        If RT1.TextLength > 0 Then
            btnCTC.Enabled = True
        End If

        btnAP.Enabled = True
        chkTestOnly.Enabled = True
        chkDataSource.Enabled = True

    End Sub

    Private Sub btnAP_Disposed(sender As Object, e As EventArgs) Handles btnAP.Disposed

        RunInd = True
    End Sub

    Private Sub InstallNewTestsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles InstallAdditionalTests.Click


        Try

            'Look into the specified database and pull the AP test key to script the rest of the process!
            Dim connect2 As New SqlConnection(connection)
            Dim command2 As New SqlCommand
            Dim connectC As New SqlConnection(connection)
            Dim commandC As New SqlCommand
            'Dim TestKey As String = ""
            Dim TKey As Integer = 0
            Dim STO As Integer = 0
            Dim UpdateCount As Integer = 0
            'Check LTDB_TEST for existing record
            connect2 = New SqlConnection(connection)

            connect2.Open()
            'command.Cancel()

            command2 = New SqlCommand("SELECT TOP (1) * FROM LTDB_TEST WHERE District = '" & strDistrict & "' AND TEST_CODE = 'AP' ", connect2)

            Dim reader As SqlDataReader =
                          command2.ExecuteReader(
                          CommandBehavior.CloseConnection)

            'Console.WriteLine(reader2.HasRows)
            If reader.HasRows = True Then

                'We are good to continue with creation!
                'Grab the test key value
                While reader.Read

                    TKey = CInt(reader.Item("Test_Key"))

                End While

                reader.Close()
                reader.Dispose()
                connect2.Close()
                connect2.Dispose()

                'We will need the highest subtest order from the LTDB_SUBTEST FOR Application to the system

                connect2 = New SqlConnection(connection)

                connect2.Open()


                command2 = New SqlCommand("SELECT TOP (1) * FROM LTDB_SUBTEST WHERE District = '" & strDistrict & "' AND TEST_CODE = 'AP' ORDER BY SUBTEST_ORDER DESC ", connect2)

                Dim readerA As SqlDataReader =
                              command2.ExecuteReader(
                              CommandBehavior.CloseConnection)

                'Console.WriteLine(reader2.HasRows)
                If readerA.HasRows = True Then

                    'We are good to continue with creation!

                    'Grab the test key value
                    While readerA.Read

                        STO = CInt(readerA.Item("SUBTEST_ORDER")) + 1

                    End While

                End If

                readerA.Close()
                readerA.Dispose()
                connect2.Close()
                connect2.Dispose()

                'MsgBox(STO & " Test Key: " & TKey)

                ' Exit Sub


                'Check for scores that are missing before adding the scores to the spec!



                '22 Seminar
                connectC = New SqlConnection(connection)

                connectC.Open()
                'command.Cancel()

                command2 = New SqlCommand("SELECT TOP (1) * FROM LTDB_SUBTEST WHERE District = '" & strDistrict & "' AND TEST_CODE = 'AP' AND SUBTEST ='22' ", connectC)

                Dim readerC As SqlDataReader =
                              command2.ExecuteReader(
                              CommandBehavior.CloseConnection)

                'Console.WriteLine(reader2.HasRows)
                If readerC.HasRows = False Then

                    'We are good to continue with creation!

                    Dim cmdSUBTEST As New SqlCommand("SET NOCOUNT ON; " &
                                           "SET XACT_ABORT ON; " &
                                           "BEGIN TRANSACTION; " &
                                           "INSERT INTO [dbo].[LTDB_SUBTEST]([DISTRICT], [TEST_CODE], [TEST_LEVEL], [TEST_FORM], [TEST_KEY], [SUBTEST], [DESCRIPTION], [SUBTEST_ORDER], [DISPLAY], [STATE_CODE_EQUIV], [PESC_CODE], [CHANGE_DATE_TIME], [CHANGE_UID]) " &
                                           "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'22        ', N'Seminar'," & STO & ", N'Y', NULL, NULL, '20150811 12:12:36.000', N'TEST.IMPORTER' " &
                                           "COMMIT;", New SqlConnection(connection))

                    'MsgBox(cmdBase.CommandText)

                    cmdSUBTEST.Connection.Open()
                    cmdSUBTEST.ExecuteNonQuery()
                    cmdSUBTEST.Connection.Close()
                    cmdSUBTEST.Connection.Dispose()

                    'Now Add Sub Score items!

                    Dim cmdSUBTESTSC As New SqlCommand("SET NOCOUNT ON; " &
                                                "SET XACT_ABORT ON; " &
                                                "BEGIN TRANSACTION; " &
                                                "INSERT INTO [dbo].[LTDB_SUBTEST_SCORE]([DISTRICT], [TEST_CODE], [TEST_LEVEL], [TEST_FORM], [TEST_KEY], [SUBTEST], [SCORE_CODE], [SCORE_ORDER], [SCORE_LABEL], [REQUIRED], [FIELD_TYPE], [DATA_TYPE], [NUMBER_TYPE], [DATA_LENGTH], [FIELD_SCALE], [FIELD_PRECISION], [DEFAULT_VALUE], [VALIDATION_LIST], [VALIDATION_TABLE], [CODE_COLUMN], [DESCRIPTION_COLUMN], [DISPLAY], [INCLUDE_DASHBOARD], [MONTHS_TO_INCLUDE], [RANGE1_HIGH_LIMIT], [RANGE2_HIGH_LIMIT], [STATE_CODE_EQUIV], [SCORE_TYPE], [PERFPLUS_GROUP], [PESC_CODE], [CHANGE_DATE_TIME], [CHANGE_UID]) " &
                                                "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'22        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:15:47.000', N'JGATES' UNION ALL " &
                                                "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'22        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 10:12:27.000', N'JGATES' UNION ALL " &
                                                "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'22        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:42:25.000', N'JGATES' UNION ALL " &
                                                "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'22        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 07:42:53.000', N'JGATES' " &
                                                "COMMIT;", New SqlConnection(connection))

                    'MsgBox(cmdBase.CommandText)

                    cmdSUBTESTSC.Connection.Open()
                    cmdSUBTESTSC.ExecuteNonQuery()
                    cmdSUBTESTSC.Connection.Close()
                    cmdSUBTESTSC.Connection.Dispose()

                    'Increment in case other scores are missing!
                    STO = STO + 1
                    UpdateCount = UpdateCount + 1
                Else
                    'This code already exists, skip!

                End If


                readerC.Close()
                readerC.Dispose()

                '---------------------------------------------------------------------------------------------------------------------------------
                '28 Chinese Language and Culture
                connectC = New SqlConnection(connection)

                connectC.Open()
                'command.Cancel()

                command2 = New SqlCommand("SELECT TOP (1) * FROM LTDB_SUBTEST WHERE District = '" & strDistrict & "' AND TEST_CODE = 'AP' AND SUBTEST ='28' ", connectC)

                readerC =
                              command2.ExecuteReader(
                              CommandBehavior.CloseConnection)

                'Console.WriteLine(reader2.HasRows)
                If readerC.HasRows = False Then

                    'We are good to continue with creation!

                    Dim cmdSUBTEST As New SqlCommand("SET NOCOUNT ON; " &
                                           "SET XACT_ABORT ON; " &
                                           "BEGIN TRANSACTION; " &
                                           "INSERT INTO [dbo].[LTDB_SUBTEST]([DISTRICT], [TEST_CODE], [TEST_LEVEL], [TEST_FORM], [TEST_KEY], [SUBTEST], [DESCRIPTION], [SUBTEST_ORDER], [DISPLAY], [STATE_CODE_EQUIV], [PESC_CODE], [CHANGE_DATE_TIME], [CHANGE_UID]) " &
                                           "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'28        ', N'Chinese Language and Culture'," & STO & ", N'Y', NULL, NULL, '20150811 12:12:36.000', N'TEST.IMPORTER' " &
                                           "COMMIT;", New SqlConnection(connection))

                    'MsgBox(cmdBase.CommandText)

                    cmdSUBTEST.Connection.Open()
                    cmdSUBTEST.ExecuteNonQuery()
                    cmdSUBTEST.Connection.Close()
                    cmdSUBTEST.Connection.Dispose()

                    'Now Add Sub Score items!

                    Dim cmdSUBTESTSC As New SqlCommand("SET NOCOUNT ON; " &
                                                "SET XACT_ABORT ON; " &
                                                "BEGIN TRANSACTION; " &
                                                "INSERT INTO [dbo].[LTDB_SUBTEST_SCORE]([DISTRICT], [TEST_CODE], [TEST_LEVEL], [TEST_FORM], [TEST_KEY], [SUBTEST], [SCORE_CODE], [SCORE_ORDER], [SCORE_LABEL], [REQUIRED], [FIELD_TYPE], [DATA_TYPE], [NUMBER_TYPE], [DATA_LENGTH], [FIELD_SCALE], [FIELD_PRECISION], [DEFAULT_VALUE], [VALIDATION_LIST], [VALIDATION_TABLE], [CODE_COLUMN], [DESCRIPTION_COLUMN], [DISPLAY], [INCLUDE_DASHBOARD], [MONTHS_TO_INCLUDE], [RANGE1_HIGH_LIMIT], [RANGE2_HIGH_LIMIT], [STATE_CODE_EQUIV], [SCORE_TYPE], [PERFPLUS_GROUP], [PESC_CODE], [CHANGE_DATE_TIME], [CHANGE_UID]) " &
                                                "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'28        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:15:47.000', N'JGATES' UNION ALL " &
                                                "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'28        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 10:12:27.000', N'JGATES' UNION ALL " &
                                                "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'28        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:42:25.000', N'JGATES' UNION ALL " &
                                                "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'28        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 07:42:53.000', N'JGATES' " &
                                                "COMMIT;", New SqlConnection(connection))

                    'MsgBox(cmdBase.CommandText)

                    cmdSUBTESTSC.Connection.Open()
                    cmdSUBTESTSC.ExecuteNonQuery()
                    cmdSUBTESTSC.Connection.Close()
                    cmdSUBTESTSC.Connection.Dispose()

                    'Increment in case other scores are missing!
                    STO = STO + 1
                    UpdateCount = UpdateCount + 1
                Else
                    'This code already exists, skip!

                End If


                readerC.Close()
                readerC.Dispose()

                '---------------------------------------------------------------------------------------------------------------------------------
                '83 Physics 1
                connectC = New SqlConnection(connection)

                connectC.Open()
                'command.Cancel()

                command2 = New SqlCommand("SELECT TOP (1) * FROM LTDB_SUBTEST WHERE District = '" & strDistrict & "' AND TEST_CODE = 'AP' AND SUBTEST ='83' ", connectC)

                readerC =
                              command2.ExecuteReader(
                              CommandBehavior.CloseConnection)

                'Console.WriteLine(reader2.HasRows)
                If readerC.HasRows = False Then

                    'We are good to continue with creation!

                    Dim cmdSUBTEST As New SqlCommand("SET NOCOUNT ON; " &
                                           "SET XACT_ABORT ON; " &
                                           "BEGIN TRANSACTION; " &
                                           "INSERT INTO [dbo].[LTDB_SUBTEST]([DISTRICT], [TEST_CODE], [TEST_LEVEL], [TEST_FORM], [TEST_KEY], [SUBTEST], [DESCRIPTION], [SUBTEST_ORDER], [DISPLAY], [STATE_CODE_EQUIV], [PESC_CODE], [CHANGE_DATE_TIME], [CHANGE_UID]) " &
                                           "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'83        ', N'Physics 1'," & STO & ", N'Y', NULL, NULL, '20150811 12:12:36.000', N'TEST.IMPORTER' " &
                                           "COMMIT;", New SqlConnection(connection))

                    'MsgBox(cmdBase.CommandText)

                    cmdSUBTEST.Connection.Open()
                    cmdSUBTEST.ExecuteNonQuery()
                    cmdSUBTEST.Connection.Close()
                    cmdSUBTEST.Connection.Dispose()

                    'Now Add Sub Score items!

                    Dim cmdSUBTESTSC As New SqlCommand("SET NOCOUNT ON; " &
                                                "SET XACT_ABORT ON; " &
                                                "BEGIN TRANSACTION; " &
                                                "INSERT INTO [dbo].[LTDB_SUBTEST_SCORE]([DISTRICT], [TEST_CODE], [TEST_LEVEL], [TEST_FORM], [TEST_KEY], [SUBTEST], [SCORE_CODE], [SCORE_ORDER], [SCORE_LABEL], [REQUIRED], [FIELD_TYPE], [DATA_TYPE], [NUMBER_TYPE], [DATA_LENGTH], [FIELD_SCALE], [FIELD_PRECISION], [DEFAULT_VALUE], [VALIDATION_LIST], [VALIDATION_TABLE], [CODE_COLUMN], [DESCRIPTION_COLUMN], [DISPLAY], [INCLUDE_DASHBOARD], [MONTHS_TO_INCLUDE], [RANGE1_HIGH_LIMIT], [RANGE2_HIGH_LIMIT], [STATE_CODE_EQUIV], [SCORE_TYPE], [PERFPLUS_GROUP], [PESC_CODE], [CHANGE_DATE_TIME], [CHANGE_UID]) " &
                                                "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'83        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:15:47.000', N'JGATES' UNION ALL " &
                                                "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'83        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 10:12:27.000', N'JGATES' UNION ALL " &
                                                "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'83        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:42:25.000', N'JGATES' UNION ALL " &
                                                "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'83        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 07:42:53.000', N'JGATES' " &
                                                "COMMIT;", New SqlConnection(connection))

                    'MsgBox(cmdBase.CommandText)

                    cmdSUBTESTSC.Connection.Open()
                    cmdSUBTESTSC.ExecuteNonQuery()
                    cmdSUBTESTSC.Connection.Close()
                    cmdSUBTESTSC.Connection.Dispose()

                    'Increment in case other scores are missing!
                    STO = STO + 1
                    UpdateCount = UpdateCount + 1

                Else
                    'This code already exists, skip!

                End If


                readerC.Close()
                readerC.Dispose()

                '---------------------------------------------------------------------------------------------------------------------------------
                '84 Physics 2
                connectC = New SqlConnection(connection)

                connectC.Open()
                'command.Cancel()

                command2 = New SqlCommand("SELECT TOP (1) * FROM LTDB_SUBTEST WHERE District = '" & strDistrict & "' AND TEST_CODE = 'AP' AND SUBTEST ='84' ", connectC)

                readerC =
                              command2.ExecuteReader(
                              CommandBehavior.CloseConnection)

                'Console.WriteLine(reader2.HasRows)
                If readerC.HasRows = False Then

                    'We are good to continue with creation!

                    Dim cmdSUBTEST As New SqlCommand("SET NOCOUNT ON; " &
                                           "SET XACT_ABORT ON; " &
                                           "BEGIN TRANSACTION; " &
                                           "INSERT INTO [dbo].[LTDB_SUBTEST]([DISTRICT], [TEST_CODE], [TEST_LEVEL], [TEST_FORM], [TEST_KEY], [SUBTEST], [DESCRIPTION], [SUBTEST_ORDER], [DISPLAY], [STATE_CODE_EQUIV], [PESC_CODE], [CHANGE_DATE_TIME], [CHANGE_UID]) " &
                                           "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'84        ', N'Physics 2'," & STO & ", N'Y', NULL, NULL, '20150811 12:12:36.000', N'TEST.IMPORTER' " &
                                           "COMMIT;", New SqlConnection(connection))

                    'MsgBox(cmdBase.CommandText)

                    cmdSUBTEST.Connection.Open()
                    cmdSUBTEST.ExecuteNonQuery()
                    cmdSUBTEST.Connection.Close()
                    cmdSUBTEST.Connection.Dispose()

                    'Now Add Sub Score items!

                    Dim cmdSUBTESTSC As New SqlCommand("SET NOCOUNT ON; " &
                                                "SET XACT_ABORT ON; " &
                                                "BEGIN TRANSACTION; " &
                                                "INSERT INTO [dbo].[LTDB_SUBTEST_SCORE]([DISTRICT], [TEST_CODE], [TEST_LEVEL], [TEST_FORM], [TEST_KEY], [SUBTEST], [SCORE_CODE], [SCORE_ORDER], [SCORE_LABEL], [REQUIRED], [FIELD_TYPE], [DATA_TYPE], [NUMBER_TYPE], [DATA_LENGTH], [FIELD_SCALE], [FIELD_PRECISION], [DEFAULT_VALUE], [VALIDATION_LIST], [VALIDATION_TABLE], [CODE_COLUMN], [DESCRIPTION_COLUMN], [DISPLAY], [INCLUDE_DASHBOARD], [MONTHS_TO_INCLUDE], [RANGE1_HIGH_LIMIT], [RANGE2_HIGH_LIMIT], [STATE_CODE_EQUIV], [SCORE_TYPE], [PERFPLUS_GROUP], [PESC_CODE], [CHANGE_DATE_TIME], [CHANGE_UID]) " &
                                                "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'84        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:15:47.000', N'JGATES' UNION ALL " &
                                                "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'84        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 10:12:27.000', N'JGATES' UNION ALL " &
                                                "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'84        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:42:25.000', N'JGATES' UNION ALL " &
                                                "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'84        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 07:42:53.000', N'JGATES' " &
                                                "COMMIT;", New SqlConnection(connection))

                    'MsgBox(cmdBase.CommandText)

                    cmdSUBTESTSC.Connection.Open()
                    cmdSUBTESTSC.ExecuteNonQuery()
                    cmdSUBTESTSC.Connection.Close()
                    cmdSUBTESTSC.Connection.Dispose()

                    'Increment in case other scores are missing!
                    STO = STO + 1
                    UpdateCount = UpdateCount + 1
                Else
                    'This code already exists, skip!

                End If

                '---------------------------------------------------------------------------------------------------------------------------------
                '23 Research
                connectC = New SqlConnection(connection)

                connectC.Open()
                'command.Cancel()

                command2 = New SqlCommand("SELECT TOP (1) * FROM LTDB_SUBTEST WHERE District = '" & strDistrict & "' AND TEST_CODE = 'AP' AND SUBTEST ='23' ", connectC)

                readerC =
                              command2.ExecuteReader(
                              CommandBehavior.CloseConnection)

                'Console.WriteLine(reader2.HasRows)
                If readerC.HasRows = False Then

                    'We are good to continue with creation!

                    Dim cmdSUBTEST As New SqlCommand("SET NOCOUNT ON; " &
                                           "SET XACT_ABORT ON; " &
                                           "BEGIN TRANSACTION; " &
                                           "INSERT INTO [dbo].[LTDB_SUBTEST]([DISTRICT], [TEST_CODE], [TEST_LEVEL], [TEST_FORM], [TEST_KEY], [SUBTEST], [DESCRIPTION], [SUBTEST_ORDER], [DISPLAY], [STATE_CODE_EQUIV], [PESC_CODE], [CHANGE_DATE_TIME], [CHANGE_UID]) " &
                                           "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'23        ', N'Research'," & STO & ", N'Y', NULL, NULL, '20150811 12:12:36.000', N'TEST.IMPORTER' " &
                                           "COMMIT;", New SqlConnection(connection))

                    'MsgBox(cmdBase.CommandText)

                    cmdSUBTEST.Connection.Open()
                    cmdSUBTEST.ExecuteNonQuery()
                    cmdSUBTEST.Connection.Close()
                    cmdSUBTEST.Connection.Dispose()

                    'Now Add Sub Score items!

                    Dim cmdSUBTESTSC As New SqlCommand("SET NOCOUNT ON; " &
                                                "SET XACT_ABORT ON; " &
                                                "BEGIN TRANSACTION; " &
                                                "INSERT INTO [dbo].[LTDB_SUBTEST_SCORE]([DISTRICT], [TEST_CODE], [TEST_LEVEL], [TEST_FORM], [TEST_KEY], [SUBTEST], [SCORE_CODE], [SCORE_ORDER], [SCORE_LABEL], [REQUIRED], [FIELD_TYPE], [DATA_TYPE], [NUMBER_TYPE], [DATA_LENGTH], [FIELD_SCALE], [FIELD_PRECISION], [DEFAULT_VALUE], [VALIDATION_LIST], [VALIDATION_TABLE], [CODE_COLUMN], [DESCRIPTION_COLUMN], [DISPLAY], [INCLUDE_DASHBOARD], [MONTHS_TO_INCLUDE], [RANGE1_HIGH_LIMIT], [RANGE2_HIGH_LIMIT], [STATE_CODE_EQUIV], [SCORE_TYPE], [PERFPLUS_GROUP], [PESC_CODE], [CHANGE_DATE_TIME], [CHANGE_UID]) " &
                                                "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'23        ', N'ADMY      ', 1, N'Admin Year', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'ADMY      ', NULL, NULL, '20101118 10:15:47.000', N'JGATES' UNION ALL " &
                                                "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'23        ', N'SCORE     ', 2, N'SCORE', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'SCORE     ', NULL, NULL, '20101118 10:12:27.000', N'JGATES' UNION ALL " &
                                                "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'23        ', N'IRegC1    ', 3, N'Irregularity Code 1 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG1     ', NULL, NULL, '20120103 07:42:25.000', N'JGATES' UNION ALL " &
                                                "SELECT " & strDistrict & ", N'AP                  ', N'1         ', N'1         ', " & TKey & ", N'23        ', N'IRegC2    ', 4, N'Irregularity Code 2 (AP)', N'N', N'I', N'C', N'I', 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Y', N'N', NULL, NULL, NULL, NULL, N'IREG2     ', NULL, NULL, '20120103 07:42:53.000', N'JGATES' " &
                                                "COMMIT;", New SqlConnection(connection))
                    'MsgBox(cmdBase.CommandText)

                    cmdSUBTESTSC.Connection.Open()
                    cmdSUBTESTSC.ExecuteNonQuery()
                    cmdSUBTESTSC.Connection.Close()
                    cmdSUBTESTSC.Connection.Dispose()

                    'Increment in case other scores are missing!
                    STO = STO + 1
                    UpdateCount = UpdateCount + 1
                Else
                    'This code already exists, skip!

                End If


                readerC.Close()
                readerC.Dispose()


                'MsgBox("Complete!")
                If UpdateCount > 0 Then

                    'New notification display!  
                    lblDisErr.Text = "The AP Test Score Definition was updated successfully!"

                Else


                    'New notification display!  
                    lblDisErr.Text = "The AP Test Score Definition has already been updated!"


                End If


                pbUpdateNeeded.Visible = False

            Else

                'Trouble time to exit!


                Dim DBDisplayString As Array
                Dim DBString As String = ""

                DBDisplayString = Split(connection, ";")

                DBString = "(Current Database: " & Replace(DBDisplayString(1), "Initial Catalog=", "") & ")"

                lblDisErr.Text = "Base AP test record does not exist in " & DBString & " !!  You need to Create AP Test Definition from the choice menu!!"


                Exit Sub

            End If

        Catch ex As Exception
            lblDisErr.Text = "An error occurred and the test import specification was not updated!"

        End Try


    End Sub

    Private Sub btnCTC_Click(sender As Object, e As EventArgs) Handles btnCTC.Click

        Try

            RT1.SelectAll()
            RT1.Copy()

            lblDisErr.Text = "Output Copied to Clipboard!"

        Catch ex As Exception
            lblDisErr.Text = "An error occurred, clipboard text was not copied!"

        End Try


    End Sub

    Private Sub ParametersMaintenanceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ParametersMaintenanceToolStripMenuItem.Click
        Parameters.ShowDialog()
        Me.Refresh()

    End Sub

    Sub UpgradeMySetings()

        'MsgBox(My.Settings.MustUpgrade)

        If My.Settings.MustUpgrade Then
            My.Settings.Upgrade()
            My.Settings.MustUpgrade = False
            My.Settings.Save()
        End If


    End Sub

    Sub RefreshDatasource()
        If chkDataSource.Checked = True Then

            connection = strConnLive

        Else

            connection = strConnDev

        End If

        Dim DBDisplayString As Array

        DBDisplayString = Split(connection, ";")

        If DBDisplayString.Length = 1 Then

            'something is not initialized properly!

        Else

            lblDatabaseLocation.Text = "(Current Database: " & Replace(DBDisplayString(1), "Initial Catalog=", "") & ")"
        End If


    End Sub

End Class


