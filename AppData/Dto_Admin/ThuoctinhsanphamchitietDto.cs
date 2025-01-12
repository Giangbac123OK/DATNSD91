using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Dto_Admin
{
	public class ThuoctinhsanphamchitietDto
	{
			[Required]
			public int Idtt { get; set; }

			[Required]
			public int Idspct { get; set; }

			[Required]
			[StringLength(100, ErrorMessage = "Tên thuộc tính chi tiết không được quá 100 ký tự.")]
			public string Tenthuoctinhchitiet { get; set; }
		
	}
}
