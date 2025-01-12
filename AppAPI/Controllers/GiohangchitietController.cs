using AppData.Dto;
using AppData.IService;
using AppData.Service;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiohangchitietController : ControllerBase
    {
        private readonly KhachHang_IGiohangchitietService _KhachHang_Service;
        public GiohangchitietController(KhachHang_IGiohangchitietService ser)
        {
            _KhachHang_Service = ser;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var a = await _KhachHang_Service.GetAllGiohangsAsync();
            return Ok(a);
        }
        [HttpGet("_KhachHang/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var a = await _KhachHang_Service.GetGiohangByIdAsync(id);
                return Ok(a);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Giỏ hàng chi tiết không tồn tại.");
            }
        }

        [HttpGet("_KhachHang/idghctbygiohangangspct/{idgh}/{idspct}")]
        public async Task<IActionResult> GetByIdspctToGiohangAsync(int idgh, int idspct)
        {
            try
            {
                var result = await _KhachHang_Service.GetByIdspctToGiohangAsync(idgh, idspct);

                if (result == null)
                {
                    return NotFound("Giỏ hàng chi tiết không tồn tại.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (ex) if needed
                return StatusCode(500, "Đã xảy ra lỗi khi xử lý yêu cầu.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(GiohangchitietDTO gh)
        {
            try
            {
                var result = await _KhachHang_Service.GetByIdspctToGiohangAsync(gh.Idgh, gh.Idspct);

                if (result == null)
                {
                    await _KhachHang_Service.AddGiohangAsync(gh);
                    return Ok("Thêm thành công!");
                }
                else
                {
                    _KhachHang_Service.UpdateSoLuongGiohangAsync(result.Id, gh);
                    return Ok("Sản phẩm đã tồn tại trong giỏ hàng, cập nhật thành công!");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Đã xảy ra lỗi: " + ex.Message });
            }
        }

        [HttpGet("_KhachHang/giohangchitietbygiohang/{id}")]
        public async Task<IActionResult> GetDiaChiByIdKH(int id)
        {
            try
            {
                var diachiDto = await _KhachHang_Service.GetGHCTByIdGH(id);


                return Ok(diachiDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Đã xảy ra lỗi: " + ex.Message });
            }
        }

        [HttpPut("_KhachHang/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] GiohangchitietDTO dto)
        {
            try
            {
                await _KhachHang_Service.UpdateGiohangAsync(id, dto);
                return Ok(dto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Giỏ hàng chi tiết không tồn tại.");
            }
        }

        [HttpPut("_KhachHang/sanpham/{id}")]
        public async Task<IActionResult> Updatesanpham(int id, [FromBody] GiohangchitietDTO dto)
        {
            try
            {
                var result = await _KhachHang_Service.GetByIdspctToGiohangAsync(dto.Idgh, dto.Idspct);

                if (result == null)
                {
                    await _KhachHang_Service.UpdateGiohangAsync(id, dto);
                    return Ok("Cập Nhật Thành Công!");
                }
                else
                {
                    await _KhachHang_Service.DeleteGiohangAsync(id);
                    _KhachHang_Service.UpdateSoLuongGiohangAsync(result.Id, dto);
                    return Ok("Sản phẩm đã tồn tại trong giỏ hàng, cập nhật thành công!");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Đã xảy ra lỗi: " + ex.Message });
            }
        }
        [HttpDelete("_KhachHang/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _KhachHang_Service.DeleteGiohangAsync(id);
                return Ok("Xoá giỏ hàng chi tiết thành công."); // Trả về thông báo khi xoá thành công
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Giỏ hàng chi tiết không tồn tại."); // Trả về lỗi nếu không tìm thấy
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Đã xảy ra lỗi: {ex.Message}"); // Xử lý lỗi không mong muốn
            }
        }
    }
}

