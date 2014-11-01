using System.Web;
using System.Web.Mvc;
using Guide.Config.Contracts;
using Guide.Services;
using Guide.Services.Contracts;
using Guide.Web.Filters;

namespace Guide.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters, IDependencyResolver dependencyResolver)
        {
			filters.Add(new HandleErrorAttribute());
			filters.Add(new LanguageDetectionAttribute(dependencyResolver.GetService<IConfigService>(),
				dependencyResolver.GetService<ILanguageService>()));
			filters.Add(new PerformanceWatchAttribute(dependencyResolver.GetService<IConfigService>()));
        }
    }
}
