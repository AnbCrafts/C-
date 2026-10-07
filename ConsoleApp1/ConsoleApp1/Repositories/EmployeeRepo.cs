using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace ConsoleApp1.Repositories
{
    public class EmployeeRepo
    {
        public static List<Employee> empLoyeeList = new List<Employee>();

        private static string GetConnectionString()
        {
            var connSetting = ConfigurationManager.ConnectionStrings["StoreDB"];
            if (connSetting != null && !string.IsNullOrWhiteSpace(connSetting.ConnectionString))
            {
                return connSetting.ConnectionString;
            }
            return "Data Source=ANUBHAW;Initial Catalog=master;Integrated Security=True;TrustServerCertificate=True;";
        }

        // 1. ADD EMPLOYEE TO DB (ExecuteScalar to retrieve generated EmployeeId)
        public static void AddEmployee(Employee emp, Store? store = null)
        {
            if (emp == null)
            {
                Console.WriteLine("An employee object is required to add employee.\n");
                return;
            }

            if (store != null)
            {
                emp.storeAssigned = true;
                emp.StoreId = store.StoreId;
            }

            string query = @"INSERT INTO practiceProjects.Employees (Name, Designation, Salary, Department, StoreId)
                             VALUES (@Name, @Designation, @Salary, @Department, @StoreId);
                             SELECT SCOPE_IDENTITY();";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", emp.Name ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Designation", emp.Designation ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Salary", emp.Salary);
                    cmd.Parameters.AddWithValue("@Department", emp.Department ?? string.Empty);
                    cmd.Parameters.AddWithValue("@StoreId", emp.StoreId > 0 ? emp.StoreId : (object)DBNull.Value);

                    try
                    {
                        conn.Open();
                        object newId = cmd.ExecuteScalar();
                        if (newId != null && newId != DBNull.Value)
                        {
                            emp.EmployeeId = Convert.ToInt32(newId);
                        }
                        Console.WriteLine($"Employee '{emp.Name}' added to DB with EmployeeId = {emp.EmployeeId}.\n");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error adding employee to DB: {ex.Message}\n");
                    }
                }
            }
        }

        // 2. GET ALL EMPLOYEES FROM DB (ExecuteReader)
        public static List<Employee> GetAllEmployees()
        {
            var list = new List<Employee>();
            string query = "SELECT EmployeeId, Name, Designation, Salary, Department, StoreId FROM practiceProjects.Employees";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int storeId = reader["StoreId"] != DBNull.Value ? Convert.ToInt32(reader["StoreId"]) : 0;
                                list.Add(new Employee
                                {
                                    EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                                    Name = reader["Name"].ToString() ?? string.Empty,
                                    Designation = reader["Designation"].ToString() ?? string.Empty,
                                    Salary = Convert.ToDouble(reader["Salary"]),
                                    Department = reader["Department"].ToString() ?? string.Empty,
                                    StoreId = storeId,
                                    StoreAssigned = storeId > 0
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error fetching employees: {ex.Message}\n");
                    }
                }
            }
            return list;
        }

        // 3. GET EMPLOYEES WORKING IN STORE (ExecuteReader)
        public static List<Employee> GetEmployeeWorkingInStore(Store store)
        {
            if (store == null) return new List<Employee>();
            return StoreRepo.GetEmpWorkingInStore(store.StoreId);
        }

        // 4. GET EMPLOYEE BY ID (ExecuteReader)
        public static Employee? GetEmployeeById(int empId)
        {
            string query = "SELECT EmployeeId, Name, Designation, Salary, Department, StoreId FROM practiceProjects.Employees WHERE EmployeeId = @Id";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", empId);
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int storeId = reader["StoreId"] != DBNull.Value ? Convert.ToInt32(reader["StoreId"]) : 0;
                                return new Employee
                                {
                                    EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                                    Name = reader["Name"].ToString() ?? string.Empty,
                                    Designation = reader["Designation"].ToString() ?? string.Empty,
                                    Salary = Convert.ToDouble(reader["Salary"]),
                                    Department = reader["Department"].ToString() ?? string.Empty,
                                    StoreId = storeId,
                                    StoreAssigned = storeId > 0
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error fetching employee by ID: {ex.Message}\n");
                    }
                }
            }

            Console.WriteLine($"Employee with ID {empId} not found in DB.\n");
            return null;
        }

        // 5. UPDATE EMPLOYEE IN DB (ExecuteNonQuery)
        public static void UpdateEmployee(int empId, string name, string designation, double salary, string department)
        {
            string query = @"UPDATE practiceProjects.Employees
                             SET Name = ISNULL(NULLIF(@Name, ''), Name),
                                 Designation = ISNULL(NULLIF(@Designation, ''), Designation),
                                 Salary = CASE WHEN @Salary > 0 THEN @Salary ELSE Salary END,
                                 Department = ISNULL(NULLIF(@Department, ''), Department)
                             WHERE EmployeeId = @Id";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", empId);
                    cmd.Parameters.AddWithValue("@Name", name ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Designation", designation ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Salary", salary);
                    cmd.Parameters.AddWithValue("@Department", department ?? string.Empty);

                    try
                    {
                        conn.Open();
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                            Console.WriteLine($"Employee ID {empId} updated successfully in DB.\n");
                        else
                            Console.WriteLine($"Employee ID {empId} not found in DB.\n");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error updating employee: {ex.Message}\n");
                    }
                }
            }
        }

        // 6. REMOVE EMPLOYEE FROM DB (ExecuteNonQuery)
        public static void RemoveEmployee(int empId)
        {
            string query = "DELETE FROM practiceProjects.Employees WHERE EmployeeId = @Id";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", empId);
                    try
                    {
                        conn.Open();
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                            Console.WriteLine($"Employee ID {empId} deleted from DB.\n");
                        else
                            Console.WriteLine($"Employee ID {empId} not found in DB.\n");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error deleting employee: {ex.Message}\n");
                    }
                }
            }
        }

        // 7. GET EMPLOYEES BY DEPARTMENT FROM DB (ExecuteReader)
        public static List<Employee> GetEmployeesByDepartment(string department)
        {
            var list = new List<Employee>();
            string query = "SELECT EmployeeId, Name, Designation, Salary, Department, StoreId FROM practiceProjects.Employees WHERE Department = @Department";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Department", department ?? string.Empty);
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int storeId = reader["StoreId"] != DBNull.Value ? Convert.ToInt32(reader["StoreId"]) : 0;
                                list.Add(new Employee
                                {
                                    EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                                    Name = reader["Name"].ToString() ?? string.Empty,
                                    Designation = reader["Designation"].ToString() ?? string.Empty,
                                    Salary = Convert.ToDouble(reader["Salary"]),
                                    Department = reader["Department"].ToString() ?? string.Empty,
                                    StoreId = storeId,
                                    StoreAssigned = storeId > 0
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error searching employees by department: {ex.Message}\n");
                    }
                }
            }

            if (list.Count == 0)
            {
                Console.WriteLine($"No employees found in department '{department}'.\n");
            }
            return list;
        }
    }
}
