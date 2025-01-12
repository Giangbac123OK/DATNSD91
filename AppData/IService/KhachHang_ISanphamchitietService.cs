using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto;
using AppData.Models;

namespace AppData.IService
{
    public interface KhachHang_ISanphamchitietService
    {
        Task<IEnumerable<Sanphamchitiet>> GetAllAsync();
        Task<Sanphamchitiet> GetByIdAsync(int id);
        Task<List<SanphamchitietsDTO>> GetByIdSPAsync(int idspct);
        Task<List<ThuoctinhsanphamchitietDTO>> GetByIdTTSPCTAsync(int idspct);
        Task<List<ThuoctinhsanphamchitietDTO>> GetByISPCTAsync(List<string> tenthuoctinh);
        Task AddAsync(SanphamchitietsDTO dto);
        Task UpdateAsync(int id, SanphamchitietsDTO dto);
        Task AddThuoctinhsanphamchitiet(int idsp, int idspct, int idtt, List<string> tenthuoctinhchitietList);
        Task DeleteAsync(int id);
    }
}
