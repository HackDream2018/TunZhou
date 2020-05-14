using Microsoft.Extensions.Configuration;
using Persistence.DbBasicOperation;

namespace TunZhou.Repositories
{
    /// <summary>
    /// TunZhou数据库操作，通过工厂模式创建DBConnection，可以通过多个DbBaseRepository来实现多库
    /// </summary>
    public class TunZhouDbBaseRepository : DbBasicOperator
    {
        private readonly IConfiguration Configuration;

        public TunZhouDbBaseRepository(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override string ConnectionString => Configuration["dbConnections:TunZhouConnectionString"];
    }
}
