using MultiTenant.Domain.Entities.Common;

namespace MultiTenant.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Duration { get; set; }
    }
}
