<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuerySearch.aspx.cs" Inherits="QueryString.QuerySearch" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Details</title>

   <style>
    * {
        margin: 0;
        padding: 0;
        box-sizing: border-box;
        font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    }

    body {
        min-height: 100vh;
        display: flex;
        justify-content: center;
        align-items: center;
        background: linear-gradient(135deg, #0078D7, #00B4D8);
        padding: 20px;
    }

    .container {
        width: 100%;
        max-width: 700px;
        background: #fff;
        border-radius: 20px;
        padding: 40px;
        box-shadow: 0 15px 40px rgba(0,0,0,0.2);
        animation: fadeIn 0.5s ease-in-out;
    }

    @keyframes fadeIn {
        from {
            opacity: 0;
            transform: translateY(20px);
        }

        to {
            opacity: 1;
            transform: translateY(0);
        }
    }

    .title {
        text-align: center;
        color: #0078D7;
        margin-bottom: 30px;
        font-size: 32px;
        font-weight: 700;
    }

    .subtitle {
        text-align: center;
        color: #666;
        margin-bottom: 35px;
        font-size: 14px;
    }

    .form-group {
        margin-bottom: 20px;
    }

    .label {
        display: block;
        margin-bottom: 8px;
        color: #333;
        font-weight: 600;
        font-size: 15px;
    }

    .textbox {
        width: 100%;
        height: 48px;
        border: 1px solid #dcdcdc;
        border-radius: 10px;
        padding: 0 15px;
        font-size: 15px;
        background: #f8fafc;
        transition: all 0.3s ease;
    }

    .textbox:focus {
        border-color: #0078D7;
        box-shadow: 0 0 10px rgba(0,120,215,0.25);
        background: #fff;
        outline: none;
    }

    .status-box {
        width: 100%;
        min-height: 55px;
        border-radius: 10px;
        border: none;
        text-align: center;
        font-weight: 600;
        font-size: 16px;
        margin-top: 20px;
        background: #e8f5e9;
        color: #2e7d32;
    }

    .user-icon {
        width: 90px;
        height: 90px;
        margin: 0 auto 20px;
        border-radius: 50%;
        background: #0078D7;
        color: white;
        font-size: 42px;
        display: flex;
        justify-content: center;
        align-items: center;
    }

    .footer {
        text-align: center;
        margin-top: 25px;
        color: #888;
        font-size: 13px;
    }
</style>
</head>
<body>
    <form id="form1" runat="server">

        <div class="container">

    <div class="user-icon">
        👤
    </div>

    <h1 class="title">User Details</h1>

    <p class="subtitle">
        Information fetched using Query String Parameters
    </p>

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
            Text="Email Address">
        </asp:Label>

        <asp:TextBox ID="emailTxt"
            runat="server"
            CssClass="textbox"
            ReadOnly="true">
        </asp:TextBox>
    </div>

    <div class="form-group">
        <asp:TextBox ID="successMessage"
            runat="server"
            CssClass="status-box"
            ReadOnly="true">
        </asp:TextBox>
    </div>

    <div class="footer">
        ASP.NET Web Forms | Query String Demo
    </div>

</div>

    </form>
</body>
</html>
