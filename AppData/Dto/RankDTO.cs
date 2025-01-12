using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Dto
{
	public class RankDTO
	{
		[Required(ErrorMessage = "Tên rank là bắt buộc.")]
		[StringLength(100, MinimumLength = 1, ErrorMessage = "Tên rank phải có ít nhất 1 ký tự và tối đa 100 ký tự.")]
		public string TenRank { get; set; }

		[Range(0, double.MaxValue, ErrorMessage = "Số tiền tối thiểu không được nhỏ hơn 0.")]
		public decimal MinMoney { get; set; }

		[Range(0, double.MaxValue, ErrorMessage = "Số tiền tối đa không được nhỏ hơn 0.")]
		public decimal MaxMoney { get; set; }

		
		public int trangthai { get; set; }

		public bool ValidateMaxGreaterThanMin()
		{
			if (MaxMoney < MinMoney)
			{
				throw new ValidationException("Số tiền tối đa phải lớn hơn hoặc bằng số tiền tối thiểu.");
			}
			return true;
		}
	}
}
