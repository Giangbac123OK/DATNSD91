using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.IRepository;
using AppData;
using AppData.Models;
using Microsoft.EntityFrameworkCore;
using AppData.Dto;
using Microsoft.Data.SqlClient;

namespace AppData.Repository
{
    public class KhachHang_SanphamchitietRepos : KhachHang_ISanphamchitietRepos
    {
        private readonly MyDbContext _context;

        public KhachHang_SanphamchitietRepos(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Sanphamchitiet>> GetAllAsync()
        {
            try
            {
                return await _context.Sanphamchitiets.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách sản phẩm chi tiết: " + ex.Message);
            }
        }

        public async Task<Sanphamchitiet> GetByIdAsync(int id)
        {
            try
            {
                var result = await _context.Sanphamchitiets.FindAsync(id);
                if (result == null)
                    throw new KeyNotFoundException("Không tìm thấy sản phẩm chi tiết với ID: " + id);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm sản phẩm chi tiết: " + ex.Message);
            }
        }
        public async Task<List<Sanphamchitiet>> GetByIdSPAsync(int idsp)
        {
            return await _context.Sanphamchitiets
                                   .Where(t => t.Idsp == idsp)
                                   .ToListAsync();
        }

        public async Task<List<Thuoctinhsanphamchitiet>> GetByIdTTSPCTAsync(int idspct)
        {
            return await _context.thuoctinhsanphamchitiets
                                   .Where(t => t.Idspct == idspct)
                                   .ToListAsync();
        }

        public async Task<List<Thuoctinhsanphamchitiet>> GetByISPCTAsync(List<string> tenthuoctinh)
        {
            if (tenthuoctinh == null || !tenthuoctinh.Any())
                throw new ArgumentException("Danh sách thuộc tính không được để trống.");

            // Tạo câu lệnh SQL an toàn với tham số thay vì nối trực tiếp
            var tenthuoctinhParams = string.Join(",", tenthuoctinh.Select((t, index) => $"@t{index}"));

            var sqlQuery = $@"
            SELECT tt.Idspct
            FROM thuoctinhsanphamchitiets tt
            WHERE tt.Tenthuoctinhchitiet IN ({tenthuoctinhParams})
            GROUP BY tt.Idspct
            HAVING COUNT(DISTINCT tt.Tenthuoctinhchitiet) = {tenthuoctinh.Count}";

            // Tạo danh sách các tham số
            var parameters = tenthuoctinh.Select((t, index) => new SqlParameter($"@t{index}", t)).ToArray();

            // Thực thi câu lệnh SQL với tham số
            var result = await _context.thuoctinhsanphamchitiets
                                       .FromSqlRaw(sqlQuery, parameters)
                                       .Select(tt => new Thuoctinhsanphamchitiet
                                       {
                                           Idspct = tt.Idspct
                                       })
                                       .ToListAsync(); // Lấy tất cả kết quả thay vì chỉ một

            return result;
        }

        public async Task<Sanphamchitiet> AddAsync(Sanphamchitiet sanphamchitiet)
        {
            try
            {
                _context.Sanphamchitiets.Add(sanphamchitiet);
                await _context.SaveChangesAsync();
                return sanphamchitiet;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Lỗi khi thêm sản phẩm chi tiết: " + ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<Sanphamchitiet> UpdateAsync(Sanphamchitiet sanphamchitiet)
        {
            try
            {
                _context.Sanphamchitiets.Update(sanphamchitiet);
                await _context.SaveChangesAsync();
                return sanphamchitiet;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Lỗi khi cập nhật sản phẩm chi tiết: " + ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var sanphamchitiet = await _context.Sanphamchitiets.FindAsync(id);
                if (sanphamchitiet == null)
                    throw new KeyNotFoundException("Không tìm thấy sản phẩm chi tiết với ID: " + id);

                _context.Sanphamchitiets.Remove(sanphamchitiet);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Lỗi khi xóa sản phẩm chi tiết: " + ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task AddThuoctinhsanphamchitiet(int idsp, int idspct, int idtt, List<string> tenthuoctinhchitietList)
        {
            try
            {
                if (tenthuoctinhchitietList == null || !tenthuoctinhchitietList.Any())
                    throw new ArgumentException("Danh sách thuộc tính chi tiết không được để trống.");

                // Kiểm tra sản phẩm
                var sanpham = await _context.sanphams.FindAsync(idsp);
                if (sanpham == null)
                    throw new Exception($"Sản phẩm không tồn tại với ID: {idsp}");

                // Kiểm tra sản phẩm chi tiết
                var sanphamchitiet = await _context.Sanphamchitiets.FindAsync(idspct);
                if (sanphamchitiet == null)
                    throw new Exception($"Sản phẩm chi tiết không tồn tại với ID: {idspct}");

                // Lấy số lượng thuộc tính từ bảng thuộc tính
                int totalAttributes = await _context.thuoctinhs.CountAsync();
                if (totalAttributes == 0)
                    throw new Exception("Không tìm thấy thuộc tính nào để kiểm tra.");

                // Lấy thuộc tính hiện tại của sản phẩm chi tiết
                var existingSPCTAttributes = await _context.thuoctinhsanphamchitiets
                    .Where(tt => tt.Idspct == idspct)
                    .Select(tt => tt.Tenthuoctinhchitiet)
                    .ToListAsync();

                // Kết hợp thuộc tính hiện có và thuộc tính mới
                var combinedAttributes = existingSPCTAttributes.Union(tenthuoctinhchitietList).ToList();

                // Lấy tất cả sản phẩm chi tiết khác thuộc cùng sản phẩm để kiểm tra
                var allSPCTs = await _context.Sanphamchitiets
                    .Where(spct => spct.Idsp == idsp && spct.Id != idspct) // Loại trừ spct hiện tại
                    .Select(spct => new
                    {
                        spct.Id,
                        ThuocTinhChiTiet = _context.thuoctinhsanphamchitiets
                            .Where(ttct => ttct.Idspct == spct.Id)
                            .Select(ttct => ttct.Tenthuoctinhchitiet)
                            .ToList()
                    })
                    .ToListAsync();

                foreach (var spct in allSPCTs)
                {
                    var existingAttributes = spct.ThuocTinhChiTiet;

                    // Kiểm tra trùng lặp nội dung thuộc tính
                    if (existingAttributes.Count == totalAttributes &&
                        !combinedAttributes.Except(existingAttributes).Any())
                    {
                        throw new Exception("Đã tồn tại một sản phẩm chi tiết khác với danh sách thuộc tính giống hệt.");
                    }
                }

                // Thêm mới từng thuộc tính chi tiết từ danh sách truyền vào
                foreach (var tenthuoctinhchitiet in tenthuoctinhchitietList)
                {
                    var newThuoctinhchitiet = new Thuoctinhsanphamchitiet
                    {
                        Idspct = idspct,
                        Idtt = idtt,
                        Tenthuoctinhchitiet = tenthuoctinhchitiet
                    };

                    _context.thuoctinhsanphamchitiets.Add(newThuoctinhchitiet);
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Lỗi khi thêm thuộc tính chi tiết: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm thuộc tính chi tiết: {ex.Message}");
            }
        }
    }
}
