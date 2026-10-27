using System;
using System.Web.UI;

namespace DevHelper.Labs
{
    public partial class StateLab : Page
    {
        private const string ViewStateKey = "Lab_ViewStateCount";
        private const string SessionKey = "Lab_SessionCount";
        private const string ApplicationKey = "Lab_AppCount";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ViewState[ViewStateKey] == null) ViewState[ViewStateKey] = 0;
                if (Session[SessionKey] == null) Session[SessionKey] = 0;
                if (Application[ApplicationKey] == null) Application[ApplicationKey] = 0;
            }

            SyncDisplay();
        }

        private void SyncDisplay()
        {
            lblViewState.Text = ((int)(ViewState[ViewStateKey] ?? 0)).ToString();
            lblSession.Text = ((int)(Session[SessionKey] ?? 0)).ToString();
            lblApplication.Text = ((int)(Application[ApplicationKey] ?? 0)).ToString();
        }

        protected void btnIncViewState_Click(object sender, EventArgs e)
        {
            int val = (int)(ViewState[ViewStateKey] ?? 0) + 1;
            ViewState[ViewStateKey] = val;
            SyncDisplay();
        }

        protected void btnIncSession_Click(object sender, EventArgs e)
        {
            int val = (int)(Session[SessionKey] ?? 0) + 1;
            Session[SessionKey] = val;
            SyncDisplay();
        }

        protected void btnIncApp_Click(object sender, EventArgs e)
        {
            Application.Lock();
            int val = (int)(Application[ApplicationKey] ?? 0) + 1;
            Application[ApplicationKey] = val;
            Application.UnLock();
            SyncDisplay();
        }

        protected void btnResetState_Click(object sender, EventArgs e)
        {
            ViewState[ViewStateKey] = 0;
            Session[SessionKey] = 0;

            Application.Lock();
            Application[ApplicationKey] = 0;
            Application.UnLock();

            SyncDisplay();
        }
    }
}
