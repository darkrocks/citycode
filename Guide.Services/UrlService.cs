using System;
using System.Web;
using Guide.Config;
using Guide.Config.Contracts;
using Guide.Model.Entities;
using Guide.Services.Contracts;

namespace Guide.Services
{
	public class UrlService : IUrlService
	{
		private readonly IConfigService _configService;

		public UrlService(IConfigService configService)
		{
			_configService = configService;
		}

		public string GetImageUrl(string imageName, ImageTypes imageType = ImageTypes.Article)
		{
			switch (imageType)
			{
					case ImageTypes.Article:
					return String.Format("/{0}/{1}", _configService.HttpImagesUrl, imageName);
				case ImageTypes.Qr740x740:
							return String.Format("/{0}/{1}", _configService.HttpQrImagesUrl, imageName);

			}
			return string.Empty;
		}



		public string GetCurrentUrlForCulture(Languages language)
		{
			var url = HttpContext.Current.Request.Url.PathAndQuery;
			if (url == "" || url == "/")
			{
				return String.Format("/{0}/", language.ToString().Trim());
			}
			if (url.Length > 2)
			{
				var currentLanguageFromUrl = url.Substring(1, 2);
				return url.Replace(currentLanguageFromUrl, language.ToString().ToLower());	
			}
			throw new Exception("Unexpected");
		}

		public string PrepareToUrl(string text)
		{
			return text.Trim().ToLower().Replace(" ", "_").Replace(".", "_");
		}
	}
}