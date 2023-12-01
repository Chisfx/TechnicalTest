using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using TechnicalTest.Domain.Entities;
namespace TechnicalTest.Application.Interfaces.Contexts
{
    public interface ITenantDbContext
    {
        Task MigrateAsync(CancellationToken cancellationToken = new CancellationToken());
        DbSet<Organization> Organization { get; set; }
    }
}
