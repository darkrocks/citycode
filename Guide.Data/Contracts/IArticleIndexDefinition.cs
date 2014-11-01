namespace Guide.Data.FullTextSearchInfrastructure
{
	using Guide.Model.Entities;

	using Lucene.Net.Documents;

	public interface IArticleIndexDefinition
	{
		Document EntityToDoc(Article entity);
		int DocToEntityId(Document document);
		string[] Fields { get; }
	}
}