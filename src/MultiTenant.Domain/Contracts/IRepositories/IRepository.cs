using MultiTenant.Domain.Entities.Common;

namespace MultiTenant.Domain.Contracts.IRepositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        T Add(T entity);
        void Delete(T entity);
        IQueryable<T> Get();
        T GetById(object id);
        T Update(T entity);
    }
}