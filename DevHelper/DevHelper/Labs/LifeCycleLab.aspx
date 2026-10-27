<%@ Page Title="Page Life Cycle Lab" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LifeCycleLab.aspx.cs" Inherits="DevHelper.Labs.LifeCycleLab" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="py-2">
        <!-- Breadcrumb & Topic Header -->
        <nav aria-label="breadcrumb">
            <ol class="breadcrumb mb-2 small">
                <li class="breadcrumb-item"><a href="~/Default.aspx" runat="server">Learning Hub</a></li>
                <li class="breadcrumb-item">Module 1: Architecture</li>
                <li class="breadcrumb-item active" aria-current="page">Page Life Cycle & IsPostBack</li>
            </ol>
        </nav>

        <div class="d-flex justify-content-between align-items-center mb-4">
            <div>
                <h2 class="fw-bold mb-1"><i class="bi bi-diagram-3-fill text-primary me-2"></i> Page Life Cycle & IsPostBack</h2>
                <p class="text-muted mb-0">Understand execution order and master state persistence across stateless HTTP.</p>
            </div>
            <div class="d-flex align-items-center gap-2">
                <span class="badge bg-success px-3 py-2">Module 1</span>
                <span class="badge bg-secondary px-3 py-2">Beginner</span>
            </div>
        </div>

        <!-- Split Screen: Left = Theory / Right = Live Playground -->
        <div class="row g-4">
            <!-- Left Side: Theory & Blueprint -->
            <div class="col-lg-6">
                <div class="card border-0 shadow-sm mb-4">
                    <div class="card-header bg-dark text-white fw-bold">
                        <i class="bi bi-book-half me-1 text-warning"></i> 1. The Core Mental Model
                    </div>
                    <div class="card-body">
                        <p class="small text-muted">
                            Every time a browser sends a request (whether opening a URL or clicking a button),
                            ASP.NET <strong>creates a brand new instance of your Page class</strong> on the server.
                        </p>

                        <div class="p-3 bg-light rounded border mb-3">
                            <h6 class="fw-bold text-primary mb-2">The 9 Life Cycle Stages:</h6>
                            <ol class="small mb-0 ps-3">
                                <li><strong>PreInit:</strong> Theme & Master Page are applied.</li>
                                <li><strong>Init:</strong> Server controls are initialized with default properties.</li>
                                <li><strong>LoadViewState:</strong> Control state restored from hidden <code>__VIEWSTATE</code>.</li>
                                <li><strong>Page_Load:</strong> Data is initialized. <em>(Always check <code>if (!IsPostBack)</code> here!)</em></li>
                                <li><strong>Control Events:</strong> Button click handlers execute (e.g. <code>btnClick</code>).</li>
                                <li><strong>PreRender:</strong> Final tweaks before HTML is created.</li>
                                <li><strong>SaveViewState:</strong> Current control properties serialized to <code>__VIEWSTATE</code>.</li>
                                <li><strong>Render:</strong> ASPX controls turn into HTML sent to browser.</li>
                                <li><strong>Unload:</strong> Page instance is destroyed from server RAM.</li>
                            </ol>
                        </div>

                        <div class="alert alert-warning py-2 small mb-0">
                            <strong><i class="bi bi-exclamation-triangle-fill me-1"></i> The #1 Beginner Mistake:</strong>
                            Loading data in <code>Page_Load</code> without <code>if (!IsPostBack)</code> will overwrite your user's edits every time they click a button!
                        </div>
                    </div>
                </div>

                <!-- Theory Code Snippet -->
                <div class="card border-0 shadow-sm">
                    <div class="card-header bg-secondary text-white fw-bold">
                        <i class="bi bi-code-square me-1"></i> 2. Syntax & Implementation Pattern
                    </div>
                    <div class="card-body p-0">
                        <pre class="bg-dark text-light p-3 rounded-bottom mb-0 small" style="overflow-x:auto;"><code>protected void Page_Load(object sender, EventArgs e)
{
    // Executes on EVERY request (GET and POST)
    lblCurrentTime.Text = DateTime.Now.ToString("T");

    // Runs ONLY when user first visits the page (HTTP GET)
    if (!IsPostBack)
    {
        txtUsername.Text = "DefaultUser";
        LoadInitialDatabaseRecords();
    }
}

protected void btnSubmit_Click(object sender, EventArgs e)
{
    // Runs in Step 5 (Control Events) AFTER Page_Load has run!
    string user = txtUsername.Text;
    SaveToDatabase(user);
}</code></pre>
                    </div>
                </div>
            </div>

            <!-- Right Side: Live Interactive Sandbox -->
            <div class="col-lg-6">
                <div class="card border-0 shadow-sm mb-4 border-top border-4 border-primary">
                    <div class="card-header bg-white fw-bold d-flex justify-content-between align-items-center">
                        <span><i class="bi bi-play-circle-fill text-primary me-1"></i> Live Interactive Sandbox</span>
                        <asp:Label ID="lblRequestType" runat="server" CssClass="badge bg-info text-dark">Initial GET</asp:Label>
                    </div>
                    <div class="card-body">
                        <p class="small text-muted mb-3">
                            Interact with the controls below. Notice how the <strong>Server Execution Timeline</strong> updates in real time to show the exact lifecycle steps executed!
                        </p>

                        <!-- Experiment Input -->
                        <div class="mb-3">
                            <label for="txtTestInput" class="form-label fw-semibold small">Type any test value:</label>
                            <asp:TextBox ID="txtTestInput" runat="server" CssClass="form-control" placeholder="Type something here..."></asp:TextBox>
                        </div>

                        <div class="row g-2 mb-3">
                            <div class="col-sm-6">
                                <asp:Button ID="btnPostBack" runat="server" Text="Trigger PostBack" CssClass="btn btn-primary w-100" OnClick="btnPostBack_Click" />
                            </div>
                            <div class="col-sm-6">
                                <asp:Button ID="btnResetLog" runat="server" Text="Reset Timeline" CssClass="btn btn-outline-secondary w-100" OnClick="btnResetLog_Click" />
                            </div>
                        </div>

                        <!-- Live Lifecycle Timeline Display -->
                        <h6 class="fw-bold small text-secondary mb-2">Server Execution Timeline:</h6>
                        <div class="p-3 bg-dark text-white rounded font-monospace small" style="max-height: 250px; overflow-y: auto;">
                            <asp:Literal ID="litTimelineLog" runat="server"></asp:Literal>
                        </div>
                    </div>
                </div>

                <!-- Personal Practice Notes Box (Stored to SQL) -->
                <div class="card border-0 shadow-sm border-top border-4 border-success">
                    <div class="card-header bg-white fw-bold d-flex justify-content-between align-items-center">
                        <span><i class="bi bi-journal-text text-success me-1"></i> My Topic Reflections & Notes</span>
                        <asp:Label ID="lblNoteStatus" runat="server" CssClass="text-success small fw-normal"></asp:Label>
                    </div>
                    <div class="card-body">
                        <p class="small text-muted mb-2">Write your own summary or code reflections. This is persisted for review.</p>
                        <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control mb-2" placeholder="e.g. Remember that button click events run AFTER Page_Load..."></asp:TextBox>
                        <div class="d-flex justify-content-between align-items-center">
                            <asp:Button ID="btnSaveNotes" runat="server" Text="Save Notes" CssClass="btn btn-sm btn-success px-3" OnClick="btnSaveNotes_Click" />
                            <a href="~/Default.aspx" runat="server" class="btn btn-sm btn-outline-dark">&laquo; Back to All Modules</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
