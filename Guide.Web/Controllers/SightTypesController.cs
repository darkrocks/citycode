using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Guide.Data.Contracts;
using Guide.Model.Entities;
using Guide.Model.Model;
using Telerik.Web.Mvc;

namespace Guide.Web.Controllers
{
	using Guide.Model.Contracts;
	using Guide.Services.Contracts;

	public class SightTypesController : BaseController
	{
		public SightTypesController(
			IUnitOfWork unit,
			IModelFactory modelFactory,
			IArticleListTitleService articleListTitleService,
			IUrlService urlService,
			IPredefinedService predefinedService)
			: base(unit, modelFactory, urlService, articleListTitleService, predefinedService)
		{
		}

		public ActionResult List()
		{
			return this.View();
		}

		[AllowAnonymous]
		[GridAction]
		public ViewResult Grid()
		{
			var model = new GridModel<SightType>(Unit.SightTypes.All);
			return View(model);
		}

		[AllowAnonymous]
		[AcceptVerbs(HttpVerbs.Post)]
		[GridAction]
		public ActionResult GridSave(int id)
		{
			var sightType = Unit.SightTypes.GetById(id);
			TryUpdateModel(sightType);
			Unit.SightTypes.Update(sightType);
			Unit.Save();
			return View(new GridModel<SightType>(Unit.SightTypes.All));
		}

		[AllowAnonymous]
		[AcceptVerbs(HttpVerbs.Post)]
		[GridAction]
		public ActionResult GridInsert()
		{
			var sightType = new SightType();
			if (TryUpdateModel(sightType))
			{
				Unit.SightTypes.Insert(sightType);
				Unit.Save();
			}
			return View(new GridModel<SightType>(Unit.SightTypes.All));
		}

		[AllowAnonymous]
		[AcceptVerbs(HttpVerbs.Post)]
		[GridAction]
		public ActionResult GridDelete(int id)
		{
			var sightType = Unit.SightTypes.All.FirstOrDefault(p => p.Id == id);
			if (sightType != null)
			{
				Unit.SightTypes.Delete(sightType);
				Unit.Save();
			}
			return View(new GridModel<SightType>(Unit.SightTypes.All));
		}
	}
}