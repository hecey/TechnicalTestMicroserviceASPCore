using TTM.ClientService.Data;
using TTM.Common.UnitOfWork;

namespace TTM.ClientService.UnitOfWork
{
    public class UnitOfWork<T> : UOW<T>, IDisposable where T : class
    {
        public UnitOfWork(DataContext context, T repository) : base(context, repository)
        {
        }

    }
}