using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto;
using AppData.Models;

namespace AppData.IService
{
	public interface KhachHang_ISalechitietService
	{
        Task<IEnumerable<Salechitiet>> GetAllAsync();
        Task<Salechitiet> GetByIdAsync(int id);
        Task<Salechitiet> GetByIdAsyncSpct(int id);
        Task AddAsync(SalechitietDTO entity);
        Task UpdateAsync(SalechitietDTO entity, int id);
        Task DeleteAsync(int id);
    }
}
