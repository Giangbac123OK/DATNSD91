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
	public class DanhgiaRepository : IDanhgiaRepository
	{
		private readonly MyDbContext _context;

		public DanhgiaRepository(MyDbContext context)
		{
			_context = context;
		}
		public async Task<IEnumerable<Danhgia>> GetDanhgiaByProductDetailIdAsync(int idspct)
		{
			var query = from dg in _context.danhgias.AsNoTracking()
							  .Include(dg => dg.Khachhang)
						join hdct in _context.hoadonchitiets on dg.Idhdct equals		hdct.Id
						join spct in _context.Sanphamchitiets on hdct.Idspct equals		spct.Id
					
						where dg.Trangthai == 0 && spct.Id == idspct
						select dg;

			return await query.ToListAsync();
		}

		public async Task<IEnumerable<Danhgia>> GetDanhgiaByProductIdAsync(int idsp)
		{
			var query = from dg in _context.danhgias.AsNoTracking()
							  .Include(dg => dg.Khachhang)
						join hdct in _context.hoadonchitiets on dg.Idhdct equals hdct.Id
						join spct in _context.Sanphamchitiets on hdct.Idspct equals spct.Id
						join kh in _context.khachhangs on dg.Idkh equals kh.Id
						where dg.Trangthai == 0 && spct.Idsp == idsp
						select dg;

			return await query.ToListAsync();
		}
	}
}
