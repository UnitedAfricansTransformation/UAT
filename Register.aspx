<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Register.aspx.vb" Inherits="NewMembership.Register" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Register User</title>

    <style >

        body {
    font-family: Arial, sans-serif;
    margin: 0;
    padding: 0;
    background-color: #f9f9f9; /* Light grey background */
}

.container {
    width: 50%;
    margin: auto;
    padding: 20px;
    background-color: #ffffff; /* White background */
    border: 1px solid #ddd; /* Light grey border */
    border-radius: 8px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); /* Subtle shadow */
    margin-top: 50px;
}

h2 {
    text-align: center;
    color: #003366; /* Dark blue for headings */
}

label {
    display: block;
    margin-bottom: 8px;
    color: #333; /* Grey text */
    font-weight: bold;
}

input[type="text"], input[type="password"], input[type="email"], select {
    width: 100%;
    padding: 10px;
    margin-bottom: 15px;
    border: 1px solid #ccc; /* Light grey border */
    border-radius: 5px;
    box-sizing: border-box;
}

button, input[type="submit"] {
    width: 100%;
    padding: 10px;
    background-color: #003366; /* Blue background */
    color: #ffffff; /* White text */
    border: none;
    border-radius: 5px;
    font-size: 16px;
    cursor: pointer;
    margin-bottom: 10px;
}

button:hover, input[type="submit"]:hover {
    background-color: #cc0000; /* Red on hover */
}

#logo {
    display: block;
    margin: auto;
    margin-bottom: 20px;
    width: 120px;
    height: auto;
}

footer {
    text-align: center;
    margin-top: 30px;
    color: #666; /* Grey footer text */
    font-size: 14px;
}

.text-danger {
    color: red;
    font-size: 0.8em;
}
    </style>
    <script>
        function validateIDNumber() {
            const idNumber = $('#txtIDNumber').val();
            let errorMsg = '';

            // 1. Length check
            if (idNumber.length !== 13 || !/^\d+$/.test(idNumber)) {
                errorMsg = "ID Number must be exactly 13 digits long and numeric.";
            } else {
                // 2. Birthdate validation
                const birthDate = idNumber.substr(0, 6);
                const year = parseInt(birthDate.substr(0, 2), 10) + (birthDate.substr(0, 2) < '50' ? 2000 : 1900);
                const month = parseInt(birthDate.substr(2, 2), 10) - 1;
                const day = parseInt(birthDate.substr(4, 2), 10);
                const date = new Date(year, month, day);

                if (date.getFullYear() !== year || date.getMonth() !== month || date.getDate() !== day) {
                    errorMsg = "Invalid birth date in ID Number.";
                }

                // 3. Gender validation
                const genderCode = parseInt(idNumber.substr(6, 4), 10);
                if (genderCode < 0 || genderCode > 9999) {
                    errorMsg = "Invalid gender indicator in ID Number.";
                }

                // 4. Citizenship validation
                const citizenship = idNumber[10];
                if (citizenship !== '0' && citizenship !== '1') {
                    errorMsg = "Invalid citizenship indicator in ID Number.";
                }

                // 5. Checksum validation using Luhn algorithm
                if (!isValidLuhn(idNumber)) {
                    errorMsg = "Invalid ID Number checksum.";
                }
            }

            $('#error-msg').text(errorMsg);
            if (!errorMsg) {
                errorMsg = "South African ID Number is Checked and Valid"
            }


        }


    </script>
    </head>
