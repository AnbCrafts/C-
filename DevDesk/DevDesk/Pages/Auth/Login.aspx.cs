using System;
using System.Web.UI;

namespace DevDesk.Pages.Auth
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if user is already logged in
                if (Session["UserId"] != null)
                {
                    Response.Redirect("~/Pages/Dashboard/Default.aspx");
                    return;
                }

                // Check for registration success message
                if (Request.QueryString["registered"] == "true")
                {
                    ShowAlert("Account created successfully! Please sign in with your credentials.", "alert-success");
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            // TODO: Connect to DB and execute sp_AuthenticateUser
            // string email = txtEmail.Text.Trim();
            // string password = txtPassword.Text.Trim();
            // bool remember = chkRememberMe.Checked;
        }

        private void ShowAlert(string message, string alertClass)
        {
            pnlAlert.CssClass = $"alert {alertClass} alert-dismissible fade show";
            lblAlertMessage.Text = message;
            pnlAlert.Visible = true;
        }
    }
}
