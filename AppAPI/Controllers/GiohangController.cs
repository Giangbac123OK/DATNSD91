using AppData.Dto;
using AppData.IService;
using AppData.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiohangController : ControllerBase
    {
        private readonly KhachHang_IGiohangService _KhachHang_Service;
        public GiohangController(KhachHang_IGiohangService ser)
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
                return NotFound("Giỏ hàng không tồn tại.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(GiohangDTO gh)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            gh.Soluong = 0;
            await _KhachHang_Service.AddGiohangAsync(gh);
            return Ok("Thêm thành công!");
        }

        [HttpGet("_KhachHang/giohangkhachhang/{id}")]
        public async Task<IActionResult> Giohangkhachhang(int id)
        {
            var hoadon = await _KhachHang_Service.GetByIdKHAsync(id);

            if (hoadon == null)
                return Ok(null); 

            return Ok(hoadon);
        }

        [HttpPut("_KhachHang/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] GiohangDTO dto)
        {
            try
            {
                await _KhachHang_Service.UpdateGiohangAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Giỏ hàng không tồn tại.");
            }
        }
        [HttpDelete("_KhachHang/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _KhachHang_Service.DeleteGiohangAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Giỏ hàng không tồn tại.");
            }
        }
    }
}
