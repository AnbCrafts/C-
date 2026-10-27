using System;
using System.Web.UI;

namespace DevHelper.Labs
{
    public partial class AjaxLab : Page
    {
        private const string ClickCountKey = "Lab_AjaxClickCount";

        protected void Page_Load(object sender, EventArgs e)
        {
            lblOuterClock.Text = DateTime.Now.ToString("HH:mm:ss.fff");

            if (!IsPostBack)
            {
                ViewState[ClickCountKey] = 0;
                lblInnerClock.Text = DateTime.Now.ToString("HH:mm:ss.fff");
                lblClickCount.Text = "0 clicks recorded";
            }
        }

        protected void btnFullPostBack_Click(object sender, EventArgs e)
        {
            // Triggers a full browser reload
            lblOuterClock.Text = DateTime.Now.ToString("HH:mm:ss.fff");
        }

        protected void btnAsyncClick_Click(object sender, EventArgs e)
        {
            // Only this code runs inside the partial async postback
            int count = (int)(ViewState[ClickCountKey] ?? 0) + 1;
            ViewState[ClickCountKey] = count;

            lblInnerClock.Text = DateTime.Now.ToString("HH:mm:ss.fff");
            lblClickCount.Text = $"Async updated {count} time{(count == 1 ? "" : "s")}!";
        }
    }
}
