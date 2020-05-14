using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TunZhou.Services.Interfaces
{
    public interface IApiResponseHandlerService
    {
        Task<T> GetApiResponseAsync<T>(string url, HttpClient httpClient);


        Task<TResponse> PostApiResponseAsync<TRequest, TResponse>(string url,
            HttpClient httpClient, TRequest request);

        T DoGet<T>(string url, HttpClient httpClient);

        TResponse DoPost<TRequest, TResponse>(string url, TRequest request, HttpClient httpClient);
    }
}
