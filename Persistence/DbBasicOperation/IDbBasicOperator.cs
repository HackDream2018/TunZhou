using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Persistence.DbBasicOperation
{
    /// <summary>
    /// 数据库基本操作（查询、非查询两类）
    /// </summary>
    public interface IDbBasicOperator
    {
        Task<T> FirstOrDefaultAsync<T>(string sql, object parames = null, CommandType? commandType = null);

        IEnumerable<T> Query<T>(string sql, object parames = null, CommandType? commandType = null,
            bool buffered = true);

        Task<IEnumerable<T>> QueryAsync<T>(string sql, object parames = null, CommandType? commandType = null);

        int ExecuteNonQuery(string sql, object parames = null, CommandType? commandType = null);

        Task<int> ExecuteNonQueryAsync(string sql, object parames = null, CommandType? commandType = null);

        GridReader QueryMultiple(string sql, object parames = null, CommandType? commandType = null);

        Task<GridReader> QueryMultipleAsync(string sql, object parames = null, CommandType? commandType = null);

        /// <summary>
        /// 使用本地事务执行非查询操作
        /// </summary>
        int ExecuteNonQueryWithLocalTransaction(string sql, object parames = null,
            IsolationLevel isolation = IsolationLevel.ReadCommitted, CommandType? commandType = null,
            bool isThrowException = false);

        Task<int> ExecuteNonQueryWithLocalTransactionAsync(string sql, object parames = null,
            IsolationLevel isolation = IsolationLevel.ReadCommitted, CommandType? commandType = null,
            bool isThrowException = false);
    }
}
