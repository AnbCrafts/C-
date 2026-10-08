using ConsoleApp5.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ConsoleApp5.Helpers
{
    public class SqlHelper : IAsyncDisposable, IDisposable
    {
        private readonly string _connectionString;
        private SqlConnection _connection;

        public SqlHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<SqlConnection> GetConnectionAsync()
        {
            if (_connection == null)
            {
                _connection = new SqlConnection(_connectionString);
                await _connection.OpenAsync();
            }
            else if (_connection.State == ConnectionState.Closed)
            {
                await _connection.OpenAsync();
            }

            return _connection;
        }

        public async Task<int> ExecuteNonQueryAsync(string query, List<SqlParameter> parameters = null)
        {
            SqlConnection connection = await GetConnectionAsync();

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                if (parameters != null && parameters.Count > 0)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }

                return await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<object> ExecuteScalarAsync(string query, List<SqlParameter> parameters = null)
        {
            SqlConnection connection = await GetConnectionAsync();

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                if (parameters != null && parameters.Count > 0)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }

                return await command.ExecuteScalarAsync();
            }
        }

        public async Task<SqlDataReader> ExecuteReaderAsync(string query, List<SqlParameter> parameters = null)
        {
            SqlConnection connection = await GetConnectionAsync();

            SqlCommand command = new SqlCommand(query, connection);

            if (parameters != null && parameters.Count > 0)
            {
                command.Parameters.AddRange(parameters.ToArray());
            }

            return await command.ExecuteReaderAsync(CommandBehavior.Default);
        }

        public async Task<DataTable> ExecuteQueryAsync(string query, List<SqlParameter> parameters = null)
        {
            SqlConnection connection = await GetConnectionAsync();

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                if (parameters != null && parameters.Count > 0)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public async Task DisposeAsync()
        {
            if (_connection != null)
            {
                if (_connection.State != ConnectionState.Closed)
                {
                    _connection.Close();
                }
                _connection.Dispose();
                _connection = null;
            }

            await Task.CompletedTask;
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                if (_connection.State != ConnectionState.Closed)
                {
                    _connection.Close();
                }
                _connection.Dispose();
                _connection = null;
            }
        }
    }
}