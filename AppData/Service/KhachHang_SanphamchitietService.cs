using AppData.Dto;
using AppData.IRepository;
using AppData.IService;
using AppData.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AppData.Service
{
    public class KhachHang_SanphamchitietService : KhachHang_ISanphamchitietService
    {
        private readonly KhachHang_ISanphamchitietRepos _repository;
        private readonly KhachHang_IsanphamRepos _isanphamchitietRepos;
        public KhachHang_SanphamchitietService(KhachHang_ISanphamchitietRepos repository, KhachHang_IsanphamRepos isanphamchitietRepos)
        {
            _repository = repository;
            _isanphamchitietRepos = isanphamchitietRepos;

        }
        public async Task<IEnumerable<Sanphamchitiet>> GetAllAsync()
        {
            var sanphamchitiets = await _repository.GetAllAsync();

            return sanphamchitiets.Select(sanphamchitiet => new Sanphamchitiet
            {
                Id = sanphamchitiet.Id,
                Mota = sanphamchitiet.Mota,
                Trangthai = sanphamchitiet.Trangthai,
                Giathoidiemhientai = sanphamchitiet.Giathoidiemhientai,
                Soluong = sanphamchitiet.Soluong,
                Idsp = sanphamchitiet.Idsp,
            });
        }

        public async Task<Sanphamchitiet> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            return new Sanphamchitiet
            {
                Id = entity.Id,
                Mota = entity.Mota,
                Trangthai = entity.Trangthai,
                Giathoidiemhientai = entity.Giathoidiemhientai,
                Soluong = entity.Soluong,
                Idsp = entity.Idsp,
            };
        }

        public async Task<List<SanphamchitietsDTO>> GetByIdSPAsync(int idspct)
        {
            try
            {
                // Gọi repository để lấy dữ liệu
                var results = await _repository.GetByIdSPAsync(idspct);

                if (results == null || !results.Any())
                    throw new KeyNotFoundException("Không tìm thấy sản phẩm trong sản phẩm chi tiết với ID: " + idspct);

                // Ánh xạ thủ công từ entity sang DTO
                var dtoList = results.Select(result => new SanphamchitietsDTO
                {
                    Id = result.Id,
                    Mota = result.Mota,
                    Trangthai = result.Trangthai,
                    Giathoidiemhientai = result.Giathoidiemhientai,
                    Soluong = result.Soluong,
                    Idsp = result.Idsp,
                }).ToList();

                return dtoList;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm thuộc tính sản phẩm chi tiết: " + ex.Message);
            }
        }

        public async Task<List<ThuoctinhsanphamchitietDTO>> GetByIdTTSPCTAsync(int idspct)
        {
            try
            {
                // Gọi repository để lấy dữ liệu
                var results = await _repository.GetByIdTTSPCTAsync(idspct);

                if (results == null || !results.Any())
                    throw new KeyNotFoundException("Không tìm thấy thuộc tính sản phẩm chi tiết với ID: " + idspct);

                // Ánh xạ thủ công từ entity sang DTO
                var dtoList = results.Select(result => new ThuoctinhsanphamchitietDTO
                {
                    Idspct = result.Idspct,
                    Idtt = result.Idtt,
                    Tenthuoctinhchitiet = result.Tenthuoctinhchitiet.Split(',').ToList()
                }).ToList();

                return dtoList;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm thuộc tính sản phẩm chi tiết: " + ex.Message);
            }
        }

        public async Task<List<ThuoctinhsanphamchitietDTO>> GetByISPCTAsync(List<string> tenthuoctinh)
        {
            if (tenthuoctinh == null || !tenthuoctinh.Any())
                throw new ArgumentException("Danh sách thuộc tính không được để trống.");

            // Gọi đến Repository để lấy dữ liệu
            var result = await _repository.GetByISPCTAsync(tenthuoctinh);

            if (result == null || !result.Any())
            {
                throw new KeyNotFoundException("Không tìm thấy sản phẩm chi tiết với các thuộc tính được cung cấp.");
            }

            // Chuyển đổi sang DTO
            var dtoResult = result.Select(x => new ThuoctinhsanphamchitietDTO
            {
                Idspct = x.Idspct
                // Ánh xạ thêm các trường khác nếu cần
            }).ToList();

            return dtoResult;
        }

        public async Task AddAsync(SanphamchitietsDTO dto)
        {
            var sanpham = await _isanphamchitietRepos.GetByIdAsync(dto.Idsp);
            if (sanpham == null) throw new ArgumentNullException("Sản phẩm không tồn tại");

            var sanphamchitiet = new Sanphamchitiet
            {
                Mota = dto.Mota,
                Trangthai = dto.Trangthai,
                Giathoidiemhientai = dto.Giathoidiemhientai,
                Soluong = dto.Soluong,
                Idsp = dto.Idsp,
            };

            await _repository.AddAsync(sanphamchitiet);
        }

        public async Task UpdateAsync(int id, SanphamchitietsDTO dto)
        {

            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Không tìm thấy sản phẩm chi tiết.");

            var sanpham = await _isanphamchitietRepos.GetByIdAsync(dto.Idsp);
            if (sanpham == null) throw new ArgumentNullException("Sản phẩm không tồn tại");

            if (entity != null)
            {
                entity.Mota = dto.Mota;
                entity.Trangthai = dto.Trangthai;
                entity.Giathoidiemhientai = dto.Giathoidiemhientai;
                entity.Soluong = dto.Soluong;
                entity.Idsp = dto.Idsp;

                await _repository.UpdateAsync(entity);
            }
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task AddThuoctinhsanphamchitiet(int idsp, int idspct, int idtt, List<string> tenthuoctinhchitietList)
        {

            try
            {
                await _repository.AddThuoctinhsanphamchitiet(idsp, idspct, idtt, tenthuoctinhchitietList);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm thuộc tính chi tiết cho sản phẩm: {ex.Message}");
            }
        }

         

    }
}
