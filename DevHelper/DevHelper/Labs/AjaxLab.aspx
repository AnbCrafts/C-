<%@ Page Title="ASP.NET AJAX & UpdatePanel Lab" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AjaxLab.aspx.cs" Inherits="DevHelper.Labs.AjaxLab" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="py-2">
        <nav aria-label="breadcrumb">
            <ol class="breadcrumb mb-2 small">
                <li class="breadcrumb-item"><a href="~/Default.aspx" runat="server">Learning Hub</a></li>
                <li class="breadcrumb-item">Module 6: ASP.NET AJAX</li>
                <li class="breadcrumb-item active" aria-current="page">UpdatePanel & Partial PostBack</li>
            </ol>
        </nav>

        <div class="d-flex justify-content-between align-items-center mb-4">
            <div>
                <h2 class="fw-bold mb-1"><i class="bi bi-lightning-charge text-secondary me-2"></i> UpdatePanel & Partial PostBack Lab</h2>
                <p class="text-muted mb-0">Learn how ScriptManager and UpdatePanel update specific page regions asynchronously without page flicker.</p>
            </div>
            <span class="badge bg-secondary px-3 py-2">Module 6</span>
        </div>

        <div class="row g-4">
            <!-- Left: Theory & Mechanics -->
            <div class="col-lg-6">
                <div class="card border-0 shadow-sm mb-4">
                    <div class="card-header bg-dark text-white fw-bold">
                        <i class="bi bi-book-half me-1 text-warning"></i> 1. The AJAX Triangle in Web Forms
                    </div>
                    <div class="card-body small">
                        <ol class="ps-3 mb-3">
                            <li class="mb-2"><strong>ScriptManager:</strong> Declared once in <code>Site.Master</code>; manages Microsoft Ajax client libraries and partial postback coordination.</li>
                            <li class="mb-2"><strong>UpdatePanel:</strong> Wraps any controls that should update asynchronously. Clicking a button inside it sends a background POST request and replaces only that panel's inner HTML!</li>
                            <li><strong>UpdateProgress:</strong> Automatically displays a loading spinner or message while the background request is in flight.</li>
                        </ol>

                        <div class="alert alert-secondary py-2 small mb-0">
                            <strong>Note:</strong> Under the hood, UpdatePanel still executes the full Page Life Cycle on the server, but only transmits the panel's updated HTML delta back across the wire!
                        </div>
                    </div>
                </div>

                <div class="card border-0 shadow-sm">
                    <div class="card-header bg-secondary text-white fw-bold">
                        <i class="bi bi-code-square me-1"></i> 2. Syntax Pattern
                    </div>
                    <div class="card-body p-0">
                        <pre class="bg-dark text-light p-3 rounded-bottom mb-0 small"><code>&lt;asp:UpdatePanel ID="upMain" runat="server"&gt;
    &lt;ContentTemplate&gt;
        &lt;asp:Label ID="lblClock" runat="server" /&gt;
        &lt;asp:Button ID="btnRefresh" runat="server" 
            Text="Refresh Async" OnClick="btnRefresh_Click" /&gt;
    &lt;/ContentTemplate&gt;
&lt;/asp:UpdatePanel&gt;</code></pre>
                    </div>
                </div>
            </div>

            <!-- Right: Interactive Experiment -->
            <div class="col-lg-6">
                <div class="card border-0 shadow-sm border-top border-4 border-secondary">
                    <div class="card-header bg-white fw-bold">
                        <i class="bi bi-play-circle-fill text-secondary me-1"></i> Live Asynchronous Sandbox
                    </div>
                    <div class="card-body">
                        <!-- Controls OUTSIDE UpdatePanel (Full Reload) -->
                        <div class="p-3 bg-light rounded border mb-3 text-center">
                            <small class="text-muted d-block fw-semibold mb-1">Standard Full-Page Timestamp (Will Reload Whole Page):</small>
                            <h5 class="fw-bold text-dark mb-2"><asp:Label ID="lblOuterClock" runat="server"></asp:Label></h5>
                            <asp:Button ID="btnFullPostBack" runat="server" Text="Full Page PostBack (Notice Flickering)" CssClass="btn btn-sm btn-outline-dark" OnClick="btnFullPostBack_Click" />
                        </div>

                        <!-- Controls INSIDE UpdatePanel (No Flicker) -->
                        <asp:UpdatePanel ID="upDemo" runat="server">
                            <ContentTemplate>
                                <div class="p-4 bg-success-subtle rounded border border-success text-center">
                                    <span class="badge bg-success mb-2">Inside &lt;asp:UpdatePanel&gt; (Async)</span>
                                    <h3 class="fw-bold text-success mb-1">
                                        <asp:Label ID="lblInnerClock" runat="server"></asp:Label>
                                    </h3>
                                    <small class="text-muted d-block mb-3">
                                        <asp:Label ID="lblClickCount" runat="server">0 clicks</asp:Label>
                                    </small>
                                    <asp:Button ID="btnAsyncClick" runat="server" Text="Click Me (Smooth Async Update!)" CssClass="btn btn-success px-4" OnClick="btnAsyncClick_Click" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <!-- Loading Indicator -->
                        <asp:UpdateProgress ID="upProgress" runat="server" AssociatedUpdatePanelID="upDemo">
                            <ProgressTemplate>
                                <div class="text-center mt-2 small text-muted">
                                    <span class="spinner-border spinner-border-sm me-1 text-success"></span> Updating panel in background...
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
