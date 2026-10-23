<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="UserListDetailedDescription.aspx.cs"
    Inherits="GridView_ObjectDataSource_Practice.UserListDetailedDescription" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Detail</title>

    <style>
        body {
            font-family: 'Segoe UI', Arial, sans-serif;
            background-color: #f4f6f9;
            margin: 0;
            padding: 30px;
        }

        .container {
            max-width: 1200px;
            margin: auto;
            background: white;
            padding: 25px;
            border-radius: 10px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }

        h2 {
            margin-top: 0;
            color: #2c3e50;
            text-align: center;
        }

        .grid {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }

        .grid th {
            background-color: #34495e;
            color: white;
            padding: 12px;
            text-align: left;
        }

        .grid td {
            padding: 10px;
            border: 1px solid #ddd;
        }

        .grid tr:nth-child(even) {
            background-color: #f8f9fa;
        }

        .grid tr:hover {
            background-color: #eaf2ff;
        }

        .back-btn {
            display: inline-block;
            margin-bottom: 15px;
            padding: 8px 14px;
            background-color: #3498db;
            color: white;
            text-decoration: none;
            border-radius: 4px;
        }

        .back-btn:hover {
            background-color: #2980b9;
        }
    </style>
</head>
<body>
   <form id="form1" runat="server">

    <div class="container">

        <a href="UserListShortDetail.aspx" class="back-btn">
            ← Back to Users
        </a>

        <h2>User Details</h2>

        <asp:GridView
            ID="gvUserDetails"
            runat="server"
            AutoGenerateColumns="true"
            CssClass="grid"
            GridLines="None">
        </asp:GridView>

    </div>

</form>
</body>
</html>