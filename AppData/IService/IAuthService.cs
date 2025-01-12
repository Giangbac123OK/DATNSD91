using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto_Admin;
using AppData.Models;

namespace AppData.IService
{
	public interface IAuthService
	{
		Task<Nhanvien> AuthenticateWithGoogleAsync(OAuthGoogleDTO oauthGoogleDTO);
	}
}
