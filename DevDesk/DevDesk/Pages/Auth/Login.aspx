<%@ Page Title="Sign In" Language="C#" MasterPageFile="~/Layouts/Auth.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="DevDesk.Pages.Auth.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="w-100" style="max-width: 440px;">
        <div class="card shadow-sm border-0 rounded-3">
            <div class="card-header bg-primary text-white text-center py-4 rounded-top-3">
                <div class="display-6 mb-1"><i class="bi bi-person-circle"></i></div>
                <h4 class="fw-bold mb-0">Welcome Back</h4>
                <small class="text-white-50">Sign in to your DevDesk account</small>
            </div>

            <div class="card-body p-4">
                <!-- Alert Feedback Banner -->
                <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-dismissible fade show" role="alert">
                    <asp:Label ID="lblAlertMessage" runat="server"></asp:Label>
                    <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </asp:Panel>

                <!-- Email Input -->
                <div class="mb-3">
                    <label for="txtEmail" class="form-label small fw-semibold">Email Address <span class="text-danger">*</span></label>
                    <div class="input-group">
                        <span class="input-group-text bg-light text-muted"><i class="bi bi-envelope-fill"></i></span>
                        <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control" placeholder="name@company.com"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                        ErrorMessage="Email address is required." ForeColor="Red" Display="Dynamic" CssClass="small" />
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                        ValidationExpression="^[\w\.-]+@[\w\.-]+\.\w+$"
                        ErrorMessage="Enter a valid email address." ForeColor="Red" Display="Dynamic" CssClass="small" />
                </div>

                <!-- Password Input -->
                <div class="mb-3">
                    <label for="txtPassword" class="form-label small fw-semibold">Password <span class="text-danger">*</span></label>
                    <div class="input-group">
                        <span class="input-group-text bg-light text-muted"><i class="bi bi-lock-fill"></i></span>
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="••••••••"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                        ErrorMessage="Password is required." ForeColor="Red" Display="Dynamic" CssClass="small" />
                </div>

                <!-- Remember Me Checkbox -->
                <div class="mb-4 form-check d-flex justify-content-between align-items-center">
                    <div>
                        <asp:CheckBox ID="chkRememberMe" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label small text-muted" for="ctl00_ContentPlaceHolder1_chkRememberMe">Remember me</label>
                    </div>
                    <a href="#" class="small text-decoration-none">Forgot password?</a>
                </div>

                <!-- Submit Button -->
                <div class="d-grid mb-3">
                    <asp:Button ID="btnLogin" runat="server" Text="Sign In" CssClass="btn btn-primary btn-lg fw-semibold" OnClick="btnLogin_Click" />
                </div>

                <div class="text-center small text-muted">
                    Don't have an account? 
                    <a href="Register.aspx" class="fw-semibold text-primary text-decoration-none">Create one</a>
                </div>
            </div>

            <!-- Demo Logins Helper Card Footer -->
            <div class="card-footer bg-light p-3 border-top-0 rounded-bottom-3 small">
                <div class="text-uppercase fw-bold text-muted mb-2" style="font-size: 0.75rem;">Demo Accounts (Password: <code>User@123</code>)</div>
                <div class="d-flex justify-content-between small text-secondary">
                    <span><strong>Admin:</strong> admin@devdesk.com</span>
                </div>
                <div class="d-flex justify-content-between small text-secondary">
                    <span><strong>IT Support:</strong> support@devdesk.com</span>
                </div>
                <div class="d-flex justify-content-between small text-secondary">
                    <span><strong>Employee:</strong> emily@company.com</span>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
