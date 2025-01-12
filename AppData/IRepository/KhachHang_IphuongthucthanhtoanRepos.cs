using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Models;

namespace AppData.IRepository
{
    public interface KhachHang_IphuongthucthanhtoanRepos
    {
        Task<IEnumerable<Phuongthucthanhtoan>> GetAllAsync();
        Task<Phuongthucthanhtoan> GetByIdAsync(int id);
        Task AddAsync(Phuongthucthanhtoan entity);
        Task UpdateAsync(Phuongthucthanhtoan entity);
        Task DeleteAsync(int id);

    }
}
