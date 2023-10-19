using MultiTenant.Domain.Entities.Common;

namespace MultiTenant.Domain.Entities
{
    public class Organization : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string SlugTenant { get; set; } = string.Empty;
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
