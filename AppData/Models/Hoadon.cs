using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlTypes;
using System.ComponentModel;

namespace AppData.Models
{
    public enum OrderStatus
    {
        [Description("Chờ xác nhận")]
        ChờXacNhan = 0,
        //aaa
        [Description("Đơn hàng đã được xác nhận")]
        DonHangDaXacNhan = 1,

        [Description("Đơn hàng đang được giao")]
        DonHangDangDuocGiao = 2,

        [Description("Đơn hàng thành công")]
        DonHangThanhCong = 3,

        [Description("Đơn hàng đã huỷ")]
        DonHangDaHuy = 4,

        [Description("Trả hàng")]
        TraHangThanhCong = 5,
    }
    public class Hoadon
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        public int? Idnv { get; set; }

        [ForeignKey("Idnv")]
        public virtual Nhanvien? Nhanvien { get; set; }


        public int? Idkh { get; set; }

        [ForeignKey("Idkh")]
        public virtual Khachhang? Khachhang { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Trạng thái thanh toán phải là số dương.")]
        public int Trangthaithanhtoan { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Đơn vị trạng thái phải là số dương.")]
        public int Donvitrangthai { get; set; }

        [Required]
        public DateTime Thoigiandathang { get; set; }


        [StringLength(200, ErrorMessage = "Địa chỉ ship không được vượt quá 200 ký tự.")]
        public string? Diachiship { get; set; }
        public decimal? Tiencoc { get; set; }

        public DateTime? Ngaygiaodukien { get; set; }

        public DateTime? Ngaygiaothucte { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền phải là số dương.")]
        public decimal Tongtiencantra { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền sản phẩm phải là số dương.")]
        public decimal Tongtiensanpham { get; set; }

        [Required]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string? Sdt { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Tổng giảm giá phải là số dương.")]
        public decimal? Tonggiamgia { get; set; }

        public int? Idgg { get; set; }

        [ForeignKey("Idgg")]
        public virtual Giamgia? Giamgia { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Trạng thái phải là số dương.")]
        public int Trangthai { get; set; }
        public string? Ghichu { get; set; }
        public virtual ICollection<Hoadonchitiet> Hoadonchitiets { get; set; }
        public virtual ICollection<Lichsuthanhtoan> Lichsuthanhtoans { get; set; }

        // Trạng thái hiển thị dưới dạng chuỗi
        public string TrangthaiStr => GetEnumDescription((OrderStatus)Trangthai);
        // Phương thức để lấy giá trị mô tả từ enum
        private string GetEnumDescription(OrderStatus status)
        {
            var field = status.GetType().GetField(status.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute == null ? status.ToString() : attribute.Description;
        }
    }
}