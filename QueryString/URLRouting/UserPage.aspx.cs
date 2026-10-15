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
    public partial class UserPage : System.Web.UI.Page
    {
        string queryDBConnection = ConfigurationManager.ConnectionStrings["urlRoutingDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string email = txtEmail.Text.Trim();

            string query = "select * from labTest.QueryUser where UserName = @username and Email = @email";
            using (SqlConnection con = new SqlConnection(queryDBConnection))
            {

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("username", username);
                    cmd.Parameters.AddWithValue("email", email);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtMessage.Text = "Found the user ";
                        Response.Redirect(GetRouteUrl(
                        "RouteForUser",
                        new
                        {
                            id = reader["UserId"],
                            username = reader["UserName"]
                        }));
                    }
                    else
                    {
                        txtMessage.Text = "Could not find the user ";

                    }
                }
            }


        }
           

    
    
    
    }

}