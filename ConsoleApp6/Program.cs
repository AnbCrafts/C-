using Microsoft.Extensions.Configuration;

namespace ConsoleApp6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string connectionString =
                configuration.GetConnectionString("connection_string");

            ProductRepository repo = new ProductRepository(connectionString);

            while (true)
            {
                Console.WriteLine("\n===== PRODUCT MANAGEMENT =====");
                Console.WriteLine("1. Create Product");
                Console.WriteLine("2. Get All Products");
                Console.WriteLine("3. Get Product By Id");
                Console.WriteLine("4. Update Product");
                Console.WriteLine("5. Delete Product");
                Console.WriteLine("6. Exit");

                Console.Write("\nChoose Option: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid Input");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        {
                            Product product = new Product();

                            Console.Write("Enter Name: ");
                            product.Name = Console.ReadLine();

                            Console.Write("Enter Price: ");
                            product.Price = Convert.ToDecimal(Console.ReadLine());

                            Console.Write("Enter Stock: ");
                            product.Stock = Convert.ToInt32(Console.ReadLine());

                            repo.AddProduct(product);
                            break;
                        }

                    case 2:
                        {
                            List<Product> products = repo.GetProducts();

                            if (products.Count == 0)
                            {
                                Console.WriteLine("No Products Found.");
                                break;
                            }

                            foreach (Product product in products)
                            {
                                Console.WriteLine(
                                    $"Id: {product.Id}, " +
                                    $"Name: {product.Name}, " +
                                    $"Price: {product.Price}, " +
                                    $"Stock: {product.Stock}");
                            }

                            break;
                        }

                    case 3:
                        {
                            Console.Write("Enter Product Id: ");
                            int id = Convert.ToInt32(Console.ReadLine());

                            Product product = repo.GetProductbyId(id);

                            if (product != null)
                            {
                                Console.WriteLine("\nProduct Details");
                                Console.WriteLine($"Id    : {product.Id}");
                                Console.WriteLine($"Name  : {product.Name}");
                                Console.WriteLine($"Price : {product.Price}");
                                Console.WriteLine($"Stock : {product.Stock}");
                            }

                            break;
                        }

                    case 4:
                        {
                            Console.Write("Enter Product Id: ");
                            int id = Convert.ToInt32(Console.ReadLine());

                            Console.Write("Enter Product Name (Leave blank to skip): ");
                            string name = Console.ReadLine() ?? "";

                            Console.Write("Enter Price (Leave blank to skip): ");
                            string priceInput = Console.ReadLine() ?? "";

                            Console.Write("Enter Stock (Leave blank to skip): ");
                            string stockInput = Console.ReadLine() ?? "";

                            decimal price = string.IsNullOrWhiteSpace(priceInput)
                                ? 0
                                : Convert.ToDecimal(priceInput);

                            int stock = string.IsNullOrWhiteSpace(stockInput)
                                ? 0
                                : Convert.ToInt32(stockInput);

                            repo.UpdateProduct(id, name, price, stock);

                            break;
                        }

                    case 5:
                        {
                            Console.Write("Enter Product Id: ");
                            int id = Convert.ToInt32(Console.ReadLine());

                            repo.DeleteProduct(id);

                            break;
                        }

                    case 6:
                        {
                            Console.WriteLine("Exiting...");
                            return;
                        }

                    default:
                        {
                            Console.WriteLine("Invalid Choice");
                            break;
                        }
                }
            }
        }
    }
}