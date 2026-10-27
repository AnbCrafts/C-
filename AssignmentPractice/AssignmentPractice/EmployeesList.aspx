<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployeesList.aspx.cs" Inherits="EmployeeManagement.EmployeesList" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Employee Management System</title>
    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin="anonymous" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet" />
    
    <style>
        :root {
            --primary: #2563eb;
            --primary-hover: #1d4ed8;
            --success: #16a34a;
            --success-hover: #15803d;
            --danger: #dc2626;
            --gray-50: #f8fafc;
            --gray-100: #f1f5f9;
            --gray-200: #e2e8f0;
            --gray-300: #cbd5e1;
            --gray-600: #475569;
            --gray-700: #334155;
            --gray-800: #1e293b;
            --gray-900: #0f172a;
            --shadow-sm: 0 1px 2px 0 rgba(0, 0, 0, 0.05);
            --shadow-md: 0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -2px rgba(0, 0, 0, 0.1);
            --shadow-lg: 0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -4px rgba(0, 0, 0, 0.1);
        }

        * { box-sizing: border-box; font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif; }
        body { background-color: #f1f5f9; margin: 0; padding: 28px 16px; color: var(--gray-800); }
        .container { max-width: 1180px; margin: 0 auto; }

        .app-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 24px;
        }
        .app-header h1 {
            font-size: 24px;
            font-weight: 700;
            color: var(--gray-900);
            margin: 0 0 4px 0;
            letter-spacing: -0.02em;
        }
        .app-header p {
            font-size: 13px;
            color: var(--gray-600);
            margin: 0;
        }

        .filter-card {
            background: #ffffff;
            border-radius: 12px;
            padding: 22px 26px;
            box-shadow: var(--shadow-sm);
            border: 1px solid var(--gray-200);
            margin-bottom: 24px;
            display: flex;
            justify-content: space-between;
            align-items: flex-end;
            gap: 20px;
            flex-wrap: wrap;
        }
        .filter-controls {
            display: flex;
            gap: 16px;
            align-items: flex-end;
            flex-wrap: wrap;
            flex: 1;
        }
        .form-group {
            display: flex;
            flex-direction: column;
            gap: 6px;
        }
        .form-group label {
            font-size: 12px;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 0.04em;
            color: var(--gray-600);
        }
        .form-control {
            height: 38px;
            padding: 0 12px;
            border: 1px solid var(--gray-300);
            border-radius: 6px;
            font-size: 14px;
            color: var(--gray-800);
            background-color: #ffffff;
            transition: all 0.15s ease-in-out;
            min-width: 200px;
        }
        .form-control:focus {
            outline: none;
            border-color: var(--primary);
            box-shadow: 0 0 0 3px rgba(37, 99, 235, 0.15);
        }
        .form-control:read-only {
            background-color: var(--gray-100);
            cursor: not-allowed;
        }

        .btn {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            height: 38px;
            padding: 0 18px;
            border-radius: 6px;
            border: none;
            font-size: 13px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.15s ease-in-out;
            text-decoration: none;
        }
        .btn-primary { background-color: var(--primary); color: #fff; }
        .btn-primary:hover { background-color: var(--primary-hover); }
        .btn-success { background-color: var(--success); color: #fff; }
        .btn-success:hover { background-color: var(--success-hover); }
        .btn-danger { background-color: #fee2e2; color: var(--danger); border: 1px solid #fecaca; }
        .btn-danger:hover { background-color: var(--danger); color: #fff; }
        .btn-secondary { background-color: var(--gray-200); color: var(--gray-700); }
        .btn-secondary:hover { background-color: var(--gray-300); }
        .btn-edit { background-color: #eff6ff; color: var(--primary); border: 1px solid #bfdbfe; }
        .btn-edit:hover { background-color: var(--primary); color: #fff; }
        .btn-sm { height: 28px; padding: 0 10px; font-size: 12px; border-radius: 4px; }

        .grid-wrapper {
            background: #ffffff;
            border-radius: 12px;
            border: 1px solid var(--gray-200);
            box-shadow: var(--shadow-sm);
            overflow: hidden;
        }
        .modern-grid {
            width: 100%;
            border-collapse: collapse;
            font-size: 13px;
            text-align: left;
        }
        .modern-grid th {
            background-color: var(--gray-900);
            color: #ffffff;
            font-weight: 600;
            padding: 14px 16px;
            letter-spacing: 0.02em;
            border: none;
        }
        .modern-grid td {
            padding: 13px 16px;
            border-bottom: 1px solid var(--gray-200);
            color: var(--gray-700);
            vertical-align: middle;
        }
        .modern-grid tr:last-child td { border-bottom: none; }
        .modern-grid tr:hover td { background-color: #f8fafc; }

        .badge {
            display: inline-block;
            padding: 3px 8px;
            font-size: 11px;
            font-weight: 600;
            border-radius: 9999px;
            background-color: #e0f2fe;
            color: #0369a1;
        }

        .modal-overlay {
            position: fixed;
            top: 0; left: 0; width: 100%; height: 100%;
            background: rgba(15, 23, 42, 0.55);
            backdrop-filter: blur(3px);
            display: none;
            justify-content: center;
            align-items: center;
            z-index: 9999;
        }
        .modal-box {
            background: #ffffff;
            width: 100%;
            max-width: 480px;
            border-radius: 12px;
            box-shadow: var(--shadow-lg);
            overflow: hidden;
            animation: modalFadeIn 0.2s ease-out;
        }
        @keyframes modalFadeIn {
            from { opacity: 0; transform: translateY(-12px) scale(0.98); }
            to { opacity: 1; transform: translateY(0) scale(1); }
        }
        .modal-header {
            padding: 18px 24px;
            background: var(--gray-50);
            border-bottom: 1px solid var(--gray-200);
            display: flex;
            justify-content: space-between;
            align-items: center;
        }
        .modal-header h3 { margin: 0; font-size: 16px; font-weight: 700; color: var(--gray-900); }
        .modal-close {
            background: none; border: none; font-size: 20px; color: var(--gray-600); cursor: pointer;
        }
        .modal-body { padding: 22px 24px; display: flex; flex-direction: column; gap: 14px; }
        .modal-row-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 12px; }
        .modal-footer {
            padding: 16px 24px;
            background: var(--gray-50);
            border-top: 1px solid var(--gray-200);
            display: flex;
            justify-content: flex-end;
            gap: 10px;
        }

        .action-btns {
            display: flex;
            gap: 8px;
            align-items: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />

        <div class="container">
            <div class="app-header">
                <div>
                    <h1>Employee Management System</h1>
                    <p>Search, filter, and manage employee records with instant data synchronization</p>
                </div>
            </div>

            <asp:UpdatePanel ID="upEmployeeGrid" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="filter-card">
                        <div class="filter-controls">
                            <div class="form-group">
                                <label>Search</label>
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="(EmployeeID, Name)"></asp:TextBox>
                            </div>

                            <div class="form-group">
                                <label>Designation</label>
                                <asp:DropDownList ID="ddlDesignation" runat="server" CssClass="form-control" 
                                    AutoPostBack="true" OnSelectedIndexChanged="FilterChanged">
                                </asp:DropDownList>
                            </div>

                            <div class="form-group">
                                <label>Sorted By</label>
                                <asp:DropDownList ID="ddlSortedBy" runat="server" CssClass="form-control" 
                                    AutoPostBack="true" OnSelectedIndexChanged="FilterChanged">
                                    <asp:ListItem Text="Name" Value="Name" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Emp. ID" Value="EmpId"></asp:ListItem>
                                    <asp:ListItem Text="Salary" Value="Salary"></asp:ListItem>
                                    <asp:ListItem Text="DOB" Value="DOB"></asp:ListItem>
                                    <asp:ListItem Text="DOJ" Value="DOJ"></asp:ListItem>
                                </asp:DropDownList>
                            </div>

                            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                        </div>

                        <div>
                            <button type="button" class="btn btn-success" onclick="openAddModal()">+ Add Employee</button>
                        </div>
                    </div>

                    <div class="grid-wrapper">
                        <asp:GridView ID="gvEmployees" runat="server" AutoGenerateColumns="False" 
                            CssClass="modern-grid" GridLines="None" 
                            EmptyDataText="No employees found matching the specified criteria."
                            OnRowCommand="gvEmployees_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="#" HeaderText="#" HeaderStyle-Width="40px" />
                                <asp:BoundField DataField="EmpId" HeaderText="Emp. ID" HeaderStyle-Width="90px" />
                                <asp:BoundField DataField="Name" HeaderText="Name" />
                                <asp:TemplateField HeaderText="Designation">
                                    <ItemTemplate>
                                        <span class="badge"><%# Eval("Designation") %></span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="DOB" HeaderText="DOB" />
                                <asp:BoundField DataField="DOJ" HeaderText="DOJ" />
                                <asp:BoundField DataField="Salary" HeaderText="Salary" DataFormatString="{0:N2}" />
                                <asp:BoundField DataField="Address" HeaderText="Address" />
                                
                                <asp:TemplateField HeaderText="Actions" HeaderStyle-Width="130px">
                                    <ItemTemplate>
                                        <div class="action-btns">
                                            <asp:LinkButton ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-edit btn-sm"
                                                CommandName="EditEmp" CommandArgument='<%# Eval("EmpId") %>'></asp:LinkButton>
                                            
                                            <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger btn-sm"
                                                CommandName="DeleteEmp" CommandArgument='<%# Eval("EmpId") %>'
                                                OnClientClick="return confirm('Are you sure you want to delete this employee?');"></asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>

                    <div id="employeeModal" class="modal-overlay">
                        <div class="modal-box">
                            <div class="modal-header">
                                <h3 id="modalTitle">Add New Employee</h3>
                                <button type="button" class="modal-close" onclick="closeModal()">&times;</button>
                            </div>

                            <div class="modal-body">
                                <div class="form-group">
                                    <label>Emp ID *</label>
                                    <asp:TextBox ID="txtAddEmpId" runat="server" CssClass="form-control" placeholder="e.g. E007"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Full Name *</label>
                                    <asp:TextBox ID="txtAddName" runat="server" CssClass="form-control" placeholder="e.g. Anubhow Gupta"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Designation *</label>
                                    <asp:TextBox ID="txtAddDesignation" runat="server" CssClass="form-control" placeholder="e.g. Software Engineer"></asp:TextBox>
                                </div>
                                <div class="modal-row-grid">
                                    <div class="form-group">
                                        <label>DOB *</label>
                                        <asp:TextBox ID="txtAddDOB" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label>DOJ *</label>
                                        <asp:TextBox ID="txtAddDOJ" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label>Salary *</label>
                                    <asp:TextBox ID="txtAddSalary" runat="server" CssClass="form-control" placeholder="e.g. 35000.00"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Address</label>
                                    <asp:TextBox ID="txtAddAddress" runat="server" CssClass="form-control" placeholder="e.g. Salt Lake, Kolkata"></asp:TextBox>
                                </div>
                            </div>

                            <div class="modal-footer">
                                <button type="button" class="btn btn-secondary" onclick="closeModal()">Cancel</button>
                                <asp:Button ID="btnSaveEmployee" runat="server" Text="Save Employee" CssClass="btn btn-success" OnClick="btnSaveEmployee_Click" />
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <script type="text/javascript">
            function openAddModal() {
                var title = document.getElementById('modalTitle');
                if (title) title.innerText = 'Add New Employee';
                var empId = document.getElementById('<%= txtAddEmpId.ClientID %>');
                if (empId) {
                    empId.readOnly = false;
                    empId.value = '';
                }
                var name = document.getElementById('<%= txtAddName.ClientID %>');
                if (name) name.value = '';
                var desig = document.getElementById('<%= txtAddDesignation.ClientID %>');
                if (desig) desig.value = '';
                var dob = document.getElementById('<%= txtAddDOB.ClientID %>');
                if (dob) dob.value = '';
                var doj = document.getElementById('<%= txtAddDOJ.ClientID %>');
                if (doj) doj.value = '';
                var salary = document.getElementById('<%= txtAddSalary.ClientID %>');
                if (salary) salary.value = '';
                var addr = document.getElementById('<%= txtAddAddress.ClientID %>');
                if (addr) addr.value = '';

                var modal = document.getElementById('employeeModal');
                if (modal) modal.style.display = 'flex';
            }

            function openEditModal() {
                var title = document.getElementById('modalTitle');
                if (title) title.innerText = 'Edit Employee';
                var modal = document.getElementById('employeeModal');
                if (modal) modal.style.display = 'flex';
            }

            function closeModal() {
                var modal = document.getElementById('employeeModal');
                if (modal) modal.style.display = 'none';
            }
        </script>
    </form>
</body>
</html>