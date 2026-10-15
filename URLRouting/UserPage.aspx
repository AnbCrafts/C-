<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserPage.aspx.cs" Inherits="URLRouting.UserPage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Search</title>

    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f7fa;
            margin: 0;
            padding: 0;
        }

        .container {
            width: 480px;
            margin: 80px auto;
            padding: 30px;
            background-color: #ffffff;
            border-radius: 10px;
            box-shadow: 0 4px 15px rgba(0,0,0,0.1);
        }

        h2 {
            text-align: center;
            color: #0078D7;
            margin-top: 0;
            margin-bottom: 25px;
        }

        .form-group {
            margin-bottom: 18px;
        }

        label {
            display: block;
            margin-bottom: 6px;
            font-weight: bold;
            color: #333333;
        }

        .textbox {
            width: 100%;
            padding: 10px 12px;
            border: 1px solid #ccc;
            border-radius: 5px;
            box-sizing: border-box;
            font-size: 14px;
        }

        .textbox:focus {
            border-color: #0078D7;
            outline: none;
        }

        .btn-group {
            display: flex;
            gap: 12px;
            margin-top: 20px;
        }

        .btn-search {
            flex: 1;
            padding: 11px;
            background-color: #0078D7;
            color: white;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-size: 15px;
            font-weight: bold;
            text-align: center;
        }

        .btn-search:hover {
            background-color: #005a9e;
        }

        .btn-viewall {
            flex: 1;
            padding: 11px;
            background-color: #28a745;
            color: white;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-size: 15px;
            font-weight: bold;
            text-align: center;
        }

        .btn-viewall:hover {
            background-color: #218838;
        }

        .msg-box {
            margin-top: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>User Search</h2>

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
                    placeholder="Enter email">
                </asp:TextBox>
            </div>

            <div class="btn-group">
                <asp:Button ID="btnSearch"
                    runat="server"
                    Text="Search"
                    CssClass="btn-search"
                    OnClick="btnSearch_Click" />

                <asp:Button ID="btnViewAll"
                    runat="server"
                    Text="View All Users"
                    CssClass="btn-viewall"
                    OnClick="btnViewAll_Click" />
            </div>

            <div class="form-group msg-box">
                <label>Status Message</label>
                <asp:TextBox
                    ID="txtMessage"
                    runat="server"
                    TextMode="MultiLine"
                    Rows="3"
                    Width="100%"
                    ReadOnly="true"
                    BackColor="#e8f5e9"
                    ForeColor="#2e7d32"
                    BorderColor="#81c784">
                </asp:TextBox>
            </div>
        </div>
    </form>
</body>
</html>