namespace Guide.Data.FullTextSearchInfrastructure
{
	using System;
	using System.Globalization;
	using System.Text;

	using Guide.Data.Contracts;
	using Guide.Model.Entities;

	using Lucene.Net.Documents;

	public class ImageIndexDefinition : IImageIndexDefinition
	{
		public  Document EntityToDoc(Image entity)
        {
            var document = new Document();
            document.Add(new Field("Id", entity.Id.ToString(CultureInfo.InvariantCulture), Field.Store.YES, Field.Index.NOT_ANALYZED));
			document.Add(new Field("Name", entity.Name, Field.Store.YES, Field.Index.ANALYZED));
			document.Add(new Field("Sizes", entity.Size, Field.Store.YES, Field.Index.ANALYZED));
            return document;
        }

		public  int DocToEntityId(Document document)
		{
			return Convert.ToInt32(document.Get("Id"));
		}

		public string[] Fields {
			get
			{
				return new[] { "Id", "Name", "Sizes"};
			}
		}
    }
}