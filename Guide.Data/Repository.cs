namespace Guide.Data
{
    using System.Data.Entity;
    using System.Linq;

    using Guide.Data.Contracts;
    using Guide.Model.Entities;

	public class Repository<T> : IRepository<T>
        where T : class
    {
		public Repository(GuideDbContext dbContext)
        {
	        this.DbContext = dbContext;
            this.DbSet = dbContext.Set<T>();
        }

        protected GuideDbContext DbContext { get; set; }

        protected DbSet<T> DbSet { get; set; }

        public IQueryable<T> All
        {
            get { return this.DbSet; }
        }

        public T GetById(int id)
        {

            return this.DbSet.Find(id);
        }

        public bool Insert(T entity)
        {
            try
            {
                var dbEntityEntry = this.DbContext.Entry(entity);
                if (dbEntityEntry.State != EntityState.Detached)
                {
                    dbEntityEntry.State = EntityState.Added;
                }
                else
                {
                    this.DbSet.Add(entity);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(T entity)
        {
            try
            {
                var dbEntityEntry = this.DbContext.Entry(entity);
                if (dbEntityEntry.State == EntityState.Detached)
                {
                    this.DbSet.Attach(entity);
                }
                dbEntityEntry.State = EntityState.Modified;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public virtual bool Delete(T entity)
        {
            try
            {
                var dbEntityEntry = this.DbContext.Entry(entity);
                if (dbEntityEntry.State != EntityState.Deleted)
                {
                    if (dbEntityEntry.State == EntityState.Detached)
                    {
                        this.DbSet.Attach(entity);
                    }
                    this.DbSet.Remove(entity);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            var entity = this.GetById(id);
            if (entity != null)
                return this.Delete(entity);
            return false;
        }
    }
}
