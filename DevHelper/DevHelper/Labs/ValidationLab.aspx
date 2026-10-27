<%@ Page Title="Validation Controls Lab" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ValidationLab.aspx.cs" Inherits="DevHelper.Labs.ValidationLab" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="py-2">
        <nav aria-label="breadcrumb">
            <ol class="breadcrumb mb-2 small">
                <li class="breadcrumb-item"><a href="~/Default.aspx" runat="server">Learning Hub</a></li>
                <li class="breadcrumb-item">Module 2: Controls & Validation</li>
                <li class="breadcrumb-item active" aria-current="page">Server Controls & Two-Layer Validation</li>
            </ol>
        </nav>

        <div class="d-flex justify-content-between align-items-center mb-4">
            <div>
                <h2 class="fw-bold mb-1"><i class="bi bi-input-cursor-text text-success me-2"></i> Two-Layer Validation Lab</h2>
                <p class="text-muted mb-0">Learn how ASP.NET validates simultaneously on the client (browser JS) and server (C# Page.IsValid).</p>
            </div>
            <span class="badge bg-success px-3 py-2">Module 2</span>
        </div>

        <div class="row g-4">
            <!-- Left: Theory & Reference -->
            <div class="col-lg-6">
                <div class="card border-0 shadow-sm mb-4">
                    <div class="card-header bg-dark text-white fw-bold">
                        <i class="bi bi-book-half me-1 text-warning"></i> 1. The 6 Built-In ASP.NET Validators
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <table class="table table-sm table-bordered small mb-3">
                                <thead class="table-light">
                                    <tr><th>Validator</th><th>Purpose</th><th>Key Attributes</th></tr>
                                </thead>
                                <tbody>
                                    <tr><td><code>RequiredFieldValidator</code></td><td>Field cannot be empty</td><td><code>InitialValue=""</code></td></tr>
                                    <tr><td><code>RangeValidator</code></td><td>Number/Date within bounds</td><td><code>MinimumValue, MaximumValue, Type</code></td></tr>
                                    <tr><td><code>RegularExpressionValidator</code></td><td>Pattern (Email, Phone)</td><td><code>ValidationExpression</code></td></tr>
                                    <tr><td><code>CompareValidator</code></td><td>Password match / greater than</td><td><code>ControlToCompare, Operator</code></td></tr>
                                    <tr><td><code>CustomValidator</code></td><td>Custom business logic</td><td><code>ClientValidationFunction, OnServerValidate</code></td></tr>
                                    <tr><td><code>ValidationSummary</code></td><td>Lists all errors in one alert</td><td><code>HeaderText, ShowSummary</code></td></tr>
                                </tbody>
                            </table>
                        </div>

                        <div class="alert alert-info py-2 small mb-0">
                            <strong><i class="bi bi-shield-check me-1"></i> Two-Layer Defense:</strong>
                            Even if a user disables JavaScript or posts malicious payloads directly via Postman, <code>Page.IsValid</code> in C# stops the request before touching your database.
                        </div>
                    </div>
                </div>

                <div class="card border-0 shadow-sm">
                    <div class="card-header bg-secondary text-white fw-bold">
                        <i class="bi bi-code-square me-1"></i> 2. The Golden Code-Behind Pattern
                    </div>
                    <div class="card-body p-0">
                        <pre class="bg-dark text-light p-3 rounded-bottom mb-0 small"><code>protected void btnSubmit_Click(object sender, EventArgs e)
{
    // Layer 2: Server validation check
    if (!Page.IsValid)
    {
        return; // HALT! Do not execute database queries!
    }

    // Safe to process verified data
    SaveToDatabase();
}</code></pre>
                    </div>
                </div>
            </div>

            <!-- Right: Live Interactive Sandbox -->
            <div class="col-lg-6">
                <div class="card border-0 shadow-sm mb-4 border-top border-4 border-success">
                    <div class="card-header bg-white fw-bold d-flex justify-content-between align-items-center">
                        <span><i class="bi bi-play-circle-fill text-success me-1"></i> Live Validation Sandbox</span>
                        <asp:Label ID="lblValidationResult" runat="server" CssClass="badge bg-secondary">Awaiting Input</asp:Label>
                    </div>
                    <div class="card-body">
                        <!-- Validation Summary Block -->
                        <asp:ValidationSummary ID="valSummary" runat="server" CssClass="alert alert-danger" HeaderText="Please fix the following validation errors:" />

                        <!-- Required Text -->
                        <div class="mb-3">
                            <label class="form-label small fw-semibold">Employee Name <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Required field"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
                                ErrorMessage="Employee Name is mandatory." ForeColor="Red" Display="Dynamic" CssClass="small" />
                        </div>

                        <!-- Email Regex -->
                        <div class="mb-3">
                            <label class="form-label small fw-semibold">Email Address <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="name@company.com"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                ErrorMessage="Email is mandatory." ForeColor="Red" Display="Dynamic" CssClass="small" />
                            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                ValidationExpression="^[\w\.-]+@[\w\.-]+\.\w+$"
                                ErrorMessage="Invalid email address format." ForeColor="Red" Display="Dynamic" CssClass="small" />
                        </div>

                        <!-- Age Range (18 - 65) -->
                        <div class="mb-3">
                            <label class="form-label small fw-semibold">Age (18 to 65) <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtAge" runat="server" TextMode="Number" CssClass="form-control" placeholder="18 - 65"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvAge" runat="server" ControlToValidate="txtAge"
                                ErrorMessage="Age is mandatory." ForeColor="Red" Display="Dynamic" CssClass="small" />
                            <asp:RangeValidator ID="rvAge" runat="server" ControlToValidate="txtAge"
                                MinimumValue="18" MaximumValue="65" Type="Integer"
                                ErrorMessage="Age must be between 18 and 65." ForeColor="Red" Display="Dynamic" CssClass="small" />
                        </div>

                        <!-- Password & Confirm Password (CompareValidator) -->
                        <div class="row g-2 mb-3">
                            <div class="col-sm-6">
                                <label class="form-label small fw-semibold">Password <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPass" runat="server" ControlToValidate="txtPassword"
                                    ErrorMessage="Password is required." ForeColor="Red" Display="Dynamic" CssClass="small" />
                            </div>
                            <div class="col-sm-6">
                                <label class="form-label small fw-semibold">Confirm Password <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                                <asp:CompareValidator ID="cvPass" runat="server" ControlToValidate="txtConfirmPassword"
                                    ControlToCompare="txtPassword" Operator="Equal" Type="String"
                                    ErrorMessage="Passwords do not match." ForeColor="Red" Display="Dynamic" CssClass="small" />
                            </div>
                        </div>

                        <!-- Buttons: Submit vs Clear (CausesValidation=false) -->
                        <div class="d-flex gap-2">
                            <asp:Button ID="btnValidate" runat="server" Text="Test Validation" CssClass="btn btn-success px-4" OnClick="btnValidate_Click" />
                            <asp:Button ID="btnClear" runat="server" Text="Clear (No Validation)" CssClass="btn btn-outline-secondary" CausesValidation="false" OnClick="btnClear_Click" />
                        </div>
                    </div>
                </div>

                <!-- Live Server Feedback Banner -->
                <asp:Panel ID="pnlFeedback" runat="server" Visible="false" CssClass="alert alert-success">
                    <asp:Label ID="lblFeedback" runat="server"></asp:Label>
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>
