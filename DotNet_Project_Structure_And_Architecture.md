# Production-Grade .NET Project Structure & Nomenclature Guide

A complete architectural reference comparing **MERN (Node/Express)** with **.NET Framework (4.8)** and **Modern .NET (.NET 6/7/8+)**, detailing every industry-standard folder, the files inside them, why we need them, and their exact code contents.

---

## 1. Nomenclature Translation: MERN vs. .NET

In .NET, concepts are identical to Node/Express, but the industry uses specific enterprise terminology:

| Concept | MERN (Node / Express) | .NET Framework (Legacy 4.8) | Modern .NET (.NET 6 / 7 / 8+) | Why We Need It |
| :--- | :--- | :--- | :--- | :--- |
| **Workspace / Root** | Monorepo root folder | **Solution (`.sln`)** | **Solution (`.sln`)** | Groups multiple sub-projects (API, Business Logic, Data Access, Tests) into one workspace. |
| **Package Manifest** | `package.json` | **`packages.config` + `.csproj`** | **`.csproj` (SDK-style)** | Tracks NuGet packages (npm equivalents), target .NET version, and project references. |
| **Installed Packages** | `node_modules/` | **`packages/`** | Global NuGet cache (`~/.nuget`) | Stores downloaded third-party libraries (`.dll` files). |
| **Build Output** | `dist/` or `build/` | **`bin/` & `obj/`** | **`bin/` & `obj/`** | Holds compiled `.dll` and `.exe` machine-ready binaries. |
| **Environment Config** | `.env` | **`Web.config` / `App.config`** (XML) | **`appsettings.json`** (JSON) | Stores SQL connection strings, API keys, and JWT secrets outside of C# code. |
| **Server Entry Point** | `server.js` / `index.js` | **`Global.asax.cs` + `App_Start/`** | **`Program.cs`** | Registers middlewares, Dependency Injection (DI), routing, and starts the server. |
| **Routes & Handlers** | `routes/` + `controllers/` | **`Controllers/`** | **`Controllers/`** | Receives HTTP requests, validates input, calls Services, and returns HTTP status codes. |
| **Business Logic** | `services/` | **`BAL/` or `BLL/`** *(Business Access/Logic Layer)* | **`Services/`** *(Application Layer)* | Enforces business rules, calculations, and maps DTOs to Database Entities. |
| **Database Layer** | `models/` (Mongoose queries) | **`DAL/`** *(Data Access Layer)* | **`Repositories/` + `Data/`** | Executes actual SQL queries via ADO.NET, Dapper, or EF Core. |
| **DB Table Schema** | Mongoose Schema | **`Entities/` or `Models/`** | **`Entities/`** *(Domain Layer)* | Pure C# classes (`POCOs`) that mirror SQL Server database tables 1-to-1. |
| **Req/Res Payloads** | Inline JSON / Zod schemas | **`DTOs/` or `ViewModels/`** | **`DTOs/`** *(Data Transfer Objects)* | Defines the exact JSON shape sent/received by the API so internal DB columns aren't exposed. |

---

## 2. Production-Grade Folder & Solution Structure (N-Tier / Clean Architecture)

In professional .NET development, code is split into **4 logical layers** (either as 4 separate `.csproj` class libraries inside one `.sln`, or as 4 main folders inside a project):

1. **Presentation / API Layer (`Company.Project.API` or `.Web`)**
2. **Business Logic Layer (`Company.Project.BAL` or `.Application`)**
3. **Data Access Layer (`Company.Project.DAL` or `.Infrastructure`)**
4. **Domain / Entities Layer (`Company.Project.Domain` or `.Entities`)**

