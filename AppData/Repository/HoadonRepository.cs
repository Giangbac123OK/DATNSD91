using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto;
using AppData.Dto_Admin;
using AppData.IRepository;
using AppData.Models;
using Microsoft.EntityFrameworkCore;

namespace AppData.Repository
{
	public class HoadonRepository : IHoadonRepository
	{
		private readonly MyDbContext _context;

		public HoadonRepository(MyDbContext context)
		{
			_context = context;
		}

		public async Task<Hoadon> GetHoaDonById(int id)
		{
			return await _context.Set<Hoadon>()
				.Include(h => h.Hoadonchitiets) // Bao gồm chi tiết hóa đơn
				.FirstOrDefaultAsync(h => h.Id == id);
		}
		public async Task<IEnumerable<Hoadon>> GetAlOlnlAsync()
		{
			var X = await _context.hoadons.Where(x => x.Donvitrangthai == 0).ToListAsync();
			return X;
		}
		public async Task AddHoaDon(Hoadon hoadon)
		{
			await _context.Set<Hoadon>().AddAsync(hoadon);
		}

		public async Task<Hoadon> GetByIdAsync(int id)
		{
			return await _context.hoadons.FirstOrDefaultAsync(h => h.Id == id);
		}

		public async Task UpdateAsync(Hoadon hoadon)
		{
			_context.hoadons.Update(hoadon);
			await SaveChangesAsync();
		}

		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
		public async Task UpdateHoaDon(Hoadon hoadon)
		{
			_context.Set<Hoadon>().Update(hoadon);
			await _context.SaveChangesAsync();
		}

		public async Task<Hoadonchitiet> AddHoaDonChiTiet(Hoadonchitiet hoadonChiTiet)
		{
			await _context.Set<Hoadonchitiet>().AddAsync(hoadonChiTiet);
			await _context.SaveChangesAsync();
			return hoadonChiTiet;
		}

		public async Task AddHoaDonWithDetails(Hoadon hoadon, List<Hoadonchitiet> hoadonChiTiets)
		{
			// Thêm hóa đơn
			await _context.Set<Hoadon>().AddAsync(hoadon);
			await _context.SaveChangesAsync();

			// Thêm chi tiết hóa đơn
			if (hoadonChiTiets != null && hoadonChiTiets.Any())
			{
				foreach (var detail in hoadonChiTiets)
				{
					detail.Idhd = hoadon.Id; // Liên kết ID hóa đơn
					await _context.Set<Hoadonchitiet>().AddAsync(detail);
				}
			}

			await _context.SaveChangesAsync();
		}
		public async Task<List<HoadonReportDto>> GetOlnOrdersByWeekAsync()
		{
			var data = await _context.Set<Hoadon>()
				.Where(h => h.Donvitrangthai == 0 && h.Trangthai == 3)
				.ToListAsync(); // Tải toàn bộ dữ liệu vào bộ nhớ

			return data
				.GroupBy(h => new
				{
					Week = ISOWeek.GetWeekOfYear(h.Thoigiandathang),
					Year = h.Thoigiandathang.Year
				})
				.Select(g => new HoadonReportDto
				{
					Year = g.Key.Year,
					Week = g.Key.Week,
					TotalOrders = g.Count(),
					TotalAmount = g.Sum(h => h.Tongtiencantra)
				})
				.ToList();
		}
		public async Task<List<HoadonReportDto>> GetOffOrdersByWeekAsync()
		{
			var data = await _context.Set<Hoadon>()
				.Where(h => h.Donvitrangthai == 1 && h.Trangthai == 3)
				.ToListAsync(); // Tải toàn bộ dữ liệu vào bộ nhớ

			return data
				.GroupBy(h => new
				{
					Week = ISOWeek.GetWeekOfYear(h.Thoigiandathang),
					Year = h.Thoigiandathang.Year
				})
				.Select(g => new HoadonReportDto
				{
					Year = g.Key.Year,
					Week = g.Key.Week,
					TotalOrders = g.Count(),
					TotalAmount = g.Sum(h => h.Tongtiencantra)
				})
				.ToList();
		}

		public async Task<List<Hoadon>> GetAllAsync()
		{
			var hoadons = await _context.Set<Hoadon>()
				.Include(h => h.Nhanvien) // Bao gồm khóa ngoại Nhanvien
				.Include(h => h.Khachhang) // Bao gồm khóa ngoại Khachhang
				.Include(h => h.Giamgia) // Bao gồm khóa ngoại Giamgia
				.Include(h => h.Hoadonchitiets) // Bao gồm các chi tiết hóa đơn
				.Include(h => h.Lichsuthanhtoans) // Bao gồm lịch sử thanh toán
				.ToListAsync();

			return hoadons;
		}
		public async Task SaveChanges()
		{
			await _context.SaveChangesAsync();
		}
		public async Task<IEnumerable<Hoadon>> GetAllOffAsync()
		{
			var X = await _context.hoadons.Where(x => x.Donvitrangthai == 1).ToListAsync();
			return X;
		}

