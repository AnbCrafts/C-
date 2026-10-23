<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LinqPracticeWebForm.aspx.cs" Inherits="LINQPractice.LinqPracticeWebForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>LINQ Practice WebForm</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 20px;
        }
        .grid {
            width: 100%;
            border-collapse: collapse;
        }
        .grid th {
            background-color: #34495e;
            color: white;
            padding: 8px;
            text-align: left;
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
            padding: 5px 10px;
            margin: 2px;
            border: 1px solid #ccc;
            text-decoration: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>LINQ to SQL - Users Practice</h2>
            <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="true" CssClass="grid">
            </asp:GridView>
            <br />
            <div class="pager">
                <asp:Repeater ID="rptPaging" runat="server">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkPage" runat="server" Text='<%# Eval("Text") %>' CommandArgument='<%# Eval("Value") %>' OnClick="Page_Changed"></asp:LinkButton>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </form>
</body>
</html>
