namespace MultiTenant.Application.DTOs
{
    public record OrganizationDto
    (
        string Name,
        string SlugTenant
    );
}
