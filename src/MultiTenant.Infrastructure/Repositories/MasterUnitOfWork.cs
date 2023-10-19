using MultiTenant.Domain.Contracts.IRepositories;
using MultiTenant.Domain.Entities;
using MultiTenant.Infrastructure.DbContexts;

namespace MultiTenant.Infrastructure.Repositories
{
    public class MasterUnitOfWork : IMasterUnitOfWork
    {
        private readonly MasterDbContext _context;
        private bool _disposed;

        ~MasterUnitOfWork() => Dispose(false);
        public MasterUnitOfWork(MasterDbContext context)
        {
            _context = context;

            UserRepository = new Repository<User>(_context);
            OrganizationRepository = new Repository<Organization>(_context);
        }

        public IRepository<User> UserRepository { get; private set; }
        public IRepository<Organization> OrganizationRepository { get; private set; }


        public int SaveChanges() => _context.SaveChanges();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(!disposing && _disposed)
                _context.Dispose();
            _disposed = true;
        }
    }
}
