using System;
using System.Globalization;
using System.Web;
using Guide.Config;
using Guide.Config.Contracts;
using Guide.Services.Contracts;

namespace Guide.Services
{
	public class LanguageService : ILanguageService
	{
		private readonly IConfigService _cnfg;

		public LanguageService(IConfigService cnfg)
		{
			_cnfg = cnfg;
		}

		public Languages CurrentLanguage
		{
			get
			{
				var languageFromSession = HttpContext.Current.Session[_cnfg.LanguageSessionKey];
				if (languageFromSession != null)
				{
					Languages language;
					if (Enum.TryParse(languageFromSession.ToString(), true, out language))
					{
						return language;
					}
				}
				return _cnfg.DefaultLanguage;
			}
			set
			{
				HttpContext.Current.Session[_cnfg.LanguageSessionKey] = value;
				var ci = new CultureInfo(value.ToString().ToLower());
				System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
				System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
			}
		}

		public string CurrentLanguageUrlVersion
		{
			get { return CurrentLanguage.ToString().ToLower(); }
		}

		public string CurrentLanguageFriendlyName
		{
			get { return GetLanguageFriendlyName(CurrentLanguage); }
		}

		public string GetLanguageFriendlyName(Languages language)
		{
			switch (language)
			{
				case Languages.En:
					return "English";
				case Languages.Ru:
					return "Русский";
			}
			throw new Exception("Unexpected");
		}
	}
}