
using AppData.Dto_Admin;
using AppData.IService;
using AppData.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class Sanphamcontroller : Controller
	{
		private readonly KhachHang_ISanPhamservice _KhachHang_service;
		private readonly ISanPhamservice _service;
		public Sanphamcontroller(KhachHang_ISanPhamservice service, ISanPhamservice service1)
        {
			_KhachHang_service = service;
            _service = service1;

		}
		[HttpGet]
		public async Task<IActionResult> GetAll() => Ok(await _KhachHang_service.GetAllAsync());

		[HttpGet("_KhachHang/{id}")]
		public async Task<IActionResult> GetById(int id)
		{
            var sanpham = await _KhachHang_service.GetByIdAsync(id);
            return sanpham != null ? Ok(sanpham) : NotFound();
        }

		[HttpPost]
		public async Task<IActionResult> Add(AppData.Dto.SanphamDTO sanphamDto)
		{
			await _KhachHang_service.AddAsync(sanphamDto);
			return CreatedAtAction(nameof(GetById), new { id = sanphamDto.Idth }, sanphamDto);
		}

		[HttpPut("_KhachHang/{id}")]
		public async Task<IActionResult> Update(int id, AppData.Dto.SanphamDTO sanphamDto)
		{
			await _KhachHang_service.UpdateAsync(id, sanphamDto);
			return NoContent();
		}

        [HttpPut("_KhachHang/{id}/cancel")]
		public async Task<IActionResult> UpdateStatusToCancelled(int id)
		{
			try
			{
				await _KhachHang_service.UpdateStatusToCancelled(id);
				return NoContent(); // Thành công mà không cần trả về nội dung
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
		}

		[HttpPut("_KhachHang/{id}/update-status-load")]
		public async Task<IActionResult> UpdateStatusload(int id)
		{
			try
			{
				await _KhachHang_service.UpdateStatusLoad(id);
				return NoContent();
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
		}

		[HttpDelete("_KhachHang/{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			await _KhachHang_service.DeleteAsync(id);
			return NoContent();
		}

		[HttpGet("_KhachHang/search")]
		public async Task<IActionResult> SearchByName(string name) => Ok(await _KhachHang_service.SearchByNameAsync(name));

        [HttpGet("_KhachHang/GetALLSanPham")]
        public async Task<IActionResult> GetAllSanphams()
        {
            try
            {
                var sanphamViewModels = await _KhachHang_service.GetAllSanphamViewModels();
                return Ok(sanphamViewModels);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ chung nếu có lỗi khác
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("_KhachHang/GetALLSanPham/{id}")]
        public async Task<IActionResult> GetAllSanphamsByIdSP(int id)
        {
            try
            {
                var sanphamViewModels = await _KhachHang_service.GetAllSanphamViewModelsByIdSP(id);
                return Ok(sanphamViewModels);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("_KhachHang/GetALLSanPhamGiamGia")]
        public async Task<IActionResult> GetAllSanphamsGiamGia()
        {
            try
            {
                var sanphamViewModels = await _KhachHang_service.GetAllSanphamGiamGiaViewModels();
                return Ok(sanphamViewModels);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ chung nếu có lỗi khác
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("_KhachHang/GetALLSanPhamByThuongHieu/{id}")]
        public async Task<IActionResult> GetAllSanphamsByThuongHieu(int id)
        {
            try
            {
                var sanphamViewModels = await _KhachHang_service.GetAllSanphamByThuongHieu(id);
                return Ok(sanphamViewModels);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("_KhachHang/SanPhamChiTiet/search")]
        public async Task<IActionResult> SearchSanphams(
             [FromQuery] List<string> tenThuocTinhs,
            [FromQuery] decimal? giaMin = null,
             [FromQuery] decimal? giaMax = null,
            [FromQuery] int? idThuongHieu = null)
            {
            try
            {
                tenThuocTinhs ??= new List<string>();

                 var sanphams = await _KhachHang_service.GetSanphamByThuocTinh(tenThuocTinhs, giaMin, giaMax, idThuongHieu);
                if (sanphams == null || !sanphams.Any())
                {
                    return NotFound(new { message = "Không tìm thấy sản phẩm nào thỏa mãn tiêu chí. thanh" });
                }

                return Ok(sanphams);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra", error = ex.Message });
            }
        }
		[HttpGet("Admin")]
		public async Task<IActionResult> GetAllAdmin() => Ok(await _service.GetAllAsync());

		[HttpGet("{id}/Admin")]
		public async Task<IActionResult> GetByIdAdmin(int id)
		{
			var sanpham = await _service.GetByIdAsync(id);
			return sanpham != null ? Ok(sanpham) : NotFound();
		}
		[HttpGet("active-products-with-attributes/Admin")]
		public async Task<IActionResult> GetActiveProductsWithAttributes(int id)
		{
			var products = await _service.GetAllActiveProductsWithAttributesAsync(id);
			return Ok(products);
		}
		[HttpPost("Admin")]
		public async Task<IActionResult> Add(SanphamDTO sanphamDto)
		{
			// Kiểm tra tính hợp lệ của model
			if (!ModelState.IsValid)
			{
				var errorMessages = ModelState.Values
								   .SelectMany(v => v.Errors)
								   .Select(e => e.ErrorMessage)
								   .ToList();

				return BadRequest(errorMessages);
			}

			await _service.AddAsync(sanphamDto);
			return CreatedAtAction(nameof(GetById), new { id = sanphamDto.Tensp }, sanphamDto);
		}

		[HttpGet("details/Admin")]
		public async Task<ActionResult<IEnumerable<SanphamDetailDto>>> GetSanphamDetails()
		{
			var sanphamDetails = await _service.GetSanphamDetailsAsync();
			return Ok(sanphamDetails);
		}
		[HttpPut("{id}/add-soluong/Admin")]
		public async Task<IActionResult> AddSoluong(int id, [FromBody] AddSoluongDto addSoluongDto)
		{
			if (!ModelState.IsValid)
			{
				var errorMessage = ModelState.Values
				 .SelectMany(v => v.Errors)
				 .Select(e => e.ErrorMessage)
				 .FirstOrDefault();

				// Trả về BadRequest với thông báo lỗi đơn
				return BadRequest(errorMessage);
			}

			var result = await _service.AddSoluongAsync(id, addSoluongDto.SoluongThem);
			if (!result)
			{
				return NotFound(new { message = $"Sản phẩm với ID {id} không được tìm thấy." });
			}

			return NoContent(); // HTTP 204 No Content
		}
		[HttpPut("{id}/Admin")]
		public async Task<IActionResult> UpdateSanpham(int id, [FromBody] SanphamDTO sanphamDto)
		{
			if (sanphamDto == null)
			{
				return BadRequest("Dữ liệu sản phẩm không được null.");
			}

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var isUpdated = await _service.UpdateAsync(id, sanphamDto);
				if (!isUpdated)
				{
					return NotFound($"Không tìm thấy sản phẩm với ID = {id}.");
				}

				return Ok(new { message = "Cập nhật sản phẩm thành công." });
			}
			catch (Exception ex)
			{
				// Log lỗi nếu cần
				return StatusCode(StatusCodes.Status500InternalServerError,
					$"Đã xảy ra lỗi trong quá trình cập nhật sản phẩm: {ex.Message}");
			}
		}
		[HttpPut("{id}/cancel/Admin")]
		public async Task<IActionResult> UpdateStatusToCancelledAdmin(int id)
		{
			try
			{
				await _service.UpdateStatusToCancelled(id);
				return NoContent(); // Thành công mà không cần trả về nội dung
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
		}

		[HttpPut("{id}/update-status-load/Admin")]
		public async Task<IActionResult> UpdateStatusloadAdmin(int id)
		{
			try
			{
				await _service.UpdateStatusLoad(id);
				return NoContent();
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
		}
		[HttpGet("check-foreign-key/{id}/Admin")]
		public async Task<IActionResult> CheckForeignKey(int id)
		{
			var hasConstraint = await _service.CheckForeignKeyConstraintAsync(id);
			if (hasConstraint)
			{
				return Ok(new { message = "Sản phẩm có ràng buộc khóa ngoại." });
			}

			return Ok(new { message = "Sản phẩm không có ràng buộc khóa ngoại." });
		}

		[HttpDelete("{id}/Admin")]
		public async Task<IActionResult> DeleteAdmin(int id)
		{
			await _service.DeleteAsync(id);
			return NoContent();
		}

		[HttpGet("search/Admin")]
		public async Task<IActionResult> SearchByNameAdmin(string name) => Ok(await _service.SearchByNameAsync(name));
		[HttpGet("searchhd/Admin")]
		public async Task<IActionResult> SearchByNameHd(string name) => Ok(await _service.SearchByNameHdAsync(name));
	}

}

