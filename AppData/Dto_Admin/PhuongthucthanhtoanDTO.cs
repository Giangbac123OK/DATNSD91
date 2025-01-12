using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Dto_Admin
{
	public class PhuongthucthanhtoanDTO
	{
		[Required(ErrorMessage = "Tên phương thức thanh toán không được để trống")]
		[MaxLength(50)]
		public string Tenpttt { get; set; }
		[Range(0, 1, ErrorMessage = "Trạng thái phải là sử dụng hoặc không sử dụng")]// 0: Đang sử dụng, 1: Không sử dụng
		public int Trangthai { get; set; }
	}
}