<body>
    <form id="form1" runat="server" width="900px">
     
        <div class="container">
            <img id="logo" src="Images/Logo.jpeg" alt="Logo" style="display:block; margin: 0 auto; width: 150px;" />

            <h2 style="text-align: center;">Admin Portal User Registration</h2>
            <table style="width: 100%;">
    <tr>
        <td><label for="txtFullname">Fullname:</label></td>
        <td><asp:TextBox ID="txtFullname" runat="server" Width="300px"></asp:TextBox></td>
    </tr>
    <tr>
        <td><label for="txtIDNumber">ID Number:</label></td>
        <td>
            <asp:TextBox ID="txtIDNumber" runat="server" Width="300px" TextMode="Number" oninput="validateIDNumber()"></asp:TextBox>
            <span id="error-msg" style="color: red;"></span><br />
            <asp:RequiredFieldValidator ID="rfvIDNumber" runat="server" ControlToValidate="txtIDNumber"
                ErrorMessage="ID Number is required." Display="Dynamic" CssClass="text-danger" style="color: #CC0000; font-size: xx-small" /><br />
            <asp:RegularExpressionValidator ID="revIDNumber" runat="server" ControlToValidate="txtIDNumber"
                ValidationExpression="^\d{13}$" ErrorMessage="Enter a valid 13-digit South African ID number."
                Display="Dynamic" CssClass="text-danger" style="color: #CC0000; font-size: xx-small" />
        </td>
    </tr>
   
    <tr>
        <td><label for="txtEmail">Email:</label></td>
        <td><asp:TextBox ID="txtEmail" runat="server" Width="300px" TextMode="Email"></asp:TextBox></td>
    </tr>
    <tr>
        <td><label for="txtCellNumber">Cell Number:</label></td>
        <td><asp:TextBox ID="txtCellNumber" runat="server" Width="300px"></asp:TextBox></td>
    </tr>
    <tr>
        <td><label for="ddlProvince">Province:</label></td>
        <td>
            <asp:DropDownList ID="ddlProvince" runat="server" Width="300px" AutoPostBack="True" OnSelectedIndexChanged="ddlProvince_SelectedIndexChanged" CssClass="form-control" />
        </td>
    </tr>
    <tr>
        <td><label for="ddlDistrictMunicipality">District Municipality/Metropolitan:</label></td>
        <td>
            <asp:DropDownList ID="ddlDistrictMunicipality" runat="server" Width="300px" AutoPostBack="True" OnSelectedIndexChanged="ddlDistrictMunicipality_SelectedIndexChanged" CssClass="form-control" /><br />
            <asp:RequiredFieldValidator ID="rfvDistrict" runat="server" ControlToValidate="ddlDistrictMunicipality"
                ErrorMessage="District Municipality is required." Display="Dynamic" CssClass="text-danger" />
        </td>
    </tr>
    <tr>
        <td><label for="ddlLocalMunicipality">Local Municipality:</label></td>
        <td>
            <asp:DropDownList ID="ddlLocalMunicipality" runat="server" Width="300px" AutoPostBack="True" CssClass="form-control" /><br />
            <asp:RequiredFieldValidator ID="rfvLocal" runat="server" ControlToValidate="ddlLocalMunicipality"
                ErrorMessage="Local Municipality is required." Display="Dynamic" CssClass="text-danger" />
        </td>
    </tr>
    <tr>
        <td><label for="ddlLocation">Location/Office:</label></td>
        <td>
            <asp:DropDownList ID="ddlLocation" runat="server" Width="300px" CssClass="form-control" /><br />
            <asp:RequiredFieldValidator ID="rfvLocation" runat="server" ControlToValidate="ddlLocation"
                ErrorMessage="Location is required." Display="Dynamic" CssClass="text-danger" />
        </td>
    </tr>
    <tr>
        <td><label for="txtJobTitle">Work Group:</label></td>
        <td>
<asp:DropDownList ID="ddlUsergroup" runat="server" Width="300px" CssClass="form-control">
    <asp:ListItem Text="Select" Value="Select" Selected="True"></asp:ListItem>
    <asp:ListItem Text="District Secretary" Value="District"></asp:ListItem>
    <asp:ListItem Text="Provincial Secretary" Value="Provincial"></asp:ListItem>
    <asp:ListItem Text="National" Value="National"></asp:ListItem>
</asp:DropDownList>
</td>
    </tr>
                 <tr>
     <td><label for="txtPassword">Password:</label></td>
     <td><asp:TextBox ID="txtPassword" runat="server" Width="300px" TextMode="Password"></asp:TextBox></td>
 </tr>
 <tr>
     <td><label for="txtConfirmPassword">Confirm Password:</label></td>
     <td><asp:TextBox ID="txtConfirmPassword" runat="server" Width="300px" TextMode="Password"></asp:TextBox></td>
 </tr>
    <tr>
        <td colspan="2" style="text-align: center;">
            <asp:Button ID="btnRegister" runat="server" Text="Register" OnClick="btnRegister_Click" />
            &nbsp;
            <asp:Button ID="btnReset" runat="server" Text="Reset" OnClientClick="return confirm('Are you sure you want to reset the form?')" />
        </td>
    </tr>
</table>

          </div>
        <footer>© 2025 United Africans Transformation. All rights reserved.</footer>
    </form>
</body>
</html>
