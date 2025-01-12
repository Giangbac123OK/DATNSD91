using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto;
using AppData.IRepository;
using AppData.Models;
using Microsoft.EntityFrameworkCore;

namespace AppData.Repository
{
    public class KhachHang_NhanvienRepos : KhachHang_INhanvienRepos
	{
		private readonly MyDbContext _context;
        public KhachHang_NhanvienRepos(MyDbContext context)
        {
			_context=context;

		}

		public async Task<IEnumerable<Nhanvien>> GetAllAsync()
		{
			return await _context.Set<Nhanvien>().ToListAsync();
		}

		public async Task<Nhanvien> GetByIdAsync(int id)
		{
			return await _context.Set<Nhanvien>().FindAsync(id);
		}

		public async Task AddAsync(Nhanvien nhanvien)
		{
			await _context.Set<Nhanvien>().AddAsync(nhanvien);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(Nhanvien nhanvien)
		{
			_context.Set<Nhanvien>().Update(nhanvien);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var nhanvien = await GetByIdAsync(id);
			if (nhanvien != null)
			{
				_context.Set<Nhanvien>().Remove(nhanvien);
				await _context.SaveChangesAsync();
			}
			else
			{
				throw new KeyNotFoundException("Không tìm thấy nhân viên");
			}
		}

        public async Task<IEnumerable<Nhanvien>> TimKiemNhanvienAsync(string search)
        {
			if (search == null)
			{
				return await _context.nhanviens.ToListAsync();
			}
			else
			{
				search = search.ToLower();
                return await _context.nhanviens.Where(x=>x.Hoten.StartsWith(search)||x.Sdt.StartsWith(search) || x.Diachi.StartsWith(search)).ToListAsync();
            }
        }

    }
}
