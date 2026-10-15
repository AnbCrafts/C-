<%@ Page Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="UserHistory.aspx.cs"
    Inherits="URLRouting.UserHistory" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="ContentHead"
    ContentPlaceHolderID="head"
    runat="server">

    <style>

        .container {
            max-width: 1300px;
            margin: 20px auto;
            background: #fff;
            padding: 30px;
            border-radius: 10px;
            box-shadow: 0 4px 15px rgba(0,0,0,.08);
        }

        .header-section {
            display: flex;
            justify-content: space-between;
            align-items: center;
            border-bottom: 2px solid #eef2f7;
            padding-bottom: 15px;
            margin-bottom: 25px;
        }

        .header-section h2 {
            margin: 0;
            color: #0078D7;
        }

        .btn {
            padding: 9px 18px;
            border: none;
            border-radius: 6px;
            cursor: pointer;
            font-size: 14px;
            font-weight: bold;
        }

        .btn-back {
            background-color: #0078D7;
            color: white;
        }

        .status-message {
            margin-bottom: 15px;
            font-size: 14px;
            font-weight: 600;
        }

        .status-success {
            color: #16a34a;
        }

        .status-empty {
            color: #dc2626;
        }

        .table-responsive {
            width: 100%;
            overflow-x: auto;
        }

        .gridview {
            width: 100%;
            border-collapse: collapse;
            font-size: 13px;
        }

        .gridview th {
            background-color: #0078D7;
            color: white;
            padding: 12px;
            text-align: left;
        }

        .gridview td {
            padding: 10px;
            border-bottom: 1px solid #ddd;
        }

        .gridview tr:nth-child(even) {
            background-color: #f8fafc;
        }

        .badge-active {
            background-color: #dcfce7;
            color: #15803d;
            padding: 4px 10px;
            border-radius: 20px;
            font-size: 12px;
            font-weight: bold;
        }

        .badge-inactive {
            background-color: #fee2e2;
            color: #b91c1c;
            padding: 4px 10px;
            border-radius: 20px;
            font-size: 12px;
            font-weight: bold;
        }

        .btn-view {
            background-color: #0284c7;
            color: white;
            padding: 5px 12px;
            border-radius: 4px;
            text-decoration: none;
            font-size: 12px;
            font-weight: bold;
        }

    </style>

</asp:Content>

<asp:Content ID="ContentMain"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container">

        <div class="header-section">

            <h2>User History</h2>

            <asp:Button ID="btnBack"
                runat="server"
                Text="← Back"
                CssClass="btn btn-back"
                OnClick="btnBack_Click" />

        </div>

        <asp:Label ID="lblStatus"
            runat="server"
            CssClass="status-message">
        </asp:Label>

        <div class="table-responsive">

            <asp:GridView ID="gvUserHistory"
                runat="server"
                AutoGenerateColumns="False"
                CssClass="gridview"
                GridLines="None">

                <Columns>

                    <asp:BoundField DataField="UserId" HeaderText="ID" />
                    <asp:BoundField DataField="UserName" HeaderText="Username" />
                    <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:BoundField DataField="MobileNo" HeaderText="Mobile No" />
                    <asp:BoundField DataField="City" HeaderText="City" />
                    <asp:BoundField DataField="StateName" HeaderText="State" />
                    <asp:BoundField DataField="Country" HeaderText="Country" />
                    <asp:BoundField DataField="Gender" HeaderText="Gender" />
                    <asp:BoundField DataField="QueryText" HeaderText="Query" />
                    <asp:BoundField DataField="CreatedDate"
                        HeaderText="Created Date"
                        DataFormatString="{0:dd-MMM-yyyy}" />

                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <%# Convert.ToBoolean(Eval("IsActive"))
                                ? "<span class='badge-active'>Active</span>"
                                : "<span class='badge-inactive'>Inactive</span>" %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>

                            <asp:HyperLink
                                ID="lnkDetails"
                                runat="server"
                                CssClass="btn-view"
                                NavigateUrl='<%# ResolveUrl(String.Format("~/Users/{0}/{1}",
                                Eval("UserId"),
                                Eval("UserName"))) %>'
                                Text="Details">
                            </asp:HyperLink>

                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

                <EmptyDataTemplate>
                    <div style="padding:20px;text-align:center;color:#64748b">
                        No user records found.
                    </div>
                </EmptyDataTemplate>

            </asp:GridView>

        </div>

    </div>

</asp:Content>