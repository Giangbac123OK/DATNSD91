using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Dto_Admin
{
	public class Updatethuoctinhspct
	{
		[Required(ErrorMessage = "Idtt là bắt buộc.")]
		public int Idtt { get; set; }

		[Required(ErrorMessage = "Tên thuộc tính chi tiết là bắt buộc.")]
		[StringLength(100, ErrorMessage = "Tên thuộc tính chi tiết không được vượt quá 100 ký tự.")]
		public string Tenthuoctinhchitiet { get; set; }
	}
}
