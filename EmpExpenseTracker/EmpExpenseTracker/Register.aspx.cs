using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace EmpExpenseTracker
{
    public partial class Register : System.Web.UI.Page
    {
        private string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ExpenseDbConn"]?.ConnectionString
                    ?? "Data Source=ANUBHAW;Initial Catalog=Practice;Integrated Security=True;";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // If already logged in, redirect to Expenses
            if (Session["UserId"] != null)
            {
                Response.Redirect("Expenses.aspx");
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
           
            if (!Page.IsValid) return;

            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim().ToLower(); 
            string password = txtPassword.Text.Trim();
            string role = ddlRole.SelectedValue;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    
                    string checkQuery = "SELECT COUNT(1) FROM Users WHERE LOWER(Email) = @Email";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@Email", email);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            ShowAlert("An account with this email address already exists. Please <a href='Login.aspx' class='alert-link'>log in</a>.", "alert-warning");
                            return;
                        }
                    }

                    
                    string insertQuery = @"INSERT INTO Users (FullName, Email, Password, Role) 
                                           VALUES (@FullName, @Email, @Password, @Role)";

                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@FullName", fullName);
                        insertCmd.Parameters.AddWithValue("@Email", email);
                        insertCmd.Parameters.AddWithValue("@Password", password);
                        insertCmd.Parameters.AddWithValue("@Role", role);

                        insertCmd.ExecuteNonQuery();
                    }
                }

                Response.Redirect("Login.aspx?registered=true");
            }
            catch (Exception ex)
            {
                ShowAlert("Registration failed due to a database error: " + ex.Message, "alert-danger");
            }
        }

        private void ShowAlert(string message, string alertClass)
        {
            pnlAlert.CssClass = $"alert {alertClass} alert-dismissible fade show";
            lblAlertMessage.Text = message;
            pnlAlert.Visible = true;
        }
    }
}