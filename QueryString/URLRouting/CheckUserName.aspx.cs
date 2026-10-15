using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace URLRouting
{
    public partial class CheckUserName : System.Web.UI.Page
    {
        string queryDBConnection = ConfigurationManager.ConnectionStrings["urlRoutingDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnCheckUsername_Click(object sender, EventArgs e)
        {

            System.Threading.Thread.Sleep(3000);
            string username = txtUsername.Text.Trim();

            using (SqlConnection con = new SqlConnection(queryDBConnection))
            {
                string query = "SELECT UserName FROM labTest.QueryUser WHERE UserName = @username";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@username", username);

                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        txtMessage.Text = "Username is not available, choose another one.";
                    }
                    else
                    {
                        txtMessage.Text = "Username is available.";
                    }
                }
            }
        }
    }
}