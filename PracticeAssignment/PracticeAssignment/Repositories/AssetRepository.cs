using System.Data;
using Microsoft.Data.SqlClient;
using PracticeAssignment.Models;

namespace PracticeAssignment.Repositories;

public class AssetRepository : IAssetRepository
{
    private readonly string _connectionString;

    public AssetRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection string not found in configuration.");
    }

    public async Task<IEnumerable<Asset>> GetAllAssetsAsync()
    {
        var assets = new List<Asset>();

        await using var conn = new SqlConnection(_connectionString);
        await using var cmd = new SqlCommand("labTest.sp_GetAllAssets", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        await conn.OpenAsync();

        // Using SqlDataAdapter configured for CommandType.StoredProcedure as required by Part B
        using var adapter = new SqlDataAdapter(cmd);
        var dataTable = new DataTable();
        adapter.Fill(dataTable);

        foreach (DataRow row in dataTable.Rows)
        {
            assets.Add(new Asset
            {
                AssetId = Convert.ToInt32(row["AssetId"]),
                AssetName = Convert.ToString(row["AssetName"]) ?? string.Empty,
                Category = Convert.ToString(row["Category"]) ?? string.Empty,
                PurchaseDate = Convert.ToDateTime(row["PurchaseDate"]),
                Status = Convert.ToString(row["Status"]) ?? string.Empty,
                Price = Convert.ToDecimal(row["Price"]),
                CreatedAt = row["CreatedAt"] != DBNull.Value ? Convert.ToDateTime(row["CreatedAt"]) : DateTime.MinValue,
                UpdatedAt = row["UpdatedAt"] != DBNull.Value ? Convert.ToDateTime(row["UpdatedAt"]) : null
            });
        }

        return assets;
    }

    public async Task<Asset?> GetAssetByIdAsync(int id)
    {
        await using var conn = new SqlConnection(_connectionString);
        await using var cmd = new SqlCommand("labTest.sp_GetAssetById", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        // Explicit parameter binding to prevent SQL injection
        cmd.Parameters.Add(new SqlParameter("@AssetId", SqlDbType.Int)
        {
            Value = id,
            Direction = ParameterDirection.Input
        });

        await conn.OpenAsync();
        await using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Asset
            {
                AssetId = reader.GetInt32(reader.GetOrdinal("AssetId")),
                AssetName = reader.GetString(reader.GetOrdinal("AssetName")),
                Category = reader.GetString(reader.GetOrdinal("Category")),
                PurchaseDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate")),
                Status = reader.GetString(reader.GetOrdinal("Status")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                UpdatedAt = !reader.IsDBNull(reader.GetOrdinal("UpdatedAt"))
                    ? reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
                    : null
            };
        }

        return null;
    }

    public async Task<int> InsertAssetAsync(Asset asset)
    {
        await using var conn = new SqlConnection(_connectionString);
        await using var cmd = new SqlCommand("labTest.sp_InsertAsset", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        // Explicit parameter bindings
        cmd.Parameters.Add(new SqlParameter("@AssetName", SqlDbType.NVarChar, 100) { Value = asset.AssetName });
        cmd.Parameters.Add(new SqlParameter("@Category", SqlDbType.NVarChar, 50) { Value = asset.Category });
        cmd.Parameters.Add(new SqlParameter("@PurchaseDate", SqlDbType.Date) { Value = asset.PurchaseDate });
        cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.NVarChar, 30) { Value = asset.Status });
        cmd.Parameters.Add(new SqlParameter("@Price", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = asset.Price });

        await conn.OpenAsync();
        var result = await cmd.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public async Task<bool> UpdateAssetAsync(int id, Asset asset)
    {
        await using var conn = new SqlConnection(_connectionString);
        await using var cmd = new SqlCommand("labTest.sp_UpdateAsset", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        // Explicit parameter bindings
        cmd.Parameters.Add(new SqlParameter("@AssetId", SqlDbType.Int) { Value = id });
        cmd.Parameters.Add(new SqlParameter("@AssetName", SqlDbType.NVarChar, 100) { Value = asset.AssetName });
        cmd.Parameters.Add(new SqlParameter("@Category", SqlDbType.NVarChar, 50) { Value = asset.Category });
        cmd.Parameters.Add(new SqlParameter("@PurchaseDate", SqlDbType.Date) { Value = asset.PurchaseDate });
        cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.NVarChar, 30) { Value = asset.Status });
        cmd.Parameters.Add(new SqlParameter("@Price", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = asset.Price });

        await conn.OpenAsync();
        var result = await cmd.ExecuteScalarAsync();
        int rowsAffected = Convert.ToInt32(result);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAssetAsync(int id)
    {
        await using var conn = new SqlConnection(_connectionString);
        await using var cmd = new SqlCommand("labTest.sp_DeleteAsset", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        // Explicit parameter binding
        cmd.Parameters.Add(new SqlParameter("@AssetId", SqlDbType.Int) { Value = id });

        await conn.OpenAsync();
        var result = await cmd.ExecuteScalarAsync();
        int rowsAffected = Convert.ToInt32(result);
        return rowsAffected > 0;
    }
}
