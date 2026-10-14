<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuerySearch.aspx.cs" Inherits="QueryString.QuerySearch" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Query Search</title>

    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
            font-family: Segoe UI, Arial, sans-serif;
        }

        body {
            min-height: 100vh;
            background: linear-gradient(135deg,#0078D7,#00B4D8);
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .card {
            width: 500px;
            background: white;
            border-radius: 15px;
            padding: 30px;
            box-shadow: 0 10px 25px rgba(0,0,0,0.2);
        }

        .title {
            text-align: center;
            color: #0078D7;
            margin-bottom: 25px;
        }

        .form-group {
            margin-bottom: 20px;
        }

        .label {
            display: block;
            margin-bottom: 8px;
            font-weight: 600;
            color: #444;
        }

        .textbox {
            width: 100%;
            height: 45px;
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 8px;
            font-size: 15px;
            background-color: #f8f9fa;
        }

        .textbox:focus {
            border-color: #0078D7;
            outline: none;
        }

        .message {
            width: 100%;
            height: 45px;
            margin-top: 15px;
            text-align: center;
            font-weight: bold;
            border-radius: 8px;
            border: 1px solid #ddd;
            background-color: #f8f9fa;
        }

        .details {
            margin-top: 20px;
            padding: 15px;
            border-radius: 10px;
            background: #f5f7fb;
            border: 1px solid #e0e0e0;
        }

        .details label {
            font-weight: bold;
            color: #0078D7;
        }

        .details span {
            color: #333;
        }
    </style>

</head>
<body>

    <form id="form1" runat="server">

        <div class="card">

            <h2 class="title">User Details</h2>

            <div class="form-group">
                <asp:Label ID="usernameLbl"
                    runat="server"
                    CssClass="label"
                    Text="Username">
                </asp:Label>

                <asp:TextBox ID="usernameTxt"
                    runat="server"
                    CssClass="textbox"
                    ReadOnly="true">
                </asp:TextBox>
            </div>

            <div class="form-group">
                <asp:Label ID="emailLbl"
                    runat="server"
                    CssClass="label"
                    Text="Email">
                </asp:Label>

                <asp:TextBox ID="emailTxt"
                    runat="server"
                    CssClass="textbox"
                    ReadOnly="true">
                </asp:TextBox>
            </div>

            <asp:TextBox
                ID="successMessage"
                runat="server"
                CssClass="message"
                ReadOnly="true">
            </asp:TextBox>

        </div>

    </form>

</body>
</html>