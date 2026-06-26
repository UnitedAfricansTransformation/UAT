<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UATDatabase.aspx.vb" Inherits="NewMembership.UATDatabase" %>

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
        .btn-export {
    display: block;
    margin-bottom: 10px;
    padding: 8px 15px;
    background-color: darkseagreen;
    color: black;
    border: none;
    cursor: pointer;
    font-size: 14px;
    border-radius: 5px;
}

.btn-export:hover {
    background-color: darkolivegreen;
}
        .btn-container {
            text-align: center;
              margin-left: 30px;
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
    margin-top: 10px;
    margin-left: 100px;
    width: 90%;
    margin-bottom: 10px;
    color: black;
    background-color: white;
}

.gridview-container table {
    width: 100%;
    border-collapse: collapse; /* Ensures borders are properly aligned */
    table-layout: fixed; /* Ensures all columns respect defined widths */
}
   .gridview-container th {
            background-color: royalblue;
            color: white;
        /*    padding: 5px;*/
        }

.gridview-container th, .gridview-container td {
    padding: 5px; /* Reduced padding to remove excessive spacing */
    text-align: left;
    font-size: 13px;
    white-space: nowrap; /* Prevents unnecessary wrapping */
    overflow: hidden; /* Hides overflowing text */
    text-overflow: ellipsis; /* Adds '...' to cut-off text */
}

/* Specific column width adjustments */
.gridview-container th:nth-child(1), /* MembershipNo */
.gridview-container td:nth-child(1) {
    width: 100px; /* Adjust width for MembershipNo column */
}

/* Specific column width adjustments */
.gridview-container th:nth-child(7), /* District */
.gridview-container td:nth-child(7) {
    width: 200px; /* Adjust width for District column */
}

/* Specific column width adjustments */
.gridview-container th:nth-child(4), /* Cell Number */
.gridview-container td:nth-child(4) {
    width: 80px; /* Adjust width for Province */
}

/* Specific column width adjustments */
.gridview-container th:nth-child(5), /* Province */
.gridview-container td:nth-child(5) {
    width: 80px; /* Adjust width for Province */
}

.gridview-container th:nth-child(6), /* Local Municipality */
.gridview-container td:nth-child(6) {
    width: 300px; /* Adjust width for Local Municipality column */
}

.gridview-container th:nth-child(8), /* Ward Number */
.gridview-container td:nth-child(8) {
    width: 80px; /* Make Ward Number column smaller */
}

.gridview-container th:nth-child(11), /* Ward Number */
.gridview-container td:nth-child(11) {
    width: 80px; /* Make Ward Number column smaller */
}
.gridview-container th:last-child, /* Edit & Delete buttons */
.gridview-container td:last-child {
    width: 50px; /* Reduce the width of the last column (Edit/Delete) */
}

/* Pagination Styling */
.gridview-container .pagination {
    text-align: right;
/*    padding: 5px 0;*/
    margin-top: 5px;
}

.gridview-container .pagination a, 
.gridview-container .pagination span {
    display: inline-block;
    padding: 5px 8px;
    margin: 0 3px;
    text-decoration: none;
    background-color: #ddd;
    color: black;
    border-radius: 3px;
    font-size: 14px;
}

.gridview-container .pagination a:hover {
    background-color: royalblue;
    color: white;
}

.gridview-container .pagination span {
    background-color: royalblue;
    color: white;
    font-weight: bold;
}

.gridview-container .pagination {
    text-align: right;  /* Aligns page numbers to the right */
    /*padding: 5px 10px;*/
    /*margin-top: 5px;*/
    display: flex;
    justify-content: flex-end;  /* Ensures numbers are positioned closely to the right */
    gap: 1px; /* Reduces spacing between numbers */
}

/* Style for pagination links */
.gridview-container .pagination a, 
.gridview-container .pagination span {
    display: inline-block;
    padding: 5px 6px; /* Reduced padding to bring numbers closer */
    margin: 0 1px; /* Minimizes spacing between numbers */
    text-decoration: none;
    background-color: #ddd;
    color: black;
    border-radius: 3px;
    font-size: 14px;
    min-width: 25px;
    text-align: center;
}

/* Hover effect */
.gridview-container .pagination a:hover {
    background-color: royalblue;
    color: white;
}

/* Style for active page */
.gridview-container .pagination span {
    background-color: royalblue;
    color: white;
    font-weight: bold;
}
       /* .auto-style2 {
            height: 75px;
        }*/
        .auto-style3 {
            width: 195px;
            height: 167px;
        }
        .auto-style4 {
            color: #FF0000;
            font-size: small;
        }

        .btn-group {
    text-align: center;
    margin-top: 15px;
}

