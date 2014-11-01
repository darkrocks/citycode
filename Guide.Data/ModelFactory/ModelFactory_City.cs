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
        public CityModel Create(City entity)
        {
            var model = new CityModel()
            {
                Id = entity.Id
            };
            switch (_languageService.CurrentLanguage)
            {
                case Languages.En:
                    model.Name = entity.NameEn;
                    break;
                case Languages.Ru:
                    model.Name = entity.NameRu;
                    break;
            }
	        model.ThreeLetterCode = entity.ThreeLetterCode;
            if (entity.Country != null) model.Country = Create(entity.Country);
            return model;
        }
    }
}