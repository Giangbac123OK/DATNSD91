using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto;
using AppData.Models;
using AppData.ViewModel;
namespace AppData.IService
{
    public interface KhachHang_IHoaDonChiTietService
    {
        Task<IEnumerable<Hoadonchitiet>> GetAllAsync();
        Task<Hoadonchitiet> GetByIdAsync(int id);
        Task AddAsync(HoaDonchitietDTO dto);
        Task ReturnProductAsync(int hoadonId);
        Task UpdateAsync(HoaDonchitietDTO dto, int id);
        Task DeleteAsync(int id);
        Task<List<HoadonchitietViewModel>> HoadonchitietTheoMaHD(int id); 
        Task<List<HoadonchitietViewModel>> Checksoluong(int id);
    }
}
