using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Models;

namespace AppData.IRepository
{
    public interface KhachHang_InhacungcapRepos
    {
        Task<Nhacungcap> GetByIdAsync(int id);
        Task<IEnumerable<Nhacungcap>> GetAllAsync();
        Task AddAsync(Nhacungcap nhaCungCap);
        Task UpdateAsync(Nhacungcap nhaCungCap);
        Task DeleteAsync(int id);
    }
}
