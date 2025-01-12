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
    public class KhachHang_SanphamRepos : KhachHang_IsanphamRepos
    {
        private readonly MyDbContext _context;

        public KhachHang_SanphamRepos(MyDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Sanpham>> GetAllAsync() => await _context.sanphams.ToListAsync();

        public async Task<Sanpham> GetByIdAsync(int id) => await _context.sanphams.FindAsync(id);

        public async Task AddAsync(Sanpham sanpham)
        {
            if (sanpham.Soluong > 0)
                sanpham.Trangthai = 0; // Đang bán
            else
                sanpham.Trangthai = 1; // Hết hàng

            _context.sanphams.Add(sanpham);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Sanpham sanpham)
        {
            _context.sanphams.Update(sanpham);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var sanpham = await GetByIdAsync(id);
            if (sanpham != null)
            {
                _context.sanphams.Remove(sanpham);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Sanpham>> SearchByNameAsync(string name) =>
        await _context.sanphams
        .Where(sp => sp.Tensp.ToLower().Contains(name.ToLower()) && sp.Soluong > 0)
        .ToListAsync();


        public async Task<IEnumerable<SanphamViewModel>> GetAllSanphamViewModels()
        {
            var sanphams = await _context.sanphams
         .Where(sp => sp.Trangthai != 2) // Lọc sản phẩm đang hoạt động
         .Select(sp => new
         {
             sp.Id,
             sp.Tensp,
             sp.Mota,
             sp.Giaban,
             sp.NgayThemMoi,
             sp.UrlHinhanh,
             sp.Soluong,
             sp.Trangthai,
             ThuongHieu = sp.Thuonghieu != null ? sp.Thuonghieu.Tenthuonghieu : "N/A",
             sp.Idth,
             Sanphamchitiets = sp.Sanphamchitiets
                 .Where(spct => spct.Trangthai !=2 )
                 .Select(spct => new
                 {
                     spct.Id,
                     spct.Mota,
                     spct.Giathoidiemhientai,
                     spct.Soluong,
                     Sales = spct.Salechitiets
                         .Where(sale => sale.Sale.Trangthai == 0 && sale.Soluong > 0) // Chỉ lấy sale đang hoạt động
                         .Select(sale => new
                         {
                             sale.Donvi,
                             sale.Giatrigiam,
                             sale.Sale.Ten,
                             GiaTriGiam = sale.Donvi == 0
                                 ? (decimal)sale.Giatrigiam // Giảm theo VND
                                 : spct.Giathoidiemhientai * ((decimal)sale.Giatrigiam / 100m), // Giảm theo %
                             GiaSaleSanPhamChiTiet = sale.Donvi == 0
                                 ? spct.Giathoidiemhientai - (decimal)sale.Giatrigiam
                                 : spct.Giathoidiemhientai * (1 - (decimal)sale.Giatrigiam / 100m)
                         })
                 })
         })
         .ToListAsync();

            // Xử lý sản phẩm với giá bán và giá giảm
            var result = sanphams.Select(sp =>
            {
                // Tìm sản phẩm chi tiết có giảm giá lớn nhất
                var spctWithMaxSale = sp.Sanphamchitiets
                    .Select(spct => new
                    {
                        spct.Id,
                        spct.Giathoidiemhientai,

                        spct.Sales,
                        MaxSale = spct.Sales.OrderByDescending(sale => sale.GiaTriGiam).FirstOrDefault()
                    })
                    .Where(spct => spct.MaxSale != null)
                    .OrderByDescending(spct => spct.MaxSale.GiaTriGiam)
                    .FirstOrDefault();

                // Tính giá bán
                var giaban = spctWithMaxSale != null
                    ? spctWithMaxSale.Giathoidiemhientai // Giá của spct được giảm giá nhiều nhất
                    : sp.Sanphamchitiets.Any()
                        ? sp.Sanphamchitiets.Min(spct => spct.Giathoidiemhientai) // Giá nhỏ nhất trong spct
                        : sp.Giaban; // Nếu không có spct, lấy giá sản phẩm gốc

                return new SanphamViewModel
                {
                    Id = sp.Id,
                    Tensp = sp.Tensp,
                    Mota = sp.Mota,
                    Giaban = giaban,
                    URlHinhAnh = sp.UrlHinhanh,
                    Soluong = sp.Soluong,
                    NgayThemSanPham = sp.NgayThemMoi,
                    TrangThai = sp.Trangthai,
                    ThuongHieu = sp.ThuongHieu,
                    idThuongHieu = sp.Idth,
                    Giasale = spctWithMaxSale?.MaxSale?.GiaSaleSanPhamChiTiet ?? giaban,
                    GiaTriGiam = spctWithMaxSale?.MaxSale != null
                    ? (spctWithMaxSale.MaxSale.Donvi == 1
                    ? spctWithMaxSale.MaxSale.Giatrigiam // Giá trị giảm theo %
                    : spctWithMaxSale.MaxSale.Giatrigiam) // Giá trị giảm theo VND
                    : 0,

                };
            });

            return result;
        }




        public async Task<SanphamViewModel> GetSanphamViewModelByIdSP(int idsp)
        {
            var sanpham = await _context.sanphams
                .Include(sp => sp.Sanphamchitiets)
                    .ThenInclude(spct => spct.Salechitiets)
                        .ThenInclude(sale => sale.Sale)
                .Include(sp => sp.Thuonghieu)
                .Include(sp => sp.Sanphamchitiets)
                    .ThenInclude(spct => spct.Thuoctinhsanphamchitiets)
                        .ThenInclude(tt => tt.Thuoctinh)
                .Where(sp => sp.Id == idsp)
                .FirstOrDefaultAsync();

            if (sanpham == null)
            {
                return null;
            }

            var sanphamViewModel = new SanphamViewModel
            {
                Id = sanpham.Id,
                Tensp = sanpham.Tensp,
                Mota = sanpham.Mota,
                Giaban = sanpham.Giaban,
                ThuongHieu = sanpham.Thuonghieu?.Tenthuonghieu ?? "N/A",
                URlHinhAnh = sanpham.UrlHinhanh,
                Soluong = sanpham.Soluong,

                idThuongHieu = sanpham.Idth,
                Sanphamchitiets = sanpham.Sanphamchitiets?
                    .Where(spct => spct.Trangthai != 2) // Lọc SPCT có trạng thái khác 2
                    .Select(spct => new SanphamchitietViewModel
                    {
                        Id = spct.Id,
                        Mota = spct.Mota,
                        Giathoidiemhientai = spct.Giathoidiemhientai,
                        TrangThai = spct.Trangthai,
                        Soluong = spct.Soluong,
                        GiaSaleSanPhamChiTiet = spct.Salechitiets?
                            .Where(salect => salect.Sale != null && salect.Sale.Trangthai == 0 && salect.Soluong > 0) // Lọc Sale có trạng thái 0
                            .Select(salect =>
                                salect.Donvi == 0
                                    ? spct.Giathoidiemhientai - (decimal)salect.Giatrigiam // Giảm theo VND
                                    : spct.Giathoidiemhientai * (1 - (decimal)salect.Giatrigiam / 100m) // Giảm theo %
                            ).DefaultIfEmpty(spct.Giathoidiemhientai).Min(), // Lấy giá giảm nhỏ nhất
                        ThuocTinhs = spct.Thuoctinhsanphamchitiets?
                            .Select(tt => new ThuoctinhsanphamchitietViewModel
                            {
                                Tenthuoctinh = tt.Thuoctinh?.Tenthuoctinh ?? "Unknown",
                                Tenthuoctinhchitiet = tt.Tenthuoctinhchitiet ?? "No detail available"
                            }).ToList() ?? new List<ThuoctinhsanphamchitietViewModel>(),
                        Sales = spct.Salechitiets?
                            .Where(salect => salect.Sale != null && salect.Sale.Trangthai == 0) // Chỉ lấy Sale đang diễn ra
                            .Select(salect => new SalechitietViewModel
                            {
                                Donvi = salect.Donvi,
                                Giatrigiam = salect.Giatrigiam,
                                Soluong = salect.Soluong
                            }).ToList() ?? new List<SalechitietViewModel>()
                    }).ToList() ?? new List<SanphamchitietViewModel>(),
            };

            return sanphamViewModel;
        }



        public async Task<IEnumerable<SanphamViewModel>> GetAllSanphamGiamGiaViewModels()
        {
            var sanphams = await _context.sanphams
        .Where(sp => sp.Trangthai != 2) // Lọc sản phẩm đang hoạt động
        .Select(sp => new
        {
            sp.Id,
            sp.Tensp,
            sp.Mota,
            sp.Giaban,
            sp.NgayThemMoi,
            sp.Trangthai,
            sp.UrlHinhanh,
            sp.Soluong,
            ThuongHieu = sp.Thuonghieu != null ? sp.Thuonghieu.Tenthuonghieu : "N/A",
            sp.Idth,
            Sanphamchitiets = sp.Sanphamchitiets
                .Where(spct => spct.Trangthai == 0)
                .Where(spct => spct.Salechitiets.Any(sale => sale.Sale.Trangthai == 0 && sale.Soluong > 0 )) // Chỉ lấy SPCT có giảm giá
                .Select(spct => new
                {
                    spct.Id,
                    spct.Mota,
                    spct.Giathoidiemhientai,
                    spct.Soluong,
                    spct.Trangthai,
                    Sales = spct.Salechitiets
                        .Where(sale => sale.Sale.Trangthai == 0) // Lọc sale đang hoạt động
                        .Select(sale => new
                        {
                            sale.Donvi,
                            sale.Giatrigiam,
                            sale.Sale.Ten,
                            GiaTriGiam = sale.Donvi == 0
                                ? (decimal)sale.Giatrigiam // Giảm theo VND
                                : spct.Giathoidiemhientai * ((decimal)sale.Giatrigiam / 100m), // Giảm theo %
                            GiaSaleSanPhamChiTiet = sale.Donvi == 0
                                ? spct.Giathoidiemhientai - (decimal)sale.Giatrigiam
                                : spct.Giathoidiemhientai * (1 - (decimal)sale.Giatrigiam / 100m)
                        })
                })
        })
        .Where(sp => sp.Sanphamchitiets.Any()) // Chỉ lấy sản phẩm có SPCT giảm giá
        .ToListAsync();

            // Xử lý sản phẩm
            var result = sanphams.Select(sp =>
            {
                var spctWithMaxSale = sp.Sanphamchitiets
                    .Select(spct => new
                    {
                        spct.Id,
                        spct.Giathoidiemhientai,
                        spct.Sales,
                        MaxSale = spct.Sales.OrderByDescending(sale => sale.GiaTriGiam).FirstOrDefault()
                    })
                    .OrderByDescending(spct => spct.MaxSale.GiaTriGiam)
                    .FirstOrDefault();

                var giaban = spctWithMaxSale != null
                    ? spctWithMaxSale.Giathoidiemhientai
                    : sp.Giaban;

                return new SanphamViewModel
                {
                    Id = sp.Id,
                    Tensp = sp.Tensp,
                    Mota = sp.Mota,
                    Giaban = giaban,
                    URlHinhAnh = sp.UrlHinhanh,
                    Soluong = sp.Soluong,
                    NgayThemSanPham = sp.NgayThemMoi,
                    ThuongHieu = sp.ThuongHieu,
                    TrangThai = sp.Trangthai,
                    idThuongHieu = sp.Idth,
                    Giasale = spctWithMaxSale?.MaxSale?.GiaSaleSanPhamChiTiet ?? giaban,
                    GiaTriGiam = spctWithMaxSale?.MaxSale != null
                        ? (spctWithMaxSale.MaxSale.Donvi == 1
                       ? spctWithMaxSale.MaxSale.Giatrigiam // Giá trị giảm theo %
                       : spctWithMaxSale.MaxSale.Giatrigiam) // Giá trị giảm theo VND
                       : 0,

                };
            });

            return result;
        }

        public async Task<IEnumerable<SanphamViewModel>> GetAllSanphamByThuongHieu(int id)
        {
            var sanphams = await _context.sanphams
                .Where(sp => sp.Trangthai != 2 && sp.Idth == id)
                .Select(sp => new
                {
                    sp.Id,
                    sp.Tensp,
                    sp.Mota,
                    sp.Giaban,
                    sp.NgayThemMoi,
                    sp.UrlHinhanh,
                    sp.Trangthai,
                    sp.Soluong,
                    ThuongHieu = sp.Thuonghieu != null ? sp.Thuonghieu.Tenthuonghieu : "N/A",
                    sp.Idth,
                    Sanphamchitiets = sp.Sanphamchitiets
                    .Where(spct => spct.Trangthai == 0)
                 .Select(spct => new
                 {
                     spct.Id,
                     spct.Mota,
                     spct.Giathoidiemhientai,
                     spct.Soluong,
                     Sales = spct.Salechitiets
                         .Where(sale => sale.Sale.Trangthai == 0 && sale.Soluong > 0) // Chỉ lấy sale đang hoạt động
                         .Select(sale => new
                         {
                             sale.Donvi,
                             sale.Giatrigiam,
                             sale.Sale.Ten,
                             GiaTriGiam = sale.Donvi == 0
                                 ? (decimal)sale.Giatrigiam // Giảm theo VND
                                 : spct.Giathoidiemhientai * ((decimal)sale.Giatrigiam / 100m), // Giảm theo %
                             GiaSaleSanPhamChiTiet = sale.Donvi == 0
                                 ? spct.Giathoidiemhientai - (decimal)sale.Giatrigiam
                                 : spct.Giathoidiemhientai * (1 - (decimal)sale.Giatrigiam / 100m)
                         })
                 })
                })
         .ToListAsync();

            // Xử lý sản phẩm với giá bán và giá giảm
            var result = sanphams.Select(sp =>
            {
                // Tìm sản phẩm chi tiết có giảm giá lớn nhất
                var spctWithMaxSale = sp.Sanphamchitiets
                    .Select(spct => new
                    {
                        spct.Id,
                        spct.Giathoidiemhientai,
                        spct.Sales,
                        MaxSale = spct.Sales.OrderByDescending(sale => sale.GiaTriGiam).FirstOrDefault()
                    })
                    .Where(spct => spct.MaxSale != null)

                    .OrderByDescending(spct => spct.MaxSale.GiaTriGiam)
                    .FirstOrDefault();

                // Tính giá bán
                var giaban = spctWithMaxSale != null
                    ? spctWithMaxSale.Giathoidiemhientai // Giá của spct được giảm giá nhiều nhất
                    : sp.Sanphamchitiets.Any()
                        ? sp.Sanphamchitiets.Min(spct => spct.Giathoidiemhientai) // Giá nhỏ nhất trong spct
                        : sp.Giaban; // Nếu không có spct, lấy giá sản phẩm gốc

                return new SanphamViewModel
                {
                    Id = sp.Id,
                    Tensp = sp.Tensp,
                    Mota = sp.Mota,
                    Giaban = giaban,
                    TrangThai = sp.Trangthai,
                    URlHinhAnh = sp.UrlHinhanh,
                    Soluong = sp.Soluong,
                    NgayThemSanPham = sp.NgayThemMoi,
                    ThuongHieu = sp.ThuongHieu,
                    idThuongHieu = sp.Idth,
                    Giasale = spctWithMaxSale?.MaxSale?.GiaSaleSanPhamChiTiet ?? giaban,
                    GiaTriGiam = spctWithMaxSale?.MaxSale != null
                        ? (spctWithMaxSale.MaxSale.Donvi == 1
                       ? spctWithMaxSale.MaxSale.Giatrigiam // Giá trị giảm theo %
                       : spctWithMaxSale.MaxSale.Giatrigiam) // Giá trị giảm theo VND
                       : 0,

                };
            });

            return result;
        }



        public async Task<IEnumerable<SanphamViewModel>> GetSanphamByThuocTinh(
    List<string> tenThuocTinhs = null,
    decimal? giaMin = null,
    decimal? giaMax = null,
    int? idThuongHieu = null)
{
    // Lấy danh sách sản phẩm từ cơ sở dữ liệu
    var sanphams = await _context.sanphams
        .Where(sp => sp.Trangthai != 2) // Lọc sản phẩm đang hoạt động
        .Select(sp => new
        {
            sp.Id,
            sp.Tensp,
            sp.Mota,
            sp.Giaban,
            sp.NgayThemMoi,
            sp.UrlHinhanh,
            sp.Soluong,
            sp.Trangthai,
            ThuongHieu = sp.Thuonghieu != null ? sp.Thuonghieu.Tenthuonghieu : "N/A",
            sp.Idth,
            Sanphamchitiets = sp.Sanphamchitiets
                .Where(spct => spct.Trangthai != 2)
              .Select(spct => new
              {
                  spct.Id,
                  spct.Mota,
                  spct.Giathoidiemhientai,
                  spct.Soluong,
                  spct.Thuoctinhsanphamchitiets,
                  Sales = spct.Salechitiets
                         .Where(sale => sale.Sale.Trangthai == 0 && sale.Soluong > 0) // Chỉ lấy sale đang hoạt động
                         .Select(sale => new
                         {
                             sale.Donvi,
                             sale.Giatrigiam,
                             sale.Sale.Ten,
                             GiaTriGiam = sale.Donvi == 0
                                 ? (decimal)sale.Giatrigiam // Giảm theo VND
                                 : spct.Giathoidiemhientai * ((decimal)sale.Giatrigiam / 100m), // Giảm theo %
                             GiaSaleSanPhamChiTiet = sale.Donvi == 0
                                 ? spct.Giathoidiemhientai - (decimal)sale.Giatrigiam
                                 : spct.Giathoidiemhientai * (1 - (decimal)sale.Giatrigiam / 100m)
                         })
              })
        })
        .ToListAsync();

    // Áp dụng logic xử lý để tính giá bán và giá giảm
    var result = sanphams
        .Select(sp =>
        {
            // Tìm sản phẩm chi tiết có giảm giá lớn nhất
            var spctWithMaxSale = sp.Sanphamchitiets
                .Select(spct => new
                {
                    spct.Id,
                    spct.Giathoidiemhientai,
                    spct.Sales,
                    MaxSale = spct.Sales.OrderByDescending(sale => sale.GiaTriGiam).FirstOrDefault()
                })
                .Where(spct => spct.MaxSale != null)
                .OrderByDescending(spct => spct.MaxSale.GiaTriGiam)
                .FirstOrDefault();

            // Tính giá bán
            var giaban = spctWithMaxSale != null
                ? spctWithMaxSale.Giathoidiemhientai
                : sp.Sanphamchitiets.Any()
                    ? sp.Sanphamchitiets.Min(spct => spct.Giathoidiemhientai)
                    : sp.Giaban;

            return new SanphamViewModel
            {
                Id = sp.Id,
                Tensp = sp.Tensp,
                Mota = sp.Mota,
                Giaban = giaban,
                URlHinhAnh = sp.UrlHinhanh,
                Soluong = sp.Soluong,
                NgayThemSanPham = sp.NgayThemMoi,
                TrangThai = sp.Trangthai,
                ThuongHieu = sp.ThuongHieu,
                idThuongHieu = sp.Idth,
                Giasale = spctWithMaxSale?.MaxSale?.GiaSaleSanPhamChiTiet ?? giaban,
                GiaTriGiam = spctWithMaxSale?.MaxSale != null
                    ? (spctWithMaxSale.MaxSale.Donvi == 1
                    ? spctWithMaxSale.MaxSale.Giatrigiam // Giá trị giảm theo %
                    : spctWithMaxSale.MaxSale.Giatrigiam) // Giá trị giảm theo VND
                    : 0,
                Sanphamchitiets = sp.Sanphamchitiets?
                .Select(spct => new SanphamchitietViewModel
                {
                    ThuocTinhs = spct.Thuoctinhsanphamchitiets?
                        .Select(tt => new ThuoctinhsanphamchitietViewModel
                        {
                            Tenthuoctinh = tt.Thuoctinh?.Tenthuoctinh ?? "Unknown",
                            Tenthuoctinhchitiet = tt.Tenthuoctinhchitiet ?? "No detail available"
                        }).ToList() ?? new List<ThuoctinhsanphamchitietViewModel>()
                }).ToList()
            };
        });

            if (tenThuocTinhs != null && tenThuocTinhs.Any())
            {
                result = result.Where(sp =>
                sp.Sanphamchitiets.Any(spct =>
                spct.ThuocTinhs.Any(tt =>
             tenThuocTinhs.Any(ttName => tt.Tenthuoctinhchitiet.Contains(ttName)))));

            }


            // Lọc theo giá bán
            if (giaMin.HasValue)
            {
              result = result.Where(sp => sp.Giasale >= giaMin.Value);
            }

            if (giaMax.HasValue)
            {
            result = result.Where(sp => sp.Giasale <= giaMax.Value);
            }

            // Lọc theo thương hiệu
             if (idThuongHieu.HasValue)
             {
             result = result.Where(sp => sp.idThuongHieu == idThuongHieu.Value);
             }

    return result.ToList();
}




    }
}