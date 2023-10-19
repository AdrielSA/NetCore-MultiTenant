namespace MultiTenant.Application.DTOs
{
    public record UserDto
    (
        string Email,
        string Password,
        OrganizationDto Organization
    );
}
