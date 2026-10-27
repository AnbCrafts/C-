<%@ Page Title="Web Methods to Web API Lab" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebMethodLab.aspx.cs" Inherits="DevHelper.Labs.WebMethodLab" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="py-2">
        <nav aria-label="breadcrumb">
            <ol class="breadcrumb mb-2 small">
                <li class="breadcrumb-item"><a href="~/Default.aspx" runat="server">Learning Hub</a></li>
                <li class="breadcrumb-item">Module 8: Bridge to Web API</li>
                <li class="breadcrumb-item active" aria-current="page">Web Methods & JSON Endpoints</li>
            </ol>
        </nav>

        <div class="d-flex justify-content-between align-items-center mb-4">
            <div>
                <h2 class="fw-bold mb-1"><i class="bi bi-arrow-left-right text-primary me-2"></i> Web Forms to Web API Bridge Lab</h2>
                <p class="text-muted mb-0">Learn how to expose server-side static [WebMethod] endpoints that return JSON to modern JavaScript fetch() requests.</p>
            </div>
            <span class="badge bg-primary px-3 py-2">Module 8</span>
        </div>

        <div class="row g-4">
            <!-- Left: Theory & Transition Map -->
            <div class="col-lg-6">
                <div class="card border-0 shadow-sm mb-4">
                    <div class="card-header bg-dark text-white fw-bold">
                        <i class="bi bi-book-half me-1 text-warning"></i> 1. The Paradigm Shift: Web Forms vs Web API
                    </div>
                    <div class="card-body small">
                        <div class="table-responsive">
                            <table class="table table-bordered mb-3">
                                <thead class="table-light">
                                    <tr><th>Feature</th><th>ASP.NET Web Forms</th><th>ASP.NET Web API</th></tr>
                                </thead>
                                <tbody>
                                    <tr><td><strong>Output</strong></td><td>HTML page with markup</td><td>JSON / XML data</td></tr>
                                    <tr><td><strong>State</strong></td><td>Stateful (ViewState, Session)</td><td>Stateless (Tokens / JWT)</td></tr>
                                    <tr><td><strong>Client</strong></td><td>Browser desktop-like views</td><td>Any client (React, Angular, Mobile, cURL)</td></tr>
                                    <tr><td><strong>Routing</strong></td><td>Direct file paths (<code>Page.aspx</code>)</td><td>RESTful routes (<code>api/expenses/5</code>)</td></tr>
                                </tbody>
                            </table>
                        </div>

                        <div class="alert alert-primary py-2 mb-0">
                            <strong>The Stepping Stone:</strong>
                            Before building full Web API controllers, Web Forms allows decorating C# methods with <code>[System.Web.Services.WebMethod]</code> so client-side JavaScript can call C# directly without a full page reload!
                        </div>
                    </div>
                </div>

                <div class="card border-0 shadow-sm">
                    <div class="card-header bg-secondary text-white fw-bold">
                        <i class="bi bi-code-square me-1"></i> 2. The [WebMethod] C# Signature
                    </div>
                    <div class="card-body p-0">
                        <pre class="bg-dark text-light p-3 rounded-bottom mb-0 small"><code>[System.Web.Services.WebMethod]
public static object GetLiveServerData(string query)
{
    // MUST be static in Web Forms!
    return new {
        ServerTime = DateTime.Now.ToString("T"),
        Echo = query,
        Status = "200 OK"
    };
}</code></pre>
                    </div>
                </div>
            </div>

            <!-- Right: Interactive Client-Side Fetch Sandbox -->
            <div class="col-lg-6">
                <div class="card border-0 shadow-sm border-top border-4 border-primary">
                    <div class="card-header bg-white fw-bold d-flex justify-content-between align-items-center">
                        <span><i class="bi bi-play-circle-fill text-primary me-1"></i> Live JavaScript fetch() to C# [WebMethod]</span>
                        <span class="badge bg-info text-dark">JSON Endpoint</span>
                    </div>
                    <div class="card-body">
                        <p class="small text-muted mb-3">
                            Click below to invoke the C# static backend method directly using JavaScript's modern <code>fetch()</code> API. Zero page flickers, pure JSON payload!
                        </p>

                        <div class="input-group mb-3">
                            <input type="text" id="clientQueryInput" class="form-control" placeholder="Type text to send to server..." value="Hello from browser fetch()!" />
                            <button type="button" class="btn btn-primary" onclick="callWebMethod();">
                                <i class="bi bi-send-fill me-1"></i> Call [WebMethod]
                            </button>
                        </div>

                        <h6 class="fw-bold small text-secondary mb-1">Server JSON Response:</h6>
                        <pre id="jsonOutput" class="bg-dark text-success p-3 rounded small font-monospace" style="min-height: 120px;">// Click the button to fetch live JSON from C# backend...</pre>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Client-side JavaScript calling C# WebMethod -->
    <script type="text/javascript">
        function callWebMethod() {
            var inputVal = document.getElementById('clientQueryInput').value;
            var out = document.getElementById('jsonOutput');
            out.innerText = "Connecting to server...";

            fetch('WebMethodLab.aspx/GetLiveServerData', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8'
                },
                body: JSON.stringify({ query: inputVal })
            })
            .then(function(res) { return res.json(); })
            .then(function(data) {
                out.innerText = JSON.stringify(data.d, null, 2);
            })
            .catch(function(err) {
                out.innerText = "Error: " + err;
            });
        }
    </script>
</asp:Content>
