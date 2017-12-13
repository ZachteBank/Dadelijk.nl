using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Models;

namespace DataAccess.Repositories
{
    public class ReactionRepository : BaseRepository
    {
        public ReactionRepository(DatabaseSettings settings) : base(settings)
        {
        }

        private Reaction CreateReactionByReader(SqlDataReader reader)
        {
            if (!reader.Read())
            {
                return null;
            }
            var accountRepository = new AccountRespository(Settings);
            var newsItemRepository = new NewsItemRepository(Settings);

            var reaction =  new Reaction((int)reader["id"])
            {
                NewsItem = newsItemRepository.GetNewsItemById((int)reader["newsItemId"]),
                Account = accountRepository.GetAccountById((int)reader["accountId"]),
                ParentReaction = GetReactionById(reader["reactionId"] as int? ?? 0),
                Text = reader["text"].ToString(),
                Active = (bool)reader["active"]
            };

            AddDateCreatedAndDateUpdated(reaction, reader);

            return reaction;
        }

        public Reaction GetReactionById(int id)
        {
            if (id == 0)
            {
                return null;
            }
            using (var conn = GetConnection())
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                        @"SELECT * 
                    FROM dbo.Reaction
                    WHERE Reaction.id = @id";
                    cmd.Parameters.Add(new SqlParameter("id", id));

                    var reader = cmd.ExecuteReader();
                    return CreateReactionByReader(reader);
                }

            }
        }

        public IEnumerable<Reaction> GetAllReactionsByNewsItemId(int id)
        {
            using (var conn = GetConnection())
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                        @"SELECT * 
                        FROM dbo.Reaction
                        WHERE Reaction.newsItemId=@id";
                    cmd.Parameters.Add(new SqlParameter("id", id));

                    var reader = cmd.ExecuteReader();
                    var items = new List<Reaction>();
                    Reaction newsItem;

                    while ((newsItem = CreateReactionByReader(reader)) != null)
                    {
                        items.Add(newsItem);
                    }
                    return items;
                }
            }
        }

        public void UpdateReaction(Reaction reaction)
        {
            if (reaction?.Text == null)
            {
                throw new ArgumentNullException();
            }

            if (reaction.Id == 0)
            {
                throw new ArgumentException("Id cant be 0");
            }

            using (var connection = GetConnection())
            {
                var command = new SqlCommand(null, connection)
                {
                    CommandText = @"UPDATE [dbo].Reaction SET text = @text, active=@active WHERE id = @id SELECT GETDATE()"
                };

                command.Parameters.Add(new SqlParameter("id", reaction.Id));
                command.Parameters.Add(new SqlParameter("text", reaction.Text));
                command.Parameters.Add(new SqlParameter("active", reaction.Active));
                //command.ExecuteNonQuery();
                var reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    return;
                }
                SetDateUpdatedOfModel(reaction, (DateTime)reader.GetDateTime(0));
            }

        }

        public bool CreateReaction(Reaction reaction)
        {
            if (reaction.Text == null)
            {
                throw new ArgumentNullException();
            }

            if (reaction.Id != 0)
            {
                throw new ArgumentException("Reaction id should be 0");
            }

            using (var connection = GetConnection())
            {
                var command = new SqlCommand(null, connection);
                command.CommandText = @"INSERT INTO Reaction(newsItemId, accountId, reactionId, text, active) 
                                        VALUES (@newsItemId, @accountId, @reactionId, @text, @active);
                                        SELECT SCOPE_IDENTITY() AS Id";

                command.Parameters.Add(new SqlParameter("newsItemId", reaction.NewsItem.Id));
                command.Parameters.Add(new SqlParameter("accountId", reaction.Account.Id));
                command.Parameters.Add(new SqlParameter("reactionId", (object)reaction.ParentReaction?.Id ?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("text", reaction.Text));
                command.Parameters.Add(new SqlParameter("active", reaction.Active));

                var reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    return false;
                }
                SetIdOfModel(reaction, (int)reader.GetDecimal(0));
                return true;
            }
        }
    }
}
