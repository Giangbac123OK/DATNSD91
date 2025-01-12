using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Dto
{
	public class SanphamDTO
	{
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
		[MaxLength(100, ErrorMessage = "Tên sản phẩm không được vượt quá 100 ký tự")]
		public string Tensp { get; set; }

		public string? Mota { get; set; }

		[Range(0, 2, ErrorMessage = "Trạng thái không hợp lệ")]
		public int Trangthai { get; set; }
		[Required(ErrorMessage = "Số lượng không được để trống")]
		[Range(0, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn bằng 0")]
		public int Soluong { get; set; }
		[Required]
		[Range(0, double.MaxValue, ErrorMessage = "Giá bán không hợp lệ")]
		public decimal Giaban { get; set; }

		public decimal? Giasale { get; set; }

		//[Url(ErrorMessage = "URL hình ảnh không hợp lệ")]
		public string? UrlHinhanh { get; set; }

		public int Idth { get; set; }
	}
}
