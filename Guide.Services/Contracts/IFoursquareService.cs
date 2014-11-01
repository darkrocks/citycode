using Guide.Model.Entities;

namespace Guide.Services.Contracts
{
	public interface IFoursquareService
	{
		bool FetchArticleData(Article article);
	}
}