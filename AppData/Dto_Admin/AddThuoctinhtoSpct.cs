using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Models;

namespace AppData.Dto_Admin
{
	public class AddThuoctinhtoSpct
	{
		[StringLength(200, ErrorMessage = "Mô tả không được quá 200 ký tự")]
		public string? Mota { get; set; }
		[Range(0, 3, ErrorMessage = "Trạng thái không hợp lệ")]
		public int Trangthai
		{
			get; set;
		}
		[Range(0, int.MaxValue, ErrorMessage = "Giá phải lớn hơn hoặc bằng 0")]
		public decimal Giathoidiemhientai
		{
			get; set;
		}
		[Required(ErrorMessage = "Số lượng không được để trống.")]
		[Range(0, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0")]
		public int Soluong { get; set; }
		public int Idsp { get; set; }  // ID sản phẩm chính (mối quan hệ 1-n với Sanpham)

		// Dữ liệu thuộc tính chi tiết
		public List<thuoctinh_ttctDto> thuoctinhchitiets { get; set; }
	
	}
}