.btn-group input[type="submit"],
.btn-group input[type="reset"] {
    padding: 8px 15px;
    border: none;
    background-color: #0066cc;
    color: white;
    border-radius: 5px;
    cursor: pointer;
    font-size: 0.9em;
}

      .overlay {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0, 0, 0, 0.7);
            display: none;
            justify-content: center;
            align-items: center;
            z-index: 1000;
        }

     .overlay-content {
            background-color: white;
            padding: 20px;
            border-radius: 10px;
            text-align: center;
            max-width: 400px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);
        }

      .overlay-content h3 {
            margin: 0 0 10px;
        }
          .progress-bar-container {
            width: 100%;
            height: 20px;
            background-color: #f3f3f3;
            border-radius: 5px;
            margin: 20px 0;
            overflow: hidden;
        }

        .progress-bar {
            height: 100%;
            background-color: blue;
            width: 0%;
            transition: width 1s linear;
        }

    .overlay-close {
        margin-top: 15px;
        background-color: #cc0000;
        color: white;
        border: none;
        padding: 10px 20px;
        border-radius: 5px;
        cursor: pointer;
    }
        .overlay-close:disabled {
            background-color: #aaaaaa;
            cursor: not-allowed;
        }

.btn-group input[type="reset"] {
    background-color: #cc0000;
}

.text-danger {
    color: red;
    font-size: 0.8em;
}
        .auto-style5 {
            color: #CC0000;
        }
        .auto-style6 {
            color: #000099;
        }
        .auto-style9 {
            font-size: small;
        }
        .auto-style10 {
            height: 189px;
        }
        .auto-style11 {
            height: 40px;
        }
                .auto-style12 {
            text-align: right;
            font-size:14px;
        }
        </style>

     <script type="text/javascript">

         function ValidateMembershipFee(sender, args) {
             var annual = document.getElementById('<%= annualFee.ClientID %>');
               var fiveYear = document.getElementById('<%= fiveYearFee.ClientID %>');
               args.IsValid = (annual.checked || fiveYear.checked);
           }

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

       
             var idleTime = 0;
             var idleInterval = setInterval(timerIncrement, 60000); // 1 minute

             document.onmousemove = resetTimer;
             document.onkeypress = resetTimer;

             function timerIncrement() {
                 idleTime += 1;
        if (idleTime >= 30) { // 30 minutes
                 window.location.href = "Login.aspx";
        }
    }

             function resetTimer() {
                 idleTime = 0;
    }
     </script>
 
</head>
<body style="left: 0px; top: 0px; height:100%" >
    <form id="form1" runat="server">
         <div id="overlay" class="overlay">
       <div class="overlay-content">

           <h3>Processing your registration...</h3>
           <p>Please wait while we validate your information.</p>
           <div class="progress-bar-container">
               <div id="progressBar" class="progress-bar"></div>
           </div>
           <button id="closeButton" class="overlay-close" onclick="closeOverlay()" disabled="disabled">Close</button>
       </div>
   </div>
        <div class="form-container">
         
            <table>
                <tr >
                    <td class="auto-style10">  <img alt="" class="auto-style3" src="Images/Logo.jpeg" /></td>
             
                    <td colspan="3" style="padding:10px; vertical-align:middle; font-size:large " class="auto-style10"> <h2 class="auto-style5">UNITED AFRICANS TRANSFORMATION (UAT)</h2> </td>
                </tr>
                <tr style="background-color:cornflowerblue; ">
                    <td colspan="4" style="text-align: center;" class="auto-style11">
                        <asp:Button ID="btnMemberReport" runat="server" Height="35px" Text="Go to UAT Membership Report" CausesValidation="False" OnClick="btnMemberReport_Click" />
                         </td>
                    
                </tr>
                <tr style="background-color:white;">
                    <td colspan="4" style="text-align: right;" class="auto-style12">
