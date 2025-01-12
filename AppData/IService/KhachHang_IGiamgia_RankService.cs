using AppData.Dto;
using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IService
{
    public interface KhachHang_IGiamgia_RankService
    {
        Task<IEnumerable<giamgia_rank>> GetAllAsync();
        Task<giamgia_rank> GetByIdAsync(int id);
        Task<List<Giamgia_RankDTO1>> GetByIdRankSPCTAsync(int idspct);
        Task AddAsync(Giamgia_RankDTO1 dto);
    }
}
