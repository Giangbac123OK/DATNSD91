using AppData.IRepository;
using AppData.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Repository
{
    public class KhachHang_Giamgia_RankRepos : KhachHang_IGiamgia_RankRepos
    {
        private readonly MyDbContext _context;

        public KhachHang_Giamgia_RankRepos(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<giamgia_rank>> GetAllAsync()
        {
            try
            {
                return await _context.giamgia_Ranks.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách giảm giá - rank: " + ex.Message);
            }
        }

        public async Task<giamgia_rank> GetByIdAsync(int id)
        {
            try
            {
                var result = await _context.giamgia_Ranks.FindAsync(id);
                if (result == null)
                    throw new KeyNotFoundException("Không tìm thấy giảm giá - rank với ID: " + id);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm giảm giá - rank: " + ex.Message);
            }
        }

        public async Task<List<giamgia_rank>> GetByIdRankSPCTAsync(int idspct)
        {
            return await _context.giamgia_Ranks
                                   .Where(t => t.Idrank == idspct)
                                   .ToListAsync();
        }


        public async Task AddAsync(giamgia_rank sanphamchitiet)
        {
            try
            {
                _context.giamgia_Ranks.Add(sanphamchitiet);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Lỗi khi thêm giảm giá - rank: " + ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
