using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class CookiePractice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();

            HttpCookie userCookie = new HttpCookie("UserInfo");
            userCookie["Username"] = username;
            userCookie["Email"] = email;
            userCookie.Expires = DateTime.Now.AddMinutes(1);

            Response.Cookies.Add(userCookie);
            Response.Redirect("GetCookiesData.aspx");

            lblMessage.Text = "Details saved successfully!";
        }
    }
}