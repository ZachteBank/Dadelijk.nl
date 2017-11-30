using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Models;

namespace DataAccess.Repositories
{
    public class NewsItemRepository : BaseRepository
    {
        public NewsItemRepository(DatabaseSettings settings) : base(settings)
        {
        }

        private NewsItem CreateNewsItemByReader(SqlDataReader reader)
        {
            if (!reader.Read())
            {
                return null;
            }
            return new NewsItem((int)reader["id"])
            {
                Subject = reader["subject"].ToString(),
                Text = reader["text"].ToString()
            };
        }

        public NewsItem GetNewsItemById(int id)
        {
            using (var conn = GetConnection())
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                        @"SELECT * 
                    FROM dbo.NewsItem
                    WHERE NewsItem.id = @id";
                    cmd.Parameters.Add(new SqlParameter("id", id));

                    var reader = cmd.ExecuteReader();
                    return CreateNewsItemByReader(reader);
                }

            }
        }
    }
}