### Complete Directory Tree
```text
Company.EmployeePortal.sln                        <-- Master Solution File (Opens entire workspace in Visual Studio)
│
├── 1. Company.EmployeePortal.API/                <-- [PRESENTATION LAYER] HTTP Entry Point, Routing, Controllers
│   ├── Properties/
│   │   ├── AssemblyInfo.cs                       <-- (.NET Framework) DLL version & assembly metadata
│   │   └── launchSettings.json                   <-- (.NET Core) Local dev URLs (https://localhost:5001) & env vars
│   ├── App_Start/                                <-- (.NET Framework ONLY) Startup configuration classes
│   │   ├── RouteConfig.cs                        <-- URL routing patterns
│   │   └── WebApiConfig.cs                       <-- JSON formatting, CORS, and attribute routing setup
│   ├── Controllers/
│   │   └── EmployeesController.cs                <-- HTTP Endpoints (GET, POST, PUT, DELETE)
│   ├── Middlewares/ (or Filters/)
│   │   └── GlobalExceptionMiddleware.cs          <-- Centralized error handler (like Express error middleware)
│   ├── Global.asax.cs                            <-- (.NET Framework) Application lifecycle entry point
│   ├── Program.cs                                <-- (Modern .NET) Application entry point & DI registration
│   ├── Web.config                                <-- (.NET Framework) XML ConnectionStrings & AppSettings
│   ├── appsettings.json                          <-- (Modern .NET) JSON ConnectionStrings & AppSettings
│   └── Company.EmployeePortal.API.csproj         <-- Project manifest & NuGet dependencies
│
├── 2. Company.EmployeePortal.BAL/                <-- [BUSINESS ACCESS LAYER] Business Rules, DTOs, Validation
│   ├── DTOs/
│   │   ├── CreateEmployeeDto.cs                  <-- Request payload shape from client
│   │   └── EmployeeResponseDto.cs                <-- Response payload shape sent back to client
│   ├── Interfaces/
│   │   └── IEmployeeService.cs                   <-- Contract for Employee business operations
│   ├── Services/
│   │   └── EmployeeService.cs                    <-- Core business logic & DTO <-> Entity mapping
│   ├── Validators/
│   │   └── EmployeeValidator.cs                  <-- Custom business validation rules
│   └── Company.EmployeePortal.BAL.csproj
│
├── 3. Company.EmployeePortal.DAL/                <-- [DATA ACCESS LAYER] ADO.NET, Dapper, EF Core, SQL Queries
│   ├── Context/ (or DbFactory/)
│   │   └── SqlConnectionFactory.cs               <-- Reads connection string & creates SqlConnection instances
│   ├── Interfaces/
│   │   └── IEmployeeRepository.cs                <-- Contract for database CRUD operations
│   ├── Repositories/
│   │   └── EmployeeRepository.cs                 <-- Actual ADO.NET (SqlConnection, SqlCommand, SqlDataReader) code
│   └── Company.EmployeePortal.DAL.csproj
│
├── 4. Company.EmployeePortal.Domain/             <-- [DOMAIN / ENTITIES LAYER] Database Table Models & Constants
│   ├── Entities/
│   │   └── Employee.cs                           <-- 1-to-1 C# representation of the SQL 'Employees' table
│   ├── Enums/
│   │   └── DepartmentType.cs                     <-- Strongly-typed constants (IT, HR, Finance)
│   ├── Exceptions/
│   │   └── EntityNotFoundException.cs            <-- Custom domain exception
│   └── Company.EmployeePortal.Domain.csproj
│
└── 5. Company.EmployeePortal.Tests/              <-- [UNIT & INTEGRATION TESTS]
    ├── Services/
    │   └── EmployeeServiceTests.cs               <-- xUnit / NUnit tests mocking IEmployeeRepository
    └── Company.EmployeePortal.Tests.csproj
```

---

## 3. Root Configuration & Startup Files (Why & Exact Contents)

### A. `.csproj` (Project File) & `packages.config`
* **Why we need it:** Tells the C# compiler which framework version to target (`net48` vs `net8.0`), which other layers this project is allowed to access (`<ProjectReference>`), and which NuGet packages are installed (`<PackageReference>`).
* **File Content (`Company.EmployeePortal.DAL.csproj`):**
```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <!-- TargetFramework: Use 'net48' for .NET Framework 4.8, or 'net8.0' for modern .NET -->
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <!-- NuGet Packages (Equivalent to "dependencies" in package.json) -->
  <ItemGroup>
    <PackageReference Include="Microsoft.Data.SqlClient" Version="5.2.0" />
    <PackageReference Include="System.Configuration.ConfigurationManager" Version="8.0.0" />
  </ItemGroup>

  <!-- Project References: DAL needs access to Domain Entities -->
  <ItemGroup>
    <ProjectReference Include="..\Company.EmployeePortal.Domain\Company.EmployeePortal.Domain.csproj" />
  </ItemGroup>

</Project>
```
*(Note: In older **.NET Framework 4.8**, NuGet packages are listed in a separate XML file called `packages.config` instead of `<PackageReference>` inside `.csproj`).*

---

### B. `Web.config` / `App.config` (.NET Framework) vs. `appsettings.json` (Modern .NET)
* **Why we need it:** Hardcoding SQL Server passwords or connection strings inside `.cs` files is a major security risk and prevents switching between Development, Staging, and Production databases. Config files store them externally (just like `.env` in MERN).

