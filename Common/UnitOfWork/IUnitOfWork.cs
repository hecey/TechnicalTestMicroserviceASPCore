namespace Common.UnitOfWork
{
    public interface IUnitOfWork<T> where T : class
    {
        T Repository { get; }
        Task<int> Complete();
    }
}
