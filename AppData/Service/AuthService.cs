using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto;
using AppData.Dto_Admin;
using AppData.IRepository;
using AppData.IService;
using AppData.Models;

namespace AppData.Service
{
	public class AuthService: IAuthService
	{
		private readonly INhanvienRepos _repository;

		public AuthService(INhanvienRepos repository)
		{
			_repository = repository;
		}

		public async Task<Nhanvien> AuthenticateWithGoogleAsync(OAuthGoogleDTO oauthGoogleDTO)
		{
			// Tìm nhân viên qua email
			var nhanvien = await _repository.GetNhanvienByEmailAsync(oauthGoogleDTO.Email);

			if (nhanvien == null)
			{
				throw new UnauthorizedAccessException("Nhân viên không tồn tại hoặc không có quyền truy cập.");
			}

			// Nếu tìm thấy nhân viên, trả về
			return nhanvien;
		}
	}
}
