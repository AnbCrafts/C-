<%@ Page Title="State Management Lab" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StateLab.aspx.cs" Inherits="DevHelper.Labs.StateLab" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="py-2">
        <nav aria-label="breadcrumb">
            <ol class="breadcrumb mb-2 small">
                <li class="breadcrumb-item"><a href="~/Default.aspx" runat="server">Learning Hub</a></li>
                <li class="breadcrumb-item">Module 4: State Management</li>
                <li class="breadcrumb-item active" aria-current="page">ViewState vs Session vs Application</li>
            </ol>
        </nav>

        <div class="d-flex justify-content-between align-items-center mb-4">
            <div>
                <h2 class="fw-bold mb-1"><i class="bi bi-database-check text-warning me-2"></i> State Management Lab</h2>
                <p class="text-muted mb-0">Understand how Web Forms preserves state across stateless HTTP requests.</p>
            </div>
            <span class="badge bg-warning text-dark px-3 py-2">Module 4</span>
        </div>

        <div class="row g-4">
            <!-- Left: Theory & Scope Matrix -->
            <div class="col-lg-6">
                <div class="card border-0 shadow-sm mb-4">
                    <div class="card-header bg-dark text-white fw-bold">
                        <i class="bi bi-book-half me-1 text-warning"></i> 1. State Scopes Compared
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <table class="table table-bordered small mb-0">
                                <thead class="table-light">
                                    <tr><th>Mechanism</th><th>Where Stored</th><th>Lifespan</th><th>Scope</th></tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td><strong>ViewState</strong></td>
                                        <td>Client (Hidden input <code>__VIEWSTATE</code>)</td>
                                        <td>Single Page visit (dies if you navigate away)</td>
                                        <td>1 Page</td>
                                    </tr>
                                    <tr>
                                        <td><strong>Session</strong></td>
                                        <td>Server RAM (Keyed by cookie)</td>
                                        <td>Until browser closes or 20 min timeout</td>
                                        <td>1 User across all pages</td>
                                    </tr>
                                    <tr>
                                        <td><strong>Application</strong></td>
                                        <td>Server RAM (Global)</td>
                                        <td>Until web server restarts</td>
                                        <td>All Users globally</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>

                <div class="card border-0 shadow-sm">
                    <div class="card-header bg-secondary text-white fw-bold">
                        <i class="bi bi-code-square me-1"></i> 2. Syntax Examples
                    </div>
                    <div class="card-body p-0">
                        <pre class="bg-dark text-light p-3 rounded-bottom mb-0 small"><code>// 1. ViewState (page temporary memory)
ViewState["Counter"] = 5;
int count = (int)(ViewState["Counter"] ?? 0);

// 2. Session (user login & profile)
Session["UserId"] = 101;
int uid = Convert.ToInt32(Session["UserId"]);

// 3. Application (global active users)
Application.Lock();
Application["Hits"] = (int)(Application["Hits"] ?? 0) + 1;
Application.UnLock();</code></pre>
                    </div>
                </div>
            </div>

            <!-- Right: Interactive Experiment -->
            <div class="col-lg-6">
                <div class="card border-0 shadow-sm border-top border-4 border-warning">
                    <div class="card-header bg-white fw-bold">
                        <i class="bi bi-play-circle-fill text-warning me-1"></i> Live State Comparison Sandbox
                    </div>
                    <div class="card-body">
                        <p class="small text-muted mb-3">
                            Click the buttons below to increment each state independently. Notice how <strong>ViewState</strong> resets when you navigate away and return, but <strong>Session</strong> and <strong>Application</strong> persist!
                        </p>

                        <div class="row text-center g-2 mb-3">
                            <div class="col-4">
                                <div class="p-3 border rounded bg-light">
                                    <small class="text-muted d-block fw-semibold">ViewState</small>
                                    <asp:Label ID="lblViewState" runat="server" CssClass="fs-3 fw-bold text-warning text-dark">0</asp:Label>
                                </div>
                            </div>
                            <div class="col-4">
                                <div class="p-3 border rounded bg-light">
                                    <small class="text-muted d-block fw-semibold">Session</small>
                                    <asp:Label ID="lblSession" runat="server" CssClass="fs-3 fw-bold text-success">0</asp:Label>
                                </div>
                            </div>
                            <div class="col-4">
                                <div class="p-3 border rounded bg-light">
                                    <small class="text-muted d-block fw-semibold">Application</small>
                                    <asp:Label ID="lblApplication" runat="server" CssClass="fs-3 fw-bold text-primary">0</asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="d-grid gap-2">
                            <asp:Button ID="btnIncViewState" runat="server" Text="Increment ViewState (+1)" CssClass="btn btn-outline-warning text-dark" OnClick="btnIncViewState_Click" />
                            <asp:Button ID="btnIncSession" runat="server" Text="Increment Session (+1)" CssClass="btn btn-outline-success" OnClick="btnIncSession_Click" />
                            <asp:Button ID="btnIncApp" runat="server" Text="Increment Application (+1)" CssClass="btn btn-outline-primary" OnClick="btnIncApp_Click" />
                            <asp:Button ID="btnResetState" runat="server" Text="Reset All Counters" CssClass="btn btn-outline-secondary" OnClick="btnResetState_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
