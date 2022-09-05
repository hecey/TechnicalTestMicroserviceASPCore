using Microsoft.EntityFrameworkCore;

namespace TTM.Common.UnitOfWork
{
    public class UOW<T> : IUnitOfWork<T>, IDisposable where T : class
    {

        protected readonly DbContext _context;
        public T Repository { get; }

        public UOW(DbContext context, T repository)
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
