using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Models;

namespace AppData.IRepository
{
    public interface KhachHang_IGiamgiaRepos
    {
        Task<IEnumerable<Giamgia>> GetAllAsync();
        Task<Giamgia> GetByIdAsync(int id);
        Task<Giamgia> AddAsync(Giamgia giamgia);
        Task<Giamgia> UpdateAsync(Giamgia giamgia);
        Task AddRankToGiamgia(int giamgiaId, List<string> rankNames);
        Task DeleteAsync(int id);
    }
}
