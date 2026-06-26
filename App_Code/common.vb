Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Imports System.Security.Cryptography
Imports System.IO
Imports System.Text
Public Class common
    Public Shared Function Encrypt(clearText As String) As String
        Dim encryptionKey As String = "MAKV2SPBNI99212"
        Dim clearBytes As Byte() = Encoding.Unicode.GetBytes(clearText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(encryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D, _
             &H65, &H64, &H76, &H65, &H64, &H65, &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write)
                    cs.Write(clearBytes, 0, clearBytes.Length)
                    cs.Close()
                End Using
                clearText = Convert.ToBase64String(ms.ToArray())
            End Using
        End Using
        Return clearText
    End Function
    Public shared Function Decrypt(cipherText As String) As String
        Dim encryptionKey As String = "MAKV2SPBNI99212"
        Dim cipherBytes As Byte() = Convert.FromBase64String(cipherText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(encryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D, _
             &H65, &H64, &H76, &H65, &H64, &H65, &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write)
                    cs.Write(cipherBytes, 0, cipherBytes.Length)
                    cs.Close()
                End Using
                cipherText = Encoding.Unicode.GetString(ms.ToArray())
            End Using
        End Using
        Return cipherText
    End Function
    Public Shared Function GetLogin(ByRef user As System.Security.Principal.IPrincipal) As String

        Dim strConstring As String = System.Configuration.ConfigurationManager.ConnectionStrings("connectionString").ConnectionString
        Dim EmployeeID As String = ""
        Dim strUName As String = user.Identity.Name.ToString()
        strUName = strUName.Substring(strUName.IndexOf("\") + 1)

        Using con As New SqlConnection(strConstring)
            Dim Query As String = "SELECT top 1 U.Employee_ID FROM [tblUser_Access] U inner join synEmployee E on U.Employee_ID = E.EMPLOYEE_ID where SUBSTRING(E.E_MAIL, 0, charindex('@', E.E_MAIL, 0)) = '" + strUName + "'"
            Using acmd As New SqlCommand(Query, con)
                acmd.CommandType = CommandType.Text
                acmd.Connection = con

                Try
                    con.Open()
                    Dim reader = acmd.ExecuteReader()
                    While reader.Read()
                        EmployeeID = Convert.ToString(reader.Item(0))
                    End While
                    reader.Close()
                    con.Close()

                Catch ex As Exception

                End Try
            End Using

        End Using

        'Return strUName
        Return strUName + "-" + EmployeeID
    End Function
    Public Shared Sub ShowMsg(page As Page, Message As String, Optional redirectTo As String = Nothing)
        Dim redirect As String = ""
        If Not String.IsNullOrEmpty(redirectTo) Then
            redirect = String.Format("window.location.href='{0}'", redirectTo.Replace("'", "\'"))
        End If
        page.ClientScript.RegisterStartupScript( _
          page.GetType(), _
          "MessageBox", _
          "<script language='javascript'>alert('" + Message + "');" & redirect & "</script>")
    End Sub

    Public Shared Function GetRoleID(ByRef user As String) As String
        Dim strConstring As String = System.Configuration.ConfigurationManager.ConnectionStrings("connectionString").ConnectionString
        Dim RoleID As Integer = 0

        Using con As New SqlConnection(strConstring)
            Dim Query As String = "SELECT TOP 1 [User_Group_ID] FROM [tblUser_Access] U inner join SynEmployee E on E.EMPLOYEE_ID = U.Employee_ID where  [Maintain_Action] <> 'Deleted' and U.EMPLOYEE_ID = '" + CStr(user) + "' order by U.Maintain_Date Desc"
            Using acmd As New SqlCommand(Query, con)
                acmd.CommandType = CommandType.Text
                acmd.Connection = con

                Try
                    con.Open()
                    Dim reader = acmd.ExecuteReader()
                    While reader.Read()
                        RoleID = Convert.ToString(reader.Item(0))
                    End While
                    reader.Close()
                    con.Close()

                Catch ex As Exception

                End Try

            End Using
        End Using


        Return RoleID
    End Function

    Public Shared Sub PopulateCombo(ByVal strValueFieldID As String, ByVal strTextField As String, ByVal strTable As String, ByVal strAppendWhere As String, ByVal DefaultText As String, ByVal cboCombo As DropDownList)

        Dim strSQL As String

        strSQL = ""
        strSQL = "SELECT Distinct " & strValueFieldID & ", " & strTextField & " FROM " & strTable & " WHERE " & strAppendWhere & " ORDER BY " & strTextField

        Dim constr As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand(strSQL)
                cmd.CommandType = CommandType.Text
                cmd.Connection = con
                con.Open()
                cboCombo.Items.Clear()
                cboCombo.DataSource = cmd.ExecuteReader()
                cboCombo.DataTextField = strTextField
                Dim d As String = cboCombo.SelectedValue
                cboCombo.DataValueField = strValueFieldID
                cboCombo.DataBind()
                con.Close()
            End Using
        End Using
        'cboCombo.Items.Insert(0, New ListItem("-Please Select-", "0"))

    End Sub
    Public Shared Sub PopulateCombo_UserCentral(ByVal strValueFieldID As String, ByVal strTextField As String, ByVal strTextField_LastName As String, ByVal strTable As String, ByVal DefaultText As String, ByVal cboCombo As DropDownList)

        Dim strSQL As String

        strSQL = ""
        strSQL = "SELECT Distinct " & strValueFieldID & ", " & strTextField & " +' '+  " & strTextField_LastName & " AS FullName FROM " & strTable & " where EMPLOYEE_TYPE <>'Former' order by 'FullName' asc"

        Dim constr As String = ConfigurationManager.ConnectionStrings("connectionString").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand(strSQL)
                cmd.CommandType = CommandType.Text
                cmd.Connection = con
                con.Open()
                cboCombo.Items.Clear()
                cboCombo.DataSource = cmd.ExecuteReader()
                cboCombo.DataTextField = "FullName"
                Dim d As String = cboCombo.SelectedValue
                cboCombo.DataValueField = strValueFieldID
                cboCombo.DataBind()
                con.Close()
            End Using
        End Using
        cboCombo.Items.Insert(0, New ListItem("--" & DefaultText & "--", "0"))

    End Sub
    Public Shared Function GetWorkflow_Email(ByRef Service_Id As String) As String
        GetWorkflow_Email = ""

        Using con As New SqlConnection(AppGlobals.ConnectionString)
            Using acmd As New SqlCommand("Select Service_ID, Company_Name,email from qryCompInfo_Services Where service_id = '" & Service_Id & "' AND Company_Maintain_Action <> 'Deleted'", con)
                Try
                    con.Open()
                    Dim reader As SqlDataReader = acmd.ExecuteReader()
                    If reader.HasRows Then
                        reader.Read()
                        GetWorkflow_Email = reader.GetValue(reader.GetOrdinal("Email"))
                        reader.Close()
                    End If

                Catch ex As Exception

                End Try

            End Using
        End Using
        Return GetWorkflow_Email

    End Function
    Public Shared Sub DisableControls(ByVal myPanel As Panel)

        '==========Clear textboxes in the panels first===========

        For Each control In myPanel.Controls
            LockControls(control)
            For Each child In control.controls
                LockControls(child)
                For Each child1 In child.controls
                    LockControls(child1)

                Next
            Next
        Next

    End Sub
    Public Shared Sub LockControls(control)
        If TypeOf control Is TextBox Then
            control.enabled = False
        ElseIf TypeOf control Is DropDownList Then
            control.enabled = False
        End If
    End Sub
    Public Shared Sub ClearControls(ByVal myPanel As Panel)

        '==========Clear textboxes in the panels first===========

        For Each control In myPanel.Controls
            resetControls(control)
            For Each child In control.controls
                resetControls(child)
                For Each child1 In child.controls
                    resetControls(child1)
                Next
            Next
        Next

        For Each control In myPanel.Controls
            resetControls(control)
            For Each child In control.controls
                resetControls(child)
                For Each child1 In child.controls
                    resetControls(child1)
                Next
            Next
        Next

        '===============================================
    End Sub
    Public Shared Sub resetControls(control)
        If TypeOf control Is TextBox Then
            control.Text = ""
        ElseIf TypeOf control Is CheckBox Then
            control.checked = False
        ElseIf TypeOf control Is DropDownList Then
            control = False
        End If
    End Sub
    Public Shared Sub PopulateCombo_UserAccess(ByVal strValueFieldID As String, ByVal strTextField As String, ByVal strTable As String, ByVal DefaultText As String, ByVal cboCombo As DropDownList)
        Dim strSQL As String
        strSQL = ""
        strSQL = "SELECT Distinct " & strValueFieldID & ", " & strTextField & " FROM " & strTable & " ORDER BY " & strTextField

        Dim constr As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand(strSQL)
                cmd.CommandType = CommandType.Text
                cmd.CommandText = strSQL
                cmd.Connection = con
                con.Open()
                cboCombo.Items.Clear()
                cboCombo.DataSource = cmd.ExecuteReader()
                cboCombo.DataTextField = strTextField
                Dim d As String = cboCombo.SelectedValue
                cboCombo.DataValueField = strValueFieldID
                cboCombo.DataBind()
                con.Close()
            End Using
        End Using
        cboCombo.Items.Insert(0, New ListItem("" & DefaultText & "", "0"))
    End Sub
    Public Shared Function GetKeyID(page As Page, ByRef tblName As String, ByRef DBKeyField As String, ByRef KeyField As String) As String

        GetKeyID = ""

        Dim strConnString As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        Dim strQuery As String = "Select *From " & tblName & " Where " & DBKeyField & " = @KeyField "
        Dim con As New SqlConnection(strConnString)
        Dim cmd As New SqlCommand()
        cmd.Parameters.AddWithValue("@KeyField", KeyField)
        cmd.CommandType = CommandType.Text
        cmd.CommandText = strQuery
        cmd.Connection = con
        Try
            con.Open()
            Dim sdr As SqlDataReader = cmd.ExecuteReader()
            While sdr.Read()
                GetKeyID = sdr(0).ToString()
            End While
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
            con.Dispose()
        End Try

        Return GetKeyID

    End Function
    Public Shared Function GetKeyID_2prms(page As Page, ByRef tblName As String, ByRef DBKeyField As String, ByRef KeyField As String, ByRef DBKeyField2 As String, ByRef KeyField2 As String) As String

        GetKeyID_2prms = ""

        Dim strConnString As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        Dim strQuery As String = "Select *From " & tblName & " Where " & DBKeyField & " = @KeyField And " & DBKeyField2 & " = @KeyField2"
        Dim con As New SqlConnection(strConnString)
        Dim cmd As New SqlCommand()
        cmd.Parameters.AddWithValue("@KeyField", KeyField)
        cmd.Parameters.AddWithValue("@KeyField2", KeyField2)
        cmd.CommandType = CommandType.Text
        cmd.CommandText = strQuery
        cmd.Connection = con
        Try
            con.Open()
            Dim sdr As SqlDataReader = cmd.ExecuteReader()
            While sdr.Read()
                GetKeyID_2prms = sdr(0).ToString()
            End While
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
            con.Dispose()
        End Try

        Return GetKeyID_2prms

    End Function
    Public Shared Function isAlphanumeric(ByVal str As String) As Boolean

        Dim iChar As String
        Dim x As Integer, iAsc As Integer
        Dim isLetter As Boolean, isCharacter As Boolean, isNumber As Boolean

        If IsNumeric(str) Then
            isAlphanumeric = False
        ElseIf Not IsNumeric(str) Then
            For x = 1 To Len(str)
                iChar = Mid$(str, x, 1)
                iAsc = Asc(iChar)
                If (iAsc >= 65 And iAsc <= 90) Or (iAsc >= 97 And iAsc <= 122) Then
                    isLetter = True
                ElseIf (iAsc < 120 And iAsc > 52) Then
                    isNumber = True
                    isCharacter = True
                End If
            Next x
        End If

        If isNumber = True And isLetter = True And isCharacter = True Then
            isAlphanumeric = True
        Else
            isAlphanumeric = False
        End If
    End Function
    Public Shared Sub PopulateGridView(ByVal strGridName As GridView, ByVal strStoredProc As String, ByVal strTextFieldName As String)
        Dim strSQL As String

        strSQL = strStoredProc

        Dim constr As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand(strSQL)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = strSQL
                cmd.Connection = con
                con.Open()
                cmd.Parameters.Add("@Value", SqlDbType.NVarChar).Value = strTextFieldName
                Dim sdr = cmd.ExecuteReader()
                strGridName.DataSource = sdr
                strGridName.DataBind()

                con.Close()
            End Using
        End Using

    End Sub
    Public Shared Sub PopulateGridQuery(ByVal strGridName As GridView, ByVal Query As String)
        Dim strSQL As String

        strSQL = Query
        Dim constr As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand(strSQL)
                cmd.CommandType = CommandType.Text
                cmd.CommandText = strSQL

                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        con.Open()
                        sda.Fill(dt)
                        strGridName.DataSource = dt
                        strGridName.DataBind()
                        con.Close()
                    End Using
                End Using



            End Using
        End Using

    End Sub

    Public Shared Function CheckExistence(ByVal strQuery As String, ByVal strTextFieldName As String, ByVal Exist As String) As String
        Dim strSQL As String
        strSQL = strQuery

        Dim constr As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand(strSQL)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = strSQL
                cmd.Connection = con
                cmd.Parameters.Add("@Value", SqlDbType.NVarChar).Value = strTextFieldName
                Try
                    con.Open()
                    Dim sdr As SqlDataReader = cmd.ExecuteReader()
                    If sdr.Read() Then
                        Exist = sdr(0).ToString()
                    End If
                Catch ex As Exception

                    MsgBox(ex.Message)
                Finally
                    con.Close()
                End Try

            End Using

        End Using

        Return Exist
    End Function

    Public Shared Sub UpdateTableRecord(ByVal strStoredProc As String, ByVal strTextFieldName As String, ByVal ValueID As String)
        Dim strSQL As String

        strSQL = strStoredProc

        Dim constr As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand(strSQL)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = strSQL
                cmd.Connection = con
                cmd.Parameters.Add("@Value", SqlDbType.NVarChar).Value = strTextFieldName
                cmd.Parameters.Add("@ValueID", SqlDbType.NVarChar).Value = ValueID
                con.Open()
                cmd.ExecuteNonQuery()

                con.Close()
            End Using
        End Using

    End Sub

End Class
