<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ForgotPassword.aspx.vb" Inherits="NewMembership.ForgotPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>Forgot Password</title>
    <style>
        .forgot-container {
            max-width: 400px;
            margin: 50px auto;
            padding: 20px;
            background: #fff;
            border-radius: 8px;
            box-shadow: 0px 4px 6px rgba(0, 0, 0, 0.1);
            font-family: Arial, sans-serif;
        }
        .form-control {
            width: 100%;
            padding: 10px;
            margin-bottom: 10px;
            border: 1px solid #ddd;
            border-radius: 4px;
        }
        .btn {
            background-color: #007BFF;
            color: #fff;
            border: none;
            padding: 10px;
            border-radius: 4px;
            cursor: pointer;
            width: 100%;
        }
        .btn:hover {
            background-color: #0056b3;
        }
        .success-msg {
            color: green;
            font-size: 14px;
            margin-top: 10px;
        }
        .error-msg {
            color: red;
            font-size: 14px;
            margin-top: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="forgot-container">
            <h2>Forgot Password</h2>
            <p>Enter your registered email address to reset your password.</p>
            <asp:Label ID="lblMessage" runat="server" CssClass=""></asp:Label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Placeholder="Enter your email"></asp:TextBox>
            <%--<asp:Button ID="btnReset" runat="server" Text="Reset Password" CssClass="btn" OnClick="btnReset_Click" />--%>
            <br />
            <br />
            <asp:Label ID="Label1" runat="server" Text="Enter Password"></asp:Label>
            <asp:TextBox ID="txtPassword" runat="server" Width="400px"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="btnEncrypt" runat="server" Text="Encrypt" />
            <br />
            <br />
            <asp:Label ID="Label2" runat="server" Text="Enctrypted Password"></asp:Label>
            <asp:TextBox ID="txtEncrypted" runat="server" Width="400px"></asp:TextBox>
            <br />
            <br />
            ==================================================<br />
            <asp:Label ID="Label3" runat="server" Text="Enter EncryptedPassword"></asp:Label>
            <asp:TextBox ID="txtPassEncrypted" runat="server" Width="400px"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="btnDecrypt" runat="server" Text="Decrypt" />
            <br />
            <br />
            <asp:Label ID="Label4" runat="server" Text="Decrypted Password"></asp:Label>
            <asp:TextBox ID="txtDecrypted" runat="server" Width="400px"></asp:TextBox>
        </div>
    </form>
</body>
</html>
