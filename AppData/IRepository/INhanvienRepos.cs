using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Models;

namespace AppData.IRepository
{
    public interface INhanvienRepos
    {
        Task<IEnumerable<Nhanvien>> GetAllAsync();
        Task<Nhanvien> GetByIdAsync(int id);
        Task AddAsync(Nhanvien nhanvien);
        Task UpdateAsync(Nhanvien nhanvien);
        Task DeleteAsync(int id);
		Task<Nhanvien> GetNhanvienByEmailAsync(string email);
		Task<Nhanvien> GetBySdtAndPasswordAsync(string email, string password);
        Task<Nhanvien> GetByEmailAsync(string email);
        Task SaveChangesAsync();
        IEnumerable<Nhanvien> SearchByName(string name);
        IEnumerable<Nhanvien> SearchByPhoneNumber(string phoneNumber);
        IEnumerable<Nhanvien> SearchByEmail(string email);
		Task<bool> CheckForeignKeyConstraintAsync(int nhanvienId);
		
		Task UpdateNhanvienAsync(Nhanvien nhanvien);
		bool EmailExists(string email, int? excludeId = null);
	}
}
