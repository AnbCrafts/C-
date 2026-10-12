<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormPage.aspx.cs" Inherits="PracticeSet1.FormPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <p>
            <asp:Label ID="lbl1" runat="server" Text="username"></asp:Label>
&nbsp;<asp:TextBox ID="txtb1" runat="server" OnTextChanged="txtb1_TextChanged" Width="197px"></asp:TextBox>
        </p>
        <p>
            <asp:Label ID="lbl2" runat="server" Text="Password"></asp:Label>
            <asp:TextBox ID="txtb2" runat="server" OnTextChanged="txtb2_TextChanged" TextMode="Password" Width="196px"></asp:TextBox>
        </p>
        <p>
            <asp:Button ID="btn" runat="server" Height="32px" OnClick="Button1_Click" Text="Submit Form" Width="101px" />
        </p>
    </form>
</body>
</html>
