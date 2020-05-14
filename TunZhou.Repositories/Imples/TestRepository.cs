using Persistence.DbBasicOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunZhou.DtoModel.DTO;
using TunZhou.Repositories.Interfaces;

namespace TunZhou.Repositories.Imples
{
    public class TestRepository : ITestRepository
    {
        private readonly IDbBasicOperator DbBasicOperator;

        public TestRepository(IEnumerable<IDbBasicOperator> dbBasicOperators)
        {
            DbBasicOperator = dbBasicOperators.First(d => d.GetType() == typeof(TunZhouDbBaseRepository));
        }

        public async Task<IEnumerable<UserDTO>> GetUser()
        {
            var sql = $@" SELECT TOP 10.*
                          FROM    PromotionUserWhiteList(NOLOCK) ";
            return await DbBasicOperator.QueryAsync<UserDTO>(sql, new { });
        }
    }
}
