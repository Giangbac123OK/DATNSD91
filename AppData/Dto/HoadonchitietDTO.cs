using System;
using System.ComponentModel.DataAnnotations;

namespace AppData.Dto
{
    public class HoaDonchitietDTO
    {
        [Required(ErrorMessage = "Vui lòng nhập hoá đơn")]
        public int Idhd { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập sản phẩm chi tiết")]
        public int Idspct { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Tổng tiền cần trả phải lớn hơn hoặc bằng 0")]
        [Required(ErrorMessage = "Vui lòng nhập tổng tiền cần trả")]
        public int soluong { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền cần trả phải lớn hơn hoặc bằng 0")]
        [Required(ErrorMessage = "Vui lòng nhập tổng tiền cần trả")]
        public decimal Giasp { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền sản phẩm phải lớn hơn hoặc bằng 0")]
        public decimal giamgia { get; set; }
    }
}
