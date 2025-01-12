using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Models;

namespace AppData.IRepository
{
	public interface KhachHang_IsaleRepos
	{
		Task<IEnumerable<Sale>> GetAllAsync();
		Task<Sale> GetByIdAsync(int id);
		Task AddAsync(Sale sale);
		Task UpdateAsync(Sale sale);
		Task DeleteAsync(int id);
	}
}
