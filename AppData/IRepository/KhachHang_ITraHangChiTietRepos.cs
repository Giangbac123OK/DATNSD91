using AppData.Models;
using AppData.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.IRepository
{
    public interface KhachHang_ITraHangChiTietRepos
    {
        Task<List<Trahangchitiet>> GetAll();
        Task<Trahangchitiet> GetById(int id);
        Task<List<Trahangchitiet>> GetByMaHD(int id);
        Task Add(Trahangchitiet ct);
        Task Update(Trahangchitiet ct);
        Task Delete(int id);
        Task UpdateSoluongTra(int idhdct, int soluong);
        Task<List<TrahangchitietViewModel>> ViewHoadonctTheoIdth(int id);
    }
}
