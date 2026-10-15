<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserList.aspx.cs"
    Inherits="WebServicePractice.UserList" %>


<!DOCTYPE html>
<html>
<head runat="server">
    <title>All Users</title>

    <style>
        body {
            font-family: Arial, sans-serif;
            background: #f4f6f9;
            padding: 30px;
        }

        .container {
            width: 90%;
            margin: auto;
            background: white;
            padding: 25px;
            border-radius: 10px;
            box-shadow: 0px 0px 10px rgba(0,0,0,.1);
        }

        h2 {
            text-align: center;
            color: #333;
        }

        .grid {
            margin-top: 20px;
            width: 100%;
        }

        .btnLoad {
            background: #0078d7;
            color: white;
            border: none;
            padding: 10px 20px;
            cursor: pointer;
            border-radius: 5px;
        }

        .btnLoad:hover {
            background: #005bb5;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <div class="container">

            <h2>All Registered Users</h2>

            <asp:Button ID="btnLoad"
                runat="server"
                Text="Load Users"
                CssClass="btnLoad"
                OnClick="btnLoad_Click" />

            <br />
            <br />

            <asp:GridView ID="gvUsers"
                runat="server"
                CssClass="grid"
                AutoGenerateColumns="true"
                CellPadding="6"
                BorderWidth="1"
                GridLines="Both">
            </asp:GridView>

        </div>

    </form>
</body>
</html>