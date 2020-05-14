using System;
using System.Collections.Generic;
using System.Text;

namespace TunZhou.Core.Http
{
    public class ApiResponse
    {
        public int Code { set; get; }

        public string Msg { set; get; }

        public ResponseCode Status() => (ResponseCode)Code;

        public ApiResponse Success()
        {
            this.Code = ResponseCode.Success.GetCode();
            this.Msg = ResponseCode.Success.GetDescribe();
            return this;
        }

        public ApiResponse Failed(string errorMessage = null)
        {
            this.Code = ResponseCode.Failed.GetCode();
            this.Msg = errorMessage ?? ResponseCode.Failed.GetDescribe();
            return this;
        }

        public ApiResponse Exception(string exMessage)
        {
            this.Code = ResponseCode.Exception.GetCode();
            this.Msg = exMessage;
            return this;
        }

        public ApiResponse Exception(Exception ex)
        {
            var exMsg = GetExceptionMessageByRecursion(ex);
            return Exception(exMsg);
        }

        public ApiResponse CreateResponse(ResponseCode responseCode, string message = null)
        {
            this.Code = responseCode.GetCode();
            this.Msg = message ?? responseCode.GetDescribe(); ;
            return this;
        }

        /// <summary>
        /// 递归获取异常中的Message
        /// </summary>
        protected string GetExceptionMessageByRecursion(Exception ex)
        {
            if (ex == null)
            {
                throw new ArgumentNullException($"{nameof(ex)}", "参数不得为空");
            }
            var exceptionMessage = "";
            var msgSeparator = " - ";
            if (ex.InnerException != null)
            {
                exceptionMessage += $"{GetExceptionMessageByRecursion(ex.InnerException)}{msgSeparator}";
            }
            exceptionMessage += ex.Message;
            return exceptionMessage;
        }

    }
    public class ApiResponse<T> : ApiResponse
    {
        public T Data { set; get; }

        public ApiResponse<T> Success(T responseData)
        {
            base.Success();
            this.Data = responseData;
            return this;
        }

        public new ApiResponse<T> Failed(string errorMessage)
        {
            base.Failed(errorMessage);
            this.Data = default;
            return this;
        }

        public new ApiResponse<T> Exception(string exMessage = null)
        {
            base.Exception(exMessage);
            this.Data = default;
            return this;
        }

        public new ApiResponse<T> Exception(Exception ex)
        {
            base.Exception(ex);
            this.Data = default;
            return this;
        }

        public ApiResponse<T> CreateResponse(ResponseCode responseCode, T responseData = default, string message = null)
        {
            base.CreateResponse(responseCode, message);
            this.Data = responseData;
            return this;
        }

    }
}
