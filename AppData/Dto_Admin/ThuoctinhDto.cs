using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Dto_Admin
{
	public class ThuoctinhDto
	{
		[Required(ErrorMessage = "Tên thuộc tính không được để trống")]
		[MaxLength(100, ErrorMessage = "Tên thuộc tính không được vượt quá 100 ký tự")]
		public string Tenthuoctinh { get; set; }
	}
}
