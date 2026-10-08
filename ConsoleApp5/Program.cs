using ConsoleApp5.Helpers;
using ConsoleApp5.Models;
using ConsoleApp5.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Fleet Management - Vehicle Module";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================================================");
            Console.WriteLine("             FLEET MANAGEMENT SYSTEM - VEHICLE FLOW               ");
            Console.WriteLine("==================================================================");
            Console.ResetColor();

            string connectionString = ConfigurationManager.ConnectionStrings["DBConn"]?.ConnectionString;

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[ERROR] 'DBConn' connection string is missing from App.config!");
                Console.ResetColor();
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            using (SqlHelper sqlHelper = new SqlHelper(connectionString))
            {
                VehicleRepo vehicleRepo = new VehicleRepo(sqlHelper);

                try
                {
                    Console.Write("Connecting to database & ensuring table exists... ");
                    await vehicleRepo.EnsureTableExistsAsync();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("SUCCESS\n");
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"FAILED\n[Database Connection Error]: {ex.Message}");
                    Console.ResetColor();
                    Console.WriteLine("Please ensure SQL Server is running and connection string in App.config is correct.");
                    Console.WriteLine("\nPress any key to exit...");
                    Console.ReadKey();
                    return;
                }

                bool exit = false;
                while (!exit)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n------------------- VEHICLE MANAGEMENT MENU -------------------");
                    Console.ResetColor();
                    Console.WriteLine("1. Run Automated End-to-End Vehicle Flow Demo");
                    Console.WriteLine("2. View All Vehicles");
                    Console.WriteLine("3. Search Vehicle by ID");
                    Console.WriteLine("4. Add New Vehicle");
                    Console.WriteLine("5. Update Existing Vehicle");
                    Console.WriteLine("6. Delete Vehicle");
                    Console.WriteLine("0. Exit");
                    Console.Write("\nChoose an option [0-6]: ");

                    string choice = Console.ReadLine()?.Trim();
                    Console.WriteLine();

                    switch (choice)
                    {
                        case "1":
                            await RunAutomatedDemoAsync(vehicleRepo);
                            break;
                        case "2":
                            await ViewAllVehiclesAsync(vehicleRepo);
                            break;
                        case "3":
                            await SearchVehicleByIdAsync(vehicleRepo);
                            break;
                        case "4":
                            await AddVehicleAsync(vehicleRepo);
                            break;
                        case "5":
                            await UpdateVehicleAsync(vehicleRepo);
                            break;
                        case "6":
                            await DeleteVehicleAsync(vehicleRepo);
                            break;
                        case "0":
                            exit = true;
                            Console.WriteLine("Exiting application. Goodbye!");
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Invalid choice. Please select 0 to 6.");
                            Console.ResetColor();
                            break;
                    }
                }
            }
        }

        private static async Task RunAutomatedDemoAsync(VehicleRepo vehicleRepo)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(">>> STARTING AUTOMATED END-TO-END VEHICLE FLOW <<<\n");
            Console.ResetColor();

            // 1. CREATE
            Console.WriteLine("1. [CREATE] Adding a new Vehicle...");
            string randomVin = "1HGCR2F83HA" + new Random().Next(100000, 999999);
            Vehicle newVehicle = new Vehicle(
                vin: randomVin,
                manufacturer: "Honda",
                model: "Civic",
                odometerReading: 15420.50m,
                isActive: true
            );

            await vehicleRepo.CreateAsync(newVehicle);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"   --> Vehicle created with generated ID: {newVehicle.Id}");
            Console.ResetColor();

            // 2. READ ALL
            Console.WriteLine("\n2. [READ ALL] Fetching all vehicles from database...");
            var allVehicles = await vehicleRepo.GetAllAsync();
            Console.WriteLine($"   --> Total vehicles found: {allVehicles.Count()}");
            foreach (var v in allVehicles.Take(5))
            {
                Console.WriteLine($"      {v}");
            }
            if (allVehicles.Count() > 5)
            {
                Console.WriteLine($"      ... and {allVehicles.Count() - 5} more.");
            }

            // 3. READ BY ID
            Console.WriteLine($"\n3. [READ BY ID] Fetching vehicle with ID {newVehicle.Id}...");
            var fetchedVehicle = await vehicleRepo.GetByIdAsync(newVehicle.Id);
            if (fetchedVehicle != null)
            {
                Console.WriteLine($"   --> Found: {fetchedVehicle}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("   --> Vehicle not found!");
                Console.ResetColor();
            }

            // 4. UPDATE
            Console.WriteLine($"\n4. [UPDATE] Modifying Vehicle ID {newVehicle.Id}...");
            newVehicle.OdometerReading += 500.00m;
            newVehicle.Model = "Civic Touring";
            await vehicleRepo.UpdateAsync(newVehicle);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"   --> Vehicle updated: Odometer is now {newVehicle.OdometerReading:N2} km, Model: {newVehicle.Model}");
            Console.ResetColor();

            var updatedVehicle = await vehicleRepo.GetByIdAsync(newVehicle.Id);
            Console.WriteLine($"   --> Verified from DB: {updatedVehicle}");

            // 5. DELETE
            Console.WriteLine($"\n5. [DELETE] Deleting test Vehicle ID {newVehicle.Id}...");
            await vehicleRepo.DeleteAsync(newVehicle.Id);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"   --> Vehicle ID {newVehicle.Id} deleted successfully.");
            Console.ResetColor();

            var checkDeleted = await vehicleRepo.GetByIdAsync(newVehicle.Id);
            if (checkDeleted == null)
            {
                Console.WriteLine("   --> Verification: Confirmed vehicle no longer exists in DB.");
            }

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n>>> END-TO-END VEHICLE FLOW DEMO COMPLETED SUCCESSFULLY <<<\n");
            Console.ResetColor();
        }

        private static async Task ViewAllVehiclesAsync(VehicleRepo vehicleRepo)
        {
            var vehicles = await vehicleRepo.GetAllAsync();
            var list = vehicles.ToList();

            if (list.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No vehicles found in the database.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Found {list.Count} vehicle(s):");
            Console.ResetColor();
            Console.WriteLine(new string('-', 95));
            foreach (var v in list)
            {
                Console.WriteLine(v);
            }
            Console.WriteLine(new string('-', 95));
        }

        private static async Task SearchVehicleByIdAsync(VehicleRepo vehicleRepo)
        {
            Console.Write("Enter Vehicle ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var vehicle = await vehicleRepo.GetByIdAsync(id);
                if (vehicle != null)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nVehicle Found:");
                    Console.ResetColor();
                    Console.WriteLine(vehicle);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"No vehicle found with ID {id}.");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid ID entered.");
                Console.ResetColor();
            }
        }

        private static async Task AddVehicleAsync(VehicleRepo vehicleRepo)
        {
            Console.WriteLine("--- Add New Vehicle ---");
            Console.Write("Enter VIN (or press Enter for auto-generated): ");
            string vin = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(vin))
            {
                vin = "VIN" + DateTime.UtcNow.Ticks.ToString().Substring(10);
            }

            Console.Write("Enter Manufacturer (e.g., Toyota): ");
            string make = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(make)) make = "Toyota";

            Console.Write("Enter Model (e.g., Camry): ");
            string model = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(model)) model = "Camry";

            Console.Write("Enter Current Odometer Reading (km): ");
            string odoInput = Console.ReadLine();
            decimal.TryParse(odoInput, out decimal odo);

            var vehicle = new Vehicle(vin, make, model, odo, true);
            await vehicleRepo.CreateAsync(vehicle);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nVehicle added successfully with ID: {vehicle.Id}");
            Console.ResetColor();
        }

        private static async Task UpdateVehicleAsync(VehicleRepo vehicleRepo)
        {
            Console.Write("Enter ID of vehicle to update: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid ID.");
                Console.ResetColor();
                return;
            }

            var vehicle = await vehicleRepo.GetByIdAsync(id);
            if (vehicle == null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Vehicle with ID {id} not found.");
                Console.ResetColor();
                return;
            }

            Console.WriteLine($"\nCurrent details: {vehicle}");
            Console.Write($"Enter new VIN (press Enter to keep '{vehicle.VIN}'): ");
            string vin = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(vin)) vehicle.VIN = vin;

            Console.Write($"Enter new Manufacturer (press Enter to keep '{vehicle.Manufacturer}'): ");
            string make = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(make)) vehicle.Manufacturer = make;

            Console.Write($"Enter new Model (press Enter to keep '{vehicle.Model}'): ");
            string model = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(model)) vehicle.Model = model;

            Console.Write($"Enter new Odometer Reading (press Enter to keep '{vehicle.OdometerReading}'): ");
            string odoInput = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(odoInput) && decimal.TryParse(odoInput, out decimal odo))
            {
                vehicle.OdometerReading = odo;
            }

            Console.Write($"Is vehicle active? (y/n, press Enter to keep '{vehicle.IsActive}'): ");
            string activeInput = Console.ReadLine()?.Trim().ToLower();
            if (activeInput == "y") vehicle.IsActive = true;
            else if (activeInput == "n") vehicle.IsActive = false;

            await vehicleRepo.UpdateAsync(vehicle);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nVehicle ID {id} updated successfully!");
            Console.ResetColor();
        }

        private static async Task DeleteVehicleAsync(VehicleRepo vehicleRepo)
        {
            Console.Write("Enter ID of vehicle to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var vehicle = await vehicleRepo.GetByIdAsync(id);
                if (vehicle == null)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Vehicle with ID {id} not found.");
                    Console.ResetColor();
                    return;
                }

                Console.Write($"Are you sure you want to delete {vehicle.Manufacturer} {vehicle.Model} (ID {id})? (y/n): ");
                string confirm = Console.ReadLine()?.Trim().ToLower();
                if (confirm == "y")
                {
                    await vehicleRepo.DeleteAsync(id);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Vehicle ID {id} deleted successfully.");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("Deletion cancelled.");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid ID.");
                Console.ResetColor();
            }
        }
    }
}
