using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.Infrastructure.Repositories;
using TechnicalTest.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using TechnicalTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TechnicalTest.Infrastructure.DbContexts;
using TechnicalTest.Application.Interfaces.Abstractions;
using TechnicalTest.Application.Interfaces.Contexts;
using TechnicalTest.Application.Abstractions;
using Finbuckle.MultiTenant;

namespace TechnicalTest.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<TenantDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("TenantConnection")),
                ServiceLifetime.Transient);

            services
            .AddSqlServer<SecurityDbContext>(configuration.GetConnectionString("SecurityConnection"))
            .AddIdentityCore<User>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<SecurityDbContext>();

            services.AddDbContext<ApplicationDbContext>();

            services
            .AddMultiTenant<Organization>()
            .WithHeaderStrategy("X-Tenant")
            .WithEFCoreStore<TenantDbContext, Organization>();

            services.AddScoped<ISecurityDbContext, SecurityDbContext>();
            services.AddScoped<ITenantDbContext, TenantDbContext>();
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<Application.Interfaces.Repositories.IUnitOfWork, UnitOfWork>();
            services.AddTransient<IIdentityService, IdentityService>();
        }
    }
}
