Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration

Partial Class InactiveMembers
    Inherits System.Web.UI.Page


    Private ReadOnly connString As String = ConfigurationManager.ConnectionStrings("connectionString").ConnectionString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadMembers()
        End If
    End Sub

    Private Sub LoadMembers()

        Dim dt As New DataTable()

        Using conn As New SqlConnection(connString)
            Dim query As String = "SELECT MemberID, MembershipNo, FirstName, LastName, IDNumber, Age, PreferredLanguage,
                                  Gender, SubscriptionType, RegisteredVoter, CountryOfResidence, Email, CellNumber,
                                  TelephoneH, TelephoneW, Province, DistrictMunicipality, LocalMunicipality,
                                  WardNo, ResidentialAddress, PostalCode, SubscriptionDate, RenewalDate,
                                  ExpiryDate, UATStructure, UATCategory, Active, University, MembershipFee,
                                  Disability, LocationName, PaidMembership, MaintainBy, MaintainDate
                                  FROM Members
                                  WHERE Active = 0"

            Using cmd As New SqlCommand(query, conn)
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
        End Using

        gvMembers.DataSource = dt
        gvMembers.DataBind()

    End Sub

    Protected Sub gvMembers_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvMembers.PageIndexChanging
        gvMembers.PageIndex = e.NewPageIndex
        LoadMembers()
    End Sub

End Class
