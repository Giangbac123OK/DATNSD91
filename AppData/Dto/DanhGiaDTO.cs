using AppData.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Dto
{
    public class DanhGiaDTO
    {
        public int Id {  get; set; }
        public int Idkh { get; set; }
        public int Trangthai { get; set; }
        public string? Noidungdanhgia { get; set; }
        
        public DateTime Ngaydanhgia { get; set; }
        public int Idhdct { get; set; }
        public string? UrlHinhanh { get; set; }
    }
}
