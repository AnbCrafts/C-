<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchUser.aspx.cs" Inherits="URLRouting.SearchUser" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Details</title>

    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f5f7fa;
        }

        .card {
            width: 700px;
            margin: 50px auto;
            background: white;
            padding: 25px;
            border-radius: 10px;
            box-shadow: 0 0 15px lightgray;
        }

        h2 {
            text-align: center;
            color: #0078D7;
        }

        .row {
            display: flex;
            margin-bottom: 12px;
        }

        .label {
            width: 180px;
            font-weight: bold;
        }

        .value {
            color: #444;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <div class="card">

            <h2>User Details</h2>

            <div class="row">
                <div class="label">User ID</div>
                <div class="value">
                    <asp:Label ID="lblUserId" runat="server" />
                </div>
            </div>

            <div class="row">
                <div class="label">Username</div>
                <div class="value">
                    <asp:Label ID="lblUserName" runat="server" />
                </div>
            </div>

            <div class="row">
                <div class="label">Email</div>
                <div class="value">
                    <asp:Label ID="lblEmail" runat="server" />
                </div>
            </div>

            <div class="row">
                <div class="label">Full Name</div>
                <div class="value">
                    <asp:Label ID="lblFullName" runat="server" />
                </div>
            </div>

            <div class="row">
                <div class="label">Mobile No</div>
                <div class="value">
                    <asp:Label ID="lblMobileNo" runat="server" />
                </div>
            </div>

            <div class="row">
                <div class="label">City</div>
                <div class="value">
                    <asp:Label ID="lblCity" runat="server" />
                </div>
            </div>

            <div class="row">
                <div class="label">State</div>
                <div class="value">
                    <asp:Label ID="lblState" runat="server" />
                </div>
            </div>

            <div class="row">
                <div class="label">Country</div>
                <div class="value">
                    <asp:Label ID="lblCountry" runat="server" />
                </div>
            </div>

            <div class="row">
                <div class="label">Gender</div>
                <div class="value">
                    <asp:Label ID="lblGender" runat="server" />
                </div>
            </div>

            <div class="row">
                <div class="label">Query</div>
                <div class="value">
                    <asp:Label ID="lblQueryText" runat="server" />
                </div>
            </div>

            <div class="row">
                <div class="label">Created Date</div>
                <div class="value">
                    <asp:Label ID="lblCreatedDate" runat="server" />
                </div>
            </div>

            <div class="row">
                <div class="label">Active</div>
                <div class="value">
                    <asp:Label ID="lblIsActive" runat="server" />
                </div>
            </div>

        </div>

    </form>
</body>
</html>