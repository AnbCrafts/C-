# ADO.NET Complete Practice Set & Revision Guide

> **Prepared for Exam & Interview Preparation**  
> Covers: `SqlConnection`, `SqlCommand` (Scalar, NonQuery, Reader), `SqlDataAdapter`, `DataTable`, `DataSet`, Stored Procedures with Input/Output Parameters, and `DataView` (Sorting & Filtering).

---

## 1. Quick Revision & Architecture Cheat Sheet

### Connected vs. Disconnected Architecture
- **Connected Mode (`SqlConnection`, `SqlCommand`, `SqlDataReader`)**:
  - Requires an active open connection to the database.
  - `SqlDataReader` is forward-only, read-only, and fast because it streams directly from the network.
  - Connection must be kept open during reading and closed immediately after.
- **Disconnected Mode (`SqlDataAdapter`, `DataTable`, `DataSet`, `DataView`)**:
  - Connection-independent.
  - `SqlDataAdapter.Fill()` automatically **opens** the connection, executes the command, fetches data into RAM, and **closes** the connection.
  - All filtering, manipulation, and reading happens offline in memory.

---

### Key Execution Methods of `SqlCommand`

| Method | Return Type | Best Used For | Example Query |
| :--- | :--- | :--- | :--- |
| **`ExecuteScalar()`** | `object` | Single value (row 1, col 1) | `SELECT COUNT(*) FROM Table`, `SELECT MAX(Salary)` |
| **`ExecuteNonQuery()`** | `int` (rows affected) | Data modification (No rows returned) | `INSERT`, `UPDATE`, `DELETE` |
| **`ExecuteReader()`** | `SqlDataReader` | Multiple rows/columns (Connected stream) | `SELECT Id, Name, Email FROM Table` |

---

## 2. Important Rules & Exam Gotchas

1. **`App.config` & Assembly Reference**:
   - Add reference to `System.Configuration` under Project References.
   - Read connection string using:
     ```csharp
     string conStr = ConfigurationManager.ConnectionStrings["TestDB"].ConnectionString;
     ```
2. **Always Dispose Connection**: Wrap `SqlConnection` in a `using` block to ensure it closes even if an exception occurs.
3. **Stored Procedures with `CommandType`**: Always set `cmd.CommandType = CommandType.StoredProcedure;`.
4. **Output Parameters with `SqlDataReader`**: Output parameters are populated at the very end of the TDS network stream. **Read `param.Value` ONLY AFTER the `SqlDataReader` is closed!**

---

## 3. Hands-On Practice Questions

---

### **Question 1: Connected CRUD & Aggregate Operations**
**Topic:** `SqlConnection`, `SqlCommand`, `ExecuteScalar`, `ExecuteNonQuery`, `ExecuteReader`

**Scenario:**
You are working with a table named `Employees` with columns `(Id INT, Name VARCHAR(50), Salary DECIMAL, Department VARCHAR(50))`.

**Tasks:**
1. Retrieve connection string from `App.config` and establish a `SqlConnection` inside a `using` block.
2. **ExecuteScalar**: Fetch and print the total number of employees using `SELECT COUNT(*) FROM Employees`.
3. **ExecuteNonQuery**: Insert a new employee record:
   `INSERT INTO Employees (Id, Name, Salary, Department) VALUES (101, 'Alex', 65000, 'IT')`. Print the count of affected rows.
4. **ExecuteReader**: Read and print all employees' `Name` and `Salary` using `while (reader.Read())`. Access one column by string name and one by column index.
5. Wrap the reader in a `using` block.

---

### **Question 2: Disconnected Data Reading with `DataTable`**
**Topic:** `SqlDataAdapter`, `DataTable`, `DataRow`

**Scenario:**
You need to load customer records into memory without keeping an active connection open.

**Tasks:**
1. Create a `SqlDataAdapter` passing the query `"SELECT * FROM Customers"` and the connection object.
2. Instantiate a `DataTable dt = new DataTable();`.
3. Use `da.Fill(dt)` to populate the table. *(Notice: No manual `con.Open()` is required!)*
4. Iterate over `dt.Rows` using a `foreach (DataRow row in dt.Rows)` loop and print:
   - Primary identifier using index `row[0]`.
   - Customer name using column key `row["CustomerName"]`.
