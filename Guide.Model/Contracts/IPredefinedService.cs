using System.Collections.Generic;
using Guide.Model.Entities;

namespace Guide.Model.Contracts
{
	using Guide.Model.Model;

	public interface IPredefinedService
	{
		List<Country> Countries { get; }
		List<City> Cities { get; }
		List<SightType> SightTypes{ get; }
		List<ImportanceModel> ImportanceModels { get; }
	}
}