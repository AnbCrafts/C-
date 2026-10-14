using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QueryString
{

    public partial class QuerySearch : System.Web.UI.Page
    {
        string queryDBConnection = ConfigurationManager.ConnectionStrings["QueryDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string username = this.Request.QueryString["username"];
                string email = this.Request.QueryString["email"];
                usernameTxt.Text = username;
                emailTxt.Text = email;
                //FetchUserDetails(username,email);

            }


        }

        private void FetchUserDetails(string username, string email)
        {
            using (SqlConnection con = new SqlConnection(queryDBConnection))
            {
                string query = "SELECT * FROM labTest.QueryUser WHERE UserName = @username AND Email = @email";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                   

                    con.Open();
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@email", email);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            successMessage.ForeColor = System.Drawing.Color.Green;
                            successMessage.Text = "User found successfully";

                            // Example: Access values from the row
                            string fullName = rdr["FullName"].ToString();
                            string mobile = rdr["MobileNo"].ToString();

                          
                        }
                        else
                        {
                            successMessage.ForeColor = System.Drawing.Color.Red;
                            successMessage.Text = "User not found";
                        }
                    }
                }
            }
        }

    }
}