using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace SmartDuplicateChecker.API.Data
{
    public class SqlDb : IDisposable
    {
        private readonly SqlConnection _connection;

        public SqlDb(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();
        }

        public DataTable ExecuteQuery(string sql)
        {
            using var cmd = new SqlCommand(sql, _connection);
            using var adapter = new SqlDataAdapter(cmd);
            var table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
