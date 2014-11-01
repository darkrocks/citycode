using System.Data.Entity;
using Guide.Model.Entities;

namespace Guide.Data
{
	using System.Data.Entity.Infrastructure;

	public class MigrationsContextFactory : IDbContextFactory<GuideDbContext>
	{
		public GuideDbContext Create()
		{
			return new GuideDbContext();
		}
	}

	public class GuideDbContext : DbContext
	{
		public GuideDbContext()
			: base("Guide")
		{
			Database.SetInitializer<GuideDbContext>(null);
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			Configuration.ProxyCreationEnabled = false;
			Configuration.LazyLoadingEnabled = false;

			base.OnModelCreating(modelBuilder);
		}

		public DbSet<Article> Articles { get; set; }
		public DbSet<City> Cities { get; set; }
		public DbSet<Country> Countries { get; set; }
		public DbSet<Image> Images { get; set; }
		public DbSet<LogMessage> LogMessages { get; set; }
		public DbSet<ArticleToImage> ArticleToImages { get; set; }
		public DbSet<SightType> SightTypes { get; set; }
		public DbSet<ArticleToSightType> ArticleToSightTypes { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<UserInRole> UsersInRoles { get; set; }
	}
}