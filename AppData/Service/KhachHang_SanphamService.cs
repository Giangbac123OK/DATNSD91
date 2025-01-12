using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto;
using AppData.IRepository;
using AppData.IService;
using AppData.Models;
using AppData.ViewModel;

namespace AppData.Service
{
	public class KhachHang_SanphamService:KhachHang_ISanPhamservice
	{
		private readonly KhachHang_IsanphamRepos _repository;
        public KhachHang_SanphamService(KhachHang_IsanphamRepos repository)
        {
			_repository = repository;

		}
		public async Task<IEnumerable<Sanpham>> GetAllAsync()
		{
			return await _repository.GetAllAsync();
			
		}

		public async Task AddAsync(SanphamDTO sanphamDto)
		{
			var sanpham = new Sanpham
			{
				Tensp = sanphamDto.Tensp,
				Mota = sanphamDto.Mota,
				Soluong = sanphamDto.Soluong,
				Giaban = sanphamDto.Giaban,
				//Giasale = sanphamDto.Giasale,
				UrlHinhanh = sanphamDto.UrlHinhanh,
				Idth = sanphamDto.Idth,
				Trangthai = sanphamDto.Soluong > 0 ? 0 : 1
			};

			await _repository.AddAsync(sanpham);
		}

		public async Task UpdateAsync(int id, SanphamDTO sanphamDto)
		{
			var sanpham = await _repository.GetByIdAsync(id);
			if (sanpham == null) return;

			sanpham.Tensp = sanphamDto.Tensp;
			sanpham.Mota = sanphamDto.Mota;
			sanpham.Soluong = sanphamDto.Soluong;
			sanpham.Giaban = sanphamDto.Giaban;
			//sanpham.Giasale = sanphamDto.Giasale;
			sanpham.UrlHinhanh = sanphamDto.UrlHinhanh;
			sanpham.Idth = sanphamDto.Idth;
			sanpham.Trangthai = sanphamDto.Soluong > 0 ? 0 : 1;

			await _repository.UpdateAsync(sanpham);
		}

		public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);

		public async Task<IEnumerable<SanphamDTO>> SearchByNameAsync(string name)
		{
			var sanphams = await _repository.SearchByNameAsync(name);
			return sanphams.Select(sp => new SanphamDTO
			{
				Id = sp.Id,
				Tensp = sp.Tensp,
				Mota = sp.Mota,
				Trangthai = sp.Trangthai,
				Soluong = sp.Soluong,
				Giaban = sp.Giaban,
				//Giasale = sp.Giasale,
				UrlHinhanh = sp.UrlHinhanh,
				Idth = sp.Idth
			});
		}
		public async Task UpdateStatusLoad(int id)
		{

			var sale = await _repository.GetByIdAsync(id);
			if (sale == null)
			{
				throw new KeyNotFoundException("Sản phẩm không tồn tại");
			}
			if (sale.Trangthai != 3)
			{
				// Cập nhật trạng thái dựa trên ngày bắt đầu và ngày kết thúc
				if (sale.Soluong >0 )
				{
					sale.Trangthai = 0; // Đang diễn ra
				}
				else if (sale.Soluong==0)
				{
					sale.Trangthai = 1; // Chuẩn bị diễn ra
				}
			}


			await _repository.UpdateAsync(sale);
		}
		public async Task UpdateStatusToCancelled(int id)
		{
			var sale = await _repository.GetByIdAsync(id);
			if (sale == null)
			{
				throw new KeyNotFoundException("Sản phẩm không tồn tại");
			}

			sale.Trangthai = 2; // Cập nhật trạng thái thành Hủy
			await _repository.UpdateAsync(sale);
		}
		public async Task<SanphamDTO> GetByIdAsync(int id)
		{
			var sanpham = await _repository.GetByIdAsync(id);

			if (sanpham == null)
				return null;

			// Chuyển đổi đối tượng Sanpham thành SanphamDTO
			return new SanphamDTO
			{
				Tensp = sanpham.Tensp,
				Mota = sanpham.Mota,
				Trangthai = sanpham.Trangthai,
				Soluong = sanpham.Soluong,
				Giaban = sanpham.Giaban,
				//Giasale = sanpham.Giasale,
				UrlHinhanh = sanpham.UrlHinhanh,
				Idth = sanpham.Idth
			};
		}

        public async Task<IEnumerable<SanphamViewModel>> GetAllSanphamViewModels()
        {
            return await _repository.GetAllSanphamViewModels();
        }

        public async Task<SanphamViewModel> GetAllSanphamViewModelsByIdSP(int idsp)
        {
            return await _repository.GetSanphamViewModelByIdSP(idsp);
        }

        public async Task<IEnumerable<SanphamViewModel>> GetAllSanphamGiamGiaViewModels()
        {
            return await _repository.GetAllSanphamGiamGiaViewModels();
        }

        public async Task<IEnumerable<SanphamViewModel>> GetAllSanphamByThuongHieu(int id)
        {
			return await _repository.GetAllSanphamByThuongHieu(id);
        }

        public async Task<IEnumerable<SanphamViewModel>> GetSanphamByThuocTinh(List<string> tenThuocTinhs, decimal? giaMin = null, decimal? giaMax = null, int? idThuongHieu = null)
        {
            return await _repository.GetSanphamByThuocTinh(tenThuocTinhs, giaMin, giaMax, idThuongHieu);
        }
    }
}
