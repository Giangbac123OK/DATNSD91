
using AppData.Dto_Admin;
using AppData.IService;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ThuoctinhController : ControllerBase
    {
		private readonly KhachHang_IThuoctinhService _KhachHang_service;
		private readonly IThuoctinhService _service;

		public ThuoctinhController(KhachHang_IThuoctinhService service, IThuoctinhService service1)
		{
			_KhachHang_service = service;
			_service = service1;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<AppData.Dto.ThuoctinhDTO>>> GetAll()
		{
			var result = await _KhachHang_service.GetAllAsync();
			return Ok(result);
		}

		[HttpGet("_KhachHang/{id}")]
		public async Task<ActionResult<AppData.Dto.ThuoctinhDTO>> GetById(int id)
		{
			var result = await _KhachHang_service.GetByIdAsync(id);
			if (result == null) return NotFound();
			return Ok(result);
		}

		[HttpPost]
		public async Task<ActionResult<AppData.Dto.ThuoctinhDTO>> Create(AppData.Dto.ThuoctinhDTO dto)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var result = await _KhachHang_service.AddAsync(dto);
			return CreatedAtAction(nameof(GetById), new { id = result.Tenthuoctinh }, result);
		}

		[HttpPut("_KhachHang/{id}")]
		public async Task<ActionResult<AppData.Dto.ThuoctinhDTO>> Update(int id, AppData.Dto.ThuoctinhDTO dto)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var result = await _KhachHang_service.UpdateAsync(id, dto);
			if (result == null) return NotFound();

			return Ok(result);
		}

		[HttpDelete("_KhachHang/{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var result = await _KhachHang_service.DeleteAsync(id);
			if (!result) return NotFound();

			return NoContent();
		}
		[HttpGet("_KhachHang/GetThuocTinh/thuocTinhChiTiet")]
		public async Task<IActionResult> GetThuocTinhsChiTiet()
		{
			var resurl = await _KhachHang_service.GetThuocTinhsChiTiet();
			return Ok(resurl);
		}

		[HttpGet("Admin")]
		public async Task<ActionResult<IEnumerable<Thuoctinh>>> GetAllAdmin()
		{

			var result = await _service.GetAll();
			return Ok(result.Select(tt => new
			{
				tt.Id,
				tt.Tenthuoctinh
			}));
		}

		[HttpGet("{id}/Admin")]
		public async Task<ActionResult<Thuoctinh>> GetByIdAdmin(int id)
		{
			try
			{
				var thuoctinh = await _service.GetById(id);
				return Ok(new
				{
					thuoctinh.Id,
					thuoctinh.Tenthuoctinh
				});

			}
			catch (KeyNotFoundException)
			{
				return NotFound("Nhân viên không tồn tại.");
			}


		}

		[HttpPost("Admin")]
		public async Task<ActionResult> Add([FromBody] ThuoctinhDto thuoctinhDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			await _service.Add(thuoctinhDto);
			return CreatedAtAction(nameof(GetById), new { id = thuoctinhDto.Tenthuoctinh }, thuoctinhDto);
		}

		[HttpPut("{id}/Admin")]
		public async Task<ActionResult> Update(int id, [FromBody] ThuoctinhDto thuoctinhDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			await _service.Update(id, thuoctinhDto);
			return NoContent();
		}

		[HttpDelete("{id}/Admin")]
		public async Task<ActionResult> DeleteAdmin(int id)
		{
			await _service.Delete(id);
			return NoContent();
		}
	}
}
