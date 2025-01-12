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
	public class ThuoctinhsanphamRepository:IthuoctinhsanphamchitietRepository
	{
		private readonly MyDbContext _context;
        public ThuoctinhsanphamRepository(MyDbContext context)
        {
			_context = context;

		}
		public async Task<IEnumerable<Thuoctinhsanphamchitiet>> GetByIdttAsync(int idtt)
		{
			return await _context.thuoctinhsanphamchitiets
								 .Where(x => x.Idtt == idtt)
								 .ToListAsync();
		}
		public async Task<IEnumerable<Thuoctinhsanphamchitiet>> GetAllAsync()
		{
			return await _context.thuoctinhsanphamchitiets.ToListAsync();
		}

		public async Task<Thuoctinhsanphamchitiet> GetByIdAsync(int idtt, int idspct)
		{
			return await _context.thuoctinhsanphamchitiets
				.FirstOrDefaultAsync(x => x.Idtt == idtt && x.Idspct == idspct);
		}

		public async Task AddAsync(Thuoctinhsanphamchitiet thuoctinhsanphamchitiet)
		{
			await _context.thuoctinhsanphamchitiets.AddAsync(thuoctinhsanphamchitiet);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(Thuoctinhsanphamchitiet thuoctinhsanphamchitiet)
		{
			_context.thuoctinhsanphamchitiets.Update(thuoctinhsanphamchitiet);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int idtt, int idspct)
		{
			var entity = await GetByIdAsync(idtt, idspct);
			if (entity != null)
			{
				_context.thuoctinhsanphamchitiets.Remove(entity);
				await _context.SaveChangesAsync();
			}
		}
	}
}
