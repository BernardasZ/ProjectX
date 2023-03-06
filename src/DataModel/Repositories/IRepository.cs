using System.Linq;

namespace DataModel.Repositories;

public interface IRepository<TEntity>
    where TEntity : class
{
    IQueryable<TEntity> FetchAll();

    TEntity GetById(int id);

    void Insert(TEntity entity);

    void Update(TEntity entity);

    void Delete(TEntity entity);

    void Save();
}
