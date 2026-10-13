<%@ Page Title="Student Management - Web Forms Demo" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Students.aspx.cs" Inherits="WebFormPracticeProject.Students" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-4">
        <!-- Header -->
        <div class="row mb-4">
            <div class="col-12">
                <h1 class="display-6 fw-bold text-primary">Student Management System</h1>
                <p class="text-muted">
                    This page demonstrates <strong>Server Controls</strong>, <strong>Form Validation</strong>, 
                    <strong>IsPostBack LifeCycle</strong>, and <strong>GridView Data Binding</strong>.
                </p>
            </div>
        </div>

        <!-- Alert Notification Message -->
        <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-dismissible fade show" role="alert">
            <asp:Label ID="lblAlertMessage" runat="server"></asp:Label>
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </asp:Panel>

        <div class="row g-4">
            <!-- Left Column: Add Student Form with Validation -->
            <div class="col-lg-5">
                <div class="card shadow-sm border-0">
                    <div class="card-header bg-primary text-white">
                        <h5 class="card-title mb-0">Add New Student</h5>
                    </div>
                    <div class="card-body">
                        <!-- Validation Summary displays all errors in one box -->
                        <asp:ValidationSummary ID="valSummary" runat="server" CssClass="alert alert-danger" HeaderText="Please fix the following errors:" />

                        <div class="mb-3">
                            <label for="txtName" class="form-label fw-semibold">Full Name <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="e.g. John Doe"></asp:TextBox>
                            <!-- RequiredFieldValidator: Ensures field is not empty -->
                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
                                ErrorMessage="Full Name is required." ForeColor="Red" Display="Dynamic" CssClass="small" />
                        </div>

                        <div class="mb-3">
                            <label for="txtEmail" class="form-label fw-semibold">Email Address <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="e.g. john@example.com"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                ErrorMessage="Email is required." ForeColor="Red" Display="Dynamic" CssClass="small" />
                            <!-- RegularExpressionValidator: Checks format -->
                            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                ValidationExpression="^[\w\.-]+@[\w\.-]+\.\w+$"
                                ErrorMessage="Invalid email address format." ForeColor="Red" Display="Dynamic" CssClass="small" />
                        </div>

                        <div class="mb-3">
                            <label for="txtAge" class="form-label fw-semibold">Age <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtAge" runat="server" CssClass="form-control" TextMode="Number" placeholder="18 - 80"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvAge" runat="server" ControlToValidate="txtAge"
                                ErrorMessage="Age is required." ForeColor="Red" Display="Dynamic" CssClass="small" />
                            <!-- RangeValidator: Ensures age is between 16 and 99 -->
                            <asp:RangeValidator ID="rvAge" runat="server" ControlToValidate="txtAge"
                                MinimumValue="16" MaximumValue="99" Type="Integer"
                                ErrorMessage="Age must be between 16 and 99." ForeColor="Red" Display="Dynamic" CssClass="small" />
                        </div>

                        <div class="mb-3">
                            <label for="ddlCourse" class="form-label fw-semibold">Course / Program</label>
                            <!-- DropDownList: Standard select element -->
                            <asp:DropDownList ID="ddlCourse" runat="server" CssClass="form-select">
                                <asp:ListItem Text="-- Select Course --" Value="" />
                                <asp:ListItem Text="Computer Science" Value="Computer Science" />
                                <asp:ListItem Text="Information Technology" Value="Information Technology" />
                                <asp:ListItem Text="Data Science" Value="Data Science" />
                                <asp:ListItem Text="Cybersecurity" Value="Cybersecurity" />
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvCourse" runat="server" ControlToValidate="ddlCourse"
                                InitialValue="" ErrorMessage="Please select a course." ForeColor="Red" Display="Dynamic" CssClass="small" />
                        </div>

                        <div class="mb-3">
                            <label class="form-label fw-semibold d-block">Gender</label>
                            <!-- RadioButtonList: Renders radio group -->
                            <asp:RadioButtonList ID="rblGender" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="d-flex gap-3">
                                <asp:ListItem Text="Male" Value="Male" Selected="True" />
                                <asp:ListItem Text="Female" Value="Female" />
                                <asp:ListItem Text="Other" Value="Other" />
                            </asp:RadioButtonList>
                        </div>

                        <div class="mb-4 form-check">
                            <!-- CheckBox Server Control -->
                            <asp:CheckBox ID="chkIsActive" runat="server" CssClass="form-check-input" Checked="true" />
                            <label class="form-check-label fw-semibold" for="ctl00_MainContent_chkIsActive">Active Enrollment Status</label>
                        </div>

                        <div class="d-flex gap-2">
                            <!-- Button with server-side OnClick event -->
                            <asp:Button ID="btnSave" runat="server" Text="Save Student" CssClass="btn btn-primary px-4" OnClick="btnSave_Click" />
                            <!-- CausesValidation='false' allows clearing without triggering error messages -->
                            <asp:Button ID="btnReset" runat="server" Text="Clear Form" CssClass="btn btn-outline-secondary" CausesValidation="false" OnClick="btnReset_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <!-- Right Column: Student List (GridView) & Filtering -->
            <div class="col-lg-7">
                <div class="card shadow-sm border-0">
                    <div class="card-header bg-dark text-white d-flex justify-content-between align-items-center">
                        <h5 class="card-title mb-0">Enrolled Students</h5>
                        <asp:Label ID="lblTotalCount" runat="server" CssClass="badge bg-info text-dark">0 Students</asp:Label>
                    </div>
                    <div class="card-body">
                        <!-- Search and Filter Bar -->
                        <div class="row g-2 mb-3">
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search by name, email, or course..."></asp:TextBox>
                            </div>
                            <div class="col-sm-4 d-flex gap-2">
                                <asp:Button ID="btnSearch" runat="server" Text="Filter" CssClass="btn btn-outline-primary w-100" CausesValidation="false" OnClick="btnSearch_Click" />
                                <asp:Button ID="btnShowAll" runat="server" Text="Reset" CssClass="btn btn-outline-secondary" CausesValidation="false" OnClick="btnShowAll_Click" />
                            </div>
                        </div>

                        <!-- GridView: Renders an HTML table bound to C# objects -->
                        <div class="table-responsive">
                            <asp:GridView ID="gvStudents" runat="server" 
                                AutoGenerateColumns="False" 
                                DataKeyNames="Id" 
                                CssClass="table table-hover table-striped align-middle"
                                EmptyDataText="No students found matching your criteria."
                                EmptyDataRowStyle-CssClass="text-center text-muted py-4 fst-italic"
                                OnRowDeleting="gvStudents_RowDeleting">
                                
                                <Columns>
                                    <asp:BoundField DataField="Id" HeaderText="#" ItemStyle-Font-Bold="true" />
                                    
                                    <asp:TemplateField HeaderText="Student Info">
                                        <ItemTemplate>
                                            <div class="fw-bold"><%# Eval("Name") %></div>
                                            <small class="text-muted"><%# Eval("Email") %></small>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="Course" HeaderText="Course" />

                                    <asp:TemplateField HeaderText="Gender / Age">
                                        <ItemTemplate>
                                            <%# Eval("Gender") %>, <%# Eval("Age") %> yrs
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <span class='badge <%# (bool)Eval("IsActive") ? "bg-success" : "bg-secondary" %>'>
                                                <%# (bool)Eval("IsActive") ? "Active" : "Inactive" %>
                                            </span>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="EnrollmentDate" HeaderText="Enrolled" DataFormatString="{0:MMM dd, yyyy}" />

                                    <asp:TemplateField HeaderText="Action" ItemStyle-Width="90px">
                                        <ItemTemplate>
                                            <!-- Delete Command in GridView with client confirmation -->
                                            <asp:LinkButton ID="btnDelete" runat="server" 
                                                CommandName="Delete" 
                                                CssClass="btn btn-sm btn-outline-danger"
                                                CausesValidation="false"
                                                OnClientClick="return confirm('Are you sure you want to remove this student?');">
                                                Delete
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>

                <!-- Web Forms Architecture Guide Note -->
                <div class="card mt-3 border-info">
                    <div class="card-body small text-secondary">
                        <h6 class="text-info fw-bold"><i class="bi bi-info-circle"></i> What's happening behind the scenes here?</h6>
                        <ul class="mb-0 ps-3">
                            <li><strong>PostBack:</strong> When you click "Save Student", the browser submits the entire form back to <code>Students.aspx</code>.</li>
                            <li><strong>Control State & ViewState:</strong> Form controls retain their submitted values across the request.</li>
                            <li><strong>Validation:</strong> ASP.NET runs client-side JavaScript before submitting; if disabled, server validation still executes.</li>
                            <li><strong>GridView:</strong> In code-behind, we assign a <code>List&lt;Student&gt;</code> to <code>gvStudents.DataSource</code> and call <code>gvStudents.DataBind()</code>.</li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
