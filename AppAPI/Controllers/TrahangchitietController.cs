using AppData.Dto;
using AppData.IService;
using AppData.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrahangchitietController : ControllerBase
    {
        private readonly KhachHang_ITraHangChiTietService _KhachHang_service;
        public TrahangchitietController(KhachHang_ITraHangChiTietService ser)
        {
            _KhachHang_service = ser;
        }
        [HttpPut]
        public async Task<IActionResult> Updatesoluongtra(int idhdct, int soluong)
        {
            try
            {
                await _KhachHang_service.UpdateSoluongTra(idhdct, soluong);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _KhachHang_service.GetAll());
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
                var a = await _KhachHang_service.GetById(id);
                if (a == null) return BadRequest("Không tồn tại");
                return Ok(a);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("_KhachHang/Chi-tiet-ma-hoa-don:{id}")]
        public async Task<IActionResult> GetByMaHD(int id)
        {
            try
            {
                var a = await _KhachHang_service.GetByMaHD(id);
                if (a == null) return BadRequest("Không tồn tại");
                return Ok(a);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post(TraHangChiTietDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    await _KhachHang_service.Add(dto);

                    return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("_KhachHang/{id}")]
        public async Task<IActionResult> Put(int id, TraHangChiTietDTO dto)
        {
            try
            {
                await _KhachHang_service.Update(id, dto);
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
                await _KhachHang_service.Delete(id);
                return Ok("Xóa thành công!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("_KhachHang/View-Hoadonct-Theo-Idth-{id}")]
        public async Task<IActionResult> ViewHoadonctTheoIdth(int id)
        {
            try
            {
                return Ok(await _KhachHang_service.ViewHoadonctTheoIdth(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
