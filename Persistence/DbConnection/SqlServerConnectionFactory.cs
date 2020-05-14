using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Persistence.DbConnection
{
    public class SqlServerConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection CreateDbConnection(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException($"连接字符串不得为空：属性值{nameof(connectionString)}不得为空！");
            }

            var conn = new SqlConnection(connectionString);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            return conn;
        }
    }
}
