using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TunZhou.Services.Interfaces;

namespace TunZhou.Services.Imples
{
    public class ApiResponseHandlerService : IApiResponseHandlerService
    {

        private readonly ILocalMemoryCacheService _localMemoryCache;
        private readonly IHttpHandlerService _httpHandlerService;

        public ApiResponseHandlerService(IHttpHandlerService httpHandler, ILocalMemoryCacheService localMemoryCache)
        {
            _localMemoryCache = localMemoryCache;
            _httpHandlerService = httpHandler;
        }

        public async Task<T> GetApiResponseAsync<T>(string url, HttpClient httpClient)
        {
            Task<T> func() => _httpHandlerService.GetAsync<T>(httpClient, url);
            return await ApiResponseHandlerCoreAsync(func, url);
        }

        public async Task<TResponse> PostApiResponseAsync<TRequest, TResponse>(string url,
            HttpClient httpClient, TRequest request)
        {
            Task<TResponse> func() => _httpHandlerService.PostAsync<TRequest, TResponse>(httpClient, url, request);
            return await ApiResponseHandlerCoreAsync(func, url);
        }

        public T DoGet<T>(string url, HttpClient httpClient)
        {
            return GetApiResponseAsync<T>(url, httpClient).Result;
        }

        public TResponse DoPost<TRequest, TResponse>(string url, TRequest request, HttpClient httpClient)
        {
            return PostApiResponseAsync<TRequest, TResponse>(url, httpClient, request).Result;
        }

        #region private

        private async Task<T> ApiResponseHandlerCoreAsync<T>(Func<Task<T>> func, string url)
        {
            try
            {
                var apiResponse = await func();
                return apiResponse;
            }
            catch (Exception ex)
            {
                var errorMessage = $"接口异常：{url}-{ex.Message}";
                return default;
            }
        }

        #endregion
    }
}
