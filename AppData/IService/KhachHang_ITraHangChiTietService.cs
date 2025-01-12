using AppData.Dto;
using AppData.Models;
using AppData.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IService
{
    public interface KhachHang_ITraHangChiTietService
    {
        Task<List<TraHangChiTietDTO>> GetAll();
        Task<TraHangChiTietDTO> GetById(int id);
        Task<List<TraHangChiTietDTO>> GetByMaHD(int id);
        Task Add(TraHangChiTietDTO ct);
        Task UpdateSoluongTra(int idhdct, int soluong);
        Task Update(int id, TraHangChiTietDTO ct);
        Task Delete(int id);
        Task<List<TrahangchitietViewModel>> ViewHoadonctTheoIdth(int id);
    }
}
