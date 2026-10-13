using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PracticeSet2
{
    public partial class LearnState : System.Web.UI.Page
    {
        //int ClicksCount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtBox.Text = "0";
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            int ClicksCount = 0;
            ClicksCount = ClicksCount + 1;
            txtBox.Text = ClicksCount.ToString();
            //if (ViewState["Clicks"] != null)
            //{
            //    ClicksCount = (int)ViewState["Clicks"] + 1;
            //}
            //txtBox.Text = ClicksCount.ToString(); ;
            //ViewState["Clicks"] = ClicksCount;


        }

        protected void Button1_Click_Protected(object sender, EventArgs e)
        {
            int ClicksCount = 0;
            if (ViewState["Clicks"] != null)
            {
                ClicksCount = (int)ViewState["Clicks"] + 1;
            }
            txtBox.Text = ClicksCount.ToString(); ;
            ViewState["Clicks"] = ClicksCount;
        }


    }
}