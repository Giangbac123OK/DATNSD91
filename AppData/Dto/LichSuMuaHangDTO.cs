using AppData.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Dto
{
    public class LichSuMuaHangDTO
    {
        public int IdHD { get; set; }
        public int IdKH { get; set; }
        public int Trangthaithanhtoan { get; set; }
        public int Donvitrangthai { get; set; }
        public DateTime Thoigiandathang { get; set; }
        public decimal Tongtiencantra { get; set; }
        public decimal Tongtiensanpham { get; set; }
        public decimal? Tonggiamgia { get; set; }
        public int TrangthaiDonHang { get; set; }
       
        public List<HoaDonCTDTO> HoaDonCTS { get; set; }
    }

    public class HoaDonCTDTO
    {
        public int IdHDCT { get; set; }
        public int Soluong { get; set; }
        public decimal Giasp { get; set; }
        public decimal? Giamgia { get; set; }
        public int idDanhDia { get; set; }
        public int IdSPCT { get; set; }
        public decimal Giathoidiemhientai { get; set; }
        public string TenSanPham { get; set; }
        public string URLHinhAnh { get; set; }
        public string tenThuocTinh { get; set; }
        public string TenThucTinhChiTiet { get; set; }
    }

}
