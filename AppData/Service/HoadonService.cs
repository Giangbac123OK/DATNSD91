using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto_Admin;
using AppData.IRepository;
using AppData.IService;
using AppData.Models;
using AppData.Repository;

namespace AppData.Service
{
	public class HoadonService : IHoadonService
	{
		private readonly IHoadonRepository _hoadonRepository;
		private readonly IGiamgiaRepos _giamgiaRepository;
		private readonly Ikhachhangrepository _khachhangRepository;

		public HoadonService(IHoadonRepository hoadonRepository, IGiamgiaRepos giamgiaRepository, Ikhachhangrepository khachhangRepository)
		{
			_hoadonRepository = hoadonRepository;
			_giamgiaRepository = giamgiaRepository;
			_khachhangRepository = khachhangRepository;
		}

		public async Task<Hoadon> AddHoaDon(CreateHoadonDTO dto)
		{
			// Tạo hóa đơn
			var hoadon = new Hoadon
			{
				Idnv = dto.Idnv,
				Idkh = null,
				Idgg = null,

				Diachiship = null,
				Tiencoc = null,
				Ngaygiaodukien = null,
				Ngaygiaothucte = null,
				Sdt = null,
				Ghichu = dto.Ghichu,
				Thoigiandathang = DateTime.Now,
				Tongtiencantra = dto.Tongtiencantra,
				Tongtiensanpham = dto.Tongtiensanpham,
				Tonggiamgia = 0,
				Trangthai = 3,
				Trangthaithanhtoan = 0,
				Donvitrangthai = 1
			};

			var hoadonChiTiets = new List<Hoadonchitiet>();
			foreach (var sp in dto.SanPhamChiTiet)
			{
				var hoadonChiTiet = new Hoadonchitiet
				{
					Idspct = sp.Idspct,
					Soluong = sp.Soluong,
					Giasp = sp.Giasp,
					Giamgia = sp.Giamgia ?? 0
				};

				hoadonChiTiets.Add(hoadonChiTiet);
			}

			
		await _hoadonRepository.AddHoaDonWithDetails(hoadon, hoadonChiTiets);

			return hoadon;
		}

		public async Task<Hoadon> AddHoaDonKhachhangthanthietoff(HoadonoffKhachhangthanthietDto dto, int diemSuDung)
		{
			// Tạo mới hóa đơn
			var hoadon = new Hoadon
			{
				Idnv = dto.Idnv,
				Idkh = dto.Idkh,
				Idgg = dto.Idgg,
				Diachiship = null,
				Tiencoc = null,
				Ngaygiaodukien = null,
				Ngaygiaothucte = null,
				Sdt = dto.Sdt,
				Ghichu = dto.Ghichu,
				Thoigiandathang = DateTime.Now,
				Tongtiencantra = dto.Tongtiencantra,
				Tongtiensanpham = dto.Tongtiensanpham,
				Tonggiamgia = dto.Tonggiamgia,
				Trangthai = 3,
				Trangthaithanhtoan = 0,
				Donvitrangthai = 1
			};

			// Thêm chi tiết hóa đơn
			var hoadonChiTiets = new List<Hoadonchitiet>();
			foreach (var sp in dto.SanPhamChiTiet)
			{
				var hoadonChiTiet = new Hoadonchitiet
				{
					Idspct = sp.Idspct,
					Soluong = sp.Soluong,
					Giasp = sp.Giasp,
					Giamgia = sp.Giamgia ?? 0
				};

				hoadonChiTiets.Add(hoadonChiTiet);
			}

		 await	_hoadonRepository.AddHoaDonWithDetails(hoadon, hoadonChiTiets);

			if (dto.Idgg.HasValue)
			{
				var giamgia = await _giamgiaRepository.GetByIdAsync(dto.Idgg.Value);
				if (giamgia != null && giamgia.Soluong > 0)
				{
					giamgia.Soluong -= 1;
				await	_giamgiaRepository.UpdateAsync(giamgia);
				}
			}

			// Cập nhật điểm sử dụng của khách hàng
			var khachhang = await _khachhangRepository.GetByIdAsyncThao(dto.Idkh.Value);
			if (khachhang != null)
			{
				// Trừ điểm đã sử dụng và cập nhật
				khachhang.Diemsudung = khachhang.Diemsudung-diemSuDung+(int)(dto.Tongtiencantra / 1000); 
			await	_khachhangRepository.UpdateAsyncThao(khachhang);
			}

			return hoadon;
		}
		public async Task<IEnumerable<Hoadon>> GetAllHoadonsAsync()
		{

			var a = await _hoadonRepository.GetAllAsync();

			
			return  a;
		}
	}
}
