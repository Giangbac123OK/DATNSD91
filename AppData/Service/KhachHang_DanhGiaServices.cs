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
    public class KhachHang_DanhGiaServices : KhachHang_IDanhGiaServices
    {
        private readonly KhachHang_IDanhGiaRepos _repos;

        public KhachHang_DanhGiaServices(KhachHang_IDanhGiaRepos repos)
        {
            _repos = repos;
        }

        public async Task Create(DanhGiaDTO danhGiaDTO)
        {
            var danhgia = new Danhgia()
            {
                Idkh = danhGiaDTO.Idkh,
                Trangthai = danhGiaDTO.Trangthai,
                Noidungdanhgia = danhGiaDTO.Noidungdanhgia,
               Ngaydanhgia = danhGiaDTO.Ngaydanhgia,
                Idhdct = danhGiaDTO.Idhdct,
                UrlHinhanh = danhGiaDTO.UrlHinhanh,
            };

            await _repos.Create(danhgia);
            await _repos.SaveChanges();
        }

        public async Task Delete(int id)
        {
           await _repos.Delete(id);
            await _repos.SaveChanges();
        }

        public async Task<List<DanhGiaDTO>> GetAll()
        {
            var list = await _repos.GetAll();
            return list.Select(list => new DanhGiaDTO()
            {
                Id = list.Id,
                Idkh = list.Idkh,
                Trangthai = list.Trangthai,
                Noidungdanhgia = list.Noidungdanhgia,
                Ngaydanhgia = list.Ngaydanhgia,
                Idhdct = list.Idhdct,
                UrlHinhanh = list.UrlHinhanh,
            }).ToList();

        }

        public async Task<DanhGiaDTO> GetById(int id)
        {
            var list = await _repos.GetById(id);
            return new DanhGiaDTO()
            {
                Id = list.Id,
                Idkh = list.Idkh,
                Trangthai = list.Trangthai,
                Noidungdanhgia = list.Noidungdanhgia,
                Ngaydanhgia = list.Ngaydanhgia,
                Idhdct = list.Idhdct,
                UrlHinhanh = list.UrlHinhanh,
            };
        }

        public async Task<DanhGiaDTO> getByidHDCT(int id)
        {
            var list = await _repos.getByidHDCT(id);
            if (list == null)
            {
                return null; 
            }


            return new DanhGiaDTO()
            {
                Id = list.Id,
                Idkh = list.Idkh,
                Trangthai = list.Trangthai,
                Noidungdanhgia = list.Noidungdanhgia,
                Ngaydanhgia = list.Ngaydanhgia,
                Idhdct = list.Idhdct,
                UrlHinhanh = list.UrlHinhanh,
            };

        }

        public async Task<List<DanhGiaDTO>> GetByidSP(int idsp)
        {
            var list = await _repos.GetByidSP(idsp);
            if (list == null || !list.Any()) // Kiểm tra nếu list null hoặc rỗng
            {
                return null;
            }

            // Ánh xạ từng phần tử trong list thành DanhGiaDTO
            return list.Select(item => new DanhGiaDTO()
            {
                Id = item.Id,
                Idkh = item.Idkh,
                Trangthai = item.Trangthai,
                Noidungdanhgia = item.Noidungdanhgia,
                Ngaydanhgia = item.Ngaydanhgia,
                Idhdct = item.Idhdct,
                UrlHinhanh = item.UrlHinhanh,
            }).ToList();
        }


   
        public async Task Update(int id, DanhGiaDTO danhGiaDTO)
        {
            var itemUpdate = await _repos.GetById(id);
            

            itemUpdate.Idhdct = danhGiaDTO.Idhdct;
            itemUpdate.UrlHinhanh = danhGiaDTO.UrlHinhanh;
            itemUpdate.Ngaydanhgia = danhGiaDTO.Ngaydanhgia;
            itemUpdate.Idkh = danhGiaDTO.Idkh;
            itemUpdate.Noidungdanhgia = danhGiaDTO .Noidungdanhgia;
            itemUpdate.Trangthai = danhGiaDTO.Trangthai;

            await _repos.Update(itemUpdate);
            await _repos.SaveChanges();
        }
    }
}
