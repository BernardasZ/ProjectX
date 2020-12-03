using DataModel.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataModel.Repositories
{
	public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> FetchAll();
        TEntity GetById(int id);
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Save();
    }

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class 
    {
        protected readonly ProjectXDbContext context;
        public Repository(ProjectXDbContext context)
        {
            this.context = context;
        }

        public IQueryable<TEntity> FetchAll()
        {
            return context.Set<TEntity>().AsQueryable();
        }

        public TEntity GetById(int id)
        {
            return context.Set<TEntity>().Find(id);
        }

        public void Insert(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
        }
        public void Update(TEntity entity)
        {
            context.Set<TEntity>().Update(entity);
        }

        public void Delete(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
        }
        public void Save()
        {
            context.SaveChanges();
        }
    }
}
