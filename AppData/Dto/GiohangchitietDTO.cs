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
    public class GiohangchitietDTO
    {
        public int Id { get; set; }
        public int Idgh { get; set; }
        public int Idspct { get; set; }
        [Range(0,int.MaxValue,ErrorMessage ="Vui lòng nhập số lượng lớn hơn 0")]
        [Required(ErrorMessage ="Vui lòng không để trống")]
        public int Soluong { get; set; }
    }
}
