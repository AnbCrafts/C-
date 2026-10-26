<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LinqCrudPractice.aspx.cs" Inherits="LINQPractice.LinqCrudPractice" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>LINQ CRUD Practice</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 20px; }
        .table-responsive { width: 100%; overflow-x: auto; }
        .grid { width: 100%; border-collapse: collapse; font-size: 13px; }
        .grid th { background-color: #2c3e50; color: white; padding: 8px; text-align: left; white-space: nowrap; }
        .grid td { border: 1px solid #ddd; padding: 6px; }
        .grid tr:hover { background-color: #f9f9f9; }
        .grid input[type="text"] { width: 90%; padding: 4px; box-sizing: border-box; }
        .btn-action { margin-right: 5px; text-decoration: none; font-weight: bold; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="table-responsive">
            <h2>User Management (LINQ CRUD)</h2>

            <asp:GridView ID="gvUsers" runat="server" CssClass="grid" 
                ShowFooter="true" AllowPaging="true" PageSize="10" AutoGenerateColumns="false" 
                DataKeyNames="UserId" 
                OnPageIndexChanging="gvDetails_PageIndexChanging" 
                OnRowCancelingEdit="gvDetails_RowCancelingEdit"
                OnRowEditing="gvDetails_RowEditing" 
                OnRowUpdating="gvDetails_RowUpdating" 
                OnRowDeleting="gvDetails_RowDeleting" 
                OnRowCommand="gvDetails_RowCommand" 
                OnRowDataBound="gvDetails_RowDataBound">

                <Columns>
                    <%-- 1. UserId (Read-only) --%>
                    <asp:TemplateField HeaderText="ID">
                        <ItemTemplate>
                            <asp:Label ID="lblUserId" runat="server" Text='<%# Eval("UserId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- 2. UserName --%>
                    <asp:TemplateField HeaderText="Username">
                        <ItemTemplate>
                            <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserName") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditUserName" runat="server" Text='<%# Bind("UserName") %>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtNewUserName" runat="server" Placeholder="Username" />
                        </FooterTemplate>
                    </asp:TemplateField>

                    <%-- 3. Email --%>
                    <asp:TemplateField HeaderText="Email">
                        <ItemTemplate>
                            <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditEmail" runat="server" Text='<%# Bind("Email") %>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtNewEmail" runat="server" Placeholder="Email" />
                        </FooterTemplate>
                    </asp:TemplateField>

                    <%-- 4. FullName --%>
                    <asp:TemplateField HeaderText="Full Name">
                        <ItemTemplate>
                            <asp:Label ID="lblFullName" runat="server" Text='<%# Eval("FullName") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditFullName" runat="server" Text='<%# Bind("FullName") %>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtNewFullName" runat="server" Placeholder="Full Name" />
                        </FooterTemplate>
                    </asp:TemplateField>

                    <%-- 5. MobileNo --%>
                    <asp:TemplateField HeaderText="Mobile No">
                        <ItemTemplate>
                            <asp:Label ID="lblMobileNo" runat="server" Text='<%# Eval("MobileNo") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditMobileNo" runat="server" Text='<%# Bind("MobileNo") %>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtNewMobileNo" runat="server" Placeholder="Mobile No" />
                        </FooterTemplate>
                    </asp:TemplateField>

                    <%-- 6. City --%>
                    <asp:TemplateField HeaderText="City">
                        <ItemTemplate>
                            <asp:Label ID="lblCity" runat="server" Text='<%# Eval("City") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditCity" runat="server" Text='<%# Bind("City") %>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtNewCity" runat="server" Placeholder="City" />
                        </FooterTemplate>
                    </asp:TemplateField>

                    <%-- 7. StateName --%>
                    <asp:TemplateField HeaderText="State">
                        <ItemTemplate>
                            <asp:Label ID="lblStateName" runat="server" Text='<%# Eval("StateName") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditStateName" runat="server" Text='<%# Bind("StateName") %>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtNewStateName" runat="server" Placeholder="State" />
                        </FooterTemplate>
                    </asp:TemplateField>

                    <%-- 8. Country --%>
                    <asp:TemplateField HeaderText="Country">
                        <ItemTemplate>
                            <asp:Label ID="lblCountry" runat="server" Text='<%# Eval("Country") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditCountry" runat="server" Text='<%# Bind("Country") %>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtNewCountry" runat="server" Placeholder="Country" />
                        </FooterTemplate>
                    </asp:TemplateField>

                    <%-- 9. Gender --%>
                    <asp:TemplateField HeaderText="Gender">
                        <ItemTemplate>
                            <asp:Label ID="lblGender" runat="server" Text='<%# Eval("Gender") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlEditGender" runat="server">
                                <asp:ListItem Text="Select" Value="" />
                                <asp:ListItem Text="Male" Value="Male" />
                                <asp:ListItem Text="Female" Value="Female" />
                                <asp:ListItem Text="Other" Value="Other" />
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddlNewGender" runat="server">
                                <asp:ListItem Text="Select" Value="" />
                                <asp:ListItem Text="Male" Value="Male" />
                                <asp:ListItem Text="Female" Value="Female" />
                                <asp:ListItem Text="Other" Value="Other" />
                            </asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <%-- 10. QueryText --%>
                    <asp:TemplateField HeaderText="Query Text">
                        <ItemTemplate>
                            <asp:Label ID="lblQueryText" runat="server" Text='<%# Eval("QueryText") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditQueryText" runat="server" Text='<%# Bind("QueryText") %>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtNewQueryText" runat="server" Placeholder="Query Text" />
                        </FooterTemplate>
                    </asp:TemplateField>

                    <%-- 11. IsActive (BIT / bool) --%>
                    <asp:TemplateField HeaderText="Active">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkIsActive" runat="server" Checked='<%# Eval("IsActive") != null && Convert.ToBoolean(Eval("IsActive")) %>' Enabled="false" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBox ID="chkEditIsActive" runat="server" Checked='<%# Eval("IsActive") != null && Convert.ToBoolean(Eval("IsActive")) %>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:CheckBox ID="chkNewIsActive" runat="server" Checked="true" />
                        </FooterTemplate>
                    </asp:TemplateField>

                    <%-- 12. QueryAmount (DECIMAL) --%>
                    <asp:TemplateField HeaderText="Amount">
                        <ItemTemplate>
                            <asp:Label ID="lblQueryAmount" runat="server" Text='<%# Eval("QueryAmount", "{0:C2}") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditQueryAmount" runat="server" Text='<%# Bind("QueryAmount") %>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtNewQueryAmount" runat="server" Placeholder="0.00" />
                        </FooterTemplate>
                    </asp:TemplateField>

                    <%-- Actions Column --%>
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text="Edit" CssClass="btn-action" />
                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" Text="Delete" CssClass="btn-action"
                                OnClientClick="return confirm('Are you sure you want to delete this user?');" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" Text="Update" CssClass="btn-action" />
                            <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" CssClass="btn-action" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:LinkButton ID="btnAdd" runat="server" CommandName="AddNew" Text="Add New" CssClass="btn-action" ForeColor="Green" />
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>

            </asp:GridView>
        </div>
    </form>
</body>
</html>