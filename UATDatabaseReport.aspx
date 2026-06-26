<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UATDatabaseReport.aspx.vb" Inherits="NewMembership.UATDatabaseReport" MaintainScrollPositionOnPostBack="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>UAT Membership Registration</title>
    <%--<link href="Styles/Layout.css" rel="stylesheet" />--%>
    <style type="text/css">

    .search-panel {
        width: 700px;
        border: 1px solid #172a57;
        background-color: #172a57;
        border-radius: 8px;
        padding: 15px;
        font-family: Arial, sans-serif;
        color: #ffffff;
    }

    .search-panel th, .search-panel td {
        padding: 10px;
        vertical-align: middle;
        color: #ffffff;
    }

    .search-panel-header {
        font-size: 18px;
        font-weight: bold;
        color: white;
        background-color: #172a57;
        padding: 10px;
        border-radius: 5px 5px 0 0;
    }

    .label-styles {
        font-weight: bold;
        color: #ffffff;
    }

    .dropdown-list {
        width: 250px !important;
        max-width: 250px;
        display: inline-block;
        box-sizing: border-box;
        padding: 5px;
        border-radius: 4px;
        border: 1px solid #4a4d55;
        background-color: #5d6168;
        color: #ffffff;
        font-weight: bold;
        vertical-align: middle;
        transition: none;
    }

    .dropdown-list option {
        font-weight: bold;
        color: #ffffff;
        background-color: #5d6168;
    }

    .search-panel label {
        color: #ffffff;
    }

    .dropdown-list.hover-listbox,
    .absolute-dropdown {
        position: absolute;
        top: 0;
        left: 0;
        width: 250px !important;
        z-index: 999;
    }

    .search-panel-row {
        background-color: transparent;
        border-bottom: 1px solid rgba(255,255,255,0.25);
    }

    .search-panel-row:hover {
        background-color: rgba(255,255,255,0.08);
    }

        body {
            background-color: snow;
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

        .search-panel input[type="text"],
        .search-panel input[type="number"],
        .search-panel select {
            color: #ffffff;
            background-color: #5d6168;
            border: 1px solid #4a4d55;
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
        .search-button {
            cursor: pointer;
            background-color: #5d6168;
            color: #ffffff;
            border: 1px solid #4a4d55;
            font-weight: bold;
        }

        .header-button {
            cursor: pointer;
            background-color: #172a57;
            color: #ffffff;
            border: 1px solid #172a57;
            border-radius: 4px;
            padding: 6px 14px;
            font-weight: bold;
        }

        .header-title {
            color: #9aa0a6;
            font-weight: bold;
            font-size: 18px;
            white-space: nowrap;
            background-color: #172a57;
            padding: 10px 18px;
            border-radius: 6px;
        }
 .auto-style3 {
     width: 195px;
     height: 167px;
 }
 .clickable, .dropdown-list {
     cursor: pointer;
     color: #ffffff;
 }
 .auto-style4 {
     color: #FF0000;
     font-size: small;
 }

         .label-styles {
     margin-left: 200px;
     padding-left:200px;
 }
                .auto-style5 {
           color: #CC0000;
       }
        .auto-style7 {
            text-align: center;
            color: darkblue;
        }
        .auto-style8 {
            color: #ffffff;
        
.form-control {
    width: 100%;
    padding: 8px;
    border: 1px solid #ccc;
    border-radius: 5px;
    box-sizing: border-box;
    font-size: 0.9em;
}

        .auto-style10 {
            text-align: right;
            font-size:14px;
            color:black;
        }

        .user-welcome {
            color: #172a57;
            font-weight: bold;
        }

        </style>

     <script type="text/javascript">
        function showDropdown(select) {
            select.classList.add('hover-listbox');
            select.size = select.options.length;
        }
        function hideDropdown(select) {
            select.classList.remove('hover-listbox');
            select.size = 1;
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
                    errorMsg = "Invalid ID Number.";
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

    
             var idleTime = 0;
             var idleInterval = setInterval(timerIncrement, 60000); // 1 minute

             document.onmousemove = resetTimer;
             document.onkeypress = resetTimer;

             function timerIncrement() {
                 idleTime += 1;
        if (idleTime >= 10) { // 10 minutes
                 window.location.href = "Login.aspx";
        }
    }

             function resetTimer() {
                 idleTime = 0;
    }
    
          
     </script>
</head>
<body style="left: 0px; top: 0px; height:100%" width="100%" >
    <form id="form1" runat="server">
         <div class="form-container">

            
        
           <table>
               <tr>
                   <td><a href="Default.aspx" title="Home"><img alt="UAT logo" class="auto-style3 clickable" src="Images/Logo.jpeg" /></a></td>
                   <td colspan="3" style="padding:10px; vertical-align:middle; font-size:large "></td>
               </tr>
               <tr style="background-color:#ffffff;">
                   <td colspan="4">
                       <div style="display:flex; justify-content:space-between; align-items:center; width:100%; gap:20px;">
                           <div class="header-title">
                               WELCOME TO UNITED AFRICANS TRANSFORMATION MEMBERS PORTAL
                           </div>
                           <div style="display:flex; align-items:center; gap:10px; flex-wrap:wrap;">
                               <asp:Button ID="btnCaptureMembership" runat="server" Text="Capture New Membership" Height="35px" CssClass="clickable header-button" />
                               <asp:Button ID="btnRegister" runat="server" PostBackUrl="~/Register.aspx"
                                   Text="Register Secretary" Visible="False" Height="35px" CssClass="clickable header-button" />
                               <asp:Button ID="Button1" runat="server" Text="Export to Excel"
                                   OnClick="Button1_Click" Height="35px" CssClass="clickable header-button" />
                           </div>
                       </div>
                   </td>
               </tr>
 <%--                       <div>
           <asp:Button ID="btnCaptureMembership" runat="server" Text="Capture New Membership" height="35px" />

 </div>

 <!-- Right column: Export to Excel -->
 <div  style="text-align:right">
     <asp:Button ID="btnRegister" runat="server" PostBackUrl="~/Register.aspx" Text="Register Secretary" Visible="False"  height="35px" />
     <asp:Button ID="Button1" runat="server" Text="Export to Excel" OnClick="Button1_Click"  height="35px" />
 </div>--%>
                       </td>
                  
               </tr>
               <tr style="background-color:white;">
                   <td colspan="4" style="text-align: right;" class="auto-style10">
                                <asp:Label ID="lblname" runat="server" Text="" CssClass="user-welcome"></asp:Label>
<asp:LinkButton ID="lnkLogout" runat="server" OnClick="lnkLogout_Click" Text="Logout Here" CssClass="clickable" />
                     </td>
                  
               </tr>
           <tr>
          <td colspan ="4"><hr />
              </td>
               </tr></table>
        
       <table style="width: 600px;" class="search-panel" align="center">
            <tr>
                <td colspan="2" class="auto-style7"> <strong>SEARCH UAT MEMBERSHIP OPTIONS</strong></td>
     
            </tr>
            <tr>
                <td> <strong> <asp:Label ID="Label1" runat="server" ForeColor="#ffffff" Text="Select Search Criteria" CssClass="auto-style8"></asp:Label></strong></td>

                <td style="position:relative; min-height:38px;"><asp:DropDownList ID="drdSearchCriteria" runat="server" CssClass="dropdown-list" Width="250px" AutoPostBack="true" onmouseover="showDropdown(this)" onmouseout="hideDropdown(this)" onblur="hideDropdown(this)">
                    <asp:ListItem Text="-- Select Search Criteria--" Value="" Selected="True" />
    <asp:ListItem Text="Province" Value="Province"></asp:ListItem>
    <asp:ListItem Text="District" Value="District"></asp:ListItem>
    <asp:ListItem Text="Local" Value="Local"></asp:ListItem>
    <asp:ListItem Text="ID Number" Value="IDNumber"></asp:ListItem>
    <asp:ListItem Text="Gender" Value="Gender"></asp:ListItem>
  <asp:ListItem Text="Structure/League" Value="Structure"></asp:ListItem>
    <asp:ListItem Text="Membership Number" Value="MembershipNumber"></asp:ListItem>
     <asp:ListItem Text="Active Members " Value="ActiveMembers"></asp:ListItem>


</asp:DropDownList></td>
            </tr>
            <tr>
                <td>
                    <strong>
                    <asp:Label ID="lblSearch" ForeColor="#666666" runat="server" Text=""></asp:Label>
                    <br />
                    </strong>
                    <asp:HiddenField ID="hdnProvince" runat="server" />
                    <asp:HiddenField ID="hdnDistrict" runat="server" />
                    <asp:HiddenField ID="hdnUserTitle" runat="server" />
                </td>
                <td style="position:relative; min-height:50px;">  <asp:DropDownList ID="ddlProvince" runat="server" CssClass="dropdown-list absolute-dropdown" AutoPostBack="true" Width = "250px" Visible="false"></asp:DropDownList>
                    <asp:DropDownList ID="ddlDistrictMunicipality" runat="server" CssClass="dropdown-list absolute-dropdown" AutoPostBack="true" Visible="false" Width = "250px"/>
                    <asp:DropDownList ID="ddlLocalMunicipality" runat="server" CssClass="dropdown-list absolute-dropdown" AutoPostBack="true" Visible="false" Width = "250px"/>
                    <asp:DropDownList ID="ddlgender" runat="server" CssClass="dropdown-list absolute-dropdown" Visible="false">
                            <asp:ListItem>--Select Gender--</asp:ListItem>
                            <asp:ListItem>Male</asp:ListItem>
                            <asp:ListItem>Female</asp:ListItem>
                        </asp:DropDownList>
                    <asp:DropDownList ID="ddlStructure" runat="server" CssClass="dropdown-list absolute-dropdown" Visible="false">
                            <asp:ListItem>--Select Leage--</asp:ListItem>
                            <asp:ListItem>PDF</asp:ListItem>
                          <asp:ListItem>PL HIV</asp:ListItem>
                            <asp:ListItem>Tomorrows Future</asp:ListItem>
                          <asp:ListItem>Mother Nature</asp:ListItem>
                        </asp:DropDownList>
                    <br />
                    <asp:TextBox ID="txtIDNumber" runat="server" TextMode="Number" CssClass="form-control" Width="250px" Visible="false" oninput="validateIDNumber()"/>
                    <asp:TextBox ID="txtMembership" runat="server" CssClass="form-control" Width="250px" Visible="false"></asp:TextBox>
                    <asp:DropDownList ID="ActiveMembers" runat="server" CssClass="dropdown-list absolute-dropdown" AutoPostBack="true" Width = "250px" Visible="false"></asp:DropDownList>
                     <asp:DropDownList ID="InactiveMembers" runat="server" CssClass="dropdown-list absolute-dropdown" AutoPostBack="true" Width = "250px" Visible="false"></asp:DropDownList>


                </td>
       
 
            </tr>
           <tr>
               <td>
                   <asp:HiddenField ID="hdnDistrictID" runat="server" />
               </td>
               <td>
                   <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="search-button clickable" />
               </td>
           </tr>
        </table>
        
        <br />
        <br />
         <strong>
        <asp:Label ID="lblTotal" runat="server" style="color: #0000CC; padding-left:100px; margin-left:100px;"></asp:Label>
          </strong>
          <br />
  <br />
        <!-- GridView for displaying data -->
         <div style="overflow-x:auto; width:100%;">
       <asp:GridView ID="gvMembers" runat="server"
        AutoGenerateColumns="False" DataKeyNames="MembershipNo" 
            GridLines="Both"
        CellPadding="5"
           Width="100%"
        OnPageIndexChanging="gvMembers_PageIndexChanging">
        <Columns>
            <asp:TemplateField HeaderText="No.">
                <ItemTemplate>
                    <%# Container.DataItemIndex + 1 %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" width="40px"/>
                <HeaderStyle HorizontalAlign="Center" width="40px"/>
            </asp:TemplateField>

            <asp:BoundField DataField="MembershipNo" HeaderText="Membership No"> <ItemStyle Width="100px" />  <HeaderStyle Width="100px"/></asp:BoundField>
            <asp:BoundField DataField="FirstName" HeaderText="First Name"> <ItemStyle Width="150px" />  <HeaderStyle Width="150px"/></asp:BoundField>
            <asp:BoundField DataField="LastName" HeaderText="Last Name"> <ItemStyle Width="100px" />  <HeaderStyle Width="100px"/></asp:BoundField>
            <asp:BoundField DataField="IDNumber" HeaderText="ID Number" Visible="false" />
            <asp:BoundField DataField="PreferredLanguage" HeaderText="Language" Visible="false" />
            <asp:BoundField DataField="CellNumber" HeaderText="Cell Number"> <ItemStyle Width="100px" />  <HeaderStyle Width="100px"/></asp:BoundField>
            <asp:BoundField DataField="Province" HeaderText="Province"> <ItemStyle Width="200px" />  <HeaderStyle Width="200px"/></asp:BoundField>
            <asp:BoundField DataField="DistrictMunicipality" HeaderText="District Municipality"> <ItemStyle Width="250px" />  <HeaderStyle Width="250px"/></asp:BoundField>
            <asp:BoundField DataField="RegisteredVoter" HeaderText="Registered Voter" Visible="false" />
            <asp:BoundField DataField="LocalMunicipality" HeaderText="Local Municipality">  
                <ItemStyle Width="250px" /> <HeaderStyle HorizontalAlign="Center" width="250px"/>
</asp:BoundField>
            <asp:BoundField DataField="WardNo" HeaderText="Ward No">  <ItemStyle Width="80px" /> <HeaderStyle HorizontalAlign="Center" width="80px"/>
</asp:BoundField>
            <asp:BoundField DataField="LocationName" HeaderText="Location">  
                <ItemStyle Width="200px" /> <HeaderStyle HorizontalAlign="Center" width="200px"/>
</asp:BoundField>
            <asp:BoundField DataField="MembershipFee" HeaderText="Membership Fee" > <ItemStyle Width="120px" />  <HeaderStyle Width="120px"/></asp:BoundField>
            <asp:BoundField DataField="PaidMembership" HeaderText="Paid">  <ItemStyle Width="40px" /> <HeaderStyle HorizontalAlign="Center" width="40px"/>
</asp:BoundField>


            <asp:BoundField DataField="Registration" HeaderText="Registration Type"> <ItemStyle Width="120px" />  <HeaderStyle Width="120px"/></asp:BoundField>
            <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date" DataFormatString="{0:dd-MMM-yyyy}" >  <ItemStyle Width="90px" /> <HeaderStyle HorizontalAlign="Center" width="90px"/> </asp:BoundField>
        </Columns>

        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White"  Font-Size="13px" />
        <RowStyle ForeColor="#000066" Font-Size="12px" />
        <AlternatingRowStyle BackColor="#F5F5F5" />
        <EmptyDataTemplate>
            <div style="text-align:center; color:red;">No membership records found.</div>
        </EmptyDataTemplate>
    </asp:GridView>
</div>
</div>
    </form>
</body>
</html>
