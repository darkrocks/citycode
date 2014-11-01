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
		public SightTypeModel Create(City city, SightType entity)
		{
			var model = new SightTypeModel()
			{
				Id = entity.Id
			};
			switch (_languageService.CurrentLanguage)
			{
				case Languages.En:
					model.Name = entity.NameEn;
					model.PluralName = entity.PluralNameEn;
					break;
				case Languages.Ru:
					model.Name = entity.NameRu;
					model.PluralName = entity.PluralNameRu;
					break;
			}

			model.UrlParam = _urlService.PrepareToUrl(articleListTitleService.Generate(city, entity)); 
			return model;
		}
	}
}