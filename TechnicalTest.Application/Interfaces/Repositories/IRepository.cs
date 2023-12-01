using TechnicalTest.Application.Interfaces.Abstractions;
namespace TechnicalTest.Application.Interfaces.Repositories
{
    public interface IRepository<T> : IRepositoryAsync<T> where T : class
    {
    }
}
