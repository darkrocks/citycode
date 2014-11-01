namespace Guide.Data.Services
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System.Linq;
	using Guide.Config.Contracts;
	using Guide.Data.Contracts;
	using Guide.Data.FullTextSearchInfrastructure;
	using Guide.Model.Entities;
	using Lucene.Net.Analysis.Standard;
	using Lucene.Net.Documents;
	using Lucene.Net.Index;
	using Lucene.Net.QueryParsers;
	using Lucene.Net.Search;
	using Lucene.Net.Store;
	using Version = Lucene.Net.Util.Version;

	public enum SearchType
	{
		Articles,
		Images
	}

	public class FullTextSearchService : IFullTextSearchService
	{
		private const string ArticlesPath = "/Articles";
		private const string ImagesPath = "/Images";
		private readonly IConfigService config;

		private readonly IArticleIndexDefinition _articleIndexDefinition;
		private readonly IImageIndexDefinition _imageIndexDefinition;

		public FullTextSearchService(IConfigService config,
			IArticleIndexDefinition articleIndexDefinition,
			IImageIndexDefinition imageIndexDefinition)
		{
			this.config = config;
			this._articleIndexDefinition = articleIndexDefinition;
			_imageIndexDefinition = imageIndexDefinition;
		}

		private FSDirectory articlesDirectoryTemp;
		private FSDirectory imagesDirectoryTemp;

		private string ArticlesDirectoryPath
		{
			get { return string.Format("{0}{1}", this.config.FullTextSearchIndexAbsolutePath, ArticlesPath); }
		}

		private string ImagesDirectoryPath
		{
			get { return string.Format("{0}{1}", this.config.FullTextSearchIndexAbsolutePath, ImagesPath); }
		}

		private DirectoryInfo ArticlesDirectory
		{
			get { return new DirectoryInfo(string.Format("{0}{1}", this.config.FullTextSearchIndexAbsolutePath, ArticlesPath)); }
		}

		private DirectoryInfo ImagesDirectory
		{
			get { return new DirectoryInfo(string.Format("{0}{1}", this.config.FullTextSearchIndexAbsolutePath, ImagesPath)); }
		}

		private FSDirectory GetDirectory(SearchType searchType)
		{
			string lockFilePath = "";
			switch (searchType)
			{
				case SearchType.Articles:
					if (this.articlesDirectoryTemp == null)
					{
						this.articlesDirectoryTemp = FSDirectory.Open(ArticlesDirectory);
					}
					if (IndexWriter.IsLocked(this.articlesDirectoryTemp))
					{
						IndexWriter.Unlock(this.articlesDirectoryTemp);
					}
					lockFilePath = Path.Combine(ArticlesDirectoryPath, "write.lock");
					if (File.Exists(lockFilePath))
					{
						File.Delete(lockFilePath);
					}
					return this.articlesDirectoryTemp;

				case SearchType.Images:
					if (this.imagesDirectoryTemp == null)
					{
						this.imagesDirectoryTemp = FSDirectory.Open(ImagesDirectory);
					}
					if (IndexWriter.IsLocked(this.imagesDirectoryTemp))
					{
						IndexWriter.Unlock(this.imagesDirectoryTemp);
					}
					lockFilePath = Path.Combine(ImagesDirectoryPath, "write.lock");
					if (File.Exists(lockFilePath))
					{
						File.Delete(lockFilePath);
					}
					return this.imagesDirectoryTemp;
			}
			return null;
		}

		public List<int> Search(string input, SearchType searchType, string fieldName = "")
		{
			if (string.IsNullOrEmpty(input)) return new List<int>();

			var terms = input.Trim().Replace("-", " ").Split(' ')
				.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");
			input = string.Join(" ", terms);

			if (string.IsNullOrEmpty(input.Replace("*", "").Replace("?", "")) ||
			    !this.GetDirectory(searchType).Directory.Exists ||
			    this.GetDirectory(searchType).Directory.GetFiles().Length == 0) return new List<int>();

			using (var searcher = new IndexSearcher(this.GetDirectory(searchType), false))
			{
				var hits_limit = 1000;
				var analyzer = new StandardAnalyzer(Version.LUCENE_29);
				if (!string.IsNullOrEmpty(fieldName))
				{
					var parser = new QueryParser(Version.LUCENE_29, fieldName, analyzer);
					var query = this._parseQuery(input, parser);
					var hits = searcher.Search(query, hits_limit).ScoreDocs;
					var results = this.MapLuceneToDataList(hits, searcher, searchType);
					analyzer.Close();
					searcher.Dispose();
					return results.ToList();
				}
				else
				{
					string[] fields = null;
					switch (searchType)
					{
						case SearchType.Articles:
							fields = _articleIndexDefinition.Fields;
							break;
						case SearchType.Images:
							fields = _imageIndexDefinition.Fields;
							break;
					}
					var parser = new MultiFieldQueryParser(Version.LUCENE_29, fields, analyzer);
					var query = this._parseQuery(input, parser);
					var hits = searcher.Search(query, null, hits_limit, Sort.INDEXORDER).ScoreDocs;
					var results = this.MapLuceneToDataList(hits, searcher, searchType);
					analyzer.Close();
					searcher.Dispose();
					return results.ToList();
				}
			}
		}

		public void AddUpdate(object entity, SearchType searchType)
		{
			var analyzer = new StandardAnalyzer(Version.LUCENE_29);
			using (var writer = new IndexWriter(this.GetDirectory(searchType), analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
			{
				TermQuery searchQuery = null;
				switch (searchType)
				{
					case SearchType.Articles:
						var article = (Article) entity;
						searchQuery = new TermQuery(new Term("Id", article.Id.ToString(CultureInfo.InvariantCulture)));
						writer.DeleteDocuments(searchQuery);
						writer.AddDocument(this._articleIndexDefinition.EntityToDoc(article));
						break;
					case SearchType.Images:
						var image = (Image) entity;
						searchQuery = new TermQuery(new Term("Id", image.Id.ToString(CultureInfo.InvariantCulture)));
						writer.DeleteDocuments(searchQuery);
						writer.AddDocument(this._imageIndexDefinition.EntityToDoc(image));
						break;
				}

				analyzer.Close();
				writer.Dispose();
			}
			this.Optimize(searchType);
		}

		public void AddUpdateList(IEnumerable<object> entities, SearchType searchType)
		{
			foreach (var entity in entities)
			{
				this.AddUpdate(entity, searchType);
			}
		}

		public void Delete(int id, SearchType searchType)
		{
			// init lucene
			var analyzer = new StandardAnalyzer(Version.LUCENE_29);
			using (var writer = new IndexWriter(this.GetDirectory(searchType), analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
			{
				// remove older index entry
				var searchQuery = new TermQuery(new Term("Id", id.ToString()));
				writer.DeleteDocuments(searchQuery);

				// close handles
				analyzer.Close();
				writer.Dispose();
			}
			this.Optimize(searchType);
		}

		public bool DeleteAll(SearchType searchType)
		{
			try
			{
				var analyzer = new StandardAnalyzer(Version.LUCENE_29);
				using (
					var writer = new IndexWriter(this.GetDirectory(searchType), analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
				{
					// remove older index entries
					writer.DeleteAll();

					// close handles
					analyzer.Close();
					writer.Dispose();
				}
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		public void Optimize(SearchType searchType)
		{
			var analyzer = new StandardAnalyzer(Version.LUCENE_29);
			using (var writer = new IndexWriter(this.GetDirectory(searchType), analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
			{
				analyzer.Close();
				writer.Optimize();
				writer.Dispose();
			}
		}

		private IEnumerable<int> MapLuceneToDataList(IEnumerable<ScoreDoc> hits, IndexSearcher searcher, SearchType searchType)
		{
			return hits.Select(hit => this.MapLuceneDocumentToData(searcher.Doc(hit.Doc), searchType)).ToList();
		}

		private int MapLuceneDocumentToData(Document doc, SearchType searchType)
		{
			switch (searchType)
			{
				case SearchType.Articles:
					return this._articleIndexDefinition.DocToEntityId(doc);
				case SearchType.Images:
					return this._imageIndexDefinition.DocToEntityId(doc);
			}
			return 0;
		}

		private Query _parseQuery(string searchQuery, QueryParser parser)
		{
			Query query;
			try
			{
				query = parser.Parse(searchQuery.Trim());
			}
			catch (ParseException)
			{
				query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
			}
			return query;
		}
	}
}