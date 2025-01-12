using AppData.IService;
using AppData.Service;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ThongkeController : Controller
	{
		private readonly IThongkeService _service;

        public ThongkeController(IThongkeService service)
        {
			_service = service;

		}

		[HttpGet("top-selling-products/Admin")]
		public async Task<IActionResult> GetTopSellingProductsByTimeAsync([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
		{
			var topSellingProducts = await _service.GetTopSellingProductsByTimeAsync(startDate, endDate);
			return Ok(topSellingProducts);
		}
	}
}
