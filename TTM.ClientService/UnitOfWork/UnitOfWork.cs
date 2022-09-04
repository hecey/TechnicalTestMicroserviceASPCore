using TTM.ClientService.Data;
using TTM.Common.UnitOfWork;

namespace TTM.ClientService.UnitOfWork
{
    public class UnitOfWork<T> : IUnitOfWork<T>, IDisposable where T : class
    {

        protected readonly DataContext _context;
        public T Repository { get; }

        public UnitOfWork(DataContext context, T repository)
        {
            _context = context;
            Repository = repository;
        }


        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }


}
