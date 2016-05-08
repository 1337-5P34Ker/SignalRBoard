using SignalRBoard.DataAccess;
using SignalRBoard.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRBoard.Business
{
    public static class BoardHelper
    {
        public static Board InitializeBoard()
        {
            Board board = new Board();
            using (BoardDataProvider provider = new BoardDataProvider())
            {
                board.Lists = provider.GetLists(new ListParameters());
                // Call the broadcastMessage method to update clients.

                foreach (var list in board.Lists)
                {
                    list.Cards = provider.GetCards(new CardParameters { ListId = list.Id });
                }
            }
            return board;
        }

        public static Card AddCard(string title, string description, string listId)
        {
            Card card;
            using (BoardDataProvider provider = new BoardDataProvider())
            {
                card = provider.UpdateCard(new CardParameters
                {
                    Title = title,
                    Description = description,
                    ListId = new Guid(listId)
                });
            }
            return card;
        }

        public static Card UpdateCard(string title, string description, string cardId)
        {
            Card card;
            using (BoardDataProvider provider = new BoardDataProvider())
            {
                card = provider.UpdateCard(new CardParameters
                {
                    Title = title,
                    Description = description,
                    Id = new Guid(cardId)
                });
            }
            return card;
        }

        public static void DeleteCard(string cardId)
        {
            using (BoardDataProvider provider = new BoardDataProvider())
            {
                provider.DeleteCard(new CardParameters
                {
                    Id = new Guid(cardId)
                });
            }
        }

        public static Board MoveCard(Guid cardId, Direction direction)
        {
            Board board = new Board();
            using (BoardDataProvider provider = new BoardDataProvider())
            {
                board.Lists = provider.GetLists(new ListParameters()).OrderBy(l => l.Position).ToList();
                if (board.Lists.Any())
                {
                    foreach (var list in board.Lists)
                    {
                        list.Cards = provider.GetCards(new CardParameters
                        {
                            ListId = list.Id
                        }).OrderBy(c => c.Position).ToList();
                        foreach (var card in list.Cards)
                        {
                            card.Position = list.Cards.IndexOf(card); // reset positions
                        }
                    }
                }
            }
            switch (direction)
            {
                case Direction.Up:

                    var affectedRow = board.Lists.FirstOrDefault(l => l.Cards.Any(c => c.Id == cardId));
                    if (affectedRow != null)
                    {
                        var card1 = affectedRow.Cards.First(c => c.Id == cardId);

                        if (card1.Position > 0)
                        {
                            var card2 = affectedRow.Cards.FirstOrDefault(c => c.Position == card1.Position - 1);
                            if (card2 != null)
                            {
                                card2.Position = card1.Position;
                                card1.Position = card1.Position - 1;

                                using (BoardDataProvider provider = new BoardDataProvider())
                                {
                                    provider.UpdateCard(new CardParameters
                                    {                                        
                                        Id = card1.Id,
                                        Position = card1.Position,
                                        Description = card1.Description,
                                        Title = card1.Title,
                                        ListId = card1.ListId
                                                                                
                                    });

                                    provider.UpdateCard(new CardParameters
                                    {
                                        Id = card2.Id,
                                        Position = card2.Position,
                                        Description = card2.Description,
                                        Title = card2.Title,
                                        ListId = card2.ListId
                                    });
                                }

                            }

                        }
                    }
                        break;

                case Direction.Down:

                    break;
            }

            foreach (var list in board.Lists)
            {
                list.Cards = list.Cards.OrderBy(c => c.Position).ToList();                
            }

            return board;
        }
    }
}
