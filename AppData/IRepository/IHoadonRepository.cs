using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Models;

namespace AppData.IRepository
{
    public interface IHoadonRepository
    {
		Task<Hoadon> GetHoaDonById(int id);
		Task AddHoaDon(Hoadon hoadon);
		Task UpdateHoaDon(Hoadon hoadon);
		Task<Hoadonchitiet> AddHoaDonChiTiet(Hoadonchitiet hoadonChiTiet);
		Task AddHoaDonWithDetails(Hoadon hoadon, List<Hoadonchitiet> hoadonChiTiets);
		Task<IEnumerable<Hoadon>> GetAllAsync();
		Task SaveChanges();

	}
}
