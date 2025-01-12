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
	public class SalechitietService:ISalechitietService
	{
		private readonly IsalechitietRepos _repository;
        public SalechitietService(IsalechitietRepos repository)
        {
			_repository = repository;

		}

		public async Task<SalechitietDTO> GetByIdAsync(int id)
		{
			var salechitiet = await _repository.GetByIdAsync(id);
			if (salechitiet == null)
				return null;

			return new SalechitietDTO
			{
				Id = salechitiet.Id,
				Idspct = salechitiet.Idspct,
				Idsale = salechitiet.Idsale,
				Donvi = salechitiet.Donvi,
				Soluong = salechitiet.Soluong,
				Giatrigiam = salechitiet.Giatrigiam
			};
		}

		public async Task<List<SalechitietDTO>> GetAllAsync()
		{
			var salechitiets = await _repository.GetAllAsync();
			return salechitiets.Select(x => new SalechitietDTO
			{
				Id = x.Id,
				Idspct = x.Idspct,
				Idsale = x.Idsale,
				Donvi = x.Donvi,
				Soluong = x.Soluong,
				Giatrigiam = x.Giatrigiam
			}).ToList();
		}

		public async Task CreateAsync(SalechitietDTO salechitietDTO)
		{
			var salechitiet = new Salechitiet
			{
				Idspct = salechitietDTO.Idspct,
				Idsale = salechitietDTO.Idsale,
				Donvi = salechitietDTO.Donvi,
				Soluong = salechitietDTO.Soluong,
				Giatrigiam = salechitietDTO.Giatrigiam
			};

			await _repository.CreateAsync(salechitiet);
		}

		public async Task UpdateAsync(SalechitietDTO salechitietDTO)
		{
			var salechitiet = new Salechitiet
			{
				Id = salechitietDTO.Id,
				Idspct = salechitietDTO.Idspct,
				Idsale = salechitietDTO.Idsale,
				Donvi = salechitietDTO.Donvi,
				Soluong = salechitietDTO.Soluong,
				Giatrigiam = salechitietDTO.Giatrigiam
			};

			await _repository.UpdateAsync(salechitiet);
		}

		public async Task DeleteAsync(int id)
		{
			await _repository.DeleteAsync(id);
		}

	}
}
