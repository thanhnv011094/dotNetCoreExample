using example.DataProvider.Repositories;
using Microsoft.EntityFrameworkCore;

namespace example.DataProvider
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;
        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
            GroupReponsitory = new GroupReponsitory(_dbContext);
        }
        public IGroupReponsitory GroupReponsitory { get; }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public void Complete()
        {
            _dbContext.SaveChanges();
        }
    }
}
