using MultiTenant.Domain.Entities.Common;

namespace MultiTenant.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int OrganizationId { get; set; }

        public Organization Organization { get; set; }
    }
}
