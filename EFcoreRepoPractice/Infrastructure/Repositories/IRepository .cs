using EFcoreRepoPractice.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace EFcoreRepoPractice.Infrastructure.repos
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();         

        IQueryable<T>? GetSelectively(Expression<Func<T, bool>> columns);

        void Create(T model);

        void Update(T model);
        void UpdateSelective(T model);

        void Delete(T model);
        //Task Save(CancellationToken ct = default);
    }
}
