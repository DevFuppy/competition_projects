using EFcoreRepoPractice.Models;
using Microsoft.EntityFrameworkCore;

namespace EFcoreRepoPractice.Infrastructure.repos
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AjaxClassContext _db;
        internal DbSet<T> _dbSet;


        public Repository(AjaxClassContext db)
        {

            _db = db;
            _dbSet = _db.Set<T>();

        }



        public async Task<IEnumerable<T?>> GetAllAsync(CancellationToken ct = default)
       => await _dbSet.AsNoTracking().ToListAsync();



        public async Task<T?> GetAsync(int id, CancellationToken ct = default)
        => await _dbSet .FindAsync(new object[] { id }, ct);




        public async Task CreateAsync(T model, CancellationToken ct = default)
        {
            await _dbSet.AddAsync(model);

            
        }


        public Task UpdateAsync(T model, CancellationToken ct = default)
        {
            //_db.Update(model);

            _dbSet.Entry(model).State = EntityState.Modified; //Same as update

            return Task.CompletedTask;
        }



        public Task DeleteAsync(T model, CancellationToken ct = default)
        {
            _db.Remove(model);

            return Task.CompletedTask;
        }

        public async Task Save(CancellationToken ct = default)
        {
            await _db.SaveChangesAsync(ct);
        }


    }
}

