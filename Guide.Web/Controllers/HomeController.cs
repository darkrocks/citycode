using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Guide.Web.Controllers
{
	using Guide.Data.Contracts;
	using Guide.Model.Contracts;
	using Guide.Model.Entities;
	using Guide.Services.Contracts;

	public class HomeController : BaseController
	{
		public HomeController(	
			IUnitOfWork unit,
			IModelFactory modelFactory, 
			IArticleListTitleService articleListTitleService,
			IUrlService urlService,
			IPredefinedService predefinedService)
			: base(unit, modelFactory, urlService, articleListTitleService, predefinedService)
		{
		}

		public ActionResult Index()
		{
			this.PopulateSightTypes(predefinedService.Cities.First(c => c.Id == 1), 0);
			ViewBag.ExampleArticles =
					this.Unit.GetArticles(new City(){Id = 1}, null, true).Take(6)
					.ToList()
					.Select(this.ModelFactory.Create)
					.ToList();
			return View();
		  //  return RedirectToAction("List", "Articles", new { id = "Spb" });
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}