using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto_Admin;
using AppData.Models;

namespace AppData.IService
{
    public interface IThuoctinhService
    {
        Task<IEnumerable<Thuoctinh>> GetAll();
        Task<Thuoctinh> GetById(int id);
        Task Add(ThuoctinhDto thuoctinhDto);
        Task Update(int id, ThuoctinhDto thuoctinhDto);
        Task Delete(int id);
    }
}
