using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guide.Model.Entities
{
	using System.ComponentModel.DataAnnotations;
	using System.Web.Mvc;

	public class Article
	{
		public Article()
		{
			CityId = 1;
		}
		public int Id { get; set; }
		public bool Published { get; set; }
		[Required]
		[DisplayName("Header Ru")]
		public string HeaderRu { get; set; }
		[Required]
		[DisplayName("Header En")]
		public string HeaderEn { get; set; }
		[DisplayName("Teaser En")]
		[MaxLength(100)]
		public string TeaserEn { get; set; }
		[DisplayName("Teaser Ru")]
		[MaxLength(100)]
		public string TeaserRu { get; set; }
		[AllowHtml]
		[DisplayName("Body Ru")]
		public string BodyRu { get; set; }
		[AllowHtml]
		[DisplayName("Body En")]
		public string BodyEn { get; set; }
		public int CityId { get; set; }
		public City City { get; set; }
		[DisplayName("Street Name Ru")]
		public string StreetNameRu { get; set; }
		[DisplayName("Street Name En")]
		public string StreetNameEn { get; set; }
		[DisplayName("Building Number")]
		public int? BuildingNumber { get; set; }
		public int? Housing { get; set; }
		public string Litera { get; set; }
		public short Importance { get; set; }
		public int? ThumbnailId { get; set; }
		public Image Thumbnail { get; set; }
		public ICollection<ArticleToImage> ArticleToImages { get; set; }
		public string Lat { get; set; }
		public string Lng { get; set; }
		public ICollection<ArticleToSightType> ArticleToSightTypes { get; set; }
		public string FoursquareId { get; set; }
		public string FoursquareName { get; set; }

		public string Phone { get; set; }
		public string Email { get; set; }
		public string Website { get; set; }
		public string UrlParam { get; set; }

		[NotMapped]
		public string ThumbnailUrl { get; set; }
		[NotMapped]
		public string Categories { get; set; }

	}
}
