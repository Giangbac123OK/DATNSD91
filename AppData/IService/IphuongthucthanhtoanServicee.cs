using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto_Admin;

namespace AppData.IService
{
    public interface IphuongthucthanhtoanServicee
    {
        Task<IEnumerable<PhuongthucthanhtoanDTO>> GetAllAsync();
        Task<PhuongthucthanhtoanDTO> GetByIdAsync(int id);
        Task AddAsync(PhuongthucthanhtoanDTO dto);
        Task UpdateAsync(int id, PhuongthucthanhtoanDTO dto);
        Task DeleteAsync(int id);
    }
}
