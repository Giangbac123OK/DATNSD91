using AppData;
using AppData.Dto;
using AppData.Dto_Admin;
using AppData.IService;
using AppData.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhachhangController : ControllerBase
    {
        private readonly KhachHang_IKhachhangService _KhachHang_Service;
        private readonly MyDbContext _context;
		private readonly IKhackhangservice _service;
		public KhachhangController(KhachHang_IKhachhangService ser, MyDbContext context, IKhackhangservice service)
        {
            _KhachHang_Service = ser;
            _context = context;
			_service = service;
		}
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _KhachHang_Service.GetAllKhachhangsAsync();
            return Ok(result.Select(kh => new
            {
                kh.Id,
                kh.Ten,
                kh.Sdt,
                kh.Ngaysinh,
                kh.Tichdiem,
                kh.Email,
                kh.Diachi,
                kh.Password,
                kh.Diemsudung,
                Trangthai = kh.Trangthai == 0 ? "Hoạt động" : "Tài khoản bị khoá",
                kh.Idrank
            }));
        }
        [HttpGet("_KhachHang/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var kh = await _KhachHang_Service.GetKhachhangByIdAsync(id);
                return Ok(new
                {
                    
                    kh.Ten,
                    kh.Sdt,
                    kh.Ngaysinh,
                    kh.Tichdiem,
                    kh.Email,
                    kh.Diachi,
                    kh.Password,
                    kh.Diemsudung,
                    Trangthai = kh.Trangthai == 0 ? "Hoạt động" : "Tài khoản bị khoá",
                    kh.Idrank
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Khách hàng không tồn tại.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(KhachhangDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _KhachHang_Service.AddKhachhangAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = dto.Ten }, dto);
        }

        [HttpPut("_KhachHang/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] KhachhangDTO dto)
        {
            try
            {
                await _KhachHang_Service.UpdateKhachhangAsync(id, dto);
               
                if (dto.Trangthai == 1)
                {
                    return Ok(new { message = "Tài khoản đã bị khóa." });
                }
                return Ok(new { message = "Cập nhật thông tin thành công." });
            }

            catch (KeyNotFoundException)
            {
                return NotFound("Khách hàng không tồn tại.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        [HttpPut("_KhachHang/diem/{id}")]
        public async Task<IActionResult> UpdateDiem(int id, int diemsudung)
        {
            try
            {
                var checkhachang = await _context.khachhangs.FirstOrDefaultAsync(kh => kh.Id == id);
                if (checkhachang == null)
                {
                    return NotFound("Khách hàng không tồn tại.");
                }
                checkhachang.Diemsudung = diemsudung;

                _context.khachhangs.Update(checkhachang);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Cập nhật thông tin thành công." });
            }

            catch (KeyNotFoundException)
            {
                return NotFound("Khách hàng không tồn tại.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        [HttpPut("_KhachHang/UpdateThongTinKhachhangAsync/{id}")]
        public async Task<IActionResult> UpdateThongTinKhachhangAsync(int id, [FromBody] KhachhangDTO dto)
        {
            try
            {
                await _KhachHang_Service.UpdateThongTinKhachhangAsync(id, dto);

                if (dto.Trangthai == 1)
                {
                    return Ok(new { message = "Tài khoản đã bị khóa." });
                }
                return Ok(new { message = "Cập nhật thông tin thành công." });
            }

            catch (KeyNotFoundException)
            {
                return NotFound("Khách hàng không tồn tại.");
            }
        }

        [HttpDelete("_KhachHang/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _KhachHang_Service.DeleteKhachhangAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Khách hàng không tồn tại.");
            }
        }
        [HttpGet("_KhachHang/find-khachhang")]
        public async Task<IActionResult> FindByEmailAsync(string email)
        {
            var nhanVien = await _KhachHang_Service.FindByEmailAsync(email);
            if (nhanVien == null)
                return NotFound("Khách hàng không tồn tại.");

            return Ok(nhanVien);
        }
        // API gửi mã OTP cho quên mật khẩu
        [HttpPost("_KhachHang/send-otp")]
        public async Task<IActionResult> SendOtpAsync(ForgotPasswordRequestKHDto dto)
        {
            var (isSent, otp) = await _KhachHang_Service.SendOtpAsync(dto.Email); // Nhận kết quả và OTP từ service

            if (!isSent)
                return BadRequest("Gửi OTP không thành công.");

            return Ok(new { success = true, message = "Mã OTP đã được gửi.", otp }); // Trả OTP cho client (chỉ khi cần, thường dùng trong môi trường phát triển)
        }
		[HttpPost("_KhachHang/doimatkhau")]
		public async Task<IActionResult> ChangePassword([FromBody] DoimkKhachhang changePasswordDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var khachHang = await _context.khachhangs.FirstOrDefaultAsync(kh => kh.Email == changePasswordDto.Email);
				if (khachHang == null)
				{
					return NotFound(new { message = "Tài khoản không tồn tại" });
				}

				// Kiểm tra mật khẩu cũ
				bool isOldPasswordValid = BCrypt.Net.BCrypt.Verify(changePasswordDto.OldPassword, khachHang.Password);
				if (!isOldPasswordValid)
				{
					return Unauthorized(new { message = "Mật khẩu cũ không chính xác" });
				}

				// Hash mật khẩu mới
				khachHang.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);

				_context.khachhangs.Update(khachHang);
				await _context.SaveChangesAsync();

				return Ok(new { message = "Đổi mật khẩu thành công" });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "Đã xảy ra lỗi trong quá trình đổi mật khẩu", error = ex.Message });
			}
		}
        [HttpPost("_KhachHang/quenmatkhau")]
        public async Task<IActionResult> quenmatkhau([FromBody] DoimkKhachhang changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var khachHang = await _context.khachhangs.FirstOrDefaultAsync(kh => kh.Email == changePasswordDto.Email);
                if (khachHang == null)
                {
                    return NotFound(new { message = "Tài khoản không tồn tại" });
                }

                // Kiểm tra mật khẩu cũ
                var isOldPasswordValid = khachHang.Password;

                // Hash mật khẩu mới
                khachHang.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);

                _context.khachhangs.Update(khachHang);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Đổi mật khẩu thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi trong quá trình đổi mật khẩu", error = ex.Message });
            }
        }
		[HttpGet("{id}/Admin")]
		public async Task<ActionResult<KhachhangDto>> GetByIdAdmin(int id)
		{
			try
			{
				var khachhang = await _service.GetByIdAsyncThao(id);
				return Ok(khachhang);
			}
			catch (Exception ex)
			{
				return NotFound(ex.Message);
			}
		}

		// GET: api/khachhang
		[HttpGet("Admin")]
		public async Task<ActionResult<IEnumerable<Khachhang>>> GetAll()
		{
			var khachhangs = await _service.GetAllAsyncThao();
			return Ok(khachhangs);
		}

		// POST: api/khachhang
		[HttpPost("Admin")]
		public async Task<ActionResult> Create(KhachhangDto dto)
		{
			try
			{
				await _service.CreateAsyncThao(dto);
				return CreatedAtAction(nameof(GetById), new { id = dto.Ten }, dto);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		[HttpPut("Toggle/Admin")]
		public async Task<IActionResult> ToggleTrangthaiAsync(int id)  // Thêm async
		{


			try
			{
				await _service.ToggleTrangthaiAsync(id);
				return Ok(new { Message = "Cập nhật trạng thái thành công." });
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = ex.Message });
			}
		}

		// PUT: api/khachhang/{id}
		[HttpPut("{id}/Admin")]
		public async Task<ActionResult> Update(int id, KhachhangDto dto)
		{
			try
			{
				await _service.UpdateAsyncThao(id, dto);
				return NoContent();
			}
			catch (Exception ex)
			{
				return NotFound(ex.Message);
			}
		}

		// DELETE: api/khachhang/{id}
		[HttpDelete("{id}/Admin")]
		public async Task<ActionResult> DeleteAdmin(int id)
		{
			var result = await _service.DeleteKhachhangAsyncThao(id);
			if (result)
			{
				return Ok(new { message = "Thực hiện thành công." });
			}
			else
			{
				return NotFound(new { message = "Không tìm thấy khách hàng với Id này." });
			}
		}

		// Tìm kiếm theo tên
		[HttpGet("searchByName/Admin")]
		public async Task<ActionResult<IEnumerable<KhachhangDto>>> SearchByName(string name)
		{
			var results = await _service.SearchByNameAsyncThao(name);
			return Ok(results);
		}

		// Tìm kiếm theo số điện thoại
		[HttpGet("searchBySdt/Admin")]
		public async Task<ActionResult<IEnumerable<KhachhangDto>>> SearchBySdt(string sdt)
		{
			var results = await _service.SearchBySdtAsyncThao(sdt);
			return Ok(results);
		}

		// Tìm kiếm theo email
		[HttpGet("searchByEmail/Admin")]
		public async Task<ActionResult<IEnumerable<KhachhangDto>>> SearchByEmail(string email)
		{
			var results = await _service.SearchByEmailAsyncThao(email);
			return Ok(results);
		}
		[HttpGet("searchByEmailSdtTen/Admin")]
		public async Task<ActionResult<IEnumerable<Khachhang>>> SearchByEmailSdtTen(string keyword)
		{
			var results = await _service.SearchByEmailTenSdtAsync(keyword);
			return Ok(results);
		}
	}
}
