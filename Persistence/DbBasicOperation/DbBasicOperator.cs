using Persistence.DbConnection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Persistence.DbBasicOperation
{
    public class DbBasicOperator : IDbBasicOperator
    {
        protected virtual IDbConnectionFactory ConnectionFactory => new SqlServerConnectionFactory();

        protected virtual string ConnectionString =>
            throw new NotImplementedException($"未实现的属性：{nameof(ConnectionString)}！");

        public async Task<T> FirstOrDefaultAsync<T>(string sql, object parames = null, CommandType? commandType = null)
        {
            using (var conn = ConnectionFactory.CreateDbConnection(ConnectionString))
            {
                return await conn.QueryFirstOrDefaultAsync<T>(sql: sql, param: parames, commandType: commandType);
            }
        }

        public IEnumerable<T> Query<T>(string sql, object parames = null, CommandType? commandType = null,
            bool buffered = true)
        {
            using (var conn = ConnectionFactory.CreateDbConnection(ConnectionString))
            {
                return conn.Query<T>(sql, parames, commandType: commandType, buffered: buffered);
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parames = null,
            CommandType? commandType = null)
        {
            using (var conn = ConnectionFactory.CreateDbConnection(ConnectionString))
            {
                return await conn.QueryAsync<T>(sql, parames, commandType: commandType);
            }
        }

        public int ExecuteNonQuery(string sql, object parames = null, CommandType? commandType = null)
        {
            using (var conn = ConnectionFactory.CreateDbConnection(ConnectionString))
            {
                return conn.Execute(sql, parames, commandType: commandType);
            }
        }

        public async Task<int> ExecuteNonQueryAsync(string sql, object parames = null, CommandType? commandType = null)
        {
            using (var conn = ConnectionFactory.CreateDbConnection(ConnectionString))
            {
                return await conn.ExecuteAsync(sql, parames, commandType: commandType);
            }
        }

        public GridReader QueryMultiple(string sql, object parames = null, CommandType? commandType = null)
        {
            using (var conn = ConnectionFactory.CreateDbConnection(ConnectionString))
            {
                using (var multi = conn.QueryMultiple(sql: sql, param: parames, commandType: commandType))
                {
                    return multi;
                }
            }
        }

        public async Task<GridReader> QueryMultipleAsync(string sql, object parames = null, CommandType? commandType = null)
        {
            using (var conn = ConnectionFactory.CreateDbConnection(ConnectionString))
            {
                using (var multi = await conn.QueryMultipleAsync(sql: sql, param: parames, commandType: commandType))
                {
                    return multi;
                }
            }
        }

        public int ExecuteNonQueryWithLocalTransaction(string sql, object parames = null,
            IsolationLevel isolation = IsolationLevel.ReadCommitted, CommandType? commandType = null,
            bool isThrowException = false)
        {
            return ExecuteNonQueryWithLocalTransactionAsync(sql, parames, isolation, commandType).Result;
        }

        public async Task<int> ExecuteNonQueryWithLocalTransactionAsync(string sql, object parames = null,
            IsolationLevel isolation = IsolationLevel.ReadCommitted, CommandType? commandType = null,
            bool isThrowException = false)
        {
            using (var conn = ConnectionFactory.CreateDbConnection(ConnectionString))
            using (var trans = conn.BeginTransaction(isolation))
            {
                var result = 0;
                try
                {
                    result = await conn.ExecuteAsync(sql, parames, transaction: trans, commandType: commandType);
                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    if (isThrowException)
                    {
                        throw;
                    }
                }

                return result;
            }
        }


    }
}
