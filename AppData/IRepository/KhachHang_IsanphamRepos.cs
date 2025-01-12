using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Models;
using AppData.ViewModel;

namespace AppData.IRepository
{
	public interface KhachHang_IsanphamRepos
	{
		Task<IEnumerable<Sanpham>> GetAllAsync();
		Task<Sanpham> GetByIdAsync(int id);
		Task AddAsync(Sanpham sanpham);
		Task UpdateAsync(Sanpham sanpham);
		Task DeleteAsync(int id);
		Task<IEnumerable<Sanpham>> SearchByNameAsync(string name);
        Task<IEnumerable<SanphamViewModel>> GetAllSanphamViewModels();
        Task<SanphamViewModel> GetSanphamViewModelByIdSP(int idsp);
        Task<IEnumerable<SanphamViewModel>> GetAllSanphamGiamGiaViewModels();
        Task<IEnumerable<SanphamViewModel>> GetAllSanphamByThuongHieu(int id );
        Task<IEnumerable<SanphamViewModel>> GetSanphamByThuocTinh(
      List<string> tenThuocTinhs, // Danh sách tên thuộc tính cần tìm
      decimal? giaMin = null,
      decimal? giaMax = null,
      int? idThuongHieu = null);
        
    }
}
