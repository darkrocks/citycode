using Guide.Data.Services;

namespace Guide.Data.UnitOfWork
{
	using System;
	using System.Data.Entity;
	using System.Linq;

	using Guide.Config;
	using Guide.Config.Contracts;
	using Guide.Data.Contracts;
	using Guide.Model.Entities;
	using Guide.Services.Contracts;

	public partial class UnitOfWork : IUnitOfWork, IDisposable
	{
		#region Fields

		private readonly ILanguageService _languageService;

		private readonly IConfigService config;

		private readonly IFullTextSearchService fullTextSearch;

		#endregion

		#region Constructors and Destructors

		public UnitOfWork(
			IConfigService config,
			IFullTextSearchService fullTextSearch,
			GuideDbContext dbContext,
			ILanguageService languageService,
			IRepository<User> users,
			IRepository<Role> roles,
			IRepository<UserInRole> userInRoles,
			IRepository<Article> articles,
			IRepository<Country> countries,
			IRepository<City> cities,
			IRepository<Image> images,
			IRepository<ArticleToImage> articleToImages,
			IRepository<SightType> sightTypes,
			IRepository<ArticleToSightType> articleToSightTypes)
		{
			this.config = config;
			this.fullTextSearch = fullTextSearch;
			this._languageService = languageService;
			this.Cities = cities;
			this.Images = images;
			this.DbContext = dbContext;
			this.Users = users;
			this.Articles = articles;
			this.Countries = countries;
			this.ArticleToImages = articleToImages;
			this.SightTypes = sightTypes;
			this.ArticleToSightTypes = articleToSightTypes;
			this.Roles = roles;
			this.UserInRoles = userInRoles;
		}

		#endregion

		#region Public Properties

		public IRepository<ArticleToImage> ArticleToImages { get; private set; }

		public IRepository<ArticleToSightType> ArticleToSightTypes { get; private set; }
		public IRepository<Article> Articles { get; private set; }
		public IRepository<City> Cities { get; private set; }
		public IRepository<Country> Countries { get; private set; }
		public GuideDbContext DbContext { get; private set; }
		public IRepository<Image> Images { get; private set; }
		public IRepository<Role> Roles { get; private set; }
		public IRepository<SightType> SightTypes { get; private set; }
		public IRepository<UserInRole> UserInRoles { get; private set; }
		public IRepository<User> Users { get; private set; }

		#endregion

		#region Public Methods and Operators

		public void Dispose()
		{
			if (this.DbContext == null)
			{
				return;
			}
			this.DbContext.Dispose();
			this.DbContext = null;
		}

		public IQueryable<Article> GetArticles(
			City city,
			int? sightTypeId = null,
			bool onlyPublished = false,
			string letter = null)
		{
			switch (this._languageService.CurrentLanguage)
			{
				case Languages.En:
					return
						this.Articles.All.Include(a => a.ArticleToImages)
							.Include(a => a.ArticleToImages.Select(ati => ati.Image))
							.Include(a => a.Thumbnail)
							.Include(a => a.City)
							.Include(a => a.ArticleToSightTypes.Select(atst => atst.SightType))
							.Where(
								a =>
								a.City.Id == city.Id && (letter == null || a.HeaderEn.StartsWith(letter))
								&& (!onlyPublished || a.Published)
								&& (sightTypeId == null || a.ArticleToSightTypes.Any(atst => atst.SightType.Id == sightTypeId)))
							.OrderBy(a => a.HeaderEn);
				case Languages.Ru:
					return
						this.Articles.All.Include(a => a.ArticleToImages)
							.Include(a => a.ArticleToImages.Select(ati => ati.Image))
							.Include(a => a.Thumbnail)
							.Include(a => a.City)
							.Include(a => a.ArticleToSightTypes.Select(atst => atst.SightType))
							.Where(
								a =>
								a.City.Id == city.Id && (letter == null || a.HeaderRu.StartsWith(letter))
								&& (!onlyPublished || a.Published)
								&& (sightTypeId == null || a.ArticleToSightTypes.Any(atst => atst.SightType.Id == sightTypeId)))
							.OrderBy(a => a.HeaderRu);
			}
			return null;
		}

		public IQueryable<Article> GetArticlesLight(
			City city,
			int? sightTypeId = null,
			bool onlyPublished = false,
			string letter = null)
		{
			switch (this._languageService.CurrentLanguage)
			{
				case Languages.En:
					return
						this.Articles.All.Include(a => a.ArticleToImages.Select(ati => ati.Image))
							.Include(a => a.City)
							.Include(a => a.Thumbnail)
							.Include(a => a.ArticleToSightTypes.Select(atst => atst.SightType))
							.Where(
								a =>
								a.City.Id == city.Id && (letter == null || a.HeaderEn.StartsWith(letter))
								&& (!onlyPublished || a.Published)
								&& (sightTypeId == null || a.ArticleToSightTypes.Any(atst => atst.SightType.Id == sightTypeId)))
							.OrderBy(a => a.HeaderEn);
				case Languages.Ru:
					return
						this.Articles.All.Include(a => a.ArticleToImages)
							.Include(a => a.ArticleToImages.Select(ati => ati.Image))
							.Include(a => a.Thumbnail)
							.Include(a => a.City)
							.Include(a => a.ArticleToSightTypes.Select(atst => atst.SightType))
							.Where(
								a =>
								a.City.Id == city.Id && (letter == null || a.HeaderRu.StartsWith(letter))
								&& (!onlyPublished || a.Published)
								&& (sightTypeId == null || a.ArticleToSightTypes.Any(atst => atst.SightType.Id == sightTypeId)))
							.OrderBy(a => a.HeaderRu);
			}
			return null;
		}

		public SightType GetSightType(string urlParam)
		{
			return
				this.SightTypes.All.ToList()
					.FirstOrDefault(st => st.PluralNameEn.Replace(" ", "-").ToLowerInvariant() == urlParam);
		}

		public void Index(Article article)
		{
				this.PrepareArticleToIndexing(article);
				this.fullTextSearch.AddUpdate(article, SearchType.Articles);
		}

		public void Index(Image image)
		{
			this.fullTextSearch.AddUpdate(image, SearchType.Images);
		}

		public bool Save()
		{
			return this.DbContext.SaveChanges() > 0;
		}


		public bool Delete(Article article)
		{
			if (this.Articles.Delete(article))
			{
				this.fullTextSearch.Delete(article.Id, SearchType.Articles);
			}
			return false;
		}

		public bool Delete(Image image)
		{
			if (this.Images.Delete(image))
			{
				this.fullTextSearch.Delete(image.Id, SearchType.Images);
			}
			return false;
		}

		#endregion

		#region Methods

		private void PrepareArticleToIndexing(Article article)
		{
				article.ArticleToSightTypes = this.ArticleToSightTypes.All.
					Include(atst => atst.SightType).
					Where(atst => atst.ArticleId == article.Id).ToList();
		}

		#endregion
	}
}