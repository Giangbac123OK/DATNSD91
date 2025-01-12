using AppData.Dto;
using AppData.Dto_Admin;
using AppData.IService;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DanhgiaController : Controller
	{
		private readonly IDanhgiaService _danhgiaService;

		public DanhgiaController(IDanhgiaService danhgiaService)
		{
			_danhgiaService = danhgiaService;
		}
		[HttpGet("ByProduct/{idsp}/Admin")]
		public async Task<ActionResult<IEnumerable<DanhgiaDto>>> GetDanhgiaByProductId(int idsp)
		{
			var result = await _danhgiaService.GetDanhgiaByProductIdAsync(idsp);
			return Ok(result);
		}
		[HttpGet("ByProductDetail/{idspct}/Admin")]
		public async Task<ActionResult<IEnumerable<DanhgiaDto>>> GetDanhgiaByProductDetailId(int idspct)
		{
			var result = await _danhgiaService.GetDanhgiaByProductDetailIdAsync(idspct);
			return Ok(result);
		}
	}
}
