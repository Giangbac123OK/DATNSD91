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
	public class ThuoctinhsanphamchitietService : IthuoctinhsanphamchitietService
	{
		private readonly IthuoctinhsanphamchitietRepository _repository;
		private readonly ISanphamchitietRepository _spctrepository;

        public ThuoctinhsanphamchitietService(IthuoctinhsanphamchitietRepository repository, ISanphamchitietRepository spctrepository)
        {
			_repository = repository;
			_spctrepository = spctrepository;

		}

		public async Task AddThuoctinhToSanphamchitietAsync(AddThuoctinhtoSpct dto)
		{
			// Bước 1: Tạo mới Sản phẩm chi tiết
			var sanphamchitiet = new Sanphamchitiet
			{
				Mota = dto.Mota,
				Trangthai = dto.Soluong > 0 ? dto.Trangthai = 0 : dto.Trangthai = 1,

				Giathoidiemhientai = dto.Giathoidiemhientai,
				Soluong = dto.Soluong,
				Idsp = dto.Idsp  // liên kết đến Sản phẩm
			};

			// Thêm sản phẩm chi tiết vào cơ sở dữ liệu
			await _spctrepository.AddAsync(sanphamchitiet);

			// Bước 2: Tạo mới các mối quan hệ thuộc tính sản phẩm chi tiết (Thuoctinhsanphamchitiet)
			foreach (var thuoctinhDto in dto.thuoctinhchitiets)
			{
				var thuoctinhsanphamchitiet = new Thuoctinhsanphamchitiet
				{
					Idtt = thuoctinhDto.Idtt,  // ID thuộc tính
					Idspct = sanphamchitiet.Id,  // ID của sản phẩm chi tiết mới tạo
					Tenthuoctinhchitiet = thuoctinhDto.Tenthuoctinhchitiet  // Tên thuộc tính chi tiết
				};

				// Thêm mối quan hệ thuộc tính vào sản phẩm chi tiết
				await _repository.AddAsync(thuoctinhsanphamchitiet);
			}
		}
		public async Task AddAsync(ThuoctinhsanphamchitietDto thuoctinhsanphamchitietDTO)
		{
			var entity = new Thuoctinhsanphamchitiet
			{
				Idtt = thuoctinhsanphamchitietDTO.Idtt,
				Idspct = thuoctinhsanphamchitietDTO.Idspct,
				Tenthuoctinhchitiet = thuoctinhsanphamchitietDTO.Tenthuoctinhchitiet
			};

			await _repository.AddAsync(entity);
		}

		public async Task DeleteAsync(int idtt, int idspct)
		{
			var entity = await _repository.GetByIdAsync(idtt, idspct);
			if (entity == null)
			{
				throw new Exception("Sản phẩm chi tiết không tồn tại.");
			}

			await _repository.DeleteAsync(idtt, idspct);
		}

		public async Task<IEnumerable<Thuoctinhsanphamchitiet>> GetAllAsync()
		{
			return await _repository.GetAllAsync();		
		}

		public async Task<ThuoctinhsanphamchitietDto> GetByIdAsync(int idtt, int idspct)
		{
			var entity = await _repository.GetByIdAsync(idtt, idspct);
			if (entity == null)
				return null;

			return new ThuoctinhsanphamchitietDto
			{
				Idtt = entity.Idtt,
				Idspct = entity.Idspct,
				Tenthuoctinhchitiet = entity.Tenthuoctinhchitiet
			};
		}
		public async Task<IEnumerable<Thuoctinhsanphamchitiet>> GetByIdttAsync(int idtt)
		{
			var entities = await _repository.GetByIdttAsync(idtt);
			var result = entities.Select(entity => new Thuoctinhsanphamchitiet
			{
				Tenthuoctinhchitiet = entity.Tenthuoctinhchitiet
			});

			return result;
		}

		public async Task UpdateAsync(ThuoctinhsanphamchitietDto thuoctinhsanphamchitietDTO)
		{
			var entity = await _repository.GetByIdAsync(thuoctinhsanphamchitietDTO.Idtt, thuoctinhsanphamchitietDTO.Idspct);
			if (entity == null)
			{
				throw new Exception("Sản phẩm chi tiết không tồn tại.");
			}

			entity.Tenthuoctinhchitiet = thuoctinhsanphamchitietDTO.Tenthuoctinhchitiet;

			await _repository.UpdateAsync(entity);
		}
	}
}
