using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto;
using AppData.Models;

namespace AppData.IService
{
    public interface KhachHang_IHoadonnhapService
    {
        Task<IEnumerable<HoadonnhapDTO>> GetHoadonnhapListAsync();
        Task<HoadonnhapDTO> GetHoadonnhapById(int id);
        Task Create(HoadonnhapDTO hoaDonNhap);
        Task Update(int id, HoadonnhapDTO hoadonnhap);
        Task Delete(int id);
    }
}
