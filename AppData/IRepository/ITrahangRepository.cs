using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto_Admin;

namespace AppData.IRepository
{
	public interface ITrahangRepository
	{
		Task<TrahangDto> GetTrahangByHoadonIdAsync(int hoadonId);
	}
}
