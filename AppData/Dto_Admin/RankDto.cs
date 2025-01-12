using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Dto_Admin
{
	public class RankDto
	{
		[Required(ErrorMessage = "Tên rank là bắt buộc.")]
		[StringLength(100, ErrorMessage = "Tên rank không được quá 100 ký tự.")]
		public string Tenrank { get; set; }

		// Kiểm tra MinMoney phải lớn hơn hoặc bằng 0
		[Range(0, double.MaxValue, ErrorMessage = "MinMoney phải lớn hơn hoặc bằng 0.")]
		public decimal MinMoney { get; set; }

		// Kiểm tra MaxMoney phải lớn hơn MinMoney
		[Range(0, double.MaxValue, ErrorMessage = "MaxMoney phải lớn hơn hoặc bằng 0.")]
		public decimal MaxMoney { get; set; }

		// Kiểm tra Trangthai phải là 0 hoặc 1
		[Range(0, 1, ErrorMessage = "Trạng thái phải là 0 hoặc 1.")]
		public int Trangthai { get; set; }
	}
}
