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
        public CountryModel Create(Country entity)
        {
            var model = new CountryModel()
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

            return model;
        }
    }
}