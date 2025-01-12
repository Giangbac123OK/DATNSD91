using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Dto_Admin
{
	public class ThuonghieuDTO
	{
		[Required(ErrorMessage = "Tên thương hiệu không được để trống")]
		[MaxLength(100, ErrorMessage = "Tên thương hiệu không được vượt quá 100 ký tự")]
		public string Tenthuonghieu { get; set; }

		[Range(0, 2, ErrorMessage = "Tình trạng không hợp lệ")]
		public int Tinhtrang { get; set; } 
	}
}
