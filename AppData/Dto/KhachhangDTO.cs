using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Dto
{
    public class KhachhangDTO
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [StringLength(50, ErrorMessage = "Họ tên không được vượt quá 50 ký tự")]
        public string Ten { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Số điện thoại phải có 10 chữ số")]
        public string Sdt { get; set; }
        public DateTime Ngaysinh { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tích điểm")]
        public decimal Tichdiem { get; set; }
        public string? Email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string Diachi { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(50, ErrorMessage = "Mật khẩu không được vượt quá 50 ký tự")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập điểm sử dụng")]
        public int Diemsudung { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập trạng thái")]
        [Range(0, 1, ErrorMessage = "Trạng thái chỉ có thể là 0 hoặc 1")]
        public int Trangthai { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập rank")]
        public int Idrank { get; set; }
    }
	public class ForgotPasswordkhDto
	{
		public string Email { get; set; }
	}
}
