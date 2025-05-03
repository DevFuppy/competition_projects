using EFcoreRepoPractice.Infrastructure.repos;
using Microsoft.EntityFrameworkCore;

namespace EFcoreRepoPractice.Data
{
    public interface IUnitOfWork
    {

        IRepository<T> GetRepository<T>() where T : class ;
     

        Task Save(CancellationToken ct) ;

        Task ExecuteTransactionAsync(Func<Task> acuAction);



    }
}
