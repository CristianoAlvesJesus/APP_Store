namespace Store.Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}