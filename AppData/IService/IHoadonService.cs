using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto_Admin;
using AppData.Models;

namespace AppData.IService
{
	public interface IHoadonService
	{
		Task<Hoadon> AddHoaDon(CreateHoadonDTO dto);
		Task<Hoadon> AddHoaDonKhachhangthanthietoff(HoadonoffKhachhangthanthietDto dto, int diemSuDung);
		Task<IEnumerable<Hoadon>> GetAllHoadonsAsync();
	}
}
