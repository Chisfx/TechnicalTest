using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TechnicalTest.Domain.Entities;
namespace TechnicalTest.Application.Interfaces.Contexts
{
    public interface IApplicationDbContext
    {
        //IDbConnection Connection { get; }
        //bool HasChanges { get; }
        //DatabaseFacade DataBase { get; }
        DbSet<Product> Product { get; set; }
    }
}
