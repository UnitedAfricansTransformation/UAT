Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Imports System.Security.Cryptography
Imports System.IO
Imports System.Text
Public Class CommonFunctions

    Public Shared Sub ShowMsg(page As Page, Message As String, Optional redirectTo As String = Nothing)
        Dim redirect As String = ""
        If Not String.IsNullOrEmpty(redirectTo) Then
            redirect = String.Format("window.location.href='{0}'", redirectTo.Replace("'", "\'"))
        End If
        page.ClientScript.RegisterStartupScript(
          page.GetType(),
          "MessageBox",
          "<script language='javascript'>alert('" + Message + "');" & redirect & "</script>")
    End Sub

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
    Public Shared Sub resetControls(control)
        If TypeOf control Is TextBox Then
            control.Text = ""
        ElseIf TypeOf control Is CheckBox Then
            control.checked = False
        ElseIf TypeOf control Is DropDownList Then
            control = False
        End If
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
End Class
