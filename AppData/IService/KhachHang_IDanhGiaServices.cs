using AppData.Dto;
using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IService
{
    public interface KhachHang_IDanhGiaServices
    {
        Task<List<DanhGiaDTO>> GetAll();
        Task<DanhGiaDTO> GetById(int id);
        Task Create(DanhGiaDTO danhGiaDTO);
        Task Update(int id, DanhGiaDTO danhGiaDTO);
        Task Delete(int id);
        Task<DanhGiaDTO> getByidHDCT(int id);
        Task<List<DanhGiaDTO>> GetByidSP(int idsp);
    }
}
