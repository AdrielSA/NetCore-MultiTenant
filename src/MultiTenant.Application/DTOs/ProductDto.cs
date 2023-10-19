namespace MultiTenant.Application.DTOs
{
    public record ProductDto
    (
        int Id,
        string Name,
        string Description,
        decimal Duration
    );
}
