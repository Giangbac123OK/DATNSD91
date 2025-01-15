using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AppData.IRepository;
using AppData.IService;
using AppData.Models;
using AppData.Repository;
using Microsoft.Extensions.Configuration;
using System.Xml.Linq;
using AppData.Dto_Admin;

namespace AppData.Service
{
    public class NhanvienService: INhanvienService
	{
		private readonly INhanvienRepos _repository;

		private readonly IConfiguration _configuration;
		public NhanvienService(INhanvienRepos repository, IConfiguration configuration)
        {
			_repository=repository;
			_configuration = configuration;

		}
		public async Task<IEnumerable<Nhanvien>> GetAllNhanviensAsync()
		{
			var nhanviens = await _repository.GetAllAsync();
			return nhanviens.Select(n => new Nhanvien
			{
				Id =n.Id,
				Hoten = n.Hoten,
				Ngaysinh = n.Ngaysinh,
				Diachi = n.Diachi,
				Gioitinh = n.Gioitinh,
				Sdt = n.Sdt,
				Email = n.Email,
				Trangthai = n.Trangthai,
				Password = n.Password,
				Role = n.Role
			});
		}

		public async Task<NhanvienDTO> GetNhanvienByIdAsync(int id)
		{
			var nhanvien = await _repository.GetByIdAsync(id);
			if (nhanvien == null) throw new KeyNotFoundException("Nhân viên không tồn tại.");

			return new NhanvienDTO
			{
				Hoten = nhanvien.Hoten,
				Ngaysinh = nhanvien.Ngaysinh,
				Diachi = nhanvien.Diachi,
				Gioitinh = nhanvien.Gioitinh,
				Sdt = nhanvien.Sdt,
				Email = nhanvien.Email,
				Trangthai = nhanvien.Trangthai,
				Password = nhanvien.Password,
				Role = nhanvien.Role
			};
		}

		public async Task AddNhanvienAsync(NhanvienDTO nhanvienDto)
		{

			if (_repository.EmailExists(nhanvienDto.Email))
			{
				throw new Exception("Email đã tồn tại.");
				
			}
			
				var nhanvien = new Nhanvien
				{
					Hoten = nhanvienDto.Hoten,
					Ngaysinh = nhanvienDto.Ngaysinh,
					Diachi = nhanvienDto.Diachi,
					Gioitinh = nhanvienDto.Gioitinh,
					Sdt = nhanvienDto.Sdt,
					Email = nhanvienDto.Email,
					Trangthai = nhanvienDto.Trangthai, // Mặc định "hoạt động"
					Password = nhanvienDto.Password,
					Role = nhanvienDto.Role // 0: Quản lý, 1: Nhân viên
				};
				await _repository.AddAsync(nhanvien);
			
			
			
		}

		public async Task UpdateNhanvienAsync(int id, NhanvienDTO nhanvienDto)
		{

			if (_repository.EmailExists(nhanvienDto.Email,id))
			{
				throw new Exception("Email đã tồn tại. Vui lòng nhập email khác.");
			}
			

			var nhanvien = await _repository.GetByIdAsync(id);
			if (nhanvien == null) throw new KeyNotFoundException("Nhân viên không tồn tại.");

			nhanvien.Hoten = nhanvienDto.Hoten;
			nhanvien.Ngaysinh = nhanvienDto.Ngaysinh;
			nhanvien.Diachi = nhanvienDto.Diachi;
			nhanvien.Gioitinh = nhanvienDto.Gioitinh;
			nhanvien.Sdt = nhanvienDto.Sdt;
			nhanvien.Email = nhanvienDto.Email;
			nhanvien.Password = nhanvienDto.Password;
			nhanvien.Trangthai = nhanvienDto.Trangthai;
			nhanvien.Role = nhanvienDto.Role;

			await _repository.UpdateAsync(nhanvien);
		
		}

		public async Task DeleteNhanvienAsync(int id)
		{
			await _repository.DeleteAsync(id);
		}
		public async Task<bool> CheckForeignKeyConstraintAsync(int nhanvienId)
		{
			return await _repository.CheckForeignKeyConstraintAsync(nhanvienId);
		}

