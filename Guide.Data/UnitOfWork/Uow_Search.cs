using Guide.Data.Services;

namespace Guide.Data.UnitOfWork
{
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.IO;
	using System.Linq;

	using Guide.Config;
	using Guide.Data.FullTextSearchInfrastructure;
	using Guide.Model.Entities;

	using Lucene.Net.Documents;

	using SimpleLucene;
	using SimpleLucene.Impl;

	public partial class UnitOfWork
	{
		#region Public Methods and Operators

		public IEnumerable<Article> SearchArticles(string keywords)
		{
			var ids = fullTextSearch.Search(keywords, SearchType.Articles);


			switch (_languageService.CurrentLanguage)
			{
				case Languages.En:
					return
						Articles.All.Include(a => a.ArticleToImages.Select(ati=> ati.Image))
							.Include(a => a.City)
							.Include(a => a.Thumbnail)
							.Include(a => a.ArticleToSightTypes.Select(atst => atst.SightType))
							.Where(a => ids.Contains(a.Id))
							.OrderBy(a => a.HeaderEn).ToList();
				case Languages.Ru:
					return
						Articles.All.Include(a => a.ArticleToImages.Select(ati => ati.Image))
							.Include(a => a.Thumbnail)
							.Include(a => a.City)
							.Include(a => a.ArticleToSightTypes.Select(atst => atst.SightType))
							.Where(a => ids.Contains(a.Id))
							.OrderBy(a => a.HeaderRu).ToList();
			}

			return null;
		}

		public IEnumerable<Image> SearchImages(string keywords)
		{
			var ids = fullTextSearch.Search(keywords, SearchType.Images);
			return Images.All.Where(a => ids.Contains(a.Id));
		}

		#endregion
	}
}