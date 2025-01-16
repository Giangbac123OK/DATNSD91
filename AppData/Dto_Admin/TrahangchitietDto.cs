using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Dto_Admin
{
	public class TrahangchitietDto
	{
		public int Id { get; set; }
		public int Soluong { get; set; }
		public int tinhtrang { get; set; }

		public string Ghichu { get; set; }
		public string Hinhthucxuly { get; set; }
		public HoadonchitietInfoDto HoadonchitietInfo { get; set; }
	}

}
