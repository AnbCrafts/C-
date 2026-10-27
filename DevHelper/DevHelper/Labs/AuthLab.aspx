<%@ Page Title="Security & Authentication Lab" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AuthLab.aspx.cs" Inherits="DevHelper.Labs.AuthLab" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="py-2">
        <nav aria-label="breadcrumb">
            <ol class="breadcrumb mb-2 small">
                <li class="breadcrumb-item"><a href="~/Default.aspx" runat="server">Learning Hub</a></li>
                <li class="breadcrumb-item">Module 7: Security</li>
                <li class="breadcrumb-item active" aria-current="page">Forms Authentication & User Sessions</li>
            </ol>
        </nav>

        <div class="d-flex justify-content-between align-items-center mb-4">
            <div>
                <h2 class="fw-bold mb-1"><i class="bi bi-shield-lock text-dark me-2"></i> Security & Authentication Lab</h2>
                <p class="text-muted mb-0">Learn how Forms Authentication, session guards, and SQL parameterization keep web applications secure.</p>
            </div>
            <span class="badge bg-dark px-3 py-2">Module 7</span>
        </div>

        <div class="row g-4">
            <!-- Left: Theory & Defense Matrix -->
            <div class="col-lg-6">
                <div class="card border-0 shadow-sm mb-4">
                    <div class="card-header bg-dark text-white fw-bold">
                        <i class="bi bi-book-half me-1 text-warning"></i> 1. The 3 Security Pillars in Web Forms
                    </div>
                    <div class="card-body small">
                        <div class="mb-3">
                            <h6 class="fw-bold text-primary mb-1">A. Authentication & Session Guard</h6>
                            <p class="text-muted mb-0">
                                Check <code>if (Session["User"] == null) Response.Redirect("Login.aspx");</code> in <code>Page_Load</code> to block unauthorized access to protected pages.
                            </p>
                        </div>
                        <div class="mb-3">
                            <h6 class="fw-bold text-success mb-1">B. SQL Injection Defense</h6>
                            <p class="text-muted mb-0">
                                <strong>Never</strong> concatenate raw text: <code>SELECT * FROM Users WHERE Email = '" + txt + "'</code>. Always use <code>cmd.Parameters.AddWithValue("@email", txt)</code>.
                            </p>
                        </div>
                        <div>
                            <h6 class="fw-bold text-danger mb-1">C. XSS (Cross-Site Scripting) Defense</h6>
                            <p class="text-muted mb-0">
                                Always use <code>Server.HtmlEncode(userInput)</code> before outputting user-provided content to prevent malicious script injection.
                            </p>
                        </div>
                    </div>
                </div>

                <div class="card border-0 shadow-sm">
                    <div class="card-header bg-secondary text-white fw-bold">
                        <i class="bi bi-code-square me-1"></i> 2. Authentication Cookie Pattern
                    </div>
                    <div class="card-body p-0">
                        <pre class="bg-dark text-light p-3 rounded-bottom mb-0 small"><code>// After verifying credentials in DB:
FormsAuthentication.SetAuthCookie(userEmail, createPersistentCookie: false);

// Storing session profile
Session["UserId"] = dbUserId;
Session["Role"] = "Admin";

// Logging out
Session.Clear();
FormsAuthentication.SignOut();
Response.Redirect("~/Login.aspx");</code></pre>
                    </div>
                </div>
            </div>

            <!-- Right: Interactive Auth Sandbox -->
            <div class="col-lg-6">
                <div class="card border-0 shadow-sm border-top border-4 border-dark">
                    <div class="card-header bg-white fw-bold d-flex justify-content-between align-items-center">
                        <span><i class="bi bi-play-circle-fill text-dark me-1"></i> Live Session Simulator</span>
                        <asp:Label ID="lblLoginStatus" runat="server" CssClass="badge bg-secondary">Guest (Not Logged In)</asp:Label>
                    </div>
                    <div class="card-body">
                        <asp:Panel ID="pnlLoginBox" runat="server">
                            <p class="small text-muted mb-2">Simulate a user logging into this learning session:</p>
                            <div class="mb-2">
                                <label class="form-label small fw-semibold">Simulated User Name:</label>
                                <asp:TextBox ID="txtSimUser" runat="server" CssClass="form-control form-control-sm" Text="Alex Developer"></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label class="form-label small fw-semibold">Role:</label>
                                <asp:DropDownList ID="ddlSimRole" runat="server" CssClass="form-select form-select-sm">
                                    <asp:ListItem Text="Student" Value="Student" />
                                    <asp:ListItem Text="Admin" Value="Admin" />
                                </asp:DropDownList>
                            </div>
                            <asp:Button ID="btnLoginSim" runat="server" Text="Log In to Session" CssClass="btn btn-sm btn-dark w-100" OnClick="btnLoginSim_Click" />
                        </asp:Panel>

                        <asp:Panel ID="pnlProfileBox" runat="server" Visible="false" CssClass="p-3 bg-light rounded border text-center mt-2">
                            <div class="fs-1 text-primary"><i class="bi bi-person-check-fill"></i></div>
                            <h5 class="fw-bold mb-1"><asp:Label ID="lblProfileName" runat="server"></asp:Label></h5>
                            <span class="badge bg-info text-dark mb-3"><asp:Label ID="lblProfileRole" runat="server"></asp:Label></span>
                            <div class="d-grid gap-2">
                                <asp:Button ID="btnLogoutSim" runat="server" Text="Log Out (Clear Session)" CssClass="btn btn-sm btn-outline-danger" OnClick="btnLogoutSim_Click" />
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
