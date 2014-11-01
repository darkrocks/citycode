using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Guide.Config;
using Guide.Data.Contracts;
using Guide.Data.UnitOfWork;
using Guide.Services;
using Guide.Services.Contracts;
using Microsoft.Ajax.Utilities;

namespace Guide.Web.Infrastructure.Extensions
{
	public static class HtmlExtensions
	{
		static readonly ILanguageService LanguageService = DependencyResolver.Current.GetService<ILanguageService>();
		static readonly IUrlService UrlService = DependencyResolver.Current.GetService<IUrlService>();
		static readonly IUnitOfWork Unit = DependencyResolver.Current.GetService<UnitOfWork>();
		public static MvcHtmlString HomeLink(this HtmlHelper helper, List<string> cssClasses, string text = "Home")
		{
			var builder = new TagBuilder("a");
			builder.Attributes["href"] = String.Format("/{0}", LanguageService.CurrentLanguageUrlVersion);
			if (cssClasses != null)
			{
				foreach (var cssClass in cssClasses)
				{
					builder.AddCssClass(cssClass);
				}
			}
			builder.InnerHtml = text;
			return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
		}

		public static string GetHomeUrl(this HtmlHelper helper)
		{
			return String.Format("/{0}", LanguageService.CurrentLanguageUrlVersion);
		}

		public static MvcHtmlString LanguageLink(this HtmlHelper helper, List<string> cssClasses, Languages language)
		{
			var builder = new TagBuilder("a");
			builder.Attributes["href"] = UrlService.GetCurrentUrlForCulture(language);
			if (cssClasses != null)
			{
				foreach (var cssClass in cssClasses)
				{
					builder.AddCssClass(cssClass);
				}
			}
			builder.InnerHtml = LanguageService.GetLanguageFriendlyName(language);
			return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
		}

		public static string GetUrlForLanguageLink(this HtmlHelper helper, Languages language)
		{
			return UrlService.GetCurrentUrlForCulture(language);
		}

		public static string GetSecondLanguageUrl(this HtmlHelper helper)
		{
			switch (LanguageService.CurrentLanguage)
			{
					case Languages.En:
					return UrlService.GetCurrentUrlForCulture(Languages.Ru);
					case Languages.Ru:
					return UrlService.GetCurrentUrlForCulture(Languages.En);
			}
			return "";
		}

		public static string GetLabelForSightTypeLink(this HtmlHelper helper, string sightTypeParam)
		{
			var type = Unit.GetSightType(sightTypeParam);
			if (type == null)
			{
				return "";
			}
			switch (LanguageService.CurrentLanguage)
			{
				case Languages.En:
					return type.PluralNameEn;
				case Languages.Ru:
					return type.PluralNameRu;
			}
			return "";
		}

		public static MvcHtmlString ValidationSummaryCustom(this HtmlHelper helper, string validationErrorsFromUrl)
		{
			if (validationErrorsFromUrl == null 
				|| string.IsNullOrWhiteSpace(validationErrorsFromUrl))
			{
				return MvcHtmlString.Empty;	
			}

			var divBuilder = new TagBuilder("div");
			divBuilder.Attributes["data-valmsg-summary"] = "true";
			divBuilder.AddCssClass("validation-summary-errors");

			var ulBuilder = new TagBuilder("ul");

			var errors = validationErrorsFromUrl.Split(';');
			foreach (var error in errors)
			{
				if (!string.IsNullOrWhiteSpace(error))
				{
					var liBuilder = new TagBuilder("li");
					liBuilder.InnerHtml = error;
					ulBuilder.InnerHtml += liBuilder.ToString();
				}
			}

			divBuilder.InnerHtml = ulBuilder.ToString();
			return MvcHtmlString.Create(divBuilder.ToString());
		}

		public static MvcHtmlString LabelCustom(this HtmlHelper helper, string text)
		{
			if (text == null
				|| string.IsNullOrWhiteSpace(text))
			{
				return MvcHtmlString.Empty;
			}
			var divBuilder = new TagBuilder("div");
			divBuilder.AddCssClass("alert alert-success");
			divBuilder.InnerHtml = text;
			return MvcHtmlString.Create(divBuilder.ToString());
		}
	}
}