#### 1. `.NET Framework` (`Web.config` for Web Apps / `App.config` for Desktop Apps)
```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <!-- Key-Value environment settings (Read via ConfigurationManager.AppSettings["Key"]) -->
  <appSettings>
    <add key="Environment" value="Development" />
    <add key="MaxPageSize" value="50" />
  </appSettings>

  <!-- Database Connection Strings (Read via ConfigurationManager.ConnectionStrings["DefaultConnection"]) -->
  <connectionStrings>
    <add name="DefaultConnection" 
         connectionString="Server=localhost\SQLEXPRESS;Database=CompanyDB;Trusted_Connection=True;TrustServerCertificate=True;" 
         providerName="System.Data.SqlClient" />
  </connectionStrings>

  <system.web>
    <compilation debug="true" targetFramework="4.8" />
    <httpRuntime targetFramework="4.8" />
  </system.web>
</configuration>
```

#### 2. `Modern .NET` (`appsettings.json`)
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AppSettings": {
    "Environment": "Development",
    "MaxPageSize": 50
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=CompanyDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

---

### C. Application Startup File: `Global.asax.cs` (.NET Framework) vs. `Program.cs` (Modern .NET)
* **Why we need it:** Equivalent to `server.js` or `app.js` in Express. It boots the web server, registers Dependency Injection (so interfaces like `IEmployeeService` automatically resolve to `EmployeeService`), and configures the HTTP middleware pipeline.

#### 1. `.NET Framework 4.8` (`Global.asax.cs` + `App_Start/WebApiConfig.cs`)
```csharp
// File: Global.asax.cs
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace Company.EmployeePortal.API
{
    public class WebApiApplication : HttpApplication
    {
        // Runs ONCE when IIS starts the application
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}

// File: App_Start/WebApiConfig.cs
using System.Web.Http;

namespace Company.EmployeePortal.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Enables [Route("api/...")] attributes on Controllers
            config.MapHttpAttributeRoutes();

            // Default convention-based route: /api/{controller}/{id}
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
```

#### 2. `Modern .NET` (`Program.cs` — Combines `Global.asax` and `WebApiConfig` into one clean file)
```csharp
// File: Program.cs
using Company.EmployeePortal.BAL.Interfaces;
using Company.EmployeePortal.BAL.Services;
using Company.EmployeePortal.DAL.Context;
using Company.EmployeePortal.DAL.Interfaces;
using Company.EmployeePortal.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. Register Controllers
builder.Services.AddControllers();

// 2. Register Dependency Injection (Wiring DAL and BAL interfaces to their implementations)
builder.Services.AddSingleton<SqlConnectionFactory>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

var app = builder.Build();

// 3. Configure Middleware Pipeline (Order matters, just like app.use() in Express!)
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

---

## 4. Layer-by-Layer Folder & File Breakdown (With Exact Code)

### Layer 1: `Domain` (or `Entities` / `Models`)
* **Why we need this folder:** Contains pure C# classes (**POCOs** — Plain Old CLR Objects) that represent SQL Server tables. Having them in a separate bottom layer allows both `DAL` and `BAL` to use them without circular dependencies.

#### `Entities/Employee.cs`
```csharp
using System;
using Company.EmployeePortal.Domain.Enums;

namespace Company.EmployeePortal.Domain.Entities
{
    // Mirrors the SQL Server 'Employees' table columns 1-to-1
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Department { get; set; }
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; }
    }
}
```

#### `Enums/DepartmentType.cs`
* **Why we need `Enums/`:** Eliminates "magic strings" or "magic numbers" scattered across your codebase.
```csharp
namespace Company.EmployeePortal.Domain.Enums
{
    public enum DepartmentType
    {
        Unassigned = 0,
        IT = 1,
        HR = 2,
        Finance = 3
    }
}
```

---

### Layer 2: `DAL` (Data Access Layer / `Infrastructure`)
* **Why we need this folder:** Isolates all database-specific code (ADO.NET `SqlConnection`, `SqlCommand`, Stored Procedures, EF Core) from the rest of the application. If you ever change a SQL query or switch from ADO.NET to Dapper/EF Core, you **only** touch the `DAL` folder.

#### `Context/SqlConnectionFactory.cs`
* **Why we need `Context/` (or `DbFactory/`):** Centralizes reading the connection string from `Web.config` or `appsettings.json` so you don't repeat connection string lookup code in every single Repository.
```csharp
using System.Configuration; // Used in .NET Framework (Web.config / App.config)
using Microsoft.Data.SqlClient;

