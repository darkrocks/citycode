using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guide.Config;
using Guide.Model.Entities;
using Guide.Model.Model;
using Guide.Services;

namespace Guide.Data.ModelFactory
{
	public partial class ModelFactory
	{
		public ArticleModel Create(Article entity)
		{
			var model = new ArticleModel()
			{
				ArticleId = entity.Id
			};
			switch (_languageService.CurrentLanguage)
			{
				case Languages.En:
					model.Body = entity.BodyEn;
					model.Teaser = entity.TeaserEn;
					model.Header = entity.HeaderEn;
					model.StreetName = entity.StreetNameEn;
					break;
				case Languages.Ru:
					model.Body = entity.BodyRu;
					model.Teaser = entity.TeaserRu;
					model.Header = entity.HeaderRu;
					model.StreetName = entity.StreetNameRu;
					break;
			}
			model.BuildingNumber = entity.BuildingNumber;
			model.Housing = entity.Housing;
			model.Litera = entity.Litera;
			model.Importance = entity.Importance;
			model.Coordinate1 = entity.Lat;
			model.Coordinate2 = entity.Lng;
			model.FoursquareId = entity.FoursquareId;
			model.Website = entity.Website;
			model.Email = entity.Email;
			model.Phone = entity.Phone;
			model.UrlParam = _urlService.PrepareToUrl(model.Header);
			model.Images = new List<ImageModel>();
			if (entity.City != null) model.City = Create(entity.City);

			if (entity.Thumbnail != null)
			{
				model.ThumbImageUrl = this.Create(entity.Thumbnail, entity.Id, 1, entity.HeaderRu, entity.HeaderEn).Url;	
			}

			if (entity.ArticleToImages != null)
			{
				var images = entity.ArticleToImages.
					Where(ati => ati.Image.ImageType == (short)ImageTypes.Article).
					OrderBy(ati => ati.Rank).
					Select(ati => this.Create(ati.Image, ati.ArticleId, ati.Rank, ati.CaptionRu, ati.CaptionEn)).
					ToList();
				if (images.Count > 0)
				{
					model.Images = images;
				}
			}

			if (entity.City != null && entity.City.Country != null)
			{
				string building = "";
				if (entity.BuildingNumber != null)
				{
					building = ", " + entity.BuildingNumber.ToString();
					if (entity.Housing != null)
					{
						building += "/" + entity.Housing.ToString();
					}

					if (!string.IsNullOrWhiteSpace(entity.Litera))
					{
						switch (_languageService.CurrentLanguage)
						{
							case Languages.En:
								building += " " + entity.Litera;
								break;
							case Languages.Ru:
								building += " литера " + entity.Litera;
								break;
						}
					}	
				}
				
				switch (_languageService.CurrentLanguage)
				{
					case Languages.En:
						model.Address = string.Format("{0}, {1}, {2}{3}", entity.City.Country.NameEn, entity.City.NameEn, entity.StreetNameEn,
							building);
						break;
					case Languages.Ru:
						model.Address = string.Format("{0}, {1}, {2}{3}", entity.City.Country.NameRu, entity.City.NameRu, entity.StreetNameRu,
					building);
						break;
				}	
			}

			if (string.IsNullOrWhiteSpace(entity.FoursquareId))
			{
				switch (_languageService.CurrentLanguage)
				{
					case Languages.En:
						model.FoursquareUrl = string.Format("https://en.foursquare.com/v/{0}", entity.FoursquareId);
						break;
					case Languages.Ru:
						model.FoursquareUrl = string.Format("https://ru.foursquare.com/v/{0}", entity.FoursquareId);
						break;
				}	
			}
			
			return model;
		}

		public Article Gridify(Article entity)
		{
			if (entity.Thumbnail != null)
			{
				entity.ThumbnailUrl = _urlService.GetImageUrl(entity.Thumbnail.FileName);
			}

			if (entity.ArticleToSightTypes != null)
			{
				var sb = new StringBuilder();
				foreach (var articleToSightType in entity.ArticleToSightTypes)
				{
					if (articleToSightType.SightType != null)
					{
						sb.Append(articleToSightType.SightType.NameRu);
						sb.Append(", ");	
					}
				}
				entity.Categories = sb.ToString();
			}
			entity.Thumbnail = null;
			entity.City = null;
			entity.ArticleToSightTypes = null;
			entity.ArticleToImages = null;

			return entity;
		}
	}
}