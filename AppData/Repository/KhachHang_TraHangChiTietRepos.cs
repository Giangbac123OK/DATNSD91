using AppData.IRepository;
using AppData.Models;
using AppData.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Repository
{
    public class KhachHang_TraHangChiTietRepos : KhachHang_ITraHangChiTietRepos
    {
        private readonly MyDbContext _context;
        public KhachHang_TraHangChiTietRepos(MyDbContext context)
        {
            _context = context;
        }
        public async Task Add(Trahangchitiet ct)
        {
            await _context.trahangchitiets.AddAsync(ct);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var a = await _context.trahangchitiets.FirstOrDefaultAsync(x=>x.Id == id);
            if(a != null)
            {
                _context.trahangchitiets.Remove(a);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Không tồn tại!");
            }
        }

        public async Task<List<Trahangchitiet>> GetAll()
        {
            return await _context.trahangchitiets.ToListAsync();
        }

        public async Task<Trahangchitiet> GetById(int id)
        {
            return await _context.trahangchitiets.FindAsync(id);
        }

        public async Task<List<Trahangchitiet>> GetByMaHD(int id)
        {
            return await _context.trahangchitiets.Where(x=>x.Idth == id).ToListAsync();
        }

        public async Task Update(Trahangchitiet ct)
        {
            _context.trahangchitiets.Update(ct);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSoluongTra(int idhdct, int soluong)
        {
            var hdct = await _context.hoadonchitiets.FirstOrDefaultAsync(x => x.Id == idhdct);
            if(hdct != null)
            {
                var spct = await _context.Sanphamchitiets.FirstOrDefaultAsync(x => x.Id == hdct.Idspct);
                if(spct != null)
                {
                    spct.Soluong += soluong;
                    _context.Sanphamchitiets.Update(spct);
                    var sp = await _context.sanphams.FirstOrDefaultAsync(c => c.Id == spct.Idsp);
                    if(sp != null)
                    {
                        sp.Soluong += soluong;
                        _context.sanphams.Update(sp);
                    }
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<List<TrahangchitietViewModel>> ViewHoadonctTheoIdth(int id)
        {
            var a = await (from trct in _context.trahangchitiets
                          join tr in _context.trahangs on trct.Idth equals tr.Id
                          join hdct in _context.hoadonchitiets on trct.Idhdct equals hdct.Id
                          join spct in _context.Sanphamchitiets on hdct.Idspct equals spct.Id
                          join sp in _context.sanphams on spct.Idsp equals sp.Id
                          where trct.Idth == id
                          select new TrahangchitietViewModel()
                          {
                              Id = trct.Id,
                              Idtr = trct.Idth,
                              Idspct = hdct.Idspct,
                              Idsp = spct.Idsp,
                              Tensp = sp.Tensp,
                              urlHinhanh = sp.UrlHinhanh,
                              Tongtienhoan = (trct.Soluong * hdct.Giasp) - hdct.Giamgia ?? 0,
                              Tinhtrang = trct.Tinhtrang,
                              Hinhthucxuly = trct.Hinhthucxuly,
                              Soluong = trct.Soluong,
                              Trangthaith = tr.Trangthai
                          }).ToListAsync();
            return a;
        }
    }
}
