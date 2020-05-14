using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TunZhou.Services.Interfaces;

namespace TunZhou.Web.Controllers
{
    public class TestController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IApiResponseHandlerService _apiResponseHandlerService;
        public TestController(IConfiguration configuration, IApiResponseHandlerService apiResponseHandlerService)
            : base(configuration, apiResponseHandlerService)
        {
            _configuration = configuration;
            _apiResponseHandlerService = apiResponseHandlerService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}