using System;
using System.Data;
using System.Web.UI;
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

        private void LoadUsers()
        {

            PracticeWebServiceRef.WebService1SoapClient client =
                new PracticeWebServiceRef.WebService1SoapClient();

            XElement xml = client.GetAllUserDetails();

            DataSet ds = new DataSet();

            using (XmlReader reader = xml.CreateReader())
            {
                ds.ReadXml(reader);
            }

            gvUsers.DataSource = ds.Tables[0];
            gvUsers.DataBind();
        }
    }
}