using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto;
using AppData.Dto_Admin;
using AppData.IRepository;
using AppData.IService;
using AppData.Models;

namespace AppData.Service
{
	public class RankService: IRankService
	{
		private readonly IRankRepository _rankRepository;

		public RankService(IRankRepository rankRepository)
		{
			_rankRepository = rankRepository;
		}
		public async Task AddRankAsync(RankDto rankDto)
		{
			var rank = new Rank
			{
				Tenrank = rankDto.Tenrank,
				MinMoney = rankDto.MinMoney,
				MaxMoney = rankDto.MaxMoney,
				Trangthai = 0
			};

			await _rankRepository.AddRankAsync(rank);
		}

		public async Task<IEnumerable<Rank>> GetAllRanksAsync()
		{
			var ranks = await _rankRepository.GetAllRanksAsync();
			return ranks.Select(r => new Rank
			{
				Id = r.Id,
				Tenrank = r.Tenrank,
				MinMoney = r.MinMoney,
				MaxMoney = r.MaxMoney,
				Trangthai = r.Trangthai
			});
		}

		public async Task<RankDto> GetRankByIdAsync(int id)
		{
			var rank = await _rankRepository.GetRankByIdAsync(id);
			if (rank == null) return null;

			return new RankDto
			{
				Tenrank = rank.Tenrank,
				MinMoney = rank.MinMoney,
				MaxMoney = rank.MaxMoney,
				Trangthai = rank.Trangthai
			};
		}

		public async Task UpdateRankAsync(int id, RankDto rankDto)
		{
			var rank = await _rankRepository.GetRankByIdAsync(id);
			if (rank != null)
			{
				rank.Tenrank = rankDto.Tenrank;
				rank.MinMoney = rankDto.MinMoney;
				rank.MaxMoney = rankDto.MaxMoney;
				rank.Trangthai = rankDto.Trangthai;

				await _rankRepository.UpdateRankAsync(rank);
			}
		}

		public async Task DeleteRankAsync(int id)
		{
			await _rankRepository.DeleteRankAsync(id);
		}

		public async Task<IEnumerable<RankDto>> SearchRanksAsync(string keyword)
		{
			var ranks = await _rankRepository.SearchRanksAsync(keyword);
			return ranks.Select(r => new RankDto
			{
				Tenrank = r.Tenrank,
				MinMoney = r.MinMoney,
				MaxMoney = r.MaxMoney,
				Trangthai = r.Trangthai
			});
		}
		public async Task ToggleTrangthaiAsync(int rankId)  // Thêm async
		{
			var rank = await _rankRepository.GetRankByIdAsync(rankId);  // Lấy dữ liệu bất đồng bộ
			if (rank != null)
			{
				rank.Trangthai = (rank.Trangthai == 0) ? 1 : 0;  // Chuyển trạng thái
				await _rankRepository.UpdateRankAsync(rank);  // Cập nhật dữ liệu bất đồng bộ
			}
		}
		public async Task<bool> DeleteRankAsyncThao(int Id)
		{
			var exists = await _rankRepository.CheckForeignKeyConstraintAsync(Id);
			if (exists)
			{
				return false; // Có Hóa đơn liên quan, không thể xóa
			}

			var rank = await _rankRepository.GetRankByIdAsync(Id);
			if (rank == null) return false;

			await _rankRepository.DeleteRankAsync(Id);
			return true;
		}
	}
}
