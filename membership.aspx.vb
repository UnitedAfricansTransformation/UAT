Imports System
Imports System.Data
Imports System.Configuration
Imports System.Collections
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Web.Caching
Imports System.Exception
Imports System.FormatException
Imports System.Web.UI.WebControls.Literal
Imports System.Web.Configuration
Imports System.IO
Imports System.Text.RegularExpressions

Partial Class membership
    Inherits System.Web.UI.Page
    Private NameValid As Boolean 'Is Name  Valid?
    Private SurnameValid As String 'Is Surname Valid?
    Private PhoneValid As Boolean 'Is Phone Number Valid?
    Private IDValid As Boolean 'Is Email Valid?
    Dim strConstring As String = System.Configuration.ConfigurationManager.ConnectionStrings("connectionString").ConnectionString
    Dim Conne As New SqlConnection(strConstring)
    Dim Exist As Integer = 0
    Dim Latest As String
    Dim one As String = 1

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Me.LoadMembershipNo()
            Me.LoadGridView()
            Me.PopulateCombos()


        End If

    End Sub
    Private Function PopulateCombos() As String


        PopulateCombo("ProvinceId", "Name", "Province", "ProvinceId = ProvinceId", "", ddlProvince)
        ddlProvince.Items.Insert(0, New ListItem("Select Province", ""))
        ddlProvince.SelectedItem.Value = 0

        PopulateCombo("LocalID", "LocalName", "LocalMunicipality", "LocalID = LocalID", "", ddlLocalMunicipality)
        ddlLocalMunicipality.Items.Insert(0, New ListItem("Select municipality ", ""))
        ddlLocalMunicipality.SelectedItem.Value = 0

        PopulateCombo("LocationID", "LocationName", "LOCATION", "LocationID = LocationID", "", ddlLocation)
        ddlLocation.Items.Insert(0, New ListItem("Select Location ", ""))
        ddlLocation.SelectedItem.Value = 0

        PopulateCombo("DistrictId", "Name", "District", "DistrictId = DistrictId", "", ddlDistrictMunicipality)
        ddlDistrictMunicipality.Items.Insert(0, New ListItem("Select District", ""))
        ddlDistrictMunicipality.SelectedItem.Value = 0

        PopulateCombo("UniversityID", "UniversityName", "Universities", "UniversityID = UniversityID", "", ddlUniversity)
        ddlUniversity.Items.Insert(0, New ListItem("Select University/College", ""))
        ddlUniversity.SelectedItem.Value = 0

        PopulateComboID("WardID", "WardNo", "tblWard", "WardID <> 0", "", ddlWardNo)
        ddlWardNo.Items.Insert(0, New ListItem("Select Ward No", ""))
        ddlWardNo.SelectedItem.Value = 0

        Return True
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
    Protected Sub btnRegister_Click(sender As Object, e As EventArgs) Handles btnRegister.Click

        Me.SearchMember()
        If Exist = 0 Then
            Me.AddMember()
        End If
        'End If
    End Sub

    Private Sub ResetFormControls()
        ' Clear text fields
        txtMemberNo.Text = String.Empty
        txtLastName.Text = String.Empty
        txtFirstName.Text = String.Empty
        txtIDNumber.Text = String.Empty
        txtCellNo.Text = String.Empty
        txtResidentialAddress.Text = String.Empty
        txtExpiryDate.Text = String.Empty

        ' Reset dropdowns to their default values or first item
        ddlDistrictMunicipality.SelectedIndex = -1 ' Set to default or first value as needed
        ddlLocalMunicipality.SelectedIndex = -1
        ddlSubscriptionType.SelectedIndex = -1
        ddlMembershipPeriod.SelectedIndex = -1
        ddlWardNumber.SelectedIndex = -1
        ddlGender.SelectedIndex = -1
        ddlProvince.SelectedIndex = -1
        ddlLanguage.SelectedIndex = -1

    End Sub

    Private Sub AddMember()

        Dim fgh() As String
        Dim MemberNo As String
        fgh = Latest.Split("T")
        MemberNo = Format(Convert.ToInt32(fgh(1)) + 1, "0000000")
        Using cmd As SqlCommand = New SqlCommand("SP_RegisterMember", Conne)
            cmd.CommandType = CommandType.StoredProcedure
            Try
                Conne.Open()
                cmd.Parameters.AddWithValue("@MembershipNo", txtMemberNo.Text)
                cmd.Parameters.AddWithValue("@Lastname", txtLastName.Text)
                cmd.Parameters.AddWithValue("@Firstname", txtFirstName.Text)
                cmd.Parameters.AddWithValue("@IDNumber", txtIDNumber.Text)
                cmd.Parameters.AddWithValue("@CellNumber", txtCellNo.Text)
                cmd.Parameters.AddWithValue("@Address", txtResidentialAddress.Text.Trim())
                cmd.Parameters.AddWithValue("@MemberNumber", MemberNo)
                cmd.Parameters.AddWithValue("@CaptureBy", "Admin")
                cmd.Parameters.AddWithValue("@CaptureDate", Date.Now.ToString("yyyy-MM-dd")) ' Use specific date input if needed
                cmd.Parameters.AddWithValue("@District", ddlDistrictMunicipality.SelectedItem)
                cmd.Parameters.AddWithValue("@City_local", ddlLocalMunicipality.SelectedItem)
                cmd.Parameters.AddWithValue("@Branch", "")
                cmd.Parameters.AddWithValue("@PostalCode", "")
                cmd.Parameters.AddWithValue("@SubscriptionType", ddlSubscriptionType.SelectedItem)
                cmd.Parameters.AddWithValue("@MembershipPeriod", ddlMembershipPeriod.SelectedItem)
                cmd.Parameters.AddWithValue("@Country", "South Africa")
                cmd.Parameters.AddWithValue("@WardNo", ddlWardNumber.SelectedItem)
                cmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedItem) ' Assuming ddlGender is a dropdown control
                cmd.Parameters.AddWithValue("@Province", ddlProvince.Text)
                cmd.Parameters.AddWithValue("@Email", "")
                cmd.Parameters.AddWithValue("@ExpiryDate", txtExpiryDate.Text) ' Assuming dtpExpiryDate is a DateTimePicker control
                cmd.Parameters.AddWithValue("@RegisteredDate", Date.Now.ToString("yyyy-MM-dd")) ' Assuming dtpRegisteredDate is a DateTimePicker control
                cmd.Parameters.AddWithValue("@Language", ddlLanguage.SelectedItem)
                cmd.Parameters.AddWithValue("@ModifyBy", "Admin")
                cmd.Parameters.AddWithValue("@ModifiedDate", Date.Now.ToString("yyyy-MM-dd")) ' Use specific date input if needed

                cmd.ExecuteNonQuery()
                common.ShowMsg(Me, "Member Registered Successfully.")
                Me.LoadMembershipNo()
                Me.LoadGridView()
                Me.ResetFormControls()
            Catch ex As Exception
                common.ShowMsg(Me, ex.Message)
            Finally

                Conne.Close()
            End Try

        End Using

    End Sub

    Private Sub LoadGridView()
        Dim fgh() As String
        fgh = Latest.Split("T")
        txtMemberNo.Text = "UAT" & Format(Convert.ToInt32(fgh(1)) + 1, "000000")
        Try
            Using cmd As New SqlCommand("SP_MembershipList", Conne)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Connection = Conne
                Conne.Open()
                Using sda As New SqlDataAdapter()
                    cmd.Connection = Conne
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        sda.Fill(dt)
                        gvMembers.DataSource = dt
                        gvMembers.DataBind()
                    End Using
                End Using

                Conne.Close()

            End Using
        Catch ex As Exception
            common.ShowMsg(Me, ex.Message)
        End Try


    End Sub
    Private Sub SearchMember()
        Try
            Dim Query As String = "Select MembershipNo ,Lastname,Firstname ,IDNumber, CellNumber from tbl_Membership where IDNumber = " & txtIDNumber.Text
            Using cmd As SqlCommand = New SqlCommand(Query, Conne)
                cmd.CommandType = CommandType.Text
                Conne.Open()
                'cmd.Parameters.Add("@IDNumber", SqlDbType.Int).Value = txtIDNumber.Text
                Using reader = cmd.ExecuteReader()
                    If reader.Read() Then

                        txtMemberNo.Text = Convert.ToString(reader.Item(0))
                        txtFirstName.Text = Convert.ToString(reader.Item(2))
                        txtLastName.Text = Convert.ToString(reader.Item(1))
                        txtIDNumber.Text = Convert.ToString(reader.Item(3))
                        txtResidentialAddress.Text = Convert.ToString(reader.Item(5))
                        txtCellNo.Text = Convert.ToString(reader.Item(4))
                        txtExpiryDate.Text = Convert.ToString(reader.Item(6))
                        Exist = 1
                        common.ShowMsg(Me, "Member already registered, See below info")
                    End If
                    reader.Close()
                End Using
                Conne.Close()

            End Using
        Catch ex As Exception
            common.ShowMsg(Me, ex.Message)
        End Try

        If Exist = 2 Then
            Me.LoadMembershipNo()


            'txtCaptureBy.Text = "Admin"
            'txtDate.Text = Date.Now.ToString("yyyy-MM-dd")
            txtFirstName.Text = ""
            txtLastName.Text = ""
            txtIDNumber.Text = ""
            txtExpiryDate.Text = ""
            txtCellNo.Text = ""
            txtIDNumber.Text = ""

            common.ShowMsg(Me, "Member NOT registered")
        End If
        Me.LoadGridView()
    End Sub

    'Protected Sub txtIDNumber0_TextChanged(sender As Object, e As EventArgs) Handles txtIDNumber0.TextChanged
    '    Exist = 2
    '    txtIDNumber.Text = ID.Text
    '    txtFirstName.Text = "Search"
    '    Me.SearchMember()

    'End Sub

    Private Sub LoadMembershipNo()
        Try
            Dim SQL = "select MembershipNo from tbl_Membership where MembershipID = (select max(MembershipID) from tbl_Membership)"
            'Dim SQL1 = "SELECT top 1 MembershipID FROM tbl_Membership order by MembershipID desc"
            Using cmd As SqlCommand = New SqlCommand(SQL, Conne)
                Conne.Open()
                Using reader = cmd.ExecuteReader()
                    While reader.Read()
                        Latest = reader("MembershipNo")
                    End While
                    reader.Close()
                End Using
                Conne.Close()
            End Using
        Catch ex As Exception
            common.ShowMsg(Me, ex.Message)
        End Try

    End Sub

    Protected Sub OnPageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        gvMembers.PageIndex = e.NewPageIndex
        Me.LoadGridView()
    End Sub

    Protected Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        Me.LoadMembershipNo()
        Me.LoadGridView()

        'txtCaptureBy.Text = "Admin"
        txtFirstName.Text = ""
        txtLastName.Text = ""
        txtIDNumber.Text = ""
        txtIDNumber.Text = ""
        txtResidentialAddress.Text = ""
        txtCellNo.Text = ""
        'txtDate.Text = Date.Now.ToString("yyyy-MM-dd")
    End Sub
    Protected Sub gvMembers_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs)
        gvMembers.EditIndex = e.NewEditIndex
        Me.LoadGridView()
    End Sub
    Protected Sub gvMembers_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)
        ' Logic to update the record in the database
        ' After updating, reset the edit index and rebind the GridView
        gvMembers.EditIndex = -1
        Me.LoadGridView()
    End Sub

    Protected Sub gvMembers_RowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs)
        ' Logic to delete the record from the database
        Me.LoadGridView()
    End Sub

    Protected Sub gvMembers_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvMembers.PageIndex = e.NewPageIndex
        Me.LoadGridView()
    End Sub

    Protected Sub gvMembers_RowCancelingEdit(ByVal sender As Object, ByVal e As GridViewCancelEditEventArgs)

        gvMembers.EditIndex = -1
        Me.LoadGridView()
    End Sub

    'Protected Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
    '    Response.Redirect("~/membershipList.aspx")
    'End Sub

    Protected Sub ddlDistrictMunicipality_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDistrictMunicipality.SelectedIndexChanged
        common.PopulateCombo("CityId", "Name", "City", "DistrictId = " & ddlDistrictMunicipality.SelectedValue, "", ddlLocalMunicipality)
        ddlLocalMunicipality.Items.Insert(0, New ListItem("Select municipality ", ""))
        ddlLocalMunicipality.SelectedItem.Value = 0
    End Sub

    Protected Sub ddlProvince_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlProvince.SelectedIndexChanged
        common.PopulateCombo("DistrictId", "Name", "District", "ProvinceId = " & ddlProvince.SelectedValue, "", ddlDistrictMunicipality)
        ddlDistrictMunicipality.Items.Insert(0, New ListItem("Please Select District", ""))
        ddlDistrictMunicipality.SelectedItem.Value = 0
    End Sub

    Protected Sub ddlMembershipPeriod_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlMembershipPeriod.SelectedIndexChanged
        ' Get the selected value from the dropdown list
        Dim selectedValue As Integer = Convert.ToInt32(ddlMembershipPeriod.SelectedValue)

        ' Calculate the expiry date based on the current date and the selected period
        Dim expiryDate As DateTime = DateTime.Now.AddYears(selectedValue)

        ' Display the calculated expiry date in the txtExpiryDate textbox in the format "dd/MM/yyyy"
        txtExpiryDate.Text = expiryDate.ToString("dd/MM/yyyy")
    End Sub
End Class
