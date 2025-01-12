using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.IRepository;
using AppData.Models;
using AppData.ViewModel;
using Microsoft.EntityFrameworkCore;


namespace AppData.Repository
{
    public class KhachHang_ThuocTinhRepos : KhachHang_IThuocTinhRepos
    {
        private readonly MyDbContext _context;
        public KhachHang_ThuocTinhRepos(MyDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Thuoctinh>> GetAllAsync()
        {
            return await _context.Set<Thuoctinh>().ToListAsync();
        }

        public async Task<Thuoctinh> GetByIdAsync(int id)
        {
            return await _context.Set<Thuoctinh>().FindAsync(id);
        }

        public async Task<Thuoctinh> AddAsync(Thuoctinh entity)
        {
            _context.Set<Thuoctinh>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Thuoctinh> UpdateAsync(Thuoctinh entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return false;

            _context.Set<Thuoctinh>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ThuocTinhViewModel>> GetThuocTinhsChiTiet()
        {
            var thuocTinhs = await _context.thuoctinhs
            .Include(tt => tt.Thuoctinhsanphamchitiets) 
        .Select(tt => new ThuocTinhViewModel
        {
            IDThuocTinh = tt.Id,
            NameThuocTinh = tt.Tenthuoctinh, 
            thuocTinhChiTietViewModels = tt.Thuoctinhsanphamchitiets.Select(ct => new ThuocTinhChiTietViewModel
            {
                idspct = ct.Idspct, 
                TenThucTinhChiTiet = ct.Tenthuoctinhchitiet 
            }).ToList() 
        })
        .ToListAsync();

            return thuocTinhs; 
        }
    }
}
