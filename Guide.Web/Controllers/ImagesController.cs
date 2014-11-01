using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace Guide.Web.Controllers
{
	using System.Data;

	using Guide.Config.Contracts;
	using Guide.Data.Contracts;
	using Guide.Model.Contracts;
	using Guide.Model.Entities;
	using Guide.Model.Model;
	using Guide.Services.Contracts;
	using Guide.Web.Attributes;

	using PagedList;

	public class ImagesController : BaseController
	{
		private readonly IImageLoadService imageLoadService;

		private readonly IConfigService config;

		public ImagesController(
			IUnitOfWork unit,
			IModelFactory modelFactory,
			IImageLoadService imageLoadService,
			IConfigService config,
			IArticleListTitleService articleListTitleService,
			IUrlService urlService,
			IPredefinedService predefinedService)
			: base(unit, modelFactory, urlService, articleListTitleService, predefinedService)
		{
			this.imageLoadService = imageLoadService;
			this.config = config;
		}

		public ActionResult List()
		{
			ViewBag.Grid = "Grid";
			ViewBag.SearchController = "Images";
			return this.View();
		}


		public ActionResult AdminSearch(string query)
		{
			this.ViewBag.Query = query;
			ViewBag.Grid = "_AdminSearch";
			ViewBag.SearchController = "Images";
			return this.View();
		}

		[GridAction]
		public ActionResult _AdminSearch(string query)
		{
			var model = Unit.SearchImages(query).Select(ModelFactory.Gridify);
			return View(new GridModel<Image>(model));
		}

		public ActionResult Create()
		{
			ViewBag.Grid = "Grid";
			ViewBag.SearchController = "Images";
			var model = new Image();
			return View(model);
		}

		[HttpPost]
		public ActionResult Create(Image model)
		{
			ViewBag.Grid = "Grid";
			ViewBag.SearchController = "Images";
			try
			{
				if (this.ModelState.IsValid)
				{
					List<string> errors;
					Image imageEntity;
					if (this.imageLoadService.LoadImage(model.File, false, out imageEntity, out errors))
					{
						imageEntity.Name = model.Name;
						Unit.Images.Insert(imageEntity);
						Unit.Save();
						Unit.Index(imageEntity);
						return this.RedirectToAction("AdminSearch", new { query = imageEntity.Name });
					}
					foreach (var error in errors)
					{
						ModelState.AddModelError("File", error);
					}
					return View(model);
				}
			}
			catch (DataException)
			{
				this.ModelState.AddModelError("", "Something wrong");
			}
			return this.View("Error");
		}

		[AllowAnonymous]
		[GridAction]
		public ViewResult Grid()
		{
			var model = new GridModel<Image>(Unit.Images.All.Where(i => i.ImageType == (short)ImageTypes.Article).Select(ModelFactory.Gridify));
			return View(model);
		}

		[AllowAnonymous]
		[AcceptVerbs(HttpVerbs.Post)]
		[GridAction]
		public ActionResult GridSave(int id)
		{
			var image = Unit.Images.GetById(id);
			TryUpdateModel(image);
			Unit.Images.Update(image);
			Unit.Save();
			Unit.Index(image);
			return View(new GridModel<Image>(Unit.Images.All.Select(ModelFactory.Gridify)));
		}

		[AllowAnonymous]
		[AcceptVerbs(HttpVerbs.Post)]
		[GridAction]
		public ActionResult GridInsert()
		{
			var image = new Image();
			if (TryUpdateModel(image))
			{
				Unit.Images.Insert(image);
				Unit.Save();
				Unit.Index(image);
			}
			return View(new GridModel<Image>(Unit.Images.All.Select(ModelFactory.Gridify)));
		}

		[AllowAnonymous]
		[AcceptVerbs(HttpVerbs.Post)]
		[GridAction]
		public ActionResult GridDelete(int id)
		{
			var image = Unit.Images.All.FirstOrDefault(p => p.Id == id);
			if (image != null)
			{
				var articlesToImages = Unit.ArticleToImages.All.Include(i => i.Image).Where(i => i.ImageId == id);
				foreach (var articlesToImage in articlesToImages)
				{
					Unit.ArticleToImages.Delete(articlesToImage);
				}

				var articles= Unit.Articles.All.Where(i => i.ThumbnailId == id);
				foreach (var article in articles)
				{
					article.ThumbnailId = null;
					Unit.Articles.Update(article);
				}
				Unit.Save();

				Unit.Delete(image);
				Unit.Save();
			}
			return View(new GridModel<Image>(Unit.Images.All.Select(ModelFactory.Gridify)));
		}

	}
}