using System;
using System.Web.UI;

namespace DevDesk.Pages.Auth
{
    public partial class Register : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // If user is already logged in, redirect to dashboard
                if (Session["UserId"] != null)
                {
                    Response.Redirect("~/Pages/Dashboard/Default.aspx");
                    return;
                }
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            // TODO: User will implement the ADO.NET query here:
            // string fullName = txtFullName.Text.Trim();
            // string email = txtEmail.Text.Trim();
            // string department = ddlDepartment.SelectedValue;
            // string phone = txtPhone.Text.Trim();
            // string password = txtPassword.Text.Trim();
            //
            // Call sp_RegisterUser in DevDesk_DB
        }

        private void ShowAlert(string message, string alertClass)
        {
            pnlAlert.CssClass = $"alert {alertClass} alert-dismissible fade show";
            lblAlertMessage.Text = message;
            pnlAlert.Visible = true;
        }
    }
}
