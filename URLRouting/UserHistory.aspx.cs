using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace URLRouting
{
    public partial class UserHistory : System.Web.UI.Page
    {
        string queryDBConnection =
            ConfigurationManager.ConnectionStrings["urlRoutingDB"].ConnectionString;

        protected void Page_Init(object sender, EventArgs e)
        {
            Master.SearchButton.Click += SearchButton_Click;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUserHistory();
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string username = Master.UserName;
            string email = Master.Email;

            LoadUserHistory(username, email);
        }

        private void LoadUserHistory(
            string username = null,
            string email = null)
        {
            string query =
            @"SELECT UserId,
                     UserName,
                     FullName,
                     Email,
                     MobileNo,
                     City,
                     StateName,
                     Country,
                     Gender,
                     QueryText,
                     CreatedDate,
                     IsActive
              FROM labTest.QueryUser
              WHERE 1 = 1";

            if (!string.IsNullOrWhiteSpace(username))
            {
                query += " AND UserName LIKE @username";
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                query += " AND Email LIKE @email";
            }

            query += " ORDER BY UserId";

            using (SqlConnection con =
                new SqlConnection(queryDBConnection))
            {
                using (SqlCommand cmd =
                    new SqlCommand(query, con))
                {
                    if (!string.IsNullOrWhiteSpace(username))
                    {
                        cmd.Parameters.AddWithValue(
                            "@username",
                            "%" + username + "%");
                    }

                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        cmd.Parameters.AddWithValue(
                            "@email",
                            "%" + email + "%");
                    }

                    using (SqlDataAdapter da =
                        new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();

                        da.Fill(dt);

                        gvUserHistory.DataSource = dt;
                        gvUserHistory.DataBind();

                        if (dt.Rows.Count > 0)
                        {
                            lblStatus.Text =
                                $"Showing {dt.Rows.Count} user record(s).";

                            lblStatus.CssClass =
                                "status-message status-success";
                        }
                        else
                        {
                            lblStatus.Text =
                                "No user records found.";

                            lblStatus.CssClass =
                                "status-message status-empty";
                        }
                    }
                }
            }
        }

        protected void btnBack_Click(
            object sender,
            EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
    }
}