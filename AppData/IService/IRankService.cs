using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto_Admin;
using AppData.Models;

namespace AppData.IService
{
	public interface IRankService
	{
		Task AddRankAsync(RankDto rankDto);
		Task<IEnumerable<Rank>> GetAllRanksAsync();
		Task<RankDto> GetRankByIdAsync(int id);
		Task UpdateRankAsync(int id, RankDto rankDto);
		Task<bool> DeleteRankAsyncThao(int Id);
		Task ToggleTrangthaiAsync(int rankId);
		Task<IEnumerable<RankDto>> SearchRanksAsync(string keyword);
	}
}
