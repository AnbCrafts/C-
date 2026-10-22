<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="User_CRUD_Non_ObjectDataSource.aspx.cs"
    Inherits="GridView_ObjectDataSource_Practice.User_CRUD_Non_ObjectDataSource" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Query Users CRUD</title>

    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 20px;
            background: #f4f6f9;
        }

        h2 {
            color: #2c3e50;
            margin-bottom: 20px;
        }

        .grid {
            width: 100%;
            border-collapse: collapse;
        }

        .grid th {
            background-color: #34495e;
            color: white;
            padding: 10px;
        }

        .grid td {
            padding: 8px;
            border: 1px solid #ddd;
        }

        .grid input[type=text] {
            width: 120px;
        }

        .action-btn {
            text-decoration: none;
            font-weight: bold;
            margin-right: 5px;
        }
    </style>

</head>
<body>

    <form id="form1" runat="server">

        <h2>Query Users Management (Without ObjectDataSource)</h2>

        <asp:GridView
            ID="gvUsers"
            runat="server"
            AutoGenerateColumns="False"
            DataKeyNames="UserId"
            CssClass="grid"
            ShowFooter="True"
            AllowPaging="True"
            PageSize="10"
            OnRowCommand="gvUsers_RowCommand"
           OnPageIndexChanging="gvUsers_PageIndexChanging" 
            >

            <Columns>

              

                <asp:TemplateField HeaderText="Actions">

                    <ItemTemplate>

                        <asp:LinkButton
                            ID="lbEdit"
                            runat="server"
                            Text="Edit"
                            CssClass="action-btn"
                            CommandName="EditRow"
                            CommandArgument='<%# Container.DataItemIndex %>' />

                        <asp:LinkButton
                            ID="lbDelete"
                            runat="server"
                            Text="Delete"
                            CssClass="action-btn"
                            CommandName="DeleteRow"
                            CommandArgument='<%# Eval("UserId") %>'
                            OnClientClick="return confirm('Delete this user?');" />

                    </ItemTemplate>

                    <EditItemTemplate>

                        <asp:LinkButton
                            ID="lbUpdate"
                            runat="server"
                            Text="Update"
                            CssClass="action-btn"
                            CommandName="UpdateRow"
                            CommandArgument='<%# Eval("UserId") %>' />

                        <asp:LinkButton
                            ID="lbCancel"
                            runat="server"
                            Text="Cancel"
                            CssClass="action-btn"
                            CommandName="CancelUpdate" />

                    </EditItemTemplate>

                </asp:TemplateField>

             

                <asp:TemplateField HeaderText="User ID">

                    <ItemTemplate>
                        <asp:Label
                            ID="lblUserId"
                            runat="server"
                            Text='<%# Eval("UserId") %>' />
                    </ItemTemplate>

                    <EditItemTemplate>
                        <asp:Label
                            ID="lblEditUserId"
                            runat="server"
                            Text='<%# Eval("UserId") %>' />
                    </EditItemTemplate>

                    <FooterTemplate>

                        <asp:LinkButton
                            ID="lbInsert"
                            runat="server"
                            Text="Insert User"
                            CommandName="InsertRow" />

                    </FooterTemplate>

                </asp:TemplateField>

             

                <asp:TemplateField HeaderText="User Name">

                    <ItemTemplate>
                        <%# Eval("UserName") %>
                    </ItemTemplate>

                    <EditItemTemplate>
                        <asp:TextBox
                            ID="txtUserName"
                            runat="server"
                            Text='<%# Bind("UserName") %>' />
                    </EditItemTemplate>

                    <FooterTemplate>
                        <asp:TextBox
                            ID="txtInsertUserName"
                            runat="server" />
                    </FooterTemplate>

                </asp:TemplateField>


                <asp:TemplateField HeaderText="Email">

                    <ItemTemplate>
                        <%# Eval("Email") %>
                    </ItemTemplate>

                    <EditItemTemplate>
                        <asp:TextBox
                            ID="txtEmail"
                            runat="server"
                            Text='<%# Bind("Email") %>' />
                    </EditItemTemplate>

                    <FooterTemplate>
                        <asp:TextBox
                            ID="txtInsertEmail"
                            runat="server" />
                    </FooterTemplate>

                </asp:TemplateField>


                <asp:TemplateField HeaderText="Full Name">

                    <ItemTemplate>
                        <%# Eval("FullName") %>
                    </ItemTemplate>

                    <EditItemTemplate>
                        <asp:TextBox
                            ID="txtFullName"
                            runat="server"
                            Text='<%# Bind("FullName") %>' />
                    </EditItemTemplate>

                    <FooterTemplate>
                        <asp:TextBox
                            ID="txtInsertFullName"
                            runat="server" />
                    </FooterTemplate>

                </asp:TemplateField>

               

                <asp:TemplateField HeaderText="Mobile">

                    <ItemTemplate>
                        <%# Eval("MobileNo") %>
                    </ItemTemplate>

                    <EditItemTemplate>
                        <asp:TextBox
                            ID="txtMobileNo"
                            runat="server"
                            Text='<%# Bind("MobileNo") %>' />
                    </EditItemTemplate>

                    <FooterTemplate>
                        <asp:TextBox
                            ID="txtInsertMobileNo"
                            runat="server" />
                    </FooterTemplate>

                </asp:TemplateField>


                <asp:TemplateField HeaderText="City">

                    <ItemTemplate>
                        <%# Eval("City") %>
                    </ItemTemplate>

                    <EditItemTemplate>
                        <asp:TextBox
                            ID="txtCity"
                            runat="server"
                            Text='<%# Bind("City") %>' />
                    </EditItemTemplate>

                    <FooterTemplate>
                        <asp:TextBox
                            ID="txtInsertCity"
                            runat="server" />
                    </FooterTemplate>

                </asp:TemplateField>

             

                <asp:TemplateField HeaderText="State">

                    <ItemTemplate>
                        <%# Eval("StateName") %>
                    </ItemTemplate>

                    <EditItemTemplate>
                        <asp:TextBox
                            ID="txtStateName"
                            runat="server"
                            Text='<%# Bind("StateName") %>' />
                    </EditItemTemplate>

                    <FooterTemplate>
                        <asp:TextBox
                            ID="txtInsertStateName"
                            runat="server" />
                    </FooterTemplate>

                </asp:TemplateField>

              

                <asp:TemplateField HeaderText="Country">

                    <ItemTemplate>
                        <%# Eval("Country") %>
                    </ItemTemplate>

                    <EditItemTemplate>
                        <asp:TextBox
                            ID="txtCountry"
                            runat="server"
                            Text='<%# Bind("Country") %>' />
                    </EditItemTemplate>

                    <FooterTemplate>
                        <asp:TextBox
                            ID="txtInsertCountry"
                            runat="server" />
                    </FooterTemplate>

                </asp:TemplateField>


                <asp:TemplateField HeaderText="Gender">

                    <ItemTemplate>
                        <%# Eval("Gender") %>
                    </ItemTemplate>

                    <EditItemTemplate>

                        <asp:DropDownList
                            ID="ddlGender"
                            runat="server"
                            SelectedValue='<%# Bind("Gender") %>'>

                            <asp:ListItem>Male</asp:ListItem>
                            <asp:ListItem>Female</asp:ListItem>

                        </asp:DropDownList>

                    </EditItemTemplate>

                    <FooterTemplate>

                        <asp:DropDownList
                            ID="ddlInsertGender"
                            runat="server">

                            <asp:ListItem>Male</asp:ListItem>
                            <asp:ListItem>Female</asp:ListItem>

                        </asp:DropDownList>

                    </FooterTemplate>

                </asp:TemplateField>

            

                <asp:TemplateField HeaderText="Query">

                    <ItemTemplate>
                        <%# Eval("QueryText") %>
                    </ItemTemplate>

                    <EditItemTemplate>
                        <asp:TextBox
                            ID="txtQueryText"
                            runat="server"
                            Text='<%# Bind("QueryText") %>' />
                    </EditItemTemplate>

                    <FooterTemplate>
                        <asp:TextBox
                            ID="txtInsertQueryText"
                            runat="server" />
                    </FooterTemplate>

                </asp:TemplateField>

        

                <asp:TemplateField HeaderText="Amount">

                    <ItemTemplate>
                        <%# Eval("QueryAmount") %>
                    </ItemTemplate>

                    <EditItemTemplate>
                        <asp:TextBox
                            ID="txtQueryAmount"
                            runat="server"
                            Text='<%# Bind("QueryAmount") %>' />
                    </EditItemTemplate>

                    <FooterTemplate>
                        <asp:TextBox
                            ID="txtInsertQueryAmount"
                            runat="server" />
                    </FooterTemplate>

                </asp:TemplateField>

               

                <asp:TemplateField HeaderText="Active">

                    <ItemTemplate>

                        <asp:CheckBox
                            ID="chkActive"
                            runat="server"
                            Enabled="false"
                            Checked='<%# Convert.ToBoolean(Eval("IsActive")) %>' />

                    </ItemTemplate>

                    <EditItemTemplate>

                        <asp:CheckBox
                            ID="chkEditActive"
                            runat="server"
                            Checked='<%# Bind("IsActive") %>' />

                    </EditItemTemplate>

                    <FooterTemplate>

                        <asp:CheckBox
                            ID="chkInsertActive"
                            runat="server"
                            Checked="true" />

                    </FooterTemplate>

                </asp:TemplateField>

            </Columns>

        </asp:GridView>

    </form>

</body>
</html>