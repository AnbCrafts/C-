using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using JQueryPractice.Models;

namespace JQueryPractice.Controllers
{
    public class UserController : Controller
    {
        private string GetConnectionString()
        {
            var cs = ConfigurationManager.ConnectionStrings["AssignmentDBEntities"];
            if (cs != null && !string.IsNullOrEmpty(cs.ConnectionString))
            {
                return cs.ConnectionString;
            }
            return "Data Source=.;Initial Catalog=AssignmentDB;Integrated Security=True;TrustServerCertificate=True;";
        }

        // GET: /User/InsertUserDetails (Slides 16-17)
        public ActionResult InsertUserDetails()
        {
            UserDetails objuser = new UserDetails();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("usercrudoperations", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "GET");
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                    List<UserDetails> userlist = new List<UserDetails>();
                    if (ds.Tables.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            UserDetails uobj = new UserDetails();
                            uobj.UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["userid"].ToString());
                            uobj.UserName = ds.Tables[0].Rows[i]["username"].ToString();
                            uobj.Education = ds.Tables[0].Rows[i]["education"].ToString();
                            uobj.Location = ds.Tables[0].Rows[i]["location"].ToString();
                            userlist.Add(uobj);
                        }
                    }
                    objuser.usersinfo = userlist;
                    con.Close();
                }
            }

            return View(objuser);
        }

        // POST: /User/InsertUserDetails (Slide 18)
        [HttpPost]
        public ActionResult InsertUserDetails(UserDetails user)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("usercrudoperations", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@name", user.UserName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@education", user.Education ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@location", user.Location ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@status", "INSERT");
                    con.Open();
                    int affected = cmd.ExecuteNonQuery();
                    ViewData["result"] = affected > 0 ? "1" : "0";
                    con.Close();
                }
            }

            // Reload the list and pass to view
            UserDetails objuser = new UserDetails();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("usercrudoperations", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", "GET");
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                    List<UserDetails> userlist = new List<UserDetails>();
                    if (ds.Tables.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            UserDetails uobj = new UserDetails();
                            uobj.UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["userid"].ToString());
                            uobj.UserName = ds.Tables[0].Rows[i]["username"].ToString();
                            uobj.Education = ds.Tables[0].Rows[i]["education"].ToString();
                            uobj.Location = ds.Tables[0].Rows[i]["location"].ToString();
                            userlist.Add(uobj);
                        }
                    }
                    objuser.usersinfo = userlist;
                    con.Close();
                }
            }

            return View(objuser);
        }
    }
}
