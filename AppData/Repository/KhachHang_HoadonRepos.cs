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
    public class KhachHang_HoadonRepos : KhachHang_IHoadonRepository
    {
        private readonly MyDbContext _context;

        public KhachHang_HoadonRepos(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Hoadon>> GetAllAsync()
        {
            try
            {
                return await _context.hoadons
                                     .Include(h => h.Giamgia)
                                     .Include(h => h.Khachhang)
                                     .Include(h => h.Nhanvien)
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách hóa đơn.", ex);
            }
        }

        public async Task<Hoadon> GetByIdAsync(int id)
        {
            try
            {
                return await _context.hoadons
                                     .FirstOrDefaultAsync(h => h.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy hóa đơn với ID {id}.", ex);
            }
        }

        public async Task<List<Hoadon>> Checkvoucher(int idspct)
        {
            return await _context.hoadons
                                       .Where(t => t.Idkh == idspct)
                                       .ToListAsync();
        }

        public async Task AddAsync(Hoadon entity)
        {
            try
            {
                await _context.hoadons.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Lỗi khi thêm hóa đơn vào cơ sở dữ liệu.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi không xác định khi thêm hóa đơn.", ex);
            }
        }

        public async Task UpdateAsync(Hoadon entity)
        {
            try
            {
                _context.hoadons.Update(entity);
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
                var entity = await _context.hoadons.FindAsync(id);
                if (entity != null)
                {
                    _context.hoadons.Remove(entity);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"Không tìm thấy hóa đơn với ID {id} để xóa.");
                }
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Lỗi khi xóa hóa đơn trong cơ sở dữ liệu.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi không xác định khi xóa hóa đơn.", ex);
            }
        }
        public async Task<List<HoaDonViewModel>> TimhoadontheoIdKH(int id)
        {
            return await _context.hoadons
                .Where(hd => hd.Idkh == id && hd.Donvitrangthai == 0)
                .OrderByDescending(hd => hd.Thoigiandathang) // Sắp xếp giảm dần theo thời gian đặt
                .Select(hd => new HoaDonViewModel
                {
                    Id = hd.Id,
                    Tongtiencantra = hd.Tongtiencantra,
                    Tongtiensanpham = _context.hoadonchitiets
                        .Where(hdct => hdct.Idhd == hd.Id)
                        .Sum(hdct => hdct.Giasp * hdct.Soluong),
                    Giamgia = _context.hoadonchitiets
                        .Where(hdct => hdct.Idhd == hd.Id)
                        .Sum(hdct => hdct.Giamgia ?? 0),
                    Thoigiandathang = hd.Thoigiandathang,

                    Trangthaithanhtoan = hd.Trangthaithanhtoan,
                    Ngaygiaodukien = hd.Ngaygiaodukien,
                    Ngaygiaothucte = hd.Ngaygiaothucte,
                    Diachiship = hd.Diachiship,
                    Tongsoluong = _context.hoadonchitiets.Where(x => x.Idhd == hd.Id).Sum(x => x.Soluong),
                    Trangthai = hd.Trangthai
                })
                .OrderByDescending(hd => hd.Thoigiandathang).ToListAsync();
        }

        public async Task Danhandonhang(int id)
        {
            var a = await _context.hoadons.FirstOrDefaultAsync(x => x.Id == id);
            if (a != null)
            {
                a.Trangthai = 3;
                a.Ngaygiaothucte = DateTime.Now;
                _context.hoadons.Update(a);
                await _context.SaveChangesAsync();
            }
        }
    }
}
