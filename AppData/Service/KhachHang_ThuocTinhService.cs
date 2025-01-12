using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.IRepository;
using AppData.Dto;
using AppData.IService;
using AppData.Models;
using AppData.ViewModel;


namespace AppData.Service
{
    public class KhachHang_ThuocTinhService : KhachHang_IThuoctinhService
    {
        private readonly KhachHang_IThuocTinhRepos _repository;

        public KhachHang_ThuocTinhService(KhachHang_IThuocTinhRepos repos)
        {
            _repository = repos;
        }


        public async Task<IEnumerable<ThuoctinhDTO>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(e => new ThuoctinhDTO
            {
                Tenthuoctinh = e.Tenthuoctinh
            });
        }

        public async Task<ThuoctinhDTO> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            return new ThuoctinhDTO
            {
                Tenthuoctinh = entity.Tenthuoctinh
            };
        }

        public async Task<ThuoctinhDTO> AddAsync(ThuoctinhDTO dto)
        {
            var entity = new Thuoctinh
            {
                Tenthuoctinh = dto.Tenthuoctinh
            };

            var addedEntity = await _repository.AddAsync(entity);
            return new ThuoctinhDTO
            {
                Tenthuoctinh = addedEntity.Tenthuoctinh
            };
        }

        public async Task<ThuoctinhDTO> UpdateAsync(int id, ThuoctinhDTO dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.Tenthuoctinh = dto.Tenthuoctinh;

            var updatedEntity = await _repository.UpdateAsync(entity);
            return new ThuoctinhDTO
            {
                Tenthuoctinh = updatedEntity.Tenthuoctinh
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ThuocTinhViewModel>> GetThuocTinhsChiTiet()
        {
            return await _repository.GetThuocTinhsChiTiet();
        }
    }
}
