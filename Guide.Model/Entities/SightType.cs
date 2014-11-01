using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guide.Model.Entities
{
	using System.ComponentModel.DataAnnotations;

	public class SightType
	{
		public int Id { get; set; }
		[MaxLength(100)]
		[Required]
		public string NameRu { get; set; }
		[MaxLength(100)]
		[Required]
		public string NameEn { get; set; }
		[MaxLength(100)]
		[Required]
		public string PluralNameRu { get; set; }
		[MaxLength(100)]
		[Required]
		public string PluralNameEn { get; set; }
		public bool Published { get; set; }
		public virtual ICollection<ArticleToSightType> ArticleToSightTypes { get; set; }
	}
}
