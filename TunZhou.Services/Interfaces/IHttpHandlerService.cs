using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TunZhou.Services.Interfaces
{
    public interface IHttpHandlerService
    {
        Task<T> GetAsync<T>(HttpClient client, string url);

        Task<TResponse> PostAsync<TRequest, TResponse>(HttpClient client, string url, TRequest request);
    }
}
