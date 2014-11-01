using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Guide.Config;
using Guide.Config.Contracts;
using Guide.Data.Contracts;

namespace Guide.Web
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			var cnfg = DependencyResolver.Current.GetService<IConfigService>();

			routes.LowercaseUrls = true;
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


			routes.MapRoute(
				name: "Default",
				url: "",
				defaults: new { controller = "home", action = "index" });

			//routes.MapRoute(
			//	name: "DetailsNoLang",
			//	url: "a/{id}/{city}/{name}",
			//	defaults: new { controller = "Articles", action = "Details" });

			routes.MapRoute(
				name: "Details",
				url: "{language}/a/{id}/{sightTypeId}/{city}/{title}",
				defaults: new { controller = "Articles", action = "Details"});

			routes.MapRoute(
				name: "Details2",
				url: "{language}/a/{id}/{city}/{title}",
				defaults: new { controller = "Articles", action = "Details"});

			routes.MapRoute(
				name: "Details3",
				url: "{language}/a/{id}",
				defaults: new { controller = "Articles", action = "Details"});

			routes.MapRoute(
				name: "Details4",
				url: "a/{id}/{sightTypeId}/{city}/{title}",
				defaults: new { controller = "Articles", action = "Details" });

			routes.MapRoute(
				name: "Details5",
				url: "a/{id}/{city}/{title}",
				defaults: new { controller = "Articles", action = "Details" });

			routes.MapRoute(
				name: "Details6",
				url: "a/{id}",
				defaults: new { controller = "Articles", action = "Details" });

			routes.MapRoute(
				name: "SpbArticlesList4",
				url: "{language}/l/{cityId}/{sightTypeId}/{page}/{title}",
				defaults: new { controller = "Articles", action = "List" });

			routes.MapRoute(
				name: "SpbArticlesList3",
				url: "{language}/l/{cityId}/{sightTypeId}/{title}",
				defaults: new { controller = "Articles", action = "List" });


			routes.MapRoute(
				name: "SpbArticlesList",
				url: "{language}/l/{cityId}/{sightTypeId}",
				defaults: new { controller = "Articles", action = "List" });

			routes.MapRoute(
				name: "SpbArticlesList5",
				url: "{language}/l/{cityId}",
				defaults: new { controller = "Articles", action = "List", sightTypeId = 0 });


			routes.MapRoute(
				name: "Login",
				url: "{language}/account/login",
				defaults: new { controller = "account", action = "login" });

			routes.MapRoute(
				name: "Login2",
				url: "account/login",
				defaults: new { controller = "account", action = "login" });

			routes.MapRoute(
				name: "DefaultFull",
				url: "{language}/{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
		}
	}
}
