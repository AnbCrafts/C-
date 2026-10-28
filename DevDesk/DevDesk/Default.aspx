<%@ Page Title="Welcome to DevDesk" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DevDesk._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Hero Banner -->
    <div class="bg-primary text-white py-5 shadow-sm">
        <div class="container py-4">
            <div class="row align-items-center g-4">
                <div class="col-lg-7">
                    <span class="badge bg-light text-primary px-3 py-2 rounded-pill fw-semibold mb-3">
                        <i class="bi bi-shield-check text-warning me-1"></i> DevDesk IT Management Portal v1.0
                    </span>
                    <h1 class="display-5 fw-bold mb-3">Enterprise IT Support &amp; Asset Lifecycle Tracking</h1>
                    <p class="lead mb-4 text-white-50">
                        Streamline incident ticketing, hardware inventory, and employee support requests in one unified, high-performance platform.
                    </p>
                    <div class="d-flex flex-wrap gap-3">
                        <a href="Pages/Auth/Login.aspx" class="btn btn-warning btn-lg fw-bold px-4 shadow-sm text-dark">
                            <i class="bi bi-box-arrow-in-right me-1"></i> Employee Portal Login
                        </a>
                        <a href="Pages/Auth/Register.aspx" class="btn btn-outline-light btn-lg px-4">
                            <i class="bi bi-person-plus me-1"></i> Create Account
                        </a>
                    </div>
                </div>
                <div class="col-lg-5 text-center">
                    <div class="p-4 bg-white bg-opacity-10 rounded-4 border border-white border-opacity-25 shadow">
                        <div class="display-1 text-warning mb-2">
                            <i class="bi bi-headset"></i>
                        </div>
                        <h4 class="fw-bold">Fast-Track Support</h4>
                        <p class="small text-white-50 mb-3">Need urgent IT assistance or hardware allocation? DevDesk dispatches tickets directly to designated support engineers.</p>
                        <div class="row g-2 text-center">
                            <div class="col-4">
                                <div class="bg-dark bg-opacity-25 p-2 rounded">
                                    <div class="fw-bold fs-5 text-warning">&lt; 1 hr</div>
                                    <small class="text-white-50" style="font-size: 0.75rem;">Avg Response</small>
                                </div>
                            </div>
                            <div class="col-4">
                                <div class="bg-dark bg-opacity-25 p-2 rounded">
                                    <div class="fw-bold fs-5 text-info">99.8%</div>
                                    <small class="text-white-50" style="font-size: 0.75rem;">SLA Met</small>
                                </div>
                            </div>
                            <div class="col-4">
                                <div class="bg-dark bg-opacity-25 p-2 rounded">
                                    <div class="fw-bold fs-5 text-success">RBAC</div>
                                    <small class="text-white-50" style="font-size: 0.75rem;">Enterprise</small>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Core Features Grid -->
    <div class="container py-5">
        <div class="text-center mb-5">
            <h2 class="fw-bold">Platform Capabilities</h2>
            <p class="text-muted">Built from the ground up for reliable IT operations and asset governance</p>
        </div>

        <div class="row g-4">
            <!-- Feature 1 -->
            <div class="col-md-4">
                <div class="card h-100 shadow-sm border-0 rounded-3 hover-shadow">
                    <div class="card-body p-4 text-center">
                        <div class="rounded-circle bg-primary bg-opacity-10 text-primary d-inline-flex align-items-center justify-content-center mb-3" style="width: 64px; height: 64px;">
                            <i class="bi bi-ticket-perforated-fill fs-2"></i>
                        </div>
                        <h5 class="fw-bold mb-2">Incident &amp; Ticket Tracking</h5>
                        <p class="text-muted small">
                            Submit, prioritize, and monitor hardware/software issues. Dynamic status transitions from Open &rarr; In Progress &rarr; Resolved.
                        </p>
                        <a href="Pages/Tickets/List.aspx" class="text-primary fw-semibold small text-decoration-none">
                            View Ticket Queue <i class="bi bi-arrow-right"></i>
                        </a>
                    </div>
                </div>
            </div>

            <!-- Feature 2 -->
            <div class="col-md-4">
                <div class="card h-100 shadow-sm border-0 rounded-3 hover-shadow">
                    <div class="card-body p-4 text-center">
                        <div class="rounded-circle bg-success bg-opacity-10 text-success d-inline-flex align-items-center justify-content-center mb-3" style="width: 64px; height: 64px;">
                            <i class="bi bi-laptop-fill fs-2"></i>
                        </div>
                        <h5 class="fw-bold mb-2">Hardware Asset Inventory</h5>
                        <p class="text-muted small">
                            Comprehensive tracking of enterprise assets: Laptops, Monitors, and Accessories tagged with serial numbers and assigned employees.
                        </p>
                        <a href="Pages/Assets/List.aspx" class="text-success fw-semibold small text-decoration-none">
                            Browse Asset Registry <i class="bi bi-arrow-right"></i>
                        </a>
                    </div>
                </div>
            </div>

            <!-- Feature 3 -->
            <div class="col-md-4">
                <div class="card h-100 shadow-sm border-0 rounded-3 hover-shadow">
                    <div class="card-body p-4 text-center">
                        <div class="rounded-circle bg-warning bg-opacity-10 text-warning d-inline-flex align-items-center justify-content-center mb-3" style="width: 64px; height: 64px;">
                            <i class="bi bi-shield-lock-fill fs-2"></i>
                        </div>
                        <h5 class="fw-bold mb-2">Role-Based Access Control</h5>
                        <p class="text-muted small">
                            Granular security tiers for End Users, IT Technicians, and System Admins ensuring data privacy and proper privilege separation.
                        </p>
                        <a href="Pages/Dashboard/Default.aspx" class="text-warning fw-semibold small text-decoration-none">
                            Open Dashboard <i class="bi bi-arrow-right"></i>
                        </a>
                    </div>
                </div>
            </div>
        </div>

        <!-- Call to Action Banner -->
        <div class="card bg-dark text-white rounded-4 border-0 mt-5 p-4 p-md-5 text-center shadow">
            <div class="row justify-content-center">
                <div class="col-lg-8">
                    <h3 class="fw-bold mb-3">Ready to access your workspace?</h3>
                    <p class="text-white-50 mb-4">
                        Sign in with your enterprise credentials to submit an IT ticket, check asset assignments, or manage ticket resolutions.
                    </p>
                    <div class="d-flex justify-content-center gap-3">
                        <a href="Pages/Auth/Login.aspx" class="btn btn-primary btn-lg px-4 fw-semibold">
                            <i class="bi bi-box-arrow-in-right me-1"></i> Sign In
                        </a>
                        <a href="Pages/Auth/Register.aspx" class="btn btn-outline-light btn-lg px-4">
                            <i class="bi bi-person-plus me-1"></i> Register New User
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
