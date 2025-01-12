using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto_Admin;
using AppData.IRepository;
using AppData.IService;
using AppData.Models;


namespace AppData.Service
{
    public class PhuongthucthanhtoanService : IphuongthucthanhtoanServicee
	{
		private readonly IphuongthucthanhtoanRepos _repository;

		public PhuongthucthanhtoanService(IphuongthucthanhtoanRepos repos)
		{
			_repository = repos;
		}


		public async Task<IEnumerable<PhuongthucthanhtoanDTO>> GetAllAsync()
		{
			var entities = await _repository.GetAllAsync();
			return entities.Select(e => new PhuongthucthanhtoanDTO
			{
				Tenpttt = e.Tenpttt,
				Trangthai = e.Trangthai
			});
		}

		public async Task<PhuongthucthanhtoanDTO> GetByIdAsync(int id)
		{
			var entity = await _repository.GetByIdAsync(id);
			if (entity == null) throw new KeyNotFoundException("Không tìm thấy nhà cung cấp.");

			return new PhuongthucthanhtoanDTO
			{
				Tenpttt = entity.Tenpttt,
				Trangthai = entity.Trangthai
			};
		}

		public async Task AddAsync(PhuongthucthanhtoanDTO dto)
		{
			var entity = new Phuongthucthanhtoan
			{
				Tenpttt = dto.Tenpttt,
				Trangthai = dto.Trangthai
			};

			 await _repository.AddAsync(entity);
			/*return new PhuongthucthanhtoanDTO
			{
				Tenpttt = addedEntity.Tenpttt,
				Trangthai = addedEntity.Trangthai
			};*/
		}

		public async Task UpdateAsync(int id, PhuongthucthanhtoanDTO dto)
		{
			var entity = await _repository.GetByIdAsync(id);
			if (entity == null) throw new KeyNotFoundException("Không tìm thấy phương thức thanh toán.");

			entity.Tenpttt = dto.Tenpttt;
			entity.Trangthai = dto.Trangthai;
			await _repository.UpdateAsync(entity);
			/*return new PhuongthucthanhtoanDTO
			{
				Tenpttt = updatedEntity.Tenpttt,
				Trangthai = updatedEntity.Trangthai
			};*/
		}

		public async Task DeleteAsync(int id)
		{
			await _repository.DeleteAsync(id);
		}
	}
}