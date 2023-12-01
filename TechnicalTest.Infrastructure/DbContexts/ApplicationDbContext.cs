using AspNetCoreHero.Abstractions.Domain;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TechnicalTest.Application.Interfaces.Contexts;
using TechnicalTest.Domain.Entities;

namespace TechnicalTest.Infrastructure.DbContexts
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly ITenantInfo? _tenant;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IWebHostEnvironment env,
            IMultiTenantContextAccessor multiTenantContextAccessor,
            IConfiguration config)
            : base(options)
        {
            _tenant = multiTenantContextAccessor.MultiTenantContext?.TenantInfo;
            _env = env;
            _config = config;
        }

        public DbSet<Product> Product { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString;

            if (_tenant is null && _env.IsDevelopment())
            {
                connectionString = _config.GetConnectionString("Default");
            }
            else
            {
                connectionString = _tenant!.ConnectionString;
            }

            optionsBuilder.UseSqlServer(connectionString);

            base.OnConfiguring(optionsBuilder);
        }
    }
}
