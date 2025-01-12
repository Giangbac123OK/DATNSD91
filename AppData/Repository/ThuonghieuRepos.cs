using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto;
using AppData.IRepository;
using AppData.Models;
using Microsoft.EntityFrameworkCore;

namespace AppData.Repository
{
	public class ThuonghieuRepos:IThuonghieuRepos
	{
        private readonly MyDbContext _context;
        public ThuonghieuRepos(MyDbContext context)
        {
			_context=context;

		}
		public async Task<IEnumerable<Thuonghieu>> GetAllAsync()
		{
			return await _context.thuonghieus.Where(x => x.Tinhtrang == 0 || x.Tinhtrang == 1).ToListAsync();
		}

		public async Task<IEnumerable<ThuonghieuDTO>> GetAllDtoAsync()
		{
			return await _context.thuonghieus
				.Select(th => new ThuonghieuDTO
				{
					Tenthuonghieu = th.Tenthuonghieu
				}).ToListAsync();
		}

		public async Task<Thuonghieu> GetByIdAsync(int id)
		{
			return await _context.thuonghieus.FindAsync(id);
		}

		public async Task AddAsync(Thuonghieu thuonghieu)
		{
			_context.thuonghieus.Add(thuonghieu);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(Thuonghieu thuonghieu)
		{
			_context.thuonghieus.Update(thuonghieu);
			await _context.SaveChangesAsync();
		}

		public async Task<bool> XoaThuongHieu(int idThuongHieu)
		{
			// Tìm thương hiệu theo Id
			var thuongHieu = await _context.thuonghieus.FindAsync(idThuongHieu);
			if (thuongHieu == null)
			{
				return false; // Thương hiệu không tồn tại
			}

			// Kiểm tra xem thương hiệu có sản phẩm hay không
			var hasProducts = await _context.sanphams.AnyAsync(sp => sp.Idth == idThuongHieu);

			if (hasProducts)
			{
				// Nếu có sản phẩm liên quan, chỉ thay đổi tình trạng
				thuongHieu.Tinhtrang = 2; // Đã xóa
				_context.thuonghieus.Update(thuongHieu);
			}
			else
			{
				// Nếu không có sản phẩm liên quan, xóa thương hiệu
				_context.thuonghieus.Remove(thuongHieu);
			}

			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<IEnumerable<Thuonghieu>> SearchByNameAsync(string name)
		{
			return await _context.thuonghieus
				.Where(th => th.Tenthuonghieu.Contains(name))
				.Select(th => new Thuonghieu
				{
					Tenthuonghieu = th.Tenthuonghieu,
					Tinhtrang = th.Tinhtrang
				}).ToListAsync();
		}
	}
}
