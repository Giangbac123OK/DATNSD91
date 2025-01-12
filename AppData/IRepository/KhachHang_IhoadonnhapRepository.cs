using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Models;

namespace AppData.IRepository
{
    public interface KhachHang_IhoadonnhapRepository
    {
        Task<List<Hoadonnhap>> GetHoadonnhapListAsync();
        Task<Hoadonnhap> GetHoadonnhapById(int id);
        Task Create(Hoadonnhap hoaDonNhap);
        Task Update(Hoadonnhap hoadonnhap);
        Task Delete(int id);
        Task SaveChanges();
    }
}
