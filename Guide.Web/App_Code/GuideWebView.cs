using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Guide.Services;
using Guide.Services.Contracts;

namespace Guide.Web.WebViews
{
	using Guide.Config.Contracts;

	public abstract class GuideWebView<T> : System.Web.Mvc.WebViewPage<T>
	{
		IConfigService _cnfg;
		ILanguageService _languageService;
		protected override void InitializePage()
		{
			_cnfg = DependencyResolver.Current.GetService<IConfigService>();
			_languageService = DependencyResolver.Current.GetService<ILanguageService>();
			SetViewBagDefaultProperties();
			base.InitializePage();
		}

		private void SetViewBagDefaultProperties()
		{
			ViewBag.AppName = _cnfg.AppName;
			ViewBag.CurrentLanguageName = _languageService.CurrentLanguageFriendlyName;
			ViewBag.CurrentLanguage = _languageService.CurrentLanguageUrlVersion;
			ViewBag.FoursquareOAuthToken = _cnfg.FoursquareOauthToken;
		}
	}
}