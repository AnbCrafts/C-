using System;
using System.Web.UI;

namespace DevHelper.Labs
{
    public partial class ValidationLab : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlFeedback.Visible = false;
            }
        }

        protected void btnValidate_Click(object sender, EventArgs e)
        {
            // Layer 2: Server-side validation check
            if (!Page.IsValid)
            {
                lblValidationResult.Text = "Validation FAILED (Server Rejected)";
                lblValidationResult.CssClass = "badge bg-danger";
                pnlFeedback.Visible = false;
                return;
            }

            lblValidationResult.Text = "Validation PASSED (Page.IsValid = true)";
            lblValidationResult.CssClass = "badge bg-success";

            pnlFeedback.Visible = true;
            pnlFeedback.CssClass = "alert alert-success";
            lblFeedback.Text = $"<i class='bi bi-check-circle-fill me-1'></i> Success! Validated user: <strong>{Server.HtmlEncode(txtName.Text)}</strong>, Email: <strong>{Server.HtmlEncode(txtEmail.Text)}</strong>, Age: <strong>{Server.HtmlEncode(txtAge.Text)}</strong>.";
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtAge.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtConfirmPassword.Text = string.Empty;

            lblValidationResult.Text = "Cleared (No Validation Triggered)";
            lblValidationResult.CssClass = "badge bg-secondary";
            pnlFeedback.Visible = false;
        }
    }
}
