
using AppData.Dto_Admin;
using AppData.IService;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ThuonghieuController : ControllerBase
    {
		private readonly KhachHang_IThuongHieuService _service;
		private readonly IthuonghieuService _serviceAdmin;
		public ThuonghieuController(KhachHang_IThuongHieuService service, IthuonghieuService serviceAdmin)
		{
			_service = service;
			_serviceAdmin = serviceAdmin;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<AppData.Dto.ThuonghieuDTO>>> GetAll()
		{
			var result = await _service.GetAllAsync();
			return Ok(result);
		}

		[HttpGet("_KhachHang/{id}")]
		public async Task<ActionResult<AppData.Dto.ThuonghieuDTO>> GetById(int id)
		{
			var result = await _service.GetByIdAsync(id);
			if (result == null) return NotFound();
			return Ok(result);
		}

		[HttpPost]
		public async Task<ActionResult<AppData.Dto.ThuonghieuDTO>> Create(AppData.Dto.ThuonghieuDTO dto)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var result = await _service.AddAsync(dto);
			return CreatedAtAction(nameof(GetById), new { id = result.Tenthuonghieu }, result);
		}

		[HttpPut("_KhachHang/{id}")]
		public async Task<ActionResult<AppData.Dto.ThuonghieuDTO>> Update(int id, AppData.Dto.ThuonghieuDTO dto)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var result = await _service.UpdateAsync(id, dto);
			if (result == null) return NotFound();

			return Ok(result);
		}

		[HttpDelete("_KhachHang/{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var result = await _service.DeleteAsync(id);
			if (!result) return NotFound();

			return NoContent();
		}


		[HttpGet("Admin")]
		public async Task<IActionResult> GetAllAdmin()
		{
			var brands = await _serviceAdmin.GetAllAsync();
			return Ok(brands); // Trả về danh sách thương hiệu bao gồm Id và tình trạng
		}

		[HttpGet("{id}/Admin")]
		public async Task<IActionResult> GetByIdAdmin(int id)
		{
			var brand = await _serviceAdmin.GetByIdAsync(id);
			return Ok(brand);
		}

		[HttpPost("Admin")]
		public async Task<IActionResult> Add([FromBody] ThuonghieuDTO thuonghieuDto)
		{
			await _serviceAdmin.AddAsync(thuonghieuDto);
			return CreatedAtAction(nameof(GetById), new { id = thuonghieuDto.Tenthuonghieu }, thuonghieuDto);
		}

		[HttpPut("{id}/Admin")]
		public async Task<IActionResult> Update(int id, [FromBody] ThuonghieuDTO thuonghieuDto)
		{
			await _serviceAdmin.UpdateAsync(id, thuonghieuDto);
			return NoContent();
		}

		[HttpDelete("{id}/Admin")]
		public async Task<IActionResult> XoaThuongHieu(int id)
		{
			var result = await _serviceAdmin.XoaThuongHieu(id);
			if (result)
			{
				return Ok("Thương hiệu đã được xóa hoặc thay đổi tình trạng thành đã xóa.");
			}
			return NotFound("Không tìm thấy thương hiệu.");
		}

		[HttpGet("search/Admin")]
		public async Task<IActionResult> Search(string name)
		{
			var result = await _serviceAdmin.SearchByNameAsync(name);
			return Ok(result);
		}
	}
}
