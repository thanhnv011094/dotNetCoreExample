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
            RoleReponsitory = new RoleReponsitory(_dbContext);
            UserReponsitory = new UserReponsitory(_dbContext);
        }
        public IRoleReponsitory RoleReponsitory { get; }
        public IUserReponsitory UserReponsitory { get; }

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
