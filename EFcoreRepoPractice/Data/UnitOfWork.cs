using EFcoreRepoPractice.Infrastructure.repos;
using EFcoreRepoPractice.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFcoreRepoPractice.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IServiceProvider _provider;
        private readonly AjaxClassContext _context;
        private IDbContextTransaction _transaction;

        public UnitOfWork(AjaxClassContext context, IServiceProvider provider = null)
        {
            _provider = provider;
            _context = context;
        }

        public IRepository<T> GetRepository<T>() where T : class => _provider.GetRequiredService<IRepository<T>>();

        //IRepository<T> a = (IRepository<T>)_provider.GetRequiredService(typeof(IRepository<T>)); //也可

        //IRepository<T> b = _provider.GetRequiredService<IRepository<T>>();

        public async Task Save(CancellationToken ct) =>

            await _context.SaveChangesAsync(ct);



        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();

        }

        public async Task CommitTransactionAsync()
        {
            await _transaction.CommitAsync();

        }

        public async Task RollbackTransactionAsync()
        {
            await _transaction.RollbackAsync();

        }


        public async Task ExecuteTransactionAsync(Func<Task> acuAction)
        {

            {
                _transaction = await _context.Database.BeginTransactionAsync();

                try
                {

                    await acuAction();
                    await _transaction.CommitAsync();

                }
                catch
                {

                    await _transaction.RollbackAsync();
                    throw;

                }

            }




}



    }
}
