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
    public interface KhachHang_ITraHangService
    {
        Task<List<TraHangDTO>> GetAll();
        Task<TraHangDTO> GetById(int id);
        Task Add(TraHangDTO trahang);
        Task Update(int id, TraHangDTO trahang);
        Task DeleteById(int id);
        Task Trahangquahan();
        Task<List<TraHangViewModel>> ViewHoaDonTraByIdkh(int id);
    }
}
