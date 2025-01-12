using AppData.Dto;
using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IRepository
{
    public interface KhachHang_IGiohangRepos
    {
        Task<IEnumerable<Giohang>> GetAllAsync();
        Task<Giohang> GetByIdAsync(int id);
        Task AddAsync(Giohang gh);
        Task UpdateAsync(Giohang gh);
        Task <Giohang> GetByIdKHAsync(int id);
        Task DeleteAsync(int id);
    }
}
