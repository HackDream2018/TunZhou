using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TunZhou.DtoModel.DTO;
using TunZhou.DtoModel.Response;

namespace TunZhou.Services.Interfaces
{
    public interface ITestService
    {
        Task<TestResponse> GetUser();
        Task<TestResponse> GetUserForRedis();
        Task<TestResponse> GetUserForRedisAndMemoryCache();
    }
}