5. Print the total count of loaded rows using `dt.Rows.Count`.

---

### **Question 3: Multi-Table In-Memory Database with `DataSet`**
**Topic:** `DataSet`, `Tables`, `TableName`

**Scenario:**
You need to retrieve data from two related tables (`Departments` and `Employees`) in a single round-trip to the server.

**Tasks:**
1. Formulate a batched SQL query:
   ```sql
   SELECT * FROM Departments; SELECT * FROM Employees;
   ```
2. Initialize `SqlDataAdapter` with the batch query and fill a new `DataSet ds = new DataSet();`.
3. Explicitly rename the generated tables:
   - `ds.Tables[0].TableName = "Departments";`
   - `ds.Tables[1].TableName = "Employees";`
4. Print the rows of both tables independently using their explicit table names (`ds.Tables["Departments"]` and `ds.Tables["Employees"]`).

---

### **Question 4: Calling Stored Procedures with Input & Output Parameters**
**Topic:** `SqlCommand`, `SqlParameter`, `ParameterDirection.Input`, `ParameterDirection.Output`

**Scenario:**
You are executing a stored procedure:
```sql
CREATE PROCEDURE sp_RegisterEmployee
    @Name VARCHAR(50),
    @Department VARCHAR(50),
    @GeneratedId INT OUTPUT
AS
BEGIN
    INSERT INTO Employees (Name, Department) VALUES (@Name, @Department);
    SET @GeneratedId = SCOPE_IDENTITY();
END
```

**Tasks:**
1. Configure `SqlCommand` with procedure name and set `cmd.CommandType = CommandType.StoredProcedure`.
2. Add input parameters using:
   - `cmd.Parameters.AddWithValue("@Name", "Priya");`
   - `cmd.Parameters.AddWithValue("@Department", "Finance");`
3. Define and register the output parameter `@GeneratedId` of type `SqlDbType.Int` with `Direction = ParameterDirection.Output`.
4. Open the connection, execute `cmd.ExecuteNonQuery()`.
5. Print the generated ID from the output parameter's `.Value` property.

---

### **Question 5: Stored Procedure Returning Result Set + Output Parameter**
**Topic:** `ExecuteReader` combined with `ParameterDirection.Output`

**Scenario:**
You have a procedure that returns matching rows AND an output status message:
```sql
CREATE PROCEDURE sp_GetDeptEmployees
    @Dept VARCHAR(50),
    @Message VARCHAR(100) OUTPUT
AS
BEGIN
    SELECT Name, Salary FROM Employees WHERE Department = @Dept;
    SET @Message = 'Retrieved employees successfully';
END
```

**Tasks:**
1. Configure the command for `sp_GetDeptEmployees` with input `@Dept` and output `@Message` (`SqlDbType.VarChar`, size `100`).
2. Execute with `cmd.ExecuteReader()`.
3. Read each row and display `Name` and `Salary`.
4. **Crucial Step:** Close the reader (exit the `using (reader)` block) before printing the value of `@Message`.

---

### **Question 6: In-Memory Sorting & Filtering using `DataView`**
**Topic:** `DataView`, `Sort`, `RowFilter`, `DataRowView`

**Scenario:**
You have loaded the entire `Employees` table into a `DataTable`. You now need to display different filtered and sorted reports without making any additional database calls.

**Tasks:**
1. Fill a `DataTable dt` from `SELECT * FROM Employees`.
2. **View 1 (Sorting):**
   - Create a `DataView dvSorted = new DataView(dt);`.
   - Sort by Salary in descending order: `dvSorted.Sort = "Salary DESC";`.
   - Loop through `dvSorted` and print `Name` and `Salary`.
3. **View 2 (Filtering):**
   - Create a `DataView dvFiltered = dt.DefaultView;`.
   - Apply a filter for high earners in IT: `dvFiltered.RowFilter = "Salary >= 60000 AND Department = 'IT'";`.
   - Loop using `foreach (DataRowView drv in dvFiltered)` and display the records.

---

## 4. Master Reference Code Solution

