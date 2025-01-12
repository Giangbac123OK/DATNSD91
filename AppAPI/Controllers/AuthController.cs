using AppData.Dto;
using AppData.IService;
using AppData.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using AppData.Dto_Admin;

namespace AppAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : Controller
	{
		private readonly IAuthService _authService;
		private readonly IConfiguration _configuration;

		public AuthController(IAuthService authService, IConfiguration configuration)
		{
			_authService = authService;
			_configuration = configuration;
		}
		[HttpPost("google-login")]
		public async Task<IActionResult> GoogleLogin([FromBody] OAuthGoogleDTO oauthGoogleDTO)
		{
			try
			{
				// Xác thực nhân viên từ Google email
				var nhanvien = await _authService.AuthenticateWithGoogleAsync(oauthGoogleDTO);

				// Tạo token JWT cho người dùng
				var token = GenerateJwtToken(nhanvien);

				return Ok(new { token });
			}
			catch (UnauthorizedAccessException ex)
			{
				return Unauthorized(new { message = ex.Message });
			}
		}

		private string GenerateJwtToken(Nhanvien nhanvien)
		{
			var claims = new[]
			{
			new Claim(ClaimTypes.Name, nhanvien.Hoten),
			new Claim(ClaimTypes.Email, nhanvien.Email),
			new Claim(ClaimTypes.Role, nhanvien.Role.ToString()),
		};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _configuration["Jwt:Issuer"],
				audience: _configuration["Jwt:Audience"],
				claims: claims,
				expires: DateTime.Now.AddMinutes(30),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

	}
}
