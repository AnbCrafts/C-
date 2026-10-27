using System;
using System.Web.UI;

namespace DevHelper.Labs
{
    public partial class MasterPagesLab : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblCurrentTitle.Text = Page.Title;
                txtCustomTitle.Text = "Master Pages Lab - Custom Title";
            }
        }

        protected void btnUpdateTitle_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCustomTitle.Text.Trim()))
            {
                Page.Title = txtCustomTitle.Text.Trim();
                lblCurrentTitle.Text = Page.Title;
            }
        }
    }
}
