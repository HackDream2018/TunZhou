using System;
using System.Collections.Generic;
using System.Text;
using TunZhou.DtoModel.DTO;

namespace TunZhou.DtoModel.Response
{
    public class TestResponse : BaseResponse
    {
        public List<UserDTO> Data { get; set; }
    }
}
