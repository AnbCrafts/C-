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
    public partial class SearchUser : System.Web.UI.Page
    {
        string queryDBConnection = ConfigurationManager.ConnectionStrings["urlRoutingDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userId = Convert.ToInt32(
                    Page.RouteData.Values["id"]
                    
                    );

                LoadUserDetails(userId);
            }
        }
        private void LoadUserDetails(int userId)
        {
            string query = "SELECT * FROM labTest.QueryUser WHERE UserId = @UserId";

            using (SqlConnection con = new SqlConnection(queryDBConnection))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        lblUserId.Text = reader["UserId"].ToString();
                        lblUserName.Text = reader["UserName"].ToString();
                        lblEmail.Text = reader["Email"].ToString();
                        lblFullName.Text = reader["FullName"].ToString();
                        lblMobileNo.Text = reader["MobileNo"].ToString();
                        lblCity.Text = reader["City"].ToString();
                        lblState.Text = reader["StateName"].ToString();
                        lblCountry.Text = reader["Country"].ToString();
                        lblGender.Text = reader["Gender"].ToString();
                        lblQueryText.Text = reader["QueryText"].ToString();
                        lblCreatedDate.Text = Convert.ToDateTime(reader["CreatedDate"])
                                               .ToString("dd-MMM-yyyy");
                        lblIsActive.Text = Convert.ToBoolean(reader["IsActive"])
                                            ? "Yes"
                                            : "No";
                    }
                }
            }
        }
        

    
    }
}