using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guide.Model.Model
{
	using System.Web.Mvc;

	using Guide.Model.Entities;

	public class ArticleEditModel: Article
	{
		[DisplayNameAttribute("Sight Types")]
		public int[] SightTypeIds { get; set; }

		public string ThumbnailImageUrl { get; set; }
		[NotMapped]
		public List<Image> Images { get; set; }
		[NotMapped]
		public List<Image> QrImages { get; set; }
		[NotMapped]
		public object File { get; set; }
		[NotMapped]
		public bool FetchFromFoursquare { get; set; }
	}
}
