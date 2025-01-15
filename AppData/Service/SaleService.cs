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
	public class SaleService:ISaleService
	{
		private readonly IsaleRepos _repository;

		private readonly ISanphamchitietRepository _sanphamchitietRepository;
		private readonly IsalechitietRepos _salechitietRepository;
		private readonly IsanphamRepos _sanphamRepository;
		

		public SaleService(IsaleRepos repository, ISanphamchitietRepository sanphamchitietRepository, IsalechitietRepos salechitietRepository, IsanphamRepos sanphamRepository)
        {
			_repository = repository;
			_sanphamRepository=sanphamRepository;
			_sanphamchitietRepository = sanphamchitietRepository;
			_salechitietRepository = salechitietRepository;
		}
		public async Task<bool> UpdateSaleWithDetailsAsync(int saleId, CreateSaleDto updateSaleDto)
		{
			// Lấy thông tin Sale từ cơ sở dữ liệu
			var sale = await _repository.GetByIdAsync(saleId);
			if (sale == null)
			{
				return false; // Sale không tồn tại
			}

			if (updateSaleDto.Ngaybatdau > DateTime.Now)
			{
				sale.Trangthai = 2;
			}
			else if (updateSaleDto.Ngaybatdau <= DateTime.Now && updateSaleDto.Ngaybatdau >= DateTime.Now)
			{
				sale.Trangthai = 0;
			}
			else
			{
				sale.Trangthai = 1;
			}
			sale.Ten = updateSaleDto.Ten;
			sale.Mota = updateSaleDto.Mota;
			sale.Ngaybatdau = updateSaleDto.Ngaybatdau;
			sale.Ngayketthuc = updateSaleDto.Ngayketthuc;

			// Danh sách Salechitiet hiện có
			var currentDetails = sale.Salechitiets.ToList();

			// Duyệt qua các chi tiết mới từ DTO
			foreach (var newDetail in updateSaleDto.SaleDetails)
			{
				var existingDetail = currentDetails
					.FirstOrDefault(d => d.Idspct == newDetail.Idspct);

				if (existingDetail != null)
				{
					// Cập nhật chi tiết đã tồn tại
					existingDetail.Donvi = newDetail.Donvi;
					existingDetail.Soluong = newDetail.Soluong;
					existingDetail.Giatrigiam = newDetail.Giatrigiam;
				}
				else
				{
					// Thêm chi tiết mới
					var saleDetail = new Salechitiet
					{
						Idsale = saleId,
						Idspct = newDetail.Idspct,
						Donvi = newDetail.Donvi,
						Soluong = newDetail.Soluong,
						Giatrigiam = newDetail.Giatrigiam
					};
					sale.Salechitiets.Add(saleDetail);
				}
			}

			// Xóa các chi tiết không còn trong danh sách mới
			var newDetailIds = updateSaleDto.SaleDetails.Select(d => d.Idspct).ToList();
			var detailsToRemove = currentDetails
				.Where(d => !newDetailIds.Contains(d.Idspct))
				.ToList();

			foreach (var detail in detailsToRemove)
			{
				sale.Salechitiets.Remove(detail);
			}

			// Lưu thay đổi vào cơ sở dữ liệu
			await _repository.SaveChangesAsync();

			return true;
		}
		public async Task<bool> AddSaleWithDetailsAsync(CreateSaleDto createSaleDto)
		{
			DateTime cs = DateTime.Now;
			if(createSaleDto.Ngaybatdau> cs)
			{
				createSaleDto.Trangthai = 2;
			}
			else if (createSaleDto.Ngaybatdau <= cs && createSaleDto.Ngayketthuc>= cs)
			{
				createSaleDto.Trangthai = 0;
			}
			else
			{
				createSaleDto.Trangthai = 1;
			}
			var sale = new Sale
			{
				
				Ten = createSaleDto.Ten,
				Mota = createSaleDto.Mota,
				
				Trangthai = createSaleDto.Trangthai,
				Ngaybatdau = createSaleDto.Ngaybatdau,
				Ngayketthuc = createSaleDto.Ngayketthuc,
				Salechitiets = createSaleDto.SaleDetails.Select(d => new Salechitiet
				{
					Idspct = d.Idspct,
					Donvi = d.Donvi,
					Soluong = d.Soluong,
					Giatrigiam = d.Giatrigiam
				}).ToList()
			};

			await _repository.AddSaleAsync(sale);
			await _repository.SaveChangesAsync();
			return true;
		}
		public async Task<IEnumerable<Sale>> GetAllWithIdAsync()
		{
			return await _repository.GetAllAsync();
		}

		public async Task<SaleDto?> GetByIdAsync(int id)
		{
			var sale = await _repository.GetByIdAsync(id);
			if (sale == null) return null;

			return new SaleDto
			{
				Ten = sale.Ten,
				Mota = sale.Mota,
				Trangthai = sale.Trangthai,
				Ngaybatdau = sale.Ngaybatdau,
				Ngayketthuc = sale.Ngayketthuc
			};
		}

		public async Task<SaleDto> AddAsync(SaleDto saleDto)
		{
		
			

			// Kiểm tra ngày kết thúc
			if (saleDto.Ngayketthuc < saleDto.Ngaybatdau)
			{
				throw new ArgumentException("Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu.");
			}

			// Tạo một đối tượng Sale mới
			Sale newSale = new Sale
			{
				Ten = saleDto.Ten,
				Mota = saleDto.Mota,
				Ngaybatdau = saleDto.Ngaybatdau,
				Ngayketthuc = saleDto.Ngayketthuc,
			};

			// Xác định trạng thái
			if (saleDto.Ngaybatdau <= DateTime.Now && saleDto.Ngayketthuc >= DateTime.Now)
			{
				newSale.Trangthai = 0; // Đang diễn ra
			}
			else if (saleDto.Ngaybatdau > DateTime.Now)
			{
				newSale.Trangthai = 1; // Chuẩn bị diễn ra
			}
			else /*if (saleDto.Ngayketthuc < DateTime.Now)*/
			{
				newSale.Trangthai = 2; // Đã diễn ra
			}

			await _repository.AddAsync(newSale);
			saleDto.Id = newSale.Id;
			return saleDto;

		}
		public async Task<List<SaleDetailDTO>> GetSaleDetailsAsync(int saleId)
		{
			return await _repository.GetSaleDetailsBySaleIdAsync(saleId);
		}
		public async Task UpdateStatusLoad(int id)
		{

			var sale = await _repository.GetByIdAsync(id);
			if (sale == null)
			{
				throw new KeyNotFoundException("Sale không tồn tại");
			}
			if (sale.Trangthai != 3)
			{
				// Cập nhật trạng thái dựa trên ngày bắt đầu và ngày kết thúc
				if (sale.Ngaybatdau <= DateTime.Now && sale.Ngayketthuc >= DateTime.Now)
				{
					sale.Trangthai = 0; // Đang diễn ra
				}
				else if (sale.Ngaybatdau > DateTime.Now)
				{
					sale.Trangthai = 1; // Chuẩn bị diễn ra
				}
				else if (sale.Ngayketthuc < DateTime.Now)
				{
					sale.Trangthai = 2; // Đã diễn ra
				}
			}
			

			await _repository.UpdateAsync(sale);
		}
		public async Task UpdateStatusToCancelled(int id)
		{
			var sale = await _repository.GetByIdAsync(id);
			if (sale == null)
			{
				throw new KeyNotFoundException("Sale không tồn tại");
			}

			sale.Trangthai = 3; // Cập nhật trạng thái thành Hủy
			await _repository.UpdateAsync(sale);
		}
		
		public async Task UpdateStatusBasedOnDates(int id)
		{
			var sale = await _repository.GetByIdAsync(id);
			if (sale == null)
			{
				throw new KeyNotFoundException("Sale không tồn tại");
			}
			if (sale.Ngaybatdau <= DateTime.Now&& sale.Ngayketthuc >= DateTime.Now)
			{
				UpdateSanphamchitietPricesAsync(id);
				sale.Trangthai = 0;
				


			}
			else if (sale.Ngaybatdau > DateTime.Now)
			{
				sale.Trangthai = 1; // Chuẩn bị diễn ra
			}
			else if (sale.Ngayketthuc < DateTime.Now)
			{
				sale.Trangthai = 2; // Đã diễn ra
			}

			await _repository.UpdateAsync(sale);
		}
		public async Task UpdateAsync(int id, SaleDto saleDto)
		{
			// Tìm kiếm Sale theo ID
			var sale = await _repository.GetByIdAsync(id);

			if (sale == null)
			{
				throw new KeyNotFoundException($"Không tìm thấy chương trình Sale với ID = {id}.");
			}
			sale.Ten = saleDto.Ten ?? sale.Ten; // Giữ nguyên nếu giá trị mới là null
			sale.Mota = saleDto.Mota ?? sale.Mota;
			sale.Ngaybatdau = saleDto.Ngaybatdau;
			sale.Ngayketthuc = saleDto.Ngayketthuc;

			sale.Trangthai = saleDto.Trangthai;
			await _repository.UpdateAsync(sale);
		}

		public async Task<bool> DeleteSaleAsync(int id)
		{
			var sale = await _repository.GetByIdAsync(id);
			if (sale == null)
			{
				return false;
			}

			// Xóa các Salechitiet liên quan
			await _repository.DeleteSaleChitietsBySaleIdAsync(id);

			// Xóa Sale
			await _repository.DeleteSaleAsync(sale);

			// Lưu thay đổi
			await _repository.SaveChangesAsync();

			return true;
		}
		public async Task<bool> UpdateSanphamchitietPricesAsync(int saleId)
		{
			// Kiểm tra Sale tồn tại và đang ở trạng thái 0
			var sale = await _repository.GetByIdAsync(saleId);
			if (sale == null || sale.Trangthai != 0)
			{
				return false;
			}

			var sanphamchitietList = await _repository.GetSanphamchitietForUpdateAsync(saleId);
			

			foreach (var item in sanphamchitietList)
			{
				var sanphamchitiet = await _sanphamchitietRepository.GetByIdAsync(item.Id);
				if (sanphamchitiet == null)
				{
					continue; // Hoặc xử lý lỗi nếu cần
				}
				var saleChitiet = await _salechitietRepository.GetBySaleIdSanphamchitietAsync(saleId,item.Id);
				 if (saleChitiet == null) continue;

        if (saleChitiet.Donvi == 0)
        {
            sanphamchitiet.Giathoidiemhientai -= saleChitiet.Giatrigiam;
        }
        else if (saleChitiet.Donvi == 1)
        {
            sanphamchitiet.Giathoidiemhientai = sanphamchitiet.Giathoidiemhientai * (100 - saleChitiet.Giatrigiam) / 100;
        }

        if (sanphamchitiet.Giathoidiemhientai < 0) sanphamchitiet.Giathoidiemhientai = 0;
        _sanphamchitietRepository.Update(sanphamchitiet);
			}
			await _repository.SaveChangesAsync();

			return true;
		}
	}
}
