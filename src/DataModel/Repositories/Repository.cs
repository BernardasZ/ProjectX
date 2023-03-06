using DataModel.DbContexts;
using System.Linq;

namespace DataModel.Repositories;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    protected readonly ProjectXDbContext _context;

    public Repository(ProjectXDbContext context)
    {
        _context = context;
    }

	public IQueryable<TEntity> FetchAll() => _context.Set<TEntity>().AsQueryable();

	public TEntity GetById(int id) => _context.Set<TEntity>().Find(id);

	public void Insert(TEntity entity) => _context.Set<TEntity>().Add(entity);

	public void Update(TEntity entity) => _context.Set<TEntity>().Update(entity);

	public void Delete(TEntity entity) => _context.Set<TEntity>().Remove(entity);

	public void Save() => _context.SaveChanges();
}
