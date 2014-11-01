using System;
using System.Globalization;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using Guide.Config;
using Guide.Config.Contracts;
using Guide.Services;
using Guide.Services.Contracts;

namespace Guide.Web.Filters
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public sealed class LanguageDetectionAttribute : ActionFilterAttribute
	{
		private readonly IConfigService _cnfg;
		private readonly ILanguageService _languageService;

		public LanguageDetectionAttribute(IConfigService cnfg, ILanguageService languageService)
		{
			_cnfg = cnfg;
			_languageService = languageService;
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
		
			Languages languageToRedirect;

			var routeLang = filterContext.RouteData.Values["language"];
			if (routeLang != null)
			{
				if (Enum.TryParse(routeLang.ToString(), true, out languageToRedirect))
				{
					_languageService.CurrentLanguage = languageToRedirect;
					return;
				}
			}
			else
			{
				var cookie = filterContext.HttpContext.Request.Cookies[_cnfg.LanguageCookieName];
				if (cookie != null && Enum.TryParse(cookie.Value, true, out languageToRedirect))
				{
					RedirectToRoute(languageToRedirect, filterContext);
					return;
				}

				if (filterContext.HttpContext.Request.UserLanguages != null &&
					filterContext.HttpContext.Request.UserLanguages.Length > 0)
				{
					var browserLanguage = filterContext.HttpContext.Request.UserLanguages[0].ToLower();
					if (browserLanguage.Contains("en"))
					{
						RedirectToRoute(Languages.En, filterContext);
						return;
					}

					if (browserLanguage.Contains("ru"))
					{
						RedirectToRoute(Languages.Ru, filterContext);
						return;
					}
				}
			}

			RedirectToRoute(_cnfg.DefaultLanguage, filterContext);
		}

		private void RedirectToRoute(Languages language, ActionExecutingContext filterContext)
		{
			if (filterContext.HttpContext.Request.Url != null)
			{
				var redirectUrl = String.Format("http://{0}/{1}{2}", filterContext.HttpContext.Request.Url.Authority, language.ToString().ToLower(),
					filterContext.HttpContext.Request.Url.AbsolutePath);
				filterContext.Result = new RedirectResult(redirectUrl);
				return;
			}
			throw new Exception("Request.Url is null");
		}
	}
}
