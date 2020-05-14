using System;
using System.Collections.Generic;
using System.Text;

namespace TunZhou.DtoModel
{
    public class BaseResponse
    {
        public int Code { get; set; }

        public string Msg { get; set; }

        public string Message { get; set; }
    }
}
