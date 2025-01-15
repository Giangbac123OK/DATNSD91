using AppData.Dto;
using AppData.Dto_Admin;
using AppData.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Controllers
{
    [ApiController]
	[Route("api/[controller]")]
	public class NhanvienController : Controller
	{
		private readonly KhachHang_INhanvienService _KhachHang_service;
		private readonly INhanvienService _service;
		public NhanvienController(KhachHang_INhanvienService service, INhanvienService service1)
		{
			_KhachHang_service = service;
			_service = service1;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var result = await _KhachHang_service.GetAllNhanviensAsync();
			return Ok(result.Select(nv => new
			{
				nv.Hoten,
				nv.Ngaysinh,
				nv.Diachi,
				Gioitinh = nv.Gioitinh== true?"Nam":"Nữ",
				nv.Sdt,
				Trangthai = nv.Trangthai == 0 ? "Hoạt động" : "Dừng hoạt động",
				nv.Password,
				Role = nv.Role == 0 ? "Quản lý" : "Nhân viên"
			}));
		}

		[HttpGet("_KhachHang/{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			try
			{
				var nhanvien = await _KhachHang_service.GetNhanvienByIdAsync(id);
				return Ok(new
				{
					nhanvien.Hoten,
					nhanvien.Ngaysinh,
					nhanvien.Diachi,
					Gioitinh = nhanvien.Gioitinh == false ? "Nam" : "Nữ",
					nhanvien.Sdt,
					Trangthai = nhanvien.Trangthai == 0 ? "Hoạt động" : "Dừng hoạt động",
					nhanvien.Password,
					Role = nhanvien.Role == 0 ? "Quản lý" : "Nhân viên"
				});
			}
			catch (KeyNotFoundException)
			{
				return NotFound("Nhân viên không tồn tại.");
			}


		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] AppData.Dto.NhanvienDTO nhanvienDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			await _KhachHang_service.AddNhanvienAsync(nhanvienDto);
			return CreatedAtAction(nameof(GetById), new { id = nhanvienDto.Hoten }, nhanvienDto);
		}

		[HttpPut("_KhachHang/{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] AppData.Dto.NhanvienDTO nhanvienDto)
		{
			try
			{
				await _KhachHang_service.UpdateNhanvienAsync(id, nhanvienDto);
				return NoContent();
			}
			catch (KeyNotFoundException)
			{
				return NotFound("Nhân viên không tồn tại.");
			}
		}

		[HttpDelete("_KhachHang/{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await _KhachHang_service.DeleteNhanvienAsync(id);
				return NoContent();
			}
			catch (KeyNotFoundException)
			{
				return NotFound("Nhân viên không tồn tại.");
			}
		}


		[HttpGet("Admin")]
		public async Task<IActionResult> GetAllAdmin()
		{
			var result = await _service.GetAllNhanviensAsync();
			return Ok(result.Select(nv => new
			{
				nv.Id,
				nv.Hoten,
				nv.Ngaysinh,
				nv.Diachi,
				Gioitinh = nv.Gioitinh == true ? "Nam" : "Nữ",
				nv.Sdt,
				nv.Email,
				Trangthai = nv.Trangthai == 0 ? "Hoạt động" : "Dừng hoạt động",
				nv.Password,
				Role = nv.Role == 0 ? "Quản lý" : "Nhân viên"
			}));
		}
		[HttpPut("delete/{id}/Admin")]
		public async Task<IActionResult> UpdateTrangthaiToDeleted(int id)
		{
			var result = await _service.UpdateTrangthaiToDeletedAsync(id);
			if (!result)
			{
				return NotFound(new { message = "Nhân viên không tồn tại" });
			}

			return NoContent();
		}

		[HttpGet("/hoatdong/Admin")]
		public async Task<IActionResult> GetAllnhanvienhoatdong()
		{
			var result = await _service.GetAllnhanvienhoatdong();
			return Ok(result.Select(nv => new
			{
				nv.Id,
				nv.Hoten,
				nv.Ngaysinh,
				nv.Diachi,
				Gioitinh = nv.Gioitinh == true ? "Nam" : "Nữ",
				nv.Sdt,
				nv.Email,
				Trangthai = "Hoạt động",
				nv.Password,
				Role = nv.Role == 0 ? "Quản lý" : "Nhân viên"
			}));
		}
		[HttpGet("/dunghoatdong/Admin")]
		public async Task<IActionResult> GetAllnhanviendunghoatdong()
		{
			var result = await _service.GetAllnhanviendunghoatdong();
			return Ok(result.Select(nv => new
			{
				nv.Id,
				nv.Hoten,
				nv.Ngaysinh,
				nv.Diachi,
				Gioitinh = nv.Gioitinh == true ? "Nam" : "Nữ",
				nv.Sdt,
				nv.Email,
				Trangthai = "Dừng hoạt động",
				nv.Password,
				Role = nv.Role == 0 ? "Quản lý" : "Nhân viên"
			}));
		}

		[HttpGet("{id}/Admin")]
		public async Task<IActionResult> GetByIdAdmin(int id)
		{
			try
			{
				var nhanvien = await _service.GetNhanvienByIdAsync(id);
				return Ok(new
				{
					nhanvien.Hoten,
					nhanvien.Ngaysinh,
					nhanvien.Diachi,
					Gioitinh = nhanvien.Gioitinh == false ? "Nam" : "Nữ",
					nhanvien.Sdt,
					nhanvien.Email,
					Trangthai = nhanvien.Trangthai == 0 ? "Hoạt động" : "Dừng hoạt động",
					nhanvien.Password,
					Role = nhanvien.Role == 0 ? "Quản lý" : "Nhân viên"
				});
			}
			catch (KeyNotFoundException)
			{
				return NotFound("Nhân viên không tồn tại.");
			}


		}


		[HttpPost("Admin")]
		public async Task<IActionResult> Create([FromBody] AppData.Dto_Admin.NhanvienDTO nhanvienDto)
		{
			if (!ModelState.IsValid) // Kiểm tra model có hợp lệ không
			{
				// Trả về lỗi đơn giản hóa
				var firstError = ModelState.Values
					.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage)
					.FirstOrDefault(); // Lấy thông báo lỗi đầu tiên
				return BadRequest(firstError);
			}
			try
			{


				await _service.AddNhanvienAsync(nhanvienDto);
				return CreatedAtAction(nameof(GetById), new { id = nhanvienDto.Hoten }, nhanvienDto);
			}
			catch (DbUpdateException ex)
			{
				if (ex.InnerException?.Message.Contains("IX_Unique_Email") == true)
				{
					return BadRequest("Email đã tồn tại. Vui lòng nhập email khác.");
				}
				return StatusCode(500, "Lỗi khi lưu dữ liệu vào cơ sở dữ liệu.");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message); // Trả về lỗi cho client
			}
		}
		// API để cập nhật trạng thái nhân viên
		[HttpPut("update-status/{id}/Admin")]
		public async Task<IActionResult> UpdateStatusAsync(int id)
		{
			try
			{
				await _service.UpdateStatusAsync(id);
				return Ok(new { Message = "Cập nhật trạng thái thành công." });
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = ex.Message });
			}
		}



		[HttpPost("login/Admin")]
		public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
		{
			try
			{
				var nhanvien = await _service.LoginAsync(loginDto.Email, loginDto.Password);

				if (nhanvien == null)
				{
					return BadRequest(new { success = false, message = "Số điện thoại hoặc mật khẩu không đúng, hoặc tài khoản không có quyền truy cập." });
				}

				return Ok(new { success = true, message = "Đăng nhập thành công!", data = nhanvien });
			}
			catch (Exception ex)
			{
				// Log lỗi để kiểm tra sau
				Console.WriteLine("Lỗi xảy ra trong Login: " + ex.Message);
				return StatusCode(500, new { success = false, message = "Có lỗi xảy ra trên server." });
			}
		}
		[HttpPost("login/Employee")]
		public async Task<IActionResult> LoginEmployee([FromBody] LoginDto loginDto)
		{
			try
			{
				var nhanvien = await _service.LoginAsyncEmployee(loginDto.Email, loginDto.Password);

				if (nhanvien == null)
				{
					return BadRequest(new { success = false, message = "Số điện thoại hoặc mật khẩu không đúng, hoặc tài khoản không có quyền truy cập." });
				}

				return Ok(new { success = true, message = "Đăng nhập thành công!", data = nhanvien });
			}
			catch (Exception ex)
			{
				// Log lỗi để kiểm tra sau
				Console.WriteLine("Lỗi xảy ra trong Login: " + ex.Message);
				return StatusCode(500, new { success = false, message = "Có lỗi xảy ra trên server." });
			}
		}

		[HttpGet("check-foreign-key/{id}/Admin")]
		public async Task<IActionResult> CheckForeignKey(int id)
		{
			var hasConstraint = await _service.CheckForeignKeyConstraintAsync(id);
			if (hasConstraint)
			{
				return Ok(new { message = "Nhân viên có ràng buộc khóa ngoại." });
			}

			return Ok(new { message = "Nhân viên không có ràng buộc khóa ngoại." });
		}

		[HttpPut("{id}/Admin")]
		public async Task<IActionResult> Update(int id, [FromBody] AppData.Dto_Admin.NhanvienDTO nhanvienDto)
		{
			try
			{
				await _service.UpdateNhanvienAsync(id, nhanvienDto);
				return Ok(new { message = "Cập nhật thành công" });

			}
			catch (KeyNotFoundException ex)
			{
				// Phản hồi khi không tìm thấy nhân viên
				return NotFound(new { message = ex.Message });
			}
			catch (Exception ex)
			{
				// Trả về thông báo lỗi đơn giản
				return BadRequest(ex.Message);
			}
		}

		[HttpDelete("{id}/Admin")]
		public async Task<IActionResult> DeleteAdmin(int id)
		{
			try
			{
				await _service.DeleteNhanvienAsync(id);
				return NoContent();
			}
			catch (KeyNotFoundException)
			{
				return NotFound("Nhân viên không tồn tại.");
			}
		}

		// API tìm kiếm nhân viên theo email
		[HttpGet("find-by-email/Admin")]
		public async Task<IActionResult> FindByEmailAsync(string email)
		{
			var nhanVien = await _service.FindByEmailAsync(email);
			if (nhanVien == null)
				return NotFound("Nhân viên không tồn tại.");

			return Ok(nhanVien);
		}

		// API gửi mã OTP cho quên mật khẩu
		[HttpPost("send-otp/Admin")]
		public async Task<IActionResult> SendOtpAsync([FromBody] ForgotPasswordDto dto)
		{
			var (isSent, otp) = await _service.SendOtpAsync(dto.Email); // Nhận kết quả và OTP từ service

			if (!isSent)
				return BadRequest("Gửi OTP không thành công.");

			return Ok(new { success = true, message = "Mã OTP đã được gửi.", otp }); // Trả OTP cho client (chỉ khi cần, thường dùng trong môi trường phát triển)
		}
		[HttpPost("change-password/Admin")]
		public async Task<IActionResult> ChangePassword([FromBody] Doimk changePasswordDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				await _service.ChangePasswordAsync(changePasswordDto);
				return Ok(new { message = "Đổi mật khẩu thành công." });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
		[HttpGet("search-by-name/Admin")]
		public IActionResult SearchByName(string name)
		{

			var result = _service.SearchNhanvienByName(name);
			return Ok(result.Select(nv => new
			{
				nv.Id,
				nv.Hoten,
				nv.Ngaysinh,
				nv.Diachi,
				Gioitinh = nv.Gioitinh == true ? "Nam" : "Nữ",
				nv.Sdt,
				nv.Email,
				Trangthai = nv.Trangthai == 0 ? "Hoạt động" : "Dừng hoạt động",
				nv.Password,
				Role = nv.Role == 0 ? "Quản lý" : "Nhân viên"
			}));
			//return Ok(_service.SearchNhanvienByName(name));

		}

		[HttpGet("search-by-phone/Admin")]
		public IActionResult SearchByPhoneNumber(string phoneNumber)
		{
			var result = _service.SearchNhanvienByPhoneNumber(phoneNumber);
			return Ok(result.Select(nv => new
			{
				nv.Id,
				nv.Hoten,
				nv.Ngaysinh,
				nv.Diachi,
				Gioitinh = nv.Gioitinh == true ? "Nam" : "Nữ",
				nv.Sdt,
				nv.Email,
				Trangthai = nv.Trangthai == 0 ? "Hoạt động" : "Dừng hoạt động",
				nv.Password,
				Role = nv.Role == 0 ? "Quản lý" : "Nhân viên"
			}));


		}

		[HttpGet("search-by-email/Admin")]
		public IActionResult SearchByEmail(string email)
		{

			var result = _service.SearchNhanvienByEmail(email);
			return Ok(result.Select(nv => new
			{
				nv.Id,
				nv.Hoten,
				nv.Ngaysinh,
				nv.Diachi,
				Gioitinh = nv.Gioitinh == true ? "Nam" : "Nữ",
				nv.Sdt,
				nv.Email,
				Trangthai = nv.Trangthai == 0 ? "Hoạt động" : "Dừng hoạt động",
				nv.Password,
				Role = nv.Role == 0 ? "Quản lý" : "Nhân viên"
			}));


		}

	}
}
