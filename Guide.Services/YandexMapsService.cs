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
	public class YandexMapsService : IGeolocationService
	{
		private readonly IWebRequestService _webRequestService;
		private const string RussiaName = "Россия";
		private const string SaintPetersburgName = "Санкт-Петербург";
		private const string Zoom = "7";

		public YandexMapsService(IWebRequestService	webRequestService)
		{
			_webRequestService = webRequestService;
		}

		//	http://geocode-maps.yandex.ru/1.x/?geocode=Россия,+Санкт-Петербург,+проспект художников,+23к1&format=json
	
		// maps.yandex.ru/?ll=30.350076,60.041912&z=17&spn=30.350076,60.041912

		public bool FetchCoordinagesInSpb(Article article)
		{
			if (!string.IsNullOrWhiteSpace(article.Lat) && !string.IsNullOrWhiteSpace(article.Lng))
			{
				return false;
			}

			if (string.IsNullOrWhiteSpace(article.StreetNameRu))
			{
				return false;
			}

			try
			{

				var buildingN = article.BuildingNumber.ToString();
				if (article.Housing != null)
				{
					buildingN += ",+корпус " + article.Housing.ToString();
				}
				if (!string.IsNullOrWhiteSpace(article.Litera))
				{
					buildingN += ",+литера " + article.Housing.ToString();
				}


				var address = "";
				address = article.BuildingNumber != null ? 
					string.Format("{0},+{1},+{2},+{3}", RussiaName, SaintPetersburgName, article.StreetNameRu, buildingN) : 
					string.Format("{0},+{1},+{2}", RussiaName, SaintPetersburgName, article.StreetNameRu);

				string url = string.Format("http://geocode-maps.yandex.ru/1.x/?geocode={0}&format=json", address);
				dynamic responseJson = _webRequestService.GetJson(url);

				if (responseJson != null)
				{
					var coordinatesRaw = responseJson.response.GeoObjectCollection.featureMember[0].GeoObject.Point.pos.ToString();

					if (coordinatesRaw != null)
					{
						string[] coordParams = coordinatesRaw.Split(new[] { ' ' });

						var coord1 = coordParams[1];
						var coord2 = coordParams[0];

						if (!string.IsNullOrWhiteSpace(coord1) &&
							!string.IsNullOrWhiteSpace(coord2))
						{
							article.Lat = coord1;
							article.Lng = coord2;
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
