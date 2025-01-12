using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModel
{
    public class TraHangViewModel
    {
        public int Id { get; set; }
        public int Idkh { get; set; }
        public string Tenkh {  get; set; }
        public string? Lydotrahang {  get; set; }
        public string Phuongthuchoantien {  get; set; }
        public DateTime? Ngaytrahangdukien {  get; set; }
        public DateTime? Ngaytrahangthucte { get; set; }
        public decimal? Tongtienhoan { get; set; }
        public int Tongsoluong { get; set; }
        public int Trangthai { get; set; }
    }
}