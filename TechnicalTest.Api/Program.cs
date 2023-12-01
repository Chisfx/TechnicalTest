using MediatR;
using Microsoft.AspNetCore.Identity;
using TechnicalTest.Api.Extensions;
using TechnicalTest.Api.Seeds;
using TechnicalTest.Application.Interfaces.Contexts;
using TechnicalTest.Application.Interfaces.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiPresentations(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger("app");
    try
    {
        var identityServic = services.GetRequiredService<IIdentityService>();
        var mediator = services.GetRequiredService<IMediator>();
        var securityDbContext = services.GetRequiredService<ISecurityDbContext>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var tenantDbContext = services.GetRequiredService<ITenantDbContext>();

        await DefaultUsers.SeedAsync(identityServic, securityDbContext, tenantDbContext, mediator, roleManager);

        logger.LogInformation("Finished Seeding Default Data");
        logger.LogInformation("Application Starting");
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "An error occurred seeding the DB");
    }
}

app.Run();
