using System;
using System.Web.UI.WebControls;

namespace URLRouting
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public Panel SearchPanel
        {
            get
            {
                return panelSearch;
            }
        }

        public string UserName
        {
            get
            {
                return txtUsername.Text.Trim();
            }
        }

        public string Email
        {
            get
            {
                return txtEmail.Text.Trim();
            }
        }

        public Button SearchButton
        {
            get
            {
                return btnSearch;
            }
        }
    }
}