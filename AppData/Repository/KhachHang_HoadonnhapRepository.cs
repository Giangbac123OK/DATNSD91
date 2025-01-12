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
    public class KhachHang_HoadonnhapRepository : KhachHang_IhoadonnhapRepository
	{
		private readonly MyDbContext _context;

		public KhachHang_HoadonnhapRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task Create(Hoadonnhap hoaDonNhap)
        {
           await _context.hoadonnhaps.AddAsync(hoaDonNhap);
        }

        public async Task Delete(int id)
        {
            var item = await GetHoadonnhapById(id);
            _context.hoadonnhaps.Remove(item);
        }

        public async Task<Hoadonnhap> GetHoadonnhapById(int id)
        {
            return await _context.hoadonnhaps.FindAsync(id);
        }

        public async Task<List<Hoadonnhap>> GetHoadonnhapListAsync()
        {
            return await _context.hoadonnhaps.ToListAsync();
        }

        public async Task SaveChanges()
        {
           await _context.SaveChangesAsync();
        }

        public async Task Update(Hoadonnhap hoadonnhap)
        {
            var updateItem = await GetHoadonnhapById(hoadonnhap.Id);
            if (updateItem != null)
            {
                _context.Entry(hoadonnhap).State = EntityState.Modified;
            }
        }
    }
}
