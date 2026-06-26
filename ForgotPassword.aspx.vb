
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Security.Cryptography
Imports System.Text
Public Class ForgotPassword
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    'Protected Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
    '    Dim email As String = txtEmail.Text.Trim()
    '    Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("DBConnectionString").ConnectionString

    '    Try
    '        Using conn As New SqlConnection(connectionString)
    '            Dim query As String = "SELECT COUNT(*) FROM Users WHERE Email = @Email"
    '            Using cmd As New SqlCommand(query, conn)
    '                cmd.Parameters.AddWithValue("@Email", email)
    '                conn.Open()
    '                Dim result As Integer = Convert.ToInt32(cmd.ExecuteScalar())

    '                If result > 0 Then
    '                    ' Generate a secure token
    '                    Dim token As String = Guid.NewGuid().ToString()
    '                    Dim resetLink As String = Request.Url.GetLeftPart(UriPartial.Authority) & "/ResetPassword.aspx?token=" & token

    '                    ' Store the token in the database
    '                    Dim insertQuery As String = "UPDATE Users SET ResetToken = @Token, TokenExpiry = @Expiry WHERE Email = @Email"
    '                    Using insertCmd As New SqlCommand(insertQuery, conn)
    '                        insertCmd.Parameters.AddWithValue("@Token", token)
    '                        insertCmd.Parameters.AddWithValue("@Expiry", DateTime.Now.AddHours(1)) ' Token valid for 1 hour
    '                        insertCmd.Parameters.AddWithValue("@Email", email)
    '                        insertCmd.ExecuteNonQuery()
    '                    End Using

    '                    ' Send reset link via email
    '                    SendResetEmail(email, resetLink)

    '                    lblMessage.Text = "<span class='success-msg'>A password reset link has been sent to your email.</span>"
    '                Else
    '                    lblMessage.Text = "<span class='error-msg'>Email address not found.</span>"
    '                End If
    '            End Using
    '        End Using
    '    Catch ex As Exception
    '        lblMessage.Text = "<span class='error-msg'>An error occurred: " & ex.Message & "</span>"
    '    End Try
    'End Sub

    'Private Sub SendResetEmail(email As String, resetLink As String)
    '    Dim mailMessage As New MailMessage()
    '    mailMessage.From = New MailAddress("no-reply@example.com")
    '    mailMessage.To.Add(email)
    '    mailMessage.Subject = "Password Reset Request"
    '    mailMessage.Body = "Click the link below to reset your password: <br /><a href='" & resetLink & "'>" & resetLink & "</a>"
    '    mailMessage.IsBodyHtml = True

    '    Dim smtp As New SmtpClient("smtp.example.com")
    '    smtp.Credentials = New System.Net.NetworkCredential("your-email@example.com", "your-email-password")
    '    smtp.Send(mailMessage)
    'End Sub

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
    Protected Sub btnEncrypt_Click(sender As Object, e As EventArgs) Handles btnEncrypt.Click
        If Not String.IsNullOrEmpty(txtPassword.Text) Then
            txtEncrypted.Text = Encrypt(txtPassword.Text)
        Else
            lblMessage.Text = "Please enter a Password to encrypt"
        End If

    End Sub

    Protected Sub btnDecrypt_Click(sender As Object, e As EventArgs) Handles btnDecrypt.Click
        If Not String.IsNullOrEmpty(txtEncrypted.Text) Then
            txtDecrypted.Text = Decrypt(txtPassEncrypted.Text)
        Else
            lblMessage.Text = "Please enter an encrypted password to decrypt."
        End If
    End Sub
End Class