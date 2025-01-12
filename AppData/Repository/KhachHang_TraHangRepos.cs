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
    public class KhachHang_TraHangRepos : KhachHang_ITraHangRepos
    {
        private readonly MyDbContext _context;
        public KhachHang_TraHangRepos(MyDbContext context)
        {
            _context = context;
        }
        public async Task Add(Trahang trhang)
        {
            try
            {
                await _context.trahangs.AddAsync(trhang);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Lỗi khi thêm trả hàng vào cơ sở dữ liệu.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi không xác định khi thêm trả hàng.", ex);
            }
        }

        public async Task DeleteById(int id)
        {
            var a = await _context.trahangs.FirstOrDefaultAsync(x=>x.Id == id);
            if (a != null)
            {
                _context.trahangs.Remove(a);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Không tồn tại!");
            }
        }

        public async Task<List<Trahang>> GetAll()
        {
            return await _context.trahangs.ToListAsync();
        }

        public async Task<Trahang> GetById(int id)
        {
            var a = await _context.trahangs.FirstOrDefaultAsync(x => x.Id == id);
            if (a != null)
            {
                return a;
            }
            else
            {
                throw new KeyNotFoundException("Không tồn tại!");
            }
        }

        public async Task Trahangquahan()
        {
            // Tìm bản ghi trả hàng thỏa mãn điều kiện
            var th = await _context.trahangs
                .FirstOrDefaultAsync(x =>
                    (x.Ngaytrahangdukien != null && EF.Functions.DateDiffDay(x.Ngaytrahangdukien.Value, DateTime.Today) >= 15)
                    || x.Trangthai == 0);

            if (th != null)
            {
                // Lấy danh sách chi tiết trả hàng liên quan
                var thctList = await _context.trahangchitiets
                    .Where(x => x.Idth == th.Id)
                    .ToListAsync();

                // Lấy tất cả ID hóa đơn chi tiết liên quan đến trả hàng chi tiết
                var idhdctList = thctList.Select(x => x.Idhdct).ToList();

                // Lấy danh sách hóa đơn tương ứng
                var hoadons = await _context.hoadons
                    .Where(hd => idhdctList.Contains(hd.Id))
                    .ToListAsync();

                // Cập nhật trạng thái hóa đơn
                foreach (var hoadon in hoadons)
                {
                    hoadon.Trangthai = 3;
                }

                // Cập nhật dữ liệu
                _context.hoadons.UpdateRange(hoadons);

                // Nếu có chi tiết trả hàng, xóa trước
                if (thctList.Any())
                {
                    _context.trahangchitiets.RemoveRange(thctList);
                }

                // Xóa bản ghi trả hàng
                _context.trahangs.Remove(th);

                // Lưu tất cả thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
        }

        public async Task Update(Trahang trhang)
        {
            _context.trahangs.Update(trhang);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TraHangViewModel>> ViewHoaDonTraByIdkh(int id)
        {
            var a = await _context.trahangs.Where(x=>x.Idkh==id).ToListAsync();
            
            return a.Select(x => new TraHangViewModel()
            {
                Id = x.Id,
                Idkh = x.Idkh,
                Tenkh = x.Tenkhachhang,
                Lydotrahang = x.Lydotrahang,
                Tongtienhoan = x.Sotienhoan??0,
                Phuongthuchoantien = x.Phuongthuchoantien,
                Ngaytrahangdukien = x.Ngaytrahangthucte ?? null,
                Ngaytrahangthucte = x.Ngaytrahangthucte ?? null,
                Tongsoluong = _context.trahangchitiets.Where(th => th.Idth == x.Id).Sum(th => th.Soluong),
                Trangthai = x.Trangthai
            }).OrderByDescending(x => x.Ngaytrahangdukien ?? x.Ngaytrahangthucte).ToList();
        }
    }
}
