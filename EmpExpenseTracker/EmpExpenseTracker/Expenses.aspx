<%@ Page Title="Employee Expense Tracker" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Expenses.aspx.cs" Inherits="EmpExpenseTracker.Expenses" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-4">
        <!-- Header -->
        <div class="row mb-3">
            <div class="col-12">
                <h1 class="display-6 fw-bold text-primary">Employee Expense Tracker</h1>
                <p class="text-muted">
                    Submit new expense claims and view existing records below.
                </p>
            </div>
        </div>

        <!-- Notification Message Panel -->
        <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-dismissible fade show" role="alert">
            <asp:Label ID="lblAlertMessage" runat="server"></asp:Label>
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </asp:Panel>

        <!-- Summary Dashboard Cards -->
        <div class="row g-3 mb-4">
            <div class="col-md-4">
                <div class="card border-0 shadow-sm bg-primary text-white">
                    <div class="card-body">
                        <h6 class="text-uppercase fw-semibold mb-1 opacity-75">Total Claims Amount</h6>
                        <h3 class="fw-bold mb-0">
                            <asp:Label ID="lblTotalAmount" runat="server">$0.00</asp:Label>
                        </h3>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="card border-0 shadow-sm bg-success text-white">
                    <div class="card-body">
                        <h6 class="text-uppercase fw-semibold mb-1 opacity-75">Total Claims Count</h6>
                        <h3 class="fw-bold mb-0">
                            <asp:Label ID="lblTotalClaims" runat="server">0</asp:Label>
                        </h3>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="card border-0 shadow-sm bg-dark text-white">
                    <div class="card-body">
                        <h6 class="text-uppercase fw-semibold mb-1 opacity-75">Highest Single Claim</h6>
                        <h3 class="fw-bold mb-0">
                            <asp:Label ID="lblMaxExpense" runat="server">$0.00</asp:Label>
                        </h3>
                    </div>
                </div>
            </div>
        </div>

        <div class="row g-4">
            <!-- Left Column: Add Expense Form -->
            <div class="col-lg-5">
                <div class="card shadow-sm border-0">
                    <div class="card-header bg-primary text-white">
                        <h5 class="card-title mb-0">Add Expense Claim</h5>
                    </div>
                    <div class="card-body">
                        <!-- Validation Summary -->
                        <asp:ValidationSummary ID="valSummary" runat="server" CssClass="alert alert-danger" HeaderText="Please fix the following:" />

                        <!-- Employee Name -->
                        <div class="mb-3">
                            <label for="txtEmployeeName" class="form-label fw-semibold">Employee Name <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtEmployeeName" runat="server" CssClass="form-control" placeholder="e.g. Alex Rivera"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEmployeeName" runat="server" ControlToValidate="txtEmployeeName"
                                ErrorMessage="Employee Name is required." ForeColor="Red" Display="Dynamic" CssClass="small" />
                        </div>

                        <!-- Category DropDown -->
                        <div class="mb-3">
                            <label for="ddlCategory" class="form-label fw-semibold">Category <span class="text-danger">*</span></label>
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select">
                                <asp:ListItem Text="-- Select Category --" Value="" />
                                <asp:ListItem Text="Travel & Commute" Value="Travel" />
                                <asp:ListItem Text="Meals & Entertainment" Value="Meals" />
                                <asp:ListItem Text="Software & Tools" Value="Software" />
                                <asp:ListItem Text="Office Supplies" Value="Office Supplies" />
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvCategory" runat="server" ControlToValidate="ddlCategory"
                                InitialValue="" ErrorMessage="Please select a category." ForeColor="Red" Display="Dynamic" CssClass="small" />
                        </div>

                        <!-- Amount -->
                        <div class="mb-3">
                            <label for="txtAmount" class="form-label fw-semibold">Amount ($) <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" placeholder="0.00"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvAmount" runat="server" ControlToValidate="txtAmount"
                                ErrorMessage="Amount is required." ForeColor="Red" Display="Dynamic" CssClass="small" />
                            <asp:RangeValidator ID="rvAmount" runat="server" ControlToValidate="txtAmount"
                                MinimumValue="1" MaximumValue="50000" Type="Double"
                                ErrorMessage="Amount must be between $1 and $50,000." ForeColor="Red" Display="Dynamic" CssClass="small" />
                        </div>

                        <!-- Expense Date -->
                        <div class="mb-3">
                            <label for="txtExpenseDate" class="form-label fw-semibold">Expense Date <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtExpenseDate" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvExpenseDate" runat="server" ControlToValidate="txtExpenseDate"
                                ErrorMessage="Expense Date is required." ForeColor="Red" Display="Dynamic" CssClass="small" />
                        </div>

                        <!-- Payment Method Radio List -->
                        <div class="mb-3">
                            <label class="form-label fw-semibold d-block">Payment Method</label>
                            <asp:RadioButtonList ID="rblPaymentMethod" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="d-flex gap-3">
                                <asp:ListItem Text="Corporate Card" Value="Corporate Card" Selected="True" />
                                <asp:ListItem Text="Personal Card" Value="Personal Card" />
                                <asp:ListItem Text="Cash" Value="Cash" />
                            </asp:RadioButtonList>
                        </div>

                        <!-- Has Receipt CheckBox -->
                        <div class="mb-4 form-check">
                            <asp:CheckBox ID="chkHasReceipt" runat="server" CssClass="form-check-input" Checked="true" />
                            <label class="form-check-label fw-semibold" for="ctl00_MainContent_chkHasReceipt">Receipt / Bill Attached</label>
                        </div>

                        <!-- Submit & Reset Buttons -->
                        <div class="d-flex gap-2">
                            <asp:Button ID="btnSubmit" runat="server" Text="Save Expense" CssClass="btn btn-primary px-4" OnClick="btnSubmit_Click" />
                            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-outline-secondary" CausesValidation="false" OnClick="btnClear_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <!-- Right Column: GridView Table & Filter -->
            <div class="col-lg-7">
                <div class="card shadow-sm border-0">
                    <div class="card-header bg-dark text-white d-flex justify-content-between align-items-center">
                        <h5 class="card-title mb-0">Expense Records</h5>
                    </div>
                    <div class="card-body">
                        <!-- Search and Filter Bar -->
                        <div class="row g-2 mb-3">
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search by employee name or category..."></asp:TextBox>
                            </div>
                            <div class="col-sm-4 d-flex gap-2">
                                <asp:Button ID="btnSearch" runat="server" Text="Filter" CssClass="btn btn-outline-primary w-100" CausesValidation="false" OnClick="btnSearch_Click" />
                                <asp:Button ID="btnShowAll" runat="server" Text="Reset" CssClass="btn btn-outline-secondary" CausesValidation="false" OnClick="btnShowAll_Click" />
                            </div>
                        </div>

                        <!-- GridView: Connects directly to DataTable or List in your C# code -->
                        <div class="table-responsive">
                            <asp:GridView ID="gvExpenses" runat="server" 
                                AutoGenerateColumns="False" 
                                DataKeyNames="Id" 
                                CssClass="table table-hover table-striped align-middle"
                                EmptyDataText="No expense records found."
                                EmptyDataRowStyle-CssClass="text-center text-muted py-4 fst-italic"
                                OnRowDeleting="gvExpenses_RowDeleting">
                                
                                <Columns>
                                    <asp:BoundField DataField="Id" HeaderText="#" ItemStyle-Font-Bold="true" />
                                    
                                    <asp:BoundField DataField="EmployeeName" HeaderText="Employee" />

                                    <asp:BoundField DataField="Category" HeaderText="Category" />

                                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:C}" />

                                    <asp:BoundField DataField="ExpenseDate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" />

                                    <asp:BoundField DataField="PaymentMethod" HeaderText="Payment" />

                                    <asp:TemplateField HeaderText="Receipt">
                                        <ItemTemplate>
                                            <span class='badge <%# (bool)Eval("HasReceipt") ? "bg-success" : "bg-warning text-dark" %>'>
                                                <%# (bool)Eval("HasReceipt") ? "Attached" : "Missing" %>
                                            </span>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Action" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnDelete" runat="server" 
                                                CommandName="Delete" 
                                                CssClass="btn btn-sm btn-outline-danger"
                                                CausesValidation="false"
                                                OnClientClick="return confirm('Are you sure you want to delete this expense?');">
                                                Delete
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
