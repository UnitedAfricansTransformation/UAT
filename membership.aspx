<%@ Page Language="VB" AutoEventWireup="false" CodeFile="membership.aspx.vb" Inherits="NewMembership.membership" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>UAT Membership Registration</title>
    <link href="Styles/Layout.css" rel="stylesheet" />
    <style type="text/css">
        body {
            background-color: silver;
            font-family: Arial, sans-serif;
            margin: 0 auto;
        }
        .form-container {
            background-color: white;
            border-radius: 10px;
            box-shadow: 0 0 10px grey;
            margin: 20px auto;
            padding: 20px;
            width: 80%;
        }
        h2 {
            color: royalblue;
            text-align: center;
        }
        label {
            color: royalblue;
            font-weight: bold;
        }
        table {
            width: 100%;
            table-layout: fixed;
        }
        table td {
            padding: 10px;
            vertical-align: top;
        }
        input[type="text"],
        input[type="number"],
        select {
            width: 95%;
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 4px;
        }
        .btn-container {
            text-align: center;
        }
        .btn-container input[type="submit"],
        .btn-container input[type="reset"],
        .btn-container input[type="button"] {
            background-color: royalblue;
            color: white;
            border: none;
            padding: 10px 20px;
            border-radius: 4px;
            cursor: pointer;
        }
        .gridview-container {
            margin-top: 30px;
            width: 100%;
           margin-bottom: 100px;
            color:black;
             background-color: white;
        }
        .gridview-container table {
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 100px;
        }
        .gridview-container th {
            background-color: royalblue;
            color: white;
            padding: 10px;
        }
        .gridview-container td {
            /*padding: 10px;*/
            text-align: center;
            width:200px;
        }
       /* .auto-style2 {
            height: 75px;
        }*/
        .auto-style3 {
            width: 195px;
            height: 167px;
        }
    </style>

     <script type="text/javascript">
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

        // Luhn algorithm for checksum validation
        function isValidLuhn(idNumber) {
            let sum = 0;
            for (let i = 0; i < idNumber.length; i++) {
                let num = parseInt(idNumber[i]);
                if (i % 2 === 0) {
                    sum += num;
                } else {
                    num *= 2;
                    if (num > 9) num -= 9;
                    sum += num;
                }
            }
            return sum % 10 === 0;
         }

             function showOverlay() {
            const overlay = document.getElementById("overlay");
             const progressBar = document.getElementById("progressBar");
             const closeButton = document.getElementById("closeButton");

             // Show the overlay
             overlay.style.display = "flex";

             // Reset progress bar and disable the Close button
             progressBar.style.width = "0%";
             closeButton.disabled = true;

             let progress = 0;

            // Simulate progress bar filling over 60 seconds
            const interval = setInterval(() => {
                 progress += 1; // Increment progress by 1%
             progressBar.style.width = progress + "%";

                if (progress >= 100) {
                 clearInterval(interval); // Stop interval when progress reaches 100%
             closeButton.disabled = false; // Enable the Close button
                }
            }, 300); // 600ms per 1% for a total of 60 seconds
        }

             function closeOverlay() {
                 document.getElementById("overlay").style.display = "none";
        }

             let selectedAmount = 0;

             // Set the amount based on subscription choice
             function setAmount(amount) {
                 selectedAmount = amount;
             document.getElementById('payAmount').value = selectedAmount.toFixed(2); // Update Payfast amount
        }
          
     </script>
