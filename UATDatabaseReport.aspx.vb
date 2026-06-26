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
Imports System.Net
Imports ClosedXML.Excel


Public Class UATDatabaseReport
    Inherits System.Web.UI.Page
    Dim strConstring As String = System.Configuration.ConfigurationManager.ConnectionStrings("connectionString").ConnectionString
    Dim Conne As New SqlConnection(strConstring)
    Dim membershipNo As String
    Dim RenewalDate As Date
    Dim ExpiryDate As Date
    Dim AgeDepartment As String
    Dim LatestRef As String
    Dim MemberAge As Integer
    Dim strGender As String
    Dim UATStructure As String
    Dim UATCategory As String
    Dim Disability As String
    Dim Exist As Integer = 0
    Dim ExpiryYearValue As Integer = 1
    Dim UpdatePayment As String = ""
    Dim PrmMemberID As String
    Dim strURL As String = ""
    Dim loggedInUser As String
    Dim IDNumber As String
    Dim MemberID As Integer
    Dim Latest As String
    Dim Query As String

    Dim userTitle, Province, District, DistrictID As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            If Session("Username") Is Nothing Then
                Response.Redirect("login.aspx")
            Else
                Dim Query As String = ""
                userTitle = Session("JobTitle")
                District = Session("DistrictMunicipality")
                Province = Session("Province")
                DistrictID = Session("DistrictID")
                hdnDistrictID.Value = DistrictID
                hdnDistrict.Value = District
                hdnProvince.Value = Province
                hdnUserTitle.Value = userTitle

                lblname.Text = "Welcome " & Session("username")

                If userTitle = "National" Then
                    Query = "Select MembershipNo, expirydate, FirstName, LastName, IDNumber, PreferredLanguage, RegisteredVoter, CellNumber,Province,DistrictMunicipality,LocalMunicipality,WardNo,LocationName, MembershipFee, PaidMembership, CASE WHEN MaintainBy = 'Self Registration' THEN 'Self Registration' ELSE 'Admin Registered' END AS Registration  FROM tblMembership order by Province,DistrictMunicipality,LocalMunicipality, LastName "
                    Me.LoadGridFilter(Query)
                    btnRegister.Visible = True

                ElseIf userTitle = "Provincial" Then
                    Query = "Select MembershipNo, expirydate, FirstName, LastName, IDNumber, PreferredLanguage, RegisteredVoter, CellNumber,Province,DistrictMunicipality,LocalMunicipality,WardNo,LocationName, MembershipFee, PaidMembership, CASE WHEN MaintainBy = 'Self Registration' THEN 'Self Registration' ELSE 'Admin Registered' END AS Registration  FROM tblMembership where Province = '" & Province & "' order by DistrictMunicipality,LocalMunicipality,LastName "
                    Me.LoadGridFilter(Query)
                ElseIf userTitle = "District" Then
                    Query = "Select MembershipNo, expirydate, FirstName, LastName, IDNumber, PreferredLanguage, RegisteredVoter, CellNumber,Province,DistrictMunicipality,LocalMunicipality,WardNo,LocationName, MembershipFee, PaidMembership, CASE WHEN MaintainBy = 'Self Registration' THEN 'Self Registration' ELSE 'Admin Registered' END AS Registration  FROM tblMembership where DistrictMunicipality = '" & District & "' order by LocalMunicipality,LastName "
                    Me.LoadGridFilter(Query)
                End If
            End If
        End If
    End Sub
    Private Sub LoadGridFilter(ByVal Query As String)

        Try

            Using comd As New SqlCommand(Query, Conne)
                comd.CommandType = CommandType.Text
                comd.Connection = Conne
                Conne.Open()
                Using sda As New SqlDataAdapter()
                    comd.Connection = Conne
                    sda.SelectCommand = comd
                    Using dt As New DataTable()
                        sda.Fill(dt)
                        gvMembers.DataSource = dt
                        gvMembers.DataBind()
                        lblTotal.Text = "Total Membership Count: " & dt.Rows.Count.ToString()
                    End Using
                End Using
                Conne.Close()
            End Using
        Catch ex As Exception
            MsgBox(Me, ex.Message)
        End Try

    End Sub

    Private Sub LoadGridView()

        Try
            Dim strQuery As String = "SELECT MembershipNo, ExpiryDate, FirstName, LastName, IDNumber, PreferredLanguage, RegisteredVoter, CellNumber, Province, DistrictMunicipality, LocalMunicipality, WardNo, LocationName, MembershipFee, PaidMembership, CASE WHEN MaintainBy = 'Self Registration' THEN 'Self Registration' ELSE 'Admin Registered' END AS Registration FROM [wywymzbn_dbUserUATTechteam].[tblMembership] ORDER BY MembershipNo Desc;"
            Using cmd As New SqlCommand(strQuery, Conne)
                cmd.CommandType = CommandType.Text
                cmd.Connection = Conne
                Conne.Open()
                Using sda As New SqlDataAdapter()
                    cmd.Connection = Conne
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        sda.Fill(dt)
                        gvMembers.DataSource = dt
                        gvMembers.DataBind()
                        lblTotal.Text = "Total Membership Count: " & dt.Rows.Count.ToString()

                    End Using
                End Using
                Conne.Close()
            End Using
        Catch ex As Exception
            MsgBox(Me, ex.Message)
        End Try

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try

            District = CStr(hdnDistrict.Value)
            Province = CStr(hdnProvince.Value)
            userTitle = CStr(hdnUserTitle.Value)

            If userTitle = "National" Then
                Query = "Select MembershipNo, expirydate, FirstName, LastName, IDNumber, PreferredLanguage, RegisteredVoter, CellNumber,Province,DistrictMunicipality,LocalMunicipality,WardNo,LocationName, MembershipFee, PaidMembership, CASE WHEN MaintainBy = 'Self Registration' THEN 'Self Registration' ELSE 'Admin Registered' END AS Registration  FROM tblMembership order by Province,DistrictMunicipality,LocalMunicipality, LastName "
                Me.LoadGridFilter(Query)
                btnRegister.Visible = True

            ElseIf userTitle = "Provincial" Then
                Query = "Select MembershipNo, expirydate, FirstName, LastName, IDNumber, PreferredLanguage, RegisteredVoter, CellNumber,Province,DistrictMunicipality,LocalMunicipality,WardNo,LocationName, MembershipFee, PaidMembership, CASE WHEN MaintainBy = 'Self Registration' THEN 'Self Registration' ELSE 'Admin Registered' END AS Registration  FROM tblMembership where Province = '" & Province & "' order by DistrictMunicipality,LocalMunicipality,LastName "
                Me.LoadGridFilter(Query)
            ElseIf userTitle = "District" Then
                Query = "Select MembershipNo, expirydate, FirstName, LastName, IDNumber, PreferredLanguage, RegisteredVoter, CellNumber,Province,DistrictMunicipality,LocalMunicipality,WardNo,LocationName, MembershipFee, PaidMembership, CASE WHEN MaintainBy = 'Self Registration' THEN 'Self Registration' ELSE 'Admin Registered' END AS Registration  FROM tblMembership where DistrictMunicipality = '" & District & "' order by LocalMunicipality,LastName "
                Me.LoadGridFilter(Query)
            End If

            ' Ensure GridView has data before exporting
            If gvMembers.Rows.Count = 0 Then
                Response.Write("No data available to export.")
                Return
            End If

            ' Disable paging to export all rows
            gvMembers.AllowPaging = False

            ' Create a new Excel workbook
            Dim wb As New XLWorkbook()
            Dim ws As IXLWorksheet = wb.Worksheets.Add("UAT Members")

            ' Add headers to the Excel file
            For colIndex As Integer = 0 To gvMembers.HeaderRow.Cells.Count - 1
                ws.Cell(1, colIndex + 1).Value = gvMembers.HeaderRow.Cells(colIndex).Text.Trim()
                ws.Cell(1, colIndex + 1).Style.Font.Bold = True
            Next

            ' Add GridView data to the worksheet
            For rowIndex As Integer = 0 To gvMembers.Rows.Count - 1
                Dim row As GridViewRow = gvMembers.Rows(rowIndex)
                For colIndex As Integer = 0 To row.Cells.Count - 1
                    Dim cellValue As String = HttpUtility.HtmlDecode(row.Cells(colIndex).Text.Trim())

                    ' Handle empty or non-printable values
                    If String.IsNullOrWhiteSpace(cellValue) OrElse cellValue = "&nbsp;" Then
                        cellValue = " "
                    End If

                    cellValue = cellValue.Replace(vbCrLf, " ") ' Remove line breaks
                    ws.Cell(rowIndex + 2, colIndex + 1).Value = cellValue
                Next
            Next

            ' Auto-fit columns
            ws.Columns().AdjustToContents()

            ' Prepare response for download
            Response.Clear()
            Response.Buffer = True
            Response.ContentEncoding = System.Text.Encoding.UTF8
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"

            ' Fix filename by formatting date properly
            Dim fileName As String = "UATDatabase_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".xlsx"
            Response.AddHeader("content-disposition", "attachment;filename=" & fileName)

            ' Save workbook to memory stream
            Using memoryStream As New MemoryStream()
                wb.SaveAs(memoryStream)
                memoryStream.WriteTo(Response.OutputStream)
            End Using

            ' Complete response properly
            Response.Flush()
            Response.SuppressContent = True
            HttpContext.Current.ApplicationInstance.CompleteRequest()

        Catch ex As Exception
            Response.Write("Error Exporting Data: " & ex.Message)
        End Try
    End Sub


    'Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    '    ' Confirms that an HTML form control is rendered for the specified ASP.NET server control.
    'End Sub
    'Protected Sub gvMembers_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs)
    '    gvMembers.EditIndex = e.NewEditIndex
    '    Me.LoadGridFilter(Query)
    'End Sub
    'Protected Sub gvMembers_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)
    '    ' Logic to update the record in the database
    '    ' After updating, reset the edit index and rebind the GridView
    '    gvMembers.EditIndex = -1
    '    Me.LoadGridFilter(Query)
    'End Sub

    Protected Sub gvMembers_RowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs)
        ' Logic to delete the record from the database
        Me.LoadGridFilter(Query)
    End Sub

    Protected Sub gvMembers_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvMembers.PageIndex = e.NewPageIndex
        Me.LoadGridFilter(Query)
    End Sub

    'Protected Sub gvMembers_RowCancelingEdit(ByVal sender As Object, ByVal e As GridViewCancelEditEventArgs)

    '    gvMembers.EditIndex = -1
    '    Me.LoadGridFilter(Query)
    'End Sub

    Protected Sub ddlProvince_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlProvince.SelectedIndexChanged
        If drdSearchCriteria.SelectedItem.Text = "District" Or drdSearchCriteria.SelectedItem.Text = "Local" Then
            ddlDistrictMunicipality.Visible = True
            lblSearch.Text = "Select District Name"
            Me.LoadDistrict()
        End If
    End Sub

    Private Sub LoadFilters()

        PopulateCombo("ProvinceId", "Name", "Province", "ProvinceId = ProvinceId", "", ddlProvince)
        ddlProvince.Items.Insert(0, New ListItem("Select Province", ""))
        ddlProvince.SelectedItem.Value = 0

    End Sub

    Private Sub LoadDistrict()
        Dim Province As String = ddlProvince.SelectedValue

        PopulateCombo("DistrictId", "Name", "District", "ProvinceId = " & Province, "Name", ddlDistrictMunicipality)
        ddlDistrictMunicipality.Items.Insert(0, New ListItem("Select District", ""))
        ddlDistrictMunicipality.SelectedItem.Value = 0
    End Sub

    Private Sub LoadLocal()
        PopulateCombo("LocalID", "LocalName", "LocalMunicipality", "DistrictId = " & ddlDistrictMunicipality.SelectedValue, "LocalName", ddlLocalMunicipality)
        ddlLocalMunicipality.Items.Insert(0, New ListItem("Select municipality ", ""))
        ddlLocalMunicipality.SelectedItem.Value = 0
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
                Dim d As String = If(cboCombo.SelectedItem IsNot Nothing, cboCombo.SelectedItem.Text, String.Empty)
                cboCombo.DataValueField = strValueFieldID
                cboCombo.DataBind()
                con.Close()
            End Using
        End Using
    End Sub

    Protected Sub drdSearchCriteria_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drdSearchCriteria.SelectedIndexChanged
        ' Hide all controls by default
        ddlProvince.Visible = False
        ddlDistrictMunicipality.Visible = False
        ddlLocalMunicipality.Visible = False
        txtIDNumber.Visible = False
        txtMembership.Visible = False
        ddlStructure.Visible = False
        ddlgender.Visible = False
        Dim ProvID As Integer
        Dim selectedCriteria As String = drdSearchCriteria.SelectedItem.Text

        ' ---------------------
        ' NATIONAL USERS
        ' ---------------------
        If hdnUserTitle.Value = "National" Then
            Select Case selectedCriteria
                Case "Province"
                    lblSearch.Text = "Select Province"
                    ddlProvince.Visible = True
                    Me.LoadFilters() ' load all provinces
                Case "District"
                    lblSearch.Text = "Select District"
                    ddlProvince.Visible = True
                    ddlDistrictMunicipality.Visible = True
                    Me.LoadFilters()
                Case "Local"
                    lblSearch.Text = "Select Local Municipality"
                    ddlProvince.Visible = True
                    ddlDistrictMunicipality.Visible = True
                    ddlLocalMunicipality.Visible = True
                    Me.LoadFilters()
                Case "ID Number"
                    lblSearch.Text = "Enter ID Number"
                    txtIDNumber.Visible = True
                Case "Membership Number"
                    lblSearch.Text = "Enter Membership Number"
                    txtMembership.Visible = True
                Case "Structure"
                    lblSearch.Text = "Select Structure/League"
                    ddlStructure.Visible = True
                Case "Gender"
                    lblSearch.Text = "Select Gender"
                    ddlgender.Visible = True
            End Select
        End If

        ' ---------------------
        ' PROVINCIAL USERS
        ' ---------------------
        If hdnUserTitle.Value = "Provincial" Then
            Select Case selectedCriteria

                Case "Province"
                    lblSearch.Text = "Province"
                    PopulateCombo("ProvinceId", "Name", "Province", "Name = '" & CStr(hdnProvince.Value) & "'", "", ddlProvince)
                    Dim item As ListItem = ddlProvince.Items.FindByText(CStr(hdnProvince.Value))
                    If item IsNot Nothing Then
                        ddlProvince.ClearSelection()
                        item.Selected = True
                    End If

                    ddlProvince.Enabled = False
                    ddlProvince.Visible = True

                Case "District"
                    PopulateCombo("ProvinceId", "Name", "Province", "Name = '" & CStr(hdnProvince.Value) & "'", "", ddlProvince)
                    Dim item As ListItem = ddlProvince.Items.FindByText(CStr(hdnProvince.Value))
                    If item IsNot Nothing Then
                        ddlProvince.ClearSelection()
                        item.Selected = True
                    End If
                    ddlProvince.Enabled = False
                    ddlProvince.Visible = True
                    ProvID = ddlProvince.SelectedValue
                    lblSearch.Text = "Select District"
                    PopulateCombo("DistrictId", "Name", "District", "ProvinceId = " & ProvID, "Name", ddlDistrictMunicipality)
                    ddlDistrictMunicipality.Visible = True
                Case "Local"
                    lblSearch.Text = "Select Local Municipality"
                    ' optionally restrict locals based on district logic if needed
                    Me.LoadFilters()
                Case "ID Number"
                    lblSearch.Text = "Enter ID Number"
                    txtIDNumber.Visible = True
                Case "Membership Number"
                    lblSearch.Text = "Enter Membership Number"
                    txtMembership.Visible = True
                Case "Structure"
                    lblSearch.Text = "Select Structure/League"
                    ddlStructure.Visible = True
                Case "Gender"
                    lblSearch.Text = "Select Gender"
                    ddlgender.Visible = True
            End Select
        End If

        ' ---------------------
        ' DISTRICT USERS
        ' ---------------------
        If hdnUserTitle.Value = "District" Then
            Select Case selectedCriteria
                Case "Province"
                    lblSearch.Text = "Province"
                    PopulateCombo("ProvinceId", "Name", "Province", "Name = '" & CStr(hdnProvince.Value) & "'", "", ddlProvince)
                    ddlProvince.SelectedItem.Text = CStr(hdnProvince.Value)
                    ddlProvince.Enabled = False
                    ddlProvince.Visible = True
                    ProvID = ddlProvince.SelectedValue
                Case "District"
                    lblSearch.Text = "District"
                    'PopulateCombo("DistrictMunicipalityId", "Name", "DistrictMunicipality", "DistrictMunicipalityId = " & CStr(hdnDistrict.Value), "", ddlDistrictMunicipality)
                    PopulateCombo("DistrictId", "Name", "District", "DistrictId = " & CStr(hdnDistrictID.Value), "Name", ddlDistrictMunicipality)
                    ddlDistrictMunicipality.SelectedItem.Text = CStr(hdnDistrict.Value)
                    ddlDistrictMunicipality.Enabled = False
                    ddlDistrictMunicipality.Visible = True
                Case "Local"
                    lblSearch.Text = "Select Local Municipality"
                    ' optionally restrict locals based on district
                    Me.LoadFilters()
                Case "ID Number"
                    lblSearch.Text = "Enter ID Number"
                    txtIDNumber.Visible = True
                Case "Membership Number"
                    lblSearch.Text = "Enter Membership Number"
                    txtMembership.Visible = True
                Case "Structure"
                    lblSearch.Text = "Select Structure/League"
                    ddlStructure.Visible = True
                Case "Gender"
                    lblSearch.Text = "Select Gender"
                    ddlgender.Visible = True
            End Select
        End If
    End Sub


    Protected Sub ddlDistrictMunicipality_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDistrictMunicipality.SelectedIndexChanged
        If drdSearchCriteria.SelectedItem.Text = "Local" Then
            Me.LoadLocal()
            ddlLocalMunicipality.Visible = True
            lblSearch.Text = "Select Local Municipality"
        End If

    End Sub

    Protected Sub gvMembers_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub hdnProvince_ValueChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        If drdSearchCriteria.SelectedItem.Text = "ID Number" Then
            Query = "SELECT MembershipNo, expirydate, FirstName, LastName, IDNumber, PreferredLanguage, RegisteredVoter, CellNumber,Province,DistrictMunicipality,LocalMunicipality,WardNo,LocationName, MembershipFee, PaidMembership, CASE WHEN MaintainBy = 'Self Registration' THEN 'Self Registration' ELSE 'Admin Registered' END AS Registration  FROM [tblMembership] where IDNumber = '" + txtIDNumber.Text + "' order by LastName "
            Me.LoadGridFilter(Query)
        ElseIf drdSearchCriteria.SelectedItem.Text = "Membership Number" Then
            Query = "SELECT MembershipNo, expirydate, FirstName, LastName, IDNumber, PreferredLanguage, RegisteredVoter, CellNumber,Province,DistrictMunicipality,LocalMunicipality,WardNo,LocationName, MembershipFee, PaidMembership, CASE WHEN MaintainBy = 'Self Registration' THEN 'Self Registration' ELSE 'Admin Registered' END AS Registration  FROM [tblMembership] where MembershipNo = '" + txtMembership.Text + "' order by LastName "
            Me.LoadGridFilter(Query)
        ElseIf drdSearchCriteria.SelectedItem.Text = "Structure" And ddlStructure.SelectedItem.Text <> "pdf" Then
            Query = "SELECT MembershipNo, expirydate, FirstName, LastName, IDNumber, PreferredLanguage, RegisteredVoter, CellNumber,Province,DistrictMunicipality,LocalMunicipality,WardNo,LocationName, MembershipFee, PaidMembership, CASE WHEN MaintainBy = 'Self Registration' THEN 'Self Registration' ELSE 'Admin Registered' END AS Registration  FROM [tblMembership] where UATStructure = '" + ddlStructure.SelectedItem.Text + "' order by LastName "
            Me.LoadGridFilter(Query)
        ElseIf drdSearchCriteria.SelectedItem.Text = "District" Then
            Query = "Select MembershipNo, expirydate, FirstName, LastName, IDNumber, PreferredLanguage, RegisteredVoter, CellNumber,Province,DistrictMunicipality,LocalMunicipality,WardNo,LocationName, MembershipFee, PaidMembership, CASE WHEN MaintainBy = 'Self Registration' THEN 'Self Registration' ELSE 'Admin Registered' END AS Registration  FROM tblMembership where DistrictMunicipality = '" & ddlDistrictMunicipality.SelectedItem.Text & "' order by LocalMunicipality,LastName "
            Me.LoadGridFilter(Query)
        ElseIf drdSearchCriteria.SelectedItem.Text = "Structure" AndAlso ddlStructure.SelectedItem.Text = "pdf" AndAlso userTitle <> "National" Then
            Query = "SELECT MembershipNo, expirydate, FirstName, LastName, IDNumber, PreferredLanguage, RegisteredVoter, CellNumber,Province,DistrictMunicipality,LocalMunicipality,WardNo,LocationName, MembershipFee, PaidMembership, CASE WHEN MaintainBy = 'Self Registration' THEN 'Self Registration' ELSE 'Admin Registered' END AS Registration  FROM [tblMembership] where Disability = 'Yes' and DistrictMunicipality = '" + District + "' order by LocalMunicipality,LastName"
            Me.LoadGridFilter(Query)
        ElseIf drdSearchCriteria.SelectedItem.Text = "Gender" AndAlso userTitle <> "National" Then
            Query = "SELECT MembershipNo, expirydate, FirstName, LastName, IDNumber, PreferredLanguage, RegisteredVoter, CellNumber,Province,DistrictMunicipality,LocalMunicipality,WardNo,LocationName, MembershipFee, PaidMembership, CASE WHEN MaintainBy = 'Self Registration' THEN 'Self Registration' ELSE 'Admin Registered' END AS Registration  FROM [[tblMembership] where Gender = '" + ddlgender.SelectedItem.Text + "' and DistrictMunicipality = '" + District + "' order by LocalMunicipality,LastName"
            Me.LoadGridFilter(Query)


        ElseIf drdSearchCriteria.SelectedItem.Text = "Structure" AndAlso ddlStructure.SelectedItem.Text = "pdf" AndAlso userTitle = "National" Then
            Query = "SELECT MembershipNo, expirydate, FirstName, LastName, IDNumber, PreferredLanguage, RegisteredVoter, CellNumber,Province,DistrictMunicipality,LocalMunicipality,WardNo,LocationName, MembershipFee, PaidMembership, CASE WHEN MaintainBy = 'Self Registration' THEN 'Self Registration' ELSE 'Admin Registered' END AS Registration  FROM [tblMembership] where Disability = 'Yes' order by DistrictMunicipality,LocalMunicipality,LastName"
            Me.LoadGridFilter(Query)
        ElseIf drdSearchCriteria.SelectedItem.Text = "Gender" AndAlso userTitle = "National" Then
            Query = "SELECT MembershipNo, expirydate, FirstName, LastName, IDNumber, PreferredLanguage, RegisteredVoter, CellNumber,Province,DistrictMunicipality,LocalMunicipality,WardNo,LocationName, MembershipFee, PaidMembership, CASE WHEN MaintainBy = 'Self Registration' THEN 'Self Registration' ELSE 'Admin Registered' END AS Registration  FROM [[tblMembership] where Gender = '" + ddlgender.SelectedItem.Text + "' order by DistrictMunicipality,LocalMunicipality,LastName"
            Me.LoadGridFilter(Query)
        End If
        If drdSearchCriteria.SelectedItem.Text = " ActiveMembers " AndAlso userTitle = "National" Then
            Query ="SELECT MemberID, MembershipNo, FirstName, LastName, IDNumber, Age, PreferredLanguage,
                               Gender, SubscriptionType, RegisteredVoter, CountryOfResidence, Email, CellNumber,
                               TelephoneH, TelephoneW, Province, DistrictMunicipality, LocalMunicipality,
                               WardNo, ResidentialAddress, PostalCode, SubscriptionDate, RenewalDate,
                               ExpiryDate, Active, University, MembershipFee,
                               PaidMembership
                               From tblMembership
                               Where Active = 0"

            Me.LoadGridFilter(Query)

        End If
    End Sub

    Protected Sub btnCaptureMembership_Click(sender As Object, e As EventArgs) Handles btnCaptureMembership.Click
        Response.Redirect("UATDatabase.aspx")
    End Sub

    Protected Sub lnkLogout_Click(sender As Object, e As EventArgs) Handles lnkLogout.Click
        Session.Abandon()
        Session.Clear()

        ' Clear cookies (optional)
        If Request.Cookies("Username") IsNot Nothing Then
            Dim usernameCookie As New HttpCookie("Username") With {.Expires = DateTime.Now.AddDays(-1)}
            Response.Cookies.Add(usernameCookie)
        End If
        If Request.Cookies("Password") IsNot Nothing Then
            Dim passwordCookie As New HttpCookie("Password") With {.Expires = DateTime.Now.AddDays(-1)}
            Response.Cookies.Add(passwordCookie)
        End If

        ' Redirect to login
        Response.Redirect("login.aspx")
    End Sub
End Class