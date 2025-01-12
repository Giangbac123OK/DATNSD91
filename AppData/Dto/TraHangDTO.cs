using AppData.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Dto
{
    public class TraHangDTO
    {
        public int Id {  get; set; }
        [Required(ErrorMessage = "Vui lòng không để trống tên khách hàng!")]
        [StringLength(100, ErrorMessage ="Vui lòng nhập tên khách hàng không quá 100 ký tự!")]
        public string Tenkhachhang { get; set; }
        public int? Idnv { get; set; }
        [Required(ErrorMessage = "Vui lòng không để trống mã khách hàng!")]
        public int Idkh { get; set; }
        public decimal? Sotienhoan { get; set; }
        public string? Lydotrahang { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn trạng thái!")]
        [Range(0,3)]
        public int Trangthai { get; set; }
        [Required(ErrorMessage = "Vui lòng phương thức hoàn tiền!")]
        public string Phuongthuchoantien { get; set; }
        public DateTime? Ngaytrahangdukien { get; set; }
        public DateTime? Ngaytrahangthucte { get; set; }
        public string? Chuthich { get; set; }
    }
}
