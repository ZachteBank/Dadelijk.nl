using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Models;

namespace DataAccess.Repositories
{
    public class TagRepository : BaseRepository
    {
        public TagRepository(DatabaseSettings settings) : base(settings)
        {
        }

        private Tag CreateTagByReader(SqlDataReader reader)
        {
            if (!reader.Read())
            {
                return null;
            }

            var tag =  new Tag((int)reader["id"])
            {
                Name = reader["name"].ToString(),
            };

            AddDateCreatedAndDateUpdated(tag, reader);

            return tag;
        }

        public Tag GetTagById(int id)
        {
            using (var conn = GetConnection())
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                        @"SELECT * 
                    FROM dbo.Tag
                    WHERE Tag.id = @id";
                    cmd.Parameters.Add(new SqlParameter("id", id));

                    var reader = cmd.ExecuteReader();
                    return CreateTagByReader(reader);
                }

            }
        }
        public Tag GetTagByName(string name)
        {
            using (var conn = GetConnection())
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                        @"SELECT * 
                    FROM dbo.Tag
                    WHERE Tag.name = @name";
                    cmd.Parameters.Add(new SqlParameter("name", name));

                    var reader = cmd.ExecuteReader();
                    return CreateTagByReader(reader);
                }

            }
        }

        public IEnumerable<Tag> GetAllTags(int newsItemId = 0)
        {
            using (var conn = GetConnection())
            {
                using (var cmd = conn.CreateCommand())
                {
                    if (newsItemId == 0)
                    {
                        cmd.CommandText =
                            @"SELECT * 
                            FROM dbo.Tag";
                    }
                    else
                    {
                        cmd.CommandText =
                            @"SELECT a.id, a.name FROM [bram].[dbo].[Tag] as a LEFT JOIN [bram].[dbo].[TagNewsItem] as b ON b.tagId=a.id WHERE b.newsItemId=@id";

                        cmd.Parameters.Add(new SqlParameter("id", newsItemId));
                    }

                    var reader = cmd.ExecuteReader();
                    var items = new List<Tag>();
                    Tag Tag = null;

                    while ((Tag = CreateTagByReader(reader)) != null)
                    {
                        items.Add(Tag);
                    }
                    return items;
                }
            }
        }



        public void UpdateTag(Tag tag)
        {
            if (tag?.Name == null)
            {
                throw new ArgumentNullException();
            }

            if (tag.Id == 0)
            {
                throw new ArgumentException("Id cant be 0");
            }

            using (var connection = GetConnection())
            {
                var command = new SqlCommand(null, connection)
                {
                    CommandText = @"UPDATE [dbo].Tag SET name=@name WHERE id = @id SELECT GETDATE()"
                };

                command.Parameters.Add(new SqlParameter("id", tag.Id));
                command.Parameters.Add(new SqlParameter("name", tag.Name));
                //command.ExecuteNonQuery();
                var reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    return;
                }
                SetDateUpdatedOfModel(tag, (DateTime)reader.GetDateTime(0));
            }

        }

        public bool CreateTag(Tag tag)
        {
            if (tag?.Name == null)
            {
                throw new ArgumentNullException();
            }

            if (tag.Id != 0)
            {
                throw new ArgumentException("Tag id should be 0");
            }

            using (var connection = GetConnection())
            {
                var command = new SqlCommand(null, connection);
                command.CommandText = @"INSERT INTO Tag(name) 
                                        VALUES (@name);
                                        SELECT SCOPE_IDENTITY() AS Id";

                command.Parameters.Add(new SqlParameter("name", tag.Name));

                var reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    return false;
                }
                SetIdOfModel(tag, (int)reader.GetDecimal(0));
                return true;
            }
        }

        public void CreateOrAddTag(Tag tag)
        {
            if (GetTagByName(tag.Name) == null)
            {
                CreateTag(tag);
            }
        }

        public bool BindTagToNewsItem(Tag tag, int newsItemId)
        {
            using (var connection = GetConnection())
            {
                var command = new SqlCommand(null, connection);
                command.CommandText = @"INSERT INTO TagNewsItem(newsItemId, tagId) 
                                        VALUES (@newsItemId, @tagId);
                                        SELECT SCOPE_IDENTITY() AS Id";

                command.Parameters.Add(new SqlParameter("newsItemID", newsItemId));
                command.Parameters.Add(new SqlParameter("name", tag.Id));

                var reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    return false;
                }
                SetIdOfModel(tag, (int)reader.GetDecimal(0));
                return true;
            }
        }
    }
}
