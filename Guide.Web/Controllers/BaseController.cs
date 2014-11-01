using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Guide.Data.Contracts;

namespace Guide.Web.Controllers
{
	using System.Data.Entity;

	using Guide.Model.Contracts;
	using Guide.Model.Entities;
	using Guide.Model.Model;
	using Guide.Services.Contracts;

	public class BaseController : Controller
    {
        protected readonly IUnitOfWork Unit;
        protected readonly IModelFactory ModelFactory;
		protected readonly IUrlService urlService;
		protected readonly IArticleListTitleService articleListTitleService;

		protected readonly IPredefinedService predefinedService;

		public BaseController(IUnitOfWork unit, IModelFactory modelFactory, IUrlService urlService, IArticleListTitleService articleListTitleService, IPredefinedService predefinedService)
        {
            Unit = unit;
            ModelFactory = modelFactory;
	        this.urlService = urlService;
			this.articleListTitleService = articleListTitleService;
			this.predefinedService = predefinedService;
        }


		protected void PopulateSightTypes(City city, int activeSightTypeId)
		{
			List<SightTypeModel> sightTypes = this.Unit.SightTypes.All.Include(st => st.ArticleToSightTypes).Where(s => s.Published && s.ArticleToSightTypes.Any()).ToList().Select(st => this.ModelFactory.Create(city, st)).ToList();
			ViewBag.LandmarksListUrlParam = urlService.PrepareToUrl(articleListTitleService.Generate(city, null));
			ViewBag.MuseumsListUrlParam = urlService.PrepareToUrl(articleListTitleService.Generate(city, predefinedService.SightTypes.First(s => s.NameEn == "Museum")));
			ViewBag.BuildingsListUrlParam = urlService.PrepareToUrl(articleListTitleService.Generate(city, predefinedService.SightTypes.First(s => s.NameEn == "Building")));
			this.ViewBag.CityId = city.Id;
			if (activeSightTypeId != 0)
			{
				SightTypeModel activeType = sightTypes.FirstOrDefault(st => st.Id == activeSightTypeId);
				if (activeType != null)
				{
					activeType.Active = true;
				}
			}
			this.ViewBag.SightTypesNavList = sightTypes;
		}
    }
}