using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto;
using AppData.Models;

namespace AppData.IRepository
{
	public interface KhachHang_IsalechitietRepos
	{
        Task<IEnumerable<Salechitiet>> GetAllAsync();
        Task<Salechitiet> GetByIdAsync(int id);
        Task<List<Salechitiet>> GetByIdAsyncSpct(int id);
        Task AddAsync(Salechitiet entity);
        Task UpdateAsync(Salechitiet entity);
        Task DeleteAsync(int id);
	}
}
