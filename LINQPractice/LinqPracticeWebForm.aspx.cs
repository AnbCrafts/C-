using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LINQPractice
{
    public partial class LinqPracticeWebForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindUserData();
            }
        }

        private void BindUserData()
        {
            using (QueryUserContext dataContext = new QueryUserContext())
            {
                var users =  dataContext.udp_GetAllUsers();
                            

                gvUsers.DataSource = users.ToList();
                gvUsers.DataBind();
            }
        }

        protected void Page_Changed(object sender, EventArgs e)
        {
            
        }
    }
}