using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto;

namespace AppData.IService
{
    public interface KhachHang_IThuongHieuService
    {
        Task<IEnumerable<ThuonghieuDTO>> GetAllAsync();
        Task<ThuonghieuDTO> GetByIdAsync(int id);
        Task<ThuonghieuDTO> AddAsync(ThuonghieuDTO dto);
        Task<ThuonghieuDTO> UpdateAsync(int id, ThuonghieuDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
