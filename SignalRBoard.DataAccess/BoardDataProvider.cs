using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignalRBoard.DataModel;

namespace SignalRBoard.DataAccess
{
    public class BoardDataProvider : SqlDataProviderBase, IBoardDataProvider
    {
        public Card UpdateCard(CardParameters parameters)
        {
            var card = new Card();

            using (SqlConnection con = CreateConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("[dbo].[spUpdateCard]", con))
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ListId", parameters.ListId);
                    cmd.Parameters.AddWithValue("@Id", parameters.Id);
                    cmd.Parameters.AddWithValue("@Title", parameters.Title);
                    cmd.Parameters.AddWithValue("@Description", parameters.Description);
                    cmd.Parameters.AddWithValue("@Position", parameters.Position);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        var id = reader.GetOrdinal("Id");
                        var desc = reader.GetOrdinal("Description");
                        var title = reader.GetOrdinal("Title");
                        var listId = reader.GetOrdinal("ListId");
                        var position = reader.GetOrdinal("Position");

                        while (reader.Read())
                        {
                            card = new Card
                            {
                                Id = reader.GetGuid(id),
                                Title = reader.GetString(title),
                                Description = reader.GetString(desc),
                                Position = reader.GetInt32(position),
                                ListId = reader.GetGuid(listId)
                            };

                        }
                        return card;
                    }
                }
            }
        }

        public void DeleteCard(CardParameters parameters)
        {
            using (var con = CreateConnection())
            {
                con.Open();
                using (var cmd = new SqlCommand("[dbo].[spDeleteCard]", con))
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", parameters.Id);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        throw new Exception(string.Format("Database error executing[spDeleteCard](code { 0 }): { 1}", e.State, e.Message));
                    }
                }
            }
        }

        public ICollection<Card> GetCards(CardParameters parameters)
        {
            var cards = new List<Card>();

            using (SqlConnection con = CreateConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("[dbo].[spGetCards]", con))
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ListId", parameters.ListId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        var id = reader.GetOrdinal("Id");
                        var desc = reader.GetOrdinal("Description");
                        var title = reader.GetOrdinal("Title");
                        var listId = reader.GetOrdinal("ListId");
                        var position = reader.GetOrdinal("Position");

                        while (reader.Read())
                        {
                            var card = new Card
                            {
                                Id = reader.GetGuid(id),
                                Title = reader.GetString(title),
                                Description = reader.GetString(desc),
                                Position = reader.GetInt32(position),
                                ListId = reader.GetGuid(listId)
                            };
                            cards.Add(card);
                        }
                        return cards;
                    }
                }
            }
        }

        public ICollection<List> GetLists(ListParameters parameters)
        {
            var lists = new List<List>();
            using (SqlConnection con = CreateConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("[dbo].[spGetLists]", con))
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BoardId", parameters.BoardId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        var id = reader.GetOrdinal("Id");
                        var name = reader.GetOrdinal("Name");
                        var max = reader.GetOrdinal("MaxItems");

                        while (reader.Read())
                        {
                            var list = new List
                            {
                                Id = reader.GetGuid(id),
                                Name = reader.GetString(name),
                                MaxItems = reader.GetInt32(max)
                            };
                            lists.Add(list);
                        }
                        return lists;
                    }
                }
            }
        }
    }

    internal interface IBoardDataProvider
    {
        ICollection<Card> GetCards(CardParameters parameters);

        ICollection<List> GetLists(ListParameters parameters);
    }
}
