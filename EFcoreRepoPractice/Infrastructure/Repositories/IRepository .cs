using EFcoreRepoPractice.Models;

namespace EFcoreRepoPractice.Infrastructure.repos
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T?>> GetAllAsync(CancellationToken ct = default);
        Task<T?> GetAsync(int id, CancellationToken ct = default);
        
        Task CreateAsync(T model, CancellationToken ct = default);

        Task UpdateAsync(T model, CancellationToken ct = default);

        Task DeleteAsync(T model, CancellationToken ct = default);
        Task Save(CancellationToken ct = default);
    }
}
