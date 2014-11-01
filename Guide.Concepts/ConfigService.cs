using System;
using System.Configuration;
using Guide.Config.Contracts;

namespace Guide.Config
{
	using System.Web;

	public class ConfigService : IConfigService
	{
		public string HttpImagesUrl
		{
			get { return ConfigurationManager.AppSettings["HttpImagesUrl"]; }
		}

		public string HttpQrImagesUrl
		{
			get { return ConfigurationManager.AppSettings["HttpQrImagesUrl"]; }
		}

		public string FullTextSearchIndexPath
		{
			get { return ConfigurationManager.AppSettings["FullTextSearchIndexPath"]; }
		}

		public string FullTextSearchIndexAbsolutePath
		{
			get
			{
				return string.Format("{0}{1}", HttpContext.Current.Server.MapPath("~/"), FullTextSearchIndexPath.Replace("/", "\\"));
			}
		}

		public Languages DefaultLanguage
		{
			get
			{
				Languages language;
				if (Enum.TryParse(DefaultLanguageCode, true, out language))
				{
					return language;
				}
				throw new Exception("DefaultSiteLanguage not specified in Web.Config");
			}
		}

		public int TeaserLetterCount
		{
			get
			{
				int result;
				var raw = ConfigurationManager.AppSettings["TeaserLetterCount"];
				if (int.TryParse(raw, out result))
				{
					return result;
				}
				throw new Exception("TeaserLetterCount is missing in Web.Config");
			}
		}
		public string DefaultLanguageCode
		{
			get { return ConfigurationManager.AppSettings["DefaultSiteLanguage"]; }
		}

		public int CountOfElementsOnPage
		{
			get
			{
				int result;
				var raw = ConfigurationManager.AppSettings["CountOfElementsOnPage"];
				if (int.TryParse(raw, out result))
				{
					return result;
				}
				throw new Exception("CountOfElementsOnPage is missing in Web.Config");
			}
		}

		public string GoogleApiKey
		{
			get
			{
				return ConfigurationManager.AppSettings["GoogleApiKey"]; 
			}
		}

		public string FoursquareOauthToken
		{
			get
			{
				return ConfigurationManager.AppSettings["FoursquareOauthToken"];
			}
		}

		public string LanguageCookieName {
			get { return "language"; }
		}

		public string LanguageSessionKey
		{
			get { return "Language"; }
		}

		public string LanguageRouteParam
		{
			get { return "language"; }
		}

		public string AppName
		{
			get { return "Spb Guide"; }
		}
	}
}
