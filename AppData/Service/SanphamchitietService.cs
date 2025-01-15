using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto_Admin;
using AppData.IRepository;
using AppData.IService;
using AppData.Models;
using AppData.Repository;

namespace AppData.Service
{
	public class SanphamchitietService : ISanphamchitietService
	{
		private readonly ISanphamchitietRepository _repository;
		private readonly ISanPhamservice _sprepository;
		private readonly IthuoctinhsanphamchitietRepository _ttspctrepository;
		private readonly IsanphamRepos _spRepository;
		private readonly MyDbContext _context;

		public SanphamchitietService(ISanphamchitietRepository repository, ISanPhamservice sprepository, MyDbContext context, IsanphamRepos spRepository, IthuoctinhsanphamchitietRepository ttspctrepository)
		{
			_repository = repository;
			_sprepository = sprepository;
			_context = context;
			_spRepository = spRepository;
			_ttspctrepository = ttspctrepository;


		}

		public async Task<IEnumerable<Sanphamchitiet>> GetAllAsync()
		{
			
			var entities = await _repository.GetAllAsync();
			
			return entities.Select(sp => new Sanphamchitiet
			{
				Id = sp.Id,
				Mota = sp.Mota,
				Giathoidiemhientai =sp.Giathoidiemhientai,
				Soluong = sp.Soluong,
				Trangthai= sp.Trangthai,
				Idsp = sp.Idsp,
				


		});
		}

		// Phương thức lấy sản phẩm chi tiết theo Id
		public async Task<Sanphamchitiet> GetByIdAsync(int id)
		{
			var entity = await _repository.GetByIdAsync(id);
			if (entity == null) return null;

			return new Sanphamchitiet
			{
				Id = entity.Id,
				Mota = entity.Mota,
				Trangthai = entity.Trangthai,
				Soluong = entity.Soluong,
				Idsp = entity.Idsp,
				
			};
		}

		// Phương thức  phẩm chi tiết
		public async Task AddAsync(SanphamchitietDto dto)
		{
			var sanpham = await _sprepository.GetByIdAsync(dto.Idsp);
			var entity = new Sanphamchitiet
			{
				Mota = dto.Mota,
				Soluong = dto.Soluong,
				Trangthai = dto.Soluong > 0 ? 0 : 1,
				Giathoidiemhientai = sanpham.Giaban,
				Idsp = dto.Idsp
				// Không cần thiết cập nhật Trangthai và Giathoidiemhientai ở đây
				// vì chúng sẽ được tính tự động khi truy xuất
			};
			await _repository.AddAsync(entity);
		}

		// Phương thức cập nhật sản phẩm chi tiết
		public async Task UpdateAsync(SanphamchitietDto dto,int id)
		{
			var sanpham = await _sprepository.GetByIdAsync(dto.Idsp);
			var entity = await _repository.GetByIdAsync(id);
			if (entity != null)
			{
				entity.Mota = dto.Mota;
				entity.Soluong = dto.Soluong;
				if (entity.Trangthai == 2)
				{
					entity.Trangthai = 2;
				}
				else
				{
					if (dto.Soluong > 0)
					{
						entity.Trangthai = 0;

					}
					else if (dto.Soluong == 0)
					{
						entity.Trangthai = 1;
					}
				}
				entity.Idsp = dto.Idsp;
				//entity.Trangthai = dto.Soluong > 0 ? 0 : 1;
				entity.Giathoidiemhientai = sanpham.Giaban;
				
				await _repository.UpdateAsync(entity);
			}
		}

		// Phương thức xóa sản phẩm chi tiết
		public async Task DeleteAsync(int id)
		{
			await _repository.DeleteAsync(id);
		}

