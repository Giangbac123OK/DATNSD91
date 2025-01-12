using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
	public class Hoadonnhap
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id {  get; set; }
		public int Idnv { get; set; }
		[ForeignKey("Idnv")]
		public virtual Nhanvien Nhanvien {  get; set; }
		public int Idncc { get; set; }
		[ForeignKey("Idncc")]
		public virtual Nhacungcap Nhacungcap {  get; set; }
		[Required(ErrorMessage = "Ngày nhập không được để trống")]
		public DateTime Ngaynhap { get; set; }
		[Required(ErrorMessage = "Trạng thái không được để trống")]
		[Range(0, 1, ErrorMessage = "Trạng thái phải là 0 (Hợp lệ) hoặc 1 (Không hợp lệ)")]
		public int Trangthai {  get; set; }
		[Required(ErrorMessage = "Tổng tiền hàng không được để trống")]
		[Range(0, 20000000, ErrorMessage = "Tổng tiền hàng phải nhỏ hơn 20 triệu")]
		public decimal Tongtienhang {  get; set; }
		[Required(ErrorMessage = "Người giao không được để trống")]
		public string Nguoigiao {  get; set; }
		[Required(ErrorMessage = "Số điện thoại người giao không được để trống")]
		[Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
		public string Sdtnguoigiao {  get; set; }
		public virtual ICollection<Hoadonnhapchitiet> Hoadonnhapchitiets { get; set; }
	}
}
