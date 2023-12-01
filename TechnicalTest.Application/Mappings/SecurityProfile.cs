using AutoMapper;
using Finbuckle.MultiTenant;
using TechnicalTest.Application.Features.Security.Organizations.Commands.Create;
using TechnicalTest.Domain.Entities;
namespace TechnicalTest.Application.Mappings
{
    internal class SecurityProfile : Profile
    {
        public SecurityProfile()
        {
            CreateMap<CreateOrganizationCommand, Organization>()
           .ForMember(a => a.ConnectionString, b => b.MapFrom(c => $"Server=localhost;Database={c.Identifier};Trusted_Connection=True;"));
        }
    }
}
