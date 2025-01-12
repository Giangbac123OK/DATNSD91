
using AppData.Dto;
using AppData.Dto_Admin;
using AppData.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RankController : ControllerBase
	{
		private readonly KhachHang_IRankServiece _KhachHang_service;
		private readonly IRankService _rankService;

		public RankController(KhachHang_IRankServiece service, IRankService rankService)
		{
			_KhachHang_service = service;
			_rankService = rankService;
		}
		[HttpGet("_KhachHang/{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var x = await _KhachHang_service.GetRankByIdAsync(id);
			if (x == null) return NotFound();

			return Ok(new
			{
				x.TenRank,
				x.MaxMoney,
				x.MinMoney,
				Trangthai = x.trangthai == 0 ? "Đang hoạt động" : "Dừng hoạt động"
			});
		}
		[HttpPost]
		public async Task<IActionResult> Add([FromBody] RankDTO rankDTO)
		{


			try
			{
				await _KhachHang_service.AddRankDTOAsync(rankDTO);
				return CreatedAtAction(nameof(GetById), new { id = rankDTO.TenRank }, rankDTO); // Trả về trạng thái 201 Created
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "Có lỗi xảy ra: " + ex.Message }); // Trả về thông điệp lỗi chung
			}
		}

		// Cập nhật nhà cung cấp theo ID
		[HttpPut("_KhachHang/{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] RankDTO rankDTO)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState); // Trả về lỗi nếu model không hợp lệ

			try
			{
				await _KhachHang_service.UpdateRankAsync(id, rankDTO);
				return NoContent(); // Trả về trạng thái 204 No Content nếu cập nhật thành công
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "Có lỗi xảy ra: " + ex.Message }); // Trả về thông điệp lỗi chung
			}
		}

		// Xóa nhà cung cấp theo ID
		[HttpDelete("_KhachHang/{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await _KhachHang_service.DeleteRankAsync(id);
				return NoContent(); // Trả về trạng thái 204 No Content nếu xóa thành công
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "Có lỗi xảy ra: " + ex.Message }); // Trả về thông điệp lỗi chung
			}
		}

		// Lấy tất cả nhà cung cấp
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var result = await _KhachHang_service.GetAllRanksAsync();
			return Ok(result);
		}

		[HttpPost("Admin")]
		public async Task<IActionResult> AddRank([FromBody] AppData.Dto_Admin.RankDto rankDto)
		{
			// Kiểm tra tính hợp lệ của ModelState
			if (!ModelState.IsValid)
			{
				// Trả về tất cả lỗi từ ModelState
				var errors = ModelState
					.Where(ms => ms.Value.Errors.Any())  // Chỉ lấy các trường có lỗi
					.ToDictionary(
						kv => kv.Key,  // Tên trường
						kv => kv.Value.Errors.Select(e => e.ErrorMessage).ToList()  // Danh sách thông báo lỗi
					);

				return BadRequest(errors);
			}

			// Nếu không có lỗi, tiếp tục xử lý
			await _rankService.AddRankAsync(rankDto);
			return CreatedAtAction(nameof(GetRankById), new { Id = rankDto.Tenrank }, rankDto);
		}

		// Lấy tất cả Rank
		[HttpGet("Admin")]
		public async Task<IActionResult> GetAllRanks()
		{
			var ranks = await _rankService.GetAllRanksAsync();
			Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(ranks));

			return Ok(ranks);
		}

		// Lấy Rank theo ID
		[HttpGet("{id}/Admin")]
		public async Task<IActionResult> GetRankById(int id)
		{
			var rank = await _rankService.GetRankByIdAsync(id);
			if (rank == null)
			{
				return NotFound();
			}

			return Ok(rank);
		}

		// Cập nhật Rank
		[HttpPut("{id}/Admin")]
		public async Task<IActionResult> UpdateRank(int id, [FromBody] RankDto rankDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			await _rankService.UpdateRankAsync(id, rankDto);
			return NoContent();
		}

		[HttpDelete("{id}/Admin")]
		public async Task<ActionResult> DeleteAdmin(int id)
		{
			var success = await _rankService.DeleteRankAsyncThao(id);
			if (!success)
			{
				return BadRequest("Không thể xóa rank vì đã có dữ liệu liên quan.");
			}

			return Ok("Xóa khách hàng thành công.");
		}
		[HttpPut("Toggle/Admin")]
		public async Task<IActionResult> ToggleTrangthaiAsync(int id)  // Thêm async
		{


			try
			{
				await _rankService.ToggleTrangthaiAsync(id);
				return Ok(new { Message = "Cập nhật trạng thái thành công." });
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = ex.Message });
			}
		}


		// Tìm kiếm Rank
		[HttpGet("search/Admin")]
		public async Task<IActionResult> SearchRanks([FromQuery] string keyword)
		{
			var ranks = await _rankService.SearchRanksAsync(keyword);
			return Ok(ranks);
		}
	}
}
