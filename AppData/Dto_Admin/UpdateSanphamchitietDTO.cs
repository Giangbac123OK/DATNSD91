using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Dto_Admin
{
	public class UpdateSanphamchitietDTO
	{

		[Required(ErrorMessage = "Số lượng là bắt buộc.")]
		[Range(0, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0.")]
		public int Soluong { get; set; }

		[StringLength(200, ErrorMessage = "Mô tả không được vượt quá 200 ký tự.")]
		public string Mota { get; set; }

		public ICollection<Updatethuoctinhspct> Thuoctinhsanphamchitiets { get; set; }
	}
}
