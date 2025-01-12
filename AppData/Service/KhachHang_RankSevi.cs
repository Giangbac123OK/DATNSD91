using AppData.Dto;
using AppData.IRepository;
using AppData.IService;
using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Service
{
	public class KhachHang_RankSevi : KhachHang_IRankServiece
	{
		private readonly KhachHang_IRankRepos _repos;
		public KhachHang_RankSevi(KhachHang_IRankRepos repos)
		{
			_repos = repos;
		}
		public async Task AddRankDTOAsync(RankDTO rankDto)
		{
			var asf = new Rank()
			{
                Tenrank = rankDto.TenRank,
                MaxMoney = rankDto.MaxMoney,
                MinMoney = rankDto.MinMoney,
				Trangthai = 0,
				
			};
			await _repos.AddAsync(asf);
		}

		public async Task DeleteRankAsync(int id)
		{
			await _repos.DeleteAsync(id);
		}

		public async Task<IEnumerable<RankDTO>> GetAllRanksAsync()
		{
			var a = await _repos.GetAllAsync();
			return a.Select(x => new RankDTO()
			{
				TenRank = x.Tenrank,
				MaxMoney = x.MaxMoney,
				MinMoney = x.MinMoney,
				trangthai = x.Trangthai,
			});
		}

		public async Task<RankDTO> GetRankByIdAsync(int id)
		{
			var x = await _repos.GetByIdAsync(id);
			return new RankDTO()
			{
				TenRank = x.Tenrank,
				MaxMoney = x.MaxMoney,
				MinMoney = x.MinMoney,
				trangthai = x.Trangthai,
			};
		}

		public async Task UpdateRankAsync(int id, RankDTO rankDTO)
		{
			var x = await _repos.GetByIdAsync(id);
			if (x == null) throw new KeyNotFoundException("Khách hàng không tồn tại.");
			x.Tenrank = rankDTO.TenRank;
			x.MaxMoney = rankDTO.MaxMoney;
			x.MinMoney = rankDTO.MinMoney;
			x.Trangthai = rankDTO.trangthai;
			await _repos.UpdateAsync(x);
				
			
		}
	}
}
