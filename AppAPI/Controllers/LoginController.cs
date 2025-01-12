using AppData;
using AppData.Dto;
using AppData.IRepository;
using AppData.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoginController : ControllerBase
	{
		private readonly MyDbContext _context;
		private readonly ILogger<LoginController> _logger;
        private readonly KhachHang_IKhachhangRepos _KhachHang_Repos;
        private readonly KhachHang_IGiohangRepos _KhachHang_GHrepos;

        public LoginController(MyDbContext context, ILogger<LoginController> logger, KhachHang_IGiohangRepos GHrepos, KhachHang_IKhachhangRepos repos)
		{
			_context = context;
			_logger = logger;
			_KhachHang_GHrepos = GHrepos;
			_KhachHang_Repos = repos;
		}
		[HttpPost("_KhachHang/register")]
		public async Task<IActionResult> Register([FromBody] RegisterUserDTO dto)
		{
			try
			{
				if (_context.khachhangs.Any(kh => kh.Email == dto.Email))
				{
					return BadRequest("Email đã tồn tại");
				}
                if (_context.khachhangs.Any(kh => kh.Sdt == dto.Sdt))
                {
                    return BadRequest("Số điện thoại đã tồn tại");
                }

                var khachHang = new Khachhang
				{
					Ten = dto.Ten,
					Sdt = dto.Sdt,
					Ngaysinh = dto.Ngaysinh,
					Email = dto.Email,
					Diachi = dto.Diachi,
					Password = BCrypt.Net.BCrypt.HashPassword(dto.Password), // Băm mật khẩu
					Ngaytaotaikhoan = DateTime.UtcNow,
					Tichdiem = 0, // Giá trị mặc định
					Diemsudung = 0,
					Trangthai = 0,
					Idrank = 1 // Rank mặc định
				};

                await _KhachHang_Repos.AddAsync(khachHang);

                var gh = new Giohang()
                {
                    Soluong = 0,
                    Idkh = khachHang.Id
                };
                await _KhachHang_GHrepos.AddAsync(gh);

                return Ok("Đăng ký thành công");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error during registration");
				return StatusCode(500, "Đã xảy ra lỗi trong quá trình đăng ký");
			}
			// Kiểm tra xem email đã tồn tại chưa


		}

        [HttpPost("_KhachHang/checkemail")]
        public IActionResult CheckEmail(string checkEmail)
        {
            var khachHang = _context.khachhangs.FirstOrDefault(x => x.Email == checkEmail);

            if (khachHang == null)
            {
                return Ok(new { status = "not_found", message = "Email không tồn tại." });
            }
            else if (khachHang.Password == null)
            {
                return Ok(new { status = "new_account", message = "Tài khoản chưa có mật khẩu." });
            }
            else
            {
                return Ok(new { status = "password_required", message = "Vui lòng nhập mật khẩu." });
            }
        }

        [HttpPost]
        [Route("RegisterPassword")]
        public IActionResult RegisterPassword([FromBody] LoginUserDTO request)
        {
            var user = _context.khachhangs.FirstOrDefault(x => x.Email == request.Email);
            if (user == null)
            {
                return BadRequest("Tài khoản không tồn tại.");
            }

            if (!string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Tài khoản đã có mật khẩu.");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            _context.khachhangs.Update(user);
			_context.SaveChanges();

            return Ok(new { message = "Mật khẩu đã được đăng ký thành công." });
        }

        [HttpPost]
        [Route("RegisterPassword2")]
        public IActionResult RegisterPassword2([FromBody] LoginUserDTO request)
        {
            var user = _context.khachhangs.FirstOrDefault(x => x.Email == request.Email);
            if (user == null)
            {
                return BadRequest("Tài khoản không tồn tại.");
            }

            if (!string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Tài khoản đã có mật khẩu.");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            _context.khachhangs.Update(user);
            _context.SaveChanges();

            var khachhang = _context.khachhangs.FirstOrDefault(x => x.Id == user.Id);
            if (khachhang == null)
            {
                var giohang = new Giohang()
                {
                    Soluong = 0,
                    id = user.Id,
                };
                _context.giohangs.Add(giohang);
                _context.SaveChanges();
            }
            return Ok(new { message = "Mật khẩu đã được đăng ký thành công." });
        }


        [HttpPost("_KhachHang/login")]
		public async Task<IActionResult> Login([FromBody] LoginUserDTO dto)
		{
			try
			{
				_logger.LogInformation($"Login attempt for email: {dto.Email}");
				if (string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password))
				{
					_logger.LogWarning("Email or password is empty");
					return BadRequest("Email và mật khẩu không được để trống");
				}
				var khachHang = await _context.khachhangs
					.FirstOrDefaultAsync(kh => kh.Email == dto.Email);
				if (khachHang == null)
				{
					_logger.LogWarning($"Account not found for email: {dto.Email}");
					return NotFound("Tài khoản không tồn tại");
                 
                }
				bool passwordValid = BCrypt.Net.BCrypt.Verify(dto.Password, khachHang.Password);
				_logger.LogInformation($"Password validation result: {passwordValid}");
				if (!passwordValid)
				{
					_logger.LogWarning($"Invalid password for email: {dto.Email}");
					return Unauthorized("Mật khẩu không đúng");
				}
				_logger.LogInformation($"Login successful for email: {dto.Email}");
				return Ok(new
				{
					Message = "Đăng nhập thành công",
					KhachHangId = khachHang.Id,
					trangthai = khachHang.Trangthai,
					Ten = khachHang.Ten,
					Email = khachHang.Email,
					Ngaytaotaikhoan = khachHang.Ngaytaotaikhoan
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error during login process");
				return StatusCode(500, "Đã xảy ra lỗi trong quá trình đăng nhập");
			}
		}

	}

}

