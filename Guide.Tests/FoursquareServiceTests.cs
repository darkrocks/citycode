using System;
using Guide.Model.Entities;
using Guide.Services;
using Guide.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Guide.Tests
{
	[TestClass]
	public class FoursquareServiceTests
	{
		[TestMethod]
		public void Can_Fetch_Isaakievskiy()
		{
			var foursquarerService = new FoursquareService(new ConfigServiceFakes(), new WebRequestService());
			var article = new Article
			{
				Lat = "59.933992",
				Lng = "30.306148",
				HeaderRu = "Исаакиевский собор"
			};

			var success = foursquarerService.FetchArticleData(article);

			Assert.AreEqual(true, success);
			Assert.AreEqual("4cd179cd7561236ab7eb31f3", article.FoursquareId);
			Assert.AreEqual("+7 812 314-21-68", article.Phone);
			//Assert.AreEqual("Saint Isaac's Cathedral", article.HeaderEn);
			Assert.AreEqual("www.cathedral.ru", article.Website);
		}

		public void Can_Fetch_Isaakievskiy2()
		{
			var foursquarerService = new FoursquareService(new ConfigServiceFakes(), new WebRequestService());
			var article = new Article
			{
				Lat = "59.933992",
				Lng = "30.306148",
				HeaderRu = "Saint Isaac's Cathedral"
			};

			var success = foursquarerService.FetchArticleData(article);

			Assert.AreEqual(true, success);
			Assert.AreEqual("4cd179cd7561236ab7eb31f3", article.FoursquareId);
			Assert.AreEqual("+7 812 314-21-68", article.Phone);
			//Assert.AreEqual("Исаакиевский собор", article.HeaderRu);
			Assert.AreEqual("http://www.cathedral.ru", article.Website);
		}

		public void Not_Fetch_If_Already()
		{
			var foursquarerService = new FoursquareService(new ConfigServiceFakes(), new WebRequestService());
			var article = new Article
			{
				FoursquareId = "sdfa",
				Lat = "59.933992",
				Lng = "30.306148",
				HeaderRu = "Исаакиевский собор"
			};

			var success = foursquarerService.FetchArticleData(article);

			Assert.AreEqual(false, success);
		}

		public void Not_Fetch_If_Already2()
		{
			var foursquarerService = new FoursquareService(new ConfigServiceFakes(), new WebRequestService());
			var article = new Article
			{
				Website = "sdfa",
				Phone = "sdfa",

				Lat = "59.933992",
				Lng = "30.306148",
				HeaderRu = "Исаакиевский собор"
			};

			var success = foursquarerService.FetchArticleData(article);

			Assert.AreEqual(false, success);
		}
	}
}
