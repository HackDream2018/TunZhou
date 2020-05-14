using System;
using System.Collections.Generic;
using System.Text;

namespace TunZhou.Core.Http
{
    public enum ResponseCode
    {
        Success = 0,
        Failed = 1,
        Exception = 2,
        InvalidRequest = 3
    }

    public static class ResponseCodeExtension
    {
        public static int GetCode(this ResponseCode code) => (int)code;

        public static string GetDescribe(this ResponseCode code) => code.ToString();
    }
}
