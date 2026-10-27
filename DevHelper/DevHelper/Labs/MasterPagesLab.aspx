<%@ Page Title="Master Pages & Layouts Lab" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MasterPagesLab.aspx.cs" Inherits="DevHelper.Labs.MasterPagesLab" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="py-2">
        <nav aria-label="breadcrumb">
            <ol class="breadcrumb mb-2 small">
                <li class="breadcrumb-item"><a href="~/Default.aspx" runat="server">Learning Hub</a></li>
                <li class="breadcrumb-item">Module 3: Master Pages & Layouts</li>
                <li class="breadcrumb-item active" aria-current="page">Master Pages & User Controls (.ascx)</li>
            </ol>
        </nav>

        <div class="d-flex justify-content-between align-items-center mb-4">
            <div>
                <h2 class="fw-bold mb-1"><i class="bi bi-window-stack text-info me-2"></i> Master Pages & User Controls Lab</h2>
                <p class="text-muted mb-0">Learn layout inheritance with ContentPlaceHolder and encapsulating reusable widgets into .ascx user controls.</p>
            </div>
            <span class="badge bg-info text-dark px-3 py-2">Module 3</span>
        </div>

        <div class="row g-4">
            <!-- Left: Theory & Architecture -->
            <div class="col-lg-6">
                <div class="card border-0 shadow-sm mb-4">
                    <div class="card-header bg-dark text-white fw-bold">
                        <i class="bi bi-book-half me-1 text-warning"></i> 1. The Master Page Architecture
                    </div>
                    <div class="card-body">
                        <p class="small text-muted">
                            A Master Page (<code>Site.Master</code>) defines the shared outer shell (header, navigation, footer) and defines placeholder slots using <code>&lt;asp:ContentPlaceHolder&gt;</code>.
                        </p>
                        <div class="p-3 bg-light rounded border mb-3 small font-monospace">
                            <div>&lt;html&gt;</div>
                            <div class="ms-3">&lt;nav class="navbar"&gt;...&lt;/nav&gt;</div>
                            <div class="ms-3 text-primary fw-bold">&lt;asp:ContentPlaceHolder ID="MainContent" runat="server" /&gt;</div>
                            <div class="ms-3">&lt;footer&gt;...&lt;/footer&gt;</div>
                            <div>&lt;/html&gt;</div>
                        </div>
                        <p class="small text-muted mb-0">
                            Child pages (<code>.aspx</code>) declare <code>MasterPageFile="~/Site.Master"</code> and wrap their content inside <code>&lt;asp:Content ContentPlaceHolderID="MainContent"&gt;</code>.
                        </p>
                    </div>
                </div>

                <div class="card border-0 shadow-sm">
                    <div class="card-header bg-secondary text-white fw-bold">
                        <i class="bi bi-code-square me-1"></i> 2. User Controls (.ascx)
                    </div>
                    <div class="card-body small">
                        <p class="text-muted">
                            Need a reusable widget across multiple pages (e.g. User Profile Card, Search Widget, Rating Control)?
                            Create a <strong>User Control (<code>.ascx</code>)</strong>!
                        </p>
                        <ol class="ps-3 mb-0">
                            <li>Has its own markup (<code>.ascx</code>) and code-behind (<code>.ascx.cs</code>).</li>
                            <li>Does not have <code>&lt;html&gt;</code> or <code>&lt;body&gt;</code> tags.</li>
                            <li>Registered in page via <code>&lt;%@ Register TagPrefix="uc" TagName="Card" Src="~/Controls/Card.ascx" %&gt;</code>.</li>
                        </ol>
                    </div>
                </div>
            </div>

            <!-- Right: Interactive Experiment -->
            <div class="col-lg-6">
                <div class="card border-0 shadow-sm border-top border-4 border-info">
                    <div class="card-header bg-white fw-bold">
                        <i class="bi bi-play-circle-fill text-info me-1"></i> Live Layout Interaction
                    </div>
                    <div class="card-body">
                        <p class="small text-muted mb-3">
                            You can interact with the parent Master Page directly from child C# code! Try updating page title or master properties below:
                        </p>

                        <div class="mb-3">
                            <label class="form-label small fw-semibold">Dynamic Browser Tab Title:</label>
                            <asp:TextBox ID="txtCustomTitle" runat="server" CssClass="form-control" placeholder="Type new title..."></asp:TextBox>
                        </div>

                        <asp:Button ID="btnUpdateTitle" runat="server" Text="Set Page.Title dynamically" CssClass="btn btn-info text-dark w-100 mb-3" OnClick="btnUpdateTitle_Click" />

                        <div class="alert alert-light border small">
                            <strong>Current Page.Title:</strong> <asp:Label ID="lblCurrentTitle" runat="server" CssClass="fw-bold text-primary"></asp:Label>
                        </div>

                        <hr />
                        <h6 class="fw-bold small text-secondary">Summary Checklist for Module 3:</h6>
                        <ul class="small text-muted ps-3 mb-0">
                            <li>Shared layout resides in <code>Site.Master</code>.</li>
                            <li>Child views inject via <code>&lt;asp:Content&gt;</code>.</li>
                            <li>User controls end with <code>.ascx</code>.</li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
