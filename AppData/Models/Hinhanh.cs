using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
	public class Hinhanh

	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id {  get; set; }
		public string Urlhinhanh { get; set; }
		public int Idth { get; set; }
		[ForeignKey("Idth")]
		public virtual Trahang Trahang { get; set; }
	}
}
