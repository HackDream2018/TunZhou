using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TunZhou.Core.Http;
using TunZhou.DtoModel.DTO;

namespace TunZhou.Services.Interfaces
{
    public interface ITestService
    {
        Task<ApiResponse<IEnumerable<UserDTO>>> GetUser();
        Task<ApiResponse<IEnumerable<UserDTO>>> GetUserForRedis();
        Task<ApiResponse<IEnumerable<UserDTO>>> GetUserForRedisAndMemoryCache();
    }
}
