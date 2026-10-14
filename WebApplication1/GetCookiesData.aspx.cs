using System;
using System.Web;

namespace WebApplication1
{
    public partial class SetCookies : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HttpCookie userCookie = Request.Cookies["UserInfo"];

                if (userCookie != null)
                {
                    lblUsername.Text = userCookie["Username"];
                    lblEmail.Text = userCookie["Email"];
                }
                else
                {
                    Response.Redirect("CookiePractice.aspx");
                }
            }
        }
    }
}