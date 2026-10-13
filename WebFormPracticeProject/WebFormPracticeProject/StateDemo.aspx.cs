using System;
using System.Web.UI;

namespace WebFormPracticeProject
{
    public partial class StateDemo : Page
    {
        private const string ViewStateCounterKey = "Demo_ViewStateCounter";
        private const string ViewStatePostBackCountKey = "Demo_PostBackCount";
        private const string ViewStateFirstLoadedKey = "Demo_FirstLoadedTime";
        private const string SessionCounterKey = "Demo_SessionCounter";
        private const string ViewStateAjaxClickCountKey = "Demo_AjaxClickCount";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Always runs on every HTTP request before control event handlers (e.g. btnClick)
            lblCurrentExecution.Text = DateTime.Now.ToString("T");

            if (!IsPostBack)
            {
                // This block executes ONLY on the first request (GET)
                string firstTime = DateTime.Now.ToString("T");
                ViewState[ViewStateFirstLoadedKey] = firstTime;
                ViewState[ViewStatePostBackCountKey] = 0;
                ViewState[ViewStateCounterKey] = 0;
                ViewState[ViewStateAjaxClickCountKey] = 0;

                lblFirstLoaded.Text = firstTime;
                lblPostBackCount.Text = "0 (Initial GET)";
                lblServerClock.Text = DateTime.Now.ToString("T");
            }
            else
            {
                // This block executes on any POST (Button clicks, etc.)
                int count = (int)(ViewState[ViewStatePostBackCountKey] ?? 0) + 1;
                ViewState[ViewStatePostBackCountKey] = count;
                lblPostBackCount.Text = $"{count} postback(s)";
                lblFirstLoaded.Text = (string)ViewState[ViewStateFirstLoadedKey];
            }

            // Sync ViewState & Session labels
            lblViewStateVal.Text = ((int)(ViewState[ViewStateCounterKey] ?? 0)).ToString();

            if (Session[SessionCounterKey] == null)
            {
                Session[SessionCounterKey] = 0;
            }
            lblSessionVal.Text = Session[SessionCounterKey].ToString();
        }

        protected void btnTriggerPostBack_Click(object sender, EventArgs e)
        {
            // Triggers full page reload / postback cycle
        }

        protected void btnIncrementViewState_Click(object sender, EventArgs e)
        {
            int currentVal = (int)(ViewState[ViewStateCounterKey] ?? 0);
            currentVal++;
            ViewState[ViewStateCounterKey] = currentVal;
            lblViewStateVal.Text = currentVal.ToString();
        }

        protected void btnIncrementSession_Click(object sender, EventArgs e)
        {
            int currentVal = (int)(Session[SessionCounterKey] ?? 0);
            currentVal++;
            Session[SessionCounterKey] = currentVal;
            lblSessionVal.Text = currentVal.ToString();
        }

        protected void btnAjaxClick_Click(object sender, EventArgs e)
        {
            // Runs inside an asynchronous partial page postback (UpdatePanel)
            int clicks = (int)(ViewState[ViewStateAjaxClickCountKey] ?? 0) + 1;
            ViewState[ViewStateAjaxClickCountKey] = clicks;

            lblServerClock.Text = DateTime.Now.ToString("T");
            lblAjaxClickCount.Text = $"Ajax clicked {clicks} time{(clicks == 1 ? "" : "s")}!";
        }
    }
}
