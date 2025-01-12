using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppData;
using AppData.Models;
using AppData.IService;
using AppData.Dto;
using Humanizer;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiachisController : ControllerBase
    {
        private readonly KhachHang_IDiaChiService _KhachHang_diaChiService;

        public DiachisController(KhachHang_IDiaChiService diaChiService)
        {
            _KhachHang_diaChiService = diaChiService;
        }

        // GET: api/Diachis
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiaChiDTO>>> Getdiachis()
        {
            try
            {
                var item = await _KhachHang_diaChiService.GetAllDiaChi();
                return Ok(item);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        // GET: api/Diachis/5
        [HttpGet("_KhachHang/{id}")]
        public async Task<ActionResult<DiaChiDTO>> GetDiachi(int id)
        {
            try
            {
                var diachi = await _KhachHang_diaChiService.GetByIdAsync(id);
                if (diachi == null) return NotFound(new { message = "Địa chỉ không tìm thấy" });
                return Ok(diachi);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("_KhachHang/khachhang/{id}")]
        public async Task<IActionResult> GetDiaChiByIdKH(int id)
        {
            try
            {
                var diachiDto = await _KhachHang_diaChiService.GetDiaChiByIdKH(id);
                
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

        // PUT: api/Diachis/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("_KhachHang/{id}")]
        public async Task<IActionResult> PutDiachi(int id, DiaChiDTO diachi)
        {
           
            try
            {
                await _KhachHang_diaChiService.Update(id, diachi);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        // POST: api/Diachis
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DiaChiDTO>> PostDiachi(DiaChiDTO diachi)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _KhachHang_diaChiService.Create(diachi);
            return CreatedAtAction(nameof(Getdiachis), new { id = diachi.Diachicuthe }, diachi);

        }

        // DELETE: api/Diachis/5
        [HttpDelete("_KhachHang/{id}")]
        public async Task<IActionResult> DeleteDiachi(int id)
        {
            try
            {
                 await _KhachHang_diaChiService.Delete(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
