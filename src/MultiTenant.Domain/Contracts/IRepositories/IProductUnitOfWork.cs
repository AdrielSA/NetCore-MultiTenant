using MultiTenant.Domain.Entities;

namespace MultiTenant.Domain.Contracts.IRepositories
{
    public interface IProductUnitOfWork : IDisposable
    {
        IRepository<Product> ProductRepository { get; }

        void MigrateDatabase();
        int SaveChanges();
    }
}