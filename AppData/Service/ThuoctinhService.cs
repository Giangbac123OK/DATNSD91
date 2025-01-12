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
	public class ThuoctinhService:IThuoctinhService
	{
		private readonly IThuoctinhRepository _repository;
        public ThuoctinhService(IThuoctinhRepository repository)
        {
			_repository = repository;

		}
		public async Task<IEnumerable<Thuoctinh>> GetAll()
		{
			return await _repository.GetAll();
		}

		public async Task<Thuoctinh> GetById(int id)
		{
			//turn await _repository.GetById(id);
			var thuoctinh = await _repository.GetById(id);
			if (thuoctinh == null) throw new KeyNotFoundException("Không tồn tại thuộc tính");
			return new Thuoctinh
			{
				
				Id = thuoctinh.Id,
				Tenthuoctinh = thuoctinh.Tenthuoctinh
				
			};
		}

		public async Task Add(ThuoctinhDto thuoctinhDto)
		{
			// Tạo một đối tượng Thuoctinh mới và gán dữ liệu từ DTO
			var thuoctinh = new Thuoctinh
			{
				Tenthuoctinh = thuoctinhDto.Tenthuoctinh
			};

			await _repository.Add(thuoctinh);
		}

		public async Task Update(int id, ThuoctinhDto thuoctinhDto)
		{
			var existingThuoctinh = await _repository.GetById(id);
			if (existingThuoctinh != null)
			{
				// Cập nhật dữ liệu từ DTO sang Model
				existingThuoctinh.Tenthuoctinh = thuoctinhDto.Tenthuoctinh;

				await _repository.Update(existingThuoctinh);
			}
		}

		public async Task Delete(int id)
		{
			await _repository.Delete(id);
		}
	}
}
