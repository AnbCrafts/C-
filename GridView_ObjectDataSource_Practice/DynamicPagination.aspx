<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="DynamicPagination.aspx.cs"
    Inherits="GridView_ObjectDataSource_Practice.DynamicPagination" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dynamic Pagination</title>

    <style>
        body {
            font-family: Arial;
            margin: 20px;
            background-color: #f4f6f9;
        }

        h2 {
            color: #2c3e50;
        }

        .grid {
            width: 100%;
            border-collapse: collapse;
        }

        .grid th {
            background: #34495e;
            color: white;
            padding: 10px;
        }

        .grid td {
            padding: 8px;
            border: 1px solid #ddd;
        }

        .pager {
            margin-top: 20px;
            text-align: center;
        }

        .pager input[type=text] {
            width: 60px;
            text-align: center;
        }

        .page-info {
            font-weight: bold;
            margin-left: 10px;
            margin-right: 10px;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">

        <h2>Dynamic Pagination Using Stored Procedure</h2>

        <asp:GridView
            ID="gvUsers"
            runat="server"
            CssClass="grid"
            AutoGenerateColumns="true">
        </asp:GridView>

        <div class="pager">

            <asp:Button
                ID="btnPrevious"
                runat="server"
                Text="Previous"
                OnClick="btnPrevious_Click" />

            <asp:Label
                ID="lblPageInfo"
                runat="server"
                CssClass="page-info" />

            <asp:Button
                ID="btnNext"
                runat="server"
                Text="Next"
                OnClick="btnNext_Click" />

            &nbsp;&nbsp;&nbsp;

            Go To Page :

            <asp:TextBox
                ID="txtPageNo"
                runat="server">
            </asp:TextBox>

            <asp:Button
                ID="btnGo"
                runat="server"
                Text="Go"
                OnClick="btnGo_Click" />

        </div>

    </form>
</body>
</html>