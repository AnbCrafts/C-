<%@ Page Title="GridView CRUD Lab" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GridViewLab.aspx.cs" Inherits="DevHelper.Labs.GridViewLab" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="py-2">
        <nav aria-label="breadcrumb">
            <ol class="breadcrumb mb-2 small">
                <li class="breadcrumb-item"><a href="~/Default.aspx" runat="server">Learning Hub</a></li>
                <li class="breadcrumb-item">Module 5: Data Binding</li>
                <li class="breadcrumb-item active" aria-current="page">GridView CRUD & Data Binding Lab</li>
            </ol>
        </nav>

        <div class="d-flex justify-content-between align-items-center mb-4">
            <div>
                <h2 class="fw-bold mb-1"><i class="bi bi-table text-danger me-2"></i> GridView CRUD & Data Binding Lab</h2>
                <p class="text-muted mb-0">Learn how DataSource + DataBind() turns collections/DataTables into HTML tables with edit and delete events.</p>
            </div>
            <span class="badge bg-danger px-3 py-2">Module 5</span>
        </div>

        <div class="row g-4">
            <!-- Left: Theory & Field Types -->
            <div class="col-lg-5">
                <div class="card border-0 shadow-sm mb-4">
                    <div class="card-header bg-dark text-white fw-bold">
                        <i class="bi bi-book-half me-1 text-warning"></i> 1. BoundField vs TemplateField
                    </div>
                    <div class="card-body small">
                        <div class="p-2 border rounded bg-light mb-2">
                            <strong>BoundField:</strong>
                            <p class="text-muted mb-0">Renders raw property text: <code>&lt;asp:BoundField DataField="Name" HeaderText="Name" /&gt;</code></p>
                        </div>
                        <div class="p-2 border rounded bg-light mb-2">
                            <strong>TemplateField:</strong>
                            <p class="text-muted mb-0">Allows custom HTML markup (badges, icons, buttons): <code>&lt;%# Eval("Status") %&gt;</code></p>
                        </div>
                        <div class="p-2 border rounded bg-light">
                            <strong>DataKeyNames="Id":</strong>
                            <p class="text-muted mb-0">Safely stores row primary keys in ViewState so you can retrieve the ID in row events!</p>
                        </div>
                    </div>
                </div>

                <!-- Add Quick Record Card -->
                <div class="card border-0 shadow-sm border-top border-4 border-danger">
                    <div class="card-header bg-white fw-bold">
                        <i class="bi bi-plus-circle-fill text-danger me-1"></i> Add Record to GridView
                    </div>
                    <div class="card-body">
                        <div class="mb-2">
                            <label class="form-label small fw-semibold">Item Name:</label>
                            <asp:TextBox ID="txtItemName" runat="server" CssClass="form-control form-control-sm" placeholder="e.g. Laptop Stand"></asp:TextBox>
                        </div>
                        <div class="mb-2">
                            <label class="form-label small fw-semibold">Category:</label>
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select form-select-sm">
                                <asp:ListItem Text="Hardware" Value="Hardware" />
                                <asp:ListItem Text="Software" Value="Software" />
                                <asp:ListItem Text="Accessories" Value="Accessories" />
                            </asp:DropDownList>
                        </div>
                        <div class="mb-3">
                            <label class="form-label small fw-semibold">Price ($):</label>
                            <asp:TextBox ID="txtPrice" runat="server" TextMode="Number" CssClass="form-control form-control-sm" placeholder="45.00"></asp:TextBox>
                        </div>
                        <asp:Button ID="btnAddItem" runat="server" Text="Add to Table" CssClass="btn btn-sm btn-danger w-100" OnClick="btnAddItem_Click" />
                    </div>
                </div>
            </div>

            <!-- Right: Interactive GridView -->
            <div class="col-lg-7">
                <div class="card border-0 shadow-sm border-top border-4 border-danger">
                    <div class="card-header bg-white fw-bold d-flex justify-content-between align-items-center">
                        <span><i class="bi bi-play-circle-fill text-danger me-1"></i> Live GridView Sandbox</span>
                        <asp:Label ID="lblRowCount" runat="server" CssClass="badge bg-danger">0 items</asp:Label>
                    </div>
                    <div class="card-body">
                        <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-dismissible fade show" role="alert">
                            <asp:Label ID="lblAlert" runat="server"></asp:Label>
                            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                        </asp:Panel>

                        <div class="table-responsive">
                            <asp:GridView ID="gvDemo" runat="server" 
                                AutoGenerateColumns="False" 
                                DataKeyNames="Id" 
                                CssClass="table table-hover table-striped align-middle small"
                                OnRowDeleting="gvDemo_RowDeleting">
                                <Columns>
                                    <asp:BoundField DataField="Id" HeaderText="#" ItemStyle-Font-Bold="true" />
                                    <asp:BoundField DataField="Name" HeaderText="Item Name" />
                                    <asp:BoundField DataField="Category" HeaderText="Category" />
                                    <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />
                                    <asp:TemplateField HeaderText="Actions" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" CssClass="btn btn-sm btn-outline-danger"
                                                OnClientClick="return confirm('Delete this record?');">
                                                Delete
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
