using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto;
using AppData.Dto_Admin;
using AppData.IRepository;
using AppData.Models;
using Microsoft.EntityFrameworkCore;

namespace AppData.Repository
{
	public class SanphamRepos : IsanphamRepos
	{
		private readonly MyDbContext _context;

		public SanphamRepos(MyDbContext context)
		{
			_context = context;
		}
		public async Task AddSoluongAsync(int id, int soluongThem)
		{
			var sanpham = await GetByIdAsync(id);
			if (sanpham == null)
			{
				throw new KeyNotFoundException($"Sản phẩm với ID {id} không tồn tại.");
			}
			sanpham.Soluong += soluongThem;
			if (sanpham.Trangthai == 1)
			{
				// Cập nhật Trangthai dựa trên Soluong mới
				sanpham.Trangthai = sanpham.Soluong > 0 ? 0 : 1;
			}

			_context.sanphams.Update(sanpham);
			await _context.SaveChangesAsync();
		}
	

		public async Task<List<ProductWithAttributesDTO>> GetAllActiveProductsWithAttributesAsync(int id)
		{
			var result = await _context.sanphams
		   .Where(p => p.Trangthai == 0)  // Lọc sản phẩm có trạng thái hoạt động
		   .SelectMany(p => p.Sanphamchitiets  // Lấy tất cả Sanphamchitiet của sản phẩm
			   .Where(spct => spct.Trangthai == 0)  // Lọc Sanphamchitiet có trạng thái hoạt động
			   .Select(spct => new ProductWithAttributesDTO
			   {
				   Idsp = p.Id,
				   Giaban = p.Id,
				   Giathoidiemhientai =spct.Giathoidiemhientai,
				   Tensp = p.Tensp,  // Tên sản phẩm
				   Soluong = spct.Soluong,
				   Idspct = spct.Id,  // ID của Sanphamchitiet
				   SPCTAttributes = string.Join(", ", spct.Thuoctinhsanphamchitiets
					   .Where(attr => attr.Sanphamchitiet.Trangthai == 0)  // Lọc thuộc tính chi tiết có trạng thái hoạt động
					   .Select(attr => attr.Tenthuoctinhchitiet)) 
			   })
		   )
		   .ToListAsync();
			var x = result.Where(x=>x.Idsp==id).ToList();

			return x;
		}
		public async Task<IEnumerable<Sanpham>> GetAllAsync() => await _context.sanphams.Where(x=>x.Trangthai==0|| x.Trangthai == 1|| x.Trangthai == 2).ToListAsync();

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
		public async Task<IEnumerable<SanphamDetailDto>> GetSanphamDetailsAsync()
		{
			var query = _context.sanphams
				.Where(sp => sp.Trangthai == 0)
				.Join(_context.Sanphamchitiets.Where(spct => spct.Trangthai == 0),
					  sp => sp.Id,
					  spct => spct.Idsp,
					  (sp, spct) => new { sp, spct })
				.Select(x => new
				{
					x.sp.Tensp,
					x.spct.Id,
					x.sp.Giaban,
					x.spct.Soluong,
					Thuoctinh = x.spct.Thuoctinhsanphamchitiets
								.Select(tt => tt.Tenthuoctinhchitiet)
				})
				.GroupBy(x => new { x.Tensp, x.Id , x.Giaban, x.Soluong })
				.Select(g => new SanphamDetailDto
				{
					Tensp = g.Key.Tensp,
					Idspct = g.Key.Id,
					Giaban = g.Key.Giaban,
					soluongspct = g.Key.Soluong,
					TenThuocTinhSpct = string.Join(", ", g.SelectMany(x => x.Thuoctinh).Distinct())
				});

			return await query.ToListAsync();
		}
		public async Task<bool> CheckForeignKeyConstraintAsync(int Id)
		{
			//var existsInTrahangs = await _context.trahangs.AnyAsync(th => th.Idnv == Id);
			var existsInHoadonnhapchitiets = await _context.hoadonnhapchitiets.AnyAsync(hdn => hdn.Idsp == Id);

			return existsInHoadonnhapchitiets;

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
			await _context.sanphams.Where(sp => sp.Tensp.Contains(name)).ToListAsync();
		public async Task<IEnumerable<Sanpham>> SearchByNameHdAsync(string name) =>
			
			await _context.sanphams.Where(sp => sp.Tensp.Contains(name) && sp.Trangthai==0).ToListAsync();
	}
}