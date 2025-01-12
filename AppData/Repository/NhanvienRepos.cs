using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.IRepository;
using AppData.Models;
using Microsoft.EntityFrameworkCore;


namespace AppData.Repository
{
    public class NhanvienRepos: INhanvienRepos
	{
		private readonly MyDbContext _context;
        public NhanvienRepos(MyDbContext context)
        {
			_context=context;

		}
		public bool EmailExists(string email, int? excludeId = null)
		{
			return _context.nhanviens.Any(n =>
				n.Email == email && (excludeId == null || n.Id != excludeId));
		}
		public async Task<Nhanvien> GetNhanvienByEmailAsync(string email)
		{
			return await _context.nhanviens.FirstOrDefaultAsync(nv => nv.Email == email && nv.Trangthai == 0 && nv.Role == 0);
		}
		public IEnumerable<Nhanvien> SearchByName(string name)
		{
			return _context.nhanviens.Where(nv => nv.Hoten.Contains(name)).ToList();
		}

		public IEnumerable<Nhanvien> SearchByPhoneNumber(string phoneNumber)
		{
			return _context.nhanviens.Where(nv => nv.Sdt.Contains(phoneNumber)).ToList();
		}

		public IEnumerable<Nhanvien> SearchByEmail(string email)
		{
			return _context.nhanviens.Where(nv => nv.Email.Contains(email)).ToList();
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
			try
			{
				_context.nhanviens.Add(nhanvien);
				_context.SaveChanges(); // Nếu có lỗi, DbUpdateException sẽ được ném ra
			}
			catch (DbUpdateException ex)
			{
				throw new Exception("Lỗi khi lưu dữ liệu vào cơ sở dữ liệu. Có thể email đã tồn tại.", ex);
			}
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
		public async Task<bool> CheckForeignKeyConstraintAsync(int Id)
		{
			var existsInTrahangs = await _context.trahangs.AnyAsync(th => th.Idnv == Id);
			var existsInHoadonnhaps = await _context.hoadonnhaps.AnyAsync(hdn => hdn.Idnv == Id);
			var existsInHoadons = await _context.hoadons.AnyAsync(hd => hd.Idnv == Id);
			return existsInTrahangs || existsInHoadonnhaps || existsInHoadons;

		}

		
		public async Task<Nhanvien> GetBySdtAndPasswordAsync(string email, string password)
		{
			return await _context.nhanviens.FirstOrDefaultAsync(nv => nv.Email == email && nv.Password == password);
		}
		// Tìm kiếm nhân viên theo email
		public async Task<Nhanvien> GetByEmailAsync(string email)
		{
			return await _context.nhanviens.FirstOrDefaultAsync(nv => nv.Email == email);
		}

		// Lưu thay đổi nhân viên
		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
		public async Task UpdateNhanvienAsync(Nhanvien nhanvien)
		{
			_context.nhanviens.Update(nhanvien);
			await _context.SaveChangesAsync();
		}


	}
}
