using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories
{
    class NewsItemRepository : BaseRepository
    {
        public NewsItemRepository(DatabaseSettings settings) : base(settings)
        {
        }
    }
}
