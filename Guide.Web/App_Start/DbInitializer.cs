using Guide.Data;
using System;
using System.Web.Mvc;
using Guide.Model.Entities;

namespace Guide.Web.App_Start
{
	using System.Threading;

	using Guide.Data.Contracts;

	using WebMatrix.WebData;

	public static class DbInitializer
	{
		private static SimpleMembershipInitializer _initializer;
		private static object _initializerLock = new object();
		private static bool _isInitialized;

		public static readonly string InitialPasswordForUsers = "123456";
		public static void Initialize(IDependencyResolver dependencyResolver)
		{
			var unitOfWork = dependencyResolver.GetService<IUnitOfWork>();
			if (!unitOfWork.DbInitialized)
			{
				var dbContext = dependencyResolver.GetService<GuideDbContext>();
				var initializer = dependencyResolver.GetService<GuideDatabaseInitializer>();
				initializer.Seed(dbContext);
			}

			LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
		}
	}

	public class SimpleMembershipInitializer
	{
		public SimpleMembershipInitializer()
		{

			try
			{
				WebSecurity.InitializeDatabaseConnection("Guide", "Users", "Id", "Name", autoCreateTables: true);

				if (!WebSecurity.UserExists("Dima") && !WebSecurity.UserExists("Sasha"))
				{
					var uow = DependencyResolver.Current.GetService<IUnitOfWork>();

					WebSecurity.CreateUserAndAccount("Dima", "123456");
					uow.UserInRoles.Insert(new UserInRole()
					{
						UserId = WebSecurity.GetUserId("Dima"),
						RoleId = UserRoles.ApplicationAdministrator.RoleId
					});

					WebSecurity.CreateUserAndAccount("Sasha", "123456");
					uow.UserInRoles.Insert(new UserInRole()
					{
						UserId = WebSecurity.GetUserId("Sasha"),
						RoleId = UserRoles.ApplicationAdministrator.RoleId
					});
					uow.Save();
				}
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("Database could not be initialized.", ex);
			}
		}
	}
}