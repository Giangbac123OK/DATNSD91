using AppData.Dto;
using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IService
{
    public interface KhachHang_IHinhanhService
    {
        Task<IEnumerable<Hinhanh>> GetAllAsync();
        Task<Hinhanh> GetByIdAsync(int id);
        Task AddAsync(HinhanhDTO entity);
        Task DeleteAsync(int id);
        Task UpdateAsync(HinhanhDTO entity,int id);
    }
}
