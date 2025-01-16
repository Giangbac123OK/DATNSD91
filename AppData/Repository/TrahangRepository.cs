using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto_Admin;
using AppData.IRepository;
using AppData.Models;
using Microsoft.EntityFrameworkCore;

namespace AppData.Repository
{
	public class TrahangRepository : ITrahangRepository
	{
		private readonly MyDbContext _context;

		public TrahangRepository(MyDbContext context)
		{
			_context = context;
		}

		public async Task<TrahangDto> GetTrahangByHoadonIdAsync(int hoadonId)
		{
			var tra = await _context.trahangs
				.Where(t => t.Trahangchitiets.Any(tc => tc.Hoadonchitiet.Hoadon.Id == hoadonId))
				.Include(t => t.Nhanvien)
				.Include(t => t.Khachhang)
				.Include(t => t.Hinhanhs)
				.Include(t => t.Trahangchitiets)
					.ThenInclude(tc => tc.Hoadonchitiet)
						.ThenInclude(hc => hc.Idspchitiet)
							.ThenInclude(sp => sp.Sanpham)
				.FirstOrDefaultAsync();

			if (tra == null)
				return null;

			var dto = new TrahangDto
			{
				Id = tra.Id,
				// Tenkhachhang = tra.Tenkhachhang, // Loại bỏ
				NhanvienName = tra.Nhanvien?.Hoten,
				KhachhangName = tra.Khachhang?.Ten,
				Sotienhoan = tra.Sotienhoan,
				Lydotrahang = tra.Lydotrahang,
				TrangthaiStr = tra.TrangthaiStr,
				Phuongthuchoantien = tra.Phuongthuchoantien,
				Ngaytrahangdukien = tra.Ngaytrahangdukien,
				Ngaytrahangthucte = tra.Ngaytrahangthucte,
				Chuthich = tra.Chuthich,
				Hinhanhs = tra.Hinhanhs.Select(h => h.Urlhinhanh).ToList(),
				Trahangchitiets = tra.Trahangchitiets.Select(tc => new TrahangchitietDto
				{
					Id = tc.Id,
					Soluong = tc.Soluong,
					tinhtrang = tc.Tinhtrang,
					Ghichu = tc.Ghichu,
					Hinhthucxuly = tc.Hinhthucxuly,
					HoadonchitietInfo = new HoadonchitietInfoDto
					{
						Id = tc.Hoadonchitiet.Id,
						Tensp = tc.Hoadonchitiet.Idspchitiet?.Sanpham?.Tensp,
						Soluong = tc.Hoadonchitiet.Soluong
					}
				}).ToList()
			};

			return dto;
		}
	}
}
