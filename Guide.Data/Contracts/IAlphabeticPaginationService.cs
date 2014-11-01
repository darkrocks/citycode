namespace Guide.Data.Contracts
{
	using System.Collections.Generic;
	using System.Linq;

	using Guide.Data.Services;
	using Guide.Model.Entities;
	using Guide.Services;

	public interface IAlphabeticPaginationService
	{
		List<AlphabeticalPage> GetPageLinks(List<Article> articles, string cityCode, string activeLetter);
	}
}