<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="CRUD_User.aspx.cs"
    Inherits="GridView_ObjectDataSource_Practice.CRUD_User" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CRUD Users</title>

    <style>
        body {
            font-family: Arial;
            margin: 20px;
            background-color: #f5f7fa;
        }

        .header {
            color: #2c3e50;
            margin-bottom: 20px;
        }

        .grid {
            width: 100%;
            margin-top: 20px;
        }

        .insert-panel {
            background: white;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 0 8px #ddd;
            margin-bottom: 20px;
        }

        .detailsview {
            width: 100%;
        }

        .detailsview td {
            padding: 8px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <h2 class="header">Query Users Management</h2>

        <asp:ObjectDataSource
            ID="odsUsers"
            runat="server"
            TypeName="GridView_ObjectDataSource_Practice.QueryUserDAL"
            SelectMethod="GetAllUsers"
            UpdateMethod="UpdateUser"
            DeleteMethod="DeleteUserById"
            InsertMethod="InsertUser">

            <DeleteParameters>
                <asp:Parameter Name="userId" Type="Int32" />
            </DeleteParameters>

            <UpdateParameters>
                <asp:Parameter Name="userId" Type="Int32" />
                <asp:Parameter Name="userName" Type="String" />
                <asp:Parameter Name="email" Type="String" />
                <asp:Parameter Name="fullName" Type="String" />
                <asp:Parameter Name="mobileNo" Type="String" />
                <asp:Parameter Name="city" Type="String" />
                <asp:Parameter Name="stateName" Type="String" />
                <asp:Parameter Name="country" Type="String" />
                <asp:Parameter Name="gender" Type="String" />
                <asp:Parameter Name="queryText" Type="String" />
                <asp:Parameter Name="queryAmount" Type="Decimal" />
                <asp:Parameter Name="isActive" Type="Boolean" />
            </UpdateParameters>

            <InsertParameters>
    <asp:Parameter Name="UserName" Type="String" />
    <asp:Parameter Name="Email" Type="String" />
    <asp:Parameter Name="FullName" Type="String" />
    <asp:Parameter Name="MobileNo" Type="String" />
    <asp:Parameter Name="City" Type="String" />
    <asp:Parameter Name="StateName" Type="String" />
    <asp:Parameter Name="Country" Type="String" />
    <asp:Parameter Name="Gender" Type="String" />
    <asp:Parameter Name="QueryText" Type="String" />
    <asp:Parameter Name="QueryAmount" Type="Decimal" />
</InsertParameters>

        </asp:ObjectDataSource>

        <!-- INSERT FORM -->
        <div class="insert-panel">

           <asp:DetailsView
    ID="dvInsertUser"
    runat="server"
    DataSourceID="odsUsers"
    DefaultMode="Insert"
    AutoGenerateRows="False"
    CssClass="detailsview">

    <Fields>

        <asp:BoundField DataField="UserName" HeaderText="User Name" />
        <asp:BoundField DataField="Email" HeaderText="Email" />
        <asp:BoundField DataField="FullName" HeaderText="Full Name" />
        <asp:BoundField DataField="MobileNo" HeaderText="Mobile No" />
        <asp:BoundField DataField="City" HeaderText="City" />
        <asp:BoundField DataField="StateName" HeaderText="State" />
        <asp:BoundField DataField="Country" HeaderText="Country" />
        <asp:BoundField DataField="Gender" HeaderText="Gender" />
        <asp:BoundField DataField="QueryText" HeaderText="Query" />
        <asp:BoundField DataField="QueryAmount" HeaderText="Amount" />

        <asp:CommandField
            ShowInsertButton="True"
            InsertText="Add User" />

    </Fields>

</asp:DetailsView>

        </div>

        <!-- GRIDVIEW -->

        <asp:GridView
            ID="gvUsers"
            runat="server"
            DataSourceID="odsUsers"
            AutoGenerateColumns="False"
            DataKeyNames="UserId"
            AllowPaging="True"
            PageSize="10"
            CssClass="grid">

            <Columns>

                <asp:CommandField
                    ShowEditButton="True"
                    ShowDeleteButton="True" />

                <asp:BoundField
                    DataField="UserId"
                    HeaderText="ID"
                    ReadOnly="True" />

                <asp:BoundField
                    DataField="UserName"
                    HeaderText="User Name" />

                <asp:BoundField
                    DataField="Email"
                    HeaderText="Email" />

                <asp:BoundField
                    DataField="FullName"
                    HeaderText="Full Name" />

                <asp:BoundField
                    DataField="MobileNo"
                    HeaderText="Mobile No" />

                <asp:BoundField
                    DataField="City"
                    HeaderText="City" />

                <asp:BoundField
                    DataField="StateName"
                    HeaderText="State" />

                <asp:BoundField
                    DataField="Country"
                    HeaderText="Country" />

                <asp:BoundField
                    DataField="Gender"
                    HeaderText="Gender" />

                <asp:BoundField
                    DataField="QueryText"
                    HeaderText="Query" />

                <asp:BoundField
                    DataField="QueryAmount"
                    HeaderText="Amount" />

                <asp:CheckBoxField
                    DataField="IsActive"
                    HeaderText="Active" />

            </Columns>

            <HeaderStyle
                BackColor="#34495E"
                ForeColor="White"
                Font-Bold="True" />

            <RowStyle BackColor="#F4F4F4" />
            <AlternatingRowStyle BackColor="#EAEAEA" />

        </asp:GridView>

    </form>
</body>
</html>