using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto;
using AppData.IRepository;
using AppData.IService;
using AppData.Models;

namespace AppData.Service
{
    public class KhachHang_HoadonnhapService:KhachHang_IHoadonnhapService
	{
		private readonly KhachHang_IhoadonnhapRepository _repository;
        public KhachHang_HoadonnhapService(KhachHang_IhoadonnhapRepository repository)
        {
			_repository = repository;

		}

        public async Task Create(HoadonnhapDTO hoaDonNhap)
        {
            var HoaDonNhap = new Hoadonnhap()
            {
                Idnv = hoaDonNhap.Idnv,
                Idncc = hoaDonNhap.Idncc,
                Trangthai = hoaDonNhap.Trangthai,
                Ngaynhap = hoaDonNhap.Ngaynhap,
                Tongtienhang = hoaDonNhap.Tongtienhang,
                Nguoigiao = hoaDonNhap.Nguoigiao,
                Sdtnguoigiao = hoaDonNhap.Sdtnguoigiao,
            };

            await _repository.Create(HoaDonNhap);
            await _repository.SaveChanges();
        }

        public async Task Delete(int id)
        {
            await _repository.Delete(id);
            await _repository.SaveChanges();
        }

        public async Task<HoadonnhapDTO> GetHoadonnhapById(int id)
        {
            var HoaDonNhap = await _repository.GetHoadonnhapById(id);
            return  new HoadonnhapDTO()
            {
                Idnv = HoaDonNhap.Idnv,
                Idncc = HoaDonNhap.Idncc,
                Ngaynhap = HoaDonNhap.Ngaynhap,
                Trangthai = HoaDonNhap.Trangthai,
                Tongtienhang = HoaDonNhap.Tongtienhang,
                Nguoigiao = HoaDonNhap.Nguoigiao,
                Sdtnguoigiao = HoaDonNhap.Sdtnguoigiao,

            };

        }

        public async Task<IEnumerable<HoadonnhapDTO>> GetHoadonnhapListAsync()
        {
            var HoaDonNhap = await _repository.GetHoadonnhapListAsync();
            return HoaDonNhap.Select(x => new HoadonnhapDTO()
            {
                Idnv = x.Idnv,
                Idncc = x.Idncc,
                Ngaynhap = x.Ngaynhap,
                Trangthai = x.Trangthai,
                Tongtienhang = x.Tongtienhang,
                Nguoigiao = x.Nguoigiao,
                Sdtnguoigiao = x.Sdtnguoigiao,

            });
        }

        public async Task Update(int id, HoadonnhapDTO hoadonnhap)
        {
            var item = await _repository.GetHoadonnhapById(id);
            item.Ngaynhap = hoadonnhap.Ngaynhap;
            item.Idncc = hoadonnhap.Idncc;
            item.Idnv = hoadonnhap.Idnv;
            item.Trangthai = hoadonnhap.Trangthai;
            item.Tongtienhang = hoadonnhap.Tongtienhang;
            item.Sdtnguoigiao = hoadonnhap.Sdtnguoigiao;
            item.Nguoigiao = hoadonnhap.Nguoigiao;
            await _repository.Update(item);
            await _repository.SaveChanges();
        }
    }

}
