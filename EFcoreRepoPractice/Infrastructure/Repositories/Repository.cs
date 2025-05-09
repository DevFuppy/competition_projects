using EFcoreRepoPractice.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections;

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
        => await _dbSet.FindAsync(new object[] { id }, ct);



        public async Task CreateAsync(T model, CancellationToken ct = default)
        {
            await _dbSet.AddAsync(model);


        }


        public Task UpdateAsync(T model, CancellationToken ct = default)
        {
            _db.Update(model);

            //_dbSet.Entry(model).State = EntityState.Modified; //Same as update

            return Task.CompletedTask;
        }

        public Task UpdateSelectiveAsync(T model, CancellationToken ct = default)
        {

            //var trackedEntry = _db.ChangeTracker.Entries().FirstOrDefault(e => ReferenceEquals(e.Entity, model));

            //_db.Attach(model);
            
            //Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<T>
            var entry = _db.Entry(model);

            _db.Attach(model);

            //取得primarykeys
            var primaryKeys = _db.Model.FindEntityType(typeof(T))?.FindPrimaryKey()?.Properties;
            List<string> keynames = primaryKeys?.Select(x => x.Name).ToList() ?? new List<string>();


            //System.Reflection.PropertyInfo
            foreach (var prop in typeof(T).GetProperties())
            {
                var value = prop.GetValue(model);

                //設定不更新的狀況
                bool isPrimaryKey = keynames.Contains(prop.Name);
                bool isIdLike = prop.Name.EndsWith("id", StringComparison.CurrentCultureIgnoreCase);
                bool isCollection = typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) && prop.PropertyType != typeof(string);

                if (
                    value == null
                    || isIdLike
                    || isPrimaryKey
                    || isCollection
                    )
                {
                    
                    //entry.Property(prop.Name).IsModified = false;
                    continue;
                }

                entry.Property(prop.Name).IsModified = true;
            }

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

