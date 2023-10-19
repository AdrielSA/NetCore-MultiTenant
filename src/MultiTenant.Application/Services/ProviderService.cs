using Microsoft.AspNetCore.Http;
using MultiTenant.Domain.Contracts.IRepositories;
using MultiTenant.Domain.Contracts.IServices;

namespace MultiTenant.Application.Services
{
    public class ProviderService : IProviderService
    {
        private readonly HttpContext _httpContext;
        private readonly IMasterUnitOfWork _masterUnitOfWork;

        public ProviderService(IHttpContextAccessor contextAccessor, IMasterUnitOfWork masterUnitOfWork)
        {
            _httpContext = contextAccessor.HttpContext;
            _masterUnitOfWork = masterUnitOfWork;
        }

        public string GetConnetionString()
        {
            var slugTenant = _httpContext.Items["slugTenant"]?.ToString();
            var org = _masterUnitOfWork.OrganizationRepository
                .Get().FirstOrDefault(x => x.SlugTenant == slugTenant);

            return org is not null 
                ? $"Server=LPTFGJC1T3;Database=DataBase-{org.SlugTenant};Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true;" 
                : throw new ArgumentException("Invalid slugTenant");
        }
    }
}
