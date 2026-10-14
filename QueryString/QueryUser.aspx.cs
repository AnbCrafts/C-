using System;

namespace QueryString
{
    public partial class QueryUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void SearchUser_btn(object sender, EventArgs e)
        {
            string username = usernameTxt.Text.Trim();
            string email = emailTxt.Text.Trim();

            Response.Redirect(
                $"QuerySearch.aspx?username={Server.UrlEncode(username)}&email={Server.UrlEncode(email)}");
        }





    }
}