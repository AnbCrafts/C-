<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserForm.aspx.cs" Inherits="PracticeSet2.UserForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style4 {
            height: 45px;
        }
        .auto-style5 {
            height: 45px;
            width: 212px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="height: 335px">
            <table class="auto-style1">
                <tr>
                    <td class="auto-style5">
                        <asp:Label ID="name" runat="server" Text="Enter Product Name"></asp:Label>
                    </td>
                    <td class="auto-style4">
                        <asp:TextBox ID="nameVal" runat="server" Height="33px" Width="569px"></asp:TextBox>
                        <asp:RequiredFieldValidator ForeColor="Red" Display="Dynamic" ControlToValidate="nameVal" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Name must be valid"></asp:RequiredFieldValidator>
                        <asp:CustomValidator

ID="cvName"

runat="server"

ControlToValidate="nameVal"

ErrorMessage="Name must contain only letters and be at least 3 characters long"

ForeColor="Red"

OnServerValidate="ValidateName">

</asp:CustomValidator>
                    </td>

                </tr>
                <tr>
                    <td class="auto-style5">
                        <asp:Label ID="price" runat="server" Text="Enter Product Price"></asp:Label>
                    </td>
                    <td class="auto-style4">
                        <asp:TextBox ID="priceVal" runat="server" Height="33px" Width="569px"></asp:TextBox>
                        <asp:RequiredFieldValidator ForeColor="Red" Display="Dynamic" ControlToValidate="priceVal" ID="RequiredFieldValidator2" runat="server" ErrorMessage="Price is required"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ForeColor="Red" Display="Dynamic" ID="RangeValidator2" Type="Integer" runat="server" ErrorMessage="Value is out of range" MinimumValue="50" MaximumValue="1000" ControlToValidate="priceVal"></asp:RangeValidator>
                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style5">
                        <asp:Label ID="stock" runat="server" Text="Enter Stock Availability"></asp:Label>
                    </td>
                    <td class="auto-style4">
                        <asp:TextBox ID="stockVal" runat="server" Height="33px" Width="569px"></asp:TextBox>
                        <asp:RequiredFieldValidator ForeColor="Red" Display="Dynamic" ControlToValidate="stockVal" ID="RequiredFieldValidator3" runat="server" ErrorMessage="Stock is required"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ForeColor="Red" Display="Dynamic" Type="Integer" ID="RangeValidator1" runat="server" ErrorMessage="Value is out of range" MinimumValue="5" MaximumValue="100" ControlToValidate="stockVal"></asp:RangeValidator>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style5"></td>
                    <td class="auto-style4">
<asp:Button ID="saveBtn"
    runat="server"
    Height="36px"
    Width="116px"
    Text="Save Data"
    OnClick="SubmitForm" />                    </td>
                </tr>
                <tr>
                    <td class="auto-style5"></td>
                    <td class="auto-style4">
                        <asp:Label ID="resMessage" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
