using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

namespace WebServicePractice
{
    public partial class UserListMannualEditPage : System.Web.UI.Page
    {
        protected void gvUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvUsers.EditIndex = e.NewEditIndex;
            LoadUsers();
        }

        protected void gvUsers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvUsers.EditIndex = -1;
            LoadUsers();
        }

        protected void gvUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvUsers.Rows[e.RowIndex];

            int userId = Convert.ToInt32(
                gvUsers.DataKeys[e.RowIndex].Value);

            string userName =
                ((TextBox)row.FindControl("txtUserName")).Text;

            string email =
                ((TextBox)row.FindControl("txtEmail")).Text;

            string fullName =
                ((TextBox)row.FindControl("txtFullName")).Text;

            string mobile =
                ((TextBox)row.FindControl("txtMobile")).Text;

            string city =
                ((TextBox)row.FindControl("txtCity")).Text;

            PracticeWebServiceRef.WebService1SoapClient client =
                new PracticeWebServiceRef.WebService1SoapClient();

            string result = client.UpdateUser(
                userId,
                userName,
                email,
                fullName,
                mobile,
                city);

            gvUsers.EditIndex = -1;

            LoadUsers();
        }



        protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int userId = Convert.ToInt32(
                gvUsers.DataKeys[e.RowIndex].Value);

            PracticeWebServiceRef.WebService1SoapClient client =
                new PracticeWebServiceRef.WebService1SoapClient();

            string result = client.DeleteUser(userId);

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

        protected void gvUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUsers.PageIndex = e.NewPageIndex;
            LoadUsers();
        }

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
    }
}