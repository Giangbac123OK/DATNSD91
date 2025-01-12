using AppData.Dto;
using AppData.Models;

namespace AppData.IRepository
{
    public interface KhachHang_ISanphamchitietRepos
    {
        Task<IEnumerable<Sanphamchitiet>> GetAllAsync();
        Task<Sanphamchitiet> GetByIdAsync(int id);
        Task<List<Sanphamchitiet>> GetByIdSPAsync(int idsp);
        Task<List<Thuoctinhsanphamchitiet>> GetByIdTTSPCTAsync(int idspct);
        Task<List<Thuoctinhsanphamchitiet>> GetByISPCTAsync(List<string> tenthuoctinh);
        Task<Sanphamchitiet> AddAsync(Sanphamchitiet entity);
        Task<Sanphamchitiet> UpdateAsync(Sanphamchitiet entity);
        Task AddThuoctinhsanphamchitiet(int idsp, int idspct, int idtt, List<string> tenthuoctinhchitietList);
        Task DeleteAsync(int id);
    }
}
