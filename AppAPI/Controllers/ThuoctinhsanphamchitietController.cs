
using AppData.Dto_Admin;
using AppData.IService;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace AppAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ThuoctinhsanphamchitietController : Controller
	{
		private readonly IthuoctinhsanphamchitietService _service;
		

		public ThuoctinhsanphamchitietController(IthuoctinhsanphamchitietService service)
        {
			_service = service;
			
		}


		[HttpPost("AddThuoctinhToSanphamchitiet/Admin")]
		public async Task<IActionResult> AddThuoctinhToSanphamchitiet([FromBody] AddThuoctinhtoSpct dto)
		{
			if (!ModelState.IsValid)
			{
				var errorMessages = ModelState.Values
											  .SelectMany(v => v.Errors)
											  .Select(e => e.ErrorMessage)
											  .ToList();

				var firstError = errorMessages.FirstOrDefault();

				return BadRequest(new { message = firstError });

			}
			try
			{
				await _service.AddThuoctinhToSanphamchitietAsync(dto);
				var updatedList = await _service.GetAllAsync();

				return Ok(new { message = "Thêm sản phẩm chi tiết và thuộc tính thành công!" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
		[HttpGet("Admin")]
		public async Task<IActionResult> GetAll()
		{
		
				var result = await _service.GetAllAsync();
				return Ok(result);
			


	
		}
		
		[HttpGet("{idtt}/{idspct}/Admin")]
		public async Task<IActionResult> GetById(int idtt, int idspct)
		{
			// Truy vấn dữ liệu chi tiết sản phẩm từ service
			var data = await _service.GetByIdAsync(idtt, idspct);
			if (data == null)
			{
				return NotFound(); // Nếu không tìm thấy, trả về lỗi 404
			}
			return Ok(data); // Trả về dữ liệu tìm thấy
		}
		
		[HttpPost("Admin")]
		public async Task<IActionResult> Create([FromBody] ThuoctinhsanphamchitietDto thuoctinhsanphamchitietDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState); // Nếu dữ liệu không hợp lệ, trả về lỗi 400
			}

			// Gọi service để thêm sản phẩm chi tiết mới
			await _service.AddAsync(thuoctinhsanphamchitietDTO);
			return CreatedAtAction(nameof(GetById), new { idtt = thuoctinhsanphamchitietDTO.Idtt, idspct = thuoctinhsanphamchitietDTO.Idspct }, thuoctinhsanphamchitietDTO);
		}
	
		[HttpPut("{idtt}/{idspct}/Admin")]
		public async Task<IActionResult> Update(int idtt, int idspct, [FromBody] ThuoctinhsanphamchitietDto thuoctinhsanphamchitietDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState); // Nếu dữ liệu không hợp lệ, trả về lỗi 400
			}

			try
			{
				// Gọi service để cập nhật thông tin sản phẩm chi tiết
				await _service.UpdateAsync(thuoctinhsanphamchitietDTO);
				return NoContent(); // Trả về HTTP 204 No Content nếu thành công
			}
			catch (Exception ex)
			{
				return NotFound(new { message = ex.Message }); // Nếu có lỗi (ví dụ không tìm thấy sản phẩm), trả về lỗi 404
			}
		}

		[HttpGet("getbyidtt/{idtt}/Admin")]
    public async Task<IActionResult> GetByIdtt(int idtt)
    {
      
        var data = await _service.GetByIdttAsync(idtt);
        
       
        return Ok(data);
		}
		
		[HttpDelete("{idtt}/{idspct}/Admin")]
		public async Task<IActionResult> Delete(int idtt, int idspct)
		{
			try
			{
				// Gọi service để xóa sản phẩm chi tiết
				await _service.DeleteAsync(idtt, idspct);
				return NoContent(); // Trả về HTTP 204 No Content nếu thành công
			}
			catch (Exception ex)
			{
				return NotFound(new { message = ex.Message }); // Nếu có lỗi (ví dụ không tìm thấy sản phẩm), trả về lỗi 404
			}
		}
		

	}
}
