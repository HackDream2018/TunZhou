using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TunZhou.Core.Http;
using TunZhou.DtoModel;
using TunZhou.DtoModel.DTO;
using TunZhou.Services.Interfaces;

namespace TunZhou.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : BaseController
    {
        #region 接口注入
        private readonly ITestService _testService;
        private readonly ILogger<TestController> _logger;

        public TestController(ITestService testService, ILogger<TestController> logger)
        {
            _testService = testService;
            _logger = logger;
        }
        #endregion
        [HttpGet]
        [Route("GetData")]
        public async Task<ApiResponse<IEnumerable<UserDTO>>> GetData()
        {
            _logger.LogInformation("getdata test");
            return await _testService.GetUserForRedisAndMemoryCache();
        }
    }
}