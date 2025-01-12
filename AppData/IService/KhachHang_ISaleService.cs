using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto;
using AppData.Models;

namespace AppData.IService
{
	public interface KhachHang_ISaleService
	{
		//Task<IEnumerable<SaleDto>> GetAllAsync();
		Task<IEnumerable<Sale>> GetAllWithIdAsync();
		Task<SaleDto> GetByIdAsync(int id);
		Task AddAsync(SaleDto saleDto);
		Task UpdateStatusToCancelled(int id);
		Task UpdateStatusBasedOnDates(int id);
		Task UpdateStatusLoad(int id);
		Task UpdateAsync(int id, SaleDto saleDto);
		Task DeleteAsync(int id);
	}
}
