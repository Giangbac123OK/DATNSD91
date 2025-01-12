using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Models;

namespace AppData.IRepository
{
	public interface IthuoctinhsanphamchitietRepository
	{
		Task<IEnumerable<Thuoctinhsanphamchitiet>> GetAllAsync();
		Task<Thuoctinhsanphamchitiet> GetByIdAsync(int idtt, int idspct);
		Task AddAsync(Thuoctinhsanphamchitiet thuoctinhsanphamchitiet);
		Task UpdateAsync(Thuoctinhsanphamchitiet thuoctinhsanphamchitiet);
		Task DeleteAsync(int idtt, int idspct);
		Task<IEnumerable<Thuoctinhsanphamchitiet>> GetByIdttAsync(int idtt);
	}
}
