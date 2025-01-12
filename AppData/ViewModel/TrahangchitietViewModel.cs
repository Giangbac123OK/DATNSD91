using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModel
{
    public class TrahangchitietViewModel
    {
        public int Id { get; set; }
        public int Idtr { get; set; }
        public int Idspct { get; set; }
        public int Idsp { get; set; }
        public string Tensp { get; set; }
        public string? urlHinhanh { get; set; }
        public decimal Tongtienhoan { get; set; }
        public int Tinhtrang {  get; set; }
        public string Hinhthucxuly {  get; set; }
        public int Soluong { get; set; }
        public int Trangthaith { get; set; }
    }
}
