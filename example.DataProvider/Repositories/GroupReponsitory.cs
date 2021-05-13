using example.DataProvider.Models;
using Microsoft.EntityFrameworkCore;

namespace example.DataProvider.Repositories
{
    public class GroupReponsitory : Reponsitory<Group>, IGroupReponsitory
    {
        public GroupReponsitory(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
