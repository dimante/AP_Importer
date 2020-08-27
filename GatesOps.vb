Imports System.IO
Imports System.Data.SqlClient
Imports WPFGrowlNotification
Imports System.Windows

Module GatesOps


    Public DBServerNM As String = ""
    Public DBDevServerNM As String = ""
    Public DBServerDBNM As String = ""
    Public DBDevServerDBNM As String = ""

    Public DBUN As String = ""
    Public DBPassword As String = ""
    Public DBDEVUN As String = ""
    Public DBDevPassword As String = ""

    Public strDistrict As String = ""
    Public IntegratedSecurity As String = ""

    Public strConn As String = ""
    Public strConn3 As String = ""

    Public userLevel As String = ""

    'Live DB Init String
    Public strConnLive As String = ""
    'Development DB Init String
    Public strConnDev As String = ""

    'If program closes set to true to abort current process!
    Public RunInd As Boolean = False
    Public chkFlag As Boolean = False

    Public Sub APTestInsertUpdate(ByVal connection As String, ByVal strID As String, ByVal examCode As String, ByVal TestCode As String, ByVal testKey As String, ByVal examDate As String, ByVal examScore As String, ByVal adminYear As String, ByVal IReg1 As String, ByVal IReg2 As String)
        'Routine to process test updates (Get rid of redundant code)
        'Subtest Routine

        'Check for duplicate record.  If so apply update logic.
        System.Windows.Forms.Application.DoEvents()

        Dim connectT1 As New SqlConnection(connection)

        connectT1.Open()

        Dim commandT1 As New SqlCommand("SELECT TOP (1) * FROM LTDB_STU_SUBTEST WHERE District = '" & strDistrict & "' AND TEST_CODE = 'AP' AND Student_ID = '" & strID & "' AND SUBTEST = '" & examCode & "' AND SCORE_CODE ='SCORE'", connectT1)

        Dim readerT1 As SqlDataReader =
                      commandT1.ExecuteReader(
                      CommandBehavior.CloseConnection)

        If readerT1.HasRows = False Then
            'Test Code does not exist insert
            Try


                Dim cmdSUB_1 As New SqlCommand("INSERT INTO LTDB_STU_SUBTEST (DISTRICT, TEST_CODE, TEST_LEVEL, TEST_FORM, TEST_KEY, STUDENT_ID, TEST_DATE, SUBTEST, SCORE_CODE, SCORE, CHANGE_DATE_TIME, CHANGE_UID)VALUES('" & strDistrict & "', '" & TestCode & "', '1','1','" & testKey & "','" & strID & "','" & examDate & "', '" & examCode & "', 'SCORE','" & examScore & "','" & Date.Now.ToShortDateString & " " & Date.Now.ToShortTimeString & "'" & ",'Test.Import')", New SqlConnection(connection))

                cmdSUB_1.Connection.Open()
                cmdSUB_1.ExecuteNonQuery()
                cmdSUB_1.Connection.Close()
                cmdSUB_1.Connection.Dispose()


                Dim cmdSUB_1a As New SqlCommand("INSERT INTO LTDB_STU_SUBTEST (DISTRICT, TEST_CODE, TEST_LEVEL, TEST_FORM, TEST_KEY, STUDENT_ID, TEST_DATE, SUBTEST, SCORE_CODE, SCORE, CHANGE_DATE_TIME, CHANGE_UID)VALUES('" & strDistrict & "', '" & TestCode & "', '1','1','" & testKey & "','" & strID & "','" & examDate & "', '" & examCode & "', 'ADMY','" & adminYear & "','" & Date.Now.ToShortDateString & " " & Date.Now.ToShortTimeString & "'" & ",'Test.Import')", New SqlConnection(connection))

                cmdSUB_1a.Connection.Open()
                cmdSUB_1a.ExecuteNonQuery()
                cmdSUB_1a.Connection.Close()
                cmdSUB_1a.Connection.Dispose()

                'Write irregularity codes (If Present)
                Dim cmdSUB_1b As New SqlCommand("INSERT INTO LTDB_STU_SUBTEST (DISTRICT, TEST_CODE, TEST_LEVEL, TEST_FORM, TEST_KEY, STUDENT_ID, TEST_DATE, SUBTEST, SCORE_CODE, SCORE, CHANGE_DATE_TIME, CHANGE_UID)VALUES('" & strDistrict & "', '" & TestCode & "', '1','1','" & testKey & "','" & strID & "','" & examDate & "', '" & examCode & "', 'IRegC1','" & IReg1 & "','" & Date.Now.ToShortDateString & " " & Date.Now.ToShortTimeString & "'" & ",'Test.Import')", New SqlConnection(connection))

                cmdSUB_1b.Connection.Open()
                cmdSUB_1b.ExecuteNonQuery()
                cmdSUB_1b.Connection.Close()
                cmdSUB_1b.Connection.Dispose()

                Dim cmdSUB_1c As New SqlCommand("INSERT INTO LTDB_STU_SUBTEST (DISTRICT, TEST_CODE, TEST_LEVEL, TEST_FORM, TEST_KEY, STUDENT_ID, TEST_DATE, SUBTEST, SCORE_CODE, SCORE, CHANGE_DATE_TIME, CHANGE_UID)VALUES('" & strDistrict & "', '" & TestCode & "', '1','1','" & testKey & "','" & strID & "','" & examDate & "', '" & examCode & "', 'IRegC2','" & IReg2 & "','" & Date.Now.ToShortDateString & " " & Date.Now.ToShortTimeString & "'" & ",'Test.Import')", New SqlConnection(connection))

                cmdSUB_1c.Connection.Open()
                cmdSUB_1c.ExecuteNonQuery()
                cmdSUB_1c.Connection.Close()
                cmdSUB_1c.Connection.Dispose()

            Catch ex As Exception

                Import.lblDisErr.Text = ex.Message

            End Try


        Else
            'Record exists, update!

            While readerT1.Read

                Dim chkScore As String = ""

                chkScore = readerT1.Item("SCORE").ToString

                If examScore >= chkScore Then

                    chkFlag = True

                Else

                    chkFlag = False

                End If

            End While


            Try

                If chkFlag = True Then
                    'Update the new test record
                    Try

                        'Score
                        Dim cmdSUB_1 As New SqlCommand("UPDATE LTDB_STU_SUBTEST SET SCORE = '" & examScore & "', TEST_DATE = '" & examDate & "', CHANGE_DATE_TIME = '" & Date.Now.ToShortDateString & " " & Date.Now.ToShortTimeString & "', CHANGE_UID = 'Test.Import' WHERE DISTRICT = '" & strDistrict & "' AND TEST_CODE = '" & TestCode & "' AND SUBTEST = '" & examCode & "' AND STUDENT_ID = '" & strID & "' AND SCORE_CODE = 'SCORE'", New SqlConnection(connection))

                        ''debug.Print(cmdSUB1.CommandText.ToString)

                        cmdSUB_1.Connection.Open()
                        cmdSUB_1.ExecuteNonQuery()
                        cmdSUB_1.Connection.Close()
                        cmdSUB_1.Connection.Dispose()

                        'Admin Year
                        Dim cmdSUB_1a As New SqlCommand("UPDATE LTDB_STU_SUBTEST SET SCORE = '" & adminYear & "', TEST_DATE = '" & examDate & "', CHANGE_DATE_TIME = '" & Date.Now.ToShortDateString & " " & Date.Now.ToShortTimeString & "', CHANGE_UID = 'Test.Import' WHERE DISTRICT = '" & strDistrict & "' AND TEST_CODE = '" & TestCode & "' AND SUBTEST = '" & examCode & "' AND STUDENT_ID = '" & strID & "' AND SCORE_CODE = 'ADMY'", New SqlConnection(connection))

                        cmdSUB_1a.Connection.Open()
                        cmdSUB_1a.ExecuteNonQuery()
                        cmdSUB_1a.Connection.Close()
                        cmdSUB_1a.Connection.Dispose()


                        'Write irregularity codes (If Present)
                        Dim cmdSUB_1b As New SqlCommand("UPDATE LTDB_STU_SUBTEST SET SCORE = '" & IReg1 & "', TEST_DATE = '" & examDate & "', CHANGE_DATE_TIME = '" & Date.Now.ToShortDateString & " " & Date.Now.ToShortTimeString & "', CHANGE_UID = 'Test.Import' WHERE DISTRICT = '" & strDistrict & "' AND TEST_CODE = '" & TestCode & "' AND SUBTEST = '" & examCode & "' AND STUDENT_ID = '" & strID & "' AND SCORE_CODE = 'IRegC1'", New SqlConnection(connection))

                        cmdSUB_1b.Connection.Open()
                        cmdSUB_1b.ExecuteNonQuery()
                        cmdSUB_1b.Connection.Close()
                        cmdSUB_1b.Connection.Dispose()

                        Dim cmdSUB_1c As New SqlCommand("UPDATE LTDB_STU_SUBTEST SET SCORE = '" & IReg2 & "', TEST_DATE = '" & examDate & "', CHANGE_DATE_TIME = '" & Date.Now.ToShortDateString & " " & Date.Now.ToShortTimeString & "', CHANGE_UID = 'Test.Import' WHERE DISTRICT = '" & strDistrict & "' AND TEST_CODE = '" & TestCode & "' AND SUBTEST = '" & examCode & "' AND STUDENT_ID = '" & strID & "' AND SCORE_CODE = 'IRegC2'", New SqlConnection(connection))

                        cmdSUB_1c.Connection.Open()
                        cmdSUB_1c.ExecuteNonQuery()
                        cmdSUB_1c.Connection.Close()
                        cmdSUB_1c.Connection.Dispose()

                    Catch ex As Exception
                        Import.lblDisErr.Text = ex.Message

                    End Try

                End If

                'Clear flag for next read!

                chkFlag = False

            Catch ex As Exception

                Import.lblDisErr.Text = ex.Message

            End Try

        End If

        readerT1.Close()
        connectT1.Dispose()
        readerT1.Dispose()
        'End Subtest Routine!
        System.Windows.Forms.Application.DoEvents()

    End Sub
    Public Sub PLTWTestInsertUpdate(ByVal connection As String, ByVal strID As String, ByVal examCode As String, ByVal TestCode As String, ByVal testKey As String, ByVal examDate As String, ByVal examScore As String, ByVal adminYear As String, ByVal achLevel As String)
        'Routine to process test updates (Get rid of redundant code)
        'Subtest Routine

        'Check for duplicate record.  If so apply update logic.
        System.Windows.Forms.Application.DoEvents()

        Dim connectT1 As New SqlConnection(connection)

        connectT1.Open()

        Dim commandT1 As New SqlCommand("SELECT TOP (1) * FROM LTDB_STU_SUBTEST WHERE District = '" & strDistrict & "' AND TEST_CODE = 'PLTW' AND Student_ID = '" & strID & "' AND SUBTEST = '" & examCode & "' AND SCORE_CODE ='SCORE'", connectT1)

        Dim readerT1 As SqlDataReader =
                      commandT1.ExecuteReader(
                      CommandBehavior.CloseConnection)

        If readerT1.HasRows = False Then
            'Test Code does not exist insert
            Try


                Dim cmdSUB_1 As New SqlCommand("INSERT INTO LTDB_STU_SUBTEST (DISTRICT, TEST_CODE, TEST_LEVEL, TEST_FORM, TEST_KEY, STUDENT_ID, TEST_DATE, SUBTEST, SCORE_CODE, SCORE, CHANGE_DATE_TIME, CHANGE_UID)VALUES('" & strDistrict & "', '" & TestCode & "', '1','1','" & testKey & "','" & strID & "','" & examDate & "', '" & examCode & "', 'SCORE','" & examScore & "','" & Date.Now.ToShortDateString & " " & Date.Now.ToShortTimeString & "'" & ",'Test.Import')", New SqlConnection(connection))

                cmdSUB_1.Connection.Open()
                cmdSUB_1.ExecuteNonQuery()
                cmdSUB_1.Connection.Close()
                cmdSUB_1.Connection.Dispose()


                Dim cmdSUB_1a As New SqlCommand("INSERT INTO LTDB_STU_SUBTEST (DISTRICT, TEST_CODE, TEST_LEVEL, TEST_FORM, TEST_KEY, STUDENT_ID, TEST_DATE, SUBTEST, SCORE_CODE, SCORE, CHANGE_DATE_TIME, CHANGE_UID)VALUES('" & strDistrict & "', '" & TestCode & "', '1','1','" & testKey & "','" & strID & "','" & examDate & "', '" & examCode & "', 'ADMY','" & adminYear & "','" & Date.Now.ToShortDateString & " " & Date.Now.ToShortTimeString & "'" & ",'Test.Import')", New SqlConnection(connection))

                cmdSUB_1a.Connection.Open()
                cmdSUB_1a.ExecuteNonQuery()
                cmdSUB_1a.Connection.Close()
                cmdSUB_1a.Connection.Dispose()

                'Write Achievement Level (If Present)
                Dim cmdSUB_1b As New SqlCommand("INSERT INTO LTDB_STU_SUBTEST (DISTRICT, TEST_CODE, TEST_LEVEL, TEST_FORM, TEST_KEY, STUDENT_ID, TEST_DATE, SUBTEST, SCORE_CODE, SCORE, CHANGE_DATE_TIME, CHANGE_UID)VALUES('" & strDistrict & "', '" & TestCode & "', '1','1','" & testKey & "','" & strID & "','" & examDate & "', '" & examCode & "', 'ACHVL','" & achLevel & "','" & Date.Now.ToShortDateString & " " & Date.Now.ToShortTimeString & "'" & ",'Test.Import')", New SqlConnection(connection))

                cmdSUB_1b.Connection.Open()
                cmdSUB_1b.ExecuteNonQuery()
                cmdSUB_1b.Connection.Close()
                cmdSUB_1b.Connection.Dispose()

                'Dim cmdSUB_1c As New SqlCommand("INSERT INTO LTDB_STU_SUBTEST (DISTRICT, TEST_CODE, TEST_LEVEL, TEST_FORM, TEST_KEY, STUDENT_ID, TEST_DATE, SUBTEST, SCORE_CODE, SCORE, CHANGE_DATE_TIME, CHANGE_UID)VALUES('" & strDistrict & "', '" & TestCode & "', '1','1','" & testKey & "','" & strID & "','" & examDate & "', '" & examCode & "', 'IRegC2','" & IReg2 & "','" & Date.Now.ToShortDateString & " " & Date.Now.ToShortTimeString & "'" & ",'Test.Import')", New SqlConnection(connection))

                'cmdSUB_1c.Connection.Open()
                'cmdSUB_1c.ExecuteNonQuery()
                'cmdSUB_1c.Connection.Close()
                'cmdSUB_1c.Connection.Dispose()

            Catch ex As Exception
                'MsgBox(ex.Message)
                Import.lblDisErr.Text = ex.Message

            End Try


        Else
            'Record exists, update!

            While readerT1.Read

                Dim chkScore As String = ""

                chkScore = readerT1.Item("SCORE").ToString

                If examScore >= chkScore Then

                    chkFlag = True

                Else

                    chkFlag = False

                End If

            End While


            Try

                If chkFlag = True Then
                    'Update the new test record
                    Try

                        'Score
                        Dim cmdSUB_1 As New SqlCommand("UPDATE LTDB_STU_SUBTEST SET SCORE = '" & examScore & "', TEST_DATE = '" & examDate & "', CHANGE_DATE_TIME = '" & Date.Now.ToShortDateString & " " & Date.Now.ToShortTimeString & "', CHANGE_UID = 'Test.Import' WHERE DISTRICT = '" & strDistrict & "' AND TEST_CODE = '" & TestCode & "' AND SUBTEST = '" & examCode & "' AND STUDENT_ID = '" & strID & "' AND SCORE_CODE = 'SCORE'", New SqlConnection(connection))

                        ''debug.Print(cmdSUB1.CommandText.ToString)

                        cmdSUB_1.Connection.Open()
                        cmdSUB_1.ExecuteNonQuery()
                        cmdSUB_1.Connection.Close()
                        cmdSUB_1.Connection.Dispose()

                        'Admin Year
                        Dim cmdSUB_1a As New SqlCommand("UPDATE LTDB_STU_SUBTEST SET SCORE = '" & adminYear & "', TEST_DATE = '" & examDate & "', CHANGE_DATE_TIME = '" & Date.Now.ToShortDateString & " " & Date.Now.ToShortTimeString & "', CHANGE_UID = 'Test.Import' WHERE DISTRICT = '" & strDistrict & "' AND TEST_CODE = '" & TestCode & "' AND SUBTEST = '" & examCode & "' AND STUDENT_ID = '" & strID & "' AND SCORE_CODE = 'ADMY'", New SqlConnection(connection))

                        cmdSUB_1a.Connection.Open()
                        cmdSUB_1a.ExecuteNonQuery()
                        cmdSUB_1a.Connection.Close()
                        cmdSUB_1a.Connection.Dispose()


                        'Write irregularity codes (If Present)
                        Dim cmdSUB_1b As New SqlCommand("UPDATE LTDB_STU_SUBTEST SET SCORE = '" & achLevel & "', TEST_DATE = '" & examDate & "', CHANGE_DATE_TIME = '" & Date.Now.ToShortDateString & " " & Date.Now.ToShortTimeString & "', CHANGE_UID = 'Test.Import' WHERE DISTRICT = '" & strDistrict & "' AND TEST_CODE = '" & TestCode & "' AND SUBTEST = '" & examCode & "' AND STUDENT_ID = '" & strID & "' AND SCORE_CODE = 'ACHVL'", New SqlConnection(connection))

                        cmdSUB_1b.Connection.Open()
                        cmdSUB_1b.ExecuteNonQuery()
                        cmdSUB_1b.Connection.Close()
                        cmdSUB_1b.Connection.Dispose()


                    Catch ex As Exception
                        'MsgBox(ex.Message)
                        Import.lblDisErr.Text = ex.Message

                    End Try

                End If

                'Clear flag for next read!

                chkFlag = False

            Catch ex As Exception

                Import.lblDisErr.Text = ex.Message

            End Try

        End If

        readerT1.Close()
        connectT1.Dispose()
        readerT1.Dispose()
        'End Subtest Routine!
        System.Windows.Forms.Application.DoEvents()

    End Sub


End Module
