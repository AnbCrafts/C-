<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserListShortDetail.aspx.cs" Inherits="GridView_ObjectDataSource_Practice.UserListShortDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pagination Using Query String</title>

    <style>
        body {
            font-family: Arial;
            margin: 20px;
        }

        .grid {
            width: 100%;
            border-collapse: collapse;
        }

        .grid th {
            background: #34495e;
            color: white;
            padding: 8px;
        }

        .grid td {
            border: 1px solid #ddd;
            padding: 8px;
        }

        .pager {
            margin-top: 20px;
            text-align: center;
        }

        .pager a {
            margin: 0 5px;
        }
    </style>

</head>
<body>

    <form id="form1" runat="server">

        <h2>Pagination Using Query String</h2>

        <asp:GridView
            ID="gvUsers"
            runat="server"
            AutoGenerateColumns="true"
            CssClass="grid"
            OnRowDataBound="gvUsers_RowDataBound"
            >
        </asp:GridView>

        <br />

        <asp:Label
            ID="lblPageInfo"
            runat="server">
        </asp:Label>

        <div class="pager">

            <asp:HyperLink
                ID="lnkPrevious"
                runat="server"
                Text="Previous">
            </asp:HyperLink>

            &nbsp;&nbsp;&nbsp;

            <asp:HyperLink
                ID="lnkNext"
                runat="server"
                Text="Next">
            </asp:HyperLink>

        </div>

    </form>

</body>
</html>
