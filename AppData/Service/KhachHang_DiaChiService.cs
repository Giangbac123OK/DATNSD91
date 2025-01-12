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
    public class KhachHang_DiaChiService : KhachHang_IDiaChiService
    {
        private readonly KhachHang_IDiaChiRepos diaChiRepos;

        public KhachHang_DiaChiService(KhachHang_IDiaChiRepos diaChiRepos)
        {
            this.diaChiRepos = diaChiRepos;
        }

        public async Task Create(DiaChiDTO diachi)
        {
              var Diachi = new Diachi()
              {
                    Thanhpho = diachi.Thanhpho,
                    Idkh = diachi.Idkh,
                    Diachicuthe = diachi.Diachicuthe,
                    Quanhuyen = diachi.Quanhuyen,
                    Phuongxa = diachi.Phuongxa,
                       Ten = diachi.Ten,
                       SDT = diachi.SDT,

                };
                await diaChiRepos.Create(Diachi);
                await diaChiRepos.SaveChanges();
        }

        public async Task Delete(int id)
        {
            await diaChiRepos.Delete(id);
            await diaChiRepos.SaveChanges();
        }

        public async Task<IEnumerable<DiaChiDTO>> GetAllDiaChi()
        {
            var diaChis = await diaChiRepos.GetAllDiaChi();
            return diaChis.Select(diaChis => new DiaChiDTO()
            {
                Id = diaChis.Id,
                Diachicuthe = diaChis.Diachicuthe,
                Thanhpho = diaChis.Thanhpho,
                Phuongxa = diaChis.Phuongxa,
                Quanhuyen = diaChis.Quanhuyen,
                Idkh = diaChis.Idkh,
                Ten = diaChis.Ten,
                SDT= diaChis.SDT,
            });
        }

        public async Task<Diachi> GetByIdAsync(int id)
        {

            var diaChi = await diaChiRepos.GetByIdAsync(id);
            if (diaChi == null) throw new KeyNotFoundException("Không tìm thấy Dịa chỉ");
            return new Diachi()
            {
                Id = diaChi.Id,
                Diachicuthe = diaChi.Diachicuthe,
                Thanhpho = diaChi.Thanhpho,
                Phuongxa = diaChi.Phuongxa,
                Quanhuyen = diaChi.Quanhuyen,
                Idkh = diaChi.Idkh,
                Ten= diaChi.Ten,
                SDT = diaChi.SDT,
            };
        }

        public async Task<List<DiaChiDTO>> GetDiaChiByIdKH(int idspct)
        {
            try
            {
                var results = await diaChiRepos.GetDiaChiByIdKH(idspct);

                var dtoList = results.Select(result => new DiaChiDTO
                {
                    Id = result.Id,
                    Idkh = result.Idkh,
                    Thanhpho = result.Thanhpho,
                    Quanhuyen = result.Quanhuyen,
                    Phuongxa = result.Phuongxa,
                    Diachicuthe = result.Diachicuthe,
                    Ten = result.Ten,
                    SDT = result.SDT,
                }).ToList();

                return dtoList;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm địa chỉ khách hàng: " + ex.Message);
            }
        }

        public async Task Update(int id, DiaChiDTO diaChiDTO)
        {
            var diaChi = await diaChiRepos.GetByIdAsync(id);
            diaChi.Quanhuyen= diaChiDTO.Quanhuyen;
            diaChi.Thanhpho = diaChiDTO.Thanhpho;
            diaChi.Diachicuthe = diaChiDTO.Diachicuthe;
            diaChi.Idkh = diaChiDTO.Idkh;
            diaChi.Phuongxa = diaChiDTO.Phuongxa;
            diaChi.Ten = diaChiDTO.Ten;
            diaChi.SDT = diaChiDTO.SDT;
            await diaChiRepos.Update(diaChi);
            await diaChiRepos.SaveChanges();
        }
    }
}
