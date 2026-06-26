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
Imports System.Drawing
Imports System.Net.Mail
Imports System.Net
Imports System.ComponentModel
Imports System.ComponentModel.EditorBrowsableAttribute

Public Class UATDatabase
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
    Dim Exist As Integer
    Dim ExpiryYearValue As Integer = 1
    Dim UpdatePayment As String = ""
    Dim PrmMemberID As String
    Dim strURL As String = ""
    Dim loggedInUser As String
    Dim IDNumber As String
    Dim MemberID As Integer
    Dim Latest As String
    Dim userTitle As String
    Dim activeMember As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session.Timeout = 30
        If Not IsPostBack Then

            If Session("Username") Is Nothing Then
                Response.Redirect("login.aspx")
            Else
                Me.PopulateCombos()
                ExpiryDate = DateTime.Now.AddYears(ExpiryYearValue)
                txtExpiryDate.Text = ExpiryDate
                userTitle = Session("JobTitle")
                lblname.Text = "Welcome " & Session("Username")
            End If

        End If
        ' btnUpdateMembership.Visible = False
    End Sub
    Private Sub LoadMembershipNo()
        Try
            Dim Query As String = "select MembershipNo from tblMembership where MemberID = (select max(MemberID) from tblMembership)"
            Dim constr As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            Using conne As New SqlConnection(constr)
                Using cmd As SqlCommand = New SqlCommand(Query, conne)
                    cmd.CommandType = CommandType.Text

                    conne.Open()
                    Using reader = cmd.ExecuteReader()
                        While reader.Read()
                            LatestRef = reader("MembershipNo")
                        End While
                        reader.Close()
                    End Using
                    Dim prefix As String = "UAT2024"
                    Dim numberPart As String
                    If LatestRef.StartsWith(prefix) Then
                        numberPart = LatestRef.Substring(prefix.Length)
                        If numberPart.Length = 6 AndAlso IsNumeric(numberPart) Then
                            Dim number As Integer = Convert.ToInt32(numberPart)
                            number += 1
                            Dim formattedNumber As String = Format(number, "000000")
                            membershipNo = prefix & formattedNumber
                            hdnMembershipNo.Value = membershipNo
                            txtMemberNo.Text = membershipNo
                        End If
                    End If
                    conne.Close()
                End Using
            End Using

        Catch ex As Exception
            Console.WriteLine("Error: " & ex.Message)
        End Try

    End Sub
    Public Function PopulateCombos() As String

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
    Public Shared Sub PopulateComboID(ByVal strValueFieldID As String, ByVal strTextField As String, ByVal strTable As String, ByVal strAppendWhere As String, ByVal DefaultText As String, ByVal cboCombo As DropDownList)

        Dim strSQL As String
        strSQL = ""
        strSQL = "SELECT Distinct " & strValueFieldID & ", " & strTextField & " FROM " & strTable & " WHERE " & strAppendWhere & " ORDER BY " & strValueFieldID

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
    Private Sub SearchMember()
        Try
            Dim Expiry As String = ""
            Dim Query As String = "SELECT IDNumber FROM [wywymzbn_dbUserUATTechteam].[tblMembership] where [MemberID] = " & PrmMemberID
            Using cmd As SqlCommand = New SqlCommand(Query, Conne)
                cmd.CommandType = CommandType.Text
                Conne.Open()

                Using reader = cmd.ExecuteReader()
                    If reader.Read() Then
                        Exist = 1
                        IDNumber = Convert.ToString(reader.Item(0))
                        txtIDNumber.Text = IDNumber

                    End If
                    reader.Close()
                End Using
                Conne.Close()

            End Using

        Catch ex As Exception

        End Try
    End Sub
    Protected Sub ddlProvince_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlProvince.SelectedIndexChanged
        PopulateCombo("DistrictId", "Name", "District", "ProvinceId = " & ddlProvince.SelectedValue, "", ddlDistrictMunicipality)
        ddlDistrictMunicipality.Items.Insert(0, New ListItem("Select District", ""))
        ddlDistrictMunicipality.SelectedItem.Value = 0
    End Sub

    Protected Sub ddlDistrictMunicipality_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDistrictMunicipality.SelectedIndexChanged
        PopulateCombo("LocalId", "LocalName", "LocalMunicipality", "DistrictId = " & ddlDistrictMunicipality.SelectedValue, "", ddlLocalMunicipality)
        ddlLocalMunicipality.Items.Insert(0, New ListItem("Select Municipality ", ""))
        ddlLocalMunicipality.SelectedItem.Value = 0
    End Sub
    Public Function GetAgeAndUATStructure(idNumber As String)
        ' Extract the date of birth from the ID number
        Dim year As Integer = Convert.ToInt32(idNumber.Substring(0, 2))
        Dim month As Integer = Convert.ToInt32(idNumber.Substring(2, 2))
        Dim day As Integer = Convert.ToInt32(idNumber.Substring(4, 2))

        ' Determine the full year
        Dim currentYear As Integer = DateTime.Now.Year
        Dim fullYear As Integer = If(year <= (currentYear Mod 100), 2000 + year, 1900 + year)

        ' Calculate age
        Dim birthDate As New DateTime(fullYear, month, day)
        MemberAge = DateTime.Now.Year - birthDate.Year
        If (DateTime.Now < birthDate.AddYears(MemberAge)) Then
            MemberAge -= 1
        End If

        ' Extract the gender from the ID number
        Dim genderIndicator As Integer = Convert.ToInt32(idNumber.Substring(6, 4))
        Dim gender As String = If(genderIndicator >= 5000, "Male", "Female")

        ' Determine the UAT Structure based on age and gender
        If MemberAge < 35 Then
            UATStructure = "Tomorrow's Future"
        ElseIf MemberAge >= 35 Then
            If gender = "Female" Then
                UATStructure = "Mother Nature"
            Else
                UATStructure = "Main Structure"
            End If
        Else
            UATStructure = "Main Structure"
        End If
        hdnUATSTructure.Value = UATStructure

        If MemberAge > 55 Then
            UATCategory = "Pensioner"
        ElseIf ckDisability.Checked = True Then
            UATCategory = "People living with disability"
        ElseIf ckDisability.Checked = False And MemberAge < 55 Then

            UATCategory = "Mother body"
        End If
        hdnUATCategory.Value = UATCategory

        Return True

    End Function
    Public Function GetAgeAndGender(idNumber As String)

        Dim year As Integer = Convert.ToInt32(idNumber.Substring(0, 2))
        Dim month As Integer = Convert.ToInt32(idNumber.Substring(2, 2))
        Dim day As Integer = Convert.ToInt32(idNumber.Substring(4, 2))
        Dim currentYear As Integer = DateTime.Now.Year
        Dim fullYear As Integer = If(year <= (currentYear Mod 100), 2000 + year, 1900 + year)
        Dim birthDate As New DateTime(fullYear, month, day)

        MemberAge = DateTime.Now.Year - birthDate.Year
        If (DateTime.Now < birthDate.AddYears(MemberAge)) Then
            MemberAge -= 1
        End If
        Return True

    End Function

    Protected Sub btnRegister_Click(sender As Object, e As EventArgs) Handles btnRegister.Click
        Try
            If hdnExist.Value <> "" Then
                Exist = CInt(hdnExist.Value)
            Else
                Exist = 0
            End If


            If Exist = 0 Then

                Me.GetAgeAndGender(txtIDNumber.Text)
                Me.LoadMembershipNo()
                Me.InsertMembership()
                Me.SendEmailToMember()
                Me.Reset()
            Else
                Me.GetAgeAndUATStructure(txtIDNumber.Text)
                Me.GetAgeAndGender(txtIDNumber.Text)
                Me.UpdateMembership()
                Me.SendEmailToMember()
                Me.Reset()
            End If
        Catch ex As Exception
            ' Display the actual application error message
            lblMessage.Text = "Report To the Administrator - Error: " & ex.Message
            lblMessage.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub
    Public Function InsertMembership() As String
        Dim Disability As String
        Dim MembershipFee As String
        Me.GetAgeAndUATStructure(txtIDNumber.Text)
        Dim expireDate As DateTime = DateTime.Parse(txtExpiryDate.Text.Trim())
        Dim newDate As DateTime = expireDate.AddDays(-1)
        txtCellNumber.Text = txtCellNumber.Text.Replace(" ", "")
        Dim Paid As String = "No"


        If annualFee.Checked Then
            ExpiryYearValue = 1
            MembershipFee = "R20 Annual Fee"
            Paid = "Yes"
        ElseIf fiveYearFee.Checked Then
            ExpiryYearValue = 5
            MembershipFee = "R100 5 Years Fee"
            Paid = "Yes"
        End If

        If ckDisability.Checked Then
            Disability = "Yes"
        Else
            Disability = "No"
        End If

        Dim expiryDate As DateTime = DateTime.Now.AddYears(ExpiryYearValue)
        Dim renewalDate As DateTime = newDate
        Dim firstName As String = txtFirstName.Text
        Dim lastName As String = txtLastName.Text
        Dim idNumber As String = txtIDNumber.Text
        Dim age As Integer = Convert.ToInt32(txtAge.Text.Trim)
        Dim preferredLanguage As String = ddlLanguage.SelectedItem.Text
        Dim gender As String = ddlGender.SelectedItem.Text
        Dim subscriptionType As String = ddlSubscriptionType.SelectedItem.Text
        Dim registeredVoter As String = "Yes"
        Dim countryOfResidence As String = "South Africa"
        Dim email As String = If(txtEmail.Text = "", Session("Username"), txtEmail.Text)
        Dim cellNumber As String = txtCellNumber.Text

        Dim province As String = ddlProvince.SelectedItem.ToString()
        Dim districtMunicipality As String = ddlDistrictMunicipality.SelectedItem.Text
        Dim localMunicipality As String = ddlLocalMunicipality.SelectedItem.Text
        Dim wardNo As String = If(ddlWardNo.SelectedItem.Text = "Select Ward No", "", ddlWardNo.SelectedItem.Text)
        Dim Location As String = If(ddlLocation.SelectedItem.Text = "Select Location", "", ddlLocation.SelectedItem.Text)
        Dim residentialAddress As String = txtResidentialAddress.Text

        Dim subscriptionDate As String = Date.Now.ToString("dd-MM-yyyy")
        Dim Institution As String
        If ddlUniversity.SelectedItem.ToString() = "Select University/College" Then
            Institution = ""
        End If

        Dim RegisterDate As DateTime = DateTime.ParseExact(subscriptionDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture)

        Using conne As New SqlConnection(strConstring)
            Using cmd As New SqlCommand("sp_InsertMembership")
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Connection = conne

                'Add parameters to the stored procedure
                cmd.Parameters.AddWithValue("@MembershipNo", membershipNo)
                cmd.Parameters.AddWithValue("@FirstName", firstName)
                cmd.Parameters.AddWithValue("@LastName", lastName)
                cmd.Parameters.AddWithValue("@IDNumber", idNumber)
                cmd.Parameters.AddWithValue("@PreferredLanguage", preferredLanguage)
                cmd.Parameters.AddWithValue("@Gender", gender)
                cmd.Parameters.AddWithValue("@SubscriptionType", subscriptionType)
                cmd.Parameters.AddWithValue("@RegisteredVoter", registeredVoter)
                cmd.Parameters.AddWithValue("@CountryOfResidence", countryOfResidence)
                cmd.Parameters.AddWithValue("@Email", email)
                cmd.Parameters.AddWithValue("@CellNumber", cellNumber)
                cmd.Parameters.AddWithValue("@TelephoneH", cellNumber)
                cmd.Parameters.AddWithValue("@TelephoneW", cellNumber)
                cmd.Parameters.AddWithValue("@Province", province)
                cmd.Parameters.AddWithValue("@DistrictMunicipality", districtMunicipality)
                cmd.Parameters.AddWithValue("@LocalMunicipality", localMunicipality)
                cmd.Parameters.AddWithValue("@WardNo", wardNo)
                cmd.Parameters.AddWithValue("@ResidentialAddress", residentialAddress)
                cmd.Parameters.AddWithValue("@PostalCode", "0000")
                cmd.Parameters.AddWithValue("@SubscriptionDate", RegisterDate)
                cmd.Parameters.AddWithValue("@RenewalDate", renewalDate)
                cmd.Parameters.AddWithValue("@ExpiryDate", expiryDate)
                cmd.Parameters.AddWithValue("@UATCategory", hdnUATCategory.Value)
                cmd.Parameters.AddWithValue("@Age", MemberAge)
                cmd.Parameters.AddWithValue("@Structure", hdnUATSTructure.Value)
                cmd.Parameters.AddWithValue("@University", If(String.IsNullOrEmpty(Institution), DBNull.Value, Institution))
                cmd.Parameters.AddWithValue("@MembershipFee", MembershipFee)
                cmd.Parameters.AddWithValue("@Disability", Disability)
                cmd.Parameters.AddWithValue("@Location", Location)
                cmd.Parameters.AddWithValue("@PaidMember", Paid)
                cmd.Parameters.AddWithValue("@MaintainBy", Session("Username"))
                'cmd.Parameters.AddWithValue("@Date", DateTime.Now)
                conne.Open()
                ' Execute the stored procedure and retrieve the MemberID
                MemberID = Convert.ToInt32(cmd.ExecuteScalar())

                lblMessage.Text = ExpiryYearValue & " YEAR UAT Membership Registered! Membership No:" & membershipNo & " Check your email."
                hdnMembershipNo.Value = membershipNo
                conne.Close()
                Me.SendEmailToMember()

            End Using
        End Using


        Return True
    End Function
    Protected Sub txtIDNumber_TextChanged(sender As Object, e As EventArgs) Handles txtIDNumber.TextChanged
        Dim idNumber As String = txtIDNumber.Text.Trim()
        Try
            ' Ensure ID number has at least 10 digits
            If idNumber.Length >= 10 Then
                ' Extract birth date
                Dim yearPart As Integer = CInt(idNumber.Substring(0, 2))
                Dim monthPart As Integer = CInt(idNumber.Substring(2, 2))
                Dim dayPart As Integer = CInt(idNumber.Substring(4, 2))

                ' Handle century (South African IDs assume 1900s or 2000s)
                Dim currentYear As Integer = DateTime.Now.Year Mod 100
                Dim century As Integer = If(yearPart <= currentYear, 2000, 1900)
                Dim birthDate As New DateTime(century + yearPart, monthPart, dayPart)

                ' Calculate age
                Dim today As DateTime = DateTime.Today
                Dim age As Integer = today.Year - birthDate.Year
                If (today.Month < birthDate.Month) OrElse (today.Month = birthDate.Month AndAlso today.Day < birthDate.Day) Then
                    age -= 1
                End If

                ' Display age
                txtAge.Text = age.ToString()

                Dim genderDigit As Integer = CInt(txtIDNumber.Text.Substring(6, 1))
                If genderDigit >= 0 AndAlso genderDigit <= 4 Then
                    ddlGender.SelectedItem.Text = "Female"
                ElseIf genderDigit >= 5 AndAlso genderDigit <= 9 Then
                    ddlGender.SelectedItem.Text = "Male"
                End If

                Me.FindMember()
            End If
        Catch ex As Exception
            ' Display the actual application error message
            lblMessage.Text = "Report to the Administrator - Error: " & ex.Message
            lblMessage.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub
    Private Sub FindMember()
        Dim dbMembershipFee As String
        annualFee.Checked = False
        fiveYearFee.Checked = False
        hdnExist.Value = "0"
        Try
            Dim Query As String = "SELECT MembershipNo, expirydate,Age, FirstName, LastName, IDNumber, PreferredLanguage, Gender, SubscriptionType, RegisteredVoter, CountryOfResidence, Email, CellNumber,MembershipFee,Province,DistrictMunicipality,LocalMunicipality,WardNo,ResidentialAddress,PostalCode,SubscriptionDate,University,MembershipFee,Disability,LocationName FROM [wywymzbn_dbUserUATTechteam].[tblMembership] WHERE [IDNumber] = @IDNumber"
            Using cmd As SqlCommand = New SqlCommand(Query, Conne)
                cmd.CommandType = CommandType.Text
                cmd.Parameters.AddWithValue("@IDNumber", txtIDNumber.Text.Trim())
                Conne.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        hdnExist.Value = "1"
                        ' Assign values to TextBox controls
                        lblMessage.Text = Convert.ToString(reader("MembershipNo")) & ": Membership Number Already Exist"
                        txtMemberNo.Text = Convert.ToString(reader("MembershipNo"))
                        membershipNo = Convert.ToString(reader("MembershipNo"))
                        Session("membershipNo") = Convert.ToString(reader("MembershipNo"))
                        'txtIDNumber.Text = IDNumber
                        txtFirstName.Text = If(IsDBNull(reader("FirstName")), "", Convert.ToString(reader("FirstName")))
                        txtLastName.Text = If(IsDBNull(reader("LastName")), "", Convert.ToString(reader("LastName")))
                        txtExpiryDate.Text = If(IsDBNull(reader("expirydate")), "", Convert.ToString(reader("expirydate")))
                        txtCellNumber.Text = If(IsDBNull(reader("CellNumber")), "", Convert.ToString(reader("CellNumber"))).Trim()
                        txtResidentialAddress.Text = If(IsDBNull(reader("ResidentialAddress")), "", Convert.ToString(reader("ResidentialAddress")))
                        txtAge.Text = If(IsDBNull(reader("Age")), "", Convert.ToString(reader("Age")))


                        ' Assign values to DropDownList controls
                        If Not IsDBNull(reader("PreferredLanguage")) Then
                            ddlLanguage.SelectedItem.Text = Convert.ToString(reader("PreferredLanguage"))
                        End If
                        If Not IsDBNull(reader("Gender")) Then
                            ddlGender.SelectedItem.Text = Convert.ToString(reader("Gender"))
                        End If
                        If Not IsDBNull(reader("SubscriptionType")) Then
                            ddlSubscriptionType.SelectedItem.Text = Convert.ToString(reader("SubscriptionType"))
                        End If

                        If Not IsDBNull(reader("Province")) Then
                            Dim provinceValue As String = Convert.ToString(reader("Province"))
                            If ddlProvince.SelectedItem IsNot Nothing Then
                                ddlProvince.SelectedItem.Text = provinceValue
                            End If
                        End If
                        If Not IsDBNull(reader("DistrictMunicipality")) Then
                            ddlDistrictMunicipality.SelectedItem.Text = Convert.ToString(reader("DistrictMunicipality"))
                        End If
                        If Not IsDBNull(reader("LocalMunicipality")) Then
                            ddlLocalMunicipality.SelectedItem.Text = Convert.ToString(reader("LocalMunicipality"))
                        End If
                        If Not IsDBNull(reader("WardNo")) Then
                            ddlWardNo.SelectedItem.Text = Convert.ToString(reader("WardNo"))
                        End If
                        If Not IsDBNull(reader("University")) Then
                            ddlUniversity.SelectedItem.Text = Convert.ToString(reader("University"))
                        End If
                        If Not IsDBNull(reader("LocationName")) Then
                            ddlLocation.SelectedItem.Text = Convert.ToString(reader("LocationName"))
                        End If
                        If Not IsDBNull(reader("Disability")) Then
                            If reader("Disability") = "Yes" Then
                                ckDisability.Checked = True
                            ElseIf reader("Disability") = "No" Then
                                ckDisability.Checked = False
                            End If
                        ElseIf (IsDBNull(reader("Disability"))) Then
                            ckDisability.Checked = False
                        End If
                        dbMembershipFee = Convert.ToString(reader("MembershipFee")).Trim()

                        If dbMembershipFee = "R20 Annual Fee" Then
                            annualFee.Checked = True
                            fiveYearFee.Checked = False
                            ExpiryYearValue = 1
                            chkPaid.Checked = True

                        ElseIf dbMembershipFee = "R100 5Years Fee" Then
                            fiveYearFee.Checked = True
                            annualFee.Checked = False
                            ExpiryYearValue = 5
                            chkPaid.Checked = True
                        Else
                            annualFee.Checked = True
                            fiveYearFee.Checked = False
                            chkPaid.Checked = True
                        End If
                    End While

                    'txtIDNumber.Text = IDNumber.Trim()

                End Using
                Conne.Close()

            End Using
        Catch ex As Exception
            ' Display the actual application error message
            lblMessage.Text = "Report to the Administrator - Error: " & ex.Message
            lblMessage.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub


    Protected Sub ddlLocalMunicipality_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlLocalMunicipality.SelectedIndexChanged
        PopulateCombo("LocationID", "LocationName", "LOCATION", "LocalID = " & ddlLocalMunicipality.SelectedValue, "", ddlLocation)
        ddlLocation.Items.Insert(0, New ListItem("Select Location ", ""))
        ddlLocation.SelectedItem.Value = 0
    End Sub

    Public Sub Reset()

        Me.PopulateCombos()
        ckDisability.Checked = "false"
        annualFee.Checked = "false"
        txtFirstName.Text = ""
        txtLastName.Text = ""
        txtIDNumber.Text = ""
        txtEmail.Text = ""
        txtCellNumber.Text = ""
        txtResidentialAddress.Text = ""
        ddlLanguage.SelectedIndex = "0"
        ddlGender.SelectedIndex = "0"
        ddlSubscriptionType.SelectedIndex = "0"
        ddlUniversity.SelectedIndex = "0"
        txtMemberNo.Text = ""
        txtExpiryDate.Text = ""
    End Sub

    Public Function SearchMemberID()
        Dim dbMembershipFee As String
        annualFee.Checked = False
        fiveYearFee.Checked = False

        hdnExist.Value = "0"

        Try
            If txtIDNumber.Text <> "" Then

                IDNumber = txtIDNumber.Text.Trim()
            Else
                IDNumber = txtSearchIDNo.Text.Trim()
            End If

            Dim Query As String = "SELECT MembershipNo,Age, expirydate, FirstName, LastName, IDNumber,TelephoneH, PreferredLanguage, Gender, SubscriptionType, RegisteredVoter, CountryOfResidence, Email, CellNumber,MembershipFee,Province,DistrictMunicipality,LocalMunicipality,WardNo,ResidentialAddress,PostalCode,SubscriptionDate,University,MembershipFee,Disability,LocationName,Active FROM [wywymzbn_dbUserUATTechteam].[tblMembership] WHERE [IDNumber] = @IDNumber"
            Using cmd As SqlCommand = New SqlCommand(Query, Conne)
                cmd.CommandType = CommandType.Text
                cmd.Parameters.AddWithValue("@IDNumber", IDNumber.Trim())
                Conne.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()

                        Exist = 1
                        hdnExist.Value = CStr(Exist)
                        ' Assign values to TextBox controls
                        lblMessage.Text = Convert.ToString(reader("MembershipNo")) & " Membership Number"
                        txtMemberNo.Text = Convert.ToString(reader("MembershipNo"))
                        membershipNo = Convert.ToString(reader("MembershipNo"))
                        Session("membershipNo") = Convert.ToString(reader("MembershipNo"))
                        txtIDNumber.Text = txtIDNumber.Text = Convert.ToString(reader("IDNumber")).Trim()
                        txtFirstName.Text = If(IsDBNull(reader("FirstName")), "", Convert.ToString(reader("FirstName")))
                        txtLastName.Text = If(IsDBNull(reader("LastName")), "", Convert.ToString(reader("LastName")))
                        txtEmail.Text = If(IsDBNull(reader("Email")), "", Convert.ToString(reader("Email")))
                        txtCellNumber.Text = If(IsDBNull(reader("TelephoneH")), "", Convert.ToString(reader("TelephoneH"))).Trim()
                        txtResidentialAddress.Text = If(IsDBNull(reader("ResidentialAddress")), "", Convert.ToString(reader("ResidentialAddress")))
                        txtMemberNo.Text = membershipNo
                        txtExpiryDate.Text = If(IsDBNull(reader("expirydate")), "", Convert.ToString(reader("expirydate")))
                        txtAge.Text = If(IsDBNull(reader("Age")), "", Convert.ToString(reader("Age")))
                        activeMember = Convert.ToInt32(reader("Active"))
                        ' Assign values to DropDownList controls
                        If Not IsDBNull(reader("PreferredLanguage")) Then
                            ddlLanguage.SelectedItem.Text = Convert.ToString(reader("PreferredLanguage"))
                        End If
                        If Not IsDBNull(reader("Gender")) Then
                            ddlGender.SelectedItem.Text = Convert.ToString(reader("Gender"))
                        End If
                        If Not IsDBNull(reader("SubscriptionType")) Then
                            ddlSubscriptionType.SelectedItem.Text = Convert.ToString(reader("SubscriptionType"))
                        End If

                        If Not IsDBNull(reader("Province")) Then
                            Dim provinceValue As String = Convert.ToString(reader("Province"))
                            If ddlProvince.SelectedItem IsNot Nothing Then
                                ddlProvince.SelectedItem.Text = provinceValue
                            End If
                        End If
                        If Not IsDBNull(reader("DistrictMunicipality")) Then
                            ddlDistrictMunicipality.SelectedItem.Text = Convert.ToString(reader("DistrictMunicipality"))
                        End If
                        If Not IsDBNull(reader("LocalMunicipality")) Then
                            ddlLocalMunicipality.SelectedItem.Text = Convert.ToString(reader("LocalMunicipality"))
                        End If
                        If Not IsDBNull(reader("WardNo")) Then
                            ddlWardNo.SelectedItem.Text = Convert.ToString(reader("WardNo"))
                        End If
                        If Not IsDBNull(reader("University")) Then
                            ddlUniversity.SelectedItem.Text = Convert.ToString(reader("University"))
                        End If
                        If Not IsDBNull(reader("LocationName")) Then
                            ddlLocation.SelectedItem.Text = Convert.ToString(reader("LocationName"))
                        End If
                        If Not IsDBNull(reader("Disability")) Then
                            If reader("Disability") = "Yes" Then
                                ckDisability.Checked = True
                            ElseIf reader("Disability") = "No" Then
                                ckDisability.Checked = False
                            End If
                        ElseIf (IsDBNull(reader("Disability"))) Then
                            ckDisability.Checked = False
                        End If
                        dbMembershipFee = Convert.ToString(reader("MembershipFee")).Trim()

                        If dbMembershipFee = "R20 Annual Fee" Then
                            annualFee.Checked = True
                            fiveYearFee.Checked = False
                            ExpiryYearValue = 1

                        ElseIf dbMembershipFee = "R100 5Years Fee" Then
                            fiveYearFee.Checked = True
                            annualFee.Checked = False
                            ExpiryYearValue = 5
                        Else
                            annualFee.Checked = True
                            fiveYearFee.Checked = False
                        End If
                    End While

                    txtIDNumber.Text = IDNumber.Trim()


                End Using
                Conne.Close()

            End Using
        Catch ex As Exception
            ' Display the actual application error message
            lblMessage.Text = "Report to the Administrator - Error: " & ex.Message
            lblMessage.ForeColor = System.Drawing.Color.Red
        End Try

        Return True
    End Function

    Protected Sub ddlSubscriptionType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSubscriptionType.SelectedIndexChanged
        If annualFee.Checked Then
            ExpiryYearValue = 1

        ElseIf fiveYearFee.Checked Then
            ExpiryYearValue = 5

        End If

        ExpiryDate = DateTime.Now.AddYears(ExpiryYearValue)
        txtExpiryDate.Text = ExpiryDate
    End Sub



    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        txtIDNumber.Text = ""
        IDNumber = txtSearchIDNo.Text
        Me.SearchMemberID()
        Me.RenewMembership()
    End Sub

    Public Function RenewMembership() As Boolean




        If activeMember = 0 Then

            btnRegister.Visible = False
            btnUpdateMembership.Visible = True
            txtExpiryDate.Text = ExpiryDate.ToString("yyyy-MM-dd")
            chkPaid.Checked = False
            annualFee.Checked = False
            fiveYearFee.Checked = False




        ElseIf activeMember = 1 Then
            btnRegister.Visible = True
            btnUpdateMembership.Visible = False
        End If



        Return True

    End Function

    Public Function UpdateMembership() As Boolean
        Dim Disability As String
        Dim MembershipFee As String
        Dim Paid As String = "No"
        Dim expireDate As DateTime = DateTime.Parse(txtExpiryDate.Text.Trim())
        Dim newDate As DateTime = expireDate.AddDays(-1)
        txtCellNumber.Text = txtCellNumber.Text.Replace(" ", "")

        If annualFee.Checked Then
            ExpiryYearValue = 1
            MembershipFee = "R20 Annual Fee"
            Paid = "Yes"
        ElseIf fiveYearFee.Checked Then
            ExpiryYearValue = 5
            MembershipFee = "R100 5 Years Fee"
            Paid = "Yes"
        End If

        If ckDisability.Checked Then
            Disability = "Yes"
        Else
            Disability = "No"
        End If

        Dim expiryDate As DateTime = txtExpiryDate.Text.Trim()
        Dim renewalDate As DateTime = newDate
        Dim firstName As String = txtFirstName.Text.Trim()
        Dim lastName As String = txtLastName.Text.Trim()
        Dim idNumber As String = txtIDNumber.Text.Trim()
        Dim age As Integer = Convert.ToInt32(txtAge.Text.Trim())
        Dim preferredLanguage As String = ddlLanguage.SelectedItem.Text.Trim()
        Dim gender As String = ddlGender.SelectedItem.Text.Trim()
        Dim subscriptionType As String = ddlSubscriptionType.SelectedItem.Text.Trim()
        Dim registeredVoter As String = "Yes"
        Dim countryOfResidence As String = "South Africa"
        Dim email As String = If(txtEmail.Text = "", Session("Username"), txtEmail.Text)
        Dim cellNumber As String = txtCellNumber.Text

        Dim province As String = ddlProvince.SelectedItem.ToString()
        Dim districtMunicipality As String = ddlDistrictMunicipality.SelectedItem.Text
        Dim localMunicipality As String = ddlLocalMunicipality.SelectedItem.Text
        Dim wardNo As String = ddlWardNo.SelectedItem.Text
        Dim Location As String = ddlLocation.SelectedItem.Text
        Dim residentialAddress As String = txtResidentialAddress.Text

        Dim subscriptionDate As String = Date.Now.ToString("dd-MM-yyyy")
        Dim Institution As String = If(ddlUniversity.SelectedItem.ToString() = "Select University/College", "", ddlUniversity.SelectedItem.ToString())

        Dim RegisterDate As DateTime = DateTime.ParseExact(subscriptionDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture)

        Using cmd As New SqlCommand("UPDATE tblMembership SET 
                FirstName = @FirstName,
                LastName = @LastName,
                PreferredLanguage = @PreferredLanguage,
                Gender = @Gender,
                SubscriptionType = @SubscriptionType,
                RegisteredVoter = @RegisteredVoter,
                CountryOfResidence = @CountryOfResidence,
                Email = @Email,
                CellNumber = @CellNumber,
                Province = @Province,
                DistrictMunicipality = @DistrictMunicipality,
                LocalMunicipality = @LocalMunicipality,
                WardNo = @WardNo,
                ResidentialAddress = @ResidentialAddress,
                PostalCode = @PostalCode,
                SubscriptionDate = @SubscriptionDate,
                RenewalDate = @RenewalDate,
                ExpiryDate = @ExpiryDate,
                UATCategory = @UATCategory,
                Age = @Age,
                UATStructure = @Structure,
                University = @University,
                MembershipFee = @MembershipFee,
                Disability = @Disability,
                LocationName = @Location,
                PaidMembership = @PaidMember,
MaintainBy= @MaintainBy,
MaintainDate =@Date
            WHERE IDNumber = @IDNumber", Conne)

            ' Add parameters
            cmd.Parameters.AddWithValue("@MembershipNo", txtMemberNo.Text)
            cmd.Parameters.AddWithValue("@FirstName", firstName)
            cmd.Parameters.AddWithValue("@LastName", lastName)
            cmd.Parameters.AddWithValue("@IDNumber", idNumber)
            cmd.Parameters.AddWithValue("@PreferredLanguage", preferredLanguage)
            cmd.Parameters.AddWithValue("@Gender", gender)
            cmd.Parameters.AddWithValue("@SubscriptionType", subscriptionType)
            cmd.Parameters.AddWithValue("@RegisteredVoter", registeredVoter)
            cmd.Parameters.AddWithValue("@CountryOfResidence", countryOfResidence)
            cmd.Parameters.AddWithValue("@Email", email)
            cmd.Parameters.AddWithValue("@CellNumber", cellNumber)
            cmd.Parameters.AddWithValue("@Province", province)
            cmd.Parameters.AddWithValue("@DistrictMunicipality", districtMunicipality)
            cmd.Parameters.AddWithValue("@LocalMunicipality", localMunicipality)
            cmd.Parameters.AddWithValue("@WardNo", wardNo)
            cmd.Parameters.AddWithValue("@ResidentialAddress", residentialAddress)
            cmd.Parameters.AddWithValue("@PostalCode", "0000")
            cmd.Parameters.AddWithValue("@SubscriptionDate", RegisterDate)
            cmd.Parameters.AddWithValue("@RenewalDate", renewalDate)
            cmd.Parameters.AddWithValue("@ExpiryDate", expiryDate)
            cmd.Parameters.AddWithValue("@UATCategory", UATCategory)
            cmd.Parameters.AddWithValue("@Age", age)
            cmd.Parameters.AddWithValue("@Structure", UATStructure)
            cmd.Parameters.AddWithValue("@University", If(String.IsNullOrEmpty(Institution), DBNull.Value, Institution))
            cmd.Parameters.AddWithValue("@MembershipFee", MembershipFee)
            cmd.Parameters.AddWithValue("@Disability", Disability)
            cmd.Parameters.AddWithValue("@Location", Location)
            cmd.Parameters.AddWithValue("@PaidMember", Paid)
            cmd.Parameters.AddWithValue("@Date", DateTime.Now)
            cmd.Parameters.AddWithValue("@MaintainBy", Session("Username"))

            Conne.Open()
            Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
            Conne.Close()

            If rowsAffected > 0 Then
                lblMessage.Text = "Membership record updated successfully for ID: " & idNumber & " with Membership No: " & txtMemberNo.Text
                Return True
            Else
                lblMessage.Text = "No membership record found for ID: " & idNumber
                Return False
            End If
        End Using

    End Function

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

    Private Sub SendEmailToMember()

        Try
            ' Retrieve the email address from txtEmail
            Dim expiryDate As DateTime = txtExpiryDate.Text.Trim()
            Dim emailAddress As String = If(txtEmail.Text = "", Session("Username"), txtEmail.Text)

            ' Ensure the email address is not empty
            If String.IsNullOrEmpty(emailAddress) Then
                Throw New Exception("Email address cannot be empty.")
            End If
            Try
                ' Set up the email components
                Dim mail As New MailMessage()
                mail.From = New MailAddress("itdepartment@uat2023.org.za") ' Replace with your email
                mail.To.Add(emailAddress)
                mail.Subject = "Success! UAT Membership Registration : " & hdnMembershipNo.Value
                mail.IsBodyHtml = True

                ' Load the image and attach it to the email
                Dim imagePath As String = Server.MapPath("~/Images/Logo.jpeg")
                Dim linkedImage As New LinkedResource(imagePath)
                linkedImage.ContentId = "UATLogo"  ' Content ID for referencing in HTML

                ' Create an AlternateView for the HTML body
                Dim htmlView As AlternateView = AlternateView.CreateAlternateViewFromString(
    "Khanimambo Doer,<br/><br/>" &
    "You have successfully registered your UAT Membership.<br/><br/>" &
    "Your Membership Number Is: <strong>" & hdnMembershipNo.Value & "</strong><br/><br/>" &
    "We are excited to have you as a member of the United Africans Transformation (UAT) Party. We look forward to your active participation in our mission to drive positive change across Africa.<br/><br/>" &
    "Stay tuned for upcoming events and updates.<br/><br/>" &
    "<div style='border: 2px solid #d1d1d1; padding: 20px; max-width: 600px; border-radius: 10px; font-family: Arial, sans-serif; background-color: #ffffff;'>" &
    "   <h3 style='color: #ffffff; text-align: center; background-color: #2c3e50; padding: 10px; border-radius: 5px;'>UAT Membership Card</h3>" &
    "   <table style='width: 100%; border-collapse: collapse;'>" &
    "       <tr>" &
    "           <td style='width: 70%; vertical-align: top; padding-right: 10px;'>" &
    "               <h4 style='color: #2c3e50; border-bottom: 2px solid #d1d1d1; padding-bottom: 5px;'>Personal Information</h4>" &
    "               <p style='color: #333; font-size: 12px;'><strong>UAT Membership Number:</strong> <span style='color: #1e90ff;'>" & hdnMembershipNo.Value & "</span></p>" &
    "               <p style='color: #333; font-size: 11px;'><strong>Member Name:</strong> <span style='color: #ff0000;'>" & txtFirstName.Text & " " & txtLastName.Text & "</span></p>" &
    "               <p style='color: #333; font-size: 11px;'><strong>ID Number:</strong> <span style='color: #888888;'>" & txtIDNumber.Text & "</span></p>" &
    "               <p style='color: #333; font-size: 11px;'><strong>Expiry Date:</strong> <span style='color: #888888;'>" & expiryDate.ToString("dd MMM yyyy") & "</span></p>" &
    "               <p style='color: #333; font-size: 11px;'><strong>CellPhone Number:</strong> <span style='color: #888888;'>" & txtCellNumber.Text.Trim() & "</span></p>" &
    "           </td>" &
    "           <td style='width: 30%; text-align: right; vertical-align: top;'>" &
    "               <img src='cid:UATLogo' alt='UAT Logo' style='width: auto; height: 100px;'>" &
    "           </td>" &
    "       </tr>" &
    "       <tr>" &
    "           <td colspan='2'>" &
    "               <hr style='border: none; border-top: 1px solid #d1d1d1; margin: 20px 0;'>" &
    "               <h4 style='color: #2c3e50; border-bottom: 2px solid #d1d1d1; padding-bottom: 5px;'>Location Details</h4>" &
    "               <div style='display: flex; justify-content: space-between;'>" &
    "                   <div style='width: 48%;'>" &
    "                       <p style='color: #333; font-size: 11px;'><strong>Province:</strong> <span style='color: #1e90ff;'>" & ddlProvince.SelectedItem.ToString() & "</span></p>" &
    "                       <p style='color: #333; font-size: 11px;'><strong>District Municipality:</strong> <span style='color: #1e90ff;'>" & ddlDistrictMunicipality.SelectedItem.Text & "</span></p>" &
    "                   </div>" &
    "                   <div style='width: 48%;'>" &
    "                       <p style='color: #333; font-size: 11px;'><strong>Local Municipality:</strong> <span style='color: #1e90ff;'>" & ddlLocalMunicipality.SelectedItem.Text & "</span></p>" &
    "                       <p style='color: #333; font-size: 11px;'><strong>Ward Number:</strong> <span style='color: #1e90ff;'>" & ddlWardNo.SelectedItem.Text & "</span></p>" &
    "                   </div>" &
    "               </div>" &
    "           </td>" &
    "       </tr>" &
    "   </table>" &
    "</div><br/><br/>" &
    "Warm Regards,<br/>" &
    "UAT Administration Team", Nothing, "text/html")


                ' Add the image as a linked resource
                htmlView.LinkedResources.Add(linkedImage)

                ' Attach the HTML view to the mail
                mail.AlternateViews.Add(htmlView)


                'UAT  SMTP client Info
                Dim smtpClient As New SmtpClient("mail.uat2023.org.za")
                smtpClient.Port = 25
                smtpClient.Credentials = New NetworkCredential("itdepartment@uat2023.org.za", "it@uat2023")
                smtpClient.EnableSsl = True
                smtpClient.Timeout = 60000

                smtpClient.Send(mail)
            Catch ex As SmtpException
                lblMessage.Text = "SMTP Error: " & ex.Message
                If ex.InnerException IsNot Nothing Then
                    lblMessage.Text = "Inner Exception: " & ex.InnerException.Message
                End If
            Catch ex As Exception
                lblMessage.Text = "General Error: " & ex.Message
            End Try

        Catch ex As Exception
            ' Handle any errors that occur
            lblMessage.Text = "Error: " & ex.Message
        End Try
    End Sub

    Protected Sub btnMemberReport_Click(sender As Object, e As EventArgs) Handles btnMemberReport.Click
        Response.Redirect("~/UATDatabaseReport.aspx", False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub

    Protected Sub btnUpdateMembership_Click(sender As Object, e As EventArgs) Handles btnUpdateMembership.Click

    End Sub
End Class