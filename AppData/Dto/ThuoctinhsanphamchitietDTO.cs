using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Dto
{
	public class ThuoctinhsanphamchitietDTO
	{
        public int Idtt { get; set; }
        public int Idspct { get; set; }

        [Required(ErrorMessage = "Tên thuộc tính chi tiết không được để trống")]
		[MaxLength(50)]
        public List<string> Tenthuoctinhchitiet { get; set; }
    }
}
