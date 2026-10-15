<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserPage.aspx.cs" Inherits="URLRouting.UserPage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Search</title>

    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f7fa;
        }

        .container {
            width: 450px;
            margin: 80px auto;
            padding: 25px;
            background-color: white;
            border-radius: 10px;
            box-shadow: 0 0 12px rgba(0,0,0,0.15);
        }

        h2 {
            text-align: center;
            color: #333;
        }

        .form-group {
            margin-bottom: 15px;
        }

        label {
            display: block;
            margin-bottom: 5px;
            font-weight: bold;
        }

        .textbox {
            width: 100%;
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 5px;
        }

        .btn-search {
            width: 100%;
            padding: 10px;
            background-color: #0078D7;
            color: white;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-size: 16px;
        }

        .btn-search:hover {
            background-color: #005a9e;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">

            <h2>Search User</h2>

            <div class="form-group">
                <label for="txtUsername">Username</label>
                <asp:TextBox ID="txtUsername"
                    runat="server"
                    CssClass="textbox"
                    placeholder="Enter username">
                </asp:TextBox>
            </div>

            <div class="form-group">
                <label for="txtEmail">Email</label>
                <asp:TextBox ID="txtEmail"
                    runat="server"
                    CssClass="textbox"
                    TextMode="Email"
                    placeholder="Enter email">
                </asp:TextBox>
            </div>

            <asp:Button ID="btnSearch"
                runat="server"
                Text="Search"
                CssClass="btn-search"
                OnClick="btnSearch_Click" />

        </div>
        <div class="form-group">
    <label>Status Message</label>

    <asp:TextBox
        ID="txtMessage"
        runat="server"
        TextMode="MultiLine"
        Rows="4"
        Width="100%"
        ReadOnly="true"
        BackColor="#e8f5e9"
        ForeColor="#2e7d32"
        BorderColor="#81c784">
    </asp:TextBox>
</div>
    </form>
</body>
</html>