using System;
using System.Linq;
using Guide.Config;
using Guide.Model.Entities;
using Guide.Model.Model;
using Guide.Services;

namespace Guide.Data.ModelFactory
{
	public partial class ModelFactory
	{
		public ArticleEditModel CreateArticleEditModel(Article entity)
		{
			var model = new ArticleEditModel()
			{
				Id = entity.Id,
				BodyEn = entity.BodyEn,
				BodyRu = entity.BodyRu,
				City = entity.City,
				CityId = entity.CityId,
				HeaderEn = entity.HeaderEn,
				HeaderRu = entity.HeaderRu,
				Published = entity.Published,
				StreetNameEn = entity.StreetNameEn,
				StreetNameRu = entity.StreetNameRu,
				BuildingNumber = entity.BuildingNumber,
				Housing = entity.Housing,
				Litera = entity.Litera,
				TeaserEn =  entity.TeaserEn,
				TeaserRu =  entity.TeaserRu,
				Importance = entity.Importance,
				ThumbnailId = entity.ThumbnailId,
				Thumbnail = entity.Thumbnail,
				Lat = entity.Lat,
				Lng = entity.Lng,
				FoursquareId = entity.FoursquareId,
				FoursquareName = entity.FoursquareName,
				Website = entity.Website,
				Phone = entity.Phone,
				Email = entity.Email
			};

			if (entity.ArticleToImages == null)
			{
				return model;
			}

			if (entity.Thumbnail != null)
			{
				model.ThumbnailImageUrl = _urlService.GetImageUrl(entity.Thumbnail.FileName);
			}
			if (entity.ArticleToImages != null)
			{
				var images = entity.ArticleToImages.
					Where(ati => ati.Image.ImageType == (short)ImageTypes.Article).
					OrderBy(i => i.Rank).
					Select(ati => this.Gridify(ati.Image)).
					ToList();

				if (images.Count > 0)
				{
					model.Images = images;
				}

				var qrImages = entity.ArticleToImages.
				Where(ati => ati.Image.ImageType == (short)ImageTypes.Qr740x740).
				Select(ati => this.Gridify(ati.Image, ImageTypes.Qr740x740)).
				ToList();

				if (qrImages.Count > 0)
				{
					model.QrImages = qrImages;
				}
			}

			if (entity.ArticleToSightTypes != null)
			{
				model.SightTypeIds = entity.ArticleToSightTypes.Select(ati => ati.SightTypeId).ToArray();
			}
			return model;
		}

		public Article Parse(ArticleEditModel model)
		{
			var entity = new Article
				             {
					             Id = model.Id,
								 Published = model.Published,
								 HeaderRu = model.HeaderRu,
								 HeaderEn = model.HeaderEn,
								 TeaserRu = model.TeaserRu,
								 TeaserEn = model.TeaserEn,
								 BodyEn = model.BodyEn,
								 BodyRu = model.BodyRu,
								 CityId = model.CityId,
								 StreetNameEn = model.StreetNameEn,
								 StreetNameRu = model.StreetNameRu,
								 BuildingNumber = model.BuildingNumber,
								 Housing = model.Housing,
								 Litera = model.Litera,
								 Importance = model.Importance,
								 ThumbnailId = model.ThumbnailId,
								 Lat = model.Lat,
								 Lng = model.Lng,
								 FoursquareId = model.FoursquareId,
								 Website = model.Website,
								 Phone = model.Phone,
								 Email = model.Email,
								 FoursquareName = model.FoursquareName
				             };

			return entity;
		}
	}
}