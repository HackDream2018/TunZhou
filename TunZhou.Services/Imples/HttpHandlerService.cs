using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TunZhou.Services.Interfaces;

namespace TunZhou.Services.Imples
{
    public class HttpHandlerService : IHttpHandlerService
    {
        public async Task<T> GetAsync<T>(HttpClient client, string url)
        {
            Func<Task<HttpResponseMessage>> func = async () => await client.GetAsync(url);

            return await HttpHandlerCore<T>(func, url);
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(HttpClient client, string url, TRequest request)
        {
            Func<Task<HttpResponseMessage>> func = async () =>
            {
                var content = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
                return await client.PostAsync(url, httpContent);
            };

            return await HttpHandlerCore<TResponse>(func, url);
        }


        #region private
        private async Task<T> HttpHandlerCore<T>(Func<Task<HttpResponseMessage>> func, string url)
        {
            CheckUrl(url);

            try
            {
                var responseMessage = await func();
                return await HttpResponseMessageHandler<T>(responseMessage);
            }
            catch (HttpRequestException ex)
            {
                // Logger.LogError(ex, $"HTTP请求异常：{url}-{ex.Message}");
                throw;
            }
            //catch (HttpResponseException ex)
            //{
            //    //Logger.LogError(ex, $"HTTP返回异常：{url}-{ex.Message}");
            //    throw;
            //}
            catch (TaskCanceledException ex)
            {
                // Logger.LogError(ex, $"HTTP请求超时：{url}-{ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Logger.LogError(ex, $"HTTP未识别异常：{url}-{ex.Message}");
                throw;
            }
        }

        private async Task<T> HttpResponseMessageHandler<T>(HttpResponseMessage message)
        {
            if (message == null)
            {
                throw new HttpRequestException($"HTTP响应为空");
            }
            if (message.IsSuccessStatusCode == false)
            {
                message.EnsureSuccessStatusCode();
            }

            var content = await message.Content.ReadAsStringAsync();
            if (content == null)
            {
                throw new HttpRequestException("HTTP响应体中不包含任何数据");
            }

            return JsonConvert.DeserializeObject<T>(content);
        }

        private void CheckUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new HttpRequestException($"HTTP请求Url地址不得为空");
            }
        }
        #endregion
    }
}
