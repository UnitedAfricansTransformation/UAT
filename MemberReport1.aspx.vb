Imports System
Imports System.Data
Imports System.Configuration
Imports System.Collections
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.WebControls.Literal

Imports System.Web.UI.HtmlControls
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Web.Caching
Imports System.Exception
Imports System.FormatException

Imports System.Web.Configuration
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Drawing
Imports System.Web.UI.DataVisualization
Imports System.Web.UI.DataVisualization.Charting


Public Class MemberReport1
    Inherits System.Web.UI.Page
    Dim query As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            chartDistrict.ChartAreas("ChartArea1").BackColor = System.Drawing.Color.WhiteSmoke
            chartDistrict.Series("TotalMembers").IsValueShownAsLabel = True
            chartDistrict.Series("TotalMembers").LabelForeColor = System.Drawing.Color.Black
            query = "SELECT DistrictMunicipality, COUNT(*) AS MemberCount FROM tblMembership GROUP BY DistrictMunicipality"
            LoadMembershipData()
            LoadDistrictMembershipChart()
            LoadFilters()

        End If
    End Sub
    Private Sub LoadFilters()

        'ddlProvince, ddlDistrict, ddlLocal, ddlStructure, ddlDisability, ddlSubscriptionType,
        'ddlUniversity, ddlStructure,

        PopulateCombo("ProvinceId", "Name", "Province", "ProvinceId = ProvinceId", "", ddlProvince)
        ddlProvince.Items.Insert(0, New ListItem("Select Province", ""))
        ddlProvince.SelectedItem.Value = 0

        PopulateCombo("CityId", "Name", "City", "CityId = CityId", "", ddlLocal)
        ddlLocal.Items.Insert(0, New ListItem("Select Municipality ", ""))
        ddlLocal.SelectedItem.Value = 0

        PopulateCombo("DistrictId", "Name", "District", "DistrictId = DistrictId", "", ddlDistrict)
        ddlDistrict.Items.Insert(0, New ListItem("Select District", ""))
        ddlDistrict.SelectedItem.Value = 0

        PopulateCombo("UniversityID", "UniversityName", "Universities", "UniversityID = UniversityID", "", ddlUniversity)
        ddlUniversity.Items.Insert(0, New ListItem("Select University", ""))
        ddlUniversity.SelectedItem.Value = 0


        PopulateCombo("MemberID", "UATCategory", "tblMembership", "MemberID = MemberID", "", ddlStructure)
        ddlStructure.Items.Insert(0, New ListItem("Select Structure", ""))
        ddlStructure.SelectedItem.Value = 0

        PopulateCombo("MemberID", "Disability", "tblMembership", "MemberID = MemberID", "", ddlDisability)
        ddlDisability.Items.Insert(0, New ListItem("Select Disability", ""))
        ddlDisability.SelectedItem.Value = 0

        PopulateCombo("MemberID", "MembershipFee", "tblMembership", "MemberID = MemberID", "", ddlSubscriptionType)
        ddlSubscriptionType.Items.Insert(0, New ListItem("Select Subscription", ""))
        ddlSubscriptionType.SelectedItem.Value = 0

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
        'cboCombo.Items.Insert(0, New ListItem("-Please Select-", "0"))

    End Sub
    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFilter.Click
        query = "Select * FROM vwMembershipReport WHERE " &
                               "(@Province Is NULL Or Province = @Province) " &
                               "or (@District Is NULL Or DistrictMunicipality = @District) " &
                               "or (@Local Is NULL Or LocalMunicipality = @Local) " &
                               "or (@Structure Is NULL Or UATStructure = @Structure) " &
                               "or (@Disability Is NULL Or Disability = @Disability) " &
                               "or (@Active Is NULL Or Active = @Active) " &
                               "or (@SubscriptionType Is NULL Or SubscriptionType = @SubscriptionType) " &
                               "or (@University Is NULL Or University = @University)"
        BindGridView()
        LoadDistrictMembershipChart()
        'LoadFilters()
    End Sub
    Private Sub BindGridView()
        Dim constr As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        Using connection As New SqlConnection(constr)
            Using cmd As New SqlCommand(query, connection)
                cmd.CommandType = CommandType.Text
                cmd.Parameters.AddWithValue("@Province", If(String.IsNullOrEmpty(ddlProvince.SelectedItem.Text) OrElse ddlProvince.SelectedItem.Text = "Select Province", DBNull.Value, ddlProvince.SelectedItem.Text))
                cmd.Parameters.AddWithValue("@District", If(String.IsNullOrEmpty(ddlDistrict.SelectedItem.Text) OrElse ddlDistrict.SelectedItem.Text = "Select District", DBNull.Value, ddlDistrict.SelectedItem.Text))
                cmd.Parameters.AddWithValue("@Local", If(String.IsNullOrEmpty(ddlLocal.SelectedItem.Text) OrElse ddlLocal.SelectedItem.Text = "Select Municipality", DBNull.Value, ddlLocal.SelectedItem.Text))
                cmd.Parameters.AddWithValue("@Structure", If(String.IsNullOrEmpty(ddlStructure.SelectedItem.Text) OrElse ddlStructure.SelectedItem.Text = "Select Structure", DBNull.Value, ddlStructure.SelectedItem.Text))
                cmd.Parameters.AddWithValue("@Disability", If(String.IsNullOrEmpty(ddlDisability.SelectedItem.Text) OrElse ddlDisability.SelectedItem.Text = "Select Disability", DBNull.Value, ddlDisability.SelectedItem.Text))
                cmd.Parameters.AddWithValue("@Active", If(chkInactiveMembers.Checked, 0, 1)) ' Assuming checkbox indicates 0 or 1
                cmd.Parameters.AddWithValue("@SubscriptionType", If(String.IsNullOrEmpty(ddlSubscriptionType.SelectedItem.Text) OrElse ddlSubscriptionType.SelectedItem.Text = "Select Subscription", DBNull.Value, ddlSubscriptionType.SelectedItem.Text))
                cmd.Parameters.AddWithValue("@University", If(String.IsNullOrEmpty(ddlUniversity.SelectedItem.Text) OrElse ddlUniversity.SelectedItem.Text = "Select University", DBNull.Value, ddlUniversity.SelectedItem.Text))

                connection.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                gvMembershipReport.DataSource = reader
                gvMembershipReport.DataBind()
                connection.Close()
            End Using
        End Using
    End Sub
    Private Sub LoadMembershipData()
        Dim constr As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        Dim query As String = "SELECT * FROM vwMembershipReport"

        Using connection As New SqlConnection(constr)
            Using cmd As New SqlCommand(query)
                Using command As New SqlCommand(query, connection)
                    Dim dt As New DataTable()
                    Dim adapter As New SqlDataAdapter(command)
                    adapter.Fill(dt)
                    gvMembershipReport.DataSource = dt
                    gvMembershipReport.DataBind()
                End Using
            End Using
        End Using
    End Sub

    Private Sub LoadDistrictMembershipChart()
        Dim constr As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        Using connection As New SqlConnection(constr)
            Using cmd As New SqlCommand(query, connection)

                ' Add the @Province parameter to the command with the selected value
                cmd.Parameters.AddWithValue("@Province", If(String.IsNullOrEmpty(ddlProvince.SelectedValue), DBNull.Value, ddlProvince.SelectedValue))
                cmd.Parameters.AddWithValue("@District", If(String.IsNullOrEmpty(ddlDistrict.SelectedValue), DBNull.Value, ddlDistrict.SelectedValue))
                cmd.Parameters.AddWithValue("@Local", If(String.IsNullOrEmpty(ddlLocal.SelectedValue), DBNull.Value, ddlLocal.SelectedValue))
                cmd.Parameters.AddWithValue("@Structure", If(String.IsNullOrEmpty(ddlStructure.SelectedValue), DBNull.Value, ddlStructure.SelectedValue))
                cmd.Parameters.AddWithValue("@Disability", If(String.IsNullOrEmpty(ddlDisability.SelectedValue), DBNull.Value, ddlDisability.SelectedValue))
                cmd.Parameters.AddWithValue("@Active", If(chkInactiveMembers.Checked, 0, 1)) ' Assuming checkbox indicates 0 or 1
                cmd.Parameters.AddWithValue("@SubscriptionType", If(String.IsNullOrEmpty(ddlSubscriptionType.SelectedValue), DBNull.Value, ddlSubscriptionType.SelectedValue))
                cmd.Parameters.AddWithValue("@University", If(String.IsNullOrEmpty(ddlUniversity.SelectedValue), DBNull.Value, ddlUniversity.SelectedValue))

                connection.Open()

                ' Execute the query and get the result
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                Dim dt As New DataTable()
                dt.Load(reader)

                ' Clear the existing chart data
                chartDistrict.Series("TotalMembers").Points.Clear()
                chartDistrict.Series("TotalMembers").IsValueShownAsLabel = True

                ' Add data to the chart
                For Each row As DataRow In dt.Rows
                    Dim district As String = row("DistrictMunicipality").ToString()
                    Dim count As Integer = Convert.ToInt32(row("MemberCount"))
                    chartDistrict.Series("TotalMembers").Points.AddXY(district, count)
                Next

            End Using
        End Using

    End Sub
    Partial Public Class MemberReport1
        Protected WithEvents Chart1 As Global.System.Web.UI.DataVisualization.Charting.Chart
    End Class

    Protected Sub ddlProvince_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlProvince.SelectedIndexChanged
        ClientScript.RegisterStartupScript(Me.GetType(), "setFocus", "document.getElementById('" & ddlProvince.ClientID & "').focus();", True)
        PopulateCombo("DistrictId", "Name", "District", "ProvinceId = " & ddlProvince.SelectedValue, "", ddlDistrict)
        ddlDistrict.Items.Insert(0, New ListItem("Select District", ""))
        ddlDistrict.SelectedItem.Value = 0
    End Sub

    Protected Sub ddlDistrict_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDistrict.SelectedIndexChanged
        ClientScript.RegisterStartupScript(Me.GetType(), "setFocus", "document.getElementById('" & ddlDistrict.ClientID & "').focus();", True)
        PopulateCombo("CityId", "Name", "City", "DistrictId = " & ddlDistrict.SelectedValue, "", ddlLocal)
        ddlLocal.Items.Insert(0, New ListItem("Select municipality ", ""))
        ddlLocal.SelectedItem.Value = 0
    End Sub
End Class
