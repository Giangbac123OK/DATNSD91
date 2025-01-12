using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto;

namespace AppData.IService
{
    public interface KhachHang_IGiamgiaService
    {
        Task<IEnumerable<GiamgiaDTO>> GetAllAsync();
        Task<GiamgiaDTO> GetByIdAsync(int id);
        Task AddAsync(GiamgiaDTO dto);
        Task UpdateAsync(int id, GiamgiaDTO dto);
        Task AddRankToGiamgia(Giamgia_RankDTO dto);
        Task DeleteAsync(int id);
    }
}
