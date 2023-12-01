using MediatR;
using Microsoft.AspNetCore.Identity;
using TechnicalTest.Application.DTOs;
using TechnicalTest.Application.Enums;
using TechnicalTest.Application.Features.Security.Organizations.Commands.Create;
using TechnicalTest.Application.Interfaces.Contexts;
using TechnicalTest.Application.Interfaces.Repositories;
namespace TechnicalTest.Api.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedAsync(IIdentityService _identityService, ISecurityDbContext securityDbContext, ITenantDbContext tenantDbContext, IMediator _mediator, RoleManager<IdentityRole> roleManager)
        {
            try
            {
                await securityDbContext.MigrateAsync();
                await tenantDbContext.MigrateAsync();

                var organizations = new List<CreateOrganizationCommand>()
                {
                    new CreateOrganizationCommand
                    {
                        Identifier = "Organization1",
                        Name = "Organization 1",
                    },
                    new CreateOrganizationCommand
                    {
                        Identifier = "Organization2",
                        Name = "Organization 2",
                    }
                };

                await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));

                foreach (var org in organizations)
                {
                    var response = await _mediator.Send(org);

                    if (response.Succeeded)
                    {
                        var user = new RegisterRequest
                        {
                            Email = $"user{org.Identifier}@{org.Identifier}.com",
                            Password = "123Pa$$word!",
                            OrganizationId = response.Data
                        };
                        await _identityService.RegisterAsync(user, false);
                    }
                    else
                    {
                        throw new ApiException(response.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.Message);
            }
        }
    }
}
