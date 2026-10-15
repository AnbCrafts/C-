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
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
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
    }
}
