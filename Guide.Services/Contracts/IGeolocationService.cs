using Guide.Model.Entities;

namespace Guide.Services.Contracts
{
	public interface IGeolocationService
	{
		bool FetchCoordinagesInSpb(Article article);
	}
}