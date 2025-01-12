using AppData.Dto;
using AppData.IService;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class giamgia_rankController : Controller
    {
        private readonly KhachHang_IGiamgia_RankService _KhachHang_Service;

        public giamgia_rankController(KhachHang_IGiamgia_RankService service)
        {
            _KhachHang_Service = service;
        }

        // API để lấy tất cả hoá đơn
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var hoadonList = await _KhachHang_Service.GetAllAsync();
            return Ok(hoadonList);
        }

        // API để lấy hoá đơn theo Id
        [HttpGet("_KhachHang/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var hoadon = await _KhachHang_Service.GetByIdAsync(id);
            if (hoadon == null) return NotFound(new { message = "Sale không tìm thấy" });
            return Ok(hoadon);
        }

        // API để lấy hoá đơn theo Id
        [HttpGet("_KhachHang/rank/{id}")]
        public async Task<IActionResult> GetByIdSPCT(int id)
        {
            var hoadon = await _KhachHang_Service.GetByIdRankSPCTAsync(id);
            if (hoadon == null) return NotFound(new { message = "Idspct không tìm thấy" });
            return Ok(hoadon);
        }

        // API để thêm hoá đơn
        [HttpPost]
        public async Task<IActionResult> Add(Giamgia_RankDTO1 dto)
        {
            // Kiểm tra tính hợp lệ của dữ liệu
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Trả về lỗi nếu DTO không hợp lệ
            }

            try
            {
                // Thêm hóa đơn
                await _KhachHang_Service.AddAsync(dto);

                // Trả về ID của hóa đơn mới được tạo
                return CreatedAtAction(nameof(GetById), new { id = dto.Idrank }, dto);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có khi thêm hoá đơn
                return StatusCode(500, new { message = "Lỗi khi thêm hoá đơn", error = ex.Message });
            }
        }
    }
}
