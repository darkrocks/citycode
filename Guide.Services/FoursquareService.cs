using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Guide.Config.Contracts;
using Guide.Model.Entities;
using Guide.Services.Contracts;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Guide.Services
{
	public class FoursquareService : IFoursquareService
	{
		private readonly IConfigService _config;
		private readonly IWebRequestService _webRequestService;

		public FoursquareService(IConfigService config, IWebRequestService webRequestService)
		{
			_config = config;
			_webRequestService = webRequestService;
		}

		public bool FetchArticleData(Article article)
		{
			if (!string.IsNullOrWhiteSpace(article.FoursquareId) ||
				(!string.IsNullOrWhiteSpace(article.Website) &&
				!string.IsNullOrWhiteSpace(article.Phone)))
			{
				return false;
			}

			if (string.IsNullOrWhiteSpace(article.Lat) || string.IsNullOrWhiteSpace(article.Lng))
			{
				return false;
			}

			string headingRu = string.Empty;
			if (!string.IsNullOrWhiteSpace(article.HeaderRu))
			{
				headingRu = article.HeaderRu.ToLower();	
			}
			string headingEn = string.Empty;
			if (!string.IsNullOrWhiteSpace(article.HeaderEn))
			{
				headingEn = article.HeaderEn.ToLower();
			}

			if (string.IsNullOrWhiteSpace(headingRu) && string.IsNullOrWhiteSpace(headingEn))
			{
				return false;
			}

			try
			{
				string url = string.Format("https://api.foursquare.com/v2/venues/search?ll={0},{1}&oauth_token={2}&v=20140406", article.Lat, article.Lng, _config.FoursquareOauthToken);
				dynamic responseJson = _webRequestService.GetJson(url);
				var venues = responseJson.response.venues;
				if (venues != null)
				{
					foreach (var venue in venues)
					{
						var venueName = ((string)venue.name);
						var venueNameLower = venueName.ToLower();
						if ((!string.IsNullOrWhiteSpace(headingEn) && venueNameLower.Contains(headingEn))
							|| (!string.IsNullOrWhiteSpace(headingRu) && venueNameLower.Contains(headingRu)))
						{
							if (string.IsNullOrWhiteSpace(article.FoursquareId) && venue.id != null)
							{
								article.FoursquareId = venue.id;
							}

							if (string.IsNullOrWhiteSpace(article.Phone) && venue.contact.formattedPhone != null)
							{
								article.Phone = venue.contact.formattedPhone;
							}

							if (string.IsNullOrWhiteSpace(article.Website) && venue.url != null)
							{
								article.Website = ((string)venue.url).Replace("http://", string.Empty);
							}

							if (venue.name != null)
							{
								article.FoursquareName = venue.name;
							}

							//if (string.IsNullOrWhiteSpace(article.HeaderEn))
							//{
							//	if (venueName.Contains("/"))
							//	{
							//		var names = venueName.Split('/');
							//		if (names.Length == 2)
							//		{
							//			var englName = names[1];
							//			if (!string.IsNullOrWhiteSpace(englName))
							//			{
							//				article.HeaderEn = englName.Trim();
							//			}
							//		}
							//	}
							//	article.Website = venue.url;
							//}

							//if (string.IsNullOrWhiteSpace(article.HeaderRu))
							//{
							//	if (venueName.Contains("/"))
							//	{
							//		var names = venueName.Split('/');
							//		if (names.Length == 2)
							//		{
							//			var ruName = names[0];
							//			if (!string.IsNullOrWhiteSpace(ruName))
							//			{
							//				article.HeaderRu = ruName;
							//			}
							//		}
							//	}
							//	article.Website = venue.url;
							//}
							return true;
						}
					}
				}
			}
			catch (Exception)
			{
				return false;
			}
			return false;
		}
	}
}
