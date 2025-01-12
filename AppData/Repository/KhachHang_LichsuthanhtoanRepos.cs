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
    public class KhachHang_LichsuthanhtoanRepos : KhachHang_ILichsuthanhtoanRepos
    {
        private readonly MyDbContext _context;

        public KhachHang_LichsuthanhtoanRepos(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Lichsuthanhtoan>> GetAllAsync()
        {
            try
            {
                return await _context.lichsuthanhtoans
                                     .Include(h => h.Phuongthucthanhtoan)
                                     .Include(h => h.Hoadon)
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách lịch sử thanh toán.", ex);
            }
        }

        public async Task<Lichsuthanhtoan> GetByIdAsync(int id)
        {
            try
            {
                return await _context.lichsuthanhtoans
                                     .Include(h => h.Phuongthucthanhtoan)
                                     .Include(h => h.Hoadon)
                                     .FirstOrDefaultAsync(h => h.Idhoadon == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy hóa đơn với ID {id}.", ex);
            }
        }

        public async Task<List<Lichsuthanhtoan>> GetByIdHDAsync(int id)
        {
            try
            {
                return await _context.lichsuthanhtoans
                                     .Where(h => h.Idhoadon == id)
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy hóa đơn với ID {id}.", ex);
            }
        }

        public async Task AddAsync(Lichsuthanhtoan entity)
        {
            try
            {
                await _context.lichsuthanhtoans.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Lỗi khi thêm lịch sử thanh toán vào cơ sở dữ liệu.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi không xác định khi thêm lịch sử thanh toán.", ex);
            }
        }

        public async Task UpdateAsync(Lichsuthanhtoan entity)
        {
            try
            {
                _context.lichsuthanhtoans.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Lỗi khi cập nhật hóa đơn trong cơ sở dữ liệu.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi không xác định khi cập nhật hóa đơn.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.lichsuthanhtoans.FindAsync(id);
                if (entity != null)
                {
                    _context.lichsuthanhtoans.Remove(entity);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"Không tìm thấy lịch sử thanh toán với ID {id} để xóa.");
                }
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Lỗi khi xóa lịch sử thanh toán trong cơ sở dữ liệu.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi không xác định khi xóa lịch sử thanh toán.", ex);
            }
        }
    }
}
