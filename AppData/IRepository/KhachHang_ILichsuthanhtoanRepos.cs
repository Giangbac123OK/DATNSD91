using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IRepository
{
    public interface KhachHang_ILichsuthanhtoanRepos
    {
        Task<IEnumerable<Lichsuthanhtoan>> GetAllAsync();
        Task<Lichsuthanhtoan> GetByIdAsync(int id);
        Task<List<Lichsuthanhtoan>> GetByIdHDAsync(int id);
        Task AddAsync(Lichsuthanhtoan entity);
        Task UpdateAsync(Lichsuthanhtoan entity);
        Task DeleteAsync(int id);
    }
}
