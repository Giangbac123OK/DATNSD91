using AppData.Dto;
using AppData.IService;
using AppData.Models;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrahangController : ControllerBase
    {
		private readonly KhachHang_ITraHangService _ser;
		private readonly ITrahangService _service;
		public TrahangController(KhachHang_ITraHangService ser, ITrahangService service)
		{
			_ser = ser;
			_service = service;
		}
		[HttpGet("by-hoadon/{hoadonId}/Admin")]
		public async Task<IActionResult> GetTrahangByHoadonId(int hoadonId)
		{
			var tra = await _service.GetTrahangByHoadonIdAsync(hoadonId);
			if (tra == null)
				return NotFound(new { message = "Không tìm thấy yêu cầu trả hàng cho hóa đơn này." });

			return Ok(tra);
		}
		[HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _ser.GetAll());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("_KhachHang/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var a = await _ser.GetById(id);
                if (a == null) return BadRequest("Không tồn tại");
                return Ok(a);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post(TraHangDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    await _ser.Add(dto);
                    return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("_KhachHang/{id}")]
        public async Task<IActionResult> Put(int id, TraHangDTO dto)
        {
            try
            {
                await _ser.Update(id, dto);
                return Ok("Sửa thành công!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("_KhachHang/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _ser.DeleteById(id);
                return Ok("Xóa thành công!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("_KhachHang/tra-hang-qua-han")]
        public async Task<IActionResult> DeleteTrahangQua15Days()
        {
            try
            {
                await _ser.Trahangquahan();///a
                return NoContent(); // HTTP 204 - Thành công, không trả về nội dung
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message }); // HTTP 404 - Không tìm thấy
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống", details = ex.Message }); // HTTP 500 - Lỗi server
            }
        }
        [HttpGet("_KhachHang/View-Hoa-Don-Tra-By-Idkh-{id}")]
        public async Task<IActionResult> ViewHoaDonTraByIdkh(int id)
        {
            try
            {
                return Ok(await _ser.ViewHoaDonTraByIdkh(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
