<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Secretary.Master" CodeBehind="MembershipReport.aspx.vb" Inherits="NewMembership.MembershipReport" MaintainScrollPositionOnPostBack="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <style type="text/css">

    .search-panel {
        width: 700px;
        border: 1px solid #007acc;
        background-color: #f2f2f2;
        border-radius: 8px;
        padding: 15px;
        font-family: Arial, sans-serif;
    }

    .search-panel th, .search-panel td {
        padding: 10px;
        vertical-align: middle;
    }

    .search-panel-header {
        font-size: 18px;
        font-weight: bold;
        color: white;
        background-color: #007acc;
        padding: 10px;
        border-radius: 5px 5px 0 0;
    }

    .label-styles {
        font-weight: bold;
        color: #333;
    }

    .dropdown-list {
        width: 250px;
        padding: 5px;
        border-radius: 4px;
        border: 1px solid #ccc;
    }

    .search-panel-row {
        background-color: #ffffff;
        border-bottom: 1px solid #ccc;
    }

    .search-panel-row:hover {
        background-color: #e6f2ff;
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

        .header-button {
            background-color: #5d6168;
            color: #ffffff;
            border: 1px solid #4a4d55;
            border-radius: 4px;
            padding: 8px 16px;
            cursor: pointer;
        }
       
   .gridview-container {
    margin-top: 10px;
    margin-left: 5px;
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
    width: 50px; /* Adjust width for MembershipNo column */
}

.gridview-container th:nth-child(2), /* MembershipNo */
.gridview-container td:nth-child(2) {
    width: 100px; /* Adjust width for MembershipNo column */
}
.gridview-container th:nth-child(3), /* MembershipNo */
.gridview-container td:nth-child(3) {
    width: 150px; /* Adjust width for MembershipNo column */
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
    width: 100px; /* Adjust width for Local Municipality column */
}

/* Specific column width adjustments */
.gridview-container th:nth-child(7), /* District */
.gridview-container td:nth-child(7) {
    width: 200px; /* Adjust width for District column */
}

.gridview-container th:nth-child(8), /* Ward Number */
.gridview-container td:nth-child(8) {
    width: 200px; /* Make Ward Number column smaller */
}

.gridview-container th:nth-child(9), /* Local Municipality */
.gridview-container td:nth-child(9) {
    width: 50px; /* Adjust width for Local Municipality column */
}

.gridview-container th:nth-child(10), /* Local Municipality */
.gridview-container td:nth-child(10) {
    width: 200px; /* Adjust width for Local Municipality column */
}

.gridview-container th:nth-child(11), /* Ward Number */
.gridview-container td:nth-child(11) {
    width: 100px; /* Make Ward Number column smaller */
}
.gridview-container th:last-child, /* Edit & Delete buttons */
.gridview-container td:last-child {
    width: 100px; /* Reduce the width of the last column (Edit/Delete) */
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
         .label-styles {
     margin-left: 200px;
     padding-left:200px;
 }
        .auto-style7 {
            text-align: center;
            color: darkblue;
        }
        .auto-style8 {
            color: #ffffff;
        }
        
.form-control {
    width: 100%;
    padding: 8px;
    border: 1px solid #ccc;
    border-radius: 5px;
    box-sizing: border-box;
    font-size: 0.9em;
}

            .auto-style10 {
                color: #000066;
            }
            .auto-style11 {
                text-align: left;
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
          
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
             <div style="vertical-align:middle; color:red; font-size:18px; font-weight:700; margin-left:30%">
                 <h2 class="auto-style11">
        <asp:Label ID="lblName" runat="server" Font-Bold="True" Font-Size="Large" ForeColor="#000066" CssClass="auto-style10"></asp:Label>
                 </h2>
                 <h2 class="auto-style11">
        <asp:Label ID="Label2" runat="server" Text="United Africans Transformation(UAT) Membership Report" Font-Bold="True" Font-Size="Large" ForeColor="#C00C27"></asp:Label>
                 </h2>
  </div>
             <div style="display: flex; background-color:#7b7f84; justify-content: space-between; align-items: center; width: 100%; margin: 30px 50px 10px 50px;">
    <!-- Left column: Capture Membership -->
    <div>
              <asp:Button ID="btnCaptureMembership" runat="server" Text="Capture New Membership" CssClass="clickable header-button" />

    </div>

    <!-- Right column: Export to Excel -->
    <div  style="margin-right:100px">
        <asp:Button ID="btnRegister" runat="server" PostBackUrl="~/Register.aspx" Text="Register Secretary" Visible="False" CssClass="clickable header-button" />
        <asp:Button ID="Button1" runat="server" Text="Export to Excel" OnClick="Button1_Click" Visible="false" CssClass="clickable header-button" />
        <asp:LinkButton ID="lnkLogout" runat="server" OnClick="lnkLogout_Click" Text="Logout" />
    </div>
</div>
      
        
       <table style="width: 600px;" class="search-panel" align="center">
            <tr>
                <td colspan="2" class="auto-style7"> <strong>SEARCH UAT MEMBERSHIP OPTIONS</strong></td>
     
            </tr>
            <tr>
                <td> <strong> <asp:Label ID="Label1" runat="server" ForeColor="#ffffff" Text="Select Search Criteria" CssClass="auto-style8"></asp:Label></strong></td>

                <td><asp:DropDownList ID="drdSearchCriteria" runat="server" CssClass="dropdown-list" Width="250px" AutoPostBack="true">
                    <asp:ListItem Text="-- Select Search Criteria--" Value="" Selected="True" />
<%--    <asp:ListItem Text="Province" Value="Province"></asp:ListItem>
    <asp:ListItem Text="District" Value="District"></asp:ListItem>
    <asp:ListItem Text="Local" Value="Local"></asp:ListItem>--%>
    <asp:ListItem Text="ID Number" Value="IDNumber"></asp:ListItem>
    <asp:ListItem Text="Gender" Value="Gender"></asp:ListItem>
  <asp:ListItem Text="Structure/League" Value="Structure"></asp:ListItem>
    <asp:ListItem Text="Membership Number" Value="MembershipNumber"></asp:ListItem>
</asp:DropDownList></td>
            </tr>
            <tr>
                <td>
                    <strong>
                    <asp:Label ID="lblSearch" ForeColor="#666666" runat="server" Text=""></asp:Label>
                    </strong>
                </td>
                <td><%--    <asp:ListItem Text="Province" Value="Province"></asp:ListItem>
    <asp:ListItem Text="District" Value="District"></asp:ListItem>
    <asp:ListItem Text="Local" Value="Local"></asp:ListItem>--%>
                    <asp:DropDownList ID="ddlgender" runat="server" CssClass="dropdown-list" Visible="false">
                            <asp:ListItem>--Select Gender--</asp:ListItem>
                            <asp:ListItem>Male</asp:ListItem>
                            <asp:ListItem>Female</asp:ListItem>
                        </asp:DropDownList>
                    <asp:DropDownList ID="ddlStructure" runat="server" CssClass="dropdown-list" Visible="false">
                            <asp:ListItem>--Select Leage--</asp:ListItem>
                            <asp:ListItem>PDF</asp:ListItem>
                          <asp:ListItem>PL HIV</asp:ListItem>
                            <asp:ListItem>Tomorrows Future</asp:ListItem>
                          <asp:ListItem>Mother Nature</asp:ListItem>
                        </asp:DropDownList>
                    <br />
                    <asp:TextBox ID="txtIDNumber" runat="server" TextMode="Number" CssClass="form-control" Width="250px" Visible="false" oninput="validateIDNumber()"/>
                    <asp:TextBox ID="txtMembership" runat="server" CssClass="form-control" Width="250px" Visible="false"></asp:TextBox>
                </td>
       
 
            </tr>
           <tr>
               <td></td>
               <td>
                   <asp:Button ID="btnSearch" runat="server" Text="Search" />
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
           <div class="gridview-container">
            
                 <asp:GridView ID="gvMembers" runat="server" AutoGenerateColumns="False" OnPageIndexChanging="gvMembers_PageIndexChanging">
                <Columns>
                         <asp:TemplateField HeaderText="No." ItemStyle-Width="40px">
            <ItemTemplate>
                <asp:Label ID="lblRowNumber" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
                    <asp:BoundField DataField="MembershipNo" HeaderText="Membership No"  ItemStyle-Width ="80px"/>
                    <asp:BoundField DataField="FirstName" HeaderText="Fullnames"  ItemStyle-Width="150px"/>
                    <asp:BoundField DataField="LastName" HeaderText="Lastname"  ItemStyle-Width="150px"/>
                    <asp:BoundField DataField="CellNumber" HeaderText="Cell Number" ItemStyle-Width ="100px"/>
                    <asp:BoundField DataField="Province" HeaderText="Province"  ItemStyle-Width="150px"/>
                    <asp:BoundField DataField="DistrictMunicipality" HeaderText="District" />
                     <asp:BoundField DataField="LocalMunicipality" HeaderText="Local Municipality"/>
                     <asp:BoundField DataField="WardNo" HeaderText="Ward Number" ItemStyle-Width="90px"/>
                     <asp:BoundField DataField="LocationName" HeaderText="Location"  ItemStyle-Width="120px"/>
                     <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date" DataFormatString="{0:dd-MM-yyyy}" HtmlEncode="false" ItemStyle-Width="100px" />

                </Columns>
            </asp:GridView>
     </div>
</asp:Content>