<asp:Label ID="lblname" runat="server" Text="" ForeColor="Black" Font-Size="Medium"></asp:Label>
<asp:LinkButton ID="lnkLogout" runat="server" OnClick="lnkLogout_Click" Text="Logout Here" />
                       </td>
                   
                </tr>
            <tr>
           <td colspan ="4"><hr />
               </td>
       </tr>
               <tr>
         <td> <strong> <asp:Label ID="Label1" runat="server" Text="Search Member by ID Number" width="250px" CssClass="form-control" Style="font-size:16px; color:#000066;"></asp:Label></strong></td>
             <td colspan="2">
                 <asp:TextBox ID="txtSearchIDNo" runat="server"  CssClass="form-control" Width="200px"></asp:TextBox> <asp:Button ID="btnSearch" runat="server" Text="Search Member" CssClass="form-control" />
                   </td> <td>
                     <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="https://www.checkid.co.za/" Text="Click here to verify South African ID Number" Target="_blank" CssClass="auto-style9"> </asp:HyperLink>
 </td>
     </tr>
                       <tr>
           <td colspan ="4"><hr />
               </td>
       </tr>
                <tr>
                    <td colspan ="4">
                         <h3 style="font-size:medium; text-align:left" class="auto-style6">UAT Membership Form - New Registration/Renewal</h3>
                        <asp:Label ID="lblMessage" runat="server" Text="" CssClass="auto-style4"></asp:Label>
                        </td>
                </tr>
                <tr>
                    <td>
                        <label for="txtMemberNo">Membership No</label><br />
                        <asp:TextBox ID="txtMemberNo" runat="server" Enabled="False"></asp:TextBox>
                    </td>
                    <td>
                      <label for="txtFirstName"> FirstName:</label><br /><asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" />
                    </td>
                    <td>
              <label for="txtLastName">Surname:</label><br /><asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" />
                    </td>
                    <td>
  <label for="idNumber">ID Number:</label><br /><asp:TextBox ID="txtIDNumber" runat="server" TextMode="Number" CssClass="form-control" oninput="validateIDNumber()" AutoPostBack="True"/>
     <span id="error-msg" style="color: red;"></span>
     <asp:RegularExpressionValidator ID="revIDNumber" runat="server" ControlToValidate="txtIDNumber"
         ValidationExpression="^\d{13}$" ErrorMessage="Enter a valid 13-digit South African ID number." Display="Dynamic" CssClass="text-danger" style="color: #FF0000; font-size: xx-small" />
  <asp:TextBox ID="txtAge" runat="server" TextMode="Number" CssClass="form-control" Visible="False"/>
                     
                    </td>
                </tr>
                <tr>
                    <td>
                        <label for="txtCellNo">Cell Number</label><br />
                        <asp:TextBox ID="txtCellNumber" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <label for="ddlProvince">Province</label><br />
                        <asp:DropDownList ID="ddlProvince" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlProvince_SelectedIndexChanged" />
                        <asp:RequiredFieldValidator ID="rfvProvince" runat="server" ControlToValidate="ddlProvince" ErrorMessage="Province is required." Display="Dynamic" CssClass="text-danger" style="color: #FF0000" />
                    </td>
                    <td>
                        <label for="ddlDistrictMunicipality">District Municipality</label><br />
                      <asp:DropDownList ID="ddlDistrictMunicipality" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlDistrictMunicipality_SelectedIndexChanged"/>
                        <asp:RequiredFieldValidator ID="rfvDistrict" runat="server" ControlToValidate="ddlDistrictMunicipality" ErrorMessage="District Municipality is required." Display="Dynamic" CssClass="text-danger" style="color: #FF0000" />

                    </td>
                    <td>
                        <label for="ddlLocalMunicipality">Local Municipality/City</label><br />
