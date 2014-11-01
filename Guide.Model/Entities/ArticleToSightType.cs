using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guide.Model.Entities
{
	public class ArticleToSightType
	{
		[Key, Column(Order = 0)]
		[Required]
		public int ArticleId { get; set; }
		public Article Article { get; set; }

		[Key, Column(Order = 1)]
		[Required]
		public int SightTypeId { get; set; }
		public SightType SightType { get; set; }
	}
}
