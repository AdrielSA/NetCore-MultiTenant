using MultiTenant.Domain.Entities;

namespace MultiTenant.Domain.Contracts.IRepositories
{
    public interface IMasterUnitOfWork : IDisposable
    {
        IRepository<User> UserRepository { get; }
        IRepository<Organization> OrganizationRepository { get; }

        int SaveChanges();
    }
}