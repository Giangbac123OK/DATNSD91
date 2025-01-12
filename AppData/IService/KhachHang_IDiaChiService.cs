using AppData.Dto;
using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IService
{
    public interface KhachHang_IDiaChiService
    {
        Task<IEnumerable<DiaChiDTO>> GetAllDiaChi();
        Task<Diachi> GetByIdAsync(int id);
        Task<List<DiaChiDTO>> GetDiaChiByIdKH(int idsp);
        Task Create(DiaChiDTO diachi);
        Task Delete(int id);
        Task Update(int id, DiaChiDTO diaChiDTO);
    }
}
