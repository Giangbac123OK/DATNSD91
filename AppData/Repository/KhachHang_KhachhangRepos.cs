using AppData.IRepository;
using AppData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Repository
{
    public class KhachHang_KhachhangRepos : KhachHang_IKhachhangRepos
    {
        private readonly MyDbContext _context;
        public KhachHang_KhachhangRepos(MyDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Khachhang kh)
        {
            await _context.khachhangs.AddAsync(kh);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var a = await GetByIdAsync(id);
            if (a != null)
            {
                _context.khachhangs.Remove(a);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Không tìm thấy nhân viên");
            }
        }

        public async Task<IEnumerable<Khachhang>> GetAllAsync()
        {
            return await _context.khachhangs.ToListAsync();
        }

		public async Task<Khachhang> GetByEmailAsync(string email)
		{
			return await _context.khachhangs.FirstOrDefaultAsync(nv => nv.Email == email);
		}

		public async Task<Khachhang> GetByIdAsync(int id)
        {
            return await _context.khachhangs.FindAsync(id);
        }

        public async Task<IEnumerable<Khachhang>> TimKiemAsync(string search)
        {
            if (search == null)
            {
                return await _context.khachhangs.ToListAsync();
            }
            else
            {
                search = search.ToLower();
                return await _context.khachhangs.Where(x => x.Ten.StartsWith(search) || x.Sdt.StartsWith(search) || x.Email.StartsWith(search) || x.Diachi.StartsWith(search)).ToListAsync();
            }
        }

        public async Task UpdateAsync(Khachhang kh)
        {
            _context.khachhangs.Update(kh);
            await _context.SaveChangesAsync();
        }

    }
}
