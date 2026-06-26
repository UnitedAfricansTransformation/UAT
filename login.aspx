<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="login.aspx.vb" Inherits="NewMembership.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
<title>Login Page</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f9;
        }
        .login-container {
            max-width: 400px;
            margin: 50px auto;
            padding: 20px;
            background: #fff;
            border-radius: 8px;
            box-shadow: 0px 4px 6px rgba(0, 0, 0, 0.1);
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
        .forgot-password {
            display: block;
            margin-top: 10px;
            text-align: center;
        }
        .forgot-password a {
            color: #007BFF;
            text-decoration: none;
        }
        .forgot-password a:hover {
            text-decoration: underline;
        }
        .register-link {
            display: block;
            margin-top: 15px;
            text-align: center;
        }
        .register-link a {
            color: #28a745;
            text-decoration: none;
            font-weight: bold;
        }
        .register-link a:hover {
            text-decoration: underline;
        }
        .error-msg {
            color: red;
            font-size: 14px;
            margin-top: 10px;
        }
        .auto-style1 {
            font-size: x-small;
            color: red;
        }
        .auto-style2 {
            color: #CC3300;
        }
        .auto-style3 {
            text-align: center;
            color: #000066;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <img id="logo" src="Images/Logo.jpeg" alt="Logo" style="display:block; margin: 0 auto; width: 150px;" />
            <h2 class="auto-style3">UAT Portal Login Page</h2>
            <asp:Label ID="lblErrorMessage" runat="server" CssClass="error-msg"></asp:Label><br />

            <label for="txtUsername">Email:</label>
            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"></asp:TextBox>

            <label for="txtPassword">Password:</label>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>

            <div class="remember-me">
                <asp:CheckBox ID="chkRememberMe" runat="server" Text="Remember Me" />
            </div>

            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn" OnClick="btnLogin_Click" />
            
            <!-- NEW REGISTER LINK -->
          <%--  <div class="register-link">
                <a href="Register.aspx">Don't have an account? Register here</a>
            </div>--%>

            <div class="forgot-password">
               <asp:Label ID="Label1" runat="server"  class="auto-style2" Text="Forgot Password?"></asp:Label> <br />
                <span class="auto-style1">Type in the Email address and then click Retrieve Password below</span> <br class="auto-style1" />
                <asp:Button ID="btnRetrieve" runat="server" Text="Retrieve Password" Width="300px"/>
            </div>
        </div>
    </form>
    </body>
</html>