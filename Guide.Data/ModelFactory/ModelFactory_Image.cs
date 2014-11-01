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
		public ImageModel Create(Image entity, int articleId, int rank, string captionRu, string captionEn)
		{
			var model = new ImageModel()
			{
				ImageId = entity.Id,
				ArticleId = articleId,
				FileName = entity.FileName,
				Url = _urlService.GetImageUrl(entity.FileName),
				Rank = rank
			};
			switch (_languageService.CurrentLanguage)
			{
				case Languages.En:
					model.Caption = captionEn;
					break;
				case Languages.Ru:
					model.Caption = captionRu;
					break;
			}
			return model;
		}

		public Image Parse(ImageModel model)
		{
			return new Image()
			{
				Id = model.ImageId,
				FileName = model.FileName
			};
		}
		public Image Gridify(Image entity)
		{
			return Gridify(entity, ImageTypes.Article);
		}
		public Image Gridify(Image entity, ImageTypes imageType = ImageTypes.Article)
		{
			entity.Url = _urlService.GetImageUrl(entity.FileName, imageType);
			return entity;
		}
	}
}