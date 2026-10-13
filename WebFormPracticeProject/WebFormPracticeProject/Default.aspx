<%@ Page Title="Welcome to ASP.NET Web Forms" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebFormPracticeProject._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="py-4">
        <!-- Hero Section -->
        <div class="p-5 mb-4 bg-light rounded-3 shadow-sm border">
            <div class="container-fluid py-2">
                <span class="badge bg-primary mb-2 px-3 py-2">ASP.NET Web Forms Starter</span>
                <h1 class="display-5 fw-bold">Mastering ASP.NET Web Forms</h1>
                <p class="col-md-9 fs-5 text-muted">
                    Welcome! Web Forms was created to bring desktop event-driven programming (like Windows Forms) to the web. 
                    Instead of manually writing HTTP requests and JSON endpoints, you drop server controls into ASPX pages and handle events like button clicks directly in C#.
                </p>
                <div class="d-flex gap-3">
                    <a href="Students.aspx" class="btn btn-primary btn-lg px-4">Open Student Demo &raquo;</a>
                    <a href="StateDemo.aspx" class="btn btn-outline-success btn-lg px-4">Core Concepts Lab &raquo;</a>
                </div>
            </div>
        </div>

        <!-- 3 Pillars of Web Forms -->
        <div class="row g-4 mb-4">
            <div class="col-md-4">
                <div class="card h-100 shadow-sm border-0">
                    <div class="card-body">
                        <div class="text-primary fs-3 mb-2">&#9881;</div>
                        <h4 class="card-title fw-bold">1. Server Controls</h4>
                        <p class="card-text text-muted">
                            Controls with <code>runat="server"</code> (like <code>&lt;asp:TextBox&gt;</code> or <code>&lt;asp:GridView&gt;</code>) live on the server during request execution and render standard HTML before sending it to the browser.
                        </p>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="card h-100 shadow-sm border-0">
                    <div class="card-body">
                        <div class="text-warning fs-3 mb-2">&#128260;</div>
                        <h4 class="card-title fw-bold">2. PostBack & LifeCycle</h4>
                        <p class="card-text text-muted">
                            Forms submit back to the same URL. ASP.NET triggers <code>Page_Init</code>, <code>Page_Load</code>, your control events (e.g. <code>btnSave_Click</code>), and finally renders the page.
                        </p>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="card h-100 shadow-sm border-0">
                    <div class="card-body">
                        <div class="text-success fs-3 mb-2">&#128190;</div>
                        <h4 class="card-title fw-bold">3. State (ViewState)</h4>
                        <p class="card-text text-muted">
                            Web Forms keeps server control state alive across postbacks by encoding state in a hidden input field called <code>__VIEWSTATE</code>, giving the illusion of a stateful application.
                        </p>
                    </div>
                </div>
            </div>
        </div>

        <!-- What to explore -->
        <div class="card border-0 shadow-sm bg-light">
            <div class="card-body p-4">
                <h4 class="fw-bold mb-3">What's included in this mini-project:</h4>
                <div class="row g-3">
                    <div class="col-md-6">
                        <div class="p-3 bg-white rounded border">
                            <h5 class="fw-bold text-primary">Student Management (Students.aspx)</h5>
                            <p class="text-muted small mb-2">A full interactive page showing:</p>
                            <ul class="small mb-0 ps-3">
                                <li>Form controls: TextBox, DropDownList, RadioButtonList, CheckBox</li>
                                <li>Client + Server Validators: RequiredFieldValidator, RangeValidator, RegexValidator</li>
                                <li>GridView DataBinding with dynamic row counting and row deletion</li>
                                <li>Search / Filter filtering lists in real time</li>
                            </ul>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="p-3 bg-white rounded border">
                            <h5 class="fw-bold text-success">Core Concepts Lab (StateDemo.aspx)</h5>
                            <p class="text-muted small mb-2">Hands-on interactive experiments showing:</p>
                            <ul class="small mb-0 ps-3">
                                <li>Visualizing the difference between <code>!IsPostBack</code> (GET) and PostBack (POST)</li>
                                <li>Comparison of page-level <code>ViewState</code> vs user-level <code>Session</code></li>
                                <li>Asynchronous partial page update using <code>&lt;asp:UpdatePanel&gt;</code> without full reload</li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </main>
</asp:Content>
