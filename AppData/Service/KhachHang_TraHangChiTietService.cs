using AppData.Dto;
using AppData.IRepository;
using AppData.IService;
using AppData.Models;
using AppData.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Service
{
    public class KhachHang_TraHangChiTietService : KhachHang_ITraHangChiTietService
    {
        private readonly KhachHang_ITraHangChiTietRepos _repos;
        private readonly KhachHang_IHoaDonChiTietRepository _HDCTrepos;
        private readonly KhachHang_ITraHangRepos _THrepos;
        public KhachHang_TraHangChiTietService(KhachHang_ITraHangChiTietRepos repos, KhachHang_IHoaDonChiTietRepository hDCTrepos, KhachHang_ITraHangRepos tHrepos)
        {
            _repos = repos;
            _HDCTrepos = hDCTrepos;
            _THrepos = tHrepos;
        }

        public async Task Add(TraHangChiTietDTO ct)
        {
            // Kiểm tra nếu trà hàng không tồn tại
            var trahang = await _THrepos.GetById(ct.Idth);
            if (trahang == null)
                throw new ArgumentNullException("Trà hàng không tồn tại");

            // Kiểm tra nếu trà hàng không tồn tại
            var hdct = await _HDCTrepos.GetByIdAsync(ct.Idhdct);
            if (hdct == null)
                throw new ArgumentNullException("Hoá đơn chi tiết không tồn tại");

            var a = new Trahangchitiet
            {
                Idth = ct.Idth,
                Soluong = ct.Soluong,
                Tinhtrang = ct.Tinhtrang,
                Ghichu = ct.Ghichu,
                Hinhthucxuly = ct.Hinhthucxuly,
                Idhdct = ct.Idhdct
            };
            await _repos.Add(a);

            ct.Id = a.Id;
        }

        public async Task Delete(int id)
        {
            await _repos.Delete(id);
        }

        public async Task<List<TraHangChiTietDTO>> GetAll()
        {
            var a = await _repos.GetAll();
            return a.Select(x => new TraHangChiTietDTO 
            { 
                Id = x.Id,
                Idth = x.Idth,
                Soluong = x.Soluong,
                Tinhtrang = x.Tinhtrang,
                Ghichu = x.Ghichu,
                Hinhthucxuly = x.Hinhthucxuly,
                Idhdct = x.Idhdct
            }).ToList();
        }

        public async Task<TraHangChiTietDTO> GetById(int id)
        {
            var x = await _repos.GetById(id);
            return new TraHangChiTietDTO
            {
                Id = x.Id,
                Idth = x.Idth,
                Soluong = x.Soluong,
                Tinhtrang = x.Tinhtrang,
                Ghichu = x.Ghichu,
                Hinhthucxuly = x.Hinhthucxuly,
                Idhdct = x.Idhdct
            };
        }

        public async Task<List<TraHangChiTietDTO>> GetByMaHD(int id)
        {
            var a = await _repos.GetByMaHD(id);
            return a.Select(x => new TraHangChiTietDTO
            {
                Id = x.Id,
                Idth = x.Idth,
                Soluong = x.Soluong,
                Tinhtrang = x.Tinhtrang,
                Ghichu = x.Ghichu,
                Hinhthucxuly = x.Hinhthucxuly,
                Idhdct = x.Idhdct
            }).ToList();
        }

        public async Task Update(int id,TraHangChiTietDTO ct)
        {

            // Kiểm tra nếu trà hàng không tồn tại
            var a = await _repos.GetById(id);
            if (a == null)
                throw new ArgumentNullException("Trà hàng không tồn tại");
            // Kiểm tra nếu trà hàng không tồn tại
            var trahang = await _THrepos.GetById(ct.Idth);
            if (trahang == null)
                throw new ArgumentNullException("Trà hàng không tồn tại");

            // Kiểm tra nếu trà hàng không tồn tại
            var hdct = await _THrepos.GetById(ct.Idhdct);
            if (hdct == null)
                throw new ArgumentNullException("Hoá đơn chi tiết không tồn tại");

            if (a != null)
            {
                a.Idth = ct.Idth;
                a.Soluong = ct.Soluong;
                a.Tinhtrang = ct.Tinhtrang;
                a.Ghichu = ct.Ghichu;
                a.Hinhthucxuly = ct.Hinhthucxuly;
                a.Idhdct = ct.Idhdct;
                await _repos.Update(a);
            }
            else
            {
                throw new KeyNotFoundException("Không tồn tại!");
            }
        }

        public async Task UpdateSoluongTra(int idhdct, int soluong)
        {
            await _repos.UpdateSoluongTra(idhdct, soluong);
        }

        public async Task<List<TrahangchitietViewModel>> ViewHoadonctTheoIdth(int id)
        {
            return await _repos.ViewHoadonctTheoIdth(id);
        }
    }
}
