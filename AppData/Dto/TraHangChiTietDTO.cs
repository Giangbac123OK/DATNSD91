using AppData.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Dto
{
    public class TraHangChiTietDTO
    {
        public int Id { get; set; }
        public int Idth { get; set; }
        public int Soluong { get; set; }
        public int Tinhtrang { get; set; }
        public string? Ghichu { get; set; }
        public string Hinhthucxuly { get; set; }
        public int Idhdct { get; set; }
    }
}