```csharp
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace TestPractice
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string conStr = ConfigurationManager.ConnectionStrings["TestDB"].ConnectionString;

            // =========================================================
            // 1. CONNECTED ARCHITECTURE: ExecuteScalar, NonQuery, Reader
            // =========================================================
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();

                // A. ExecuteScalar
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Employees", con))
                {
                    int total = Convert.ToInt32(cmd.ExecuteScalar());
                    Console.WriteLine($"Total Employees: {total}");
                }

                // B. ExecuteNonQuery
                string insertSql = "INSERT INTO Employees (Id, Name, Salary, Department) VALUES (@id, @name, @salary, @dept)";
                using (SqlCommand cmd = new SqlCommand(insertSql, con))
                {
                    cmd.Parameters.AddWithValue("@id", 101);
                    cmd.Parameters.AddWithValue("@name", "Alex");
                    cmd.Parameters.AddWithValue("@salary", 65000);
                    cmd.Parameters.AddWithValue("@dept", "IT");

                    int rows = cmd.ExecuteNonQuery();
                    Console.WriteLine($"Rows Inserted: {rows}");
                }

                // C. ExecuteReader
                using (SqlCommand cmd = new SqlCommand("SELECT Id, Name, Salary FROM Employees", con))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader[0]} | Name: {reader["Name"]} | Salary: {reader["Salary"]}");
                    }
                }
            } // Connection closes automatically here

            // =========================================================
            // 2. DISCONNECTED ARCHITECTURE: SqlDataAdapter, DataTable, DataSet
            // =========================================================
            using (SqlConnection con = new SqlConnection(conStr))
            {
                // A. DataTable
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Employees", con);
                DataTable dt = new DataTable();
                da.Fill(dt); // Auto opens & closes connection

                Console.WriteLine("\n--- In-Memory DataTable Rows ---");
                foreach (DataRow row in dt.Rows)
                {
                    Console.WriteLine($"{row["Name"]} works in {row["Department"]}");
                }

                // B. Multi-Table DataSet
                string multiQuery = "SELECT * FROM Departments; SELECT * FROM Employees";
                SqlDataAdapter daMulti = new SqlDataAdapter(multiQuery, con);
                DataSet ds = new DataSet();
                daMulti.Fill(ds);

                ds.Tables[0].TableName = "Departments";
                ds.Tables[1].TableName = "Employees";

                Console.WriteLine($"\nDepartments Count: {ds.Tables["Departments"].Rows.Count}");
                Console.WriteLine($"Employees Count: {ds.Tables["Employees"].Rows.Count}");

                // =========================================================
                // 3. DATAVIEW: Sorting and Filtering
                // =========================================================
                DataView dv = new DataView(dt);

                // Sorting
                dv.Sort = "Salary DESC";
                Console.WriteLine("\n--- DataView Sorted by Salary DESC ---");
                foreach (DataRowView drv in dv)
                {
                    Console.WriteLine($"{drv["Name"]} - {drv["Salary"]}");
                }

                // Filtering
                dv.RowFilter = "Department = 'IT' AND Salary > 50000";
                Console.WriteLine("\n--- DataView Filtered (IT & Salary > 50000) ---");
                foreach (DataRowView drv in dv)
                {
                    Console.WriteLine($"{drv["Name"]} | {drv["Department"]} | {drv["Salary"]}");
                }
            }

            // =========================================================
            // 4. STORED PROCEDURE WITH INPUT & OUTPUT PARAMETER
            // =========================================================
            using (SqlConnection con = new SqlConnection(conStr))
            using (SqlCommand cmd = new SqlCommand("sp_GetDeptEmployees", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Input Parameter
                cmd.Parameters.AddWithValue("@Dept", "IT");

                // Output Parameter
                SqlParameter outMsg = new SqlParameter("@Message", SqlDbType.VarChar, 100)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outMsg);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Emp: {reader["Name"]} - {reader["Salary"]}");
                    }
                } // Reader closed here!

                // Read output parameter AFTER reader is closed
                Console.WriteLine($"\nOutput Parameter Value: {outMsg.Value}");
            }

            Console.WriteLine("\nPractice Completed. Press Enter to exit.");
            Console.ReadLine();
        }
    }
}
```
