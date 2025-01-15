using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto_Admin;
using AppData.Models;

namespace AppData.IService
{
    public interface INhanvienService
	{
        Task<bool> UpdateTrangthaiToDeletedAsync(int id);

		Task<IEnumerable<Nhanvien>> GetAllNhanviensAsync();
        Task<IEnumerable<Nhanvien>> GetAllnhanvienhoatdong();
        Task<IEnumerable<Nhanvien>> GetAllnhanviendunghoatdong();
        Task<NhanvienDTO> GetNhanvienByIdAsync(int id);
        Task AddNhanvienAsync(NhanvienDTO nhanvienDto);
        Task UpdateNhanvienAsync(int id, NhanvienDTO nhanvienDto);
        Task DeleteNhanvienAsync(int id);
        Task<Nhanvien> LoginAsync(string Email, string password);
        Task<NhanvienDTO> FindByEmailAsync(string email);
		Task<(bool IsSuccess, string Otp)> SendOtpAsync(string email);
        string GenerateOtp();
		Task<bool> ChangePasswordAsync(Doimk changePasswordDto);
        IEnumerable<Nhanvien> SearchNhanvienByName(string name);
        IEnumerable<Nhanvien> SearchNhanvienByPhoneNumber(string phoneNumber);
        IEnumerable<Nhanvien> SearchNhanvienByEmail(string email);
		Task<bool> CheckForeignKeyConstraintAsync(int nhanvienId);
		Task UpdateStatusAsync(int id);
        Task<Nhanvien> LoginAsyncEmployee(string email, string password);
	}
}
