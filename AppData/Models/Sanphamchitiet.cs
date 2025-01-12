using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
	public class Sanphamchitiet
	{

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id {  get; set; }
		[StringLength(200, ErrorMessage = "Mô tả không được quá 200 ký tự")]
		public string? Mota {  get; set; }
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
		public int Soluong {  get; set; }
		[Required(ErrorMessage = "Id sản phẩm là bắt buộc")]
		public int Idsp { get; set; }
		[ForeignKey("Idsp")]
		public virtual Sanpham Sanpham { get; set; }
		public virtual ICollection<Hoadonchitiet> Hoadonchitiets { get; set; }
		public virtual ICollection<Thuoctinhsanphamchitiet> Thuoctinhsanphamchitiets { get; set; }
		public virtual ICollection<Giohangchitiet> Giohangchitiets { get; set; }
		public virtual ICollection<Salechitiet> Salechitiets { get; set; }
		// Phương thức để cập nhật trạng thái dựa trên số lượng
		
	}
}
