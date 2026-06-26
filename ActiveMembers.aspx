<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ActiveMembers.aspx.vb" Inherits="NewMembership.ActiveMembers" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>Inactive Members</title>

    <style>

       <head runat="server">
    <title>Inactive Members</title>

    <style>

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
            border-collapse: collapse;
            table-layout: fixed;
            margin-left: 0px;
        }

        .gridview-container th {
            background-color: royalblue;
            color: white;
        }

        .gridview-container th,
        .gridview-container td {
            padding: 5px;
            text-align: left;
            font-size: 13px;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
        }

        .gridview-container tr:nth-child(even) {
            background-color: #f2f2f2;
        }

        .gridview-container tr:hover {
            background-color: #ddd;
        }

    </style>

</head>

    </style>
</head>
<body>
    <form id="form1" runat="server">
<div class="form-container">

    <h2>Inactive Members</h2>

    <div class="gridview-container">

        <asp:GridView ID="gvMembers"
            runat="server"
            AutoGenerateColumns="False"
            AllowPaging="True"
            GridLines="None" CellPadding="4" ForeColor="#333333">

            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />

            <Columns>

                <asp:BoundField DataField="MembershipNo" HeaderText="Membership No" />
                <asp:BoundField DataField="FirstName" HeaderText="First Name" />
                <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                <asp:BoundField DataField="IDNumber" HeaderText="ID Number" />
                <asp:BoundField DataField="Age" HeaderText="Age" />
                <asp:BoundField DataField="PreferredLanguage" HeaderText="Language" />
                <asp:BoundField DataField="Gender" HeaderText="Gender" />
                <asp:BoundField DataField="SubscriptionType" HeaderText="Subscription" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:BoundField DataField="CellNumber" HeaderText="Cell Number" />
                <asp:BoundField DataField="Province" HeaderText="Province" />
                  <asp:BoundField DataField="SubscriptionDate" HeaderText="Subscription Date" />
                <asp:BoundField DataField="RenewalDate" HeaderText="Renewal Date" />
                <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date" />

            </Columns>

            <EditRowStyle BackColor="#999999" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#E9E7E2" />
            <SortedAscendingHeaderStyle BackColor="#506C8C" />
            <SortedDescendingCellStyle BackColor="#FFFDF8" />
            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />

        </asp:GridView>

    </div>

</div>
    </form>
</body>
</html>
