<%@ Page Title="Core Concepts Lab - Web Forms" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StateDemo.aspx.cs" Inherits="WebFormPracticeProject.StateDemo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-4">
        <div class="row mb-4">
            <div class="col-12">
                <h1 class="display-6 fw-bold text-success">Web Forms Core Concepts Lab</h1>
                <p class="text-muted">
                    Learn the three defining features of ASP.NET Web Forms:
                    <strong>Page LifeCycle (IsPostBack)</strong>, 
                    <strong>State Management (ViewState vs Session)</strong>, and 
                    <strong>Partial PostBacks (UpdatePanel / AJAX)</strong>.
                </p>
            </div>
        </div>

        <div class="row g-4">
            <!-- Concept 1: IsPostBack & Page LifeCycle -->
            <div class="col-md-6 col-lg-4">
                <div class="card h-100 shadow-sm border-0">
                    <div class="card-header bg-primary text-white">
                        <h5 class="card-title mb-0">1. IsPostBack & Life Cycle</h5>
                    </div>
                    <div class="card-body">
                        <p class="small text-muted">
                            Every time a button is clicked, Web Forms submits back to the same page. 
                            <code>IsPostBack</code> tells you if this is the initial load or a user action.
                        </p>
                        
                        <div class="p-3 bg-light rounded mb-3 small">
                            <div><strong>First Loaded At:</strong></div>
                            <asp:Label ID="lblFirstLoaded" runat="server" CssClass="text-primary fw-bold" />
                            <hr class="my-2" />
                            <div><strong>Current Execution At:</strong></div>
                            <asp:Label ID="lblCurrentExecution" runat="server" CssClass="text-secondary fw-bold" />
                            <hr class="my-2" />
                            <div><strong>Total PostBacks on this page:</strong></div>
                            <asp:Label ID="lblPostBackCount" runat="server" CssClass="badge bg-primary fs-6">0</asp:Label>
                        </div>

                        <asp:Button ID="btnTriggerPostBack" runat="server" Text="Trigger Full PostBack" CssClass="btn btn-primary w-100" OnClick="btnTriggerPostBack_Click" />
                    </div>
                    <div class="card-footer bg-transparent border-top-0 small text-muted">
                        <em>Notice how the whole browser tab flickers and reloads.</em>
                    </div>
                </div>
            </div>

            <!-- Concept 2: ViewState vs Session -->
            <div class="col-md-6 col-lg-4">
                <div class="card h-100 shadow-sm border-0">
                    <div class="card-header bg-warning text-dark">
                        <h5 class="card-title mb-0">2. ViewState vs Session</h5>
                    </div>
                    <div class="card-body">
                        <p class="small text-muted">
                            HTTP is stateless. Web Forms uses <strong>ViewState</strong> (encoded hidden field) for page-level state and <strong>Session</strong> (server memory) for user-level state.
                        </p>

                        <div class="row text-center mb-3">
                            <div class="col-6">
                                <div class="p-2 border rounded bg-light">
                                    <small class="text-muted d-block">ViewState</small>
                                    <asp:Label ID="lblViewStateVal" runat="server" CssClass="fs-3 fw-bold text-warning text-dark">0</asp:Label>
                                </div>
                            </div>
                            <div class="col-6">
                                <div class="p-2 border rounded bg-light">
                                    <small class="text-muted d-block">Session</small>
                                    <asp:Label ID="lblSessionVal" runat="server" CssClass="fs-3 fw-bold text-success">0</asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="d-grid gap-2">
                            <asp:Button ID="btnIncrementViewState" runat="server" Text="Increment ViewState (+1)" CssClass="btn btn-outline-warning text-dark" OnClick="btnIncrementViewState_Click" />
                            <asp:Button ID="btnIncrementSession" runat="server" Text="Increment Session (+1)" CssClass="btn btn-outline-success" OnClick="btnIncrementSession_Click" />
                        </div>
                    </div>
                    <div class="card-footer bg-transparent border-top-0 small text-muted">
                        <em>Try navigating to another page and returning: ViewState resets; Session persists!</em>
                    </div>
                </div>
            </div>

            <!-- Concept 3: AJAX & UpdatePanel -->
            <div class="col-md-12 col-lg-4">
                <div class="card h-100 shadow-sm border-0">
                    <div class="card-header bg-success text-white">
                        <h5 class="card-title mb-0">3. UpdatePanel (AJAX)</h5>
                    </div>
                    <div class="card-body">
                        <p class="small text-muted">
                            Don't want full-page reloads? Wrapping controls inside an <code>&lt;asp:UpdatePanel&gt;</code> does an asynchronous background POST!
                        </p>

                        <!-- UpdatePanel triggers async postbacks without refreshing the outer page -->
                        <asp:UpdatePanel ID="upAjaxDemo" runat="server">
                            <ContentTemplate>
                                <div class="p-3 bg-light rounded text-center mb-3">
                                    <span class="badge bg-success mb-2">Asynchronous Region</span>
                                    <h4 class="fw-bold mb-1">
                                        <asp:Label ID="lblServerClock" runat="server" Text="--:--:--" />
                                    </h4>
                                    <small class="text-muted">
                                        <asp:Label ID="lblAjaxClickCount" runat="server" Text="No clicks yet" />
                                    </small>
                                </div>

                                <asp:Button ID="btnAjaxClick" runat="server" Text="Update via AJAX (No Refresh!)" CssClass="btn btn-success w-100" OnClick="btnAjaxClick_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="card-footer bg-transparent border-top-0 small text-muted">
                        <em>Notice that the page scroll position and the rest of the page remain completely untouched.</em>
                    </div>
                </div>
            </div>
        </div>

        <!-- Comparison Table Guide -->
        <div class="row mt-5">
            <div class="col-12">
                <div class="card shadow-sm border-0">
                    <div class="card-header bg-dark text-white">
                        <h5 class="card-title mb-0">Quick Reference: Web Forms vs Modern MVC/Core</h5>
                    </div>
                    <div class="card-body p-0">
                        <div class="table-responsive">
                            <table class="table table-bordered mb-0">
                                <thead class="table-light">
                                    <tr>
                                        <th>Concept</th>
                                        <th>ASP.NET Web Forms</th>
                                        <th>ASP.NET Core / MVC</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td><strong>UI Paradigm</strong></td>
                                        <td>Event-driven, component-based (Buttons, DropDowns have server-side Click events)</td>
                                        <td>Action-based, RESTful routing (GET/POST mapped to Controller Actions)</td>
                                    </tr>
                                    <tr>
                                        <td><strong>HTML Rendering</strong></td>
                                        <td>Server Controls like <code>&lt;asp:TextBox&gt;</code> generate HTML tags</td>
                                        <td>Pure HTML5 with Tag Helpers (<code>asp-for="..."</code>) or Razor syntax</td>
                                    </tr>
                                    <tr>
                                        <td><strong>State Retention</strong></td>
                                        <td>Automatic via <code>ViewState</code> (hidden form field)</td>
                                        <td>Stateless (client handles state or passes model DTOs)</td>
                                    </tr>
                                    <tr>
                                        <td><strong>Ajax / Partial Refresh</strong></td>
                                        <td><code>&lt;asp:UpdatePanel&gt;</code> &amp; <code>ScriptManager</code></td>
                                        <td>Fetch API / JavaScript / Blazor components</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
