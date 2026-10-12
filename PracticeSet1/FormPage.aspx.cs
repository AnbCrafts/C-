using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PracticeSet1
{
    public partial class FormPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["txtb1"] = txtb1.Text; 
            Session["txtb2"] = txtb2.Text;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (lbl1.Text.Length != 0 && lbl2.Text.Length!=0)
            {
            Response.Redirect("RedirectedPage.aspx");

            }

        }

        protected void txtb2_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtb1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}