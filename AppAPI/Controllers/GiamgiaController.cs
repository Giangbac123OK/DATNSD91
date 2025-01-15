using AppData;
using AppData.IService;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class GiamgiaController : Controller
	{
		private readonly KhachHang_IGiamgiaService _KhachHang_Service;
		private readonly MyDbContext _context;

		private readonly IGiamgiaService _service;
		private readonly ILogger<GiamgiaController> _logger;
		public GiamgiaController(KhachHang_IGiamgiaService service, IGiamgiaService service1, MyDbContext context, ILogger<GiamgiaController> logger)
		{
			_logger = logger;
			_KhachHang_Service = service;
			_service = service1;
			_context = context;

		}
		[HttpPut("update-trangthai/Admin")]
		public async Task<IActionResult> StartUpdatingTrangthai()
		{
			// Gọi phương thức để bắt đầu cập nhật trạng thái liên tục
			await _service.UpdateTrangthaiContinuouslyAsync();

			return Ok("Đang cập nhật trạng thái...");
		}

		[HttpGet("Admin")]
		public async Task<IActionResult> GetAll()
		{
			var result = await _service.GetAllAsync();
			return Ok(result.Select(gg => new
			{
				gg.Id,
				gg.Mota,
				gg.Donvi,
				gg.Soluong,
				gg.Giatri,
				gg.Ngaybatdau,
				gg.Ngayketthuc,
				gg.Trangthai,
			}));
		}
		[HttpGet("{id}/Ranks/Admin")]
		public IActionResult GetRanksByVoucherId(int id)
		{
			var ranks = _context.giamgia_Ranks
			.Where(gr => gr.IDgiamgia == id)
								.Join(_context.ranks, gr => gr.Idrank, r => r.Id,
									  (gr, r) => new { r.Tenrank })
								.Select(x => x.Tenrank)
								.ToList();

			if (ranks == null || !ranks.Any())
				return NotFound("Không tìm thấy rank cho voucher này");

			return Ok(ranks);
		}

		[HttpGet("{id}/Admin")]
		public async Task<IActionResult> GetById(int id)
		{
			try
			{
				var giamgia = await _service.GetByIdAsync(id);
				return Ok(new
				{
					giamgia.Mota,
					giamgia.Donvi,
					giamgia.Giatri,
					giamgia.Soluong,
					giamgia.Ngaybatdau,
					giamgia.Ngayketthuc,
					giamgia.Trangthai
				});
			}
			catch (KeyNotFoundException)
			{
				return NotFound("Không tìm thấy mã giảm giá.");
			}
		}
		[HttpGet("vouchers-by-customer/{customerId}/Admin")]
		public async Task<ActionResult<IEnumerable<AppData.Dto_Admin.GiamgiaDTO>>> GetVouchersByCustomerId(int customerId)
		{
			var vouchers = await _service.GetVouchersByCustomerIdAsync(customerId);
			if (!vouchers.Any())
				return NotFound("Không tìm thấy voucher phù hợp.");
			return Ok(vouchers);
		}


		[HttpPost("Admin")]
		public async Task<IActionResult> Create(AppData.Dto_Admin.GiamgiaDTO dto)
		{
			await _service.AddAsync(dto);
			return CreatedAtAction(nameof(GetById), new { id = dto.Mota }, dto);
		}
		[HttpPost("AddRankToGiamgia/Admin")]
		public async Task<IActionResult> AddRankToGiamgia([FromBody] AppData.Dto_Admin.Giamgia_RankDTO dto)
		{
			if (dto == null)
			{
				return BadRequest("Dữ liệu không hợp lệ.");
			}

			try
			{
				await _service.AddRankToGiamgia(dto);
				return Ok(new { message = "Thêm rank thành công." });
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message); // Trả về lỗi nếu validation không hợp lệ
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "Đã có lỗi xảy ra.", details = ex.Message });
			}
		}

		[HttpPut("{id}/Admin")]
		public async Task<IActionResult> UpdateGiamgiaRank(int id, [FromBody] AppData.Dto_Admin.Giamgia_RankDTO dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				await _service.UpdateGiamgiaRankAsync(id, dto);
				return NoContent();
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { message = ex.Message });
			}
			catch (Exception ex)
			{
				// Log hoặc ghi lại lỗi chi tiết để kiểm tra
				_logger.LogError(ex, "Lỗi xảy ra khi cập nhật giảm giá với ID: {id}", id);

				return StatusCode(500, new
				{
					message = "Đã xảy ra lỗi khi cập nhật giảm giá.",
					detail = ex.InnerException?.Message ?? ex.Message // Hiển thị lỗi chi tiết hơn
				});
			}

		}

		[HttpDelete("{id}/Admin")]
		public async Task<IActionResult> DeleteGiamgia(int id)
		{
			// Call service to delete the Giamgia
			var result = await _service.DeleteGiamgiaAsync(id);

			if (!result)
			{

				return BadRequest("Không thể xóa voucher vì nó đã được sử dụng");
			}

			return NoContent();
		}


		[HttpPut("change-status/{id}/Admin")]
		public async Task<IActionResult> ChangeTrangthai(int id)
		{
			try
			{
				await _service.ChangeTrangthaiAsync(id);
				return Ok("Trạng thái giảm giá đã được cập nhật");
			}
			catch (KeyNotFoundException)
			{
				return NotFound("Giảm giá không tồn tại");
			}
		}
		[HttpGet("search/Admin")]
		public async Task<IActionResult> SearchGiamgia([FromQuery] string description)
		{


			var result = await _service.SearchByDescriptionAsync(description);

			if (result == null || result.Count == 0)
			{
				return NotFound("Không tìm thấy giảm giá với mô tả này");
			}

			return Ok(result);
		}
		[HttpGet]
		public async Task<IActionResult> GetAllKH()
		{
			var result = await _KhachHang_Service.GetAllAsync();
			return Ok(result.Select(gg => new
			{
				gg.Id,
				gg.Mota,
				Donvi = gg.Donvi == 0 ? "VND" : "%",
				gg.Giatri,
				gg.Ngaybatdau,
				gg.Ngayketthuc,
				gg.Soluong,
				Trangthai = gg.Trangthai switch
				{
					0 => "Đang phát hành",
					1 => "Chuẩn bị phát hành",
					2 => "Dừng phát hành",
					_ => "Không xác định"
				}
			}));
		}

		[HttpGet("_KhachHang/{id}")]
		public async Task<IActionResult> GetByIdKh(int id)
		{
			try
			{
				var giamgia = await _KhachHang_Service.GetByIdAsync(id);
				return Ok(new
                {
                    giamgia.Id,
                    giamgia.Mota,
					Donvi = giamgia.Donvi == 0 ? "VND" : "%",
					giamgia.Giatri,
					giamgia.Ngaybatdau,
					giamgia.Ngayketthuc,
                    giamgia.Soluong,
                    Trangthai = giamgia.Trangthai switch
					{
						0 => "Đang phát hành",
						1 => "Chuẩn bị phát hành",
						2 => "Dừng phát hành",
						_ => "Không xác định"
					}
				});
			}
			catch (KeyNotFoundException)
			{
				return NotFound("Không tìm thấy mã giảm giá.");
			}
		}

		[HttpPost]
		public async Task<IActionResult> CreateKh(AppData.Dto.GiamgiaDTO dto)
		{
			await _KhachHang_Service.AddAsync(dto);
			return CreatedAtAction(nameof(GetById), new { id = dto.Mota }, dto);
		}
		[HttpPost("_KhachHang/AddRankToGiamgia")]
		public async Task<IActionResult> AddRankToGiamgia([FromBody] AppData.Dto.Giamgia_RankDTO dto)
		{
			try
			{
				await _KhachHang_Service.AddRankToGiamgia(dto);
				return Ok("Rank added to Giảm Giá thành công.");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPut("_KhachHang/{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] AppData.Dto.GiamgiaDTO dto)
		{
		await _KhachHang_Service.UpdateAsync(id, dto);
			return NoContent();
		}

		[HttpDelete("_KhachHang/{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			await _KhachHang_Service.DeleteAsync(id);
			return NoContent();
		}
	}
}
