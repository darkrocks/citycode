using System;
using Guide.Model.Entities;
using Guide.Services;
using Guide.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Guide.Tests
{
	[TestClass]
	public class GeolocationService_Tests
	{
		[TestMethod]
		public void Can_Get_Khudozhnikov_Location()
		{
			var geolocationService = new YandexMapsService(new WebRequestService());

			var article = new Article
			{
				StreetNameRu = "проспект художников",
				BuildingNumber = 23,
				Housing = 1
			};

			var success = geolocationService.FetchCoordinagesInSpb(article);

			Assert.AreEqual(true, success);
			Assert.AreEqual("60.041912", article.Lat);
			Assert.AreEqual("30.350076", article.Lng);
		}

				[TestMethod]
		public void Not_Fetch_If_Already()
		{
			var geolocationService = new YandexMapsService(new WebRequestService());

			var article = new Article
			{
				Lat = "asdf",
				Lng = "sadfa",
				StreetNameRu = "проспект художников",
				BuildingNumber = 23,
				Housing = 1
			};

			var success = geolocationService.FetchCoordinagesInSpb(article);
			Assert.AreEqual(false, success);
		}
	}
}
