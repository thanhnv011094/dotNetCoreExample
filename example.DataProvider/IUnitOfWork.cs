using example.DataProvider.Repositories;
using System;

namespace example.DataProvider
{
    public interface IUnitOfWork : IDisposable
    {
        IRoleReponsitory RoleReponsitory { get; }
        IUserReponsitory UserReponsitory { get; }

        void Complete();
    }
}
