using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace URLRouting
{
    public partial class UserPage : System.Web.UI.Page
    {
        string queryDBConnection = ConfigurationManager.ConnectionStrings["urlRoutingDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();

            List<string> queryParams = new List<string>();

            if (!string.IsNullOrEmpty(username))
            {
                queryParams.Add("username=" + Server.UrlEncode(username));
            }

            if (!string.IsNullOrEmpty(email))
            {
                queryParams.Add("email=" + Server.UrlEncode(email));
            }

            string targetUrl = "UserHistory.aspx";
            if (queryParams.Count > 0)
            {
                targetUrl += "?" + string.Join("&", queryParams);
            }

            txtMessage.Text = "Redirecting to User History...";
            Response.Redirect(targetUrl);
        }

        protected void btnViewAll_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserHistory.aspx");
        }
    }
}