<%@ Page Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="SearchUser.aspx.cs"
    Inherits="URLRouting.SearchUser" %>

<asp:Content ID="ContentHead"
    ContentPlaceHolderID="head"
    runat="server">

    <style>
        .card {
            width: 700px;
            margin: 50px auto;
            background: white;
            padding: 25px;
            border-radius: 10px;
            box-shadow: 0 0 15px lightgray;
        }

        .card h2 {
            text-align: center;
            color: #0078D7;
            margin-bottom: 25px;
        }

        .row {
            display: flex;
            margin-bottom: 12px;
            padding-bottom: 6px;
            border-bottom: 1px solid #f1f1f1;
        }

        .label {
            width: 180px;
            font-weight: bold;
            color: #333;
        }

        .value {
            color: #444;
        }
    </style>

</asp:Content>

<asp:Content ID="ContentMain"
    ContentPlaceHolderID="MainContent"
    runat="server">

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

</asp:Content>