using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;

namespace WebServicePractice
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class WebService1 : System.Web.Services.WebService
    {
        string connection = ConfigurationManager.ConnectionStrings["webServiceDB"].ConnectionString;
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        [WebMethod]
        public XmlElement GetUserDetails(string userName)
        {
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from labTest.QueryUser where UserName like @userName+'%'", con);
            cmd.Parameters.AddWithValue("@userName", userName);
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();
            XmlDataDocument xmldata = new XmlDataDocument(ds);
            XmlElement xmlElement = xmldata.DocumentElement;
            return xmlElement;
        }

        [WebMethod]
        public XmlElement GetAllUserDetails()
        {
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from labTest.QueryUser", con);
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();
            XmlDataDocument xmldata = new XmlDataDocument(ds);
            XmlElement xmlElement = xmldata.DocumentElement;
            return xmlElement;
        }

        [WebMethod]
        public XmlElement GetMillionUsertableData()
        {
            SqlConnection con = new SqlConnection(connection);
            
            SqlCommand cmd = new SqlCommand("select * from [labTest].[tblOrder]",con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            XmlDataDocument doc = new XmlDataDocument(ds);
            XmlElement xmldata = doc.DocumentElement;
            return (xmldata);

        }

        [WebMethod] 
        public string DeleteUser(int id)
        {
            string message = "";

            using (SqlConnection con = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM [labTest].[QueryUser] WHERE UserId = @UserId",
                    con);

                cmd.Parameters.AddWithValue("@UserId", id);

                con.Open();

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    message = $"The row with id -> {id} has been deleted";
                }
                else
                {
                    message = $"The row with id -> {id} could not be deleted";
                }
            }

            return message;
        }

        [WebMethod]
        public string UpdateUser(
                int userId,
                string userName,
                string email,
                string fullName,
                string mobileNo,
                string city)
        {
            string message = "";

            using (SqlConnection con = new SqlConnection(connection))
            {
                string query = @"
            UPDATE labTest.QueryUser
            SET
                UserName = @UserName,
                Email = @Email,
                FullName = @FullName,
                MobileNo = @MobileNo,
                City = @City
            WHERE UserId = @UserId";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@FullName", fullName);
                cmd.Parameters.AddWithValue("@MobileNo", mobileNo);
                cmd.Parameters.AddWithValue("@City", city);

                con.Open();

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    message = $"User {userId} updated successfully";
                }
                else
                {
                    message = $"User {userId} could not be updated";
                }
            }

            return message;
        }



    }
}
