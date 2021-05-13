using example.DataProvider.Repositories;
using System;

namespace example.DataProvider
{
    public interface IUnitOfWork : IDisposable
    {
        IGroupReponsitory GroupReponsitory { get; }

        void Complete();
    }
}
