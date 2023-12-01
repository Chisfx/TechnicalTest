using Finbuckle.MultiTenant.Stores;
using Finbuckle.MultiTenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TechnicalTest.Domain.Entities;
using System.Threading;
using Microsoft.AspNetCore.Identity;
using AspNetCoreHero.Abstractions.Domain;

namespace TechnicalTest.Infrastructure.DbContexts
{
    public class TenantDbContext : EFCoreStoreDbContext<Organization>, ITenantDbContext
    {
        public TenantDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Organization> Organization { get; set; }

        public async Task MigrateAsync(CancellationToken cancellationToken = default)
        {
            await base.Database.MigrateAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("Tenat");

            builder.Entity<Organization>(entity =>
            {
                entity.ToTable(name: "Organizations");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });           
        }

    }
}
