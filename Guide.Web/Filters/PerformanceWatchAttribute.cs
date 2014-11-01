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
	using System.Diagnostics;

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public sealed class PerformanceWatchAttribute : ActionFilterAttribute
	{
		private readonly IConfigService configService;

		public PerformanceWatchAttribute(IConfigService configService)
		{
			this.configService = configService;
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (filterContext.HttpContext.Session != null)
			{
				filterContext.HttpContext.Session["PerformanceWatch"] = Stopwatch.StartNew();
			}
		}

		public override void OnResultExecuting(ResultExecutingContext filterContext)
		{
			var viewResult = filterContext.Result as ViewResult;
			if (viewResult != null)
			{
				if (filterContext.HttpContext.Session != null)
				{
					var watch = filterContext.HttpContext.Session["PerformanceWatch"] as Stopwatch;
					if (watch == null)
					{
						return;
					}
					watch.Stop();
					viewResult.ViewBag.PerformanceWatch = string.Format("{0}ms", watch.ElapsedMilliseconds);
				}
			}


		}
	}
}
