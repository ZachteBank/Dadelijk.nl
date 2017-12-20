using System;
using System.Data.SqlClient;
using System.Reflection;
using DataAccess.Contexts;
using Models;

namespace DataAccess.Repositories
{
    public abstract class BaseRepository<T> : IRepository where T : IBaseContext
    {
        protected readonly T Context;

        protected BaseRepository(T context)
        {
            Context = context;
        }
    }


}
