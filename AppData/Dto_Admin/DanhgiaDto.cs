using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Dto_Admin
{
	public class DanhgiaDto
	{
		public int Id { get; set; }
		public int Idkh { get; set; }
		public string KhachhangTen { get; set; }
		public int Trangthai { get; set; }
		public string Noidungdanhgia { get; set; }
		public DateTime Ngaydanhgia { get; set; }
	
		public string UrlHinhanh { get; set; }
	}
}
