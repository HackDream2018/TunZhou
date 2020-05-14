using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TunZhou.DtoModel.DTO;

namespace TunZhou.Repositories.Interfaces
{
    public interface ITestRepository
    {
        Task<IEnumerable<UserDTO>> GetUser();
    }
}
