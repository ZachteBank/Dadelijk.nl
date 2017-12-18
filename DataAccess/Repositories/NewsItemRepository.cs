using System;
using System.Collections.Generic;
using System.Data;
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
            var reactionRepository = new ReactionRepository(Settings);
            var tagRepository = new TagRepository(Settings);

            var newsItem =  new NewsItem((int)reader["id"])
            {
                Subject = reader["subject"].ToString(),
                Text = reader["text"].ToString(),
                Active = (bool)reader["active"],
                Reactions = reactionRepository.GetAllReactionsByNewsItemId((int)reader["id"]),
                Tags = tagRepository.GetAllTags((int)reader["id"])
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

        public IEnumerable<NewsItem> GetAllNewsItems(DateTime? date = null)
        {
            using (var conn = GetConnection())
            {
                using (var cmd = conn.CreateCommand())
                {
                    if (date == null)
                    {
                        cmd.CommandText =
                            @"SELECT * 
                            FROM dbo.NewsItem";
                    }
                    else
                    {
                        //20081220 00:00:00.000
                        var dateFormatBegin = date.Value.ToString("yyyyMMdd 00:00:00.000"); 
                        var dateFormatEnd = date.Value.AddDays(1).ToString("yyyyMMdd 00:00:00.000");
                        cmd.CommandText =
                            @"SELECT *
                            FROM dbo.NewsItem
                            WHERE (dateCreated >= '" + dateFormatBegin + @"'
                            AND dateCreated < '" + dateFormatEnd + @"')
                            OR (dateUpdated >= '" + dateFormatBegin + @"'
                            AND dateUpdated < '" + dateFormatEnd + @"')";
                    }

                    var reader = cmd.ExecuteReader();
                    var items = new List<NewsItem>();
                    NewsItem newsItem = null;

                    while ((newsItem = CreateNewsItemByReader(reader)) != null)
                    {
                        items.Add(newsItem);
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
                    CommandText = @"UPDATE [dbo].NewsItem SET subject = @subject, text = @text, active=@active WHERE id = @id SELECT GETDATE()"
                };

                command.Parameters.Add(new SqlParameter("id", newsItem.Id));
                command.Parameters.Add(new SqlParameter("subject", newsItem.Subject));
                command.Parameters.Add(new SqlParameter("text", newsItem.Text));
                command.Parameters.Add(new SqlParameter("active", newsItem.Active));
                //command.ExecuteNonQuery();
                var reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    return;
                }
                SetDateUpdatedOfModel(newsItem, (DateTime)reader.GetDateTime(0));
            }

        }

        public int GetAllNewsItemsAndTags(int newsItemId)
        {
            using (var connection = GetConnection())
            {
                using (var command = new SqlCommand("CountTagsByNewsItem", connection)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.Add(new SqlParameter("newsItemId", newsItemId));

                    var reader = command.ExecuteReader();
                    if (!reader.Read())
                    {
                        return (int) reader["count"];
                    }
                }
            }
            throw new Exception("Something went wrong with the database");

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
