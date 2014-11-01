using System.Linq;

namespace Guide.Data.Contracts
{
    public interface IRepository<T>
    {
        IQueryable<T> All { get; }
        T GetById(int id);
        bool Insert(T entity);
        bool Update(T entity);
        bool Delete(T entity);
        bool Delete(int id);
    }
}
