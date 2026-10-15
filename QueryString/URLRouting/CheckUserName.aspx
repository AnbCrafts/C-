<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CheckUserName.aspx.cs" Inherits="URLRouting.CheckUserName" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Check Username Availability</title>

    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f6f9;
            margin: 0;
            padding: 0;
        }

        .container {
            width: 500px;
            margin: 80px auto;
            background-color: white;
            padding: 30px;
            border-radius: 12px;
            box-shadow: 0 0 18px rgba(0,0,0,0.1);
        }

        .title {
            text-align: center;
            color: #2c3e50;
            margin-bottom: 25px;
        }

        .form-group {
            margin-bottom: 20px;
        }

        label {
            display: block;
            font-weight: bold;
            margin-bottom: 8px;
            color: #333;
        }

        .textbox {
            width: 100%;
            padding: 12px;
            border: 1px solid #ccc;
            border-radius: 6px;
            box-sizing: border-box;
            font-size: 14px;
        }

        .textbox:focus {
            border-color: #0078d7;
            outline: none;
        }

        .btn-check {
            width: 100%;
            padding: 12px;
            background-color: #0078d7;
            color: white;
            border: none;
            border-radius: 6px;
            cursor: pointer;
            font-size: 16px;
            font-weight: bold;
        }

        .btn-check:hover {
            background-color: #005ea8;
        }

        .message-box {
            margin-top: 20px;
        }

        .message {
            width: 100%;
            box-sizing: border-box;
            padding: 12px;
            border-radius: 6px;
            border: 1px solid #ccc;
            background-color: #f8f9fa;
            resize: none;
        }

        .loading {
            text-align: center;
            color: #0078d7;
            font-weight: bold;
            margin-top: 15px;
        }
    </style>
</head>
<body>

    <form id="form1" runat="server">

        <asp:ScriptManager ID="ScriptManager1"
            runat="server">
        </asp:ScriptManager>

        <div class="container">

            <h2 class="title">Check Username Availability</h2>

            <asp:UpdatePanel ID="upUsernameCheck"
                runat="server">

                <ContentTemplate>

                    <div class="form-group">
                        <label>Username</label>

                        <asp:TextBox
                            ID="txtUsername"
                            runat="server"
                            CssClass="textbox"
                            placeholder="Enter username"
                            AutoPostBack="true"
                            OnTextChanged="btnCheckUsername_Click">
                        </asp:TextBox>
                    </div>

                    <asp:Button
                        ID="btnCheckUsername"
                        runat="server"
                        Text="Check Availability"
                        CssClass="btn-check"
                        OnClick="btnCheckUsername_Click" />

                    <div class="message-box">

                        <label>Status Message</label>

                        <asp:TextBox
                            ID="txtMessage"
                            runat="server"
                            CssClass="message"
                            TextMode="MultiLine"
                            Rows="3"
                            ReadOnly="true">
                        </asp:TextBox>

                    </div>
                    
                </ContentTemplate>

               <Triggers>
                <asp:AsyncPostBackTrigger
                    ControlID="txtUsername"
                    EventName="TextChanged" />
            </Triggers>


            </asp:UpdatePanel>

            <asp:UpdateProgress
                ID="UpdateProgress1"
                runat="server"
                AssociatedUpdatePanelID="upUsernameCheck">

                <ProgressTemplate>

                    <div style="

                        margin-top:20px;

                        padding:15px;
                        background-color:#fff3cd;
                        border:1px solid #ffeeba;
                        color:#856404;
                        border-radius:6px;
                        font-weight:bold;"
                        class="loading">
                        Checking Username...
                    </div>

                </ProgressTemplate>

            </asp:UpdateProgress>

        </div>

    </form>

</body>
</html>