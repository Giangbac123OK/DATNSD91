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
    public class KhachHang_NhanvienService : KhachHang_INhanvienService
	{
		private readonly KhachHang_INhanvienRepos _repository;
        public KhachHang_NhanvienService(KhachHang_INhanvienRepos repository)
        {
			_repository=repository;

		}
		public async Task<IEnumerable<NhanvienDTO>> GetAllNhanviensAsync()
		{
			var nhanviens = await _repository.GetAllAsync();
			return nhanviens.Select(n => new NhanvienDTO
			{
				Hoten = n.Hoten,
				Ngaysinh = n.Ngaysinh,
				Diachi = n.Diachi,
				Gioitinh = n.Gioitinh,
				Sdt = n.Sdt,
				Trangthai = n.Trangthai,
				Password = n.Password,
				Role = n.Role
			});
		}

		public async Task<NhanvienDTO> GetNhanvienByIdAsync(int id)
		{
			var nhanvien = await _repository.GetByIdAsync(id);
			if (nhanvien == null) throw new KeyNotFoundException("Nhân viên không tồn tại.");

			return new NhanvienDTO
			{
				Hoten = nhanvien.Hoten,
				Ngaysinh = nhanvien.Ngaysinh,
				Diachi = nhanvien.Diachi,
				Gioitinh = nhanvien.Gioitinh,
				Sdt = nhanvien.Sdt,
				Trangthai = nhanvien.Trangthai,
				Password = nhanvien.Password,
				Role = nhanvien.Role
			};
		}

		public async Task AddNhanvienAsync(NhanvienDTO nhanvienDto)
		{
			var nhanvien = new Nhanvien
			{
				Hoten = nhanvienDto.Hoten,
				Ngaysinh = nhanvienDto.Ngaysinh,
				Diachi = nhanvienDto.Diachi,
				Gioitinh = nhanvienDto.Gioitinh,
				Sdt = nhanvienDto.Sdt,
				Trangthai = 0, // Mặc định "hoạt động"
				Password = nhanvienDto.Password,
				Role = nhanvienDto.Role // 0: Quản lý, 1: Nhân viên
			};
			await _repository.AddAsync(nhanvien);
		}

		public async Task UpdateNhanvienAsync(int id, NhanvienDTO nhanvienDto)
		{
			var nhanvien = await _repository.GetByIdAsync(id);
			if (nhanvien == null) throw new KeyNotFoundException("Nhân viên không tồn tại.");

			nhanvien.Hoten = nhanvienDto.Hoten;
			nhanvien.Ngaysinh = nhanvienDto.Ngaysinh;
			nhanvien.Diachi = nhanvienDto.Diachi;
			nhanvien.Gioitinh = nhanvienDto.Gioitinh;
			nhanvien.Sdt = nhanvienDto.Sdt;
			nhanvien.Password = nhanvienDto.Password;

			await _repository.UpdateAsync(nhanvien);
		}

		public async Task DeleteNhanvienAsync(int id)
		{
			await _repository.DeleteAsync(id);
		}
        public async Task<IEnumerable<NhanvienDTO>> TimKiemNhanvienAsync(string search)
        {
            var nhanviens = await _repository.TimKiemNhanvienAsync(search);
            return nhanviens.Select(n => new NhanvienDTO
            {
                Hoten = n.Hoten,
                Ngaysinh = n.Ngaysinh,
                Diachi = n.Diachi,
                Gioitinh = n.Gioitinh,
                Sdt = n.Sdt,
                Trangthai = n.Trangthai,
                Password = n.Password,
                Role = n.Role
            });
        }
    }
}
