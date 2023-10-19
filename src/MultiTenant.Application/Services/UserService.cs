using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MultiTenant.Application.Options;
using MultiTenant.Domain.Contracts.IRepositories;
using MultiTenant.Domain.Contracts.IServices;
using MultiTenant.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MultiTenant.Application.Services
{
    public class UserService : IUserService
    {
        private readonly HttpContext _context;
        private readonly IMasterUnitOfWork _masterUnitOfWork;
        private readonly IProductUnitOfWork _productUnitOfWork;
        private readonly AuthOptions _authOptions;

        public UserService(
            IHttpContextAccessor contextAccessor,
            IMasterUnitOfWork masterUnitOfWork,
            IProductUnitOfWork productUnitOfWork,
            IOptions<AuthOptions> authOptions)
        {
            _context = contextAccessor.HttpContext;
            _masterUnitOfWork = masterUnitOfWork;
            _productUnitOfWork = productUnitOfWork;
            _authOptions = authOptions.Value;
        }

        public string AddUser(User user)
        {
            user.Password = HashPassword(user.Password);

            var entityUser = _masterUnitOfWork.UserRepository.Add(user);
            entityUser.Organization.UserId = entityUser.Id;
            var entityOrg = _masterUnitOfWork.OrganizationRepository.Update(entityUser.Organization);
            _masterUnitOfWork.SaveChanges();

            _context.Items.Add("slugTenant", entityOrg.SlugTenant);

            _productUnitOfWork.MigrateDatabase();

            return entityOrg.SlugTenant;
        }

        public string AuthenticateUser(string email, string pwd)
        {
            var user = _masterUnitOfWork.UserRepository.Get()
                .Include(x => x.Organization).FirstOrDefault(x => x.Email == email);

            if (user is not null)
            {
                bool isValidPassword = ValidatePassword(pwd, user.Password);
                if (isValidPassword)
                {
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.SecretKey));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, email),
                        new Claim(ClaimTypes.NameIdentifier, user.Organization?.SlugTenant)
                    };

                    var token = new JwtSecurityToken
                    (
                        issuer: _authOptions.Issuer,
                        audience: _authOptions.Audience,
                        expires: DateTime.Now.AddHours(_authOptions.ExpirationTime),
                        claims: claims,
                        signingCredentials: credentials
                    );

                    return new JwtSecurityTokenHandler().WriteToken(token);
                }
            }
            return string.Empty;
        }



        #region Private Methods

        private static string HashPassword(string password)
        {
            byte[] salt;

            using (var rng = RandomNumberGenerator.Create())
            {
                salt = new byte[32];
                rng.GetBytes(salt);
            }

            using (var pbkd = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                byte[] hash = pbkd.GetBytes(32);
                byte[] bytes = new byte[salt.Length + hash.Length];

                Array.Copy(salt, 0, bytes, 0, salt.Length);
                Array.Copy(hash, 0, bytes, salt.Length, hash.Length);

                return Convert.ToBase64String(bytes);
            }
        }

        private static bool ValidatePassword(string pwd, string hash)
        {
            byte[] hashBytes = Convert.FromBase64String(hash);
            byte[] salt = new byte[32];

            Array.Copy(hashBytes, 0, salt, 0, 32);

            using (var pkbd = new Rfc2898DeriveBytes(pwd, salt, 10000))
            {
                byte[] toCheck = pkbd.GetBytes(32);
                for (int i = 0; i < 32; i++)
                {
                    if (hashBytes[i + 32] != toCheck[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        #endregion
    }
}
