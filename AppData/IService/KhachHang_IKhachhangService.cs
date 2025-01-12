using AppData.Dto;
using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IService
{
    public interface KhachHang_IKhachhangService
    {
        Task<IEnumerable<Khachhang>> GetAllKhachhangsAsync();
        Task<KhachhangDTO> GetKhachhangByIdAsync(int id);
        Task AddKhachhangAsync(KhachhangDTO dto);
        Task UpdateKhachhangAsync(int id, KhachhangDTO dto);
        Task UpdateThongTinKhachhangAsync(int id, KhachhangDTO dto);
        Task DeleteKhachhangAsync(int id);
        Task<IEnumerable<KhachhangDTO>> TimKiemAsync(string search);
		Task <KhachhangDTO> FindByEmailAsync(string email);
		Task<(bool isSent, object otp)> SendOtpAsync (string email);
		string GenerateOtp();
	}
}