namespace Company.EmployeePortal.DAL.Context
{
    public class SqlConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory()
        {
            // Reads <add name="DefaultConnection" ... /> from Web.config / App.config
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
```

#### `Interfaces/IEmployeeRepository.cs`
* **Why we need `Interfaces/`:** Defines the contract for what database operations exist. Allows the Business Layer (`BAL`) to be unit-tested using fake/mock repositories without hitting a real SQL Server database.
```csharp
using System.Collections.Generic;
using Company.EmployeePortal.Domain.Entities;

namespace Company.EmployeePortal.DAL.Interfaces
{
    public interface IEmployeeRepository
    {
        Employee? GetById(int id);
        IEnumerable<Employee> GetAllActive();
        int Add(Employee employee);
    }
}
```

#### `Repositories/EmployeeRepository.cs`
* **Why we need `Repositories/`:** Implements `IEmployeeRepository` and contains the actual ADO.NET SQL queries. **Zero HTTP logic or business rules belong here.**
```csharp
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Company.EmployeePortal.DAL.Context;
using Company.EmployeePortal.DAL.Interfaces;
using Company.EmployeePortal.Domain.Entities;

namespace Company.EmployeePortal.DAL.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public EmployeeRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public Employee? GetById(int id)
        {
            using SqlConnection conn = _connectionFactory.CreateConnection();
            const string sql = "SELECT Id, FullName, Department, Salary, HireDate, IsActive FROM Employees WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();

            if (!reader.Read()) return null;

            return new Employee
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                FullName = reader.GetString(reader.GetOrdinal("FullName")),
                Department = reader.IsDBNull(reader.GetOrdinal("Department")) 
                    ? null 
                    : reader.GetString(reader.GetOrdinal("Department")),
                Salary = reader.GetDecimal(reader.GetOrdinal("Salary")),
                HireDate = reader.GetDateTime(reader.GetOrdinal("HireDate")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
            };
        }

        public IEnumerable<Employee> GetAllActive()
        {
            var employees = new List<Employee>();
            using SqlConnection conn = _connectionFactory.CreateConnection();
            const string sql = "SELECT Id, FullName, Department, Salary, HireDate, IsActive FROM Employees WHERE IsActive = 1";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                employees.Add(new Employee
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    FullName = reader["FullName"].ToString()!,
                    Department = reader["Department"] == DBNull.Value ? null : reader["Department"].ToString(),
                    Salary = Convert.ToDecimal(reader["Salary"]),
                    HireDate = Convert.ToDateTime(reader["HireDate"]),
                    IsActive = Convert.ToBoolean(reader["IsActive"])
                });
            }
            return employees;
        }

        public int Add(Employee emp)
        {
            using SqlConnection conn = _connectionFactory.CreateConnection();
            const string sql = @"
                INSERT INTO Employees (FullName, Department, Salary, HireDate, IsActive)
                VALUES (@FullName, @Department, @Salary, @HireDate, @IsActive);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@FullName", SqlDbType.NVarChar, 100).Value = emp.FullName;
            cmd.Parameters.Add("@Department", SqlDbType.NVarChar, 50).Value = (object?)emp.Department ?? DBNull.Value;
            cmd.Parameters.Add("@Salary", SqlDbType.Decimal).Value = emp.Salary;
            cmd.Parameters.Add("@HireDate", SqlDbType.DateTime2).Value = emp.HireDate;
            cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = emp.IsActive;

            conn.Open();
            return (int)cmd.ExecuteScalar()!;
        }
    }
}
```

---

### Layer 3: `BAL` / `BLL` (Business Access/Logic Layer / `Application`)
* **Why we need this folder:** This is the "brain" of your backend. It validates business rules, performs calculations, and translates between external **DTOs** and internal **Database Entities**.

#### `DTOs/CreateEmployeeDto.cs` & `DTOs/EmployeeResponseDto.cs`
* **Why we need `DTOs/`:**
  1. Prevents **Over-posting attacks** (e.g., a hacker sending `"IsActive": true` or `"Id": 99` in the POST request body when those should be controlled by the server).
  2. Hides sensitive database columns (like `PasswordHash` or `InternalNotes`) from API responses.
```csharp
using System.ComponentModel.DataAnnotations;

namespace Company.EmployeePortal.BAL.DTOs
{
    // Input DTO (What the frontend sends in POST /api/employees)
    public class CreateEmployeeDto
    {
        [Required(ErrorMessage = "Full name is required.")]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Department { get; set; }

        [Range(1000, 500000, ErrorMessage = "Salary must be between 1,000 and 500,000.")]
        public decimal Salary { get; set; }
    }

