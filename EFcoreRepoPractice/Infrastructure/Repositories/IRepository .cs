using EFcoreRepoPractice.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace EFcoreRepoPractice.Infrastructure.repos
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T?>> GetAllAsync(CancellationToken ct = default);
        Task<T?> GetByIdAsync(int id, CancellationToken ct = default);

        Task<List<T>?> GetSelectivelyAsync(Expression<Func<T, bool>> columns, CancellationToken ct = default);
        Task CreateAsync(T model, CancellationToken ct = default);

        Task UpdateAsync(T model, CancellationToken ct = default);
        Task UpdateSelectiveAsync(T model, CancellationToken ct = default);

        Task DeleteAsync(T model, CancellationToken ct = default);
        Task Save(CancellationToken ct = default);
    }
}
