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
    public class DiaChiDTO
    {
        public int Id { get; set; }
        public int Idkh { get; set; }
        [Required(ErrorMessage = "Tên không được để trống.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên phải từ 3 đến 50 ký tự.")]
        public string Ten { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại phải bao gồm đúng 10 chữ số.")]
        public string SDT { get; set; }
        public string Thanhpho { get; set; }
        public string Quanhuyen { get; set; }
        public string Phuongxa { get; set; }
        public string? Diachicuthe { get; set; }
    }
}
