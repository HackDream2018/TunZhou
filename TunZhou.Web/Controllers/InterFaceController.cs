using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TunZhou.Core.Enums;
using TunZhou.Core.Http;
using TunZhou.DtoModel;
using TunZhou.DtoModel.DTO;
using TunZhou.Services.Interfaces;

namespace TunZhou.Web.Controllers
{
    public class InterFaceController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IApiResponseHandlerService _apiResponseHandlerService;
        private readonly HttpClient _tunzhouApiHttpClient;
        private readonly ILogger<InterFaceController> _logger;

        public InterFaceController(IConfiguration configuration, IApiResponseHandlerService apiResponseHandlerService,
            IHttpClientFactory httpClientFactory, ILogger<InterFaceController> logger)
            : base(configuration, apiResponseHandlerService)
        {
            _configuration = configuration;
            _apiResponseHandlerService = apiResponseHandlerService;
            _logger = logger;
            //通过HTTP工厂模式创建HTTPClient对象
            _tunzhouApiHttpClient = httpClientFactory.CreateClient(HttpClientConst.TunZhouApi_BaseAddress_CLIENT_NAME);
        }
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            _logger.LogInformation("interface init LogInformation");
            _logger.LogTrace("interface init LogTrace");
            _logger.LogCritical("interface init LogTrace");
            //GetData为config中接口定义的Key，也可以写成const，嫌麻烦就没加
            var apiUrl = _configuration[$"{CommonConst.JSON_CONFIG_API_NAME_PREFIX}GetData"];
            var apiResponse = await GetAsync<ApiResponse<IEnumerable<UserDTO>>>(apiUrl, _tunzhouApiHttpClient);
            return Json(apiResponse);
        }
    }
}