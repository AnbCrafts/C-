using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASP_DotNet_Web_Application
{
    public partial class MywebForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnClick_Click(object sender, EventArgs e)
        {
            lbl.Text = "Button Clicked";
        }
    }
}