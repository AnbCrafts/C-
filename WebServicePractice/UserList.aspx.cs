using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;


namespace WebServicePractice
{
    public partial class UserList : System.Web.UI.Page
    {
        protected void btnLoad_Click(object sender, EventArgs e)
        {
            LoadUsers();
        }

        protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool isActive = Convert.ToBoolean(
                    DataBinder.Eval(e.Row.DataItem, "IsActive"));

                if (!isActive)
                {
                    e.Row.BackColor = System.Drawing.Color.Red;
                    e.Row.ForeColor = System.Drawing.Color.White;
                }
            }
        }
        private void LoadUsers()
        {

            PracticeWebServiceRef.WebService1SoapClient client =
            new PracticeWebServiceRef.WebService1SoapClient();

            XElement xml = client.GetAllUserDetails();
            //XElement xml = client.GetMillionUsertableData();

            DataSet ds = new DataSet();

            using (XmlReader reader = xml.CreateReader())
            {
                ds.ReadXml(reader);
            }

            gvUsers.DataSource = ds.Tables[0];
            gvUsers.DataBind();
        }

        protected void gvUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUsers.PageIndex = e.NewPageIndex;
            LoadUsers();
        }


    }
}