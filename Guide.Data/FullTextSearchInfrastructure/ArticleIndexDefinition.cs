namespace Guide.Data.FullTextSearchInfrastructure
{
	using System;
	using System.Globalization;
	using System.Text;

	using Guide.Data.Contracts;
	using Guide.Model.Entities;

	using Lucene.Net.Documents;

	public class ArticleIndexDefinition : IArticleIndexDefinition
	{
		public  Document EntityToDoc(Article entity)
        {
			if (entity.ArticleToSightTypes == null)
			{
				throw new Exception("FTS indexing error");
			}
            var document = new Document();
            document.Add(new Field("Id", entity.Id.ToString(CultureInfo.InvariantCulture), Field.Store.YES, Field.Index.NOT_ANALYZED));
			string headerRu = string.Empty;
			if (!string.IsNullOrEmpty(entity.HeaderRu))
			{
				headerRu = entity.HeaderRu;
			}
			string headerEn = string.Empty;
			if (!string.IsNullOrEmpty(entity.HeaderEn))
			{
				headerEn = entity.HeaderEn;
			}
			document.Add(new Field("HeaderEn", headerRu, Field.Store.YES, Field.Index.ANALYZED));
			document.Add(new Field("HeaderRu", headerEn, Field.Store.YES, Field.Index.ANALYZED));

			var sightTypesEn = new StringBuilder();
			var sightTypesRu = new StringBuilder();
			foreach (var articleToSightType in entity.ArticleToSightTypes)
			{
				sightTypesEn.Append(articleToSightType.SightType.NameEn);
				sightTypesEn.Append(" ");
				sightTypesRu.Append(articleToSightType.SightType.NameRu);
				sightTypesRu.Append(" ");
			}

			document.Add(new Field("SightTypesEn", sightTypesEn.ToString(), Field.Store.YES, Field.Index.ANALYZED));
			document.Add(new Field("SightTypesRu", sightTypesRu.ToString(), Field.Store.YES, Field.Index.ANALYZED));
            return document;
        }

		public  int DocToEntityId(Document document)
		{
			return Convert.ToInt32(document.Get("Id"));
		}

		public string[] Fields {
			get
			{
				return new[] { "Id", "HeaderEn", "HeaderRu", "SightTypesEn", "SightTypesRu" };
			}
		}
    }
}