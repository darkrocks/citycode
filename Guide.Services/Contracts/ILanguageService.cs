using Guide.Config;

namespace Guide.Services.Contracts
{
	public interface ILanguageService
	{
		Languages CurrentLanguage { get; set; }
		string CurrentLanguageUrlVersion { get; }
		string CurrentLanguageFriendlyName { get; }
		string GetLanguageFriendlyName(Languages language);
	}
}