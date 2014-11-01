using System.Data.Entity.Core.Objects;

using Guide.Data;

namespace Guide.Web.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Data.Entity;
	using System.Linq;
	using System.Web;
	using System.Web.Mvc;
	using System.Web.Routing;

	using Guide.Config;
	using Guide.Config.Contracts;
	using Guide.Data.Contracts;
	using Guide.Extensions;
	using Guide.Model.Contracts;
	using Guide.Model.Entities;
	using Guide.Model.Model;
	using Guide.Services;
	using Guide.Services.Contracts;
	using Guide.Web.Attributes;
	using Guide.Web.Infrastructure.Extensions;

	using PagedList;

	using Resx;

	using Telerik.Web.Mvc;

	public class ArticlesController : BaseController
	{
		#region Fields

		private readonly ICityNameCaseService _cityNameCaseService;

		private readonly ILanguageService _languageService;

		private readonly IConfigService config;

		private readonly ITransliterationService transliterationService;

		#endregion

		#region Constructors and Destructors

		public ArticlesController(
			IUnitOfWork unit,
			IModelFactory modelFactory,
			IPredefinedService predefinedService,
			ILanguageService languageService,
			ICityNameCaseService cityNameCaseService,
			IConfigService config,
			ITransliterationService transliterationService,
			IArticleListTitleService articleListTitleService,
			IUrlService urlService)
			: base(unit, modelFactory, urlService, articleListTitleService, predefinedService)
		{
			this._languageService = languageService;
			this._cityNameCaseService = cityNameCaseService;
			this.config = config;
			this.transliterationService = transliterationService;
		}

		#endregion

		#region Public Methods and Operators

		public ActionResult Details(int id, int? sightTypeId = null, bool onlyPublished = true)
		{
			Article article =
				this.Unit.Articles.All.Include(a => a.ArticleToImages.Select(ati => ati.Article))
					.Include(a => a.ArticleToImages.Select(ati => ati.Image))
					.Include(a => a.City)
					.Include(a => a.City.Country)
					.Include(a => a.ArticleToSightTypes.Select(atst => atst.Article))
					.Include(a => a.ArticleToSightTypes.Select(atst => atst.SightType))
					.FirstOrDefault(a => a.Id == id && (!onlyPublished || a.Published));

			if (article != null)
			{
				List<Article> threeArticles =
					this.Unit.GetArticlesLight(article.City, sightTypeId == 0 ? null : sightTypeId, true)
						.FindSandwichedItem(a => a.Id == id)
						.ToList();
				this.ViewBag.SightTypeId = sightTypeId;
				var routeValues = GetRouteValues();
				if (threeArticles.Count == 3)
				{
					this.ViewBag.PrevArticleLinkDisabled = threeArticles[0] == null;
					this.ViewBag.NextArticleLinkDisabled = threeArticles[2] == null;

					var httpContextWrapper = new HttpContextWrapper(System.Web.HttpContext.Current);
					if (RouteTable.Routes != null)
					{
						var urlHelper =
							new UrlHelper(new RequestContext(httpContextWrapper, RouteTable.Routes.GetRouteData(httpContextWrapper)));

						this.ViewBag.PrevArticleUrl = "";
						Article prevArticle = threeArticles[0];

						if (prevArticle != null)
						{
							routeValues["id"] = prevArticle.Id;
							var prevModel = ModelFactory.Create(prevArticle);
							routeValues["title"] = urlService.PrepareToUrl(prevModel.Header);
							routeValues["city"] = urlService.PrepareToUrl(prevModel.City.Name);
							this.ViewBag.PrevArticleUrl = urlHelper.Action("Details", "Articles", routeValues);
						}

						this.ViewBag.NextArticleUrl = "";
						Article nextArticle = threeArticles[2];
						if (nextArticle != null)
						{
							routeValues["id"] = nextArticle.Id;
							var nextModel = ModelFactory.Create(nextArticle);
							routeValues["title"] = urlService.PrepareToUrl(nextModel.Header);
							routeValues["city"] = urlService.PrepareToUrl(nextModel.City.Name);
							this.ViewBag.NextArticleUrl = urlHelper.Action("Details", "Articles", routeValues);
						}
					}

					var model = ModelFactory.Create(article);
					this.PopulateSightTypes(article.City, sightTypeId ?? 0);
					this.ViewBag.Title = model.Header;

					// switch language button
					routeValues = GetRouteValues();
					switch (_languageService.CurrentLanguage)
					{
						case Languages.En:
							routeValues["language"] = Languages.Ru.ToString().ToLower();
							routeValues["title"] = urlService.PrepareToUrl(article.HeaderRu);
							routeValues["city"] = urlService.PrepareToUrl(article.City.NameRu);
							ViewBag.SecondLanguageUrl = Url.Action("Details", "Articles", routeValues);
							break;
						case Languages.Ru:
							routeValues["language"] = Languages.En.ToString().ToLower();
							routeValues["title"] = urlService.PrepareToUrl(article.HeaderEn);
							routeValues["city"] = urlService.PrepareToUrl(article.City.NameEn);
							ViewBag.SecondLanguageUrl = Url.Action("Details", "Articles", routeValues);

							break;
					}

					// short url
					routeValues = GetRouteValues();
					ViewBag.ShortUrl = string.Format(
						"http://{0}/{1}/a/{2}",
						System.Web.HttpContext.Current.Request.Url.Authority,
						_languageService.CurrentLanguageUrlVersion,
						id);

					// canonical url
					routeValues = GetRouteValues();
					routeValues["title"] = urlService.PrepareToUrl(model.Header);
					routeValues["city"] = urlService.PrepareToUrl(model.City.Name);
					routeValues["sightTypeId"] = sightTypeId ?? 0;
					ViewBag.CanonicalUrl = string.Format(
						"http://{0}{1}",
						System.Web.HttpContext.Current.Request.Url.Authority,
						Url.Action("Details", "Articles", routeValues));

					ViewBag.PlacesNear =
					this.Unit.GetArticles(article.City, null, true).Take(3)
					.ToList()
					.Select(this.ModelFactory.Create)
					.ToList();

					return View(model);
				}
			}

			return this.View("Error");
		}

		public ActionResult List(int cityId, int? page, int sightTypeId)
		{
			SightType sightType = null;
			if (sightTypeId != 0)
			{
				sightType = Unit.SightTypes.GetById(sightTypeId);
			}

			this.ViewBag.SightTypeId = sightTypeId;

			City city = predefinedService.Cities.First(c => c.Id == cityId);
			CityModel cityModel = ModelFactory.Create(predefinedService.Cities.First(c => c.Id == cityId));

			this.ViewBag.Title = articleListTitleService.Generate(city, sightType);
			this.ViewBag.TitleForUrl = urlService.PrepareToUrl(ViewBag.Title);
			this.ViewBag.UrlCityParam = urlService.PrepareToUrl(cityModel.Name);

			this.PopulateSightTypes(city, sightTypeId);

			List<ArticleModel> articleModels =
				this.Unit.GetArticles(city, sightType != null ? sightType.Id : (int?)null, true)
					.ToList()
					.Select(this.ModelFactory.Create)
					.ToList();

			IPagedList<ArticleModel> model = articleModels.ToPagedList((page ?? 1), this.config.CountOfElementsOnPage);
			ViewBag.Description = String.Format(
				"{0}. {1}",
				ViewBag.Title,
				string.Join(", ", model.Take(5).Select(a => a.Header)));

			// switch language button
			var routeValues = GetRouteValues();
			switch (_languageService.CurrentLanguage)
			{
				case Languages.En:
					routeValues["language"] = Languages.Ru.ToString().ToLower();
					routeValues["title"] = urlService.PrepareToUrl(articleListTitleService.Generate(city, sightType, Languages.Ru));
					ViewBag.SecondLanguageUrl = Url.Action("List", "Articles", routeValues);
					break;
				case Languages.Ru:
					routeValues["language"] = Languages.En.ToString().ToLower();
					routeValues["title"] = urlService.PrepareToUrl(articleListTitleService.Generate(city, sightType, Languages.En));
					ViewBag.SecondLanguageUrl = Url.Action("List", "Articles", routeValues);
					break;
			}

			// short url
			ViewBag.ShortUrl = string.Format(
				"http://{0}/{1}/l/{2}/{3}/{4}",
				System.Web.HttpContext.Current.Request.Url.Authority,
				_languageService.CurrentLanguageUrlVersion,
				cityId,
				sightTypeId,
				page ?? 1);

			// canonical url
			routeValues = GetRouteValues();
			routeValues["title"] = ViewBag.TitleForUrl;
			routeValues["sightTypeId"] = sightTypeId;

			routeValues["page"] = page ?? 1;
			ViewBag.CanonicalUrl = string.Format(
				"http://{0}{1}",
				System.Web.HttpContext.Current.Request.Url.Authority,
				Url.Action("List", "Articles", routeValues));

			return this.View(model);
		}

		private RouteValueDictionary GetRouteValues()
		{
			var routeValues = new RouteValueDictionary();
			foreach (var value in Url.RequestContext.RouteData.Values)
			{
				routeValues.Add(value.Key, value.Value);
			}
			return routeValues;
		}

		[AllowAnonymous]
		public ActionResult Search(int cityId, string query, int? page)
		{
			this.ViewBag.Query = query;

			this.PopulateSightTypes(predefinedService.Cities.First(c => c.Id == cityId), 0);
			return
				this.View(
					this.Unit.SearchArticles(query)
						.Select(this.ModelFactory.Create)
						.ToPagedList((page ?? 1), this.config.CountOfElementsOnPage));
		}

		#endregion
	}
}