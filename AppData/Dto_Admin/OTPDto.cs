using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Dto_Admin
{
	public class SendOtpRequestDto
	{
		public string PhoneNumber { get; set; } // Số điện thoại của người dùng
	}

	public class VerifyOtpRequestDto
	{
		public string PhoneNumber { get; set; }
		public string OtpCode { get; set; }
	}
}
