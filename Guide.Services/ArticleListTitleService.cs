using System;

namespace Guide.Services
{
	using Guide.Config;
	using Guide.Extensions;
	using Guide.Model.Entities;
	using Guide.Services.Contracts;

	public class ArticleListTitleService : IArticleListTitleService
	{
		private readonly ILanguageService languageService;

		private readonly ICityNameCaseService cityNameCaseService;

		private readonly ITransliterationService transliterationService;

		public ArticleListTitleService(ILanguageService languageService, 
			ICityNameCaseService cityNameCaseService,
			ITransliterationService transliterationService)
		{
			this.languageService = languageService;
			this.cityNameCaseService = cityNameCaseService;
			this.transliterationService = transliterationService;
		}


		public string Generate(City city, SightType sightType)
		{
			return Generate(city, sightType, this.languageService.CurrentLanguage);
		}

		public string Generate(City city, SightType sightType, Languages language)
		{
			string typePluralName = "";
			if (city == null)
			{
				return "";
			}

			string cityPart = this.cityNameCaseService.GetName(city, PossessiveCase.Genitive, language);

			if (sightType == null)
			{
				
				switch (language)
				{
					case Languages.En:
						typePluralName = "Landmarks";
						return string.Format("{0} {1}", cityPart, typePluralName);
					case Languages.Ru:
						typePluralName = "Достопримечательности";
						return string.Format("{0} {1}", typePluralName, cityPart);
				}
			}
			else
			{
				switch (language)
				{
					case Languages.En:
						typePluralName = sightType.PluralNameEn.ToUpperFirstLetter();
						break;
					case Languages.Ru:
						typePluralName = sightType.PluralNameRu.ToUpperFirstLetter();
						break;
				}
			}

			switch (language)
			{
				case Languages.En:
					return string.Format("{0} {1}", cityPart, typePluralName);
				case Languages.Ru:
					return string.Format("{0} {1}", typePluralName, cityPart);
			}

			throw new Exception("Unexpected");
		}
	}
}
