using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto_Admin;

namespace AppData.IService
{
	public interface ISalechitietService
	{
		Task<SalechitietDTO> GetByIdAsync(int id);
		Task<List<SalechitietDTO>> GetAllAsync();
		Task CreateAsync(SalechitietDTO salechitietDTO);
		Task UpdateAsync(SalechitietDTO salechitietDTO);
		Task DeleteAsync(int id);

	}
}
