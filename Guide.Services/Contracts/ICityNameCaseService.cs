using Guide.Model.Entities;

namespace Guide.Services.Contracts
{
	using Guide.Config;

	public interface ICityNameCaseService
	{
		string GetName(City city, PossessiveCase possessiveCase, Languages language);
		string GetName(City city, PossessiveCase possessiveCase);
	}
}