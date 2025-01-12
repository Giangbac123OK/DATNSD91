using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto;

namespace AppData.IService
{
    public interface KhachHang_InhacungcapService
    {
        Task<NhacungcapDto> GetByIdAsync(int id);
        Task<IEnumerable<NhacungcapDto>> GetAllAsync();
        Task AddAsync(NhacungcapDto nhaCungCapDto);
        Task UpdateAsync(int id, NhacungcapDto nhaCungCapDto);
        Task DeleteAsync(int id);
    }
}
