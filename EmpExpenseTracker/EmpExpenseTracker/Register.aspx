<%@ Page Title="Register - Employee Expense Tracker" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="EmpExpenseTracker.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-5">
        <div class="row justify-content-center">
            <div class="col-md-6 col-lg-5">
                <div class="card shadow border-0 rounded-3">
                    <div class="card-header bg-primary text-white text-center py-3">
                        <h4 class="mb-0 fw-bold">Create an Account</h4>
                        <small class="opacity-75">Sign up to manage and track your expenses</small>
                    </div>
                    <div class="card-body p-4">
                        <!-- Alert Notification Message Panel -->
                        <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-dismissible fade show" role="alert">
                            <asp:Label ID="lblAlertMessage" runat="server"></asp:Label>
                            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                        </asp:Panel>

                        <!-- Validation Summary: Lists all form errors -->
                        <asp:ValidationSummary ID="valSummary" runat="server" CssClass="alert alert-danger" HeaderText="Please fix the following:" />

                        <!-- 1. Full Name -->
                        <div class="mb-3">
                            <label for="txtFullName" class="form-label fw-semibold">Full Name <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" placeholder="e.g. John Doe"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="txtFullName"
                                ErrorMessage="Full Name is required." ForeColor="Red" Display="Dynamic" CssClass="small" />
                        </div>

                        <!-- 2. Email Address -->
                        <div class="mb-3">
                            <label for="txtEmail" class="form-label fw-semibold">Email Address <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control" placeholder="name@company.com"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                ErrorMessage="Email is required." ForeColor="Red" Display="Dynamic" CssClass="small" />
                            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                ValidationExpression="^[\w\.-]+@[\w\.-]+\.\w+$"
                                ErrorMessage="Please enter a valid email address." ForeColor="Red" Display="Dynamic" CssClass="small" />
                        </div>

                        <!-- 3. Password -->
                        <div class="mb-3">
                            <label for="txtPassword" class="form-label fw-semibold">Password <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="At least 6 characters"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                                ErrorMessage="Password is required." ForeColor="Red" Display="Dynamic" CssClass="small" />
                        </div>

                        <!-- 4. Confirm Password with CompareValidator -->
                        <div class="mb-3">
                            <label for="txtConfirmPassword" class="form-label fw-semibold">Confirm Password <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Repeat password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword"
                                ErrorMessage="Please confirm your password." ForeColor="Red" Display="Dynamic" CssClass="small" />
                            <asp:CompareValidator ID="cvPassword" runat="server" 
                                ControlToValidate="txtConfirmPassword" 
                                ControlToCompare="txtPassword" 
                                Operator="Equal" 
                                Type="String" 
                                ErrorMessage="Passwords do not match." ForeColor="Red" Display="Dynamic" CssClass="small" />
                        </div>

                        <!-- 5. Role (Employee / Admin) -->
                        <div class="mb-4">
                            <label for="ddlRole" class="form-label fw-semibold">Role</label>
                            <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-select">
                                <asp:ListItem Text="Employee" Value="Employee" Selected="True" />
                                <asp:ListItem Text="Admin / Manager" Value="Admin" />
                            </asp:DropDownList>
                        </div>

                        <!-- Submit Button -->
                        <div class="d-grid mb-3">
                            <asp:Button ID="btnRegister" runat="server" Text="Create Account" CssClass="btn btn-primary btn-lg" OnClick="btnRegister_Click" />
                        </div>

                        <!-- Login link -->
                        <div class="text-center">
                            <span class="text-muted small">Already have an account? </span>
                            <a href="Login.aspx" class="text-decoration-none fw-semibold">Sign In here</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
