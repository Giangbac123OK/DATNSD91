using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto_Admin;

namespace AppData.IService
{
    public interface ITrahangService
    {
        Task<TrahangDto> GetTrahangByHoadonIdAsync(int hoadonId);
    }
}
