using Guide.Config;
using Guide.Model.Entities;

namespace Guide.Services.Contracts
{
	public interface IUrlService
	{
		string GetImageUrl(string imageName, ImageTypes imageType = ImageTypes.Article);
		string GetCurrentUrlForCulture(Languages language);
		string PrepareToUrl(string text);
	}
}