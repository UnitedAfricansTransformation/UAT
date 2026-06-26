Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Security.Cryptography
Imports System.Text
Imports System.Drawing
Imports System.Net


Public Class login
    Inherits System.Web.UI.Page
    Dim strConstring As String = System.Configuration.ConfigurationManager.ConnectionStrings("connectionString").ConnectionString
    Dim conn As New SqlConnection(strConstring)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' Check if cookies exist for username and password
            If Request.Cookies("Username") IsNot Nothing Then
                txtUsername.Text = Request.Cookies("Username").Value
            End If

            If Request.Cookies("Password") IsNot Nothing Then
                txtPassword.Attributes("value") = Request.Cookies("Password").Value
                chkRememberMe.Checked = True
            End If
        End If
    End Sub
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
    Protected Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim username As String = txtUsername.Text.Trim()
        Dim password As String = Encrypt(txtPassword.Text.Trim())

        Try
            Dim query As String = "SELECT DistrictMunicipality, Province, JobTitle  FROM Users WHERE Email = @Username AND PasswordHash = @PasswordHash"

            Using cmd As New SqlCommand(query, conn)
                ' Add parameters to avoid SQL injection
                cmd.Parameters.AddWithValue("@Username", username)
                cmd.Parameters.AddWithValue("@PasswordHash", password) ' Already encrypted

                conn.Open()

                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        ' ✅ Login successful
                        Session("Username") = username
                        Session("DistrictMunicipality") = reader("DistrictMunicipality").ToString()
                        Session("Province") = reader("Province").ToString()
                        Session("JobTitle") = reader("JobTitle").ToString()

                        Response.Redirect("UATDatabaseReport.aspx", False)
                        Return
                    Else
                        ' ❌ Login failed
                        lblErrorMessage.Text = "Invalid username or password."
                    End If
                End Using

                ' Get District ID immediately using the same open connection
                Dim IDQuery As String = "SELECT DistrictId FROM District WHERE [Name] = @DistrictName"
                Using idCmd As New SqlCommand(IDQuery, conn)
                    idCmd.Parameters.Add("@DistrictName", SqlDbType.VarChar).Value = Session("DistrictMunicipality").ToString()

                    Using idReader As SqlDataReader = idCmd.ExecuteReader()
                        If idReader.Read() Then
                            Session("DistrictID") = CInt(idReader("DistrictId"))
                        End If
                    End Using
                End Using
            End Using

            ' ✅ Remember Me functionality
            If chkRememberMe.Checked Then
                Dim usernameCookie As New HttpCookie("Username", username)
                Dim passwordCookie As New HttpCookie("Password", password)

                usernameCookie.Expires = DateTime.Now.AddDays(7)
                passwordCookie.Expires = DateTime.Now.AddDays(7)

                Response.Cookies.Add(usernameCookie)
                Response.Cookies.Add(passwordCookie)
            Else
                If Request.Cookies("Username") IsNot Nothing Then
                    Dim usernameCookie As New HttpCookie("Username") With {.Expires = DateTime.Now.AddDays(-1)}
                    Response.Cookies.Add(usernameCookie)
                End If
                If Request.Cookies("Password") IsNot Nothing Then
                    Dim passwordCookie As New HttpCookie("Password") With {.Expires = DateTime.Now.AddDays(-1)}
                    Response.Cookies.Add(passwordCookie)
                End If
            End If

        Catch ex As Exception
            lblErrorMessage.Text = "An error occurred: " & ex.Message
        Finally
            ' ✅ Always close connection safely
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    ' Sample hashing function (replace this with a more secure hashing algorithm like SHA256 or bcrypt)
    Private Function HashPassword(password As String) As String
        ' This is a simplified example. Use secure hashing algorithms for production use.
        Return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password))
    End Function

    Protected Sub btnRetrieve_Click(sender As Object, e As EventArgs) Handles btnRetrieve.Click
        Dim storedHash As Object
        Dim passwordPlain As String = String.Empty

        Dim query As String = "SELECT PasswordHash FROM Users WHERE Email = @Email"

        Using con As New SqlConnection(conn.ConnectionString)
            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddWithValue("@Email", txtUsername.Text.Trim())
                con.Open()
                storedHash = cmd.ExecuteScalar()
            End Using
        End Using

        ' Check if we found a record
        If storedHash Is Nothing OrElse storedHash Is DBNull.Value Then
            ClientScript.RegisterStartupScript(Me.GetType(), "UserNotFound", "alert('No account found with that Email Address.');", True)
            Exit Sub
        End If

        ' Convert stored hash back to plain password (if you are decrypting — not recommended for production)
        passwordPlain = Decrypt(storedHash.ToString())

        ' Send email with password
        SendEmail(txtUsername.Text.Trim(), passwordPlain, "https://www.uatmembership.org.za/login")

        ClientScript.RegisterStartupScript(Me.GetType(), "SuccessMessage", "alert('Password retrieval information sent to your email.');", True)

    End Sub

    Private Sub SendEmail(toEmail As String, password As String, loginUrl As String)
        Try
            Using smtp As New SmtpClient("mail.uat2023.org.za")
                smtp.Port = 587 ' Or 25 if your server uses it
                smtp.Credentials = New System.Net.NetworkCredential("itdepartment@uat2023.org.za", "it@uat2023")
                smtp.EnableSsl = True
                smtp.Timeout = 60000

                Using mail As New MailMessage()
                    mail.From = New MailAddress("itdepartment@uat2023.org.za", "UAT Administration Team")
                    mail.To.Add(toEmail)
                    mail.Subject = "Password Reminder Email"
                    mail.IsBodyHtml = True

                    ' HTML Body with styling
                    mail.Body =
                    "<p>Khanimambo Doer,</p>" &
                    "<p> Here are your login details:</p>" &
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
End Class