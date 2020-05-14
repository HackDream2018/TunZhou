using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TunZhou.DtoModel;
using TunZhou.DtoModel.Response;
using TunZhou.Services.Interfaces;

namespace TunZhou.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : BaseController
    {
        #region 接口注入
        private readonly ITestService _testService;

        public TestController(ITestService testService)
        {
            _testService = testService;
        }
        #endregion
        [HttpGet]
        [Route("GetData")]
        public async Task<TestResponse> GetData()
        {
            return await _testService.GetUserForRedisAndMemoryCache();
        }
    }
}