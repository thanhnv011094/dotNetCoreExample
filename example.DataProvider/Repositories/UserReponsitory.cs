using example.DataProvider.Entities;
using Microsoft.EntityFrameworkCore;

namespace example.DataProvider.Repositories
{
    public class UserReponsitory : Reponsitory<User>, IUserReponsitory
    {
        public UserReponsitory(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
