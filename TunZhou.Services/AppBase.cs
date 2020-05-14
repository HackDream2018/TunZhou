using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TunZhou.Core.Http;

namespace TunZhou.Services
{
    public class AppBase
    {
        private readonly ILogger _logger;
        public AppBase(ILogger<AppBase> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 处理具体业务
        /// </summary>
        /// <param name="errorMessage">发生异常时返回的错误信息</param>
        /// <param name="args">发生异常时需要记录的参数</param>
        protected async Task<ApiResponse<T>> ServiceHandlerAsync<T>(Func<Task<T>> func, string errorMessage, params object[] args)
        {
            try
            {
                var result = await func();
                return new ApiResponse<T>().Success(result);
            }
            catch (Exception ex)
            {
                var msg = string.IsNullOrWhiteSpace(errorMessage) ? ex.Message : errorMessage;
                _logger.LogError(ex, msg, args);
                return new ApiResponse<T>().Failed(msg);
            }
        }

        /// <summary>
        /// 处理具体业务
        /// </summary>
        /// <param name="errorMessage">发生异常时返回的错误信息</param>
        /// <param name="args">发生异常时需要记录的参数</param>
        protected async Task<ApiResponse> ServiceHandlerAsync(Func<Task> func, string errorMessage, params object[] args)
        {
            try
            {
                await func();
                return new ApiResponse().Success();
            }
            catch (Exception ex)
            {
                var msg = string.IsNullOrWhiteSpace(errorMessage) ? ex.Message : errorMessage;
                _logger.LogError(ex, msg, args);
                return new ApiResponse().Failed(msg);
            }
        }
    }
}
