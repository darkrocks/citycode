using System.Collections.Generic;
using Guide.Data.Services;

namespace Guide.Data.Contracts
{
	using Guide.Model.Entities;

	public interface IFullTextSearchService
	{
		List<int> Search(string input, SearchType searchType, string fieldName = "");
		void AddUpdate(object entity, SearchType searchType);
		void AddUpdateList(IEnumerable<object> entities, SearchType searchType);
		void Delete(int id, SearchType searchType);
		bool DeleteAll(SearchType searchType);
	}

}
