using AppData.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Dto
{
    public class lichsuthanhtoanDTO
    {
        public int idHd { get; set; }
        public int idPttt { get; set; }
        public DateTime Thoigianthanhtoan { get; set; }
        public int Trangthai { get; set; }
    }
}
