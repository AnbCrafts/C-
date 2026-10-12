using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PracticeSet1
{
    public partial class RedirectedPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lbl3.Text = Session["txtb1"]?.ToString()??"No user found";
            lbl4.Text = Session["txtb2"]?.ToString()??"No password found";

        }
    }
}