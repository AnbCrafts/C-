using System;
using System.Data;

namespace GridView_ObjectDataSource_Practice
{
    public partial class UserListDetailedDescription : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUser();
            }
        }

        private void LoadUser()
        {
            int userId;

            if (Request.QueryString["UserId"] != null
                && int.TryParse(
                    Request.QueryString["UserId"],
                    out userId))
            {
                DataTable dt =
                    QueryUserDALStatic.GetUserById(userId);

                gvUserDetails.DataSource = dt;
                gvUserDetails.DataBind();
            }
        }
    }
}