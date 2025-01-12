using AppData.Dto;
using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IService
{
    public interface KhachHang_ILichsuthanhtoanService
    {
        Task<IEnumerable<Lichsuthanhtoan>> GetAllAsync();
        Task<Lichsuthanhtoan> GetByIdAsync(int id);
        Task<List<lichsuthanhtoanDTO>> GetByIdHDAsync(int id);
        Task UpdateTrangThaiAsync(int orderCode, int status);
        Task AddAsync(lichsuthanhtoanDTO dto);
        Task UpdateAsync(lichsuthanhtoanDTO dto, int id);
        Task DeleteAsync(int id);
    }
}
