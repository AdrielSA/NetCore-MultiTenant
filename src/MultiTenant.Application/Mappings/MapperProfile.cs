using AutoMapper;
using MultiTenant.Application.DTOs;
using MultiTenant.Domain.Entities;

namespace MultiTenant.Application.Mappings
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserDto, User>()
                .ForMember(x => x.Id, c => c.Ignore())
                .ForMember(x => x.Email, c => c.MapFrom(d => d.Email))
                .ForMember(x => x.Password, c => c.MapFrom(d => d.Password))
                .ForMember(x => x.OrganizationId, c => c.Ignore())
                .ForMember(x => x.Organization, c => c.MapFrom(d => new Organization
                {
                    Name = d.Organization.Name,
                    SlugTenant = d.Organization.SlugTenant
                })).ReverseMap();

            CreateMap<ProductDto, Product>()
                .ForMember(x => x.Id, c => c.MapFrom(d => d.Id))
                .ForMember(x => x.Name, c => c.MapFrom(d => d.Name))
                .ForMember(x => x.Description, c => c.MapFrom(d => d.Description))
                .ForMember(x => x.Duration, c => c.MapFrom(d => d.Duration))
                .ReverseMap();
        }
    }
}