    // Output DTO (What the API returns to the frontend)
    public class EmployeeResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string DepartmentDisplay { get; set; } = string.Empty;
        public decimal AnnualSalary { get; set; }
        public decimal MonthlySalary { get; set; } // Computed field not in DB!
    }
}
```

#### `Interfaces/IEmployeeService.cs` & `Services/EmployeeService.cs`
* **Why we need `Services/`:** Orchestrates business workflows. Controllers call Services; Services call Repositories.
```csharp
using System;
using Company.EmployeePortal.BAL.DTOs;
using Company.EmployeePortal.BAL.Interfaces;
using Company.EmployeePortal.DAL.Interfaces;
using Company.EmployeePortal.Domain.Entities;

namespace Company.EmployeePortal.BAL.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepo;

        public EmployeeService(IEmployeeRepository employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        public EmployeeResponseDto? GetEmployeeProfile(int id)
        {
            Employee? entity = _employeeRepo.GetById(id);
            if (entity is null) return null;

            // Map Database Entity -> Output DTO
            return new EmployeeResponseDto
            {
                Id = entity.Id,
                FullName = entity.FullName,
                DepartmentDisplay = entity.Department ?? "Unassigned",
                AnnualSalary = entity.Salary,
                MonthlySalary = Math.Round(entity.Salary / 12, 2)
            };
        }

        public int HireEmployee(CreateEmployeeDto dto)
        {
            // Business Rule: IT employees must earn at least 40,000
            if (string.Equals(dto.Department, "IT", StringComparison.OrdinalIgnoreCase) && dto.Salary < 40000m)
            {
                throw new InvalidOperationException("IT department minimum salary is 40,000.");
            }

            // Map Input DTO -> Database Entity
            var newEmployee = new Employee
            {
                FullName = dto.FullName.Trim(),
                Department = dto.Department,
                Salary = dto.Salary,
                HireDate = DateTime.UtcNow,
                IsActive = true
            };

            return _employeeRepo.Add(newEmployee);
        }
    }
}
```

---

### Layer 4: `API` / `Web` / `UI` (Presentation Layer)
* **Why we need this folder:** Handles HTTP communication (or Desktop UI forms). Controllers should be **"thin"**—meaning they only parse the request, call `IEmployeeService`, and return the appropriate HTTP status code (`200 OK`, `201 Created`, `404 Not Found`, `400 Bad Request`).

#### `Controllers/EmployeesController.cs`
```csharp
using Microsoft.AspNetCore.Mvc; // (In .NET Framework 4.8 Web API, this is: using System.Web.Http;)
using Company.EmployeePortal.BAL.DTOs;
using Company.EmployeePortal.BAL.Interfaces;

namespace Company.EmployeePortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Route: /api/employees
    public class EmployeesController : ControllerBase // (In .NET Framework 4.8, inherits from: ApiController)
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // GET: api/employees/5
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            EmployeeResponseDto? employee = _employeeService.GetEmployeeProfile(id);
            if (employee is null)
                return NotFound(new { Message = $"Employee with ID {id} not found." }); // HTTP 404

            return Ok(employee); // HTTP 200 with JSON body
        }

        // POST: api/employees
        [HttpPost]
        public IActionResult Create([FromBody] CreateEmployeeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // HTTP 400 if DataAnnotations ([Required], [Range]) fail

            int createdId = _employeeService.HireEmployee(dto);

            // Returns HTTP 201 Created with 'Location: /api/employees/{createdId}' header
            return CreatedAtAction(nameof(GetById), new { id = createdId }, new { Id = createdId });
        }
    }
}
```

---

## 5. End-to-End Request Lifecycle Summary

1. **Client / React / Postman** sends `POST /api/employees` with JSON body.
2. **`Controllers/EmployeesController.cs`** (`API` Layer) receives the request into `CreateEmployeeDto` and validates `[Required]` / `[Range]` attributes.
3. **`Services/EmployeeService.cs`** (`BAL` Layer) checks business rules (e.g., IT minimum salary), sets server fields (`HireDate = DateTime.UtcNow`, `IsActive = true`), and maps `CreateEmployeeDto` $\rightarrow$ `Employee` entity.
4. **`Repositories/EmployeeRepository.cs`** (`DAL` Layer) opens a `SqlConnection` via `SqlConnectionFactory`, runs parameterized `INSERT INTO Employees...` using `SqlCommand`, and returns the new `Id` to the Service $\rightarrow$ Controller $\rightarrow$ Client.
