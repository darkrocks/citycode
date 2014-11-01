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

	[Authorize]
	public class ArticlesAdminController : BaseController
	{
		#region Fields

		private readonly IConfigService config;
		private readonly IUrlService _urlService;
		private readonly IGeolocationService _geolocationService;
		private readonly IFoursquareService _foursquareService;
		private readonly IQrService _qrService;
		private readonly ITransliterationService _transliterationService;
		private readonly IImageLoadService imageLoadService;
		private readonly IModelStateService modelStateService;

		#endregion

		#region Constructors and Destructors

		public ArticlesAdminController(
			IUnitOfWork unit,
			IModelFactory modelFactory,
			IAlphabeticPaginationService alphabeticPaginationService,
			IImageLoadService imageLoadService,
			IPredefinedService predefinedService,
			ILanguageService languageService,
			ICityNameCaseService cityNameCaseService,
			IModelStateService modelStateService,
			IConfigService config,
			IUrlService urlService,
			IGeolocationService geolocationService,
			IFoursquareService foursquareService,
			IQrService qrService,
			ITransliterationService transliterationService,
			IArticleListTitleService articleListTitleService)
			: base(unit, modelFactory, urlService, articleListTitleService, predefinedService)
		{
			this.imageLoadService = imageLoadService;
			this.modelStateService = modelStateService;
			this.config = config;
			_urlService = urlService;
			_geolocationService = geolocationService;
			_foursquareService = foursquareService;
			_qrService = qrService;
			_transliterationService = transliterationService;
		}

		#endregion

		#region Public Methods and Operators

		[AllowAnonymous]
		public ActionResult QrCode(int id)
		{
			return this.View(this.ModelFactory.CreateArticleEditModel(
				this.Unit.Articles.All.Include(a => a.City)
					.Include(a => a.Thumbnail)
					.Include(a => a.ArticleToImages.Select(ati => ati.Article))
					.Include(a => a.ArticleToImages.Select(ati => ati.Image))
					.Include(a => a.ArticleToSightTypes.Select(atst => atst.Article))
					.Include(a => a.ArticleToSightTypes.Select(atst => atst.SightType))
					.FirstOrDefault(a => a.Id == id)));
		}

		public ActionResult AdminList(string cityCode)
		{
			ViewBag.Grid = "Grid";
			ViewBag.SearchController = "ArticlesAdmin";
			return this.View();
		}

		public ActionResult AdminSearch(string query)
		{
			this.ViewBag.Query = query;
			ViewBag.SearchController = "ArticlesAdmin";
			ViewBag.Grid = "_AdminSearch";
			return this.View();
		}

		[GridAction]
		public ActionResult _AdminSearch(string query)
		{
			var model = Unit.SearchArticles(query).Select(ModelFactory.Gridify);
			return View(new GridModel<Article>(model));
		}

		public ActionResult Create()
		{
			this.ViewBag.Method = "Create";
			ViewBag.SearchController = "ArticlesAdmin";
			var article = new ArticleEditModel {City = this.predefinedService.Cities[0]};
			this.FillViewbagForEditing(article);
			return View("Edit", article);
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Create(ArticleEditModel model)
		{
			
			if (model.FetchFromFoursquare)
			{
				this.ViewBag.Method = "Edit";

			}
			else
			{
				this.ViewBag.Method = "Create";
			}

			ViewBag.SearchController = "ArticlesAdmin";
			var entity = SaveArticle(model, true);
			if (entity != null)
			{
				if (model.FetchFromFoursquare)
				{
					entity.City = Unit.Cities.All.First(c => c.Id == entity.CityId);
					model = ModelFactory.CreateArticleEditModel(entity);
					this.FillViewbagForEditing(model);
					ModelState.Clear();
					return this.View("Edit", model);
				}
				return this.RedirectToAction("AdminList", new {cityCode = this.Unit.Cities.GetById(model.CityId).ThreeLetterCode});
			}
			return this.View("Edit", model);
		}

		public ActionResult Edit(int id)
		{
			this.ViewBag.Method = "Edit";
			ArticleEditModel article =
				this.ModelFactory.CreateArticleEditModel(
					this.Unit.Articles.All.Include(a => a.City)
						.Include(a => a.Thumbnail)
						.Include(a => a.ArticleToImages.Select(ati => ati.Article))
						.Include(a => a.ArticleToImages.Select(ati => ati.Image))
						.Include(a => a.ArticleToSightTypes.Select(atst => atst.Article))
						.Include(a => a.ArticleToSightTypes.Select(atst => atst.SightType))
						.FirstOrDefault(a => a.Id == id));
			if (article != null)
			{
				this.FillViewbagForEditing(article);
				return View(article);
			}
			return this.View("Error");
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Edit(ArticleEditModel model)
		{
			this.ViewBag.Method = "Edit";
			ViewBag.SearchController = "ArticlesAdmin";
			var entity = SaveArticle(model, false);
			if (entity != null)
			{
				if (model.FetchFromFoursquare)
				{
					entity.City = Unit.Cities.All.First(c => c.Id == entity.CityId);
					model = ModelFactory.CreateArticleEditModel(entity);
					this.FillViewbagForEditing(model);
					ModelState.Clear();
					return this.View("Edit", model);
				}
				return this.RedirectToAction("AdminList", new {cityCode = this.Unit.Cities.GetById(model.CityId).ThreeLetterCode});
			}

			return this.View(model);
		}

		private Article SaveArticle(ArticleEditModel model, bool isCreation)
		{
			var errors = new List<string>();
			var thumbnailFile = new Image();
			var mainImageFile = new Image();

			try
			{
				if (this.ModelState.IsValid &&
				    (!imageLoadService.FileExist(model.File) ||
				     (this.imageLoadService.LoadImage(model.File, true, out thumbnailFile, out errors) &&
				      this.imageLoadService.LoadImage(model.File, false, out mainImageFile, out errors))))
				{
					Article article = this.ModelFactory.Parse(model);
					PopulateUrlParam(article);
					_geolocationService.FetchCoordinagesInSpb(article);
					if (model.FetchFromFoursquare)
					{
						_foursquareService.FetchArticleData(article);
					}

					if (isCreation)
					{
						this.Unit.Articles.Insert(article);
					}
					else
					{
						this.Unit.Articles.Update(article);
					}
					Unit.Save();


					if (isCreation)
					{
						CreateQrCode(article);
					}
					else
					{
						if (!Unit.ArticleToImages.All.
							Include(ati => ati.Image).
							Any(ati => ati.ArticleId == article.Id && ati.Image.ImageType == (short) ImageTypes.Qr740x740))
						{
							CreateQrCode(article);
						}
					}

					List<ArticleToSightType> sightTypesDb = Unit.ArticleToSightTypes.All.Where(i => i.ArticleId == article.Id).ToList();

					if (model.SightTypeIds != null)
					{
						sightTypesDb.Where(i => !model.SightTypeIds.Contains(i.SightTypeId)).ToList()
							.ForEach(a => this.Unit.ArticleToSightTypes.Delete(a));

						model.SightTypeIds.Where(i => !sightTypesDb.Select(s => s.SightTypeId).Contains(i))
							.Select(i => new ArticleToSightType {ArticleId = article.Id, SightTypeId = i})
							.ToList().ForEach(s => this.Unit.ArticleToSightTypes.Insert(s));
					}

					this.Unit.Save();
					this.Unit.Index(article);
					SaveImage(article, thumbnailFile, mainImageFile);

					return article;
				}
			}
			catch (DataException)
			{
				this.ModelState.AddModelError("", "Something wrong");
			}

			foreach (string error in errors)
			{
				this.ModelState.AddModelError("File", error);
			}
			this.FillViewbagForEditing(model);
			model.File = null;
			return null;
		}

		private void PopulateUrlParam(Article article)
		{
			article.UrlParam = string.Format("{0}-{1}", _transliterationService.Transliterate(article.HeaderRu.ToLower()),
				article.HeaderEn.ToLower().Trim().Replace(" ", "-"));
		}

		private void CreateQrCode(Article article)
		{
			var url = string.Format("http://{0}/a/{1}", System.Web.HttpContext.Current.Request.Url.Authority,article.Id);
			var qrImageName = _qrService.GeneratePng(url, ImageTypes.Qr740x740);
			Image qrImage = new Guide.Model.Entities.Image {FileName = qrImageName, ImageType = (short) ImageTypes.Qr740x740};
			Unit.Images.Insert(qrImage);
			Unit.Save();
			Unit.ArticleToImages.Insert(new ArticleToImage {ArticleId = article.Id, ImageId = qrImage.Id});
			Unit.Save();
		}

		private void SaveImage(Article article, Image thumbnailFile, Image mainImageFile)
		{
			if (!string.IsNullOrEmpty(thumbnailFile.FileName) &&
			    !string.IsNullOrEmpty(mainImageFile.FileName))
			{
				var oldThumb = Unit.Images.All.FirstOrDefault(i => i.Id == article.ThumbnailId);
				if (oldThumb != null)
				{
					this.Unit.Images.Delete(oldThumb);
				}

				// thumb
				thumbnailFile.Name = article.HeaderRu;
				this.Unit.Images.Insert(thumbnailFile);
				this.Unit.Save();

				article.ThumbnailId = thumbnailFile.Id;
				this.Unit.Articles.Update(article);
				this.Unit.Save();

				// main image 
				foreach (ArticleToImage articleToImage in this.Unit.ArticleToImages.All.Where(a => a.ArticleId == article.Id))
				{
					this.Unit.ArticleToImages.Delete(articleToImage);
				}
				this.Unit.Save();

				mainImageFile.Name = article.HeaderRu;
				this.Unit.Images.Insert(mainImageFile);
				this.Unit.Save();

				this.Unit.ArticleToImages.Insert(
					new ArticleToImage
					{
						ImageId = mainImageFile.Id,
						ArticleId = article.Id,
						CaptionRu = article.HeaderRu,
						CaptionEn = article.HeaderEn,
						Rank = 1
					});
				this.Unit.Save();
			}
		}


		[AllowAnonymous]
		[GridAction]
		public ViewResult Grid()
		{
			var model =
				new GridModel<Article>(
					Unit.Articles.All.Include(a => a.Thumbnail)
						.Include(a => a.ArticleToSightTypes.Select(atst => atst.SightType))
						.Select(ModelFactory.Gridify));
			return View(model);
		}

		[AllowAnonymous]
		[AcceptVerbs(HttpVerbs.Post)]
		[GridAction]
		public ActionResult GridSave(int id)
		{
			var article = Unit.Articles.GetById(id);

			TryUpdateModel(article);
			Unit.Articles.Update(article);
			Unit.Save();
			Unit.Index(article);
			article.ArticleToSightTypes = null;
			return
				View(
					new GridModel<Article>(
						Unit.Articles.All.Include(a => a.Thumbnail)
							.Include(a => a.ArticleToSightTypes.Select(atst => atst.SightType))
							.Select(ModelFactory.Gridify)));
		}

		[AllowAnonymous]
		[AcceptVerbs(HttpVerbs.Post)]
		[GridAction]
		public ActionResult GridInsert()
		{
			var article = new Article();
			if (TryUpdateModel(article))
			{
				Unit.Articles.Insert(article);
				Unit.Save();
				Unit.Index(article);
			}
			article.ArticleToSightTypes = null;
			return
				View(
					new GridModel<Article>(
						Unit.Articles.All.Include(a => a.Thumbnail)
							.Include(a => a.ArticleToSightTypes.Select(atst => atst.SightType))
							.Select(ModelFactory.Gridify)));
		}

		[AllowAnonymous]
		[AcceptVerbs(HttpVerbs.Post)]
		[GridAction]
		public ActionResult GridDelete(int id)
		{
			var article = Unit.Articles.All.FirstOrDefault(p => p.Id == id);
			if (article != null)
			{
				Unit.Articles.Delete(id);
				Unit.Save();
			}
			return
				View(
					new GridModel<Article>(
						Unit.Articles.All.Include(a => a.Thumbnail)
							.Include(a => a.ArticleToSightTypes.Select(atst => atst.SightType))
							.Select(ModelFactory.Gridify)));
		}

		#endregion

		#region Methods

		private void FillViewbagForEditing(ArticleEditModel article)
		{
			var cities = this.Unit.Cities.All.Where(c => c.CountryId == article.City.CountryId).ToList();
			this.ViewBag.Cities = new SelectList(
				cities,
				"Id",
				"NameEn",
				article.Id);
			this.ViewBag.SightTypes = new SelectList(this.Unit.SightTypes.All.ToList(), "Id", "NameEn");
			this.ViewBag.Importances = new SelectList(this.predefinedService.ImportanceModels, "Value", "NameEn");
		}

		#endregion
	}
}