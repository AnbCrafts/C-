<%@ Page Title="Learning Hub" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DevHelper._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <!-- Hero Section -->
        <div class="p-4 p-md-5 mb-4 rounded-3 text-white bg-dark shadow-sm position-relative overflow-hidden">
            <div class="col-md-9 position-relative z-1">
                <span class="badge bg-warning text-dark mb-2 px-3 py-2 fw-semibold">
                    <i class="bi bi-rocket-takeoff-fill me-1"></i> Interactive Web Forms Mastery
                </span>
                <h1 class="display-5 fw-bold">ASP.NET Web Forms Learning Lab</h1>
                <p class="fs-5 text-light opacity-75 mb-3">
                    Learn Web Forms by seeing the theory on one side, writing code in your editor, 
                    and executing live server controls on the other. Master everything before transitioning to Web APIs.
                </p>
                <div class="d-flex flex-wrap gap-2 align-items-center">
                    <span class="text-white-50 small me-2"><i class="bi bi-check2-circle text-success me-1"></i> 8 Core Modules</span>
                    <span class="text-white-50 small me-2"><i class="bi bi-cpu text-info me-1"></i> Live Interactive Labs</span>
                    <span class="text-white-50 small"><i class="bi bi-database text-warning me-1"></i> ADO.NET & SQL Server</span>
                </div>
            </div>
        </div>

        <!-- Quick Progress Stats Bar -->
        <div class="row g-3 mb-4">
            <div class="col-sm-6 col-lg-3">
                <div class="card border-0 shadow-sm bg-light">
                    <div class="card-body d-flex align-items-center justify-content-between p-3">
                        <div>
                            <small class="text-muted text-uppercase fw-semibold">Total Modules</small>
                            <h3 class="fw-bold mb-0 text-primary">
                                <asp:Label ID="lblTotalModules" runat="server">8</asp:Label>
                            </h3>
                        </div>
                        <div class="fs-1 text-primary opacity-50"><i class="bi bi-collection"></i></div>
                    </div>
                </div>
            </div>
            <div class="col-sm-6 col-lg-3">
                <div class="card border-0 shadow-sm bg-light">
                    <div class="card-body d-flex align-items-center justify-content-between p-3">
                        <div>
                            <small class="text-muted text-uppercase fw-semibold">Core Topics</small>
                            <h3 class="fw-bold mb-0 text-info">
                                <asp:Label ID="lblTotalTopics" runat="server">8</asp:Label>
                            </h3>
                        </div>
                        <div class="fs-1 text-info opacity-50"><i class="bi bi-journal-code"></i></div>
                    </div>
                </div>
            </div>
            <div class="col-sm-6 col-lg-3">
                <div class="card border-0 shadow-sm bg-light">
                    <div class="card-body d-flex align-items-center justify-content-between p-3">
                        <div>
                            <small class="text-muted text-uppercase fw-semibold">Completed</small>
                            <h3 class="fw-bold mb-0 text-success">
                                <asp:Label ID="lblCompletedTopics" runat="server">1</asp:Label>
                            </h3>
                        </div>
                        <div class="fs-1 text-success opacity-50"><i class="bi bi-check-circle-fill"></i></div>
                    </div>
                </div>
            </div>
            <div class="col-sm-6 col-lg-3">
                <div class="card border-0 shadow-sm bg-light">
                    <div class="card-body d-flex align-items-center justify-content-between p-3">
                        <div>
                            <small class="text-muted text-uppercase fw-semibold">Database Mode</small>
                            <h5 class="fw-bold mb-0 text-secondary">
                                <asp:Label ID="lblDbStatus" runat="server" CssClass="badge bg-success">Ready</asp:Label>
                            </h5>
                        </div>
                        <div class="fs-1 text-secondary opacity-50"><i class="bi bi-hdd-network"></i></div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Notification Panel if DB not connected -->
        <asp:Panel ID="pnlDbAlert" runat="server" Visible="false" CssClass="alert alert-info alert-dismissible fade show" role="alert">
            <i class="bi bi-info-circle-fill me-1"></i>
            <asp:Label ID="lblDbAlert" runat="server"></asp:Label>
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </asp:Panel>

        <!-- Topic Cards Section Header -->
        <div class="d-flex justify-content-between align-items-center mb-3">
            <div>
                <h3 class="fw-bold mb-0">Web Forms Learning Modules</h3>
                <p class="text-muted small mb-0">Click any topic card to open its Theory Blueprint and Live Playground.</p>
            </div>
        </div>

        <!-- Dynamic Module & Topic Cards (Repeater) -->
        <div class="row g-4">
            <asp:Repeater ID="rptModules" runat="server" OnItemDataBound="rptModules_ItemDataBound">
                <ItemTemplate>
                    <div class="col-lg-6">
                        <div class="card h-100 shadow-sm border-0 border-top border-4 border-primary">
                            <div class="card-header bg-white py-3 d-flex justify-content-between align-items-center border-0">
                                <div class="d-flex align-items-center">
                                    <div class="p-2 rounded bg-light text-primary me-3 fs-4">
                                        <i class='bi <%# Eval("IconClass") %>'></i>
                                    </div>
                                    <div>
                                        <h5 class="fw-bold mb-0"><%# Eval("ModuleName") %></h5>
                                        <small class="text-muted"><%# Eval("ShortDesc") %></small>
                                    </div>
                                </div>
                                <span class='badge <%# Eval("BadgeColor") %>'>Module <%# Eval("DisplayOrder") %></span>
                            </div>
                            <div class="card-body pt-1">
                                <!-- Nested Repeater for Topics in this Module -->
                                <asp:Repeater ID="rptTopics" runat="server">
                                    <HeaderTemplate>
                                        <div class="list-group list-group-flush">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div class="list-group-item px-0 py-3 border-light">
                                            <div class="d-flex justify-content-between align-items-start mb-1">
                                                <h6 class="fw-bold mb-1 text-dark">
                                                    <%# Eval("Title") %>
                                                </h6>
                                                <div class="d-flex gap-1">
                                                    <!-- Status Badge -->
                                                    <span class='badge <%# GetStatusBadgeClass(Eval("Status").ToString()) %>'>
                                                        <%# Eval("Status") %>
                                                    </span>
                                                    <!-- Difficulty Badge -->
                                                    <span class='badge bg-secondary-subtle text-secondary'>
                                                        <%# Eval("Difficulty") %>
                                                    </span>
                                                </div>
                                            </div>
                                            <p class="small text-muted mb-2"><%# Eval("Summary") %></p>
                                            
                                            <!-- Launch Lab Link -->
                                            <a href='<%# ResolveUrl(Eval("PageUrl").ToString()) %>' class="btn btn-sm btn-outline-primary">
                                                <i class="bi bi-play-circle-fill me-1"></i> Open Topic Lab & Playground &raquo;
                                            </a>
                                        </div>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </div>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </main>
</asp:Content>
