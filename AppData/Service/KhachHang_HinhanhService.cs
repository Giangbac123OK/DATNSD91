using AppData.Dto;
using AppData.IRepository;
using AppData.IService;
using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Service
{
    public class KhachHang_HinhanhService : KhachHang_IHinhanhService
    {
        private readonly KhachHang_IHinhanhRepos _repository;
        private readonly KhachHang_ITraHangRepos _THrepository;
        public KhachHang_HinhanhService(KhachHang_IHinhanhRepos repository, KhachHang_ITraHangRepos THrepository)
        {
            _repository = repository;
            _THrepository = THrepository;
        }

        public async Task AddAsync(HinhanhDTO entity)
        {
            // Kiểm tra nếu trà hàng không tồn tại
            var trahang = await _THrepository.GetById(entity.idth);
            if (trahang == null)
                throw new ArgumentNullException("Trà hàng không tồn tại");

            // Tạo đối tượng Hinhanh mới
            var hinhanh = new Hinhanh
            {
                Idth = entity.idth,
                Urlhinhanh = entity.hinhanh, // Đây có thể là chuỗi Base64 hoặc URL của ảnh
            };

            // Thêm hình ảnh vào cơ sở dữ liệu và đợi kết quả
             await _repository.AddAsync(hinhanh);

            // Trả về dữ liệu hình ảnh đã thêm vào cơ sở dữ liệu
            entity.id = hinhanh.Id; // Gán ID từ Hoadon vào DTO
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Hinhanh>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();

            return entities.Select(hoaDon => new Hinhanh
            {
                Id = hoaDon.Id,
                Idth = hoaDon.Idth,
                Urlhinhanh = hoaDon.Urlhinhanh,
            });
        }

        public async Task<Hinhanh> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            return new Hinhanh
            {
                Id = entity.Id,
                Idth = entity.Idth,
                Urlhinhanh = entity.Urlhinhanh,
            };
        }

        public async Task UpdateAsync(HinhanhDTO dto, int id)
        {

            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Hình ảnh trả hàng không tồn tại");

            var trahang = await _THrepository.GetById(dto.idth);
            if (trahang == null) throw new ArgumentNullException("Trà hàng không tồn tại");

            if (entity != null)
            {
                entity.Idth = dto.idth;
                entity.Urlhinhanh = dto.hinhanh;

                await _repository.AddAsync(entity);
            };

        }
    }
}
