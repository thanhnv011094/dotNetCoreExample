using example.DataProvider.Entities;
using Microsoft.EntityFrameworkCore;

namespace example.DataProvider.Repositories
{
    public class RoleReponsitory : Reponsitory<Role>, IRoleReponsitory
    {
        public RoleReponsitory(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