<asp:DropDownList ID="ddlLocalMunicipality" runat="server" CssClass="form-control" AutoPostBack="True" />
                        <asp:RequiredFieldValidator ID="rfvLocal" runat="server" ControlToValidate="ddlLocalMunicipality" ErrorMessage="Local Municipality is required." Display="Dynamic" CssClass="text-danger" style="color: #FF0000" />

                    </td>
                </tr>
                <tr>
                    <td>
                             <label for="ddlLocation" width="300px">Location/ City</label><br /><asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvLocation" runat="server" ControlToValidate="ddlLocation" ErrorMessage="Location is required." Display="Dynamic" CssClass="text-danger" style="color: #FF0000" />
                   </td>
                    <td>
                                 <label for="ddlWardNumber">Ward Number</label><br />
                 <asp:DropDownList ID="ddlWardNo" runat="server" CssClass="form-control" />
                         <asp:RequiredFieldValidator ID="RFVWardNo" runat="server" ControlToValidate="ddlWardNo" ErrorMessage="Ward Number is required." Display="Dynamic" CssClass="text-danger" style="color: #FF0000" />
                   </td>
                    <td>
                              <label for="ddlGender">Gender</label><br />
                        <asp:DropDownList ID="ddlGender" runat="server">
                            <asp:ListItem Value="0">Please Select</asp:ListItem>
                            <asp:ListItem Value="Male">Male</asp:ListItem>
                            <asp:ListItem Value="Female">Female</asp:ListItem>
                        </asp:DropDownList>
                               <asp:RequiredFieldValidator ID="rfvGender" runat="server" ControlToValidate="ddlgender"
                            InitialValue="Select Gender" ErrorMessage="Gender is required." Display="Dynamic" CssClass="text-danger" style="color: #FF0000" /></td>
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
  <asp:RequiredFieldValidator ID="rfvLanguage" runat="server" ControlToValidate="ddllanguage" ErrorMessage="Language is required." Display="Dynamic" CssClass="text-danger" style="color: #FF0000" />
                   </td>
                </tr>
                <tr>
                    <td>                    <label for="postalCode">University/ College /Tertiary Students:</label><br /><asp:DropDownList ID="ddlUniversity" runat="server" CssClass="form-control" />
                     <br />
  
               
                    </td>
                    <td>
                                                  <label for="email">Email Address:</label><br /><asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                        
            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ErrorMessage="Enter a valid email address." Display="Dynamic" CssClass="text-danger" style="color: #FF0000" /> 
                         </td>
                    <td>
                        <label for="txtResidentialAddress">Residential Address</label><br />
                       
                        <asp:TextBox ID="txtResidentialAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Width="90%" />
                    </td>
                    <td>
                          <strong style="font-size:12px">Tick Membership Subscription:</strong><br />
            <asp:RadioButton ID="annualFee" runat="server" GroupName="MembershipFee" Text="R20 - Annual Fee" style="font-size: 11px;" OnClientClick="setAmount(20)" />
            <br />
            <asp:RadioButton ID="fiveYearFee" runat="server" GroupName="MembershipFee" Text="R100 - (5) Years Fee" style="font-size: 11px;" OnClientClick="setAmount(100)" /><br />
                 <asp:CustomValidator ID="cvMembershipFee" runat="server"  ErrorMessage="Please select a membership fee Paid." ForeColor="Red" ClientValidationFunction="ValidateMembershipFee"  Display="Dynamic"></asp:CustomValidator> </td>
                </tr>
                <tr>
                    <td>
             Disability:<br /><strong><asp:CheckBox ID="ckDisability" Text="Person Living with Disability" runat="server" CssClass="auto-style12"/>
                    </td>
                    <td>
                        <label for="subscription">Subscription Type:</label><br /><asp:DropDownList ID="ddlSubscriptionType" runat="server" CssClass="form-control">
                            <asp:ListItem>Select Subscription</asp:ListItem>
                          <%--<asp:ListItem>Donation</asp:ListItem>--%>
                            <asp:ListItem>Renewal</asp:ListItem>
                            <asp:ListItem>New Registration</asp:ListItem>
                        </asp:DropDownList>
              <asp:RequiredFieldValidator ID="RFVSubscribe" runat="server" ControlToValidate="ddlSubscriptionType"
                            InitialValue="Select Subscription Type:" ErrorMessage="Subscription Type is required." Display="Dynamic" CssClass="text-danger" style="color: #FF0000" />
                    </td>
                    <td>
                        <label for="txtExpiryDate">Expiry Date</label><br />
                        <asp:TextBox ID="txtExpiryDate" runat="server" Enabled="False"></asp:TextBox>
                    </td>
                    <td>
                        <br />
                        <label><asp:CheckBox ID="chkPaid" runat="server" /> &nbsp;Membership is Paid</label>
                    </td>
                </tr>
                <tr>
                    <td> </td> <td colspan="2"  class="btn-container">
                        <asp:Button ID="btnRegister" runat="server" Text="Submit" OnClick="btnRegister_Click" />
                        <asp:Button ID ="btnUpdateMembership" runat="server" BackColor="Red" visible="false" Text="Renew Membership" OnClick="btnUpdateMembership_Click" />
                            </td>


                    <td class="btn-container">
                        <%--<asp:ListItem>Donation</asp:ListItem>--%>
                        <asp:HiddenField ID="hdnExist" runat="server" />
                            </td>
                </tr>
              
                <tr>
                    <td colspan="2">
                        <%--<asp:ListItem>Donation</asp:ListItem>--%>&nbsp;<asp:HiddenField ID="hdnUATCategory" runat="server" />
                    </td> <td class="btn-container">
                        <asp:HiddenField ID="hdnUATSTructure" runat="server" />
                    </td> <td class="btn-container">
                        <asp:HiddenField ID="hdnMembershipNo" runat="server" />
                    </td>
                </tr>
            </table>
        </div>

       
    </form>
</body>
</html>
