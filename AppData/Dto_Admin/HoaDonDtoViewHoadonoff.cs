using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto;

namespace AppData.Dto_Admin
{
	public class HoaDonDtoViewHoadonoff
	{
		public string HoTenNV { get; set; }
		public string TenKH { get; set; }
		public DateTime ThoiGianDatHang { get; set; }
		public decimal TongTienSanPham { get; set; }
		public decimal TongTienCanTra { get; set; }
		public decimal TongGiamGia { get; set; }
		public List<SanphamDto_HDoff> sanphamDto_HDoffs { get; set; }
	}
}
