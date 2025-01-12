using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
	public class Lichsuthanhtoan
	{
		[Key, Column(Order = 0)]
		public int Idhoadon { get; set; }

		[Key, Column(Order = 1)]
		public int idPttt { get; set; }
		
		public DateTime Thoigianthanhtoan { get; set; }
		public int Trangthai {  get; set; }
		[ForeignKey("Idhoadon")]
		public virtual Hoadon Hoadon { get; set; }
		[ForeignKey("idPttt")]
		public virtual Phuongthucthanhtoan Phuongthucthanhtoan { get; set; }

	}
}
