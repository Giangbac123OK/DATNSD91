using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto;
using AppData.IRepository;
using AppData.IService;
using AppData.Models;

namespace AppData.Service
{
    public class KhachHang_NhacungcapService:KhachHang_InhacungcapService
	{
		private readonly KhachHang_InhacungcapRepos _repository;
		public KhachHang_NhacungcapService(KhachHang_InhacungcapRepos repository)
        {
			_repository = repository;
        }
		public async Task<NhacungcapDto> GetByIdAsync(int id)
		{
			var nhaCungCap = await _repository.GetByIdAsync(id);
			if (nhaCungCap == null) throw new KeyNotFoundException("Không tìm thấy phương thức thanh toán.");
			return new NhacungcapDto
			{
				Tennhacungcap = nhaCungCap.Tennhacungcap,
				Sdt = nhaCungCap.Sdt,
				Diachi = nhaCungCap.Diachi,
				Email = nhaCungCap.Email,
				Trangthai = nhaCungCap.Trangthai
			};
		}

		public async Task<IEnumerable<NhacungcapDto>> GetAllAsync()
		{
			var nhaCungCaps = await _repository.GetAllAsync();
			return nhaCungCaps.Select(n => new NhacungcapDto
			{
				Tennhacungcap = n.Tennhacungcap,
				Sdt = n.Sdt,
				Diachi = n.Diachi,
				Email = n.Email,
				Trangthai = n.Trangthai
			});
		}

		public async Task AddAsync(NhacungcapDto nhaCungCapDto)
		{
			var nhaCungCap = new Nhacungcap
			{
				Tennhacungcap = nhaCungCapDto.Tennhacungcap,
				Sdt = nhaCungCapDto.Sdt,
				Diachi = nhaCungCapDto.Diachi,
				Email = nhaCungCapDto.Email,
				Trangthai = nhaCungCapDto.Trangthai
			};

			await _repository.AddAsync(nhaCungCap);
		}

		public async Task UpdateAsync(int id, NhacungcapDto nhaCungCapDto)
		{
			var nhaCungCap = await _repository.GetByIdAsync(id);
			if (nhaCungCap == null)
			{
				throw new KeyNotFoundException("Không tìm thấy nhà cung cấp với ID đã cung cấp");
			}

			// Cập nhật thông tin
			nhaCungCap.Tennhacungcap = nhaCungCapDto.Tennhacungcap;
			nhaCungCap.Sdt = nhaCungCapDto.Sdt;
			nhaCungCap.Diachi = nhaCungCapDto.Diachi;
			nhaCungCap.Email = nhaCungCapDto.Email;
			nhaCungCap.Trangthai = nhaCungCapDto.Trangthai;

			await _repository.UpdateAsync(nhaCungCap);
		}

		public async Task DeleteAsync(int id)
		{
			await _repository.DeleteAsync(id);
		}
	}
}
