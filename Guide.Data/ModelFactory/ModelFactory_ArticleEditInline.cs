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
		public ArticleEditInlineModel CreateArticleEditInlineModel(Article entity)
		{
			var model = new ArticleEditInlineModel()
			{
				Id = entity.Id,
				HeaderEn = entity.HeaderEn,
				HeaderRu = entity.HeaderRu,
				Published = entity.Published,
				City = entity.City
			};

			if (entity.Thumbnail != null)
			{
				model.ThumbnailImageUrl = _urlService.GetImageUrl(entity.Thumbnail.FileName);
			}
			
			return model;
		}
	}
}