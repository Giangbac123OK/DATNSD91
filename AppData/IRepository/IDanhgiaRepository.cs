using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Models;

namespace AppData.IRepository
{
	public interface IDanhgiaRepository
	{
		Task<IEnumerable<Danhgia>> GetDanhgiaByProductIdAsync(int idsp);
		Task<IEnumerable<Danhgia>> GetDanhgiaByProductDetailIdAsync(int idspct);

	}
}
