using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Dto_Admin;
using AppData.IRepository;
using AppData.IService;

namespace AppData.Service
{
	// TrahangService.cs
	public class TrahangService : ITrahangService
	{
		private readonly ITrahangRepository _repository;

		public TrahangService(ITrahangRepository repository)
		{
			_repository = repository;
		}

		public async Task<TrahangDto> GetTrahangByHoadonIdAsync(int hoadonId)
		{
			return await _repository.GetTrahangByHoadonIdAsync(hoadonId);
		}
	}

}
