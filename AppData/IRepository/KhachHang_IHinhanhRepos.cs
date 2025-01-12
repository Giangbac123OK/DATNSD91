using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IRepository
{
    public interface KhachHang_IHinhanhRepos
    {
        Task<IEnumerable<Hinhanh>> GetAllAsync();
        Task<Hinhanh> GetByIdAsync(int id);
        Task AddAsync(Hinhanh entity);
        Task DeleteAsync(int id);
        Task UpdateAsync(Hinhanh entity);
    }
}
