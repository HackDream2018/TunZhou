using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TunZhou.API.Filters
{
    #region IAuthorizationFilter
    /// <summary>
    /// IAuthorizationFilter 是五種 Filter 中優先序最高的，通常用於驗證 Request 合不合法，不合法後面就直接跳過。
    /// </summary>
    public class SignVerificationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {

        }
    }
    /// <summary>
    /// 异步 IAuthorizationFilter
    /// </summary>
    public class SignVerificationFilterAsync : IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {

        }
    }
    #endregion

    #region IResourceFilter
    /// <summary>
    /// Resource 是第二優先，會在 Authorization 之後，Model Binding 之前執行。通常會是需要對 Model 加工處裡才用。
    /// </summary>
    public class SignVerificationResourceFilter : IResourceFilter
    {
        /// <summary>
        /// 操作执行前做的事情
        /// </summary>
        /// <param name="context"></param>
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
        }
        /// <summary>
        ///  //操作执行后做的事情
        /// </summary>
        /// <param name="context"></param>
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
    }
    /// <summary>
    /// 异步 IResourceFilter
    /// </summary>
    public class SignVerificationResourceFilterAsync : IAsyncResourceFilter
    {
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            //操作执行前做的事情
            await next();
            //操作执行后做的事情
        }
    }
    #endregion

    #region IActionFilter
    /// <summary>
    /// 最容易使用的 Filter，封包進出都會經過它，使用上沒捨麼需要特別注意的。跟 Resource Filter 很類似，但並不會經過 Model Binding。
    /// </summary>
    public class SignVerificationActionFilter : IActionFilter
    {
        /// <summary>
        /// 操作执行前做的事情
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
        /// <summary>
        /// 操作执行后做的事情
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
    /// <summary>
    /// 异步 IActionFilter
    /// </summary>
    public class SignVerificationActionFilterAsync : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //操作执行前做的事情
            await next();
            //操作执行后做的事情
        }
    }
    #endregion
    
    #region IExceptionFilter
    /// <summary>
    /// 错误处理过滤器
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
        }
    }
    /// <summary>
    /// 异步 IExceptionFilter
    /// </summary>
    public class ExceptionFilterAsync : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            context.HttpContext.Response.WriteAsync($"{GetType().Name} in. \r\n");
            return Task.CompletedTask;
        }
    }
    #endregion
}
