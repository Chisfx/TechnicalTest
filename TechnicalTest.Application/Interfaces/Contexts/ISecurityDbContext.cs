using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using TechnicalTest.Domain.Entities;

namespace TechnicalTest.Application.Interfaces.Contexts
{
    public interface ISecurityDbContext
    {
        IDbConnection Connection { get; }
        bool HasChanges { get; }
        DatabaseFacade DataBase { get; }
        Task MigrateAsync(CancellationToken cancellationToken = new CancellationToken());
        DbSet<User> User { get; set; }
    }
}
