using Microsoft.EntityFrameworkCore;
using MultiTenant.Domain.Contracts.IRepositories;
using MultiTenant.Domain.Entities.Common;

namespace MultiTenant.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly DbContext _context;

        public Repository(DbContext context)
        {
            _context = context;
        }

        public virtual T GetById(object id)
            => _context.Set<T>().Find(id);

        public virtual IQueryable<T> Get()
            => _context.Set<T>().AsQueryable();

        public virtual T Add(T entity)
            => _context.Set<T>().Add(entity).Entity;

        public virtual T Update(T entity)
            => _context.Set<T>().Update(entity).Entity;

        public virtual void Delete(T entity)
            => _context.Set<T>().Remove(entity);
    }
}
