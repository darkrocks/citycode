using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guide.Config;
using Guide.Model.Entities;
using Guide.Services.Contracts;

namespace Guide.Services
{
	public enum PossessiveCase
	{
		Genitive,
		Locativus
	}

	public class CityNameCaseService : ICityNameCaseService
	{
		private readonly ILanguageService _languageService;

		public CityNameCaseService(ILanguageService languageService)
		{
			_languageService = languageService;
		}

		public string GetName(City city, PossessiveCase possessiveCase)
		{
			return GetName(city, possessiveCase, _languageService.CurrentLanguage);
		}

		public string GetName(City city, PossessiveCase possessiveCase, Languages language)
		{
			switch (city.ThreeLetterCode)
			{
				case "spb":
					switch (language)
					{
						case Languages.Ru:
							switch (possessiveCase)
							{
								case PossessiveCase.Genitive:
									return "Санкт-Петербурга";
								case PossessiveCase.Locativus:
									return "Санкт-Петербурге";
							}
							return "";
						case Languages.En:
							return city.NameEn;
					}
					break;
			}
			throw new Exception("Unexpected");
		}
	}
}
