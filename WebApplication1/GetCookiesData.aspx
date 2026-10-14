<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetCookiesData.aspx.cs" Inherits="WebApplication1.SetCookies" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cookie Details</title>

    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
            font-family: 'Segoe UI', sans-serif;
        }

        body {
            min-height: 100vh;
            display: flex;
            justify-content: center;
            align-items: center;
            background: linear-gradient(135deg, #4f46e5, #7c3aed);
        }

        .card {
            width: 450px;
            background: white;
            border-radius: 15px;
            padding: 30px;
            box-shadow: 0 8px 20px rgba(0,0,0,.2);
        }

        .title {
            text-align: center;
            margin-bottom: 25px;
            color: #333;
        }

        .info-row {
            margin-bottom: 20px;
        }

        .label {
            display: block;
            font-size: 14px;
            font-weight: 600;
            color: #666;
            margin-bottom: 5px;
        }

        .value {
            width: 100%;
            padding: 12px;
            background: #f5f7fa;
            border: 1px solid #ddd;
            border-radius: 8px;
            font-size: 15px;
            color: #222;
        }

        .status {
            text-align: center;
            margin-top: 15px;
            font-weight: 600;
            color: #dc2626;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <div class="card">

            <h2 class="title">Cookie Information</h2>

            <div class="info-row">
                <span class="label">Username</span>
                <asp:Label ID="lblUsername" runat="server" CssClass="value"></asp:Label>
            </div>

            <div class="info-row">
                <span class="label">Email Address</span>
                <asp:Label ID="lblEmail" runat="server" CssClass="value"></asp:Label>
            </div>

            <asp:Label ID="lblStatus" runat="server" CssClass="status"></asp:Label>

        </div>

    </form>
</body>
</html>