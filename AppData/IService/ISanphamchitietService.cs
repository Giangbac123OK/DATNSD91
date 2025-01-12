using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto_Admin;
using AppData.Models;

namespace AppData.IService
{
	public interface ISanphamchitietService
	{
		Task<bool> XoaSanphamchitietAsync(int spctId);
		Task<int> UpdateTrangThaiAsync(int id);
		Task UpdateSanphamchitietAndThuoctinhAsync(int idspct, UpdateSanphamchitietDTO dto);
		Task<IEnumerable<Sanphamchitiet>> GetAllAsync();
		Task<Sanphamchitiet> GetByIdAsync(int id);
		Task AddAsync(SanphamchitietDto dto);
		Task UpdateAsync(SanphamchitietDto dto, int id);
		Task DeleteAsync(int id);
	}
}
