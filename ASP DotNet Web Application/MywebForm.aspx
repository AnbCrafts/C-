<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MywebForm.aspx.cs" Inherits="ASP_DotNet_Web_Application.MywebForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        #btn {
            width: 153px;
            height: 50px;
        }
    </style>
</head>
<body>
    <form id="form" runat="server">
        <div>
            <asp:Button ID="btnClick" runat="server" OnClick="btnClick_Click" Text="Click me" />
&nbsp;</div>
        <p>
            &nbsp;</p>
        <p>
            &nbsp;</p>
        <p>
            <asp:Label ID="lbl" runat="server"></asp:Label>
        </p>
    </form>
</body>
</html>
