using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PracticeSet2
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DisplayActiveUsers();
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            DisplayActiveUsers();
        }

        protected void btnEndSession_Click(object sender, EventArgs e)
        {
            // Abandons the current session which triggers Session_End in Global.asax
            Session.Abandon();
            // Redirect to refresh and start/clear the request
            Response.Redirect(Request.RawUrl);
        }

        private void DisplayActiveUsers()
        {
            if (Application["user"] != null)
            {
                lblUserCount.Text = Application["user"].ToString();
            }
            else
            {
                lblUserCount.Text = "0";
            }
        }
    }
}