</head>
<body style="left: 0px; top: 0px; height:100%" >
    <form id="form1" runat="server">
        <div class="form-container">
         
            <table>
                <tr >
                    <td>  <img alt="" class="auto-style3" src="Images/Logo.JPG" /></td>
             
                    <td colspan="3" style="padding:10px; vertical-align:middle; font-size:large "> <h2>UNITED AFRICANS TRANSFORMATION (UAT)</h2> </td>
                </tr>
                <tr style="background-color:lightgrey">
                    <td colspan="2">
                        </td>
                    <td style="font-size: 10px">
                       <label for="txtSearchIDNumber" style="color: #000080">Enter South African ID Number to Search</label><br />
                          <asp:TextBox ID="SearchIDNo" runat="server" TextMode="Number"></asp:TextBox>
                    </td>
                    <td class="btn-container">
                        <asp:Button ID="btnSearch" runat="server" Text="Search"/>
                    </td>
                </tr>
                <tr>
                    <td colspan ="4">
                         <h3 style="color: #000080; font-size:medium; text-align:left">UAT Membership Registration Form</h3>
                        </td>
                </tr>
                <tr>
                    <td>
                        <label for="txtMemberNo">Membership No</label><br />
                        <asp:TextBox ID="txtMemberNo" runat="server" Enabled="False"></asp:TextBox>
                    </td>
                    <td>
                      <label for="txtFirstName"> FirstName:</label><br /><asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtFirstName"
                            ErrorMessage="Name is required." Display="Dynamic" CssClass="text-danger" />
                    </td>
                    <td>
              <label for="txtLastName">Surname:</label><br /><asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvtxtLastName" runat="server" ControlToValidate="txtLastName"
                            ErrorMessage="Surname is required." Display="Dynamic" CssClass="text-danger" />
                    </td>
                    <td>
  <label for="idNumber">ID Number:</label><br /><asp:TextBox ID="txtIDNumber" runat="server" TextMode="Number" CssClass="form-control" oninput="validateIDNumber()" AutoPostBack="True"/>
     <span id="error-msg" style="color: red;"></span>
     <asp:RequiredFieldValidator ID="rfvIDNumber" runat="server" ControlToValidate="txtIDNumber"
         ErrorMessage="ID Number is required." Display="Dynamic" CssClass="text-danger" />
     <asp:RegularExpressionValidator ID="revIDNumber" runat="server" ControlToValidate="txtIDNumber"
         ValidationExpression="^\d{13}$" ErrorMessage="Enter a valid 13-digit South African ID number." Display="Dynamic" CssClass="text-danger" />
  <asp:TextBox ID="txtAge" runat="server" TextMode="Number" CssClass="form-control" Visible="false"/>
                     
                    </td>
                </tr>
                <tr>
                    <td>
                        <label for="txtCellNo">Cell Number</label><br />
                        <asp:TextBox ID="txtCellNo" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <label for="ddlProvince">Province</label><br />
                        <asp:DropDownList ID="ddlProvince" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlProvince_SelectedIndexChanged" />
                        <asp:RequiredFieldValidator ID="rfvProvince" runat="server" ControlToValidate="ddlProvince"
                            InitialValue="" ErrorMessage="Province is required." Display="Dynamic" CssClass="text-danger" />
                    </td>
                    <td>
                        <label for="ddlDistrictMunicipality">District Municipality</label><br />
                      <asp:DropDownList ID="ddlDistrictMunicipality" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlDistrictMunicipality_SelectedIndexChanged"/>
                        <asp:RequiredFieldValidator ID="rfvDistrict" runat="server" ControlToValidate="ddlDistrictMunicipality" ErrorMessage="District Municipality is required." Display="Dynamic" CssClass="text-danger" />

                    </td>
                    <td>
                        <label for="ddlLocalMunicipality">Local Municipality/City</label><br />
