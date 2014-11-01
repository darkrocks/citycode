namespace Guide.Services.Contracts
{
	using Guide.Config;
	using Guide.Model.Entities;

	public interface IArticleListTitleService
	{
		string Generate(City city, SightType sightType, Languages language);
		string Generate(City city, SightType sightType);
	}
}