using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AppData.Dto
{
    public class HoaDonDTO
    {
        public int Id { get; set; }
        public int? Idnv { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập khách hàng")]
        public int? Idkh { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập trạng thái thanh toán")]
        public int Trangthaithanhtoan { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập đơn vị trạng thái")]
        public int Donvitrangthai { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập thời gian đặt hàng")]
        public DateTime Thoigiandathang { get; set; }
        public string? Ghichu { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ ship")]
        [StringLength(200, ErrorMessage = "Địa chỉ ship không được quá 200 ký tự")]
        public string Diachiship { get; set; }

        public DateTime? Ngaygiaodukien { get; set; }
        public decimal? Tiencoc { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày giao thực tế")]
        public DateTime? Ngaygiaothucte { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền cần trả phải lớn hơn hoặc bằng 0")]
        [Required(ErrorMessage = "Vui lòng nhập tổng tiền cần trả")]
        public decimal Tongtiencantra { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền sản phẩm phải lớn hơn hoặc bằng 0")]
        [Required(ErrorMessage = "Vui lòng nhập tổng tiền sản phẩm")]
        public decimal Tongtiensanpham { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại chỉ được nhập 10 ký tự số và không nhập kí tự đặc biệt")]
        public string Sdt { get; set; }

        public decimal? Tonggiamgia { get; set; }
        public int? Idgg { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập trạng thái")]
        public int Trangthai { get; set; }
    }
}
