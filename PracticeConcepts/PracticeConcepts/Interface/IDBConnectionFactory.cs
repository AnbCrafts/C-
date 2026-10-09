using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace PracticeConcepts.Interface
{
    public interface IDBConnectionFactory
    {
        Task<SqlConnection> CreateConnectionAsync(CancellationToken cancellationToken = default);
        SqlConnection CreateConnection();
    }

}
