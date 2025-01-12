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
    public class KhachHang_LichsuthanhtoanService : KhachHang_ILichsuthanhtoanService
    {
        private readonly KhachHang_ILichsuthanhtoanRepos _repository;
        private readonly KhachHang_IphuongthucthanhtoanRepos _PTTTrepository;
        private readonly KhachHang_IHoadonRepository _HDrepository;
        public KhachHang_LichsuthanhtoanService(KhachHang_ILichsuthanhtoanRepos repository, KhachHang_IphuongthucthanhtoanRepos PTTTrepository, KhachHang_IHoadonRepository HDrepository)
        {
            _repository = repository;
            _PTTTrepository = PTTTrepository;
            _HDrepository = HDrepository;
        }

        public async Task<IEnumerable<Lichsuthanhtoan>> GetAllAsync()
        {

            var entities = await _repository.GetAllAsync();

            return entities.Select(Lichsuthanhtoans => new Lichsuthanhtoan
            {
                Idhoadon = Lichsuthanhtoans.Idhoadon,
                idPttt = Lichsuthanhtoans.idPttt,
                Thoigianthanhtoan = Lichsuthanhtoans.Thoigianthanhtoan,
                Trangthai = Lichsuthanhtoans.Trangthai,
            });
        }

        public async Task UpdateTrangThaiAsync(int orderCode, int status)
        {
            var entity = await _repository.GetByIdAsync(orderCode);
            if (entity == null) throw new KeyNotFoundException("Lịch sử thanh toán không tồn tại");

            entity.Trangthai = status;
            await _repository.UpdateAsync(entity);
        }

        public async Task<Lichsuthanhtoan> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            return new Lichsuthanhtoan
            {
                Idhoadon = entity.Idhoadon,
                idPttt = entity.idPttt,
                Thoigianthanhtoan = entity.Thoigianthanhtoan,
                Trangthai = entity.Trangthai,
            };
        }

        public async Task<List<lichsuthanhtoanDTO>> GetByIdHDAsync(int idspct)
        {
            try
            {
                var results = await _repository.GetByIdHDAsync(idspct);

                if (results == null || !results.Any())
                    throw new KeyNotFoundException("Không tìm thấy hoá đơn trong lịch sử thanh toán với ID: " + idspct);

                var dtoList = results.Select(result => new lichsuthanhtoanDTO
                {
                    idHd = result.Idhoadon,
                    idPttt = result.idPttt,
                    Thoigianthanhtoan = result.Thoigianthanhtoan,
                    Trangthai = result.Trangthai,
                }).ToList();

                return dtoList;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm lịch sử thanh toán: " + ex.Message);
            }
        }

        public async Task AddAsync(lichsuthanhtoanDTO lichsuthanhtoanDTO)
        {
            var hoadon = await _HDrepository.GetByIdAsync(lichsuthanhtoanDTO.idHd);
            if (hoadon == null) throw new ArgumentNullException("Hoá đơn không tồn tại");

            var phuongthucthanhtoan = await _PTTTrepository.GetByIdAsync(lichsuthanhtoanDTO.idPttt);
            if (phuongthucthanhtoan == null) throw new ArgumentNullException("Phương thức thanh toán không tồn tại");

            var lichsuthanhtoan = new Lichsuthanhtoan
            {
                Idhoadon = lichsuthanhtoanDTO.idHd,
                idPttt = lichsuthanhtoanDTO.idPttt,
                Thoigianthanhtoan = lichsuthanhtoanDTO.Thoigianthanhtoan,
                Trangthai = lichsuthanhtoanDTO.Trangthai > 0 ? 0 : 1,
            };

            await _repository.AddAsync(lichsuthanhtoan);

        }


        public async Task UpdateAsync(lichsuthanhtoanDTO dto, int id)
        {

            var hoadon = await _repository.GetByIdAsync(id);
            if (hoadon == null) throw new KeyNotFoundException("Hóa đơn không tồn tại");

            var phuongthucthanhtoan = await _PTTTrepository.GetByIdAsync(dto.idPttt);
            if (phuongthucthanhtoan == null) throw new ArgumentNullException("Phương thức thanh toán không tồn tại");

            if (hoadon != null)
            {
                hoadon.Idhoadon = dto.idHd;
                hoadon.idPttt = dto.idPttt;
                hoadon.Thoigianthanhtoan = dto.Thoigianthanhtoan;
                hoadon.Trangthai = dto.Trangthai > 0 ? 0 : 1;

                await _repository.UpdateAsync(hoadon);
            }
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
