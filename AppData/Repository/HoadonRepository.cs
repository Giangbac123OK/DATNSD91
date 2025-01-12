using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.IRepository;
using AppData.Models;
using Microsoft.EntityFrameworkCore;

namespace AppData.Repository
{
	public class HoadonRepository:IHoadonRepository
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

		public async Task AddHoaDon(Hoadon hoadon)
		{
			await _context.Set<Hoadon>().AddAsync(hoadon);
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
		public async Task<IEnumerable<Hoadon>> GetAllAsync()
		{
			var X = await _context.hoadons.ToListAsync();
			return X;
		}

		public async Task SaveChanges()
		{
			await _context.SaveChangesAsync();
		}

	}
}
