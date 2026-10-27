using System;
using System.Web.UI;

namespace DevHelper.Labs
{
    public partial class AuthLab : Page
    {
        private const string SimUserKey = "Lab_AuthUser";
        private const string SimRoleKey = "Lab_AuthRole";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RenderAuthState();
            }
        }

        private void RenderAuthState()
        {
            if (Session[SimUserKey] != null)
            {
                pnlLoginBox.Visible = false;
                pnlProfileBox.Visible = true;
                lblProfileName.Text = Server.HtmlEncode(Session[SimUserKey].ToString());
                lblProfileRole.Text = Server.HtmlEncode(Session[SimRoleKey]?.ToString() ?? "Student");
                lblLoginStatus.Text = "Authenticated (Session Active)";
                lblLoginStatus.CssClass = "badge bg-success";
            }
            else
            {
                pnlLoginBox.Visible = true;
                pnlProfileBox.Visible = false;
                lblLoginStatus.Text = "Guest (Not Logged In)";
                lblLoginStatus.CssClass = "badge bg-secondary";
            }
        }

        protected void btnLoginSim_Click(object sender, EventArgs e)
        {
            string username = txtSimUser.Text.Trim();
            if (string.IsNullOrEmpty(username)) username = "DemoUser";

            Session[SimUserKey] = username;
            Session[SimRoleKey] = ddlSimRole.SelectedValue;

            RenderAuthState();
        }

        protected void btnLogoutSim_Click(object sender, EventArgs e)
        {
            Session.Remove(SimUserKey);
            Session.Remove(SimRoleKey);

            RenderAuthState();
        }
    }
}
