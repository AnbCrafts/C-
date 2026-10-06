//using System;
//using System.Collections.Generic;
//using System.Configuration;    // DOT NET DATA PROVIDER
//using System.Data.SqlClient;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Data;

//namespace ServerConnectionInitiation
//{
//    public class Program
//    {
//        //static void Main(string[] args)
//        //{
//        //    //string connectionString = "Data Source=.;Initial Catalog=AssignmentDB;User ID=sa;Password=mcc#1234";

//        //    //using app.config file

//        //    string connectionString = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

//        //    using (SqlConnection conn = new SqlConnection(connectionString))
//        //    {
//        //        conn.Open();
//        //        string query = "select * from Assignment.products  INNER JOIN Assignment.productVariant ON Assignment.products.productId = Assignment.productVariant.productId";

//        //        //SqlCommand cmd = new SqlCommand(query, conn);
//        //        //using (SqlDataReader reader = cmd.ExecuteReader())
//        //        //{
//        //        //    while (reader.Read())
//        //        //    {
//        //        //        Console.WriteLine($"ID : {reader["productId"]} Name : {reader["productName"]}");

//        //        //    }
//        //        //    conn.Close();

//        //        //    conn.Open();

//        //        //    string insertQuery = "INSERT INTO [labTest].[products]\r\n(productName, category, finishType, binderType, vocLevel, createdAt)\r\nVALUES\r\n('Apex3 Shield2','Exterior','Satin','Pure Acrylic',45.00,'2026-09-12')";
//        //        //    SqlCommand cmdI = new SqlCommand(insertQuery, conn);
//        //        //    int rowsAffected = cmdI.ExecuteNonQuery();
//        //        //    Console.WriteLine("Inserted Rows = " + rowsAffected);


//        //        //    Console.WriteLine("---------------");


//        //        //    Console.WriteLine("Using Data Reader");
//        //        //    SqlDataReader sdr = cmd.ExecuteReader();
//        //        //    while (sdr.Read())
//        //        //    {
//        //        //        Console.WriteLine(sdr["productId"] + ",  " + sdr["productName"]);
//        //        //    }
//        //        //    conn.Close();

//        //        //    Console.WriteLine("---------------");



//        //        //    conn.Open();
//        //        //    SqlDataAdapter da = new SqlDataAdapter("select * from Assignment.products", conn);
//        //        //    DataTable dt = new DataTable();
//        //        //    da.Fill(dt);



//        //        //    Console.WriteLine("---------------");


//        //        //    Console.WriteLine("Using Data Table");


//        //        //    foreach (DataRow row in dt.Rows)
//        //        //    {
//        //        //        Console.WriteLine(row["productId"] + ",  " + row["productName"]);
//        //        //    }
//        //        //    Console.WriteLine("---------------");

//        //        //}



//        //    }



//        //}

//        public static void Main(string[] args)
//        {
//            BikeshopRepo repo = new BikeshopRepo();
//            repo.CreateDataTable("select * from labTest.Bikeshop");

//            //while (true)
//            //{
//            //    Console.Clear();

//            //    Console.WriteLine("\n*************************** Operations ************************\n");
//            //    //Console.WriteLine("1 - Add Bike");
//            //    //Console.WriteLine("2 - Get All Bikes");
//            //    //Console.WriteLine("3 - Get Bike By Id");
//            //    //Console.WriteLine("4 - Update Bike");
//            //    //Console.WriteLine("5 - Delete Bike");
//            //    //Console.WriteLine("6 - Exit");
//            //    //Console.Write("\nEnter your choice: ");

//            //    //int choice;
//            //    //if (!int.TryParse(Console.ReadLine(), out choice))
//            //    //{
//            //    //    Console.WriteLine("Invalid choice!");
//            //    //    Console.ReadKey();
//            //    //    continue;
//            //    //}

//            //    //BikeshopRepo repo = new BikeshopRepo();


//            //    //switch (choice)
//            //    //{
//            //    //    case 1:
//            //    //        Console.Write("Enter Bike Name: ");
//            //    //        string name = Console.ReadLine();

//            //    //        Console.Write("Enter Bike Price: ");
//            //    //        int price = Convert.ToInt32(Console.ReadLine());

//            //    //        Bikeshop bike = new Bikeshop
//            //    //        {
//            //    //            Name = name,
//            //    //            Price = price
//            //    //        };

//            //    //        repo.AddBikeShop(bike);
//            //    //        Console.WriteLine("Bike Added Successfully!");
//            //    //        break;

//            //    //    case 2:
//            //    //        List<Bikeshop> bikes = repo.GetAllBikeShop();

//            //    //        foreach (var b in bikes)
//            //    //        {
//            //    //            Console.WriteLine(
//            //    //                $"Id: {b.Id}, Name: {b.Name}, Price: {b.Price}");
//            //    //        }
//            //    //        break;

//            //    //    case 3:
//            //    //        Console.Write("Enter Bike Id: ");
//            //    //        int getId = Convert.ToInt32(Console.ReadLine());

//            //    //        Bikeshop foundBike = repo.GetBikeById(getId);

//            //    //        if (foundBike != null)
//            //    //        {
//            //    //            Console.WriteLine(
//            //    //                $"Id: {foundBike.Id}, Name: {foundBike.Name}, Price: {foundBike.Price}");
//            //    //        }
//            //    //        else
//            //    //        {
//            //    //            Console.WriteLine("Bike not found.");
//            //    //        }
//            //    //        break;

//            //    //    case 4:
//            //    //        Console.Write("Enter Bike Id: ");
//            //    //        int updateId = Convert.ToInt32(Console.ReadLine());

//            //    //        Console.Write("Enter New Name: ");
//            //    //        string newName = Console.ReadLine();

//            //    //        Console.Write("Enter New Price: ");
//            //    //        decimal newPrice = Convert.ToDecimal(Console.ReadLine());

//            //    //        repo.UpdateBikeById(updateId, newName, newPrice);
//            //    //        Console.WriteLine("Bike Updated Successfully!");
//            //    //        break;

//            //    //    case 5:
//            //    //        Console.Write("Enter Bike Id: ");
//            //    //        int deleteId = Convert.ToInt32(Console.ReadLine());

//            //    //        repo.DeleteBikeById(deleteId);
//            //    //        Console.WriteLine("Bike Deleted Successfully!");
//            //    //        break;

//            //    //    case 6:
//            //    //        return;

//            //    //    default:
//            //    //        Console.WriteLine("Invalid Choice!");
//            //    //        break;
//            //    //}

//            //    Console.WriteLine("\nPress any key to continue...");
//            //    Console.ReadKey();
//            //}
//        }


//    }
//}


using System;

namespace ServerConnectionInitiation
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            BikeshopRepo repo = new BikeshopRepo();
            repo.CreateDataTable("select * from labTest.Bikeshop");
            repo.CreateDataSet("select * from labTest.Bikeshop");
            repo.CreateDataSetByProcedure(2);


        }

    }
}