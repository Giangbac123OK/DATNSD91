using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto_Admin;
using AppData.Models;

namespace AppData.IService
{
	public interface IthuoctinhsanphamchitietService
	{
		Task<IEnumerable<Thuoctinhsanphamchitiet>> GetAllAsync();
		Task<ThuoctinhsanphamchitietDto> GetByIdAsync(int idtt, int idspct);
		Task<IEnumerable<Thuoctinhsanphamchitiet>> GetByIdttAsync(int idtt);
		Task AddAsync(ThuoctinhsanphamchitietDto thuoctinhsanphamchitietDTO);
		Task AddThuoctinhToSanphamchitietAsync(AddThuoctinhtoSpct dto);
		Task UpdateAsync(ThuoctinhsanphamchitietDto thuoctinhsanphamchitietDTO);
		Task DeleteAsync(int idtt, int idspct);
	}
}
