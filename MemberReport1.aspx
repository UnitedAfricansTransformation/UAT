<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MemberReport1.aspx.vb" Inherits="NewMembership.MemberReport1" MaintainScrollPositionOnPostBack="true" %>
<%@ Register Assembly="System.Web.DataVisualization" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Membership Report</title>
       <style>
        .report-table {
            width: 100%;
            border-collapse: collapse;
        }
        .report-table th, .report-table td {
            border: 1px solid #ddd;
            padding: 8px;
        }
        .report-table th {
            background-color: #f2f2f2;
            text-align: left;
        }
        .chart-container {
            margin-top: 20px;
        }
/* General Form Styles */
form {
    max-width: 1200px;
    margin: 20px auto;
    padding: 20px;
    background-color: #f9f9f9;
    border: 1px solid #ddd;
    border-radius: 8px;
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    font-size:8px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

/* Filter Fields Styles */
div {
    display: flex;
    flex-wrap: wrap;
    gap: 15px;
    margin-bottom: 20px;
}
.dropdown-list, .check-box, .button {
    padding: 8px;
    border-radius: 4px;
    border: 1px solid #ccc;
    font-size: 14px;
    min-width: 200px;
}

.check-box {
    align-self: center;
}

.button {
    background-color: #0078d4;
    color: #fff;
    border: none;
    cursor: pointer;
    font-weight: bold;
    transition: background-color 0.3s;
}

.button:hover {
    background-color: #005ea2;
}

/* GridView Styles */
.grid-view {
    width: 100%;
    border-collapse: collapse;
    margin-top: 20px;
}

.grid-view th {
    background-color: #0078d4;
    color: white;
    text-align: left;
    /*padding: 10px;*/
    font-weight: bold;
}

.grid-view td {
    /*padding: 10px;*/
    border-bottom: 1px solid #ddd;
}

.grid-view tr:nth-child(even) {
    background-color: #f2f2f2;
}

.grid-view tr:hover {
    background-color: #f1f1f1;
}

.grid-view .report-table {
    font-size: 14px;
    border: 1px solid #ddd;
    border-radius: 4px;
}

/* Responsive Design */
@media (max-width: 768px) {
    div {
        flex-direction: column;
    }
    .dropdown-list, .button {
        min-width: 100%;
    }
}

    </style>
</head>
<body>
    <div style="text-align:center; background-color: #0078D4; color: white; padding: 15px; border-radius: 8px; box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);">
    <h2 style="margin: 0; font-size: 24px; font-weight: bold;">UAT Membership Report</h2>
</div>
    <form id="form1" runat="server">
      

          <h3>Membership Status Per District Municipaility</h3>
          <div class="chart-container">
      <asp:Chart ID="chartDistrict" runat="server" Width="800px" Height="500px" style="border:hidden">
        <ChartAreas>
            <asp:ChartArea Name="ChartArea1">
                <AxisX>
                    <LabelStyle Angle="0" />
                    <MajorGrid LineColor="LightGray" LineDashStyle="Solid" /> 
                </AxisX>
                <AxisY>
                    <MajorGrid LineColor="Transparent" />
                </AxisY>
               
                <Area3DStyle Enable3D="false" />
            </asp:ChartArea>
        </ChartAreas>
        <Series>
            <asp:Series Name="TotalMembers" ChartType="Column" ChartArea="ChartArea1">
             
          <%--      <IsValueShownAsLabel="true" /> 
                <LabelForeColor="Black" /> --%>
            </asp:Series>
        </Series>
        <Legends>
            <asp:Legend Name="Legend1" Enabled="true"></asp:Legend>
        </Legends>
    </asp:Chart>
        </div>
        <%--<asp:GridView ID="gvMembership" runat="server" AutoGenerateColumns="True" CssClass="report-table"></asp:GridView>--%>
               <div>
            <!-- Filter Fields -->
            <asp:DropDownList ID="ddlProvince" runat="server" CssClass="dropdown-list" AutoPostBack="true"></asp:DropDownList>
            <asp:DropDownList ID="ddlDistrict" runat="server" CssClass="dropdown-list" AutoPostBack="true"></asp:DropDownList>
            <asp:DropDownList ID="ddlLocal" runat="server" CssClass="dropdown-list" AutoPostBack="true"></asp:DropDownList>
            <asp:DropDownList ID="ddlStructure" runat="server" CssClass="dropdown-list" AutoPostBack="true"></asp:DropDownList>
            <asp:DropDownList ID="ddlDisability" runat="server" CssClass="dropdown-list" AutoPostBack="true"></asp:DropDownList>
            <asp:DropDownList ID="ddlSubscriptionType" runat="server" CssClass="dropdown-list" AutoPostBack="true"></asp:DropDownList>
            <asp:DropDownList ID="ddlUniversity" runat="server" CssClass="dropdown-list" AutoPostBack="true"></asp:DropDownList>
            <asp:CheckBox ID="chkInactiveMembers" runat="server" CssClass="check-box" Text="Inactive Members" AutoPostBack="true" />

            <!-- Button to Filter -->
            <asp:Button ID="btnFilter" runat="server" Text="Search Report" CssClass="button" OnClick="btnFilter_Click" />
                   <br /><br />
            <!-- GridView to Display Membership Data -->
            <asp:GridView ID="gvMembershipReport" runat="server" CssClass="grid-view" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="MembershipNo" HeaderText="Membership No" />
                    <asp:BoundField DataField="FirstName" HeaderText="First Name" />
                    <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                     <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:BoundField DataField="CellNumber" HeaderText="Cell Number" />
                     <asp:BoundField DataField="Province" HeaderText="Province" />
                    <asp:BoundField DataField="DistrictMunicipality" HeaderText="District Municipality" />
                    <asp:BoundField DataField="LocalMunicipality" HeaderText="Local Municipality" />
                    <asp:BoundField DataField="WardNo" HeaderText="Ward No" />
                    <asp:BoundField DataField="University" HeaderText="University" />
                         <asp:BoundField DataField="UATStructure" HeaderText="Structure" />
                          <asp:BoundField DataField="SubscriptionType" HeaderText="Subscription Type" />
                    <asp:BoundField DataField="SubscriptionDate" HeaderText="Subscription Date" />
                  <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date" />
                </Columns>
            </asp:GridView>
        </div>
        

    </form>
</body>
</html>
