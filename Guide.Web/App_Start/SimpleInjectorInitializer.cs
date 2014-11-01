using System.Reflection;
using System.Web.Mvc;
using Guide.Config;
using Guide.Config.Contracts;
using Guide.Data;
using Guide.Data.Contracts;
using Guide.Data.ModelFactory;
using Guide.Data.UnitOfWork;
using Guide.Model.Contracts;
using Guide.Model.Entities;
using Guide.Model.Services;
using Guide.Services.Contracts;

using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;

namespace Guide.Web
{
	using Guide.Data.FullTextSearchInfrastructure;
	using Guide.Data.Services;
	using Guide.Services;

	public static class SimpleInjectorInitializer
	{
		public static void Initialize()
		{
			var container = new Container();
			
			InitializeContainer(container);

			container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
			
			container.RegisterMvcAttributeFilterProvider();
	   
			container.Verify();
			
			DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
		}
	 
		private static void InitializeContainer(Container container)
		{
			// single
			container.RegisterSingle<IImageLoadService, ImageLoadService>();
			container.RegisterSingle<ITextGenerator, TextGenerator>();
			container.RegisterSingle<IConfigService, ConfigService>();
			container.RegisterSingle<IUrlService, UrlService>();
			container.RegisterSingle<IArticleTeaserService, ArticleTeaserService>();
			container.RegisterSingle<IAlphabeticPaginationService, AlphabeticPaginationService>();
			container.RegisterSingle<IPredefinedService, PredefinedService>();
			container.RegisterSingle<ICityNameCaseService, CityNameCaseService>();
			container.RegisterSingle<IModelStateService, ModelStateService>();
			container.RegisterSingle<IGeolocationService, YandexMapsService>();
			container.RegisterSingle<IWebRequestService, WebRequestService>();
			container.RegisterSingle<IFoursquareService, FoursquareService>();
			container.RegisterSingle<IQrService, QrService>();
			container.RegisterSingle<ITransliterationService, TransliterationService>();

			// web request 
			container.RegisterPerWebRequest<IArticleListTitleService, ArticleListTitleService>();
			container.RegisterPerWebRequest<IArticleIndexDefinition, ArticleIndexDefinition>();
			container.RegisterPerWebRequest<IImageIndexDefinition, ImageIndexDefinition>();
			container.RegisterPerWebRequest<IFullTextSearchService, FullTextSearchService>();
			container.RegisterPerWebRequest<ILanguageService, LanguageService>();
			container.RegisterPerWebRequest<GuideDatabaseInitializer>();
			container.RegisterPerWebRequest<GuideDbContext>();
			container.RegisterPerWebRequest<IUnitOfWork, UnitOfWork>();
			container.RegisterPerWebRequest<IModelFactory, ModelFactory>();
			container.RegisterPerWebRequest<IRepository<Image>, Repository<Image>>();
			container.RegisterPerWebRequest<IRepository<ArticleToImage>, Repository<ArticleToImage>>();
			container.RegisterPerWebRequest<IRepository<User>, Repository<User>>();
			container.RegisterPerWebRequest<IRepository<Role>, Repository<Role>>();
			container.RegisterPerWebRequest<IRepository<UserInRole>, Repository<UserInRole>>();
			container.RegisterPerWebRequest<IRepository<Article>, Repository<Article>>();
			container.RegisterPerWebRequest<IRepository<Country>, Repository<Country>>();
			container.RegisterPerWebRequest<IRepository<City>, Repository<City>>();
			container.RegisterPerWebRequest<IRepository<SightType>, Repository<SightType>>();
			container.RegisterPerWebRequest<IRepository<ArticleToSightType>, Repository<ArticleToSightType>>();
		}
	}
}