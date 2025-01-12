using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Dto
{
    public class SanphamchitietsDTO
    {
        public int Id { get; set; }

        public string? Mota { get; set; }

        [Range(0, 1, ErrorMessage = "Trạng thái phải là sử dụng hoặc không sử dụng")]
        public int Trangthai { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá thời điểm hiện tại")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá không được âm")]
        public decimal Giathoidiemhientai { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int Soluong { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn sản phẩm")]
        public int Idsp { get; set; }
    }
}
