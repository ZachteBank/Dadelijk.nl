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
            var newsItem =  new NewsItem((int)reader["id"])
            {
                Subject = reader["subject"].ToString(),
                Text = reader["text"].ToString(),
                Active = (bool)reader["active"],
            };

            AddDateCreatedAndDateUpdated(newsItem, reader);

            return newsItem;
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

        public IEnumerable<NewsItem> GetAllNewsItems()
        {
            using (var conn = GetConnection())
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                        @"SELECT * 
                    FROM dbo.NewsItem";

                    var reader = cmd.ExecuteReader();
                    var items = new List<NewsItem>();
                    
                    /*
                    while (reader.Read())
                    {
                        var item = new NewsItem((int)reader["id"])
                        {
                            Subject = reader["subject"].ToString(),
                            Text = reader["text"].ToString(),
                            Active = (bool) reader["active"]
                        };
                        items.Add(item);
                    }
                    */
                    for (int i = 0; i < reader.Depth; i++)
                    {
                        items.Add(CreateNewsItemByReader(reader));
                    }
                    return items;
                }

            }
        }

        public void UpdateNewsItem(NewsItem newsItem)
        {
            if (newsItem?.Text == null || newsItem.Subject == null)
            {
                throw new ArgumentNullException();
            }

            if (newsItem.Id == 0)
            {
                throw new ArgumentException("Id cant be 0");
            }

            using (var connection = GetConnection())
            {
                var command = new SqlCommand(null, connection)
                {
                    CommandText = @"UPDATE [dbo].NewsItem SET subject = @subject, text = @text, active=@active WHERE id = @id"
                };

                command.Parameters.Add(new SqlParameter("id", newsItem.Id));
                command.Parameters.Add(new SqlParameter("subject", newsItem.Subject));
                command.Parameters.Add(new SqlParameter("text", newsItem.Text));
                command.Parameters.Add(new SqlParameter("active", newsItem.Active));
                command.ExecuteNonQuery();
            }

        }

        public bool CreateNewsItem(NewsItem newsItem)
        {
            if (newsItem?.Subject == null || newsItem.Text == null)
            {
                throw new ArgumentNullException();
            }

            if (newsItem.Id != 0)
            {
                throw new ArgumentException("Newsitem id should be 0");
            }

            using (var connection = GetConnection())
            {
                var command = new SqlCommand(null, connection);
                command.CommandText = @"INSERT INTO NewsItem(subject, text, active) 
                                        VALUES (@subject, @text, @active);
                                        SELECT SCOPE_IDENTITY() AS Id";

                command.Parameters.Add(new SqlParameter("subject", newsItem.Subject));
                command.Parameters.Add(new SqlParameter("text", newsItem.Text));
                command.Parameters.Add(new SqlParameter("active", newsItem.Active));

                var reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    return false;
                }
                SetIdOfModel(newsItem, (int)reader.GetDecimal(0));
                return true;
            }
        }
    }
}