		public Task UpdateAsync(SanphamchitietDto dto)
		{
			throw new NotImplementedException();
		}

		

	
		public async Task UpdateSanphamchitietAndThuoctinhAsync(int idspct, UpdateSanphamchitietDTO dto)
		{
			using var transaction = await _context.Database.BeginTransactionAsync();
			try
			{
				// Lấy Sanphamchitiet hiện tại
				var sanphamchitiet = await _repository.GetByIdAsync(idspct);
				if (sanphamchitiet == null)
					throw new Exception("Sanphamchitiet không tồn tại");

				// Lưu lại số lượng cũ để cập nhật Sanpham
				int soluongCu = sanphamchitiet.Soluong;

				// Cập nhật Soluong và Mota
				sanphamchitiet.Soluong = dto.Soluong;
				sanphamchitiet.Mota = dto.Mota;

				if (sanphamchitiet.Trangthai == 2)
				{
					sanphamchitiet.Trangthai =2;
				}
				else
				{
					if (dto.Soluong == 0)
					{
						sanphamchitiet.Trangthai = 1;
					}
					else
					{
						sanphamchitiet.Trangthai = 0;
					}
				}
				

				await _repository.UpdateAsync(sanphamchitiet);

				// Cập nhật các Thuoctinhsanphamchitiet
				if (dto.Thuoctinhsanphamchitiets != null)
				{
					foreach (var ttDto in dto.Thuoctinhsanphamchitiets)
					{
						// Lấy Thuoctinhsanphamchitiet dựa trên idtt và idspct
						var thuoctinh = await _ttspctrepository.GetByIdAsync(ttDto.Idtt, idspct);
						if (thuoctinh == null)
							throw new Exception($"Thuoctinhsanphamchitiet không tồn tại với Idtt: {ttDto.Idtt}");

						// Cập nhật Tenthuoctinhchitiet
						thuoctinh.Tenthuoctinhchitiet = ttDto.Tenthuoctinhchitiet;
						await _ttspctrepository.UpdateAsync(thuoctinh);
					}
				}

				// Cập nhật Soluong của Sanpham
				var sanpham = await _spRepository.GetByIdAsync(sanphamchitiet.Idsp);
				if (sanpham == null)
					throw new Exception("Sanpham không tồn tại");

				sanpham.Soluong = sanpham.Soluong - soluongCu + dto.Soluong;

				if (sanpham.Soluong == 0)
				{
					sanpham.Trangthai = 1;
				}

				await _spRepository.UpdateAsync(sanpham);

				

				// Commit transaction
				await transaction.CommitAsync();
			}
			catch (Exception)
			{
				// Rollback transaction nếu có lỗi
				await transaction.RollbackAsync();
				throw;
			}
		}
		
		public async Task<int> UpdateTrangThaiAsync(int id)
		{
			var sanphamchitiet = await _repository.GetByIdAsync(id);
			var sanpham = await _spRepository.GetByIdAsync(sanphamchitiet.Idsp);
			if (sanphamchitiet == null)
			{
				throw new Exception("Sản phẩm chi tiết không tồn tại");
			}

			if (sanphamchitiet.Trangthai == 2)
			{
				if (sanphamchitiet.Soluong == 0)
				{
					sanphamchitiet.Trangthai = 1; // Trạng thái = 1 nếu số lượng = 0
				}
				else
				{
					sanpham.Soluong += sanphamchitiet.Soluong;
					sanphamchitiet.Trangthai = 0; // Trạng thái = 0 nếu số lượng > 0
				}
			}
			else
			{
				sanpham.Soluong -= sanphamchitiet.Soluong;
				sanphamchitiet.Trangthai = 2; // Trạng thái = 2 nếu ban đầu không phải là 2
			}
			
			await _repository.UpdateAsync(sanphamchitiet);
			await _spRepository.UpdateAsync(sanpham);
			return sanphamchitiet.Trangthai;
		}

		public async Task<bool> XoaSanphamchitietAsync(int spctId)
		{
			var spct = await _repository.GetByIdAsync(spctId);
			if (spct == null)
			{
				return false; // Sản phẩm chi tiết không tồn tại
			}

			// Kiểm tra nếu spct tồn tại trong bảng Giohangchitiet và Salechitiet
			var giohangchitiets = await _repository.GetGiohangchitietBySpctIdAsync(spctId);
			var salechitiets = await _repository.GetSalechitietBySpctIdAsync(spctId);

			if (giohangchitiets.Any())
			{
				// Xóa Giohangchitiet
				foreach (var giohangchitiet in giohangchitiets)
				{
					_repository.DeleteGiohangchitiet(giohangchitiet);
				}
			}

			if (salechitiets.Any())
			{
				// Xóa Salechitiet
				foreach (var salechitiet in salechitiets)
				{
					_repository.DeleteSalechitiet(salechitiet);
				}
			}

			// Kiểm tra nếu spct tồn tại trong bảng Hoadonchitiet
			var hoadonchitiets = await _repository.GetHoadonchitietBySpctIdAsync(spctId);
			if (hoadonchitiets.Any())
			{
				// Cập nhật trạng thái của spct thành 3 (đã xóa)
				spct.Trangthai = 3;
				// Trừ số lượng của sản phẩm trong bảng Sanpham (vì SPCT được xóa)
				var sanpham = spct.Sanpham;
				sanpham.Soluong -= spct.Soluong;

				// Lưu thay đổi vào cơ sở dữ liệu
				await _repository.SaveChangesAsync();
			}
			else
			{
				// Cập nhật số lượng sản phẩm khi xóa sản phẩm chi tiết (trừ đi số lượng SPCT)
				var sanpham = spct.Sanpham;
				sanpham.Soluong -= spct.Soluong; // Trừ số lượng SPCT khỏi sản phẩm chính

				// Xóa sản phẩm chi tiết
				_repository.DeleteSanphamchitiet(spct);

				await _repository.SaveChangesAsync();
			}

			// Lưu thay đổi vào cơ sở dữ liệu (nếu có thay đổi nào chưa được lưu)
			await _repository.SaveChangesAsync();

			return true;
		}
	}
	}
	
