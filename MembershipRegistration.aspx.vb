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

Partial Class MembershipRegistration
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
    Dim ExpiryYearValue As Integer
    Dim UpdatePayment As String = ""
    Dim PrmMemberID As String
    Dim strURL As String = ""
    Dim loggedInUser As String
    Dim IDNumber As String
    Dim MemberID As Integer


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Optional: Any code to run when the page loads for the first time
        PrmMemberID = Request.QueryString("prmMemberID")
        UpdatePayment = Request.QueryString("prmStatus")
        'PrmMemberID = "25032"
        'UpdatePayment = "1"

        If Not IsPostBack Then
            If UpdatePayment = 1 Then
                Me.SearchMember()
                Me.PopulateCombos()
                Me.FindMember()
                PaymentAdmin.Visible = True
                submitButton.Text = "Submit Payment"
            ElseIf UpdatePayment = "" Then
                Me.PopulateCombos()
                PaymentAdmin.Visible = False
            End If

            ' Handle PayFast Notification

        End If
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

                        IDNumber = Convert.ToString(reader.Item(0))
                        txtIDNumber.Text = IDNumber.Trim()

                    End If
                    reader.Close()
                End Using
                Conne.Close()

            End Using

        Catch ex As Exception

        End Try
    End Sub
    Private Sub FindMember()

        Dim Query As String = "SELECT MembershipNo, expirydate, FirstName, LastName, IDNumber, PreferredLanguage, Gender, SubscriptionType, RegisteredVoter, CountryOfResidence, Email, CellNumber,MembershipFee,Province,DistrictMunicipality,LocalMunicipality,WardNo,ResidentialAddress,PostalCode,SubscriptionDate,University,MembershipFee,Disability,LocationName FROM [wywymzbn_dbUserUATTechteam].[tblMembership] WHERE [IDNumber] = @IDNumber"
        Using cmd As SqlCommand = New SqlCommand(Query, Conne)
                cmd.CommandType = CommandType.Text
            cmd.Parameters.AddWithValue("@IDNumber", IDNumber.Trim())
            Conne.Open()
            Using reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    ' Assign values to TextBox controls
                    lblMessage.Text = Convert.ToString(reader("MembershipNo")) & " Membership Number Exist"
                    membershipNo = Convert.ToString(reader("MembershipNo"))
                    Session("membershipNo") = Convert.ToString(reader("MembershipNo"))
                    txtIDNumber.Text = txtIDNumber.Text = Convert.ToString(reader("IDNumber")).Trim()
                    txtFirstName.Text = If(IsDBNull(reader("FirstName")), "", Convert.ToString(reader("FirstName")))
                    txtLastName.Text = If(IsDBNull(reader("LastName")), "", Convert.ToString(reader("LastName")))
                    txtEmail.Text = If(IsDBNull(reader("Email")), "", Convert.ToString(reader("Email")))
                    txtCellNumber.Text = If(IsDBNull(reader("CellNumber")), "", Convert.ToString(reader("CellNumber"))).Trim()
                    txtResidentialAddress.Text = If(IsDBNull(reader("ResidentialAddress")), "", Convert.ToString(reader("ResidentialAddress")))
                    txtPostalCode.Text = Convert.ToString(reader("PostalCode"))

                    ' Assign values to DropDownList controls
                    If Not IsDBNull(reader("PreferredLanguage")) Then
                        ddllanguage.SelectedItem.Text = Convert.ToString(reader("PreferredLanguage"))
                    End If
                    If Not IsDBNull(reader("Gender")) Then
                        ddlgender.SelectedItem.Text = Convert.ToString(reader("Gender"))
                    End If
                    If Not IsDBNull(reader("SubscriptionType")) Then
                        ddlSubscriptionType.SelectedItem.Text = Convert.ToString(reader("SubscriptionType"))
                    End If
                    If Not IsDBNull(reader("RegisteredVoter")) Then
                        ddlregisteredVoter.SelectedItem.Text = Convert.ToString(reader("RegisteredVoter"))
                    End If
                    If Not IsDBNull(reader("CountryOfResidence")) Then
                        ddlCountry.SelectedItem.Text = Convert.ToString(reader("CountryOfResidence"))
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
                    If reader("MembershipFee") = "R20 Annual Fee" Then
                        annualFee.Checked = True
                        fiveYearFee.Checked = False
                    ElseIf reader("MembershipFee") = "R100 5 Years Fee" Then
                        annualFee.Checked = False
                        fiveYearFee.Checked = True
                    ElseIf (IsDBNull(reader("MembershipFee"))) Then
                        annualFee.Checked = False
                        fiveYearFee.Checked = False
                    End If
                End While
                DeclareName.Text = txtLastName.Text
                txtIDNumber.Text = IDNumber.Trim()
                txtPostalCode.Text = "0000"
            End Using
            Conne.Close()

        End Using

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
    Public Function InsertMembership() As String
        Dim Disability As String
        Dim MembershipFee As String
        Dim Paid As String = "No"


        If annualFee.Checked Then
            ExpiryYearValue = 1
            MembershipFee = "R20 Annual Fee"
        Else
            ExpiryYearValue = 5
            MembershipFee = "R100 5Years Fee"
        End If

        If ckDisability.Checked Then
            Disability = "Yes"
        Else
            Disability = "No"
        End If
        Dim expiryDate As DateTime = DateTime.Now.AddYears(ExpiryYearValue)
        Dim renewalDate As DateTime = DateTime.Now.AddYears(ExpiryYearValue)
        Dim firstName As String = txtFirstName.Text
        Dim lastName As String = txtLastName.Text
        Dim idNumber As String = txtIDNumber.Text
        Dim age As Integer? = If(String.IsNullOrEmpty(txtAge.Text), CType(Nothing, Integer?), Convert.ToInt32(txtAge.Text))
        Dim preferredLanguage As String = ddllanguage.SelectedItem.Text
        Dim gender As String = ddlgender.SelectedItem.Text
        Dim subscriptionType As String = ddlSubscriptionType.SelectedItem.Text
        Dim registeredVoter As String = ddlregisteredVoter.SelectedItem.Text
        Dim countryOfResidence As String = ddlCountry.SelectedItem.Text
        Dim email As String = txtEmail.Text
        Dim cellNumber As String = txtCellNumber.Text
        Dim telephoneH As String = If(String.IsNullOrEmpty(txtTelephoneH.Text), Nothing, txtTelephoneH.Text)
        Dim telephoneW As String = If(String.IsNullOrEmpty(txtTelephoneW.Text), Nothing, txtTelephoneW.Text)
        Dim province As String = ddlProvince.SelectedItem.ToString()
        Dim districtMunicipality As String = ddlDistrictMunicipality.SelectedItem.Text
        Dim localMunicipality As String = ddlLocalMunicipality.SelectedItem.Text
        Dim wardNo As String = ddlWardNo.SelectedItem.Text
        Dim Location As String = ddlLocation.SelectedItem.Text
        Dim residentialAddress As String = txtResidentialAddress.Text
        Dim postalCode As String = txtPostalCode.Text
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
                cmd.Parameters.AddWithValue("@TelephoneH", If(String.IsNullOrEmpty(telephoneH), DBNull.Value, telephoneH))
                cmd.Parameters.AddWithValue("@TelephoneW", If(String.IsNullOrEmpty(telephoneW), DBNull.Value, telephoneW))
                cmd.Parameters.AddWithValue("@Province", province)
                cmd.Parameters.AddWithValue("@DistrictMunicipality", districtMunicipality)
                cmd.Parameters.AddWithValue("@LocalMunicipality", localMunicipality)
                cmd.Parameters.AddWithValue("@WardNo", wardNo)
                cmd.Parameters.AddWithValue("@ResidentialAddress", residentialAddress)
                cmd.Parameters.AddWithValue("@PostalCode", postalCode)
                cmd.Parameters.AddWithValue("@SubscriptionDate", RegisterDate)
                cmd.Parameters.AddWithValue("@RenewalDate", renewalDate)
                cmd.Parameters.AddWithValue("@ExpiryDate", expiryDate)
                cmd.Parameters.AddWithValue("@UATCategory", UATCategory)
                cmd.Parameters.AddWithValue("@Age", MemberAge)
                cmd.Parameters.AddWithValue("@Structure", UATStructure)
                cmd.Parameters.AddWithValue("@University", If(String.IsNullOrEmpty(Institution), DBNull.Value, Institution))
                cmd.Parameters.AddWithValue("@MembershipFee", MembershipFee)
                cmd.Parameters.AddWithValue("@Disability", Disability)
                cmd.Parameters.AddWithValue("@Location", Location)
                cmd.Parameters.AddWithValue("@PaidMember", Paid)
                cmd.Parameters.AddWithValue("@MaintainBy", "Self Registration")
                conne.Open()
                ' Execute the stored procedure and retrieve the MemberID
                memberID = Convert.ToInt32(cmd.ExecuteScalar())
                hdnMemberID.Value = Convert.ToString(MemberID)
                lblMessage.Text = ExpiryYearValue & " YEAR UAT Membership Registered! Membership No:" & membershipNo & " Check your email."
                conne.Close()


            End Using
        End Using


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
    Public Sub resetControls(control)
        If TypeOf control Is TextBox Then
            control.Text = ""
        ElseIf TypeOf control Is CheckBox Then
            control.checked = False
        ElseIf TypeOf control Is DropDownList Then
            control = False
        End If
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
    Private Sub ClearFields()
        ' Clear TextBoxes
        txtFirstName.Text = String.Empty
        txtLastName.Text = String.Empty
        txtIDNumber.Text = String.Empty
        txtAge.Text = String.Empty
        txtEmail.Text = String.Empty
        txtCellNumber.Text = String.Empty
        txtTelephoneH.Text = String.Empty
        txtTelephoneW.Text = String.Empty
        txtResidentialAddress.Text = String.Empty
        txtPostalCode.Text = String.Empty
        hdnMemberID.Value = String.Empty
        hdnMemberNo.Value = String.Empty
        ' Reset DropDownLists to first/default item
        If ddllanguage.Items.Count > 0 Then ddllanguage.SelectedIndex = 0
        If ddlgender.Items.Count > 0 Then ddlgender.SelectedIndex = 0
        If ddlSubscriptionType.Items.Count > 0 Then ddlSubscriptionType.SelectedIndex = 0
        If ddlregisteredVoter.Items.Count > 0 Then ddlregisteredVoter.SelectedIndex = 0
        If ddlCountry.Items.Count > 0 Then ddlCountry.SelectedIndex = 0
        If ddlProvince.Items.Count > 0 Then ddlProvince.SelectedIndex = 0
        If ddlDistrictMunicipality.Items.Count > 0 Then ddlDistrictMunicipality.SelectedIndex = 0
        If ddlLocalMunicipality.Items.Count > 0 Then ddlLocalMunicipality.SelectedIndex = 0
        If ddlWardNo.Items.Count > 0 Then ddlWardNo.SelectedIndex = 0
        If ddlLocation.Items.Count > 0 Then ddlLocation.SelectedIndex = 0
        If ddlUniversity.Items.Count > 0 Then ddlUniversity.SelectedIndex = 0

        ' Reset CheckBoxes
        annualFee.Checked = False
        ckDisability.Checked = False

        ' Clear or reset related variables
        ExpiryYearValue = 0

        Disability = String.Empty

        ' Optionally reset date fields
        Dim expiryDate As DateTime = DateTime.Now
        Dim renewalDate As DateTime = DateTime.Now
        Dim subscriptionDate As String = String.Empty
    End Sub

    Protected Sub submitButton_Click(sender As Object, e As EventArgs) Handles submitButton.Click
        Dim Msg As String

        Try
            If PaymentAdmin.Visible = True Then

                Dim query As String = "UPDATE tblMembership SET PaidMembership = 'Yes', MaintainBy = @MaintainBy, MaintainDate = @MaintainDate OUTPUT Inserted.MembershipNo WHERE MemberID = @MemberID"
                Dim constr As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

                Using conne As New SqlConnection(constr)
                    Using cmd As SqlCommand = New SqlCommand(query, conne)
                        cmd.CommandType = CommandType.Text
                        cmd.Parameters.AddWithValue("@MaintainBy", loggedInUser)
                        cmd.Parameters.AddWithValue("@MaintainDate", DateTime.Now)
                        cmd.Parameters.AddWithValue("@MemberID", PrmMemberID)

                        conne.Open()
                        Dim result As Object = cmd.ExecuteScalar()
                        If result IsNot Nothing Then
                            membershipNo = result.ToString()
                        End If
                        conne.Close()
                        Me.SendEmailToMember()
                        Msg = "Congratulations Doer! Your membership is successfully registered. Your Membership Number is: " + membershipNo + " Remember to renew your account before " + ExpiryDate
                        Me.SendSMS()
                        ClearFields()
                    End Using

                End Using

            ElseIf PaymentAdmin.Visible = False Then

                Me.SearchMember()
                If Exist = 0 Then
                    Me.LoadMembershipNo()
                    Dim prefix As String = "UAT2024"
                    Dim numberPart As String
                    If LatestRef.StartsWith(prefix) Then
                        numberPart = LatestRef.Substring(prefix.Length)
                        If numberPart.Length = 6 AndAlso IsNumeric(numberPart) Then
                            Dim number As Integer = Convert.ToInt32(numberPart)
                            number += 1
                            Dim formattedNumber As String = Format(number, "000000")
                            membershipNo = prefix & formattedNumber
                            lblMessage.Text = "New Membership No: " & membershipNo
                        Else
                            lblMessage.Text = "The extracted number part is not valid or the wrong length."
                        End If
                    Else
                        lblMessage.Text = "The LatestRef does not start with the expected prefix."
                    End If

                    Me.GetAgeAndUATStructure(txtIDNumber.Text)
                    Me.GetAgeAndGender(txtIDNumber.Text)
                    Me.LoadMembershipNo()
                    Me.InsertMembership()
                    Me.SubmitProof()
                    submitButton.Enabled = False
                    ClearFields()
                ElseIf Exist = 1 Then
                    lblMessage.Text = "WARNING! Member EXIST- Membership No is " & membershipNo & " Expiry date: " & ExpiryDate & "."
                    submitButton.Enabled = False
                End If

            End If

        Catch ex As Exception
            lblMessage.Text = "Error! " + ex.Message
        End Try


    End Sub
    Public Function SubmitProof() As String
        If UploadPoP.HasFile Then
            Try
                ' Validate file type
                Dim validExtensions As String() = {".pdf", ".jpg", ".png"}
                Dim fileExtension As String = Path.GetExtension(UploadPoP.FileName).ToLower()

                If Not validExtensions.Contains(fileExtension) Then
                    Throw New Exception("Only PDF, JPG, or PNG files are allowed.")
                End If

                ' Save uploaded file to a temporary folder
                Dim filePath As String = Server.MapPath("~/Uploads/") & Path.GetFileName(UploadPoP.FileName)
                UploadPoP.SaveAs(filePath)

                ' Send email with the file as an attachment
                Me.SendEmailWithAttachment(filePath)

                Response.Write("<script>alert('CONGRATULATIONS! Membership is registered, The electronic Membership Card will be sent to you on payment confirmation on email by UAT Admin);</script>")

            Catch ex As Exception
                ' Handle errors
                Response.Write($"<script>alert('Error: {ex.Message}');</script>")
                lblMessage.Text = "Error: " + ex.Message
            End Try
        End If

        Return True
    End Function
    Protected Sub ddllanguage_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddllanguage.SelectedIndexChanged
        Dim genderIndicator As Integer = Convert.ToInt32(txtIDNumber.Text.Substring(6, 4))
        Dim strGender As String = If(genderIndicator >= 5000, "Male", "Female")

        ' Assign the value to the DropDownList
        ddlgender.SelectedValue = strGender
        ddlgender.Enabled = False
    End Sub
    Public Function GetAgeAndUATStructure(idNumber As String)
        Try
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

            If MemberAge > 55 Then
                UATCategory = "Pensioner"
            ElseIf ckDisability.Checked = True Then
                UATCategory = "People living with disability"
            ElseIf ckDisability.Checked = False And MemberAge < 55 Then

                UATCategory = "Mother body"
            End If

            Return True
        Catch ex As Exception
            lblMessage.Text = "Error! Problem with the ID Number captured. " & ex.Message
            Return False
        End Try


    End Function
    Protected Sub txtIDNumber_TextChanged(sender As Object, e As EventArgs) Handles txtIDNumber.TextChanged

        Dim genderDigit As Integer = CInt(txtIDNumber.Text.Substring(6, 1))
        If genderDigit >= 0 AndAlso genderDigit <= 4 Then
            ddlgender.SelectedItem.Text = "Female"
        ElseIf genderDigit >= 5 AndAlso genderDigit <= 9 Then
            ddlgender.SelectedItem.Text = "Male"
        End If
        IDNumber = txtIDNumber.Text
        Me.FindMember()

    End Sub
    Private Sub SendEmailWithAttachment(filePath As String)
        Try
            UpdatePayment = 1
            Dim emailAddress As String = "membership@uat2023.org.za"
            Dim baseUrl As String = "https://www.uatmembership.org.za/MembershipRegistration.aspx?prmMemberID=" + CStr(hdnMemberID.Value) + "&prmStatus=" + UpdatePayment

            ' Ensure the email address is not empty
            If String.IsNullOrEmpty(emailAddress) Then
                Throw New Exception("Email address cannot be empty.")
            End If
            Try
                ' Set up the email components
                Dim mail As New MailMessage()
                mail.From = New MailAddress("itdepartment@uat2023.org.za") ' Replace with your email
                mail.To.Add(emailAddress)
                mail.Subject = "New Member Registration with Proof of Payment for Ref: " + membershipNo
                mail.IsBodyHtml = True


                Dim imagePath As String = Server.MapPath("~/Images/Logo.jpeg")
                Dim linkedImage As New LinkedResource(imagePath)
                linkedImage.ContentId = "UATLogo"  ' Content ID for referencing in HTML

                ' Create an AlternateView for the HTML body
                Dim htmlView As AlternateView = AlternateView.CreateAlternateViewFromString(
                "Khanimambo Doer,<br/><br/>" &
                "New Membership received with proof of payment attached.<br/><br/>" &
                "<strong>Member FullName:</strong>" & txtFirstName.Text & " " & txtLastName.Text & "; <strong>Membership Number:</strong>" & membershipNo & "<br/><br/>" &
                "Please access the membership form for the member on this link:" & baseUrl & "</strong><br/><br/>" &
                "<strong>Once you confirmed payment on the form, the Member will receive electronic Membership Card </strong> <br/><br/>" &
                "Warm Regards,<br/><br/>" &
                "UAT IT Administration <br/>", Nothing, "text/html")

                ' Add the image as a linked resource
                htmlView.LinkedResources.Add(linkedImage)

                ' Attach the HTML view to the mail
                mail.AlternateViews.Add(htmlView)

                ' Attach the file
                If Not String.IsNullOrEmpty(filePath) AndAlso IO.File.Exists(filePath) Then
                    Dim attachment As New Attachment(filePath)
                    mail.Attachments.Add(attachment)
                Else
                    Throw New Exception("Attachment file is missing.")
                End If

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
    Private Sub SendEmailToMember()

        Try
            ' Retrieve the email address from txtEmail
            Dim expiryDate As DateTime = DateTime.Now.AddYears(ExpiryYearValue)
            Dim emailAddress As String = txtEmail.Text.Trim()

            ' Ensure the email address is not empty
            If String.IsNullOrEmpty(emailAddress) Then
                Throw New Exception("Email address cannot be empty.")
            End If
            Try
                ' Set up the email components
                Dim mail As New MailMessage()
                mail.From = New MailAddress("itdepartment@uat2023.org.za") ' Replace with your email
                mail.To.Add(emailAddress)
                mail.Subject = "Success! UAT Membership Registration : " + membershipNo
                mail.IsBodyHtml = True

                ' Load the image and attach it to the email
                Dim imagePath As String = Server.MapPath("~/Images/Logo.jpeg")
                Dim linkedImage As New LinkedResource(imagePath)
                linkedImage.ContentId = "UATLogo"  ' Content ID for referencing in HTML

                ' Create an AlternateView for the HTML body
                Dim htmlView As AlternateView = AlternateView.CreateAlternateViewFromString(
    "Khanimambo Doer,<br/><br/>" &
    "You have successfully registered your UAT Membership.<br/><br/>" &
    "Your Membership Number Is: <strong>" & Session("membershipNo") & "</strong><br/><br/>" &
    "We are excited to have you as a member of the United Africans Transformation (UAT) Party. We look forward to your active participation in our mission to drive positive change across Africa.<br/><br/>" &
    "Stay tuned for upcoming events and updates.<br/><br/>" &
    "<div style='border: 2px solid #d1d1d1; padding: 20px; max-width: 600px; border-radius: 10px; font-family: Arial, sans-serif; background-color: #ffffff;'>" &
    "   <h3 style='color: #ffffff; text-align: center; background-color: #2c3e50; padding: 10px; border-radius: 5px;'>UAT Membership Card</h3>" &
    "   <table style='width: 100%; border-collapse: collapse;'>" &
    "       <tr>" &
    "           <td style='width: 70%; vertical-align: top; padding-right: 10px;'>" &
    "               <h4 style='color: #2c3e50; border-bottom: 2px solid #d1d1d1; padding-bottom: 5px;'>Personal Information</h4>" &
    "               <p style='color: #333; font-size: 12px;'><strong>Membership Number:</strong> <span style='color: #1e90ff;'>" & Session("membershipNo") & "</span></p>" &
    "               <p style='color: #333; font-size: 11px;'><strong>Member Name:</strong> <span style='color: #ff0000;'>" & txtFirstName.Text & " " & txtLastName.Text & "</span></p>" &
    "               <p style='color: #333; font-size: 11px;'><strong>ID Number:</strong> <span style='color: #888888;'>" & txtIDNumber.Text & "</span></p>" &
    "               <p style='color: #333; font-size: 11px;'><strong>Expiry Date:</strong> <span style='color: #888888;'>" & expiryDate.ToString("dd MMM yyyy") & "</span></p>" &
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
    Protected Sub resetButton_Click(sender As Object, e As EventArgs) Handles resetButton.Click
        'txtFirstName.Text = ""
        'txtLastName.Text = ""
        'txtIDNumber.Text = ""
        'txtAge.Text = ""
        'ddllanguage.SelectedValue = 0
        'ddlgender.SelectedValue = 0
        'ddlSubscriptionType.SelectedValue = 0
        'ddlregisteredVoter.SelectedValue = 0
        'ddlCountry.SelectedValue = 0
        'txtEmail.Text = ""
        'txtCellNumber.Text = ""
        'txtTelephoneH.Text = ""
        'txtTelephoneW.Text = ""
        'ddlProvince.SelectedValue = 0
        'ddlDistrictMunicipality.SelectedValue = 0
        'ddlLocalMunicipality.SelectedValue = 0
        'ddlWardNo.SelectedValue = 0
        'txtResidentialAddress.Text = ""
        'txtPostalCode.Text = ""
        'ddlUniversity.SelectedValue = 0
        'membershipNo = "UAT202412345"
        'Me.SendSMS()
    End Sub

    Protected Sub ddlLocalMunicipality_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlLocalMunicipality.SelectedIndexChanged
        PopulateCombo("LocationID", "LocationName", "LOCATION", "LocalID = " & ddlLocalMunicipality.SelectedValue, "", ddlLocation)
        ddlLocation.Items.Insert(0, New ListItem("Select Location ", ""))
        ddlLocation.SelectedItem.Value = 0
    End Sub

    Private Sub MembershipRegistration_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        loggedInUser = Environment.UserName
    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Dim Msg As String
        Dim MembershipFee As String

        If annualFee.Checked Then
            ExpiryYearValue = 1
            MembershipFee = "R20 Annual Fee"
        Else
            ExpiryYearValue = 5
            MembershipFee = "R100 5Years Fee"
        End If
        Dim expiryDate As DateTime = DateTime.Now.AddYears(ExpiryYearValue)
        Dim renewalDate As DateTime = DateTime.Now.AddYears(ExpiryYearValue)
        Dim query As String = "UPDATE tblMembership SET PaidMembership = 'Yes', MaintainBy = @MaintainBy, MaintainDate = @MaintainDate, MembershipFee = @MembershipFee  OUTPUT Inserted.MembershipNo WHERE MemberID = @MemberID"
        Dim constr As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

        Using conne As New SqlConnection(constr)
            Using cmd As SqlCommand = New SqlCommand(query, conne)
                cmd.CommandType = CommandType.Text
                cmd.Parameters.AddWithValue("@MaintainBy", loggedInUser)
                cmd.Parameters.AddWithValue("@MaintainDate", DateTime.Now)
                cmd.Parameters.AddWithValue("@MemberID", PrmMemberID)
                cmd.Parameters.AddWithValue("@MembershipFee", MembershipFee)

                conne.Open()
                Dim result As Object = cmd.ExecuteScalar()
                If result IsNot Nothing Then
                    membershipNo = result.ToString()
                End If
                conne.Close()
                Me.SendEmailToMember()
                Msg = "Congratulations Doer! Your membership is successfully registered. Your Membership Number is: " + membershipNo + " Remember to renew your account before " + ExpiryDate
                Me.SendSMS()
            End Using
        End Using

    End Sub
    Function readHtmlPage(ByVal url As String) As String
        Dim objResponse As WebResponse
        Dim objRequest As WebRequest
        Dim result As String
        Try
            objRequest = System.Net.HttpWebRequest.Create(url)
            objResponse = objRequest.GetResponse()
            Dim sr As New StreamReader(objResponse.GetResponseStream())
            result = sr.ReadToEnd()
            'clean up StreamReader
            sr.Close()
            Return result
        Catch ex As Exception
            Dim s As String = ex.ToString
            If InStr(s, "(404)") Then
                Return "URL: " & url & " not found."
            Else
                Return s
            End If
        End Try
    End Function
    Public Sub SendSMS()
        Try
            ' Your WinSMS credentials
            Dim username As String = "idakgopiso@gmail.com"
            Dim password As String = "Ida@kopano88"
            Dim message As String = "Congratulations Doer! Your membership is successfully registered. Your Membership Number is: " + membershipNo + " Remember to renew your account before " + ExpiryDate
            Dim cellNumber As String = txtCellNumber.Text

            ' Build the URL for the SMS API
            Dim MyString As String = "http://www.winsms.co.za/api/batchmessage.asp?User=" & username &
                                 "&Password=" & password & "&Delivery=No" &
                                 "&Message=" & Uri.EscapeDataString(message) & "&Numbers=" & cellNumber

            ' Create the request
            Dim request As HttpWebRequest = CType(WebRequest.Create(MyString), HttpWebRequest)
            request.Method = "GET"

            ' Get the response
            Using response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                Using reader As New StreamReader(response.GetResponseStream())
                    Dim responseContent As String = reader.ReadToEnd()
                    Console.WriteLine("Response: " & responseContent)
                    MsgBox("SMS sent successfully: " & responseContent)
                End Using
            End Using

        Catch ex As Exception
            Console.WriteLine("Error sending SMS: " & ex.Message)
            MsgBox("Error sending SMS: " & ex.Message)
        End Try
    End Sub
    Private Sub UpdatePaymentStatus(ByVal transactionId As String, ByVal paymentAmount As String, ByVal membershipNo As String)
        Try
            ' Update the payment status in your database
            ' Example: Save transaction details to the database
            Console.WriteLine("Updating payment status...")
            Console.WriteLine("Transaction ID: " & transactionId)
            Console.WriteLine("Payment Amount: R" & paymentAmount)
            Console.WriteLine("Membership No: " & membershipNo)

            ' Perform actual database update here
            ' Example: Use ADO.NET or Entity Framework to update the payment status
        Catch ex As Exception
            ' Log any errors that occur during the update
            Console.WriteLine("Error updating payment status: " & ex.Message)
        End Try
    End Sub

End Class