<asp:DropDownList ID="ddlLocalMunicipality" runat="server" CssClass="form-control" AutoPostBack="True" />
                        <asp:RequiredFieldValidator ID="rfvLocal" runat="server" ControlToValidate="ddlLocalMunicipality" ErrorMessage="Local Municipality is required." Display="Dynamic" CssClass="text-danger" />

                    </td>
                </tr>
                <tr>
                    <td>
                        <label for="ddlWardNumber">Ward Number</label><br />
                 <asp:DropDownList ID="ddlWardNo" runat="server" CssClass="form-control" />
                         <asp:RequiredFieldValidator ID="RFVWardNo" runat="server" ControlToValidate="ddlWardNo" ErrorMessage="Ward Number is required." Display="Dynamic" CssClass="text-danger" />
                   
                    </td>
                    <td>
                        <label for="ddlSubscriptionType">Subscription Type</label><br />
                        <asp:DropDownList ID="ddlSubscriptionType" runat="server">
                            <asp:ListItem Value="0">Please Select</asp:ListItem>
                            <asp:ListItem Value="Donation">Donation</asp:ListItem>
                            <asp:ListItem Value="Renewal">Renewal</asp:ListItem>
                            <asp:ListItem Value="New Member">New Member</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <label for="txtResidentialAddress">Residential Address</label><br />
                        <asp:TextBox ID="txtResidentialAddress" runat="server" TextMode="MultiLine" ></asp:TextBox>
                    </td>
                    <td>
                        <label for="ddlLanguage">Language</label><br />
                        <asp:DropDownList ID="ddlLanguage" runat="server">
                            <asp:ListItem Value="0">Please Select</asp:ListItem>
   <asp:ListItem Value="1">Afrikaans</asp:ListItem>
    <asp:ListItem Value="2">English</asp:ListItem>
    <asp:ListItem Value="3">IsiNdebele</asp:ListItem>
    <asp:ListItem Value="4">IsiXhosa</asp:ListItem>
    <asp:ListItem Value="5">IsiZulu</asp:ListItem>
    <asp:ListItem Value="6">Sepedi (Northern Sotho)</asp:ListItem>
    <asp:ListItem Value="7">Sesotho</asp:ListItem>
    <asp:ListItem Value="8">Setswana</asp:ListItem>
    <asp:ListItem Value="9">SiSwati</asp:ListItem>
    <asp:ListItem Value="10">Tshivenda</asp:ListItem>
    <asp:ListItem Value="11">Xitsonga</asp:ListItem>
    <asp:ListItem Value="12">Sign Language</asp:ListItem>
    <asp:ListItem Value="13">Shona</asp:ListItem>
    <asp:ListItem Value="14">Portuguese</asp:ListItem>
    <asp:ListItem Value="15">Ndau</asp:ListItem>
                        </asp:DropDownList>
  <asp:RequiredFieldValidator ID="rfvLanguage" runat="server" ControlToValidate="ddllanguage" ErrorMessage="Language is required." Display="Dynamic" CssClass="text-danger" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label for="ddlGender">Gender</label><br />
                        <asp:DropDownList ID="ddlGender" runat="server">
                            <asp:ListItem Value="0">Please Select</asp:ListItem>
                            <asp:ListItem Value="Male">Male</asp:ListItem>
                            <asp:ListItem Value="Female">Female</asp:ListItem>
                        </asp:DropDownList>
                               <asp:RequiredFieldValidator ID="rfvGender" runat="server" ControlToValidate="ddlgender"
                            InitialValue="Select Gender" ErrorMessage="Gender is required." Display="Dynamic" CssClass="text-danger" />
                    </td>
                    <td>
                        <label for="ddlMembershipPeriod"> Membership Period</label><br />
                        <asp:DropDownList ID="ddlMembershipPeriod" runat="server" AutoPostBack="True">
                               <asp:ListItem Value="0">Please Select</asp:ListItem>
                            <asp:ListItem Value="1">1 Year</asp:ListItem>
                            <asp:ListItem Value="5">5 Years</asp:ListItem>
                            <asp:ListItem Value="10">10 Years</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <label for="txtExpiryDate">Expiry Date</label><br />
                        <asp:TextBox ID="txtExpiryDate" runat="server" Enabled="False"></asp:TextBox>
                    </td>
                    <td>
                        <label><asp:CheckBox ID="chkPaid" runat="server" /> Paid Membership</label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="btn-container">
                        <!-- Add Search Button -->
                       </td> <td class="btn-container">
                        <asp:Button ID="btnRegister" runat="server" Text="Register" OnClick="btnRegister_Click" />
                            </td> <td class="btn-container">
                        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="btn-container">
                        &nbsp;</td> <td class="btn-container">
                        &nbsp;</td> <td class="btn-container">
                        &nbsp;</td>
                </tr>
            </table>
        </div>

        <!-- GridView for displaying data -->
        <div class="gridview-container">
                 <asp:GridView ID="gvMembers" runat="server" AutoGenerateColumns="False" OnRowEditing="gvMembers_RowEditing" OnRowDeleting="gvMembers_RowDeleting" OnRowCancelingEdit="gvMembers_RowCancelingEdit" OnRowUpdating="gvMembers_RowUpdating" OnPageIndexChanging="gvMembers_PageIndexChanging" AllowPaging="True" PageSize="10">
                <Columns>
                    <asp:BoundField DataField="MembershipNo" HeaderText="Membership No"  ItemStyle-Width ="300px"/>
                    <asp:BoundField DataField="FirstName" HeaderText="Firstname"/>
                    <asp:BoundField DataField="LastName" HeaderText="Lastname"/>
                    <asp:BoundField DataField="IDNumber" HeaderText="ID Number"/>
                    <asp:BoundField DataField="CellNumber" HeaderText="Cell No"/>
                    <asp:BoundField DataField="Gender" HeaderText="Gender"/>
                     <asp:BoundField DataField="Languege" HeaderText="Gender" />
                    <asp:BoundField DataField="Province" HeaderText="Province"/>
                    <asp:BoundField DataField="District" HeaderText="District"/>
                     <asp:BoundField DataField="City_local" HeaderText="Local Municipality"/>
                     <asp:BoundField DataField="MembershipPeriod" HeaderText="Period"/>
                     <asp:BoundField DataField="RegisteredDate" HeaderText="Registered Date"  DataFormatString="{0:dd-MM-yyyy}" HtmlEncode="false" />
                     <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date" DataFormatString="{0:dd-MM-yyyy}" HtmlEncode="false" />
                     <asp:CommandField ShowEditButton="True" ItemStyle-Width ="50px"/>
                    <asp:CommandField ShowDeleteButton="True"  ItemStyle-Width ="50px"/>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
