<%@ Page Title="Create Account" Language="C#" MasterPageFile="~/Layouts/Auth.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="DevDesk.Pages.Auth.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="w-100 my-4" style="max-width: 500px;">
        <div class="card shadow-sm border-0 rounded-3">
            <div class="card-header bg-primary text-white text-center py-4 rounded-top-3">
                <div class="display-6 mb-1"><i class="bi bi-person-plus-fill"></i></div>
                <h4 class="fw-bold mb-0">Join DevDesk</h4>
                <small class="text-white-50">Create your employee support account</small>
            </div>

            <div class="card-body p-4">
                <!-- Feedback Alert Panel -->
                <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-dismissible fade show" role="alert">
                    <asp:Label ID="lblAlertMessage" runat="server"></asp:Label>
                    <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </asp:Panel>

                <!-- Validation Summary -->
                <asp:ValidationSummary ID="valSummary" runat="server" CssClass="alert alert-danger" HeaderText="Please fix the following:" />

                <!-- Full Name -->
                <div class="mb-3">
                    <label for="txtFullName" class="form-label small fw-semibold">Full Name <span class="text-danger">*</span></label>
                    <div class="input-group">
                        <span class="input-group-text bg-light text-muted"><i class="bi bi-person-fill"></i></span>
                        <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" placeholder="e.g. John Doe"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="txtFullName"
                        ErrorMessage="Full Name is required." ForeColor="Red" Display="Dynamic" CssClass="small" />
                </div>

                <!-- Email -->
                <div class="mb-3">
                    <label for="txtEmail" class="form-label small fw-semibold">Company Email <span class="text-danger">*</span></label>
                    <div class="input-group">
                        <span class="input-group-text bg-light text-muted"><i class="bi bi-envelope-fill"></i></span>
                        <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control" placeholder="name@company.com"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                        ErrorMessage="Email is required." ForeColor="Red" Display="Dynamic" CssClass="small" />
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                        ValidationExpression="^[\w\.-]+@[\w\.-]+\.\w+$"
                        ErrorMessage="Please enter a valid email." ForeColor="Red" Display="Dynamic" CssClass="small" />
                </div>

                <!-- Department & Phone Row -->
                <div class="row g-2 mb-3">
                    <div class="col-sm-6">
                        <label for="ddlDepartment" class="form-label small fw-semibold">Department <span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-select">
                            <asp:ListItem Text="-- Select Department --" Value="" />
                            <asp:ListItem Text="Engineering" Value="Engineering" />
                            <asp:ListItem Text="Finance" Value="Finance" />
                            <asp:ListItem Text="Operations" Value="Operations" />
                            <asp:ListItem Text="Marketing" Value="Marketing" />
                            <asp:ListItem Text="Human Resources" Value="Human Resources" />
                            <asp:ListItem Text="IT Operations" Value="IT Operations" />
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvDept" runat="server" ControlToValidate="ddlDepartment"
                            InitialValue="" ErrorMessage="Please select your department." ForeColor="Red" Display="Dynamic" CssClass="small" />
                    </div>
                    <div class="col-sm-6">
                        <label for="txtPhone" class="form-label small fw-semibold">Phone Number</label>
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="e.g. 555-0199"></asp:TextBox>
                    </div>
                </div>

                <!-- Password -->
                <div class="mb-3">
                    <label for="txtPassword" class="form-label small fw-semibold">Password <span class="text-danger">*</span></label>
                    <div class="input-group">
                        <span class="input-group-text bg-light text-muted"><i class="bi bi-lock-fill"></i></span>
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="At least 6 characters"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                        ErrorMessage="Password is required." ForeColor="Red" Display="Dynamic" CssClass="small" />
                </div>

                <!-- Confirm Password -->
                <div class="mb-4">
                    <label for="txtConfirmPassword" class="form-label small fw-semibold">Confirm Password <span class="text-danger">*</span></label>
                    <div class="input-group">
                        <span class="input-group-text bg-light text-muted"><i class="bi bi-shield-lock-fill"></i></span>
                        <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Repeat password"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword"
                        ErrorMessage="Please confirm your password." ForeColor="Red" Display="Dynamic" CssClass="small" />
                    <asp:CompareValidator ID="cvPassword" runat="server" ControlToValidate="txtConfirmPassword"
                        ControlToCompare="txtPassword" Operator="Equal" Type="String"
                        ErrorMessage="Passwords do not match." ForeColor="Red" Display="Dynamic" CssClass="small" />
                </div>

                <!-- Submit Button -->
                <div class="d-grid mb-3">
                    <asp:Button ID="btnRegister" runat="server" Text="Create Account" CssClass="btn btn-primary btn-lg fw-semibold" OnClick="btnRegister_Click" />
                </div>

                <div class="text-center small text-muted">
                    Already have an account? 
                    <a href="Login.aspx" class="fw-semibold text-primary text-decoration-none">Sign in here</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
