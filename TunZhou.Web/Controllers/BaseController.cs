using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using TunZhou.Services.Interfaces;

namespace TunZhou.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IApiResponseHandlerService _apiResponseHandlerService;

        public BaseController(IConfiguration configuration, IApiResponseHandlerService apiResponseHandlerService)
        {
            _configuration = configuration;
            _apiResponseHandlerService = apiResponseHandlerService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {

        }

        public async Task<T> GetAsync<T>(string url, HttpClient httpClient)
        {
            //if (!httpClient.DefaultRequestHeaders.Contains(CookieKey.UserInfo))
            //{
            //    //给Client对象添加头信息
            //    httpClient.DefaultRequestHeaders.Add(CookieKey.UserInfo, DESEncrypt.DesEncrypt(JsonConvert.SerializeObject(userInfo), DESName.UserInfoDESName));
            //}
            return await _apiResponseHandlerService.GetApiResponseAsync<T>(url, httpClient);
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest request, HttpClient httpClient)
        {
            //if (!httpClient.DefaultRequestHeaders.Contains(CookieKey.UserInfo))
            //{
            //    //给Client对象添加头信息
            //    httpClient.DefaultRequestHeaders.Add(CookieKey.UserInfo, DESEncrypt.DesEncrypt(JsonConvert.SerializeObject(userInfo), DESName.UserInfoDESName));
            //}
            return await _apiResponseHandlerService.PostApiResponseAsync<TRequest, TResponse>(url, httpClient, request);
        }

    }
}