		public async Task<Nhanvien> LoginAsync(string email, string password)
		{
			
			var nhanvien = await _repository.GetBySdtAndPasswordAsync(email, password);

			// Kiểm tra nếu nhân viên không tồn tại hoặc không có quyền đăng nhập
			if (nhanvien == null || nhanvien.Trangthai != 0 || nhanvien.Role != 0)
			{
				return null; // Trả về null nếu không hợp lệ
			}

			return nhanvien; // Trả về đối tượng nhân viên nếu hợp lệ
		}
		public async Task<Nhanvien> LoginAsyncEmployee(string email, string password)
		{

			var nhanvien = await _repository.GetBySdtAndPasswordAsync(email, password);

			// Kiểm tra nếu nhân viên không tồn tại hoặc không có quyền đăng nhập
			if (nhanvien == null || nhanvien.Trangthai != 0 || nhanvien.Role != 1)
			{
				return null; // Trả về null nếu không hợp lệ
			}

			return nhanvien; // Trả về đối tượng nhân viên nếu hợp lệ
		}


		public async Task<NhanvienDTO> FindByEmailAsync(string email)
		{
			var nhanvien = await _repository.GetByEmailAsync(email);
			if (nhanvien == null)
				return null;

			return new NhanvienDTO
			{
				
				Hoten = nhanvien.Hoten,
				Ngaysinh = nhanvien.Ngaysinh,
				Diachi = nhanvien.Diachi,
				Gioitinh = nhanvien.Gioitinh,
				Sdt = nhanvien.Sdt,
				Email = nhanvien.Email,
				Trangthai = nhanvien.Trangthai,
				Password = nhanvien.Password,
				Role = nhanvien.Role
			};
		}
		public async Task<bool> ChangePasswordAsync(Doimk changePasswordDto)
		{
			// Lấy nhân viên từ email
			var nhanvien = await _repository.GetByEmailAsync(changePasswordDto.Email);
			if (nhanvien == null)
			{
				throw new Exception("Không tìm thấy nhân viên với email này.");
			}

			// Kiểm tra mật khẩu cũ
			if (nhanvien.Password != changePasswordDto.OldPassword)
			{
				throw new Exception("Mật khẩu cũ không đúng.");
			}

			// Cập nhật mật khẩu mới
			nhanvien.Password = changePasswordDto.NewPassword;
			await _repository.UpdateNhanvienAsync(nhanvien);

			return true;
		}
		public async Task<(bool IsSuccess, string Otp)> SendOtpAsync(string email)
		{
			var otp = GenerateOtp();
			var subject = "Mã OTP xác thực quên mật khẩu";
			var body = $"Mã OTP của bạn là: {otp}. Vui lòng không chia sẻ mã này với bất kỳ ai.";

			try
			{
				var senderEmail = _configuration["EmailSettings:SenderEmail"];
				var senderPassword = _configuration["EmailSettings:SenderPassword"];
				var smtpServer = _configuration["EmailSettings:SmtpServer"];
				var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);

				var client = new SmtpClient(smtpServer)
				{
					Port = smtpPort,
					Credentials = new NetworkCredential(senderEmail, senderPassword),
					EnableSsl = true
				};

				var message = new MailMessage(senderEmail, email, subject, body);
				await client.SendMailAsync(message);
				return (true, otp); // Trả về OTP sau khi gửi thành công
			}
			catch (Exception ex)
			{
				// Ghi log lỗi
				Console.WriteLine($"Error sending email: {ex.Message}");
				if (ex.InnerException != null)
				{
					Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
				}
				return (false, string.Empty); // Trả về lỗi và không có OTP
			}
		}
		public async Task UpdateStatusAsync(int id)
		{
			// Lấy nhân viên từ repository
			var nhanvien = await _repository.GetByIdAsync(id);

			if (nhanvien == null)
			{
				throw new Exception("Nhân viên không tồn tại.");
			}

			// Kiểm tra trạng thái hiện tại và thay đổi trạng thái
			if (nhanvien.Trangthai == 0)
			{
				nhanvien.Trangthai = 1; // Dừng hoạt động
			}
			else if (nhanvien.Trangthai == 1)
			{
				nhanvien.Trangthai = 0; // Hoạt động lại
			}

			// Cập nhật nhân viên qua repository
			await _repository.UpdateAsync(nhanvien);
		}
		// Hàm tạo mã OTP ngẫu nhiên
		public string GenerateOtp()
		{
			var random = new Random();
			var otp = random.Next(100000, 999999).ToString();
			return otp;
		}
		public IEnumerable<Nhanvien> SearchNhanvienByName(string name)
		{
			var nhanviens = _repository.SearchByName(name);
		
			return nhanviens.Select(nhanvien => new Nhanvien
			{
				Id =nhanvien.Id,
				Hoten = nhanvien.Hoten,
				Ngaysinh = nhanvien.Ngaysinh,
				Diachi = nhanvien.Diachi,
				Gioitinh = nhanvien.Gioitinh,
				Sdt = nhanvien.Sdt,
				Email = nhanvien.Email,
				Trangthai = nhanvien.Trangthai,
				Password = nhanvien.Password,
				Role = nhanvien.Role

			});
		}

