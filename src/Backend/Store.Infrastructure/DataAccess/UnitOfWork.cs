using Store.Domain.Repositories;

namespace Store.Infrastructure.DataAccess
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly StoreDbContext _dbContext;
        public UnitOfWork(StoreDbContext dbContext) => _dbContext = dbContext;

        public async Task Commit() => await _dbContext.SaveChangesAsync();
    }
}
