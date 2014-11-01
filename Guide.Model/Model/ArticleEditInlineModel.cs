using System.Collections.Generic;

namespace Guide.Model.Model
{
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;

	using Guide.Model.Entities;

	public class ArticleEditInlineModel
    {
		public int Id { get; set; }
		public bool Published { get; set; }
		[Required]
		[DisplayName("Header Ru")]
		public string HeaderRu { get; set; }
		[Required]
		[DisplayName("Header En")]
		public string HeaderEn { get; set; }
		public City City { get; set; }
		public string ThumbnailImageUrl { get; set; }
    }
}