		public IEnumerable<Nhanvien> SearchNhanvienByPhoneNumber(string phoneNumber)
		{
			var nhanviens = _repository.SearchByPhoneNumber(phoneNumber);
			
			return nhanviens.Select(nhanvien => new Nhanvien
			{
				Id = nhanvien.Id,
				Hoten = nhanvien.Hoten,
				Ngaysinh = nhanvien.Ngaysinh,
				Diachi = nhanvien.Diachi,
				Gioitinh = nhanvien.Gioitinh,
				Sdt = nhanvien.Sdt,
				Email = nhanvien.Email,
				Trangthai = nhanvien.Trangthai,
				Password = nhanvien.Password,
				Role = nhanvien.Role

			});
		}

		public IEnumerable<Nhanvien> SearchNhanvienByEmail(string email)
		{
			var nhanviens = _repository.SearchByEmail(email);
		
			return nhanviens.Select(nhanvien => new Nhanvien
			{
				Id = nhanvien.Id,
				Hoten = nhanvien.Hoten,
				Ngaysinh = nhanvien.Ngaysinh,
				Diachi = nhanvien.Diachi,
				Gioitinh = nhanvien.Gioitinh,
				Sdt = nhanvien.Sdt,
				Email = nhanvien.Email,
				Trangthai = nhanvien.Trangthai,
				Password = nhanvien.Password,
				Role = nhanvien.Role

			});

		}

		public async Task<IEnumerable<Nhanvien>> GetAllnhanvienhoatdong()
		{
			var nhanviens = await _repository.GetAllAsync();
		
			return nhanviens.Select(n => new Nhanvien
			{
				Id = n.Id,
				Hoten = n.Hoten,
				Ngaysinh = n.Ngaysinh,
				Diachi = n.Diachi,
				Gioitinh = n.Gioitinh,
				Sdt = n.Sdt,
				Email = n.Email,
				Trangthai = n.Trangthai,
				Password = n.Password,
				Role = n.Role
			}).Where(x=> x.Trangthai==0);
		}

		public async Task<IEnumerable<Nhanvien>> GetAllnhanviendunghoatdong()
		{
			var nhanviens = await _repository.GetAllAsync();

			return nhanviens.Select(n => new Nhanvien
			{
				Id = n.Id,
				Hoten = n.Hoten,
				Ngaysinh = n.Ngaysinh,
				Diachi = n.Diachi,
				Gioitinh = n.Gioitinh,
				Sdt = n.Sdt,
				Email = n.Email,
				Trangthai = n.Trangthai,
				Password = n.Password,
				Role = n.Role
			}).Where(x => x.Trangthai == 1);
		}

		public async Task<bool> UpdateTrangthaiToDeletedAsync(int id)
		{
			var nhanvien = await _repository.GetByIdAsync(id);
			if (nhanvien == null)
			{
				return false;
			}

			nhanvien.Trangthai = 2; 
			await _repository.UpdateAsync(nhanvien);
			return true;
		}
	}
}
