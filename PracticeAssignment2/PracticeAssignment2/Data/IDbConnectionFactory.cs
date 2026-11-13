using Microsoft.Data.SqlClient;

namespace PracticeAssignment2.Data;

public interface IDbConnectionFactory
{
    SqlConnection CreateConnection();
    Task<SqlConnection> CreateConnectionAsync(CancellationToken cancellationToken = default);
}
