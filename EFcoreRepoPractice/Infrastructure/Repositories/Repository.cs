using EFcoreRepoPractice.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections;
using System.Linq.Expressions;

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



        public IQueryable<T> GetAll()
       => _dbSet;



        //public IQueryable<T>? GetById(int id)
        //=>  _dbSet.Where(x=> x.id ==  id );


        public IQueryable<T>? GetSelectively(Expression<Func<T, bool>> columns)
        => _dbSet.Where(columns);



        public void Create(T model)
        {
            _dbSet.Add(model);

        }


        public void Update(T model)
        {
            _db.Update(model);

            //_dbSet.Entry(model).State = EntityState.Modified; //Same as update

 
        }

        public void UpdateSelective(T model)
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

           
        }



        public void Delete(T model)
        {
            _db.Remove(model);

        
        }

        //public async Task Save(CancellationToken ct = default)
        //{
        //    await _db.SaveChangesAsync(ct);
        //}


    }
}

