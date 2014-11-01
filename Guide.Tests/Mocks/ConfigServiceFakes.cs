using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guide.Config;
using Guide.Config.Contracts;

namespace Guide.Tests.Mocks
{
	public class ConfigServiceFakes: IConfigService
	{
		public string HttpImagesUrl
		{
			get { return "content/Img/QR"; }
		}
		public string HttpQrImagesUrl { get; private set; }

		public string GoogleApiKey {
			get { return "AIzaSyAiWuXC3fT5okpd8XZ-8yDMUml4Hr8OLqo"; }
		}

		public string FoursquareOauthToken {
			get { return "PQUDFBUBOTDWVIX25PHYNY1KLFIRZB5AXJCOS55OQTSHTMQB"; }
		}
		public string FullTextSearchIndexPath { get; private set; }
		public string FullTextSearchIndexAbsolutePath { get; private set; }
		public Languages DefaultLanguage { get; private set; }
		public int TeaserLetterCount { get; private set; }
		public string DefaultLanguageCode { get; private set; }
		public string LanguageCookieName { get; private set; }
		public string LanguageSessionKey { get; private set; }
		public string LanguageRouteParam { get; private set; }
		public int CountOfElementsOnPage { get; private set; }
		public string AppName { get; private set; }
	}
}
