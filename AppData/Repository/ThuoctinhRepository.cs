using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.IRepository;
using AppData.Models;
using Microsoft.EntityFrameworkCore;

namespace AppData.Repository
{
	public class ThuoctinhRepository:IThuoctinhRepository
	{
		private readonly MyDbContext _context;
        public ThuoctinhRepository(MyDbContext context)
        {
			_context=context;

		}
		public async Task<IEnumerable<Thuoctinh>> GetAll()
		{
			return await _context.Set<Thuoctinh>().ToListAsync();
		}

		public async Task<Thuoctinh> GetById(int id)
		{
			return await _context.Set<Thuoctinh>().FindAsync(id);
		}

		public async Task Add(Thuoctinh thuoctinh)
		{
			_context.thuoctinhs.Add(thuoctinh);
			await _context.SaveChangesAsync();
		}

		public async Task Update(Thuoctinh thuoctinh)
		{
			_context.thuoctinhs.Update(thuoctinh);
			await _context.SaveChangesAsync();
		}

		public async Task Delete(int id)
		{
			var thuoctinh = await _context.thuoctinhs.FindAsync(id);
			if (thuoctinh != null)
			{
				_context.thuoctinhs.Remove(thuoctinh);
				await _context.SaveChangesAsync();
			}
		}
	}
}
