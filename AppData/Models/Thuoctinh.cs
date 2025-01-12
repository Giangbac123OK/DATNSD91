using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
	public class Thuoctinh
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id {  get; set; }
		[Required(ErrorMessage = "Tên thuộc tính không được để trống")]
		[MaxLength(50, ErrorMessage = "Tên thuộc tính không được vượt quá 50 ký tự")]
		public string Tenthuoctinh { get; set; }
		public virtual ICollection<Thuoctinhsanphamchitiet> Thuoctinhsanphamchitiets { get; set; }
	}
}
