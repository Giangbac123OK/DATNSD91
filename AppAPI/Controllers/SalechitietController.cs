
using AppData.IService;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class SalechitietController : Controller
	{
        private readonly KhachHang_ISalechitietService _KhachHang_service;
		private readonly ISalechitietService _service;

		public SalechitietController(KhachHang_ISalechitietService service, ISalechitietService salechitietService)
        {
            _KhachHang_service = service;
			_service = salechitietService;
		}

        // API để lấy tất cả hoá đơn
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var hoadonList = await _KhachHang_service.GetAllAsync();
            return Ok(hoadonList);
        }

        // API để lấy hoá đơn theo Id
        [HttpGet("_KhachHang/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var hoadon = await _KhachHang_service.GetByIdAsync(id);
            if (hoadon == null) return NotFound(new { message = "Sale không tìm thấy" });
            return Ok(hoadon);
        }

        // API để lấy hoá đơn theo Id
        [HttpGet("_KhachHang/SanPhamCT/{id}")]
        public async Task<IActionResult> GetByIdSPCT(int id)
        {
            var hoadon = await _KhachHang_service.GetByIdAsyncSpct(id);
            if (hoadon == null) return NotFound(new { message = "Idspct không tìm thấy" });
            return Ok(hoadon);
        }

        // API để thêm hoá đơn
        [HttpPost]
        public async Task<IActionResult> Add(AppData.Dto.SalechitietDTO dto)
        {
            // Kiểm tra tính hợp lệ của dữ liệu
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Trả về lỗi nếu DTO không hợp lệ
            }

            try
            {
                // Thêm hóa đơn
                await _KhachHang_service.AddAsync(dto);

                // Trả về ID của hóa đơn mới được tạo
                return CreatedAtAction(nameof(GetById), new { id = dto.Idsale }, dto);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có khi thêm hoá đơn
                return StatusCode(500, new { message = "Lỗi khi thêm hoá đơn", error = ex.Message });
            }
        }

        // API để cập nhật hoá đơn
        [HttpPut("_KhachHang/{id}")]
        public async Task<IActionResult> Update(int id, AppData.Dto.SalechitietDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Trả về lỗi nếu DTO không hợp lệ
            }

            var existingHoadon = await _KhachHang_service.GetByIdAsync(id);
            if (existingHoadon == null)
            {
                return NotFound(new { message = "Hoá đơn không tìm thấy" });
            }

            try
            {
                await _KhachHang_service.UpdateAsync(dto, id);
                return NoContent(); // Trả về status code 204 nếu cập nhật thành công
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có khi cập nhật hoá đơn
                return StatusCode(500, new { message = "Lỗi khi cập nhật hoá đơn", error = ex.Message });
            }
        }

        // API để xóa hoá đơn
        [HttpDelete("_KhachHang/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingHoadon = await _KhachHang_service.GetByIdAsync(id);
            if (existingHoadon == null)
            {
                return NotFound(new { message = "Hoá đơn không tìm thấy" });
            }

            try
            {
                await _KhachHang_service.DeleteAsync(id);
                return NoContent(); // Trả về status code 204 nếu xóa thành công
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có khi xóa hoá đơn
                return StatusCode(500, new { message = "Lỗi khi xóa hoá đơn", error = ex.Message });
            }
        }

		[HttpGet("Admin")]
		public async Task<ActionResult<IEnumerable<AppData.Dto_Admin.SalechitietDTO>>> GetSalechitiets()
		{
			var result = await _service.GetAllAsync();
			return Ok(result);
		}

		// GET: api/Salechitiet/5
		[HttpGet("{id}/Admin")]
		public async Task<ActionResult<AppData.Dto_Admin.SalechitietDTO>> GetSalechitiet(int id)
		{
			var result = await _service.GetByIdAsync(id);
			if (result == null)
			{
				return NotFound();
			}
			return Ok(result);
		}

		[HttpPost("Admin")]
		public async Task<ActionResult> CreateSalechitiet([FromBody] List<AppData.Dto_Admin.SalechitietDTO> salechitietDTOs)
		{
			if (salechitietDTOs == null || !salechitietDTOs.Any())
			{
				return BadRequest("Dữ liệu chi tiết sale không hợp lệ.");
			}

			foreach (var salechitietDTO in salechitietDTOs)
			{
				await _service.CreateAsync(salechitietDTO); // Thêm từng đối tượng vào database
			}

			return Ok("Thêm chi tiết sale thành công.");
		}


		// PUT: api/Salechitiet/5
		[HttpPut("{id}/Admin")]
		public async Task<IActionResult> UpdateSalechitiet(int id, AppData.Dto_Admin.SalechitietDTO salechitietDTO)
		{
			if (id != salechitietDTO.Id)
			{
				return BadRequest();
			}

			await _service.UpdateAsync(salechitietDTO);
			return NoContent();
		}

		// DELETE: api/Salechitiet/5
		[HttpDelete("{id}/Admin")]
		public async Task<IActionResult> DeleteSalechitiet(int id)
		{
			await _service.DeleteAsync(id);
			return NoContent();
		}
	}
}
