using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto;
using AppData.Models;

namespace AppData.IRepository
{
    public interface KhachHang_INhanvienRepos
    {
        Task<IEnumerable<Nhanvien>> GetAllAsync();
        Task<Nhanvien> GetByIdAsync(int id);
        Task AddAsync(Nhanvien nhanvien);
        Task UpdateAsync(Nhanvien nhanvien);
        Task DeleteAsync(int id);
        Task<IEnumerable<Nhanvien>> TimKiemNhanvienAsync(string search);
    }
}