		public List<Hoadon> GetAllHoadons()
		{

			return _context.hoadons
				.ToList();
		}
		public async Task<(decimal TongTienThanhToan, int TongSoLuongDonHang)> GetDailyReportAsync(DateTime date)
		{
			var result = await _context.hoadons
				.Where(h => h.Thoigiandathang.Date == date.Date  && h.Donvitrangthai == 1&& h.Trangthai ==3) // Chỉ lấy các hóa đơn đã thanh toán
				.GroupBy(h => 1)
				.Select(g => new
				{
					TongTienThanhToan = g.Sum(h => h.Tongtiencantra),
					TongSoLuongDonHang = g.Count(),

				})
				.FirstOrDefaultAsync();

			return result == null
				? (0, 0)
				: (result.TongTienThanhToan, result.TongSoLuongDonHang);
		}
		public IEnumerable<HoadonSummaryDto> GetOrderSummaryByTime(string timeUnit)
		{
			var query = _context.hoadons
				.Where(hd => hd.Donvitrangthai == 1 && hd.Trangthai == 3);

			var result = timeUnit.ToLower() switch
			{
				"day" => query
					.GroupBy(hd => hd.Thoigiandathang.Date)
					.Select(g => new HoadonSummaryDto
					{
						ThoiGian = g.Key,
						TongTienThanhToan = g.Sum(hd => hd.Tongtiencantra),
						TongSoLuongDonHang = g.Count()
					}),

				"week" => query
					.AsEnumerable()
					.GroupBy(hd => CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
						hd.Thoigiandathang,
						CalendarWeekRule.FirstDay,
						DayOfWeek.Monday))
					.Select(g => new HoadonSummaryDto
					{
						ThoiGian = g.Min(hd => hd.Thoigiandathang),
						TongTienThanhToan = g.Sum(hd => hd.Tongtiencantra),
						TongSoLuongDonHang = g.Count()
					}),

				"month" => query
					.GroupBy(hd => new { hd.Thoigiandathang.Year, hd.Thoigiandathang.Month })
					.Select(g => new HoadonSummaryDto
					{
						ThoiGian = new DateTime(g.Key.Year, g.Key.Month, 1),
						TongTienThanhToan = g.Sum(hd => hd.Tongtiencantra),
						TongSoLuongDonHang = g.Count()
					}),

				"year" => query
					.GroupBy(hd => hd.Thoigiandathang.Year)
					.Select(g => new HoadonSummaryDto
					{
						ThoiGian = new DateTime(g.Key, 1, 1),
						TongTienThanhToan = g.Sum(hd => hd.Tongtiencantra),
						TongSoLuongDonHang = g.Count()
					}),

				_ => throw new ArgumentException("Invalid time unit")
			};

			return result.ToList(); // Gọi ToList() sau biểu thức switch
		}
		public async Task DeleteHoadonAsync(Hoadon hoadon)
		{
			_context.Set<Hoadon>().Remove(hoadon);
			await Task.CompletedTask; // Placeholder for consistency
		}

		public async Task DeleteHoadonchitietsAsync(IEnumerable<Hoadonchitiet> hoadonchitiets)
		{
			_context.Set<Hoadonchitiet>().RemoveRange(hoadonchitiets);
			await Task.CompletedTask; // Placeholder for consistency
		}

		public async Task DeleteLichsuthanhtoansAsync(IEnumerable<Lichsuthanhtoan> lichsuthanhtoans)
		{
			_context.Set<Lichsuthanhtoan>().RemoveRange(lichsuthanhtoans);
			await Task.CompletedTask; // Placeholder for consistency
		}

		public HoaDonDtoViewHoadonoff GetHoaDonDetails(int idHoaDon)
		{
			var hoaDon = (from hd in _context.hoadons
						  join nv in _context.nhanviens on hd.Idnv equals nv.Id into nvGroup
						  from nhanVien in nvGroup.DefaultIfEmpty()
						  join kh in _context.khachhangs on hd.Idkh equals kh.Id into khGroup
						  from khachHang in khGroup.DefaultIfEmpty()
						  where hd.Id == idHoaDon
						  select new
						  {
							  HoaDon = hd,
							  NhanVien = nhanVien,
							  KhachHang = khachHang
						  }).FirstOrDefault();

			if (hoaDon == null)
				return null;

			// Truy vấn chi tiết hóa đơn và sản phẩm liên quan
			var sanPhamChiTiet = (from hdct in _context.hoadonchitiets
								  join spct in _context.Sanphamchitiets on hdct.Idspct equals spct.Id
								  join sp in _context.sanphams on spct.Idsp equals sp.Id
								  where hdct.Idhd == idHoaDon
								  select new SanphamDto_HDoff
								  {
									  TenSP = sp.Tensp,
									  PhanLoaiSP = string.Join(", ", _context.thuoctinhsanphamchitiets
										  .Where(tt => tt.Idspct == spct.Id)
										  .Select(tt => tt.Tenthuoctinhchitiet)),
									  GiaBan = hdct.Giasp,
									  GiaGiam = hdct.Giamgia
								  }).ToList();

			// Tạo DTO kết quả
			var hoaDonDto = new HoaDonDtoViewHoadonoff
			{
				HoTenNV = hoaDon.NhanVien != null ? hoaDon.NhanVien.Hoten : "Không có",
				TenKH = hoaDon.KhachHang != null ? hoaDon.KhachHang.Ten : "Khách lẻ",
				ThoiGianDatHang = hoaDon.HoaDon.Thoigiandathang,
				TongTienSanPham = hoaDon.HoaDon.Tongtiensanpham,
				TongTienCanTra = hoaDon.HoaDon.Tongtiencantra,
				TongGiamGia = hoaDon.HoaDon.Tonggiamgia ?? 0,
				sanphamDto_HDoffs = sanPhamChiTiet
			};

			return hoaDonDto;
		}

		
	}

	}

