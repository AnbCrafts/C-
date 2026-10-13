<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LearnState.aspx.cs" Inherits="PracticeSet2.LearnState" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        #form1 {
            height: 386px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="txtBoxContainer" style="height: 229px">
            <asp:TextBox ID="txtBox" runat="server" BorderColor="#0066FF" BorderStyle="Solid" BorderWidth="4px" Height="214px" Width="841px"></asp:TextBox>
            <asp:Button ID="btn_1" OnClick="Button1_Click" runat="server" Height="60px" Text="Click me" Width="294px" />
            <asp:Button ID="btn_2" OnClick="Button1_Click_Protected" runat="server" Height="53px" style="margin-left: 82px; margin-top: 0px" Text="Click me for protected increment" Width="285px" />
        </div>
    </form>
</body>
</html>
