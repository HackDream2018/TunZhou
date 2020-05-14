using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Persistence.DbConnection
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateDbConnection(string connectionString);
    }
}
