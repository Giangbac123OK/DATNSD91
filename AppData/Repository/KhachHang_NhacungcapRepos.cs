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
    public class KhachHang_NhacungcapRepos : KhachHang_InhacungcapRepos
	{
		private readonly MyDbContext _context;
        public KhachHang_NhacungcapRepos(MyDbContext context)
        {
			_context= context;
		}

		public async Task<Nhacungcap> GetByIdAsync(int id)
		{
			return await _context.nhacungcaps.FindAsync(id);
		}

		public async Task<IEnumerable<Nhacungcap>> GetAllAsync()
		{
			return await _context.nhacungcaps.ToListAsync();
		}

		public async Task AddAsync(Nhacungcap nhaCungCap)
		{
			await _context.nhacungcaps.AddAsync(nhaCungCap);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(Nhacungcap nhaCungCap)
		{
			_context.nhacungcaps.Update(nhaCungCap);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var nhaCungCap = await GetByIdAsync(id);
			if (nhaCungCap != null)
			{
				_context.nhacungcaps.Remove(nhaCungCap);
				await _context.SaveChangesAsync();
			}
			else
			{
				throw new KeyNotFoundException("Không tìm thấy nhà cung cấp với ID đã cung cấp");
			}
		}

	}
}
