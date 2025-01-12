using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto_Admin;
using AppData.IRepository;
using AppData.IService;
using AppData.Repository;

namespace AppData.Service
{
	public class DanhgiaService : IDanhgiaService
	{
		private readonly IDanhgiaRepository _danhgiaRepository;
		private readonly Ikhachhangrepository _khackhangRepository;

		public DanhgiaService(IDanhgiaRepository danhgiaRepository, Ikhachhangrepository khackhangRepository)
		{
			_danhgiaRepository = danhgiaRepository;
			_khackhangRepository = khackhangRepository;
		}
		public async Task<IEnumerable<DanhgiaDto>> GetDanhgiaByProductDetailIdAsync(int idspct)
		{
			var danhgias = await _danhgiaRepository.GetDanhgiaByProductDetailIdAsync(idspct);
			
			//var tenkh =_khackhangRepository.GetByIdAsyncThao(danhgias.)
			return danhgias.Select(dg => new DanhgiaDto
			{
				Id = dg.Id,
				Idkh = dg.Idkh,
				Trangthai = dg.Trangthai,
				Noidungdanhgia = dg.Noidungdanhgia,
				Ngaydanhgia = dg.Ngaydanhgia,
				UrlHinhanh = dg.UrlHinhanh,
				KhachhangTen = dg.Khachhang != null ? dg.Khachhang.Ten : "Không xác định"
			});
		}

		public async Task<IEnumerable<DanhgiaDto>> GetDanhgiaByProductIdAsync(int idsp)
		{
			var danhgias = await _danhgiaRepository.GetDanhgiaByProductIdAsync(idsp);
			return danhgias.Select(dg => new DanhgiaDto
			{
				Id = dg.Id,
				Idkh = dg.Idkh,
				Trangthai = dg.Trangthai,
				Noidungdanhgia = dg.Noidungdanhgia,
				Ngaydanhgia = dg.Ngaydanhgia,
				UrlHinhanh = dg.UrlHinhanh,
				KhachhangTen = dg.Khachhang != null ? dg.Khachhang.Ten : "Không xác định"
			});
		}
	}
}
