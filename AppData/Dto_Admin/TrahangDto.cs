using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto;

namespace AppData.Dto_Admin
{
	public class TrahangDto
	{
		public int Id { get; set; }
		//
		public string NhanvienName { get; set; }
		public string KhachhangName { get; set; }
		public decimal? Sotienhoan { get; set; }
		public string Lydotrahang { get; set; }
		public string TrangthaiStr { get; set; }
		public string Phuongthuchoantien { get; set; }
		public DateTime? Ngaytrahangdukien { get; set; }
		public DateTime? Ngaytrahangthucte { get; set; }
		public string Chuthich { get; set; }
		public List<string> Hinhanhs { get; set; }
		public List<TrahangchitietDto> Trahangchitiets { get; set; }
	}
}
