using Guide.Model.Entities;
using Lucene.Net.Documents;

namespace Guide.Data.Contracts
{
	public interface IImageIndexDefinition
	{
		Document EntityToDoc(Image entity);
		int DocToEntityId(Document document);
		string[] Fields { get; }
	}
}