using ConsoleApp5.Helpers;
using ConsoleApp5.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ConsoleApp5.Repositories
{
    public class VehicleRepo : BaseRepository<Vehicle>
    {
        public VehicleRepo(SqlHelper sqlHelper) : base(sqlHelper)
        {
        }

        public async Task EnsureTableExistsAsync()
        {
            string createTableSql = @"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Vehicles')
                BEGIN
                    CREATE TABLE Vehicles (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        VIN NVARCHAR(50) NOT NULL,
                        Manufacturer NVARCHAR(100) NOT NULL,
                        Model NVARCHAR(100) NOT NULL,
                        OdometerReading DECIMAL(18, 2) NOT NULL,
                        IsActive BIT NOT NULL DEFAULT 1,
                        CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
                    );
                END";

            await _sqlHelper.ExecuteNonQueryAsync(createTableSql);
        }

        public override async Task CreateAsync(Vehicle entity)
        {
            string query = @"
                INSERT INTO Vehicles (VIN, Manufacturer, Model, OdometerReading, IsActive, CreatedDate)
                VALUES (@VIN, @Manufacturer, @Model, @OdometerReading, @IsActive, @CreatedDate);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@VIN", entity.VIN ?? (object)DBNull.Value),
                new SqlParameter("@Manufacturer", entity.Manufacturer ?? (object)DBNull.Value),
                new SqlParameter("@Model", entity.Model ?? (object)DBNull.Value),
                new SqlParameter("@OdometerReading", entity.OdometerReading),
                new SqlParameter("@IsActive", entity.IsActive),
                new SqlParameter("@CreatedDate", entity.CreatedDate)
            };

            object result = await _sqlHelper.ExecuteScalarAsync(query, parameters);
            if (result != null && int.TryParse(result.ToString(), out int newId))
            {
                entity.Id = newId;
            }
        }

        public override async Task UpdateAsync(Vehicle entity)
        {
            string query = @"
                UPDATE Vehicles
                SET VIN = @VIN,
                    Manufacturer = @Manufacturer,
                    Model = @Model,
                    OdometerReading = @OdometerReading,
                    IsActive = @IsActive
                WHERE Id = @Id;";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Id", entity.Id),
                new SqlParameter("@VIN", entity.VIN ?? (object)DBNull.Value),
                new SqlParameter("@Manufacturer", entity.Manufacturer ?? (object)DBNull.Value),
                new SqlParameter("@Model", entity.Model ?? (object)DBNull.Value),
                new SqlParameter("@OdometerReading", entity.OdometerReading),
                new SqlParameter("@IsActive", entity.IsActive)
            };

            await _sqlHelper.ExecuteNonQueryAsync(query, parameters);
        }

        public override async Task DeleteAsync(int id)
        {
            string query = "DELETE FROM Vehicles WHERE Id = @Id;";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Id", id)
            };

            await _sqlHelper.ExecuteNonQueryAsync(query, parameters);
        }

        public override async Task<Vehicle> GetByIdAsync(int id)
        {
            string query = "SELECT Id, VIN, Manufacturer, Model, OdometerReading, IsActive, CreatedDate FROM Vehicles WHERE Id = @Id;";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Id", id)
            };

            using (SqlDataReader reader = await _sqlHelper.ExecuteReaderAsync(query, parameters))
            {
                if (await reader.ReadAsync())
                {
                    return MapVehicle(reader);
                }
            }

            return null;
        }

        public override async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            string query = "SELECT Id, VIN, Manufacturer, Model, OdometerReading, IsActive, CreatedDate FROM Vehicles ORDER BY Id DESC;";

            var vehicles = new List<Vehicle>();

            using (SqlDataReader reader = await _sqlHelper.ExecuteReaderAsync(query))
            {
                while (await reader.ReadAsync())
                {
                    vehicles.Add(MapVehicle(reader));
                }
            }

            return vehicles;
        }

        private Vehicle MapVehicle(SqlDataReader reader)
        {
            return new Vehicle
            {
                Id = Convert.ToInt32(reader["Id"]),
                VIN = Convert.ToString(reader["VIN"]),
                Manufacturer = Convert.ToString(reader["Manufacturer"]),
                Model = Convert.ToString(reader["Model"]),
                OdometerReading = Convert.ToDecimal(reader["OdometerReading"]),
                IsActive = Convert.ToBoolean(reader["IsActive"]),
                CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
            };
        }
    }
}
