using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EmpExpenseTracker
{
    public partial class Expenses : System.Web.UI.Page
    {
        // Helper property to read Connection String from Web.config
        private string ConnectionString
        {
            get
            {
                var connSetting = ConfigurationManager.ConnectionStrings["ExpenseDbConn"];
                if (connSetting != null && !string.IsNullOrEmpty(connSetting.ConnectionString))
                {
                    return connSetting.ConnectionString;
                }
                // Fallback default
                return "Data Source=ANUBHAW;Initial Catalog=Practice;Integrated Security=True;";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // CRITICAL: Load data only on initial GET request (not on button postbacks)
            if (!IsPostBack)
            {
                // Set default date to today in the date picker
                txtExpenseDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                LoadExpenses();
            }
        }

        /// <summary>
        /// Fetches expenses from SQL Server into a DataTable and binds to gvExpenses GridView.
        /// Also updates the Summary Dashboard cards (Total Sum, Count, Highest).
        /// </summary>
        private void LoadExpenses(string searchQuery = "")
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    string query = "SELECT Id, EmployeeName, Category, Amount, ExpenseDate, PaymentMethod, HasReceipt FROM Expenses";

                    // If user is searching, append a WHERE clause with parameters (prevents SQL Injection)
                    if (!string.IsNullOrEmpty(searchQuery))
                    {
                        query += " WHERE EmployeeName LIKE @search OR Category LIKE @search";
                    }

                    query += " ORDER BY ExpenseDate DESC, Id DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (!string.IsNullOrEmpty(searchQuery))
                        {
                            cmd.Parameters.AddWithValue("@search", "%" + searchQuery + "%");
                        }

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            // 1. Bind to GridView
                            gvExpenses.DataSource = dt;
                            gvExpenses.DataBind();

                            // 2. Update Dashboard Summary Cards
                            UpdateDashboard(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowAlert("Database error while loading records: " + ex.Message, "alert-danger");
            }
        }

        /// <summary>
        /// Calculates total amount, claim count, and highest claim from the loaded DataTable.
        /// </summary>
        private void UpdateDashboard(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                // Total Count
                lblTotalClaims.Text = dt.Rows.Count.ToString();

                // Compute SUM using DataTable.Compute
                object sumObj = dt.Compute("SUM(Amount)", "");
                decimal totalSum = (sumObj != DBNull.Value) ? Convert.ToDecimal(sumObj) : 0m;
                lblTotalAmount.Text = totalSum.ToString("C");

                // Compute MAX using DataTable.Compute
                object maxObj = dt.Compute("MAX(Amount)", "");
                decimal maxAmount = (maxObj != DBNull.Value) ? Convert.ToDecimal(maxObj) : 0m;
                lblMaxExpense.Text = maxAmount.ToString("C");
            }
            else
            {
                lblTotalClaims.Text = "0";
                lblTotalAmount.Text = "$0.00";
                lblMaxExpense.Text = "$0.00";
            }
        }

        /// <summary>
        /// Handles the "Save Expense" button click (INSERT into DB).
        /// </summary>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // Always verify server validation passed
            if (!Page.IsValid) return;

            try
            {
                // Read input values directly from controls
                string employeeName = txtEmployeeName.Text.Trim();
                string category = ddlCategory.SelectedValue;
                decimal amount = decimal.Parse(txtAmount.Text.Trim());
                DateTime expenseDate = DateTime.Parse(txtExpenseDate.Text.Trim());
                string paymentMethod = rblPaymentMethod.SelectedValue;
                bool hasReceipt = chkHasReceipt.Checked;

                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    string insertQuery = @"INSERT INTO Expenses 
                                           (EmployeeName, Category, Amount, ExpenseDate, PaymentMethod, HasReceipt) 
                                           VALUES 
                                           (@EmployeeName, @Category, @Amount, @ExpenseDate, @PaymentMethod, @HasReceipt)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        // Use SQL Parameters to prevent SQL Injection
                        cmd.Parameters.AddWithValue("@EmployeeName", employeeName);
                        cmd.Parameters.AddWithValue("@Category", category);
                        cmd.Parameters.AddWithValue("@Amount", amount);
                        cmd.Parameters.AddWithValue("@ExpenseDate", expenseDate);
                        cmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                        cmd.Parameters.AddWithValue("@HasReceipt", hasReceipt);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                ShowAlert($"Expense of <strong>{amount:C}</strong> for <strong>{Server.HtmlEncode(employeeName)}</strong> saved successfully!", "alert-success");

                // Reset inputs and refresh table
                ClearFormInputs();
                LoadExpenses();
            }
            catch (Exception ex)
            {
                ShowAlert("Failed to save expense: " + ex.Message, "alert-danger");
            }
        }

        /// <summary>
        /// Handles row deletion in the GridView (DELETE FROM DB).
        /// </summary>
        protected void gvExpenses_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                // Retrieve the primary key (Id) using DataKeys collection
                int expenseId = Convert.ToInt32(gvExpenses.DataKeys[e.RowIndex].Value);

                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    string deleteQuery = "DELETE FROM Expenses WHERE Id = @Id";

                    using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", expenseId);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                ShowAlert($"Expense record #{expenseId} was deleted successfully.", "alert-warning");
                LoadExpenses();
            }
            catch (Exception ex)
            {
                ShowAlert("Failed to delete record: " + ex.Message, "alert-danger");
            }
        }

        /// <summary>
        /// Handles search filtering.
        /// </summary>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string search = txtSearch.Text.Trim();
            LoadExpenses(search);
        }

        /// <summary>
        /// Clears search filter and reloads all records.
        /// </summary>
        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            LoadExpenses();
        }

        /// <summary>
        /// Clears all input controls.
        /// </summary>
        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearFormInputs();
            pnlAlert.Visible = false;
        }

        private void ClearFormInputs()
        {
            txtEmployeeName.Text = string.Empty;
            ddlCategory.SelectedIndex = 0;
            txtAmount.Text = string.Empty;
            txtExpenseDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            rblPaymentMethod.SelectedIndex = 0;
            chkHasReceipt.Checked = true;
        }

        private void ShowAlert(string message, string alertClass)
        {
            pnlAlert.CssClass = $"alert {alertClass} alert-dismissible fade show";
            lblAlertMessage.Text = message;
            pnlAlert.Visible = true;
        }
    }
}