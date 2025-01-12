using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Models;

namespace AppData.IRepository
{
    public interface KhachHang_IThuongHieuRepos
    {
        Task<IEnumerable<Thuonghieu>> GetAllAsync();
        Task<Thuonghieu> GetByIdAsync(int id);
        Task<Thuonghieu> AddAsync(Thuonghieu entity);
        Task<Thuonghieu> UpdateAsync(Thuonghieu entity);
        Task<bool> DeleteAsync(int id);
    }
}
