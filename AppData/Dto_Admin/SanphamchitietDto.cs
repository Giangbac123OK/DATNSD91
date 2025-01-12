using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Dto_Admin
{
	public class SanphamchitietDto
	{
		public string? Mota { get; set; }
		public int Trangthai { get; set; }
		public decimal Giathoidiemhientai { get; set; }
		public int Soluong { get; set; }
		public int Idsp { get; set; }
	}
}
