using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guide.Model.Entities
{
	public class ArticleToImage
	{
		[Key, Column(Order = 0)]
		[Required]
		public int ArticleId { get; set; }
		public Article Article { get; set; }

		[Key, Column(Order = 1)]
		[Required]
		public int ImageId { get; set; }
		public Image Image { get; set; }

		public int Rank { get; set; }
		[DisplayName("Caption Ru")]
		[MaxLength(100)]
		public string CaptionRu { get; set; }
		[DisplayName("Caption En")]
		[MaxLength(100)]
		public string CaptionEn { get; set; }
	}
}
