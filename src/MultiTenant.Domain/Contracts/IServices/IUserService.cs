using MultiTenant.Domain.Entities;

namespace MultiTenant.Domain.Contracts.IServices
{
    public interface IUserService
    {
        string AddUser(User user);
        string AuthenticateUser(string email, string pwd);
    }
}