using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Models;

namespace DataAccess.Repositories
{
    public class AccountRespository : BaseRepository<IAccountContext>
    {
        public AccountRespository(IAccountContext context) : base(context)
        {
        }
    }
}
