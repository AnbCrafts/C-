using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;


namespace PracticeSet2
{
    public partial class UserForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        protected void SubmitForm(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
            string insertQuery = "Insert into dbo.ProductTbl (Name,Price,Stock) values (@name,@price,@stock)";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                {
                    //cmd.CommandText = insertQuery;
                    //cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@name", nameVal.Text);
                    cmd.Parameters.AddWithValue("@price", priceVal.Text);
                    cmd.Parameters.AddWithValue("@stock", stockVal.Text);

                    con.Open();
                    int rows = Convert.ToInt32(cmd.ExecuteNonQuery());
                    if (rows > 0)
                    {
                        resMessage.Text =
                         "Name : " + nameVal.Text + "<br/>" +
                         "Price : " + priceVal.Text + "<br/>" +
                         "Stock : " + stockVal.Text;
                        Console.WriteLine($"Successfully added {rows} rows int the table");

                    }
                    else
                    {
                        Console.WriteLine($"Failed to add rows int the table");

                    }



                }
            }
            
        }


        protected void ValidateName(object source, ServerValidateEventArgs args)
            {
        string name = args.Value.Trim();

        if (name.Length < 3)
        {
            args.IsValid = false;
            return;
        }

        args.IsValid = Regex.IsMatch(name, @"^[A-Za-z\s]+$");
            
        
        }

    }
}