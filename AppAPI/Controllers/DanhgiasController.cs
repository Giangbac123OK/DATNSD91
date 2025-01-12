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
using AppData.Service;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhgiasController : ControllerBase
    {
        private readonly KhachHang_IDanhGiaServices _KhachHang_services;

        public DanhgiasController(KhachHang_IDanhGiaServices services)
        {
            _KhachHang_services = services;
        }

        // GET: api/Danhgias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DanhGiaDTO>>> Getdanhgias()
        {
            try
            {
                if (await _KhachHang_services.GetAll() == null)
                {
                    return NotFound();
                }
                return await _KhachHang_services.GetAll();
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Danhgias/5
        [HttpGet("_KhachHang/{id}")]
        public async Task<ActionResult<DanhGiaDTO>> GetDanhgia(int id)
        {
            try
            {

                if (await _KhachHang_services.GetAll() == null)
                {
                    return NotFound();
                }
                var danhgia = await _KhachHang_services.GetById(id);

                if (danhgia == null)
                {
                    return NotFound();
                }

                return danhgia;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("_KhachHang/byIDhdct/{id}")]
        public async Task<ActionResult<DanhGiaDTO>> GetDanhgiaByidHDCT(int id)
        {
            try
            {

                if (await _KhachHang_services.GetAll() == null)
                {
                    return NotFound();
                }
                var danhgia = await _KhachHang_services.getByidHDCT(id);

                if (danhgia == null)
                {
                    return NotFound();
                }

                return danhgia;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Danhgias/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("_KhachHang/{id}")]
        public async Task<IActionResult> PutDanhgia(int id, DanhGiaDTO danhgia)
        {
            if (id != danhgia.Id)
            {
                return BadRequest("ID trong URL không khớp với ID trong dữ liệu.");
            }

            try
            {
               
              
                await _KhachHang_services.Update(id, danhgia);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Đã xảy ra lỗi: {ex.Message}");
            }
        }


        // POST: api/Danhgias
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DanhGiaDTO>> PostDanhgia(DanhGiaDTO danhgia)
        {
            try
            {
                // Kiểm tra và chuyển đổi ngày giờ thành UTC nếu cần
                if (danhgia.Ngaydanhgia != null)
                {
                    // Đảm bảo rằng NgayDanhGia được xử lý là UTC
                    danhgia.Ngaydanhgia = DateTime.Parse(danhgia.Ngaydanhgia.ToString()).ToUniversalTime();
                }
                else
                {
                    // Nếu không có ngày, sử dụng ngày hiện tại theo UTC
                    danhgia.Ngaydanhgia = DateTime.UtcNow;
                }

                // Thực hiện thao tác lưu vào cơ sở dữ liệu
                await _KhachHang_services.Create(danhgia);

                // Trả về kết quả sau khi tạo mới
                return CreatedAtAction("GetDanhgia", new { id = danhgia.Id }, danhgia);
            }
            catch (Exception ex)
            {
                return BadRequest($"Đã xảy ra lỗi: {ex.Message}");
            }
        }



        // DELETE: api/Danhgias/5
        [HttpDelete("_KhachHang/{id}")]
        public async Task<IActionResult> DeleteDanhgia(int id)
        {
            if (await _KhachHang_services.GetAll() == null)
            {
                return NotFound();
            }

            try
            {
               await _KhachHang_services.Delete(id);
                return NoContent();
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("_KhachHang/GetByIdSPCT/{id}")]
        public async Task<IActionResult> GetByIdSPCT(int id)
        {
            // Kiểm tra ID
            if (id <= 0)
            {
                return BadRequest(new { message = "ID sản phẩm chi tiết không hợp lệ." });
            }

            // Gọi dịch vụ để lấy dữ liệu
            var result = await _KhachHang_services.GetByidSP(id);

            // Kiểm tra kết quả trả về
            if (result == null || !result.Any())
            {
                return Ok(null); // Trả về null trong response
            }

            return Ok(result); // Trả về kết quả nếu tìm thấy
        }
    }
}
