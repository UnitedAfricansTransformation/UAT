Imports System.IO
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
Imports System.Text.RegularExpressions
Imports System.Drawing
Imports System.Net.Mail
Imports System.Net
Imports System.Data.SqlClient
Imports System.Security.Cryptography
Imports System.Text
Public Class Register
    Inherits System.Web.UI.Page
    Dim strConstring As String = System.Configuration.ConfigurationManager.ConnectionStrings("connectionString").ConnectionString
    Dim Conne As New SqlConnection(strConstring)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.PopulateCombos()
        End If
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

    Protected Sub ddlDistrictMunicipality_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDistrictMunicipality.SelectedIndexChanged
        PopulateCombo("LocalId", "LocalName", "LocalMunicipality", "DistrictId = " & ddlDistrictMunicipality.SelectedValue, "", ddlLocalMunicipality)
        ddlLocalMunicipality.Items.Insert(0, New ListItem("Select Municipality ", ""))
        ddlLocalMunicipality.SelectedItem.Value = 0
    End Sub

    Protected Sub ddlLocalMunicipality_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlLocalMunicipality.SelectedIndexChanged
        PopulateCombo("LocationID", "LocationName", "LOCATION", "LocalID = " & ddlLocalMunicipality.SelectedValue, "", ddlLocation)
        ddlLocation.Items.Insert(0, New ListItem("Select Location ", ""))
        ddlLocation.SelectedItem.Value = 0
    End Sub

    Protected Sub ddlProvince_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlProvince.SelectedIndexChanged
        PopulateCombo("DistrictId", "Name", "District", "ProvinceId = " & ddlProvince.SelectedValue, "", ddlDistrictMunicipality)
        ddlDistrictMunicipality.Items.Insert(0, New ListItem("Select District", ""))
        ddlDistrictMunicipality.SelectedItem.Value = 0
    End Sub

    Protected Sub btnRegister_Click(sender As Object, e As EventArgs) Handles btnRegister.Click
        Dim fullname As String = txtFullname.Text.Trim()
        Dim idNumber As String = txtIDNumber.Text.Trim()
        Dim password As String = Encrypt(txtPassword.Text.Trim()) ' Ideally hash the password
        Dim email As String = txtEmail.Text.Trim()
        Dim cellNumber As String = txtCellNumber.Text.Trim()
        Dim province As String = ddlProvince.SelectedItem.Text
        Dim districtMunicipality As String = ddlDistrictMunicipality.SelectedItem.Text
        Dim localMunicipality As String = ddlLocalMunicipality.SelectedItem.Text
        Dim location As String = ddlLocation.SelectedItem.Text
        Dim jobTitle As String = ddlUsergroup.SelectedValue

        ' Step 1: Check if user already exists
        Dim existingPasswordObj As Object = Nothing

        Using con As New SqlConnection(Conne.ConnectionString)
            Using checkCmd As New SqlCommand("SELECT PasswordHash FROM Users WHERE IDNumber = @IDNumber OR Email = @Email", con)
                checkCmd.Parameters.AddWithValue("@IDNumber", idNumber)
                checkCmd.Parameters.AddWithValue("@Email", email)
                con.Open()
                existingPasswordObj = checkCmd.ExecuteScalar()
            End Using
        End Using

        If existingPasswordObj IsNot Nothing AndAlso Not IsDBNull(existingPasswordObj) Then
            ' User exists — send email with existing password
            Dim existingPassword As String = Decrypt(existingPasswordObj.ToString())
            SendEmail(email, fullname, existingPassword, "https://www.uatmembership.org.za/login")
            ClientScript.RegisterStartupScript(Me.GetType(), "ExistsMessage", "alert('User already exists. Login details sent to email.');", True)
            Exit Sub
        End If

        ' Step 2: Insert new user
        Using con As New SqlConnection(Conne.ConnectionString)
            Using cmd As New SqlCommand("INSERT INTO Users (Fullname, IDNumber, PasswordHash, Email, CellNumber, Province, DistrictMunicipality, LocalMunicipality, Location, JobTitle) 
                                     VALUES (@Fullname, @IDNumber, @PasswordHash, @Email, @CellNumber, @Province, @DistrictMunicipality, @LocalMunicipality, @Location, @JobTitle)", con)
                cmd.Parameters.AddWithValue("@Fullname", fullname)
                cmd.Parameters.AddWithValue("@IDNumber", idNumber)
                cmd.Parameters.AddWithValue("@PasswordHash", password)
                cmd.Parameters.AddWithValue("@Email", email)
                cmd.Parameters.AddWithValue("@CellNumber", cellNumber)
                cmd.Parameters.AddWithValue("@Province", province)
                cmd.Parameters.AddWithValue("@DistrictMunicipality", districtMunicipality)
                cmd.Parameters.AddWithValue("@LocalMunicipality", localMunicipality)
                cmd.Parameters.AddWithValue("@Location", location)
                cmd.Parameters.AddWithValue("@JobTitle", jobTitle)

                con.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using

        ' Send welcome email with login link
        SendEmail(email, fullname, txtPassword.Text.Trim(), "https://www.uatmembership.org.za/login")
        ClientScript.RegisterStartupScript(Me.GetType(), "SuccessMessage", "alert('Registration successful! Login details sent to email.');", True)

        ClearForm()
    End Sub

    Private Sub SendEmail(toEmail As String, fullName As String, password As String, loginUrl As String)
        Try
            Using smtp As New SmtpClient("mail.uat2023.org.za")
                smtp.Port = 587 ' Or 25 if your server uses it
                smtp.Credentials = New System.Net.NetworkCredential("itdepartment@uat2023.org.za", "it@uat2023")
                smtp.EnableSsl = True
                smtp.Timeout = 60000

                Using mail As New MailMessage()
                    mail.From = New MailAddress("itdepartment@uat2023.org.za", "UAT Administration Team")
                    mail.To.Add(toEmail)
                    mail.Subject = "Your Login Details"
                    mail.IsBodyHtml = True

                    ' HTML Body with styling
                    mail.Body =
                    "<p>Khanimambo Doer " & fullName & ",</p>" &
                    "<p>Your account is ready. Here are your login details:</p>" &
                     "<p><strong>Username:</strong> " & toEmail & "</p>" &
                    "<p><strong>Password:</strong> " & password & "</p>" &
                    "<p><a href='" & loginUrl & "'>Click here to login</a></p>" &
                    "<p>Warm Regards,<br/>UAT Administration Team</p>"

                    smtp.Send(mail)
                End Using
            End Using
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "EmailError", "alert('Could not send email: " & ex.Message & "');", True)
        End Try
    End Sub
    Private Sub ClearForm()
        txtFullname.Text = ""
        txtIDNumber.Text = ""
        txtPassword.Text = ""
        txtConfirmPassword.Text = ""
        txtEmail.Text = ""
        txtCellNumber.Text = ""
        ddlProvince.ClearSelection()
        ddlDistrictMunicipality.ClearSelection()
        ddlLocalMunicipality.ClearSelection()
        ddlLocation.ClearSelection()
        ddlUsergroup.ClearSelection()
    End Sub

    ' Function to hash passwords (for security)
    Private Function HashPassword(password As String) As String

        Dim sha256 As New System.Security.Cryptography.SHA256Managed()
        Dim bytes As Byte() = System.Text.Encoding.UTF8.GetBytes(password)
        Dim hash As Byte() = sha256.ComputeHash(bytes)
        Return Convert.ToBase64String(hash)
    End Function

    Private Function Encrypt(password As String) As String

        Dim md5 As New MD5CryptoServiceProvider()
        Dim tdes As New TripleDESCryptoServiceProvider()
        Dim utf8 As New UTF8Encoding()

        tdes.Key = md5.ComputeHash(utf8.GetBytes("Pass123"))
        tdes.Mode = CipherMode.ECB

        Dim encryptor As ICryptoTransform = tdes.CreateEncryptor()
        Dim buffer As Byte() = utf8.GetBytes(password)

        Return Convert.ToBase64String(encryptor.TransformFinalBlock(buffer, 0, buffer.Length))

    End Function
    Private Function Decrypt(encryptedPassword As String) As String

        Dim md5 As New MD5CryptoServiceProvider()
        Dim tdes As New TripleDESCryptoServiceProvider()
        Dim utf8 As New UTF8Encoding()

        tdes.Key = md5.ComputeHash(utf8.GetBytes("Pass123"))
        tdes.Mode = CipherMode.ECB

        Dim decryptor As ICryptoTransform = tdes.CreateDecryptor()
        Dim buffer As Byte() = Convert.FromBase64String(encryptedPassword)

        Return utf8.GetString(decryptor.TransformFinalBlock(buffer, 0, buffer.Length))

    End Function


End Class