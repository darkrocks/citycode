namespace Guide.Data.Contracts
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Guide.Model.Entities;

	public interface IUnitOfWork : IDisposable
	{
		#region Public Properties

		IRepository<ArticleToImage> ArticleToImages { get; }
		IRepository<ArticleToSightType> ArticleToSightTypes { get; }
		IRepository<Article> Articles { get; }
		IRepository<City> Cities { get; }
		IRepository<Country> Countries { get; }
		GuideDbContext DbContext { get; }
		bool DbInitialized { get; }
		IRepository<Image> Images { get; }
		IRepository<Role> Roles { get; }
		IRepository<SightType> SightTypes { get; }
		IRepository<UserInRole> UserInRoles { get; }
		IRepository<User> Users { get; }

		#endregion

		#region Public Methods and Operators

		bool Delete(Article article);
		bool Delete(Image image);

		IQueryable<Article> GetArticles(
			City city,
			int? sightTypeId = null,
			bool onlyPublished = false,
			string letter = null);

		IQueryable<Article> GetArticlesLight(
			City city,
			int? sightTypeId = null,
			bool onlyPublished = false,
			string letter = null);

		SightType GetSightType(string urlParam);

		void Index(Article article);
		void Index(Image image);
		bool Save();
		IEnumerable<Article> SearchArticles(string keywords);
		IEnumerable<Image> SearchImages(string keywords);

		#endregion
	}
}