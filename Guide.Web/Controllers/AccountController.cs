using System.Web.Mvc;

namespace Guide.Web.Controllers
{
	using System.Web.Routing;

	using Guide.Model.Model;
	using Guide.Services.Contracts;

	using WebMatrix.WebData;

	[Authorize]
	public class AccountController : Controller
	{
		private readonly ILanguageService languageService;

		public AccountController(ILanguageService languageService)
		{
			this.languageService = languageService;
		}


		[AllowAnonymous]
		public ActionResult Login(string returnUrl, string message = "")
		{
			ViewBag.ReturnUrl = returnUrl;
			ViewBag.Message = message;
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult Login(LoginModel model, string returnUrl)
		{
			if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
			{
				if (string.IsNullOrEmpty(returnUrl))
				{
					returnUrl = "/";
				}
				return this.Redirect(returnUrl);
			}

			// If we got this far, something failed, redisplay form
			ModelState.AddModelError("", "The user name or password provided is incorrect.");
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LogOff()
		{
			WebSecurity.Logout();

			return RedirectToAction("List", "Articles", new {cityId = 1});
		}
	}
}