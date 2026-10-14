<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CookiePractice.aspx.cs" Inherits="WebApplication1.CookiePractice" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Registration</title>

    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }

        body {
            min-height: 100vh;
            background: linear-gradient(135deg, #4f46e5, #7c3aed);
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .card {
            background: white;
            width: 400px;
            padding: 35px;
            border-radius: 15px;
            box-shadow: 0 10px 25px rgba(0,0,0,0.2);
        }

        .card h2 {
            text-align: center;
            color: #333;
            margin-bottom: 25px;
        }

        .input-group {
            margin-bottom: 18px;
        }

        .input-group label {
            display: block;
            margin-bottom: 6px;
            font-weight: 600;
            color: #444;
        }

        .textbox {
            width: 100%;
            padding: 12px;
            border: 1px solid #d1d5db;
            border-radius: 8px;
            font-size: 15px;
            transition: 0.3s;
        }

        .textbox:focus {
            border-color: #4f46e5;
            outline: none;
            box-shadow: 0 0 8px rgba(79, 70, 229, .3);
        }

        .btn-submit {
            width: 100%;
            padding: 12px;
            background: #4f46e5;
            color: white;
            border: none;
            border-radius: 8px;
            cursor: pointer;
            font-size: 16px;
            font-weight: 600;
            transition: 0.3s;
        }

        .btn-submit:hover {
            background: #4338ca;
        }

        .message {
            display: block;
            text-align: center;
            margin-top: 15px;
            color: green;
            font-weight: 600;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="card">

            <h2>User Details</h2>

            <div class="input-group">
                <label>Username</label>
                <asp:TextBox
                    ID="txtUsername"
                    runat="server"
                    CssClass="textbox"
                    placeholder="Enter your username">
                </asp:TextBox>
            </div>

            <div class="input-group">
                <label>Email Address</label>
                <asp:TextBox
                    ID="txtEmail"
                    runat="server"
                    CssClass="textbox"
                    TextMode="Email"
                    placeholder="Enter your email">
                </asp:TextBox>
            </div>

            <asp:Button
                ID="btnSubmit"
                runat="server"
                Text="Save Details"
                CssClass="btn-submit"
                OnClick="btnSubmit_Click" />

            <asp:Label
                ID="lblMessage"
                runat="server"
                CssClass="message">
            </asp:Label>

        </div>
    </form>
</body>
</html>