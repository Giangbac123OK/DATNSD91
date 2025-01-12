using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto;
using AppData.Models;
using AppData.ViewModel;
namespace AppData.IRepository
{
    public interface KhachHang_IHoaDonChiTietRepository
    {
        Task<IEnumerable<Hoadonchitiet>> GetAllAsync();
        Task<Hoadonchitiet> GetByIdAsync(int id);
        Task AddAsync(Hoadonchitiet entity);
        Task UpdateAsync(Hoadonchitiet entity);
        Task DeleteAsync(int id);
        Task<List<Hoadonchitiet>> HoadonchitietByIDHD(int id);
        Task<List<HoadonchitietViewModel>> HoadonchitietTheoMaHD(int id);
        Task<List<HoadonchitietViewModel>> Checksoluong(int id);
    }
}
