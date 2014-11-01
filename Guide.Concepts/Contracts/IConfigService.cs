namespace Guide.Config.Contracts
{
	public interface IConfigService
	{
		string HttpImagesUrl { get; }
		string HttpQrImagesUrl { get; }
		string GoogleApiKey { get; }
		string FoursquareOauthToken { get; }
		string FullTextSearchIndexPath { get; }
		string FullTextSearchIndexAbsolutePath { get; }
		Languages DefaultLanguage { get; }
		int TeaserLetterCount { get; }
		string DefaultLanguageCode { get; }
		string LanguageCookieName { get; }
		string LanguageSessionKey { get; }
		string LanguageRouteParam { get; }
		int CountOfElementsOnPage { get; }
		string AppName { get; }
	}
}