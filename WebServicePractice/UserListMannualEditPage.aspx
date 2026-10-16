<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="UserListMannualEditPage.aspx.cs"
    Inherits="WebServicePractice.UserListMannualEditPage" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Manual CRUD GridView</title>

    <style>
        body {
            font-family: Arial;
            background: #f4f6f9;
            padding: 30px;
        }

        .container {
            width: 95%;
            margin: auto;
            background: white;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0px 0px 8px #ccc;
        }

        .grid {
            width: 100%;
            margin-top: 20px;
        }

        .txtBox {
            width: 95%;
            padding: 5px;
        }

        .btn {
            padding: 5px 10px;
            border: none;
            border-radius: 4px;
            color: white;
            cursor: pointer;
        }

        .editBtn {
            background: #f39c12;
        }

        .updateBtn {
            background: green;
        }

        .deleteBtn {
            background: red;
        }

        .cancelBtn {
            background: gray;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">

        <div class="container">

            <h2>Manual CRUD GridView</h2>

            <asp:Button ID="btnLoad"
                runat="server"
                Text="Load Users"
                OnClick="btnLoad_Click" />

            <br />
            <br />

            <asp:GridView ID="gvUsers"
                runat="server"
                CssClass="grid"
                AutoGenerateColumns="false"
                AllowPaging="true"
                PageSize="10"
                DataKeyNames="UserId"
                OnPageIndexChanging="gvUsers_PageIndexChanging"
                OnRowEditing="gvUsers_RowEditing"
                OnRowCancelingEdit="gvUsers_RowCancelingEdit"
                OnRowUpdating="gvUsers_RowUpdating"
                OnRowDeleting="gvUsers_RowDeleting"
                >

                <Columns>

                    <asp:BoundField DataField="UserId"
                        HeaderText="User ID"
                        ReadOnly="true" />

                    <asp:TemplateField HeaderText="User Name">

                        <ItemTemplate>
                            <%# Eval("UserName") %>
                        </ItemTemplate>

                        <EditItemTemplate>
                            <asp:TextBox ID="txtUserName"
                                runat="server"
                                CssClass="txtBox"
                                Text='<%# Bind("UserName") %>' />
                        </EditItemTemplate>

                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Email">

                        <ItemTemplate>
                            <%# Eval("Email") %>
                        </ItemTemplate>

                        <EditItemTemplate>
                            <asp:TextBox ID="txtEmail"
                                runat="server"
                                CssClass="txtBox"
                                Text='<%# Bind("Email") %>' />
                        </EditItemTemplate>

                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Full Name">

                        <ItemTemplate>
                            <%# Eval("FullName") %>
                        </ItemTemplate>

                        <EditItemTemplate>
                            <asp:TextBox ID="txtFullName"
                                runat="server"
                                CssClass="txtBox"
                                Text='<%# Bind("FullName") %>' />
                        </EditItemTemplate>

                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Mobile">

                        <ItemTemplate>
                            <%# Eval("MobileNo") %>
                        </ItemTemplate>

                        <EditItemTemplate>
                            <asp:TextBox ID="txtMobile"
                                runat="server"
                                CssClass="txtBox"
                                Text='<%# Bind("MobileNo") %>' />
                        </EditItemTemplate>

                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="City">

                        <ItemTemplate>
                            <%# Eval("City") %>
                        </ItemTemplate>

                        <EditItemTemplate>
                            <asp:TextBox ID="txtCity"
                                runat="server"
                                CssClass="txtBox"
                                Text='<%# Bind("City") %>' />
                        </EditItemTemplate>

                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Created Date">

                        <ItemTemplate>
                            <%# Convert.ToDateTime(Eval("CreatedDate")).ToString("dd-MM-yyyy") %>
                        </ItemTemplate>

                    </asp:TemplateField>

                    <asp:CommandField
                        ShowEditButton="true"
                        HeaderText="Edit" />

                    <asp:CommandField
                        ShowDeleteButton="true"
                        HeaderText="Delete" />

                </Columns>

                <HeaderStyle BackColor="#0078d7"
                    ForeColor="White"
                    Font-Bold="true" />

                <AlternatingRowStyle BackColor="#f2f2f2" />

                <PagerStyle BackColor="#0078d7"
                    ForeColor="White" />

            </asp:GridView>

        </div>

    </form>
</body>
</html>