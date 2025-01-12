using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Dto
{
	public class NhacungcapDto
	{
		[Required(ErrorMessage = "Tên nhà cung cấp không được để trống")]
		[StringLength(50, ErrorMessage = "Tên nhà cung cấp không được quá 50 ký tự")]
		public string Tennhacungcap { get; set; }

		[Required(ErrorMessage = "Số điện thoại không được để trống")]
		[Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
		public string Sdt { get; set; }

		[Required(ErrorMessage = "Địa chỉ không được để trống")]
		[StringLength(200, ErrorMessage = "Địa chỉ không được quá 200 ký tự")]
		public string Diachi { get; set; }

		[EmailAddress(ErrorMessage = "Email không hợp lệ")]
		public string? Email { get; set; }

		[Required(ErrorMessage = "Trạng thái không được để trống")]
		[Range(0, 1, ErrorMessage = "Trạng thái phải là Hoạt động hoặc dừng hoạt động")]
		public int Trangthai { get; set; }
	}
}
