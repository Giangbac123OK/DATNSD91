using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Models;

namespace AppData.IRepository
{
	public interface IThuoctinhRepository
	{
		Task<IEnumerable<Thuoctinh>> GetAll();
		Task<Thuoctinh> GetById(int id);
		Task Add(Thuoctinh thuoctinh);
		Task Update(Thuoctinh thuoctinh);
		Task Delete(int id);

	}
}
