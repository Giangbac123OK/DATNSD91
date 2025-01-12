using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Dto
{
	public class HoadonnhapDTO
	{
        public int Idnv { get; set; }
        public int Idncc { get; set; }
		public DateTime Ngaynhap { get; set; }
		public int Trangthai { get; set; }
        public string? Ghichu { get; set; }
        public decimal Tongtienhang { get; set; }
		public string Nguoigiao { get; set; }
		public string Sdtnguoigiao { get; set; }
	}
}
