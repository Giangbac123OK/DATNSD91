using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
	public class Hoadonnhapchitiet
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public int Idhdn { get; set; }
		[ForeignKey("Idhdn")]
		public virtual Hoadonnhap Hoadonnhap { get; set; }
		public int Idsp { get; set; }
		[ForeignKey("Idsp")]
		public virtual Sanpham Sanpham {  get; set; }
		public int Soluong {  get; set; }
		public string? Ghichu {  get; set; }
		public decimal Gianhap {  get; set; }
		
		
	}
}
