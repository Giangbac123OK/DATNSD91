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
    public class KhachHang_Giamgia_RankService : KhachHang_IGiamgia_RankService
    {
        private readonly KhachHang_IGiamgia_RankRepos _repository;
        private readonly KhachHang_IRankRepos _Rankrepository;
        private readonly KhachHang_IGiamgiaRepos _GGrepository;
        public KhachHang_Giamgia_RankService(KhachHang_IGiamgia_RankRepos repository, KhachHang_IRankRepos Rankrepository, KhachHang_IGiamgiaRepos GGrepository)
        {
            _repository = repository;
            _Rankrepository = Rankrepository;
            _GGrepository = GGrepository;
        }

        public async Task<IEnumerable<giamgia_rank>> GetAllAsync()
        {

            var entities = await _repository.GetAllAsync();

            return entities.Select(hoaDon => new giamgia_rank
            {
                IDgiamgia = hoaDon.IDgiamgia,
                Idrank = hoaDon.Idrank,
            });
        }

        public async Task<giamgia_rank> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            return new giamgia_rank
            {
                IDgiamgia = entity.IDgiamgia,
                Idrank = entity.Idrank,
            };
        }

        public async Task<List<Giamgia_RankDTO1>> GetByIdRankSPCTAsync(int idspct)
        {
            try
            {
                // Gọi repository để lấy dữ liệu
                var results = await _repository.GetByIdRankSPCTAsync(idspct);

                if (results == null || !results.Any())
                    throw new KeyNotFoundException("Không tìm thấy Sale-rank chi tiết với ID: " + idspct);

                // Ánh xạ thủ công từ entity sang DTO
                var dtoList = results.Select(result => new Giamgia_RankDTO1
                {
                    Idrank = result.Idrank,
                    IDgiamgia = result.IDgiamgia,
                }).ToList();

                return dtoList;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm thuộc tính sản phẩm chi tiết: " + ex.Message);
            }
        }

        public async Task AddAsync(Giamgia_RankDTO1 hoaDonDTO)
        {
            var rank = await _Rankrepository.GetByIdAsync(hoaDonDTO.Idrank);
            if (rank == null) throw new ArgumentNullException("Rank không tồn tại");


            var giamgia = await _GGrepository.GetByIdAsync(hoaDonDTO.IDgiamgia);
            if (giamgia == null) throw new ArgumentNullException("Giảm giá không tồn tại");

            // Tạo đối tượng Hoadon từ DTO
            var hoaDon = new giamgia_rank
            {
                IDgiamgia = hoaDonDTO.IDgiamgia,
                Idrank = hoaDonDTO.Idrank,
            };

            // Thêm hóa đơn vào cơ sở dữ liệu
            await _repository.AddAsync(hoaDon);
        }
    }
}
